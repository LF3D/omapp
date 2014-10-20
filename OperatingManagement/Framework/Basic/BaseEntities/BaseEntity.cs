using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;

namespace OperatingManagement.Framework.Basic
{
    /// <summary>
    /// Provides the base class of entities.
    /// </summary>
    /// <typeparam name="K">Key type.</typeparam>
    /// <typeparam name="T">Entity type.</typeparam>
    public abstract class BaseEntity<K, T> : IDataErrorInfo, IDisposable
        where T : BaseEntity<K, T>, new()
    {
        #region -Properties-
        /// <summary>
        /// Unique Identification
        /// </summary>
        public K Id { get; set; }
        /// <summary>
        /// Created time.
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// Updated time.
        /// </summary>
        public DateTime UpdatedTime { get; set; }
        #endregion

        #region -Validations-
        private HybridDictionary _validRules = new HybridDictionary();
        /// <summary>
        /// Add valid rules in the dictionary for server validation.
        /// </summary>
        /// <param name="propertyName">Name of the entity property</param>
        /// <param name="errMsg">Error message</param>
        /// <param name="validFailed">Whether is failed to validate</param>
        protected virtual void AddValidRules(string propertyName, string errMsg, bool validFailed)
        {
            if (_validRules.Contains(propertyName))
                _validRules.Remove(propertyName);
            if (validFailed)
                _validRules.Add(propertyName, errMsg);
        }
        /// <summary>
        /// You'ld better override this method and use 'AddValidRules' to design the rules.
        /// </summary>
        protected abstract void ValidationRules();
        /// <summary>
        /// Gets whether the object is valid.
        /// </summary>
        public bool IsValid
        {
            get {
                ValidationRules();
                return _validRules.Count == 0;
            }
        }
        /// <summary>
        /// Gets all the validation messages.
        /// </summary>
        public virtual string ValidationMessage
        {
            get
            {
                if (!IsValid)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var s in _validRules.Values)
                    {
                        sb.AppendLine(s.ToString());
                    }
                    return sb.ToString();
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// Gets the first validation message.
        /// </summary>
        public virtual string FirstValidationMessage
        {
            get
            {
                if (!IsValid)
                {
                    foreach (var s in _validRules.Values)
                    {
                        return s.ToString();
                    }
                }
                return string.Empty;
            
            }
        }
        #endregion

        #region -Equality overrides-

        /// <summary>
        /// Gets the ID only.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// Comapares this object with another in a simple way(type first, ID second.)
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() == this.GetType())
            {
                return obj.GetHashCode() == this.GetHashCode();
            }

            return false;
        }

        /// <summary>
        /// Override the operator '==' in order to check the two entities are same, 
        /// [it's in a fast way to compare the objects by ID.]
        /// </summary>
        /// <param name="first">First object</param>
        /// <param name="second">Second object</param>
        public static bool operator ==(BaseEntity<K, T> first, BaseEntity<K, T> second)
        {
            if (Object.ReferenceEquals(first, second)) return true;

            if ((object)first == null || (object)second == null) return false;

            return first.GetHashCode() == second.GetHashCode();
        }

        /// <summary>
        /// Override the operator '!=' in order to check the two entities are different, 
        /// [it's in a fast way to compare the objects by ID.]
        /// </summary>
        /// <param name="first">First object</param>
        /// <param name="second">Second object</param>
        public static bool operator !=(BaseEntity<K, T> first, BaseEntity<K, T> second)
        {
            return !(first == second);
        }

        #endregion

        #region -IDataErrorInfo Members-
        /// <summary>
        /// 
        /// </summary>
        public string Error
        {
            get { return ValidationMessage; }
        }
        /// <summary>
        /// Gets the validation message by name.
        /// </summary>
        /// <param name="columnName">Name of validation rules</param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get {
                if (_validRules.Contains(columnName))
                    return _validRules[columnName].ToString();
                else
                    return string.Empty;
            }
        }

        #endregion

        #region -IDisposable Members

        public void Dispose()
        {
            _validRules.Clear();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace OperatingManagement.Framework.Helper
{
    /// <summary>
    /// Provides a class to easy copy, type confirm etc.
    /// </summary>
    public class TypeHelper
    {
        #region -NameValueCollection Clone-
        /// <summary>
        /// Collection clone
        /// </summary>
        /// <typeparam name="T">Type of the collection</typeparam>
        /// <param name="source">Source collection</param>
        /// <param name="destination">Destine collection</param>
        public static void CopyCollection<T>(ICollection<T> source, ICollection<T> destination)
        {
            destination.Clear();
            foreach (T local in source)
            {
                destination.Add(local);
            }
        }
        /// <summary>
        /// Dictionary clone
        /// </summary>
        /// <typeparam name="K">Type of the dictionary key</typeparam>
        /// <typeparam name="V">Type of the dictionary value</typeparam>
        /// <param name="source">Source dictionary</param>
        /// <param name="destination">Destine dictionary</param>
        public static void CopyCollection<K, V>(IDictionary<K, V> source, IDictionary<K, V> destination)
        {
            destination.Clear();
            foreach (K local in source.Keys)
            {
                destination.Add(local, source[local]);
            }
        }
        /// <summary>
        /// NameValueCollection clone
        /// </summary>
        /// <param name="source">Source NameValueCollection</param>
        /// <param name="destination">Destine NameValueCollection</param>
        public static void CopyCollection(NameValueCollection source, NameValueCollection destination)
        {
            destination.Clear();
            foreach (string str in source.Keys)
            {
                destination.Add(str, source[str]);
            }
        }
        #endregion

        #region -Join Together-
        /// <summary>
        /// Join the array together with separator.
        /// </summary>
        /// <typeparam name="T">int or string</typeparam>
        /// <param name="separator">the separator</param>
        /// <param name="array">array list</param>
        /// <returns></returns>
        public static string JoinIntArray<T>(string separator, T[] array)
        {
            if ((array == null) || (array.Length <= 0))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for (var i = 0; i < array.Length; i++)
            {
                builder.Append(array[i]);
                if (i != array.Length - 1)
                {
                    builder.Append(separator);
                }
            }
            return builder.ToString();
        }
        #endregion
    }
}

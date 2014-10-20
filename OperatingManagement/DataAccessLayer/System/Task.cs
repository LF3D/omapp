using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using Oracle.DataAccess.Client;
using System.Data;

namespace OperatingManagement.DataAccessLayer.System
{
    /// <summary>
    /// Represents the Action object from database.
    /// </summary>
    [Serializable]
    public class Task : BaseEntity<double, Task>
    {
        /// <summary>
        /// Create a new instance of <see cref="Task"/> class.
        /// </summary>
        public Task(){
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;
        /// <summary>
        /// Gets/Sets the Task name.
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// Gets/Sets the description/note.
        /// </summary>
        public string TaskNote { get; set; }
        #endregion

        #region -Private methods-
        private OracleParameter PrepareRefCursor()
        {
            return new OracleParameter()
            {
                ParameterName = "o_cursor",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.RefCursor
            };
        }
        #endregion

        #region -Public methods-
        /// <summary>
        /// Selects all Actions from database.
        /// </summary>
        /// <returns></returns>
        public List<Task> SelectAll()
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet("up_action_selectall", new OracleParameter[]{
                p
            });
            List<Task> tasks = new List<Task>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tasks.Add(new Task()
                    {
                        Id = Convert.ToDouble(dr["ActionId"].ToString()),
                        TaskName= dr["ActionName"].ToString(),
                        TaskNote = dr["Note"].ToString()
                    });
                }
            }
            return tasks;
        }
        #endregion
        
        #region -Override BaseEntity-
        protected override void ValidationRules()
        { }
        #endregion
    }
}

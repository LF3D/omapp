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
    /// Represents the Permission object from database.
    /// </summary>
    [Serializable]
    public class Permission : BaseEntity<double, Permission>
    {
        private const string s_up_permission_selectbyln = "up_permission_selectbyln";
        private const string s_up_permission_selectall = "up_permission_selectall";
        /// <summary>
        /// Create a new instance of <see cref="Permission"/> class.
        /// </summary>
        public Permission()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;
        /// <summary>
        /// Gets/Sets the Module.
        /// </summary>
        public Module Module { get; set; }
        /// <summary>
        /// Gets/Sets the Action.
        /// </summary>
        public Task Task{ get; set; }
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
        /// Selects all Permissions from database.
        /// </summary>
        /// <returns></returns>
        public List<Permission> SelectAll()
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_permission_selectall, new OracleParameter[]{
                p
            });
            List<Permission> ps = new List<Permission>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ps.Add(new Permission()
                    {
                        Id = Convert.ToDouble(dr["PermissionId"].ToString()),
                        Module = new Module()
                        {
                            Id = Convert.ToDouble(dr["ModuleId"].ToString()),
                            ModuleName = dr["ModuleName"].ToString(),
                            ModuleNote = dr["ModuleNote"].ToString()
                        },
                        Task = new Task()
                        {
                            Id = Convert.ToDouble(dr["ActionId"].ToString()),
                            TaskName = dr["ActionName"].ToString(),
                            TaskNote = dr["ActionNote"].ToString()
                        }
                    });
                }
            }
            return ps;
        }
        /// <summary>
        /// Selects Permissions from database by user login name.
        /// </summary>
        /// <param name="loginName">The specific login name.</param>
        /// <returns></returns>
        public List<Permission> SelectByLoginName(string loginName)
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_permission_selectbyln, new OracleParameter[]{
                new OracleParameter("p_LoginName",loginName),
                p
            });
            List<Permission> ps = new List<Permission>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ps.Add(new Permission()
                    {
                        Id = Convert.ToDouble(dr["PermissionId"].ToString()),
                        Module = new Module()
                        {
                            Id = Convert.ToDouble(dr["ModuleId"].ToString()),
                            ModuleName = dr["ModuleName"].ToString(),
                            ModuleNote = dr["ModuleNote"].ToString()
                        },
                        Task = new Task()
                        {
                            Id = Convert.ToDouble(dr["ActionId"].ToString()),
                            TaskName = dr["ActionName"].ToString(),
                            TaskNote = dr["ActionNote"].ToString()
                        }
                    });
                }
            }
            return ps;
        }

        #endregion
        
        #region -Override BaseEntity-
        protected override void ValidationRules()
        { }
        #endregion
    }
}

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
    /// Represents the Role object from database.
    /// </summary>
    [Serializable]
    public class Role : BaseEntity<double, Role>
    {
        /// <summary>
        /// Create a new instance of <see cref="Role"/> class.
        /// </summary>
        public Role(){
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;
        /// <summary>
        /// Gets/Sets the role name.
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// Gets/Sets the description/note.
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Gets/Sets the permissions of this role.
        /// </summary>
        public List<Permission> Permissions { get; set; }
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
        /// Deletes the roles by identifications.
        /// </summary>
        /// <param name="ids">The identifications of roles to be deleted.</param>
        /// <returns></returns>
        public FieldVerifyResult DeleteByIds(string ids)
        {
            OracleParameter p = new OracleParameter()
            {
                ParameterName = "v_result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            _database.SpExecuteNonQuery("up_role_deletebyids", new OracleParameter[]{
                new OracleParameter("p_Ids",ids),
                p
            });
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }
        /// <summary>
        /// Selects all Roles from database.
        /// </summary>
        /// <returns></returns>
        public List<Role> SelectAll()
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet("up_role_selectall", new OracleParameter[]{
                p
            });
            List<Role> roles = new List<Role>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    roles.Add(new Role()
                    {
                        Id = Convert.ToDouble(dr["RoleId"].ToString()),
                        CreatedTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        RoleName = dr["RoleName"].ToString(),
                        Note = dr["Note"].ToString()
                    });
                }
            }
            return roles;
        }
        /// <summary>
        /// Selects the specific Role by identification.
        /// </summary>
        /// <returns></returns>
        public Role SelectById()
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet("up_role_selectbyid", new OracleParameter[]{
                new OracleParameter("p_RoleId",this.Id),
                p
            });
            Role role = null;
            if (ds != null && ds.Tables.Count == 1)
            {
                DataRow first = ds.Tables[0].Rows[0];
                role = new Role()
                {
                    Id = Convert.ToDouble(first["RoleId"].ToString()),
                    RoleName = first["RoleName"].ToString(),
                    Note = first["Note"].ToString()
                };
                List<Permission> permissions = new List<Permission>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["PermissionId"] != DBNull.Value)
                    {
                        permissions.Add(new Permission()
                        {
                            Id = Convert.ToDouble(dr["PermissionId"].ToString())
                        });
                    }
                }
                role.Permissions = permissions;
            }
            return role;
        }
        /// <summary>
        /// Inserts a new record into database.
        /// </summary>
        /// <param name="permissions">The permission identifications, e.g.: [1][2][3]...</param>
        /// <returns></returns>
        public FieldVerifyResult Add(string permissions)
        {
            OracleParameter p = new OracleParameter()
            {
                ParameterName="v_result",
                Direction = ParameterDirection.Output,
                OracleDbType= OracleDbType.Double
            }; 
            _database.SpExecuteNonQuery("up_role_insert", new OracleParameter[]{
                new OracleParameter("p_RoleName",this.RoleName),
                new OracleParameter("p_Note",this.Note),
                new OracleParameter("p_Permissions",permissions),
                p
            });
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }
        /// <summary>
        /// UPdates the specific record in database.
        /// </summary>
        /// <param name="permissions">The permission identifications, e.g.: [1][2][3]...</param>
        /// <returns></returns>
        public FieldVerifyResult Update(string permissions)
        {
            OracleParameter p = new OracleParameter()
            {
                ParameterName = "v_result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            _database.SpExecuteNonQuery("up_role_update", new OracleParameter[]{
                new OracleParameter("p_RoleId",this.Id),
                new OracleParameter("p_RoleName",this.RoleName),
                new OracleParameter("p_Note",this.Note),
                new OracleParameter("p_Permissions",permissions),
                p
            });
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }
        #endregion
        
        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
            this.AddValidRules("RoleName", "角色名不能为空。", string.IsNullOrEmpty(RoleName));
        }
        #endregion
    }
}

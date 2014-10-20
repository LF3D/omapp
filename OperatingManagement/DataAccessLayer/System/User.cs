using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using System.Data;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.System
{
    /// <summary>
    /// Represents the User object from database.
    /// </summary>
    [Serializable]
    public class User : BaseEntity<double,User>
    {
        private const string s_up_user_search = "up_user_search";
        /// <summary>
        /// Create a new instance of <see cref="User"/> class.
        /// </summary>
        public User() {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;
        /// <summary>
        /// Gets/Sets the unique login name.
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// Gets/Sets the display name.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Gets/Sets the password.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets/Sets the user type.
        /// </summary>
        public UserType UserType { get; set; }
        /// <summary>
        /// Gets/Sets the user catalog.
        /// </summary>
        public UserCatalog UserCatalog { get; set; }
        /// <summary>
        /// Gets/Sets the status.
        /// </summary>
        public FieldStatus Status { get; set; }
        /// <summary>
        /// Gets/Sets the cellphone number.
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// Gets/Sets the description/note.
        /// </summary>
        public string Note { get; set; }
        #endregion

        #region -Private methods-
        private OracleParameter PrepareRefCursor() {
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
        /// Selects all the users from database.
        /// </summary>
        /// <returns>List of <see cref="User"/> collection.</returns>
        public List<User> SelectAll() {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet("up_user_selectall", new OracleParameter[]{
                p
            });
            List<User> users = new List<User>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    users.Add(new User()
                    {
                        Id = Convert.ToDouble(dr["UserID"].ToString()),
                        CreatedTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        DisplayName = dr["DisplayName"].ToString(),
                        LoginName = dr["LoginName"].ToString(),
                        Mobile = dr["Mobile"].ToString(),
                        Note = dr["Note"].ToString(),
                        Password = dr["Password1"].ToString(),
                        UpdatedTime = Convert.ToDateTime(dr["LastUpdatedTime"].ToString()),
                        Status = (FieldStatus)(Convert.ToInt32(dr["Status"].ToString())),
                        UserType = (UserType)(Convert.ToInt32(dr["UserType"].ToString())),
                        UserCatalog = (UserCatalog)(Convert.ToInt32(dr["UserCatalog"].ToString()))
                    });
                }
            }
            return users;
        }
        /// <summary>
        /// select all user rols by userid.
        /// </summary>
        /// <returns></returns>
        public List<Role> SelectRolesById()
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet("up_user_selectrolesbyid", new OracleParameter[]{
                new OracleParameter("p_UserId",this.Id),
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
        /// Selects Modules by keywords from database.
        /// </summary>
        /// <returns></returns>
        public List<User> Search(string keyword)
        {
            OracleParameter pKeyword = new OracleParameter()
            {
                ParameterName = "p_keyword",
                OracleDbType = OracleDbType.Varchar2,
                Size = 50,
                Value = keyword
            }; 
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_user_search, new OracleParameter[]{
                pKeyword,
                p
            });
            List<User> users = new List<User>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    users.Add(new User()
                    {
                        Id = Convert.ToDouble(dr["UserID"].ToString()),
                        CreatedTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        DisplayName = dr["DisplayName"].ToString(),
                        LoginName = dr["LoginName"].ToString(),
                        Mobile = dr["Mobile"].ToString(),
                        Note = dr["Note"].ToString(),
                        Password = dr["Password1"].ToString(),
                        UpdatedTime = Convert.ToDateTime(dr["LastUpdatedTime"].ToString()),
                        Status = (FieldStatus)(Convert.ToInt32(dr["Status"].ToString())),
                        UserType = (UserType)(Convert.ToInt32(dr["UserType"].ToString())),
                        UserCatalog = (UserCatalog)(Convert.ToInt32(dr["UserCatalog"].ToString()))
                    });
                }
            }
            return users;
        }
        
        /// <summary>
        /// Select all the users by roleid.
        /// </summary>
        /// <returns></returns>
        public List<User> SelectByRoleId(int roleId)
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet("up_user_selectbyroleid", new OracleParameter[]{
                new OracleParameter("p_RoleId", roleId),
                p
            });
            List<User> users = new List<User>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    users.Add(new User()
                    {
                        Id = Convert.ToDouble(dr["UserID"].ToString()),
                        CreatedTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        DisplayName = dr["DisplayName"].ToString(),
                        LoginName = dr["LoginName"].ToString(),
                        Mobile = dr["Mobile"].ToString(),
                        Note = dr["Note"].ToString(),
                        Password = dr["Password1"].ToString(),
                        UpdatedTime = Convert.ToDateTime(dr["LastUpdatedTime"].ToString()),
                        Status = (FieldStatus)(Convert.ToInt32(dr["Status"].ToString())),
                        UserType = (UserType)(Convert.ToInt32(dr["UserType"].ToString())),
                        UserCatalog = (UserCatalog)(Convert.ToInt32(dr["UserCatalog"].ToString()))
                    });
                }
            }
            return users;
        }
        /// <summary>
        /// Inserts a new record into database.
        /// </summary>
        /// <returns></returns>
        public FieldVerifyResult Add()
        {
            OracleParameter p = new OracleParameter()
            {
                ParameterName = "v_result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            OracleParameter opId = new OracleParameter()
            {
                ParameterName = "v_UserId",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            _database.SpExecuteNonQuery("up_user_insert", new OracleParameter[]{
                new OracleParameter("p_LoginName",this.LoginName),
                new OracleParameter("p_DisplayName",this.DisplayName),
                new OracleParameter("p_Password",GlobalSettings.EncryptPassword(this.Password)),
                new OracleParameter("p_UserType",(int)this.UserType),
                new OracleParameter("p_UserCatalog",(int)this.UserCatalog),
                new OracleParameter("p_Status",(int)this.Status),
                new OracleParameter("p_Mobile",this.Mobile),
                new OracleParameter("p_Note",this.Note),
                opId,
                p
            });
            if (opId.Value != null && opId.Value != DBNull.Value)
                this.Id = Convert.ToDouble(opId.Value.ToString());
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }
        /// <summary>
        /// Updates the User object in database.
        /// </summary>
        /// <returns></returns>
        public FieldVerifyResult Update()
        {
            OracleParameter p = new OracleParameter()
            {
                ParameterName = "v_result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            _database.SpExecuteNonQuery("up_user_update", new OracleParameter[]{
                new OracleParameter("p_UserId",this.Id),
                new OracleParameter("p_DisplayName",this.DisplayName),
                new OracleParameter("p_Password",
                    string.IsNullOrEmpty(this.Password)?string.Empty:
                        GlobalSettings.EncryptPassword(this.Password)),
                new OracleParameter("p_UserType",(int)this.UserType),
                new OracleParameter("p_UserCatalog",(int)this.UserCatalog),
                new OracleParameter("p_Status",(int)this.Status),
                new OracleParameter("p_Mobile",this.Mobile),
                new OracleParameter("p_Note",this.Note),
                p
            });
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }
        /// <summary>
        /// Add the specific user to roles.
        /// </summary>
        /// <param name="roles">The roles identifications, e.g.: [1][2][3]...</param>
        /// <returns></returns>
        public bool AddToRoles(string roles) {
            OracleParameter p = new OracleParameter()
            {
                ParameterName = "v_result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            _database.SpExecuteNonQuery("up_user_addtorole", new OracleParameter[]{
                new OracleParameter("p_UserId",this.Id),
                new OracleParameter("p_Roles",roles),
                p
            });
            return Convert.ToDouble(p.Value.ToString()) > 0;
        }
        /// <summary>
        /// Selects the specific user by login name.
        /// </summary>
        /// <returns><see cref="User"/> object.</returns>
        public User SelectByLoginName()
        {
            OracleParameter p = PrepareRefCursor();

            DataSet ds = _database.SpExecuteDataSet("up_user_selectbyloginname", new OracleParameter[]{
                new OracleParameter("p_LoginName", this.LoginName), 
                p
            });

            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return new User()
                    {
                        Id = Convert.ToDouble(dr["UserID"].ToString()),
                        CreatedTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        DisplayName = dr["DisplayName"].ToString(),
                        LoginName = dr["LoginName"].ToString(),
                        Mobile = dr["Mobile"].ToString(),
                        Note = dr["Note"].ToString(),
                        Password = dr["Password1"].ToString(),
                        UpdatedTime = Convert.ToDateTime(dr["LastUpdatedTime"].ToString()),
                        Status = (FieldStatus)(Convert.ToInt32(dr["Status"].ToString())),
                        UserType = (UserType)(Convert.ToInt32(dr["UserType"].ToString())),
                        UserCatalog = (UserCatalog)(Convert.ToInt32(dr["UserCatalog"].ToString()))
                    };
                }
            }
            return null;
        }
        /// <summary>
        /// Deletes the users by identifications.
        /// </summary>
        /// <param name="ids">The identification of users to be deleted, split by ','.</param>
        public FieldVerifyResult DeleteByIds(string ids)
        {
            OracleParameter p = new OracleParameter()
            {
                ParameterName = "v_result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            _database.SpExecuteNonQuery("up_user_deletebyids", new OracleParameter[]{
                new OracleParameter("p_Ids",ids),
                p
            });
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }
        /// <summary>
        /// Selects the specific user by identification.
        /// </summary>
        /// <returns><see cref="User"/> object.</returns>
        public User SelectById()
        {
            OracleParameter p = PrepareRefCursor();

            DataSet ds = _database.SpExecuteDataSet("up_user_selectbyid", new OracleParameter[]{
                new OracleParameter("p_UserId", this.Id), 
                p
            });

            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return new User()
                    {
                        Id = Convert.ToDouble(dr["UserID"].ToString()),
                        CreatedTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        DisplayName = dr["DisplayName"].ToString(),
                        LoginName = dr["LoginName"].ToString(),
                        Mobile = dr["Mobile"].ToString(),
                        Note = dr["Note"].ToString(),
                        Password = dr["Password1"].ToString(),
                        UpdatedTime = Convert.ToDateTime(dr["LastUpdatedTime"].ToString()),
                        Status = (FieldStatus)(Convert.ToInt32(dr["Status"].ToString())),
                        UserType = (UserType)(Convert.ToInt32(dr["UserType"].ToString())),
                        UserCatalog = (UserCatalog)(Convert.ToInt32(dr["UserCatalog"].ToString()))
                    };
                }
            }
            return null;
        }
        
        /// <summary>
        /// Verifies the specific user by LoginName and Password(Not be encrypted).
        /// </summary>
        /// <param name="needEncryptPassword">A value indicating that the Password should be encrypted or not.</param>
        /// <returns><see cref="FieldVerifyResult"/> object.</returns>
        public FieldVerifyResult Verify()
        {
            return Verify(true);
        }
        /// <summary>
        /// Verifies the specific user by LoginName and Password.
        /// </summary>
        /// <param name="needEncryptPassword">A value indicating that the Password should be encrypted or not.</param>
        /// <returns><see cref="FieldVerifyResult"/> object.</returns>
        public FieldVerifyResult Verify(bool needEncryptPassword)
        {
            try
            {
                User u = this.SelectByLoginName();
                if (u == null)
                    return FieldVerifyResult.NotExist;

                string pwd = this.Password;
                if (needEncryptPassword)
                    pwd = GlobalSettings.EncryptPassword(this.Password);

                if (u.Password != pwd)
                    return FieldVerifyResult.PasswordIncorrect;

                if (u.Status != FieldStatus.Active)
                    return FieldVerifyResult.Inactive;

                this.Id = u.Id;
                this.DisplayName = u.DisplayName;
                this.CreatedTime = u.CreatedTime;
                this.Mobile = u.LoginName;
                this.Mobile = u.Mobile;
                this.Note = u.Note;
                this.Status = u.Status;
                this.UpdatedTime = u.UpdatedTime;
                this.UserType = u.UserType;
                this.UserCatalog = u.UserCatalog;
                return FieldVerifyResult.Success;
            }
            catch(Exception ex)
            {
                throw ex;
                //return FieldVerifyResult.Error;
            }
        }
        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
            this.AddValidRules("LoginName", "登录名不能为空。", string.IsNullOrEmpty(LoginName));
            this.AddValidRules("DisplayName", "显示名不能为空。", string.IsNullOrEmpty(LoginName));
            this.AddValidRules("Password", "密码不能为空。", string.IsNullOrEmpty(LoginName));
        }
        #endregion
    }
}

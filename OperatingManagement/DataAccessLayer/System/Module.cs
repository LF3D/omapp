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
    /// Represents the Module object from database.
    /// </summary>
    [Serializable]
    public class Module : BaseEntity<double, Module>
    {

        private const string s_up_module_insert = "up_module_insert";
        private const string s_up_module_deletebyids = "up_module_deletebyids";
        private const string s_up_module_update = "up_module_update";
        private const string s_up_module_selectall = "up_module_selectall";
        private const string s_up_module_search = "up_module_search";
        private const string s_up_module_selectbyid = "up_module_selectbyid";

        /// <summary>
        /// Create a new instance of <see cref="Module"/> class.
        /// </summary>
        public Module(){
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;
        /// <summary>
        /// Gets/Sets the module name.
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// Gets/Sets the description/note.
        /// </summary>
        public string ModuleNote { get; set; }
        /// <summary>
        /// Gets/Sets the description/note.
        /// </summary>
        public DateTime CTime { get; set; }

        /// <summary>
        /// gets the module actions
        /// </summary>
        public string ActionIds { get; set; }
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
        /// Inserts a new record into database.
        /// </summary>
        /// <param name="permissions">The action identifications, e.g.: [1][2][3]...</param>
        /// <returns></returns>
        public FieldVerifyResult Add()
        {
            OracleParameter p = new OracleParameter()
            {
                ParameterName = "v_result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            _database.SpExecuteNonQuery(s_up_module_insert, new OracleParameter[]{
                new OracleParameter("p_ModuleName", this.ModuleName),
                new OracleParameter("p_ModuleNote", this.ModuleNote),
                new OracleParameter("p_Actions", this.ActionIds),
                p
            });
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }

        /// <summary>
        /// Deletes the permissions by identifications.
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
            _database.SpExecuteNonQuery(s_up_module_deletebyids, new OracleParameter[]{
                new OracleParameter("p_Ids",ids),
                p
            });
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }

        /// <summary>
        /// UPdates the specific record in database.
        /// </summary>
        /// <param name="permissions">The permission identifications, e.g.: [1][2][3]...</param>
        /// <returns></returns>
        public FieldVerifyResult Update()
        {
            OracleParameter p = new OracleParameter()
            {
                ParameterName = "v_result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            _database.SpExecuteNonQuery(s_up_module_update, new OracleParameter[]{
                new OracleParameter("p_Id", this.Id),
                new OracleParameter("p_ModuleName", this.ModuleName),
                new OracleParameter("p_ModuleNote", this.ModuleNote),
                new OracleParameter("p_Actions", this.ActionIds),
                p 
            });
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }


        /// <summary>
        /// Selects the specific module by identification.
        /// </summary>
        /// <returns></returns>
        public Module SelectById()
        {
            OracleParameter opActionIds = new OracleParameter()
            {
                ParameterName = "v_actionids",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Varchar2,
                Size = 100
            }; 
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_module_selectbyid, new OracleParameter[]{
                new OracleParameter("p_Id",this.Id),
                opActionIds,
                p
            });
            Module module = null;
            if (ds != null && ds.Tables.Count == 1)
            {
                DataRow first = ds.Tables[0].Rows[0];
                module = new Module()
                {
                    Id = Convert.ToDouble(first["ModuleId"].ToString()),
                    ModuleName = first["ModuleName"].ToString(),
                    ModuleNote = first["Note"].ToString(),
                    CTime = DateTime.Parse(first["CTime"].ToString()),
                    ActionIds = opActionIds.Value.ToString()
                };
            }
            return module;
        }

        /// <summary>
        /// Selects Modules by keywords from database.
        /// </summary>
        /// <returns></returns>
        public List<Module> Search(string keyword)
        {
            OracleParameter pKeyword = new OracleParameter()
            {
                ParameterName = "p_keyword",
                OracleDbType = OracleDbType.Varchar2,
                Size = 50,
                Value = keyword
            }; 
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_module_search, new OracleParameter[]{
                pKeyword,
                p
            });
            List<Module> modules = new List<Module>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    modules.Add(new Module()
                    {
                        Id = Convert.ToDouble(dr["ModuleId"].ToString()),
                        ModuleName = dr["ModuleName"].ToString(),
                        ModuleNote = dr["Note"].ToString(),
                        CTime = DateTime.Parse(dr["CTime"].ToString())
                    });
                }
            }
            return modules;
        }

        /// <summary>
        /// Selects all Modules from database.
        /// </summary>
        /// <returns></returns>
        public List<Module> SelectAll()
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_module_selectall, new OracleParameter[]{
                p
            });
            List<Module> modules = new List<Module>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    modules.Add(new Module()
                    {
                        Id = Convert.ToDouble(dr["ModuleId"].ToString()),
                        ModuleName= dr["ModuleName"].ToString(),
                        ModuleNote = dr["Note"].ToString(),
                        CTime = DateTime.Parse(dr["CTime"].ToString())
                    });
                }
            }
            return modules;
        }
        #endregion
        
        #region -Override BaseEntity-
        protected override void ValidationRules()
        { }
        #endregion
    }
}

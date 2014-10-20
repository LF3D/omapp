using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using System.Data;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.PlanManage
{
    /// <summary>
    /// 引导数据
    /// </summary>
    public class YDSJ : BaseEntity<int, YDSJ>
    {
        private static readonly string GET_YDSJList_ByDate = "up_ydsj_getlist";
        private static readonly string SelectByID = "up_ydsj_selectByID";
        private static readonly string s_up_ydsj_insert = "up_ydsj_insert";

        /// <summary>
        /// Create a new instance of <see cref="YDSJ"/> class.
        /// </summary>
        public YDSJ()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CTime { get; set; }
        /// <summary>
        /// 任务代号
        /// </summary>
        public string TaskID { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 卫星名称
        /// </summary>
        public string SatID { get; set; }
        /// <summary>
        /// 卫星名称
        /// </summary>
        public string SatName { get; set; }
        /// <summary>
        /// 文件数据开始时间
        /// </summary>
        public DateTime DataBTime { get; set; }
        /// <summary>
        /// 文件数据结束时间
        /// </summary>
        public DateTime DataETime { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        #region 不用的属性
        ///// <summary>
        ///// 历元日期
        ///// </summary>
        //public int D { get; set; }
        ///// <summary>
        ///// 历元时刻
        ///// </summary>
        //public int T { get; set; }
        ///// <summary>
        ///// 历元时间
        ///// </summary>
        //public DateTime Times { get; set; }
        ///// <summary>
        ///// 轨道半长径
        ///// </summary>
        //public double A { get; set; }
        ///// <summary>
        ///// 轨道偏心率
        ///// </summary>
        //public double E { get; set; }
        ///// <summary>
        ///// 轨道倾角
        ///// </summary>
        //public double I { get; set; }
        ///// <summary>
        ///// 轨道升交点赤径 
        ///// </summary>
        //public double O { get; set; }
        ///// <summary>
        ///// 轨道近地点幅角
        ///// </summary>
        //public double W { get; set; }
        ///// <summary>
        ///// 平近点角
        ///// </summary>
        //public double M { get; set; }
        ///// <summary>
        ///// 备注
        ///// </summary>
        //public string Reserve { get; set; }
        #endregion
        #endregion

        #region -Methods-
        /// <summary>
        /// 获取引导数据（空间机动任务，非空间机动任务）
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<YDSJ> GetListByDate(DateTime startDate, DateTime endDate, string taskID)
        {
            DataSet ds = null;

            ds = new DataSet();
            ds.Tables.Add();
            OracleCommand command = _database.GetStoreProcCommand(GET_YDSJList_ByDate);

            if (startDate == DateTime.MinValue)
            {
                _database.AddInParameter(command, "p_startDate", OracleDbType.Date, DBNull.Value);
            }
            else
            {
                _database.AddInParameter(command, "p_startDate", OracleDbType.Date, startDate);
            }
            if (endDate == DateTime.MinValue)
            {
                _database.AddInParameter(command, "p_endDate", OracleDbType.Date, DBNull.Value);
            }
            else
            {
                _database.AddInParameter(command, "p_endDate", OracleDbType.Date, endDate);
            }
            _database.AddInParameter(command, "p_TaskID", OracleDbType.Varchar2, taskID);
            using (IDataReader reader = _database.ExecuteReader(command))
            {
                ds.Tables[0].Load(reader);
            }

            List<YDSJ> objDatas = new List<YDSJ>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    objDatas.Add(new YDSJ()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = DateTime.Parse(dr["CTIME"].ToString()),
                        TaskID = dr["TaskID"].ToString(),
                        TaskName = dr["TaskName"].ToString(),
                        SatName = dr["SatName"].ToString(),
                        SatID = dr["SatID"].ToString(),
                        DataBTime = DateTime.Parse(dr["DataBTime"].ToString()),
                        DataETime = DateTime.Parse(dr["DataETime"].ToString()),
                        FileName = dr["FileName"].ToString(),
                        FilePath = dr["FilePath"].ToString()
                        //D = Convert.ToInt32(dr["D"].ToString()),
                        //T = Convert.ToInt32(dr["T"].ToString()),
                        //Times = DateTime.Parse(dr["Times"].ToString()),
                        //A = Convert.ToDouble(dr["A"].ToString()),
                        //E = Convert.ToDouble(dr["E"].ToString()),
                        //I = Convert.ToDouble(dr["I"].ToString()),
                        //O = Convert.ToDouble(dr["O"].ToString()),
                        //W = Convert.ToDouble(dr["W"].ToString()),
                        //M = Convert.ToDouble(dr["M"].ToString()),
                        //Reserve = dr["RESERVE"].ToString()
                    });
                }
            }
            return objDatas;
        }

        /// <summary>
        /// Selects the specific YDSJ by identification.
        /// </summary>
        /// <returns>YDSJ</returns>
        public YDSJ SelectById()
        {
            OracleParameter p = PrepareRefCursor();

            DataSet ds = _database.SpExecuteDataSet(SelectByID, new OracleParameter[]{
                new OracleParameter("p_Id", this.Id), 
                p
            });

            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return new YDSJ()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = DateTime.Parse(dr["CTIME"].ToString()),
                        TaskID = dr["TaskID"].ToString(),
                        SatID = dr["SatID"].ToString(),
                        DataBTime = DateTime.Parse(dr["DataBTime"].ToString()),
                        DataETime = DateTime.Parse(dr["DataETime"].ToString()),
                        FileName = dr["FileName"].ToString(),
                        FilePath = dr["FilePath"].ToString()
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// 单个或多个ID，如("1,2,3")
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<YDSJ> SelectByIDS(string ids)
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet("up_ydsj_selectinids", new OracleParameter[]{
                new OracleParameter("p_ids",ids),
                p
            });
            List<YDSJ> jhs = new List<YDSJ>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    jhs.Add(new YDSJ()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = DateTime.Parse(dr["CTIME"].ToString()),
                        TaskID = dr["TaskID"].ToString(),
                        SatID = dr["SatID"].ToString(),
                        DataBTime = DateTime.Parse(dr["DataBTime"].ToString()),
                        DataETime = DateTime.Parse(dr["DataETime"].ToString()),
                        FileName = dr["FileName"].ToString(),
                        FilePath = dr["FilePath"].ToString()
                    });
                }
            }
            return jhs;
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
                ParameterName = "v_Id",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };
            _database.SpExecuteNonQuery(s_up_ydsj_insert, new OracleParameter[]{
                new OracleParameter("p_CTime", DateTime.Now),
                new OracleParameter("p_TaskID", this.TaskID),
                new OracleParameter("p_SatID", this.SatID),
                new OracleParameter("p_DataBTime", this.DataBTime),
                new OracleParameter("p_DataETime", this.DataETime),
                new OracleParameter("p_FileName", this.FileName),
                new OracleParameter("p_FilePath", this.FilePath),
                //new OracleParameter("p_D", this.D),
                //new OracleParameter("p_T", this.T),
                //new OracleParameter("p_Times", this.Times),
                //new OracleParameter("p_A", this.A),
                //new OracleParameter("p_E", this.E),
                //new OracleParameter("p_I", this.I),
                //new OracleParameter("p_O", this.O),
                //new OracleParameter("p_W", this.W),
                //new OracleParameter("p_M", this.M),
                //new OracleParameter("p_Reserve", this.Reserve),
                opId,
                p
            });
            if (opId.Value != null && opId.Value != DBNull.Value)
                this.Id = Convert.ToInt32(opId.Value.ToString());
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }

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

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
            //this.AddValidRules("ID", "序号不能为空。", string.IsNullOrEmpty(ID));
        }
        #endregion
    }
}

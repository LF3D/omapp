using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using System.Data;
using Oracle.DataAccess.Client;


namespace OperatingManagement.DataAccessLayer.PlanManage
{
    /// <summary>
    /// 实验计划
    /// </summary>
    public class SYJH : BaseEntity<int, SYJH>
    {
        private static readonly string GET_SYJHList_ByDate = "UP_SYJH_GETLIST";
        private static readonly string GET_SYJH_ByID = "UP_SYJH_SELECTBYID";

        /// <summary>
        /// Create a new instance of <see cref="SYJH"/> class.
        /// </summary>
        public SYJH()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;
        /// <summary>
        /// 编号
        /// </summary>
        public int JHID { get; set; }
        public DateTime CTime { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string TaskID { get; set; }
        public string SatID { get; set; }
        public string InfoType { get; set; }
        public int LineCount { get; set; }
        public SYJHType PlanType { get; set; }
        public int PlanID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string FileIndex { get; set; }
        public string Reserve { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 试验个数
        /// </summary>
        public string SYCount { get; set; }
        /// <summary>
        /// 试验列表
        /// </summary>
        public List<SYJH_SY> SYJH_SY_List { get; set; }
        #endregion

        #region -Methods-
        /// <summary>
        /// 根据时间获取试验计划列表
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public List<SYJH> GetListByDate(DateTime startDate, DateTime endDate)
        {
            DataSet ds = null;

            ds = new DataSet();
            ds.Tables.Add();
            OracleCommand command = _database.GetStoreProcCommand(GET_SYJHList_ByDate);
            if (startDate != DateTime.MinValue)
            {
                _database.AddInParameter(command, "p_startDate", OracleDbType.Date, DBNull.Value);
            }
            else
            {
                _database.AddInParameter(command, "p_startDate", OracleDbType.Date, startDate);
            }
            if (endDate != DateTime.MinValue)
            {
                _database.AddInParameter(command, "p_endDate", OracleDbType.Date, DBNull.Value);
            }
            else
            {
                _database.AddInParameter(command, "p_endDate", OracleDbType.Date, endDate);
            }
            _database.AddOutParameter(command, "o_cursor", OracleDbType.RefCursor, 0);
            using (IDataReader reader = _database.ExecuteReader(command))
            {
                ds.Tables[0].Load(reader);
            }

            List<SYJH> objDatas = new List<SYJH>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    objDatas.Add(new SYJH()
                    {
                        Id = Convert.ToInt32(dr["JHID"].ToString()),
                        CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        TaskID = dr["TASKID"].ToString(),
                        Source = dr["SOURCE"].ToString(),
                        Destination = dr["DESTINATION"].ToString(),
                        InfoType = dr["INFOTYPE"].ToString(),
                        LineCount = Convert.ToInt32(dr["LINECOUNT"].ToString()),
                        FileIndex = dr["FILEINDEX"].ToString(),
                        Reserve = dr["RESERVE"].ToString()
                    });
                }
            }

            return objDatas;
        }

        /// <summary>
        /// Selects the specific SYJH by identification.
        /// </summary>
        /// <returns>SYJH</returns>
        public SYJH SelectById()
        {
            OracleParameter p = PrepareRefCursor();

            DataSet ds = _database.SpExecuteDataSet(GET_SYJH_ByID, new OracleParameter[]{
                new OracleParameter("p_Id", this.Id), 
                p
            });

            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return new SYJH()
                    {
                        Id = Convert.ToInt32(dr["JHID"].ToString()),
                        CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        TaskID = dr["TASKID"].ToString(),
                        Source = dr["SOURCE"].ToString(),
                        Destination = dr["DESTINATION"].ToString(),
                        InfoType = dr["INFOTYPE"].ToString(),
                        LineCount = Convert.ToInt32(dr["LINECOUNT"].ToString()),
                        FileIndex = dr["FILEINDEX"].ToString(),
                        Reserve = dr["RESERVE"].ToString()
                    };
                }
            }
            return null;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format">1:xml;3:DATA</param>
        /// <returns></returns>
        public override string ToString()
        {
            int iSYCount = 0;
            XDocument doc = new XDocument();
            doc.Add(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("注：试验计划。"));
            XElement root = new XElement("试验计划");
            root.Add(
                new XElement("编号", this.PlanID),
                new XComment("从1开始编号，用4位字符标识，范围为0001～9999"),
                new XElement("时间", DateTime.Now.ToString("YYYYMMDDHHMMSS")),
                new XComment("为信源生成该文件时的北京时日期和时间（24时制）"));
            if (this.SYJH_SY_List != null)
                iSYCount = this.SYJH_SY_List.Count;
            root.Add(new XElement("试验个数"), iSYCount);
            XElement xSY = new XElement("试验");
            if (iSYCount > 0)
            {
                int iIdx = 1;
                foreach (SYJH_SY sy in this.SYJH_SY_List)
                {
                    xSY.Add(new XElement("试验-" + iIdx.ToString()),
                                new XElement("卫星名称", sy.SYSatName),
                                new XComment("对于本任务填写“探索三号卫星”、“探索四号卫星”或“探索五号卫星"),
                                new XElement("试验类别", sy.SYType),
                                new XComment("根据卫星填写"),
                                new XElement("试验项目", sy.SYType),
                                new XComment("根据试验类别填写"),
                                new XElement("开始时间", sy.SYStartTime),
                                new XComment("为试验开始时的北京时日期和时间（24时制）"),
                                new XElement("结束时间", sy.SYEndTime),
                                new XComment("为试验结束时的北京时日期和时间（24时制）"),
                                new XElement("系统名称", sy.SYSysName),
                                new XComment("系统名称"),
                                new XElement("系统任务", sy.SYSysTask),
                                new XComment("系统任务"));

                    iIdx++;
                }
            }
            root.Add(xSY);
            doc.Add(root);
            return doc.ToString();
        }
        #endregion

    }

    [Serializable]
    public class SYJH_SY
    {
        /// <summary>
        /// 卫星名称
        /// </summary>
        public string SYSatName { get; set; }
        /// <summary>
        /// 试验类别
        /// </summary>
        public string SYType { get; set; }
        /// <summary>
        /// 试验项目
        /// </summary>
        public string SYItem { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string SYStartTime { get; set; }
        /// <summary>
        /// 结束时间 
        /// </summary>
        public string SYEndTime { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SYSysName { get; set; }
        /// <summary>
        /// 系统任务
        /// </summary>
        public string SYSysTask { get; set; }
    }
}

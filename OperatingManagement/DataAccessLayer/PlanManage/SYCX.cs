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
    /// 实验程序
    /// Represents the SYCX object from database.
    /// </summary>
    public class SYCX : BaseEntity<int, SYCX>
    {
        private static readonly string GET_SYCXList_ByDate = "UP_SYCX_GETLIST";
        private static readonly string GET_SYCX_ByID = "UP_SYCX_SELECTBYID";

        /// <summary>
        /// Create a new instance of <see cref="SYCX"/> class.
        /// </summary>
        public SYCX()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;

        public int ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CTime { get; set; }
        /// <summary>
        /// 任务代号
        /// </summary>
        public string TaskID { get; set; }
        /// <summary>
        /// 实验程序类型
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 试验程序编号
        /// </summary>
        public int PNo { get; set; }
        /// <summary>
        /// 对应计划编号
        /// </summary>
        public int PCount { get; set; }
        /// <summary>
        /// 试验开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 试验结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 信息类型
        /// </summary>
        public string InfoType { get; set; }
        /// <summary>
        /// 数据区行数
        /// </summary>
        public int LineCount { get; set; }
        /// <summary>
        /// 文件索引
        /// </summary>
        public string FileIndex { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Reserve { get; set; }
        #endregion

        #region -Methods-
        /// <summary>
        /// 根据时间获取试验程序列表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<SYCX> GetListByDate(DateTime startDate, DateTime endDate, DateTime jhStartDate, DateTime jhEndDate)
        {
            DataSet ds = null;

            ds = new DataSet();
            ds.Tables.Add();
            OracleCommand command = _database.GetStoreProcCommand(GET_SYCXList_ByDate);

            if (startDate == DateTime.MinValue)
                _database.AddInParameter(command, "p_startDate", OracleDbType.Date, DBNull.Value);
            else
                _database.AddInParameter(command, "p_startDate", OracleDbType.Date, startDate);
            if (endDate == DateTime.MinValue)
                _database.AddInParameter(command, "p_endDate", OracleDbType.Date, DBNull.Value);
            else
                _database.AddInParameter(command, "p_endDate", OracleDbType.Date, endDate);


            if (jhStartDate == DateTime.MinValue)
                _database.AddInParameter(command, "p_jhStartDate", OracleDbType.Date, DBNull.Value);
            else
                _database.AddInParameter(command, "p_jhStartDate", OracleDbType.Date, jhStartDate);
            if (jhEndDate == DateTime.MinValue)
                _database.AddInParameter(command, "p_jhEndDate", OracleDbType.Date, DBNull.Value);
            else
                _database.AddInParameter(command, "p_jhEndDate", OracleDbType.Date, jhEndDate);

            using (IDataReader reader = _database.ExecuteReader(command))
            {
                ds.Tables[0].Load(reader);
            }

            List<SYCX> objDatas = new List<SYCX>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    objDatas.Add(new SYCX()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        ID = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        TaskID = dr["TASKID"].ToString(),
                        Version = dr["Version"].ToString(),
                        PNo = Convert.ToInt32(dr["PNO"].ToString()),
                        PCount = Convert.ToInt32(dr["PCount"].ToString()),
                        StartTime = Convert.ToDateTime(dr["STARTTIME"].ToString()),
                        EndTime = Convert.ToDateTime(dr["ENDTIME"].ToString()),
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
        /// Selects the specific SYCX by identification.
        /// </summary>
        /// <returns>SYCX</returns>
        public SYCX SelectById()
        {
            OracleParameter p = PrepareRefCursor();

            DataSet ds = _database.SpExecuteDataSet(GET_SYCX_ByID, new OracleParameter[]{
                new OracleParameter("p_Id", this.Id), 
                p
            });

            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return new SYCX()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        ID = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        TaskID = dr["TASKID"].ToString(),
                        Version = dr["Version"].ToString(),
                        PNo = Convert.ToInt32(dr["PNO"].ToString()),
                        PCount = Convert.ToInt32(dr["PCount"].ToString()),
                        StartTime = Convert.ToDateTime(dr["STARTTIME"].ToString()),
                        EndTime = Convert.ToDateTime(dr["ENDTIME"].ToString()),
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
        #endregion

    }
}
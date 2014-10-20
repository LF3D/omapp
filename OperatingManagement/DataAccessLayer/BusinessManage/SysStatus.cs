using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using System.Data;
using Oracle.DataAccess.Client;


namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    [Serializable]
    public class SysStatus : BaseEntity<int, SysStatus>
    {
        public SysStatus()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;
        private string s_up_sysstatus_insert = "up_sysstatus_insert";
        private string s_up_sysstatus_stat = "up_sysstatus_stat";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CTime { get; set; }
        /// <summary>
        /// 系统类型:DMZ地面站，WX卫星，ZX中心系统，FXT分系统
        /// </summary>
        public string SysType { get; set; }
        /// <summary>
        /// 系统编号
        /// </summary>
        public string SysID { get; set; }
        /// <summary>
        /// 系统状态，01异常到正常，10正常到异常
        /// </summary>
        public string Status { get; set; }
        #endregion

        #region Methods

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
            _database.SpExecuteNonQuery(s_up_sysstatus_insert, new OracleParameter[]{
                new OracleParameter("p_CTime", DateTime.Now),
                new OracleParameter("p_SysType", this.SysType),
                new OracleParameter("p_SysID", this.SysID),
                new OracleParameter("p_Status", this.Status),
                opId,
                p
            });
            if (opId.Value != null && opId.Value != DBNull.Value)
                this.Id = Convert.ToInt32(opId.Value.ToString());
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }

        /// <summary>
        /// 按系统类型获取异常统计信息.
        /// </summary>
        /// <returns>YDSJ</returns>
        public List<SysStatusStat> GetStatInfo(DateTime beginTime, DateTime endTime, SysTypes systype)
        {
            OracleParameter p = PrepareRefCursor();

            DataSet ds = _database.SpExecuteDataSet(s_up_sysstatus_stat, new OracleParameter[]{
                new OracleParameter("p_BeginDate", beginTime), 
                new OracleParameter("p_EndDate", endTime), 
                new OracleParameter("p_SysType", systype.ToString()),
                p
            });

            List<SysStatusStat> sInfos = null;
            if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
            {
                sInfos = new List<SysStatusStat>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sInfos.Add(new SysStatusStat()
                    {
                        StatDate = dr["StatDate"].ToString(),
                        SysID = dr["SysID"].ToString(),
                        ErrorCount = Convert.ToInt32(dr["ErrorCount"].ToString())
                    });
                }
            }
            return sInfos;
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

    [Serializable]
    public class SysStatusStat
    {
        /// <summary>
        /// 统计日期，yyyyMMdd
        /// </summary>
        public string StatDate { get; set; }
        /// <summary>
        /// 系统编号
        /// </summary>
        public string SysID { get; set; }
        /// <summary>
        /// 异常次数
        /// </summary>
        public int ErrorCount { get; set; }
    }

    public enum SysTypes
    {
        DMZ = 0,
        WX = 1,
        ZX = 2,
        FXT = 3
    }
}

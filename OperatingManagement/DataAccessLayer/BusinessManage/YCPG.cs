using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Data;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    [Serializable]
    public class YCPG : BaseEntity<int, YCPG>
    {
        private OracleDatabase _database = null;
        private const string s_up_ycpg_getsysj = "up_ycpg_getsysj";
        private const string s_up_ycpg_getbyid = "up_ycpg_getbyid";

        public YCPG()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        public YCPG(DataRow dr, bool getBlob)
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
            if (dr["Id"] != DBNull.Value)
                this.Id = Convert.ToInt32(dr["Id"].ToString());
            this.TaskID = dr["TaskID"] == DBNull.Value ? string.Empty : dr["TaskID"].ToString();
            this.SatID = dr["SatID"] == DBNull.Value ? string.Empty : dr["SatID"].ToString();
            if (dr["CTime"] != DBNull.Value)
                this.CTime = DateTime.Parse(dr["CTime"].ToString());
            if (dr["DataTimeB"] != DBNull.Value)
                this.STTimeStart = DateTime.Parse(dr["DataTimeB"].ToString());
            if (dr["DataTimeE"] != DBNull.Value)
                this.STTimeEnd = DateTime.Parse(dr["DataTimeE"].ToString());
            this.SType = dr["SType"] == DBNull.Value ? string.Empty : dr["SType"].ToString();
            this.Reserve = dr["Reserve"] == DBNull.Value ? string.Empty : dr["Reserve"].ToString();
            if (getBlob)
            {
                if (dr["STBlob"] != DBNull.Value)
                    this.STBlob = (byte[])dr["STBlob"];
            }
        }

        #region -Properties-
        public DateTime CTime { get; set; }
        public string TaskID { get; set; }
        public string SatID { get; set; }
        public DateTime STTimeStart { get; set; }
        public DateTime STTimeEnd { get; set; }
        public string SType { get; set; }
        public string Reserve { get; set; }
        public byte[] STBlob { get; set; }
        #endregion

        #region -Private methods
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

        #region -Public methods
        /// <summary>
        /// 搜索遥测评估中的试验数据
        /// </summary>
        /// <param name="beginTime">CTime开始时间</param>
        /// <param name="endTime">CTime结束时间</param>
        /// <param name="sType">数据类型</param>
        /// <param name="taskNo">任务代号</param>
        /// <param name="satID">卫星编号</param>
        /// <returns></returns>
        public List<YCPG> GetSYSJ(DateTime beginTime, DateTime endTime, string sType, string taskNo, string satID)
        {
            OracleParameter p = PrepareRefCursor();

            object oBeginTime = null;
            object oEndTime = null;
            if (beginTime == DateTime.MinValue)
                oBeginTime = DBNull.Value;
            else
                oBeginTime = beginTime;
            if (endTime == DateTime.MinValue)
                oEndTime = DBNull.Value;
            else
                oEndTime = endTime;

            DataSet ds = _database.SpExecuteDataSet(s_up_ycpg_getsysj, new OracleParameter[]{
                new OracleParameter("p_SType", sType),
                new OracleParameter("p_TaskNo", taskNo),
                new OracleParameter("p_SatID", satID),
                new OracleParameter("p_BeginTime", OracleDbType.Date, oBeginTime, ParameterDirection.Input),
                new OracleParameter("p_EndTime", OracleDbType.Date, oEndTime, ParameterDirection.Input),
                p
            });

            List<YCPG> sinfos = new List<YCPG>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sinfos.Add(new YCPG(dr, false));
                }
            }
            return sinfos;
        }

        /// <summary>
        /// 按ID取数据
        /// </summary>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public YCPG GetByID(int dataID)
        {
            //get row
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_ycpg_getbyid, new OracleParameter[]{
                new OracleParameter("p_id", dataID),
                p
            });
            YCPG sinfo = null;
            if (ds != null && ds.Tables.Count == 1)
            {
                if (ds.Tables[0].Rows.Count >= 1)
                    sinfo = new YCPG(ds.Tables[0].Rows[0], true);
            }
            return sinfo;
        }
        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
        }
        #endregion
    }
}

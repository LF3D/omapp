using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    [Serializable]
    public class UserFrame : BaseEntity<int, UserFrame>
    {
        private OracleDatabase _database = null;

        public UserFrame()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }
        public UserFrame(DataRow dr)
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
            if (dr["Id"] != DBNull.Value)
                this.Id = Convert.ToInt32(dr["Id"].ToString());
            this.TaskID = dr["TaskID"] == DBNull.Value ? string.Empty : dr["TaskID"].ToString();
            this.SatID = dr["SatID"] == DBNull.Value ? string.Empty : dr["SatID"].ToString();
            if (dr["CTime"] != DBNull.Value)
                this.CTime = DateTime.Parse(dr["CTime"].ToString());
            if (dr["RECEIVEB"] != DBNull.Value)
                this.RECEIVEB = DateTime.Parse(dr["RECEIVEB"].ToString());
            if (dr["RECEIVEE"] != DBNull.Value)
                this.RECEIVEE = DateTime.Parse(dr["RECEIVEE"].ToString());
            this.Userid = dr["Userid"] == DBNull.Value ? string.Empty : dr["Userid"].ToString();
            this.DelaySI = dr["DelaySI"] == DBNull.Value ? string.Empty : dr["DelaySI"].ToString();
            if (dr["DATATIMEB"] != DBNull.Value)
                this.DATATIMEB = DateTime.Parse(dr["DATATIMEB"].ToString());
            if (dr["DATATIMEE"] != DBNull.Value)
                this.DATATIMEE = DateTime.Parse(dr["DATATIMEE"].ToString());
            this.Directory = dr["Directory"] == DBNull.Value ? string.Empty : dr["Directory"].ToString();
            this.FileName = dr["FileName"] == DBNull.Value ? string.Empty : dr["FileName"].ToString();
            this.Reserve = dr["Reserve"] == DBNull.Value ? string.Empty : dr["Reserve"].ToString();
        }
        #region -Properties-
        private const string s_up_userframe_getsysj = "up_userframe_getsysj";
        private const string s_up_userframe_getbyid = "up_userframe_getbyid";

        public DateTime CTime { get; set; }
        public string TaskID { get; set; }
        public string SatID { get; set; }
        public DateTime RECEIVEB { get; set; }
        public DateTime RECEIVEE { get; set; }
        public string Userid { get; set; }
        public string DelaySI { get; set; }
        public DateTime DATATIMEB { get; set; }
        public DateTime DATATIMEE { get; set; }
        public int FrameCount { get; set; }
        public int FileSize { get; set; }
        public string Directory { get; set; }
        public string FileName { get; set; }
        public string Reserve { get; set; }
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
        /// 获取用户帧中的试验数据（图像类）
        /// </summary>
        /// <param name="beginTime">CTime开始时间</param>
        /// <param name="endTime">CTime结束时间</param>
        /// <param name="taskNo">任务代号</param>
        /// <param name="satID">卫星编号</param>
        /// <returns></returns>
        public List<UserFrame> GetSYSJ(DateTime beginTime, DateTime endTime, string taskNo, string satID, string dataType)
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

            DataSet ds = _database.SpExecuteDataSet(s_up_userframe_getsysj, new OracleParameter[]{
                new OracleParameter("p_TaskNo", taskNo),
                new OracleParameter("p_SatID", satID),
                new OracleParameter("p_BeginTime", OracleDbType.Date, oBeginTime, ParameterDirection.Input),
                new OracleParameter("p_EndTime", OracleDbType.Date, oEndTime, ParameterDirection.Input),
                new OracleParameter("p_DataType", dataType),
                p
            });

            List<UserFrame> sinfos = new List<UserFrame>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sinfos.Add(new UserFrame(dr));
                }
            }
            return sinfos;
        }

        /// <summary>
        /// 按ID取数据
        /// </summary>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public UserFrame GetByID(int dataID)
        {
            //get row
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_userframe_getbyid, new OracleParameter[]{
                new OracleParameter("p_id", dataID),
                p
            });
            UserFrame sinfo = null;
            if (ds != null && ds.Tables.Count == 1)
            {
                if (ds.Tables[0].Rows.Count == 1)
                    sinfo = new UserFrame(ds.Tables[0].Rows[0]);
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

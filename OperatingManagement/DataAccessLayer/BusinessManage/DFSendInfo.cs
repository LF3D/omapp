using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    [Serializable]
    public class DFSendInfo : BaseEntity<int, DFSendInfo>
    {
        public DFSendInfo()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        public DFSendInfo(DataRow dr)
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");

            this.Id = Convert.ToInt32(dr["RID"].ToString());
            if (dr["CTime"] == DBNull.Value)
                this.CTime = DateTime.Parse(dr["CTime"].ToString());

            if (dr["TaskID"] == DBNull.Value)
                this.TaskID = string.Empty;
            else
                this.TaskID = dr["TaskID"].ToString();

            if (dr["SatID"] == DBNull.Value)
                this.SatID = string.Empty;
            else
                this.TaskID = dr["SatID"].ToString();

            if (dr["SeqNO"] == DBNull.Value)
                this.SeqNO = 0;
            else
                this.SeqNO = Convert.ToInt32(dr["SeqNO"].ToString());

            if (dr["Source"] == DBNull.Value)
                this.Source = 0;
            else
                this.Source = Convert.ToInt32(dr["Source"].ToString());

            if (dr["Destination"] == DBNull.Value)
                this.Destination = 0;
            else
                this.Destination = Convert.ToInt32(dr["Destination"].ToString());

            if (dr["InfoTypeID"] == DBNull.Value)
                this.InfoTypeID = 0;
            else
                this.InfoTypeID = Convert.ToInt32(dr["InfoTypeID"].ToString());

            if (dr["Status"] == DBNull.Value)
                this.Status = 0;
            else
                this.Status = Convert.ToInt32(dr["Status"].ToString());

            if (dr["Remark"] == DBNull.Value)
                this.Remark = string.Empty;
            else
                this.Remark = dr["Remark"].ToString();
        }

        #region -Properties-
        private OracleDatabase _database = null;
        private string s_up_dfsendinfo_insert = "up_dfsendinfo_insert";
        private string s_up_dfsendinfo_update = "up_dfsendinfo_update";
        private string s_up_dfsendinfo_selectall = "up_dfsendinfo_selectall";
        private string s_up_dfsendinfo_search = "up_dfsendinfo_search";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CTime { get; set; }
        /// <summary>
        /// 任务代号
        /// </summary>
        public string TaskID { get; set; }
        /// <summary>
        /// 卫星代号
        /// </summary>
        public string SatID { get; set; }
        /// <summary>
        /// 信息类别
        /// </summary>
        public int InfoTypeID { get; set; }
        /// <summary>
        /// 信源
        /// </summary>
        public int Source { get; set; }
        /// <summary>
        /// 信宿
        /// </summary>
        public int Destination { get; set; }
        /// <summary>
        /// 数据报顺序号
        /// </summary>
        public int SeqNO { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #endregion

        #region -Methods-

        /// <summary>
        /// Selects all DFSendInfo.
        /// </summary>
        /// <returns>YDSJ</returns>
        public List<DFSendInfo> SelectAll()
        {
            OracleParameter p = PrepareRefCursor();

            DataSet ds = _database.SpExecuteDataSet(s_up_dfsendinfo_selectall, new OracleParameter[]{
                p
            });

            List<DFSendInfo> sinfos = new List<DFSendInfo>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sinfos.Add(new DFSendInfo(dr));
                }
            }
            return sinfos;
        }

        /// <summary>
        /// 按时间段搜索数据帧接收记录
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<DFSendInfo> Search(DateTime beginTime, DateTime endTime)
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


            DataSet ds = _database.SpExecuteDataSet(s_up_dfsendinfo_selectall, new OracleParameter[]{
                new OracleParameter("p_BTime", oBeginTime),
                new OracleParameter("p_ETime", oEndTime),
                p
            });

            List<DFSendInfo> sinfos = new List<DFSendInfo>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sinfos.Add(new DFSendInfo(dr));
                }
            }
            return sinfos;
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
            _database.SpExecuteNonQuery(s_up_dfsendinfo_insert, new OracleParameter[]{
                new OracleParameter("p_CTime", DateTime.Now),
                new OracleParameter("p_TaskID", this.TaskID),
                new OracleParameter("p_SatID", this.SatID),
                new OracleParameter("p_InfoTypeID", this.InfoTypeID),
                new OracleParameter("p_Source", this.Source),
                new OracleParameter("p_Destination", this.Destination),
                new OracleParameter("p_SeqNo", this.SeqNO),
                new OracleParameter("p_Status", this.Status),
                new OracleParameter("p_Remark", this.Remark),
                opId,
                p
            });
            if (opId.Value != null && opId.Value != DBNull.Value)
                this.Id = Convert.ToInt32(opId.Value.ToString());
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }

        public FieldVerifyResult Update()
        {
            OracleParameter p = new OracleParameter()
            {
                ParameterName = "v_result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Double
            };

            _database.SpExecuteNonQuery(s_up_dfsendinfo_update, new OracleParameter[]{
                    new OracleParameter("p_RID", this.Id),
                    new OracleParameter("p_Status", this.Status),
                    p
                });
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

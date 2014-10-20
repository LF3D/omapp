using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using Oracle.DataAccess.Client;
using System.Data;

namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    /// <summary>
    /// 发送记录，既包含文件类型又包含数据包类型
    /// </summary>
    [Serializable]
    public class SendInfo : BaseEntity<int, SendInfo>
    {
        private OracleDatabase _database = null;
        private const string s_up_sendinfo_selectbydatatype = "up_sendinfo_selectbydatatype";
        private const string s_up_sendinfo_selectbyxxtypeandtime = "up_sendinfo_selectbyxxtypeandtime";
        private const string s_up_sendinfo_selectbyid = "up_sendinfo_selectbyid";
        private const string s_up_sendinfo_insert = "up_sendinfo_insert";
        private const string s_up_sendinfo_update = "up_sendinfo_update";

        public SendInfo()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        public SendInfo(DataRow dr)
        {
            this.Id = Convert.ToInt32(dr["RID"].ToString());
            if (dr["FileName"] == DBNull.Value)
                this.FileName = string.Empty;
            else
                this.FileName = dr["FileName"].ToString();

            if (dr["FileCode"] == DBNull.Value)
                this.FileCode = string.Empty;
            else
                this.FileCode = dr["FileCode"].ToString();

            if (dr["FilePath"] == DBNull.Value)
                this.FilePath = string.Empty;
            else
                this.FilePath = dr["FilePath"].ToString();

            if (dr["FileSize"] == DBNull.Value)
                this.FileSize = 0;
            else
                this.FileSize = Convert.ToInt32(dr["FileSize"].ToString());

            if (dr["CurPosition"] == DBNull.Value)
                this.CurPosition = 0;
            else
                this.CurPosition = Convert.ToInt32(dr["CurPosition"].ToString());

            this.RetryTimes = Convert.ToInt32(dr["RetryTimes"].ToString());
            this.SenderID = Convert.ToInt32(dr["SenderID"].ToString());
            this.SenderName = dr["SenderID"].ToString();
            this.ReceiverID = Convert.ToInt32(dr["ReceiverID"].ToString());
            this.ReceiverName = dr["ReceiverID"].ToString();
            this.SendStatus = (SendStatuss)Convert.ToInt32(dr["SendStatus"].ToString());

            if (dr["Remark"] == DBNull.Value)
                this.Remark = string.Empty;
            else
                this.Remark = dr["Remark"].ToString();
            this.SubmitTime = Convert.ToDateTime(dr["UserType"].ToString());

            if (dr["LastUpdateTime"] == DBNull.Value)
                this.LastUpdateTime = DateTime.MinValue;
            else
                this.LastUpdateTime = Convert.ToDateTime(dr["UserCatalog"].ToString());

            this.SendWay = (CommunicationWays)Convert.ToInt32(dr["SendWay"].ToString());
            this.InfoTypeID = Convert.ToInt32(dr["InfoTypeID"].ToString());
            this.InfoTypeName = dr["InfoTypeName"].ToString();
            this.AutoReSend = (IFAutoReSend)Convert.ToInt32(dr["AutoReSend"].ToString());
        }

        #region -Properties-
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件标识
        /// </summary>
        public string FileCode { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件大小，单位字节
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 当前位置
        /// </summary>
        public long CurPosition { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryTimes { get; set; }

        /// <summary>
        /// 发送方标识
        /// </summary>
        public int SenderID { get; set; }

        /// <summary>
        /// 发送方名称
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 接收方标识
        /// </summary>
        public int ReceiverID { get; set; }

        /// 接收方名称
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 发送状态，使用Enum SendStatus
        /// </summary>
        public SendStatuss SendStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 提交发送时间
        /// </summary>
        public DateTime SubmitTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 信息类型标识
        /// </summary>
        public int InfoTypeID { get; set; }

        /// <summary>
        /// 信息类型名称
        /// </summary>
        public string InfoTypeName { get; set; }

        /// <summary>
        /// 发送方式，使用Enum CommunicationWay
        /// </summary>
        public CommunicationWays SendWay { get; set; }

        /// <summary>
        /// 是否自动重发
        /// </summary>
        public IFAutoReSend AutoReSend { get; set; }
        //public 
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

        /// <summary>
        /// Selects all the sendInfos from database by dataType.
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private List<SendInfo> SelectAllSendInfoByType(InfoTypes dataType)
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_sendinfo_selectbydatatype, new OracleParameter[] {
                new OracleParameter("p_DataType", Convert.ToInt32(dataType)),
                p
            });
            List<SendInfo> sinfos = new List<SendInfo>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sinfos.Add(new SendInfo(dr));
                }
            }
            return sinfos;
        }

        /// <summary>
        /// get the SendInfos by XXTYPEid, submittime and dataType.
        /// </summary>
        /// <returns></returns>
        private List<SendInfo> SelectSendInfoByXXTypeTimeandDataType(int xXTypeID, DateTime beginTime, DateTime endTime, InfoTypes dataType)
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

            DataSet ds = _database.SpExecuteDataSet(s_up_sendinfo_selectbyxxtypeandtime, new OracleParameter[]{
                new OracleParameter("p_XXTypeID", OracleDbType.Int32, xXTypeID, ParameterDirection.Input),
                new OracleParameter("p_BeginTime", OracleDbType.Date, oBeginTime, ParameterDirection.Input),
                new OracleParameter("p_EndTime", OracleDbType.Date, oEndTime, ParameterDirection.Input),
                new OracleParameter("p_DataType", OracleDbType.Int32, (int)dataType, ParameterDirection.Input),
                p
            });

            List<SendInfo> sinfos = new List<SendInfo>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sinfos.Add(new SendInfo(dr));
                }
            }
            return sinfos;
        }
        #endregion

        #region -Public methods-

        /// <summary>
        /// Selects all the file SendInfos from database.
        /// </summary>
        /// <returns>List of <see cref="User"/> collection.</returns>
        public List<SendInfo> SelectAllFileSendInfo()
        {
            return SelectAllSendInfoByType(InfoTypes.File);
        }

        /// <summary>
        /// Selects all the dataframe SendInfos from database.
        /// </summary>
        /// <returns>List of <see cref="User"/> collection.</returns>
        public List<SendInfo> SelectAllDFSendInfo()
        {
            return SelectAllSendInfoByType(InfoTypes.DataFrame);
        }
                
        /// <summary>
        /// get the SendInfo by id.
        /// </summary>
        /// <returns></returns>
        public List<SendInfo> SelectSendInfoById()
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_sendinfo_selectbyid, new OracleParameter[]{
                new OracleParameter("p_RID", this.Id),
                p
            });
            List<SendInfo> sinfos = new List<SendInfo>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sinfos.Add(new SendInfo(dr));
                }
            }
            return sinfos;
        }

        /// <summary>
        /// get the File SendInfos by XXTYPE id and submittime.
        /// </summary>
        /// <returns></returns>
        public List<SendInfo> SearchFileSendInfo(int xXTypeID, DateTime beginTime, DateTime endTime)
        {
            return SelectSendInfoByXXTypeTimeandDataType(xXTypeID, beginTime, endTime, InfoTypes.File);
        }

        /// <summary>
        /// get the DataFrame SendInfos by XXTYPE id and submittime.
        /// </summary>
        /// <returns></returns>
        public List<SendInfo> SearchDFSendInfo(int xXTypeID, DateTime beginTime, DateTime endTime)
        {
            return SelectSendInfoByXXTypeTimeandDataType(xXTypeID, beginTime, endTime, InfoTypes.DataFrame);
        }

        /// <summary>
        /// Inserts a new sendinfo into database.
        /// </summary>
        /// <returns></returns>
        public FieldVerifyResult Add()
        {
            #region -prepare for param
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

            object oFileName = null;
            object oFileCode = null;
            object oFilePath = null;
            object oFileSize = null;
            object oCurPostion = null;

            if (this.FileName.Equals(string.Empty))
            {
                oFileName = DBNull.Value;
                oFileCode = DBNull.Value;
                oFilePath = DBNull.Value;
                oFileSize = DBNull.Value;
                oCurPostion = DBNull.Value;
            }
            else
            {
                oFileName = this.FileName;
                oFileCode = this.FileCode;
                oFilePath = this.FilePath;
                oFileSize = this.FileSize;
                oCurPostion = this.CurPosition;
            }
            #endregion

            _database.SpExecuteNonQuery(s_up_sendinfo_insert, new OracleParameter[]{
                new OracleParameter("p_RID",this.Id),
                new OracleParameter("p_FileName", oFileName),
                new OracleParameter("p_FileCode", oFileCode),
                new OracleParameter("p_FilePath", oFilePath),
                new OracleParameter("p_FileSize", oFileSize),
                new OracleParameter("p_CurPosition", oCurPostion),
                new OracleParameter("p_RetryTimes", this.RetryTimes),
                new OracleParameter("p_Sender", this.SenderID),
                new OracleParameter("p_Receiver", this.ReceiverID),
                new OracleParameter("p_SendStatus", (int)this.SendStatus),
                new OracleParameter("p_Remark", DBNull.Value),
                new OracleParameter("p_SubmitTime", OracleDbType.Date, this.SubmitTime, ParameterDirection.Input),
                new OracleParameter("p_LastUpdateTime", DBNull.Value),
                new OracleParameter("p_InfoType", this.InfoTypeID),
                new OracleParameter("p_SendWay", (int)this.SendWay),
                new OracleParameter("p_AutoReSend", (int)this.AutoReSend),
                opId,
                p
            });

            if (opId.Value != null && opId.Value != DBNull.Value)
                this.Id = Convert.ToInt32(opId.Value);

            return (FieldVerifyResult)Convert.ToInt32(p.Value);
        }

        /// <summary>
        /// Updates the sendinfo object in database.
        /// only curPosition、LastUpdateTime、SendStatus、RetryTimes、Remark
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
            object oLastUpdateTime = null;
            if (this.LastUpdateTime == DateTime.MinValue)
                oLastUpdateTime = DBNull.Value;
            else
                oLastUpdateTime = this.LastUpdateTime;

            _database.SpExecuteNonQuery(s_up_sendinfo_update, new OracleParameter[]{
                new OracleParameter("p_RID", this.Id),
                new OracleParameter("p_CurPosition", this.CurPosition),
                new OracleParameter("p_SendStatus", (int)this.SendStatus),
                new OracleParameter("p_RetryTimes", this.RetryTimes),
                new OracleParameter("p_Remark", this.Remark),
                new OracleParameter("p_LastUpdateTime", oLastUpdateTime),
                p
            });
            return (FieldVerifyResult)Convert.ToInt32(p.Value);
        }
        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
        }
        #endregion
    }
}
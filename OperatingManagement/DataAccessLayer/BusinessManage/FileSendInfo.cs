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
    public class FileSendInfo : BaseEntity<int, FileSendInfo>
    {
        private OracleDatabase _database = null;
        private const string s_up_fsendinfo_selectall = "up_fsendinfo_selectall";
        private const string s_up_fsendinfo_search = "up_fsendinfo_search";
        private const string s_up_fsendinfo_selectbyid = "up_fsendinfo_selectbyid";
        private const string s_up_fsendinfo_selectbystatus = "up_fsendinfo_selectbystatus";
        private const string s_up_fsendinfo_selectbyfileinfo = "up_fsendinfo_selectbyfileinfo";
        private const string s_up_fsendinfo_insert = "up_fsendinfo_insert";
        private const string s_up_fsendinfo_update = "up_fsendinfo_update";
        private string strFileFullName = "";
        private string strFilePath = "";

        public FileSendInfo()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        public FileSendInfo(DataRow dr)
        {
            this.Id = Convert.ToInt32(dr["RID"].ToString());
            if (dr["FileName"] == DBNull.Value)
                this.FileName = string.Empty;
            else
                this.FileName = dr["FileName"].ToString();

            if (dr["FileCode"] == DBNull.Value)
                this.FileCode = 0;
            else
                this.FileCode = Convert.ToInt32(dr["FileCode"].ToString());

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
            this.SenderName = dr["SenderName"].ToString();
            this.ReceiverID = Convert.ToInt32(dr["ReceiverID"].ToString());
            this.ReceiverName = dr["ReceiverName"].ToString();
            this.SendStatus = (SendStatuss)Convert.ToInt32(dr["SendStatus"].ToString());

            if (dr["Remark"] == DBNull.Value)
                this.Remark = string.Empty;
            else
                this.Remark = dr["Remark"].ToString();
            this.SubmitTime = Convert.ToDateTime(dr["SubmitTime"].ToString());

            if (dr["LastUpdateTime"] == DBNull.Value)
                this.LastUpdateTime = DateTime.MinValue;
            else
                this.LastUpdateTime = Convert.ToDateTime(dr["LastUpdateTime"].ToString());

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
        public int FileCode { get; set; }

        /// <summary>
        /// 文件路径，以"\"结束
        /// </summary>
        public string FilePath
        {
            get
            {
                return strFilePath;
            }
            set
            {
                strFilePath = value;
                if (!strFilePath.EndsWith(@"\"))
                {
                    strFilePath += @"\";
                }
            }
        }

        /// <summary>
        /// 文件大小，单位字节
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// 当前位置
        /// </summary>
        public int CurPosition { get; set; }

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
        /// 数据包类型，非数据表结构，仅供文件服务器使用
        /// </summary>
        public int PackageType { get; set; }

        /// <summary>
        /// 数据包的发送状态，非数据表结构，仅供文件服务器使用
        /// </summary>
        public int PackageSendStatus { get; set; }

        /// <summary>
        /// 文件全名，即文件路径+文件名
        /// </summary>
        public string FileFullName
        {
            get
            {
                if (strFileFullName.Equals(String.Empty))
                    return this.FilePath + this.FileName;
                else
                    return strFileFullName;
            }
            set
            {
                strFileFullName = value;
            }
        }
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
        /// get the SendInfos by XXTYPEid, submittime and dataType.
        /// </summary>
        /// <returns></returns>
        private List<FileSendInfo> SelectSendInfoByXXTypeTimeandDataType(int xXTypeID, DateTime beginTime, DateTime endTime)
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

            DataSet ds = _database.SpExecuteDataSet(s_up_fsendinfo_search, new OracleParameter[]{
                new OracleParameter("p_XXTypeID", OracleDbType.Int32, xXTypeID, ParameterDirection.Input),
                new OracleParameter("p_BeginTime", OracleDbType.Date, oBeginTime, ParameterDirection.Input),
                new OracleParameter("p_EndTime", OracleDbType.Date, oEndTime, ParameterDirection.Input),
                p
            });

            List<FileSendInfo> sinfos = new List<FileSendInfo>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sinfos.Add(new FileSendInfo(dr));
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
        public List<FileSendInfo> SelectAll()
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_fsendinfo_selectall, new OracleParameter[] {
                p
            });
            List<FileSendInfo> sinfos = new List<FileSendInfo>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sinfos.Add(new FileSendInfo(dr));
                }
            }
            return sinfos;
        }

        /// <summary>
        /// get the SendInfo by id.
        /// </summary>
        /// <returns></returns>
        public FileSendInfo SelectById()
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_fsendinfo_selectbyid, new OracleParameter[]{
                new OracleParameter("p_RID", this.Id),
                p
            });
            FileSendInfo sinfo = null;
            if (ds != null && ds.Tables.Count == 1)
            {
                if (ds.Tables[0].Rows.Count == 1)
                    sinfo = new FileSendInfo(ds.Tables[0].Rows[0]);
            }
            return sinfo;
        }

        /// <summary>
        /// get the SendInfo by status.
        /// </summary>
        /// <returns></returns>
        public List<FileSendInfo> SelectByStatus(int onlyFailed, int retryTimes)
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_fsendinfo_selectbystatus, new OracleParameter[]{
                new OracleParameter("p_onlyfailed", onlyFailed),
                new OracleParameter("p_retrytimes", retryTimes),
                p
            });
            List<FileSendInfo> sinfos = new List<FileSendInfo>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sinfos.Add(new FileSendInfo(dr));
                }
            }
            return sinfos;
        }

        /// <summary>
        /// 按文件名获取
        /// </summary>
        /// <returns></returns>
        public FileSendInfo SelectByFileInfo()
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet(s_up_fsendinfo_selectbyfileinfo, new OracleParameter[]{
                new OracleParameter("p_FileName", this.FileName),
                new OracleParameter("p_FileCode", this.FileCode),
                new OracleParameter("p_ReceiverId", this.ReceiverID),
                p
            });
            FileSendInfo sinfo = null;
            if (ds != null && ds.Tables.Count == 1)
            {
                if (ds.Tables[0].Rows.Count == 1)
                    sinfo = new FileSendInfo(ds.Tables[0].Rows[0]);
            }
            return sinfo;
        }

        /// <summary>
        /// get the File SendInfos by XXTYPE id and submittime.
        /// </summary>
        /// <returns></returns>
        public List<FileSendInfo> Search(DateTime beginTime, DateTime endTime)
        {
            return SelectSendInfoByXXTypeTimeandDataType(this.InfoTypeID, beginTime, endTime);
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
                ParameterName = "v_rid",
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

            _database.SpExecuteNonQuery(s_up_fsendinfo_insert, new OracleParameter[]{
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
                this.Id = Convert.ToInt32(opId.Value.ToString());

            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
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

            _database.SpExecuteNonQuery(s_up_fsendinfo_update, new OracleParameter[]{
                new OracleParameter("p_RID", this.Id),
                new OracleParameter("p_CurPosition", this.CurPosition),
                new OracleParameter("p_SendStatus", (int)this.SendStatus),
                new OracleParameter("p_RetryTimes", this.RetryTimes),
                new OracleParameter("p_Remark", this.Remark),
                new OracleParameter("p_LastUpdateTime", DateTime.Now),
                p
            });
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }

        /// <summary>
        /// Return whether has same filename in database
        /// </summary>
        /// <returns></returns>
        public bool HasSameFileName()
        {
            if (SelectByFileInfo() == null)
                return false;
            else
                return true;
        }
        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
        }
        #endregion
    }
}
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
    public class Task : BaseEntity<int, Task>
    {
        public Task()
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region Properties
        private OracleDatabase _dataBase = null;
        private const string s_up_task_selectall = "up_task_selectall";
        private const string s_up_task_selectbyid = "up_task_selectbyid";
        private const string s_up_task_insert = "up_task_insert";
        private const string s_up_task_update = "up_task_update";

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }
        ///// <summary>
        ///// 内部任务代号
        ///// </summary>
        public string TaskNo { get; set; }
        ///// <summary>
        ///// 外部任务代号
        ///// </summary>
        public string OutTaskNo { get; set; }
        /// <summary>
        /// 对象标识
        /// </summary>
        public string ObjectFlag { get; set; }
        /// <summary>
        /// 航天器标识
        /// </summary>
        public string SCID { get; set; }
        /// <summary>
        /// 卫星编号，可能多个，逗号,分隔
        /// </summary>
        public string SatID { get; set; }
        /// <summary>
        /// 是否有效，1是，0否
        /// </summary>
        public string IsEffective { get; set; }
        /// <summary>
        /// emit时间
        /// </summary>
        public DateTime EmitTime { get; set; }
        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime CTime { get; set; }

        public static List<Task> _taskCache = null;
        public List<Task> Cache
        {
            get
            {
                if (_taskCache == null)
                {
                    _taskCache = SelectAll();
                }
                return _taskCache;
            }
            set 
            {
                _taskCache = value;
            }
        }
        #endregion

        #region -Private Methods-
        private OracleParameter PrepareRefCursor()
        {
            return new OracleParameter()
            {
                ParameterName = "o_Cursor",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.RefCursor
            };
        }

        private OracleParameter PrepareOutputResult()
        {
            return new OracleParameter()
            {
                ParameterName = "v_Result",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Int32,
            };
        }
        #endregion

        #region -Public Methods


        /// <summary>
        /// 获得所有任务信息实体列表
        /// </summary>
        /// <returns>任务信息实体列表</returns>
        public List<Task> SelectAll()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet(s_up_task_selectall, new OracleParameter[] { o_Cursor });

            List<Task> infoList = new List<Task>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Task info = new Task()
                    {
                        Id = Convert.ToInt32(dr["id"].ToString()),
                        TaskName = dr["TaskName"].ToString(),
                        TaskNo = dr["TaskNo"].ToString(),
                        OutTaskNo = dr["OutTaskNo"].ToString(),
                        ObjectFlag = dr["ObjectFlag"].ToString(),
                        SatID = dr["SatID"].ToString(),
                        SCID = dr["SCID"].ToString(),
                        IsEffective = dr["IsEffective"].ToString(),
                        EmitTime = dr["EmitTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["EmitTime"]),
                        CTime = dr["CTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["CTime"])
                    };

                    infoList.Add(info);
                }
            }
            return infoList;
        }

        /// <summary>
        /// Select Task by id.
        /// </summary>
        /// <returns></returns>
        public Task SelectById()
        {
            OracleParameter p = PrepareRefCursor();

            DataSet ds = _dataBase.SpExecuteDataSet(s_up_task_selectbyid, new OracleParameter[]{
                new OracleParameter("p_Id", this.Id), 
                p
            });

            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return new Task()
                    {
                        Id = Convert.ToInt32(dr["id"].ToString()),
                        TaskName = dr["TaskName"].ToString(),
                        TaskNo = dr["TaskNo"].ToString(),
                        OutTaskNo = dr["OutTaskNo"].ToString(),
                        ObjectFlag = dr["ObjectFlag"].ToString(),
                        SatID = dr["SatID"].ToString(),
                        SCID = dr["SCID"].ToString(),
                        IsEffective = dr["IsEffective"].ToString(),
                        EmitTime = dr["EmitTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["EmitTime"]),
                        CTime = dr["CTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["CTime"])
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// Insert a Task.
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
            _dataBase.SpExecuteNonQuery(s_up_task_insert, new OracleParameter[]{
                new OracleParameter("p_TaskName",this.TaskName),
                new OracleParameter("p_TaskNo",this.TaskNo),
                new OracleParameter("p_OutTaskNo",this.OutTaskNo),
                new OracleParameter("p_ObjectFlag",this.ObjectFlag),
                new OracleParameter("p_SatID",this.SatID),
                new OracleParameter("p_SCID",this.SCID),
                new OracleParameter("IsEffective",this.IsEffective),
                new OracleParameter("EmitTime",this.EmitTime.ToString("yyyy/MM/dd HH:mm:ss fff")),
                new OracleParameter("p_CTime",DateTime.Now),
                opId,
                p
            });
            if (opId.Value != null && opId.Value != DBNull.Value)
                this.Id = Convert.ToInt32(opId.Value.ToString());
            RefreshCache();
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
            _dataBase.SpExecuteNonQuery(s_up_task_update, new OracleParameter[]{
                new OracleParameter("p_Id", this.Id),
                new OracleParameter("p_TaskName", this.TaskName),
                new OracleParameter("p_TaskNo", this.TaskNo),
                new OracleParameter("p_OutTaskNo",this.OutTaskNo),
                new OracleParameter("p_ObjectFlag", this.ObjectFlag),
                new OracleParameter("p_SatID", this.SatID),
                new OracleParameter("p_SCID",this.SCID),
                new OracleParameter("IsEffective",this.IsEffective),
                new OracleParameter("EmitTime",this.EmitTime.ToString("yyyy/MM/dd HH:mm:ss fff")),
                p
            });
            RefreshCache();
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }

        /// <summary>
        /// 通过任务代号获取对象标识
        /// </summary>
        /// <param name="taskNo"></param>
        /// <returns></returns>
        public string GetObjectFlagByTaskNo(string taskNo, string satID)
        {
            string strResult = string.Empty;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.TaskNo == taskNo && a.SatID == satID);
                if (query != null && query.Count() > 0)
                    strResult = query.FirstOrDefault().ObjectFlag;
            }
            return strResult;
        }

        /// <summary>
        /// 通过对象标识获取任务代号
        /// </summary>
        /// <param name="taskNo"></param>
        /// <returns></returns>
        public string GetTaskNoByObjectFlag(string objectFlag)
        {
            string strResult = string.Empty;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.ObjectFlag == objectFlag);
                if (query != null && query.Count() > 0)
                    strResult = query.FirstOrDefault().TaskNo;
            }
            return strResult;
        }

        /// <summary>
        /// 通过内部任务代号和卫星代号获取外部任务代号
        /// </summary>
        /// <param name="taskNo"></param>
        /// <param name="satid"></param>
        /// <returns></returns>
        public string GetOutTaskNo(string taskNo, string satid)
        {
            string strResult = string.Empty;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.TaskNo == taskNo && a.SatID == satid);
                if (query != null && query.Count() > 0)
                    strResult = query.FirstOrDefault().OutTaskNo;
            }
            return strResult;
        }

        /// <summary>
        /// 通过外部任务代号获取内部任务代号及卫星代号
        /// </summary>
        /// <param name="outTaskNo"></param>
        /// <param name="taskNo"></param>
        /// <param name="satid"></param>
        public void GetTaskNoSatID(string outTaskNo, out string taskNo, out string satid)
        {
            taskNo = string.Empty;
            satid = string.Empty;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.OutTaskNo == outTaskNo);
                if (query != null && query.Count() > 0)
                {
                    taskNo = query.FirstOrDefault().TaskNo;
                    satid = query.FirstOrDefault().SatID;
                }
            }
        }

        /// <summary>
        /// 通过外部任务代号获取航天器标识
        /// </summary>
        /// <param name="outTaskNo"></param>
        /// <param name="taskNo"></param>
        /// <param name="satid"></param>
        public string GetSCID(string outTaskNo)
        {
            if (Cache != null)
            {
                var query = Cache.Where(a => a.OutTaskNo == outTaskNo);
                if (query != null && query.Count() > 0)
                {
                    return query.FirstOrDefault().SCID;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过任务代号获取任务名称
        /// </summary>
        /// <param name="taskNO"></param>
        /// <returns></returns>
        public string GetTaskName(string taskNO, string satID)
        {
            string taskName = string.Empty;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.TaskNo == taskNO && a.SatID == satID);
                if (query != null && query.Count() > 0)
                    taskName = query.FirstOrDefault().TaskName;
            }
            return taskName;
        }

        private void RefreshCache()
        {
            this.Cache = SelectAll();
        }
        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
        }
        #endregion
    }
}

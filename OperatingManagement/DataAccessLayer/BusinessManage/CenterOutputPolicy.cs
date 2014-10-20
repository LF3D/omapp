#region
//------------------------------------------------------
//Assembly:OperatingManagement.DataAccessLayer
//FileName:CenterOutputPolicy.cs
//Remark:中心输出策略管理类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20111011    Create     
//------------------------------------------------------
#endregion
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
    public class CenterOutputPolicy : BaseEntity<int, CenterOutputPolicy>
    {
        public CenterOutputPolicy()
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region Properties
        private OracleDatabase _dataBase = null;

        /// <summary>
        /// 任务代号
        /// </summary>
        public string TaskID { get; set; }
        /// <summary>
        /// 卫星名称
        /// </summary>
        public string SatName { get; set; }
        /// <summary>
        /// 信源
        /// </summary>
        public int InfoSource { get; set; }
        /// <summary>
        /// 信息类别
        /// </summary>
        public int InfoType { get; set; }
        /// <summary>
        /// 信宿
        /// </summary>
        public int Destination { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime EffectTime { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime DefectTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 创建用户ID
        /// </summary>
        public double CreatedUserID { get; set; }
        /// <summary>
        /// 最后修改用户ID
        /// </summary>
        public double UpdatedUserID { get; set; }
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

        #region -Public Method-

        /// <summary>
        /// 根据ID获得中心输出策略
        /// </summary>
        /// <returns>中心输出策略实体</returns>
        public CenterOutputPolicy SelectByID()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_COP_SelectByID", new OracleParameter[] { new OracleParameter("p_COPID", Id), o_Cursor });

            CenterOutputPolicy info = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                info = new CenterOutputPolicy()
                {
                    Id = Convert.ToInt32(ds.Tables[0].Rows[0]["COPID"]),
                    TaskID = ds.Tables[0].Rows[0]["TaskID"].ToString(),
                    SatName = ds.Tables[0].Rows[0]["SatName"].ToString(),
                    InfoSource = Convert.ToInt32(ds.Tables[0].Rows[0]["InfoSource"]),
                    InfoType = Convert.ToInt32(ds.Tables[0].Rows[0]["InfoType"]),
                    Destination = Convert.ToInt32(ds.Tables[0].Rows[0]["Ddestination"]),
                    EffectTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["EffectTime"]),
                    DefectTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["DefectTime"]),
                    Note = ds.Tables[0].Rows[0]["Note"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["Note"].ToString(),
                    CreatedTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedTime"]),
                    CreatedUserID = ds.Tables[0].Rows[0]["CreatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(ds.Tables[0].Rows[0]["CreatedUserID"]),
                    UpdatedTime = ds.Tables[0].Rows[0]["UpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ds.Tables[0].Rows[0]["UpdatedTime"]),
                    UpdatedUserID = ds.Tables[0].Rows[0]["UpdatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(ds.Tables[0].Rows[0]["UpdatedUserID"])
                };
            }
            return info;
        }

        /// <summary>
        /// 获得中心输出策略列表
        /// </summary>
        /// <returns>中心输出策略实体列表</returns>
        public List<CenterOutputPolicy> SelectAll()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_COP_SelectAll", new OracleParameter[] { o_Cursor });

            List<CenterOutputPolicy> infoList = new List<CenterOutputPolicy>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CenterOutputPolicy info = new CenterOutputPolicy()
                    {
                        Id = Convert.ToInt32(dr["COPID"]),
                        TaskID = dr["TaskID"].ToString(),
                        SatName = dr["SatName"].ToString(),
                        InfoSource = Convert.ToInt32(dr["InfoSource"]),
                        InfoType = Convert.ToInt32(dr["InfoType"]),
                        Destination = Convert.ToInt32(dr["Ddestination"]),
                        EffectTime = Convert.ToDateTime(dr["EffectTime"]),
                        DefectTime = Convert.ToDateTime(dr["DefectTime"]),
                        Note = dr["Note"] == DBNull.Value ? string.Empty : dr["Note"].ToString(),
                        CreatedTime = Convert.ToDateTime(dr["CreatedTime"]),
                        CreatedUserID = dr["CreatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["CreatedUserID"]),
                        UpdatedTime = dr["UpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["UpdatedTime"]),
                        UpdatedUserID = dr["UpdatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["UpdatedUserID"])
                    };

                    infoList.Add(info);
                }
            }
            return infoList;
        }

        /// <summary>
        /// 添加中心输出策略记录
        /// </summary>
        /// <returns>添加结果</returns>
        public FieldVerifyResult Add()
        {
            OracleParameter v_Result = PrepareOutputResult();
            OracleParameter v_ID = new OracleParameter()
            {
                ParameterName = "v_COPID",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Int32
            };

            _dataBase.SpExecuteNonQuery("UP_COP_Insert", new OracleParameter[]{
                                        new OracleParameter("p_TaskID",TaskID),
                                        new OracleParameter("p_SatName",SatName),
                                        new OracleParameter("p_InfoSource",InfoSource),
                                        new OracleParameter("p_InfoType",InfoType),
                                        new OracleParameter("p_Ddestination",Destination),
                                        new OracleParameter("p_EffectTime",EffectTime),
                                        new OracleParameter("p_DefectTime",DefectTime),
                                        new OracleParameter("p_Note",string.IsNullOrEmpty(Note) ? DBNull.Value as object : Note),
                                        new OracleParameter("p_CreatedTime",CreatedTime),
                                        new OracleParameter("p_CreatedUserID",CreatedUserID == 0.0 ? DBNull.Value as object : CreatedUserID),
                                        new OracleParameter("p_UpdatedTime",UpdatedTime == DateTime.MinValue ? DBNull.Value as object : UpdatedTime),
                                        new OracleParameter("p_UpdatedUserID",UpdatedUserID == 0.0 ? DBNull.Value as object : UpdatedUserID),
                                        v_ID,
                                        v_Result});
            if (v_ID.Value != null && v_ID.Value != DBNull.Value)
                this.Id = Convert.ToInt32(v_ID.Value);
            return (FieldVerifyResult)Convert.ToInt32(v_Result.Value);
        }

        /// <summary>
        /// 根据ID更新中心输出策略记录
        /// </summary>
        /// <returns>更新结果</returns>
        public FieldVerifyResult Update()
        {
            OracleParameter v_Result = PrepareOutputResult();

            _dataBase.SpExecuteNonQuery("UP_COP_Update", new OracleParameter[]{
                                        new OracleParameter("p_COPID",Id),
                                        new OracleParameter("p_TaskID",TaskID),
                                        new OracleParameter("p_SatName",SatName),
                                        new OracleParameter("p_InfoSource",InfoSource),
                                        new OracleParameter("p_InfoType",InfoType),
                                        new OracleParameter("p_Ddestination",Destination),
                                        new OracleParameter("p_EffectTime",EffectTime),
                                        new OracleParameter("p_DefectTime",DefectTime),
                                        new OracleParameter("p_Note", string.IsNullOrEmpty(Note) ? DBNull.Value as object : Note),
                                        new OracleParameter("p_CreatedTime",CreatedTime),
                                        new OracleParameter("p_CreatedUserID",CreatedUserID == 0.0 ? DBNull.Value as object : CreatedUserID),
                                        new OracleParameter("p_UpdatedTime",UpdatedTime == DateTime.MinValue ? DBNull.Value as object : UpdatedTime),
                                        new OracleParameter("p_UpdatedUserID",UpdatedUserID == 0.0 ? DBNull.Value as object : UpdatedUserID),
                                        v_Result});
            return (FieldVerifyResult)Convert.ToInt32(v_Result.Value);
        }
        /// <summary>
        /// 校验是否存在某个时间段中心输出策略
        /// </summary>
        /// <returns>true:存在</returns>
        public bool HaveEffectivePolicy()
        {
            List<CenterOutputPolicy> infoList = SelectAll();
            var query = infoList.Where(a =>a.Id != Id && a.TaskID.ToLower() == TaskID.ToLower() && a.SatName.ToLower() == SatName.ToLower() && 
                a.InfoType == InfoType && a.InfoSource == InfoSource && a.Destination == Destination &&
                ((a.EffectTime <= EffectTime && EffectTime <= a.DefectTime) 
                  || (a.EffectTime <= DefectTime && DefectTime <= a.DefectTime) 
                  || (EffectTime <= a.EffectTime && DefectTime >= a.DefectTime)));
            if (query != null && query.Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 根据任务代号和卫星名称查询中心输出策略
        /// </summary>
        /// <returns></returns>
        public List<CenterOutputPolicy> SelectByParameters()
        {
            List<CenterOutputPolicy> infoList = SelectAll();
            var query = infoList.Where(a => (string.IsNullOrEmpty(TaskID) || a.TaskID.ToLower() == TaskID.ToLower()) && 
                (string.IsNullOrEmpty(SatName) || a.SatName.ToLower() == SatName.ToLower())).OrderByDescending(a=> a.CreatedTime);
            return query.ToList<CenterOutputPolicy>();
        }
        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
            this.AddValidRules("TaskID", "任务代号不能为空。", string.IsNullOrEmpty(TaskID));
            this.AddValidRules("SatName", "卫星名称不能为空。", string.IsNullOrEmpty(SatName));
            this.AddValidRules("InfoSource", "信源不能为空。", InfoSource < 1);
            this.AddValidRules("InfoType", "信息类别不能为空。", InfoType < 1);
            this.AddValidRules("Ddestination", "信宿不能为空。", Destination < 1);
        }
        #endregion
    }
}

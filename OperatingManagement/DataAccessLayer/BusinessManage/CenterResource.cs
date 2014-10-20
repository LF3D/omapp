#region
//------------------------------------------------------
//Assembly:OperatingManagement.DataAccessLayer
//FileName:CenterResource.cs
//Remark:中心资源管理类
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
    public class CenterResource : BaseEntity<int, CenterResource>
    {
        public CenterResource()
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region Properties
        private OracleDatabase _dataBase = null;
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipmentType { get; set; }
        /// <summary>
        /// 支持的任务
        /// </summary>
        public string SupportTask { get; set; }
        /// <summary>
        /// 最大数据处理量
        /// </summary>
        public double DataProcess { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ExtProperties { get; set; }
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
        /// 根据ID获得中心资源实体
        /// </summary>
        /// <returns>中心资源实体</returns>
        public CenterResource SelectByID()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_CenterRes_SelectByID", new OracleParameter[] { new OracleParameter("p_CRID", Id), o_Cursor });

            CenterResource info = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                info = new CenterResource()
                {
                    Id = Convert.ToInt32(ds.Tables[0].Rows[0]["CRID"]),
                    EquipmentCode = ds.Tables[0].Rows[0]["EquipmentCode"].ToString(),
                    EquipmentType = ds.Tables[0].Rows[0]["EquipmentType"].ToString(),
                    SupportTask = ds.Tables[0].Rows[0]["SupportTask"].ToString(),
                    DataProcess = Convert.ToDouble(ds.Tables[0].Rows[0]["DataProcess"]),
                    Status = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"]),
                    ExtProperties = ds.Tables[0].Rows[0]["ExtProperties"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["ExtProperties"].ToString(),
                    CreatedTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedTime"]),
                    CreatedUserID = ds.Tables[0].Rows[0]["CreatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(ds.Tables[0].Rows[0]["CreatedUserID"]),
                    UpdatedTime = ds.Tables[0].Rows[0]["UpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ds.Tables[0].Rows[0]["UpdatedTime"]),
                    UpdatedUserID = ds.Tables[0].Rows[0]["UpdatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(ds.Tables[0].Rows[0]["UpdatedUserID"])
                };
            }
            return info;
        }

        /// <summary>
        /// 获得所有中心资源实体列表
        /// </summary>
        /// <returns>中心资源实体列表</returns>
        public List<CenterResource> SelectAll()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_CenterRes_SelectAll", new OracleParameter[] { o_Cursor });

            List<CenterResource> infoList = new List<CenterResource>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CenterResource info = new CenterResource()
                    {
                        Id = Convert.ToInt32(dr["CRID"]),
                        EquipmentCode = dr["EquipmentCode"].ToString(),
                        EquipmentType = dr["EquipmentType"].ToString(),
                        SupportTask = dr["SupportTask"].ToString(),
                        DataProcess = Convert.ToDouble(dr["DataProcess"]),
                        Status = Convert.ToInt32(dr["Status"]),
                        ExtProperties = dr["ExtProperties"] == DBNull.Value ? string.Empty : dr["ExtProperties"].ToString(),
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
        /// 根据Code获得中心资源实体
        /// </summary>
        /// <returns>中心资源实体</returns>
        public CenterResource SelectByCode()
        {
            return SelectAll().Where(a => a.Status == 1 && a.EquipmentCode.ToLower() == EquipmentCode.ToLower()).FirstOrDefault<CenterResource>();
        }

        /// <summary>
        /// 校验该设备编号是否已经存在
        /// </summary>
        /// <returns>true:已经存在</returns>
        public bool HaveActiveEquipmentCode()
        {
            List<CenterResource> infoList = SelectAll();
            var query = infoList.Where(a => a.Id != Id && a.Status == 1 && a.EquipmentCode.ToLower() == EquipmentCode.ToLower());
            if (query != null && query.Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 根据中心资源在某个时间点状态做查询
        /// </summary>
        /// <param name="status">全部:"";正常:01;异常:02;占用中:03;已删除:04</param>
        /// <param name="timePoint">中心资源在某个时间点状态</param>
        /// <returns>中心资源实体列表</returns>
        public List<CenterResource> Search(string status, DateTime timePoint)
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_CenterRes_Search", new OracleParameter[] {  
                new OracleParameter("p_Status", status),
                new OracleParameter("p_TimePoint", timePoint),
                o_Cursor });

            List<CenterResource> infoList = new List<CenterResource>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CenterResource info = new CenterResource()
                    {
                        Id = Convert.ToInt32(dr["CRID"]),
                        EquipmentCode = dr["EquipmentCode"].ToString(),
                        EquipmentType = dr["EquipmentType"].ToString(),
                        SupportTask = dr["SupportTask"].ToString(),
                        DataProcess = Convert.ToDouble(dr["DataProcess"]),
                        Status = Convert.ToInt32(dr["Status"]),
                        ExtProperties = dr["ExtProperties"] == DBNull.Value ? string.Empty : dr["ExtProperties"].ToString(),
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
        /// 根据中心资源在某个时间段状态做查询
        /// </summary>
        /// <param name="status">全部:"";正常:01;异常:02;占用中:03;已删除:04</param>
        /// <param name="beginTime">中心资源状态开始时间</param>
        /// <param name="endTime">中心资源状态结束时间</param>
        /// <returns>中心资源实体列表</returns>
        public List<CenterResource> Search(string status, DateTime beginTime, DateTime endTime)
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_CenterRes_SearchByPhase", new OracleParameter[] {  
                new OracleParameter("p_Status", status),
                new OracleParameter("p_BeginTime", beginTime),
                new OracleParameter("p_EndTime", endTime),
                o_Cursor });

            List<CenterResource> infoList = new List<CenterResource>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CenterResource info = new CenterResource()
                    {
                        Id = Convert.ToInt32(dr["CRID"]),
                        EquipmentCode = dr["EquipmentCode"].ToString(),
                        EquipmentType = dr["EquipmentType"].ToString(),
                        SupportTask = dr["SupportTask"].ToString(),
                        DataProcess = Convert.ToDouble(dr["DataProcess"]),
                        Status = Convert.ToInt32(dr["Status"]),
                        ExtProperties = dr["ExtProperties"] == DBNull.Value ? string.Empty : dr["ExtProperties"].ToString(),
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
        /// 添加中心资源记录
        /// </summary>
        /// <returns>添加结果</returns>
        public FieldVerifyResult Add()
        {
            OracleParameter v_Result = PrepareOutputResult();
            OracleParameter v_ID = new OracleParameter()
            {
                ParameterName = "v_CRID",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Int32
            };

            _dataBase.SpExecuteNonQuery("UP_CenterRes_Insert", new OracleParameter[]{
                                        new OracleParameter("p_EquipmentCode",EquipmentCode),
                                        new OracleParameter("p_EquipmentType",EquipmentType),
                                        new OracleParameter("p_SupportTask",SupportTask),
                                        new OracleParameter("p_DataProcess",DataProcess),
                                        new OracleParameter("p_Status",Status),
                                        new OracleParameter("p_ExtProperties",string.IsNullOrEmpty(ExtProperties) ? DBNull.Value as object : ExtProperties),
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
        /// 根据ID更新中心资源记录
        /// </summary>
        /// <returns>更新结果</returns>
        public FieldVerifyResult Update()
        {
            OracleParameter v_Result = PrepareOutputResult();

            _dataBase.SpExecuteNonQuery("UP_CenterRes_Update", new OracleParameter[]{
                                        new OracleParameter("p_CRID",Id),
                                        new OracleParameter("p_EquipmentCode",EquipmentCode),
                                        new OracleParameter("p_EquipmentType",EquipmentType),
                                        new OracleParameter("p_SupportTask",SupportTask),
                                        new OracleParameter("p_DataProcess",DataProcess),
                                        new OracleParameter("p_Status",Status),
                                        new OracleParameter("p_ExtProperties",string.IsNullOrEmpty(ExtProperties) ? DBNull.Value as object : ExtProperties),
                                        new OracleParameter("p_CreatedTime",CreatedTime),
                                        new OracleParameter("p_CreatedUserID",CreatedUserID == 0.0 ? DBNull.Value as object : CreatedUserID),
                                        new OracleParameter("p_UpdatedTime",UpdatedTime == DateTime.MinValue ? DBNull.Value as object : UpdatedTime),
                                        new OracleParameter("p_UpdatedUserID",UpdatedUserID == 0.0 ? DBNull.Value as object : UpdatedUserID),
                                        v_Result});
            return (FieldVerifyResult)Convert.ToInt32(v_Result.Value);
        }

        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
            this.AddValidRules("EquipmentCode", "设备编号不能为空。", string.IsNullOrEmpty(EquipmentCode));
            this.AddValidRules("EquipmentType", "设备类型不能为空。", string.IsNullOrEmpty(EquipmentType));
            this.AddValidRules("SupportTask", "支持的任务不能为空。", string.IsNullOrEmpty(SupportTask));
            //this.AddValidRules("DataProcess", "最大数据处理量不能为空。", string.IsNullOrEmpty(DataProcess));
        }
        #endregion
    }
}

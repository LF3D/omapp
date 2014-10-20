#region
//------------------------------------------------------
//Assembly:OperatingManagement.DataAccessLayer
//FileName:HealthStatus.cs
//Remark:资源健康状态管理类
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
    public class HealthStatus : BaseEntity<int, HealthStatus>
    {
        public HealthStatus()
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region Properties
        private OracleDatabase _dataBase = null;
        /// <summary>
        /// 资源序号
        /// </summary>
        public int ResourceID { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public int ResourceType { get; set; }
        /// <summary>
        /// 功能类型
        /// 可空
        /// </summary>
        public string FunctionType { get; set; }
        /// <summary>
        /// 健康状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 起始时间
        /// 可空
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// 可空
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 创建用户ID
        /// </summary>
        public double CreatedUserID { get; set; }
        /// <summary>
        /// 最后修改用户ID
        /// </summary>
        public double UpdatedUserID { get; set; }

        /// <summary>
        /// 资源扩展属性，资源名称
        /// </summary>
        public string ResourceName { get; set; }
        /// <summary>
        /// 资源扩展属性，资源编码
        /// </summary>
        public string ResourceCode { get; set; }
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
        /// 根据ID获得资源健康状态实体
        /// </summary>
        /// <returns>资源健康状态实体</returns>
        public HealthStatus SelectByID()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_HealthStatus_SelectByID", new OracleParameter[] { new OracleParameter("p_HSID", Id), o_Cursor });

            HealthStatus info = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                info = new HealthStatus()
                {
                    Id = Convert.ToInt32(ds.Tables[0].Rows[0]["HSID"]),
                    ResourceID = Convert.ToInt32(ds.Tables[0].Rows[0]["ResourceID"]),
                    ResourceType = Convert.ToInt32(ds.Tables[0].Rows[0]["ResourceType"]),
                    FunctionType = ds.Tables[0].Rows[0]["FunctionType"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["FunctionType"].ToString(),
                    Status = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"]),
                    BeginTime = ds.Tables[0].Rows[0]["BeginTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ds.Tables[0].Rows[0]["BeginTime"]),
                    EndTime = ds.Tables[0].Rows[0]["EndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ds.Tables[0].Rows[0]["EndTime"]),
                    CreatedTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedTime"]),
                    CreatedUserID = ds.Tables[0].Rows[0]["CreatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(ds.Tables[0].Rows[0]["CreatedUserID"]),
                    UpdatedTime = ds.Tables[0].Rows[0]["UpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ds.Tables[0].Rows[0]["UpdatedTime"]),
                    UpdatedUserID = ds.Tables[0].Rows[0]["UpdatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(ds.Tables[0].Rows[0]["UpdatedUserID"])
                };
            }
            return info;
        }

        /// <summary>
        /// 获得所有资源健康状态实体列表
        /// </summary>
        /// <returns>资源健康状态实体列表</returns>
        public List<HealthStatus> SelectAll()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_HealthStatus_SelectAll", new OracleParameter[] { o_Cursor });

            List<HealthStatus> infoList = new List<HealthStatus>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    HealthStatus info = new HealthStatus()
                    {
                        Id = Convert.ToInt32(dr["HSID"]),
                        ResourceID = Convert.ToInt32(dr["ResourceID"]),
                        ResourceType = Convert.ToInt32(dr["ResourceType"]),
                        FunctionType = dr["FunctionType"] == DBNull.Value ? string.Empty : dr["FunctionType"].ToString(),
                        Status = Convert.ToInt32(dr["Status"]),
                        BeginTime = dr["BeginTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["BeginTime"]),
                        EndTime = dr["EndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["EndTime"]),
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
        /// 查询资源健康状态
        /// </summary>
        /// <returns>资源健康状态实体列表</returns>
        public List<HealthStatus> Search(int resourceType, int resourceID, DateTime beginTime, DateTime endTime)
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_HealthStatus_Search", new OracleParameter[]{
                                        new OracleParameter("p_ResourceType",resourceType),                        
                                        new OracleParameter("p_ResourceID",resourceID < 1 ? DBNull.Value as object : resourceID),
                                        new OracleParameter("p_BeginTime",beginTime),
                                        new OracleParameter("p_EndTime", endTime),
                                        o_Cursor});

            List<HealthStatus> infoList = new List<HealthStatus>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    HealthStatus info = new HealthStatus()
                    {
                        Id = Convert.ToInt32(dr["HSID"]),
                        ResourceID = Convert.ToInt32(dr["ResourceID"]),
                        ResourceType = Convert.ToInt32(dr["ResourceType"]),
                        FunctionType = dr["FunctionType"] == DBNull.Value ? string.Empty : dr["FunctionType"].ToString(),
                        Status = Convert.ToInt32(dr["Status"]),
                        BeginTime = dr["BeginTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["BeginTime"]),
                        EndTime = dr["EndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["EndTime"]),
                        CreatedTime = Convert.ToDateTime(dr["CreatedTime"]),
                        CreatedUserID = dr["CreatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["CreatedUserID"]),
                        UpdatedTime = dr["UpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["UpdatedTime"]),
                        UpdatedUserID = dr["UpdatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["UpdatedUserID"]),
                        ResourceName = dr["ResourceName"] == DBNull.Value ? string.Empty : dr["ResourceName"].ToString(),
                        ResourceCode = dr["ResourceCode"] == DBNull.Value ? string.Empty : dr["ResourceCode"].ToString()
                    };

                    infoList.Add(info);
                }
            }
            return infoList;
        }
        /// <summary>
        /// 校验某资源相同功能类型下是否存在重叠时间段的健康异常记录
        /// </summary>
        /// <returns>true:存在</returns>
        public bool HaveEffectiveHealthStatus()
        {
            List<HealthStatus> infoList = Search(ResourceType, ResourceID, BeginTime, EndTime);
            var query = infoList.Where(a => a.Id != Id && a.FunctionType == FunctionType);
            if (query != null && query.Count() > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 添加健康状态记录
        /// </summary>
        /// <returns>添加结果</returns>
        public FieldVerifyResult Add()
        {
            OracleParameter v_Result = PrepareOutputResult();
            OracleParameter v_ID = new OracleParameter()
            {
                ParameterName = "v_HSID",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Int32
            };

            _dataBase.SpExecuteNonQuery("UP_HealthStatus_Insert", new OracleParameter[]{
                                        new OracleParameter("p_ResourceID",ResourceID),
                                        new OracleParameter("p_ResourceType",ResourceType),
                                        new OracleParameter("p_FunctionType", string.IsNullOrEmpty(FunctionType) ?  DBNull.Value as object : FunctionType),
                                        new OracleParameter("p_Status",Status),
                                        new OracleParameter("p_BeginTime",BeginTime == DateTime.MinValue ? DBNull.Value as object : BeginTime),
                                        new OracleParameter("p_EndTime", EndTime == DateTime.MinValue ? DBNull.Value as object : EndTime),
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
        /// 根据ID更新健康状态记录
        /// </summary>
        /// <returns>更新结果</returns>
        public FieldVerifyResult Update()
        {
            OracleParameter v_Result = PrepareOutputResult();

            _dataBase.SpExecuteNonQuery("UP_HealthStatus_Update", new OracleParameter[]{
                                        new OracleParameter("p_HSID",Id),
                                        new OracleParameter("p_ResourceID",ResourceID),
                                        new OracleParameter("p_ResourceType",ResourceType),
                                        new OracleParameter("p_FunctionType",string.IsNullOrEmpty(FunctionType) ?  DBNull.Value as object : FunctionType),
                                        new OracleParameter("p_Status",Status),
                                        new OracleParameter("p_BeginTime",BeginTime == DateTime.MinValue ? DBNull.Value as object : BeginTime),
                                        new OracleParameter("p_EndTime", EndTime == DateTime.MinValue ? DBNull.Value as object : EndTime),
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
            this.AddValidRules("ResourceID", "资源序号不能为空。", (ResourceID == 0));
            this.AddValidRules("ResourceType", "资源类型不能为空。", (ResourceType == 0));
            this.AddValidRules("Status", "健康状态不能为空。", (Status == 0));
        }
        #endregion
    }
}

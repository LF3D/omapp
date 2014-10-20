#region
//------------------------------------------------------
//Assembly:OperatingManagement.DataAccessLayer
//FileName:ResourceCalculate.cs
//Remark:资源计算类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120226    Create     
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
    public class ResourceCalculate : BaseEntity<int, ResourceCalculate>
    {
        public ResourceCalculate()
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region Properties
        private OracleDatabase _dataBase = null;
        /// <summary>
        /// 资源需求文件目录
        /// </summary>
        public string RequirementFileDirectory { get; set; }
        /// <summary>
        /// 资源需求文件名称
        /// </summary>
        public string RequirementFileName { get; set; }
        /// <summary>
        /// 资源需求文件显示名称
        /// </summary>
        public string RequirementFileDisplayName { get; set; }
        /// <summary>
        /// 资源计算结果文件目录
        /// </summary>
        public string ResultFileDirectory { get; set; }
        /// <summary>
        /// 资源计算结果文件名称
        /// </summary>
        public string ResultFileName { get; set; }
        /// <summary>
        /// 资源计算结果文件显示名称
        /// </summary>
        public string ResultFileDisplayName { get; set; }
        /// <summary>
        /// 资源计算结果来源：系统计算（1）、用户上传（2）
        /// </summary>
        public int ResultFileSource { get; set; }
        /// <summary>
        /// 资源计算结果：计算成功（1）、计算失败（2）；
        /// </summary>
        public int CalculateResult { get; set; }
        /// <summary>
        /// 状态：等待计算（1）、计算完成（2）；
        /// </summary>
        public int Status { get; set; }
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
        /// 根据ID获得资源计算实体
        /// </summary>
        /// <returns>资源计算实体</returns>
        public ResourceCalculate SelectByID()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_ResCalculate_SelectByID", new OracleParameter[] { new OracleParameter("p_CRID", Id), o_Cursor });

            ResourceCalculate info = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                info = new ResourceCalculate()
                {
                    Id = Convert.ToInt32(ds.Tables[0].Rows[0]["RCID"]),
                    RequirementFileDirectory = ds.Tables[0].Rows[0]["RequirementFileDirectory"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["RequirementFileDirectory"].ToString(),
                    RequirementFileName = ds.Tables[0].Rows[0]["RequirementFileName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["RequirementFileName"].ToString(),
                    RequirementFileDisplayName = ds.Tables[0].Rows[0]["RequirementFileDisplayName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["RequirementFileDisplayName"].ToString(),
                    ResultFileDirectory = ds.Tables[0].Rows[0]["ResultFileDirectory"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["ResultFileDirectory"].ToString(),
                    ResultFileName = ds.Tables[0].Rows[0]["ResultFileName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["ResultFileName"].ToString(),
                    ResultFileDisplayName = ds.Tables[0].Rows[0]["ResultFileDisplayName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["ResultFileDisplayName"].ToString(),
                    ResultFileSource = ds.Tables[0].Rows[0]["ResultFileSource"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["ResultFileSource"]),
                    CalculateResult = ds.Tables[0].Rows[0]["CalculateResult"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["CalculateResult"]),
                    Status = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"]),    
                    CreatedTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedTime"]),
                    CreatedUserID = ds.Tables[0].Rows[0]["CreatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(ds.Tables[0].Rows[0]["CreatedUserID"]),
                    UpdatedTime = ds.Tables[0].Rows[0]["UpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ds.Tables[0].Rows[0]["UpdatedTime"]),
                    UpdatedUserID = ds.Tables[0].Rows[0]["UpdatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(ds.Tables[0].Rows[0]["UpdatedUserID"])
                };
            }
            return info;
        }

        /// <summary>
        /// 获得所有资源计算实体列表
        /// </summary>
        /// <returns>资源计算实体列表</returns>
        public List<ResourceCalculate> SelectAll()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_ResCalculate_SelectAll", new OracleParameter[] { o_Cursor });

            List<ResourceCalculate> infoList = new List<ResourceCalculate>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ResourceCalculate info = new ResourceCalculate()
                    {
                        Id = Convert.ToInt32(dr["RCID"]),
                        RequirementFileDirectory = dr["RequirementFileDirectory"] == DBNull.Value ? string.Empty : dr["RequirementFileDirectory"].ToString(),
                        RequirementFileName = dr["RequirementFileName"] == DBNull.Value ? string.Empty : dr["RequirementFileName"].ToString(),
                        RequirementFileDisplayName = dr["RequirementFileDisplayName"] == DBNull.Value ? string.Empty : dr["RequirementFileDisplayName"].ToString(),
                        ResultFileDirectory = dr["ResultFileDirectory"] == DBNull.Value ? string.Empty : dr["ResultFileDirectory"].ToString(),
                        ResultFileName = dr["ResultFileName"] == DBNull.Value ? string.Empty : dr["ResultFileName"].ToString(),
                        ResultFileDisplayName = dr["ResultFileDisplayName"] == DBNull.Value ? string.Empty : dr["ResultFileDisplayName"].ToString(),
                        ResultFileSource = dr["ResultFileSource"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ResultFileSource"]),
                        CalculateResult = dr["CalculateResult"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CalculateResult"]),
                        Status = Convert.ToInt32(dr["Status"]),
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
        /// 获得满足条件的资源计算实体列表
        /// </summary>
        /// <param name="resultFileSource">结果文件来源</param>
        /// <param name="status">状态</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>资源计算实体列表</returns>
        public List<ResourceCalculate> Search(int resultFileSource, int status, DateTime beginTime, DateTime endTime)
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_ResCalculate_Search", new OracleParameter[] {
                new OracleParameter("p_ResultFileSource", resultFileSource == 0 ? DBNull.Value as object : resultFileSource)
               ,new OracleParameter("p_Status", status == 0 ? DBNull.Value as object : status)
               ,new OracleParameter("p_BeginTime", beginTime)
               ,new OracleParameter("p_EndTime", endTime)
               , o_Cursor });

            List<ResourceCalculate> infoList = new List<ResourceCalculate>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ResourceCalculate info = new ResourceCalculate()
                    {
                        Id = Convert.ToInt32(dr["RCID"]),
                        RequirementFileDirectory = dr["RequirementFileDirectory"] == DBNull.Value ? string.Empty : dr["RequirementFileDirectory"].ToString(),
                        RequirementFileName = dr["RequirementFileName"] == DBNull.Value ? string.Empty : dr["RequirementFileName"].ToString(),
                        RequirementFileDisplayName = dr["RequirementFileDisplayName"] == DBNull.Value ? string.Empty : dr["RequirementFileDisplayName"].ToString(),
                        ResultFileDirectory = dr["ResultFileDirectory"] == DBNull.Value ? string.Empty : dr["ResultFileDirectory"].ToString(),
                        ResultFileName = dr["ResultFileName"] == DBNull.Value ? string.Empty : dr["ResultFileName"].ToString(),
                        ResultFileDisplayName = dr["ResultFileDisplayName"] == DBNull.Value ? string.Empty : dr["ResultFileDisplayName"].ToString(),
                        ResultFileSource = dr["ResultFileSource"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ResultFileSource"]),
                        CalculateResult = dr["CalculateResult"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CalculateResult"]),
                        Status = Convert.ToInt32(dr["Status"]),
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
        /// 添加资源计算记录
        /// </summary>
        /// <returns>添加结果</returns>
        public FieldVerifyResult Add()
        {
            OracleParameter v_Result = PrepareOutputResult();
            OracleParameter v_ID = new OracleParameter()
            {
                ParameterName = "v_RCID",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Int32
            };

            _dataBase.SpExecuteNonQuery("UP_ResCalculate_Insert", new OracleParameter[]{
                                        new OracleParameter("p_RequirementFileDirectory",string.IsNullOrEmpty(RequirementFileDirectory) ? DBNull.Value as object : RequirementFileDirectory),
                                        new OracleParameter("p_RequirementFileName",string.IsNullOrEmpty(RequirementFileName) ? DBNull.Value as object : RequirementFileName),
                                        new OracleParameter("p_RequirementFileDisplayName",string.IsNullOrEmpty(RequirementFileDisplayName) ? DBNull.Value as object : RequirementFileDisplayName),
                                        new OracleParameter("p_ResultFileDirectory",string.IsNullOrEmpty(ResultFileDirectory) ? DBNull.Value as object : ResultFileDirectory),
                                        new OracleParameter("p_ResultFileName",string.IsNullOrEmpty(ResultFileName) ? DBNull.Value as object : ResultFileName),
                                        new OracleParameter("p_ResultFileDisplayName",string.IsNullOrEmpty(ResultFileDisplayName) ? DBNull.Value as object : ResultFileDisplayName),
                                        new OracleParameter("p_ResultFileSource",ResultFileSource == 0 ? DBNull.Value as object : ResultFileSource),
                                        new OracleParameter("p_CalculateResult",CalculateResult == 0 ? DBNull.Value as object : CalculateResult),
                                        new OracleParameter("p_Status",Status),
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
        /// 根据ID更新资源计算记录
        /// </summary>
        /// <returns>更新结果</returns>
        public FieldVerifyResult Update()
        {
            OracleParameter v_Result = PrepareOutputResult();

            _dataBase.SpExecuteNonQuery("UP_ResCalculate_Update", new OracleParameter[]{
                                        new OracleParameter("p_RCID",Id),
                                        new OracleParameter("p_RequirementFileDirectory",string.IsNullOrEmpty(RequirementFileDirectory) ? DBNull.Value as object : RequirementFileDirectory),
                                        new OracleParameter("p_RequirementFileName",string.IsNullOrEmpty(RequirementFileName) ? DBNull.Value as object : RequirementFileName),
                                        new OracleParameter("p_RequirementFileDisplayName",string.IsNullOrEmpty(RequirementFileDisplayName) ? DBNull.Value as object : RequirementFileDisplayName),
                                        new OracleParameter("p_ResultFileDirectory",string.IsNullOrEmpty(ResultFileDirectory) ? DBNull.Value as object : ResultFileDirectory),
                                        new OracleParameter("p_ResultFileName",string.IsNullOrEmpty(ResultFileName) ? DBNull.Value as object : ResultFileName),
                                        new OracleParameter("p_ResultFileDisplayName",string.IsNullOrEmpty(ResultFileDisplayName) ? DBNull.Value as object : ResultFileDisplayName),
                                        new OracleParameter("p_ResultFileSource",ResultFileSource == 0 ? DBNull.Value as object : ResultFileSource),
                                        new OracleParameter("p_CalculateResult",CalculateResult == 0 ? DBNull.Value as object : CalculateResult),
                                        new OracleParameter("p_Status",Status),
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

        }
        #endregion
    }
}

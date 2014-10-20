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
    public class DMZ : BaseEntity<int, DMZ>
    {
        public DMZ()
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        public DMZ(DataRow dr)
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
            Id = Convert.ToInt32(dr["RID"]);
            DMZName = dr["DMZName"].ToString();
            DMZCode = dr["DMZCode"].ToString();
            Owner = Convert.ToInt32(dr["Owner"]);
            DWCode = dr["DWCode"].ToString();
        }

        #region Properties
        private OracleDatabase _dataBase = null;
        /// <summary>
        /// DMZ名称
        /// </summary>
        public string DMZName { get; set; }
        /// <summary>
        /// DMZ编号
        /// </summary>
        public string DMZCode { get; set; }
        /// <summary>
        /// 管理单位：1总参，2总装，3遥科学站
        /// </summary>
        public int Owner { get; set; }
        /// <summary>
        /// 管理单位编码
        /// </summary>
        public string DWCode { get; set; }

        public static List<DMZ> _xyxsInfoCache = null;
        public List<DMZ> Cache
        {
            get
            {
                if (_xyxsInfoCache == null)
                {
                    _xyxsInfoCache = SelectAll();
                }
                return _xyxsInfoCache;
            }
            set
            {
                _xyxsInfoCache = value;
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

        private void RefreshCache()
        {
            _xyxsInfoCache = SelectAll();
        }
        #endregion

        #region -Public Method-
        /// <summary>
        /// 根据ID获得DMZ实体
        /// </summary>
        /// <returns>DMZ实体</returns>
        public DMZ SelectByID()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_DMZ_GetByRID", new OracleParameter[] { new OracleParameter("p_RID", Id), o_Cursor });

            DMZ info = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                info = new DMZ(ds.Tables[0].Rows[0]);
            }
            return info;
        }

        /// <summary>
        /// 获得所有DMZ实体列表
        /// </summary>
        /// <returns>DMZ实体列表</returns>
        public List<DMZ> SelectAll()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_DMZ_SelectAll", new OracleParameter[] { o_Cursor });

            List<DMZ> infoList = new List<DMZ>();
            DMZ info;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    info = new DMZ(dr);
                    infoList.Add(info);
                }
            }
            return infoList;
        }

        /// <summary>
        /// 通过地面站内部编码获取DMZ
        /// </summary>
        /// <param name="dmzIncode"></param>
        /// <returns></returns>
        public List<DMZ> SelectByOwner()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_DMZ_GetByOwner"
                , new OracleParameter[] { 
                    new OracleParameter("p_Owner", Owner),
                    o_Cursor });

            List<DMZ> infoList = new List<DMZ>();
            DMZ info;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    info = new DMZ(dr);
                    infoList.Add(info);
                }
            }
            return infoList;
        }

        /// <summary>
        /// 添加DMZ记录
        /// </summary>
        /// <returns>添加结果</returns>
        public FieldVerifyResult Add()
        {
            OracleParameter v_Result = PrepareOutputResult();
            OracleParameter v_ID = new OracleParameter()
            {
                ParameterName = "v_RID",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Int32
            };

            _dataBase.SpExecuteNonQuery("UP_DMZ_Insert", new OracleParameter[]{
                                        new OracleParameter("p_DMZName", DMZName),
                                        new OracleParameter("p_DMZCode", DMZCode),
                                        new OracleParameter("p_Owner", Owner),
                                        new OracleParameter("p_DWCode", DWCode),
                                        v_ID,
                                        v_Result});
            if (v_ID.Value != null && v_ID.Value != DBNull.Value)
                this.Id = Convert.ToInt32(v_ID.Value);
            RefreshCache();
            return (FieldVerifyResult)Convert.ToInt32(v_Result.Value);
        }

        /// <summary>
        /// 根据ID更新DMZ记录
        /// </summary>
        /// <returns>更新结果</returns>
        public FieldVerifyResult Update()
        {
            OracleParameter v_Result = PrepareOutputResult();

            _dataBase.SpExecuteNonQuery("UP_DMZ_Update", new OracleParameter[]{
                                        new OracleParameter("p_RID",Id),
                                        new OracleParameter("p_DMZName", DMZName),
                                        new OracleParameter("p_DMZCode", DMZCode),
                                        new OracleParameter("p_Owner", Owner),
                                        new OracleParameter("p_DWCode", DWCode),
                                        v_Result});
            RefreshCache();
            return (FieldVerifyResult)Convert.ToInt32(v_Result.Value);
        }

        /// <summary>
        /// 通过DMZCode获取地面站信息
        /// </summary>
        /// <returns></returns>
        public DMZ GetByCode()
        {
            List<DMZ> dmzList = new List<DMZ>();
            dmzList = this.Cache.Where(t => t.DMZCode == this.DMZCode).ToList();
            if (dmzList.Count > 0)
                return dmzList[0];
            else
                return null;
        }

        /// <summary>
        /// 通过DWCode获取地面站信息
        /// </summary>
        /// <returns></returns>
        public DMZ GetByDWCode()
        {
            List<DMZ> dmzList = new List<DMZ>();
            dmzList = this.Cache.Where(t => t.DWCode == this.DWCode).ToList();
            if (dmzList.Count > 0)
                return dmzList[0];
            else
                return null;
        }

        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
            this.AddValidRules("DMZName", "地面站名称不能为空。", string.IsNullOrEmpty(DMZName));
            this.AddValidRules("DMZCode", "地面站编号不能为空。", string.IsNullOrEmpty(DMZCode));
        }
        #endregion
    }
}

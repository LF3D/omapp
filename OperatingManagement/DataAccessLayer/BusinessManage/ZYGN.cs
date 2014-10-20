using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    [Serializable]
    public class ZYGN : BaseEntity<int, ZYGN>
    {
        public ZYGN()
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region Properties
        private OracleDatabase _dataBase = null;
        /// <summary>
        /// 名称
        /// </summary>
        public string FName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string FCode { get; set; }
        /// <summary>
        /// 匹配准则;
        /// </summary>
        public string MatchRule { get; set; }
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
        /// 根据ID获得资源功能实体
        /// </summary>
        /// <returns>资源功能实体</returns>
        public ZYGN SelectByID()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_ZYGN_SelectByID", new OracleParameter[] { new OracleParameter("p_ID", Id), o_Cursor });

            ZYGN info = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                info = new ZYGN()
                {
                    Id = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]),
                    FName = ds.Tables[0].Rows[0]["FName"].ToString(),
                    FCode = ds.Tables[0].Rows[0]["FCode"].ToString(),
                    MatchRule = ds.Tables[0].Rows[0]["MatchRule"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["MatchRule"].ToString()

                };
            }
            return info;
        }

        /// <summary>
        /// 获得所有资源功能实体列表
        /// </summary>
        /// <returns>资源功能实体列表</returns>
        public List<ZYGN> SelectAll()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_ZYGN_SelectAll", new OracleParameter[] { o_Cursor });

            List<ZYGN> infoList = new List<ZYGN>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ZYGN info = new ZYGN()
                    {
                        Id = Convert.ToInt32(dr["ID"]),
                        FName = dr["FName"].ToString(),
                        FCode = dr["FCode"].ToString(),
                        MatchRule = dr["MatchRule"] == DBNull.Value ? string.Empty : dr["MatchRule"].ToString()
                    };

                    infoList.Add(info);
                }
            }
            return infoList;
        }

        /// <summary>
        /// Insert a ZYGN.
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
            _dataBase.SpExecuteNonQuery("UP_ZYGN_Insert", new OracleParameter[]{
                new OracleParameter("p_FName",this.FName),
                new OracleParameter("p_FCode",this.FCode),
                new OracleParameter("p_MatchRule",this.MatchRule),
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
            _dataBase.SpExecuteNonQuery("UP_ZYGN_Update", new OracleParameter[]{
                new OracleParameter("p_Id", this.Id),
                new OracleParameter("p_FName",this.FName),
                new OracleParameter("p_FCode",this.FCode),
                new OracleParameter("p_MatchRule",this.MatchRule),
                p
            });
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
        }

        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
        }
        #endregion
    }

    public class MatchRule : BaseEntity<int, MatchRule>
    {
        public string PCode { get; set; }
        public emLogicSymbol LogicSymbol { get; set; }

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
        }
        #endregion
    }

    public enum emLogicSymbol
    {
        LessThan = 0,
        LessThanEqual = 1,
        Equal = 2,
        MoreThanEqual = 3,
        MoreThan = 4
    }

}

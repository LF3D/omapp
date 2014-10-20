#region
//------------------------------------------------------
//Assembly:OperatingManagement.DataAccessLayer
//FileName:XXTYPE.cs
//Remark:信息类型读取类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120114    Create     
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
    public class XXTYPE : BaseEntity<int, XXTYPE>
    {
        public XXTYPE()
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region Properties
        private OracleDatabase _dataBase = null;

        /// <summary>
        /// 信息名称
        /// </summary>
        public string DATANAME { get; set; }
        /// <summary>
        /// 内部标识
        /// </summary>
        public string INMARK { get; set; }
        /// <summary>
        /// 外部标识
        /// </summary>
        public string EXMARK { get; set; }
        /// <summary>
        /// 内部编码
        /// </summary>
        public string INCODE { get; set; }
        /// <summary>
        /// 外部编码
        /// </summary>
        public string EXCODE { get; set; }
        /// <summary>
        /// 信息类型：0-数据帧；1-文件，使用Enum InfoType
        /// </summary>
        public string DATATYPE { get; set; }

        public static List<XXTYPE> _xxTypeCache = null;
        public List<XXTYPE> XXTYPECache
        {
            get
            {
                if (_xxTypeCache == null)
                {
                    _xxTypeCache = SelectAll();
                }
                return _xxTypeCache;
            }
            set{}
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

        #region -Public Method-
        /// <summary>
        /// 根据ID获得信息类型实体
        /// </summary>
        /// <returns>信息类型实体</returns>
        public XXTYPE SelectByID()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_XXTYPE_SelectByID", new OracleParameter[] { new OracleParameter("p_RID", Id), o_Cursor });

            XXTYPE info = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                info = new XXTYPE()
                {
                    Id = Convert.ToInt32(ds.Tables[0].Rows[0]["RID"]),
                    DATANAME = ds.Tables[0].Rows[0]["DATANAME"].ToString(),
                    INMARK = ds.Tables[0].Rows[0]["INMARK"].ToString(),
                    EXMARK = ds.Tables[0].Rows[0]["EXMARK"].ToString(),
                    INCODE = ds.Tables[0].Rows[0]["INCODE"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["INCODE"].ToString(),
                    EXCODE = ds.Tables[0].Rows[0]["EXCODE"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[0]["EXCODE"].ToString(),
                    DATATYPE = ds.Tables[0].Rows[0]["DATATYPE"].ToString()
                };
            }
            return info;
        }

        /// <summary>
        /// 获得信息类型实体列表
        /// </summary>
        /// <returns>信息类型实体列表</returns>
        public List<XXTYPE> SelectAll()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_XXTYPE_SelectAll", new OracleParameter[] { o_Cursor });

            List<XXTYPE> infoList = new List<XXTYPE>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    XXTYPE info = new XXTYPE()
                    {
                        Id = Convert.ToInt32(dr["RID"]),
                        DATANAME = dr["DATANAME"].ToString(),
                        INMARK = dr["INMARK"].ToString(),
                        EXMARK = dr["EXMARK"].ToString(),
                        INCODE = dr["INCODE"] == DBNull.Value ? string.Empty : dr["INCODE"].ToString(),
                        EXCODE = dr["EXCODE"] == DBNull.Value ? string.Empty : dr["EXCODE"].ToString(),
                        DATATYPE = dr["DATATYPE"].ToString()
                    };

                    infoList.Add(info);
                }
            }
            return infoList;
        }
        /// <summary>
        /// 根据rid获得信息类型的DATANAME
        /// </summary>
        /// <param name="rid">编号</param>
        /// <returns>信息类型DATANAME</returns>
        public string GetName(int rid)
        {
            string dataName = string.Empty;
            if (XXTYPECache != null)
            {
                var query = XXTYPECache.Where(a => a.Id == rid);
                if (query != null && query.Count() > 0)
                    dataName = query.FirstOrDefault().DATANAME;
            }
            return dataName;
        }
        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
        }
        #endregion
    }
}

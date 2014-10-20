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
    public class InfoType : BaseEntity<int, InfoType>
    {
        public InfoType()
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

        public static List<InfoType> _infoTypeCache = null;
        public List<InfoType> Cache
        {
            get
            {
                if (_infoTypeCache == null)
                {
                    _infoTypeCache = SelectAll();
                }
                return _infoTypeCache;
            }
            set
            {
                _infoTypeCache = value;
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

        #region -Public Method-
        /// <summary>
        /// 根据ID获得信息类型实体
        /// </summary>
        /// <returns>信息类型实体</returns>
        public InfoType SelectByID()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            InfoType info = null;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.Id == Id);
                if (query != null && query.Count() > 0)
                    info = query.FirstOrDefault();
                return info;
            }

            DataSet ds = _dataBase.SpExecuteDataSet("UP_InfoTYPE_SelectByID", new OracleParameter[] { new OracleParameter("p_RID", Id), o_Cursor });

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                info = new InfoType()
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
        public List<InfoType> SelectAll()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_InfoTYPE_SelectAll", new OracleParameter[] { o_Cursor });

            List<InfoType> infoList = new List<InfoType>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    InfoType info = new InfoType()
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
            if (Cache != null)
            {
                var query = Cache.Where(a => a.Id == rid);
                if (query != null && query.Count() > 0)
                    dataName = query.FirstOrDefault().DATANAME;
            }
            return dataName;
        }



        /// <summary>
        /// 根据信息外部标识获取信息类型
        /// </summary>
        /// <param name="exCode"></param>
        /// <returns></returns>
        public InfoType GetByExMark(string exMark)
        {
            if (Cache != null)
            {
                var query = Cache.Where(a => a.EXMARK == exMark);
                if (query != null && query.Count() > 0)
                    return (InfoType)query.FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 根据ID获取信息类型
        /// </summary>
        /// <param name="exCode"></param>
        /// <returns></returns>
        public InfoType GetByID(int id)
        {
            if (Cache != null)
            {
                var query = Cache.Where(a => a.Id == id);
                if (query != null && query.Count() > 0)
                    return (InfoType)query.FirstOrDefault();
            }
            return null;
        }
        
        /// <summary>
        /// 根据信息外部编码获取信息内部编码
        /// </summary>
        /// <param name="exCode"></param>
        /// <returns></returns>
        public int GetIDByInCode(string inCode)
        {
            int iID = 0;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.INCODE == inCode);
                if (query != null && query.Count() > 0)
                    iID = query.FirstOrDefault().Id;
            }
            return iID;
        }

        /// <summary>
        /// 根据信息外部编码获取信息内部编码
        /// </summary>
        /// <param name="exCode"></param>
        /// <returns></returns>
        public string GetInCodeByExCode(string exCode)
        {
            string strInCode = string.Empty;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.EXCODE == exCode);
                if (query != null && query.Count() > 0)
                    strInCode = query.FirstOrDefault().INCODE;
            }
            return strInCode;
        }

        /// <summary>
        /// 根据信息内部编码获取信息内部编码
        /// </summary>
        /// <param name="exCode"></param>
        /// <returns></returns>
        public string GetExCodeByInCode(string inCode)
        {
            string strExCode = string.Empty;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.EXCODE == inCode);
                if (query != null && query.Count() > 0)
                    strExCode = query.FirstOrDefault().EXCODE;
            }
            return strExCode;
        }

        /// <summary>
        /// Get ExMark by Id
        /// </summary>
        /// <param name="exCode"></param>
        /// <returns></returns>
        public string GetExMarkById()
        {
            string strExMark = string.Empty;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.Id == this.Id);
                if (query != null && query.Count() > 0)
                    strExMark = query.FirstOrDefault().EXMARK;
            }
            return strExMark;
        }

        /// <summary>
        /// 根据EXMARK获得信息类型的rid
        /// </summary>
        /// <param name="exMark">EXMARK</param>
        /// <returns>rid</returns>
        public int GetIDByExMark(string exMark)
        {
            int rid = -1;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.EXMARK == exMark);
                if (query != null && query.Count() > 0)
                    rid = query.FirstOrDefault().Id;
            }
            return rid;
        }
        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
        }
        #endregion
    }
}

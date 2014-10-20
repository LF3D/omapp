#region
//------------------------------------------------------
//Assembly:OperatingManagement.DataAccessLayer
//FileName:XYXSInfo.cs
//Remark:信源信宿读取类
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
    public class XYXSInfo : BaseEntity<int, XYXSInfo>
    {
        public XYXSInfo()
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        public XYXSInfo(DataRow dr)
        {
            _dataBase = OracleDatabase.FromConfiguringNode("ApplicationServices");
            Id = Convert.ToInt32(dr["RID"]);
            ADDRName = dr["ADDRName"].ToString();
            ADDRMARK = dr["ADDRMARK"] == DBNull.Value ? string.Empty : dr["ADDRMARK"].ToString();
            INCODE = dr["INCODE"].ToString();
            EXCODE = dr["EXCODE"].ToString();
            MainIP = dr["MainIP"] == DBNull.Value ? string.Empty : dr["MainIP"].ToString();
            TCPPort = dr["TCPPort"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TCPPort"].ToString());
            BakIP = dr["BakIP"] == DBNull.Value ? string.Empty : dr["BakIP"].ToString();
            UDPPort = dr["UDPPort"] == DBNull.Value ? 0 : Convert.ToInt32(dr["UDPPort"].ToString());
            Type = Convert.ToInt32(dr["Type"]);
            //Own = dr["Own"] == DBNull.Value ? string.Empty : dr["Own"].ToString();
            //Coordinate = dr["Coordinate"] == DBNull.Value ? string.Empty : dr["Coordinate"].ToString();
            Status = Convert.ToInt32(dr["Status"]);
            //CreatedTime = Convert.ToDateTime(dr["CreatedTime"]);
            //CreatedUserID = dr["CreatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["CreatedUserID"]);
            //UpdatedTime = dr["UpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["UpdatedTime"]);
            //UpdatedUserID = dr["UpdatedUserID"] == DBNull.Value ? 0.0 : Convert.ToDouble(dr["UpdatedUserID"]);
            //DWCode = dr["DWCODE"].ToString();

            if (dr["FTPPath"] != DBNull.Value && dr["FTPPath"] != null)
            {
                string ftps = dr["FTPPath"].ToString();
                if (!String.IsNullOrEmpty(ftps))
                {
                    string[] strFTP = ftps.Split(new char[] { '@' });
                    FTPPath = strFTP[0];
                    FTPUser = strFTP[1];
                    FTPPwd = strFTP[2];
                }
            }
        }

        #region Properties
        private OracleDatabase _dataBase = null;

        /// <summary>
        /// 地址名称
        /// </summary>
        public string ADDRName { get; set; }
        /// <summary>
        /// 地址标识
        /// </summary>
        public string ADDRMARK { get; set; }
        /// <summary>
        /// 内部十六进制编码
        /// </summary>
        public string INCODE { get; set; }
        /// <summary>
        /// 外部十六进制编码
        /// </summary>
        public string EXCODE { get; set; }
        /// <summary>
        /// 主IP地址
        /// </summary>
        public string MainIP { get; set; }
        /// <summary>
        /// TCP端口
        /// </summary>
        public int TCPPort { get; set; }
        /// <summary>
        /// 备用IP地址
        /// </summary>
        public string BakIP { get; set; }
        /// <summary>
        /// UDP端口
        /// </summary>
        public int UDPPort { get; set; }
        /// <summary>
        /// FTPPath
        /// </summary>
        public string FTPPath { get; set; }
        /// <summary>
        /// FTP User Name
        /// </summary>
        public string FTPUser { get; set; }
        /// <summary>
        /// FTP User Password
        /// </summary>
        public string FTPPwd { get; set; }
        /// <summary>
        /// 类型；0：地面站，1：中心；2：分系统
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 地面站归属（管理单位）；01：总参，02：总装，03：遥科学站
        /// </summary>
        public string Own { get; set; }
        /// <summary>
        /// 站址坐标
        /// </summary>
        public string Coordinate { get; set; }
        /// <summary>
        /// 状态；1：正常，2：删除
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
        /// <summary>
        /// 单位编码
        /// </summary>
        public string DWCode { get; set; }
        public static List<XYXSInfo> _xyxsInfoCache = null;
        public List<XYXSInfo> Cache
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
            this.Cache = SelectAll();
        }
        #endregion

        #region -Public Method-
        /// <summary>
        /// 根据ID获得信源信宿
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public XYXSInfo SelectByID()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_XYXSINFO_SelectByID", new OracleParameter[] { new OracleParameter("p_RID", Id), o_Cursor });

            XYXSInfo info = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                info = new XYXSInfo(ds.Tables[0].Rows[0]);
            }
            return info;
        }

        /// <summary>
        /// 获得信源信宿
        /// </summary>
        /// <returns></returns>
        public List<XYXSInfo> SelectAll()
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_XYXSINFO_SelectAll", new OracleParameter[] { o_Cursor });

            List<XYXSInfo> infoList = new List<XYXSInfo>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    XYXSInfo info = new XYXSInfo(dr);

                    infoList.Add(info);
                }
            }
            return infoList;
        }
        /// <summary>
        /// 根据条件查询地面站信息
        /// </summary>
        /// <param name="addrName">地址名称</param>
        /// <param name="addrMark">地址编码</param>
        /// <param name="own">地面站归属；总参:01;总装:02;遥科学站:03</param>
        /// <param name="type">类型；地面站:0;中心:1;分系统:2</param>
        /// <param name="status">状态；正常:1;删除:2</param>
        /// <returns></returns>
        public List<XYXSInfo> Search(string addrName, string addrMark, string own, int type, int status)
        {
            OracleParameter o_Cursor = PrepareRefCursor();
            DataSet ds = _dataBase.SpExecuteDataSet("UP_XYXSINFO_Search", new OracleParameter[] { 
                                                    new OracleParameter("p_AddrName", string.IsNullOrEmpty(addrName) ? DBNull.Value as object : addrName), 
                                                    new OracleParameter("p_AddrMark", string.IsNullOrEmpty(addrMark) ? DBNull.Value as object : addrMark), 
                                                    new OracleParameter("p_Own", string.IsNullOrEmpty(own) ? DBNull.Value as object : own),
                                                    new OracleParameter("p_Type", type < 0 ? DBNull.Value as object : type), 
                                                    new OracleParameter("p_Status", status < 0 ? DBNull.Value as object : status), 
                                                    o_Cursor });

            List<XYXSInfo> infoList = new List<XYXSInfo>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    XYXSInfo info = new XYXSInfo(dr);

                    infoList.Add(info);
                }
            }
            return infoList;
        }
        /// <summary>
        /// 添加信源信宿记录
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

            _dataBase.SpExecuteNonQuery("UP_XYXSINFO_Insert", new OracleParameter[]{
                                        new OracleParameter("p_AddrName",ADDRName),
                                        new OracleParameter("p_AddrMark",string.IsNullOrEmpty(ADDRMARK) ? DBNull.Value as object : ADDRMARK),
                                        new OracleParameter("p_InCode",INCODE),
                                        new OracleParameter("p_ExCode",EXCODE),
                                        new OracleParameter("p_MainIP",string.IsNullOrEmpty(MainIP) ? DBNull.Value as object : MainIP),
                                        new OracleParameter("p_TCPPort", TCPPort == 0 ? DBNull.Value as object : TCPPort),
                                        new OracleParameter("p_BakIP",string.IsNullOrEmpty(BakIP) ? DBNull.Value as object : BakIP),
                                        new OracleParameter("p_UDPPort",UDPPort == 0 ? DBNull.Value as object : UDPPort),
                                        new OracleParameter("p_FTPPath",string.IsNullOrEmpty(FTPPath + FTPUser + FTPPwd) ? DBNull.Value as object : FTPPath + "@" + FTPUser + "@" + FTPPwd),
                                        new OracleParameter("p_Type",Type),
                                        //new OracleParameter("p_Own",string.IsNullOrEmpty(Own) ? DBNull.Value as object : Own),
                                        //new OracleParameter("p_Coordinate",string.IsNullOrEmpty(Coordinate) ? DBNull.Value as object : Coordinate),
                                        new OracleParameter("p_Status",Status),
                                        //new OracleParameter("p_CreatedTime",CreatedTime),
                                        //new OracleParameter("p_CreatedUserID",CreatedUserID == 0.0 ? DBNull.Value as object : CreatedUserID),
                                        //new OracleParameter("p_UpdatedTime",UpdatedTime == DateTime.MinValue ? DBNull.Value as object : UpdatedTime),
                                        //new OracleParameter("p_UpdatedUserID",UpdatedUserID == 0.0 ? DBNull.Value as object : UpdatedUserID),
                                        //new OracleParameter("p_DWCode",DWCode),
                                        v_ID,
                                        v_Result});
            if (v_ID.Value != null && v_ID.Value != DBNull.Value)
                this.Id = Convert.ToInt32(v_ID.Value);
            RefreshCache();
            return (FieldVerifyResult)Convert.ToInt32(v_Result.Value);
        }
        /// <summary>
        /// 根据ID更新信源信宿记录
        /// </summary>
        /// <returns>更新结果</returns>
        public FieldVerifyResult Update()
        {
            OracleParameter v_Result = PrepareOutputResult();

            _dataBase.SpExecuteNonQuery("UP_XYXSINFO_Update", new OracleParameter[]{
                                        new OracleParameter("p_RID",Id),
                                        new OracleParameter("p_AddrName",ADDRName),
                                        new OracleParameter("p_AddrMark",string.IsNullOrEmpty(ADDRMARK) ? DBNull.Value as object : ADDRMARK),
                                        new OracleParameter("p_InCode",INCODE),
                                        new OracleParameter("p_ExCode",EXCODE),
                                        new OracleParameter("p_MainIP",string.IsNullOrEmpty(MainIP) ? DBNull.Value as object : MainIP),
                                        new OracleParameter("p_TCPPort", TCPPort == 0 ? DBNull.Value as object : TCPPort),
                                        new OracleParameter("p_BakIP",string.IsNullOrEmpty(BakIP) ? DBNull.Value as object : BakIP),
                                        new OracleParameter("p_UDPPort",UDPPort == 0 ? DBNull.Value as object : UDPPort),
                                        new OracleParameter("p_FTPPath",string.IsNullOrEmpty(FTPPath) ? DBNull.Value as object : FTPPath + "@" + FTPUser + "@" + FTPPwd),
                                        new OracleParameter("p_Type",Type),
                                        //new OracleParameter("p_Own",string.IsNullOrEmpty(Own) ? DBNull.Value as object : Own),
                                        //new OracleParameter("p_Coordinate",string.IsNullOrEmpty(Coordinate) ? DBNull.Value as object : Coordinate),
                                        new OracleParameter("p_Status",Status),
                                        //new OracleParameter("p_CreatedTime",CreatedTime),
                                        //new OracleParameter("p_CreatedUserID",CreatedUserID == 0.0 ? DBNull.Value as object : CreatedUserID),
                                        //new OracleParameter("p_UpdatedTime",UpdatedTime == DateTime.MinValue ? DBNull.Value as object : UpdatedTime),
                                        //new OracleParameter("p_UpdatedUserID",UpdatedUserID == 0.0 ? DBNull.Value as object : UpdatedUserID),
                                        //new OracleParameter("p_DWCode",DWCode),
                                        v_Result});
            RefreshCache();
            return (FieldVerifyResult)Convert.ToInt32(v_Result.Value);
        }
        /// <summary>
        /// 获得状态正常的地面站列表
        /// </summary>
        /// <returns></returns>
        public List<XYXSInfo> GetGrountStationList()
        {
            return Search(string.Empty, string.Empty, string.Empty, 0, 1).OrderBy(a => a.ADDRName).ToList<XYXSInfo>();
        }
        /// <summary>
        /// 根据inCode获得信源信宿的ADDRName
        /// </summary>
        /// <param name="inCode"></param>
        /// <returns>信源信宿的地址</returns>
        public string GetName(string inCode)
        {
            string addrName = string.Empty;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.INCODE.ToLower() == inCode.ToLower());
                if (query != null && query.Count() > 0)
                    addrName = query.FirstOrDefault().ADDRName;
            }
            return addrName;
        }
        /// <summary>
        /// 根据rid获得信源信宿的ADDRName
        /// </summary>
        /// <param name="rid">编号</param>
        /// <returns>信源信宿的地址</returns>
        public string GetName(int rid)
        {
            string addrName = string.Empty;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.Id == rid);
                if (query != null && query.Count() > 0)
                    addrName = query.FirstOrDefault().ADDRName;
            }
            return addrName;
        }

        /// <summary>
        /// 根据inCode获得信源信宿的Id
        /// </summary>
        /// <param name="addrMark">编号</param>
        /// <returns>信源信宿的地址</returns>
        public int GetIdByInCode(string inCode)
        {
            int xid = 0;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.INCODE == inCode);
                if (query != null && query.Count() > 0)
                    xid = query.FirstOrDefault().Id;
            }
            return xid;
        }

        /// <summary>
        /// 根据ADDRMark获得信源信宿的Id
        /// </summary>
        /// <param name="addrMark">编号</param>
        /// <returns>信源信宿的地址</returns>
        public int GetIdByAddrMark(string addrMark)
        {
            int xid = 0;
            if (Cache != null)
            {
                var query = Cache.Where(a => a.ADDRMARK == addrMark);
                if (query != null && query.Count() > 0)
                    xid = query.FirstOrDefault().Id;
            }
            return xid;
        }

        /// <summary>
        /// 通过IP地址获取信源信宿信息
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public XYXSInfo GetByIP(string ipAddress)
        {
            if (Cache != null)
            {
                var query = Cache.Where(a => a.MainIP == ipAddress || a.BakIP == ipAddress);
                if (query != null && query.Count() > 0)
                    return (XYXSInfo)query.FirstOrDefault();
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// 通过IP和port获取信源信宿信息(test)
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public XYXSInfo GetByIPAndPort(string ipAddress, int port)
        {
            if (Cache != null)
            {
                var query = Cache.Where(a => (a.MainIP == ipAddress || a.BakIP == ipAddress)
                    && (a.TCPPort == port || a.UDPPort == port));
                if (query != null && query.Count() > 0)
                    return (XYXSInfo)query.FirstOrDefault();
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// 通过id获取信源信宿信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public XYXSInfo GetByID(int id)
        {
            if (Cache != null)
            {
                var query = Cache.Where(a => a.Id == id);
                if (query != null && query.Count() > 0)
                    return (XYXSInfo)query.FirstOrDefault();
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// 根据AddrMark获取信源信息
        /// </summary>
        /// <param name="addrMark"></param>
        /// <returns></returns>
        public XYXSInfo GetByAddrMark(string addrMark)
        {
            if (Cache != null)
            {
                var query = Cache.Where(a => a.ADDRMARK == addrMark);
                if (query != null && query.Count() > 0)
                    return (XYXSInfo)query.FirstOrDefault();
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// 获取信源信宿的IP地址和端口（tcp/udp）
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="tcpport"></param>
        /// <param name="udpport"></param>
        /// <returns></returns>
        public bool GetIPandPort(out string ip, out int tcpport, out int udpport)
        {
            ip = "";
            tcpport = 0;
            udpport = 0;
            if (this != null)
            {
                ip = !string.IsNullOrEmpty(this.MainIP) ? this.MainIP : this.BakIP;
                tcpport = this.TCPPort;
                udpport = this.UDPPort;
                return true;
            }
            return false;
        }
        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
        }
        #endregion
    }
}

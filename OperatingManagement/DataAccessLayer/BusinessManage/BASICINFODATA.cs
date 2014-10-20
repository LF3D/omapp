using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using System.Data;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    public class BASICINFODATA : BaseEntity<int, BASICINFODATA>
    {
        private static readonly string GET_BASICINFODATAList = "up_BASICINFODATA_getlist";
        private static readonly string Insert = "up_BASICINFODATA_insert";
        private static readonly string SelectByID = "up_BASICINFODATA_selectByID";

        /// <summary>
        /// Create a new instance of <see cref="BASICINFODATA"/> class.
        /// </summary>
        public BASICINFODATA()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;

        public int ID { get; set; }
        public DateTime CTime { get; set; }
        public string Version { get; set; }
        public string Flag { get; set; }
        public string MainType { get; set; }
        public string DataType { get; set; }
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public string MissionCode { get; set; }
        public string SatelliteCode { get; set; }
        public DateTime DataDate { get; set; }
        public string DataTime { get; set; }
        public string SequenceNumber { get; set; }
        public string ChildrenPackNumber { get; set; }
        public string UDPReserve { get; set; }
        public string DataLength { get; set; }
        public string DataClass { get; set; }
        public string TB_TABLE { get; set; }
        #endregion 

        #region -Methods-

        public static string GetMainTypeShowValue(string code)
        {
            string result = String.Empty;
            switch (code)
            { 
                case "00":
                    result = "服务类";
                    break;
                case "10":
                    result = "控制类";
                    break;
                case "20":
                    result = "消息类";
                    break;
                case "30":
                    result = "管理类";
                    break;
                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                    result = "处理数据类";
                    break;
                case "51":
                case "52":
                    result = "处理接口类";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        public static string GetSourceOrDestinationShowValue(string code)
        {
            string result = String.Empty;
            switch (code)
            {
                case "0260":
                    result = "西安中心";
                    break;
                case "026F":
                    result = "空间信息综合应用中心";
                    break;
                case "0204":
                    result = "运控评估中心";
                    break;
                case "02C3":
                    result = "喀什站（TW-217）";
                    break;
                case "02C6":
                    result = "厦门站（TW-218）";
                    break;
                case "02C5":
                    result = "青岛站（TY-4801）";
                    break;
                //case "02C3":
                    //result = "喀什站（TY-4801）";
                    //break;
                case "0248":
                    result = "瑞典站";
                    break;
                case "02B0":
                    result = "总参二部信息处理中心";
                    break;
                case "02B1":
                    result = "总参二部牡丹江站";
                    break;
                case "02B2":
                    result = "总参三部技侦中心";
                    break;
                case "02B3":
                    result = "总参三部长春站";
                    break;
                case "02B4":
                    result = "总参三部乌鲁木齐站";
                    break;
                case "02B5":
                    result = "总参三部广州站";
                    break;
                case "02B6":
                    result = "总参气象水文空间天气总站资料处理中心";
                    break;
                case "02B7":
                    result = "总参气象水文空间天气总站北京站";
                    break;
                case "0229":
                    result = "东风站（TW-218）";
                    break;
                case "02BA":
                    result = "863-YZ4701遥科学综合站";
                    break;
                case "02BC":
                    result = "863-YZ4702遥科学综合站";
                    break;
                case "02E1":
                    result = "天基目标观测应用研究分系统";
                    break;
                case "02E3":
                    result = "空间遥操作应用研究分系统";
                    break;
                case "02E5":
                    result = "空间机动应用研究分系统";
                    break;
                case "02E7":
                    result = "仿真推演分系统";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        public static string GetSatlliteCodeShowValue(string code)
        {
            string result = String.Empty;
            switch (code)
            {
                case "0730":
                    result = "TS-3";
                    break;
                case "074A":
                    result = "TS-4-A";
                    break;
                case "074B":
                    result = "TS-4-B";
                    break;
                case "075A":
                    result = "TS-5-A";
                    break;
                case "075B":
                    result = "TS-5-B";
                    break;
                case "0720":
                    result = "TS-2";
                    break;
                case "07CC":
                    result = "多星相关信息";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 转换历元日期
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DateTime ConvertD(int code)
        {
            DateTime init= new DateTime(2000,1,1);
            return init.AddDays(code);
        }
        /// <summary>
        /// 转换历元时刻
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ConvertT(int code)
        {
            DateTime init = new DateTime(2000, 1, 1);
            init = init.AddMilliseconds(code / 10);
            return init.ToShortTimeString();
        }

        /// <summary>
        /// Selects the specific BASICINFODATA by identification.
        /// </summary>
        /// <returns>BASICINFODATA</returns>
        public BASICINFODATA SelectById()
        {
            OracleParameter p = PrepareRefCursor();

            DataSet ds = _database.SpExecuteDataSet(SelectByID, new OracleParameter[]{
                new OracleParameter("p_Id", this.Id), 
                p
            });

            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return new BASICINFODATA()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        Version = dr["Version"].ToString(),
                        Flag = dr["Flag"].ToString(),
                        MainType = dr["MainType"].ToString(),
                        DataType = dr["DataType"].ToString(),
                        SourceAddress = dr["SourceAddress"].ToString(),
                        DestinationAddress = dr["DestinationAddress"].ToString(),
                        MissionCode = dr["MissionCode"].ToString(),
                        SatelliteCode = dr["SatelliteCode"].ToString(),
                        DataDate = Convert.ToDateTime(dr["DataDate"].ToString()),
                        DataTime = dr["DataTime"].ToString(),
                        SequenceNumber = dr["SequenceNumber"].ToString(),
                        ChildrenPackNumber = dr["ChildrenPackNumber"].ToString(),
                        UDPReserve = dr["UDPReserve"].ToString(),
                        DataLength = dr["DataLength"].ToString(),
                        DataClass = dr["DataClass"].ToString(),
                        TB_TABLE = dr["TB_TABLE"].ToString()
                    };
                }
            }
            return null;
        }

        #endregion

        #region -Private methods-
        private OracleParameter PrepareRefCursor()
        {
            return new OracleParameter()
            {
                ParameterName = "o_cursor",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.RefCursor
            };
        }
        #endregion

        #region -Override BaseEntity-
        protected override void ValidationRules()
        {
            //this.AddValidRules("ID", "序号不能为空。", string.IsNullOrEmpty(ID));
        }
        #endregion
    }
}

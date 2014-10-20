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
    /// <summary>
    /// 测角数据
    /// </summary>
    public class AE : BaseEntity<int, AE>
    {
        private static readonly string GET_AEList_ByDate = "up_ae_getlist";
        private static readonly string Insert = "up_ae_insert";

        /// <summary>
        /// Create a new instance of <see cref="AE"/> class.
        /// </summary>
        public AE()
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
        public string Reserve { get; set; }

        /// <summary>
        /// 占2个字节，二进制代码表示
        /// </summary>
        public string ZT { get; set; }
        /// <summary>
        /// 占4个字节，用无符号二进制整数表示，量化单位0.1ms
        /// </summary>
        public string T { get; set; }
        /// <summary>
        /// 占4个字节，用无符号二进制整数表示，量化单位为360/2E32(°)
        /// </summary>
        public string A { get; set; }
        /// <summary>
        /// 占2个字节，用二进制整数补码表示，量化单位2.5v/2E15
        /// </summary>
        public string E { get; set; }
        /// <summary>
        /// 占2个字节，用二进制整数补码表示，量化单位2.5v/2E15
        /// </summary>
        public string DeltaA1 { get; set; }
        /// <summary>
        /// 占2个字节，用二进制整数补码表示，量化单位2.5v/2E15
        /// </summary>
        public string DeltaE1 { get; set; }
        /// <summary>
        /// 占2个字节，用二进制整数补码表示，量化单位为25分/2E15
        /// </summary>
        public string DeltaA2 { get; set; }
        /// <summary>
        /// 占2个字节，用二进制整数补码表示，量化单位为25分/2E15
        /// </summary>
        public string DeltaE2 { get; set; }
        /// <summary>
        /// 占2个字节，用二进制整数补码表示，量化单位0.01dBHz
        /// </summary>
        public string SPhi { get; set; }
        #endregion

        #region -Methods-
        /// <summary>
        /// 获取测角数据列表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<AE> GetListByDate(DateTime startDate, DateTime endDate)
        {
            DataSet ds = null;

                ds = new DataSet();
                ds.Tables.Add();
                OracleCommand command = _database.GetStoreProcCommand(GET_AEList_ByDate);
                if (startDate != DateTime.MinValue)
                {
                    _database.AddInParameter(command, "p_startDate", OracleDbType.Date, DBNull.Value);
                }
                else
                {
                    _database.AddInParameter(command, "p_startDate", OracleDbType.Date, startDate);
                }
                if (endDate != DateTime.MinValue)
                {
                    _database.AddInParameter(command, "p_endDate", OracleDbType.Date, DBNull.Value);
                }
                else
                {
                    _database.AddInParameter(command, "p_endDate", OracleDbType.Date, endDate);
                }
                using (IDataReader reader = _database.ExecuteReader(command))
                {
                    ds.Tables[0].Load(reader);
                }

                List<AE> objDatas = new List<AE>();
                if (ds != null && ds.Tables.Count == 1)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        objDatas.Add(new AE()
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
                            Reserve = dr["RESERVE"].ToString(),
                            ZT = dr["ZT"].ToString(),
                            T = dr["T"].ToString(),
                            A = dr["A"].ToString(),
                            E = dr["E"].ToString(),
                            DeltaA1 = dr["DeltaA1"].ToString(),
                            DeltaE1 = dr["DeltaE1"].ToString(),
                            DeltaA2 = dr["DeltaA2"].ToString(),
                            DeltaE2 = dr["DeltaE2"].ToString(),
                            SPhi = dr["SPhi"].ToString()
                        });
                    }
                }


                return objDatas;
        }

        /// <summary>
        /// Inserts a new record into database.
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
            _database.SpExecuteNonQuery(Insert, new OracleParameter[]{
                new OracleParameter("p_Version",this.Version),
                new OracleParameter("p_Flag",this.Flag),
                new OracleParameter("p_Maintype",this.MainType),
                new OracleParameter("p_datatype",this.DataType),
                new OracleParameter("p_Source",this.SourceAddress),
                new OracleParameter("p_Destination",this.DestinationAddress),
                new OracleParameter("p_missioncode",this.MissionCode),
                new OracleParameter("p_satellitecode",this.SatelliteCode),
                new OracleParameter("p_datadate",(DateTime)this.DataDate),
                new OracleParameter("p_datatime",this.DataTime),
                new OracleParameter("p_sequencenumber",this.SequenceNumber),
                new OracleParameter("p_childrenpacknumber",this.ChildrenPackNumber),
                new OracleParameter("p_udpReserve",this.UDPReserve),
                new OracleParameter("p_datalength",this.DataLength),
                new OracleParameter("p_dataclass",this.DataClass),
                new OracleParameter("p_Reserve",this.Reserve),
                new OracleParameter("p_Data_ZT",this.ZT),
                new OracleParameter("p_Data_T",this.T),
                new OracleParameter("p_Data_A",this.A),
                new OracleParameter("p_Data_E",this.E),
                new OracleParameter("p_Data_DeltaA1",this.DeltaA1),
                new OracleParameter("p_Data_DeltaE1",this.DeltaE1),
                new OracleParameter("p_Data_DeltaA2",this.DeltaA2),
                new OracleParameter("p_Data_DeltaE2",this.DeltaE2),
                new OracleParameter("p_Data_SPHI",this.SPhi),
                opId,
                p
            });
            if (opId.Value != null && opId.Value != DBNull.Value)
                this.Id = Convert.ToInt32(opId.Value.ToString());
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
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

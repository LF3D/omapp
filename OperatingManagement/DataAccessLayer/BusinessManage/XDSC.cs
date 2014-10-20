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
    /// 星地时差
    /// </summary>
    public class XDSC : BaseEntity<int, XDSC>
    {
        private static readonly string GET_XDSCList_ByDate = "up_xdsc_getlist";
        private static readonly string Insert = "up_xdsc_insert";

        /// <summary>
        /// Create a new instance of <see cref="SYCX"/> class.
        /// </summary>
        public XDSC()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }


        #region -Properties-
        private OracleDatabase _database = null;

        public DateTime CTime { get; set; }
        public string TaskID { get; set; }
        public string SatID { get; set; }
        public string Reserve { get; set; }

        /// <summary>
        /// 占2个字节，用无符号二进制整数表示，量化单位为1天，北京时2000年1月1日计为第1天
        /// </summary>
        public int D { get; set; }
        /// <summary>
        /// 占4个字节，用二进制整数补码表示，量化单位为0.1ms
        /// </summary>
        public int T { get; set; }

        /// <summary>
        /// 历元时刻
        /// </summary>
        public DateTime Times { get; set; }
        /// <summary>
        /// 占2个字节，用二进制整数补码表示
        /// </summary>
        public int N { get; set; }
        /// <summary>
        /// 用4字节二进制整数补码表示，量化单位为0.1ms
        /// </summary>
        public double DealtT { get; set; }
        public int DFInfoID { get; set; }
        #endregion

        #region -Methods-
        /// <summary>
        /// 获取星地时差数列表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<XDSC> GetListByDate(DateTime startDate, DateTime endDate)
        {
            DataSet ds = null;

            ds = new DataSet();
            ds.Tables.Add();
            OracleCommand command = _database.GetStoreProcCommand(GET_XDSCList_ByDate);
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

            List<XDSC> objDatas = new List<XDSC>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    objDatas.Add(new XDSC()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        TaskID = dr["TaskID"].ToString(),
                        SatID = dr["SatID"].ToString(),
                        CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        D = Convert.ToInt32(dr["D"].ToString()),
                        T = Convert.ToInt32(dr["T"].ToString()),
                        Times = Convert.ToDateTime(dr["Times"].ToString()),
                        N = Convert.ToInt32(dr["N"].ToString()),
                        DealtT = Convert.ToDouble(dr["DealtT"].ToString()),
                        Reserve = dr["Reserve"].ToString(),
                        DFInfoID = Convert.ToInt32(dr["DFInfoID"].ToString())
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
                new OracleParameter("p_TaskID",this.TaskID),
                new OracleParameter("p_SatID",this.SatID),
                new OracleParameter("p_CTime",DateTime.Now),
                new OracleParameter("p_D",this.D),
                new OracleParameter("p_T",this.T),
                new OracleParameter("p_Times",this.Times),
                new OracleParameter("p_N",this.N),
                new OracleParameter("p_DeltaT",this.DealtT),
                new OracleParameter("p_Reserve",this.Reserve),
                new OracleParameter("p_DFInfoID",this.DFInfoID),
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

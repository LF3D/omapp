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
    /// 起飞零点
    /// </summary>
    public class T0 : BaseEntity<int, T0>
    {
        private static readonly string GET_T0List_ByDate = "up_t0_getlist";
        private static readonly string Insert = "up_t0_insert";

        /// <summary>
        /// Create a new instance of <see cref="SYCX"/> class.
        /// </summary>
        public T0()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;
        public string TaskID { get; set; }
        public string SatID { get; set; }
        public DateTime CTime { get; set; }
        public string Reserve { get; set; }
        /// <summary>
        /// 占4个字节，二进制整数补码表示，量化单位为0.1ms
        /// </summary>
        public double TZero { get; set; }
        public int DFInfoID { get; set; }
        #endregion

        #region -Methods-
        /// <summary>
        /// 获取起飞零点列表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<T0> GetListByDate(DateTime startDate, DateTime endDate)
        {
            DataSet ds = null;

            ds = new DataSet();
            ds.Tables.Add();
            OracleCommand command = _database.GetStoreProcCommand(GET_T0List_ByDate);
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

            List<T0> objDatas = new List<T0>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    objDatas.Add(new T0()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        TaskID = dr["TaskID"].ToString(),
                        SatID = dr["SatID"].ToString(),
                        CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        TZero = Convert.ToDouble(dr["T0"].ToString()),
                        DFInfoID = Convert.ToInt32(dr["DFInfoID"].ToString()),
                        Reserve = dr["Reserve"].ToString()
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
                new OracleParameter("p_TaskID", this.TaskID),
                new OracleParameter("p_SatID", this.SatID),
                new OracleParameter("p_CTime", DateTime.Now),
                new OracleParameter("p_Reserve", this.Reserve),
                new OracleParameter("p_T0", this.TZero),
                new OracleParameter("p_DFInfoID", this.DFInfoID),
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

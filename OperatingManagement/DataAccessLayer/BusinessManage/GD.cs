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
    /// 轨道信息数据
    /// </summary>
    public class GD : BaseEntity<int, GD>
    {
        private static readonly string GET_OribitalQuantityList_ByDate = "up_gd_getlist";
        private static readonly string Insert = "up_gd_insert";
        private static readonly string SelectByID = "up_gd_selectByID";

        /// <summary>
        /// Create a new instance of <see cref="GD"/> class.
        /// </summary>
        public GD()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;

        public DateTime CTime { get; set; }
        /// <summary>
        /// 任务代号
        /// </summary>
        public string TaskID { get; set; }
        public string TaskName { get; set; }
        /// <summary>
        /// 卫星ID
        /// </summary>
        public string SatID { get; set; }
        /// <summary>
        /// 卫星名称
        /// </summary>
        public string SatellteName { get; set; }
        public string DataName { get; set; }
        public string ICode { get; set; }
        /// <summary>
        /// 占2个字节，用无符号二进制整数表示，
        /// 量化单位为1天，北京时2000年1月1日计为第1天
        /// </summary>
        public int D { get; set; }
        /// <summary>
        /// 占4个字节，用无符号二进制整数表示，
        /// 量化单位为0.1ms
        /// </summary>
        public int T { get; set; }
        public DateTime Times { get; set; }
        /// <summary>
        /// 4个字节，用无符号二进制整数表示，
        /// 量化单位为0.1m
        /// </summary>
        public double A { get; set; }
        /// <summary>
        /// 占4个字节，用无符号二进制整数表示，量化单位 为2E-31
        /// </summary>
        public double E { get; set; }
        /// <summary>
        /// 占4个字节，用无符号二进制整数表示，量化单位为2E-24度
        /// </summary>
        public double I { get; set; }
        /// <summary>
        /// 占4个字节，用无符号二进制整数表示，量化单位为2E-22度
        /// </summary>
        public double Q { get; set; }
        /// <summary>
        /// 占4个字节，用无符号二进制整数表示，量化单位为2E-22度
        /// </summary>
        public double W { get; set; }
        /// <summary>
        /// 占4个字节，用无符号二进制整数表示，量化单位为2E-22度
        /// </summary>
        public double M { get; set; }
        /// <summary>
        /// 占4个字节，用二进制整数补码表示，量化单位为2E-20分钟
        /// </summary>
        public double P { get; set; }
        /// <summary>
        /// 占4个字节，用二进制整数补码表示，量化单位为2E-20秒/天
        /// </summary>
        public double PP { get; set; }
        /// <summary>
        /// 4个字节，用无符号二进制整数表示，量化单位为0.1m
        /// </summary>
        public double Ra { get; set; }
        /// <summary>
        /// 4个字节，用无符号二进制整数表示，量化单位为0.1m
        /// </summary>
        public double Rp { get; set; }
        /// <summary>
        /// 占3个字节，无符号二进制整数表示，量化单位为2E-16米2/千克
        /// </summary>
        public double CDSM { get; set; }
        /// <summary>
        /// 占3个字节，无符号二进制整数表示，量化单位为2E-16米2/千克
        /// </summary>
        public double KSM { get; set; }
        /// <summary>
        /// 各占4个字节，备用；700任务不用，固定填“0”
        /// </summary>
        public double KZ1 { get; set; }
        /// <summary>
        /// 各占4个字节，备用；700任务不用，固定填“0”
        /// </summary>
        public double KZ2 { get; set; }
        public string Reserve { get; set; }
        public int DFInfoID { get; set; }
        #endregion 

        #region -Methods-
        /// <summary>
        /// 根据时间获取轨道根数列表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="taskid"></param>
        /// <param name="itype"></param>
        /// <returns></returns>
        public List<GD> GetList(DateTime startDate, DateTime endDate,string outTaskid, string icode)
        {

            object oBeginTime = null;
            object oEndTime = null;
            string taskID = string.Empty;
            string satID = string.Empty;
            new Task().GetTaskNoSatID(outTaskid, out taskID, out satID);

            if (startDate == DateTime.MinValue)
                oBeginTime = DBNull.Value;
            else
                oBeginTime = startDate;
            if (endDate == DateTime.MinValue)
                oEndTime = DBNull.Value;
            else
                oEndTime = endDate;

            DataSet ds = null;
            OracleParameter p = PrepareRefCursor();

            ds = _database.SpExecuteDataSet("up_GD_Getlist", new OracleParameter[]{
                new OracleParameter("p_startDate", oBeginTime),
                new OracleParameter("p_endDate", oEndTime),
                new OracleParameter("p_taskID", taskID), 
                new OracleParameter("p_satID", satID), 
                new OracleParameter("p_iCode", icode), 
                p
            });

            List<GD> objDatas = new List<GD>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    objDatas.Add(new GD()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        TaskID = dr["TaskID"].ToString(),
                        TaskName = dr["TaskName"].ToString(),
                        SatID = dr["Satid"].ToString(),
                        SatellteName = dr["WXMC"].ToString(),
                        DataName = dr["DataName"].ToString(),
                        ICode = dr["icode"].ToString(),
                        D = Convert.ToInt32(dr["D"].ToString()),
                        T = Convert.ToInt32(dr["T"].ToString()),
                        Times = Convert.ToDateTime(dr["Times"].ToString()),
                        A = Convert.ToDouble(dr["A"].ToString()),
                        E = Convert.ToDouble(dr["E"].ToString()),
                        I = Convert.ToDouble(dr["I"].ToString()),
                        Q = Convert.ToDouble(dr["Q"].ToString()),
                        W = Convert.ToDouble(dr["W"].ToString()),
                        M = Convert.ToDouble(dr["M"].ToString()),
                        P = Convert.ToDouble(dr["P"].ToString()),
                        PP = Convert.ToDouble(dr["PP"].ToString()),
                        Ra = Convert.ToDouble(dr["Ra"].ToString()),
                        Rp = Convert.ToDouble(dr["Rp"].ToString()),
                        CDSM = Convert.ToDouble(dr["CDSM"].ToString()),
                        KSM = Convert.ToDouble(dr["KSM"].ToString()),
                        KZ1 = Convert.ToDouble(dr["KZ1"].ToString()),
                        KZ2 = Convert.ToDouble(dr["KZ2"].ToString()),
                        Reserve = dr["Reserve"].ToString(),
                        DFInfoID = Convert.ToInt32(dr["DFInfoID"].ToString())
                    });
                }
            }
            return objDatas;
        }
        /// <summary>
        /// 根据时间获取轨道根数列表
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<GD> GetListByDate(DateTime startDate, DateTime endDate)
        {
            DataSet ds = null;

            ds = new DataSet();
            ds.Tables.Add();
            OracleCommand command = _database.GetStoreProcCommand(GET_OribitalQuantityList_ByDate);
            if (startDate == DateTime.MinValue)
            {
                _database.AddInParameter(command, "p_startDate", OracleDbType.Date, DBNull.Value);
            }
            else
            {
                _database.AddInParameter(command, "p_startDate", OracleDbType.Date, startDate);
            }
            if (endDate == DateTime.MinValue)
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

            List<GD> objDatas = new List<GD>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    objDatas.Add(new GD()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        TaskID = dr["TaskID"].ToString(),
                        TaskName = dr["TaskName"].ToString(),
                        SatID = dr["Satid"].ToString(),
                        SatellteName = dr["WXMC"].ToString(),
                        DataName = dr["DataName"].ToString(),
                        ICode = dr["icode"].ToString(),
                        D = Convert.ToInt32(dr["D"].ToString()),
                        T = Convert.ToInt32(dr["T"].ToString()),
                        Times = Convert.ToDateTime(dr["Times"].ToString()),
                        A = Convert.ToDouble(dr["A"].ToString()),
                        E = Convert.ToDouble(dr["E"].ToString()),
                        I = Convert.ToDouble(dr["I"].ToString()),
                        Q = Convert.ToDouble(dr["Q"].ToString()),
                        W = Convert.ToDouble(dr["W"].ToString()),
                        M = Convert.ToDouble(dr["M"].ToString()),
                        P = Convert.ToDouble(dr["P"].ToString()),
                        PP = Convert.ToDouble(dr["PP"].ToString()),
                        Ra = Convert.ToDouble(dr["Ra"].ToString()),
                        Rp = Convert.ToDouble(dr["Rp"].ToString()),
                        CDSM = Convert.ToDouble(dr["CDSM"].ToString()),
                        KSM = Convert.ToDouble(dr["KSM"].ToString()),
                        KZ1 = Convert.ToDouble(dr["KZ1"].ToString()),
                        KZ2 = Convert.ToDouble(dr["KZ2"].ToString()),
                        Reserve = dr["Reserve"].ToString(),
                        DFInfoID = Convert.ToInt32(dr["DFInfoID"].ToString())
                    });
                }
            }


            return objDatas;
        }
        /// <summary>
        /// Selects the specific GD by identification.
        /// </summary>
        /// <returns>GD</returns>
        public GD SelectById()
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
                    return new GD()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        TaskID = dr["TaskID"].ToString(),
                       TaskName = dr["TaskName"].ToString(),
                        SatID = dr["Satid"].ToString(),
                        SatellteName = dr["WXMC"].ToString(),
                        DataName = dr["DataName"].ToString(),
                        ICode = dr["icode"].ToString(),
                        D = Convert.ToInt32(dr["D"].ToString()),
                        T = Convert.ToInt32(dr["T"].ToString()),
                        Times = Convert.ToDateTime(dr["Times"].ToString()),
                        A = Convert.ToDouble(dr["A"].ToString()),
                        E = Convert.ToDouble(dr["E"].ToString()),
                        I = Convert.ToDouble(dr["I"].ToString()),
                        Q = Convert.ToDouble(dr["Q"].ToString()),
                        W = Convert.ToDouble(dr["W"].ToString()),
                        M = Convert.ToDouble(dr["M"].ToString()),
                        P = Convert.ToDouble(dr["P"].ToString()),
                        PP = Convert.ToDouble(dr["PP"].ToString()),
                        Ra = Convert.ToDouble(dr["Ra"].ToString()),
                        Rp = Convert.ToDouble(dr["Rp"].ToString()),
                        CDSM = Convert.ToDouble(dr["CDSM"].ToString()),
                        KSM = Convert.ToDouble(dr["KSM"].ToString()),
                        KZ1 = Convert.ToDouble(dr["KZ1"].ToString()),
                        KZ2 = Convert.ToDouble(dr["KZ2"].ToString()),
                        Reserve = dr["Reserve"].ToString(),
                        DFInfoID = Convert.ToInt32(dr["DFInfoID"].ToString())
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// 单个或多个ID，如("1,2,3")
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<GD> SelectByIDS(string ids)
        {
            OracleParameter p = PrepareRefCursor();
            DataSet ds = _database.SpExecuteDataSet("up_gd_selectinids", new OracleParameter[]{
                new OracleParameter("p_ids",ids),
                p
            });
            List<GD> jhs = new List<GD>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    jhs.Add(new GD()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        TaskID = dr["TaskID"].ToString(),
                        //TaskName = dr["TaskName"].ToString(),
                        SatID = dr["Satid"].ToString(),
                        //SatellteName = dr["WXMC"].ToString(),
                        DataName = dr["DataName"].ToString(),
                        ICode = dr["icode"].ToString(),
                        D = Convert.ToInt32(dr["D"].ToString()),
                        T = Convert.ToInt32(dr["T"].ToString()),
                        Times = Convert.ToDateTime(dr["Times"].ToString()),
                        A = Convert.ToDouble(dr["A"].ToString()),
                        E = Convert.ToDouble(dr["E"].ToString()),
                        I = Convert.ToDouble(dr["I"].ToString()),
                        Q = Convert.ToDouble(dr["Q"].ToString()),
                        W = Convert.ToDouble(dr["W"].ToString()),
                        M = Convert.ToDouble(dr["M"].ToString()),
                        P = Convert.ToDouble(dr["P"].ToString()),
                        PP = Convert.ToDouble(dr["PP"].ToString()),
                        Ra = Convert.ToDouble(dr["Ra"].ToString()),
                        Rp = Convert.ToDouble(dr["Rp"].ToString()),
                        CDSM = Convert.ToDouble(dr["CDSM"].ToString()),
                        KSM = Convert.ToDouble(dr["KSM"].ToString()),
                        KZ1 = Convert.ToDouble(dr["KZ1"].ToString()),
                        KZ2 = Convert.ToDouble(dr["KZ2"].ToString()),
                        Reserve = dr["Reserve"].ToString(),
                        DFInfoID = Convert.ToInt32(dr["DFInfoID"].ToString())
                    });
                }
            }
            return jhs;
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
                new OracleParameter("p_CTime",DateTime.Now),
                new OracleParameter("p_Taskid",this.TaskID),
                new OracleParameter("p_Satid",this.SatID),
                new OracleParameter("p_ICode",this.ICode),
                new OracleParameter("p_D",this.D),
                new OracleParameter("p_T",this.T),
                new OracleParameter("p_Times",this.Times),
                new OracleParameter("p_A",this.A),
                new OracleParameter("p_E",this.E),
                new OracleParameter("p_I",this.I),
                new OracleParameter("p_Q",this.Q),
                new OracleParameter("p_W",this.W),
                new OracleParameter("p_M",this.M),
                new OracleParameter("p_P",this.P),
                new OracleParameter("p_PP",this.PP),
                new OracleParameter("p_RA",this.Ra),
                new OracleParameter("p_RP",this.Rp),
                new OracleParameter("p_CDSM",this.CDSM),
                new OracleParameter("p_KSM",this.KSM),
                new OracleParameter("p_KZ1",this.KZ1),
                new OracleParameter("p_KZ2",this.KZ2),
                new OracleParameter("p_Reserve",this.Reserve),
                new OracleParameter("p_DFInfid",this.DFInfoID),
                opId,
                p
            });
            if (opId.Value != null && opId.Value != DBNull.Value)
                this.Id = Convert.ToInt32(opId.Value.ToString());
            return (FieldVerifyResult)Convert.ToInt32(p.Value.ToString());
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

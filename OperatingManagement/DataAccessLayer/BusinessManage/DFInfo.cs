using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using Oracle.DataAccess.Client;
using System.Data;

namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    [Serializable]
    public class DFInfo : BaseEntity<int, DFInfo>
    {
        public DFInfo()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;
        private string s_up_dfinfo_insert = "up_dfinfo_insert";
        private string s_up_dfinfo_update = "up_dfinfo_update";
        private string s_up_dfinfo_select = "up_dfinfo_select";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CTime { get; set; }
        /// <summary>
        /// 任务代号
        /// </summary>
        public string TaskID { get; set; }
        /// <summary>
        /// 卫星代号
        /// </summary>
        public string SatID { get; set; }
        /// <summary>
        /// 数据主类别
        /// </summary>
        public string MainType { get; set; }
        /// <summary>
        /// 数据次类别
        /// </summary>
        public string SecondType { get; set; }
        /// <summary>
        /// 信息类别
        /// </summary>
        public int InfoTypeID { get; set; }
        /// <summary>
        /// 信源
        /// </summary>
        public int Source { get; set; }
        /// <summary>
        /// 信宿
        /// </summary>
        public int Destination { get; set; }
        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime DataTime { get; set; }
        /// <summary>
        /// 数据报顺序号
        /// </summary>
        public int SequenceNO { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public byte[] DBlob { get; set; }
        /// <summary>
        /// 数据长度
        /// </summary>
        public int DataLength { get; set; }
        /// <summary>
        /// 数据帧方向，发送还是接收：0-发送，1-接收
        /// </summary>
        public int Direction { get; set; }
        /// <summary>
        /// 状态，0-初始状态；1-发送成功；2-接收已归档
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #endregion

        #region -Methods-

        /// <summary>
        /// Selects the specific DFInfo by identification.
        /// </summary>
        /// <returns>YDSJ</returns>
        public DFInfo SelectById()
        {
            OracleParameter p = PrepareRefCursor();

            DataSet ds = _database.SpExecuteDataSet(s_up_dfinfo_select, new OracleParameter[]{
                new OracleParameter("p_Id", this.Id), 
                p
            });

            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return new DFInfo()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = DateTime.Parse(dr["CTIME"].ToString()),
                        TaskID = dr["TaskID"].ToString(),
                        SatID = dr["SatID"].ToString(),
                        MainType = dr["MainType"].ToString(),
                        SecondType = dr["SecondType"].ToString(),
                        InfoTypeID = Convert.ToInt32(dr["InfoTypeID"].ToString()),
                        Source = Convert.ToInt32(dr["Source"].ToString()),
                        Destination = Convert.ToInt32(dr["Destination"].ToString()),
                        DataTime = Convert.ToDateTime(dr["DataTime"].ToString()),
                        SequenceNO = Convert.ToInt32(dr["SequenceNO"].ToString()),
                        DBlob = (byte[])dr["DBlob"],
                        DataLength = Convert.ToInt32(dr["DataLength"].ToString()),
                        Direction = Convert.ToInt32(dr["Direction"].ToString()),
                        Status = Convert.ToInt32(dr["Status"].ToString()),
                        Remark = dr["Remark"].ToString()
                    };
                }
            }
            return null;
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
            _database.SpExecuteNonQuery(s_up_dfinfo_insert, new OracleParameter[]{
                new OracleParameter("p_CTime", DateTime.Now),
                new OracleParameter("p_TaskID", this.TaskID),
                new OracleParameter("p_SatID", this.SatID),
                new OracleParameter("p_MainType", this.MainType),
                new OracleParameter("p_SecondType", this.SecondType),
                new OracleParameter("p_InfoTypeID", this.InfoTypeID),
                new OracleParameter("p_Source", this.Source),
                new OracleParameter("p_Destination", this.Destination),
                new OracleParameter("p_DataTime", this.DataTime),
                new OracleParameter("p_SequeceNO", this.SequenceNO),
                new OracleParameter("p_DBlob", this.DBlob),
                new OracleParameter("p_DataLength", this.DataLength),
                new OracleParameter("p_Direction", this.Direction),
                new OracleParameter("p_Status", this.Status),
                new OracleParameter("p_Remark", this.Remark),
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

            _database.SpExecuteNonQuery(s_up_dfinfo_update, new OracleParameter[]{
                    new OracleParameter("p_RID", this.Id),
                    new OracleParameter("p_Status", this.Status),
                    p
                });
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

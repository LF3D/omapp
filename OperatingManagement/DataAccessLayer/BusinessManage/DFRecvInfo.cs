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
    public class DFRecvInfo : BaseEntity<int, DFRecvInfo>
    {
        public DFRecvInfo()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        #region -Properties-
        private OracleDatabase _database = null;
        private string s_up_dfrecvinfo_insert = "up_dfrecvinfo_insert";
        private string s_up_dfrecvinfo_update = "up_dfrecvinfo_update";
        private string s_up_dfrecvinfo_select = "up_dfrecvinfo_select";

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
        /// 接收开始时间
        /// </summary>
        public DateTime BTime { get; set; }
        /// <summary>
        /// 接收结束时间
        /// </summary>
        public DateTime ETime { get; set; }
        /// <summary>
        /// 总包数
        /// </summary>
        public int TotalPack { get; set; }
        /// <summary>
        /// 错包数
        /// </summary>
        public int ErrorPack { get; set; }
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
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize { get; set; }
        /*
        /// <summary>
        /// 文件原始内容
        /// </summary>
        public byte[] FBlob { get; set; }
         */
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #endregion

        #region -Methods-

        /// <summary>
        /// Selects the specific DFRecvInfo by identification.
        /// </summary>
        /// <returns>YDSJ</returns>
        public DFRecvInfo SelectById()
        {/*
            OracleParameter p = PrepareRefCursor();

            DataSet ds = _database.SpExecuteDataSet(s_up_dfrecvinfo_select, new OracleParameter[]{
                new OracleParameter("p_Id", this.Id), 
                p
            });

            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    return new DFRecvInfo()
                    {
                        Id = Convert.ToInt32(dr["ID"].ToString()),
                        CTime = DateTime.Parse(dr["CTIME"].ToString()),
                        TaskID = dr["TaskID"].ToString(),
                        SatID = dr["SatName"].ToString(),
                        MainType = dr["MainType"].ToString(),
                        SecondType = dr["SecondType"].ToString(),
                        InfoTypeID = Convert.ToInt32(dr["InfoTypeID"].ToString()),
                        Source = Convert.ToInt32(dr["Source"].ToString()),
                        Destination = Convert.ToInt32(dr["Destination"].ToString()),
                        DataTime = Convert.ToDateTime(dr["DataTime"].ToString()),
                        SequeceNO = Convert.ToInt32(dr["SequeceNO"].ToString()),
                        DBlob = Utility.StringToByte(dr["DBlob"].ToString()),
                        DataLength = Convert.ToInt32(dr["DataLength"].ToString()),
                        Direction = Convert.ToInt32(dr["Direction"].ToString()),
                        Status = Convert.ToInt32(dr["Status"].ToString()),
                        Remark = dr["RESERVE"].ToString()
                    };
                }
            }*/
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
            _database.SpExecuteNonQuery(s_up_dfrecvinfo_insert, new OracleParameter[]{
                new OracleParameter("p_CTime", DateTime.Now),
                new OracleParameter("p_TaskID", this.TaskID),
                new OracleParameter("p_SatID", this.SatID),
                new OracleParameter("p_BTime", this.BTime),
                new OracleParameter("p_ETime", this.ETime),
                new OracleParameter("p_TotalPack", this.TotalPack),
                new OracleParameter("p_ErrorPack", this.ErrorPack),
                new OracleParameter("p_InfoTypeID", this.InfoTypeID),
                new OracleParameter("p_Source", this.Source),
                new OracleParameter("p_Destination", this.Destination),
                new OracleParameter("p_FileName", this.FileName),
                new OracleParameter("p_FileSize", this.FileSize),
                //new OracleParameter("p_FBlob", this.FBlob),
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

            _database.SpExecuteNonQuery(s_up_dfrecvinfo_update, new OracleParameter[]{
                    new OracleParameter("p_RID", this.Id),
                    new OracleParameter("p_ETime", this.ETime),
                    new OracleParameter("p_TotalPack", this.TotalPack),
                    new OracleParameter("p_ErrorPack", this.ErrorPack),
                    new OracleParameter("p_FileSize", this.FileSize),
                    new OracleParameter("p_FileSize", this.FileName),
                    //new OracleParameter("p_FBlob", this.FBlob),
                    new OracleParameter("p_Remark", this.Remark),
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

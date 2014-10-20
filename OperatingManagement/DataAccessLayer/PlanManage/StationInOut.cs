using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using System.Data;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.PlanManage
{
    /// <summary>
    /// 进出站及航捷数据统计文件
    /// </summary>
    public class StationInOut 
    {
        /// <summary>
        /// Create a new instance of <see cref="StationInOut"/> class.
        /// </summary>
        public StationInOut()
        {
        }


        #region -Properties-
        private OracleDatabase _database = null;
        /// <summary>
        /// 行号
        /// </summary>
        public int rowIndex { get; set; }
        /// <summary>
        /// 站名
        /// </summary>
        public string ZM { get; set; }
        /// <summary>
        /// 本次预报时段中第N次进站
        /// </summary>
        public string N { get; set; }
        /// <summary>
        /// 圈次
        /// </summary>
        public string QC { get; set; }
        /// <summary>
        /// 升降轨标志，1——升轨（由南往北），0——降轨（由北往南），2——有升有降
        /// </summary>
        public string SJG { get; set; }
        /// <summary>
        /// 跟踪时长（跟踪结束-跟踪开始时间），秒
        /// </summary>
        public string SP1 { get; set; }
        /// <summary>
        /// 超过最高仰角的时长，如果本圈次航捷角没有超过最高仰角，添0，秒
        /// </summary>
        public string SP2 { get; set; }
        /// <summary>
        /// 跟踪开始时间
        /// </summary>
        public string T1 { get; set; }
        /// <summary>
        /// 任务开始时间
        /// </summary>
        public string T2 { get; set; }
        /// <summary>
        /// 到最高仰角时间
        /// </summary>
        public string T3 { get; set; }
        /// <summary>
        /// 航捷时间
        /// </summary>
        public string T4 { get; set; }
        /// <summary>
        /// 出最高仰角时间
        /// </summary>
        public string T5 { get; set; }
        /// <summary>
        /// 任务结束时间
        /// </summary>
        public string T6 { get; set; }
        /// <summary>
        /// 跟踪结束时间
        /// </summary>
        public string T7 { get; set; }
        /// <summary>
        /// 航捷角
        /// </summary>
        public string h { get; set; }

        /// <summary>
        /// 文件索引
        /// </summary>
        public string FileIndex { get; set; }
        #endregion

        #region -Methods-
        /// <summary>
        /// 读取进出站及航捷数据统计文件列表
        /// </summary>
        /// <returns></returns>
        public List<StationInOut> GetSYStationInOutList()
        {
            DataSet ds = null;

            ds = new DataSet();
            ds.Tables.Add();

            List<StationInOut> objDatas = new List<StationInOut>();
            if (ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    objDatas.Add(new StationInOut()
                    {
                        //ID = Convert.ToInt32(dr["ID"].ToString()),
                        //CTime = Convert.ToDateTime(dr["CTIME"].ToString()),
                        //TaskID = dr["taskid"].ToString(),
                        //PlanType = dr["plantype"].ToString(),
                        //PlanID = Convert.ToInt32(dr["PlanID"].ToString()),
                        //StartTime = Convert.ToDateTime(dr["StartTime"].ToString()),
                        //EndTime = Convert.ToDateTime(dr["EndTime"].ToString()),
                        //SRCType = Convert.ToInt32(dr["SRCType"].ToString()),
                        //SRCID = Convert.ToInt32(dr["SRCID"].ToString()),
                        //FileIndex = dr["FileIndex"].ToString(),
                        //Reserve = dr["Reserve"].ToString()
                    });
                }
            }
            return objDatas;
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public StationInOut Read()
        {
            StationInOut station = new StationInOut();
            if (!string.IsNullOrEmpty(FileIndex))
            {

            }
            return station;
        }

        #endregion


    }
}

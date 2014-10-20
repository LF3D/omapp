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
    /// 仿真推演试验数据
    /// </summary>
    public class TYSJ
    {
        #region -Properties-

        public int ID { get; set; }
        public DateTime CTime { get; set; }
        public string TaskID { get; set; }
        public string FileIndex { get; set; }
        ///// <summary>
        ///// 卫星名称
        ///// </summary>
        //public string SatName { get; set; }
        ///// <summary>
        ///// 试验类别
        ///// </summary>
        //public string Type { get; set; }
        ///// <summary>
        ///// 试验项目
        ///// </summary>
        //public string TestItem { get; set; }
        ///// <summary>
        ///// 计划开始时间
        ///// </summary>
        //public string StartTime { get; set; }
        ///// <summary>
        ///// 计划结束时间
        ///// </summary>
        //public string EndTime { get; set; }
        ///// <summary>
        ///// 试验条件
        ///// </summary>
        //public string Condition { get; set; }
        public string SatID { get; set; }

        public List<TYSJ_Content> SYContents { get; set; }
        #endregion
    }

    public class TYSJ_Content
    {
        #region -Properties-

        /// <summary>
        /// 卫星名称
        /// </summary>
        public string SatName { get; set; }
        /// <summary>
        /// 试验类别
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 试验项目
        /// </summary>
        public string TestItem { get; set; }
        /// <summary>
        /// 试验开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 试验结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 试验条件
        /// </summary>
        public string Condition { get; set; }
        #endregion
    }
}

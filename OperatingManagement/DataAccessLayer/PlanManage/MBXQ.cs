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
    /// 空间目标信息需求
    /// </summary>
    public class MBXQ
    {
        #region -Properties-
        /// <summary>
        /// 用户名称
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 需求制定时间
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 目标信息标志
        /// </summary>
        public string TargetInfo { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string  TimeSection1{ get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string  TimeSection2{ get; set; }
        /// <summary>
        /// 信息条数
        /// </summary>
        public string  Sum{ get; set; }

        public List<MBXQSatInfo> SatInfos
        {
            get;
            set;
        }
        #endregion
    }

    /// <summary>
    /// 空间目标信息需求-卫星信息
    /// </summary>
    public class MBXQSatInfo 
    { 
        #region -Properties-
        /// <summary>
        /// 卫星名称
        /// </summary>
        public string SatName { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string InfoName { get; set; }
        /// <summary>
        /// 提供时间
        /// </summary>
        public string InfoTime { get; set; }
        #endregion
    }
}

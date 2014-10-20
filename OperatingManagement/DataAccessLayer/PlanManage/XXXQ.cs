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
    /// 空间环境信息
    /// </summary>
    public class XXXQ
    {
        #region -Properties-
        public DateTime CTime { get; set; }
        public string TaskID { get; set; }
        public string SatID { get; set; }
        public MBXQ objMBXQ { get; set; }
        public HJXQ objHJXQ { get; set; }
        #endregion


    }
}

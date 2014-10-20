#region
//------------------------------------------------------
//Assembly:OperatingManagement.DataAccessLayer
//FileName:CutSubItemInfo.cs
//Remark:通信资源管理类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120602    Create     
//------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OperatingManagement.Framework.Basic;

namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    [Serializable]
    public class CutSubItemInfo
    {
        public CutSubItemInfo()
        {
        }

        #region Properties
        /// <summary>
        /// Item Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 卫星名称
        /// </summary>
        public string SatelliteName { get; set; }
        /// <summary>
        /// 卫星编号
        /// </summary>
        public string SatelliteNO { get; set; } 
        /// <summary>
        /// 历元时刻
        /// </summary>
        public string LYSK { get; set; }
        /// <summary>
        /// KK
        /// </summary>
        public string KK { get; set; }
        /// <summary>
        /// D1
        /// </summary>
        public string D1 { get; set; }
        /// <summary>
        /// D2
        /// </summary>
        public string D2 { get; set; }
        /// <summary>
        /// D3
        /// </summary>
        public string D3 { get; set; }
        /// <summary>
        /// D4
        /// </summary>
        public string D4 { get; set; }
        /// <summary>
        /// D5
        /// </summary>
        public string D5 { get; set; }
        /// <summary>
        /// D6
        /// </summary>
        public string D6 { get; set; }
        /// <summary>
        /// Sm
        /// </summary>
        public string Sm { get; set; }
        /// <summary>
        /// Ref
        /// </summary>
        public string Ref { get; set; }
        #endregion
    }
}

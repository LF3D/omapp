using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    /// <summary>
    /// 应答信息
    /// </summary>
    public class HD
    {
        #region -Properties-
        /// <summary>
        /// 占4个字节
        /// </summary>
        public string Bj { get; set; }
        /// <summary>
        /// 判断结果，(33)H表示错误，要求重传；(CC)H表示正确
        /// </summary>
        public string JG { get; set; }
        #endregion
    }
}

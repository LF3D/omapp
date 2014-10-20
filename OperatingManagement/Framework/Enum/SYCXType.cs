using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Framework
{
    /// <summary>
    /// Enumeration of SYCX's type.
    /// </summary>
    public enum SYCXType : int
    {
        /// <summary>
        /// It means normal user.
        /// </summary>
        Nominal = 1,
        /// <summary>
        /// It means administrative.
        /// </summary>
        Formal = 2
    }
}

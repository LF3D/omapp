using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Framework
{
    /// <summary>
    /// Enumeration of user's type.
    /// </summary>
    public enum UserType : int
    {
        /// <summary>
        /// It means normal user.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// It means administrative.
        /// </summary>
        Admin = 1,
        /// <summary>
        /// Just EveryOne.
        /// </summary>
        EveryOne = 2
    }
}

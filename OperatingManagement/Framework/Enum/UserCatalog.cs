using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Framework
{
    /// <summary>
    /// Enumeration of user's catalog.
    /// </summary>
    public enum UserCatalog : int
    {
        /// <summary>
        /// It means inside user.
        /// </summary>
        Inside = 0,
        /// <summary>
        /// It means outside user.
        /// </summary>
        Outside = 1,
        /// <summary>
        /// Ite means temporary user.
        /// </summary>
        Temporary = 2
    }
}

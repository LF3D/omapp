using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Framework
{
    /// <summary>
    /// Enumeration of the field's status.
    /// </summary>
    public enum FieldStatus : int
    {
        /// <summary>
        /// It means active.
        /// </summary>
        Active = 0,
        /// <summary>
        /// It means inactive.
        /// </summary>
        Inactive = 1,
        /// <summary>
        /// It means locked.
        /// </summary>
        Locked = 2,
        /// <summary>
        /// It means deletion.
        /// </summary>
        Deleted = 3
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Framework
{
    /// <summary>
    /// The format type of password
    /// </summary>
    public enum PasswordFormat
    {
        /// <summary>
        /// without encode.
        /// </summary>
        Clear = 0,

        /// <summary>
        /// encode the password with hashed
        /// </summary>
        Hashed = 1
    }
}

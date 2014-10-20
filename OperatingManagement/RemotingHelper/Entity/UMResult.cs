using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.RemotingHelper
{
    /// <summary>
    /// search result about user and role
    /// </summary>
    public class UMResult
    {
        /// <summary>
        /// return messages in process
        /// "",  means normal
        /// otherwise, exception
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// needed users
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// needed roles
        /// </summary>
        public List<Role> Roles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace OperatingManagement.RemotingObjectInterface
{
    /// <summary>
    /// Public interfaces for Account management.
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// This method is used to validate user status by user name and password, and returns 
        /// with the formated XML Data.
        /// </summary>
        /// <param name="userName">The name(LoginName) of user which to be validated.</param>
        /// <param name="password">The password of user which to be validated.</param>
        /// <returns></returns>
        string ValidateUser(string userName, string password);

        string GetAllRoles();

        string GetUsersByRoleID(int roleID);

        string GetAllUsers();

        string GetUserByID(int id);
    }
}

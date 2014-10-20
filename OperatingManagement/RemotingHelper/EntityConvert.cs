using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OperatingManagement.RemotingHelper
{
    /// <summary>
    /// Implements method to convert xml string to Entity object.
    /// </summary>
    public class EntityConvert
    {
        /// <summary>
        /// Converts xml string to AuthStatus object.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static AuthStatus ToAuthStatus(string xml)
        {
            XElement root = XElement.Parse(xml);
            AuthStatus status = new AuthStatus()
            {
                Code = Convert.ToInt32(root.Element("code").Value),
                Message = root.Element("msg").Value,
            };
            if (status.Code == 5)
            {
                var user = root.Element("user");
                if (user != null)
                {
                    User u = new User()
                    {
                        CreatedTime = Convert.ToDateTime(user.Element("createdTime").Value),
                        DisplayName = user.Element("displayName").Value,
                        State = Convert.ToInt32(user.Element("state").Value),
                        UserCatalog = Convert.ToInt32(user.Element("userCatalog").Value),
                        UserType = Convert.ToInt32(user.Element("userType").Value),
                        Id = Convert.ToDouble(user.Element("id").Value),
                        Mobile = user.Element("mobile").Value,
                        LoginName = user.Element("loginName").Value
                    };
                    status.User = u;
                }

                var permissions = root.Element("permissions").Elements("permission");
                if (permissions != null && permissions.Count() > 0)
                {
                    status.Permissions = (from q in permissions
                                          select new Permission()
                                          {
                                              Id = Convert.ToDouble(q.Element("id").Value),
                                              Module = new Module()
                                              {
                                                  Id = Convert.ToDouble(q.Element("module").Element("id").Value),
                                                  Name = q.Element("module").Element("name").Value
                                              },
                                              Task = new Task()
                                              {
                                                  Id = Convert.ToDouble(q.Element("task").Element("id").Value),
                                                  Name = q.Element("task").Element("name").Value
                                              }
                                          }).ToList();
                }

                var roles = root.Element("roles").Elements("role");
                if (roles != null && roles.Count() > 0)
                {
                    status.Roles = (from q in roles
                                    select new Role()
                                    {
                                        Id = Convert.ToDouble(q.Element("id").Value),
                                        Name = q.Element("name").Value
                                    }).ToList();
                }
            }
            return status;
        }

        public static UMResult ToResult(string xml)
        {
            XElement root = XElement.Parse(xml);
            UMResult oResult = new UMResult()
            {
                Msg = root.Element("msg").Value,
            };

            if (oResult.Msg == "")
            {
                //get users
                var users = root.Elements("user");
                if (users != null && users.Count() > 0)
                {
                    oResult.Users = new List<User>();
                    User oUser;
                    for (int i = 0; i< users.Count();i++)
                    {
                        oUser = new User();
                        oUser.Id = Convert.ToInt32(users.ElementAt(i).Element("id").Value);
                        oUser.LoginName = users.ElementAt(i).Element("loginName").Value;
                        oUser.DisplayName = users.ElementAt(i).Element("displayName").Value;
                        if (users.ElementAt(i).Element("state") != null)
                            oUser.State = Convert.ToInt32(users.ElementAt(i).Element("state").Value);
                        if (users.ElementAt(i).Element("userCatalog") != null)
                            oUser.UserCatalog = Convert.ToInt32(users.ElementAt(i).Element("userCatalog").Value);
                        if (users.ElementAt(i).Element("userType") != null)
                            oUser.UserType = Convert.ToInt32(users.ElementAt(i).Element("userType").Value);
                        if (users.ElementAt(i).Element("mobile") != null)
                            oUser.Mobile = users.ElementAt(i).Element("mobile").Value;
                        oUser.Roles = GetUserRoles(users.ElementAt(i));
                        oResult.Users.Add(oUser);
                    }

                    var urolesElement = users.ElementAt(0).Element("roles");
                    if (urolesElement != null)
                    {
                        var uroles = urolesElement.Elements("role");
                        oResult.Roles = (from q in uroles
                                         select new Role()
                                         {
                                             Id = Convert.ToInt32(q.Element("id").Value),
                                             Name = q.Element("name").Value
                                         }).ToList();
                    }
                }

                //get roles
                var roles = root.Elements("role");
                if (roles != null && roles.Count() > 0)
                {
                    oResult.Roles = (from q in roles
                                     select new Role()
                                     {
                                         Id = Convert.ToInt32(q.Element("id").Value),
                                         Name = q.Element("name").Value
                                     }).ToList();
                }
            }
            return oResult;
        }

        private static List<Role> GetUserRoles(XElement user)
        {
            var roles = user.Elements("role");
            List<Role> roleList = null;
            if (roles != null && roles.Count() > 0)
            {
                roleList = new List<Role>();
                roleList = (from q in roles
                                 select new Role()
                                 {
                                     Id = Convert.ToInt32(q.Element("id").Value),
                                     Name = q.Element("name").Value
                                 }).ToList();
            }
            return roleList;
        }
    }
}

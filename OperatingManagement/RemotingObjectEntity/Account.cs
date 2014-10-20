using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using OperatingManagement.RemotingObjectInterface;
using OperatingManagement.DataAccessLayer.System;
using OperatingManagement.Framework;
using System.Xml.Linq;

namespace OperatingManagement.RemotingObjectEntity
{
    public class Account:MarshalByRefObject, IAccount
    {
        //private static bool IsTest = false;

        private bool IsTest
        {
            get
            {
                if (ConfigurationManager.AppSettings["ForTest"] != null)
                {
                    if (ConfigurationManager.AppSettings["ForTest"] != "1")
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
        }
        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string ValidateUser(string userName, string password)
        {
            XElement root = new XElement("status");
            XElement code = new XElement("code");
            XElement msg = new XElement("msg");
            XElement user = new XElement("user");
            XElement roles = new XElement("roles");
            XElement permissions = new XElement("permissions");
            string sTmp = string.Empty;
            
            Logger.Info("收到认证用户请求，Name:" + userName + ",Password:" + password);

            #region forTest---
            if (IsTest)
            {
                if (userName == "wfuser1" && password == "JNsSPx9ADHJqZDVNvgZO5bWWMn0=")
                    return @"<status>\r\n  <code>5</code>\r\n  <msg />\r\n  <user>\r\n    <id>8</id>\r\n    <displayName>wf user 1</displayName>\r\n    <loginName>wfuser1</loginName>\r\n    <mobile></mobile>\r\n    <state>0</state>\r\n    <userType>0</userType>\r\n    <userCatalog>0</userCatalog>\r\n    <createdTime>2012/03/08 14:02:04</createdTime>\r\n  </user>\r\n  <roles>\r\n    <role>\r\n      <id>4</id>\r\n      <name>工作流管理</name>\r\n    </role>\r\n  </roles>\r\n  <permissions>\r\n    <permission>\r\n      <id>58</id>\r\n      <module>\r\n        <id>40</id>\r\n        <name>wf_d01</name>\r\n      </module>\r\n      <task>\r\n        <id>7</id>\r\n        <name>Do</name>\r\n      </task>\r\n    </permission>\r\n    <permission>\r\n      <id>59</id>\r\n      <module>\r\n        <id>41</id>\r\n        <name>wf_c01</name>\r\n      </module>\r\n      <task>\r\n        <id>7</id>\r\n        <name>all</name>\r\n      </task>\r\n    </permission>\r\n    <permission>\r\n      <id>60</id>\r\n      <module>\r\n        <id>42</id>\r\n        <name>wf_c02</name>\r\n      </module>\r\n      <task>\r\n        <id>7</id>\r\n        <name>Do</name>\r\n      </task>\r\n    </permission>\r\n    <permission>\r\n      <id>61</id>\r\n      <module>\r\n        <id>43</id>\r\n        <name>wf_c03</name>\r\n      </module>\r\n      <task>\r\n        <id>7</id>\r\n        <name>Do</name>\r\n      </task>\r\n    </permission>\r\n    <permission>\r\n      <id>62</id>\r\n      <module>\r\n        <id>44</id>\r\n        <name>wf_c04</name>\r\n      </module>\r\n      <task>\r\n        <id>7</id>\r\n        <name>Do</name>\r\n      </task>\r\n    </permission>\r\n  </permissions>\r\n</status>";
                if (userName == "wfuser2" && password == "JNsSPx9ADHJqZDVNvgZO5bWWMn0=")
                    return @"<status>\r\n  <code>5</code>\r\n  <msg />\r\n  <user>\r\n    <id>9</id>\r\n    <displayName>wf user 2</displayName>\r\n    <loginName>wfuser2</loginName>\r\n    <mobile></mobile>\r\n    <state>0</state>\r\n    <userType>0</userType>\r\n    <userCatalog>0</userCatalog>\r\n    <createdTime>2012/03/08 14:02:52</createdTime>\r\n  </user>\r\n  <roles>\r\n    <role>\r\n      <id>5</id>\r\n      <name>工作流客户端管理</name>\r\n    </role>\r\n  </roles>\r\n  <permissions>\r\n    <permission>\r\n      <id>59</id>\r\n      <module>\r\n        <id>41</id>\r\n        <name>wf_c01</name>\r\n      </module>\r\n      <task>\r\n        <id>7</id>\r\n        <name>Do</name>\r\n      </task>\r\n    </permission>\r\n    <permission>\r\n      <id>60</id>\r\n      <module>\r\n        <id>42</id>\r\n        <name>wf_c02</name>\r\n      </module>\r\n      <task>\r\n        <id>7</id>\r\n        <name>all</name>\r\n      </task>\r\n    </permission>\r\n    <permission>\r\n      <id>61</id>\r\n      <module>\r\n        <id>43</id>\r\n       <name>wf_c03</name>\r\n      </module>\r\n      <task>\r\n        <id>7</id>\r\n        <name>Do</name>\r\n      </task>\r\n    </permission>\r\n    <permission>\r\n      <id>62</id>\r\n      <module>\r\n        <id>44</id>\r\n        <name>wf_c04</name>\r\n      </module>\r\n      <task>\r\n        <id>7</id>\r\n        <name>Do</name>\r\n      </task>\r\n    </permission>\r\n  </permissions>\r\n</status>";
                if (userName == "wfuser3" && password == "JNsSPx9ADHJqZDVNvgZO5bWWMn0=")
                    return @"<status>\r\n  <code>5</code>\r\n  <msg />\r\n  <user>\r\n    <id>10</id>\r\n    <displayName>wf user 3</displayName>\r\n    <loginName>wfuser3</loginName>\r\n    <mobile></mobile>\r\n    <state>0</state>\r\n    <userType>0</userType>\r\n    <userCatalog>0</userCatalog>\r\n    <createdTime>2012/03/08 14:03:21</createdTime>\r\n  </user>\r\n  <roles>\r\n    <role>\r\n      <id>6</id>\r\n      <name>工作流设计器管理</name>\r\n    </role>\r\n  </roles>\r\n  <permissions>\r\n    <permission>\r\n      <id>58</id>\r\n      <module>\r\n        <id>40</id>\r\n        <name>wf_d01</name>\r\n      </module>\r\n      <task>\r\n        <id>7</id>\r\n        <name>all</name>\r\n      </task>\r\n    </permission>\r\n  </permissions>\r\n</status>";
                return @"<status>\r\n  <code>2</code>\r\n  <msg />\r\n  <user />\r\n  <roles />\r\n  <permissions />\r\n</status>";
            }
            #endregion

            try
            {
                User u = new User()
                {
                    LoginName = userName,
                    Password = password
                };
                FieldVerifyResult retVal = u.Verify(false);
                switch (retVal)
                {
                    case FieldVerifyResult.NotExist:
                        code.Value = "1";
                        break;
                    case FieldVerifyResult.PasswordIncorrect:
                        code.Value = "2";
                        break;
                    case FieldVerifyResult.Inactive:
                        code.Value = "3";
                        break;
                    case FieldVerifyResult.Error:
                        code.Value = "4";
                        break;
                    case FieldVerifyResult.Success:
                        code.Value = "5";
                        user.Add(new XElement("id", u.Id),
                            new XElement("displayName", u.DisplayName),
                            new XElement("loginName", u.LoginName),
                            new XElement("mobile", u.Mobile),
                            new XElement("state", (int)u.Status),
                            new XElement("userType", (int)u.UserType),
                            new XElement("userCatalog", (int)u.UserCatalog),
                            new XElement("createdTime", u.CreatedTime.ToString("yyyy/MM/dd HH:mm:ss",
                                System.Globalization.DateTimeFormatInfo.InvariantInfo)));

                        var rs = u.SelectRolesById();
                        if (rs != null && rs.Count > 0)
                        {
                            foreach (var r in rs) {
                                roles.Add(new XElement("role", 
                                    new XElement("id", r.Id),
                                    new XElement("name", r.RoleName)));
                            }
                        }
                        var p = new Permission();
                        var ps = p.SelectByLoginName(userName);
                        if (ps != null && ps.Count > 0)
                        {
                            foreach (var per in ps)
                            {
                                permissions.Add(new XElement("permission",
                                    new XElement("id", per.Id),
                                    new XElement("module",
                                        new XElement("id", per.Module.Id),
                                        new XElement("name", per.Module.ModuleName)),
                                    new XElement("task",
                                        new XElement("id", per.Task.Id),
                                        new XElement("name", per.Task.TaskName)))
                                        );
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                code.Value = "0";
                sTmp = "认证过程出现异常";
                msg.Add(sTmp);
                Logger.Error(sTmp, ex);
            }
            root.Add(code, msg, user, roles, permissions);

            sTmp = root.ToString();
            Logger.Info("认证完成，Name:" + userName + "，Result:" + code.Value.ToString());
            Logger.Debug(sTmp);
            return sTmp;
        }

        
        public string GetUserByID(int userid)
        {
            XElement root = new XElement("users");
            XElement roles = new XElement("roles");
            string sTmp = string.Empty;

            #region for test
            if (IsTest)
            {
                if (userid == 10)
                    return @"<users>\r\n  <msg />\r\n  <user>\r\n    <id>10</id>\r\n    <displayName>wf user 3</displayName>\r\n    <loginName>wfuser3</loginName>\r\n    <mobile></mobile>\r\n    <state>0</state>\r\n    <userType>0</userType>\r\n    <userCatalog>0</userCatalog>\r\n    <roles>\r\n      <role>\r\n        <id>1</id>\r\n        <name>用户管理员</name>\r\n      </role>\r\n      <role>\r\n        <id>4</id>\r\n        <name>工作流管理</name>\r\n      </role>\r\n      <role>\r\n        <id>5</id>\r\n        <name>工作流客户端管理</name>\r\n      </role>\r\n      <role>\r\n        <id>49</id>\r\n        <name>用户管理-无删除</name>\r\n      </role>\r\n    </roles>\r\n  </user>\r\n</users>";
                else
                    return "";
            }
            #endregion

            Logger.Info(string.Format("收到获取某用户信息请求，UserID={0}", userid));
            try
            {

                User u = new User()
                {
                    Id = userid
                };
                u = u.SelectById();

                if (u == null)
                    root.Add(new XElement("msg", "获取不到指定的用户"));
                else
                {
                    root.Add(new XElement("msg"), "");
                    XElement user = new XElement("user", 
                        new XElement("id", u.Id),
                        new XElement("displayName", u.DisplayName),
                        new XElement("loginName", u.LoginName),
                        new XElement("mobile", u.Mobile),
                        new XElement("state", (int)u.Status),
                        new XElement("userType", (int)u.UserType),
                        new XElement("userCatalog", (int)u.UserCatalog));
                    var rs = u.SelectRolesById();
                    if (rs != null && rs.Count > 0)
                    {
                        foreach (var r in rs)
                        {
                            roles.Add(new XElement("role",
                                new XElement("id", r.Id),
                                new XElement("name", r.RoleName)));
                        }
                        user.Add(roles);
                    }
                    root.Add(user);
                }
            }
            catch (Exception ex)
            {
                sTmp = "获取用户信息过程出现异常";
                root.Add(new XElement("msg", sTmp));
                Logger.Error(sTmp, ex);
            }
            finally { }

            if (sTmp == string.Empty)
            {
                sTmp = root.ToString();
                Logger.Info("获取所有角色信息成功");
                Logger.Debug(sTmp);
            }
            return sTmp;
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public string GetAllRoles()
        {
            XElement root = new XElement("roles");
            Role oRole = new Role();
            List<Role> roles;
            string sTmp = string.Empty;

            Logger.Info("收到获取所有角色请求");

            #region for Test...
            if (IsTest)
            {
                return @"<roles>\r\n  <msg />\r\n  <role>\r\n    <id>1</id>\r\n    <name>用户管理员</name>\r\n    <note>用户管理员</note>\r\n  </role>\r\n  <role>\r\n    <id>4</id>\r\n    <name>工作流管理</name>\r\n    <note>工作流管理人员</note>\r\n  </role>\r\n  <role>\r\n    <id>5</id>\r\n    <name>工作流客户端管理</name>\r\n    <note>工作流客户端管理</note>\r\n  </role>\r\n  <role>\r\n    <id>6</id>\r\n    <name>工作流设计器管理</name>\r\n    <note>工作流设计器管理</note>\r\n  </role>\r\n  <role>\r\n    <id>2</id>\r\n    <name>角色管理员</name>\r\n    <note>角色管理员</note>\r\n  </role>\r\n</roles>";
            }
            #endregion

            try
            {
                roles = oRole.SelectAll();
                if (roles != null && roles.Count > 0)
                {
                    root.Add(new XElement("msg"), "");
                    foreach (var r in roles)
                    {
                        root.Add(new XElement("role",
                            new XElement("id", r.Id),
                            new XElement("name", r.RoleName),
                            new XElement("note", r.Note)));
                    }
                }
            }
            catch (Exception ex)
            {
                sTmp = "获取所有角色信息出现异常";
                root.Add(new XElement("msg", sTmp));
                Logger.Error(sTmp, ex);
            }
            finally
            {
            }

            if (sTmp == string.Empty)
            {
                sTmp = root.ToString();
                Logger.Info("获取所有角色信息成功");
                Logger.Debug(sTmp);
            }
            return sTmp;
        }

        /// <summary>
        /// 获取某个角色的所有用户
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public string GetUsersByRoleID(int roleID)
        {
            XElement root = new XElement("users");
            string sTmp = string.Empty;

            Logger.Info("收到获取角色：" + roleID.ToString() + "用户的请求");

            #region for Test...
            if (IsTest)
            {
                if (roleID == 4)
                    return @"<users>\r\n  <msg />\r\n  <user>\r\n    <id>8</id>\r\n    <loginname>wfuser1</loginname>\r\n    <displayname>wf user 1</displayname>\r\n    <note></note>\r\n  </user>\r\n  <user>\r\n    <id>11</id>\r\n    <loginname>wfuser4</loginname>\r\n    <displayname>wf user 4</displayname>\r\n    <note></note>\r\n  </user>\r\n</users>";
                if (roleID == 5)
                    return @"<users>\r\n  <msg />\r\n  <user>\r\n    <id>9</id>\r\n    <loginname>wfuser2</loginname>\r\n    <displayname>wf user 2</displayname>\r\n    <note></note>\r\n  </user>\r\n  <user>\r\n    <id>12</id>\r\n    <loginname>wfuser5</loginname>\r\n    <displayname>wf user 5</displayname>\r\n    <note></note>\r\n  </user>\r\n</users>";
                if (roleID == 6)
                    return @"<users>\r\n  <msg />\r\n  <user>\r\n    <id>10</id>\r\n    <loginname>wfuser3</loginname>\r\n    <displayname>wf user 3</displayname>\r\n    <note></note>\r\n  </user>\r\n  <user>\r\n    <id>13</id>\r\n    <loginname>wfuser6</loginname>\r\n    <displayname>wf user 6</displayname>\r\n    <note></note>\r\n  </user>\r\n</users>";
                return @"<users>\r\n <msg>no users</msg>\r\n</users>";
            }
            #endregion
            
            User oUser = new User();
            try
            {
                List<User> users = oUser.SelectByRoleId(roleID);
                root.Add(new XElement("msg"), "");
                if (users != null && users.Count > 0)
                {
                    foreach (var u in users)
                    {
                        root.Add(new XElement("user",
                            new XElement("id", u.Id),
                            new XElement("loginname", u.LoginName),
                            new XElement("displayname", u.DisplayName),
                            new XElement("note", u.Note)));
                    }
                }
            }
            catch (Exception ex)
            {
                sTmp = "获取角色" + roleID.ToString() + "用户出现异常";
                root.Add(new XElement("msg", sTmp));
                Logger.Error(sTmp, ex);
            }
            finally
            {
            }

            if (sTmp == string.Empty)
            {
                sTmp = root.ToString();
                Logger.Info("获取角色" + roleID.ToString() + "用户成功");
                Logger.Debug(sTmp);
            }
            return sTmp;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public string GetAllUsers()
        {
            XElement root = new XElement("users");
            string sTmp = string.Empty;

            Logger.Info("收到获取所有用户请求");

            #region for Test...
            if (IsTest)
            {
                return @"<users>\r\n  <msg />\r\n  <user>\r\n    <id>1</id>\r\n    <loginName>admin</loginName>\r\n    <displayName>系统管理员</displayName>\r\n    <note></note>\r\n  </user>\r\n  <user>\r\n    <id>2</id>\r\n    <loginName>opercindy</loginName>\r\n    <displayName>付仁华</displayName>\r\n    <note>用户付仁华</note>\r\n  </user>\r\n  <user>\r\n    <id>8</id>\r\n    <loginName>wfuser1</loginName>\r\n    <displayName>wf user 1</displayName>\r\n    <note></note>\r\n  </user>\r\n  <user>\r\n    <id>10</id>\r\n    <loginName>wfuser3</loginName>\r\n    <displayName>wf user 3</displayName>\r\n    <note></note>\r\n  </user>\r\n  <user>\r\n    <id>9</id>\r\n    <loginName>wfuser2</loginName>\r\n    <displayName>wf user 2</displayName>\r\n    <note></note>\r\n  </user>\r\n  <user>\r\n    <id>11</id>\r\n    <loginName>wfuser4</loginName>\r\n    <displayName>wf user 4</displayName>\r\n    <note></note>\r\n  </user>\r\n  <user>\r\n    <id>12</id>\r\n    <loginName>wfuser5</loginName>\r\n    <displayName>wf user 5</displayName>\r\n    <note></note>\r\n  </user>\r\n  <user>\r\n    <id>13</id>\r\n    <loginName>wfuser6</loginName>\r\n    <displayName>wf user 6</displayName>\r\n    <note></note>\r\n  </user>\r\n  <user>\r\n    <id>7</id>\r\n    <loginName>mygirl</loginName>\r\n    <displayName>my girl</displayName>\r\n    <note></note>\r\n  </user>\r\n</users>";
            }
            #endregion
            User oUser = new User();
            try
            {
                List<User> users = oUser.SelectAll();
                root.Add(new XElement("msg"), "");
                if (users != null && users.Count > 0)
                {
                    foreach (var u in users)
                    {
                        //var rs = u.SelectRolesById();
                        //XElement roles = new XElement("roles");
                        //if (rs != null && rs.Count > 0)
                        //{
                        //    foreach (var r in rs)
                        //    {
                        //        roles.Add(new XElement("role",
                        //            new XElement("id", r.Id),
                        //            new XElement("name", r.RoleName)));
                        //    }
                        //}

                        root.Add(new XElement("user",
                            new XElement("id", u.Id),
                            new XElement("displayName", u.DisplayName),
                            new XElement("loginName", u.LoginName),
                            new XElement("mobile", u.Mobile),
                            new XElement("state", (int)u.Status),
                            new XElement("userType", (int)u.UserType),
                            new XElement("userCatalog", (int)u.UserCatalog)
                            //, roles
                            ));
                    }
                }
            }
            catch (Exception ex)
            {
                sTmp = "获取所有用户信息出现异常";
                root.Add(new XElement("msg", sTmp));
                Logger.Error(sTmp, ex);
            }
            finally
            {
            }

            if (sTmp == string.Empty)
            {
                sTmp = root.ToString();
                Logger.Info("获取所有用户信息成功");
                Logger.Debug(sTmp);
            }
            return sTmp;
        }
    }
}

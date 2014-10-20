using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using OperatingManagement.Framework;
using OperatingManagement.DataAccessLayer.System;

namespace OperatingManagement.WebKernel.HttpHandlers
{
    public class DataHandler:AbHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            var req = context.Request; 
            string msg = string.Empty;
            bool suc = false;
            try
            {
                switch (req["action"])
                {
                    case "deleteUsersByIds":
                        suc = DeleteUserByIds(req["ids"], out msg);
                        break;
                    case "deleteRolesByIds":
                        suc = DeleteRoleByIds(req["ids"], out msg);
                        break;
                    case "deleteModulesByIds":
                        suc = DeleteModulesByIds(req["ids"], out msg);
                        break;
                }
            }
            catch (Exception ex)
            {
                suc = false;
                msg = "服务器端错误。";
            }
            WriteResponse(msg, suc);
        }
        bool DeleteRoleByIds(string ids, out string msg)
        {
            Role r = new Role();
            var retValue = r.DeleteByIds(ids);
            if (retValue == FieldVerifyResult.Error)
            {
                msg = "删除角色关联数据失败。";
                return true;
            }
            msg = string.Empty;
            return true;
        }
        bool DeleteUserByIds(string ids, out string msg)
        {
            User u = new User();
            var retValue = u.DeleteByIds(ids); 
            if (retValue == FieldVerifyResult.Error)
            {
                msg = "删除用户关联数据失败。";
                return true;
            }
            msg = string.Empty;
            return true;
        }
        bool DeleteModulesByIds(string ids, out string msg)
        {
            Module m = new Module();
            var retValue = m.DeleteByIds(ids);
            if (retValue == FieldVerifyResult.Error)
            {
                msg = "删除权限关联数据失败。";
                return true;
            }
            msg = string.Empty;
            return true;
        }
    }
}

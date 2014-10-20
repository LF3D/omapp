using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.RemotingHelper;
using OperatingManagement.RemotingObjectInterface;

namespace SSOTest
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string token = Request.QueryString["token"];
            if (!string.IsNullOrEmpty(token))
            {
                string decryptedToken = EncryptHelper.Decrypt(HttpUtility.UrlDecode(token));
                string[] ks = decryptedToken.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);
                string userName = ks[0];
                string password = ks[1];
                string timeStr = ks[2];
                DateTime dt = new DateTime(Convert.ToInt64(timeStr));
                if (dt < DateTime.Now.AddMinutes(-5))
                {
                    throw new Exception("timeout.");
                }
                else
                {
                    IAccount account = OperatingManagement.RemotingClient.RemotingActivator.GetObject<IAccount>();

                    string xml = account.ValidateUser(userName, password);

                    txtXml.Text = xml;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Profile;
using System.Configuration;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.DataAccessLayer.System;

namespace OperatingManagement.Security
{
    public class AspNetProfileProvider:ProfileProvider
    {
        private const string accountInfo = "Account";
        private string applicationName = "AspNetProfileProvider";

        #region -Override-
        public override string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }
        public override string Description
        {
            get { return "AspNet Customize Profile Provider"; }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            string userName = (string)context["UserName"];
            bool isAuthenticated = (bool)context["IsAuthenticated"];
            SettingsPropertyValueCollection spvc = new SettingsPropertyValueCollection();
            SettingsPropertyValue spv = null;
            foreach (SettingsProperty sp in collection)
            {
                spv = new SettingsPropertyValue(sp);
                switch (spv.Property.Name)
                {
                    case accountInfo:
                        if (isAuthenticated)
                        {
                            //Gets the specific Account by userName.
                            spv.PropertyValue = GetAccountInfo(userName);
                        }
                        break;
                }
                spvc.Add(spv);
            }
            return spvc;
        }
        #endregion

        #region -Ignore override-
        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override int DeleteProfiles(string[] usernames)
        {
            throw new NotImplementedException();
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region -Private Method-
        //Gets the specific Account by userName.
        User GetAccountInfo(string userName)
        {
            User u = new User() { LoginName = userName };
            return u.SelectByLoginName();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Configuration;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.DataAccessLayer.System;

namespace OperatingManagement.Security
{
    /// <summary>
    /// It is a strongly assembly type for THIS WEB APPLICATION to provide User Profile management.
    /// </summary>
    public class WebProfile
    {
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Security.WebProfile"/> class.
        /// </summary>
        /// <param name="profile">Current Profile</param>
        public WebProfile(ProfileBase profile)
        {
            CurrentProfile = profile;
        }
        private ProfileBase CurrentProfile = null;

        //This shows how to get the Account information
        /// <summary>
        /// Gets/Sets the account information(login)
        /// </summary>
        public User Account
        {
            get
            {
                return (User)CurrentProfile.GetPropertyValue("Account");
            }
            set
            {
                CurrentProfile.SetPropertyValue("Account", value);
            }
        }
        /// <summary>
        /// Gets user name.
        /// </summary>
        public string UserName
        {
            get { return CurrentProfile.UserName; }
        }
        /// <summary>
        /// Gets Settings Context
        /// </summary>
        public SettingsContext Context
        {
            get { return CurrentProfile.Context; }
        }
        /// <summary>
        /// Gets a value indicating whether user is anonymous.
        /// </summary>
        public bool IsAnonymous
        {
            get { return CurrentProfile.IsAnonymous; }
        }
        /// <summary>
        /// Gets Settings Property Value Collection.
        /// </summary>
        public SettingsPropertyValueCollection PropertyValues
        {
            get { return CurrentProfile.PropertyValues; }
        }
        
    }
}

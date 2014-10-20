using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperatingManagement.Framework;
using log4net;
using System.Web;

namespace OperatingManagement.Framework.Core
{
    /// <summary>
    /// Provides exception manage.
    /// </summary>
    public class AspNetException: Exception
    {
        private string message;
        private string httpReferrer = string.Empty;
        private string httpVerb = string.Empty;
        private string ipAddress = string.Empty;
        private string httpPathAndQuery = string.Empty;
        private string stackTrace = string.Empty;
        private string userAgent = string.Empty;
        private DateTime dateCreated;
        private DateTime dateLastOccurred;
        private int frequency = 0;
        private int exceptionID = 0;

        private static readonly ILog logExp = LogManager.GetLogger("ExceptionLogger");
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Framework.Core.AspNetException"/> class.
        /// </summary>
        public AspNetException()
            : base()
        {
            Init();
        }
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Framework.Core.AspNetException"/> class.
        /// </summary>
        /// <param name="message">Addin Message</param>
        public AspNetException(string message)
            : base(message)
        {
            Init();
            this.message = message;
        }
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Framework.Core.AspNetException"/> class.
        /// </summary>
        /// <param name="message">Addin Message</param>
        /// <param name="inner">Inner Exception</param>
        public AspNetException(string message, Exception inner)
            : base(message, inner)
        {
            Init();
            this.message = message;
        }
        /// <summary>
        /// Gets/Sets the agent string from client browser.
        /// </summary>
        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }
        /// <summary>
        /// Gets/Sets the ip address when exception is fired.
        /// </summary>
        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        /// <summary>
        /// Gets/Sets the referrer url when exception is fired.
        /// </summary>
        public string HttpReferrer
        {
            get { return httpReferrer; }
            set { httpReferrer = value; }
        }
        /// <summary>
        /// Gets/Sets the http verb when exception is fired.
        /// </summary>
        public string HttpVerb
        {
            get { return httpVerb; }
            set { httpVerb = value; }
        }
        /// <summary>
        /// Gets/Sets the http path and query when exception is fired.
        /// </summary>
        public string HttpPathAndQuery
        {
            get { return httpPathAndQuery; }
            set { httpPathAndQuery = value; }
        }
        /// <summary>
        /// Gets/Sets the created datetime.
        /// </summary>
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }
        /// <summary>
        /// Gets/Sets the datetime last occurred.
        /// </summary>
        public DateTime DateLastOccurred
        {
            get { return dateLastOccurred; }
            set { dateLastOccurred = value; }
        }
        /// <summary>
        /// Gets/Sets the frequency.
        /// </summary>
        public int Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }
        /// <summary>
        /// Gets/Sets the logged stack trace message.
        /// </summary>
        public string LoggedStackTrace
        {
            get
            {
                return stackTrace;
            }
            set
            {
                stackTrace = value;
            }
        }
        /// <summary>
        /// Gets/Sets the exeception id.
        /// </summary>
        public int ExceptionID
        {
            get
            {
                return exceptionID;
            }
            set
            {
                exceptionID = value;
            }
        }
        /// <summary>
        /// Log current error.
        /// </summary>
        public void Log()
        {
            logExp.Error(this.message, this);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", base.ToString(), this.StackTrace);
        }

        private void Init()
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx != null && ctx.Request != null)
            {
                if (ctx.Request.UrlReferrer != null)
                    httpReferrer = ctx.Request.UrlReferrer.ToString();

                if (ctx.Request.UserAgent != null)
                    userAgent = ctx.Request.UserAgent;

                if (ctx.Request.UserHostAddress != null)
                    ipAddress = ctx.Request.UserHostAddress;

                if (ctx.Request.Url != null &&
                    ctx.Request.Url.PathAndQuery != null)
                    httpPathAndQuery = ctx.Request.Url.PathAndQuery;
            }
        }
    }
}

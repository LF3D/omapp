using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace OperatingManagement.Web.ucs
{
    public partial class ucTimer : System.Web.UI.UserControl
    {
        public enum TimeSeperator
        {
            empty, //空
            colon,  //冒号
            dash    //破折号
        }

        #region --Properties--
        private bool isShowSecond = true;   //是否显示秒
        private TimeSeperator _seperator = TimeSeperator.empty;  //时间的分隔符
        /// <summary>
        /// 是否显示秒
        /// </summary>
        public bool ShowSecond
        {
            set { isShowSecond = value; }
        }
        /// <summary>
        /// 时间分隔符
        /// </summary>
        public TimeSeperator Seperator
        {
            get { return _seperator; }
            set { _seperator = value; }
        }

        /// <summary>
        /// 设置控件的时间
        /// </summary>
        public string Timer
        {
            set
            {
                string setTime = value;
                setTime = setTime.Replace(":", "").Replace("-", "");
                if (setTime.Length == 4 || setTime.Length == 6)
                {
                    ddlHour.SelectedValue = setTime.Substring(0, 2);
                    ddlMinute.SelectedValue = setTime.Substring(2, 2);
                    if (setTime.Length == 6)
                    {
                        ddlSecond.SelectedValue = setTime.Substring(4, 2);
                    }
                }//endif
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!isShowSecond)
                {
                    ddlSecond.Visible = false;
                }
            }
        }

        /// <summary>
        /// 初始化时，分，秒
        /// </summary>
        private void InitialTime()
        {
            ddlHour.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                ddlHour.Items.Add(new ListItem(i.ToString("00") + "时", i.ToString("00")));
            }
            ddlMinute.Items.Clear();
            for (int i = 0; i < 60; i++)
            {
                ddlMinute.Items.Add(new ListItem(i.ToString("00") + "分", i.ToString("00")));
            }
            ddlSecond.Items.Clear();
            for (int i = 0; i < 60; i++)
            {
                ddlSecond.Items.Add(new ListItem(i.ToString("00") + "秒", i.ToString("00")));
            } 
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitialTime();
        }

        public override string ToString()
        {
            string seprator = "";
            string returnValue;
            switch (_seperator)
            { 
                case TimeSeperator.empty:
                    seprator = "";
                    break;
                case TimeSeperator.colon:
                    seprator = ":";
                    break;
                case TimeSeperator.dash:
                    seprator = "-";
                    break;
                default:
                    seprator = "";
                    break;
            }
            returnValue = ddlHour.SelectedValue + seprator + ddlMinute.SelectedValue;
            if (isShowSecond)
            {
                returnValue += seprator + ddlSecond.SelectedValue;
            }
            return returnValue;
            //return base.ToString();
        }

    }//
}
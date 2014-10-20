using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using System.Data;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.PlanManage
{
    /// <summary>
    /// 应用研究工作计划
    /// </summary>
    public class YJJH
    {
        #region -Properties-

        public int ID { get; set; }
        public DateTime CTime { get; set; }
        public string TaskID { get; set; }
        public string InfoType { get; set; }
        public string FileIndex { get; set; }

        public string SatID { get; set; }
        /// <summary>
        /// 信息分类
        /// </summary>
        public string XXFL { get; set; }
        /// <summary>
        /// 计划序号
        /// </summary>
        public string JXH { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SysName { get; set; }
        /// <summary>
        /// 系统任务
        /// </summary>
        public List<YJJH_Task> Tasks { get; set; }
        #endregion

        public void ReadXML(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("应用研究工作计划/XXFL");
            this.XXFL = root.InnerText;
            root = xmlDoc.SelectSingleNode("应用研究工作计划/JXH");
            this.JXH = root.InnerText;
            root = xmlDoc.SelectSingleNode("应用研究工作计划/SysName");
            this.SysName = root.InnerText;


            root = xmlDoc.SelectSingleNode("应用研究工作计划");
            List<YJJH_Task> list = new List<YJJH_Task>();
            YJJH_Task c;
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.Name == "Work")
                {
                    c = new YJJH_Task();
                    c.StartTime = n["StartTime"].InnerText;
                    c.EndTime = n["EndTime"].InnerText;
                    c.Task = n["Task"].InnerText;
                    list.Add(c);
                }
            }
            this.Tasks = list;
        }
    }

    public class YJJH_Task
    {
        /// <summary>
        /// 试验开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 试验结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 系统任务
        /// </summary>
        public string Task { get; set; }
    }
}

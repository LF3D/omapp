using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using System.Data;
using Oracle.DataAccess.Client;
using System.Xml;

namespace OperatingManagement.DataAccessLayer.PlanManage
{
    /// <summary>
    /// 设备工作计划
    /// </summary>
    public class DJZYJH
    {
        #region -Properties-
        public int ID { get; set; }
        public DateTime CTime { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string TaskID { get; set; }
        public string InfoType { get; set; }
        public int LineCount { get; set; }
        public string Format1 { get; set; }
        public string Format2 { get; set; }
        public string DataSection { get; set; }
        public string FileIndex { get; set; }
        public string Reserve { get; set; }

        public string SatID { get; set; }
        public string SNO { get; set; }    //申请序列号
        public string SJ { get; set; }    //时间
        public string HTQID { get; set; }    //航天器标识
        public string SNUM { get; set; }    //计划数量

        public List<DJZYJH_Plan> DJZYJHPlans { get; set; }
        #endregion

        /// <summary>
        /// 根据ID获取使用申请实例
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public DJZYJH GetByID(string sID)
        {
            List<JH> jh = (new JH()).SelectByIDS(sID);
            string fileIndex = jh[0].FileIndex;

            DJZYJH obj = new DJZYJH();
            obj.DJZYJHPlans = new List<DJZYJH_Plan>();
            DJZYJH_Plan task;
            DJZYJH_GZDP dp;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileIndex);
            XmlNode root = xmlDoc.SelectSingleNode("测控资源使用计划/时间");
            obj.SJ = root.InnerText;
            root = xmlDoc.SelectSingleNode("测控资源使用计划/计划序列号");
            obj.SNO = root.InnerText;
            root = xmlDoc.SelectSingleNode("测控资源使用计划/航天器标识");
            obj.HTQID = root.InnerText;
            root = xmlDoc.SelectSingleNode("测控资源使用计划/计划数量");
            obj.SNUM = root.InnerText;

            root = xmlDoc.SelectSingleNode("测控资源使用计划/计划");
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.Name == "计划内容")
                {
                    task = new DJZYJH_Plan();
                    task.SXH = n["计划序号"].InnerText;
                    task.DF = n["答复标志"].InnerText;
                    task.SXZ = n["计划性质"].InnerText;
                    task.MLB = n["任务类别"].InnerText;
                    task.FS = n["工作方式"].InnerText;
                    task.GZDY = n["工作单元"].InnerText;
                    task.SBDH = n["设备代号"].InnerText;
                    task.QC = n["圈次"].InnerText;
                    task.QB = n["圈标"].InnerText;
                    task.SHJ = n["测控事件类型"].InnerText;
                    task.FNUM = n["工作点频数量"].InnerText;
                    task.DJZYJHGZDPs = new List<DJZYJH_GZDP>();
                    foreach (XmlNode nn in n["工作点频"].ChildNodes)
                    {
                        if (nn.Name == "工作点频内容")
                        {
                            dp = new DJZYJH_GZDP();
                            dp.FXH = nn["点频序号"].InnerText;
                            dp.PDXZ = nn["频段选择"].InnerText;
                            dp.DPXZ = nn["点频选择"].InnerText;
                            task.DJZYJHGZDPs.Add(dp);
                        }
                    }
                   
                    task.TNUM = n["同时支持目标数"].InnerText;
                    task.ZHB = n["任务准备开始时间"].InnerText;
                    task.RK = n["任务开始时间"].InnerText;
                    task.GZK = n["跟踪开始时间"].InnerText;
                    task.KSHX = n["开上行载波时间"].InnerText;
                    task.GSHX = n["关上行载波时间"].InnerText;
                    task.GZJ = n["跟踪结束时间"].InnerText;
                    task.JS = n["任务结束时间"].InnerText;
                    obj.DJZYJHPlans.Add(task);
                }
            }

            return obj;
        }
    }

    /// <summary>
    /// 测控资源使用计划-任务
    /// </summary>
    [Serializable]
    public class DJZYJH_Plan
    {
        #region -Properties-
        /// <summary>
        /// 计划序号
        /// </summary>
        public string SXH { get; set; }
        /// <summary>
        /// 答复标志
        /// 1－全部满足；
        /// 2－部分满足，时间安排有所调整；
        /// F－无法满足，相应时间参数填全0。
        /// </summary>
        public string DF { get; set; }
        /// <summary>
        /// 计划性质
        /// </summary>
        public string SXZ { get; set; }
        /// <summary>
        /// 任务类别
        /// </summary>
        public string MLB { get; set; }
        /// <summary>
        /// 工作方式
        /// </summary>
        public string FS { get; set; }
        /// <summary>
        /// 工作单元
        /// </summary>
        public string GZDY { get; set; }
        /// <summary>
        /// 设备代号
        /// </summary>
        public string SBDH { get; set; }
        /// <summary>
        /// 圈次
        /// </summary>
        public string QC { get; set; }
        /// <summary>
        /// 圈标
        /// </summary>
        public string QB { get; set; }
        /// <summary>
        /// 测控事件类型
        /// </summary>
        public string SHJ { get; set; }
        /// <summary>
        /// 工作点频数量
        /// </summary>
        public string FNUM { get; set; }
        /// <summary>
        /// 工作点频
        /// </summary>
        public List<DJZYJH_GZDP> DJZYJHGZDPs { get; set; }
        /// <summary>
        /// 同时支持目标数
        /// </summary>
        public string TNUM { get; set; }
        /// <summary>
        /// 任务准备开始时间
        /// </summary>
        public string ZHB { get; set; }
        /// <summary>
        /// 任务开始时间
        /// </summary>
        public string RK { get; set; }
        /// <summary>
        /// 跟踪开始时间
        /// </summary>
        public string GZK { get; set; }
        /// <summary>
        /// 开上行载波时间
        /// </summary>
        public string KSHX { get; set; }
        /// <summary>
        /// 关上行载波时间
        /// </summary>
        public string GSHX { get; set; }
        /// <summary>
        /// 跟踪结束时间
        /// </summary>
        public string GZJ { get; set; }
        /// <summary>
        /// 任务结束时间
        /// </summary>
        public string JS { get; set; }

        #endregion
    }

    /// <summary>
    /// 测控资源使用计划-工作点频
    /// </summary>
    public class DJZYJH_GZDP
    {
        /// <summary>
        /// 点频序号
        /// </summary>
        public string FXH { get; set; }
        /// <summary>
        /// 频段选择
        /// </summary>
        public string PDXZ { get; set; }
        /// <summary>
        /// 点频选择
        /// </summary>
        public string DPXZ { get; set; }
    }
}

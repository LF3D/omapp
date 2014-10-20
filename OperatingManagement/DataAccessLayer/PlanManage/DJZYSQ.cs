﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using System.Data;
using Oracle.DataAccess.Client;
using System.Xml;

namespace OperatingManagement.DataAccessLayer.PlanManage
{
    /// <summary>
    /// 测控资源使用申请
    /// </summary>
    public class DJZYSQ
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
        public string SCID { get; set; }    //航天器标识
        //public string EquipmentID { get; set; }
        public string SNUM { get; set; }    //申请数量

        public List<DJZYSQ_Task> DMJHTasks { get; set; }
        #endregion

        /// <summary>
        /// 根据ID获取使用申请实例
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public DJZYSQ GetByID( string sID)
        {
            List<JH> jh = (new JH()).SelectByIDS(sID);
            string fileIndex = jh[0].FileIndex;

            DJZYSQ obj = new DJZYSQ();
            obj.DMJHTasks = new List<DJZYSQ_Task>();
            DJZYSQ_Task task;
            DJZYSQ_Task_GZDP dp;
            DJZYSQ_Task_ReakTimeTransfor rtt;
            DJZYSQ_Task_AfterFeedBack afb;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileIndex);
            XmlNode root = xmlDoc.SelectSingleNode("测控资源使用申请/时间");
            obj.SJ = root.InnerText;
            root = xmlDoc.SelectSingleNode("测控资源使用申请/申请序列号");
            obj.SNO = root.InnerText;
            root = xmlDoc.SelectSingleNode("测控资源使用申请/航天器标识");
            obj.SCID = root.InnerText;
            root = xmlDoc.SelectSingleNode("测控资源使用申请/申请数量");
            obj.SNUM = root.InnerText;

            root = xmlDoc.SelectSingleNode("测控资源使用申请/申请");
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.Name == "申请内容")
                {
                    task = new DJZYSQ_Task();
                    task.SXH = n["申请序号"].InnerText;
                    task.SXZ = n["申请性质"].InnerText;
                    task.MLB = n["任务类别"].InnerText;
                    task.FS = n["工作方式"].InnerText;
                    task.GZDY = n["工作单元"].InnerText;
                    task.SBDH = n["设备代号"].InnerText;
                    task.QC = n["圈次"].InnerText;
                    task.QB = n["圈标"].InnerText;
                    task.SHJ = n["测控事件类型"].InnerText;
                    task.FNUM = n["工作点频数量"].InnerText;
                    task.GZDPs = new List<DJZYSQ_Task_GZDP>();
                    foreach (XmlNode nn in n["工作点频"].ChildNodes)
                    {
                        if (nn.Name == "工作点频内容")
                        {
                            dp = new DJZYSQ_Task_GZDP();
                            dp.FXH = nn["点频序号"].InnerText;
                            dp.PDXZ = nn["频段选择"].InnerText;
                            dp.DPXZ = nn["点频选择"].InnerText;
                            task.GZDPs.Add(dp);
                        }
                    }
                    task.ReakTimeTransfors = new List<DJZYSQ_Task_ReakTimeTransfor>();

                    foreach (XmlNode nn in n.ChildNodes)
                    {
                        if (nn.Name == "实时传输")
                        {
                            rtt = new DJZYSQ_Task_ReakTimeTransfor();
                            rtt.GBZ = nn["格式标志"].InnerText;
                            rtt.XBZ = nn["信息流标志"].InnerText;
                            rtt.RTs = nn["数据传输开始时间"].InnerText;
                            rtt.RTe = nn["数据传输结束时间"].InnerText;
                            rtt.SL = nn["数据传输速率"].InnerText;
                            task.ReakTimeTransfors.Add(rtt);
                        }
                    }
                    task.AfterFeedBacks = new List<DJZYSQ_Task_AfterFeedBack>();
                    foreach (XmlNode nn in n.ChildNodes)
                    {
                        if (nn.Name == "事后回放")
                        {
                            afb = new DJZYSQ_Task_AfterFeedBack();
                            afb.GBZ = nn["格式标志"].InnerText;
                            afb.XBZ = nn["信息流标志"].InnerText;
                            afb.Ts = nn["数据起始时间"].InnerText;
                            afb.Te = nn["数据结束时间"].InnerText;
                            afb.RTs = nn["数据传输开始时间"].InnerText;
                            afb.SL = nn["数据传输速率"].InnerText;
                            task.AfterFeedBacks.Add(afb);
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
                    obj.DMJHTasks.Add(task);
                }
            }

            return obj;
        }
    }
  
    /// <summary>
    /// 申请
    /// </summary>
    [Serializable]
    public class DJZYSQ_Task
    {
        #region -Properties-
        /// <summary>
        /// 申请序号
        /// </summary>
        public string SXH { get; set; }
        /// <summary>
        /// 申请性质
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
        /// <summary>
        /// 工作点频
        /// </summary>
        public List<DJZYSQ_Task_GZDP> GZDPs { get; set; }
        public List<DJZYSQ_Task_ReakTimeTransfor> ReakTimeTransfors { get; set; }
        public List<DJZYSQ_Task_AfterFeedBack> AfterFeedBacks { get; set; }
        #endregion
    }

    /// <summary>
    /// 测控资源使用申请-实时传输
    /// </summary>
    [Serializable]
    public class DJZYSQ_Task_GZDP
    {
        #region -Properties-
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

        #endregion
    }
    /// <summary>
    /// 测控资源使用申请-实时传输
    /// </summary>
    [Serializable]
    public class DJZYSQ_Task_ReakTimeTransfor
    {
        #region -Properties-
        /// <summary>
        /// 格式标志
        /// </summary>
        public string GBZ { get; set; }
        /// <summary>
        /// 信息流标志
        /// </summary>
        public string XBZ { get; set; }
        /// <summary>
        /// 数据传输开始时间
        /// </summary>
        public string RTs { get; set; }
        /// <summary>
        /// 数据传输结束时间
        /// </summary>
        public string RTe { get; set; }
        /// <summary>
        /// 数据传输速率
        /// </summary>
        public string SL { get; set; }

        #endregion
    }

    /// <summary>
    /// 测控资源使用申请-事后回放
    /// </summary>
    [Serializable]
    public class DJZYSQ_Task_AfterFeedBack
    {
        #region -Properties-
        /// <summary>
        /// 格式标志
        /// </summary>
        public string GBZ { get; set; }
        /// <summary>
        /// 信息流标志
        /// </summary>
        public string XBZ { get; set; }
        /// <summary>
        /// 数据起始时间
        /// </summary>
        public string Ts { get; set; }
        /// <summary>
        /// 数据结束时间
        /// </summary>
        public string Te { get; set; }
        /// <summary>
        /// 数据传输开始时间
        /// </summary>
        public string RTs { get; set; }
        /// <summary>
        /// 数据传输速率
        /// </summary>
        public string SL { get; set; }

        #endregion
    }

}

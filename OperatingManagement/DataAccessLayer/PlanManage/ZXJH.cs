using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using System.Data;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.PlanManage
{
    /// <summary>
    /// 中心计划
    /// </summary>
    public class ZXJH
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
        /// <summary>
        /// 计划对应日期
        /// </summary>
        public string Date { get; set; }

        //试验计划SY

        /// <summary>
        /// 对应日期的试验个数
        /// </summary>
        public string SYCount { get; set; }

        /// <summary>
        /// 试验计划-试验内容
        /// </summary>
        public List<ZXJH_SYContent> SYContents { get; set; }

        //工作计划
        /// <summary>
        /// 任务管理-工作内容
        /// </summary>
        public List<ZXJH_WorkContent> WorkContents { get; set; }
        /// <summary>
        /// 指Ling制作
        /// </summary>
        public List<ZXJH_CommandMake> CommandMakes { get; set; }
        /// <summary>
        /// RealTime试验数据处理
        /// </summary>
        public List<ZXJH_SYDataHandle> SYDataHandles { get; set; }
        /// <summary>
        /// 指挥与监视
        /// </summary>
        public List<ZXJH_DirectAndMonitor> DirectAndMonitors { get; set; }
        /// <summary>
        /// RealTime控制
        /// </summary>
        public List<ZXJH_RealTimeControl> RealTimeControls { get; set; }
        /// <summary>
        /// 处理评估
        /// </summary>
        public List<ZXJH_SYEstimate> SYEstimates { get; set; }
        #endregion

        #region -Methods-
        /// <summary>
        /// 此方法For 运行管理使用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sharedPath">计划文件的共享路径，以/结束</param>
        /// <returns></returns>
        public ZXJH SelectByID(int id, string sharedPath)
        {
            ZXJH result = new ZXJH();
            string fileindex;   //Detail File Path
            List<JH> list;
            list = (new JH()).SelectByIDS(id.ToString());

            if (list.Count <= 0)
            {
                return null;
            }

            fileindex = list[0].FileIndex;
            fileindex = sharedPath + fileindex.Substring(fileindex.LastIndexOf(@"\") + 1);
            result.ID = id;
            result.TaskID = list[0].TaskID;
            result.SatID = list[0].SatID;

            #region 变量
            List<ZXJH_SYContent> listSY = new List<ZXJH_SYContent>();
            List<ZXJH_WorkContent> listWC = new List<ZXJH_WorkContent>();
            List<ZXJH_CommandMake> listCM = new List<ZXJH_CommandMake>();
            List<ZXJH_SYDataHandle> listDH = new List<ZXJH_SYDataHandle>();
            List<ZXJH_DirectAndMonitor> listDM = new List<ZXJH_DirectAndMonitor>();
            List<ZXJH_RealTimeControl> listRC = new List<ZXJH_RealTimeControl>();
            List<ZXJH_SYEstimate> listE = new List<ZXJH_SYEstimate>();

            ZXJH_SYContent sy;
            ZXJH_WorkContent wc;
            ZXJH_CommandMake cm;
            ZXJH_SYDataHandle dh;
            ZXJH_DirectAndMonitor dam;
            ZXJH_RealTimeControl rc;
            ZXJH_SYEstimate sye;

            #endregion
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileindex);
            #region 试验计划
            XmlNode root = xmlDoc.SelectSingleNode("中心运行计划/日期");
            result.Date = root.InnerText;
            root = xmlDoc.SelectSingleNode("中心运行计划/试验计划/对应日期的试验个数");
            result.SYCount = root.InnerText;

            #endregion

            #region listItems
            #region 试验计划
            root = xmlDoc.SelectSingleNode("中心运行计划/试验计划");
            foreach (XmlNode no in root.ChildNodes)
            {
                if (no.Name == "试验内容")
                {
                    sy = new ZXJH_SYContent();
                    foreach (XmlNode n in no.ChildNodes)
                    {
                        switch (n.Name)
                        {
                            case "卫星代号":
                                sy.SatID = n.InnerText;
                                break;
                            case "试验":
                                sy.SYID = n["在当日计划中的ID"].InnerText;
                                sy.SYName = n["试验项目名称"].InnerText;
                                sy.SYStartTime = n["试验开始时间"].InnerText;
                                sy.SYEndTime = n["试验结束时间"].InnerText;
                                sy.SYDays = n["试验运行的天数"].InnerText;
                                sy.SYNote = n["说明"].InnerText;
                                break;
                            case "数传":
                                sy.SY_SCStationNO = n["站编号"].InnerText;
                                sy.SY_SCEquipmentNO = n["设备编号"].InnerText;
                                sy.SY_SCFrequencyBand = n["频段"].InnerText;
                                sy.SY_SCLaps = n["圈次"].InnerText;
                                sy.SY_SCStartTime = n["开始时间"].InnerText;
                                sy.SY_SCEndTime = n["结束时间"].InnerText;
                                break;
                            case "测控":
                                sy.SY_CKStationNO = n["站编号"].InnerText;
                                sy.SY_CKEquipmentNO = n["设备编号"].InnerText;
                                sy.SY_CKLaps = n["圈次"].InnerText;
                                sy.SY_CKStartTime = n["开始时间"].InnerText;
                                sy.SY_CKEndTime = n["结束时间"].InnerText;
                                break;
                            case "注数":
                                sy.SY_ZSFirst = n["最早时间要求"].InnerText;
                                sy.SY_ZSLast = n["最晚时间要求"].InnerText;
                                sy.SY_ZSContent = n["主要内容"].InnerText;
                                break;
                        }
                    }
                    listSY.Add(sy);
                }

                //listSY.Add(sy);
            }
            result.SYContents = listSY;

            #endregion 试验计划

            #region  任务管理
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/任务管理");
            foreach (XmlNode n in root.ChildNodes)
            {
                wc = new ZXJH_WorkContent();
                wc.Work = n["工作"].InnerText;
                wc.SYID = n["对应试验ID"].InnerText;
                wc.StartTime = n["开始时间"].InnerText;
                wc.MinTime = n["最短持续时间"].InnerText;
                wc.MaxTime = n["最长持续时间"].InnerText;
                listWC.Add(wc);
            }
            result.WorkContents = listWC;

            #endregion

            #region  指令制作
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/指令制作");
            foreach (XmlNode n in root.ChildNodes)
            {
                cm = new ZXJH_CommandMake();
                cm.Work_Command_SatID = n["卫星代号"].InnerText;
                cm.Work_Command_SYID = n["对应试验ID"].InnerText;
                cm.Work_Command_Programe = n["对应控制程序"].InnerText;
                cm.Work_Command_FinishTime = n["完成时间"].InnerText;
                cm.Work_Command_UpWay = n["上注方式"].InnerText;
                cm.Work_Command_UpTime = n["上注时间"].InnerText;
                cm.Work_Command_Note = n["说明"].InnerText;
                listCM.Add(cm);
            }
            result.CommandMakes = listCM;

            #endregion

            #region 实时试验数据处理
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/实时试验数据处理");
            foreach (XmlNode n in root.ChildNodes)
            {
                dh = new ZXJH_SYDataHandle();
                dh.SYID = n["对应试验ID"].InnerText;
                dh.SatID = n["卫星代号"].InnerText;
                dh.Laps = n["圈次"].InnerText;
                dh.MainStation = n["主站"].InnerText;
                dh.MainStationEquipment = n["主站设备"].InnerText;
                dh.BakStation = n["备站"].InnerText;
                dh.BakStationEquipment = n["备站设备"].InnerText;
                dh.Content = n["工作内容"].InnerText;
                dh.StartTime = n["实时开始处理时间"].InnerText;
                dh.EndTime = n["实时结束处理时间"].InnerText;
                listDH.Add(dh);
            }
            result.SYDataHandles = listDH;

            #endregion

            #region 指挥与监视
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/指挥与监视");
            foreach (XmlNode n in root.ChildNodes)
            {
                dam = new ZXJH_DirectAndMonitor();
                dam.SYID = n["对应试验ID"].InnerText;
                dam.StartTime = n["开始时间"].InnerText;
                dam.EndTime = n["结束时间"].InnerText;
                dam.RealTimeDemoTask = n["实时演示任务"].InnerText;
                listDM.Add(dam);
            }
            result.DirectAndMonitors = listDM;

            #endregion

            #region 实时控制
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/实时控制");
            foreach (XmlNode n in root.ChildNodes)
            {
                rc = new ZXJH_RealTimeControl();
                rc.Work = n["工作"].InnerText;
                rc.SYID = n["对应试验ID"].InnerText;
                rc.StartTime = n["开始时间"].InnerText;
                rc.EndTime = n["结束时间"].InnerText;
                listRC.Add(rc);
            }
            result.RealTimeControls = listRC;

            #endregion

            #region 处理评估
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/处理评估");
            foreach (XmlNode n in root.ChildNodes)
            {
                sye = new ZXJH_SYEstimate();
                sye.SYID = n["对应试验ID"].InnerText;
                sye.StartTime = n["开始时间"].InnerText;
                sye.EndTime = n["结束时间"].InnerText;
                listE.Add(sye);
            }
            result.SYEstimates = listE;

            #endregion

            #endregion


            return result;
        }
        #endregion
    }

    /// <summary>
    /// 试验计划-试验内容
    /// </summary>
    [Serializable]
    public class ZXJH_SYContent
    {
        /// <summary>
        /// WX代号
        /// </summary>
        public string SatID { get; set; }
        #region 试验
        /// <summary>
        /// 在当日计划中的ID
        /// </summary>
        public string SYID { get; set; }
        /// <summary>
        /// 试验项目名称
        /// </summary>
        public string SYName { get; set; }
        /// <summary>
        /// 试验开始时间
        /// </summary>
        public string SYStartTime { get; set; }
        /// <summary>
        /// 试验结束时间
        /// </summary>
        public string SYEndTime { get; set; }
        /// <summary>
        /// 试验运行的天数
        /// </summary>
        public string SYDays { get; set; }
        /// <summary>
        /// 试验-说明
        /// </summary>
        public string SYNote { get; set; }
        #endregion

        #region 数chuan
        public List<ZXJH_SYContent_SC> SCList { get; set; }
        /// <summary>
        /// 数chuan-站编号
        /// </summary>
        public string SY_SCStationNO { get; set; }
        /// <summary>
        /// 数chuan-设备编号
        /// </summary>
        public string SY_SCEquipmentNO { get; set; }
        /// <summary>
        /// 数chuan-频段; S或者X
        /// </summary>
        public string SY_SCFrequencyBand { get; set; }
        /// <summary>
        /// 数chuan-QC
        /// </summary>
        public string SY_SCLaps { get; set; }
        /// <summary>
        /// 数chuan-开始时间
        /// </summary>
        public string SY_SCStartTime { get; set; }
        /// <summary>
        /// 数chuan-结束时间
        /// </summary>
        public string SY_SCEndTime { get; set; }
        #endregion

        #region 测kong
        public List<ZXJH_SYContent_CK> CKList { get; set; }
        /// <summary>
        /// 测kong-站编号
        /// </summary>
        public string SY_CKStationNO { get; set; }
        /// <summary>
        /// 测kong-设备编号
        /// </summary>
        public string SY_CKEquipmentNO { get; set; }
        /// <summary>
        /// 测kong-QC
        /// </summary>
        public string SY_CKLaps { get; set; }
        /// <summary>
        /// 测kong-开始时间
        /// </summary>
        public string SY_CKStartTime { get; set; }
        /// <summary>
        /// 测kong-结束时间
        /// </summary>
        public string SY_CKEndTime { get; set; }
        #endregion

        #region 注shu
        public List<ZXJH_SYContent_ZS> ZSList { get; set; }
        /// <summary>
        /// 注shu-最早时间要求
        /// </summary>
        public string SY_ZSFirst { get; set; }
        /// <summary>
        /// 注shu-最晚时间要求
        /// </summary>
        public string SY_ZSLast { get; set; }
        /// <summary>
        /// 注shu-主要内容
        /// </summary>
        public string SY_ZSContent { get; set; }
        #endregion
    }

    /// <summary>
    /// 工作计划-试验内容-数chuan
    /// </summary>
    [Serializable]
    public class ZXJH_SYContent_SC : Object, ICloneable
    {
        /// <summary>
        /// 数传-站编号
        /// </summary>
        public string SY_SCStationNO { get; set; }
        /// <summary>
        /// 数传-设备编号
        /// </summary>
        public string SY_SCEquipmentNO { get; set; }
        /// <summary>
        /// 数传-频段; S或者X
        /// </summary>
        public string SY_SCFrequencyBand { get; set; }
        /// <summary>
        /// 数传-QC
        /// </summary>
        public string SY_SCLaps { get; set; }
        /// <summary>
        /// 数传-开始时间
        /// </summary>
        public string SY_SCStartTime { get; set; }
        /// <summary>
        /// 数传-结束时间
        /// </summary>
        public string SY_SCEndTime { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>测kong
    /// 工作计划-试验内容-数chuan
    /// </summary>
    [Serializable]
    public class ZXJH_SYContent_CK : Object, ICloneable
    {
        /// <summary>
        /// 测kong-站编号
        /// </summary>
        public string SY_CKStationNO { get; set; }
        /// <summary>
        /// 测kong-设备编号
        /// </summary>
        public string SY_CKEquipmentNO { get; set; }
        /// <summary>
        /// 测kong-QC
        /// </summary>
        public string SY_CKLaps { get; set; }
        /// <summary>
        /// 测kong-开始时间
        /// </summary>
        public string SY_CKStartTime { get; set; }
        /// <summary>
        /// 测kong-结束时间
        /// </summary>
        public string SY_CKEndTime { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>测kong
    /// 工作计划-试验内容-注shu
    /// </summary>
    [Serializable]
    public class ZXJH_SYContent_ZS : Object, ICloneable
    {
        /// <summary>
        /// 注shu-最早时间要求
        /// </summary>
        public string SY_ZSFirst { get; set; }
        /// <summary>
        /// 注shu-最晚时间要求
        /// </summary>
        public string SY_ZSLast { get; set; }
        /// <summary>
        /// 注shu-主要内容
        /// </summary>
        public string SY_ZSContent { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// 工作计划-任务管理
    /// </summary>
    [Serializable]
    public class ZXJH_WorkContent : Object, ICloneable
    {
        #region -Properties-
        /// <summary>
        /// 工作: 试验规划、计划管理、试验数据处理
        /// </summary>
        public string Work { get; set; }
        /// <summary>
        /// 对应试验ID
        /// </summary>
        public string SYID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 最短持续时间
        /// </summary>
        public string MinTime { get; set; }
        /// <summary>
        /// 最长持续时间
        /// </summary>
        public string MaxTime { get; set; }
        /// <summary>
        /// 完成状态
        /// </summary>
        public string Status { get; set; }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// 工作计划-指Ling制作
    /// </summary>
    [Serializable]
    public class ZXJH_CommandMake
    {
        /// <summary>
        /// 指Ling制作-WX代号
        /// </summary>
        public string Work_Command_SatID { get; set; }
        /// <summary>
        /// 指Ling制作-对应试验ID
        /// </summary>
        public string Work_Command_SYID { get; set; }
        /// <summary>
        /// 指Ling制作-对应控制程序
        /// </summary>
        public string Work_Command_Programe { get; set; }
        /// <summary>
        /// 指Ling制作-完成时间
        /// </summary>
        public string Work_Command_FinishTime { get; set; }
        /// <summary>
        /// 指Ling制作-上zhu方式
        /// </summary>
        public string Work_Command_UpWay { get; set; }
        /// <summary>
        /// 指Ling制作-上zhu时间/QC
        /// </summary>
        public string Work_Command_UpTime { get; set; }
        /// <summary>
        /// 指Ling制作-说明
        /// </summary>
        public string Work_Command_Note { get; set; }
        /// <summary>
        /// 完成状态
        /// </summary>
        public string Status { get; set; }
    }

    /// <summary>
    /// 工作计划-RealTime试验数据处理
    /// </summary>
    [Serializable]
    public class ZXJH_SYDataHandle : Object, ICloneable
    {
        #region -Properties-
        /// <summary>
        /// 对应试验ID
        /// </summary>
        public string SYID { get; set; }
        /// <summary>
        /// WX代号
        /// </summary>
        public string SatID { get; set; }
        /// <summary>
        /// QC
        /// </summary>
        public string Laps { get; set; }
        /// <summary>
        /// 主zhan
        /// </summary>
        public string MainStation { get; set; }
        /// <summary>
        /// 主zhan设备
        /// </summary>
        public string MainStationEquipment { get; set; }
        /// <summary>
        /// 备站
        /// </summary>
        public string BakStation { get; set; }
        /// <summary>
        /// 备站设备
        /// </summary>
        public string BakStationEquipment { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// RealTime开始处理时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// RealTime结束处理时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 完成状态
        /// </summary>
        public string Status { get; set; }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// 工作计划-指挥与监视
    /// </summary>
    [Serializable]
    public class ZXJH_DirectAndMonitor
    {
        #region -Properties-
        /// <summary>
        /// 对应试验ID
        /// </summary>
        public string SYID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// RealTime演示任务：有/无
        /// </summary>
        public string RealTimeDemoTask { get; set; }
        /// <summary>
        /// 完成状态
        /// </summary>
        public string Status { get; set; }
        #endregion
    }

    /// <summary>
    /// 工作计划-RealTime控制
    /// </summary>
    [Serializable]
    public class ZXJH_RealTimeControl
    {
        #region -Properties-
        /// <summary>
        /// 工作
        /// </summary>
        public string Work { get; set; }
        /// <summary>
        /// 对应试验ID
        /// </summary>
        public string SYID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 完成状态
        /// </summary>
        public string Status { get; set; }
        #endregion
    }

    /// <summary>
    /// 工作计划-处理评估
    /// </summary>
    [Serializable]
    public class ZXJH_SYEstimate
    {
        #region -Properties-
        /// <summary>
        /// 对应试验ID
        /// </summary>
        public string SYID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 完成状态
        /// </summary>
        public string Status { get; set; }
        #endregion
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Configuration;
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.Framework.Storage;
using OperatingManagement.Framework.Components;
using ServicesKernel.GDFX;

namespace ServicesKernel.File
{
    public class PlanFileCreator
    {
        public PlanFileCreator()
        {
            isTempJH = false;   //默认为正式计划
        }

        public PlanFileCreator(bool istempjh)
        {
            isTempJH = istempjh;
        }

        #region -Properties-
        StreamReader sr;
        StreamWriter sw;
        string filename = "";
        XmlTextWriter xmlWriter;
        XmlTextReader xmlReader;
        XmlDocument xmldoc;
        XmlDeclaration xmldecl;
        XmlNode xmlnode;
        XmlElement xmlelem;

        /// <summary>
        /// 是否为临时计划：true-临时计划；false-正式计划
        /// </summary>
        public bool isTempJH { get; set; }
        public int ID { get; set; }
        public DateTime CTime { get; set; }
        /// <summary>
        /// 发送源名称
        /// </summary>
        public string Source
        {
            get
            {
                return Param.SourceName;
            }
            set { }
        }
        public string Destination { get; set; }
        public string TaskID { get; set; }
        public string InfoType { get; set; }
        public int LineCount { get; set; }
        public string Format1 { get; set; }
        public string Format2 { get; set; }
        public string DataSection { get; set; }
        public string FileIndex { get; set; }
        public string Reserve { get; set; }

        private string strOutputPath = null;
        private string strSavePath = null;
        private string _filepath = null;
        private const string strSplitorTwoBlanks = "  ";
        /// <summary>
        /// 内部文件完整路径
        /// </summary>
        public string FilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_filepath))
                {
                    return SavePath + filename;
                }
                else
                {
                    return _filepath;
                }
            }
            set
            {
                _filepath = value;
            }
        }
        /// <summary>
        /// 外发文件完整路径
        /// </summary>
        public string SendingPath
        {
            get
            {
                if (string.IsNullOrEmpty(_filepath))
                {
                    return OutPutPath + filename;
                }
                else
                {
                    return _filepath;
                }
            }
            set
            {
                _filepath = value;
            }
        }
        /// <summary>
        /// 内部文件保存目录
        /// </summary>
        public string SavePath
        {
            get
            {
                if (string.IsNullOrEmpty(strSavePath))
                {
                    strSavePath = Param.SavePath;
                    if (isTempJH)
                    {
                        strSavePath = Param.TempJHSavePath;
                    }
                }
                return strSavePath;
            }
        }
        /// <summary>
        /// 外发文件保存目录
        /// </summary>
        public string OutPutPath
        {
            get
            {
                if (string.IsNullOrEmpty(strOutputPath))
                    strOutputPath = Param.OutPutPath;
                return strOutputPath;
            }
        }

        #endregion

        #region <Internal file>

        /// <summary>
        /// 应用研究
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type">0:Add;1:Edit</param>
        /// <returns></returns>
        public string CreateYJJHFile(YJJH obj, int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("YJJH", obj.TaskID, obj.SatID);
                FilePath = SavePath + filename;
            }
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("应用研究工作计划");

            xmlWriter.WriteStartElement("XXFL");
            xmlWriter.WriteString(obj.XXFL);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("JXH");
            xmlWriter.WriteString(obj.JXH);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("SysName");
            xmlWriter.WriteString(obj.SysName);
            xmlWriter.WriteEndElement();

            foreach (YJJH_Task task in obj.Tasks)
            {
                xmlWriter.WriteStartElement("Work");
                xmlWriter.WriteStartElement("StartTime");
                xmlWriter.WriteString(task.StartTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("EndTime");
                xmlWriter.WriteString(task.EndTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Task");
                xmlWriter.WriteString(task.Task);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Close();

            return FilePath;
        }

        /// <summary>
        /// 空间信息需求计划
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type">0:新增; 1:修改</param>
        /// <returns></returns>
        public string CreateXXXQFile(XXXQ obj, int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("XXXQ", obj.TaskID, obj.SatID);
            }
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("空间信息需求");

            #region MBXQ
            xmlWriter.WriteStartElement("空间目标信息需求");

            xmlWriter.WriteStartElement("User");
            xmlWriter.WriteString(obj.objMBXQ.User);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Time");
            xmlWriter.WriteString(obj.objMBXQ.Time);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TargetInfo");
            xmlWriter.WriteString(obj.objMBXQ.TargetInfo);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TimeSection1");
            xmlWriter.WriteString(obj.objMBXQ.TimeSection1);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TimeSection2");
            xmlWriter.WriteString(obj.objMBXQ.TimeSection2);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Sum");
            xmlWriter.WriteString(obj.objMBXQ.Sum);
            xmlWriter.WriteEndElement();


            for (int i = 1; i <= obj.objMBXQ.SatInfos.Count; i++)
            {
                xmlWriter.WriteStartElement("卫星");

                xmlWriter.WriteStartElement("SatName");
                xmlWriter.WriteString(obj.objMBXQ.SatInfos[i - 1].SatName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoName");
                xmlWriter.WriteString(obj.objMBXQ.SatInfos[i - 1].InfoName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoTime");
                xmlWriter.WriteString(obj.objMBXQ.SatInfos[i - 1].InfoTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }



            xmlWriter.WriteEndElement();
            #endregion

            #region HJXQ
            xmlWriter.WriteStartElement("空间环境信息需求");

            xmlWriter.WriteStartElement("User");
            xmlWriter.WriteString(obj.objHJXQ.User);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Time");
            xmlWriter.WriteString(obj.objHJXQ.Time);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("EnvironInfo");
            xmlWriter.WriteString(obj.objHJXQ.EnvironInfo);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TimeSection1");
            xmlWriter.WriteString(obj.objHJXQ.TimeSection1);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TimeSection2");
            xmlWriter.WriteString(obj.objHJXQ.TimeSection2);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Sum");
            xmlWriter.WriteString(obj.objHJXQ.Sum);
            xmlWriter.WriteEndElement();


            for (int i = 1; i <= obj.objHJXQ.SatInfos.Count; i++)
            {
                xmlWriter.WriteStartElement("卫星");

                xmlWriter.WriteStartElement("SatName");
                xmlWriter.WriteString(obj.objHJXQ.SatInfos[i - 1].SatName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoName");
                xmlWriter.WriteString(obj.objHJXQ.SatInfos[i - 1].InfoName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoArea");
                xmlWriter.WriteString(obj.objHJXQ.SatInfos[i - 1].InfoArea);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoTime");
                xmlWriter.WriteString(obj.objHJXQ.SatInfos[i - 1].InfoTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }


            xmlWriter.WriteEndElement();
            #endregion

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            return FilePath;
        }

        /// <summary>
        /// 内部存储ZZDMZ
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type">0:新增; 1:修改</param>
        /// <returns></returns>
        public string CreateZZGZJHFile(ZZGZJH obj, int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("ZZGZJH", obj.TaskID, obj.SatID);
                FilePath = SavePath + filename;
            }

            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("总装地面站工作计划");
            #region basicinfo
            xmlWriter.WriteStartElement("编号");
            xmlWriter.WriteString(obj.Sequence);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("时间");
            xmlWriter.WriteString(obj.DateTime);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("工作单位");
            xmlWriter.WriteString(obj.StationName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("设备代号");
            xmlWriter.WriteString(obj.EquipmentID);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("任务个数");
            xmlWriter.WriteString(obj.TaskCount);
            xmlWriter.WriteEndElement();
            #endregion

            #region 任务
            for (int i = 0; i < obj.DMJHTasks.Count; i++)
            {
                xmlWriter.WriteStartElement("任务");
                #region BasicInfo
                xmlWriter.WriteStartElement("任务标志");
                xmlWriter.WriteString(obj.DMJHTasks[i].TaskFlag);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("工作方式");
                xmlWriter.WriteString(obj.DMJHTasks[i].WorkWay);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("计划性质");
                xmlWriter.WriteString(obj.DMJHTasks[i].PlanPropertiy);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("工作模式");
                xmlWriter.WriteString(obj.DMJHTasks[i].WorkMode);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("任务准备开始时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].PreStartTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("任务开始时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].StartTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("跟踪开始时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].TrackStartTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("开上行载波时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].WaveOnStartTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("关上行载波时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].WaveOffStartTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("跟踪结束时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].TrackEndTime);
                xmlWriter.WriteEndElement();
                #endregion

                #region 实时传输
                for (int j = 0; j < obj.DMJHTasks[i].ReakTimeTransfors.Count; j++)
                {
                    xmlWriter.WriteStartElement("实时传输");

                    xmlWriter.WriteStartElement("格式标志");
                    xmlWriter.WriteString(obj.DMJHTasks[i].ReakTimeTransfors[j].FormatFlag);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("信息流标志");
                    xmlWriter.WriteString(obj.DMJHTasks[i].ReakTimeTransfors[j].InfoFlowFlag);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据传输开始时间");
                    xmlWriter.WriteString(obj.DMJHTasks[i].ReakTimeTransfors[j].TransStartTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据传输结束时间");
                    xmlWriter.WriteString(obj.DMJHTasks[i].ReakTimeTransfors[j].TransEndTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据传输速率");
                    xmlWriter.WriteString(obj.DMJHTasks[i].ReakTimeTransfors[j].TransSpeedRate);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                #endregion

                #region 事后回放
                for (int j = 0; j < obj.DMJHTasks[i].AfterFeedBacks.Count; j++)
                {
                    xmlWriter.WriteStartElement("事后回放");

                    xmlWriter.WriteStartElement("格式标志");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].FormatFlag);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("信息流标志");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].InfoFlowFlag);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据起始时间");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].DataStartTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据结束时间");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].DataEndTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据传输开始时间");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].TransStartTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据传输速率");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].TransSpeedRate);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                }
                #endregion
                xmlWriter.WriteStartElement("任务结束时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].EndTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }
            #endregion
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            return FilePath;
        }

        /// <summary>
        /// 内部存储测控资源使用申请
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type">0:新增; 1:修改</param>
        /// <returns></returns>
        public string CreateDMJHFile(DJZYSQ obj, int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("DJZYSQ", obj.TaskID, obj.SatID);
                FilePath = SavePath + filename;
            }
            Logger.GetLogger().Error("DJZYSQ:" + filename + " | SatID" + obj.SatID + " | TaskID" + obj.TaskID);
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("测控资源使用申请");
            #region basicinfo
            xmlWriter.WriteStartElement("时间");
            xmlWriter.WriteString(obj.SJ);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("申请序列号");
            xmlWriter.WriteString(obj.SNO);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("航天器标识");
            xmlWriter.WriteString(obj.SCID);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("申请数量");
            xmlWriter.WriteString(obj.SNUM);
            xmlWriter.WriteEndElement();
            #endregion

            #region 申请
            xmlWriter.WriteStartElement("申请");
            for (int i = 0; i < obj.DMJHTasks.Count; i++)
            {
                xmlWriter.WriteStartElement("申请内容");
                #region BasicInfo
                xmlWriter.WriteStartElement("申请序号");
                xmlWriter.WriteString(obj.DMJHTasks[i].SXH);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("申请性质");
                xmlWriter.WriteString(obj.DMJHTasks[i].SXZ);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("任务类别");
                xmlWriter.WriteString(obj.DMJHTasks[i].MLB);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("工作方式");
                xmlWriter.WriteString(obj.DMJHTasks[i].FS);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("工作单元");
                xmlWriter.WriteString(obj.DMJHTasks[i].GZDY);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("设备代号");
                xmlWriter.WriteString(obj.DMJHTasks[i].SBDH);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("圈次");
                xmlWriter.WriteString(obj.DMJHTasks[i].QC);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("圈标");
                xmlWriter.WriteString(obj.DMJHTasks[i].QB);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("测控事件类型");
                xmlWriter.WriteString(obj.DMJHTasks[i].SHJ);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("工作点频数量");
                xmlWriter.WriteString(obj.DMJHTasks[i].FNUM);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("工作点频");
                #region 工作点频内容
                for (int j = 0; j < obj.DMJHTasks[i].GZDPs.Count; j++)
                {
                    xmlWriter.WriteStartElement("工作点频内容");
                    xmlWriter.WriteStartElement("点频序号");
                    xmlWriter.WriteString(obj.DMJHTasks[i].GZDPs[j].FXH);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("频段选择");
                    xmlWriter.WriteString(obj.DMJHTasks[i].GZDPs[j].PDXZ);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("点频选择");
                    xmlWriter.WriteString(obj.DMJHTasks[i].GZDPs[j].DPXZ);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                #endregion
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("同时支持目标数");
                xmlWriter.WriteString(obj.DMJHTasks[i].TNUM);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("任务准备开始时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].ZHB);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("任务开始时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].RK);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("跟踪开始时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].GZK);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("开上行载波时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].KSHX);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("关上行载波时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].GSHX);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("跟踪结束时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].GZJ);
                xmlWriter.WriteEndElement();
                #endregion

                #region 实时传输
                for (int j = 0; j < obj.DMJHTasks[i].ReakTimeTransfors.Count; j++)
                {
                    xmlWriter.WriteStartElement("实时传输");

                    xmlWriter.WriteStartElement("格式标志");
                    xmlWriter.WriteString(obj.DMJHTasks[i].ReakTimeTransfors[j].GBZ);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("信息流标志");
                    xmlWriter.WriteString(obj.DMJHTasks[i].ReakTimeTransfors[j].XBZ);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据传输开始时间");
                    xmlWriter.WriteString(obj.DMJHTasks[i].ReakTimeTransfors[j].RTs);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据传输结束时间");
                    xmlWriter.WriteString(obj.DMJHTasks[i].ReakTimeTransfors[j].RTe);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据传输速率");
                    xmlWriter.WriteString(obj.DMJHTasks[i].ReakTimeTransfors[j].SL);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                #endregion

                #region 事后回放
                for (int j = 0; j < obj.DMJHTasks[i].AfterFeedBacks.Count; j++)
                {
                    xmlWriter.WriteStartElement("事后回放");

                    xmlWriter.WriteStartElement("格式标志");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].GBZ);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("信息流标志");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].XBZ);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据起始时间");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].Ts);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据结束时间");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].Te);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据传输开始时间");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].RTs);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("数据传输速率");
                    xmlWriter.WriteString(obj.DMJHTasks[i].AfterFeedBacks[j].SL);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                }
                #endregion
                xmlWriter.WriteStartElement("任务结束时间");
                xmlWriter.WriteString(obj.DMJHTasks[i].JS);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            #endregion
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            return FilePath;
        }
        /// <summary>
        /// ZC地面站工作计划
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type">0:Add;1:Edit</param>
        /// <returns></returns>
        public string CreateGZJHFile(GZJH obj, int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("GZJH", obj.TaskID, obj.SatID);
                FilePath = SavePath + filename;
            }
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("地面站工作计划");

            xmlWriter.WriteStartElement("JXH");
            xmlWriter.WriteString(obj.JXH);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("XXFL");
            xmlWriter.WriteString(obj.XXFL);
            xmlWriter.WriteEndElement();

            //xmlWriter.WriteStartElement("QS");
            //xmlWriter.WriteString(obj.QS);
            //xmlWriter.WriteEndElement();

            #region Content
            for (int i = 1; i <= obj.GZJHContents.Count; i++)
            {
                xmlWriter.WriteStartElement("Content");

                xmlWriter.WriteStartElement("DW");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].DW);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("SB");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].SB);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("QS");
                xmlWriter.WriteString(obj.GZJHContents.Where(t => t.DW == obj.GZJHContents[i - 1].DW && t.SB == obj.GZJHContents[i - 1].SB).ToList().Count().ToString());
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("QH");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].QH);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("DH");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].DH);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("FS");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].FS);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("JXZ");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].JXZ);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("MS");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].MS);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("QB");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].QB);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("GXZ");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].GXZ);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ZHB");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].ZHB);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("RK");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].RK);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("GZK");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].GZK);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("KSHX");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].KSHX);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("GSHX");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].GSHX);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("GZJ");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].GZJ);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("JS");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].JS);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("BID");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].BID);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("SBZ");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].SBZ);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("RTs");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].RTs);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("RTe");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].RTe);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("SL");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].SL);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("HBID");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].HBID);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("HBZ");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].HBZ);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Ts");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].Ts);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Te");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].Te);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("HRTs");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].HRTs);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("HSL");
                xmlWriter.WriteString(obj.GZJHContents[i - 1].HSL);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }
            #endregion

            xmlWriter.WriteEndElement();
            xmlWriter.Close();

            return FilePath;
        }
        /// <summary>
        /// 中心计划
        /// </summary>
        public string CreateZXJHFile(ZXJH obj, int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("ZXJH", obj.TaskID, obj.SatID);
                FilePath = SavePath + filename;
            }
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("中心运行计划");
            xmlWriter.WriteStartElement("日期");
            xmlWriter.WriteString(obj.Date);
            xmlWriter.WriteEndElement();

            #region 试验计划
            xmlWriter.WriteStartElement("试验计划");

            xmlWriter.WriteStartElement("对应日期的试验个数");
            xmlWriter.WriteString(obj.SYCount);
            xmlWriter.WriteEndElement();
            #region 试验内容
            for (int i = 1; i <= obj.SYContents.Count; i++)
            {

                xmlWriter.WriteStartElement("试验内容");

                xmlWriter.WriteStartElement("卫星代号");
                xmlWriter.WriteString(obj.SYContents[i - 1].SatID);
                xmlWriter.WriteEndElement();

                #region 试验
                xmlWriter.WriteStartElement("试验");
                xmlWriter.WriteStartElement("在当日计划中的ID");
                xmlWriter.WriteString(obj.SYContents[i - 1].SYID);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("试验项目名称");
                xmlWriter.WriteString(obj.SYContents[i - 1].SYName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("试验开始时间");
                xmlWriter.WriteString(obj.SYContents[i - 1].SYStartTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("试验结束时间");
                xmlWriter.WriteString(obj.SYContents[i - 1].SYEndTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("试验运行的天数");
                xmlWriter.WriteString(obj.SYContents[i - 1].SYDays);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("说明");
                xmlWriter.WriteString(obj.SYContents[i - 1].SYNote);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                #endregion
                #region 数传
                if (obj.SYContents[i - 1].SCList != null)
                {
                    foreach (ZXJH_SYContent_SC sc in obj.SYContents[i - 1].SCList)
                    {
                        xmlWriter.WriteStartElement("数传");

                        xmlWriter.WriteStartElement("站编号");
                        xmlWriter.WriteString(sc.SY_SCStationNO);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("设备编号");
                        xmlWriter.WriteString(sc.SY_SCEquipmentNO);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("频段");
                        xmlWriter.WriteString(sc.SY_SCFrequencyBand);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("圈次");
                        xmlWriter.WriteString(sc.SY_SCLaps);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("开始时间");
                        xmlWriter.WriteString(sc.SY_SCStartTime);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("结束时间");
                        xmlWriter.WriteString(sc.SY_SCEndTime);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteEndElement();
                    }
                }
                #endregion
                #region 测控
                if (obj.SYContents[i - 1].CKList != null)
                {
                    foreach (ZXJH_SYContent_CK ck in obj.SYContents[i - 1].CKList)
                    {
                        xmlWriter.WriteStartElement("测控");

                        xmlWriter.WriteStartElement("站编号");
                        xmlWriter.WriteString(ck.SY_CKStationNO);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("设备编号");
                        xmlWriter.WriteString(ck.SY_CKEquipmentNO);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("圈次");
                        xmlWriter.WriteString(ck.SY_CKLaps);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("开始时间");
                        xmlWriter.WriteString(ck.SY_CKStartTime);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("结束时间");
                        xmlWriter.WriteString(ck.SY_CKEndTime);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteEndElement();
                    }
                }
                #endregion
                #region 注数
                if (obj.SYContents[i - 1].ZSList != null)
                {
                    foreach (ZXJH_SYContent_ZS zs in obj.SYContents[i - 1].ZSList)
                    {
                        xmlWriter.WriteStartElement("注数");

                        xmlWriter.WriteStartElement("最早时间要求");
                        xmlWriter.WriteString(zs.SY_ZSFirst);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("最晚时间要求");
                        xmlWriter.WriteString(zs.SY_ZSLast);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("主要内容");
                        xmlWriter.WriteString(zs.SY_ZSContent);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteEndElement();
                    }
                }
                #endregion

                xmlWriter.WriteEndElement();
            }
            #endregion

            xmlWriter.WriteEndElement();
            #endregion

            #region 工作计划
            xmlWriter.WriteStartElement("工作计划");
            #region 任务管理
            xmlWriter.WriteStartElement("任务管理");
            if (obj.WorkContents != null)
            {
                for (int i = 1; i <= obj.WorkContents.Count; i++)
                {
                    xmlWriter.WriteStartElement("工作内容" + i.ToString());

                    xmlWriter.WriteStartElement("工作");
                    xmlWriter.WriteString(obj.WorkContents[i - 1].Work);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("对应试验ID");
                    xmlWriter.WriteString(obj.WorkContents[i - 1].SYID);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("开始时间");
                    xmlWriter.WriteString(obj.WorkContents[i - 1].StartTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("最短持续时间");
                    xmlWriter.WriteString(obj.WorkContents[i - 1].MinTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("最长持续时间");
                    xmlWriter.WriteString(obj.WorkContents[i - 1].MaxTime);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            #endregion
            #region 指令制作
            if (obj.CommandMakes != null)
            {
                xmlWriter.WriteStartElement("指令制作");
                for (int i = 1; i <= obj.CommandMakes.Count; i++)
                {
                    xmlWriter.WriteStartElement("工作内容" + i.ToString());

                    xmlWriter.WriteStartElement("卫星代号");
                    xmlWriter.WriteString(obj.CommandMakes[i - 1].Work_Command_SatID);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("对应试验ID");
                    xmlWriter.WriteString(obj.CommandMakes[i - 1].Work_Command_SYID);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("对应控制程序");
                    xmlWriter.WriteString(obj.CommandMakes[i - 1].Work_Command_Programe);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("完成时间");
                    xmlWriter.WriteString(obj.CommandMakes[i - 1].Work_Command_FinishTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("上注方式");
                    xmlWriter.WriteString(obj.CommandMakes[i - 1].Work_Command_UpWay);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("上注时间");
                    xmlWriter.WriteString(obj.CommandMakes[i - 1].Work_Command_UpTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("说明");
                    xmlWriter.WriteString(obj.CommandMakes[i - 1].Work_Command_Note);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            #endregion
            #region 实时试验数据处理
            xmlWriter.WriteStartElement("实时试验数据处理");
            if (obj.SYDataHandles != null)
            {
                for (int i = 1; i <= obj.SYDataHandles.Count; i++)
                {
                    xmlWriter.WriteStartElement("工作内容" + i.ToString());

                    xmlWriter.WriteStartElement("对应试验ID");
                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].SYID);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("卫星代号");
                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].SatID);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("圈次");
                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].Laps);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("主站");
                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].MainStation);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("主站设备");
                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].MainStationEquipment);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("备站");
                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].BakStation);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("备站设备");
                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].BakStationEquipment);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("工作内容");
                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].Content);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("实时开始处理时间");
                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].StartTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("实时结束处理时间");
                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].EndTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
            #endregion
            #region 指挥与监视
            if (obj.DirectAndMonitors != null)
            {
                xmlWriter.WriteStartElement("指挥与监视");
                for (int i = 1; i <= obj.DirectAndMonitors.Count; i++)
                {
                    xmlWriter.WriteStartElement("工作内容" + i.ToString());

                    xmlWriter.WriteStartElement("对应试验ID");
                    xmlWriter.WriteString(obj.DirectAndMonitors[i - 1].SYID);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("开始时间");
                    xmlWriter.WriteString(obj.DirectAndMonitors[i - 1].StartTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("结束时间");
                    xmlWriter.WriteString(obj.DirectAndMonitors[i - 1].EndTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("实时演示任务");
                    xmlWriter.WriteString(obj.DirectAndMonitors[i - 1].RealTimeDemoTask);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            #endregion
            #region 实时控制
            if (obj.RealTimeControls != null)
            {
                xmlWriter.WriteStartElement("实时控制");
                for (int i = 1; i <= obj.RealTimeControls.Count; i++)
                {
                    xmlWriter.WriteStartElement("工作内容" + i.ToString());

                    xmlWriter.WriteStartElement("工作");
                    xmlWriter.WriteString(obj.RealTimeControls[i - 1].Work);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("对应试验ID");
                    xmlWriter.WriteString(obj.RealTimeControls[i - 1].SYID);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("开始时间");
                    xmlWriter.WriteString(obj.RealTimeControls[i - 1].StartTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("结束时间");
                    xmlWriter.WriteString(obj.RealTimeControls[i - 1].EndTime);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            #endregion
            #region 处理评估
            if (obj.SYEstimates != null)
            {
                xmlWriter.WriteStartElement("处理评估");
                for (int i = 1; i <= obj.SYEstimates.Count; i++)
                {
                    xmlWriter.WriteStartElement("工作内容" + i.ToString());

                    xmlWriter.WriteStartElement("对应试验ID");
                    xmlWriter.WriteString(obj.SYEstimates[i - 1].SYID);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("开始时间");
                    xmlWriter.WriteString(obj.SYEstimates[i - 1].StartTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("结束时间");
                    xmlWriter.WriteString(obj.SYEstimates[i - 1].EndTime);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            #endregion

            xmlWriter.WriteEndElement();
            #endregion
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            return FilePath;
        }
        /// <summary>
        /// 仿真推演
        /// </summary>
        public string CreateTYSJFile(TYSJ obj, int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("TYSJ", obj.TaskID, obj.SatID);
            }
            if (TestFileName())
            {
                return "EXIST";
            }
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("仿真推演试验数据");

            #region 内容
            for (int i = 1; i <= obj.SYContents.Count; i++)
            {
                xmlWriter.WriteStartElement("Content");

                xmlWriter.WriteStartElement("SatName");
                xmlWriter.WriteString(obj.SYContents[i - 1].SatName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Type");
                xmlWriter.WriteString(obj.SYContents[i - 1].Type);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("TestItem");
                xmlWriter.WriteString(obj.SYContents[i - 1].TestItem);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("StartTime");
                xmlWriter.WriteString(obj.SYContents[i - 1].StartTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("EndTime");
                xmlWriter.WriteString(obj.SYContents[i - 1].EndTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Condition");
                xmlWriter.WriteString(obj.SYContents[i - 1].Condition);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }

            #endregion

            xmlWriter.WriteEndElement();

            xmlWriter.Close();
            return FilePath;
        }

        /// <summary>
        /// 创建实验计划外发文件 xml格式-文件类型一
        /// 返回0： 没有创建文件； 1：创建文件成功
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filename"></param>
        private int CreateSYJHFile(SYJH obj, string filename)
        {
            if (obj.SYJH_SY_List.Count <= 0)
            {
                return 0;
            }

            xmlWriter = new XmlTextWriter(filename, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("试验计划");

            xmlWriter.WriteStartElement("编号");
            xmlWriter.WriteString(obj.JHID.ToString("0000"));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("时间");
            xmlWriter.WriteString(obj.CreateTime);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("试验个数");
            xmlWriter.WriteString(obj.SYCount);
            xmlWriter.WriteEndElement();

            #region 试验
            foreach (SYJH_SY sy in obj.SYJH_SY_List)
            {
                xmlWriter.WriteStartElement("试验");

                xmlWriter.WriteStartElement("卫星名称");
                xmlWriter.WriteString(sy.SYSatName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("试验类别");
                xmlWriter.WriteString(sy.SYType);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("试验项目");
                xmlWriter.WriteString(sy.SYItem);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("开始时间");
                xmlWriter.WriteString(sy.SYStartTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("结束时间");
                xmlWriter.WriteString(sy.SYEndTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("系统名称");
                xmlWriter.WriteString(sy.SYSysName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("系统任务");
                xmlWriter.WriteString(sy.SYSysTask);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }
            #endregion

            xmlWriter.WriteEndElement();

            xmlWriter.Close();

            return 1;
        }
        /// <summary>
        /// 测试要生成的文件文件名是否已存在
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool TestYJJHFileName(YJJH obj)
        {
            filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("YJJH", obj.TaskID, obj.SatID);
            return TestFileName();
        }
        /// <summary>
        /// 测试要生成的文件文件名是否已存在
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool TestXXXQFileName(XXXQ obj)
        {
            filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("XXXQ", obj.TaskID, obj.SatID);
            return TestFileName();
        }
        /// <summary>
        /// 测试要生成的文件文件名是否已存在
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool TestZZGZJHFileName(ZZGZJH obj)
        {
            filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("ZZGZJH", obj.TaskID, obj.SatID);
            return TestFileName();
        }
        /// <summary>
        /// 测试要生成的文件文件名是否已存在
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool TestDMJHFileName(DJZYSQ obj)
        {
            filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("DMJH", obj.TaskID, obj.SatID);
            return TestFileName();
        }
        /// <summary>
        /// 测试要生成的文件文件名是否已存在
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool TestGZJHFileName(GZJH obj)
        {
            filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("GZJH", obj.TaskID, obj.SatID);
            return TestFileName();
        }
        /// <summary>
        /// 测试要生成的文件文件名是否已存在
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool TestZXJHFileName(ZXJH obj)
        {
            filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("ZXJH", obj.TaskID, obj.SatID);
            return TestFileName();
        }
        /// <summary>
        /// 测试要生成的文件文件名是否已存在
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool TestTYSJFileName(TYSJ obj)
        {
            filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("TYSJ", obj.TaskID, obj.SatID);
            return TestFileName();
        }
        #endregion

        #region <External File>
        /// <summary>
        /// 创建外部应用计划文件
        /// </summary>
        /// <param name="ids">计划ID串</param>
        /// <param name="desValue">信宿地址值</param>
        /// <param name="destinationName">信宿地址名称</param>
        /// <returns>生成的外发文件完整路径名串</returns>
        public string CreateSendingSYJHFile(string ids, string desValue, string runningMode)
        {
            XYXSInfo info = new XYXSInfo();
            info = info.GetByAddrMark(desValue);
            string destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";
            OperatingManagement.DataAccessLayer.BusinessManage.InfoType itype = new OperatingManagement.DataAccessLayer.BusinessManage.InfoType();

            string SendFileNames = "";
            List<JH> listJH = (new JH()).SelectByIDS(ids);
            string strFilePath = System.Configuration.ConfigurationManager.AppSettings["SYJHPath"];
            string strFileName = string.Empty;
            SYJH obj;
            SYJH_SY task;
            string dataCode = PlanParameters.ReadParamValue("SYJHDataCode");
            Task oTask = new Task();
            foreach (JH jh in listJH)
            {
                string[] sysnames;   //系统名称,逗号分隔
                string[] systasks;       //系统任务,逗号分隔

                obj = new SYJH();
                obj.TaskID = jh.TaskID;
                obj.SatID = jh.SatID;
                obj.SYJH_SY_List = new List<SYJH_SY>();
                strFileName = jh.FileIndex.Substring(jh.FileIndex.LastIndexOf(@"\") + 1);
                if (System.IO.File.Exists(Path.Combine(strFilePath, strFileName)))
                {
                    #region 读取计划XML文件，变量赋值
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(Path.Combine(strFilePath, strFileName));

                    obj.CTime = DateTime.Now;
                    XmlNode root = xmlDoc.SelectSingleNode("试验计划/编号");
                    obj.JHID = Convert.ToInt32(root.InnerText);
                    root = xmlDoc.SelectSingleNode("试验计划/时间");
                    obj.CreateTime = root.InnerText;
                    root = xmlDoc.SelectSingleNode("试验计划/试验个数");
                    obj.SYCount = root.InnerText;
                    root = xmlDoc.SelectSingleNode("试验计划");
                    foreach (XmlNode n in root.ChildNodes)
                    {
                        if (n.Name == "试验")
                        {
                            sysnames = n["系统名称"].InnerText.Split('|');
                            systasks = n["系统任务"].InnerText.Split('|');

                            for (int i = 0; i < sysnames.Length; i++)
                            {
                                if (desValue != ConfigurationManager.AppSettings["ZXBM"])
                                {
                                    if (desValue == sysnames[i]) //名称编码与发送目标一致时才发送，所以只保留编码相同的试验
                                    {
                                        task = new SYJH_SY();
                                        task.SYSatName = n["卫星名称"].InnerText;
                                        task.SYType = n["试验类别"].InnerText;
                                        task.SYItem = n["试验项目"].InnerText;
                                        task.SYStartTime = n["开始时间"].InnerText;
                                        task.SYEndTime = n["结束时间"].InnerText;
                                        //task.SYSysName = n["系统名称"].InnerText;
                                        //task.SYSysTask = n["系统任务"].InnerText;
                                        task.SYSysName = info.GetByAddrMark(sysnames[i]).ADDRName; //文件里存的是编码，发送时转成中文
                                        task.SYSysTask = systasks[i];
                                        obj.SYJH_SY_List.Add(task);
                                    }
                                }
                                else//发给中心的是中心内部要生成文件
                                {
                                    task = new SYJH_SY();
                                    task.SYSatName = n["卫星名称"].InnerText;
                                    task.SYType = n["试验类别"].InnerText;
                                    task.SYItem = n["试验项目"].InnerText;
                                    task.SYStartTime = n["开始时间"].InnerText;
                                    task.SYEndTime = n["结束时间"].InnerText;
                                    //task.SYSysName = n["系统名称"].InnerText;
                                    //task.SYSysTask = n["系统任务"].InnerText;
                                    task.SYSysName = info.GetByAddrMark(sysnames[i]).ADDRName; //文件里存的是编码，发送时转成中文
                                    task.SYSysTask = systasks[i];
                                    obj.SYJH_SY_List.Add(task);
                                }
                            }
                            //obj.SYJH_SY_List.Add(task);
                        }
                    }
                    obj.SYCount = obj.SYJH_SY_List.Count.ToString("0000"); //重新计算新的试验数目
                    #endregion

                    //西安中心
                    if (desValue == PlanParameters.ReadParamValue("XSCCCode"))
                    {
                        #region 写入文件-文件类型一
                        filename = FileNameMaker.GenarateFileNameTypeOne(dataCode, "B", desValue, runningMode);
                        //RenamePlanFile(jh.FileIndex, SendingPath);  //移动并重命名文件到外发目录
                        //当返回值不为0，即创建文件成功时才保存文件路径
                        if (0 != CreateSYJHFile(obj, SendingPath))
                        {
                            SendFileNames = SendFileNames + SendingPath + ",";
                        }
                        #endregion

                    }
                    else
                    {
                        #region 写入文件-文件类型三
                        filename = FileNameMaker.GenarateFileNameTypeThree(dataCode, desValue);
                        SendFileNames = SendFileNames + SendingPath + ",";
                        itype = itype.GetByExMark(dataCode);

                        sw = new StreamWriter(SendingPath);
                        sw.WriteLine("<说明区>");
                        sw.WriteLine("[生成时间T]：" + obj.CTime.ToString("yyyy-MM-dd-HH:mm"));
                        sw.WriteLine("[信源S]：" + this.Source);
                        sw.WriteLine("[信宿D]：" + destinationName);
                        sw.WriteLine("[任务代码M]：" + GetSendingTaskName(obj.TaskID, obj.SatID));
                        sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
                        sw.WriteLine("[数据区行数L]：" + obj.SYJH_SY_List.Count.ToString("0000"));
                        sw.WriteLine("<符号区>");
                        sw.WriteLine("[格式标识1]：XXFL  JXH  SatName  Type  TestItem  StartTime  EndTime");
                        sw.WriteLine("[格式标识2]：SysName  Task");
                        sw.WriteLine("<数据区>");
                        foreach (SYJH_SY sy in obj.SYJH_SY_List)
                        {
                            sw.WriteLine("ZJ" + "  " + obj.JHID + "  " + sy.SYSatName + "  " + sy.SYType + "  " + sy.SYItem + "  " + sy.SYStartTime + "  " + sy.SYEndTime);
                            sw.WriteLine(sy.SYSysName + "  " + sy.SYSysTask);
                        }
                        sw.WriteLine("<辅助区>");
                        sw.WriteLine("[备注]：");
                        sw.WriteLine("[结束]：END");

                        sw.Close();
                        #endregion
                    }
                }
            }

            if (!string.IsNullOrEmpty(SendFileNames) && SendFileNames[SendFileNames.Length - 1] == ',')
            {
                SendFileNames = SendFileNames.Substring(0, SendFileNames.Length - 1);
            }
            return SendFileNames;
        }
        /// <summary>
        /// 创建外部应用计划文件
        /// </summary>
        /// <param name="ids">计划ID串</param>
        /// <param name="desValue">信宿地址值</param>
        /// <param name="destinationName">信宿地址名称</param>
        /// <returns>生成的外发文件完整路径名串</returns>
        public string CreateSendingYJJHFile(string ids, out string targets, bool noSend)
        {
            targets = string.Empty;
            XYXSInfo info = new XYXSInfo();
            List<PlanParameter> targetList = PlanParameters.ReadParameters("YJJHSendTargetMapping");
            string targetMark = string.Empty;
            string destinationName = string.Empty;
            OperatingManagement.DataAccessLayer.BusinessManage.InfoType itype = new OperatingManagement.DataAccessLayer.BusinessManage.InfoType();

            string SendFileNames = "";
            List<JH> listJH = (new JH()).SelectByIDS(ids);
            YJJH obj;
            string dataCode = PlanParameters.ReadParamValue("YJJHDataCode");
            Task oTask = new Task();
            foreach (JH jh in listJH)
            {
                #region 读取计划XML文件，变量赋值
                obj = new YJJH();
                obj.ReadXML(jh.FileIndex);
                obj.CTime = DateTime.Now;
                obj.TaskID = jh.TaskID;
                obj.SatID = jh.SatID;
                for (int i = 0; i < targetList.Count; i++)
                {
                    if (targetList[i].Text == jh.SatID.Substring(0, 3))
                    {
                        targetMark = targetList[i].Value;
                        break;
                    }
                }
                if (noSend)//中心内部生成的文件不发送，故文件发送方为中心
                    targetMark = ConfigurationManager.AppSettings["ZXBM"];
                if (!targetMark.Equals(string.Empty))
                {
                    info = info.GetByAddrMark(targetMark);
                    targets += targetMark + ",";
                    destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";
                }
                else
                    continue;
                #endregion
                
                if (noSend || obj.SysName == info.ADDRName)   //系统名称和发送目标一致时才发送
                {
                    #region 写入文件
                    filename = FileNameMaker.GenarateFileNameTypeThree(dataCode, targetMark);
                    SendFileNames = SendFileNames + SendingPath + ",";
                    if (obj.XXFL.ToUpper() == "ZJ")
                        itype = itype.GetByExMark(dataCode + "ZJ");
                    else
                        itype = itype.GetByExMark(dataCode);

                    sw = new StreamWriter(SendingPath);
                    sw.WriteLine("<说明区>");
                    sw.WriteLine("[生成时间T]：" + obj.CTime.ToString("yyyy-MM-dd-HH:mm"));
                    sw.WriteLine("[信源S]：" + this.Source);
                    sw.WriteLine("[信宿D]：" + destinationName);
                    sw.WriteLine("[任务代码M]：" + GetSendingTaskName(obj.TaskID, obj.SatID));
                    sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
                    sw.WriteLine("[数据区行数L]：" + obj.Tasks.Count.ToString("X4"));
                    sw.WriteLine("<符号区>");
                    sw.WriteLine("[格式标识1]：XXFL  JXH  SysName  StartTime  EndTime  Task");
                    sw.WriteLine("<数据区>");
                    foreach (YJJH_Task task  in obj.Tasks)
                    {
                        sw.WriteLine(obj.XXFL + "  " + obj.JXH + "  " + obj.SysName + "  " + task.StartTime + "  " + task.EndTime + "  " + task.Task);
                    }
                    sw.WriteLine("<辅助区>");
                    sw.WriteLine("[备注]：");
                    sw.WriteLine("[结束]：END");

                    sw.Close();
                    #endregion
                }

            }

            if (!string.IsNullOrEmpty(SendFileNames) && SendFileNames[SendFileNames.Length - 1] == ',')
            {
                SendFileNames = SendFileNames.Substring(0, SendFileNames.Length - 1);
            }

            targets = targets.TrimEnd(new char[] { ',' });
            return SendFileNames;
        }
        /// <summary>
        /// 生成外发信息需求文件
        /// </summary>
        /// <param name="ids">计划ID串</param>
        /// <param name="desValue">信宿地址值</param>
        /// <param name="destinationName">信宿地址名称</param>
        /// <returns>生成的外发文件完整路径名串</returns>
        public string CreateSendingXXXQFile(string ids, string desValue)
        {
            XYXSInfo info = new XYXSInfo();
            info = info.GetByAddrMark(desValue);
            string destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";

            OperatingManagement.DataAccessLayer.BusinessManage.InfoType itype = new OperatingManagement.DataAccessLayer.BusinessManage.InfoType();

            string SendFileNames = "";
            string filenameMBXQ = "";
            string filenameHJXQ = "";
            List<JH> listJH = (new JH()).SelectByIDS(ids);
            XXXQ obj;
            string strMBXQDataCode = PlanParameters.ReadParamValue("MBXQDataCode");
            string strHJXQDataCode = PlanParameters.ReadParamValue("HJXQDataCode");
            string strXXXQDataCode = PlanParameters.ReadParamValue("XXXQDataCode");
            Task oTask = new Task();
            foreach (JH jh in listJH)
            {
                #region 读取计划XML文件，变量赋值
                obj = new XXXQ();
                obj.TaskID = jh.TaskID;
                obj.SatID = jh.SatID;
                obj.objMBXQ = new MBXQ();
                obj.objHJXQ = new HJXQ();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(jh.FileIndex);

                obj.CTime = DateTime.Now; //jh.CTime;
                #region 空间目标信息需求
                XmlNode root = xmlDoc.SelectSingleNode("空间信息需求/空间目标信息需求/User");
                obj.objMBXQ.User = root.InnerText;
                root = xmlDoc.SelectSingleNode("空间信息需求/空间目标信息需求/Time");
                obj.objMBXQ.Time = root.InnerText;
                root = xmlDoc.SelectSingleNode("空间信息需求/空间目标信息需求/TargetInfo");
                obj.objMBXQ.TargetInfo = root.InnerText;
                root = xmlDoc.SelectSingleNode("空间信息需求/空间目标信息需求/TimeSection1");
                obj.objMBXQ.TimeSection1 = root.InnerText;
                root = xmlDoc.SelectSingleNode("空间信息需求/空间目标信息需求/TimeSection2");
                obj.objMBXQ.TimeSection2 = root.InnerText;
                root = xmlDoc.SelectSingleNode("空间信息需求/空间目标信息需求/Sum");
                obj.objMBXQ.Sum = root.InnerText;

                root = xmlDoc.SelectSingleNode("空间信息需求/空间目标信息需求");
                List<MBXQSatInfo> list = new List<MBXQSatInfo>();
                MBXQSatInfo sat;
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "卫星")
                    {
                        sat = new MBXQSatInfo();
                        sat.SatName = n["SatName"].InnerText;
                        sat.InfoName = n["InfoName"].InnerText;
                        sat.InfoTime = n["InfoTime"].InnerText;
                        list.Add(sat);
                    }
                }
                obj.objMBXQ.SatInfos = list;

                #endregion
                #region 空间环境信息需求
                root = xmlDoc.SelectSingleNode("空间信息需求/空间环境信息需求/User");
                obj.objHJXQ.User = root.InnerText;
                root = xmlDoc.SelectSingleNode("空间信息需求/空间环境信息需求/Time");
                obj.objHJXQ.Time = root.InnerText;
                root = xmlDoc.SelectSingleNode("空间信息需求/空间环境信息需求/EnvironInfo");
                obj.objHJXQ.EnvironInfo = root.InnerText;
                root = xmlDoc.SelectSingleNode("空间信息需求/空间环境信息需求/TimeSection1");
                obj.objHJXQ.TimeSection1 = root.InnerText;
                root = xmlDoc.SelectSingleNode("空间信息需求/空间环境信息需求/TimeSection2");
                obj.objHJXQ.TimeSection2 = root.InnerText;
                root = xmlDoc.SelectSingleNode("空间信息需求/空间环境信息需求/Sum");
                obj.objHJXQ.Sum = root.InnerText;

                root = xmlDoc.SelectSingleNode("空间信息需求/空间环境信息需求");
                List<HJXQSatInfo> listhj = new List<HJXQSatInfo>();
                HJXQSatInfo sathj;
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "卫星")
                    {
                        sathj = new HJXQSatInfo();
                        sathj.SatName = n["SatName"].InnerText;
                        sathj.InfoName = n["InfoName"].InnerText;
                        sathj.InfoArea = n["InfoArea"].InnerText;
                        sathj.InfoTime = n["InfoTime"].InnerText;
                        listhj.Add(sathj);
                    }
                }
                obj.objHJXQ.SatInfos = listhj;
                #endregion
                #endregion

                #region 写入文件

                #region MBXQ
                filename = FileNameMaker.GenarateFileNameTypeThree(strMBXQDataCode, desValue);
                SendFileNames = SendFileNames + SendingPath + ",";
                filenameMBXQ = filename;    //用于在XXXQ文件里的数据行记录
                itype = itype.GetByExMark(strMBXQDataCode);

                sw = new StreamWriter(SendingPath);
                sw.WriteLine("<说明区>");
                sw.WriteLine("[生成时间T]：" + obj.CTime.ToString("yyyy-MM-dd-HH:mm"));
                sw.WriteLine("[信源S]：" + this.Source);
                sw.WriteLine("[信宿D]：" + destinationName);
                sw.WriteLine("[任务代码M]：" + GetSendingTaskName(obj.TaskID, obj.SatID));
                sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
                sw.WriteLine("[数据区行数L]：" + (obj.objMBXQ.SatInfos.Count + 1).ToString("0000"));
                sw.WriteLine("<符号区>");
                sw.WriteLine("[格式标识1]：User  Time  TargetInfo  TimeSection1  TimeSection2  Sum");
                sw.WriteLine("[格式标识2]：SatName  InfoName  InfoTime");
                sw.WriteLine("<数据区>");
                sw.WriteLine(obj.objMBXQ.User + "  " + obj.objMBXQ.Time + "  " + obj.objMBXQ.TargetInfo + "  " + obj.objMBXQ.TimeSection1 + "  " + obj.objMBXQ.TimeSection2 + "  " + obj.objMBXQ.Sum);
                for (int i = 0; i < obj.objMBXQ.SatInfos.Count; i++)
                {
                    sw.WriteLine(obj.objMBXQ.SatInfos[i].SatName + "  " + obj.objMBXQ.SatInfos[i].InfoName + "  " + obj.objMBXQ.SatInfos[i].InfoTime);
                }
                sw.WriteLine("<辅助区>");
                sw.WriteLine("[备注]：");
                sw.WriteLine("[结束]：END");

                sw.Close();
                #endregion
                #region HJXQ
                filename = FileNameMaker.GenarateFileNameTypeThree(strHJXQDataCode, desValue);
                SendFileNames = SendFileNames + SendingPath + ",";
                filenameHJXQ = filename;    //用于在XXXQ文件里的数据行记录
                itype = itype.GetByExMark(strHJXQDataCode);

                sw = new StreamWriter(SendingPath);
                sw.WriteLine("<说明区>");
                sw.WriteLine("[生成时间T]：" + obj.CTime.ToString("yyyy-MM-dd-HH:mm"));
                sw.WriteLine("[信源S]：" + this.Source);
                sw.WriteLine("[信宿D]：" + destinationName);
                sw.WriteLine("[任务代码M]：" + GetSendingTaskName(obj.TaskID, obj.SatID));
                sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
                sw.WriteLine("[数据区行数L]：" + (obj.objHJXQ.SatInfos.Count + 1).ToString("0000"));
                sw.WriteLine("<符号区>");
                sw.WriteLine("[格式标识1]：User  Time  EnvironInfo  TimeSection1  TimeSection2  Sum");
                sw.WriteLine("[格式标识2]：SatName  InfoName  InfoArea  InfoTime");
                sw.WriteLine("<数据区>");
                sw.WriteLine(obj.objHJXQ.User + "  " + obj.objHJXQ.Time + "  " + obj.objHJXQ.EnvironInfo + "  " + obj.objHJXQ.TimeSection1 + "  " + obj.objHJXQ.TimeSection2 + "  " + obj.objHJXQ.Sum);
                for (int i = 0; i < obj.objHJXQ.SatInfos.Count; i++)
                {
                    sw.WriteLine(obj.objHJXQ.SatInfos[i].SatName + "  " + obj.objHJXQ.SatInfos[i].InfoName + "  " + obj.objHJXQ.SatInfos[i].InfoArea + "  " + obj.objHJXQ.SatInfos[i].InfoTime);
                }
                sw.WriteLine("<辅助区>");
                sw.WriteLine("[备注]：");
                sw.WriteLine("[结束]：END");

                sw.Close();
                #endregion
                #region XXXQ
                filename = FileNameMaker.GenarateFileNameTypeThree(strXXXQDataCode, desValue);
                SendFileNames = SendFileNames + SendingPath + ",";
                itype = itype.GetByExMark(strXXXQDataCode);

                sw = new StreamWriter(SendingPath);
                sw.WriteLine("<说明区>");
                sw.WriteLine("[生成时间T]：" + obj.CTime.ToString("yyyy-MM-dd-HH:mm"));
                sw.WriteLine("[信源S]：" + this.Source);
                sw.WriteLine("[信宿D]：" + destinationName);
                sw.WriteLine("[任务代码M]：" + GetSendingTaskName(obj.TaskID, obj.SatID));
                sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
                sw.WriteLine("[数据区行数L]：0002");
                sw.WriteLine("<符号区>");
                sw.WriteLine("<数据区>");
                sw.WriteLine(filenameMBXQ);
                sw.WriteLine(filenameHJXQ);
                sw.WriteLine("<辅助区>");
                sw.WriteLine("[备注]：");
                sw.WriteLine("[结束]：END");

                sw.Close();
                #endregion
                #endregion
            }

            if (!string.IsNullOrEmpty(SendFileNames) && SendFileNames[SendFileNames.Length - 1] == ',')
            {
                SendFileNames = SendFileNames.Substring(0, SendFileNames.Length - 1);
            }
            return SendFileNames;
        }
        /// <summary>
        /// 生成外发地面工作计划文件
        /// </summary>
        /// <param name="ids">计划ID串</param>
        /// <param name="desValue">信宿地址值</param>
        /// <param name="destinationName">信宿地址名称</param>
        /// <returns>生成的外发文件完整路径名串</returns>
        public string CreateSendingGZJHFile(string ids, out string targets, bool noSend)
        {
            targets = string.Empty;
            XYXSInfo info = new XYXSInfo();
            GroundResource oGR = new GroundResource();
            string destinationName = string.Empty;
            OperatingManagement.DataAccessLayer.BusinessManage.InfoType itype = new OperatingManagement.DataAccessLayer.BusinessManage.InfoType();

            string SendFileNames = "";
            List<JH> listJH = (new JH()).SelectByIDS(ids);
            List<GZJH_Content> lstContents;
            Dictionary<string, List<string>> dicDWSB;
            Dictionary<string, List<string>> dicSBXSMapping;
            string strDw = string.Empty;
            string strSB = string.Empty;
            GZJH obj;
            string dataCode = PlanParameters.ReadParamValue("GZJHDataCode");
            Task oTask = new Task();
            foreach (JH jh in listJH)
            {
                #region 读取计划XML文件，变量赋值
                dicDWSB = new Dictionary<string, List<string>>();
                obj = new GZJH();
                obj.TaskID = jh.TaskID;
                obj.SatID = jh.SatID;
                obj.CTime = DateTime.Now;
                obj.GZJHContents = new List<GZJH_Content>();
                GZJH_Content c;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(jh.FileIndex);

                XmlNode root = xmlDoc.SelectSingleNode("地面站工作计划/JXH");
                obj.JXH = root.InnerText;
                root = xmlDoc.SelectSingleNode("地面站工作计划/XXFL");
                obj.XXFL = root.InnerText;

                #region Content
                root = xmlDoc.SelectSingleNode("地面站工作计划");

                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "Content")
                    {
                        c = new GZJH_Content();
                        c.DW = n["DW"].InnerText;
                        //c.DW = info.GetByAddrMark(n["DW"].InnerText).ADDRName;
                        c.SB = n["SB"].InnerText;
                        c.QS = n["QS"].InnerText;
                        c.QH = n["QH"].InnerText;
                        c.DH = n["DH"].InnerText;
                        c.FS = n["FS"].InnerText;
                        c.JXZ = n["JXZ"].InnerText;
                        c.MS = n["MS"].InnerText;
                        c.QB = n["QB"].InnerText;
                        c.GXZ = n["GXZ"].InnerText;
                        c.ZHB = n["ZHB"].InnerText;
                        c.GZK = n["GZK"].InnerText;
                        c.GZJ = n["GZJ"].InnerText;
                        c.KSHX = n["KSHX"].InnerText;
                        c.GSHX = n["GSHX"].InnerText;
                        c.RK = n["RK"].InnerText;
                        c.JS = n["JS"].InnerText;
                        c.BID = n["BID"].InnerText;
                        c.SBZ = n["SBZ"].InnerText;
                        c.RTs = n["RTs"].InnerText;
                        c.RTe = n["RTe"].InnerText;
                        c.SL = n["SL"].InnerText;
                        c.HBID = n["HBID"].InnerText;
                        c.HBZ = n["HBZ"].InnerText;
                        c.Ts = n["Ts"].InnerText;
                        c.Te = n["Te"].InnerText;
                        c.HRTs = n["HRTs"].InnerText;
                        c.HSL = n["HSL"].InnerText;
                        obj.GZJHContents.Add(c);
                        if (dicDWSB.ContainsKey(c.DW))
                        {
                            if (!dicDWSB[c.DW].Contains(c.SB))
                                dicDWSB[c.DW].Add(c.SB);
                        }
                        else
                        {
                            dicDWSB.Add(c.DW, new List<string>());
                            dicDWSB[c.DW].Add(c.SB);
                        }
                    }
                }
                #endregion

                #endregion

                if (obj.GZJHContents.Count > 0)
                {
                    if (obj.XXFL.ToUpper() == "ZJ")
                        itype = itype.GetByExMark(dataCode + "ZJ");
                    else
                        itype = itype.GetByExMark(dataCode);
                    if (noSend)
                    {
                        #region 仅生成文件，不外发，写入文件
                        info = new XYXSInfo().GetByAddrMark(ConfigurationManager.AppSettings["ZXBM"]);
                        if (info == null)
                            continue;
                        if (!FileExchangeConfig.GetTgtListForSending(dataCode).Contains(info.ADDRMARK))
                            continue;
                        targets += info.ADDRMARK + ",";
                        destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";
                        filename = FileNameMaker.GenarateFileNameTypeThree(dataCode, info.ADDRMARK);
                        SendFileNames = SendFileNames + SendingPath + ",";

                        sw = new StreamWriter(SendingPath);
                        sw.WriteLine("<说明区>");
                        sw.WriteLine("[生成时间T]：" + obj.CTime.ToString("yyyy-MM-dd-HH:mm"));
                        sw.WriteLine("[信源S]：" + this.Source);
                        sw.WriteLine("[信宿D]：" + destinationName);
                        sw.WriteLine("[任务代码M]：" + GetSendingTaskName(obj.TaskID, obj.SatID));
                        sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
                        sw.WriteLine("[数据区行数L]：" + Convert.ToInt32(obj.GZJHContents.Count).ToString("0000"));
                        sw.WriteLine("<符号区>");
                        sw.WriteLine("[格式标识1]：JXH  XXFL  DW  SB  QS");
                        sw.WriteLine("[格式标识2]：QH  DH  FS  JXZ  MS  QB  GXZ  ZHB  RK  GZK  KSHX  GSHX  GZJ  JS  BID  SBZ  RTs  RTe  SL  BID  HBZ  Ts  Te  RTs  SL");
                        sw.WriteLine("<数据区>");
                        foreach (KeyValuePair<string, List<string>> kValue in dicDWSB)
                        {
                            strDw = kValue.Key;
                            dicSBXSMapping = oGR.SelectXyxsIDsInCodes(StrList2String(kValue.Value));
                            foreach (string val in dicSBXSMapping.Keys.ToList())
                            {
                                foreach (string sb in dicSBXSMapping[val])
                                {
                                    lstContents = obj.GZJHContents.Where(t => t.DW == strDw && t.SB == sb).ToList();
                                    sw.WriteLine(obj.JXH + "  " + obj.XXFL + "  " + strDw + "  " + sb + "  " + lstContents[0].QS);
                                    foreach (GZJH_Content t in lstContents)
                                    {
                                        sw.WriteLine(t.QH + "  " + t.DH + "  " + t.FS + "  " + t.JXZ + "  "
                                            + t.MS + "  " + t.QB + "  " + t.GXZ + "  " + t.ZHB + "  " + t.RK + "  " + t.GZK + "  " + t.GZJ
                                             + "  " + t.KSHX + "  " + t.GSHX + "  " + t.GZJ + "  " + t.JS + "  " + t.BID + "  " + t.SBZ
                                             + "  " + t.RTs + "  " + t.RTe + "  " + t.SL + "  " + t.HBID + "  " + t.HBZ + "  " + t.Ts
                                             + "  " + t.Te + "  " + t.HRTs + "  " + t.HSL);
                                    }
                                }
                            }
                        }
                        sw.WriteLine("<辅助区>");
                        sw.WriteLine("[备注]：");
                        sw.WriteLine("[结束]：END");
                        sw.Close();
                        #endregion
                    }
                    else
                    {
                        //自动匹配计划用到站，给站发送文件
                        #region 外发文件，写入文件
                        foreach (KeyValuePair<string, List<string>> kValue in dicDWSB)
                        {
                            strDw = kValue.Key;
                            dicSBXSMapping = oGR.SelectXyxsIDsInCodes(StrList2String(kValue.Value));
                            foreach (string val in dicSBXSMapping.Keys.ToList())
                            {
                                info = new XYXSInfo().GetByID(Convert.ToInt32(val));
                                if (info == null)
                                    continue;
                                if (!FileExchangeConfig.GetTgtListForSending(dataCode).Contains(info.ADDRMARK))
                                    continue;
                                targets += info.ADDRMARK + ",";
                                destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";
                                filename = FileNameMaker.GenarateFileNameTypeThree(dataCode, info.ADDRMARK);
                                SendFileNames = SendFileNames + SendingPath + ",";

                                sw = new StreamWriter(SendingPath);
                                sw.WriteLine("<说明区>");
                                sw.WriteLine("[生成时间T]：" + obj.CTime.ToString("yyyy-MM-dd-HH:mm"));
                                sw.WriteLine("[信源S]：" + this.Source);
                                sw.WriteLine("[信宿D]：" + destinationName);
                                sw.WriteLine("[任务代码M]：" + GetSendingTaskName(obj.TaskID, obj.SatID));
                                sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
                                sw.WriteLine("[数据区行数L]：" + Convert.ToInt32(obj.GZJHContents.Count).ToString("0000"));
                                sw.WriteLine("<符号区>");
                                sw.WriteLine("[格式标识1]：JXH  XXFL  DW  SB  QS");
                                sw.WriteLine("[格式标识2]：QH  DH  FS  JXZ  MS  QB  GXZ  ZHB  RK  GZK  KSHX  GSHX  GZJ  JS  BID  SBZ  RTs  RTe  SL  BID  HBZ  Ts  Te  RTs  SL");
                                sw.WriteLine("<数据区>");
                                foreach (string sb in dicSBXSMapping[val])
                                {
                                    lstContents = obj.GZJHContents.Where(t => t.DW == strDw && t.SB == sb).ToList();
                                    sw.WriteLine(obj.JXH + "  " + obj.XXFL + "  " + strDw + "  " + sb + "  " + lstContents[0].QS);
                                    foreach (GZJH_Content t in lstContents)
                                    {
                                        sw.WriteLine(t.QH + "  " + t.DH + "  " + t.FS + "  " + t.JXZ + "  "
                                            + t.MS + "  " + t.QB + "  " + t.GXZ + "  " + t.ZHB + "  " + t.RK + "  " + t.GZK + "  " + t.GZJ
                                             + "  " + t.KSHX + "  " + t.GSHX + "  " + t.GZJ + "  " + t.JS + "  " + t.BID + "  " + t.SBZ
                                             + "  " + t.RTs + "  " + t.RTe + "  " + t.SL + "  " + t.HBID + "  " + t.HBZ + "  " + t.Ts
                                             + "  " + t.Te + "  " + t.HRTs + "  " + t.HSL);
                                    }
                                }
                                sw.WriteLine("<辅助区>");
                                sw.WriteLine("[备注]：");
                                sw.WriteLine("[结束]：END");
                                sw.Close();
                            }
                        }
                        #endregion
                    }
                }
            }

            if (!string.IsNullOrEmpty(SendFileNames) && SendFileNames[SendFileNames.Length - 1] == ',')
            {
                SendFileNames = SendFileNames.Substring(0, SendFileNames.Length - 1);
            }
            targets = targets.TrimEnd(new char[] { ',' });
            return SendFileNames;
        }

        /// <summary>
        /// 生成测控资源使用申请外发文件
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="desValue"></param>
        /// <returns></returns>
        public string CreateSendingDJZYSQFile(string ids, string desValue, string runningMode)
        {
            XYXSInfo info = new XYXSInfo();
            info = info.GetByAddrMark(desValue);
            string destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";
            OperatingManagement.DataAccessLayer.BusinessManage.InfoType itype = new OperatingManagement.DataAccessLayer.BusinessManage.InfoType();

            string SendFileNames = "";
            List<JH> listJH = (new JH()).SelectByIDS(ids);
            string dataCode = PlanParameters.ReadParamValue("DJZYSQDataCode");
            foreach (JH jh in listJH)
            {
                this.filename = FileNameMaker.GenarateFileNameTypeOne(dataCode, "B", desValue, runningMode);
                CopyFile(jh.FileIndex, this.SendingPath);
                SendFileNames = SendFileNames + this.SendingPath + ",";
            }

            if (!string.IsNullOrEmpty(SendFileNames) && SendFileNames[SendFileNames.Length - 1] == ',')
            {
                SendFileNames = SendFileNames.Substring(0, SendFileNames.Length - 1);
            }
            return SendFileNames;
        }

        /// <summary>
        /// 中心计划不外发
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="desValue"></param>
        /// <returns></returns>
        public string CreateSendingZXJHFile(string ids, string desValue)
        {
            XYXSInfo info = new XYXSInfo();
            info = info.GetByAddrMark(desValue);
            string destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";

            //string SendFileNames = "";
            //List<JH> listJH = (new JH()).SelectByIDS(ids);
            //ZXJH obj;
            //foreach (JH jh in listJH)
            //{
            //    #region 读取计划XML文件，变量赋值
            //    #endregion

            //    #region 写入文件
            //    filename = FileNameMaker.GenarateFileNameTypeThree("ZXJH", desValue);
            //    SendFileNames += SendingPath;

            //    #endregion
            //}

            //return SendFileNames;
            return "";
        }
        /// <summary>
        /// 生成外发推演数据文件
        /// </summary>
        /// <param name="ids">计划ID串</param>
        /// <param name="desValue">信宿地址值</param>
        /// <param name="destinationName">信宿地址名称</param>
        /// <returns>生成的外发文件完整路径名串</returns>
        public string CreateSendingTYSJFile(string ids, string desValue)
        {
            XYXSInfo info = new XYXSInfo();
            info = info.GetByAddrMark(desValue);
            string destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";
            OperatingManagement.DataAccessLayer.BusinessManage.InfoType itype = new OperatingManagement.DataAccessLayer.BusinessManage.InfoType();

            string SendFileNames = "";
            List<JH> listJH = (new JH()).SelectByIDS(ids);
            TYSJ obj;
            string strYDSJDataCode = PlanParameters.ReadParamValue("TYSJDataCode");
            Task oTask = new Task();
            foreach (JH jh in listJH)
            {
                #region 读取计划XML文件，变量赋值
                obj = new TYSJ();
                obj.SYContents = new List<TYSJ_Content>();
                TYSJ_Content ct;
                obj.TaskID = jh.TaskID;
                obj.SatID = jh.SatID;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(jh.FileIndex);

                obj.CTime = DateTime.Now; //jh.CTime;
                XmlNode root = xmlDoc.SelectSingleNode("仿真推演试验数据");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "Content")
                    {
                        ct = new TYSJ_Content();
                        ct.SatName = n["SatName"].InnerText;
                        ct.Type = n["Type"].InnerText;
                        ct.TestItem = n["TestItem"].InnerText;
                        ct.StartTime = n["StartTime"].InnerText;
                        ct.EndTime = n["EndTime"].InnerText;
                        ct.Condition = n["Condition"].InnerText;
                        obj.SYContents.Add(ct);
                    }
                }

                #endregion

                #region 写入文件
                filename = FileNameMaker.GenarateFileNameTypeThree(strYDSJDataCode, desValue);
                SendFileNames = SendFileNames + SendingPath + ",";
                itype = itype.GetByExMark(strYDSJDataCode);

                sw = new StreamWriter(SendingPath);
                sw.WriteLine("<说明区>");
                sw.WriteLine("[生成时间T]：" + obj.CTime.ToString("yyyy-MM-dd-HH:mm"));
                sw.WriteLine("[信源S]：" + this.Source);
                sw.WriteLine("[信宿D]：" + destinationName);
                sw.WriteLine("[任务代码M]：" + GetSendingTaskName(obj.TaskID, obj.SatID));
                sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
                sw.WriteLine("[数据区行数L]："+obj.SYContents.Count.ToString("0000"));
                sw.WriteLine("<符号区>");
                sw.WriteLine("[格式标识1]：SatName  Type  TestItem  StartTime  EndTime  Condition");
                sw.WriteLine("<数据区>");
                foreach (TYSJ_Content t in obj.SYContents)
                {
                    sw.WriteLine(t.SatName + "  " + t.Type + "  " + t.TestItem + "  " + t.StartTime + "  " + t.EndTime + "  " + t.Condition);
                }
                sw.WriteLine("<辅助区>");
                sw.WriteLine("[备注]：");
                sw.WriteLine("[结束]：END");

                sw.Close();
                #endregion

            }

            if (SendFileNames[SendFileNames.Length - 1] == ',')
            {
                SendFileNames = SendFileNames.Substring(0, SendFileNames.Length - 1);
            }
            return SendFileNames;
        }
        /// <summary>
        /// 生成外发轨道根数文件
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="desValue"></param>
        /// <returns></returns>
        public string CreateSendingGDGSFile(string ids, string desValue)
        {
            XYXSInfo info = new XYXSInfo();
            info = info.GetByAddrMark(desValue);
            string destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";
            OperatingManagement.DataAccessLayer.BusinessManage.InfoType itype = new OperatingManagement.DataAccessLayer.BusinessManage.InfoType();

            string SendFileNames = "";
            List<GD> listJH = (new GD()).SelectByIDS(ids);
            //Dictionary<string, int> dicTaskID = new Dictionary<string, int>();
            //Task oTask = new Task();
            //foreach (GD obj in listJH)
            //{
            //    if (dicTaskID.ContainsKey(obj.TaskID))
            //    {
            //        dicTaskID[obj.TaskID] = dicTaskID[obj.TaskID] + 1;
            //    }
            //    else
            //    {
            //        dicTaskID.Add(obj.TaskID, 1);

            //    }
            //}
            //相同任务的轨道数据写进一个文件里
            string strGDGSDataCode = PlanParameters.ReadParamValue("GDGSDataCode");
            //foreach (string key in dicTaskID.Keys)
            //{
                #region 写入文件
                filename = FileNameMaker.GenarateFileNameTypeThree(strGDGSDataCode, desValue);
                SendFileNames = SendFileNames + SendingPath + ",";
                itype = itype.GetByExMark("GD");
                sw = new StreamWriter(SendingPath);

                sw.WriteLine("<说明区>");
                sw.WriteLine("[生成时间T]：" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm"));
                sw.WriteLine("[信源S]：" + this.Source);
                sw.WriteLine("[信宿D]：" + destinationName);
                sw.WriteLine("[任务代码M]：" + GetSendingTaskName(listJH[0].TaskID, listJH[0].SatID));
                sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
                sw.WriteLine("[数据区行数L]：" + listJH.Count().ToString("0000"));
                sw.WriteLine("<符号区>");
                sw.WriteLine("[格式标识1]：T01  T02  a  e  i  Ω  w  M");
                sw.WriteLine("<数据区>");
                foreach (GD obj in listJH)
                {
                    //if (obj.TaskID == key)
                    //{
                        sw.WriteLine(obj.Times.ToString("yyyyMMdd") + "  " + obj.Times.ToString("HHmmssffff") + "  "
                            + (obj.A * 0.001).ToString("f4") + "  " + obj.E.ToString("f6") + "  " + obj.I.ToString("f4") 
                            + "  " + obj.Q.ToString("f6") + "  " + obj.W.ToString("f6") + "  " + obj.M.ToString("f6"));
                    //}
                }
                sw.WriteLine("<辅助区>");
                sw.WriteLine("[备注]：");
                sw.WriteLine("[结束]：END");


                #endregion
            //}
            sw.Close();

            if (SendFileNames[SendFileNames.Length - 1] == ',')
            {
                SendFileNames = SendFileNames.Substring(0, SendFileNames.Length - 1);
            }
            return SendFileNames;
        }
        /// <summary>
        /// 生成外发轨道根数文件
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="desValue"></param>
        /// <returns></returns>
        public string CreateSendingGDGSFile(string ids, int desID)
        {
            XYXSInfo info = new XYXSInfo();
            info = info.GetByID(desID);
            string destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";
            OperatingManagement.DataAccessLayer.BusinessManage.InfoType itype = new OperatingManagement.DataAccessLayer.BusinessManage.InfoType();

            string SendFileNames = "";
            List<GD> listJH = (new GD()).SelectByIDS(ids);
            //Dictionary<string, int> dicTaskID = new Dictionary<string, int>();
            //Task oTask = new Task();
            //foreach (GD obj in listJH)
            //{
            //    if (dicTaskID.ContainsKey(obj.TaskID))
            //    {
            //        dicTaskID[obj.TaskID] = dicTaskID[obj.TaskID] + 1;
            //    }
            //    else
            //    {
            //        dicTaskID.Add(obj.TaskID, 1);

            //    }
            //}
            //相同任务的轨道数据写进一个文件里
            string strGDGSDataCode = PlanParameters.ReadParamValue("GDGSDataCode");
            //foreach (string key in dicTaskID.Keys)
            //{
            #region 写入文件
            filename = FileNameMaker.GenarateFileNameTypeThree(strGDGSDataCode, info.ADDRMARK);
            SendFileNames = SendFileNames + SendingPath + ",";
            itype = itype.GetByExMark("GD");
            sw = new StreamWriter(SendingPath);

            sw.WriteLine("<说明区>");
            sw.WriteLine("[生成时间T]：" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm"));
            sw.WriteLine("[信源S]：" + this.Source);
            sw.WriteLine("[信宿D]：" + destinationName);
            sw.WriteLine("[任务代码M]：" + GetSendingTaskName(listJH[0].TaskID, listJH[0].SatID));
            sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
            sw.WriteLine("[数据区行数L]：" + listJH.Count().ToString("0000"));
            sw.WriteLine("<符号区>");
            sw.WriteLine("[格式标识1]：T01  T02  a  e  i  Ω  w  M");
            sw.WriteLine("<数据区>");
            foreach (GD obj in listJH)
            {
                //if (obj.TaskID == key)
                //{
                sw.WriteLine(obj.Times.ToString("yyyyMMdd") + "  " + obj.Times.ToString("HHmmssffff") + "  "
                    + (obj.A * 0.001).ToString("f4") + "  " + obj.E.ToString("f6") + "  " + obj.I.ToString("f4")
                    + "  " + obj.Q.ToString("f6") + "  " + obj.W.ToString("f6") + "  " + obj.M.ToString("f6"));
                //}
            }
            sw.WriteLine("<辅助区>");
            sw.WriteLine("[备注]：");
            sw.WriteLine("[结束]：END");


            #endregion
            //}
            sw.Close();

            if (SendFileNames[SendFileNames.Length - 1] == ',')
            {
                SendFileNames = SendFileNames.Substring(0, SendFileNames.Length - 1);
            }
            return SendFileNames;
        }
        /// <summary>
        /// 从轨道数据生成外发引导数据文件
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="desValue"></param>
        /// <returns></returns>
        public string CreateSendingYDSJFileFromGD(string ids, string desValue)
        {
            XYXSInfo info = new XYXSInfo();
            info = info.GetByAddrMark(desValue);
            string destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";
            OperatingManagement.DataAccessLayer.BusinessManage.InfoType itype = new OperatingManagement.DataAccessLayer.BusinessManage.InfoType();

            string SendFileNames = "";
            List<GD> listJH = (new GD()).SelectByIDS(ids);
            //Dictionary<string, int> dicTaskID = new Dictionary<string, int>();
            //Task oTask = new Task();
            //foreach (GD obj in listJH)
            //{
            //    if (dicTaskID.ContainsKey(obj.TaskID))
            //    {
            //        dicTaskID[obj.TaskID] = dicTaskID[obj.TaskID] + 1;
            //    }
            //    else
            //    {
            //        dicTaskID.Add(obj.TaskID, 1);

            //    }
            //}
            //相同任务的轨道数据写进一个文件里
            string strYDSJDataCode = PlanParameters.ReadParamValue("YDSJDataCode");
            //foreach (string key in dicTaskID.Keys)
            //{
                #region 写入文件
                filename = FileNameMaker.GenarateFileNameTypeThree(strYDSJDataCode, desValue);
                SendFileNames = SendFileNames + SendingPath + ",";
                itype = itype.GetByExMark(strYDSJDataCode);
                sw = new StreamWriter(SendingPath);

                sw.WriteLine("<说明区>");
                sw.WriteLine("[生成时间T]：" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm"));
                sw.WriteLine("[信源S]：" + this.Source);
                sw.WriteLine("[信宿D]：" + destinationName);
                sw.WriteLine("[任务代码M]：" + GetSendingTaskName(listJH[0].TaskID, listJH[0].SatID));
                sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
                sw.WriteLine("[数据区行数L]：" + listJH.Count().ToString("0000"));
                sw.WriteLine("<符号区>");
                sw.WriteLine("[格式标识1]：T01  T02  a  e  i  Ω  w  M");
                sw.WriteLine("<数据区>");
                foreach (GD obj in listJH)
                {
                    //if (obj.TaskID == key)
                    {
                        sw.WriteLine(obj.Times.ToString("yyyyMMdd") + "  " + obj.Times.ToString("HHmmssffff") + "  "
                            + (obj.A * 0.001).ToString("f4") + "  " + obj.E.ToString("f6") + "  " + obj.I.ToString("f4")
                            + "  " + obj.Q.ToString("f6") + "  " + obj.W.ToString("f6") + "  " + obj.M.ToString("f6"));
                    }
                }
                sw.WriteLine("<辅助区>");
                sw.WriteLine("[备注]：");
                sw.WriteLine("[结束]：END");


                #endregion
            //}
            sw.Close();

            if (SendFileNames[SendFileNames.Length - 1] == ',')
            {
                SendFileNames = SendFileNames.Substring(0, SendFileNames.Length - 1);
            }
            return SendFileNames;
        }
        /// <summary>
        /// 生成引导数据外发文件（目前没用到）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="desValue"></param>
        /// <returns></returns>
        public string CreateSendingYDSJFile(string ids, string desValue)
        {
            XYXSInfo info = new XYXSInfo();
            info = info.GetByAddrMark(desValue);
            string destinationName = info.ADDRName + info.ADDRMARK + "(" + info.EXCODE + ")";
            OperatingManagement.DataAccessLayer.BusinessManage.InfoType itype = new OperatingManagement.DataAccessLayer.BusinessManage.InfoType();

            string SendFileNames = "";
            List<YDSJ> listJH = (new YDSJ()).SelectByIDS(ids);
            Dictionary<string, int> dicTaskID = new Dictionary<string, int>();
            Task oTask = new Task();
            foreach (YDSJ obj in listJH)
            {
                if (dicTaskID.ContainsKey(obj.TaskID))
                {
                    dicTaskID[obj.TaskID] = dicTaskID[obj.TaskID] + 1;
                }
                else
                {
                    dicTaskID.Add(obj.TaskID, 1);

                }
            }
            //相同任务的引导数据写进一个文件里
            string strDataCode = PlanParameters.ReadParamValue("YDSJDataCode");
            foreach (string key in dicTaskID.Keys)
            {
                #region 写入文件
                filename = FileNameMaker.GenarateFileNameTypeThree(strDataCode, desValue);
                SendFileNames = SendFileNames + SendingPath + ",";
                itype = itype.GetByExMark(strDataCode);
                sw = new StreamWriter(SendingPath);

                sw.WriteLine("<说明区>");
                sw.WriteLine("[生成时间T]：" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm"));
                sw.WriteLine("[信源S]：" + this.Source);
                sw.WriteLine("[信宿D]：" + destinationName);
                //sw.WriteLine("[任务代码M]：" + oTask.GetTaskName(key) + "(" + oTask.GetObjectFlagByTaskNo(key) + ")");
                sw.WriteLine("[信息类别B]：" + itype.DATANAME + "(" + itype.EXCODE + ")");
                sw.WriteLine("[数据区行数L]：" + dicTaskID[key].ToString("0000"));
                sw.WriteLine("<符号区>");
                sw.WriteLine("[格式标识1]：SatName  T01  T02  a  e  i  Ω  w  M");
                sw.WriteLine("<数据区>");
                //A数据库里存的是米，发出去时转为千米
                foreach (YDSJ obj in listJH)
                {
                    if (obj.TaskID == key)
                    {
                        //sw.WriteLine(obj.SatName + strSplitorTwoBlanks + obj.Times.ToString("yyyyMMdd") + strSplitorTwoBlanks + obj.Times.ToString("HHmmssffff") + strSplitorTwoBlanks
                        //    + (obj.A * 0.001).ToString("f4") + strSplitorTwoBlanks + obj.E.ToString("f6") + strSplitorTwoBlanks + obj.I.ToString("f4") + "  "
                        //    + obj.O.ToString("f6") + strSplitorTwoBlanks + obj.W.ToString("f6") + strSplitorTwoBlanks + obj.M.ToString("f6"));
                    }
                }
                sw.WriteLine("<辅助区>");
                sw.WriteLine("[备注]：");
                sw.WriteLine("[结束]：END");
                #endregion
            }
            sw.Close();

            if (SendFileNames[SendFileNames.Length - 1] == ',')
            {
                SendFileNames = SendFileNames.Substring(0, SendFileNames.Length - 1);
            }
            return SendFileNames;
        }

        /// <summary>
        /// 生成引导数据外发文件（向4701/4702发送）
        /// </summary>
        /// <param name="desValue"></param>
        /// <returns></returns>
        public string CreateSendingYDSJFile(string taskNo, string satID, string desValue, string[] datas)
        {
            FileBaseInfo oFBInfo = new FileBaseInfo();
            XYXSInfo info = new XYXSInfo();
            InfoType itype = new InfoType();
            string strDataCode = PlanParameters.ReadParamValue("YDSJDataCode");
            itype = itype.GetByExMark(strDataCode);
            info = info.GetByAddrMark(desValue);
            filename = FileNameMaker.GenarateFileNameTypeThree(strDataCode, desValue);
            oFBInfo.FullName = this.SendingPath;
            oFBInfo.From = this.Source;
            oFBInfo.To = info.ADDRName +info.ADDRMARK + "(" + info.EXCODE + ")";
            oFBInfo.InfoTypeName = itype.DATANAME + "(" + itype.EXCODE + ")";
            oFBInfo.TaskID = GetSendingTaskName(taskNo, satID);
            oFBInfo.LineCount = datas.Length;

            DataFileHandle oDFHandle = new DataFileHandle(oFBInfo.FullName);
            string[] fields = new string[]{"SatName  T01  T02  X  Y  Z  VX  VY  VZ"};
            string satName = new Satellite().GetName(satID);
            FileCreateResult oResult = oDFHandle.CreateFormat3FileForYDSJ(oFBInfo, fields, datas, satName);
            if (oResult == FileCreateResult.CreateSuccess)
                return oFBInfo.FullName;
            else
                return null;
        }
        #endregion

        /// <summary>
        /// 重命名计划文件
        /// </summary>
        /// <param name="oldfile"></param>
        /// <param name="newfile"></param>
        public void RenamePlanFile(string oldfile, string newfile)
        {
            FileInfo fi = new FileInfo(oldfile);
            fi.MoveTo(newfile);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="tgtFile"></param>
        private void CopyFile(string srcFile, string tgtFile)
        {
            System.IO.File.Copy(srcFile, tgtFile, true);
        }

        /// <summary>
        /// 判断要新生成要的文件，文件名是否已经存在
        /// </summary>
        /// <returns></returns>
        private bool TestFileName()
        {
            return System.IO.File.Exists(FilePath);
        }

        /// <summary>
        /// 通过内部任务号和卫星号获取外部任务代号
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="satID"></param>
        /// <returns></returns>
        private string GetSendingTaskName(string taskID, string satID)
        {
            return new Task().GetTaskName(taskID, satID) + "(" + new Task().GetOutTaskNo(taskID, satID) + ")";
        }

        private string StrList2String(List<string> list)
        {
            StringBuilder oSB = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                oSB.Append("'" + list[i] + "',");
            }
            return oSB.ToString().TrimEnd(new char[] { ',' });
        }

    }
}
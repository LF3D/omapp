using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using OperatingManagement.DataAccessLayer.PlanManage;

namespace ServicesKernel.File
{
    public class CreatePlanFile
    {
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

        private string _filepath=null;
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
        public string SavePath 
        {
            get
            {
                string savepath =System.Configuration.ConfigurationManager.AppSettings["savepath"];
                if (savepath[savepath.Length-1] !='\\')
                {
                    savepath += "\\";
                }
                return savepath;
            }
        }

        #endregion

        public void NewFile()
        {
            sw= new StreamWriter(FilePath);
            sw.WriteLine("<说明区>");
            sw.WriteLine("[生成时间]：" + this.CTime.ToString("yyyy-MM-dd-HH-mm"));
                sw.WriteLine("[信源S]："+this.Source);
                sw.WriteLine("[信宿D]："+this.Destination);
                sw.WriteLine("[任务代码M]："+this.TaskID);
                sw.WriteLine("[信息类别B]："+this.InfoType);
                sw.WriteLine("[数据区行数L]："+this.LineCount.ToString("0000"));
            sw.WriteLine("<符号区>");
                sw.WriteLine("[格式标识1]："+this.Format1);
                if (!string.IsNullOrEmpty(this.Format2))
                {
                    sw.WriteLine("[格式标识2]：" + this.Format2);    //没有这个字段时不输出
                }
            sw.WriteLine("[数据区]：");
                sw.WriteLine(this.DataSection);
            sw.WriteLine("<辅助区>");
                sw.WriteLine("[备注]：");
                sw.WriteLine("[结束]：END");

                sw.Close();
        }

        /// <summary>
        /// 应用研究
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type">0:Add;1:Edit</param>
        /// <returns></returns>
        public string CreateYJJHFile(YJJH obj,int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("YJJH", obj.TaskID, obj.SatID);
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

            xmlWriter.WriteStartElement("StartTime");
            xmlWriter.WriteString(obj.StartTime);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("EndTime");
            xmlWriter.WriteString(obj.EndTime);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Task");
            xmlWriter.WriteString(obj.Task);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.Close();

            return FilePath;
        }
        /// <summary>
        /// 信息空间
        /// </summary>
        public string CreateXXXQFile(XXXQ obj)
        {
            filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("XXXQ", obj.TaskID, obj.SatID);
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

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

            xmlWriter.WriteStartElement("卫星");
            for (int i = 1; i <= obj.objMBXQ.SatInfos.Count; i++)
            {
                xmlWriter.WriteStartElement("SatName");
                xmlWriter.WriteString(obj.objMBXQ.SatInfos[i-1].SatName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoName");
                xmlWriter.WriteString(obj.objMBXQ.SatInfos[i-1].InfoName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoTime");
                xmlWriter.WriteString(obj.objMBXQ.SatInfos[i-1].InfoTime);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();


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

            xmlWriter.WriteStartElement("卫星");
            for (int i = 1; i <= obj.objHJXQ.SatInfos.Count; i++)
            {
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
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            #endregion

            xmlWriter.Close();
            return FilePath;
        }

        public string CreateMBXQFile(MBXQ obj,int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("MBXQ", obj.TaskID, obj.SatID);
            }
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            #region MBXQ
            xmlWriter.WriteStartElement("空间目标信息需求");

            xmlWriter.WriteStartElement("User");
            xmlWriter.WriteString(obj.User);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Time");
            xmlWriter.WriteString(obj.Time);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TargetInfo");
            xmlWriter.WriteString(obj.TargetInfo);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TimeSection1");
            xmlWriter.WriteString(obj.TimeSection1);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TimeSection2");
            xmlWriter.WriteString(obj.TimeSection2);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Sum");
            xmlWriter.WriteString(obj.Sum);
            xmlWriter.WriteEndElement();

           // xmlWriter.WriteStartElement("卫星");
            for (int i = 1; i <= obj.SatInfos.Count; i++)
            {
                xmlWriter.WriteStartElement("卫星");

                xmlWriter.WriteStartElement("SatName");
                xmlWriter.WriteString(obj.SatInfos[i - 1].SatName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoName");
                xmlWriter.WriteString(obj.SatInfos[i - 1].InfoName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoTime");
                xmlWriter.WriteString(obj.SatInfos[i - 1].InfoTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }
           // xmlWriter.WriteEndElement();


            xmlWriter.WriteEndElement();
            #endregion

            xmlWriter.Close();
            return FilePath;
        }

        public string CreateHJXQFile(HJXQ obj,int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("HJXQ", obj.TaskID, obj.SatID);
            }
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            #region HJXQ
            xmlWriter.WriteStartElement("空间环境信息需求");

            xmlWriter.WriteStartElement("User");
            xmlWriter.WriteString(obj.User);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Time");
            xmlWriter.WriteString(obj.Time);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("EnvironInfo");
            xmlWriter.WriteString(obj.EnvironInfo);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TimeSection1");
            xmlWriter.WriteString(obj.TimeSection1);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TimeSection2");
            xmlWriter.WriteString(obj.TimeSection2);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Sum");
            xmlWriter.WriteString(obj.Sum);
            xmlWriter.WriteEndElement();

            //xmlWriter.WriteStartElement("卫星");
            for (int i = 1; i <= obj.SatInfos.Count; i++)
            {
                xmlWriter.WriteStartElement("卫星");

                xmlWriter.WriteStartElement("SatName");
                xmlWriter.WriteString(obj.SatInfos[i - 1].SatName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoName");
                xmlWriter.WriteString(obj.SatInfos[i - 1].InfoName);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoArea");
                xmlWriter.WriteString(obj.SatInfos[i - 1].InfoArea);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("InfoTime");
                xmlWriter.WriteString(obj.SatInfos[i - 1].InfoTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }
            //xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            #endregion

            xmlWriter.Close();
            return FilePath;
        }
        /// <summary>
        /// 地面站工作计划
        /// </summary>
        public string CreateDMJHFile(DMJH obj,int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("DMJH", obj.TaskID, obj.SatID);
            }
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("地面站工作计划");
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
        /// 中心计划
        /// </summary>
        public string CreateZXJHFile(ZXJH obj,int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("ZXJH", obj.TaskID, obj.SatID);
            }
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("中心运行计划");
            xmlWriter.WriteStartElement("日期");
            xmlWriter.WriteString(obj.Date);
            xmlWriter.WriteEndElement();

            #region 试验内容
            xmlWriter.WriteStartElement("试验内容");

            xmlWriter.WriteStartElement("对应日期的试验个数");
            xmlWriter.WriteString(obj.SYCount);
            xmlWriter.WriteEndElement();
            #region 试验项
            xmlWriter.WriteStartElement("试验项");
            xmlWriter.WriteStartElement("在当日计划中的ID");
            xmlWriter.WriteString(obj.SYID);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("试验项目名称");
            xmlWriter.WriteString(obj.SYName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("试验开始日期及时间");
            xmlWriter.WriteString(obj.SYDateTime);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("试验运行的天数");
            xmlWriter.WriteString(obj.SYDays);
            xmlWriter.WriteEndElement();
            #region 载荷
            xmlWriter.WriteStartElement("载荷");
                xmlWriter.WriteStartElement("开始时间");
                xmlWriter.WriteString(obj.SYLoadStartTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("结束时间");
                xmlWriter.WriteString(obj.SYLoadEndTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("动作内容");
                xmlWriter.WriteString(obj.SYLoadContent);
                xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            #endregion
            #region 数传
            xmlWriter.WriteStartElement("数传");
            xmlWriter.WriteStartElement("圈次");
            xmlWriter.WriteString(obj.SY_SCLaps);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("开始时间");
            xmlWriter.WriteString(obj.SY_SCStartTime);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("结束时间");
            xmlWriter.WriteString(obj.SY_SCEndTime);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            #endregion
            #region 测控
            xmlWriter.WriteStartElement("测控");
            xmlWriter.WriteStartElement("圈次");
            xmlWriter.WriteString(obj.SY_CKLaps);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("开始时间");
            xmlWriter.WriteString(obj.SY_CKStartTime);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("结束时间");
            xmlWriter.WriteString(obj.SY_CKEndTime);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            #endregion
            #region 注数
            xmlWriter.WriteStartElement("注数");
            xmlWriter.WriteStartElement("最早时间要求");
            xmlWriter.WriteString(obj.SY_ZSFirst);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("最晚时间要求");
            xmlWriter.WriteString(obj.SY_ZSLast);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("主要内容");
            xmlWriter.WriteString(obj.SY_ZSContent);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            #endregion
            xmlWriter.WriteEndElement();
            #endregion
            xmlWriter.WriteEndElement();
            #endregion

            #region 工作计划
            xmlWriter.WriteStartElement("工作计划");
            #region 工作内容
            xmlWriter.WriteStartElement("工作内容");
            for (int i = 1; i <= obj.WorkContents.Count; i++)
            {
                xmlWriter.WriteStartElement("工作内容"+i.ToString());

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
            #endregion
            #region 载荷管理
            xmlWriter.WriteStartElement("载荷管理");
                  #region 载荷管理
            xmlWriter.WriteStartElement("载荷管理");
                xmlWriter.WriteStartElement("对应试验ID");
                xmlWriter.WriteString(obj.Work_Load_SYID);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("卫星代号");
                xmlWriter.WriteString(obj.Work_Load_SatID);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("进程");
                xmlWriter.WriteString(obj.Work_Load_Process);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("事件");
                xmlWriter.WriteString(obj.Work_Load_Event);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("动作内容");
                xmlWriter.WriteString(obj.Work_Load_Action);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("开始时间");
                xmlWriter.WriteString(obj.Work_Load_StartTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("结束时间");
                xmlWriter.WriteString(obj.Work_Load_EndTime);
                xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            #endregion
                  #region 指令制作
            xmlWriter.WriteStartElement("指令制作");
                xmlWriter.WriteStartElement("对应试验ID");
                xmlWriter.WriteString(obj.Work_Command_SYID);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("试验项目");
                xmlWriter.WriteString(obj.Work_Command_SYItem);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("卫星代号");
                xmlWriter.WriteString(obj.Work_Command_SatID);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("指令内容");
                xmlWriter.WriteString(obj.Work_Command_Content);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("上注要求");
                xmlWriter.WriteString(obj.Work_Command_UpRequire);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("指令发送方向");
                xmlWriter.WriteString(obj.Work_Command_Direction);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("特殊需求");
                xmlWriter.WriteString(obj.Work_Command_SpecialRequire);
                xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            #endregion
            xmlWriter.WriteEndElement();
            #endregion
            #region 试验数据处理
            xmlWriter.WriteStartElement("试验数据处理");
                            for (int i = 1; i <= obj.SYDataHandles.Count; i++)
                            {
                                xmlWriter.WriteStartElement("工作内容"+i.ToString());

                                    xmlWriter.WriteStartElement("对应试验ID");
                                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].SYID);
                                    xmlWriter.WriteEndElement();

                                    xmlWriter.WriteStartElement("卫星代号");
                                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].SatID);
                                    xmlWriter.WriteEndElement();

                                    xmlWriter.WriteStartElement("圈次");
                                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].Laps);
                                    xmlWriter.WriteEndElement();

                                    xmlWriter.WriteStartElement("主站名称");
                                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].MainStationName);
                                    xmlWriter.WriteEndElement();

                                    xmlWriter.WriteStartElement("备站名称");
                                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].BakStationName);
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

                                    xmlWriter.WriteStartElement("事后数据处理");
                                    xmlWriter.WriteString(obj.SYDataHandles[i - 1].AfterWardsDataHandle);
                                    xmlWriter.WriteEndElement();
                                xmlWriter.WriteEndElement();
                            }
            xmlWriter.WriteEndElement();
            #endregion
            #region 指挥与监视
            xmlWriter.WriteStartElement("指挥与监视");
            for (int i = 1; i <= obj.DirectAndMonitors.Count; i++)
            {
                xmlWriter.WriteStartElement("工作内容"+i.ToString());

                    xmlWriter.WriteStartElement("对应试验ID");
                    xmlWriter.WriteString(obj.DirectAndMonitors[i - 1].SYID);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("时间段");
                    xmlWriter.WriteString(obj.DirectAndMonitors[i - 1].DateSection);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("指挥与监视任务");
                    xmlWriter.WriteString(obj.DirectAndMonitors[i - 1].Task);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("实时显示任务");
                    xmlWriter.WriteString(obj.DirectAndMonitors[i - 1].RealTimeShowTask);
                    xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            #endregion
            #region 实时控制
            xmlWriter.WriteStartElement("实时控制");
            for (int i = 1; i <= obj.RealTimeControls.Count; i++)
            {
                xmlWriter.WriteStartElement("工作内容"+i.ToString());

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
            #endregion
            #region 试验评估
            xmlWriter.WriteStartElement("试验评估");
            for (int i = 1; i <= obj.SYEstimates.Count; i++)
            {
                xmlWriter.WriteStartElement("工作内容"+i.ToString());

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
            #endregion
            #region 数据管理
            xmlWriter.WriteStartElement("数据管理");
            for (int i = 1; i <= obj.DataManages.Count; i++)
            {
                xmlWriter.WriteStartElement("工作内容"+i.ToString());

                    xmlWriter.WriteStartElement("工作");
                    xmlWriter.WriteString(obj.DataManages[i - 1].Work);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("对应数据描述");
                    xmlWriter.WriteString(obj.DataManages[i - 1].Description);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("开始时间");
                    xmlWriter.WriteString(obj.DataManages[i - 1].StartTime);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("结束时间");
                    xmlWriter.WriteString(obj.DataManages[i - 1].EndTime);
                    xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
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
        public string CreateTYSJFile(TYSJ obj,int type)
        {
            if (type == 0)
            {
                filename = (new FileNameMaker()).GenarateInternalFileNameTypeOne("TYSJ", obj.TaskID, obj.SatID);
            }
            xmlWriter = new XmlTextWriter(FilePath, Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("仿真推演试验数据");

            xmlWriter.WriteStartElement("SatName");
            xmlWriter.WriteString(obj.SatName);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Type");
            xmlWriter.WriteString(obj.Type);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TestItem");
            xmlWriter.WriteString(obj.TestItem);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("StartTime");
            xmlWriter.WriteString(obj.StartTime);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("EndTime");
            xmlWriter.WriteString(obj.EndTime);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Condition");
            xmlWriter.WriteString(obj.Condition);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            xmlWriter.Close();
            return FilePath;
        }
    }
}

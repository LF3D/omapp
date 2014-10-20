using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading;
using System.IO;
using OperatingManagement.Framework.Core;

namespace ServicesKernel.File
{
    public class FileNameMaker
    {
        private static string seperator = "_";
        private static Dictionary<string, string> dicTypeDates = new Dictionary<string, string>();
        private static Dictionary<string, int> dicTypeNos = new Dictionary<string, int>();

        #region 外部文件命名
        /// <summary>
        /// 只有外发才需要：版本号_对象标识_信源标识_模式标识_信息类型标识_日期_时刻_编号.xml
        /// </summary>
        /// <param name="infotype"></param>
        /// <param name="dateType">日期类型U:UTC日期;B:北京日期</param>
        /// <returns></returns>
        public static string GenarateFileNameTypeOne(string infotype,string dateType)
        {
            //版本号用2个字符表示，本版本命名方法固定为“01”，程序中配置为“01”；
            string ver = Param.Version;
            //对象标识用4个字符表示，采用可读性ASCII码字符，本任务固定为“7000”，程序中配置为“7000”；
            string flag = System.Configuration.ConfigurationManager.AppSettings["ObjectCode"];
            //模式标识用2个字符表示，用来标识文件内信息所对应的运行模式。“OP”代表实战，“TS”代表联试；
            string mode = Param.RunnningMode;

            if (ver == null || flag == null ||  mode == null)
                return null;

            string DateFlag = "";
            if (dateType == "U")
            {
                DateFlag = "U"+DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            }
            else if (dateType == "B")
            {
                DateFlag = "B" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            }
            return GetFileName(infotype, "", DateFlag, 1, ver, flag, mode);
        }

        /// <summary>
        /// 只有外发才需要：版本号_对象标识_信源标识_模式标识_信息类型标识_日期_时刻_编号.xml
        /// </summary>
        /// <param name="infotype"></param>
        /// <param name="dateType">日期类型U:UTC日期;B:北京日期</param>
        /// <param name="toMark">发送目标</param>
        /// <returns></returns>
        public static string GenarateFileNameTypeOne(string infotype, string dateType, string toMark, string runningMode)
        {
            //版本号用2个字符表示，本版本命名方法固定为“01”，程序中配置为“01”；
            string ver = Param.Version;
            //对象标识用4个字符表示，采用可读性ASCII码字符，本任务固定为“7000”，程序中配置为“7000”；
            string flag = System.Configuration.ConfigurationManager.AppSettings["ObjectCode"];
            //模式标识用2个字符表示，用来标识文件内信息所对应的运行模式。“OP”代表实战，“TS”代表联试；

            if (ver == null || flag == null)
                return null;

            string DateFlag = "";
            if (dateType == "U")
            {
                DateFlag = "U" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            }
            else if (dateType == "B")
            {
                DateFlag = "B" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            }
            return GetFileName(infotype, toMark, DateFlag, 1, ver, flag, runningMode);
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="infoCode"></param>
        /// <param name="fromMark"></param>
        /// <param name="toMark"></param>
        /// <param name="time"></param>
        /// <param name="nameWay">1,2,3分别代表命名方式一二三</param>
        /// <param name="version"></param>
        /// <param name="objectCode"></param>
        /// <param name="runningMode"></param>
        /// <returns></returns>
        private static string GetFileName(string infoCode, string toMark, string time
            , int nameWay, string version, string objectCode, string runningMode)
        {
            //中心编码
            string fromMark = Param.SourceCode;
            //扩展名
            string sSuffix = FileExchangeConfig.GetSuffixForSending(infoCode, fromMark, toMark);
            if (sSuffix == null)
                return null;
            else
            {
                switch (nameWay)
                {
                    case 1:
                        //版本号_对象标识_信源标识_模式标识_信息类型标识_日期_编号. xml
                        string sequence = GetSequenceNO(infoCode);
                        return version + seperator + objectCode + seperator + fromMark + seperator + runningMode +
                            seperator + infoCode + seperator + time + seperator + sequence + ".xml";
                    case 2:
                        return "";
                    case 3:
                        //信息名称_信息时间_发送方标识_接收方标识.扩展名
                        return infoCode + seperator + time + seperator + fromMark + seperator + toMark + "." + sSuffix;
                    default:
                        return null;
                }
            }
        }

        public static string GenarateFileNameTypeTwo(string type, string source, string target, string coordinate, int sequence)
        {
            string fileName = "";
            string extName = "";
            if (type.ToUpper()=="CODE")
            {
                extName = ".COD"; 
                fileName = source+DateTime.Now.ToString("yyyyMMdd")+sequence.ToString("00")+extName;
            }
            else if (type.ToUpper()=="JMXL")
            {
                extName = ".EPH";
                fileName = source+target+DateTime.Now.ToString("yyyyMMdd")+sequence.ToString("00")+coordinate+extName;
            }
            
            return fileName;
        }

        /// <summary>
        /// 外发时才需要，信息名称_信息时间_发送方标识_接收方标识.扩展名，时间为当前时间
        /// </summary>
        /// <param name="infotype"></param>
        /// <param name="source"></param>
        /// <param name="des"></param>
        /// <returns></returns>
        public static string GenarateFileNameTypeThree(string infotype, string des)
        {
            string DateFlag = DateTime.Now.ToString("yyyyMMddHHmm");
            return GetFileName(infotype, des, DateFlag, 3, "", "", "");
        }

        /// <summary>
        /// 外发时才需要，信息名称_信息时间_发送方标识_接收方标识.扩展名，时间为指定时间
        /// </summary>
        /// <param name="infotype"></param>
        /// <param name="des"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GenarateFileNameTypeThree(string infotype, string des, DateTime time)
        {
            string DateFlag = time.ToString("yyyyMMddHHmm");
            return GetFileName(infotype, des, DateFlag, 3, "", "", "");
        }

        /// <summary>
        /// 获取当日某类信息的流水号0000-9999
        /// sequence file: <sequence><info><name></name><value></value></info><info><name></name><value></value></info></sequence>>
        /// </summary>
        /// <param name="infoCode"></param>
        /// <returns></returns>
        private static string GetSequenceNO(string infoCode)
        {
            string strSeqPath = @"~/app_data/sequence.xml";
            string strSeqNo = "";
            string time = "";
            int iSeq = 0;

            ReaderWriterLock rwl = new ReaderWriterLock();
            try
            {
                rwl.AcquireWriterLock(1000);
                XDocument doc = XDocument.Load(GlobalSettings.MapPath(strSeqPath));
                XElement root = doc.Root;
                XElement element = root.Element(infoCode);
                strSeqNo = element.Value;
                if (element.Attribute("time") != null)
                {
                    time = element.Attribute("time").Value;
                    if (time == DateTime.Now.ToString("yyyyMMdd"))
                        iSeq = Convert.ToInt32(strSeqNo);
                    else
                        element.Attribute("time").Value = DateTime.Now.ToString("yyyyMMdd");
                }
                else
                    element.Add(new XAttribute("time", DateTime.Now.ToString("yyyyMMdd")));

                element.Value = (iSeq + 1).ToString().PadLeft(4, '0');
                StreamWriter oSW = new StreamWriter(GlobalSettings.MapPath(strSeqPath));
                oSW.Write(doc.ToString());
                oSW.Close();
            }
            catch (Exception ex)
            {
                strSeqNo = string.Empty;
            }
            finally
            {
                rwl.ReleaseWriterLock();
            }
            return strSeqNo;
        }
        #endregion

        #region 内部文件
        public string GenarateInternalFileNameTypeOne(string infotype,string taskid,string satid)
        {
            string filename = "";
            filename = "GL_" + infotype.ToUpper() + "_" + taskid.ToUpper() + "_" + satid.ToUpper() + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xml";
            return filename;
        }
        #endregion

    }
}

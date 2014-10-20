using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;
using OperatingManagement.Framework.Core;

namespace ServicesKernel.File
{
    public class FileExchangeConfig
    {
        private static string filePath = @"~/app_data/FileExchangeConfig.xml";
        private static string sSeperator = "-";
        private static Dictionary<string, string> sendList = null;
        private static Dictionary<string, string> recvList = null;
        private static Dictionary<string, List<string>> tgtList = null;
        private static Dictionary<string, string> nameList = null;
        private static List<string> recvSuffixList = null;

        /// <summary>
        /// 获取要发送数据类型后缀名
        /// </summary>
        /// <param name="infoCode">信息类型，Infotype表里的Exmark</param>
        /// <param name="fromMark">信源，XyxsInfo表里的AddrMark</param>
        /// <param name="toMark">信宿，XyxsInfo表里的AddrMark</param>
        /// <returns></returns>
        public static string GetSuffixForSending(string infoCode, string fromMark, string toMark)
        {
            if (sendList == null)
                Load();
            if (sendList == null)
                return null;

            string sKey = infoCode + sSeperator + fromMark + sSeperator + toMark;
            if (sendList.ContainsKey(sKey))
                return sendList[sKey].ToString();
            else
                return null;
        }

        /// <summary>
        /// 获取要发送的数据类型的信宿列表，XyxsInfo表里的AddrMark
        /// </summary>
        /// <param name="infoCode">信息类型，Infotype表里的Exmark</param>
        /// <returns></returns>
        public static List<string> GetTgtListForSending(string infoCode)
        {
            if (tgtList == null)
            { Load(); }
            if (sendList == null)
            { return null; }

            if (tgtList.ContainsKey(infoCode))
            { return tgtList[infoCode]; }
            else
            { return null; }
        }

        /// <summary>
        /// 获取要发送的目标系统的名称
        /// </summary>
        /// <param name="infoCode"></param>
        /// <returns></returns>
        public static string GetNameForType(string infoCode)
        {
            if (nameList == null)
            { Load(); }
            if (nameList == null)
            { return null; }

            if (nameList.ContainsKey(infoCode))
            { return nameList[infoCode]; }
            else
            { return null; }
        }

        private static void Load()
        {
            XDocument doc = XDocument.Load(GlobalSettings.MapPath(filePath));
            XElement root = doc.Root;
            if (doc == null)
                return;

            string sType = "";
            string sSrc = "";
            string sSuffix = "";
            string sName = "";
            XElement node = null;

            #region toSend
            var sInfos = root.Element("toSend").Elements("Info");
            if (sInfos != null && sInfos.Count() > 0)
            {
                sendList = new Dictionary<string, string>();
                tgtList = new Dictionary<string,List<string>>();
                List<string> listtgt;
                for (int i = 0; i < sInfos.Count(); i++)
                {
                    node = sInfos.ElementAt(i);
                    sType = node.Element("Type").Value;
                    sSrc = node.Element("SrcCode").Value;
                    sSuffix = node.Element("Suffix").Value;
                    listtgt= new List<string>();
                    foreach (XElement tgt in node.Element("TgtList").Elements("TgtCode"))
                    {
                        sendList.Add(sType + sSeperator + sSrc + sSeperator + tgt.Value, sSuffix);
                        if (!listtgt.Contains(tgt.Value))
                            listtgt.Add(tgt.Value);
                    }
                    if (!tgtList.ContainsKey(sType))
                    { tgtList.Add(sType, listtgt); }
                    else
                    {
                        tgtList[sType].AddRange(listtgt);
                    }
                }
            }
            #endregion

            #region toReceive
            var rInfos = root.Element("toReceive").Elements("Info");
            if (rInfos != null && rInfos.Count() > 0)
            {
                recvList = new Dictionary<string, string>();
                recvSuffixList = new List<string>();
                for (int i = 0; i < rInfos.Count(); i++)
                {
                    node = rInfos.ElementAt(i);
                    sType = node.Element("Type").Value;
                    sSrc = node.Element("SrcCode").Value;
                    sSuffix = node.Element("Suffix").Value;
                    if (sSrc.Contains(';'))
                    {
                        string[] strSrcs = sSrc.Split(new char[] { ';' });
                        for (int j = 0; j < strSrcs.Length; j++)
                        {
                            recvList.Add(sType + sSeperator + strSrcs[j], sSuffix);
                        }
                    }
                    else
                        recvList.Add(sType + sSeperator + sSrc, sSuffix);

                    if (!recvSuffixList.Contains(sSuffix))
                        recvSuffixList.Add(sSuffix);
                }
            }
        #endregion

            #region target
            var tInfos = root.Element("target").Elements("Info");
            if (tInfos != null && tInfos.Count() > 0)
            {
                nameList = new Dictionary<string, string>();
                for (int i = 0; i < tInfos.Count(); i++)
                {
                    node = tInfos.ElementAt(i);
                    sType = node.Element("Type").Value;
                    sName = node.Element("Name").Value;
                    nameList.Add(sType, sName);
                }
            }

            #endregion
            doc = null;
        }
    }
}

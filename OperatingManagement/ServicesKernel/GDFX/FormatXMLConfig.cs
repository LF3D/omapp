using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using OperatingManagement.Framework.Core;

namespace ServicesKernel.GDFX
{
    public class FormatXMLConfig
    {
        private static string filePath = @"~/app_data/GDJSJGFormat.xml";
        private static List<ResultType> lstResults;

        private static void LoadXML()
        {
            XDocument doc = XDocument.Load(GlobalSettings.MapPath(filePath));
            XElement root = doc.Root;
            if (doc == null)
                return;
            XElement node = null;
            XElement dataNode = null;
            var sTypes = root.Elements("type");
            if (sTypes != null && sTypes.Count() > 0)
            {
                lstResults = new List<ResultType>();
                ResultType oRType;
                for (int i = 0; i < sTypes.Count(); i++)
                {
                    node = sTypes.ElementAt(i);
                    oRType = new ResultType();
                    oRType.DisplayName = node.Element("displayname").Value;
                    oRType.Name = node.Element("name").Value;
                    oRType.FileName = node.Element("filename").Value;
                    oRType.IsBigFile = bool.Parse(node.Element("isBigFile").Value);
                    if (node.Element("timeBeginPoint") != null)
                        oRType.TimeBeginPoint = int.Parse(node.Element("timeBeginPoint").Value);
                    if (node.Element("timeFormat") != null)
                        oRType.TimeFormat = int.Parse(node.Element("timeFormat").Value);
                    var sDatas = node.Elements("data");
                    if (sDatas != null && sDatas.Count() > 0)
                    {
                        oRType.Results = new List<ResultData>();
                        ResultData oRData;
                        for (int j = 0; j < sDatas.Count(); j++)
                        {
                            oRData = new ResultData();
                            dataNode = sDatas.ElementAt(j);
                            oRData.Name = dataNode.Element("name").Value;
                            oRData.Position = int.Parse(dataNode.Element("position").Value);
                            oRData.Type = (dataNode.Element("datatype").Value == "double"? DataType.doubletype: DataType.inttype);
                            oRData.IntLen = int.Parse(dataNode.Element("intlen").Value);
                            oRData.DecLen = int.Parse(dataNode.Element("declen").Value);
                            oRType.Results.Add(oRData);
                        }
                    }
                    lstResults.Add(oRType);
                }
            }
        }

        public static List<ResultType> Results
        {
            get 
            {
                if (lstResults == null || lstResults.Count == 0)
                    LoadXML();
                return lstResults;
            }
        }

        public static ResultType GetTypeByName(string tname)
        {
            if (lstResults == null)
                LoadXML();
            if (lstResults == null)
                return default(ResultType);

            var query = lstResults.Where(a => a.Name == tname);
            ResultType oRType = default(ResultType);
            if (query != null && query.Count() > 0)
                oRType = query.FirstOrDefault();
            return oRType;
        }

        public static ResultType GetTypeByDisplayName(string displayName)
        {
            if (lstResults == null)
                LoadXML();
            if (lstResults == null)
                return default(ResultType);

            var query = lstResults.Where(a => a.DisplayName == displayName);
            ResultType oRType = default(ResultType);
            if (query != null && query.Count() > 0)
                oRType = query.FirstOrDefault();
            return oRType;
        }
    }

    public class ResultType
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public bool IsBigFile { get; set; }
        public int TimeBeginPoint { get; set; }
        public int TimeFormat { get; set; }
        public List<ResultData> Results { get; set; }

        public ResultData GetDataByName(string dname)
        {
            if (this.Results == null)
                return default(ResultData);

            var query = this.Results.Where(a => a.Name == dname);
            ResultData oRData = default(ResultData);
            if (query != null && query.Count() > 0)
                oRData = query.FirstOrDefault();
            return oRData;
        }
    }

    public class ResultData
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public DataType Type { get; set; }
        public int IntLen { get; set; }
        public int DecLen { get; set; }
    }

    public enum DataType
    {
        doubletype =0,
        inttype = 1
    }
}

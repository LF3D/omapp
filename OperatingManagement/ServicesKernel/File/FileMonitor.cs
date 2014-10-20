using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using OperatingManagement.DataAccessLayer.BusinessManage;


namespace OperatingManagement.ServicesKernel.File
{
    public class FileMonitor
    {

        #region -Properties-
        FileSystemWatcher watcher;
        private XmlDocument XmlDoc = null;
        private static object configLocker = new object();
        string directory;
        string filterType;
        #endregion

        public FileMonitor()
        {
            LoadValuesFromConfigurationXml();
            CreateWatcher();
        }
        public void CreateWatcher()
        {
            directory = attr["filePath"].ToString();
            filterType = attr["filterType"].ToString();
            //Create a new FileSystemWatcher
            watcher = new FileSystemWatcher();
            //Set the filter types of files;
            watcher.Filter = filterType;
            //Set the Path;
            watcher.Path = directory;
            //Subscribe to the Created event
            watcher.Created += new FileSystemEventHandler(watcher_FileCreated);
            //Enable the FileSystemWatcher events.
            watcher.EnableRaisingEvents = true;
        }

        void watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            //得到目录下所有文件进行处理

            foreach (string file in Directory.GetFiles(directory,filterType) )
            {
                FileInfo fi = new FileInfo(file);
                ReadToDataBase(fi);                
            }
            

            Console.WriteLine("A new file has been created");
            Console.ReadLine();

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 读取文件存入数据库
        /// </summary>
        void ReadToDataBase( FileInfo file)
        {
            string prefix = "";  //文件名前缀（既数据库表名)
            prefix = file.Name.Substring(0, file.Name.IndexOf('_'));
            string ctime;
            string source;
            string destination;
            string taskid;
            string infotype;
            string linecount;
            DataFileHandle df = new DataFileHandle(file.FullName);
            df.GetDataFileBaseInfo(out ctime,out source,out destination,out taskid,out infotype,out linecount);//读取文件基本信息

            switch (prefix)
            {
                case "SBJH"://设备工作计划
                    SBJH dllSBJH = new SBJH() { 
                        CTime = Convert.ToDateTime(ctime),
                        Source = source,
                        Destination = destination,
                        TaskID = taskid,
                        InfoType = infotype,
                        LineCount = Convert.ToInt32(linecount)
                    };
                    dllSBJH.Add();
                    break;
                case "MBXX"://空间目标信息
                    MBXX dllMBXX = new MBXX() {
                        CTime = Convert.ToDateTime(ctime),
                        Source = source,
                        Destination = destination,
                        TaskID = taskid,
                        InfoType = infotype,
                        LineCount = Convert.ToInt32(linecount)
                    };
                    dllMBXX.Add();
                    break;
                case "HJXX"://空间环境信息
                    HJXX dllHJXX = new HJXX() {
                        CTime = Convert.ToDateTime(ctime),
                        Source = source,
                        Destination = destination,
                        TaskID = taskid,
                        InfoType = infotype,
                        LineCount = Convert.ToInt32(linecount)
                    };
                    dllHJXX.Add();
                    break;
                case "YJBG"://碰撞预警报告
                    YJBG dllYJBG = new YJBG() {
                        CTime = Convert.ToDateTime(ctime),
                        Source = source,
                        Destination = destination,
                        TaskID = taskid,
                        InfoType = infotype,
                        LineCount = Convert.ToInt32(linecount)
                    };
                    dllYJBG.Add();
                    break;
                case "JHBG"://交会预报报告
                    JHBG dllJHBG = new JHBG() {
                        CTime = Convert.ToDateTime(ctime),
                        Source = source,
                        Destination = destination,
                        TaskID = taskid,
                        InfoType = infotype,
                        LineCount = Convert.ToInt32(linecount)
                    };
                    dllJHBG.Add();
                    break;
                case "SYXQ"://试验需求
                    SYXQ dllSYXQ = new SYXQ() {
                        CTime = Convert.ToDateTime(ctime),
                        Source = source,
                        Destination = destination,
                        TaskID = taskid,
                        InfoType = infotype,
                        LineCount = Convert.ToInt32(linecount)
                    };
                    dllSYXQ.Add();
                    break;
                case "GCJG"://天基目标观测试验数据处理结果
                    GCJG dllGCJG = new GCJG() {
                        CTime = Convert.ToDateTime(ctime),
                        Source = source,
                        Destination = destination,
                        TaskID = taskid,
                        InfoType = infotype,
                        LineCount = Convert.ToInt32(linecount)
                    };
                    dllGCJG.Add();
                    break;
                case "CZJG"://遥操作试验数据处理结果
                    CZJG dllCZJG = new CZJG() {
                        CTime = Convert.ToDateTime(ctime),
                        Source = source,
                        Destination = destination,
                        TaskID = taskid,
                        InfoType = infotype,
                        LineCount = Convert.ToInt32(linecount)
                    };
                    dllCZJG.Add();
                    break;
                case "JDJG"://空间机动试验数据处理结果
                    JDJG dllJDJG = new JDJG() {
                        CTime = Convert.ToDateTime(ctime),
                        Source = source,
                        Destination = destination,
                        TaskID = taskid,
                        InfoType = infotype,
                        LineCount = Convert.ToInt32(linecount)
                    };
                    dllJDJG.Add();
                    break;
                case "TYJG"://仿真推演试验数据处理结果
                    TYJG dllTYJG = new TYJG() {
                        CTime = Convert.ToDateTime(ctime),
                        Source = source,
                        Destination = destination,
                        TaskID = taskid,
                        InfoType = infotype,
                        LineCount = Convert.ToInt32(linecount)
                    };
                    dllTYJG.Add();
                    break;
            }
        }

        private void LoadValuesFromConfigurationXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ConfigFilePath);
            XmlDoc = doc;
            XmlNode node = XmlDoc.SelectSingleNode("configuration/core");
            XmlAttributeCollection attributeCollection = node.Attributes;
            for (int i = 0; i < attributeCollection.Count; i++)
            {
                attr.Add(attributeCollection[i].Name, attributeCollection[i].Value);
            }
        }

        #region -Indexer-
        private Dictionary<string, object> attr = new Dictionary<string, object>();
        /// <summary>
        /// Gets the value from congiguration settings by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                if (attr.ContainsKey(name))
                    return attr[name];
                else
                    return string.Empty;
            }
        }
        #endregion

        public static string PhysicalPath(string path)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            char dirSep = Path.DirectorySeparatorChar;
            rootPath = rootPath.Replace("/", dirSep.ToString());
            return string.Concat(rootPath.TrimEnd(dirSep), dirSep, path.TrimStart(dirSep));
        }

        #region -Config path-
        /// <summary>
        /// Gets the file path of configuration
        /// </summary>
        public static string ConfigFilePath
        {
            get
            {
                string path = "~/App.config";
                return PhysicalPath(path.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("~", ""));
            }
        }
        #endregion
    }

}

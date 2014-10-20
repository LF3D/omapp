using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ServicesKernel
{
    public class Param
    {
        /// <summary>
        /// 中心名称（文件方式三使用）
        /// </summary>
        public static string SourceName
        {
            get 
            {
                string config = System.Configuration.ConfigurationManager.AppSettings["ZXBMName"];
                if (config.Equals(string.Empty))
                    config = "运控评估中心YKZX(11 65 02 00)";
                return config;
            }
        }

        /// <summary>
        /// 中心编码（创建文件名使用）
        /// </summary>
        public static string SourceCode
        {
            get 
            {
                string config = System.Configuration.ConfigurationManager.AppSettings["ZXBM"];
                if (config.Equals(string.Empty))
                    config = "YKZX";
                return config;
            }
        }

        /// <summary>
        /// 版本号，文件名一使用
        /// </summary>
        public static string Version
        {
            get 
            {
                string config = System.Configuration.ConfigurationManager.AppSettings["Version"];
                if (config.Equals(string.Empty))
                    config = "01";
                return config;
            }
        }

        /// <summary>
        /// 运行模式，文件名一使用
        /// </summary>
        public static string RunnningMode
        {
            get 
            {
                string config = System.Configuration.ConfigurationManager.AppSettings["RunningMode"];
                if (config.Equals(string.Empty))
                    config = "TS";//联试
                return config;
            }
        }

        /// <summary>
        /// 保存路径，新建正式计划使用
        /// </summary>
        public static string SavePath
        {
            get 
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["SavePath"];
                if (path != string.Empty)
                {
                    if (path[path.Length - 1] != '\\')
                        path = path + @"\";
                }
                else
                    path = AppDomain.CurrentDomain.BaseDirectory +  @"SavePath\";
                return path;
            }
        }

        /// <summary>
        /// 保存路径，新建临时计划使用
        /// </summary>
        public static string TempJHSavePath
        {
            get 
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["tempjhsavepath"];
                if (path != string.Empty)
                {
                    if (path[path.Length - 1] != '\\')
                        path = path + @"\";
                }
                else
                    path = AppDomain.CurrentDomain.BaseDirectory +  @"TempJHSavePath\";
                return path;
            }
        }
        /// <summary>
        /// 输出路径，外发时使用
        /// </summary>
        public static string OutPutPath
        {
            get 
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["OutPutPath"];
                if (path != string.Empty)
                {
                    if (path[path.Length - 1] != '\\')
                        path = path + @"\";
                }
                else
                    path = AppDomain.CurrentDomain.BaseDirectory +  @"OutputPath\";
                return path;
            }
        }

        /// <summary>
        /// 轨道预报在服务总线注册的接收数据类型
        /// </summary>
        public static string GDYBType
        {
            get
            {
                return GetConfig("GDYBType", "S_GDYB");
            }
        }

        /// <summary>
        /// 轨道预报结果文件路径
        /// </summary>
        public static string GDYBResultFilePath
        {
            get
            {
                return GetConfig("GDYBResultFilePath"
                    , Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\Files\GDYBResults"));
            }
        }

        /// <summary>
        /// 资源调度在服务总线注册的接收数据类型
        /// </summary>
        public static string ZYDDType
        {
            get
            {
                return GetConfig("ZYDDType", "S_ZYDD");
            }
        }

        /// <summary>
        /// 资源调度结果文件路径
        /// </summary>
        public static string ZYDDResultFilePath
        {
            get
            {
                return GetConfig("ZYDDResultFilePath"
                    , Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\Files\ZYDDResults"));
            }
        }

        /// <summary>
        /// 本地IP
        /// </summary>
        public static string LocalIP
        {
            get
            {
                return GetConfig("LocalIP", "");
            }
        }

        /// <summary>
        /// 获取某项配置值
        /// </summary>
        private static string GetConfig(string configName, string defaultValue)
        {
            string config = System.Configuration.ConfigurationManager.AppSettings[configName];
            if (config.Equals(string.Empty))
                config = defaultValue;
            return config;
        }
    }
}

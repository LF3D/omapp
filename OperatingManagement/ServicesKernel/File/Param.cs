using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServicesKernel.File
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
                    config = "运控评估中心YKZX(02 04 00 00)";
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
    }
}

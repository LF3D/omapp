using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperatingManagement.DataAccessLayer.PlanManage;

namespace ServicesKernel.File
{
    public class StationInOutFileReader
    {
        /// <summary>
        /// 获得完整路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string GetFullFilePath(string filename)
        {
            string path = System.Configuration.ConfigurationManager.AppSettings["StationInOutFilePath"];
            if (path != string.Empty)
            {
                if (path[path.Length - 1] != '\\')
                    path = path + @"\";
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"TempJHSavePath\";
            }
            path += filename;
            return path ;
        }

        /// <summary>
        /// 读取进出站及航捷数据统计文件
        /// </summary>
        /// <param name="filename">文件完整路径</param>
        /// <returns></returns>
        public List<StationInOut> Read(string filename)
        {
            string fullpath = GetFullFilePath(filename);
            return ReadData(fullpath);
        }

        /// <summary>
        /// 读取进出站及航捷数据统计文件
        /// </summary>
        /// <param name="filename">文件完整路径</param>
        /// <returns></returns>
        public List<StationInOut> ReadData(string fileFullName)
        {
            StationInOut station;
            List<StationInOut> list = new List<StationInOut>();
            string[] content = System.IO.File.ReadAllLines(fileFullName);
            for (int i = 1; i < content.Length; i++)
            {
                station = GetStationInOutFromString(content[i]);
                station.rowIndex = i;
                list.Add(station);
            }

            return list;
        }


        public StationInOut GetStationInOutFromString(string data)
        {
            string[] temp;
            //ZM, N, QC, SJG, SP1, SP2, T1, T2, T3, T4, T5, T6, T7, h
            int[] vPos = new int[] { 0, 10, 19, 31, 34, 43, 54, 77, 100, 123, 146, 169, 192, 213 };//值在文件行中的起始位置
            int[] vLen = new int[] { 10, 9, 12, 3,  9, 9, 21, 21, 21, 21, 21, 21, 21, 9 };//值长度
            
            int i=0;
            StationInOut sta = new StationInOut();
            sta.ZM = data.Substring(vPos[i], vLen[i]).Trim();
            i++;
            sta.N = data.Substring(vPos[i], vLen[i]).Trim();
            i++;
            sta.QC = data.Substring(vPos[i], vLen[i]).Trim();
            i++;
            sta.SJG = data.Substring(vPos[i], vLen[i]).Trim();
            i++;
            sta.SP1 = data.Substring(vPos[i], vLen[i]).Trim();
            i++;
            sta.SP2 = data.Substring(vPos[i], vLen[i]).Trim();
            i++;
            sta.T1 = data.Substring(vPos[i], vLen[i]).Trim().Replace(": ", ":0").Replace(" ", "").Replace(":", "");
            i++;
            sta.T2 = data.Substring(vPos[i], vLen[i]).Trim().Replace(": ", ":0").Replace(" ", "").Replace(":", "");
            i++;
            sta.T3 = data.Substring(vPos[i], vLen[i]).Trim().Replace(": ", ":0").Replace(" ", "").Replace(":", "");
            i++;
            sta.T4 = data.Substring(vPos[i], vLen[i]).Trim().Replace(": ", ":0").Replace(" ", "").Replace(":", "");
            i++;
            sta.T5 = data.Substring(vPos[i], vLen[i]).Trim().Replace(": ", ":0").Replace(" ", "").Replace(":", "");
            i++;
            sta.T6 = data.Substring(vPos[i], vLen[i]).Trim().Replace(": ", ":0").Replace(" ", "").Replace(":", "");
            i++;
            sta.T7 = data.Substring(vPos[i], vLen[i]).Trim().Replace(": ", ":0").Replace(" ", "").Replace(":", "");
            i++;
            sta.h = data.Substring(vPos[i], vLen[i]).Trim();
            i++;
            
            //data = data.Replace("          ", ",");//10个空格
            //data = data.Replace("        ", ",");//8个空格
            //data = data.Replace("      ", ",");//6个空格
            //data = data.Replace("   ", ",");//3个空格
            //data = data.Replace("  ", ",");//2个空格
            //temp = data.Split(',');

            //sta.ZM = temp[i++];
            //sta.N = temp[i++];
            //sta.QC = temp[i++];
            //sta.SJG = temp[i++];
            //sta.SP1 = temp[i++];
            //sta.SP2 = temp[i++];
            //sta.T1 = temp[i++];
            //sta.T2 = temp[i++];
            //sta.T3 = temp[i++];
            //sta.T4 = temp[i++];
            //sta.T5 = temp[i++];
            //sta.T6 = temp[i++];
            //sta.T7 = temp[i++];
            //sta.h = temp[i++];
            return sta;
        }
    }
}
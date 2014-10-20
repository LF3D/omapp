using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;
using ServicesKernel.File;
using OperatingManagement.Framework.Core;

namespace ServicesKernel.GDFX
{
    /// <summary>
    /// 轨道分析-差值分析，计算所需的文件必须在同一文件夹下
    /// </summary>
    public class Intepolater
    {
        private string[] fileNames = new string[] { "JPLEPH", "TESTRECL", "WGS84.GEO", "eopc04_IAU2000.dat" };
        private const string dllPath = @"E:\YKZX_Manage\";
        private const string dllFolder = @"GDDLL\Intep\";
        private const string dllName = @"IntpDLL.dll";
        //private const string outputPath = @"output\";
        private string strDllPath;
        //
        //private IntPtr handle;
        private static Intepolater _instance = null;
        private static object _locker = new object();
        public bool isCaculating = false;

        /// <summary>
        /// 使用唯一实例是为了控制同一时间只能有一个计算在进行
        /// </summary>
        public static Intepolater Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new Intepolater();
                        }
                    }
                }
                return _instance;
            }
        }

        public Intepolater()
        {
            strDllPath = string.Empty;

        }

        private string DllPath
        {
            get
            {
                if (strDllPath.Equals(string.Empty))
                    strDllPath = dllPath + dllFolder;//GlobalSettings.MapPath("~" + dllFolder);
                return strDllPath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public string CheckEphemerisFileData(string fileName, DateTime beginDate, DateTime endDate)
        {
            string msg = "星历文件数据行{0}格式不合法";
            bool blResult = false;
            int iLine = 1;
            string strLine = string.Empty;
            //int iTmp = 0;
            string[] datas = new string[0];
            string sTmp = string.Empty;
            DateTime date;
            StreamReader oSReader = new StreamReader(fileName);

            oSReader.BaseStream.Seek(0, SeekOrigin.Begin);
            strLine = oSReader.ReadLine();//I6

            /*
            if (strLine != null)
                strLine = strLine.Trim();
            else
            {
                oSReader.Close();
                return string.Format(msg + "【不正常结束】", iLine);
            }

            blResult = DataValidator.ValidateInt(strLine, 6, out iTmp);
            if (!blResult)
            {
                oSReader.Close();
                return string.Format(msg + "【int，长为6】", iLine);
            }*/

            //iLine++;
            //strLine = oSReader.ReadLine();
            while (strLine != null)
            {
                blResult = CheckEphemeris2ndRowData(strLine, out date);
                if (!blResult)
                {
                    oSReader.Close();
                    return string.Format(msg + "【日期不合法】", iLine);
                }
                if (date < beginDate || date > endDate)
                {
                    oSReader.Close();
                    return string.Format(msg + "【星历文件数据时间不在差值文件范围内】", iLine);
                }
                iLine++;
                strLine = oSReader.ReadLine();
            }
            iLine--;
            oSReader.Close();
            //if (iTmp != (iLine - 1))//第一行的行数和后面的行数对应不起来
            //    return string.Format("【星历文件数据总行数{0}与第一行总数{1}不符】", iLine - 1, iTmp);
            return string.Empty;
        }

        /// <summary>
        /// 检查星历文件每行数据的合法性
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private bool CheckEphemeris2ndRowData(string rowData, out DateTime date)
        {
            date = DateTime.MinValue;
            rowData = rowData.Trim();
            if (rowData.Equals(string.Empty))
                return false;

            string sTmp = string.Empty;
            int iIdx = 0;
            string[] datas = new string[0];
            bool blResult = false;
            datas = DataValidator.SplitRowDatas(rowData);//YYYY  MM  DD  HH  MM  SS.SSSSSS  x  y  z  vx  vy  vz
            if (datas.Length != 12)
                return blResult;

            iIdx = 0;
            sTmp = DataValidator.GetArrayTimeString(datas, iIdx);
            blResult = DataValidator.ValidateDate(sTmp, out date);
            if (!blResult)
                return blResult;

            iIdx += 6;//为double
            for (int i = iIdx; i < datas.Length; i++)
            {
                blResult = DataValidator.ValidateFloat(datas[i], 16, 6);
                if (!blResult)
                    return blResult;
            }
            return blResult;
        }

        /// <summary>
        /// 检查差值文件数据合法性
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public string CheckTimeSeriesFileData(string fileName, out DateTime beginDate, out DateTime endDate)
        {
            beginDate = DateTime.MaxValue;
            endDate = DateTime.MinValue;
            string msg = "差值计算时间文件数据行{0}格式不合法";
            bool blResult = false;
            int iLine = 1;
            string strLine = string.Empty;
            int iIdx = 0;
            int iTmp = 0;
            double dbTmp = 0;
            string[] datas = new string[0];
            string sTmp = string.Empty;
            DateTime lastDate = DateTime.MinValue;
            DateTime date;
            StreamReader oSReader = new StreamReader(fileName);

            oSReader.BaseStream.Seek(0, SeekOrigin.Begin);
            strLine = oSReader.ReadLine();
            if (strLine != null)
                strLine = strLine.Trim();
            else
            {
                oSReader.Close();
                return string.Format("差值计算时间文件{0}是空文件", fileName);
            }

            blResult = DataValidator.ValidateInt(strLine, 0, out iTmp);
            if (!blResult)
            {
                oSReader.Close();
                return string.Format(msg + "【应为int】", iLine);
            }
            if (iTmp > 1 || iTmp < 0)
            {
                oSReader.Close();
                return string.Format(msg + "【应为0或者1】", iLine);
            }

            iLine++;
            strLine = oSReader.ReadLine();
            if (iTmp == 0)//YYYY  MM  DD  HH  MM  SS.SSSSSS  	DT		N
            {
                #region 检查等差数列时间值
                datas = DataValidator.SplitRowDatas(strLine);
                if (datas.Length != 8)
                {
                    oSReader.Close();
                    return string.Format(msg + "【包含数据量不足，应为8个】", iLine);
                }

                iIdx = 0;
                sTmp = DataValidator.GetArrayTimeString(datas, iIdx);
                blResult = DataValidator.ValidateDate(sTmp, out date);
                if (!blResult)
                {
                    oSReader.Close();
                    return string.Format(msg + "【日期不合法】", iLine);
                }

                iIdx += 5;
                if (!DataValidator.ValidateFloat(datas[iIdx], 10, 6, out dbTmp) || dbTmp > 60 || dbTmp < 0)
                {
                    oSReader.Close();
                    return string.Format(msg + "【最大差值应在60秒内】", iLine);
                }

                iIdx++;
                if (!DataValidator.ValidateInt(datas[iIdx], 6, out iTmp))
                {
                    oSReader.Close();
                    return string.Format(msg + "【int，长度为6】", iLine);
                }

                beginDate = date;
                endDate = date.AddSeconds(iTmp * dbTmp);
                #endregion
            }
            else
            {
                while (strLine != null)
                {
                    blResult = CheckTimeSeriesRowData(strLine, out date);
                    if (!blResult)
                    {
                        oSReader.Close();
                        return string.Format(msg, iLine);
                    }
                    if (beginDate >= date)
                        beginDate = date;
                    if (endDate <= date)
                        endDate = date;

                    if (lastDate != DateTime.MinValue && (date - lastDate).TotalSeconds > 60)
                    {
                        oSReader.Close();
                        return string.Format(msg + "【与上一条的时间差大于60秒】", iLine);
                    }
                    lastDate = date;
                    iLine++;
                    strLine = oSReader.ReadLine();
                }
            }
            oSReader.Close();
            return string.Empty;
        }

        /// <summary>
        /// 检查差值文件每行数据合法性
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private bool CheckTimeSeriesRowData(string rowData, out DateTime date)
        {
            date = DateTime.MinValue;
            rowData = rowData.Trim();
            if (rowData.Equals(string.Empty))
                return false;

            string sTmp = string.Empty;
            int iIdx = 0;
            string[] datas = new string[0];
            bool blResult = false;
            datas = DataValidator.SplitRowDatas(rowData);//YYYY  MM  DD  HH  MM  SS.SSSSSS
            if (datas.Length != 6)
                return blResult;

            iIdx = 0;
            sTmp = DataValidator.GetArrayTimeString(datas, iIdx);
            blResult = DataValidator.ValidateDate(sTmp, out date);
            return blResult;
        }



        /// <summary>
        /// 计算，两个文件路径必须一致
        /// </summary>
        /// <param name="mainFilePath"></param>
        /// <returns></returns>
        public string DoCaculate(string ephemerisFileFullName, string timeSeriesFileFullName, out string resultFileFullName)
        {
            resultFileFullName = string.Empty;
            string strResult = string.Empty;
            if (isCaculating)
            {
                strResult = "同一时间只能只能执行单次计算任务，请稍候再计算";
                return strResult;
            }
            isCaculating = true;

            int iTmp = ephemerisFileFullName.LastIndexOf(@"\") + 1;
            string filePath = ephemerisFileFullName.Substring(0, iTmp);
            int[] iPath = DataValidator.GetIntPath(filePath);
            int[] iEName = DataValidator.GetIntPath(ephemerisFileFullName.Substring(iTmp));
            int[] iTName = DataValidator.GetIntPath(timeSeriesFileFullName.Substring(iTmp));
            if ((iPath.Length + iEName.Length) > 100 || (iPath.Length + iTName.Length) > 101)
            {
                strResult = "文件全路径字符长度不得大于100，请重新设置。";
                isCaculating = false;
                return strResult;
            }
            int iResult = -1;
            INTEP(ref iPath[0], ref iEName[0], ref iTName[0], iPath.Length, iEName.Length, iTName.Length, ref iResult);
            resultFileFullName = filePath + "intepolateResult.txt";
            isCaculating = false;
            return strResult;
        }

        /// <summary>
        /// 差值分析
        /// </summary>
        /// <param name="dirIn">文件路径数组</param>
        /// <param name="f1">星历数据文件名数组</param>
        /// <param name="f2">时间序列数据文件名数组</param>
        /// <param name="Ndir">文件路径数组长度</param>
        /// <param name="n1">f1的长度</param>
        /// <param name="n2">f2的长度</param>
        /// <param name="kjg">表示计算是否成功，0失败，1成功</param>
        [DllImport(dllPath + dllFolder + dllName,
            SetLastError = true, CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.StdCall)]
        public static extern void INTEP(ref int dirIn, ref int f1, ref int f2, int Ndir, int n1, int n2, ref int kjg);
    }
}

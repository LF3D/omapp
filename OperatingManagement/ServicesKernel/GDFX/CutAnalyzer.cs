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
    /// 轨道分析-交会分析
    /// </summary>
    public class CutAnalyzer
    {
        private string[] fileNames = new string[] { "JPLEPH", "TESTRECL", "WGS84.GEO", "eopc04_IAU2000.dat" };
        private const string dllPath = @"E:\YKZX_Manage\";
        private const string dllFolder = @"GDDLL\CutAna\";
        private const string dllName = @"CutAnaDLL.dll";
        private const string outputPath = @"output\";
        private string strDllPath;
        //
        //private IntPtr handle;
        private static CutAnalyzer _instance = null;
        private static object _locker = new object();
        public bool isCaculating = false;

        /// <summary>
        /// 使用唯一实例是为了控制同一时间只能有一个计算在进行
        /// </summary>
        public static CutAnalyzer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new CutAnalyzer();
                        }
                    }
                }
                return _instance;
            }
        }

        public CutAnalyzer()
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
        /// 检查文件中数据的合法性
        /// </summary>
        /// <param name="fileFullName">文件全名</param>
        /// <param name="minDate">最小时间</param>
        /// <param name="maxDate">最大时间</param>
        /// <param name="interval">单位豪秒</param>
        /// <returns></returns>
        public string CheckFileData(string fileFullName, out DateTime minDate, out DateTime maxDate, out int interval)
        {
            /*
             * 先读两行，以获取时间间隔和最小时间
             * 校验每行数据合法性
             * 最大时间等于最后一行的时间
             */
            minDate = DateTime.MinValue;
            maxDate = DateTime.MaxValue;
            interval = 0;

            bool blResult = true;
            int iLine = 0;
            string msg = "交会分析文件数据行{0}格式不合法";
            string strLine = string.Empty;
            DateTime date;

            StreamReader oSReader = new StreamReader(fileFullName);
            oSReader.BaseStream.Seek(0, SeekOrigin.Begin);
            strLine = oSReader.ReadLine();
            iLine++;
            if (strLine != null)
            {
                //检查第一行，获取开始时间
                blResult = CheckRowData(strLine.Trim(), out date);
                if (!blResult)
                {
                    oSReader.Close();
                    return string.Format(msg + "【日期不合法】", iLine);
                }
                minDate = date;
            }
            else
            {
                oSReader.Close();
                return string.Format(msg + "【不正常结束】", iLine);//第一行为空返回false
            }

            //获取第二行，获取时间间隔
            strLine = oSReader.ReadLine();
            iLine++;
            if (strLine != null)
            {
                blResult = CheckRowData(strLine.Trim(), out date);
                if (!blResult)
                {
                    oSReader.Close();
                    return string.Format(msg + "【日期不合法】", iLine);
                }
                interval = (int)(date - minDate).TotalMilliseconds;
            }
            else
            {
                maxDate = minDate;
                return string.Empty;//第二行为空返回true
            }

            //逐行检查数据合法性
            strLine = oSReader.ReadLine();
            while (strLine != null)
            {
                blResult = CheckRowData(strLine.Trim(), out date);
                if (!blResult)
                {
                    oSReader.Close();
                    return string.Format(msg + "【日期不合法】", iLine);
                }
                iLine++;
                strLine = oSReader.ReadLine();
            }
            oSReader.Close();
            maxDate = date;
            return string.Empty;
        }

        /// <summary>
        /// 检查每行数据
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        private bool CheckRowData(string rowData, out DateTime date)
        {
            date = DateTime.MinValue;
            if (rowData == null || rowData.Trim().Equals(string.Empty))
                return false;

            rowData = rowData.Trim();
            bool blResult = true;
            string[] datas = new string[0];
            string sTmp = string.Empty;
            //YYYY  MM  DD  HH  MM  SS.SSSSSS x1 y1 z1 vx1 vy1 vz1
            //格式: 2空格，I4，空格，2（I2，空格），2（I2，‘：’），F4.1，2空格，6(F16.6,2空格)
            datas = DataValidator.SplitRowDatas(rowData);
            if (datas.Length != 12)
                return false;

            //前六个数组成日期型
            sTmp = DataValidator.GetArrayTimeString(datas, 0);
            blResult = DataValidator.ValidateDate(sTmp, out date);
            if (!blResult)
                return blResult;

            //后6个数为double
            for (int i = 6; i < datas.Length; i++)
            {
                blResult = DataValidator.ValidateFloat(datas[i], 16, 6);
                if (!blResult)
                    return blResult;
            }
            return blResult;
        }

        /// <summary>
        /// 从行中取数据
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="ym">年月日时分，int[5]</param>
        /// <param name="s">秒，double</param>
        /// <param name="orb">double[6]</param>
        /// <returns></returns>
        private bool GetRowData(string rowData, out int[] ym, out double s, out double[] orb)
        {
            ym = new int[0];
            s = 0;
            orb = new double[0];

            string[] datas = new string[0];
            int iIdx = 0;
            //YYYY  MM  DD  HH  MM  SS.SSSSSS x1 y1 z1 vx1 vy1 vz1
            //格式: 2空格，I4，空格，2（I2，空格），2（I2，‘：’），F4.1，2空格，6(F16.6,2空格)
            datas = DataValidator.SplitRowDatas(rowData);
            ym = new int[5];
            for (int i = 0; i < 5; i++)
            {
                ym[i] = Convert.ToInt32(datas[i]);
            }
            iIdx = 5;
            s = Convert.ToDouble(datas[iIdx]);

            //后6个数为double
            iIdx++;
            orb = new double[6];
            for (int i = iIdx; i < datas.Length; i++)
            {
                orb[i - 6] = Convert.ToDouble(datas[i]);
            }
            return true;
        }

        /// <summary>
        /// 从行中取日期
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="ym">年月日时分，int[5]</param>
        /// <param name="s">秒，double</param>
        /// <param name="orb">double[6]</param>
        /// <returns></returns>
        private bool GetRowDate(string rowData, out DateTime date)
        {
            date = DateTime.MinValue;

            string[] datas = new string[0];
            string sTmp = string.Empty;
            //YYYY  MM  DD  HH  MM  SS.SSSSSS x1 y1 z1 vx1 vy1 vz1
            //格式: 2空格，I4，空格，2（I2，空格），2（I2，‘：’），F4.1，2空格，6(F16.6,2空格)
            datas = DataValidator.SplitRowDatas(rowData);
            sTmp = DataValidator.GetArrayTimeString(datas, 0);
            date = DateTime.ParseExact(sTmp, "yyyyMMdd HHmmss.fff", CultureInfo.InvariantCulture);
            return true;
        }

        /// <summary>
        /// 复制数据文件到交会分析程序路径下
        /// </summary>
        /// <param name="srcFileFullName"></param>
        /// <returns></returns>
        public bool CopyDataFile(string srcFileFullName)
        {
            bool blResult = true;
            string strResult = DataFileHandle.CopyFile(srcFileFullName, DllPath + @"Input\", "CutAnalyzeData.dat");
            if (strResult.Equals(string.Empty))
                blResult = true;
            else
                blResult = false;
            return blResult;
        }

        /// <summary>
        /// 判断所有文件是否都存在
        /// </summary>
        /// <returns></returns>
        public string IsAllFileExist()
        {
            string strResult = string.Empty;
            for (int i = 0; i < fileNames.Length; i++)
            {
                if (!DataFileHandle.Exists(DllPath + fileNames[i]))
                {
                    strResult = string.Format("文件{0}不存在", fileNames[i]);
                    break;
                }
            }
            return strResult;
        }

        /// <summary>
        /// 开始计算
        /// </summary>
        /// <param name="subFileFullName">主文件名</param>
        /// <param name="targetFileFullName">计算目标文件名</param>
        /// <param name="beginDate">计算数据开始时间</param>
        /// <param name="endDate">计算数据结束时间</param>
        /// <param name="maxInterval">较大时间间隔</param>
        /// <param name="minInterval">较小时间间隔</param>
        /// <param name="isSubInterval">是否主文件的间隔大</param>
        /// <param name="isSubBeginDate">是否从主文件数据开始计算</param>
        /// <param name="isSubEndDate">是否从主文件数据结束计算</param>
        /// <param name="resultFileName">结果文件名（未加_STW.dat和_UNW.dat）</param>
        /// <returns></returns>
        public string DoCaculate(string subFileFullName, string targetFileFullName, DateTime beginDate, DateTime endDate, int maxInterval
            , int minInterval, bool isSubInterval, bool isSubBeginDate, bool isSubEndDate, out string resultFileName)
        {
            DateTime now = DateTime.Now;
            resultFileName = "CutAna";
            string strResult = string.Empty;
            if (isCaculating)
            {
                strResult = "同一时间只能只能执行单次计算任务，请稍后再计算";
                return strResult;
            }
            isCaculating = true;

            #region 变量声明
            StreamReader oSubSR = new StreamReader(subFileFullName);
            StreamReader oTgtSR = new StreamReader(targetFileFullName);
            string filePath = subFileFullName.Substring(0, subFileFullName.LastIndexOf(@"\") + 1) + @"\output\";
            StreamWriter oSTWResult = new StreamWriter(filePath + string.Format("{0}_{1}_{2}.dat", resultFileName, "STW", now.ToString("yyyyMMddhhmmss")));
            StreamWriter oUNWResult = new StreamWriter(filePath + string.Format("{0}_{1}_{2}.dat", resultFileName, "UNW", now.ToString("yyyyMMddhhmmss")));
            #endregion

            resultFileName = string.Format("{0}_{1}_{2}.dat", resultFileName, "UNW", now.ToString("yyyyMMddhhmmss"));
            oSubSR.BaseStream.Seek(0, SeekOrigin.Begin);
            oTgtSR.BaseStream.Seek(0, SeekOrigin.Begin);
            //间隔大的放在前面，哪个间隔大又是从它开始，则开始时间设为minValue
            if (isSubInterval)
            {
                if (isSubBeginDate)
                    strResult = CaculateTotalFile(oSubSR, oTgtSR, oSTWResult, oUNWResult, maxInterval, minInterval, DateTime.MinValue, endDate, true);
                else
                    strResult = CaculateTotalFile(oSubSR, oTgtSR, oSTWResult, oUNWResult, maxInterval, minInterval, beginDate, endDate, true);

            }
            else
            {
                if (isSubBeginDate)
                    strResult = CaculateTotalFile(oTgtSR, oSubSR, oSTWResult, oUNWResult, maxInterval, minInterval, beginDate, endDate, false);
                else
                    strResult = CaculateTotalFile(oTgtSR, oSubSR, oSTWResult, oUNWResult, maxInterval, minInterval, DateTime.MinValue, endDate, false);
            }

            oSubSR.Close();
            oTgtSR.Close();
            oSTWResult.Close();
            oUNWResult.Close();

            isCaculating = false;
            return strResult;
        }

        /// <summary>
        /// 计算整个文件
        /// </summary>
        /// <param name="oSrcSR">间隔大的文件流</param>
        /// <param name="oTgtSR">间隔小的文件流</param>
        /// <param name="oSTWResult">STW结果的文件流</param>
        /// <param name="oUNWResult">UNW结果的文件流</param>
        /// <param name="maxInterval">最大间隔</param>
        /// <param name="minInterval">最小间隔</param>
        /// <param name="beginDate">计算开始时间</param>
        /// <param name="endDate">计算结束时间</param>
        /// <param name="isRightTurn">是否正常顺序</param>
        /// <returns></returns>
        private string CaculateTotalFile(StreamReader oSrcSR, StreamReader oTgtSR, StreamWriter oSTWResult, StreamWriter oUNWResult
            , int maxInterval, int minInterval, DateTime beginDate, DateTime endDate, bool isRightTurn)
        {
            #region 变量声明
            string strResult = string.Empty;
            int[] YM1;
            double S1;
            double[] Orb1;
            int[] YM2;
            double S2;
            double[] Orb2;
            int[] YM3;
            double S3;
            double[,] Orb3;
            string strSubLine = string.Empty;
            string strTgtLine = string.Empty;
            DateTime subDate = DateTime.MinValue;
            DateTime tgtDate = DateTime.MinValue;
            bool blResult = false;
            bool isEnd = false;
            bool isBegin = false;
            double dbTmp = 0;
            string strLastSrcLine = string.Empty;
            #endregion

            strSubLine = oSrcSR.ReadLine();
            if (beginDate == DateTime.MinValue)
                isBegin = true;
            while (strSubLine != null)
            {
                strSubLine = strSubLine.Trim();
                if (strSubLine.Equals(string.Empty))
                    break;

                //得出源日期
                blResult = GetRowDate(strSubLine, out subDate);
                if (!blResult)
                {
                    strResult = string.Format("获取文件行{0}中的数据日期错误", strSubLine);
                    break;
                }

                //获取离开始日期最近的一个日期
                if (!isBegin)//标明开始日期不是间隔大文件的开始日期
                {
                    dbTmp = Math.Abs((subDate - beginDate).TotalMilliseconds);
                    if (dbTmp < maxInterval / 2)//开始日期距某时间点的（取短一侧）最大时差为：可能会落在两时间点中间，故取最大间隔/2
                        isBegin = true;
                    else if (dbTmp == maxInterval / 2)//正好落在中间
                    {
                        if (subDate < beginDate)//如小于开始日期，则取大一点的时间值
                        {
                            strSubLine = oSrcSR.ReadLine();
                            blResult = GetRowDate(strSubLine, out subDate);
                            if (!blResult)
                            {
                                strResult = string.Format("获取文件行{0}中的数据日期错误", strSubLine);
                                break;
                            }
                        }
                        isBegin = true;
                    }
                }

                if (isBegin)
                {
                    if (subDate > endDate)//如果大于结束时间则终止循环
                        break;
                    //得出源数据
                    blResult = GetRowData(strSubLine, out YM1, out S1, out Orb1);
                    if (!blResult)
                    {
                        strResult = string.Format("获取文件行{0}中的数据错误", strSubLine);
                        break;
                    }

                    //取得计算目标的数据
                    strResult = GetToCaculateParam(oTgtSR, minInterval, subDate, out YM2, out S2, out Orb2, out strTgtLine, out isEnd);
                    if (!strResult.Equals(string.Empty))
                        break;

                    int iResult;
                    //计算，如果计算函数不关心左右，则可以不用调整顺序
                    if (isRightTurn)
                        iResult = Caculate(YM1, S1, Orb1, YM2, S2, Orb2, out YM3, out S3, out Orb3);
                    else
                        iResult = Caculate(YM2, S2, Orb2, YM1, S1, Orb1, out YM3, out S3, out Orb3);

                    if (iResult != 1)
                    {
                        strResult = string.Format("计算数据行{0}，{1}错误，错误号：{2}", strSubLine, strTgtLine, iResult);
                        break;
                    }

                    ////存储计算结果到文件
                    //if (isRightTurn)
                    //    blResult = WriteToResult(oSTWResult, oUNWResult, YM3, S3, Orb3);
                    //else
                    blResult = WriteToResult(oSTWResult, oUNWResult, YM3, S3, Orb3);
                    if (!blResult)
                    {
                        strResult = "存储计算结果出错";
                        break;
                    }

                    //如果计算目标已经到最后一行，终止
                    if (isEnd)
                        break;
                }
                strSubLine = oSrcSR.ReadLine();
            }
            return strResult;
        }

        /// <summary>
        /// 获取计算目标行的计算参数
        /// </summary>
        /// <param name="oSR">目标文件流</param>
        /// <param name="minInterval">最小时间间隔</param>
        /// <param name="compareDate">要比较的日期</param>
        /// <param name="ym">输出</param>
        /// <param name="s">输出</param>
        /// <param name="orb">输出</param>
        /// <param name="line">行文本</param>
        /// <param name="isEnd">是否到结尾了</param>
        /// <returns></returns>
        private string GetToCaculateParam(StreamReader oSR, int minInterval, DateTime compareDate
            , out int[] ym, out double s, out double[] orb, out string line, out bool isEnd)
        {
            #region 变量声明
            ym = new int[0];
            s = 0;
            orb = new double[0];
            line = string.Empty;
            isEnd = false;
            string strLine = string.Empty;
            string strLastLine = string.Empty;
            bool blResult = false;
            string strResult = string.Empty;
            DateTime date = DateTime.MinValue;
            TimeSpan ts1 = new TimeSpan();
            TimeSpan ts2;
            #endregion

            strLine = oSR.ReadLine();
            while (strLine != null)
            {
                blResult = GetRowDate(strLine, out date);
                if (!blResult)
                {
                    strResult = string.Format("获取文件行{0}中的数据日期错误", strLine);
                    break;
                }
                ts1 = date - compareDate;
                //间隔大的时间点散落在间隔小的时间点内，故距离某间隔大的时间点最近的间隔小的时间点，二者最长的时差才会是小间隔，找到此点既退出
                if (Math.Abs(ts1.TotalMilliseconds) <= minInterval)
                {
                    strLastLine = strLine;
                    break;
                }
                strLine = oSR.ReadLine();
            }

            if (!strResult.Equals(string.Empty))
                return strResult;

            if (!strLastLine.Equals(string.Empty))
            {
                if (date < compareDate)//如果日期比比较日期小就再读一行，不然没必要再读一行了
                {
                    strLine = oSR.ReadLine();//再读一行，和上一行的Timespan比较
                    if (strLine != null && !strLine.Trim().Equals(string.Empty))
                    {
                        blResult = GetRowDate(strLine, out date);
                        if (!blResult)//Error
                        {
                            strResult = string.Format("获取文件行{0}中的数据日期错误", strLine);
                            return strResult;
                        }
                        ts2 = date - compareDate;
                        if (Math.Abs(ts2.TotalMilliseconds) < minInterval)
                        {
                            //比较两个时间间隔，如果上个小，则取上个
                            if (Math.Abs(ts1.TotalMilliseconds) < Math.Abs(ts2.TotalMilliseconds))
                                strLine = strLastLine;
                        }
                        else if (!(Math.Abs(ts2.TotalMilliseconds) == minInterval && Math.Abs(ts1.TotalMilliseconds) == Math.Abs(ts2.TotalMilliseconds)))
                            strLine = strLastLine;
                    }
                    else
                    {
                        isEnd = true;
                        strLine = strLastLine;
                    }
                }
                else
                    strLine = strLastLine;

                blResult = GetRowData(strLine, out ym, out s, out orb);
                if (!blResult)//Error
                {
                    strResult = string.Format("获取文件行{0}中的数据错误", strLine);
                    return strResult;
                }
            }
            line = strLine;
            return strResult;
        }

        /// <summary>
        /// 对两组数据进行交会分析计算
        /// </summary>
        /// <param name="YM1"></param>
        /// <param name="S1"></param>
        /// <param name="Orb1"></param>
        /// <param name="YM2"></param>
        /// <param name="S2"></param>
        /// <param name="Orb2"></param>
        /// <param name="YM3"></param>
        /// <param name="S3"></param>
        /// <param name="Orb3"></param>
        /// <returns></returns>
        private int Caculate(int[] YM1, double S1, double[] Orb1, int[] YM2, double S2, double[] Orb2, out int[] YM3, out double S3, out double[,] Orb3)
        {
            YM3 = new int[5];
            S3 = 0;
            Orb3 = new double[2, 11];
            int iResult = 0;

            iResult = CutAna(ref YM1[0], S1, ref Orb1[0], ref YM2[0], S2, ref Orb2[0]
                , ref YM3[0], ref S3, ref Orb3[0, 0]);
            //Orb3 = ConvertArray(Orb3);
            return iResult;
        }

        /// <summary>
        /// 转换数组，Fortran数组列与C#数组的行与列是反的（[0,1]->[1,0]）
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private double[,] ConvertArray(double[,] result)
        {
            double[,] dbArray = new double[result.GetLength(1), result.GetLength(0)];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    dbArray[j, i] = result[i, j];
                }
            }
            return dbArray;
        }


        /// <summary>
        /// 交会分析
        /// </summary>
        /// <param name="dirIn">文件路径</param>
        /// <param name="Ndir">路径数组长度</param>
        /// <param name="Kjg">计算是否成功，1成功0失败</param>
        [DllImport(dllPath + dllFolder + dllName,
            SetLastError = true, CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int CutAna(ref int Ym1, double S1, ref double Orb1, ref int Ym2, double S2, ref double Orb2,
            ref int Ym3, ref double S3, ref double Res);

        /// <summary>
        /// 把计算结果写入结果文件
        /// </summary>
        /// <param name="oSTWResult"></param>
        /// <param name="oUNWResult"></param>
        /// <param name="YM3"></param>
        /// <param name="S3"></param>
        /// <param name="Orb3"></param>
        /// <returns></returns>
        private bool WriteToResult(StreamWriter oSTWResult, StreamWriter oUNWResult, int[] YM3, double S3, double[,] Orb3)
        {
            bool blResult = true;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbUNW = new StringBuilder();
            string sTmp = string.Empty;
            sb.Append("  ");
            sb.Append(YM3[0].ToString());

            sb.Append(" ");
            sb.Append(YM3[1].ToString().PadLeft(2, '0'));

            sb.Append(" ");
            sb.Append(YM3[2].ToString().PadLeft(2, '0'));

            sb.Append(" ");
            sb.Append(YM3[3].ToString().PadLeft(2, '0'));

            sb.Append(":");
            sb.Append(YM3[4].ToString().PadLeft(2, '0'));

            sb.Append(":");
            sb.Append(S3.ToString("00.0"));

            sTmp = sb.ToString();//年月日时分秒
            sb.Clear();

            string strFormat = string.Empty;
            int iLen = 0;
            for (int i = 0; i < 11; i++)
            {
                if (i <= 3)
                {
                    strFormat = "f2";
                    iLen = 12;
                }
                else if ((i > 3 && i <= 5) || i > 8)
                {
                    strFormat = "f3";
                    iLen = 7;
                }
                else if (i > 5 && i <= 8)
                {
                    strFormat = "f4";
                    iLen = 10;
                }

                sb.Append("  ");
                sb.Append(Orb3[0, i].ToString(strFormat).PadLeft(iLen, ' '));

                sbUNW.Append("  ");
                sbUNW.Append(Orb3[1, i].ToString(strFormat).PadLeft(iLen, ' '));
            }
            oSTWResult.WriteLine(sTmp + sb.ToString());
            oUNWResult.WriteLine(sTmp + sbUNW.ToString());

            return blResult;
        }

        /// <summary>
        /// 把计算参数写入文件，For Test
        /// </summary>
        /// <param name="oSTWResult"></param>
        /// <param name="YM1"></param>
        /// <param name="S1"></param>
        /// <param name="Orb1"></param>
        /// <param name="YM2"></param>
        /// <param name="S2"></param>
        /// <param name="Orb2"></param>
        /// <returns></returns>
        private bool WriteToResult(StreamWriter oSTWResult, int[] YM1, double S1, double[] Orb1, int[] YM2, double S2, double[] Orb2)
        {
            bool blResult = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("  ");
            sb.Append(YM1[0].ToString());

            sb.Append("  ");
            sb.Append(YM1[1].ToString().PadLeft(2, ' '));

            sb.Append("  ");
            sb.Append(YM1[2].ToString().PadLeft(2, ' '));

            sb.Append("  ");
            sb.Append(YM1[3].ToString().PadLeft(2, ' '));

            sb.Append("  ");
            sb.Append(YM1[4].ToString().PadLeft(2, ' '));

            sb.Append("  ");
            sb.Append(S1.ToString("00.000000"));

            sb.Append("  ");
            sb.Append(Orb1[0].ToString("0000000.000000"));

            sb.Append("  ");
            sb.Append(Orb1[1].ToString("0000000.000000"));

            sb.Append("  ");
            sb.Append(Orb1[2].ToString("0000000.000000"));

            sb.Append("  ");
            sb.Append(Orb1[3].ToString("0000000.000000"));

            sb.Append("  ");
            sb.Append(Orb1[4].ToString("0000000.000000"));

            sb.Append("  ");
            sb.Append(Orb1[5].ToString("0000000.000000"));

            oSTWResult.WriteLine(sb.ToString());
            sb.Clear();

            sb.Append("  ");
            sb.Append(YM2[0].ToString());

            sb.Append("  ");
            sb.Append(YM2[1].ToString().PadLeft(2, ' '));

            sb.Append("  ");
            sb.Append(YM2[2].ToString().PadLeft(2, ' '));

            sb.Append("  ");
            sb.Append(YM2[3].ToString().PadLeft(2, ' '));

            sb.Append("  ");
            sb.Append(YM2[4].ToString().PadLeft(2, ' '));

            sb.Append("  ");
            sb.Append(S2.ToString("00.000000"));

            sb.Append("  ");
            sb.Append(Orb2[0].ToString("0000000.000000"));

            sb.Append("  ");
            sb.Append(Orb2[1].ToString("0000000.000000"));

            sb.Append("  ");
            sb.Append(Orb2[2].ToString("0000000.000000"));

            sb.Append("  ");
            sb.Append(Orb2[3].ToString("0000000.000000"));

            sb.Append("  ");
            sb.Append(Orb2[4].ToString("0000000.000000"));

            sb.Append("  ");
            sb.Append(Orb2[5].ToString("0000000.000000"));

            oSTWResult.WriteLine(sb.ToString());
            oSTWResult.WriteLine();
            return blResult;
        }
    }
}

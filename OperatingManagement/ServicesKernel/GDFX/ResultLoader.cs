using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServicesKernel.GDFX
{
    public class ResultLoader
    {
        private Dictionary<string, string> resultFileNames;

        private Dictionary<string, string> FileNames
        {
            get 
            {
                if (resultFileNames == null)
                    LoadFileNames();
                return resultFileNames;
            }
        }

        /// <summary>
        /// 载入轨道计算结果文件指定数据
        /// </summary>
        /// <param name="filePath">结果文件路径,FullName</param>
        /// <param name="resultType">结果类型</param>
        /// <param name="dataType">数据列名</param>
        /// <param name="parseDate">是否解析日期</param>
        /// <param name="dates">日期</param>
        /// <param name="dblDatas">double型数据列</param>
        /// <param name="intDatas">int型数据列</param>
        /// <returns></returns>
        public string LoadResultFile(string filePath, string resultType, string dataType
            , bool parseDate, out List<DateTime> dates, out List<string> datestrs
            , out List<double> dblDatas, out List<int> intDatas
            , out double maxValue, out double minValue, out int totalCount)
        {
            #region Declare variant
            dates = new List<DateTime>();
            datestrs = new List<string>();
            dblDatas = new List<double>();
            intDatas = new List<int>();
            maxValue = double.MinValue;
            minValue = double.MaxValue;
            totalCount = 0;

            string strResult = string.Empty;
            bool blResult;
            string strLine = string.Empty;
            int iLine = 1;
            int iTick = 1;
            double dbTmp = 0;
            //int iTmp = 0;
            string[] strDatas;
            DateTime date = DateTime.MinValue;
            ResultType oRType = FormatXMLConfig.GetTypeByName(resultType);
            bool isGDYB = false;
            #endregion

            #region Load Config Info
            if (oRType == null)
            {
                strResult = string.Format("读取轨道计算配置信息{0}失败", resultType);
                return strResult;
            }

            ResultData oRData = oRType.GetDataByName(dataType);
            if (oRData == null)
            {
                strResult = string.Format("读取轨道计算配置信息Data of {0}失败", dataType);
                return strResult;
            }
            #endregion

            #region Open file & read n Line
            strResult = string.Format("读取{0}文件【{1}】出错，", resultType, filePath);
            StreamReader oReader = new StreamReader(filePath);
            oReader.BaseStream.Seek(0, SeekOrigin.Begin);
            switch(resultType.ToLower().Substring(0, 4))
            {
                case "cutp"://交会预报
                    oReader.ReadLine();
                    oReader.ReadLine();
                    strLine = oReader.ReadLine();
                    break;
                case "cuta"://交会分析
                    strLine = oReader.ReadLine();
                    break;
                case "gdyb"://轨道预报
                    oReader.ReadLine();
                    strLine = oReader.ReadLine();
                    if (resultType.ToLower().Substring(0, 8) == "gdyb_map")
                        isGDYB = true;
                    break;
            }
            #endregion


            strLine = strLine.TrimStart();
            while (!string.IsNullOrEmpty(strLine))
            {
                if (strLine.IndexOf(": ") > 0)
                    strLine = strLine.Replace(": ", ":0");//有的数据时分秒位会有dd:dd: d.ddd的情况，应为dd:dd:dd.ddd
                if ((oRType.IsBigFile && iTick == 1) || !oRType.IsBigFile)
                {
                    strDatas = DataValidator.SplitRowDatas(strLine);
                    if (parseDate)
                    {
                        if (isGDYB)
                        {
                            try
                            {
                                strResult = strLine.Substring(0, 10) + " " + strLine.Substring(11, 10).Replace("  ", " 0");
                                date = DateTime.ParseExact(strResult, "yyyy MM dd HH mm ss.f", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch (Exception ex)
                            {
                                strResult += string.Format("行{0}日期格式错误", iLine);
                                oReader.Close();
                                return strResult;
                            }
                        }
                        else
                        {
                            try
                            {
                                blResult = DataValidator.ValidateDateColon(strDatas, oRType.TimeBeginPoint, out date);
                            }
                            catch (Exception ex)
                            {
                                blResult = false;
                            }
                            if (!blResult)
                            {
                                strResult += string.Format("行{0}日期格式错误", iLine);
                                oReader.Close();
                                return strResult;
                            }
                        }
                        dates.Add(date);
                        datestrs.Add(date.ToString("yyyy/MM/dd hh:mm:ss.fff"));
                    }

                    //if (oRData.Type == DataType.doubletype)
                        blResult = DataValidator.ValidateFloat(strDatas[oRData.Position], oRData.IntLen + oRData.DecLen, oRData.DecLen, out dbTmp);
                    //else
                    //    blResult = DataValidator.ValidateInt(strDatas[oRData.Position], oRData.IntLen, out iTmp);
                    if (!blResult)
                    {
                        strResult += string.Format("行{0}数据格式错误", iLine);
                        oReader.Close();
                        return strResult;
                    }

                    //if (oRData.Type == DataType.doubletype)
                        dblDatas.Add(dbTmp);
                    //else
                    //    intDatas.Add(iTmp);
                    if (dbTmp < minValue)
                        minValue = dbTmp;
                    if (dbTmp > maxValue)
                        maxValue = dbTmp;
                }
                strLine = oReader.ReadLine();
                if (strLine != null)
                    strLine = strLine.TrimStart();
                iLine++;
                iTick++;
                if (oRType.IsBigFile && iTick == 301)//对于大文件，每n分钟显示一个点
                    iTick = 1;
            }
            oReader.Close();
            totalCount = iLine;
            return strResult;
        }

        /// <summary>
        /// 载入文件名集合
        /// </summary>
        private void LoadFileNames()
        {
            if (resultFileNames == null)
            {
                resultFileNames = new Dictionary<string,string>();
                //交会预报，STW结果
                resultFileNames.Add("CutPre_STW", "CutSTW.dat");
                //交会预报，UNW结果
                resultFileNames.Add("CutPre_UNW", "CutUNW.dat");
                //交会分析，STW结果
                resultFileNames.Add("CutAna_STW", "CutAnalyze_STW.dat");
                //交会分析，UNW结果
                resultFileNames.Add("CutAna_UNW", "CutAnalyze_UNW.dat");
                //轨道预报，空间位置预报，J2000系
                resultFileNames.Add("GDYB_MapJ", "MapJ.dat");
                //轨道预报，空间位置预报，WGS-84坐标系
                resultFileNames.Add("GDYB_MapW", "MapW.dat");
                //轨道预报，星下点预报
                resultFileNames.Add("GDYB_SubSatPoint", "SubSatPoint.dat");
                //轨道预报，测站观测量
                resultFileNames.Add("GDYB_StaObsPre", "StaObsPre.dat");
                //轨道预报，观测引导文件
                resultFileNames.Add("GDYB_ObsGuiding", "ObsGuiding.dat");
                //轨道预报，太阳角度
                resultFileNames.Add("GDYB_SunAH", "SunAH.dat");
                //轨道预报，测站可见性统计文件
                resultFileNames.Add("GDYB_VisibleStatistics", "VisibleStatistics.dat");
                //轨道预报，进出站及航捷数据统计文件
                resultFileNames.Add("GDYB_StationInOut", "StationInOut.dat");
                //轨道预报，日凌历元文件
                resultFileNames.Add("GDYB_SunTransit", "SunTransit.dat");
                //轨道预报，月凌历元文件
                resultFileNames.Add("GDYB_MoonTransit", "MoonTransit.dat");
                //轨道预报，地影历元文件
                resultFileNames.Add("GDYB_EarthShadow", "EarthShadow.dat");  
            }
        }
    }
    public enum ResultDataType
    {
        /// <summary>
        /// CutPre、CutAna
        /// </summary>
        dR,
        /// <summary>
        /// CutPre、CutAna、GDYB_StaObsPre、GDYB_ObsGuiding、GDYB_SunAH
        /// </summary>
        A,
        /// <summary>
        /// CutPre、CutAna、GDYB_StaObsPre、GDYB_ObsGuiding、GDYB_SunAH
        /// </summary>
        E,
        /// <summary>
        /// CutPre、CutAna
        /// </summary>
        ddR,
        /// <summary>
        /// CutPre、CutAna
        /// </summary>
        dA,
        /// <summary>
        /// CutPre、CutAna
        /// </summary>
        dE,
        /// <summary>
        /// CutPre、CutAna
        /// </summary>
        ANG,
        /// <summary>
        /// GDYB：MapJ、MapW
        /// </summary>
        X,
        /// <summary>
        /// GDYB：MapJ、MapW
        /// </summary>
        Y,
        /// <summary>
        /// GDYB：MapJ、MapW
        /// </summary>
        Z,
        /// <summary>
        /// GDYB：MapJ、MapW
        /// </summary>
        vX,
        /// <summary>
        /// GDYB：MapJ、MapW
        /// </summary>
        vY,
        /// <summary>
        /// GDYB：MapJ、MapW
        /// </summary>
        vZ,
        /// <summary>
        /// GDYB：SubSatPoint
        /// </summary>
        L,
        /// <summary>
        /// GDYB：SubSatPoint
        /// </summary>
        B,
        /// <summary>
        /// GDYB：SubSatPoint
        /// </summary>
        H,
        /// <summary>
        /// GDYB_StaObsPre
        /// </summary>
        R,
        /// <summary>
        /// GDYB_StaObsPre
        /// </summary>
        RR,
        /// <summary>
        /// GDYB_StaObsPre、GDYB_ObsGuiding
        /// </summary>
        AA,
        /// <summary>
        /// GDYB_StaObsPre、GDYB_ObsGuiding
        /// </summary>
        EE,
        /// <summary>
        /// GDYB_StaObsPre
        /// </summary>
        Th,
        /// <summary>
        /// GDYB_StaObsPre
        /// </summary>
        TTh,
        /// <summary>
        /// GDYB_StaObsPre
        /// </summary>
        K,
        /// <summary>
        /// GDYB_StaObsPre
        /// </summary>
        M
    }
}

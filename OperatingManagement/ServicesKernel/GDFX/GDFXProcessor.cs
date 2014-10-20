using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileServer.Base;
using log4net;

namespace ServicesKernel.GDFX
{
    /// <summary>
    /// 轨道分析逻辑处理
    /// </summary>
    public class GDFXProcessor
    {
        /// <summary>
        /// 交会分析
        /// </summary>
        /// <param name="subFileFullName"></param>
        /// <param name="tgtFileFullName"></param>
        /// <param name="resultFileName">结果文件名的一部分（未加_STW.dat和_UNW.dat）</param>
        /// <returns></returns>
        public string CutAnalyze(string subFileFullName, string tgtFileFullName, out string resultFileName)
        {
            /*
             * 1、检查依赖的文件是否都存在
             * 2、检查主星文件和目标星文件数据是否合法
             * 3、开始计算
             */

            resultFileName = string.Empty;
            string strResult = string.Empty;
            #region 检查数据文件存在及合法性
            try
            {
                strResult = CutAnalyzer.Instance.IsAllFileExist();
            }
            catch (Exception ex)
            {
                strResult = "检查参数文件是否都存在出现异常";
                Logger.GetLogger().Error("CutAnalyze:" + strResult, ex);
            }
            finally { }
            if (!strResult.Equals(string.Empty))
                return strResult;

            DateTime subMinDate;
            DateTime subMaxDate;
            int subInterval;
            try
            {
                strResult = CutAnalyzer.Instance.CheckFileData(subFileFullName, out subMinDate, out subMaxDate, out subInterval);
            }
            catch (Exception ex)
            {
                strResult = "检查主星文件出现异常";
                Logger.GetLogger().Error("CutAnalyze:" + strResult, ex);
                return strResult;
            }
            finally { }
            if (!strResult.Equals(string.Empty))
            {
                strResult = string.Format("主星文件数据不合法{0}", strResult);
                return strResult;
            }

            DateTime tgtMinDate;
            DateTime tgtMaxDate;
            int tgtInterval;
            try
            {
                strResult = CutAnalyzer.Instance.CheckFileData(tgtFileFullName, out tgtMinDate, out tgtMaxDate, out tgtInterval);
            }
            catch (Exception ex)
            {
                strResult = "检查目标星文件出现异常";
                Logger.GetLogger().Error("CutAnalyze:" + strResult, ex);
                return strResult;
            }
            finally { }
            if (!strResult.Equals(string.Empty))
            {
                strResult = string.Format("目标星文件数据不合法{0}", strResult);
                return strResult;
            }
            #endregion

            //检查时间是否有交集
            if (subMinDate > tgtMaxDate || tgtMinDate > subMaxDate)
            {
                strResult = "两个数据文件时间没有交集";
                return strResult;
            }

            #region 比较间隔、开始时间、结束时间
            DateTime beginDate;
            DateTime endDate;
            int maxInterval;
            int minInterval;
            bool IsSubInterval = false;
            bool IsSubBeginDate = false;
            bool IsSubEndDate = false;

            if (subInterval >= tgtInterval)
            {
                IsSubInterval = true;
                maxInterval = subInterval;
                minInterval = tgtInterval;
            }
            else
            {
                maxInterval = tgtInterval;
                minInterval = subInterval;
            }

            if (subMinDate >= tgtMinDate)
            {
                IsSubBeginDate = true;
                beginDate = subMinDate;
            }
            else
                beginDate = tgtMinDate;

            if (subMaxDate >= tgtMaxDate)
                endDate = tgtMaxDate;
            else
            {
                IsSubEndDate = true;
                endDate = subMaxDate;
            }
            #endregion

            try
            {
                strResult = CutAnalyzer.Instance.DoCaculate(subFileFullName, tgtFileFullName, beginDate, endDate, maxInterval, minInterval
                    , IsSubInterval, IsSubBeginDate, IsSubEndDate, out resultFileName);
            }
            catch (Exception ex)
            {
                strResult = "进行交会分析计算中出现异常";
                CutAnalyzer.Instance.isCaculating = false;
                Logger.GetLogger().Error("CutAnalyze:" + strResult, ex);
                return strResult;
            }
            finally { }

            return strResult;
        }

        /// <summary>
        /// 参数转换
        /// </summary>
        /// <param name="angle">是为度，否为弧度</param>
        /// <param name="length">是为千米，否为米</param>
        /// <param name="timezone">时区，正负12之间，整数</param>
        /// <param name="convertType"></param>
        /// <param name="convertFilePath">路径+文件名</param>
        /// <param name="emitFilePath">路径+文件名</param>
        /// <returns></returns>
        public string ParamConvert(bool deg, bool km, int timezone, string convertType, string convertFileFullName, string emitFileFullName
            , out string resultFileName)
        {
            /*
             * 1、校验发射系文件，时间合法
             * 2、校验待转换文件，时间合法，数据合法
             * 3、进行转换
             */
            resultFileName = string.Empty;
            string strResult = string.Empty;
            if (convertType.IndexOf('8') >= 0 || convertType.IndexOf('9') >= 0)
            {
                if (!emitFileFullName.Equals(string.Empty))
                {
                    try
                    {
                        strResult = ParamConvertor.Instance.CheckEmitFileData(emitFileFullName);
                    }
                    catch (Exception ex)
                    {
                        strResult = "检查发射系文件出现异常";
                        Logger.GetLogger().Error("ParamConvert:" + strResult, ex);
                    }
                    finally { }
                    if (!strResult.Equals(string.Empty))
                        return strResult;
                }
                else
                    return "发射系文件不能为空";
            }

            try
            {
                strResult = ParamConvertor.Instance.CheckConvertFileData(convertFileFullName);
            }
            catch (Exception ex)
            {
                strResult = "检查待转换文件出现异常";
                ParamConvertor.Instance.isCaculating = false;
                Logger.GetLogger().Error("ParamConvert:" + strResult, ex);
            }
            finally { }
            if (!strResult.Equals(string.Empty))
                return strResult;

            try
            {
                strResult = ParamConvertor.Instance.DoConvert(deg, km, timezone, convertType, convertFileFullName, emitFileFullName, out resultFileName);
            }
            catch (Exception ex)
            {
                strResult = "进行参数转换中出现异常";
                Logger.GetLogger().Error("ParamConvert:" + strResult, ex);
            }
            finally { }
            return strResult;
        }

        /// <summary>
        /// 差值分析，俩文件必须在同一路径中
        /// </summary>
        /// <param name="ephemerisFileFullName">路径+文件名</param>
        /// <param name="timeSeriesFileFullName">路径+文件名</param>
        /// <param name="resultFileFullName">路径+文件名</param>
        /// <returns></returns>
        public string Intepolate(string ephemerisFileFullName, string timeSeriesFileFullName, out string resultFileFullName)
        {
            /*
             * 1、校验差值分析时间文件，时间合法，数据合法
             * 2、校验星历文件，时间合法
             * 3、进行差值分析
             */
            resultFileFullName = string.Empty;
            string strResult = string.Empty;
            DateTime beginDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;
            try
            {
                strResult = Intepolater.Instance.CheckTimeSeriesFileData(timeSeriesFileFullName, out beginDate, out endDate);
            }
            catch (Exception ex)
            {
                strResult = "检查时间序列文件出现异常";
                Logger.GetLogger().Error("Intepolate:" + strResult, ex);
            }
            finally { }
            if (!strResult.Equals(string.Empty))
                return strResult;

            try
            {
                strResult = Intepolater.Instance.CheckEphemerisFileData(ephemerisFileFullName, beginDate, endDate);
            }
            catch (Exception ex)
            {
                strResult = "检查星历文件出现异常";
                Logger.GetLogger().Error("Intepolate:" + strResult, ex);
            }
            finally { }
            if (!strResult.Equals(string.Empty))
                return strResult;

            try
            {
                strResult = Intepolater.Instance.DoCaculate(ephemerisFileFullName, timeSeriesFileFullName, out resultFileFullName);
            }
            catch (Exception ex)
            {
                strResult = "进行参数转换中出现异常";
                Intepolater.Instance.isCaculating = false;
                Logger.GetLogger().Error("Intepolate:" + strResult, ex);
            }
            finally { }
            return strResult;
        }

        /// <summary>
        /// 交会预报，只给路径，文件名固定
        /// </summary>
        /// <param name="filesPath">交会预报的文件路径</param>
        /// <returns></returns>
        public string CutPre(string filesPath)
        {
            /*
             * 1、检查需要的文件是否都有
             * 2、校验主文件数据合法性
             * 3、校验Sub文件数据合法性
             * 4、校验Optional文件数据合法性
             * 5、进行交会预报计算
             */
            string strResult = string.Empty;
            //try
            //{
            //    strResult = CutPrer.Instance.IsAllFileExist();
            //}
            //catch (Exception ex)
            //{
            //    strResult = "检查参数文件是否都存在时出现异常";
            //    Logger.GetLogger().Error("CutPre:" + strResult, ex);
            //}
            //finally { }
            if (!strResult.Equals(string.Empty))
                return strResult;

            try
            {
                strResult = CutPrer.Instance.CheckMainFileData(filesPath + "CutMain.dat");
            }
            catch (Exception ex)
            {
                strResult = "检查CutMain文件出现异常";
                Logger.GetLogger().Error("CutPre:" + strResult, ex);
            }
            finally { }
            if (!strResult.Equals(string.Empty))
                return strResult;

            try
            {
                strResult = CutPrer.Instance.CheckSubFileData(filesPath + "CutSub.dat");
            }
            catch (Exception ex)
            {
                strResult = "检查CutSub文件出现异常";
                Logger.GetLogger().Error("CutPre:" + strResult, ex);
            }
            finally { }
            if (!strResult.Equals(string.Empty))
                return strResult;

            try
            {
                strResult = CutPrer.Instance.CheckOptionalFileData(filesPath + "CutOptional.dat");
            }
            catch (Exception ex)
            {
                strResult = "检查CutOptional文件出现异常";
                Logger.GetLogger().Error("CutPre:" + strResult, ex);
            }
            finally { }
            if (!strResult.Equals(string.Empty))
                return strResult;

            try
            {
                strResult = CutPrer.Instance.DoCaculate(filesPath);
            }
            catch (Exception ex)
            {
                strResult = "进行交会预报计算中异常";
                CutPrer.Instance.isCaculating = false;
                Logger.GetLogger().Error("CutPre:" + strResult, ex);
            }
            finally { }
            return strResult;
        }
    }

    public class Logger
    {
        //private static string loggerName = "OMServer.Logging";
        //private static ILog mLogger = null;
        public static ILog GetLogger()
        {
            return LogManager.GetLogger("ExceptionLogger");
        }
    }
}

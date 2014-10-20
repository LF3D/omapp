using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ServicesKernel.GDFX
{
    /// <summary>
    /// 用来校验数据
    /// </summary>
    public class DataValidator
    {
        /// <summary>
        /// 验证字符串是否为日期格式， yyyy MM dd HH mm SS.ssss
        /// </summary>
        /// <param name="data"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool ValidateDate(string data, out DateTime date)
        {
            return DateTime.TryParseExact(data, "yyyyMMdd HHmmss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }

        /// <summary>
        /// 验证数组指定位置段是否为日期格式， yyyy MM dd HH mm SS.ssss
        /// </summary>
        /// <param name="data"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool ValidateDate(string[] datas, int startIdx, out DateTime date)
        {
            string strTmp = GetArrayTimeString(datas, startIdx);
            return DateTime.TryParseExact(strTmp, "yyyyMMdd HHmmss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }

        /// <summary>
        /// 验证数组指定位置段是否为日期格式， yyyy MM dd HH:mm:SS.ssss
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="startIdx"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool ValidateDateColon(string[] datas, int startIdx, out DateTime date)
        {
            string strTmp = GetColonArrayTimeString(datas, startIdx);
            return DateTime.TryParseExact(strTmp, "yyyyMMdd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }

        /// <summary>
        /// 验证是否为正确的浮点型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalLength"></param>
        /// <param name="decimalLength"></param>
        /// <returns></returns>
        public static bool ValidateFloat(string data, int maxLength, int decimalLength)
        {
            bool blResult = true;
            int iIdx = data.IndexOf('.');
            data = data.TrimEnd(new char[] { '0' });
            if (iIdx > 0)
            {
                if (data.Length - iIdx - 1 > decimalLength)//小数位置超限
                    blResult = false;
                else
                {
                    if (data.Length - 1 > maxLength)//总长度超限
                        blResult = false;
                }
            }
            else
            {
                if (data.Length > (maxLength - decimalLength))//没有小数位，总长度超限
                    blResult = false;
            }

            if (blResult)
            {
                double dbResult;
                return double.TryParse(data, out dbResult);
            }
            else
                return blResult;
        }


        /// <summary>
        /// 验证是否为正确的浮点型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalLength"></param>
        /// <param name="decimalLength"></param>
        /// <returns></returns>
        public static bool ValidateFloat(string data, int maxLength, int decimalLength, out double result)
        {
            result = 0;
            bool blResult = true;
            int iIdx = data.IndexOf('.');
            if (iIdx > 0)
            {
                if (data.Length - iIdx - 1 > decimalLength)//小数位置超限
                    blResult = false;
                else
                {
                    if (data.Length - 1 > maxLength)//总长度超限
                        blResult = false;
                }
            }
            else
            {
                if (data.Length > maxLength - decimalLength)//没有小数位，总长度超限
                    blResult = false;
            }

            if (blResult)
            {
                return double.TryParse(data, out result);
            }
            else
                return blResult;
        }

        /// <summary>
        /// 从行数据获取分割后的结果(空格分隔数据)
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        public static string[] SplitRowDatas(string rowData)
        {
            string[] datas;
            int iIdx = 0;
            rowData = rowData.Trim();
            iIdx = rowData.IndexOf("  ");
            while (iIdx >= 0)
            {
                //提供的样本数据空格数不一致，干脆把多余的空格换成一个
                rowData = rowData.Replace("  ", " ");
                iIdx = rowData.IndexOf("  ");
            }
            datas = rowData.Split(new char[] { ' ' });
            return datas;
        }

        /// <summary>
        /// 验证是否为正确格式的整型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool ValidateInt(string data, int maxLength)
        {
            if (maxLength != 0)
            {
                if (data.Length > maxLength)
                    return false;
            }
            int iResult;
            return int.TryParse(data, out iResult);
        }

        /// <summary>
        /// 验证是否为正确格式的整型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool ValidateInt(string data, int maxLength, out int result)
        {
            result = 0;
            if (maxLength != 0)
            {
                if (data.Length > maxLength)
                    return false;
            }
            return int.TryParse(data, out result);
        }

        /// <summary>
        /// 获取路径的ascii int数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int[] GetIntPath(string path)
        {
            int[] iPath = new int[path.Length];
            byte[] bPath;
            ASCIIEncoding ascii = new ASCIIEncoding();
            bPath = ascii.GetBytes(path);
            for (int i = 0; i < iPath.Length; i++)
            {
                iPath[i] = (int)bPath[i];
            }
            return iPath;
        }

        /// <summary>
        /// 从数组中取时间字符串
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="iIdx">日期的起始位置</param>
        /// <returns></returns>
        public static string GetArrayTimeString(string[] datas, int iIdx)
        {
            int iTmp = 0;
            int iLast = iIdx + 5;
            iTmp = datas[iLast].IndexOf('.');
            //目地是把秒变成xx.xxx格式，时间解析只认这个
            string sLeft;
            string sRight;
            if (iTmp >= 0)
            {
                if (datas[iLast].Substring(0, iTmp) != "")
                    sLeft = int.Parse(datas[iLast].Substring(0, iTmp)).ToString("00");
                else
                    sLeft = "00";
                if (datas[iLast].Substring(iTmp + 1) != "")
                    sRight = int.Parse(datas[iLast].Substring(iTmp + 1)).ToString("000");
                else
                    sRight = "000";
                datas[iLast] = sLeft + "." + sRight;
            }
            else
                datas[iLast] = int.Parse(datas[iLast]).ToString("00") + ".000";

            return string.Format("{0}{1}{2} {3}{4}{5}", datas[iIdx], datas[iIdx + 1].PadLeft(2, '0'), datas[iIdx + 2].PadLeft(2, '0')
                    , datas[iIdx + 3].PadLeft(2, '0'), datas[iIdx + 4].PadLeft(2, '0'), datas[iLast]);
        }

        /// <summary>
        /// 从数组中取时间字符串，带冒号的
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="iIdx">日期的起始位置</param>
        /// <returns></returns>
        public static string GetColonArrayTimeString(string[] datas, int iIdx)
        {
            int iTmp = 0;
            int iLast = iIdx + 3;
            iTmp = datas[iLast].IndexOf('.');
            //目地是把秒变成xx.xxx格式，时间解析只认这个
            string sLeft;
            string sRight;
            if (iTmp >= 0)
            {
                sLeft = datas[iLast].Substring(0, iTmp);
                sRight = datas[iLast].Substring(iTmp + 1).TrimEnd(new char[] { '0' }).PadRight(3, '0');
                datas[iLast] = sLeft + "." + sRight;
            }
            else
                datas[iLast] = datas[iLast] + ".000";

            return string.Format("{0}{1}{2} {3}", datas[iIdx], datas[iIdx + 1].PadLeft(2, '0'), datas[iIdx + 2].PadLeft(2, '0')
                    , datas[iLast]);
        }
    }
}

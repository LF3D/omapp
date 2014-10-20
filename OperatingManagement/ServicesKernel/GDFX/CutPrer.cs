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
    /// 轨道分析-交会预报
    /// </summary>
    public class CutPrer
    {
        private string[] fileNames = new string[] { "JPLEPH", "TESTRECL", "WGS84.GEO", "eopc_IAU2000.txt" };
        private const string dllPath = @"E:\YKZX_Manage\";
        private const string dllFolder = @"GDDLL\CutPre\";
        private const string dllName = @"CutPreDLL.dll";
        //private const string outputPath = @"output\";
        private string strDllPath;
        //
        //private IntPtr handle;
        private static CutPrer _instance = null;
        private static object _locker = new object();
        public bool isCaculating = false;

        /// <summary>
        /// 使用唯一实例是为了控制同一时间只能有一个计算在进行
        /// </summary>
        public static CutPrer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new CutPrer();
                        }
                    }
                }
                return _instance;
            }
        }

        public CutPrer()
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
        /// 判断所有文件是否都存在
        /// </summary>
        /// <returns></returns>
        public string IsAllFileExist()
        {
            string strResult = string.Empty;
            for (int i = 0; i < fileNames.Length; i++)
            {
                if (!DataFileHandle.Exists(DllPath + @"\Include\" + fileNames[i]))
                {
                    strResult = string.Format("文件{0}不存在", fileNames[i]);
                    break;
                }
            }
            return strResult;
        }

        /// <summary>
        /// 检查主文件数据合法性
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CheckMainFileData(string fileName)
        {
            bool blResult = false;
            int iLine = 1;
            string strLine = string.Empty;
            int iIdx = 0;
            string[] datas = new string[0];
            string sTmp = string.Empty;
            string msg = "主文件数据行{0}格式不合法";
            DateTime date;
            StreamReader oSReader = new StreamReader(fileName);

            oSReader.BaseStream.Seek(0, SeekOrigin.Begin);
            strLine = oSReader.ReadLine();

            #region 检查第一行，后面与第一行格式不一致
            if (strLine != null)
            {
                //Ys  Ms  Ds  Hs  Mins  SS.Ss  Yf  Mf  Df  Hf  Minf  SS.Sf
                datas = DataValidator.SplitRowDatas(strLine);
                if (datas.Length != 12)
                {
                    oSReader.Close();
                    return string.Format(msg, iLine);
                }

                iIdx = 0;
                sTmp = DataValidator.GetArrayTimeString(datas, iIdx);
                blResult = DataValidator.ValidateDate(sTmp, out date);
                if (!blResult)
                {
                    oSReader.Close();
                    return string.Format(msg + "【起始时间不合法】", iLine);
                }

                iIdx = 6;
                sTmp = DataValidator.GetArrayTimeString(datas, iIdx);
                blResult = DataValidator.ValidateDate(sTmp, out date);
                if (!blResult)
                {
                    oSReader.Close();
                    return string.Format(msg + "【终止时间不合法】", iLine);
                }
            }
            else
            {
                oSReader.Close();
                return string.Format(msg + "【空文件】", iLine);
            }
            #endregion

            //检查第二行
            strLine = oSReader.ReadLine();
            iLine++;
            blResult = CheckMain2ndRowData(strLine);
            if (!blResult)
            {
                oSReader.Close();
                return string.Format(msg, iLine);
            }

            //检查第三行
            strLine = oSReader.ReadLine();
            iLine++;
            blResult = CheckMain3rdRowData(strLine);
            if (!blResult)
            {
                oSReader.Close();
                return string.Format(msg, iLine);
            }

            //检查第四行及之后
            strLine = oSReader.ReadLine();
            iLine++;
            while (strLine != null)
            {
                blResult = CheckMain4thRowData(strLine);
                if (!blResult)
                {
                    oSReader.Close();
                    return string.Format(msg, iLine);
                }
                iLine++;
                strLine = oSReader.ReadLine();
            }
            oSReader.Close();
            return string.Empty;
        }

        /// <summary>
        /// 检查主数据第二行数据的合法性
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        private bool CheckMain2ndRowData(string rowData)
        {
            if (rowData == null || rowData.Trim().Equals(string.Empty))
                return false;

            rowData = rowData.Trim();
            string sTmp = string.Empty;
            int iIdx = 0;
            string[] datas = new string[0];
            bool blResult = false;
            DateTime date;
            datas = DataValidator.SplitRowDatas(rowData);
            if (datas.Length != 17)
                return blResult;
            iIdx = 2;

            //Name	NO	  YYYY  MM  DD  HH  MM  SS.SSSSSS	KK 	D1  D2  D3  D4  D5  D6	 Sm	Ref
            sTmp = DataValidator.GetArrayTimeString(datas, iIdx);
            blResult = DataValidator.ValidateDate(sTmp, out date);
            if (!blResult)
                return blResult;

            iIdx += 6;
            blResult = !(datas[iIdx] != "1" && datas[iIdx] != "2" && datas[iIdx] != "3");
            if (!blResult)
                return blResult;

            iIdx += 1;
            for (int i = iIdx; i < datas.Length; i++)
            {
                blResult = DataValidator.ValidateFloat(datas[i], 16, 6);
                if (!blResult)
                    break;
            }

            return blResult;
        }

        /// <summary>
        /// 检查主数据第三行数据的合法性
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        private bool CheckMain3rdRowData(string rowData)
        {
            if (rowData == null || rowData.Trim().Equals(string.Empty))
                return false;

            rowData = rowData.Trim();
            string sTmp = string.Empty;
            int iIdx = 0;
            string[] datas = new string[0];
            bool blResult = false;
            datas = DataValidator.SplitRowDatas(rowData);//NN	 dR 	KAE	 dA		dE
            if (datas.Length != 5)
                return blResult;

            iIdx = 0;
            blResult = DataValidator.ValidateInt(datas[iIdx], 3);
            if (!blResult)
                return blResult;

            iIdx++;
            blResult = DataValidator.ValidateFloat(datas[iIdx], 16, 6);
            if (!blResult)
                return blResult;

            iIdx++;
            blResult = DataValidator.ValidateInt(datas[iIdx], 2);
            if (!blResult)
                return blResult;

            iIdx++;
            blResult = DataValidator.ValidateFloat(datas[iIdx], 16, 6);
            if (!blResult)
                return blResult;

            iIdx++;
            blResult = DataValidator.ValidateFloat(datas[iIdx], 16, 6);
            return blResult;
        }

        /// <summary>
        /// 检查主文件第四行及之后行数据的合法性
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        private bool CheckMain4thRowData(string rowData)
        {
            if (rowData == null || rowData.Trim().Equals(string.Empty))
                return false;

            rowData = rowData.Trim();
            return (rowData.Length <= 6);//6个字符
        }

        /// <summary>
        /// 检查Sub文件数据合法性
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CheckSubFileData(string fileName)
        {
            bool blResult = false;
            int iLine = 1;
            string strLine = string.Empty;
            StreamReader oSReader = new StreamReader(fileName);

            oSReader.BaseStream.Seek(0, SeekOrigin.Begin);
            strLine = oSReader.ReadLine();
            while (strLine != null)
            {
                blResult = CheckMain2ndRowData(strLine);
                if (!blResult)
                {
                    oSReader.Close();
                    return string.Format("Sub文件数据行{0}格式不合法", iLine);
                }
                iLine++;
                strLine = oSReader.ReadLine();
            }
            oSReader.Close();
            return string.Empty;
        }


        /// <summary>
        /// 检查Optional文件数据合法性
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CheckOptionalFileData(string fileName)
        {
            int iLine = 0;
            string strLine = string.Empty;
            string[] datas = new string[0];
            string msg = "Optional文件数据行{0}不合法";
            int iTmp = 0;
            double dbTmp = 0;
            StreamReader oSReader = new StreamReader(fileName);

            oSReader.BaseStream.Seek(0, SeekOrigin.Begin);
            #region 第一行，预报数据时间间隔，1-30
            iLine++;
            strLine = oSReader.ReadLine();
            datas = GetOptionalFileRowDatas(strLine);
            if (datas != null)
            {
                if (!double.TryParse(datas[1].Trim(), out dbTmp) || (dbTmp > 30 || dbTmp < 1))
                {
                    oSReader.Close();
                    return string.Format(msg + "【double，1-30之间】", iLine);
                }
            }
            else
            {
                oSReader.Close();
                return string.Format(msg + "【不正常结束】", iLine);
            }
            #endregion

            #region 第二行，力模型控制，Pass
            iLine++;
            strLine = oSReader.ReadLine();
            //datas = GetOptionalFileRowDatas(strLine);
            //if (datas == null)
            //{
            //    oSReader.Close();
            //    return string.Format(msg, iLine);
            //}
            #endregion

            #region 第三行，非球形引力阶数，int，>0 & < 50
            iLine++;
            strLine = oSReader.ReadLine();
            datas = GetOptionalFileRowDatas(strLine);
            if (datas != null)
            {
                if (!int.TryParse(datas[1], out iTmp))
                {
                    oSReader.Close();
                    return string.Format(msg + "【应为int】", iLine);
                }

                if (iTmp > 50 || iTmp < 0)
                {
                    oSReader.Close();
                    return string.Format(msg + "【应大于0小于50】", iLine);
                }
            }
            else
            {
                oSReader.Close();
                return string.Format(msg + "【不正常结束】", iLine);
            }
            #endregion

            #region 第四行，非球形引力，int：0，1，2
            iLine++;
            strLine = oSReader.ReadLine();
            datas = GetOptionalFileRowDatas(strLine);
            if (datas != null)
            {
                if (!int.TryParse(datas[1], out iTmp))
                {
                    oSReader.Close();
                    return string.Format(msg + "【应为int】", iLine);
                }
                if (iTmp > 2 || iTmp < 0)
                {
                    oSReader.Close();
                    return string.Format(msg + "【应为0或者1或者2】", iLine);
                }
            }
            else
            {
                oSReader.Close();
                return string.Format(msg + "【不正常结束】", iLine);
            }
            #endregion

            #region 第五行，第三体引力，0 - 3
            iLine++;
            strLine = oSReader.ReadLine();
            datas = GetOptionalFileRowDatas(strLine);
            if (datas != null)
            {
                if (!int.TryParse(datas[1], out iTmp))
                {
                    oSReader.Close();
                    return string.Format(msg + "【应为int】", iLine);
                }
                if (iTmp > 3 || iTmp < 0)
                {
                    oSReader.Close();
                    return string.Format(msg + "【int，应在0-3之间】", iLine);
                }
            }
            else
            {
                oSReader.Close();
                return string.Format(msg + "【不正常结束】", iLine);
            }
            #endregion

            #region 第六行，潮汐摄动，0 - 3
            iLine++;
            strLine = oSReader.ReadLine();
            datas = GetOptionalFileRowDatas(strLine);
            if (datas != null)
            {
                if (!int.TryParse(datas[1], out iTmp))
                {
                    oSReader.Close();
                    return string.Format(msg + "【应为int】", iLine);
                }
                if (iTmp > 3 || iTmp < 0)
                {
                    oSReader.Close();
                    return string.Format(msg + "【int，应在0-3之间】", iLine);
                }
            }
            else
            {
                oSReader.Close();
                return string.Format(msg + "【不正常结束】", iLine);
            }
            #endregion

            #region 第七行，光压摄动，0 or 1
            iLine++;
            strLine = oSReader.ReadLine();
            datas = GetOptionalFileRowDatas(strLine);
            if (datas != null)
            {
                datas[1] = datas[1].Trim();
                if (datas[1] != "0" && datas[1] != "1")
                {
                    oSReader.Close();
                    return string.Format(msg + "【应为0 或 1】", iLine);
                }
            }
            else
            {
                oSReader.Close();
                return string.Format(msg + "【不正常结束】", iLine);
            }
            #endregion

            #region 第八行，大气阻尼摄动，0 or 1
            iLine++;
            strLine = oSReader.ReadLine();
            datas = GetOptionalFileRowDatas(strLine);
            if (datas != null)
            {
                datas[1] = datas[1].Trim();
                if (datas[1] != "0" && datas[1] != "1")
                {
                    oSReader.Close();
                    return string.Format(msg + "【应为0 或 1】", iLine);
                }
            }
            else
            {
                oSReader.Close();
                return string.Format(msg + "【不正常结束】", iLine);
            }
            #endregion

            #region 第九行，后牛顿项，0 or 1
            iLine++;
            strLine = oSReader.ReadLine();
            datas = GetOptionalFileRowDatas(strLine);
            if (datas != null)
            {
                datas[1] = datas[1].Trim();
                if (datas[1] != "0" && datas[1] != "1")
                {
                    oSReader.Close();
                    return string.Format(msg + "【应为0 或 1】", iLine);
                }
            }
            else
            {
                oSReader.Close();
                return string.Format(msg + "【不正常结束】", iLine);
            }
            #endregion

            #region 第十行，相关力模型参数，Pass
            iLine++;
            strLine = oSReader.ReadLine();
            //datas = GetOptionalFileRowDatas(strLine);
            //if (datas == null)
            //{
            //    oSReader.Close();
            //    return string.Format(msg, iLine);
            //}
            #endregion

            oSReader.Close();
            return string.Empty;
        }

        /// <summary>
        /// 获取Optional文件的行数据
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>
        private string[] GetOptionalFileRowDatas(string rowData)
        {
            string[] datas;
            if (rowData == null || rowData.Trim().Equals(string.Empty))
                return null;
            rowData = rowData.Trim();
            datas = rowData.Split(new char[] { ':' });
            datas[1] = datas[1].Trim().Split(new char[] { '!' })[0].Trim();
            return datas;
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="mainFilePath"></param>
        /// <returns></returns>
        public string DoCaculate(string mainFilePath)
        {
            string strResult = string.Empty;
            if (isCaculating)
            {
                strResult = "同一时间只能只能执行单次计算任务，请稍候再计算";
                return strResult;
            }
            isCaculating = true;

            int[] iPath = DataValidator.GetIntPath(mainFilePath);
            if (iPath.Length > 100)
            {
                strResult = "文件路径字符长度不得大于100，请重新设置。";
                isCaculating = false;
                return strResult;
            }
            int iResult = 0;
            CutPre(ref iPath[0], iPath.Length, ref iResult);
            isCaculating = false;
            return strResult;
        }


        /// <summary>
        /// 交会预报
        /// </summary>
        /// <param name="dirIn">文件路径</param>
        /// <param name="Ndir">路径数组长度</param>
        /// <param name="Kjg">计算是否成功，1成功0失败</param>
        [DllImport(dllPath + dllFolder + dllName,
            SetLastError = true, CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.StdCall)]
        public static extern void CutPre(ref int dirIn, int Ndir, ref int Kjg);
    }
}

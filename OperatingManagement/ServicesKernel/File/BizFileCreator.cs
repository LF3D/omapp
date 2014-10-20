using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using OperatingManagement.Framework;
using OperatingManagement.DataAccessLayer.BusinessManage;

namespace ServicesKernel.File
{
    /// <summary>
    /// 业务管理部分文件生成器
    /// </summary>
    public class BizFileCreator
    {
        private static readonly ILog logExp = LogManager.GetLogger("ExceptionLogger");

        #region -For Log
        /// <summary>
        /// Log current error.
        /// </summary>
        private void Log(string msg)
        {
            logExp.Error(msg);
        }
        /// <summary>
        /// Log current error.
        /// </summary>
        private void Log(string msg, Exception ex)
        {
            logExp.Error(msg, ex);
        }
        #endregion

        /// <summary>
        /// 生成天基目标观测试验数据
        /// </summary>
        /// <param name="ycids"></param>
        /// <param name="ufids"></param>
        /// <param name="taskNo"></param>
        /// <param name="sendWay"></param>
        public void CreateAndSendGCSJDataFile(string dataType, string[] ycids, string[] ufids, string taskNo
            , CommunicationWays sendWay , bool send, int desID, out string[] filePaths, out string msg)
        {
            string subDataType;
            string fileName;
            string dataFileName = string.Empty;
            msg = string.Empty;
            filePaths = null;
            Dictionary<string, string> fileList = new Dictionary<string,string>();
            YCPG oYc;
            UserFrame oUF;
            bool blResult = true;
            FileCreateResult oFCResult = FileCreateResult.CreateSuccess;

            #region 逐个创建文件
            //生成状态数据文件
            for (int i = 0; i < ycids.Length; i++)
            {
                oYc = new YCPG().GetByID(Convert.ToInt32(ycids[i]));
                oFCResult = CreateYCPGFile(oYc, out subDataType, taskNo, out fileName, out msg);
                if (oFCResult != FileCreateResult.CreateSuccess)
                {
                    blResult = false;
                    break;
                }
                else
                    fileList.Add(fileName, subDataType);
            }
            //生成图像数据文件
            if (blResult)
            {
                for (int j = 0; j < ufids.Length; j++)
                {
                    oUF = new UserFrame().GetByID(Convert.ToInt32(ufids[j]));
                    oFCResult = CreateUFFile(oUF, out fileName, out subDataType, out msg);
                    if (oFCResult != FileCreateResult.CreateSuccess)
                    {
                        blResult = false;
                        break;
                    }
                else
                    fileList.Add(fileName, subDataType);
                }
                //生成试验数据文件
                if (blResult)
                {
                    string[] subFiles = ListKey2Array(fileList);
                    oFCResult = CreateTestDataFile(dataType, taskNo, subFiles, out dataFileName, out msg);
                    if (oFCResult != FileCreateResult.CreateSuccess)
                        blResult = false;
                    else
                        fileList.Add(dataFileName, dataType);
                }
            }
            #endregion

            if (!send)
                return;//不发送
            //都创建成功了，调用文件发送服务器逐个进行发送
            int senderID = new XYXSInfo().GetByAddrMark(Param.SourceCode).Id;
            int infoTypeID = new InfoType().GetIDByExMark(dataType);
            if (blResult)
            {
                FileSender oSender = new FileSender();
                int infoID = 0;
                filePaths = new string[fileList.Count];
                int i = 0;
                foreach (KeyValuePair<string, string> kval in fileList)
                {
                    infoID = new InfoType().GetIDByExMark(kval.Value);
                    oSender.SendFile(kval.Key, Param.OutPutPath, sendWay, senderID, desID
                        , infoID, true);
                    filePaths[i] = Path.Combine(Param.OutPutPath, kval.Key);
                    GDFX.Logger.GetLogger().Error(string.Format("GCSJ Path{0}:  {1}", i, filePaths[i]));
                    i++;
                }
            }
            else//有创建失败的，就删除已创建文件，并在文件发送记录里写一条总记录
            {
                foreach (KeyValuePair<string, string> kval in fileList)
                {
                    dataFileName = kval.Key;
                    DataFileHandle.DeleteFile(Param.OutPutPath + kval.Key);
                    GDFX.Logger.GetLogger().Error(string.Format("Delete GCSJ Path:  {0}", Param.OutPutPath + kval.Key));
                }
                #region 插入一条发送记录
                FileSendInfo oSendInfo = new FileSendInfo();
                oSendInfo.FileName = dataFileName;
                oSendInfo.FileSize = 0;
                oSendInfo.FilePath = Param.OutPutPath;
                oSendInfo.FileCode = 0;
                oSendInfo.CurPosition = 0;
                oSendInfo.RetryTimes = 0;
                oSendInfo.SenderID = senderID;
                oSendInfo.ReceiverID = desID;
                oSendInfo.SendStatus = SendStatuss.GenerateFailed;
                oSendInfo.Remark = string.Empty;
                oSendInfo.SubmitTime = DateTime.Now;
                oSendInfo.InfoTypeID = infoTypeID;
                try
                {
                    FieldVerifyResult oResult = oSendInfo.Add();
                    if (oResult != FieldVerifyResult.Success)
                    {
                        msg = string.Format("删除文件完成，插入发送记录失败，{0}", oResult.ToString());
                        Log(msg);
                    }
                }
                catch (Exception ex)
                {
                    msg = string.Format("删除文件完成，插入发送记录异常，异常信息：{0}", ex.Message);
                    Log("删除文件完成，插入发送记录异常", ex);
                }
                finally { }
                #endregion
            }
        }

        /// <summary>
        /// 生成KJJD试验数据
        /// </summary>
        /// <param name="ycids"></param>
        /// <param name="ufids"></param>
        /// <param name="taskNo"></param>
        /// <param name="sendWay"></param>
        public void CreateAndSendJDSJDataFile(string dataType, string[] ycids, string taskNo, CommunicationWays sendWay, bool send
            , int desID, out string[] filePaths, out string msg)
        {
            string subDataType;
            string fileName;
            string dataFileName = string.Empty;
            filePaths = null;
            Dictionary<string, string> fileList = new Dictionary<string, string>();
            YCPG oYc;
            bool blResult = true;
            FileCreateResult oFCResult = FileCreateResult.CreateSuccess;
            msg = string.Empty;

            #region 逐个创建文件
            //生成状态数据文件
            for (int i = 0; i < ycids.Length; i++)
            {
                oYc = new YCPG().GetByID(Convert.ToInt32(ycids[i]));
                oFCResult = CreateYCPGFile(oYc, out subDataType, taskNo, out fileName, out msg);
                if (oFCResult != FileCreateResult.CreateSuccess)
                {
                    blResult = false;
                    break;
                }
                else
                    fileList.Add(fileName, subDataType);
            }
            //生成试验数据文件
            if (blResult)
            {
                string[] subFiles = ListKey2Array(fileList);
                oFCResult = CreateTestDataFile(dataType, taskNo, subFiles, out dataFileName, out msg);
                if (oFCResult != FileCreateResult.CreateSuccess)
                    blResult = false;
                else
                    fileList.Add(dataFileName, dataType);
            }
            #endregion

            if (!send)
                return;//不发送
            //都创建成功了，调用文件发送服务器逐个进行发送
            int senderID = new XYXSInfo().GetByAddrMark(Param.SourceCode).Id;
            int infoTypeID = new InfoType().GetIDByExMark(dataType);
            if (blResult)//都创建成功标志
            {
                FileSender oSender = new FileSender();
                int infoID = 0;
                int i = 0;
                filePaths = new string[fileList.Count];
                foreach (KeyValuePair<string, string> kval in fileList)
                {
                    infoID = new InfoType().GetIDByExMark(kval.Value);
                    oSender.SendFile(kval.Key, Param.OutPutPath, sendWay, senderID, desID
                        , infoID, true);
                    filePaths[i] = Path.Combine(Param.OutPutPath, kval.Key);
                    GDFX.Logger.GetLogger().Error(string.Format("JDSJ Path{0}:  {1}", i, filePaths[i]));
                    i++;
                }
            }
            else//有创建失败的，就删除已创建文件，并在文件发送记录里写一条总记录
            {
                foreach (KeyValuePair<string, string> kval in fileList)
                {
                    dataFileName = kval.Key;
                    DataFileHandle.DeleteFile(Param.OutPutPath + kval.Key);
                    GDFX.Logger.GetLogger().Error(string.Format("Delete JDSJ Path:  {0}", Param.OutPutPath + kval.Key));
                }
                #region 插入一条发送记录
                FileSendInfo oSendInfo = new FileSendInfo();
                oSendInfo.FileName = dataFileName;
                oSendInfo.FileSize = 0;
                oSendInfo.FilePath = Param.OutPutPath;
                oSendInfo.FileCode = 0;
                oSendInfo.CurPosition = 0;
                oSendInfo.RetryTimes = 0;
                oSendInfo.SenderID = senderID;
                oSendInfo.ReceiverID = desID;
                oSendInfo.SendStatus = SendStatuss.GenerateFailed;
                oSendInfo.Remark = string.Empty;
                oSendInfo.SubmitTime = DateTime.Now;
                oSendInfo.InfoTypeID = infoTypeID;
                try
                {
                    FieldVerifyResult oResult = oSendInfo.Add();
                    if (oResult != FieldVerifyResult.Success)
                    {
                        msg = string.Format("删除文件完成，插入发送记录失败", oResult.ToString());
                        Log(msg);
                    }
                }
                catch (Exception ex)
                {
                    msg = string.Format("删除文件完成，插入发送记录异常，异常信息：{0}", ex.Message);
                    Log("删除文件完成，插入发送记录异常", ex);
                }
                finally { }
                #endregion
            }
        }

        /// <summary>
        /// YCPG数据生成文件，试验数据分发时使用
        /// </summary>
        /// <param name="ycinfo">YCPG数据</param>
        /// <param name="dataType">试验数据类型</param>
        /// <param name="taskNo">任务代号</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        private FileCreateResult CreateYCPGFile(YCPG ycinfo, out string dataType, string taskNo, out string fileName, out string msg)
        {
            FileCreateResult oResult = FileCreateResult.CreateSuccess;
            dataType = GetInfoTypeFromYCPG(ycinfo);
            fileName = string.Empty;
            msg = string.Empty;
            if (string.IsNullOrEmpty(dataType))
            {
                msg = "获取不到dataType";
                Log(msg);
                return FileCreateResult.LackFileInfo;
            }

            List<string> tgtList = FileExchangeConfig.GetTgtListForSending(dataType);
            FileBaseInfo oFile = new FileBaseInfo();
            InfoType oInfo = new InfoType();
            try
            {
                #region Build File Basic info
                oInfo.Id = oInfo.GetIDByExMark(dataType);
                oInfo = oInfo.SelectByID();
                oFile.CTime = ycinfo.CTime;//DateTime.Now;
                oFile.From = Param.SourceName;
                oFile.TaskID = GetSendingTaskName(taskNo);
                oFile.InfoTypeName = oInfo.DATANAME + "(" + oInfo.EXCODE + ")";
                oFile.LineCount = 1;
                XYXSInfo oXyxs = new XYXSInfo();
                oXyxs = oXyxs.GetByAddrMark(tgtList[0]);
                oFile.To = oXyxs.ADDRName + tgtList[0] + "(" + oXyxs.EXCODE + ")";
                fileName = FileNameMaker.GenarateFileNameTypeThree(dataType, tgtList[0], ycinfo.CTime);
                oFile.FullName = Param.OutPutPath + fileName;
                #endregion
            }
            catch (Exception ex)
            {
                msg = string.Format("构建{0}文件基本信息异常", dataType);
                Log(msg, ex);
            }
            finally { }

            if (ycinfo.STBlob == null)
                return FileCreateResult.SomethingError;

            DataFileHandle oFileHandle = new DataFileHandle("");
            #region YC数据写入文件，读完就删除
            string strLine = string.Empty;
            string strYCFName = Param.TempJHSavePath + Guid.NewGuid() + ".txt";
            List<string> lstDatas = new List<string>();
            if (!Directory.Exists(Param.TempJHSavePath))
                Directory.CreateDirectory(Param.TempJHSavePath);
            DataFileHandle.WriteDataToFile(strYCFName, ycinfo.STBlob, ycinfo.STBlob.Length);

            StreamReader oReader = new StreamReader(strYCFName);
            strLine = oReader.ReadLine();
            while (!string.IsNullOrEmpty(strLine))
            {
                lstDatas.Add(strLine);
                strLine = oReader.ReadLine();
            }
            oReader.Close();
            DataFileHandle.DeleteFile(strYCFName);
            #endregion

            string[] fields = BuildYCFields(dataType);
            oFile.LineCount = lstDatas.Count();
            if (lstDatas.Count > 0)
            {
                try
                {
                    oResult = oFileHandle.CreateFormat3File(oFile, fields, lstDatas.ToArray());
                }
                catch (Exception ex)
                {
                    msg = string.Format("创建{0}文件异常，文件路径{1}", dataType, oFile.FullName);
                    Log(msg, ex);
                }
                finally { }
            }

            return oResult;
        }

        /// <summary>
        /// 根据用户帧数据创建文件
        /// </summary>
        /// <param name="ufInfo"></param>
        /// <returns></returns>
        private FileCreateResult CreateUFFile(UserFrame ufInfo, out string fileName, out string dataType, out string msg)
        {
            FileCreateResult oResult = FileCreateResult.CreateSuccess;
            msg = string.Empty;
            dataType = GetInfoTypeFromUserFrame(ufInfo);
            fileName = string.Empty;
            if (string.IsNullOrEmpty(dataType))
            {
                msg = "获取不到dataType";
                Log(msg);
                return FileCreateResult.LackFileInfo;
            }

            List<string> tgtList = FileExchangeConfig.GetTgtListForSending(dataType);
            try
            {
                fileName = FileNameMaker.GenarateFileNameTypeThree(dataType, tgtList[0], ufInfo.CTime);
            }
            catch (Exception ex)
            {
                msg = "生成图像数据文件名异常";
                Log(msg, ex);
            }
            finally { }

            string fileFullName = Param.OutPutPath + fileName;
            string strUFFilePath = System.Configuration.ConfigurationManager.AppSettings["UFFilePath"];
            strUFFilePath += ufInfo.Directory.Substring(3).Replace('/', '\\');
            Log(strUFFilePath);
            if (dataType == "PLEO")//LEO成像 相机图像数据
            {
                if (string.IsNullOrEmpty(strUFFilePath))
                {
                    msg = "无法获取UFFilePath";
                    Log(msg);
                    oResult = FileCreateResult.LackFileInfo;
                }
                else
                {
                    if (ConvertPLEOData(strUFFilePath + ufInfo.FileName, fileFullName, out msg))
                        oResult = FileCreateResult.CreateSuccess;
                    else
                        oResult = FileCreateResult.SomethingError;
                }
            }
            else
            {
                string stmp = DataFileHandle.CopyFile(strUFFilePath + ufInfo.FileName, Param.OutPutPath, fileName);
                if (stmp != string.Empty)
                {
                    msg = string.Format("复制图像数据失败：{0}", stmp);
                    Log(msg);
                }
            }
            return oResult;
        }

        /// <summary>
        /// 创建试验数据文件
        /// </summary>
        /// <param name="dataType">试验数据类型（GCSJ\JDSJ\TYSJ）</param>
        /// <param name="taskNo"></param>
        /// <param name="datas"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private FileCreateResult CreateTestDataFile(string dataType, string taskNo, 
            string[] datas, out string fileName, out string msg)
        {
            FileCreateResult oResult = FileCreateResult.CreateSuccess;
            msg = string.Empty;
            fileName = string.Empty;
            List<string> tgtList = FileExchangeConfig.GetTgtListForSending(dataType);
            FileBaseInfo oFile = new FileBaseInfo();
            InfoType oInfo = new InfoType();
            try
            {
                #region Build File Basic info
                oInfo.Id = oInfo.GetIDByExMark(dataType);
                oInfo = oInfo.SelectByID();
                oFile.CTime = DateTime.Now;
                oFile.From = Param.SourceName;
                oFile.TaskID = GetSendingTaskName(taskNo);
                oFile.InfoTypeName = oInfo.DATANAME + "(" + oInfo.EXCODE + ")";
                oFile.LineCount = datas.Length;
                XYXSInfo oXyxs = new XYXSInfo();
                oXyxs = oXyxs.GetByAddrMark(tgtList[0]);
                oFile.To = oXyxs.ADDRName + tgtList[0] + "(" + oXyxs.EXCODE + ")";
                fileName = FileNameMaker.GenarateFileNameTypeThree(dataType, tgtList[0]);
                oFile.FullName = Param.OutPutPath + fileName;
                #endregion
            }
            catch (Exception ex)
            {
                msg = string.Format("构建试验数据{0}文件基本信息异常", dataType);
                Log(msg, ex);
            }
            finally { }

            DataFileHandle oFileHandle = new DataFileHandle("");
            string[] fields = new string[0];
            try
            {
                oResult = oFileHandle.CreateFormat3File(oFile, fields, datas);
            }
            catch (Exception ex)
            {
                msg = string.Format("创建试验数据{0}文件异常，文件路径{1}，异常信息：{2}", dataType, oFile.FullName, ex.Message);
                Log(msg, ex);
            }
            finally { }
            return oResult;
        }

        /// <summary>
        /// 根据数据类型构建字段
        /// </summary>
        /// <param name="dataType">InfoType:Exmark</param>
        /// <returns></returns>
        private string[] BuildYCFields(string dataType)
        {
            string[] fields = new string[1];
            switch (dataType)
            {
                case "GCZT"://45
                    fields[0] = "T0,Px78,Px79,Px80,Px81,Px82,Px83,PK1,PK2,PK3,PK4,PK5,PK6,PK7,PK8,PK9,PK10,PK11,PK12,PK14,PK15,PK16,PK32,PK33,PK34,PK35,PK36,PK37,PK38,PK39,ZT68,Px148,Px150,Px153,Px154,Px155,ZCX32,ZCX13,ZCX14,ZCX15,ZCX25,ZCX21,ZCX71,ZCX22,ZCX72".Replace(",", "  ");
                    break;
                case "JDZT"://80
                    fields[0] = "ZJ,T0,FL1,FL2,FL3,X1,Y1,Z1,VX1,VY1,VZ1,q0,q1,q2,q3,φ,θ,ψ,φφ,θθ,ψψ,AX1,AY1,AZ1,R,A,E,BJ1,BJ2,a1,e1,i1,Ω1,w1,u1,RL1,X2,Y2,Z2,VX2,VY2,VZ2,X3,Y3,Z3,VX3,VY3,VZ3,Tx,Ty,Tz2,BJ3,a2,u2,a3,e3,i3,Ω3,w3,u3,a4,e4,i4,Ω4,w4,u4,RL2,RL3,X4,Y4,Z4,VX4,VY4,VZ4,AX4,AY4,AZ4,Px,Py,Pz".Replace(",", "  ");
                    break;
                case "JDCL"://50
                    fields[0] = "ZJ,T0,T1,BZ1,GLAX,GLAY,GLAZ,GLAXX,GLAYY,GLAZZ,T2,BZ2,GLBX,GLBY,GLBZ,GLBXX,GLBYY,GLBZZ,BZ3,R1,A1,E1,RR1,AA1,EE1,LA1,LE1,BZ4,R2,A2,E2,RR2,AA2,EE2,LA2,LE2,qa0,qa1,qa2,qa3,qb0,qb1,qb2,qb3,Wx,Wy,Wz,Wxx,Wyy,Wzz".Replace(",", "  ");
                    break;
            }
            return fields;
        }

        /// <summary>
        /// 通过YCPG类型获取信息类型
        /// </summary>
        /// <param name="ycinfo"></param>
        /// <returns></returns>
        private string GetInfoTypeFromYCPG(YCPG ycinfo)
        {
            string strTJType = System.Configuration.ConfigurationManager.AppSettings["TJtypeInYCPG"];
            string strJDTypes = System.Configuration.ConfigurationManager.AppSettings["JDtypeInYCPG"];

            string strType = string.Empty;
            if (strTJType != null)
                strTJType = strTJType.Replace("'", "");
            else
                return null;

            if (strJDTypes != null)
                strJDTypes = strJDTypes.Replace("'", "");
            else
                return null;
            string[] strJDType = strJDTypes.Split(new char[]{','});
            if (strJDType.Length != 2)
                return null;

            if (ycinfo.SType == strTJType)
                strType = "GCZT";
            else if (ycinfo.SType == strJDType[0])
                strType = "JDZT";
            else if (ycinfo.SType == strJDType[1])
                strType = "JDCL";
            else
                return null;
            return strType;
        }
        
        /// <summary>
        /// 通过用户帧信息获取信息类型
        /// </summary>
        /// <param name="ufinfo"></param>
        /// <returns></returns>
        private string GetInfoTypeFromUserFrame(UserFrame ufinfo)
        {
            //格式：'','',''
            string strUFTypes = System.Configuration.ConfigurationManager.AppSettings["TJtypeInUserFrame"];
            string strType = string.Empty;

            if (strUFTypes != null)
                strUFTypes = strUFTypes.Replace("'", "");
            else
                return null;
            string[] strJDType = strUFTypes.Split(new char[] { ',' });
            if (strJDType.Length != 3)
                return null;

            if (ufinfo.Userid == strJDType[0])
                strType = "PGEO";
            else if (ufinfo.Userid == strJDType[1])
                strType = "PLEO";
            else if (ufinfo.Userid == strJDType[2])
                strType = "GLEO";
            else
                return null;
            return strType;
        }

        /// <summary>
        /// 将PLEO引导成像图像数据转成输出格式
        /// </summary>
        /// <param name="srcDataFilePath"></param>
        /// <param name="tgtDataFilePath"></param>
        /// <returns></returns>
        private bool ConvertPLEOData(string srcDataFilePath, string tgtDataFilePath, out string msg)
        {
            //文件总大小：41944000b / 8 = 5243000B
            msg = string.Empty;
            long iFileSize = 5243000;
            bool blResult = false;
            FileStream oFRStream = new FileStream(srcDataFilePath, FileMode.Open, FileAccess.Read);
            System.IO.File.Copy(srcDataFilePath, tgtDataFilePath, true);
            iFileSize = (new System.IO.FileInfo(srcDataFilePath)).Length;
            FileStream oFWStream = new FileStream(tgtDataFilePath, FileMode.Truncate, FileAccess.Write);
            try
            {
                byte[] btData;
                byte[] btTmp;
                int iLen = 0;
                int iIdx = 0;
                string sTmp = string.Empty;
                string sOff = string.Empty;
                string sBin = string.Empty;
                oFRStream.Seek(0, SeekOrigin.Begin);

                iLen = 12;//4B同步码+8B时间
                btData = new byte[iLen];
                iIdx += oFRStream.Read(btData, iIdx, iLen);
                oFWStream.Write(btData, 0, btData.Length);

                iLen = 5;//40b帧标识
                btData = new byte[iLen];
                iIdx += oFRStream.Read(btData, 0, iLen);
                oFWStream.Write(btData, 0, btData.Length);

                iLen = 3;//20bit帧计数，高12bit补0
                btData = new byte[iLen];
                iIdx += oFRStream.Read(btData, 0, iLen);
                sTmp = Byte2BinaryStr(btData[iLen - 1]);
                sOff = sTmp.Substring(4).PadLeft(8, '0');
                btData[iLen - 1] = BinaryStr2Byte(sOff);
                btTmp = new byte[4];
                Array.Copy(btData, 0, btTmp, 0, iLen);
                oFWStream.Write(btTmp, 0, btTmp.Length);

                sOff = sTmp.Substring(0, 4);
                iLen = 5;//40b时间码，接前面错过来的4位
                btData = new byte[iLen];
                iIdx += oFRStream.Read(btData, 0, iLen);
                btTmp = new byte[iLen - 1];
                Array.Copy(btData, 0, btTmp, 0, iLen - 1);
                sBin = Bytes2BinaryStr(btTmp);
                sTmp = Byte2BinaryStr(btData[iLen - 1]);
                sBin = sTmp.Substring(4) + sOff + sBin;
                btTmp = BinaryStr2Bytes(sBin);
                oFWStream.Write(btData, 0, btData.Length);

                sOff = sTmp.Substring(0, 4);
                iLen = 25;//200b遥测参数，接前面错过来的4位
                btData = new byte[iLen];
                iIdx += oFRStream.Read(btData, 0, iLen);
                btTmp = new byte[iLen - 1];
                Array.Copy(btData, 0, btTmp, 0, iLen - 1);
                sBin = Bytes2BinaryStr(btTmp);
                sTmp = Byte2BinaryStr(btData[iLen - 1]);
                sBin = sTmp.Substring(4) + sOff + sBin;
                btTmp = BinaryStr2Bytes(sBin);
                oFWStream.Write(btData, 0, btData.Length);

                sOff = sTmp.Substring(0, 4);
                iLen = 82;//660b保留
                btData = new byte[iLen];
                iIdx += oFRStream.Read(btData, 0, iLen);
                //Array.Copy(btData, 0, btTmp, 0, iLen - 1);
                sBin = Bytes2BinaryStr(btData);
                //sTmp = Byte2BinaryStr(btData[iLen - 1]).Substring(4);
                sBin = "0000" + sBin + sOff;// +sTmp;
                btTmp = BinaryStr2Bytes(sBin);
                oFWStream.Write(btData, 0, btData.Length);

                int iBufferSize = 1024;
                btData = new byte[iBufferSize];
                iLen = iBufferSize;
                while (iLen == iBufferSize)
                {
                    iLen = oFRStream.Read(btData, 0, iBufferSize);
                    oFWStream.Write(btData, 0, iLen);
                }
                blResult = true;
            }
            catch (Exception ex)
            {
                blResult = false;
                msg = "转换LEO引导 相机成像文件出现异常" + ex.Message;
                Log("转换LEO引导 相机成像文件出现异常", ex);
            }
            finally 
            {
                if (oFRStream != null)
                {
                    oFRStream.Close();
                    oFRStream.Dispose();
                }
                if (oFWStream != null)
                {
                    oFWStream.Close();
                    oFWStream.Dispose();
                }
            }
            return blResult;
        }

        /// <summary>
        /// 把List的key转成字符串数组
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string[] ListKey2Array(Dictionary<string, string> list)
        {
            string[] keys = new string[list.Count];
            int iIdx = 0;
            foreach (KeyValuePair<string, string> kval in list)
            {
                keys[iIdx] = kval.Key;
                iIdx++;
            }
            return keys;
        }

        /// <summary>
        /// byte转换为二进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string Byte2BinaryStr(byte data)
        {
            return Convert.ToString(data, 2).PadLeft(8, '0');
        }

        /// <summary>
        /// (8位长)二进制字符串转换为1字节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte BinaryStr2Byte(string data)
        {
            return Convert.ToByte(data, 2);
        }

        /// <summary>
        /// 字节数组转成二进制字符串，逗号分隔
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string Bytes2BinaryStr(byte[] data)
        {
            StringBuilder oSB = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                oSB.Append(Byte2BinaryStr(data[i]));
            }
            return oSB.ToString();
        }

        /// <summary>
        /// 逗号分隔长二进制字符串转成字节数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] BinaryStr2Bytes(string data)
        {
            byte[] btData = new byte[data.Length / 8];
            int iIdx = 0;
            for (int i = 0; i < btData.Length; i++)
            {
                btData[i] = BinaryStr2Byte(data.Substring(iIdx, 8));
                iIdx += 8;
            }
            return btData;
        }

        /// <summary>
        /// 通过内部任务号和卫星号获取外部任务代号
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="satID"></param>
        /// <returns></returns>
        private string GetSendingTaskName(string taskno)
        {
            string taskID = string.Empty;
            string satID = string.Empty;
            Task otask = new Task();
            otask.GetTaskNoSatID(taskno, out taskID, out satID);
            return otask.GetTaskName(taskID, satID) + "(" + otask.GetOutTaskNo(taskID, satID) + ")";
        }
    }
}
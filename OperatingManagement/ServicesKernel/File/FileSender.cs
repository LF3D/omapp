using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using OperatingManagement.RemotingObjectInterface;
using OperatingManagement.Framework;


namespace ServicesKernel.File
{
    public class FileSender
    {
        /// <summary>
        /// return submit result: success or failed
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="sendway"></param>
        /// <param name="senderID"></param>
        /// <param name="receiverID"></param>
        /// <param name="infotypeID"></param>
        /// <param name="isAutoRetry"></param>
        /// <returns></returns>
        public bool SendFile(string fileName, string filePath, CommunicationWays sendway, int senderID, int receiverID, int infotypeID, bool isAutoRetry)
        {
            IFileSender fileSender = FileSenderClientAgent.GetObject<IFileSender>();
            string strResult = "";
            //0:ftp;1:udp;2:tcp
            try
            {
                strResult = fileSender.SendFile(fileName, filePath, (int)sendway, senderID, receiverID, infotypeID, isAutoRetry);
            }
            catch (Exception ex)
            {
                //should to do sth
                return false;
            }
            finally { }
            if (strResult.Equals(string.Empty))
            {
                return false;
            }
            XElement root = XElement.Parse(strResult);
            //1，提交失败，0，提交成功
            if (root.Element("code").Value == "1")
                return false;
            else
                return true;
        }

        /// <summary>
        /// 获取文件发送状态
        /// </summary>
        /// <param name="fileID"></param>
        /// <returns></returns>
        public string GetSendStatus(int fileID)
        {
            string strResult = string.Empty;
            IFileSender fileSender = FileSenderClientAgent.GetObject<IFileSender>();
            strResult = fileSender.GetSendStatus(fileID);
            return strResult;
        }

        /// <summary>
        /// 重新发送文件
        /// </summary>
        /// <param name="fileID"></param>
        /// <returns></returns>
        public string ReSendFile(int fileID)
        {
            string strResult = string.Empty;
            IFileSender fileSender = FileSenderClientAgent.GetObject<IFileSender>();
            strResult = fileSender.ReSendFile(fileID);
            return strResult;
        }
    }
}

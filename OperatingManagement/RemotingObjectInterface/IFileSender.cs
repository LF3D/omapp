using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.RemotingObjectInterface
{
    /// <summary>
    /// 文件服务器的接口
    /// </summary>
    public interface IFileSender
    {
        /// <summary>
        /// 发送文件，返回结果为xml，提交结果，如果提交成功，返回FileID
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="sendWay"></param>
        /// <param name="senderID"></param>
        /// <param name="receiverID"></param>
        /// <param name="infoTypeID"></param>
        /// <param name="autoResend"></param>
        /// <returns></returns>
        string SendFile(string fileName, string filePath, int sendWay, int senderID, int receiverID, int infoTypeID, bool autoResend);

        /// <summary>
        /// 发送文件，返回结果为xml，提交结果，如果提交成功，返回FileID
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="sendWay">0:ftp;1:udp;2:tcp</param>
        /// <param name="senderAddr"></param>
        /// <param name="receiverAddr"></param>
        /// <param name="infoTypeExMark">信息外部编码</param>
        /// <param name="autoResend"></param>
        /// <returns></returns>
        string SendFile(string fileName, string filePath, int sendWay, string senderAddr, string receiverAddr, string infoTypeExMark, bool autoResend);
        
        /// <summary>
        /// 查询文件发送状态，返回文件发送状态及备注
        /// </summary>
        /// <param name="fileID"></param>
        /// <returns></returns>
        string GetSendStatus(int fileID);

        /// <summary>
        /// 重新发送文件，返回任务提交结果
        /// </summary>
        /// <param name="fileID"></param>
        /// <returns></returns>
        string ReSendFile(int fileID);
    }
}

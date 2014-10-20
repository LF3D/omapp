using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.RemotingObjectInterface
{
    /// <summary>
    /// 数据帧处理器的接口
    /// </summary>
    public interface IDFSender
    {
        /// <summary>
        /// 发送数据帧，返回结果为xml，发送结果，如果发送成功，返回记录ID
        /// </summary>
        /// <param name="data"></param>
        /// <param name="taskCode"></param>
        /// <param name="satelliteID"></param>
        /// <param name="mainType"></param>
        /// <param name="secondType"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="dataTime"></param>
        /// <returns></returns>
        string SendDF(byte[] data, string taskCode, string satelliteID, int infoTypeID, int source, int destination, DateTime dataTime);

        /// <summary>
        /// 查询文件发送状态，返回数据发送状态及备注
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        string GetSendStatus(int ID);

        /// <summary>
        /// 获取DFServer的运行状态，xml字符串，for运行管理
        /// </summary>
        /// <returns></returns>
        string GetDFSvrStatus();
    }
}

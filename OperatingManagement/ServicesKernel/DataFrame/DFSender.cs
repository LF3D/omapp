using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using OperatingManagement.RemotingObjectInterface;
using OperatingManagement.Framework;

namespace ServicesKernel.DataFrame
{
    public class DFSender
    {
        /// <summary>
        /// 发送数据帧
        /// </summary>
        /// <param name="data"></param>
        /// <param name="taskCode"></param>
        /// <param name="satelliteID"></param>
        /// <param name="infoTypeID"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public string SendDF(byte[] data, string taskCode, string satelliteID, int infoTypeID, int source, int destination, DateTime dataTime)
        {
            IDFSender oSender = DFSenderClientAgent.GetObject<IDFSender>();
            return oSender.SendDF(data, taskCode, satelliteID, infoTypeID, source, destination, dataTime);
        }
    }
}

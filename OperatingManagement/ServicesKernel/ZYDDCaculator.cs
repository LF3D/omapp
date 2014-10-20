using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FileServer.Base;
using ServiceBusAPI;

namespace ServicesKernel
{
    public class ZYDDCaculator
    {
        private const string seperator = "|";
        /// <summary>
        /// 网络流
        /// </summary>
        private NetworkStream NetStream { get; set; }
        private TcpClient client = null;
        public string[] ResultFileNames { get; set; }

        public ZYDDCaculator()
        {
        }

        public string DoCaculate(string reqFilePath, out string resultPath)
        {
            string strResult = string.Empty;
            resultPath = string.Empty;
            //获取资源调度服务IP，端口
            IPAddress ip;
            int port;
            if (!GetZYDDServerIP(out ip, out port))
            {
                strResult = "获取不到资源调度服务IP地址信息";
                return strResult;
            }

            this.client = new TcpClient();
            //连接资源调度服务
            if (!ConnectServer(ip, port))
            {
                strResult = "连接资源调度服务失败";
                return strResult;
            }

            //构建请求包
            byte[] datas = Utility.StringToByte(reqFilePath);
            if (!SendData(datas))
            {
                strResult = "发送参数数据失败";
                return strResult;
            }
            int readCount = 0;
            int bufferSize = 1024;
            byte[] responseBlock = new byte[bufferSize];
            DateTime beginWaitResponseTime = DateTime.Now;

            #region -等待应答包，超时或异常时关闭连接
            try
            {
                while (true)
                {
                    lock (this.NetStream)
                    {
                        if (this.NetStream.DataAvailable)
                        {
                            readCount = this.NetStream.Read(responseBlock, 0, bufferSize);
                        }
                    }
                    if (readCount > 0)
                    {
                        break;
                    }
                    TimeSpan ts = DateTime.Now - beginWaitResponseTime;
                    if (ts.TotalMilliseconds >= 60000)//1分钟
                    {
                        strResult = string.Format("等待应答超时，目标系统：{0}", ip.ToString());
                        Logger.GetLogger().Debug(strResult);
                        CloseClientConnect();
                        return strResult;
                    }
                }
            }
            catch (Exception ex)
            {
                strResult = string.Format("资源调度计算出现异常{0}", ex.Message);
                Logger.GetLogger().Error("资源调度计算出现异常", ex);
                CloseClientConnect();
                return strResult;
            }
            finally { }
            #endregion

            //关闭连接
            CloseClientConnect();
            resultPath = Utility.ByteToString(responseBlock);
            if (resultPath.ToUpper() == "ERR")
                strResult = "资源调度计算出现错误";
            else
                this.ResultFileNames = Directory.GetFiles(resultPath);
            return strResult;
        }

        /// <summary>
        /// 向目标系统发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool SendData(byte[] data)
        {
            try
            {
                if (this.NetStream == null)
                {
                    Logger.GetLogger().Error("网络流为空，不能发送请求");
                    return false;
                }
                lock (this.NetStream)
                {
                    this.NetStream.Write(data, 0, data.Length);
                    this.NetStream.Flush();
                }
            }
            catch (Exception e)
            {
                Logger.GetLogger().Error("SendData Exception", e);
                return false;
            }
            finally { }
            return true;
        }


        private bool GetZYDDServerIP(out IPAddress serverIP, out int port)
        {
            serverIP = null;
            port = 0;
            ServiceAPI oSrvApi = new ServiceAPI();
            string msg = string.Empty;
            try
            {
                msg = oSrvApi.Subscribe(IPAddress.Parse(Param.LocalIP), Param.ZYDDType);
                if (msg == "服务匹配成功！")
                {
                    serverIP = oSrvApi.ServiceIP;
                    port = oSrvApi.ServicePort;
                    oSrvApi.UnRegiste();
                    return true;
                }
                else
                {
                    Logger.GetLogger().Error(string.Format("获取资源调度服务IP失败，{0}", msg));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().Error("获取资源调度服务IP异常", ex);
                return false;
            }
        }

        private bool ConnectServer(IPAddress ip, int port)
        {
            while (true)
            {
                try
                {
                    client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    client.Connect(ip, port);
                    NetStream = client.GetStream();
                    Logger.GetLogger().Debug(string.Format("连接目标系统{0}服务器成功", ip));
                    return true;
                }
                catch (Exception ex)
                {
                    if (client != null)
                        CloseClientConnect();
                    Logger.GetLogger().Error(string.Format("连接目标系统{0}服务器异常", ip), ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        private void CloseClientConnect()
        {
            if (this.NetStream != null)
            {
                this.NetStream.Close();
                this.NetStream.Dispose();
            }
            if (this.client != null)
            {
                this.client.Close();
                this.client = null;
            }
            Logger.GetLogger().Debug("关闭连接");
        }
    }
}

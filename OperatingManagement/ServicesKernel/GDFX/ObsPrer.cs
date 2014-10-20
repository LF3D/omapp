using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ServiceBusAPI;
using FileServer.Base;

namespace ServicesKernel.GDFX
{
    /// <summary>
    /// 轨道分析-轨道预报
    /// </summary>
    public class ObsPrer
    {
        private const string seperator = "|";
        /// <summary>
        /// 网络流
        /// </summary>
        private NetworkStream NetStream { get; set; }
        private TcpClient client = null;
        public string[] ResultFileNames { get; set; }

        public ObsPrer()
        {
        }

        /// <summary>
        /// 轨道预报
        /// </summary>
        /// <param name="fromDate">起始日期</param>
        /// <param name="preDays">预报时长（天）</param>
        /// <param name="preInterval">预报间隔</param>
        /// <param name="satID">卫星编号</param>
        /// <param name="dmzID">地面站编码</param>
        /// <param name="QCY">圈次源，是否</param>
        /// <param name="QC">圈次</param>
        /// <returns></returns>
        public string DoCaculate(DateTime fromDate, int preDays, int preInterval
            , string satID, string[] dmzID, bool QCY, int QC, out string resultPath)
        {
            string strResult = string.Empty;
            resultPath = string.Empty;
            //获取轨道预报服务IP，端口
            IPAddress ip;
            int port;
            if (!GetObsPreServerIP(out ip, out port))
            {
                strResult = "获取不到轨道预报服务IP地址信息";
                return strResult;
            }

            this.client = new TcpClient();
            //连接轨道预报服务
            if (!ConnectServer(ip, port))
            {
                strResult = "连接轨道预报服务失败";
                return strResult;
            }

            //构建请求包
            byte[] datas = BuildRequestPack(fromDate, preDays, preInterval, satID, dmzID, QCY, QC);
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
                strResult = string.Format("轨道预报计算出现异常{0}", ex.Message);
                Logger.GetLogger().Error("轨道预报计算出现异常", ex);
                CloseClientConnect();
                return strResult;
            }
            finally { }
            #endregion

            //关闭连接
            CloseClientConnect();
            resultPath = Utility.ByteToString(responseBlock);
            if (resultPath.ToUpper() == "ERR")
                strResult = "轨道预报计算出现错误";
            else
                this.ResultFileNames = Directory.GetFiles(resultPath);
            return strResult;
        }

        private bool GetObsPreServerIP(out IPAddress serverIP, out int port)
        {
            serverIP = null;
            port = 0;
            ServiceAPI oSrvApi = new ServiceAPI();
            string msg = string.Empty;
            try
            {
                msg = oSrvApi.Subscribe(IPAddress.Parse(Param.LocalIP), Param.GDYBType);
                if (msg == "服务匹配成功！")
                {
                    serverIP = oSrvApi.ServiceIP;
                    port = oSrvApi.ServicePort;
                    oSrvApi.UnRegiste();
                    return true;
                }
                else
                {
                    Logger.GetLogger().Error(string.Format("获取轨道预报服务IP失败，{0}", msg));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().Error("获取轨道预报服务IP异常", ex);
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

        private byte[] BuildRequestPack(DateTime fromDate, int preDays, int preInterval
            , string satID, string[] dmzID, bool QCY, int QC)
        {
            byte[] datas = null;
            try
            {
                short year = (short)fromDate.Year;
                byte month = (byte)fromDate.Month;
                byte day = (byte)fromDate.Day;
                int dmzCount = dmzID.Length;
                TimeSpan ts = fromDate - new DateTime(year, month, day);
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryWriter br = new BinaryWriter(ms);
                    br.Write(year);
                    br.Write(month);
                    br.Write(day);
                    br.Write(Convert.ToUInt32(ts.TotalMilliseconds));
                    br.Write(preDays * 86400000);//毫秒
                    br.Write(preInterval);
                    br.Write(Convert.ToInt32(satID, 16));
                    br.Write(QCY);//QCY
                    br.Write(QC);
                    br.Write(dmzCount);
                    for (int i = 0; i < dmzID.Length; i++)
                    {
                        br.Write(Convert.ToInt16(dmzID[i], 16));
                    }
                    datas = ms.ToArray();
                    Logger.GetLogger().Error(string.Format("year:{0}, month:{1}, day:{2}, ms:{3}, predays:{4}, preInterval:{5}, satid:{6}, QCY:{7}, QC:{8}, DMZCount:{9}, DMZID:{10}"
                        , year, month, day, ts.TotalMilliseconds, preDays, preInterval, satID, QCY, QC, dmzCount, dmzID));
                    string print = string.Empty;
                    for (int i = 0; i < datas.Length; i++)
                    {
                        print += string.Format("Datas[{0}]：{1}", i, datas[i]);
                    }
                    Logger.GetLogger().Error(print);
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().Error("构建请求包出现异常", ex);
            }
            return datas;
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

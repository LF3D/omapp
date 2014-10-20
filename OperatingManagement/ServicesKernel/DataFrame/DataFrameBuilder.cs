using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperatingManagement.DataAccessLayer.PlanManage;
using FileServer.Base;

namespace ServicesKernel.DataFrame
{
    /// <summary>
    /// 数据帧构建器
    /// </summary>
    public class DataFrameBuilder
    {
        /// <summary>
        /// 将引导数据转换为byte数组
        /// </summary>
        /// <param name="ydsj"></param>
        /// <param name="dataTime"></param>
        /// <returns></returns>
        public byte[] BuildYDSJDF(OperatingManagement.DataAccessLayer.PlanManage.YDSJ ydsj, DateTime dataTime)
        {
            byte[] data = new byte[30];
            int iIdx = 0;
            int iLen = 0;
            TimeSpan ts = dataTime - new DateTime(2000, 1, 1, 0, 0, 0);

            //D
            iIdx = 0;
            iLen = 2;
            byte[] btTmp = Utility.ShortToNetByte((short)ts.TotalDays);
            Array.Copy(btTmp, 0, data, iIdx, iLen);

            //T
            iIdx += iLen;
            iLen = 4;
            btTmp = Utility.IntToNetByte(ts.Milliseconds * 10);
            Array.Copy(btTmp, 0, data, iIdx, iLen);

            //A，量化单位0.1m
            iIdx += iLen;
            iLen = 4;
            //btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.A * 10));
            //Array.Copy(btTmp, 0, data, iIdx, iLen);

            ////E，量化单位2-31
            //iIdx += iLen;
            //iLen = 4;
            //btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.E / Math.Pow(2, -31)));
            //Array.Copy(btTmp, 0, data, iIdx, iLen);

            ////I，量化单位2-24
            //iIdx += iLen;
            //iLen = 4;
            //btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.I / Math.Pow(2, -24)));
            //Array.Copy(btTmp, 0, data, iIdx, iLen);

            ////Q，量化单位2-22
            //iIdx += iLen;
            //iLen = 4;
            //btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.O / Math.Pow(2, -22)));
            //Array.Copy(btTmp, 0, data, iIdx, iLen);

            ////W，量化单位2-22
            //iIdx += iLen;
            //iLen = 4;
            //btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.W / Math.Pow(2, -22)));
            //Array.Copy(btTmp, 0, data, iIdx, iLen);

            ////M，量化单位2-22
            //iIdx += iLen;
            //iLen = 4;
            //btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.W / Math.Pow(2, -22)));
            //Array.Copy(btTmp, 0, data, iIdx, iLen);
            return data;
        }

        /// <summary>
        /// 将轨道数据转换为byte数组
        /// </summary>
        /// <param name="ydsj"></param>
        /// <param name="dataTime"></param>
        /// <returns></returns>
        public byte[] BuildYDSJDF(OperatingManagement.DataAccessLayer.BusinessManage.GD ydsj, DateTime dataTime)
        {
            byte[] data = new byte[30];
            int iIdx = 0;
            int iLen = 0;
            TimeSpan ts = dataTime - new DateTime(2000, 1, 1, 0, 0, 0);

            //D
            iIdx = 0;
            iLen = 2;
            byte[] btTmp = Utility.ShortToNetByte((short)ts.TotalDays);
            Array.Copy(btTmp, 0, data, iIdx, iLen);

            //T
            iIdx += iLen;
            iLen = 4;
            btTmp = Utility.IntToNetByte(ts.Milliseconds * 10);
            Array.Copy(btTmp, 0, data, iIdx, iLen);

            //A，量化单位0.1m
            iIdx += iLen;
            iLen = 4;
            btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.A * 10));
            Array.Copy(btTmp, 0, data, iIdx, iLen);

            //E，量化单位2-31
            iIdx += iLen;
            iLen = 4;
            btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.E / Math.Pow(2, -31)));
            Array.Copy(btTmp, 0, data, iIdx, iLen);

            //I，量化单位2-24
            iIdx += iLen;
            iLen = 4;
            btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.I / Math.Pow(2, -24)));
            Array.Copy(btTmp, 0, data, iIdx, iLen);

            //Q，量化单位2-22
            iIdx += iLen;
            iLen = 4;
            btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.Q / Math.Pow(2, -22)));
            Array.Copy(btTmp, 0, data, iIdx, iLen);

            //W，量化单位2-22
            iIdx += iLen;
            iLen = 4;
            btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.W / Math.Pow(2, -22)));
            Array.Copy(btTmp, 0, data, iIdx, iLen);

            //M，量化单位2-22
            iIdx += iLen;
            iLen = 4;
            btTmp = Utility.IntToNetByte(Convert.ToInt32(ydsj.W / Math.Pow(2, -22)));
            Array.Copy(btTmp, 0, data, iIdx, iLen);
            return data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperatingManagement.DataAccessLayer.BusinessManage;

namespace ServicesKernel.File
{
    /// <summary>
    /// 解析UDP帧内容
    /// </summary>
    public class UDPFrameHandle
    {
        private readonly DateTime InitialDate = new DateTime(2000,1,1);

        #region -Properties-
        private string _version = string.Empty;
        private string _flag = string.Empty;
        private string _maintype = string.Empty;
        private string _datatype = string.Empty;
        private string _sourceaddress = string.Empty;
        private string _destinationaddress = string.Empty;
        private string _missioncode = string.Empty;
        private string _satellitecode = string.Empty;
        private string _datadate = string.Empty;
        private string _datatime = string.Empty;
        private string _sequencenumber = string.Empty;
        private string _childrenpacknumber = string.Empty;
        private string _reserve = string.Empty;
        private string _datalength = string.Empty;
        private string _datainfo = string.Empty;

        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
        /// <summary>
        /// 标志
        /// </summary>
        public string Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
        /// <summary>
        /// 数据主类别
        /// </summary>
        public string MainType
        {
            get { return _maintype; }
            set { _maintype = value; }
        }
        /// <summary>
        /// 数据次类别
        /// </summary>
        public string DataType
        {
            get { return _datatype; }
            set { _datatype = value; }
        }
        /// <summary>
        /// 信源地址
        /// </summary>
        public string SourceAddress
        {
            get { return _sourceaddress; }
            set { _sourceaddress = value; }
        }
        /// <summary>
        /// 住宿地址
        /// </summary>
        public string DestinationAddress
        {
            get { return _destinationaddress; }
            set { _destinationaddress = value; }
        }
        /// <summary>
        /// 任务代号
        /// </summary>
        public string MissionCode
        {
            get { return _missioncode; }
            set { _missioncode = value; }
        }
        /// <summary>
        /// 卫星编号
        /// </summary>
        public string SatelliteCode
        {
            get { return _satellitecode; }
            set { _satellitecode = value; }
        }
        /// <summary>
        /// 数据日期
        /// </summary>
        public string DataDate
        {
            get { return _datadate; }
            set { _datadate = value; }
        }
        /// <summary>
        /// 数据时间
        /// </summary>
        public string DataTime
        {
            get { return _datatime; }
            set { _datatime = value; }
        }
        /// <summary>
        /// 数据报顺序编号
        /// </summary>
        public string SequenceNumber
        {
            get { return _sequencenumber; }
            set { _sequencenumber = value; }
        }
        /// <summary>
        /// 子包数
        /// </summary>
        public string ChildrenPackNumber
        {
            get { return _childrenpacknumber; }
            set { _childrenpacknumber = value; }
        }
        /// <summary>
        /// 保留
        /// </summary>
        public string Reserve
        {
            get { return _reserve; }
            set { _reserve = value; }
        }
        /// <summary>
        /// 数据长度
        /// </summary>
        public string DataLength
        {
            get { return _datalength; }
            set { _datalength = value; }
        }
        /// <summary>
        /// 数据（长度可变）
        /// </summary>
        public string DataInfo
        {
            get { return _datainfo; }
            set { _datainfo = value; }
        }

        #endregion

        #region -Method-
        public void ReadFramePack(byte[] data)
        {
            #region String
            //Version = Encoding.ASCII.GetString(data, 0, 1);
            //Flag = Encoding.ASCII.GetString(data, 0, 1);

            //MainType = Encoding.ASCII.GetString(data, 1, 1);
            //DataType = Encoding.ASCII.GetString(data, 2, 2);
            //SourceAddress = Encoding.ASCII.GetString(data, 4, 2);
            //DestinationAddress = Encoding.ASCII.GetString(data, 6, 2);
            //MissionCode = Encoding.ASCII.GetString(data, 8, 2);
            //SatelliteCode = Encoding.ASCII.GetString(data, 10, 2);
            //DataDate = Encoding.ASCII.GetString(data, 12, 4);
            //DataTime = Encoding.ASCII.GetString(data, 16, 4);
            //SequenceNumber = Encoding.ASCII.GetString(data, 20, 2);
            //ChildrenPackNumber = Encoding.ASCII.GetString(data, 22, 1);
            //Reserve = Encoding.ASCII.GetString(data, 23, 1);
            //DataLength = Encoding.ASCII.GetString(data, 24, 4);
            //DataInfo = Encoding.ASCII.GetString(data, 28, 4);

            #endregion

            Version = (data[0] & 0x0F).ToString("X2");
            Flag = (data[0] & 0xF0).ToString("X2");

            MainType = GetBytesData(data,1,1);
            DataType = GetBytesData(data,2,2);
            SourceAddress = GetBytesData(data,4,2);
            DestinationAddress = GetBytesData(data,6,2);
            MissionCode = GetBytesData(data,8,2);
            SatelliteCode = GetBytesData(data,10,2);
            DataDate = GetBytesData(data,12,4);
            DataTime = GetBytesData(data,16,4);
            SequenceNumber = GetBytesData(data,20,2);
            ChildrenPackNumber = GetBytesData(data,22,1);
            Reserve = GetBytesData(data,23,1);
            DataLength = GetBytesData(data,24,4);
            DataInfo = GetBytesData(data,28,data.Length -28); //长度是可变的，用总长减去前面的定长

        }

        /// <summary>
        /// 返回指定从指定位置开始长度为length的数组值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="start">起始位置</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        string GetBytesData(byte[] data, int start, int length)
        {
            if (start > data.Length || (start + length) > data.Length)
            {
                return null;
            }
            string result=string.Empty;
            for (int i = 0; i < length; i++)
            {
                result += data[start + i].ToString("X2");
            }
            return result;
        }

        public void ReadGD()
        {
            GD objData = new GD()
            {
                /*Version = this.Version,
                Flag = this.Flag,
                MainType =this.MainType,
                DataType =this.DataType,
                SourceAddress =this.SourceAddress,
                DestinationAddress =this.DestinationAddress,
                MissionCode = this.MissionCode,
                SatelliteCode = this.SatelliteCode,
                DataDate = Convert.ToDateTime( this.DataDate),
                DataTime = this.DataTime,
                SequenceNumber = this.SequenceNumber,
                ChildrenPackNumber = this.ChildrenPackNumber,
                UDPReserve = this.Reserve,
                DataLength = this.DataLength,
                DataClass ="GD",
                Reserve = "",*/

                D = InitialDate.AddDays( Convert.ToInt32( this.DataInfo.Substring(0, 2),2)).ToShortDateString(),
                T = Convert.ToUInt32( this.DataInfo.Substring(2, 4),2).ToString(),
                A = Convert.ToUInt32( this.DataInfo.Substring(6, 4),2).ToString(),
                E = Convert.ToUInt32( this.DataInfo.Substring(10, 4),2).ToString(),
                I = Convert.ToUInt32( this.DataInfo.Substring(14, 4),2).ToString(),
                Q = Convert.ToUInt32( this.DataInfo.Substring(18, 4),2).ToString(),
                W = Convert.ToUInt32( this.DataInfo.Substring(22, 4),2).ToString(),
                M = Convert.ToUInt32( this.DataInfo.Substring(26, 4),2).ToString(),
                P = GetNumberFromComplement( this.DataInfo.Substring(30, 4) ),
                DELTP = GetNumberFromComplement( this.DataInfo.Substring(34, 4)),
                Ra = Convert.ToUInt32( this.DataInfo.Substring(38, 4),2).ToString(),
                Rp = Convert.ToUInt32( this.DataInfo.Substring(42, 4),2).ToString()
            };

            objData.Add();
        }

        public void ReadGDSH()
        {
            GDSH objData = new GDSH()
            {
                Version = this.Version,
                Flag = this.Flag,
                MainType = this.MainType,
                DataType = this.DataType,
                SourceAddress = this.SourceAddress,
                DestinationAddress = this.DestinationAddress,
                MissionCode = this.MissionCode,
                SatelliteCode = this.SatelliteCode,
                DataDate = Convert.ToDateTime(this.DataDate),
                DataTime = this.DataTime,
                SequenceNumber = this.SequenceNumber,
                ChildrenPackNumber = this.ChildrenPackNumber,
                UDPReserve = this.Reserve,
                DataLength = this.DataLength,
                DataClass = "GDSH",
                Reserve = "",

                D = InitialDate.AddDays(Convert.ToInt32(this.DataInfo.Substring(0, 2), 2)).ToShortDateString(),
                T = Convert.ToUInt32(this.DataInfo.Substring(2, 4), 2).ToString(),
                A = Convert.ToUInt32(this.DataInfo.Substring(6, 4), 2).ToString(),
                E = Convert.ToUInt32(this.DataInfo.Substring(10, 4), 2).ToString(),
                I = Convert.ToUInt32(this.DataInfo.Substring(14, 4), 2).ToString(),
                Ohm = Convert.ToUInt32(this.DataInfo.Substring(18, 4), 2).ToString(),
                Omega = Convert.ToUInt32(this.DataInfo.Substring(22, 4), 2).ToString(),
                M = Convert.ToUInt32(this.DataInfo.Substring(26, 4), 2).ToString(),
                CDSM = Convert.ToUInt32( this.DataInfo.Substring(30, 3),2).ToString(),
                KSM = Convert.ToUInt32(this.DataInfo.Substring(33, 3),2).ToString(),
                KZ1 = this.DataInfo.Substring(36, 4),
                KZ2 = this.DataInfo.Substring(40, 4)
            };



            objData.Add();
        }

        public void ReadT0()
        {
            T0 objData = new T0()
            {
                Version = this.Version,
                Flag = this.Flag,
                MainType = this.MainType,
                DataType = this.DataType,
                SourceAddress = this.SourceAddress,
                DestinationAddress = this.DestinationAddress,
                MissionCode = this.MissionCode,
                SatelliteCode = this.SatelliteCode,
                DataDate = Convert.ToDateTime(this.DataDate),
                DataTime = this.DataTime,
                SequenceNumber = this.SequenceNumber,
                ChildrenPackNumber = this.ChildrenPackNumber,
                UDPReserve = this.Reserve,
                DataLength = this.DataLength,
                DataClass = "T0",
                Reserve = "",

                TZero = GetNumberFromComplement( this.DataInfo.Substring(0,4))
            };

            objData.Add();
        }

        public void ReadXDSC()
        {
            XDSC objData = new XDSC()
            {
                Version = this.Version,
                Flag = this.Flag,
                MainType = this.MainType,
                DataType = this.DataType,
                SourceAddress = this.SourceAddress,
                DestinationAddress = this.DestinationAddress,
                MissionCode = this.MissionCode,
                SatelliteCode = this.SatelliteCode,
                DataDate = Convert.ToDateTime(this.DataDate),
                DataTime = this.DataTime,
                SequenceNumber = this.SequenceNumber,
                ChildrenPackNumber = this.ChildrenPackNumber,
                UDPReserve = this.Reserve,
                DataLength = this.DataLength,
                DataClass = "XDSC",
                Reserve = "",

                D = InitialDate.AddDays(Convert.ToInt32(this.DataInfo.Substring(0, 2), 2)).ToShortDateString(),
                T = GetNumberFromComplement( this.DataInfo.Substring(2, 4)),
                N = GetNumberFromComplement( this.DataInfo.Substring(6, 2)),
                DealtT = GetNumberFromComplement( this.DataInfo.Substring(8, 4))
            };

            objData.Add();
        }

        public void ReadR()
        {
            R objData = new R()
            {
                Version = this.Version,
                Flag = this.Flag,
                MainType = this.MainType,
                DataType = this.DataType,
                SourceAddress = this.SourceAddress,
                DestinationAddress = this.DestinationAddress,
                MissionCode = this.MissionCode,
                SatelliteCode = this.SatelliteCode,
                DataDate = Convert.ToDateTime(this.DataDate),
                DataTime = this.DataTime,
                SequenceNumber = this.SequenceNumber,
                ChildrenPackNumber = this.ChildrenPackNumber,
                UDPReserve = this.Reserve,
                DataLength = this.DataLength,
                DataClass = "R",
                Reserve = "",

                ZT = Convert.ToInt32( this.DataInfo.Substring(0, 2),2).ToString(),
                T = Convert.ToUInt32( this.DataInfo.Substring(2, 4),2).ToString(),
                Re = Convert.ToUInt32( this.DataInfo.Substring(6, 8),2).ToString(),
                DeltaT = GetNumberFromComplement( this.DataInfo.Substring(14, 8)),
                SPhi = GetNumberFromComplement( this.DataInfo.Substring(22, 2))
            };

            objData.Add();
        }

        public void ReadRR()
        {
            RR objData = new RR()
            {
                Version = this.Version,
                Flag = this.Flag,
                MainType = this.MainType,
                DataType = this.DataType,
                SourceAddress = this.SourceAddress,
                DestinationAddress = this.DestinationAddress,
                MissionCode = this.MissionCode,
                SatelliteCode = this.SatelliteCode,
                DataDate = Convert.ToDateTime(this.DataDate),
                DataTime = this.DataTime,
                SequenceNumber = this.SequenceNumber,
                ChildrenPackNumber = this.ChildrenPackNumber,
                UDPReserve = this.Reserve,
                DataLength = this.DataLength,
                DataClass = "RR",
                Reserve = "",

                ZT = Convert.ToInt32(this.DataInfo.Substring(0, 2), 2).ToString(),
                T = Convert.ToUInt32(this.DataInfo.Substring(2, 4), 2).ToString(),
                RR0 = GetNumberFromComplement( this.DataInfo.Substring(6, 4)),
                DeltaF = GetNumberFromComplement( this.DataInfo.Substring(10, 4)),
                SPhi = GetNumberFromComplement( this.DataInfo.Substring(14, 2))
            };

            objData.Add();
        }

        public void ReadAE()
        {
            AE objData = new AE()
            {
                Version = this.Version,
                Flag = this.Flag,
                MainType = this.MainType,
                DataType = this.DataType,
                SourceAddress = this.SourceAddress,
                DestinationAddress = this.DestinationAddress,
                MissionCode = this.MissionCode,
                SatelliteCode = this.SatelliteCode,
                DataDate = Convert.ToDateTime(this.DataDate),
                DataTime = this.DataTime,
                SequenceNumber = this.SequenceNumber,
                ChildrenPackNumber = this.ChildrenPackNumber,
                UDPReserve = this.Reserve,
                DataLength = this.DataLength,
                DataClass = "AE",
                Reserve = "",

                ZT = Convert.ToInt32(this.DataInfo.Substring(0, 2), 2).ToString(),
                T = Convert.ToUInt32(this.DataInfo.Substring(2, 4), 2).ToString(),
                A =  Convert.ToUInt32( this.DataInfo.Substring(6, 4),2).ToString(),
                E = GetNumberFromComplement( this.DataInfo.Substring(10, 2)),
                DeltaA1 = GetNumberFromComplement( this.DataInfo.Substring(12, 2)),
                DeltaE1 = GetNumberFromComplement( this.DataInfo.Substring(14, 2)),
                DeltaA2 = GetNumberFromComplement( this.DataInfo.Substring(16, 2)),
                DeltaE2 = GetNumberFromComplement( this.DataInfo.Substring(18, 2)),
                SPhi = GetNumberFromComplement( this.DataInfo.Substring(20, 2))
            };

            objData.Add();
        }

        /// <summary>
        /// 得到补码
        /// </summary>
        /// <returns></returns>
        public string GetComplement(string input)
        {
            string result = string.Empty; ;
            UInt32 temp;
            temp = Convert.ToUInt32(input, 2);
            temp = ~temp + 1;
            result = Convert.ToString(temp, 2);
            return result.Substring(result.Length-input.Length);
        }

        /// <summary>
        /// 把补码形式转为原数字字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetNumberFromComplement(string input)
        {
            Int32 temp;
            temp = Convert.ToInt32(input, 2);
            temp = ~(temp - 1);
            //result = Convert.ToString(temp, 2);
            return temp.ToString();
        }

        #endregion
    }
}
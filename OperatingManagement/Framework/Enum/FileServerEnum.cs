using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Framework
{
    /// <summary>
    /// 发送状态
    /// </summary>
    public enum SendStatuss : int
    {
        /// <summary>
        /// 生成失败
        /// </summary>
        GenerateFailed = -1,
        /// <summary>
        /// 已提交发送
        /// </summary>
        Submitted = 0,
        /// <summary>
        /// 正在发送中
        /// </summary>
        Sending = 1,
        /// <summary>
        /// 发送成功
        /// </summary>
        Success = 2,
        /// <summary>
        /// 发送失败
        /// </summary>
        Failed = 3
    }

    /// <summary>
    /// 发送接收方式
    /// </summary>
    public enum CommunicationWays : int
    {
        /// <summary>
        /// 未知，适用于解析接收到的文件出错的情况
        /// </summary>
        Unknow = -1,
        /// <summary>
        /// FTP协议
        /// </summary>
        FTP = 0,
        /// <summary>
        /// 基于UDP的FEP协议
        /// </summary>
        FEPwithUDP = 1,
        /// <summary>
        /// 基于TCP的FEP协议
        /// </summary>
        FEPwithTCP = 2,
        /// <summary>
        /// 数据帧
        /// </summary>
        DataFrame = 9
    }

    /// <summary>
    /// 接收状态
    /// </summary>
    public enum ReceiveStatuss : int
    {
        /// <summary>
        /// 已开始接收
        /// </summary>
        HasReceive = 0,
        /// <summary>
        /// 接收中
        /// </summary>
        Receiving = 1,
        /// <summary>
        /// 接收成功
        /// </summary>
        ReceiveSuccesss = 2,
        /// <summary>
        /// 接收失败
        /// </summary>
        Failed = 3,
        /// <summary>
        /// 待处理
        /// </summary>
        ToProcess = 4,
        /// <summary>
        /// 处理成功
        /// </summary>
        ProcessSuccess =5,
        /// <summary>
        /// 无法识别
        /// </summary>
        UnRecognized = 6,
        /// <summary>
        /// 处理失败
        /// </summary>
        ProcessFailed= 7
    }

    /// <summary>
    /// 信息类型
    /// </summary>
    public enum InfoTypes : int
    {
        /// <summary>
        /// 数据帧
        /// </summary>
        DataFrame = 9,
        /// <summary>
        /// 文件
        /// </summary>
        File = 0
    }

    /// <summary>
    /// 是否自动重发
    /// </summary>
    public enum IFAutoReSend : int
    {
        /// <summary>
        /// 自动重发
        /// </summary>
        AutoReSend = 0,
        /// <summary>
        /// 不自动重发
        /// </summary>
        NotAutoReSend = 1
    }
}

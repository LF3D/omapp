using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.DataAccessLayer.PlanManage
{
    //总zhuang DMZ 工作计划
    public class ZZGZJH
    {
        #region -Properties-
        public int ID { get; set; }
        public DateTime CTime { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string TaskID { get; set; }
        public string InfoType { get; set; }
        public int LineCount { get; set; }
        public string Format1 { get; set; }
        public string Format2 { get; set; }
        public string DataSection { get; set; }
        public string FileIndex { get; set; }
        public string Reserve { get; set; }

        public string SatID { get; set; }
        public string Sequence { get; set; }
        public string DateTime { get; set; }
        public string StationName { get; set; }
        public string EquipmentID { get; set; }
        public string TaskCount { get; set; }

        public List<DMJH_Task> DMJHTasks { get; set; }
        #endregion
    }

    [Serializable]
    public class DMJH_Task
    {
        #region -Properties-
        /// <summary>
        /// 任wu标志
        /// </summary>
        public string TaskFlag { get; set; }
        /// <summary>
        /// 工作方式
        /// </summary>
        public string WorkWay { get; set; }
        /// <summary>
        /// 计划性质
        /// </summary>
        public string PlanPropertiy { get; set; }
        /// <summary>
        /// 工作模式
        /// </summary>
        public string WorkMode { get; set; }
        /// <summary>
        /// 任wu准备开始时间
        /// </summary>
        public string PreStartTime { get; set; }
        /// <summary>
        /// 任wu开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 跟踪开始时间
        /// </summary>
        public string TrackStartTime { get; set; }
        /// <summary>
        /// 开上xing载bo时间
        /// </summary>
        public string WaveOnStartTime { get; set; }
        /// <summary>
        /// 关上xing载bo时间
        /// </summary>
        public string WaveOffStartTime { get; set; }
        /// <summary>
        /// 跟踪结束时间
        /// </summary>
        public string TrackEndTime { get; set; }
        /// <summary>
        /// 任wu结束时间
        /// </summary>
        public string EndTime { get; set; }

        public List<DMJH_Task_ReakTimeTransfor> ReakTimeTransfors { get; set; }
        public List<DMJH_Task_AfterFeedBack> AfterFeedBacks { get; set; }
        #endregion
    }

    /// <summary>
    /// DMZ 工作计划- RealTime 传输
    /// </summary>
    [Serializable]
    public class DMJH_Task_ReakTimeTransfor
    {
        #region -Properties-
        /// <summary>
        /// 格式标志
        /// </summary>
        public string FormatFlag { get; set; }
        /// <summary>
        /// 信息流标志
        /// </summary>
        public string InfoFlowFlag { get; set; }
        /// <summary>
        ///  DataTransfer 开始时间
        /// </summary>
        public string TransStartTime { get; set; }
        /// <summary>
        ///  DataTransfer 结束时间
        /// </summary>
        public string TransEndTime { get; set; }
        /// <summary>
        ///  DataTransfer 速率
        /// </summary>
        public string TransSpeedRate { get; set; }

        #endregion
    }

    /// <summary>
    /// DMZ 工作计划- shi后 回放
    /// </summary>
    [Serializable]
    public class DMJH_Task_AfterFeedBack
    {
        #region -Properties-
        /// <summary>
        /// 格式标志
        /// </summary>
        public string FormatFlag { get; set; }
        /// <summary>
        /// 信息流标志
        /// </summary>
        public string InfoFlowFlag { get; set; }
        /// <summary>
        /// 数据起始时间
        /// </summary>
        public string DataStartTime { get; set; }
        /// <summary>
        /// 数据结束时间
        /// </summary>
        public string DataEndTime { get; set; }
        /// <summary>
        ///  DataTransfer 开始时间
        /// </summary>
        public string TransStartTime { get; set; }
        /// <summary>
        ///  DataTransfer 速率
        /// </summary>
        public string TransSpeedRate { get; set; }

        #endregion
    }
}

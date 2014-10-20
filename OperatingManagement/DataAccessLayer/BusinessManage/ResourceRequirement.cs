#region
//------------------------------------------------------
//Assembly:OperatingManagement.DataAccessLayer
//FileName:ResourceRequirement.cs
//Remark:资源需求实体对象类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120211    Create     
//------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.DataAccessLayer.BusinessManage
{
    [Serializable]
    public class ResourceRequirement
    {
        #region Properties
        /// <summary>
        /// 需求名称
        /// </summary>
        public string RequirementName { get; set; }
        /// <summary>
        /// 时间基准
        /// </summary>
        public string TimeBenchmark { get; set; }
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// 卫星编码
        /// </summary>
        public string WXBM { get; set; }
        /// <summary>
        /// 功能类型
        /// </summary>
        public string FunctionType { get; set; }
        /// <summary>
        /// 不可用设备
        /// </summary>
        public List<UnusedEquipment> UnusedEquipmentList { get; set; }
        /// <summary>
        /// 持续时长/秒
        /// </summary>
        public int PersistenceTime{ get; set; }
        /// <summary>
        /// 支持时段
        /// </summary>
        public List<PeriodOfTime> PeriodOfTimeList { get; set; }
        /// <summary>
        /// 卫星序号,与卫星编码组成需求名称,格式为:卫星编码+卫星序号,其中卫星序号三位数字如001,073,102
        /// </summary>
        public int WXBMIndex { get; set; }
        #endregion

        #region -Public Method-

        public static string GenerateResourceCalculateXML(DateTime timeBenchmark, List<ResourceRequirement> resourceRequirementList)
        {
            StringBuilder strBuilder = new StringBuilder("");
            if (resourceRequirementList != null && resourceRequirementList.Count > 0)
            {
                strBuilder.Append("<?xml version=\"1.0\"?>");
                strBuilder.Append("<!--注释区，需要时在此对文件内容进行说明。-->");
                strBuilder.Append("<资源需求>");
                strBuilder.Append("<需求个数>" + resourceRequirementList.Count.ToString() + "</需求个数>");
                strBuilder.Append("<时间基准>" + timeBenchmark.ToString("yyyyMMddHHmmss") + "</时间基准>");
                foreach (ResourceRequirement resourceRequirement in resourceRequirementList)
                {
                    strBuilder.Append("<需求>");

                    strBuilder.Append("<需求名称>" + resourceRequirement.RequirementName + "</需求名称>");
                    strBuilder.Append("<需求优先级>" + resourceRequirement.Priority.ToString() + "</需求优先级>");
                    strBuilder.Append("<卫星编码>" + resourceRequirement.WXBM + "</卫星编码>");
                    strBuilder.Append("<功能类型>" + resourceRequirement.FunctionType + "</功能类型>");

                    strBuilder.Append("<不可用设备>");
                    strBuilder.Append("<个数>" + resourceRequirement.UnusedEquipmentList.Count.ToString() + "</个数>");
                    foreach (UnusedEquipment unUsedEquipment in resourceRequirement.UnusedEquipmentList)
                    {
                        strBuilder.Append("<设备描述>");
                        strBuilder.Append("<地面站编码>" + unUsedEquipment.GRCode + "</地面站编码>");
                        strBuilder.Append("<地面站设备编码>" + unUsedEquipment.EquipmentCode + "</地面站设备编码>");
                        strBuilder.Append("</设备描述>");
                    }
                    strBuilder.Append("</不可用设备>");

                    strBuilder.Append("<持续时长>" + resourceRequirement.PersistenceTime.ToString() + "</持续时长>");

                    strBuilder.Append("<支持时段>");
                    strBuilder.Append("<个数>" + resourceRequirement.PeriodOfTimeList.Count.ToString() + "</个数>");
                    foreach (PeriodOfTime periodOfTime in resourceRequirement.PeriodOfTimeList)
                    {
                        strBuilder.Append("<时段描述>");
                        strBuilder.Append("<开始时间>" + periodOfTime.BeginTime.ToString("yyyyMMddHHmmss") + "</开始时间>");
                        strBuilder.Append("<结束时间>" + periodOfTime.EndTime.ToString("yyyyMMddHHmmss") + "</结束时间>");
                        strBuilder.Append("</时段描述>");
                    }
                    strBuilder.Append("</支持时段>");

                    strBuilder.Append("</需求>");
                }
                strBuilder.Append("</资源需求>");
            }
            return strBuilder.ToString();
        }

        #endregion
    }

    [Serializable]
    public class UnusedEquipment
    {
        /// <summary>
        /// 地面站编号
        /// </summary>
        public string GRCode { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }
    }

    [Serializable]
    public class PeriodOfTime
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }

}

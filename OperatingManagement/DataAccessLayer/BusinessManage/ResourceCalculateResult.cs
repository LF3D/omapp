#region
//------------------------------------------------------
//Assembly:OperatingManagement.DataAccessLayer
//FileName:ResourceCalculateResult.cs
//Remark:资源计算结果实体对象类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120304    Create     
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
    public class ResourceCalculateResult
    {
        #region Properties
        /// <summary>
        /// 总需求数
        /// </summary>
        public int RequirementNumber { get; set; }
        /// <summary>
        /// 完成需求数
        /// </summary>
        public int CompleteRequirementNumber { get; set; }
        /// <summary>
        /// 总得分
        /// </summary>
        public double TotalScore { get; set; }
        /// <summary>
        /// 优先级原则得分
        /// </summary>
        public double PriorityScore { get; set; }
        /// <summary>
        /// 效能原则得分
        /// </summary>
        public double EfficiencyScore { get; set; }
        /// <summary>
        /// 集中原则得分
        /// </summary>
        public double FocusScore { get; set; }
        /// <summary>
        /// 地面站均衡原则得分
        /// </summary>
        public double GroundStationProportionScore { get; set; }
        /// <summary>
        /// 卫星均衡原则得分
        /// </summary>
        public double SatelliteProportionScore { get; set; }
        /// <summary>
        /// 需求列表
        /// </summary>
        public List<Requirement> RequirementList { get; set; }
        #endregion

        #region -Public Method-

        public static ResourceCalculateResult GenerateResourceCalculateResultList(XmlDocument xmlDocument, out string message)
        {
            message = string.Empty;
            ResourceCalculateResult resourceCalculateResult = null;
            List<Requirement> requirementList = new List<Requirement>();

            XmlNode rootNode = xmlDocument.SelectSingleNode(@"//排班结果");
            if (rootNode == null)
            {
                message = "结果文件格式错误！";
                return null;
            }

            resourceCalculateResult = new ResourceCalculateResult();
            int requirementNumber = 0;
            int completeRequirementNumber = 0;
            double totalScore = 0.0;
            double priorityScore = 0.0;
            double efficiencyScore = 0.0;
            double focusScore = 0.0;
            double groundStationProportionScore = 0.0;
            double satelliteProportionScore = 0.0;
            if (!int.TryParse(rootNode.Attributes["总需求数"].Value, out requirementNumber))
            {
                message = "结果文件总需求数格式错误！";
                return null;
            }
            if (!int.TryParse(rootNode.Attributes["完成需求数"].Value, out completeRequirementNumber))
            {
                message = "结果文件完成需求数格式错误！";
                return null;
            }
            if (!double.TryParse(rootNode.Attributes["总得分"].Value, out totalScore))
            {
                message = "结果文件总得分格式错误！";
                return null;
            }
            if (!double.TryParse(rootNode.Attributes["优先级原则得分"].Value, out priorityScore))
            {
                message = "结果文件优先级原则得分格式错误！";
                return null;
            }
            if (!double.TryParse(rootNode.Attributes["效能原则得分"].Value, out efficiencyScore))
            {
                message = "结果文件效能原则得分格式错误！";
                return null;
            }
            if (!double.TryParse(rootNode.Attributes["集中原则得分"].Value, out focusScore))
            {
                message = "结果文件集中原则得分格式错误！";
                return null;
            }
            if (!double.TryParse(rootNode.Attributes["地面站均衡原则得分"].Value, out groundStationProportionScore))
            {
                message = "结果文件地面站均衡原则得分格式错误！";
                return null;
            }
            if (!double.TryParse(rootNode.Attributes["卫星均衡原则得分"].Value, out satelliteProportionScore))
            {
                message = "结果文件卫星均衡原则得分格式错误！";
                return null;
            }
            resourceCalculateResult.RequirementNumber = requirementNumber;
            resourceCalculateResult.CompleteRequirementNumber = completeRequirementNumber;
            resourceCalculateResult.TotalScore = totalScore;
            resourceCalculateResult.PriorityScore = priorityScore;
            resourceCalculateResult.EfficiencyScore = efficiencyScore;
            resourceCalculateResult.FocusScore = focusScore;
            resourceCalculateResult.GroundStationProportionScore = groundStationProportionScore;
            resourceCalculateResult.SatelliteProportionScore = satelliteProportionScore;

            XmlNodeList requirementNodeList = rootNode.SelectNodes(@"//排班结果/需求");
            if (requirementNodeList == null || requirementNodeList.Count < 1)
            {
                message = "结果文件格式错误！";
                return null;
            }
            foreach (XmlNode requirementNode in requirementNodeList)
            {
                Requirement requirement = new Requirement();
                requirement.RequirementName = requirementNode.Attributes["需求名"].Value;
                requirement.UseSatellite = requirementNode.Attributes["使用卫星"].Value;
                List<SatelliteGroundPhaseInfo> satelliteGroundPhaseInfoList = new List<SatelliteGroundPhaseInfo>();

                if (!requirementNode.HasChildNodes)
                {
                    message = "结果文件格式错误！";
                    return null;
                }
                foreach (XmlNode satelliteGroundPhaseInfoNode in requirementNode.ChildNodes)
                {
                    SatelliteGroundPhaseInfo satelliteGroundPhaseInfo = new SatelliteGroundPhaseInfo();
                    int overHeadCondition = 0;
                    if (!int.TryParse(satelliteGroundPhaseInfoNode.Attributes["过顶情况"].Value, out overHeadCondition))
                    {
                        message = "结果文件过顶情况格式错误！";
                        return null;
                    }
                    satelliteGroundPhaseInfo.PhaseName = satelliteGroundPhaseInfoNode.Attributes["阶段名"].Value;
                    satelliteGroundPhaseInfo.FunctionType = satelliteGroundPhaseInfoNode.Attributes["功能类型"].Value;
                    satelliteGroundPhaseInfo.UseGroundStation = satelliteGroundPhaseInfoNode.Attributes["使用地面站"].Value;
                    satelliteGroundPhaseInfo.UseEquipment = satelliteGroundPhaseInfoNode.Attributes["使用设备"].Value;
                    satelliteGroundPhaseInfo.OverHeadCondition = overHeadCondition;
                    satelliteGroundPhaseInfo.BeginTime = satelliteGroundPhaseInfoNode.Attributes["开始时间"].Value;
                    satelliteGroundPhaseInfo.EndTime = satelliteGroundPhaseInfoNode.Attributes["结束时间"].Value;
                    satelliteGroundPhaseInfoList.Add(satelliteGroundPhaseInfo);
                }
                requirement.SatelliteGroundPhaseInfoList = satelliteGroundPhaseInfoList;
                requirementList.Add(requirement);
            }
            resourceCalculateResult.RequirementList = requirementList;

            return resourceCalculateResult;
        }

        #endregion
    }

    [Serializable]
    public class Requirement
    {
        #region Properties
        /// <summary>
        /// 需求名
        /// </summary>
        public string RequirementName { get; set; }
        /// <summary>
        /// 使用卫星
        /// </summary>
        public string UseSatellite { get; set; }
        /// <summary>
        /// 星地阶段列表
        /// </summary>
        public List<SatelliteGroundPhaseInfo> SatelliteGroundPhaseInfoList { get; set; }
        #endregion
    }

    [Serializable]
    public class SatelliteGroundPhaseInfo
    {
        #region Properties
        /// <summary>
        /// 阶段名
        /// </summary>
        public string PhaseName { get; set; }
        /// <summary>
        /// 功能类型
        /// </summary>
        public string FunctionType { get; set; }
        /// <summary>
        /// 使用地面站
        /// </summary>
        public string UseGroundStation { get; set; }
        /// <summary>
        /// 使用设备
        /// </summary>
        public string UseEquipment { get; set; }
        /// <summary>
        /// 过顶情况
        /// </summary>
        public int OverHeadCondition { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        #endregion
    }
}

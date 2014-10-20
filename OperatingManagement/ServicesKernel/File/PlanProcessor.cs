using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Configuration;
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.Framework.Storage;
using OperatingManagement.Framework.Components;
using ServicesKernel.GDFX;

namespace OperatingManagement.ServicesKernel.File
{
    public class PlanProcessor
    {
        #region Variant Declare

        private char[] splitor_hline = new char[] { '-' };
        private char[] splitor_underline = new char[] { '_' };
        private Dictionary<string, string[]> dicParticipator = new Dictionary<string, string[]>();
        private Dictionary<string, TaskTime> dicTaskTimes = new Dictionary<string, TaskTime>();
        private Dictionary<string, Action> dicActions = new Dictionary<string, Action>();
        private Dictionary<string, string> dicZYSQ_GZDY = new Dictionary<string, string>();
        private Dictionary<string, string> dicZYSQ_SB = new Dictionary<string, string>();
        private Dictionary<string, string> dicSCIDs = new Dictionary<string, string>();

        #endregion

        #region 试验CX 转 中心YX计划
        /// <summary>
        /// 试验CX文件转ZX计划
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="JLX">计划类型，周计划ZJ，日计划RJ</param>
        /// <param name="beginTime">计划开始日期</param>
        /// <param name="endTime">计划结束日期</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public ZXJH SYCXFile2ZXJH(string fileFullName, string JLX
            , DateTime beginTime, DateTime endTime, out string result)
        {
            result = string.Empty;
            string strResult = string.Empty;
            string strTmp = string.Empty;
            ZXJH oJH = new ZXJH();
            XElement root = LoadXmlDoc(fileFullName, out result);
            if (result.Equals(string.Empty))
            {
                var eps = root.Elements(PEDefinition.E_ExperimentProcedure);
                for (int i = 0; i < eps.Count(); i++)
                {
                    SYCXElement2ZXJH(eps.ElementAt(i), ref oJH, JLX, beginTime, endTime, out strResult);

                    if (!strResult.Equals(string.Empty))
                    {
                        result += string.Format("ID=\"{0}\"的试验程序生成中心运行计划时出现问题：{1}<br>"
                            , GetAttributeValue(PEDefinition.P_ID, eps.ElementAt(i), out strTmp)
                            , strResult);
                    }
                }
            }
            //if (result.Equals(string.Empty))
            return oJH;
            //else
            //    return null;
        }

        /// <summary>
        /// 一个试验CX转ZX计划列表
        /// </summary>
        /// <param name="noe"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private void SYCXElement2ZXJH(XElement node, ref ZXJH oJH, string JLX
            , DateTime fromTime, DateTime toTime, out string result)
        {
            result = string.Empty;
            string strSYID = string.Empty;
            string strTmp = string.Empty;
            string strDMZID = string.Empty;
            string strDMZDeviceID = string.Empty;

            #region Read all important data
            //Get BeginTime & EndTime
            DateTime from = GetElementDTValue(PEDefinition.P_BeginTime, node, out result);
            if (!result.Equals(string.Empty))
                return;
            DateTime to = GetElementDTValue(PEDefinition.P_EndTime, node, out result);
            if (!result.Equals(string.Empty))
                return;
            //时间段不在目标范围内
            if (fromTime > to || toTime < from)
            {
                result = string.Format("没有符合{0}~{1}的试验程序", fromTime, toTime);
                return;
            }

            //Get SatIDs from Participators
            string satids = GetSatIDsFromParticipators(node, out result);
            if (!result.Equals(string.Empty))
                return;

            //获取试验时间
            GetTaskTimes(node, out result);
            if (!result.Equals(string.Empty))
                return;

            //获取所有动作
            GetAllActions(node, out result);
            if (!result.Equals(string.Empty))
                return;
            #endregion

            DateTime beginTime = from;
            DateTime endTime;
            int iTmp = 0;
            //试验计划区域
            oJH.SatID = satids;
            oJH.Date = fromTime.ToString(PEDefinition.ShortTimeFormat);
            oJH.SYCount = "1";
            strTmp = GetAttributeValue(PEDefinition.P_Code, node, out result);
            if (!result.Equals(string.Empty))
                return;
            if (strTmp.Split(splitor_hline).Length < 4)
            {
                result = string.Format("编码{0}格式不符", strTmp);//应为TP-TaskID-No
                return;
            }
            oJH.TaskID = strTmp.Split(splitor_hline)[1];

            #region Get SYContent For JH
            if (oJH.SYContents == null)
                oJH.SYContents = new List<ZXJH_SYContent>();
            strSYID = (oJH.SYContents.Count() + 1).ToString("0000");
            #region 试验
            ZXJH_SYContent oSYContent = new ZXJH_SYContent();
            oSYContent.SatID = satids;
            oSYContent.SYID = strSYID;

            List<PlanParameter> lstSYXMDef = PlanParameters.ReadParameters("SYXMDef");
            List<PlanParameter> lstParam = lstSYXMDef.Where(t=>t.Value == strTmp.Split(splitor_hline)[2].Substring(0, 3)).ToList();
            if (lstParam.Count > 0)
                oSYContent.SYName = lstParam[0].Text;//试验项目——>试验.试验项目名称
            //有疑问，待确认，这个时间应该是谁的？？
            oSYContent.SYStartTime = from.ToString(PEDefinition.LongTimeFormat14);
            oSYContent.SYEndTime = to.ToString(PEDefinition.LongTimeFormat14);
            endTime = to;
            oSYContent.SYDays = Math.Ceiling((to - from).TotalDays).ToString();
            #endregion

            #region 数chuan
            oSYContent.SCList = new List<ZXJH_SYContent_SC>();
            List<Action> lstSCActions = GetSomeDMZActions(PEDefinition.V_SC, fromTime, toTime, out result);
            if (!result.Equals(string.Empty))
                return;
            ZXJH_SYContent_SC oSC;
            for (int i = 0; i < lstSCActions.Count(); i++)
            {
                oSC = new ZXJH_SYContent_SC();
                if (lstSCActions.ElementAt(i).WorkingParams.ContainsKey(PEDefinition.V_PDXZ))
                    oSC.SY_SCFrequencyBand = lstSCActions.ElementAt(i).WorkingParams[PEDefinition.V_PDXZ].Value;
                oSC.SY_SCLaps = lstSCActions.ElementAt(i).QC.ToString();
                oSC.SY_SCStartTime = lstSCActions.ElementAt(i).BeginTime.ToString(PEDefinition.LongTimeFormat14);
                oSC.SY_SCEndTime = lstSCActions.ElementAt(i).EndTime.ToString(PEDefinition.LongTimeFormat14);

                //执行者编号为系统编号或是子系统编号需要单独处理
                strTmp = lstSCActions.ElementAt(i).ParticipatorCode;
                strDMZID = GetDMZID(strTmp, out strDMZDeviceID);
                if (string.IsNullOrEmpty(strDMZDeviceID))
                {
                    ZXJH_SYContent_SC oNSc;
                    if (dicParticipator.ContainsKey(strDMZID))
                    {
                        for (int j = 0; j < dicParticipator[strDMZID].Length; j++)
                        {
                            oNSc = (ZXJH_SYContent_SC)oSC.Clone();
                            oNSc.SY_SCStationNO = strDMZID;
                            oNSc.SY_SCEquipmentNO = dicParticipator[strDMZID][j];
                            oSYContent.SCList.Add(oNSc);
                        }
                    }
                    //没有这个key怎么办？
                }
                else
                {
                    oSC.SY_SCStationNO = strDMZID;
                    oSC.SY_SCEquipmentNO = strDMZDeviceID;
                    oSYContent.SCList.Add(oSC);
                }
                //else 执行者编号不合法？
            }
            #endregion

            #region 测kong
            oSYContent.CKList = new List<ZXJH_SYContent_CK>();
            List<Action> lstCKActions = GetSomeDMZActions(PEDefinition.V_YC, fromTime, toTime, out result);
            if (result.Equals(string.Empty))
            {
                ZXJH_SYContent_CK oCK;
                for (int i = 0; i < lstCKActions.Count(); i++)
                {
                    oCK = new ZXJH_SYContent_CK();
                    oCK.SY_CKLaps = lstCKActions.ElementAt(i).QC.ToString();
                    oCK.SY_CKStartTime = lstCKActions.ElementAt(i).BeginTime.ToString(PEDefinition.LongTimeFormat14);
                    oCK.SY_CKEndTime = lstCKActions.ElementAt(i).EndTime.ToString(PEDefinition.LongTimeFormat14);

                    //执行者编号为系统编号或是子系统编号需要单独处理
                    strTmp = lstSCActions.ElementAt(i).ParticipatorCode;
                    strDMZID = GetDMZID(strTmp, out strDMZDeviceID);
                    if (string.IsNullOrEmpty(strDMZDeviceID))
                    {
                        ZXJH_SYContent_CK oNCK;
                        if (dicParticipator.ContainsKey(strDMZID))
                        {
                            for (int j = 0; j < dicParticipator[strDMZID].Length; j++)
                            {
                                oNCK = (ZXJH_SYContent_CK)oCK.Clone();
                                oNCK.SY_CKStationNO = strDMZID;
                                oNCK.SY_CKEquipmentNO = dicParticipator[strDMZID][j];
                                oSYContent.CKList.Add(oNCK);
                            }
                        }
                        //没有这个key怎么办？
                    }
                    else
                    {
                        oCK.SY_CKStationNO = strDMZID;
                        oCK.SY_CKEquipmentNO = strDMZDeviceID;
                        oSYContent.CKList.Add(oCK);
                    }
                    //else 执行者编号不合法？
                }
            }

            #endregion

            #region 注shu
            List<Action> lstSXYKActions = new List<Action>();
            oSYContent.ZSList = new List<ZXJH_SYContent_ZS>();
            lstSXYKActions = GetSomeDMZActions(PEDefinition.V_SXYK, fromTime, toTime, out result);
            if (result.Equals(string.Empty))
            {
                ZXJH_SYContent_ZS oZS;

                for (int i = 0; i < lstSXYKActions.Count(); i++)
                {
                    oZS = new ZXJH_SYContent_ZS();
                    oZS.SY_ZSFirst = lstSXYKActions.ElementAt(i).BeginTime.ToString(PEDefinition.LongTimeFormat14);
                    oZS.SY_ZSLast = lstSXYKActions.ElementAt(i).EndTime.ToString(PEDefinition.LongTimeFormat14);
                    oZS.SY_ZSContent = lstSXYKActions.ElementAt(i).Name;
                    oSYContent.ZSList.Add(oZS);
                }
            }
            #endregion

            oJH.SYContents.Add(oSYContent);
            #endregion

            #region Get TaskManage For JH
            if (oJH.WorkContents == null)
                oJH.WorkContents = new List<ZXJH_WorkContent>();
            //SY规划
            ZXJH_WorkContent oWKContent = new ZXJH_WorkContent();
            oWKContent.SYID = strSYID;
            oWKContent.Work = PEDefinition.W_SYGH;
            oWKContent.StartTime = beginTime.AddDays(-1).ToString(PEDefinition.LongTimeFormat14);
            oWKContent.MinTime = endTime.ToString(PEDefinition.LongTimeFormat14);
            oWKContent.MaxTime = endTime.AddHours(2).ToString(PEDefinition.LongTimeFormat14);
            oJH.WorkContents.Add(oWKContent);
            //计划管理
            ZXJH_WorkContent oNContent = new ZXJH_WorkContent();
            oNContent = (ZXJH_WorkContent)oWKContent.Clone();
            oNContent.Work = PEDefinition.W_JHGL;
            oJH.WorkContents.Add(oNContent);
            //试验数据处理
            oNContent = new ZXJH_WorkContent();
            oNContent = (ZXJH_WorkContent)oWKContent.Clone();
            oNContent.Work = PEDefinition.W_SYSJCL;
            if (ConfigurationManager.AppSettings["SYSJCLLastMinTime"] != null)
                iTmp = Convert.ToInt32(ConfigurationManager.AppSettings["SYSJCLLastMinTime"]);
            oNContent.MinTime = endTime.AddHours(iTmp).ToString(PEDefinition.LongTimeFormat14);
            if (ConfigurationManager.AppSettings["SYSJCLLastMaxTime"] != null)
                iTmp = Convert.ToInt32(ConfigurationManager.AppSettings["SYSJCLLastMaxTime"]);
            oNContent.MaxTime = endTime.AddHours(iTmp).ToString(PEDefinition.LongTimeFormat14);
            oJH.WorkContents.Add(oNContent);
            #endregion

            //指令Make
            oJH.CommandMakes = new List<ZXJH_CommandMake>();

            #region Get SYDataHandle For JH--实时试验数据处理
            if (oJH.SYDataHandles == null)
                oJH.SYDataHandles = new List<ZXJH_SYDataHandle>();
            ZXJH_SYDataHandle oDataHandle = new ZXJH_SYDataHandle();

            #region 先SC、SC+CK，再CK
            List<ZXJH_SYDataHandle> lstHandles = new List<ZXJH_SYDataHandle>();
            strTmp = string.Empty;
            //SC、SC+CK
            if (lstSCActions != null)
            {
                foreach (Action act in lstSCActions)
                {
                    oDataHandle = new ZXJH_SYDataHandle();
                    oDataHandle.SYID = strSYID;
                    oDataHandle.SatID = satids;
                    oDataHandle.Laps = act.QC.ToString();
                    oDataHandle.StartTime = act.BeginTime.ToString(PEDefinition.LongTimeFormat14);
                    oDataHandle.EndTime = act.EndTime.ToString(PEDefinition.LongTimeFormat14);

                    if (lstCKActions != null)
                    {
                        var oCKActions = lstCKActions.Where(o => o.QC == act.QC
                            && o.ParticipatorCode == act.ParticipatorCode);
                        if (oCKActions != null && oCKActions.Count() > 0)
                        {
                            oDataHandle.Content = PEDefinition.V_SC + "," + PEDefinition.V_YC;
                            strTmp += act.ParticipatorCode + "_" + oDataHandle.Laps + "|";
                        }
                        else//没有CK的
                            oDataHandle.Content = PEDefinition.V_SC;
                    }
                    else//没有CK的
                        oDataHandle.Content = PEDefinition.V_SC;
                    oDataHandle.MainStation = act.ParticipatorCode;
                    lstHandles.Add(oDataHandle);

                }
            }
            //only CK
            if (lstCKActions != null)
            {
                foreach (Action act in lstCKActions)
                {
                    if (strTmp.IndexOf(act.ParticipatorCode + "_" + act.QC.ToString()) < 0)
                    {
                        oDataHandle = new ZXJH_SYDataHandle();
                        oDataHandle.SYID = strSYID;
                        oDataHandle.SatID = satids;
                        oDataHandle.Laps = act.QC.ToString();
                        oDataHandle.Content = PEDefinition.V_YC;
                        oDataHandle.MainStation = act.ParticipatorCode;
                        oDataHandle.StartTime = act.BeginTime.ToString(PEDefinition.LongTimeFormat14);
                        oDataHandle.EndTime = act.EndTime.ToString(PEDefinition.LongTimeFormat14);
                        lstHandles.Add(oDataHandle);
                    }
                }
            }
            #endregion

            //再处理执行者
            ZXJH_SYDataHandle oNHandle;
            for (int m = 0; m < lstHandles.Count(); m++)
            {
                strTmp = GetDMZID(lstHandles.ElementAt(m).MainStation, out strDMZDeviceID);
                //执行者是子系统须单独处理
                if (!string.IsNullOrEmpty(strDMZDeviceID))
                {
                    oDataHandle.MainStation = strTmp;
                    oDataHandle.MainStationEquipment = strDMZDeviceID;
                    oJH.SYDataHandles.Add(lstHandles.ElementAt(m));
                }
                else
                {
                    if (dicParticipator.ContainsKey(lstHandles.ElementAt(m).MainStation))
                    {
                        for (int k = 0; k < dicParticipator[lstHandles.ElementAt(m).MainStation].Length; k++)
                        {
                            oNHandle = new ZXJH_SYDataHandle();
                            oNHandle = (ZXJH_SYDataHandle)lstHandles.ElementAt(m).Clone();
                            oDataHandle.MainStation = strTmp;
                            oNHandle.MainStationEquipment = dicParticipator[lstHandles.ElementAt(m).MainStation][k];
                            oJH.SYDataHandles.Add(oNHandle);
                        }
                    }
                    //lstHandles.RemoveAt(m);
                }
            }
            //oJH.SYDataHandles.AddRange(lstHandles);
            #endregion

            //指挥JS
            if (oJH.DirectAndMonitors == null)
                oJH.DirectAndMonitors = new List<ZXJH_DirectAndMonitor>();

            //SS控制
            if (oJH.RealTimeControls == null)
                oJH.RealTimeControls = new List<ZXJH_RealTimeControl>();

            //处理PG
            if (oJH.SYEstimates == null)
                oJH.SYEstimates = new List<ZXJH_SYEstimate>();
        }
        #endregion

        #region 试验CX 转 ZC DMZ 工作计划，只有SC动作

        /// <summary>
        /// 试验CX文件转DMZ工作计划（ZC），一个试验CX生成一个DMZ计划
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="xxfl">周计划ZJ，日计划RJ</param>
        /// <param name="beginTime">计划开始日期</param>
        /// <param name="endTime">计划结束日期</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<GZJH> SYCXFile2ZCDMZGZJHs(string fileFullName, string xxfl
            , DateTime fromTime, DateTime toTime, out string result)
        {
            result = string.Empty;
            List<GZJH> lstGZJHs = new List<GZJH>();
            XElement root = LoadXmlDoc(fileFullName, out result);
            if (!result.Equals(string.Empty))
                return null;

            string[] strZCDMZs = GetDMZList(1, out result);
            if (!result.Equals(string.Empty))
                return null;

            string strResult = string.Empty;
            string strTmp = string.Empty;
            GZJH oJH = new GZJH();
            var eps = root.Elements(PEDefinition.E_ExperimentProcedure);
            for (int i = 0; i < eps.Count(); i++)
            {
                oJH = SYCXElement2ZCDMZGZJH(eps.ElementAt(i), xxfl, fromTime, toTime, strZCDMZs, out strResult);
                if (oJH != null)
                    lstGZJHs.Add(oJH);

                if (!strResult.Equals(string.Empty))
                {
                    result += string.Format("ID=\"{0}\"的试验程序生成地面站计划时出现问题：{1}<br>"
                        , GetAttributeValue(PEDefinition.P_ID, eps.ElementAt(i), out strTmp)
                        , strResult);
                }
            }

            //if (result.Equals(string.Empty))
            return lstGZJHs;
            //else
            //    return null;
        }

        /// <summary>
        /// 一个试验CX元素转一个DMZ工作计划（ZC）
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xxfl">周计划ZJ，日计划RJ</param>
        /// <param name="beginTime">计划开始日期</param>
        /// <param name="endTime">计划结束日期</param>
        /// <param name="result"></param>
        /// <returns></returns>
        private GZJH SYCXElement2ZCDMZGZJH(XElement node, string xxfl
            , DateTime fromTime, DateTime toTime, string[] ZCDMZList, out string result)
        {
            result = string.Empty;
            #region Read all important data
            //Get BeginTime & EndTime
            DateTime from = GetElementDTValue(PEDefinition.P_BeginTime, node, out result);
            if (!result.Equals(string.Empty))
                return null;
            DateTime to = GetElementDTValue(PEDefinition.P_EndTime, node, out result);
            if (!result.Equals(string.Empty))
                return null;
            //时间段不在目标范围内
            if (fromTime > to || toTime < from)
            {
                result = string.Format("没有符合{0}~{1}的试验程序", fromTime, toTime);
                return null;
            }

            //Get SatIDs from Participators
            string satids = GetSatIDsFromParticipators(node, out result);
            if (!result.Equals(string.Empty))
                return null;

            //获取试验时间
            GetTaskTimes(node, out result);
            if (!result.Equals(string.Empty))
                return null;

            //获取所有动作
            GetAllActions(node, out result);
            if (!result.Equals(string.Empty))
                return null;
            #endregion

            List<Action> lstActions = GetSomeDMZActions(PEDefinition.V_SC, fromTime, toTime, out result);
            if (!result.Equals(string.Empty))
                return null;

            GZJH oJH = new GZJH();
            //oJH.JXH，存储前需生成
            oJH.XXFL = xxfl;
            List<GZJH_Content> lstContents = new List<GZJH_Content>();
            GZJH_Content oContent = new GZJH_Content();
            string strFS = string.Empty;
            string strJXZ = string.Empty;
            string strDH = string.Empty;
            DMZ oDMZ = new DMZ();
            Task oTask = new Task();
            if (lstActions != null && lstActions.Count() > 0)
            {
                #region Get WorkingPattern, WorkingQuality, TaskID
                //工作方式
                strFS = GetElementValue(PEDefinition.E_WorkingPattern, node, out result);
                if (!result.Equals(string.Empty))
                    return null;
                //计划性质
                strJXZ = GetElementValue(PEDefinition.E_WorkingQuality, node, out result);
                if (!result.Equals(string.Empty))
                    return null;
                //任务代号
                strDH = GetAttributeValue(PEDefinition.P_Code, node, out result);
                if (!result.Equals(string.Empty))
                    return null;
                strDH = strDH.Split(splitor_hline)[1];

                oJH.TaskID = strDH;
                oJH.CTime = DateTime.Now;
                oJH.SatID = "AAAA";

                #endregion
                string strPCode = string.Empty;
                string strDeviceID = string.Empty;
                string strSCID = string.Empty;

                #region Set Content Value
                for (int i = 0; i < lstActions.Count(); i++)
                {
                    oContent = new GZJH_Content();
                    //工作单位 & 设备
                    strPCode = GetDMZID(lstActions[i].ParticipatorCode, out strDeviceID);
                    oContent.DW = strPCode;
                    if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_SCID))
                        strSCID = lstActions[i].WorkingParams[PEDefinition.V_SCID].Value;
                    if (string.IsNullOrEmpty(strSCID))
                        strSCID = "AAAA";
                    //属于总can的dmz才生成计划
                    if (ZCDMZList.Contains(oContent.DW))
                    {
                        oContent.FS = strFS;
                        oContent.JXZ = strJXZ;
                        oDMZ.DMZCode = strPCode;
                        oContent.DW = oDMZ.GetByCode().DWCode;
                        oContent.DH = oTask.GetOutTaskNo(strDH, strSCID);
                        oContent.QH = lstActions[i].QC.ToString();
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_MS))
                            oContent.MS = lstActions[i].WorkingParams[PEDefinition.V_MS].Value;
                        oContent.QB = PEDefinition.V_QB;//一般Q
                        oContent.GXZ = "M";//有疑问，待解决
                        oContent.RK = lstActions[i].BeginTime.ToString("yyyyMMddHHmmss") ;
                        oContent.GZK = lstActions[i].BeginTime.AddSeconds(-30).ToString("yyyyMMddHHmmss");
                        oContent.ZHB = lstActions[i].BeginTime.AddSeconds(-1830).ToString("yyyyMMddHHmmss");
                        oContent.KSHX = "FFFFFFFFFFFFFF";
                        oContent.GSHX = "FFFFFFFFFFFFFF";
                        //if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_KSHX))
                        //    oContent.KSHX = lstActions[i].WorkingParams[PEDefinition.V_KSHX].Value;
                        //if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_GSHX))
                        //    oContent.GSHX = lstActions[i].WorkingParams[PEDefinition.V_GSHX].Value;
                        oContent.JS = lstActions[i].EndTime.ToString("yyyyMMddHHmmss");
                        oContent.GZJ = lstActions[i].EndTime.AddSeconds(30).ToString("yyyyMMddHHmmss");
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_BID))
                            oContent.BID = lstActions[i].WorkingParams[PEDefinition.V_BID].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_SBZ))
                            oContent.SBZ = lstActions[i].WorkingParams[PEDefinition.V_SBZ].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_RTs))
                            oContent.RTs = lstActions[i].WorkingParams[PEDefinition.V_RTs].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_RTe))
                            oContent.RTe = lstActions[i].WorkingParams[PEDefinition.V_RTe].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_SL))
                            oContent.SL = lstActions[i].WorkingParams[PEDefinition.V_SL].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_HBZ))
                            oContent.HBZ = lstActions[i].WorkingParams[PEDefinition.V_HBZ].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_HBID))
                            oContent.HBID = lstActions[i].WorkingParams[PEDefinition.V_HBID].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_HRTs))
                            oContent.RTs = lstActions[i].WorkingParams[PEDefinition.V_HRTs].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_Ts))
                            oContent.Ts = lstActions[i].WorkingParams[PEDefinition.V_Ts].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_Te))
                            oContent.Te = lstActions[i].WorkingParams[PEDefinition.V_Te].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_HSL))
                            oContent.HSL = lstActions[i].WorkingParams[PEDefinition.V_HSL].Value;

                        //设备
                        if (!string.IsNullOrEmpty(strDeviceID))
                        {
                            oContent.SB = strDeviceID;
                            lstContents.Add(oContent);
                        }
                        else
                        {
                            GZJH_Content oNContent = null;
                            if (dicParticipator.ContainsKey(lstActions[i].ParticipatorCode))
                            {
                                for (int j = 0; j < dicParticipator[lstActions[i].ParticipatorCode].Length; j++)
                                {
                                    oNContent = (GZJH_Content)oContent.Clone();
                                    oNContent.SB = dicParticipator[lstActions[i].ParticipatorCode][j];
                                    lstContents.Add(oNContent);
                                }
                            }
                            else
                                lstContents.Add(oContent);
                        }
                    }
                }
                #endregion
            }
            //oJH.QS = lstContents.Count().ToString();
            oJH.GZJHContents = lstContents;
            return oJH;
        }
        #endregion

        #region 试验CX 转 CK 资源使用申请，SC\上行YK\SJCS\YCJS动作

        /// <summary>
        /// 试验CX文件转CK 资源使用申请，一个试验CX生成一个CK 资源使用申请
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="xxfl"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<DJZYSQ> SYCXFile2CKZYSYSQ(string fileFullName, string xxfl
            , DateTime fromTime, DateTime toTime, out string result)
        {
            result = string.Empty;
            string strResult = string.Empty;
            string strTmp = string.Empty;
            Dictionary<string, DJZYSQ> dicCKZYSQs = new Dictionary<string, DJZYSQ>();
            XElement root = LoadXmlDoc(fileFullName, out result);
            if (!result.Equals(string.Empty))
                return null;

            string[] strZZDMZs = GetDMZList(2, out result);
            if (!result.Equals(string.Empty))
                return null;

            List<DJZYSQ> LstSQs = new List<DJZYSQ>();
            var eps = root.Elements(PEDefinition.E_ExperimentProcedure);
            for (int i = 0; i < eps.Count(); i++)
            {
                LstSQs = SYCXElement2CKZYSYSQ(eps.ElementAt(i), xxfl, fromTime, toTime, strZZDMZs, out strResult);

                if (LstSQs != null)
                {
                    for (int j = 0; j < LstSQs.Count(); j++)
                    {
                        if (!dicCKZYSQs.ContainsKey(LstSQs[j].SatID))
                        {
                            dicCKZYSQs.Add(LstSQs[j].SatID, LstSQs[j]);
                        }
                        else
                        {
                            foreach (DJZYSQ_Task oTask in LstSQs[j].DMJHTasks)
                            {
                                dicCKZYSQs[LstSQs[j].SatID].DMJHTasks.Add(oTask);
                            }
                        }
                        dicCKZYSQs[LstSQs[j].SatID].SNUM = dicCKZYSQs[LstSQs[j].SatID].DMJHTasks.Count().ToString();
                    }
                }
                if (!string.IsNullOrEmpty(strResult))
                {
                    result += string.Format("ID=\"{0}\"的试验程序生成测控资源试验申请时出现问题：{1}<br>"
                        , GetAttributeValue(PEDefinition.P_ID, eps.ElementAt(i), out strTmp)
                        , strResult);
                }
            }

            //if (result.Equals(string.Empty))
            return dicCKZYSQs.Values.ToList();
            //else
            //    return null;
        }

        /// <summary>
        /// 一个试验CX元素转多个CK 资源使用申请（一个航天器对应一个），取SC\上行YK\SJCS\YCJS动作
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xxfl"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private List<DJZYSQ> SYCXElement2CKZYSYSQ(XElement node, string xxfl
            , DateTime fromTime, DateTime toTime, string[] ZZDMZList, out string result)
        {
            result = string.Empty;
            #region Read all important data
            //Get BeginTime & EndTime
            DateTime from = GetElementDTValue(PEDefinition.P_BeginTime, node, out result);
            if (!result.Equals(string.Empty))
                return null;
            DateTime to = GetElementDTValue(PEDefinition.P_EndTime, node, out result);
            if (!result.Equals(string.Empty))
                return null;
            //时间段不在目标范围内
            if (fromTime > to || toTime < from)
            {
                result = string.Format("没有符合{0}~{1}的试验程序", fromTime, toTime);
                return null;
            }

            //Get SatIDs from Participators
            string satids = GetSatIDsFromParticipators(node, out result);
            if (!result.Equals(string.Empty))
                return null;

            //获取试验时间
            GetTaskTimes(node, out result);
            if (!result.Equals(string.Empty))
                return null;

            //获取所有动作
            GetAllActions(node, out result);
            if (!result.Equals(string.Empty))
                return null;
            //载入工作单元和设备
            GetGZDYandSB();
            //读入htq标识
            GetSCIDs();
            #endregion

            List<string> lstFilter = new List<string>();
            lstFilter.Add(PEDefinition.V_SC);
            lstFilter.Add(PEDefinition.V_SXYK);
            lstFilter.Add(PEDefinition.V_YC);
            List<Action> lstActions = GetSomeDMZActions(PEDefinition.V_SC, fromTime, toTime, out result);
            if (!result.Equals(string.Empty))
                return null;

            Dictionary<string, DJZYSQ> dicSQs = new Dictionary<string, DJZYSQ>();
            DJZYSQ oSQ;
            DJZYSQ_Task oTask = new DJZYSQ_Task();
            string strFS = string.Empty;
            string strSXZ = string.Empty;
            string strDH = string.Empty;
            string strSCID = string.Empty;
            string strTmp = string.Empty;
            if (lstActions != null && lstActions.Count() > 0)
            {
                #region Get WorkingPattern, WorkingQuality, TaskID
                //工作方式
                strFS = GetElementValue(PEDefinition.E_WorkingPattern, node, out result);
                if (!result.Equals(string.Empty))
                    return null;
                //计划性质
                strSXZ = GetElementValue(PEDefinition.E_WorkingQuality, node, out result);
                if (!result.Equals(string.Empty))
                    return null;
                //任务代号
                strDH = GetAttributeValue(PEDefinition.P_Code, node, out result);
                if (!result.Equals(string.Empty))
                    return null;
                strDH = strDH.Split(splitor_hline)[1];
                #endregion
                string strPCode = string.Empty;
                string strPName = string.Empty;
                string strDeviceID = string.Empty;
                DMZ oDMZ = new DMZ();

                #region Set Task Value
                for (int i = 0; i < lstActions.Count(); i++)
                {
                    oTask = new DJZYSQ_Task();
                    //工作单位 & 设备
                    strPCode = GetDMZID(lstActions[i].ParticipatorCode, out strDeviceID);
                    //属于总zhuang的动作才生成计划
                    if (ZZDMZList.Contains(strPCode))
                    {
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_SCID))
                            strSCID = lstActions[i].WorkingParams[PEDefinition.V_SCID].Value;
                        if (dicSQs.ContainsKey(strSCID))
                            oSQ = dicSQs[strSCID];
                        else
                        {
                            oSQ = new DJZYSQ();
                            oSQ.TaskID = strDH;
                            oSQ.SJ = DateTime.Now.ToString(PEDefinition.LongTimeFormat14);
                            if (string.IsNullOrEmpty(strSCID))
                                oSQ.SatID = satids;
                            else
                                oSQ.SatID = strSCID;
                            oSQ.SCID = oSQ.SatID;
                        }
                        oDMZ.DMZCode = strPCode;
                        strPName = oDMZ.GetByCode().DMZName;
                        if (dicZYSQ_GZDY.ContainsKey(strPName))
                            oTask.GZDY = dicZYSQ_GZDY[strPName];
                        oTask.SXH = (i + 1).ToString();
                        oTask.SXZ = strSXZ;
                        oTask.MLB = "TT";
                        oTask.FS = strFS;
                        //strTmp = lstActions[i].ParticipatorCode.Split(splitor_hline)[1];
                        //oTask.GZDY = dicZYSQ_GZDY[strPCode];
                        oTask.SBDH = strDeviceID;
                        oTask.QC = lstActions[i].QC.ToString();
                        oTask.QB = PEDefinition.V_QB;//默认一般Q
                        if (lstActions[i].Name == PEDefinition.V_SC)
                            oTask.SHJ = "5";//CK事件类型，5为SC
                        else if (lstActions[i].Name == PEDefinition.V_SXYK)
                            oTask.SHJ = "2";//CK事件类型，2为SXYK
                        else if (lstActions[i].Name == PEDefinition.V_YC)
                            oTask.SHJ = "4";//CK事件类型，2为状态监视(遥测)
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_SHJ))
                            oTask.SHJ = lstActions[i].WorkingParams[PEDefinition.V_SHJ].Value;

                        var DPXZs = lstActions[i].WorkingParams.Values.Where(p => p.Code.Length >= 4
                            && p.Code.ToUpper().Substring(0, 4) == "DPXZ");
                        List<DJZYSQ_Task_GZDP> lstDPs = new List<DJZYSQ_Task_GZDP>();
                        if (DPXZs.Count() > 0)
                        {
                            DPXZs.OrderBy(p => p.Code);
                            DJZYSQ_Task_GZDP oDP;
                            for (int j = 0; j < DPXZs.Count(); j++)
                            {
                                oDP = new DJZYSQ_Task_GZDP();
                                oDP.FXH = (j + 1).ToString();// from 1 to .....
                                oDP.DPXZ = DPXZs.ElementAt(j).Value;
                                oDP.PDXZ = lstActions[i].WorkingParams["PDXZ" + DPXZs.ElementAt(j).Code.Substring(4)].Value;
                                lstDPs.Add(oDP);
                            }
                        }
                        oTask.GZDPs = lstDPs;
                        oTask.FNUM = lstDPs.Count().ToString();
                        oTask.TNUM = "1";//仅SC， so为1

                        oTask.RK = lstActions[i].BeginTime.ToString("yyyyMMddHHmmss");
                        oTask.GZK = lstActions[i].BeginTime.AddSeconds(-30).ToString("yyyyMMddHHmmss");
                        oTask.ZHB = lstActions[i].BeginTime.AddSeconds(-1830).ToString("yyyyMMddHHmmss");
                        oTask.KSHX = "FFFFFFFFFFFFFF";
                        oTask.GSHX = "FFFFFFFFFFFFFF";
                        oTask.JS = lstActions[i].EndTime.ToString("yyyyMMddHHmmss");
                        oTask.GZJ = lstActions[i].EndTime.AddSeconds(30).ToString("yyyyMMddHHmmss");

                        List<DJZYSQ_Task_ReakTimeTransfor> lstRtTrans = new List<DJZYSQ_Task_ReakTimeTransfor>();
                        DJZYSQ_Task_ReakTimeTransfor oRtTrans = new DJZYSQ_Task_ReakTimeTransfor();
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_BID))
                            oRtTrans.GBZ = lstActions[i].WorkingParams[PEDefinition.V_BID].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_SBZ))
                            oRtTrans.XBZ = lstActions[i].WorkingParams[PEDefinition.V_SBZ].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_RTs))
                            oRtTrans.RTs = lstActions[i].WorkingParams[PEDefinition.V_RTs].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_RTe))
                            oRtTrans.RTe = lstActions[i].WorkingParams[PEDefinition.V_RTe].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_SL))
                            oRtTrans.SL = lstActions[i].WorkingParams[PEDefinition.V_SL].Value;
                        lstRtTrans.Add(oRtTrans);
                        oTask.ReakTimeTransfors = lstRtTrans;

                        List<DJZYSQ_Task_AfterFeedBack> lstAfFbak = new List<DJZYSQ_Task_AfterFeedBack>();
                        DJZYSQ_Task_AfterFeedBack oAfFback = new DJZYSQ_Task_AfterFeedBack();
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_HBZ))
                            oAfFback.GBZ = lstActions[i].WorkingParams[PEDefinition.V_HBZ].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_HBID))
                            oAfFback.XBZ = lstActions[i].WorkingParams[PEDefinition.V_HBID].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_Ts))
                            oAfFback.Ts = lstActions[i].WorkingParams[PEDefinition.V_Ts].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_Te))
                            oAfFback.Te = lstActions[i].WorkingParams[PEDefinition.V_Te].Value;
                        if (lstActions[i].WorkingParams.ContainsKey(PEDefinition.V_HSL))
                            oAfFback.SL = lstActions[i].WorkingParams[PEDefinition.V_HSL].Value;
                        lstAfFbak.Add(oAfFback);
                        oTask.AfterFeedBacks = lstAfFbak;

                        if (oSQ.DMJHTasks == null)
                            oSQ.DMJHTasks = new List<DJZYSQ_Task>();
                        oSQ.DMJHTasks.Add(oTask);

                        if (!dicSQs.ContainsKey(oSQ.SatID))
                            dicSQs.Add(oSQ.SatID, oSQ);
                        else
                            dicSQs[oSQ.SatID] = oSQ;
                    }
                }
                #endregion
            }
            return dicSQs.Values.ToList();
        }
        #endregion

        #region 试验CX 转 YJJH

        /// <summary>
        /// 试验CX文件转YJJH，一个类试验项目生成一个YJJH
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="xxfl"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<YJJH> SYCXFile2YJJH(string fileFullName, string xxfl, DateTime fromTime, DateTime toTime, out string result)
        {
            result = string.Empty;
            string strResult = string.Empty;
            string strTmp = string.Empty;
            Dictionary<string, YJJH> dicYJJHs = new Dictionary<string, YJJH>();
            XElement root = LoadXmlDoc(fileFullName, out result);
            if (!result.Equals(string.Empty))
                return null;

            YJJH oYJJH = new YJJH();
            var eps = root.Elements(PEDefinition.E_ExperimentProcedure);
            for (int i = 0; i < eps.Count(); i++)
            {
                oYJJH = SYCXElement2YJJH(eps.ElementAt(i), xxfl, fromTime, toTime, out strResult);
                if (oYJJH != null)
                {
                    if (!dicYJJHs.ContainsKey(oYJJH.SatID))
                        dicYJJHs.Add(oYJJH.SatID, oYJJH);
                    else
                    {
                        foreach (YJJH_Task oTask in oYJJH.Tasks)
                        {
                            dicYJJHs[oYJJH.SatID].Tasks.Add(oTask);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(strResult))
                {
                    result += string.Format("ID=\"{0}\"的试验程序生成应用研究计划时出现问题：{1}<br>"
                        , GetAttributeValue(PEDefinition.P_ID, eps.ElementAt(i), out strTmp)
                        , strResult);
                }
            }

            return dicYJJHs.Values.ToList();
        }

        /// <summary>
        /// 从一个试验程序节点生成一个应用研究工作计划
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xxfl"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private YJJH SYCXElement2YJJH(XElement node, string xxfl, DateTime fromTime, DateTime toTime, out string result)
        {
            result = string.Empty;
            string strTmp = string.Empty;
            #region 检查时间段
            //Get BeginTime & EndTime
            DateTime from = GetElementDTValue(PEDefinition.P_BeginTime, node, out result);
            if (!result.Equals(string.Empty))
                return null;
            DateTime to = GetElementDTValue(PEDefinition.P_EndTime, node, out result);
            if (!result.Equals(string.Empty))
                return null;
            //时间段不在目标范围内
            if (fromTime > to || toTime < from)
            {
                result = string.Format("没有符合{0}~{1}的试验程序", fromTime, toTime);
                return null;
            }
            #endregion
            
            strTmp = GetAttributeValue(PEDefinition.P_Code, node, out result);
            if (!result.Equals(string.Empty))
                return null;
            if (strTmp.Split(splitor_hline).Length < 4)
            {
                result = string.Format("编码{0}格式不符", strTmp);//应为TP-TaskID-No
                return null;
            }

            YJJH oYJJH = new YJJH();
            oYJJH.CTime = DateTime.Now;
            oYJJH.SatID = "";//这个从哪里来
            List<PlanParameter> lstSYXMDef = PlanParameters.ReadParameters("SYXMDef");
            List<PlanParameter> lstParam = lstSYXMDef.Where(t=>t.Value == strTmp.Split(splitor_hline)[2].Substring(0, 3)).ToList();
            if (lstParam.Count > 0)
            {
                oYJJH.Tasks = new List<YJJH_Task>();
                YJJH_Task oTask = new YJJH_Task();
                oTask.StartTime = from.ToString("yyyyMMddHHmmss");
                oTask.EndTime = to.ToString("yyyyMMddHHmmss");
                oTask.Task = lstParam[0].Text;
                oYJJH.Tasks.Add(oTask);
            }

            lstSYXMDef = PlanParameters.ReadParameters("YJJHSendTargetMapping");
            lstParam = lstSYXMDef.Where(t=>t.Text == strTmp.Split(splitor_hline)[2].Substring(0, 3)).ToList();
            if (lstParam.Count > 0)
                oYJJH.SysName = lstParam[0].Value;

            oYJJH.TaskID = strTmp.Split(splitor_hline)[1];
            oYJJH.XXFL = xxfl;
            return oYJJH;
        }
        #endregion

        /// <summary>
        /// 载入试验CXxml文件，一个xml文件里可能有多个试验CX，一个试验CX里只有一个试验项目，一个试验CX可跨天
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private XElement LoadXmlDoc(string fileFullName, out string result)
        {
            result = string.Empty;
            XDocument doc = XDocument.Load(fileFullName);
            if (doc.Equals(null))
            {
                result = string.Format("无法解析XML文件，路径：", fileFullName);
                return null;
            }
            XElement root = doc.Root;
            return root;
        }

        /// <summary>
        /// 载入工作单元和设备号（For ZYSQ）
        /// </summary>
        private void GetGZDYandSB()
        {
            dicZYSQ_GZDY.Clear();
            dicZYSQ_SB.Clear();
            string[] strGZDYNames = PEDefinition.GZDYName.Split(new char[] { ',' });
            string[] strGZDYCodes = PEDefinition.GZDYCode.Split(new char[] { ',' });
            string[] strSBCodes = PEDefinition.SBDH.Split(new char[] { ',' });
            for (int i = 0; i < strGZDYCodes.Length; i++)
            {
                dicZYSQ_GZDY.Add(strGZDYNames[i], strGZDYCodes[i]);
                dicZYSQ_SB.Add(strGZDYNames[i], strSBCodes[i]);
            }
        }

        /// <summary>
        /// 获取HTQ标识（For ZYSQ）
        /// </summary>
        private void GetSCIDs()
        {
            dicSCIDs.Clear();
            string[] keys = PEDefinition.SCIDKeys.Split(new char[] { ',' });
            string[] scids = PEDefinition.SCIDs.Split(new char[] { ',' });
            for (int i = 0; i < keys.Length; i++)
            {
                dicSCIDs.Add(keys[i], scids[i]);
            }
        }

        /// <summary>
        /// 获取DMZ code 列表
        /// </summary>
        /// <param name="dmzType">1:ZC + JDZ; 2:ZZhuang</param>
        /// <returns></returns>
        private string[] GetDMZList(out string result)
        {
            string[] strDMZCodes = null;
            result = string.Empty;

            List<XYXSInfo> lstXyxs = new XYXSInfo().Cache;
            if (lstXyxs == null)
            {
                result = "无法获取总参地面站列表信息";
                return null;
            }
            lstXyxs = (List<XYXSInfo>)lstXyxs.Where(x => x.Type == 0);
            strDMZCodes = new string[lstXyxs.Count()];
            for (int i = 0; i < strDMZCodes.Length; i++)
            {
                strDMZCodes[i] = lstXyxs[i].ADDRMARK;
            }
            return strDMZCodes;
        }

        /// <summary>
        /// 获取DMZ code 列表
        /// </summary>
        /// <param name="dmzType">1:ZC + JDZ; 2:ZZhuang</param>
        /// <returns></returns>
        private string[] GetDMZList(int dmzType, out string result)
        {
            string[] strDMZCodes = null;
            result = string.Empty;
            List<DMZ> lstDMZ = new DMZ().Cache;
            if (lstDMZ == null)
            {
                result = "无法获取地面站列表信息";
                return null;
            }
            if (dmzType == 1)
            {
                var dmz = lstDMZ.Where(x => x.Owner == 1 || x.Owner == 3);

                strDMZCodes = new string[dmz.Count()];
                for (int i = 0; i < strDMZCodes.Length; i++)
                {
                    strDMZCodes[i] = dmz.ElementAt(i).DMZCode;
                }
            }
            else
            {
                var dmz = lstDMZ.Where(x => x.Owner == 1 || x.Owner == 3);

                strDMZCodes = new string[dmz.Count()];
                for (int i = 0; i < strDMZCodes.Length; i++)
                {
                    strDMZCodes[i] = dmz.ElementAt(i).DMZCode;
                }
            }
            #region old code, no use
            /*
            List<XYXSInfo> lstXyxs = new XYXSInfo().Cache;
            if (lstXyxs == null)
            {
                result = "无法获取地面站列表信息";
                return null;
            }
            if (dmzType == 1)
            {
                var xyxs = lstXyxs.Where(x => x.Type == 0 && (x.Own == "01" || x.Own == "03"));

                strDMZCodes = new string[xyxs.Count()];
                for (int i = 0; i < strDMZCodes.Length; i++)
                {
                    strDMZCodes[i] = xyxs.ElementAt(i).INCODE;
                }
            }
            else
            {
                var xyxs = lstXyxs.Where(x => x.Type == 0 && x.Own == "02");

                strDMZCodes = new string[xyxs.Count()];
                for (int i = 0; i < strDMZCodes.Length; i++)
                {
                    strDMZCodes[i] = xyxs.ElementAt(i).INCODE;
                }
            }*/
            #endregion
            return strDMZCodes;
        }

        #region 将StaionInOut文件加至现有计划
        /// <summary>
        /// 将StaionInOut文件加至ZC DMZ计划
        /// </summary>
        /// <param name="oJH">要加入的计划对象</param>
        /// <param name="SIOFileFullName">StaionInOut文件全路径</param>
        /// <param name="lines">指定的行号，英文逗号分隔，从小到大排列</param>
        /// <returns></returns>
        public string AddSIOtoZCDMZJH(ref GZJH oJH, string SIOFileFullName, string lines)
        {
            #region Check Params
            if (string.IsNullOrEmpty(SIOFileFullName))
                return "传入的路径为空";
            if (string.IsNullOrEmpty(lines))
                return "传入的行号为空";
            if (!System.IO.File.Exists(SIOFileFullName))
                return string.Format("找不到指定的文件{0}", SIOFileFullName);
            #endregion

            #region variant declare
            string[] lIDs = lines.Split(new char[] { ',' });
            string[] strLines = new string[lIDs.Length];
            int[] vPos = new int[] { 21, 54, 77, 169, 192, 0 };//值在文件行中的起始位置,QC\GZK\RK\RJ\GZJ\ZM
            int[] vLen = new int[] { 10, 21, 21, 21, 21, 10 };//值长度
            string strLine = string.Empty;
            string result = string.Empty;
            int iRow = 0;
            int iIdx = 0;
            bool blNewJH = false;
            XYXSInfo oXyxs = new XYXSInfo();
            DMZ oDMZ = new DMZ();
            #endregion

            #region ReadFile
            StreamReader oReader = new StreamReader(SIOFileFullName);
            try
            {
                strLine = oReader.ReadLine();//第一行标题
                iRow++;

                while (!string.IsNullOrEmpty(strLine))
                {
                    strLine = oReader.ReadLine();

                    if (lIDs.Contains(iRow.ToString()))
                    {
                        strLines[iIdx] = strLine;
                        iIdx++;
                    }
                    iRow++;
                }
                oReader.Close();
            }
            catch (Exception ex)
            {
                oReader.Close();
                return string.Format("AddSIOtoZCDMZJH读取StationInOut文件异常", ex.Message);
            }
            finally { }
            #endregion

            #region Set Time value
            if (oJH == null)
            {
                oJH = new GZJH();
                blNewJH = true;
                oJH.GZJHContents = new List<GZJH_Content>();
            }
            string strQC = string.Empty;
            string strRK = string.Empty;
            string strGZK = string.Empty;
            string strGZJ = string.Empty;
            string strRJ = string.Empty;
            string strZM = string.Empty;
            string strSB = string.Empty;
            GZJH_Content oContent;
            for (int i = 0; i < strLines.Length; i++)
            {
                strQC = strLines[i].Substring(vPos[0], vLen[0]).Trim();
                strGZK = strLines[i].Substring(vPos[1], vLen[1]).Replace(": ", ":0").Replace(" ", "").Replace(":", "").Substring(0, 14);
                //if (strGZK.Substring(0, 1).ToUpper() == "F")
                //    strGZK = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                strRK = strLines[i].Substring(vPos[2], vLen[2]);
                strRJ = strLines[i].Substring(vPos[3], vLen[3]).Replace(": ", ":0").Replace(" ", "").Replace(":", "").Substring(0, 14);
                //if (strRJ.Substring(0, 1).ToUpper() == "F")
                //    strRJ = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                strGZJ = strLines[i].Substring(vPos[4], vLen[4]).Replace(": ", ":0").Replace(" ", "").Replace(":", "").Substring(0, 14);
                //if (strGZJ.Substring(0, 1).ToUpper() == "F")
                //    strGZJ = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                strZM = strLines[i].Substring(vPos[5], vLen[5]).Trim();
                strSB = strZM.Substring(strZM.IndexOf('_') + 1);
                strZM = strZM.Substring(0, strZM.IndexOf('_'));
                oDMZ.DMZCode = strZM;
                oDMZ = oDMZ.GetByCode();
                if (oDMZ != null)
                    strZM = oDMZ.DWCode ;
                if (blNewJH)//为新计划读值
                {
                    oContent = new GZJH_Content();
                    oContent.DW = strZM;
                    oContent.SB = strSB;
                    oContent.QH = strQC;
                    //准备开始时间=开始时间-30分钟
                    if (strRK.Substring(0, 1).ToUpper() != "F")
                    {
                        oContent.ZHB = DateTime.Parse(strRK.Replace(": ", ":0")).AddMinutes(-30).ToString(PEDefinition.LongTimeFormat14);
                        oContent.RK = strRK.Replace(": ", ":0").Replace(" ", "").Replace(":", "").Substring(0, 14);
                    }
                    else
                    {
                        oContent.RK = strRK;
                        //oContent.ZHB = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                        //oContent.RK = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                    }
                    oContent.GZK = strGZK;
                    oContent.GZJ = strGZJ;
                    oContent.JS = strRJ;
                    oJH.GZJHContents.Add(oContent);
                }
                else
                {
                    if (oJH.GZJHContents != null)
                    {
                        for (int j = 0; j < oJH.GZJHContents.Count(); j++)
                        {
                            if (strQC == oJH.GZJHContents.ElementAt(j).QH)
                            {
                                if (strRK.Substring(0, 1).ToUpper() != "F")
                                {
                                    oJH.GZJHContents.ElementAt(j).ZHB = DateTime.Parse(strRK.Replace(": ", ":0")).AddMinutes(-30).ToString(PEDefinition.LongTimeFormat14);
                                    oJH.GZJHContents.ElementAt(j).RK = strRK.Replace(": ", ":0").Replace(" ", "").Replace(":", "").Substring(0, 14);
                                }
                                else
                                {
                                    oJH.GZJHContents.ElementAt(j).RK = strRK;
                                    //oJH.GZJHContents.ElementAt(j).ZHB = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                                    //oJH.GZJHContents.ElementAt(j).RK = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                                }
                                oJH.GZJHContents.ElementAt(j).DW = strZM;
                                oJH.GZJHContents.ElementAt(j).SB = strSB;
                                oJH.GZJHContents.ElementAt(j).GZK = strGZK;
                                oJH.GZJHContents.ElementAt(j).GZJ = strGZJ;
                                oJH.GZJHContents.ElementAt(j).JS = strRJ;
                            }
                        }
                    }
                }
            }
            #endregion
            return result;
        }

        /// <summary>
        /// 将StaionInOut文件加至ZYSQ
        /// </summary>
        /// <param name="oJH">要加入的计划对象</param>
        /// <param name="SIOFileFullName">StaionInOut文件全路径</param>
        /// <param name="lines">指定的行号，英文逗号分隔，从小到大排列</param>
        /// <returns></returns>
        public string AddSIOtoCKZYSQ(ref DJZYSQ oJH, string SIOFileFullName, string lines)
        {
            #region Check Params
            if (string.IsNullOrEmpty(SIOFileFullName))
                return "传入的路径为空";
            if (string.IsNullOrEmpty(lines))
                return "传入的行号为空";
            if (!System.IO.File.Exists(SIOFileFullName))
                return string.Format("找不到指定的文件{0}", SIOFileFullName);
            #endregion

            #region variant declare
            string[] lIDs = lines.Split(new char[] { ',' });
            string[] strLines = new string[lIDs.Length];
            int[] vPos = new int[] { 21, 54, 77, 169, 192, 0 };//值在文件行中的起始位置,QC\GZK\RK\RJ\GZJ\ZM
            int[] vLen = new int[] { 10, 21, 21, 21, 21, 10 };//值长度
            string strLine = string.Empty;
            string result = string.Empty;
            int iRow = 0;
            int iIdx = 0;
            bool blNewJH = false;
            #endregion

            #region ReadFile
            StreamReader oReader = new StreamReader(SIOFileFullName);
            try
            {
                strLine = oReader.ReadLine();//第一行标题
                iRow++;

                while (!string.IsNullOrEmpty(strLine))
                {
                    strLine = oReader.ReadLine();

                    if (lIDs.Contains(iRow.ToString()))
                    {
                        strLines[iIdx] = strLine;
                        iIdx++;
                    }
                    iRow++;
                }
                oReader.Close();
            }
            catch (Exception ex)
            {
                oReader.Close();
                return string.Format("AddSIOtoZCDMZJH读取StationInOut文件异常", ex.Message);
            }
            finally { }
            #endregion

            #region Set Time value
            if (oJH == null)
            {
                oJH = new DJZYSQ();
                blNewJH = true;
                oJH.DMJHTasks = new List<DJZYSQ_Task>();
            }
            string strQC = string.Empty;
            string strRK = string.Empty;
            string strGZK = string.Empty;
            string strGZJ = string.Empty;
            string strRJ = string.Empty;
            string strZM = string.Empty;
            string strSB = string.Empty;
            DJZYSQ_Task oContent;
            DMZ oDMZ = new DMZ();
            if (dicZYSQ_GZDY == null || dicZYSQ_GZDY.Count == 0)
                GetGZDYandSB();
            for (int i = 0; i < strLines.Length; i++)
            {
                strQC = strLines[i].Substring(vPos[0], vLen[0]).Trim();
                strGZK = strLines[i].Substring(vPos[1], vLen[1]).Replace(": ", ":0").Replace(" ", "").Replace(":", "").Substring(0, 14);
                //if (strGZK.Substring(0, 1).ToUpper() == "F")
                //    strGZK = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                strRK = strLines[i].Substring(vPos[2], vLen[2]);
                strRJ = strLines[i].Substring(vPos[3], vLen[3]).Replace(": ", ":0").Replace(" ", "").Replace(":", "").Substring(0, 14);
                //if (strRJ.Substring(0, 1).ToUpper() == "F")
                //    strRJ = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                strGZJ = strLines[i].Substring(vPos[4], vLen[4]).Replace(": ", ":0").Replace(" ", "").Replace(":", "").Substring(0, 14);
                //if (strGZJ.Substring(0, 1).ToUpper() == "F")
                //    strGZJ = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                strZM = strLines[i].Substring(vPos[5], vLen[5]).Trim();//地面站编码_设备编码
                strZM = strZM.Substring(0, strZM.IndexOf('_'));
                //strSB = strZM.Substring(strZM.IndexOf('_') + 1);
                oDMZ.DMZCode = strZM;
                strZM = oDMZ.GetByCode().DMZName;
                strSB = dicZYSQ_SB[strZM];
                strZM = dicZYSQ_GZDY[strZM];
                if (blNewJH)//为新计划读值
                {
                    oContent = new DJZYSQ_Task();
                    oContent.GZDY = strZM;
                    oContent.SBDH = strSB;
                    oContent.QC = strQC;
                    //ZHB = RK - 30min
                    if (strRK.Substring(0, 1).ToUpper() != "F")
                    {
                        oContent.ZHB = DateTime.Parse(strRK.Replace(": ", ":0")).AddMinutes(-30).ToString(PEDefinition.LongTimeFormat14);
                        oContent.RK = strRK.Replace(": ", ":0").Replace(" ", "").Replace(":", "").Substring(0, 14);
                    }
                    else
                    {
                        oContent.RK = strRK;
                        //oContent.ZHB = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                        //oContent.RK = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                    }
                    oContent.GZK = strGZK;
                    oContent.GZJ = strGZJ;
                    oContent.JS = strRJ;
                    oJH.DMJHTasks.Add(oContent);
                }
                else
                {
                    if (oJH.DMJHTasks != null)
                    {
                        for (int j = 0; j < oJH.DMJHTasks.Count(); j++)
                        {
                            if (strQC == oJH.DMJHTasks.ElementAt(j).QC)
                            {
                                oJH.DMJHTasks.ElementAt(j).GZDY = strZM;
                                oJH.DMJHTasks.ElementAt(j).SBDH = strSB;
                                if (strRK.Substring(0, 1).ToUpper() != "F")
                                {
                                    oJH.DMJHTasks.ElementAt(j).ZHB = DateTime.Parse(strRK.Replace(": ", ":0")).AddMinutes(-30).ToString(PEDefinition.LongTimeFormat14);
                                    oJH.DMJHTasks.ElementAt(j).RK = strRK.Replace(": ", ":0").Replace(" ", "").Replace(":", "").Substring(0, 14);
                                }
                                else
                                {
                                    oJH.DMJHTasks.ElementAt(j).RK = strRK;
                                    //oJH.DMJHTasks.ElementAt(j).ZHB = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                                    //oJH.DMJHTasks.ElementAt(j).RK = DateTime.MinValue.ToString(PEDefinition.LongTimeFormat14);
                                }
                                oJH.DMJHTasks.ElementAt(j).GZK = strGZK;
                                oJH.DMJHTasks.ElementAt(j).GZJ = strGZJ;
                                oJH.DMJHTasks.ElementAt(j).JS = strRJ;
                            }
                        }
                    }
                }
            }
            #endregion
            return result;
        }
        #endregion

        #region
        /// <summary>
        /// 测kong资源使用计划转换总zhuang DMZ 工作计划
        /// 读取源文件，生成系列目标文件，返回生成的文件名s
        /// </summary>
        /// <param name="ZYJHFileFullName"></param>
        /// <param name="ZZDMJHFileNames"></param>
        /// <param name="taskNO">生成文件名时使用</param>
        /// <returns>为empty表示成功，否则为失败提示信息</returns>
        public string ZYJH2ZZDMGZJHs(string ZYJHFileFullName, out string[] ZZDMJHFileNames, string taskNO)
        {
            string result = string.Empty;
            ZZDMJHFileNames = new string[0];
            XDocument srcDoc = XDocument.Load(ZYJHFileFullName);
            XElement root = srcDoc.Root;
            string strPlanID = string.Empty;
            string strTime = DateTime.Now.ToString(PEDefinition.LongTimeFormat14);
            var plans = root.Element("计划").Elements("计划内容");
            XElement jh;
            foreach (XElement xe in plans)
            {
                result = ZYJHElement2DMGZJH(xe, out jh);
            }
            return result;
        }

        private string ZYJHElement2DMGZJH(XElement node, out XElement jh)
        {
            string result = string.Empty;
            string strTmp = string.Empty;
            string strEName = "答复标志";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            if (strTmp.ToUpper() == "F")//申请无法满足
            {
                jh = null;
                return result;
            }

            string strPlanID = string.Empty;
            string strTime = DateTime.Now.ToString(PEDefinition.LongTimeFormat14);
            strEName = "计划序号";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }

            jh = new XElement("DMZ工作计划GZJH");
            jh.Add(new XElement("编号", strTmp),
                new XElement("时间", strTime));

            strEName = "任务类别";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            jh.Add(new XElement("任务标志", strTmp));

            strEName = "工作方式";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            jh.Add(new XElement(strEName, strTmp));

            strEName = "计划性质";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            jh.Add(new XElement(strEName, strTmp));

            strEName = "计划性质";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            jh.Add(new XElement(strEName, strTmp));

            strEName = "工作模式";//找不到数据源
            jh.Add(new XElement(strEName, "B"));

            strEName = "任务准备开始时间";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            jh.Add(new XElement(strEName, strTmp));

            strEName = "任务开始时间";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            jh.Add(new XElement(strEName, strTmp));

            strEName = "跟踪开始时间";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            jh.Add(new XElement(strEName, strTmp));

            strEName = "开上行载波时间";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            jh.Add(new XElement(strEName, strTmp));

            strEName = "关上行载波时间";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            jh.Add(new XElement(strEName, strTmp));

            strEName = "跟踪结束时间";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            jh.Add(new XElement(strEName, strTmp));

            strEName = "任务结束时间";
            strTmp = GetElementValue(strEName, node, out result);
            if (!result.Equals(string.Empty))
            {
                jh = null;
                return result;
            }
            jh.Add(new XElement(strEName, strTmp));

            return result;
        }
        #endregion

        #region Get Participators, TaskTimes , Actions
        /// <summary>
        /// 从参与者列表中获取涉及的WX编号","分隔，并取得所有参与者
        /// </summary>
        /// <param name="epNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetSatIDsFromParticipators(XElement epNode, out string result)
        {
            result = string.Empty;
            dicParticipator.Clear();
            string satids = string.Empty;
            string strCode = string.Empty;
            string strDMZid = string.Empty;
            string strDeviceID = string.Empty;
            string[] strSubSysCodes;
            var participator = epNode.Element(PEDefinition.E_ParticipatorDeclare).Elements(PEDefinition.E_System);
            for (int i = 0; i < participator.Count(); i++)
            {
                strCode = GetAttributeValue(PEDefinition.P_Code, participator.ElementAt(i), out result);
                if (!result.Equals(string.Empty))
                    return null;
                if (strCode.Substring(0, 1).ToUpper() == "S")//Satellite
                {
                    satids += strCode.Split(splitor_hline)[1] + ",";
                }

                var subSyss = participator.ElementAt(i).Element(PEDefinition.E_SubSysList).Elements(PEDefinition.E_SubSys);
                if (strCode.Substring(0, 5).ToUpper() == "G-CKZ")//地面站到组件
                {
                    for (int j = 0; j < subSyss.Count(); j++)
                    {
                        strCode = GetAttributeValue(PEDefinition.P_Code, subSyss.ElementAt(j), out result);
                        strDMZid = GetDMZID(strCode, out strDeviceID);
                        var cops = subSyss.ElementAt(j).Element(PEDefinition.E_ComponentList).Elements(PEDefinition.E_Component);
                        strSubSysCodes = new string[cops.Count()];
                        for (int k = 0; k < cops.Count(); k++)
                        {
                            strCode = GetAttributeValue(PEDefinition.P_Code, cops.ElementAt(k), out result);
                            strSubSysCodes[k] = strCode.Split(splitor_hline)[3];
                        }
                        dicParticipator.Add(strDMZid, strSubSysCodes);
                    }
                }
                else
                {
                    strSubSysCodes = new string[subSyss.Count()];
                    for (int j = 0; j < subSyss.Count(); j++)
                    {
                        strSubSysCodes[j] = GetAttributeValue(PEDefinition.P_Code, subSyss.ElementAt(j), out result);
                        if (strSubSysCodes[j].Split(splitor_hline).Length == 3)
                            strSubSysCodes[j] = strSubSysCodes[j].Split(splitor_hline)[2];
                    }
                    dicParticipator.Add(strCode, strSubSysCodes);
                }
            }
            satids = satids.TrimEnd(new char[] { ',' });
            if (satids.IndexOf(',') >= 0)
                satids = "AAAA";
            return satids;
        }

        /// <summary>
        /// 获取试验时间
        /// </summary>
        /// <param name="epNode"></param>
        /// <param name="result"></param>
        private void GetTaskTimes(XElement epNode, out string result)
        {
            result = string.Empty;
            TaskTime oTTime;
            var ttimes = epNode.Element(PEDefinition.E_ExperimentTime).Elements(PEDefinition.E_TaskTime);
            dicTaskTimes.Clear();
            for (int i = 0; i < ttimes.Count(); i++)
            {
                oTTime = new TaskTime();
                oTTime.Name = GetElementValue(PEDefinition.P_Name, ttimes.ElementAt(i), out result);
                if (!result.Equals(string.Empty))
                    return;
                oTTime.Mark = GetElementValue(PEDefinition.E_Symbol, ttimes.ElementAt(i), out result);
                if (!result.Equals(string.Empty))
                    return;
                oTTime.Type = GetElementValue(PEDefinition.P_Type, ttimes.ElementAt(i), out result);
                if (!result.Equals(string.Empty))
                    return;
                oTTime.Value = GetElementDTValue(PEDefinition.E_Value, ttimes.ElementAt(i), out result);
                if (!result.Equals(string.Empty))
                    return;
                dicTaskTimes.Add(oTTime.Mark, oTTime);
            }
        }

        /// <summary>
        /// 获取开始时间到结束时间跨越的天数
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private int GetDays(DateTime from, DateTime to)
        {
            TimeSpan ts = to - from;
            return ts.Days + 1;
        }

        /// <summary>
        /// 获取相对时间的时间差值
        /// </summary>
        /// <param name="timeString"></param>
        /// <param name="result"></param>
        /// <param name="timeSymbol"></param>
        /// <returns></returns>
        private TimeSpan GetTimeSpan(string timeString, out string result, out string timeSymbol)
        {
            result = string.Empty;
            timeSymbol = string.Empty;
            string strTmp = string.Empty;
            string strUnit = string.Empty;
            int iTmp = 0;
            int iOffset = 0;
            int iIdx = timeString.IndexOf('(');
            double dbTmp = 0;
            strUnit = timeString.Substring(iIdx + 1, 1);
            switch (strUnit.ToLower())
            {
                case "d":
                case "s":
                    iOffset = 1;
                    break;
                case "h":
                case "m":
                    iOffset = 2;
                    break;
            }

            TimeSpan ts = new TimeSpan();
            //X+/-m天(d)/小时(h)/分钟(m)/秒(s)
            iTmp = timeString.IndexOf('+');
            if (iTmp < 0)
                iTmp = timeString.IndexOf('-');
            if (iTmp > 0)
            {
                strTmp = timeString.Substring(iTmp, iIdx - iOffset - iTmp);
                if (!double.TryParse(strTmp, out dbTmp))
                    result = string.Format("{0}中的时间值不符合double型", timeString);
                timeSymbol = timeString.Substring(0, iTmp);
            }
            else
                result = string.Format("{0}中的时间值不符合预定义格式", timeString);

            if (result.Equals(string.Empty))
            {
                DateTime dt = DateTime.Now;
                switch (strUnit.ToLower())
                {
                    case "d":
                        ts = (dt.AddDays(dbTmp) - dt);
                        break;
                    case "h":
                        ts = (dt.AddHours(dbTmp) - dt);
                        break;
                    case "m":
                        ts = (dt.AddMinutes(dbTmp) - dt);
                        break;
                    case "s":
                        ts = (dt.AddSeconds(dbTmp) - dt);
                        break;
                }
            }
            return ts;
        }

        /// <summary>
        /// 把所有的动作都读入到列表中
        /// </summary>
        /// <param name="epNode"></param>
        /// <param name="result"></param>
        private void GetAllActions(XElement epNode, out string result)
        {
            result = string.Empty;
            dicActions.Clear();
            var procs = epNode.Element(PEDefinition.E_ProcessList).Elements(PEDefinition.E_Process);
            Action oAct;
            string strTimeSymbol = string.Empty;
            TimeSpan ts = new TimeSpan();
            string strTmp = string.Empty;
            XElement node;
            //进程列表
            for (int i = 0; i < procs.Count(); i++)
            {
                var events = procs.ElementAt(i).Element(PEDefinition.E_EventList).Elements(PEDefinition.E_Event);
                //事件列表
                for (int j = 0; j < events.Count(); j++)
                {
                    var actions = events.ElementAt(j).Element(PEDefinition.E_ActionList).Elements(PEDefinition.E_Action);
                    //动作列表
                    for (int m = 0; m < actions.Count(); m++)
                    {
                        node = actions.ElementAt(m);
                        oAct = new Action();
                        #region Set Action Value
                        oAct.Code = GetAttributeValue(PEDefinition.P_Code, node, out result);
                        if (!result.Equals(string.Empty))
                            return;

                        oAct.Name = GetAttributeValue(PEDefinition.P_Name, node, out result);
                        if (!result.Equals(string.Empty))
                            return;
                        //开始时间
                        strTmp = GetElementValue(PEDefinition.P_BeginTime, node, out result);
                        if (!result.Equals(string.Empty))
                            return;
                        ts = GetTimeSpan(strTmp, out result, out strTimeSymbol);
                        if (!result.Equals(string.Empty))
                            return;
                        oAct.BeginTime = dicTaskTimes[strTimeSymbol].Value.Add(ts);

                        //结束时间
                        strTmp = GetElementValue(PEDefinition.P_EndTime, node, out result);
                        if (!result.Equals(string.Empty))
                            return;
                        ts = GetTimeSpan(strTmp, out result, out strTimeSymbol);
                        if (!result.Equals(string.Empty))
                            return;
                        oAct.EndTime = dicTaskTimes[strTimeSymbol].Value.Add(ts);

                        //圈次
                        oAct.QC = GetElementIntValue(PEDefinition.E_QC, node, out result);
                        if (!result.Equals(string.Empty))
                            return;
                        //执行者
                        oAct.ParticipatorCode = GetElementValue(PEDefinition.E_ActorCode, node, out result);
                        if (!result.Equals(string.Empty))
                            return;
                        //动作类型
                        oAct.ActionType = GetElementValue(PEDefinition.E_ActionType, node, out result);
                        if (!result.Equals(string.Empty))
                            return;
                        //必需标识
                        oAct.RequiredFlag = GetElementValue(PEDefinition.E_RequiredFlag, node, out result);
                        if (!result.Equals(string.Empty))
                            return;
                        //影响标识
                        oAct.AffectFlag = GetElementValue(PEDefinition.E_AffectFlag, node, out result);
                        if (!result.Equals(string.Empty))
                            return;
                        //星上动作
                        oAct.IsOnSatAction = GetElementValue(PEDefinition.E_IsOnSatAction, node, out result);
                        if (!result.Equals(string.Empty))
                            return;

                        //工作参数
                        #region 工作参数
                        oAct.WorkingParams = new Dictionary<string, WorkingParm>();
                        WorkingParm oParam = null;
                        var wparams = node.Element(PEDefinition.E_WorkingParam).Elements(PEDefinition.E_Param);
                        int iDPXZCount = 1;
                        int iPDXZCount = 1;
                        if (wparams != null)
                        {
                            for (int n = 0; n < wparams.Count(); n++)
                            {
                                oParam = new WorkingParm();
                                oParam.Code = GetElementValue(PEDefinition.E_DH, wparams.ElementAt(n), out result);
                                if (!result.Equals(string.Empty))
                                    return;
                                oParam.Name = GetElementValue(PEDefinition.P_Name, wparams.ElementAt(n), out result);
                                if (!result.Equals(string.Empty))
                                    return;
                                oParam.Value = GetElementValue(PEDefinition.E_Value, wparams.ElementAt(n), out result);
                                if (!result.Equals(string.Empty))
                                    return;
                                #region 处理重复BID、RTs、SL
                                //因为有俩个BID、RTs、SL，默认将第二个视为事后hui放的
                                if (oParam.Code == PEDefinition.V_BID)
                                {
                                    if (oAct.WorkingParams.ContainsKey(oParam.Code))
                                        oParam.Code = PEDefinition.V_HBID;
                                }
                                else if (oParam.Code == PEDefinition.V_RTs)
                                {
                                    if (oAct.WorkingParams.ContainsKey(oParam.Code))
                                        oParam.Code = PEDefinition.V_HRTs;
                                }
                                else if (oParam.Code == PEDefinition.V_SL)
                                {
                                    if (oAct.WorkingParams.ContainsKey(oParam.Code))
                                        oParam.Code = PEDefinition.V_HSL;
                                }
                                #endregion

                                #region 处理多个点pin选择，如果有多个，则后续的名字为DPXZ_1,DPXZ_n，频段选择类似
                                if (oParam.Code == PEDefinition.V_DPXZ)
                                {
                                    if (oAct.WorkingParams.ContainsKey(oParam.Code))
                                    {
                                        oParam.Code += "_" + iDPXZCount.ToString();
                                        iDPXZCount++;
                                    }
                                }
                                //频段选择
                                if (oParam.Code == PEDefinition.V_PDXZ)
                                {
                                    if (oAct.WorkingParams.ContainsKey(oParam.Code))
                                    {
                                        oParam.Code += "_" + iPDXZCount.ToString();
                                        iPDXZCount++;
                                    }
                                }
                                #endregion
                                oAct.WorkingParams.Add(oParam.Code, oParam);
                            }
                        }
                        #endregion

                        //紧前动作
                        oAct.PreActionSeqs = new List<string>();
                        var preSeqs = node.Element(PEDefinition.E_PreAction).Elements(PEDefinition.P_Sequence);
                        for (int n = 0; n < preSeqs.Count(); n++)
                        {
                            strTmp = preSeqs.ElementAt(n).Value;
                            oAct.PreActionSeqs.Add(strTmp);
                        }
                        //紧后动作
                        oAct.NextActionSeqs = new List<string>();
                        var nextSeqs = node.Element(PEDefinition.E_NextAction).Elements(PEDefinition.P_Sequence);
                        for (int n = 0; n < nextSeqs.Count(); n++)
                        {
                            strTmp = nextSeqs.ElementAt(n).Value;
                            oAct.NextActionSeqs.Add(strTmp);
                        }
                        #endregion
                        if (!dicActions.ContainsKey(oAct.Code))
                            dicActions.Add(oAct.Code, oAct);
                        else
                        {
                            Logger.GetLogger().Error(string.Format("出现相同Code:{0}的动作", oAct.Code));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定的Action
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private Action GetSomeAction(string value, out string result)
        {
            result = string.Empty;
            if (dicActions != null)
            {
                var actions = dicActions.Values.Where(i => i.Name == value);
                if (actions != null && actions.Count() > 0)
                    return (Action)actions.ElementAt(0);
                else
                {
                    result = string.Format("没有名为{0}的动作信息", value);
                    return null;
                }
            }
            else
            {
                result = "解析不到动作信息";
                return null;
            }
        }

        /// <summary>
        /// 获取参与者是DMZ的指定的Action
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private Action GetSomeDMZAction(string value, out string result)
        {
            result = string.Empty;
            if (dicActions != null)
            {
                var actions = dicActions.Values.Where(i => i.Name == value
                    && i.ParticipatorCode.Substring(0, 5).ToUpper() == "G-DMZZ");
                if (actions != null && actions.Count() > 0)
                    return (Action)actions.ElementAt(0);
                else
                {
                    result = string.Format("没有名为{0}的动作信息", value);
                    return null;
                }
            }
            else
            {
                result = "解析不到动作信息";
                return null;
            }
        }

        /// <summary>
        /// 获取指定的Actions
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private List<Action> GetSomeActions(string value, out string result)
        {
            result = string.Empty;
            if (dicActions != null)
            {
                var actions = dicActions.Values.Where(i => i.Name == value);
                if (actions != null && actions.Count() > 0)
                {
                    List<Action> lstActions = new List<Action>();
                    foreach (Action ac in actions)
                    {
                        lstActions.Add(ac);
                    }
                    return lstActions;
                }
                else
                {
                    result = string.Format("没有名为{0}的动作信息", value);
                    return null;
                }
            }
            else
            {
                result = "解析不到动作信息";
                return null;
            }
        }

        /// <summary>
        /// 获取参与者是DMZ的指定的Actions
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private List<Action> GetSomeDMZActions(string value, out string result)
        {
            result = string.Empty;
            if (dicActions != null)
            {
                var actions = dicActions.Values.Where(i => i.Name == value
                    && i.ParticipatorCode.Substring(0, 5).ToUpper() == "G-DMZZ");
                if (actions != null && actions.Count() > 0)
                {
                    List<Action> lstActions = new List<Action>();
                    foreach (Action ac in actions)
                    {
                        lstActions.Add(ac);
                    }
                    return lstActions;
                }
                else
                {
                    result = string.Format("没有名为{0}的动作信息", value);
                    return null;
                }
            }
            else
            {
                result = "解析不到动作信息";
                return null;
            }
        }

        /// <summary>
        /// 获取参与者是DMZ的指定的Actions
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private List<Action> GetSomeDMZActions(string value, DateTime beginTime, DateTime endTime, out string result)
        {
            result = string.Empty;
            if (dicActions != null)
            {
                var actions = dicActions.Values.Where(i => i.Name == value
                    && i.ParticipatorCode.Substring(0, 6).ToUpper() == "G-DMZZ"
                    && (i.BeginTime >= beginTime && i.BeginTime <= endTime));
                if (actions != null && actions.Count() > 0)
                {
                    List<Action> lstActions = new List<Action>();
                    foreach (Action ac in actions)
                    {
                        lstActions.Add(ac);
                    }
                    return lstActions;
                }
                else
                {
                    result = string.Format("没有名为{0}的动作信息", value);
                    return null;
                }
            }
            else
            {
                result = "解析不到动作信息";
                return null;
            }
        }



        /// <summary>
        /// 获取参与者是DMZ的指定的Actions
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private List<Action> GetSomeDMZActions(List<string> valueList, DateTime beginTime, DateTime endTime, out string result)
        {
            result = string.Empty;
            string info = string.Empty;
            if (dicActions != null)
            {
                List<Action> lstResult = new List<Action>();
                List<Action> lstActions = new List<Action>();
                for (int i = 0; i < valueList.Count; i++)
                {
                    lstActions = GetSomeDMZActions(valueList[i], beginTime, endTime, out info);
                }
                if (lstActions != null)
                    lstResult.AddRange(lstActions);
                result += info + ";";
                if (lstResult != null)
                    return lstResult;
                else
                    return null;
            }
            else
            {
                result = "解析不到动作信息";
                return null;
            }
        }

        /// <summary>
        /// 获取地面站编码
        /// </summary>
        /// <param name="actionOwner"></param>
        /// <returns></returns>
        private string GetDMZID(string actionOwner
            , out string deviceID)
        {
            deviceID = string.Empty;
            if (!actionOwner.Contains("-"))
                return string.Empty;
            string strTmp = string.Empty;
            string[] stmps = actionOwner.Split(splitor_hline);
            if (stmps.Length > 3)
            {
                strTmp = "-" + stmps[2] + "-";
                deviceID = actionOwner.Substring(actionOwner.IndexOf(strTmp) + strTmp.Length);
            }
            if (stmps.Length >= 3)
                return stmps[2];
            else
                return string.Empty;
        }
        #endregion

        #region Get Attribute or Element Value by Name、Element, if Attribute or Element is null ,return null
        /// <summary>
        /// 获取指定属性的字符型值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="node"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetAttributeValue(string name, XElement node, out string result)
        {
            result = string.Empty;
            XAttribute attr = node.Attribute(XName.Get(name));
            string value = string.Empty;
            if (attr != null)
            {
                value = attr.Value;
                if (value == string.Empty)
                    result = string.Format("名为{0}的属性值为空", name);
            }
            else
                result = string.Format("名为{0}的属性获取不到", name);
            return value;
        }

        /// <summary>
        /// 获取指定属性的日期型值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="node"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private DateTime GetAttributeDTValue(string name, XElement node, out string result)
        {
            result = string.Empty;
            XAttribute attr = node.Attribute(XName.Get(name));
            DateTime dt = DateTime.MinValue;
            if (attr != null)
            {
                if (attr.Value != string.Empty)
                {
                    if (!DateTime.TryParseExact(attr.Value, PEDefinition.LongTimeFormat
                        , CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                        result = string.Format("名为{0}的属性值{1}不符合日期格式", name, attr.Value);
                }
                else
                    result = string.Format("名为{0}的属性值为空", name);
            }
            else
                result = string.Format("名为{0}的属性获取不到", name);
            return dt;
        }

        /// <summary>
        /// 获取指定属性的数值型值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="node"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private int GetAttributeIntValue(string name, XElement node, out string result)
        {
            result = string.Empty;
            XAttribute attr = node.Attribute(XName.Get(name));
            int value = 0;
            if (attr != null)
            {
                if (attr.Value == string.Empty)
                    result = string.Format("名为{0}的属性值为空，ParentNode{1}", name, node.ToString());
                else
                {
                    if (!int.TryParse(attr.Value, out value))
                        result = string.Format("名为{0}的属性值{1}不符合数值格式，ParentNode{1}", name, node.ToString());
                }
            }
            else
                result = string.Format("名为{0}的属性获取不到，ParentNode{1}", name, node.ToString());
            return value;
        }

        /// <summary>
        /// 获取指定元素的字符型值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetElementValue(string name, XElement parentNode, out string result)
        {
            result = string.Empty;
            XElement node = parentNode.Element(XName.Get(name));
            string value = string.Empty;
            if (node != null)
            {
                value = node.Value;
                if (value == string.Empty)
                    result = string.Format("名为{0}的元素值为空，ParentNode{1}", name, parentNode.ToString());
            }
            else
                result = string.Format("名为{0}的元素获取不到，ParentNode{1}", name, parentNode.ToString());
            return value;
        }

        /// <summary>
        /// 获取指定元素的日期型值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private DateTime GetElementDTValue(string name, XElement parentNode, out string result)
        {
            result = string.Empty;
            XElement node = parentNode.Element(XName.Get(name));
            DateTime dt = DateTime.MinValue;
            if (node != null)
            {
                if (node.Value != string.Empty)
                {
                    if (!DateTime.TryParseExact(node.Value, PEDefinition.LongTimeFormat
                        , CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                        result = string.Format("名为{0}的元素的值{1}不符合日期格式", name, node.Value);
                }
                else
                    result = string.Format("名为{0}的元素值为空", name);
            }
            else
                result = string.Format("名为{0}的元素获取不到", name);
            return dt;
        }

        /// <summary>
        /// 获取指定元素的数值型值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private int GetElementIntValue(string name, XElement parentNode, out string result)
        {
            result = string.Empty;
            XElement node = parentNode.Element(XName.Get(name));
            int value = 0;
            if (node != null)
            {
                if (node.Value == string.Empty)
                    result = string.Format("名为{0}的元素值为空", name);
                else
                {
                    if (!int.TryParse(node.Value, out value))
                        result = string.Format("名为{0}的元素值{1}不符合数值格式", name, node.Value);
                }
            }
            else
                result = string.Format("名为{0}的元素获取不到", name);
            return value;
        }
        #endregion
    }

    #region 动作、任务时间类定义

    /// <summary>
    /// 动作
    /// </summary>
    public class Action
    {
        #region XML Properties
        /// <summary>
        /// 系统描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 归属
        /// </summary>
        public string Own { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Sequence { get; set; }
        #endregion

        #region XML Elements
        /// <summary>
        /// 圈次
        /// </summary>
        public int QC { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 参与者编号，=sys.Code
        /// </summary>
        public string ParticipatorCode { get; set; }
        /// <summary>
        /// 功能描述
        /// </summary>
        public string FunctionDesc { get; set; }
        /// <summary>
        /// 工作类型
        /// </summary>
        public string ActionType { get; set; }
        /// <summary>
        /// 必需标识
        /// </summary>
        public string RequiredFlag { get; set; }
        /// <summary>
        /// 影响标识
        /// </summary>
        public string AffectFlag { get; set; }
        /// <summary>
        /// 星上动作
        /// </summary>
        public string IsOnSatAction { get; set; }
        /// <summary>
        /// 工作参数
        /// </summary>
        public Dictionary<string, WorkingParm> WorkingParams { get; set; }
        /// <summary>
        /// 紧前动作
        /// </summary>
        public List<string> PreActionSeqs { get; set; }
        /// <summary>
        /// 紧后动作
        /// </summary>
        public List<string> NextActionSeqs { get; set; }
        #endregion
    }

    /// <summary>
    /// 工作参数
    /// </summary>
    public class WorkingParm
    {
        /// <summary>
        /// 代号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 任务时间
    /// </summary>
    public class TaskTime
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 符号
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public DateTime Value { get; set; }
    }
    #endregion

    #region 常量定义：xml各属性及Element名称定义
    public class PEDefinition
    {
        public const string RootName = "工作程序";
        public const string ProcCount = "程序个数";
        public const string P_BeginTime = "开始时间";
        public const string P_EndTime = "结束时间";
        public const string P_ID = "ID";
        public const string P_Description = "描述";
        public const string P_Type = "类型";
        public const string P_Own = "归属";
        public const string P_Code = "编码";
        public const string P_Name = "名称";
        public const string P_Sequence = "序号";

        public const string E_ExperimentProcedure = "试验程序";
        public const string E_TaskName = "任务名称";
        public const string E_ExperimentItem = "试验项目";
        public const string E_ExperimentType = "试验类型";
        //创建时间、作者、当前版本、版本记录、版本文件对于本系统而言无意义，所以不解析也不定义
        public const string E_WorkingPattern = "工作方式";
        public const string E_WorkingQuality = "工作性质";
        public const string E_Notes = "备注";
        //以下是试验参与者相关
        public const string E_ParticipatorDeclare = "试验参与者声明";
        public const string E_System = "系统";
        public const string E_MatchFlag = "通配标识";
        public const string E_MatchTypeCode = "通配类型标识";
        public const string E_SubSysList = "子系统列表";
        public const string E_SubSys = "子系统";
        public const string E_ComponentList = "组件列表";
        public const string E_Component = "组件";

        //以下是试验时间相关
        public const string E_ExperimentTime = "试验时间";
        public const string E_TaskTime = "任务时间";
        public const string E_Symbol = "符号";
        public const string E_Value = "值";

        //以下是试验进程相关
        public const string E_ProcessList = "试验进程列表";
        public const string E_Process = "进程";
        public const string E_EventList = "事件列表";
        public const string E_Event = "工作事件";
        public const string E_EnterCondition = "进入条件";

        public const string E_Condition = "条件";
        public const string E_Logic = "逻辑";
        public const string E_ParamList = "参数列表";
        public const string E_ConditionItemList = "条件项列表";
        public const string E_ConditionItem = "条件项";
        public const string E_ParamCode = "参数编码";
        public const string E_RelationCode = "关系符";

        public const string E_LeaveCondition = "退出条件";
        public const string E_ActionList = "动作列表";
        public const string E_Action = "动作";
        public const string E_QC = "圈次";
        public const string E_ActorCode = "执行者编号";
        public const string E_FucntionDescription = "功能描述";
        public const string E_ActionType = "动作类型";
        public const string E_RequiredFlag = "必需标识";
        public const string E_AffectFlag = "影响标识";
        public const string E_IsOnSatAction = "星上动作";
        public const string E_WorkingParam = "工作参数";
        public const string E_PreAction = "紧前动作";
        public const string E_NextAction = "紧后动作";
        public const string E_Param = "参数";
        public const string E_DH = "代号";

        public const string V_SC = "数据接收与实时传输";
        public const string V_HFSC = "事后回放数传数据";
        public const string V_YC = "遥测接收";
        public const string V_SXYK = "上行遥控";
        public const string V_CG = "测轨";
        public const string V_SHJ = "SHJ";//测控事件类型
        public const string V_PDXZ = "PDXZ";//频段选择
        public const string V_DPXZ = "DPXZ";//点频选择
        public const string V_SCID = "SCID";//HTQ标识
        public const string V_MS = "MS";//设备工作模式
        public const string V_QB = "YB";
        public const string V_KSHX = "KSHX";//开上行zai波时间
        public const string V_GSHX = "GSHX";//关上行zai波时间
        public const string V_BID = "BID";//信息类别标志
        public const string V_SBZ = "SBZ";//实时传送数据标志
        public const string V_RTs = "RTs";//数据传输开始时间
        public const string V_RTe = "RTe";//数据传输结束时间
        public const string V_SL = "SL";//数据传输速率
        public const string V_HBZ = "HBZ";//事后回放数据标志
        public const string V_Ts = "Ts";//数据起始时间
        public const string V_Te = "Te";//数据结束时间
        public const string V_HBID = "HBID";//事后hui放信息类别标志
        public const string V_HRTs = "HRTs";//事后hui放数据传输开始时间
        public const string V_HSL = "HSL";//事后hui放数据传输速率

        //WorkingContent
        public const string W_SYGH = "试验规划";
        public const string W_JHGL = "计划管理";
        public const string W_SYSJCL = "试验数据处理";

        public const string LongTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string ShortTimeFormat = "yyyy-MM-dd";
        public const string LongTimeFormat14 = "yyyyMMddHHmmss";
        public const string GZDYName = "东风站,大树里站,青岛站,喀什站,厦门站,三亚站,佳木斯站,瑞典站,远望五号测量船,远望六号测量船";//"东风站,大树里站,青岛站,喀什站,厦门站,三亚站,佳木斯站,瑞典站,远望五号测量船,远望六号测量船";
        public const string GZDYCode = "20-DF,20-DSL,26-QD,26-KS,26-XM,26-SY,26-JMS,26-RD,23-YW5,23-YW6";
        public const string SBDH = "TS-4216,YQ-2512,TY-4801,TS-4217/TS-4801,TS-4222,TS-4205,TS-4205,瑞典站设备,TS-4203,TS-4210";
        public const string SCIDKeys = "TS3,TS4A,TS4B,TS5A,TS5B";
        public const string SCIDs = "TS0301,TS0401,TS0402,TS0501,TS0502";
    }
    #endregion
}
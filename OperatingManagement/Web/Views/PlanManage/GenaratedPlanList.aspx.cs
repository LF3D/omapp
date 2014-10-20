using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.Framework;
using OperatingManagement.DataAccessLayer.PlanManage;
using System.Web.Security;
using System.Data;


namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class GenaratedPlanList : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    string sID = Request.QueryString["id"];
                    int id = 0;
                    Int32.TryParse(sID, out id);
                    string ids = CreatePlans(id);
                    BindPlans(ids);
                }
                else
                {
                    //test
                    //string ids = CreatePlans(1);
                    BindPlans("2,3,4,5");
                }
            }

        }

        void BindPlans(string ids)
        {
            JH objJH = new JH();

            List<JH> listDatas = objJH.SelectByIDS(ids);
            rpDatas.DataSource = listDatas;
            rpDatas.DataBind();
        }

        string  CreatePlans(int proid)
        {
            string jhIDs = "";
            JH objJH = new JH();
            objJH.SRCType = 1; //试验程序
            objJH.SRCID = proid;

            #region  应用程序计划
            YJJH objYJJH = new YJJH();
            objYJJH.Source = "运控评估中心YKZX(02 04 00 00)";
            objYJJH.Destination = "仿真推演分系统FZTY(02 E7 00 00)";
            objYJJH.TaskID = "700任务(0500)";
            objYJJH.InfoType = "应用研究计划(00 70 06 00)";
            objYJJH.Format1 = "XXFL  JXH  SysName  StartTime  EndTime  Task";
            objYJJH.LineCount = 2;
            objYJJH.Add();

            objJH.TaskID = objYJJH.TaskID;
            objJH.PlanType = "YJJH";
            objJH.PlanID = objYJJH.Id;
            objJH.Add();
            jhIDs = objJH.Id.ToString();
            #endregion

            #region  空间信息需求
            XXXQ objXXXQ = new XXXQ();
            objXXXQ.ID = 6;
            objXXXQ.Source = "111运控评估中心YKZX(02 04 00 00)";
            objXXXQ.Destination = "空间信息综合应用中心ZCZX(02 6F 00 00)";
            objXXXQ.TaskID = "700任务(0501)";
            objXXXQ.InfoType = "空间目标信息需求(00 70 60 00)";
            objXXXQ.Format1 = "User  Time  TargetInfo  TimeSection1  TimeSection2  Sum";
            objXXXQ.Format2 = "SatName  InfoName  InfoTime";
            objXXXQ.LineCount = 3;
            objXXXQ.Add();

            objJH.TaskID = objXXXQ.TaskID;
            objJH.PlanType = "XXXQ";
            objJH.PlanID = objXXXQ.Id;
            objJH.Add();
            jhIDs = jhIDs+","+objJH.Id.ToString();
            #endregion

            #region  地面站工作计划
            GZJH objGZJH = new GZJH();
            objGZJH.Source = "运控评估中心YKZX(02 04 00 00)";
            objGZJH.Destination = "空间信息综合应用中心ZCZX(02 6F 00 00)";
            objGZJH.TaskID = "700任务(0501)";
            objGZJH.InfoType = "地面站工作计划(00 70 60 00)";
            objGZJH.Format1 = "JXH  XXFL  DW  SB  QS";
            objGZJH.Format2 = "QH  DH  FS  JXZ  MS  QB  GXZ  ZHB  RK  GZK  KSHX  GSHX  GZJ  JS  BID  SBZ  RTs  RTe  SL  BID  HBZ  Ts  Te  RTs  SL";
            objGZJH.LineCount = 3;
            objGZJH.Add();

            objJH.TaskID = objGZJH.TaskID;
            objJH.PlanType = "GZJH";
            objJH.PlanID = objGZJH.Id;
            objJH.Add();
            jhIDs = jhIDs + "," + objJH.Id.ToString();
            #endregion

            #region  中心运行计划

            #endregion

            #region  仿真推演试验数据
            TYSJ objTYSJ = new TYSJ();
            objTYSJ.Source = "运控评估中心YKZX(02 04 00 00)";
            objTYSJ.Destination = "仿真推演分系统FZTY(02 E7 00 00)";
            objTYSJ.TaskID = "700任务(0501)";
            objTYSJ.InfoType = "仿真推演数据(00 70 32 00)";
            objTYSJ.Format1 = "SatName  Type  TestItem  StartTime  EndTime  Condition";
            objTYSJ.LineCount = 3;
            objTYSJ.Add();

            objJH.TaskID = objTYSJ.TaskID;
            objJH.PlanType = "TYSJ";
            objJH.PlanID = objTYSJ.Id;
            objJH.Add();
            jhIDs = jhIDs + "," + objJH.Id.ToString();
            #endregion

            return jhIDs;
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "Plan.Genarate";
            this.ShortTitle = "生成计划";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/GenaratedPlanList.aspx.js");
        }
    }
}
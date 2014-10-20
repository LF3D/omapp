<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PlanList.aspx.cs" Inherits="OperatingManagement.Web.Views.PlanManage.PlanList" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="planmanage" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuPlan" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MapPathContent" runat="server">
    计划管理 &gt; 查看计划
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="BodyContent" runat="server">
    <table cellspacing="0" cellpadding="0" class="searchTable" width="100%">
        <tr>
            <th style="width:10%;">创建时间</th>
            <th style="width:10%;">起始时间：</th>
            <td style="width:25%;">
                <asp:TextBox ID="txtCStartDate" ClientIDMode="Static" Width="200px"  CssClass="text" runat="server" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
            <th style="width:10%;">结束时间：</th>
            <td style="width:25%;">
                <asp:TextBox ID="txtCEndDate" ClientIDMode="Static" Width="200px"  CssClass="text" runat="server" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
            <td style="width:20%;">
                <asp:CompareValidator ID="cv1" runat="server" 
                    ControlToCompare="txtCStartDate" ControlToValidate="txtCEndDate" 
                    Display="Dynamic" ErrorMessage="结束时间应大于起始时间" ForeColor="Red" 
                    Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator></td>
        </tr>
        <tr>
            <th>计划时间</th>
            <th>起始时间：</th>
            <td>
                <asp:TextBox ID="txtJHStartDate" ClientIDMode="Static" Width="200px"  CssClass="text" runat="server" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
            <th>结束时间：</th>
            <td>
                <asp:TextBox ID="txtJHEndDate" ClientIDMode="Static" Width="200px"  CssClass="text" runat="server" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
            <td>
                <asp:CompareValidator ID="cv2" runat="server" 
                    ControlToCompare="txtJHStartDate" ControlToValidate="txtJHEndDate" 
                    Display="Dynamic" ErrorMessage="计划时间的结束时间应大于起始时间" ForeColor="Red" 
                    Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator></td>
        </tr>
        <tr>
            <th>计划类型</th>
            <th></th>
            <td colspan="3">
                <asp:DropDownList ID="ddlType" ClientIDMode="Static" runat="server" Width="200px" Height="20px">
                    <%--<asp:ListItem Value="0">==全部==</asp:ListItem>--%>
                    <asp:ListItem Value="YJJH">应用研究工作计划</asp:ListItem>
                    <asp:ListItem Value="XXXQ">空间信息需求</asp:ListItem>
                    <asp:ListItem Value="DJZYSQ">测控资源使用申请</asp:ListItem>
                    <asp:ListItem Value="GZJH">地面站工作计划</asp:ListItem>
                    <asp:ListItem Value="ZXJH">中心运行计划</asp:ListItem>
                    <asp:ListItem Value="TYSJ">仿真推演试验数据</asp:ListItem>
                    <asp:ListItem Value="DJZYJH">测控资源使用计划</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button class="button" ID="btnSearch" runat="server" onclick="btnSearch_Click" Text="查询" Width="65px" />
                <button class="button" style="width:65px" onclick="return clearField();">清空</button>
                <div style="display:none;">
                    <asp:TextBox ID="txtId" runat="server" ClientIDMode="Static"></asp:TextBox>
                    <asp:TextBox ID="txtPlanID" runat="server" ClientIDMode="Static"></asp:TextBox>
                    <asp:TextBox ID="txtPlanType" runat="server" ClientIDMode="Static"></asp:TextBox>
                    <asp:Button ID="btnHidden" runat="server" ClientIDMode="Static" Text="btnHidden" 
                            OnClick="btnHidden_Click" />
                    <div>
                        <asp:Button ClientIDMode="Static"  ID="btnSubmit" class="button" runat="server" OnClick="btnSubmit_Click"
                            Text="发送" />&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" class="button" runat="server" OnClick="btnCancel_Click"
                            Text="取消" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:Repeater ID="rpDatas" runat="server" onitemdatabound="rpDatas_ItemDataBound">
        <HeaderTemplate>
            <table class="list">
                <tr>
                    <th style="width:1.5%;"><input type="checkbox" onclick="checkAll(this)" /></th>
                    <th style="width: 7%;">
                        计划编号
                    </th>
                    <th style="width: 9%;">
                        任务代号
                    </th>
                    <th style="width: 12%;">
                        计划类别
                    </th>
                    <th style="width: 7%;">
                        使用状态
                    </th>
                    <th style="width: 7%;">
                        发送状态
                    </th>
                    <th style="width: 6.3%;">
                        源类型
                    </th>
                    <th style="width: 5.5%;">
                        源序号
                    </th>
                    <th style="width: 10.7%;">
                        创建时间
                    </th>
                    <th style="width: 10.7%;">
                        开始时间
                    </th>
                    <th style="width: 10.6%;">
                        结束时间
                    </th>
                    <th style="width: 6.3%;">
                        查看
                    </th>
                    <th style="width: 6.3%;">
                        编辑
                    </th>
                    <th style="width:0.1%;visibility:hidden;"></th>
                        <%--                                    <th style="width: 70px;">
                        明细
                    </th>--%>
                        <%-- <th style="width: 70px;">
                            发送
                        </th>--%>
                    </tr>
            <tbody id="tbPlans">
            </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <input type="checkbox" id="chkDelete" name="chkDelete" value="22" runat="server" />
                </td>
                <td>
                    <%# Eval("planid")%>
                </td>
                <td>
                    <%# Eval("taskname")%>
                </td>
                <td>
                    <%# Eval("PlanTypeName")%>
                </td>
                <td>
                    <%# (Eval("USESTATUS").ToString() == "1" ? "已确认" : "未确认")%>
                </td>
                <td>
                    <%# (Eval("SENDSTATUS").ToString() == "1"? "已发送":"未发送")%>
                </td>
                <td>
                    <%# (Eval("SRCType").ToString() == "1" ? "试验程序" : (Eval("SRCType").ToString() == "0"? "空白计划" : "其他"))%>
                </td>
                <td>
                    <%# Eval("SRCID")%>
                </td>
                <td>
                    <%# Eval("CTime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                </td>
                <td>
                    <%# Eval("starttime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                </td>
                <td>
                    <%# Eval("endtime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                </td>
                <td>
                    <button class="button"  name="htmbtnView"  onclick="return showView('<%# Eval("ID") %>','<%# Eval("PLANTYPE") %>')">
                        查看</button>
                <td>
                    <button class="button"  name="htmbtnEdit"  onclick="return showEdit('<%# Eval("ID") %>','<%# Eval("PLANTYPE") %>')">
                        编辑</button>
                </td>
                <td style="visibility:hidden">
                    <%# Eval("Id")%>
                </td>
                <%--                                <td>
                <button class="button" onclick="return showDetail('<%# Eval("ID") %>','<%# Eval("PLANTYPE") %>')">
                    明细</button>
            </td>--%>
            <%--<td>
                <button class="button" onclick="return showSend('<%# Eval("ID") %>','<%# Eval("PLANID") %>','<%# Eval("PLANTYPE") %>')">
                    发送计划</button>
            </td>--%>
        </tr>
    </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Panel ID ="pnlAll2" runat="server">
        <table class="listTitle">
                    <tr>
                        <td class="listTitle-c1">
                            <button class="button" onclick="return selectAll();">
                                全选</button>&nbsp;&nbsp;
                            <button class="button" onclick="return sendPlan();">
                                发送计划</button>
                        </td>
                        <td class="listTitle-c2">
                            <om:CollectionPager ID="cpPager" runat="server" PageSize="1">
                            </om:CollectionPager>
                        </td>
                    </tr>
                </table>
    </asp:Panel>
    <div id="tartgetPanel" style="display:none">
        <table style = " text-align:center;">
            <tr>
                <td>
                    <b>请选择要使用的发送协议：</b>
                    <br />
                    <asp:RadioButtonList ID="rbtProtocl" runat="server" 
                    RepeatDirection="Horizontal">
                            <asp:ListItem Value="2" Selected="True">Fep with Tcp</asp:ListItem>
                            <asp:ListItem Value="1">Fep with Udp</asp:ListItem>
                            <asp:ListItem Value="0">Ftp</asp:ListItem>
                        </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <b>任务运行模式（仅对测控资源使用申请有效）：</b>
                    <br />
                    <asp:RadioButtonList ID="rbtMode" runat="server" 
                    RepeatDirection="Horizontal">
                            <asp:ListItem Value="OP" Selected="True">实战</asp:ListItem>
                            <asp:ListItem Value="TS">联试</asp:ListItem>
                            <asp:ListItem Value="DR">日常运行</asp:ListItem>
                        </asp:RadioButtonList>
                </td>
            </tr>
            <tr runat="server" id="trSendTarget">
                <td align="center"  style="text-align: center">
                <br />
                <b>请选择计划待发送的目标系统，可以多选</b>
                <br />
                    <%--<asp:RadioButtonList ID="rbtDestination" runat="server">
                    </asp:RadioButtonList>--%>
                    <asp:CheckBoxList ID="ckbDestination" runat="server">
                    </asp:CheckBoxList>
                    
                    <br />
                   <asp:Label ClientIDMode="Static" CssClass="error" ID="lblTargetMessage" runat="server" ForeColor="Red" style="display:none;">请选择要发送的目标系统</asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div id="divMessage"  title="消息">
            <asp:Label ID="lblMessage" CssClass="error" runat="server" ForeColor="Red"></asp:Label>
        </div>
    <div id="dialog-form" style="display:none" title="提示信息">
	    <p class="content"></p>
    </div>
</asp:Content>
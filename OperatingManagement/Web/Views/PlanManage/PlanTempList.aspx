<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PlanTempList.aspx.cs" Inherits="OperatingManagement.Web.Views.PlanManage.PlanTempList" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="planmanage" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuPlan" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MapPathContent" runat="server">
    计划管理 &gt; 查看初步计划
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
                    <asp:ListItem Value="DJZYSQ">测控资源使用申请</asp:ListItem>
                    <asp:ListItem Value="GZJH">地面站工作计划</asp:ListItem>
                    <asp:ListItem Value="ZXJH">中心运行计划</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button class="button" ID="btnSearch" runat="server" onclick="btnSearch_Click" Text="查询" Width="65px" />
                <button class="button" style="width:65px" onclick="return clearField();">清空</button>
            </td>
        </tr>
    </table>
    <asp:Repeater ID="rpDatas" runat="server">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <th style="width: 150px;">
                            计划编号
                        </th>
                        <th style="width: 150px;">
                            任务名称
                        </th>
                        <th style="width: 70px;">
                            源类型
                        </th>
                        <th style="width: 70px;">
                            源序号
                        </th>
                        <th style="width: 140px;">
                            创建时间
                        </th>
                        <th style="width: 150px;">
                            开始时间
                        </th>
                        <th style="width: 150px;">
                            结束时间
                        </th>
                        <th style="width: 70px;">
                            编辑
                        </th>
                        </tr>
                <tbody id="tbPlans">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Eval("planid")%>
                        </td>
                        <td>
                            <%# Eval("taskname")%>
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
                            <button class="button"  name="htmbtnEdit"  onclick="return showEdit('<%# Eval("ID") %>','<%# Eval("PLANTYPE") %>')">
                                编辑</button>
                        </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    <asp:Panel ID ="pnlAll2" runat="server">
                    <div id="selectAll2" >
                    <table class="listTitle">
                        <tr>
                            <td class="listTitle-c2">
                                <om:CollectionPager ID="cpPager" runat="server" PageSize="1">
                                </om:CollectionPager>
                            </td>
                        </tr>
                    </table>
                    </div>
                    </asp:Panel>
    <div id="divMessage"  title="消息">
        <asp:Label ID="lblMessage" CssClass="error" runat="server" ForeColor="Red"></asp:Label>
    </div>
    <div id="dialog-form" style="display:none" title="提示信息">
	<p class="content"></p>
    </div>
</asp:Content>
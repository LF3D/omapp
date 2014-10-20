<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ExperimentPlanList.aspx.cs" Inherits="OperatingManagement.Web.Views.PlanManage.ExperimentPlanList" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="planmanage" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuPlan" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MapPathContent" runat="server">
    计划管理 &gt; 查看试验计划
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="BodyContent" runat="server">
    <table cellspacing="0" cellpadding="0" class="searchTable">
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
                    Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                    
                <asp:Button class="button" ID="btnSearch" runat="server" OnClick="btnSearch_Click"
                    Text="查询" Width="69px" />
                <button class="button" onclick="return clearField();" style="width: 65px;">清空</button>&nbsp;
                <div style="display:none;">
                    <asp:TextBox ID="txtId" runat="server" ClientIDMode="Static"></asp:TextBox>
                    <asp:Button ID="btnHidden" runat="server" ClientIDMode="Static" Text="btnHidden" OnClick="btnHidden_Click" />
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
    <asp:Repeater ID="rpDatas" runat="server">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <th style="width:20px;"><input type="checkbox" onclick="checkAll(this)" /></th>
                        <th style="width: 80px;">
                            计划编号
                        </th>
                        <th style="width: 150px;">
                            任务名称
                        </th>
                        <th style="width: 80px;">
                            计划类别
                        </th>
                        <th style="width: 80px;">
                            源类型
                        </th>
                        <th style="width: 70px;">
                            源序号
                        </th>
                        <th style="width: 150px;">
                            创建时间
                        </th>
                        <th style="width: 180px;">
                            文件路径
                        </th>
                        <th style="width: 150px;">
                            开始时间
                        </th>
                        <th style="width: 150px;">
                            结束时间
                        </th>
                        <th style="width: 70px;">
                            明细
                        </th>
                    </tr>
                    <tbody  id="tbPlans">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <%--<td><input type="checkbox" <%# Eval("LoginName").ToString().Equals(this.Profile.UserName,StringComparison.InvariantCultureIgnoreCase)?"disabled=\"true\"":"" %> name="chkDelete" value="<%# Eval("Id") %>" /></td>--%>
                    <td><input type="checkbox" name="chkDelete" value="<%# Eval("Id") %>" /></td>
                    <td>
                        <%# Eval("planid")%>
                    </td>
                    <td>
                        <%# Eval("taskname")%>
                    </td>
                    <td>
                        <%# Eval("plantype")%>
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
                        <%# Eval("FileIndex")%>
                    </td>
                    <td>
                        <%# Eval("starttime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                    </td>
                    <td>
                        <%# Eval("endtime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                    </td>
                    <td>
                        <button class="button" onclick="return showDetail('<%# Eval("ID") %>')">
                            查看</button>
                    </td>
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
                    <om:CollectionPager ID="cpPager" runat="server">
                    </om:CollectionPager>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div id="tartgetPanel" style="display: none">
        <table style="text-align: center;">
            <tr>
                <td>
                    <b>请选择要使用的发送协议：</b>
                    <br />
                    <asp:RadioButtonList ID="rbtProtocl" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="2" Selected="True">Fep with Tcp</asp:ListItem>
                        <asp:ListItem Value="1">Fep with Udp</asp:ListItem>
                        <asp:ListItem Value="0">Ftp</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <b>任务运行模式：</b>
                    <br />
                    <asp:RadioButtonList ID="rbtMode" runat="server" 
                    RepeatDirection="Horizontal">
                            <asp:ListItem Value="OP" Selected="True">实战</asp:ListItem>
                            <asp:ListItem Value="TS">联试</asp:ListItem>
                            <asp:ListItem Value="DR">日常运行</asp:ListItem>
                        </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td align="center" style="text-align: center">
                    <br />
                    <b>请选择计划待发送的目标系统，可以多选</b>
                    <br />
                    <%--<asp:RadioButtonList ID="rbtDestination" runat="server">
                    </asp:RadioButtonList>--%>
                    <asp:CheckBoxList ID="ckbDestination" runat="server">
                    </asp:CheckBoxList>
                    <br />
                    <asp:Label ClientIDMode="Static" CssClass="error" ID="lblTargetMessage" runat="server"
                        ForeColor="Red" Style="display: none;">请选择要发送的目标系统</asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div id="divMessage" title="消息">
        <asp:Label ID="lblMessage" CssClass="error" runat="server" ForeColor="Red"></asp:Label>
    </div>
    <div id="dialog-form" style="display: none" title="提示信息">
        <p class="content">
        </p>
    </div>
</asp:Content>
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ExperimentProgramList.aspx.cs" Inherits="OperatingManagement.Web.Views.PlanManage.ExperimentProgramList" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="planmanage" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuPlan" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MapPathContent" runat="server">
    计划管理 &gt; 生成初步计划
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
                    Text="查询" Width="69px" CausesValidation="false" />
                <button class="button" onclick="return clearField();" style="width: 65px;">清空</button>&nbsp;
                <div style="display:none;">
                    <asp:TextBox ID="txtID" runat="server" ClientIDMode="Static"></asp:TextBox>
                </div>
            </td>
        </tr>
    </table>
    <asp:Repeater ID="rpDatas" runat="server">
        <HeaderTemplate>
            <table class="list">
                <tr>
                    <%--<th style="width:20px;"><input type="checkbox" onclick="checkAll(this)" /></th>--%>
                    <th style="width: 5%;">
                        任务代号
                    </th>
                    <th style="width: 5%;">
                        试验程序序号
                    </th>
                    <th style="width: 5%;">
                        版本号
                    </th>
                    <th style="width: 5%;">
                        项目数
                    </th>
                    <th style="width: 10%;">
                        开始时间
                    </th>
                    <th style="width: 10%;">
                        结束时间
                    </th>
                    <th style="width: 48%;">
                        文件路径
                    </th>
                    <th style="width: 6%;">
                        明细
                    </th>
                    <th style="width: 6%;">
                        生成计划
                    </th>
                </tr>
                <tbody id="tbUsers">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <%--<td><input type="checkbox" <%# Eval("LoginName").ToString().Equals(this.Profile.UserName,StringComparison.InvariantCultureIgnoreCase)?"disabled=\"true\"":"" %> name="chkDelete" value="<%# Eval("Id") %>" /></td>--%>
                <td>
                    <%# Eval("TaskID")%>
                </td>
                <td>
                    <%# Eval("pno")%>
                </td>
                <td>
                    <%# Eval("version")%>
                </td>
                <td>
                    <%# Eval("PCount")%>
                </td>
                <td>
                    <%# Eval("starttime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                </td>
                <td>
                    <%# Eval("endtime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                </td>
                <td>
                    <%# Eval("FileIndex")%>
                </td>
                <td>
                    <button class="button" onclick="return showDetail('<%# Eval("ID") %>')">
                        明细</button>
                </td>
                <td>
                    <button class="button" onclick="return showPopForm('<%# Eval("ID") %>')">
                        生成</button>
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
            </td>
            <td class="listTitle-c2">
                <om:CollectionPager ID="cpPager" runat="server">
                </om:CollectionPager>
            </td>
        </tr>
        <tr id="trMessage" runat="server" visible="false">
            <td align="left" colspan="2">
                <asp:Label ID="ltMessage" runat="server" CssClass="error" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <div id="tartgetPanel" style="display: none" title="计划信息">
        <table style="text-align: center;">
            <tr>
                <td colspan="2">
                    <b>计划信息</b>
                </td>
            </tr>
                    <tr>
            <th>信息分类</th>
            <td>
                <asp:RadioButtonList ID="radBtnXXFL" runat="server" 
                    RepeatDirection="Horizontal" ClientIDMode="Static">
                    <asp:ListItem Value="ZJ" Selected="True">周计划</asp:ListItem>
                    <asp:ListItem Value="RJ">日计划</asp:ListItem> 
                </asp:RadioButtonList>
            </td>
        </tr>
                <tr>
            <th class="style1">试验开始时间</th>
            <td>
                <asp:TextBox ID="txtStartTime" runat="server" Width="150px" CssClass="text" 
                    MaxLength="14" ClientIDMode="Static"   onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            &nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartTime"
                        ErrorMessage="开始时间不能为空" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
        </tr>
        <tr>
            <th class="style1">试验结束时间</th>
            <td>
                <asp:TextBox ID="txtEndTime" runat="server" Width="150px" CssClass="text" 
                    MaxLength="14" ClientIDMode="Static"   onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            &nbsp;<span style="color:#3399FF;">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEndTime"
                        ErrorMessage="结束时间不能为空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </span><span style="color:#3399FF;">
                <asp:CompareValidator ID="CompareValidator2" runat="server" 
                    ControlToCompare="txtStartTime" ControlToValidate="txtEndTime" 
                    Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red" 
                    Operator="GreaterThan" Type="Date"></asp:CompareValidator>
                </span></td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:right">
            <asp:Button class="button" ClientIDMode="Static" ID="btnCreatePlan" runat="server"
                            OnClick="btnCreatePlan_Click" Text="生成" Width="69px" />
            </td>
        </tr>
        </table>
    </div>
</asp:Content>
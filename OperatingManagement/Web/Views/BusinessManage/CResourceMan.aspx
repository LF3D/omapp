<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CResourceMan.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.CResourceMan" %>
<%@ Import Namespace="OperatingManagement.DataAccessLayer.BusinessManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
<om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="resmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
<om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuRes" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
 业务管理 &gt; 查询中心资源
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
<div class="index_content_search">
    <table cellspacing="0" cellpadding="0" class="listTitle">
        <tr>
            <th width="10%">
                健康/占用状态：
            </th>
            <td width="10%">
                <asp:DropDownList ID="dplResourceStatus" runat="server" CssClass="norDpl">
                </asp:DropDownList>
            </td>
            <th width="10%">
                起始时间：
            </th>
            <td width="20%">
                <asp:TextBox ID="txtBeginTime" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                    CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtBeginTime" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
            <th width="10%">
                结束时间：
            </th>
            <td width="20%">
                <asp:TextBox ID="txtEndTime" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                    CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtEndTime" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtEndTime" ControlToCompare="txtBeginTime" Type="Date" Operator="GreaterThanEqual"
                    ErrorMessage="起始时间应大于结束时间"></asp:CompareValidator>
            </td>
            <td width="20%">
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="button" Text="查 询" />
                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" CssClass="button" Text="添 加" />
            </td>
        </tr>
    </table>
</div>
<div id="divCenterResource" runat="server" class="index_content_view">
    <asp:Repeater ID="rpCenterResourceList" runat="server" 
            onitemdatabound="rpCenterResourceList_ItemDataBound">
        <HeaderTemplate>
            <table class="list">
                <tr>
                    <th style="width: 15%;">
                        设备编号
                    </th>
                    <th style="width: 20%;">
                        设备类型
                    </th>
                    <th style="width: 20%;">
                        支持的任务
                    </th>
                    <th style="width: 20%;">
                        最大数据处理量（兆bps）
                    </th>
                    <th style="width: 5%;">
                        状态
                    </th>
                        <th style="width: 10%;">
                        健康/占用状态
                    </th>
                    <th style="width: 5%;">
                        编辑
                    </th>
                        <th style="width: 5%;">
                        删除
                    </th>
                </tr>
                <tbody id="tbCenterResourceList">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("EquipmentCode")%>
                </td>
                <td>
                        <%# SystemParameters.GetSystemParameterText(SystemParametersType.CenterResourceEquipmentType, Eval("EquipmentType").ToString())%>
                </td>
                <td>
                    <%# Eval("SupportTask")%>
                </td>
                <td>
                    <%# Eval("DataProcess")%>
                </td>
                    <td>
                    <%# Eval("Status").ToString() == "2" ? "已删除" : "正常"%>
                </td>
                <td>
                    <asp:LinkButton ID="lbtnManageResourceStatus" runat="server" OnClick="lbtnManageResourceStatus_Click" CommandName="3"  CommandArgument='<%# Eval("EquipmentCode")%>'>管理</asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lbtnEditResource" runat="server" OnClick="lbtnEditResource_Click" CommandName="3" CommandArgument='<%# Eval("Id")%>'>编辑</asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lbtnDeleteResource" runat="server" OnClick="lbtnDeleteResource_Click" OnClientClick="javascript:return confirm('是否删除该资源？')" CommandName="3" CommandArgument='<%# Eval("Id")%>'>删除</asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody></table>
        </FooterTemplate>
    </asp:Repeater>
    <table class="listTitle">
        <tr>
            <td class="listTitle-c1">
            </td>
            <td class="listTitle-c2">
                <om:CollectionPager ID="cpCenterResourcePager" runat="server">
                </om:CollectionPager>
            </td>
        </tr>
    </table>
</div>
</asp:Content>


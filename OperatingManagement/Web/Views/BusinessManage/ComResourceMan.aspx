<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ComResourceMan.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.ComResourceMan" %>
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
 业务管理 &gt; 查询通讯资源
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
<div id="divCommunicationResource" runat="server" class="index_content_view">
    <asp:Repeater ID="rpCommunicationResourceList" runat="server" 
        onitemdatabound="rpCommunicationResourceList_ItemDataBound">
        <HeaderTemplate>
            <table class="list">
                <tr>
                    <th style="width: 20%;">
                        线路名称
                    </th>
                    <th style="width: 10%;">
                        线路编号
                    </th>
                    <th style="width: 25%;">
                        方向
                    </th>
                    <th style="width: 15%;">
                        带宽（兆bps）
                    </th>
                    <th style="width: 5%;">
                        状态
                    </th>
                        <th style="width: 11%;">
                        健康/占用状态
                    </th>
                    <th style="width: 7%;">
                        编辑
                    </th>
                        <th style="width: 7%;">
                        删除
                    </th>
                </tr>
                <tbody id="tbCommunicationResourceList">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("RouteName")%>
                </td>
                <td>
                    <%# Eval("RouteCode")%>
                </td>
                <td>
                    <%# SystemParameters.GetSystemParameterText(SystemParametersType.CommunicationResourceDirection, Eval("Direction").ToString())%>
                </td>
                <td>
                    <%# Eval("BandWidth")%>
                </td>
                    <td>
                    <%# Eval("Status").ToString() == "2" ? "已删除" : "正常"%>
                </td>
                <td>
                    <asp:LinkButton ID="lbtnManageResourceStatus" runat="server" OnClick="lbtnManageResourceStatus_Click" CommandName="2"  CommandArgument='<%# Eval("RouteCode")%>'>管理</asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lbtnEditResource" runat="server" OnClick="lbtnEditResource_Click" CommandName="2" CommandArgument='<%# Eval("Id")%>'>编辑</asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lbtnDeleteResource" runat="server" OnClick="lbtnDeleteResource_Click" OnClientClick="javascript:return confirm('是否删除该资源？')" CommandName="2" CommandArgument='<%# Eval("Id")%>'>删除</asp:LinkButton>
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
                <om:CollectionPager ID="cpCommunicationResourcePager" runat="server">
                </om:CollectionPager>
            </td>
        </tr>
    </table>
</div>
</asp:Content>

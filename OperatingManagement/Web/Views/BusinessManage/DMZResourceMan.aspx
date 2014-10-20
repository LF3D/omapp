<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="DMZResourceMan.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.DMZResourceMan" %>
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
 业务管理 &gt; 查询地面站资源
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
    <div id="divGroundResource" runat="server" class="index_content_view">
        <asp:Repeater ID="rpGroundResourceList" runat="server" 
            onitemdatabound="rpGroundResourceList_ItemDataBound">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <th style="width: 20%;">
                            地面站名称
                        </th>
                        <th style="width: 6%;">
                            地面站编号
                        </th>
                        <th style="width: 10%;">
                            设备名称
                        </th>
                        <th style="width: 6%;">
                            设备编号
                        </th>
                        <th style="width: 12%;">
                            站址坐标
                        </th>
                        <th style="width: 6%;">
                            光学设备
                        </th>
                        <th style="width: 5%;">
                            状态
                        </th>
                        <th style="width: 15%;">
                            功能类型
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
                    <tbody id="tbGroundResourceList">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("DMZName")%>
                    </td>
                    <td>
                        <%# Eval("DMZCode")%>
                    </td>
                    <td>
                        <%# Eval("EquipmentName")%>
                    </td>
                    <td>
                        <%# Eval("EquipmentCode")%>
                    </td>
                    <td>
                        <%#Eval("Coordinate").ToString()%>
                    </td>
                     <td>
                        <%# SystemParameters.GetSystemParameterText(SystemParametersType.GroundResourceOpticalEquipment, Eval("OpticalEquipment").ToString())%>
                    </td>
                    <td>
                       <%# Eval("Status").ToString() == "2" ? "已删除" : "正常"%>
                    </td>
                    <td>
                        <%# GetGroundResourceFunctionType(Eval("FunctionType").ToString())%>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnManageResourceStatus" runat="server" OnClick="lbtnManageResourceStatus_Click" CommandName="1"  CommandArgument='<%# Eval("EquipmentCode")%>'>管理</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnEditResource" runat="server" OnClick="lbtnEditResource_Click" CommandName="1" CommandArgument='<%# Eval("Id")%>'>编辑</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnDeleteResource" runat="server" OnClick="lbtnDeleteResource_Click" OnClientClick="javascript:return confirm('是否删除该资源？')" CommandName="1" CommandArgument='<%# Eval("Id")%>'>删除</asp:LinkButton>
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
                    <om:CollectionPager ID="cpGroundResourcePager" runat="server">
                    </om:CollectionPager>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

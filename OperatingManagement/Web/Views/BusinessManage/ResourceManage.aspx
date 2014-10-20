<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResourceManage.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.ResourceManage" %>
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
 业务管理 &gt; 查询资源
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
 <%-- <script type="text/javascript">
        $(function () {
            $("#txtBeginTime").datepicker();
            $("#txtEndTime").datepicker();
        });
    </script>--%>
 <div class="index_content_search">
     <table cellspacing="0" cellpadding="0" class="listTitle">
         <tr>
             <th width="15%">
                 资源类型：
             </th>
             <td width="25%">
                 <asp:DropDownList ID="dplResourceType" runat="server" CssClass="norDpl">
                 </asp:DropDownList>
             </td>
             <th width="15%">
                 资源健康/占用状态：
             </th>
             <td width="25%">
                 <asp:DropDownList ID="dplResourceStatus" runat="server" CssClass="norDpl">
                 </asp:DropDownList>
             </td>
             <td width="20%">
             </td>
         </tr>
         <tr>
             <th>
                 起始时间：
             </th>
             <td>
                 <asp:TextBox ID="txtBeginTime" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                     CssClass="norText"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                     ForeColor="Red" ControlToValidate="txtBeginTime" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
             </td>
             <th>
                 结束时间：
             </th>
             <td>
                 <asp:TextBox ID="txtEndTime" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                     CssClass="norText"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic"
                     ForeColor="Red" ControlToValidate="txtEndTime" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                 <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtEndTime" ControlToCompare="txtBeginTime" Type="Date" Operator="GreaterThanEqual"
                     ErrorMessage="起始时间应大于结束时间"></asp:CompareValidator>
             </td>
             <td>
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
                        <th style="width: 10%;">
                            地面站名称
                        </th>
                        <th style="width: 8%;">
                            地面站编号
                        </th>
                        <th style="width: 10%;">
                            设备名称
                        </th>
                        <th style="width: 7%;">
                            设备编号
                        </th>
                        <th style="width: 7%;">
                            管理单位
                        </th>
                        <th style="width: 15%;">
                            站址坐标
                        </th>
                        <th style="width: 9%;">
                            是否光学设备
                        </th>
                        <th style="width: 12%;">
                            功能类型
                        </th>
                        <th style="width: 5%;">
                            状态
                        </th>
                        <th style="width: 7%;">
                            健康/占用
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
                        <%# Eval("AddrName")%>
                    </td>
                    <td>
                        <%# Eval("AddrMark")%>
                    </td>
                    <td>
                        <%# Eval("EquipmentName")%>
                    </td>
                    <td>
                        <%# Eval("EquipmentCode")%>
                    </td>
                    <td>
                        <%# SystemParameters.GetSystemParameterText(SystemParametersType.XYXSInfoOwn, Eval("Own").ToString())%>
                    </td>
                    <td>
                        <%#Eval("Coordinate").ToString()%>
                    </td>
                     <td>
                        <%# SystemParameters.GetSystemParameterText(SystemParametersType.GroundResourceOpticalEquipment, Eval("OpticalEquipment").ToString())%>
                    </td>
                    <td>
                        <%# GetGroundResourceFunctionType(Eval("FunctionType").ToString())%>
                    </td>
                    <td>
                       <%# Eval("Status").ToString() == "2" ? "已删除" : "正常"%>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnManageResourceStatus" runat="server" OnClick="lbtnManageResourceStatus_Click" CommandName="1"  CommandArgument='<%# Eval("EquipmentCode")%>'>管理状态</asp:LinkButton>
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
                        <th style="width: 9%;">
                            状态
                        </th>
                         <th style="width: 7%;">
                            健康/占用
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
                        <asp:LinkButton ID="lbtnManageResourceStatus" runat="server" OnClick="lbtnManageResourceStatus_Click" CommandName="2"  CommandArgument='<%# Eval("RouteCode")%>'>管理状态</asp:LinkButton>
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
                       <th style="width: 8%;">
                            状态
                        </th>
                         <th style="width: 7%;">
                            健康/占用
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
                       <%# Eval("Status").ToString() == "2" ? "删除" : "正常"%>
                    </td>
                   <td>
                        <asp:LinkButton ID="lbtnManageResourceStatus" runat="server" OnClick="lbtnManageResourceStatus_Click" CommandName="3"  CommandArgument='<%# Eval("EquipmentCode")%>'>管理状态</asp:LinkButton>
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

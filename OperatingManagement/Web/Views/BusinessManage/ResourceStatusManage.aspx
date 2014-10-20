<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResourceStatusManage.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.ResourceStatusManage" %>
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
业务管理 &gt; 查询资源状态
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
<%--<script type="text/javascript">
    $(function () {
        $("#txtBeginTime").datepicker();
        $("#txtEndTime").datepicker();
    });
    </script>--%>
 <div>
        <table cellspacing="0" cellpadding="0" class="listTitle">
            <tr>
                <th width="15%">
                    资源类型：
                </th>
                <td width="20%">
                    <asp:DropDownList ID="dplResourceType" runat="server" CssClass="norDpl">
                    </asp:DropDownList>
                </td>
                <th width="15%">
                    资源编号：
                </th>
                <td width="25%">
                    <asp:TextBox ID="txtResourceCode" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtResourceCode" ErrorMessage="（必填）" ValidationGroup="SearchStatus"></asp:RequiredFieldValidator>
                </td>
                <td width="25%">
                </td>
            </tr>
            <tr>
               <th>
                  起始时间：
               </th>
               <td>
                  <asp:TextBox ID="txtBeginTime" runat="server" CssClass="norText" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" ClientIDMode="Static"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtBeginTime" ErrorMessage="（必填）" ValidationGroup="SearchStatus"></asp:RequiredFieldValidator>
               </td>
               <th>
                  结束时间：
               </th>
               <td>
                 <asp:TextBox ID="txtEndTime" runat="server" CssClass="norText" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" ClientIDMode="Static"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtEndTime" ErrorMessage="（必填）" ValidationGroup="SearchStatus"></asp:RequiredFieldValidator>
               </td>
               <td>
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="button" ValidationGroup="SearchStatus" Text="查 询"/>
                    <asp:Button ID="btnViewChart" runat="server" OnClick="btnViewChart_Click" CssClass="button" Text="图 示"/>
                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" CssClass="button" Text="添 加"/>
               </td>
            </tr>
        </table>
    </div>
     <div id="divResourceStatus" class="index_content_view">
        <asp:Repeater ID="rpResourceHealthStatusList" runat="server">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <th style="width: 10%;">
                            资源类型
                        </th>
                        <th style="width: 15%;">
                            资源名称
                        </th>
                        <th style="width: 10%;">
                            资源编号
                        </th>
                        <th style="width: 15%;">
                            功能类型
                        </th>
                        <th style="width: 10%;">
                            健康状态
                        </th>
                        <th style="width: 15%;">
                            起始时间
                        </th>
                        <th style="width: 15%;">
                            结束时间
                        </th>
                        <th style="width: 10%;">
                            编辑
                        </th>
                    </tr>
                    <tbody id="tbResourceHealthStatusList">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# SystemParameters.GetSystemParameterText(SystemParametersType.ResourceType,Eval("ResourceType").ToString())%>
                    </td>
                    <td>
                        <%# Eval("ResourceName")%>
                    </td>
                    <td>
                        <%# Eval("ResourceCode")%>
                    </td>
                    <td>
                        <%# SystemParameters.GetSystemParameterText(SystemParametersType.HealthStatusFunctionType,Eval("FunctionType").ToString())%>
                    </td>
                    <td>
                        <%# SystemParameters.GetSystemParameterText(SystemParametersType.HealthStatus, Eval("Status").ToString())%>
                    </td>
                    <td>
                         <%# Eval("BeginTime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                    </td>
                    <td>
                        <%# Eval("EndTime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                    </td>
                     <td>
                        <asp:LinkButton ID="lbtnEditResourceStatus" runat="server" OnClick="lbtnEditResourceStatus_Click" CommandName="1" CommandArgument='<%# Eval("Id")%>'>编辑</asp:LinkButton>
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
                    <om:CollectionPager ID="cpResourceHealthStatusPager" runat="server">
                    </om:CollectionPager>
                </td>
            </tr>
        </table>
        <br />
         <asp:Repeater ID="rpResourceUseStatusList" runat="server">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                         <th style="width: 8%;">
                            资源类型
                        </th>
                        <th style="width: 10%;">
                            资源名称
                        </th>
                        <th style="width:8%;">
                            资源编号
                        </th>
                        <th style="width:7%;">
                            占用类型
                        </th>
                        <th style="width:13%;">
                            起始时间
                        </th>
                        <th style="width:13%;">
                            结束时间
                        </th>
                          <th style="width:8%;">
                            服务对象
                        </th>
                         <th style="width:8%;">
                            服务种类
                        </th>
                         <th style="width:10%;">
                            占用原因
                        </th>
                         <th style="width:10%;">
                            是否可执行任务
                        </th>
                        <th style="width:5%;">
                            编辑
                        </th>
                    </tr>
                    <tbody id="tbResourceHealthStatusList">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                   <td>
                        <%# SystemParameters.GetSystemParameterText(SystemParametersType.ResourceType,Eval("ResourceType").ToString())%>
                    </td>
                    <td>
                        <%# Eval("ResourceName")%>
                    </td>
                    <td>
                        <%# Eval("ResourceCode")%>
                    </td>
                    <td>
                        <%# SystemParameters.GetSystemParameterText(SystemParametersType.UseStatusUsedType, Eval("UsedType").ToString())%>
                    </td>
                    <td>
                         <%# Eval("BeginTime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                    </td>
                    <td>
                        <%# Eval("EndTime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                    </td>
                     <td>
                        <%# Eval("UsedBy")%>
                    </td>
                      <td>
                        <%# Eval("UsedCategory")%>
                    </td>
                     </td>
                      <td>
                        <%# Eval("UsedFor")%>
                    </td>
                     </td>
                      <td>
                        <%#SystemParameters.GetSystemParameterText(SystemParametersType.UseStatusCanBeUsed, Eval("CanBeUsed").ToString())%>
                    </td>
                     <td>
                        <asp:LinkButton ID="lbtnEditResourceStatus" runat="server" OnClick="lbtnEditResourceStatus_Click" CommandName="2" CommandArgument='<%# Eval("Id")%>'>编辑</asp:LinkButton>
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
                    <om:CollectionPager ID="cpResourceUseStatusPager" runat="server">
                    </om:CollectionPager>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

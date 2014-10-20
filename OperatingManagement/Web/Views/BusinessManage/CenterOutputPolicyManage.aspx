<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CenterOutputPolicyManage.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.CenterOutputPolicyManage" %>
<%@ Register src="../../ucs/ucTask.ascx" tagname="ucTask" tagprefix="uc1" %>
<%@ Register src="../../ucs/ucSatellite.ascx" tagname="ucSatellite" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="bizmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuBusiness" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    中心输出策略管理 &gt; 查询中心输出策略
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="index_content_search">
        <table cellspacing="0" cellpadding="0" class="searchTable">
            <tr>
                <th width="15%">
                    任务名称：
                </th>
                <td width="25%">
                    <uc1:ucTask ID="dplTask" runat="server" />
                </td>
                <th width="15%">
                    卫星名称：
                </th>
                <td width="25%">
                    <uc4:ucSatellite ID="dplSatellite" runat="server" />
                </td>
                <td width="20%">
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="button" Text="查 询" />
                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" CssClass="button" Text="添 加"/>
                </td>
            </tr>
        </table>
    </div>
    <div class="index_content_view">
        <asp:Repeater ID="rpCOPList" runat="server">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <th style="width: 15%;">
                            任务名称
                        </th>
                        <th style="width: 10%;">
                            卫星名称
                        </th>
                        <th style="width: 15%;">
                            信源
                        </th>
                        <th style="width: 15%;">
                            信息类别
                        </th>
                        <th style="width: 15%;">
                            信宿
                        </th>
                        <th style="width: 10%;">
                            生效时间
                        </th>
                        <th style="width: 10%;">
                            失效时间
                        </th>
                        <th style="width: 5%;">
                            编辑
                        </th>
                        <th style="width: 5%;">
                            导出文件
                        </th>
                    </tr>
                    <tbody id="tbCOPList">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#GetTaskName(Eval("TaskID").ToString())%>
                    </td>
                    <td>
                        <%#GetSatelliteWXMC(Eval("SatName").ToString())%>
                    </td>
                    <td>
                        <%#GetXYXSADDRName(Convert.ToInt32(Eval("InfoSource")))%>
                    </td>
                    <td>
                         <%#GetXXTypeDATANAME(Convert.ToInt32(Eval("InfoType")))%>
                    </td>
                    <td>
                        <%#GetXYXSADDRName(Convert.ToInt32(Eval("Destination")))%>
                    </td>
                    <td>
                        <%# Eval("EffectTime", "{0:" + this.SiteSetting.DateFormat + "}")%>
                    </td>
                    <td>
                        <%# Eval("DefectTime", "{0:" + this.SiteSetting.DateFormat + "}")%>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnEdit" runat="server" OnClick="lbtnEdit_Click" CommandArgument='<%# Eval("Id")%>'>编辑</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnDownload" runat="server" OnClick="lbtnDownload_Click" CommandArgument='<%# Eval("Id")%>'>导出</asp:LinkButton>
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
                    <om:CollectionPager ID="cpPager" runat="server">
                    </om:CollectionPager>
                </td>
            </tr>
        </table> 
    </div>
</asp:Content>
 
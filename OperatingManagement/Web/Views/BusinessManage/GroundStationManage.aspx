<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="GroundStationManage.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.GroundStationManage" %>
<%@ Import Namespace="OperatingManagement.DataAccessLayer.BusinessManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="bizmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuBusiness" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 地面站管理
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="index_content_search">
        <table cellspacing="0" cellpadding="0" class="listTitle">
            <tr>
                <th width="15%">
                    地面站名称：
                </th>
                <td width="25%">
                    <asp:TextBox ID="txtAddrName" runat="server" ClientIDMode="Static" MaxLength="50"
                        CssClass="norText"></asp:TextBox>
                </td>
                <th width="15%">
                    地面站编号：
                </th>
                <td width="25%">
                    <asp:TextBox ID="txtAddrMark" runat="server" ClientIDMode="Static" MaxLength="10"
                        CssClass="norText"></asp:TextBox>
                </td>
                <td width="20%">
                </td>
            </tr>
            <tr>
                <th>
                    管理单位：
                </th>
                <td>
                    <asp:DropDownList ID="dplOwn" runat="server" CssClass="norDpl">
                    </asp:DropDownList>
                </td>
                <th>
                    地面站状态：
                </th>
                <td>
                    <asp:DropDownList ID="dplStatus" runat="server" CssClass="norDpl">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="button" Text="查 询" />
                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" CssClass="button" Text="添 加" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divGroundStation" class="index_content_view">
        <asp:Repeater ID="rpGroundStationList" runat="server" OnItemDataBound="rpGroundStationList_ItemDataBound">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <th style="width: 28%;">
                            地面站名称
                        </th>
                        <th style="width: 8%;">
                            地面站编号
                        </th>
                        <th style="width: 8%;">
                            内部编码
                        </th>
                        <th style="width: 12%;">
                            外部编码
                        </th>
                        <th style="width: 8%;">
                            管理单位
                        </th>
                        <th style="width: 15%;">
                            站址坐标
                        </th>
                        <th style="width: 5%;">
                            状态
                        </th>
                        <th style="width: 8%;">
                            编辑
                        </th>
                        <th style="width: 8%;">
                            删除
                        </th>
                    </tr>
                    <tbody id="tbGroundStationList">
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
                        <%# Eval("InCode")%>
                    </td>
                    <td>
                        <%# Eval("ExCode")%>
                    </td>
                    <td>
                        <%# SystemParameters.GetSystemParameterText(SystemParametersType.XYXSInfoOwn, Eval("Own").ToString())%>
                    </td>
                    <td>
                        <%#Eval("Coordinate").ToString()%>
                    </td>
                    <td>
                         <%# SystemParameters.GetSystemParameterText(SystemParametersType.XYXSInfoStatus, Eval("Status").ToString())%>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnEditGroundStation" runat="server" OnClick="lbtnEditGroundStation_Click"
                            CommandName="1" CommandArgument='<%# Eval("Id")%>'>编辑</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnDeleteGroundStation" runat="server" OnClick="lbtnDeleteGroundStation_Click"
                            OnClientClick="javascript:return confirm('是否删除该地面站？')" CommandName="1" CommandArgument='<%# Eval("Id")%>'>删除</asp:LinkButton>
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
                    <om:CollectionPager ID="cpGroundStationPager" runat="server">
                    </om:CollectionPager>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

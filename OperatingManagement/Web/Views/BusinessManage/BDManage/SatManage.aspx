<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SatManage.aspx.cs"
    Inherits="OperatingManagement.Web.Views.BusinessManage.BDManage.SatManage" %>
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
    基础数据管理 &gt; 卫星管理
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="index_content_search">
        <table cellspacing="0" cellpadding="0" class="listTitle">
            <tr>
                <th width="15%">
                    卫星名称：
                </th>
                <td width="25%">
                    <asp:TextBox ID="txtWXMC" runat="server" ClientIDMode="Static" MaxLength="50" CssClass="norText"></asp:TextBox>
                </td>
                <th width="15%">
                    卫星编码：
                </th>
                <td width="25%">
                    <asp:TextBox ID="txtWXBM" runat="server" ClientIDMode="Static" MaxLength="10" CssClass="norText"></asp:TextBox>
                </td>
                <td width="20%">
                </td>
            </tr>
            <tr>
                <th>
                    卫星标识：
                </th>
                <td>
                    <asp:TextBox ID="txtWXBS" runat="server" ClientIDMode="Static" MaxLength="10" CssClass="norText"></asp:TextBox>
                </td>
                <th>
                    卫星状态：
                </th>
                <td>
                    <asp:DropDownList ID="dplState" runat="server" CssClass="norDpl">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="button" Text="查 询" />
                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" CssClass="button" Text="添 加" />
                </td>
            </tr>
        </table>
    </div>
    <div class="index_content_view">
        <asp:Repeater ID="rpSatelliteList" runat="server">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <th style="width: 10%">
                            卫星名称
                        </th>
                        <th style="width: 10%">
                            卫星编码
                        </th>
                        <th style="width: 10%">
                            卫星标识
                        </th>
                        <th style="width: 10%">
                            面质比
                        </th>
                        <th style="width: 10%">
                            表面反射系数
                        </th>
                        <th style="width: 5%">
                            形状
                        </th>
                        <th style="width: 12%">
                            直径
                        </th>
                        <th style="width: 13%">
                            长度
                        </th>
                        <th style="width: 10%">
                            表面情况
                        </th>
                        <th style="width: 5%">
                            状态
                        </th>
                        <th style="width: 5%">
                            编辑
                        </th>
                    </tr>
                    <tbody id="tbTasks">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("WXMC") %>
                    </td>
                    <td>
                        <%# Eval("WXBM") %>
                    </td>
                    <td>
                        <%# Eval("WXBS") %>
                    </td>
                    <td>
                        <%# Eval("SM") %>
                    </td>
                    <td>
                        <%# Eval("Ref") %>
                    </td>
                    <td>
                        <%# GetShapeName(Eval("Shape").ToString())%>
                    </td>
                    <td>
                        <%# Eval("D") %>
                    </td>
                    <td>
                        <%# Eval("L") %>
                    </td>
                    <td>
                        <%# GetRGName(Eval("RG").ToString())%>
                    </td>
                    <td>
                        <%# SystemParameters.GetSystemParameterText(SystemParametersType.SatelliteState, Eval("State").ToString())%>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnEditSatellite" runat="server" OnClick="lbtnEditSatellite_Click"
                            CommandName="1" CommandArgument='<%# Eval("Id")%>'>编辑</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <table class="listTitle">
            <tr>
                <td class="listTitle-c2" align="right">
                    <om:CollectionPager ID="cpSatellitePager" runat="server">
                    </om:CollectionPager>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="ZYGNEdit.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.BDManage.ZYGNEdit" %>

<%@ Register Src="../../../ucs/ucSatellite.ascx" TagName="ucSatellite" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="bizmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuBusiness" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    基础数据管理 &gt; 修改资源功能
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="edit" style="width: 800px;">
        <tr>
            <th style="width: 100px;">
                名称(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtName" runat="server" Width="300px" CssClass="text" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv1" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtName" ErrorMessage="必须填写“名称”。"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th style="width: 100px;">
                编码(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtCode" runat="server" Width="300px" CssClass="text" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv2" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtCode" ErrorMessage="必须填写“编码”。"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th style="width: 100px;">
                匹配准则(<span class="red">*</span>)
            </th>
            <td>
                <asp:Repeater ID="rpPPZZ" runat="server" OnItemCommand="rpPPZZ_ItemCommand"
                        OnItemDataBound="rpPPZZ_ItemDataBound">
                        <HeaderTemplate>
                            <table class="edit1">
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width: 100px;">属性</td>
                                <td style="width: 200px;" colspan="2" >
                                    <asp:DropDownList ID="ddlZYSX" runat="server" Width="180px" DataTextField="Text" DataValueField="Value">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top;">
                                    卫星
                                    <br />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLogic" runat="server" Width="180px">
                                        <asp:ListItem Value="LessThan">&lt;</asp:ListItem>
                                        <asp:ListItem Value="LessThanEqual">&lt;=</asp:ListItem>
                                        <asp:ListItem Value="Equal">=</asp:ListItem>
                                        <asp:ListItem Value="MoreThanEqual">&gt;=</asp:ListItem>
                                        <asp:ListItem Value="MoreThan">&gt;</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px;">
                                    地面站资源
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="right">
                                    <asp:Button ID="btnAdd" runat="server" CssClass="button" CausesValidation="false" CommandName="Add" Text="添加" />
                                    <asp:Button ID="btnDel" runat="server" CssClass="button" CausesValidation="false" CommandName="Del" Text="删除" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
            </td>
        </tr>
        <tr>
            <th>
                &nbsp;
            </th>
            <td>
                <asp:Label ID="ltMessage" runat="server" CssClass="error" Text="“名称”、“编码”必须唯一。"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                &nbsp;
            </th>
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="保存" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnEmpty" runat="server" CssClass="button" Text="重置" CausesValidation="False"
                    OnClick="btnEmpty_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnReturn" class="button" runat="server" 
                    Text="返回" onclick="btnReturn_Click" CausesValidation="False" />
                    <asp:HiddenField ID="hfID" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
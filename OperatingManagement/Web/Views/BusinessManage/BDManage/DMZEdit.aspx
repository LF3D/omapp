<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="DMZEdit.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.BDManage.DMZEdit" %>
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
    基础数据管理 &gt; 编辑地面站
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="edit" style="width: 800px;">
        <tr>
            <th style="width: 200px;">
                地面站名称(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtName" runat="server" Width="300px" CssClass="text" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv1" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtName" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                地面站编码(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtCode" runat="server" Width="300px" CssClass="text" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv2" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtCode" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                管理单位(<span class="red">*</span>)
            </th>
            <td>
                <asp:RadioButtonList ID="rblOwner" runat="server" BorderColor="White" 
                    BorderStyle="Double" BorderWidth="2px" RepeatDirection="Horizontal" ClientIDMode="Static">
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th>
                单位编码(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtDWCode" runat="server" Width="300px" CssClass="text" MaxLength="2"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv4" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtDWCode" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="rev3" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtDWCode" ErrorMessage="（请输入2位数字）" ValidationExpression="\d{1,2}"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr id="trMessage" runat="server" visible="false">
            <th>
                &nbsp;
            </th>
            <td>
                <asp:Label ID="lblMessage" runat="server" CssClass="error" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                &nbsp;
            </th>
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="提 交" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnReset" runat="server" CssClass="button" Text="重 置" OnClick="btnReset_Click"
                    CausesValidation="false" />&nbsp;&nbsp;
                <asp:Button ID="btnReturn" runat="server" CssClass="button" Text="返 回" OnClick="btnReturn_Click"
                    CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Content>

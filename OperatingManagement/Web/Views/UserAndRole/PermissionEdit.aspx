<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PermissionEdit.aspx.cs" Inherits="OperatingManagement.Web.Views.UserAndRole.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="usernrole" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuUserNRole" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    权限管理 &gt; 编辑权限
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="edit" style="width:700px;">
        <tr>
            <th style="width:100px;">名称(<span class="red">*</span>)</th>
            <td>
                <asp:TextBox ID="txtName" runat="server" Width="300px" CssClass="text" MaxLength="20"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv1" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtName" ErrorMessage="必须填写“名称”。"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="fev1" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtName" ErrorMessage="只能输入字母、数字、横杠、下划线，最小长度为6个字符。"
                     ValidationExpression="^[a-zA-Z][\-\w]{5,20}$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>描述(<span class="red">*</span>)</th>
            <td>
                <asp:TextBox ID="txtNote" runat="server" Width="300px"  CssClass="text" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtNote" ErrorMessage="必须填写“描述”。"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>操作</th>
            <td>
                
                <asp:CheckBoxList ID="cblActions" runat="server" RepeatDirection="Horizontal">
                </asp:CheckBoxList>
                
            </td>
        </tr>
        <tr id="trMessage" runat="server" visible="false">
            <th>&nbsp;</th>
            <td>
                <asp:Label ID="ltMessage" runat="server" CssClass="error" Text="填写“名称”（唯一）并选择相应的“操作”即可编辑权限。"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="提交" 
                    onclick="btnSubmit_Click" />&nbsp;&nbsp;<button class="button" onclick="window.location.href = 'permissions.aspx';return false;">返回</button> 
            </td>
        </tr>
    </table>
</asp:Content>

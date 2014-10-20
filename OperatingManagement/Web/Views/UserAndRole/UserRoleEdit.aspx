<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserRoleEdit.aspx.cs" Inherits="OperatingManagement.Web.Views.UserAndRole.UserRoleEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="usernrole" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuUserNRole" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    用户管理 &gt; 分配角色
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="edit" style="width:700px;">
        <tr>
            <th style="width:100px;">用户</th>
            <td>
                <asp:Literal ID="ltUserName" runat="server"></asp:Literal>    
            </td>
        </tr>
        <tr>
            <th>角色</th>
            <td>
                <asp:HiddenField ID="hfRoles" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfUserID" runat="server"  ClientIDMode="Static" />
                <asp:Repeater ID="rpRoles" runat="server">
                    <HeaderTemplate>
                        <table class="list" id="tbRoles">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <ul class="taskList">
                                    <li>
                                        <input type="checkbox" id="chkRole<%# Eval("Id") %>" value="<%# Eval("Id") %>" />
                                        <span class="spanTaskNotes" style="width:auto;"><%# Eval("RoleName") %></span>
                                    </li>
                                </ul>
                                <br class="clear" />
                                <span class="darkgray"><%# Eval("Note") %></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>                    
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td>
                <asp:Label ID="ltMessage" runat="server" CssClass="error" Text="可为用户指定多个“角色”。"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="提交" 
                    OnClientClick="return verifyRole();" onclick="btnSubmit_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnReturn" class="button" runat="server" 
                    Text="返回" onclick="btnReturn_Click" CausesValidation="False" />
            </td>
        </tr>
    </table>
    <div id="dialog-form" style="display:none" title="提示信息">
	    <p class="content"></p>
    </div>
</asp:Content>


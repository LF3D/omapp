<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleAdd.aspx.cs" Inherits="OperatingManagement.Web.Views.UserAndRole.RoleAdd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="usernrole" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuUserNRole" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    用户管理 &gt; 新增角色
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="edit" style="width:700px;">
        <tr>
            <th style="width:100px;">名称(<span class="red">*</span>)</th>
            <td>
                <asp:TextBox ID="txtName" runat="server" Width="300px" CssClass="text" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv1" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtName" ErrorMessage="必须填写“名称”。"></asp:RequiredFieldValidator>
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
            <th>权限</th>
            <td>
                <asp:HiddenField ID="hfModuleTasks" runat="server" ClientIDMode="Static" />
                <asp:Repeater ID="rpModules" runat="server" OnItemDataBound="rpModules_ItemDataBound">
                    <HeaderTemplate>
                        <table class="list" id="tbModuleTasks">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <th style="width:80px; vertical-align:middle"><%# Eval("ModuleNote") %></th>
                            <td>
                                <asp:Repeater ID="rpTasks" runat="server">
                                    <HeaderTemplate>
                                        <ul class="taskList">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <input type="checkbox" id="chkPermission<%# Eval("Id") %>" value="<%# Eval("Id") %>" />
                                            <span class="spanTaskNotes"><%# Eval("Task.TaskNote") %></span>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                        <br class="clear" />
                                    </FooterTemplate>
                                </asp:Repeater>
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
                <asp:Label ID="ltMessage" runat="server" CssClass="error" Text="填写“名称”（唯一）并选择相应的“权限”即可创建角色。"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="提交" 
                    onclick="btnSubmit_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnEmpty" runat="server" CssClass="button" Text="清空" CausesValidation=false
                    onclick="btnEmpty_Click" />
            </td>
        </tr>
    </table>
</asp:Content>

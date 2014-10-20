<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Permissions.aspx.cs" Inherits="OperatingManagement.Web.Views.UserAndRole.Permissions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="usernrole" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuUserNRole" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    权限管理 &gt; 权限列表
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="listTitle">
        <tr>
            <td class="listTitle-c1" valign="middle"><div>关键字&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button CssClass="button" ID="btnSearch" runat="server" OnClick="btnSearch_Click"
                        Text="查询" /></div>
            </td>
            <td class="listTitle-c2"></td>
        </tr>
    </table>
    <asp:Repeater ID="rpRermissions" runat="server">
        <HeaderTemplate>
            <table class="list">
                <tr>
                    <th style="width:20px;"><input type="checkbox" onclick="checkAll(this)" /></th>
                    <th style="width:200px;">权限名</th>
                    <th>描述</th>
                    <th style="width:150px;">创建时间</th>
                    <th style="width:70px;">操作</th>
                </tr>  
                <tbody id="tbPermissions">        
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><input type="checkbox" <%# HasDeletePermission()?"disabled=\"true\"":"" %> name="chkDelete" value="<%# Eval("Id") %>" /></td>
                <td><%# Eval("ModuleName") %></td>
                <td><%# Eval("ModuleNote") %></td>
                <td><%# Eval("CTime","{0:"+this.SiteSetting.DateTimeFormat+"}") %></td>
                <td>
                    <button class="button" onclick="return editPermission('<%# Eval("Id") %>')">编辑</button>
                </td>
            </tr>            
        </ItemTemplate>
        <FooterTemplate>   
                </tbody>           
            </table>            
        </FooterTemplate>
    </asp:Repeater>
    <table class="listTitle">
        <tr>
            <td class="listTitle-c1">
                <button class="button" onclick="return selectAll();">全选</button>&nbsp;&nbsp;
                <button class="button" onclick="return deletePermissions();">删除</button>          
            </td>
            <td class="listTitle-c2">
                <om:CollectionPager ID="cpPager" runat="server" ></om:CollectionPager>
            </td>
        </tr>
    </table>
    <div id="dialog-form" style="display:none" title="提示信息">
	    <p class="content"></p>
    </div>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="DMZManage.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.BDManage.DMZManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="bizmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuBusiness" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    基础数据管理 &gt; 查看地面站
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:Repeater ID="rpTasks" runat="server">
        <HeaderTemplate>
            <table class="list">
                <tr>
                    <th style="width:20%;">地面站名称</th>
                    <th style="width:15%;">地面站编码</th>
                    <th style="width:15%;">管理单位</th>
                    <th style="width:10%;">单位编码</th>
                    <th style="width:10%;">操作</th>
                </tr>  
                <tbody id="tbTasks">        
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("DMZName") %></td>
                <td><%# Eval("DMZCode") %></td>
                <td><%# Eval("Owner").ToString() == "1" ? "总参" : (Eval("Owner").ToString() == "2" ? "总装" : "遥科学站")%></td>
                <td><%# Eval("DWCode") %></td>
                <td>
                    <button class="button" onclick="window.location.href = '/views/BusinessManage/BDManage/DMZEdit.aspx?rid=<%# Eval("Id") %>';return false;">编辑</button>
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
            <td class="listTitle-c2" align="right">
                <om:CollectionPager ID="cpPager" runat="server"></om:CollectionPager>
            </td>
        </tr>
    </table>
</asp:Content>

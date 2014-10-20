<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="ZYSXManage.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.BDManage.ZYSXManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="bizmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuBusiness" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    基础数据管理 &gt; 查看资源属性
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:Repeater ID="rpZY" runat="server">
        <HeaderTemplate>
            <table class="list">
                <tr>
                    <th style="width:100px;">属性名称</th>
                    <th style="width:100px;">属性编码</th>
                    <th style="width:100px;">属性类型</th>
                    <th style="width:100px;">属性值区间</th>
                    <th style="width:100px;">属性归属</th>
                    <th style="width:70px;">操作</th>
                </tr>  
                <tbody id="tbTasks">        
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("PName")%></td>
                <td><%# Eval("PCode")%></td>
                <td><%# GetTypeName( Eval("Type").ToString() )%></td>
                <td><%# Eval("Scope")%></td>
                <td><%# GetOwnName (Eval("Own").ToString() )%></td>
                <td>
                    <button class="button" onclick="window.location.href = '/views/BusinessManage/BDManage/ZYSXEdit.aspx?id=<%# Eval("Id") %>';return false;">编辑</button>
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
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="GetFileRcvInfos.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.GetFileRcvInfos" %>
<%@ Register src="../../ucs/ucInfoType.ascx" tagname="ucInfoType" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="fileserver" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuFS" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 查看文件接收记录
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="listTitle" width="1500px">
        <tr>
            <td class="listTitle-c1" width="85%" id="tdFilter"><div>信息类型<uc2:ucInfoType ID="ddlInfoType" runat="server" 
                    AllowBlankItem="True" />
                开始日期<asp:TextBox ID="txtFrom" ClientIDMode="Static" CssClass="text"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"  
                    runat="server"></asp:TextBox>
                结束日期<asp:TextBox ID="txtTo" ClientIDMode="Static" CssClass="text"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" 
                    runat="server"></asp:TextBox>
                </div>
            </td>
            <td width="15%"><asp:Button CssClass="button" ID="btnSearch" runat="server" OnClick="btnSearch_Click"
                        Text="查询" />&nbsp;&nbsp;<button class="button" onclick="return clearField();">清空</button></td>
        </tr>
    </table>
    <asp:Repeater ID="rpDatas" runat="server">
        <HeaderTemplate>
            <table class="list">
                <tr>
                    <th style="width:3.5%;">记录号</th>
                    <th style="width:13%;">文件名</th>
                    <th style="width:12%;">文件路径</th>
                    <th style="width:4.5%;">接收方式</th>
                    <th style="width:14%;">发送方</th>
                    <th style="width:10%;">信息类型</th>
                    <th style="width:14%;">接收方</th>
                    <th style="width:3.5%;">状态</th>
                    <th style="width:10%;">备注</th>
                    <th style="width:7%;">接收开始时间</th>
                    <th style="width:7.5%;">接收结束时间</th>
                </tr>  
                <tbody id="tbRoles">        
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("id") %></td>
                <td><%# Eval("FileName") %></td>
                <td><%# Eval("FilePath") %></td>
                <td><%# Eval("ReceiveWay") %></td>
                <td><%# Eval("SenderName") %></td>
                <td><%# Eval("InfoTypeName")%></td>
                <td><%# Eval("ReceiverName")%></td>
                <td><%# Eval("ReceiveStatus")%></td>
                <td><%# Eval("Remark")%></td>
                <td><%# Eval("RecvBeginTime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%></td>
                <td><%# Eval("LastUpdateTime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%></td>
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
                <om:CollectionPager ID="cpPager" runat="server" ></om:CollectionPager>
            </td>
        </tr>
        <tr id="trMessage" runat="server" visible="false">
            <td><asp:Label ID="lblMessage" runat="server" CssClass="error" Text=""></asp:Label></td>
        </tr>
    </table>
</asp:Content>

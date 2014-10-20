<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SendFile.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.SendFile" %>
<%@ Register src="../../ucs/ucInfoType.ascx" tagname="ucInfoType" tagprefix="uc1" %>
<%@ Register src="../../ucs/ucXYXSInfo.ascx" tagname="ucXYXSInfo" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="fileserver" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuFS" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 发送文件
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="edit" style="width:800px;">
    <tr>
        <th style="width:120px;" align="right">发送方式(<span class="red">*</span>)</th>
        <td>
            <asp:RadioButtonList ID="rblSendWay" runat="server" 
                RepeatDirection="Horizontal" BorderColor="White" BorderStyle="Double" 
                BorderWidth="2px">
                <asp:ListItem Selected="true" Text="FEP UDP" Value="1"></asp:ListItem>
                <asp:ListItem Text="FEP TCP" Value="2"></asp:ListItem>
                <asp:ListItem Text="FTP" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th style="width:120px;" align="right">自动重发(<span class="red">*</span>)</th>
        <td>
            <asp:RadioButtonList ID="rblAutoResend" runat="server" 
                RepeatDirection="Horizontal" BorderColor="White" BorderStyle="Double" 
                BorderWidth="2px">
                <asp:ListItem Selected="true" Text="是" Value="1"></asp:ListItem>
                <asp:ListItem Text="否" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th style="width:120px;" align="right">信息类型(<span class="red">*</span>)</th>
        <td>
            <uc1:ucInfoType ID="ddlInfoType" runat="server" />
        </td>
    </tr>
    <tr>
        <th style="width:120px;" align="right">发送方(<span class="red">*</span>)</th>
        <td>
            <uc2:ucXYXSInfo ID="ddlSender" runat="server" />
        </td>
    </tr>
    <tr>
        <th style="width:120px;" align="right">接收方(<span class="red">*</span>)</th>
        <td>
            <uc2:ucXYXSInfo ID="ddlReceiver" runat="server" />
        </td>
    </tr>
    <tr>
        <th style="width:120px;" align="right">待发送文件(<span class="red">*</span>)</th>
        <td>
            <asp:FileUpload ID="fuToSend" ClientIDMode="Static"  runat="server" />
        </td>
    </tr>
    <tr>
        <th>&nbsp;</th>
        <td>
            <asp:Label ID="lblMessage" runat="server" CssClass="error" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <th>&nbsp;</th>
        <td>
            <asp:Button ID="btnSend" runat="server" Text="提交发送" CssClass="button" 
                onclick="btnSend_Click" />
        </td>
    </tr>
</table>
<div id="dialog-form" style="display:none" title="提示信息">
	<p class="content">
        <b>角度：</b><asp:Literal ID="ltAngle" runat="server"></asp:Literal><br />
        <b>长度：</b><asp:Literal ID="ltDist" runat="server"></asp:Literal><br />
        <b>类型：</b><asp:Literal ID="ltOrbit" runat="server"></asp:Literal><br />
        <b>参数：</b><asp:Literal ID="ltPara" runat="server"></asp:Literal><br />
        <b>计算：</b><asp:Literal ID="ltCal" runat="server"></asp:Literal><br />
    </p>
</div>
</asp:Content>

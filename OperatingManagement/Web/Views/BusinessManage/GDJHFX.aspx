<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GDJHFX.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.GDJHFX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            height: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="gdfx" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuGD" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 轨道分析 - 交会分析
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
<table class="edit" style="width:800px;">
    <tr>
        <th style="width:200px;">交会分析主星数据文件(<span class="red">*</span>)</th>
        <td>
            <asp:FileUpload ID="fuSubFile" ClientIDMode="Static" runat="server" />
            <div><span class="darkgray">交会分析主星数据文件：2X，I4，X，2（I2，X），2（I2，':'），F4.1，2X，6(F16.6，2X)</span></div>
        </td>
    </tr>
    <tr>
        <th>交会分析目标星数据文件(<span class="red">*</span>)</th>
        <td>
            <asp:FileUpload ID="fuTgtFile" ClientIDMode="Static" runat="server" />
            <div><span class="darkgray">交会分析目标星数据文件：2X，I4，X，2（I2，X），2（I2，':'），F4.1，2X，6(F16.6，2X)</span></div>
        </td>
    </tr>
    <div id="divCalResult" runat="server" visible="false">
    <tr style="height:48px;">
        <th>计算结果文件路径</th>
        <td>
            <asp:Label ID="lblResultFilePath" runat="server" Text=""></asp:Label>
            <asp:Label ID="lblResultPath" runat="server" Text="" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr style="height:24px;">
       <th class="style1">计算结果</th>
       <td class="style1">
          <asp:Label ID="lblCalResult" runat="server" Text="计算成功" ForeColor="Red"></asp:Label>
          <!--<asp:LinkButton ID="lbtnViewResult" runat="server" OnClientClick="javascript:_resultDialog.dialog('open');return false;">查看</asp:LinkButton> !-->
          &nbsp;
          <asp:LinkButton ID="lbtUNWFileDownload" runat="server" OnClick="lbtUNWFileDownload_Click" CausesValidation="false">保存UNW结果文件</asp:LinkButton>
          &nbsp;
          <asp:LinkButton ID="lbtSTWFileDownload" runat="server" OnClick="lbtSTWFileDownload_Click" CausesValidation="false">保存STW结果文件</asp:LinkButton>
          &nbsp;
          <asp:LinkButton ID="lbtViewCurves" runat="server" OnClick="lbtViewCurves_Click" CausesValidation="false">查看曲线图</asp:LinkButton>
       </td>    
    </tr>
    </div>
    <tr id="trMessage" runat="server" visible="false">
        <th>&nbsp;</th>
        <td>
            <asp:Label ID="lblMessage" runat="server" CssClass="error" Text="“数据文件”内容必须严格按照格式要求编写。"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>&nbsp;</th>
        <td>
            <asp:Button ID="btnCalculate" runat="server" Text="开始分析" CssClass="button" 
                onclick="btnCalculate_Click" />
        </td>
    </tr>
</table>
<div id="dialog-form" style="display:none" title="提示信息">
	<p class="content">
        <b>角度：</b><asp:Literal ID="ltAngle" runat="server"></asp:Literal><br />
    </p>
</div>
</asp:Content>

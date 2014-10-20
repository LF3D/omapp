<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GDCSZH.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.GDCSZH" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="gdfx" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuGD" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 轨道分析 - 参数转换
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
<table class="edit" style="width:800px;">
    <tr>
        <th style="width:100px;">角度单位(<span class="red">*</span>)</th>
        <td>
            <asp:RadioButtonList ID="rblAngle" runat="server" RepeatDirection="Horizontal" 
                BorderColor="White" BorderStyle="Double" BorderWidth="2px">
                <asp:ListItem Selected="true" Text="度" Value="True"></asp:ListItem>
                <asp:ListItem Text="弧度" Value="False"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th style="width:100px;">长度单位(<span class="red">*</span>)</th>
        <td>
            <asp:RadioButtonList ID="rblDistance" runat="server" 
                RepeatDirection="Horizontal" BorderColor="White" BorderStyle="Double" 
                BorderWidth="2px">
                <asp:ListItem Selected="true" Text="米" Value="False"></asp:ListItem>
                <asp:ListItem Text="千米" Value="True"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th style="width:100px;">转换类型(<span class="red">*</span>)</th>
        <td>
            <asp:RadioButtonList ID="rblOrbitParameters" ClientIDMode="Static" 
                runat="server" RepeatColumns="2" BorderColor="White" BorderStyle="Double" 
                BorderWidth="2px">
            
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th style="width:100px;">发射系相关文件(<span class="red">*</span>)</th>
        <td>
            <asp:FileUpload ID="fuParaFile" ClientIDMode="Static" runat="server" />
            <span class="darkgray">发射系文件格式：2X，4(F6.2，2X)</span>
        </td>
    </tr>
    <tr>
        <th style="width:100px;">待转换相关文件(<span class="red">*</span>)</th>
        <td>
            <asp:FileUpload ID="fuCalFile" ClientIDMode="Static"  runat="server" />
            <span class="darkgray">待转换文件格式：2X，I4，X，2（I2，X），2（I2，':'），F4.1，2X，6(F16.6，2X)</span>
        </td>
    </tr>
    <div id="divCalResult" runat="server" visible="false">
    <tr style="height:24px;">
        <th>计算结果文件路径</th>
        <td>
            <asp:Label ID="lblResultFilePath" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr style="height:24px;">
       <th>计算结果</th>
       <td>
          <asp:Label ID="lblCalResult" runat="server" Text="计算成功" ForeColor="Red"></asp:Label>
          <!--<asp:LinkButton ID="lbtnViewResult" runat="server" OnClientClick="javascript:_resultDialog.dialog('open');return false;">查看</asp:LinkButton> !-->
          <asp:LinkButton ID="lbtnResultFileDownload" runat="server" OnClick="lbtnResultFileDownload_Click" CausesValidation="false">保存计算结果</asp:LinkButton>
       </td>    
    </tr>
    </div>
    <tr id="trMessage" runat="server" visible="false">
        <th>&nbsp;</th>
        <td>
            <asp:Label ID="lblMessage" runat="server" CssClass="error" Text="“相关文件”内容必须严格按照格式要求编写。"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>&nbsp;</th>
        <td>
            <asp:Button ID="btnCalculate" runat="server" Text="开始转换" CssClass="button" 
                onclick="btnCalculate_Click" />
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
<div id="result-form" style="display:none" title="结果文件内容">
	<p class="content">
        <b>结果文件内容：</b><asp:Literal ID="ltResultFile" runat="server"></asp:Literal><br />
    </p>
</div>
</asp:Content>

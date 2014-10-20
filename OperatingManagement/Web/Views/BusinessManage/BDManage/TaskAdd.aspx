<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="TaskAdd.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.BDManage.TaskAdd" %>
<%@ Register src="../../../ucs/ucCBLSat.ascx" tagname="ucCBLSat" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .text
        {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="bizmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuBusiness" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    基础数据管理 &gt; 新增任务
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="edit" style="width:800px;">
        <tr>
            <th style="width:100px;">任务名称(<span class="red">*</span>)</th>
            <td>
                <asp:TextBox ID="txtTaskName" runat="server" Width="300px" CssClass="text" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv1" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtTaskName" ErrorMessage="必须填写“任务名称”。"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th style="width:100px;">内部任务代号(<span class="red">*</span>)</th>
            <td>
                <asp:TextBox ID="txtTaskNo" runat="server" Width="300px" CssClass="text" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv2" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtTaskNo" ErrorMessage="必须填写“内部任务代号”。"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th style="width:100px;">外部任务代号(<span class="red">*</span>)</th>
            <td>
                <asp:TextBox ID="txtOutTaskNo" runat="server" Width="300px" CssClass="text" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv5" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtOutTaskNo" ErrorMessage="必须填写“外部任务代号”。"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th style="width:100px;">对象标识(<span class="red">*</span>)</th>
            <td>
                <asp:TextBox ID="txtObjectFlag" runat="server" Width="300px" CssClass="text" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv3" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtObjectFlag" ErrorMessage="必须填写“对象标识”。"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th style="width:100px;">卫星(<span class="red">*</span>)</th>
            <td>
                <uc1:ucCBLSat ID="ucCBLSats" runat="server" />
            </td>
        </tr>
        <tr>
            <th style="width:100px;">航天器标识(<span class="red">*</span>)</th>
            <td>
                <asp:TextBox ID="txtSCID" runat="server" Width="300px" CssClass="text" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv6" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtSCID" ErrorMessage="必须填写“航天器标识”。"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th style="width:100px;">是否有效(<span class="red">*</span>)</th>
            <td>
                <asp:RadioButtonList ID="rblIsEffective" runat="server" BorderColor="White" 
                    BorderStyle="Double" BorderWidth="2px" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Selected="True">是</asp:ListItem>
                    <asp:ListItem Value="0">否</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th style="width:100px;">发射时间(<span class="red">*</span>)</th>
            <td><asp:TextBox ID="txtEmitTime" ClientIDMode="Static" CssClass="text"  onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"  
                    runat="server" Width="179px"></asp:TextBox><span style="color:#3399FF;">毫秒数</span><asp:TextBox ID="txtMiniSeconds" 
                    ClientIDMode="Static" CssClass="text" 
                    runat="server" Width="48px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rv2" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtEmitTime" ErrorMessage="必须填写“发射时间”。"></asp:RequiredFieldValidator>
                     <asp:RequiredFieldValidator ID="rv3" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtMiniSeconds" ErrorMessage="必须填写“毫秒数”，3位数字。"></asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rv4" runat="server" Display="Dynamic" MinimumValue="0"
                    MaximumValue="999" ControlToValidate="txtMiniSeconds" Type="Integer" ForeColor="Red"
                    ErrorMessage="毫秒数必须在0至999之间"></asp:RangeValidator></td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td>
                <asp:HiddenField ID="hfUserId" runat="server" />
                <asp:Literal ID="ltHref" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td>
                <asp:Label ID="ltMessage" runat="server" CssClass="error" Text="“任务名称”、“任务代号”、“任务标识”必须唯一。"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="提交" 
                    onclick="btnSubmit_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnEmpty" runat="server" CssClass="button" Text="清空" CausesValidation="False"
                    onclick="btnEmpty_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
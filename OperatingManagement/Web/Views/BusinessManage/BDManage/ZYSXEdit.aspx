<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="ZYSXEdit.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.BDManage.ZYSXEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="bizmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuBusiness" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    基础数据管理 &gt; 修改资源属性
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="edit" style="width:800px;">
        <tr>
            <th style="width:100px;">属性名称(<span class="red">*</span>)</th>
            <td>
                <asp:TextBox ID="txtZYSXName" runat="server" Width="300px" CssClass="text" 
                    MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv1" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtZYSXName" ErrorMessage="必须填写“属性名称”。"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th style="width:100px;">属性编码(<span class="red">*</span>)</th>
            <td>
                <asp:TextBox ID="txtZYSXCode" runat="server" Width="300px" CssClass="text" 
                    MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv2" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtZYSXCode" ErrorMessage="必须填写“属性编码”。"></asp:RequiredFieldValidator>
            </td>
        </tr>

        <tr>
            <th style="width:100px;">属性类型(<span class="red">*</span>)</th>
            <td>
                <asp:RadioButtonList ID="rblType" runat="server" BorderColor="White" ClientIDMode="Static"
                    BorderStyle="Double" BorderWidth="2px" RepeatDirection="Horizontal" >
                    <asp:ListItem Value="1" Selected="True">int</asp:ListItem>
                    <asp:ListItem Value="2">double</asp:ListItem>
                    <asp:ListItem Value="3">string</asp:ListItem>
                    <asp:ListItem Value="4">bool</asp:ListItem>
                    <asp:ListItem Value="5">enum</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th style="width:100px;">属性值区间(<span class="red">*</span>)</th>
            <td><asp:TextBox ID="txtScope" ClientIDMode="Static" CssClass="text" 
                    runat="server" Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rv2" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtScope" ErrorMessage="必须填写“属性值区间”。"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="rvInt" runat="server" ClientIDMode = "Static"
                    ControlToValidate="txtScope" Display="Dynamic" ErrorMessage="请按int型说明填写" 
                    ForeColor="Red" ValidationExpression="\d{1}-\d{6}"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="rvDouble" runat="server"  ClientIDMode = "Static"
                    ControlToValidate="txtScope" Display="Dynamic" ErrorMessage="请按double型说明填写" 
                    ForeColor="Red" ValidationExpression="\d{1}.\d{1}"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="rvString" runat="server"  ClientIDMode = "Static"
                    ControlToValidate="txtScope" Display="Dynamic" ErrorMessage="请按string型说明填写" 
                    ForeColor="Red" ValidationExpression="\d{1,}"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="rvBool" runat="server"  ClientIDMode = "Static"
                    ControlToValidate="txtScope" Display="Dynamic" ErrorMessage="请按bool型说明填写" 
                    ForeColor="Red" ValidationExpression="^([\w\d\u4e00-\u9fa5])+(,[\w\d\u4e00-\u9fa5])$"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="rvEnum" runat="server"  ClientIDMode = "Static"
                    ControlToValidate="txtScope" Display="Dynamic" ErrorMessage="请按enum型说明填写" 
                    ForeColor="Red" ValidationExpression="^([\w\d\u4e00-\u9fa5])+(,[\w\d\u4e00-\u9fa5]+)*$"></asp:RegularExpressionValidator>
                <br />
                <asp:Label ID="Label1" runat="server" ForeColor="#3399FF" 
                    Text="int(0-xxxxxx);double(m.n);string(length);bool(a,b);enum(a,b,c)"></asp:Label>
            </td>
        </tr>
        <tr>
            <th style="width:100px;">属性属于(<span class="red">*</span>)</th>
            <td>
                <asp:RadioButtonList ID="rblOwn" runat="server" BorderColor="White" 
                    BorderStyle="Double" BorderWidth="2px" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0" Selected="True">卫星</asp:ListItem>
                    <asp:ListItem Value="1">地面站资源</asp:ListItem>
                    <asp:ListItem Value="2">卫星和地面站资源</asp:ListItem>
                    <asp:ListItem Value="3">都不属于</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td>
                <asp:Label ID="ltMessage" runat="server" CssClass="error" Text="“属性名称”、“属性编码”必须唯一。"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="保存" 
                    onclick="btnSubmit_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnEmpty" runat="server" CssClass="button" Text="重置" CausesValidation="False"
                    onclick="btnEmpty_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnReturn" class="button" runat="server" 
                    Text="返回" onclick="btnReturn_Click" CausesValidation="False" />
                <asp:HiddenField ID="hfID" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
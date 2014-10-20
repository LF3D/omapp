<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GDYB.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.GDYB" %>
<%@ Register src="../../ucs/ucSatellite.ascx" tagname="ucSatellite" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="gdfx" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuGD" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 轨道分析 - 轨道预报
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
<table class="edit" style="width:800px;">
    <tr>
        <th style="width:150px; text-align:right;">起始日期(<span class="red">*</span>)</th>
        <td>
            <asp:TextBox ID="txtFrom" ClientIDMode="Static" CssClass="text"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"  
                    runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="cv2" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtFrom" ErrorMessage="必须填写“起始日期”。"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th style="text-align:right;">预报时长（天）(<span class="red">*</span>)</th>
        <td>
            <asp:TextBox ID="txtDays" ClientIDMode="Static" CssClass="text"  
                    runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="rv1" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtDays" ErrorMessage="必须填写“预报时长”。"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="rev3" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtDays" ErrorMessage="必须为有效整数。"
                     ValidationExpression="((\d{1,5}))"></asp:RegularExpressionValidator></td>
    </tr>
    <tr>
        <th style="text-align:right;">预报时间间隔（秒）(<span class="red">*</span>)</th>
        <td>
            <asp:TextBox ID="txtTimeSpan" ClientIDMode="Static" CssClass="text"  
                    runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="rv2" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtTimeSpan" ErrorMessage="必须填写“预报时间间隔”。"></asp:RequiredFieldValidator>
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtTimeSpan" ErrorMessage="必须为有效整数。"
                     ValidationExpression="((\d{1,3}))"></asp:RegularExpressionValidator></td>
    </tr>
    <tr>
        <th style="text-align:right;">卫星名称(<span class="red">*</span>)</th>
        <td>
            <uc1:ucSatellite ID="ucSatellite1" runat="server" AllowBlankItem="False" />
        </td>
    </tr>
    <tr>
        <th style="text-align:right;">测站编码(<span class="red">*</span>)</th>
        <td>
            <asp:CheckBoxList ID="cblXyxs1" runat="server" BorderColor="White" 
                BorderStyle="Double" BorderWidth="2px" ClientIDMode="Static">
            </asp:CheckBoxList>
            <asp:RadioButtonList ID="rblDMZ" runat="server" BorderColor="White" 
                BorderStyle="Double" BorderWidth="2px" RepeatColumns="2" Visible="False">
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th style="text-align:right;">圈次源(<span class="red">*</span>)</th>
        <td id="tdQcy">
            <asp:RadioButton ID="rb1" runat="server" Checked="True" 
                GroupName="qcy" Text="是" />
            <asp:RadioButton ID="rb2" runat="server" GroupName="qcy" Text="否" />
        </td>
    </tr>
    <tr>
        <th style="text-align:right;">圈次（整数）(<span class="red">*</span>)</th>
        <td>
            <asp:TextBox ID="txtQC" ClientIDMode="Static" CssClass="text"
                    runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic" ForeColor="Red"
                     ControlToValidate="txtQC" ErrorMessage="必须为有效整数。"
                     ValidationExpression="((\d{1,3}))"></asp:RegularExpressionValidator></td>
    </tr>
    <tr>
        <th>&nbsp;</th>
        <td>
            <asp:Label ID="ltMessage" runat="server" CssClass="error" Text="请严格按照格式要求填写。" ClientIDMode="Static"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>&nbsp;</th>
        <td>
            <asp:Button ID="btnCalculate" runat="server" Text="开始预报" CssClass="button" CausesValidation="true"
                onclick="btnCalculate_Click" />
        </td>
    </tr>
    <div id="divCalResult" runat="server" visible="false">
        <tr style="height:24px;">
            <th>
                计算结果文件路径
            </th>
            <td>
                <asp:Label ID="lblResultFilePath" runat="server" Text=""></asp:Label>
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lbtViewCurves_Click" CausesValidation="false">查看曲线图</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                地影文件
            </th>
            <td>
                <asp:LinkButton ID="lbtEarthShadow" runat="server" OnClick="lbtEarthShadow_Click" CausesValidation="false">保存EarthShadow结果文件</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                空间位置预报J
            </th>
            <td>
                <asp:LinkButton ID="lbtMapJ" runat="server" OnClick="lbtMapJ_Click" CausesValidation="false">保存MapJ结果文件</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                空间位置预报JK
            </th>
            <td>
                <asp:LinkButton ID="lbtMapJK" runat="server" OnClick="lbtMapJK_Click" CausesValidation="false">保存MapJK结果文件</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                空间位置预报W
            </th>
            <td>
                <asp:LinkButton ID="lbtMapW" runat="server" OnClick="lbtMapW_Click" CausesValidation="false">保存MapW结果文件</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                月凌文件
            </th>
            <td>
                <asp:LinkButton ID="lbtMoonTransit" runat="server" OnClick="lbtMoonTransit_Click" CausesValidation="false">保存MoonTransit结果文件</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                观测引导文件
            </th>
            <td>
                <asp:LinkButton ID="lbtObsGuiding" runat="server" OnClick="lbtObsGuiding_Click" CausesValidation="false">保存ObsGuiding结果文件</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                测站观测量文件
            </th>
            <td>
                <asp:LinkButton ID="lbtStaObsPre" runat="server" OnClick="lbtStaObsPre_Click" CausesValidation="false">保存StaObsPre结果文件</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                进出站及航捷数据统计文件
            </th>
            <td>
                <asp:LinkButton ID="lbtStationInOut" runat="server" OnClick="lbtStationInOut_Click" CausesValidation="false">保存StationInOut结果文件</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                星下点预报
            </th>
            <td>
                <asp:LinkButton ID="lbtSubSatPoint" runat="server" OnClick="lbtSubSatPoint_Click" CausesValidation="false">保存SubSatPoint结果文件</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                太阳角度文件
            </th>
            <td>
                <asp:LinkButton ID="lbtSunAH" runat="server" OnClick="lbtSunAH_Click" CausesValidation="false">保存SunAH结果文件</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                日凌文件
            </th>
            <td>
                <asp:LinkButton ID="lbtSunTransit" runat="server" OnClick="lbtSunTransit_Click" CausesValidation="false">保存SunTransit结果文件</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <th class="style1">
                测站可见性统计文件
            </th>
            <td>
                <asp:LinkButton ID="lbtVS" runat="server" OnClick="lbtVS_Click" CausesValidation="false">保存VisibleStatistics结果文件</asp:LinkButton>
            </td>
        </tr>
    </div>
</table>
</asp:Content>

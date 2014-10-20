<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GDJSCurves.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.GDJSCurves" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 16%;
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
    业务管理 &gt; 轨道分析 - 曲线图
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
<div class="index_content_search">
<table style="width:1100px;" cellspacing="0" cellpadding="0" class="searchTable">
    <tr style="height:30px;">
        <th class="style1">结果类型(<span class="red">*</span>)</th>
        <td style="width:35%;"><select id="resulttype"></select></td>
        <th style="width:10%;">数据名称(<span class="red">*</span>)</th>
        <td style="width:35%;"><select id="dataname"></select></td>
    </tr>
    <tr style="height:30px;">
        <th class="style1">轨道分析结果数据路径</th>
        <td>
            <asp:TextBox ID="txtPath" runat="server" Width="398px"></asp:TextBox></td>
        <th>&nbsp;</th>
        <td align="right">
            <asp:Button ID="btnCurve2" runat="server" Text="查看曲线图" CssClass="button" 
                onclick="btnCurve2_Click" />
        </td>
    </tr>
    <tr style="height:30px;">
        <th class="style1">选择本地结果数据文件</th>
        <td><asp:FileUpload ID="fuPath" ClientIDMode="Static" runat="server" 
                Width="389px" /></td>
        <th>&nbsp;</th>
        <td align="right">
            <asp:Button ID="btnCurve" runat="server" Text="查看曲线图" CssClass="button" 
                onclick="btnCurve_Click" />
                <asp:HiddenField ID="hdResultType" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdDataType" ClientIDMode="Static" runat="server" />
        </td>
    </tr>
    <tr id="trMessage" runat="server" visible="false">
        <th class="style1"></th>
        <td><asp:Label ID="lbMessage" runat="server" Text="" CssClass="error"></asp:Label></td>
    </tr>
</table>
</div>
<div id="divChart" runat="server" visible="false" class="index_content_view">
    <asp:CHART id="ChartCurve" runat="server" Palette="BrightPastel" 
        BackColor="AliceBlue" ImageType="Png" 
        ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" Width="1100px" Height="600px" 
        BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2" 
        BorderColor="181, 64, 1" BackSecondaryColor="LightBlue">
		<legends>
			<asp:Legend Enabled="False" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold"></asp:Legend>
		</legends>
		<borderskin SkinStyle="Emboss"></borderskin>
		<series>
			<asp:Series ChartArea="ChartArea1" MarkerSize="8" BorderWidth="3" XValueType="DateTime" YValueType="Double" Name="Series1" ChartType="FastLine" MarkerStyle="Circle" ShadowColor="Black" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" ShadowOffset="2"></asp:Series>
		</series>
		<chartareas>
			<asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" 
                BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="White" 
                ShadowColor="Transparent" BackGradientStyle="TopBottom">
				<axisy LineColor="64, 64, 64, 64">
					<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
					<MajorGrid LineColor="64, 64, 64, 64" />
				</axisy>
				<axisx LineColor="64, 64, 64, 64">
					<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Interval="Auto" />
					<MajorGrid LineColor="64, 64, 64, 64" />
				</axisx>
			</asp:ChartArea>
		</chartareas>
	</asp:CHART>
</div>
<div id="dialog-station" style="display: none" title="查看进出站及航捷数据">
    <asp:Repeater ID="rpStation" runat="server">
        <HeaderTemplate>
                <table class="list" style="width: 1500px">
                    <tr>
                        <th style="width: 100px;">
                            站名
                        </th>
                        <th style="width: 70px;">
                            次数
                        </th>
                        <th style="width: 70px;">
                            圈次
                        </th>
                        <th style="width: 70px;">
                            升降轨
                        </th>
                        <th style="width: 120px;">
                            跟踪时长
                        </th>
                        <th style="width: 120px;">
                            超过最高仰角时长
                        </th>
                        <th style="width: 120px;">
                            跟踪开始时间
                        </th>
                        <th style="width: 120px;">
                            任务开始时间
                        </th>
                        <th style="width: 120px;">
                            到最高仰角时间
                        </th>
                        <th style="width: 120px;">
                            航捷时间
                        </th>
                        <th style="width: 120px;">
                            出最高仰角时间
                        </th>
                        <th style="width: 120px;">
                            任务结束时间
                        </th>
                        <th style="width: 120px;">
                            跟踪结束时间
                        </th>
                        <th style="width: 120px;">
                            航捷角
                        </th>
                    </tr>
                    <tbody id="tbStations">
            </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("ZM")%>
                    </td>
                    <td>
                        <%# Eval("N")%>
                    </td>
                    <td>
                        <%# Eval("QC")%>
                    </td>
                    <td>
                        <%# Eval("SJG")%>
                    </td>
                    <td>
                        <%# Eval("SP1")%>
                    </td>
                    <td>
                        <%# Eval("SP2")%>
                    </td>
                    <td>
                        <%# Eval("T1")%>
                    </td>
                    <td>
                        <%# Eval("T2")%>
                    </td>
                    <td>
                        <%# Eval("T3")%>
                    </td>
                    <td>
                        <%# Eval("T4")%>
                    </td>
                    <td>
                        <%# Eval("T5")%>
                    </td>
                    <td>
                        <%# Eval("T6")%>
                    </td>
                    <td>
                        <%# Eval("T7")%>
                    </td>
                    <td>
                        <%# Eval("h")%>
                    </td>
                </tr>
            </ItemTemplate>
        <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
    </asp:Repeater>
</div>
<div id="dialog-VS" style="display: none" title="查看轨道预报测站可见性统计数据">
    <asp:Repeater ID="rpVS" runat="server">
        <HeaderTemplate>
                <table class="list" style="width: 1500px">
                    <tr>
                        <th style="width: 100px;">
                            站名
                        </th>
                        <th style="width: 70px;">
                            次数（可见）
                        </th>
                        <th style="width: 70px;">
                            圈次
                        </th>
                        <th style="width: 70px;">
                            光学可见开始时间
                        </th>
                        <th style="width: 120px;">
                            光学可见结束时间
                        </th>
                    </tr>
                    <tbody id="tbStations">
            </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("ZM")%>
                    </td>
                    <td>
                        <%# Eval("N")%>
                    </td>
                    <td>
                        <%# Eval("dt")%>
                    </td>
                    <td>
                        <%# Eval("QC")%>
                    </td>
                    <td>
                        <%# Eval("T1")%>
                    </td>
                    <td>
                        <%# Eval("T2")%>
                    </td>
                </tr>
            </ItemTemplate>
        <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
    </asp:Repeater>
</div>
<div id="dialog-ST" style="display: none" title="查看日凌数据">
    <asp:Repeater ID="rpST" runat="server">
        <HeaderTemplate>
            <table class="list" style="width: 1500px">
                <tr>
                    <th style="width: 70px;">
                        次数（可见）
                    </th>
                    <th style="width: 70px;">
                        圈次
                    </th>
                    <th style="width: 70px;">
                        开始时间
                    </th>
                    <th style="width: 120px;">
                        结束时间
                    </th>
                </tr>
                <tbody id="tbStations">
            </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("N")%>
                </td>
                <td>
                    <%# Eval("QC")%>
                </td>
                <td>
                    <%# Eval("T1")%>
                </td>
                <td>
                    <%# Eval("T2")%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
<div id="dialog-MT" style="display: none" title="查看月凌数据">
    <asp:Repeater ID="rpMT" runat="server">
        <HeaderTemplate>
            <table class="list" style="width: 1500px">
                <tr>
                    <th style="width: 70px;">
                        次数（可见）
                    </th>
                    <th style="width: 70px;">
                        圈次
                    </th>
                    <th style="width: 70px;">
                        开始时间
                    </th>
                    <th style="width: 120px;">
                        结束时间
                    </th>
                </tr>
                <tbody id="tbStations">
            </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("N")%>
                </td>
                <td>
                    <%# Eval("QC")%>
                </td>
                <td>
                    <%# Eval("T1")%>
                </td>
                <td>
                    <%# Eval("T2")%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
<div id="dialog-ES" style="display: none" title="查看地影数据">
    <asp:Repeater ID="rpES" runat="server">
        <HeaderTemplate>
            <table class="list" style="width: 1500px">
                <tr>
                    <th style="width: 70px;">
                        次数（可见）
                    </th>
                    <th style="width: 70px;">
                        圈次
                    </th>
                    <th style="width: 70px;">
                        开始时间
                    </th>
                    <th style="width: 120px;">
                        结束时间
                    </th>
                </tr>
                <tbody id="tbStations">
            </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("N")%>
                </td>
                <td>
                    <%# Eval("QC")%>
                </td>
                <td>
                    <%# Eval("T1")%>
                </td>
                <td>
                    <%# Eval("T2")%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
</asp:Content>

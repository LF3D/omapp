<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ResourceCalculateResultView.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.ResourceCalculateResultView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
        .norText
        {
            width: 155px;
            margin: 0px;
            padding: 0px;
        }
        .norDpl
        {
            width: 160px;
            margin: 0px;
            padding: 0px;
        }
        .index_content_search
        {
            margin-top: 10px;
        }
        
        .index_content_search table
        {
            border: 1px solid #eeeeee;
            border-collapse: collapse;
            width: 100%;
        }
        
        .index_content_search table td
        {
            border: 1px solid #eeeeee;
            line-height: 26px;
            color: #333333;
            text-align: left;
            height: 26px;
        }
        .index_content_search table th
        {
            border: 1px solid #eeeeee;
            font-weight: bold;
            line-height: 26px;
            color: #333333;
            text-align: right;
            height: 26px;
        }
        .index_content_view
        {
            margin-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="resmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuRes" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 查看资源调度计算结果
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="index_content_search">
        <table cellspacing="0" cellpadding="0" class="edit">
            <tr>
                <th width="15%">
                    总需求数：
                </th>
                <td width="25%">
                    <asp:Label ID="lblRequirementNumber" runat="server" Text="Label"></asp:Label>
                </td>
                <th width="15%">
                    完成需求数：
                </th>
                <td width="25%">
                    <asp:Label ID="lblCompleteRequirementNumber" runat="server" Text="Label"></asp:Label>
                </td>
                <td width="20%">
                </td>
            </tr>
            <tr>
                <th width="15%">
                    总得分：
                </th>
                <td width="25%">
                    <asp:Label ID="lblTotalScore" runat="server" Text="Label"></asp:Label>
                </td>
                <th width="15%"> 
                    优先级原则得分：
                </th>
                <td width="25%">
                    <asp:Label ID="lblPriorityScore" runat="server" Text="Label"></asp:Label>
                </td>
                <td width="20%">
                </td>
            </tr>
            <tr>
                <th width="15%">
                    效能原则得分：
                </th>
                <td width="25%">
                    <asp:Label ID="lblEfficiencyScore" runat="server" Text="Label"></asp:Label>
                </td>
                <th width="15%">
                    集中原则得分：
                </th>
                <td width="25%">
                    <asp:Label ID="lblFocusScore" runat="server" Text="Label"></asp:Label>
                </td>
                <td width="20%">
                </td>
            </tr>
            <tr>
                <th width="15%">
                    地面站均衡原则得分：
                </th>
                <td width="25%">
                    <asp:Label ID="lblGroundStationProportionScore" runat="server" Text="Label"></asp:Label>
                </td>
                <th width="15%">
                    卫星均衡原则得分：
                </th>
                <td width="25%">
                    <asp:Label ID="lblSatelliteProportionScore" runat="server" Text="Label"></asp:Label>
                </td>
                <td width="20%">
                <asp:Button ID="btnReturn" runat="server" OnClick="btnReturn_Click" CssClass="button" Text="返 回" />
                </td>
            </tr>
        </table>
    </div>
    <div class="index_content_view">
        <asp:Repeater ID="rpResourceCalculateResultList" runat="server" OnItemDataBound="rpResourceCalculateResultList_ItemDataBound">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <th style="width: 15%;">
                            需求名
                        </th>
                        <th style="width: 15%;">
                            使用卫星
                        </th>
                        <th style="width: 70%;">
                            星地阶段
                        </th>
                    </tr>
                    <tbody id="tbResourceCalculateResultList">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("RequirementName")%>
                    </td>
                    <td>
                        <%# Eval("UseSatellite")%>
                    </td>
                    <td>
                        <asp:Repeater ID="rpSatelliteGroundPhaseInfoList" runat="server">
                            <HeaderTemplate>
                                <table class="list">
                                    <tr>
                                        <th style="width: 20%;">
                                            阶段名
                                        </th>
                                        <th style="width: 15%;">
                                            功能类型
                                        </th>
                                        <th style="width: 15%;">
                                            使用地面站
                                        </th>
                                        <th style="width: 10%;">
                                            使用设备
                                        </th>
                                        <th style="width: 10%;">
                                            过顶情况
                                        </th>
                                        <th style="width: 15%;">
                                            开始时间
                                        </th>
                                        <th style="width: 15%;">
                                            结束时间
                                        </th>
                                    </tr>
                                    <tbody id="tbSatelliteGroundPhaseInfoList">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# Eval("PhaseName")%>
                                    </td>
                                    <td>
                                        <%# Eval("FunctionType")%>
                                    </td>
                                    <td>
                                        <%# Eval("UseGroundStation")%>
                                    </td>
                                    <td>
                                        <%# Eval("UseEquipment")%>
                                    </td>
                                    <td>
                                        <%# Eval("OverHeadCondition")%>
                                    </td>
                                    <td>
                                        <%# Eval("BeginTime")%>
                                    </td>
                                    <td>
                                        <%# Eval("EndTime")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody></table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody></table>
            </FooterTemplate>
        </asp:Repeater>
        <table class="listTitle">
            <tr>
                <td class="listTitle-c1">
                </td>
                <td class="listTitle-c2">
                    <om:CollectionPager ID="cpResourceCalculateResultPager" runat="server">
                    </om:CollectionPager>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResourceStatusChartManage.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.ResourceStatusChartManage" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script language="javascript" type="text/javascript">
    function showAllResourceStatus() {
        var resourceCodeObj = document.getElementById('<%=txtResourceCode.ClientID%>');
        if(resourceCodeObj != null)
        {
           if(resourceCodeObj.value == "")
           {
              return confirm("是否查询所有资源状态图？");
           }
        }
        return true;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
<om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="resmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
<om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuRes" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
业务管理 &gt; 查询资源状态分布图
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
<%--<script type="text/javascript">
    $(function () {
        $("#txtBeginTime").datepicker();
        $("#txtEndTime").datepicker();
    });
    </script>--%>
<div class="index_content_search">
        <table cellspacing="0" cellpadding="0" class="searchTable">
            <tr>
                <th width="15%">
                    资源类型：
                </th>
                <td width="25%">
                    <asp:DropDownList ID="dplResourceType" runat="server" CssClass="norDpl">
                    </asp:DropDownList>
                </td>
                <th width="15%">
                    资源编号：
                </th>
                <td width="25%">
                    <asp:TextBox ID="txtResourceCode" runat="server" CssClass="norText"></asp:TextBox>
                </td>
                <td width="20%">
                </td>
            </tr>
            <tr>
               <th>
                  起始时间：
               </th>
               <td>
                  <asp:TextBox ID="txtBeginTime" runat="server" CssClass="norText" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" ClientIDMode="Static"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtBeginTime" ErrorMessage="（必填）" ValidationGroup="SearchStatus"></asp:RequiredFieldValidator>
               </td>
               <th>
                  结束时间：
               </th>
               <td>
                 <asp:TextBox ID="txtEndTime" runat="server" CssClass="norText" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" ClientIDMode="Static"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtEndTime" ErrorMessage="（必填）" ValidationGroup="SearchStatus"></asp:RequiredFieldValidator>
               </td>
               <td>
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return showAllResourceStatus();" CssClass="button" ValidationGroup="SearchStatus" Text="查 询"/>
                    <asp:Button ID="btnReturn" runat="server" OnClick="btnReturn_Click" CssClass="button" Text="返 回" />
               </td>
            </tr>
        </table>
    </div>
     <div id="divOneResourceStatus" runat="server" visible="false" class="index_content_view">
        <asp:Chart ID="chartOneResourceStatus" runat="server" Width="1100px" BackColor="#D3DFF0" OnPreRender="chartOneResourceStatus_PreRender">
        <Titles>
           <asp:Title Text="资源状态图形显示"></asp:Title>
        </Titles>
        <Legends>
            <asp:Legend Name="图例" Title="资源状态图例"></asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="seriesHealthStatus" ChartType="RangeBar" YValueType="Date" Legend="图例" LegendText="异常状态">
            </asp:Series>
            <asp:Series Name="seriesUseStatus" ChartType="RangeBar" YValueType="Date" Legend="图例" LegendText="占用状态">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
              <AxisY IntervalType="Days"  Interval="1"></AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    </div>
     <div id="divAllResourceStatus" runat="server" visible="false" class="index_content_view">
     <asp:Chart ID="chartAllResourceStatus" runat="server" Width="1100px" BackColor="#D3DFF0" OnPreRender="chartAllResourceStatus_PreRender">
        <Titles>
           <asp:Title Text="全部资源状态图形显示"></asp:Title>
        </Titles>
        <Legends>
            <asp:Legend Name="图例" Title="资源状态图例"></asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="seriesHealthStatus" ChartType="RangeBar" YValueType="Date" Legend="图例" LegendText="健康状态">
            </asp:Series>
            <asp:Series Name="seriesUseStatus" ChartType="RangeBar" YValueType="Date" Legend="图例" LegendText="占用状态">
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1">
              <AxisY IntervalType="Days"  Interval="1"></AxisY>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
     </div> 
</asp:Content>

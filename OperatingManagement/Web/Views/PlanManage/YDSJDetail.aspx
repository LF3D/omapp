<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YDSJDetail.aspx.cs" Inherits="OperatingManagement.Web.Views.PlanManage.YDSJDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" class="edit">
<%--            <tr>
                <td>
                    版本：
                </td>
                <td>
                    <asp:Label ID="lblVersion" runat="server"></asp:Label>
                </td>
                <td>
                    标志：
                </td>
                <td>
                    <asp:Label ID="lblFlag" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    数据主类别：
                </td>
                <td>
                    <asp:Label ID="lblMainType" runat="server"></asp:Label>
                </td>
                <td>
                    数据次类别：
                </td>
                <td>
                    <asp:Label ID="lblDataType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    信源地址：
                </td>
                <td>
                    <asp:Label ID="lblSource" runat="server"></asp:Label>
                </td>
                <td>
                    信宿地址：
                </td>
                <td>
                    <asp:Label ID="lblDestination" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    任务代号：
                </td>
                <td>
                    <asp:Label ID="lblMissionCode" runat="server"></asp:Label>
                </td>
                <td>
                    卫星编号：
                </td>
                <td>
                    <asp:Label ID="lblSatelliteCode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    数据日期：
                </td>
                <td>
                    <asp:Label ID="lblDataDate" runat="server"></asp:Label>
                </td>
                <td>
                    数据时间：
                </td>
                <td>
                    <asp:Label ID="lblDataTime" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>--%>

            <tr>
                <th>
                    历元日期：
                </th>
                <td>
                    <asp:Label ID="lblD" runat="server"></asp:Label>
                </td>
                <th>
                    历元时刻：
                </th>
                <td>
                    <asp:Label ID="lblT" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    轨道半长轴：
                </th>
                <td>
                    <asp:Label ID="lblA" runat="server"></asp:Label>
                    &nbsp;米
                </td>
                <th>
                    升交点赤经：
                </th>
                <td>
                    <asp:Label ID="lblOhm" runat="server"></asp:Label>
                    &nbsp;度
                </td>
            </tr>
            <tr>
                <th>
                    轨道偏心率：
                </th>
                <td>
                    <asp:Label ID="lblE" runat="server"></asp:Label>
                    &nbsp;</td>
                <th>
                    轨道倾角：
                </th>
                <td>
                    <asp:Label ID="lblI" runat="server"></asp:Label>
                    &nbsp;度
                </td>
            </tr>
            <tr>
                <th>
                    近地点幅角：
                </th>
                <td>
                    <asp:Label ID="lblOmega" runat="server"></asp:Label>
                    &nbsp;度
                </td>
                <th>
                    平近点角：
                </th>
                <td>
                    <asp:Label ID="lblM" runat="server"></asp:Label>
                    &nbsp;度
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

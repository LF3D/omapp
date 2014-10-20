<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OribitalQuantityDetail.aspx.cs"
    Inherits="OperatingManagement.Web.Views.PlanManage.OribitalQuantityDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" class="edit">
            <tr>
                <th style="width:130px;">
                    历元日期：
                </th>
                <td style="width:140px;">
                    <asp:Label ID="lblD" runat="server"></asp:Label>
                    &nbsp;天
                </td>
                <th style="width:130px;">
                    历元时刻：
                </th>
                <td style="width:140px;">
                    <asp:Label ID="lblT" runat="server"></asp:Label>
                    &nbsp;毫秒
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
                    远地点地心距：
                </th>
                <td>
                    <asp:Label ID="lblRa" runat="server"></asp:Label>
                    &nbsp;米
                </td>
                <th>
                    近地点地心距：
                </th>
                <td>
                    <asp:Label ID="lblRp" runat="server"></asp:Label>
                    &nbsp;米
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
            <tr>
                <th>
                    轨道周期：
                </th>
                <td>
                    <asp:Label ID="lblP" runat="server"></asp:Label>
                    &nbsp;分钟
                </td>
                <th>
                    轨道周期变化率：
                </th>
                <td>
                    <asp:Label ID="lblPi" runat="server"></asp:Label>
                    &nbsp;秒/天
                </td>
            </tr>
            <tr>
                <th>
                    大气阻力摄动系数：</th>
                <td>
                    <asp:Label ID="lblCDSM" runat="server"></asp:Label>
                    &nbsp;米<sup>2</sup>/千克</td>
                <th>
                    光压摄动系数：</th>
                <td>
                    <asp:Label ID="lblKSM" runat="server"></asp:Label>
                    &nbsp;米<sup>2</sup>/千克</td>
            </tr>
            <tr>
                <th>
                    扩展字1：</th>
                <td>
                    <asp:Label ID="lblKZ1" runat="server"></asp:Label>
                </td>
                <th>
                    扩展字2：</th>
                <td>
                    <asp:Label ID="lblKZ2" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

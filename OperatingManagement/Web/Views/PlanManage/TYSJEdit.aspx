<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TYSJEdit.aspx.cs"
    Inherits="OperatingManagement.Web.Views.PlanManage.TYSJEdit" %>

<%@ Register Src="../../ucs/ucTask.ascx" TagName="ucTask" TagPrefix="uc1" %>
<%@ Register Src="../../ucs/ucSatellite.ascx" TagName="ucSatellite" TagPrefix="uc2" %>
<%@ Register Src="../../ucs/ucTimer.ascx" TagName="ucTimer" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="ykinfoman" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuYKInfo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    计划管理 &gt; 仿真推演试验数据
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <div>
        <table class="edit" style="width: 900px;">
            <tr>
                <th style="width: 120px;">
                    任务代号(<span class="red">*</span>)
                </th>
                <td style="width: 300px;">
                    <uc1:ucTask ID="ucTask1" runat="server" AllowBlankItem="False" />
                </td>
                <th style="width: 120px;">
                    计划序号
                </th>
                <td>
                    <asp:TextBox ID="txtJXH" runat="server" Width="100px" CssClass="text" MaxLength="20"
                        Enabled="False"></asp:TextBox>
                    &nbsp;<span style="color: #3399FF;">自动生成，不可编辑</span>
                </td>
            </tr>
            <tr>
                <th style="width: 120px;">
                    备注
                </th>
                <td colspan="3" style=" width:770px;">
                    <asp:TextBox ID="txtNote" runat="server" CssClass="text" MaxLength="100" Width="559px"
                        Height="50px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Repeater ID="rpData" runat="server" OnItemCommand="rpData_ItemCommand" 
                        onitemdatabound="rpData_ItemDataBound">
                        <HeaderTemplate>
                            <table class="edit1" style="width: 900px;">
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <th style="width: 110px;">
                                    卫星名称
                                </th>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlSatName" runat="server" AutoPostBack="True" Height="20px"
                                        OnSelectedIndexChanged="ddlSatName_SelectedIndexChanged" Width="190px">
                                        <asp:ListItem Value="0730">探索三号卫星</asp:ListItem>
                                        <asp:ListItem Value="074A">探索四号卫星</asp:ListItem>
                                        <asp:ListItem Value="075A">探索五号卫星</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th style="width: 110px;">
                                    试验类别
                                </th>
                                <td style="width: 300px;">
                                    <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" Height="20px" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                        Width="190px">
                                        <asp:ListItem>GEO目标观测试验</asp:ListItem>
                                        <asp:ListItem>LEO目标成像试验</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <th style="width: 110px;">
                                    试验项目
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddlTestItem" runat="server" Height="20px" Width="190px">
                                        <asp:ListItem>自然交会观测试验</asp:ListItem>
                                        <asp:ListItem>同步带凝视观测试验</asp:ListItem>
                                        <asp:ListItem>天地基联合观测试验</asp:ListItem>
                                        <asp:ListItem>其它扩展试验项目</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    试验开始时间
                                </th>
                                <td>
                                    <asp:TextBox ID="txtStartTime" runat="server" Width="180px" CssClass="text" MaxLength="14"
                                        onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"  Text='<%# Eval("StartTime")%>'></asp:TextBox>
                                    <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartTime"
                                        ErrorMessage="开始时间不能为空" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                                <th>
                                    试验结束时间
                                </th>
                                <td>
                                    <asp:TextBox ID="txtEndTime" runat="server" Width="180px" CssClass="text" MaxLength="14"
                                         onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"  Text='<%# Eval("EndTime")%>'></asp:TextBox>
                                    &nbsp;<span style="color: #3399FF;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEndTime"
                                            ErrorMessage="结束时间不能为空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <span style="color: #3399FF;">
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtStartTime"
                                                ControlToValidate="txtEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red"
                                                Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                                        </span></span>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    试验条件
                                </th>
                                <td colspan="2">
                                    <asp:TextBox ID="txtCondition" runat="server" Width="390px" CssClass="text" Height="40px"
                                        TextMode="MultiLine" Text='<%# Eval("Condition")%>'></asp:TextBox>
                                </td>
                                <td style="text-align:left;">
                                    <asp:Button ID="btn3" CausesValidation="False" CssClass="button" runat="server" CommandName="Add"
                                        Text="添加" />
                                    <asp:Button ID="btn4" CausesValidation="False" CssClass="button" runat="server" CommandName="Del"
                                        Text="删除" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr id="trMessage" runat="server" visible="false">
                <th>
                </th>
                <td colspan="3">
                    <asp:Label ID="ltMessage" runat="server" CssClass="error" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <th style="width: 120px;">
                    &nbsp;
                </th>
                <td colspan="3">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="保存计划" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnSaveTo" runat="server" CssClass="button" Text="另存计划" OnClick="btnSaveTo_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnReset" CssClass="button" runat="server" Text="重置" Width="65px" OnClick="btnReset_Click" CausesValidation="False" />&nbsp;&nbsp;
                    <asp:Button ID="btnReturn" CssClass="button" runat="server" Text="返回" Width="65px" OnClick="btnReturn_Click" CausesValidation="False" />&nbsp;&nbsp;
                    <asp:Button ID="btnSurePlan"  CssClass="button" runat="server" Text="确认计划" onclick="btnSurePlan_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnFormal" CssClass="button" runat="server" OnClick="btnFormal_Click" Text="转为正式计划" />
                    <asp:HiddenField ID="HfID" runat="server" />
                    <asp:HiddenField ID="HfFileIndex" runat="server" />
                    <asp:HiddenField ID="hfTaskID" runat="server" />
                    <asp:HiddenField ID="hfSatID" runat="server" />
                    <asp:HiddenField ID="hfStatus" runat="server" />
                    <asp:HiddenField ID="hfURL" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dialog-form" style="display: none" title="提示信息">
        <p class="content">
        </p>
    </div>
</asp:Content>

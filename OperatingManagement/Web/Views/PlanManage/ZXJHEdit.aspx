<%@ Page MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" Language="C#"
    AutoEventWireup="true" CodeBehind="ZXJHEdit.aspx.cs" Inherits="OperatingManagement.Web.Views.PlanManage.ZXJHEdit" %>

<%@ Register Src="../../ucs/ucSatellite.ascx" TagName="ucSatellite" TagPrefix="uc2" %>
<%@ Register src="../../ucs/ucOutTask.ascx" tagname="ucOutTask" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="planmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuPlan" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    计划管理 &gt; 中心运行计划
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:Panel runat="server" ID="pnlStation">
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlMain">
        <div>
            <table cellpadding="0" class="edit1" style="width: 950px;">
                <tr style="display:none;">
                    <th style="width: 120px;">
                        上传进出站及航捷数据统计文件
                    </th>
                    <td align="left" colspan="3">
                        <asp:FileUpload ID="FileUpload1" class="upload" runat="server" Width="455px" />
                        <asp:Button ID="btnUpdate" class="button" runat="server" Text="上传" CausesValidation="False"
                            OnClick="btnUpdate_Click" Width="54px" />
                        <asp:Label ID="lblUpload" CssClass="error" runat="server" Text="文件上传成功" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr  style="display:none;">
                    <th style="width: 120px;">
                        &nbsp;
                    </th>
                    <td style="width: 332px;">
                        &nbsp;
                    </td>
                    <th style="width: 120px;">
                        &nbsp;
                    </th>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <th style="width: 120px;">
                        任务代号(<span class="red">*</span>)
                    </th>
                    <td style="width: 332px;">
                        <uc3:ucOutTask ID="ucOutTask1" runat="server" AllowBlankItem="False" />
                    </td>
                    <th style="width: 120px;">
                        日期
                    </th>
                    <td>
                        <asp:TextBox ID="txtDate" CssClass="text" runat="server" MaxLength="10" ClientIDMode="Static"
                            onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th style="width: 120px;">
                        计划开始时间
                    </th>
                    <td>
                        <asp:TextBox ID="txtPlanStartTime" runat="server" CssClass="text" MaxLength="10"
                            ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPlanStartTime"
                            ErrorMessage="开始时间不能为空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <th style="width: 120px;">
                        计划结束时间
                    </th>
                    <td>
                        <asp:TextBox ID="txtPlanEndTime" runat="server" CssClass="text" MaxLength="10" ClientIDMode="Static"
                            onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPlanEndTime"
                            ErrorMessage="结束时间不能为空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPlanStartTime"
                            ControlToValidate="txtPlanEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                            ForeColor="Red" Operator="GreaterThan"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        备注
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtNote" runat="server" CssClass="text" MaxLength="100" Width="620px"
                            Height="40px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div id="detailtable">
                <table class="edit1" style="width: 950px;">
                    <tr>
                        <th colspan="4" style="color: Black; text-align: left;">
                            <b>试验内容</b>
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 120px;">
                            对应日期的试验个数
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="txtSYCount" runat="server" CssClass="text" Text='<%# Eval("SYCount")%>'
                                onkeypress="return event.keyCode>=48&&event.keyCode<=57" onpaste="return !clipboardData.getData('text').match(/\D/)"
                                ondragenter="return false" Style="ime-mode: Disabled;" MaxLength="4" 
                                ReadOnly="True" Enabled="false"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtSYCount"
                                ErrorMessage="只能是数字" ForeColor="Red" ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
                                &nbsp;<span style="color:#3399FF;">保存时自动生成</span>
                        </td>
                    </tr>
                </table>
                <table class="edit1" style="width: 950px">
                    <tr>
                        <th colspan="2" style="color: Black; text-align: left;">
                            <b>试验计划</b>
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 120px;">
                            试验内容
                        </th>
                        <td>
                            <asp:Repeater ID="rpSYContent" runat="server" OnItemCommand="rpSYContent_ItemCommand"
                             OnItemDataBound="rpSYContent_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="edit1">
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <th style="width: 120px;">
                                            在当日计划中的ID
                                        </th>
                                        <td style="width: 270px;">
                                            <asp:TextBox ID="txtSYID" runat="server" CssClass="text" Text='<%# Eval("SYID")%>'></asp:TextBox>
                                        </td>
                                        <th style="width: 120px;">
                                            试验项目名称
                                        </th>
                                        <td style="width: 270px;">
                                            <asp:TextBox ID="txtSYName" runat="server" CssClass="text" Text='<%# Eval("SYName")%>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            试验开始时间
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtSYStartDateTime" runat="server" CssClass="text" Text='<%# Eval("SYStartTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        </td>
                                        <th>
                                            &nbsp;试验结束时间
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtSYEndDateTime" runat="server" CssClass="text" Text='<%# Eval("SYEndTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                 <asp:CompareValidator ID="CompareValidator111" runat="server" ControlToCompare="txtSYStartDateTime"
                            ControlToValidate="txtSYEndDateTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                            ForeColor="Red" Operator="GreaterThan"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            试验运行的天数
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtSYDays" runat="server" CssClass="text" Text='<%# Eval("SYDays")%>'
                                                onkeypress="return event.keyCode>=48&&event.keyCode<=57" onpaste="return !clipboardData.getData('text').match(/\D/)"
                                                ondragenter="return false" Style="ime-mode: Disabled"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSYDays"
                                                ErrorMessage="只能是数字" ForeColor="Red" ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
                                        </td>
                                        <th>
                                            试验说明
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtSYNote" runat="server" CssClass="text" Text='<%# Eval("SYNote")%>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            卫星代号
                                        </th>
                                        <td colspan="3">
                                            <uc2:ucSatellite ID="ddlSYSatID" runat="server" AllowBlankItem="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Repeater ID="rpSYContentSC" runat="server" OnItemCommand="rpSYContentSC_ItemCommand" 
                                             OnItemDataBound="rpSYContentSC_ItemDataBound">
                                                <HeaderTemplate>
                                                    <table class="list">
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <th style="width: 115px;">
                                                            数传-站编号
                                                        </th>
                                                        <td style="width: 270px;">
                                                            <asp:DropDownList ID="ddlDW" Width="260px"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDW_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                        </td>
                                                        <th style="width: 120px;">
                                                            数传-设备编号
                                                        </th>
                                                        <td style="width: 265px;">
                                                            <asp:DropDownList ID="ddlSB" runat="server"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            数传-频段
                                                        </th>
                                                        <td>
                                                            <asp:DropDownList ID="ddlSCFrequencyBand" runat="server">
                                                                <asp:ListItem Value="S">S</asp:ListItem>
                                                                <asp:ListItem Value="X">X</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <th>
                                                            数传-圈次
                                                        </th>
                                                        <td>
                                                            <asp:TextBox ID="txtSCLaps" runat="server" CssClass="text" Text='<%# Eval("SY_SCLaps")%>'></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            数传-开始时间
                                                        </th>
                                                        <td>
                                                            <asp:TextBox ID="txtSCStartTime" runat="server" CssClass="text" Text='<%# Eval("SY_SCStartTime")%>'
                                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                        </td>
                                                        <th>
                                                            数传-结束时间
                                                        </th>
                                                        <td>
                                                            <asp:TextBox ID="txtSCEndTime" runat="server" CssClass="text" Text='<%# Eval("SY_SCEndTime")%>'
                                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txtSCStartTime"
                                                                ControlToValidate="txtSCEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                                                                ForeColor="Red" Operator="GreaterThan"></asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"></td>
                                                        <td colspan="2">
                                                        <asp:Button ID="btnSYSCAdd" CausesValidation="False" CssClass="button" runat="server"
                                                CommandName="Add" Text="添加" />
                                            <asp:Button ID="btnSYSCDel" CausesValidation="False" CssClass="button" runat="server"
                                                CommandName="Del" Text="删除" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody> </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Repeater ID="rpSYContentCK" runat="server" OnItemCommand="rpSYContentCK_ItemCommand"
                                             OnItemDataBound="rpSYContentCK_ItemDataBound">
                                                <HeaderTemplate>
                                                    <table class="list">
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <th style="width: 115px;">
                                                            测控-站编号
                                                        </th>
                                                        <td style="width: 270px;">
                                                            <asp:DropDownList ID="ddlDW" Width="260px"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDW_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                        </td>
                                                        <th style="width: 120px;">
                                                            测控-设备编号
                                                        </th>
                                                        <td style="width: 265px;">
                                                            <asp:DropDownList ID="ddlSB" runat="server"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            测控-开始时间
                                                        </th>
                                                        <td>
                                                            <asp:TextBox ID="txtCKStartTime" runat="server" CssClass="text" Text='<%# Eval("SY_CKStartTime")%>'
                                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                        </td>
                                                        <th>
                                                            测控-结束时间
                                                        </th>
                                                        <td>
                                                            <asp:TextBox ID="txtCKEndTime" runat="server" CssClass="text" Text='<%# Eval("SY_CKEndTime")%>'
                                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="txtCKStartTime"
                                                                ControlToValidate="txtCKEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                                                                ForeColor="Red" Operator="GreaterThan"></asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            测控-圈次
                                                        </th>
                                                        <td>
                                                            <asp:TextBox ID="txtCKLaps" runat="server" CssClass="text" Text='<%# Eval("SY_CKLaps")%>'></asp:TextBox>
                                                        </td>
                                                        <td colspan="2">
                                                        <asp:Button ID="btnSYCKAdd" CausesValidation="False" CssClass="button" runat="server"
                                                CommandName="Add" Text="添加" />
                                            <asp:Button ID="btnSYCKDel" CausesValidation="False" CssClass="button" runat="server"
                                                CommandName="Del" Text="删除" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody> </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Repeater ID="rpSYContentZS" runat="server" OnItemCommand="rpSYContentZS_ItemCommand">
                                                <HeaderTemplate>
                                                    <table class="list">
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <th style="width: 115px;">
                                                            注数-最早时间要求
                                                        </th>
                                                        <td style="width: 270px;">
                                                            <asp:TextBox ID="txtZSFirst" runat="server" CssClass="text" Text='<%# Eval("SY_ZSFirst")%>'
                                                             onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                        </td>
                                                        <th style="width: 120px;">
                                                            注数-最晚时间要求
                                                        </th>
                                                        <td style="width: 265px;">
                                                            <asp:TextBox ID="txtZSLast" runat="server" CssClass="text" Text='<%# Eval("SY_ZSLast")%>'
                                                             onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                             <asp:CompareValidator ID="CompareVa3" runat="server" ControlToCompare="txtZSFirst"
                                                                ControlToValidate="txtZSLast" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                                                                ForeColor="Red" Operator="GreaterThan"></asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>
                                                            注数-主要内容
                                                        </th>
                                                        <td>
                                                            <asp:TextBox ID="txtZSContent" runat="server" CssClass="text" Text='<%# Eval("SY_ZSContent")%>'></asp:TextBox>
                                                        </td>
                                                        <td colspan="2">
                                                        <asp:Button ID="btnSYZCAdd" CausesValidation="False" CssClass="button" runat="server"
                                                CommandName="Add" Text="添加" />
                                            <asp:Button ID="btnSYZCDel" CausesValidation="False" CssClass="button" runat="server"
                                                CommandName="Del" Text="删除" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody> </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Button ID="btnSYAdd" CausesValidation="False" CssClass="button" runat="server"
                                                CommandName="Add" Text="添加试验内容" />
                                            <asp:Button ID="btnSYDel" CausesValidation="False" CssClass="button" runat="server"
                                                CommandName="Del" Text="删除" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody> </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="2" style="color: Black; text-align: left;">
                            <b>工作计划</b>
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 120px;">
                            任务管理
                        </th>
                        <td>
                            <asp:Repeater ID="rpWork" runat="server" OnItemCommand="rpWork_ItemCommand" OnItemDataBound="rpWork_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="edit1">
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <th style="width: 120px;">
                                            工作
                                        </th>
                                        <td style="width: 270px;">
                                            <asp:DropDownList ID="ddlWC_Work" runat="server">
                                                <asp:ListItem Value="试验规划">试验规划</asp:ListItem>
                                                <asp:ListItem Value="计划管理">计划管理</asp:ListItem>
                                                <asp:ListItem Value="试验数据处理">试验数据处理</asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtWC_Work" CssClass="text" runat="server" Text='<%# Eval("Work")%>'></asp:TextBox>--%>
                                        </td>
                                        <th style="width: 120px;">
                                            对应试验ID
                                        </th>
                                        <td style="width: 270px;">
                                            <asp:TextBox ID="txtWC_SYID" CssClass="text" runat="server" Text='<%# Eval("SYID")%>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            开始时间
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtWC_StartTime" CssClass="text" runat="server" Text='<%# Eval("StartTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        </td>
                                        <th>
                                            最短持续时间
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtWC_MinTime" CssClass="text" runat="server" Text='<%# Eval("MinTime")%>'
                                             onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            最长持续时间
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtWC_MaxTime" CssClass="text" runat="server" Text='<%# Eval("MaxTime")%>'
                                             onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                             <asp:CompareValidator ID="CompareValidator31" runat="server" ControlToCompare="txtWC_MinTime"
                            ControlToValidate="txtWC_MaxTime" Display="Dynamic" ErrorMessage="最长持续时间应大于最短持续时间"
                            ForeColor="Red" Operator="GreaterThan"></asp:CompareValidator>
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btn1" CausesValidation="False" CssClass="button" runat="server" CommandName="Add"
                                                Text="添加" />
                                            <asp:Button ID="btn2" CausesValidation="False" CssClass="button" runat="server" CommandName="Del"
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
                    <tr>
                        <th>
                            指令制作
                        </th>
                        <td>
                            <asp:Repeater ID="rpCommandMake" runat="server" 
                                OnItemCommand="rpCommandMake_ItemCommand" 
                                onitemdatabound="rpCommandMake_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="edit1">
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <th style="width: 120px;">
                                            卫星代号
                                        </th>
                                        <td style="width: 270px;">
                                            <uc2:ucSatellite ID="txtWork_Command_SatID" runat="server" AllowBlankItem="False" />
                                            <%--<asp:TextBox ID="txtWork_Command_SatID" runat="server" CssClass="text" Text='<%# Eval("Work_Command_SatID")%>'></asp:TextBox>--%>
                                        </td>
                                        <th style="width: 120px;">
                                            对应试验ID
                                        </th>
                                        <td style="width: 270px;">
                                            <asp:TextBox ID="txtWork_Command_SYID" runat="server" CssClass="text" Text='<%# Eval("Work_Command_SYID")%>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            对应控制程序
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtWork_Command_Programe" runat="server" CssClass="text" Text='<%# Eval("Work_Command_Programe")%>'></asp:TextBox>
                                        </td>
                                        <th>
                                            完成时间
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtWork_Command_FinishTime" runat="server" CssClass="text" Text='<%# Eval("Work_Command_FinishTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            上注方式
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtWork_Command_UpWay" runat="server" CssClass="text" Text='<%# Eval("Work_Command_UpWay")%>'></asp:TextBox>
                                        </td>
                                        <th>
                                            上注时间/圈次
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtWork_Command_UpTime" runat="server" CssClass="text" Text='<%# Eval("Work_Command_UpTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            说明
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtWork_Command_Note" runat="server" CssClass="text" Text='<%# Eval("Work_Command_Note")%>'></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btnCommandAdd" CausesValidation="False" CssClass="button" runat="server"
                                                CommandName="Add" Text="添加" />
                                            <asp:Button ID="btnCommandDel" CausesValidation="False" CssClass="button" runat="server"
                                                CommandName="Del" Text="删除" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody> </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <table>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            实时试验数据处理
                        </th>
                        <td>
                            <asp:Repeater ID="rpSYDataHandle" runat="server" OnItemCommand="rpSYDataHandle_ItemCommand"
                                OnItemDataBound="rpSYDataHandle_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="edit1">
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <th style="width: 120px;">
                                            卫星代号
                                        </th>
                                        <td style="width: 270px;">
                                            <uc2:ucSatellite ID="ddlSYDataHandle_SatID" runat="server" AllowBlankItem="False" />
                                        </td>
                                        <th style="width: 120px;">
                                            对应试验ID
                                        </th>
                                        <td style="width: 270px;">
                                            <asp:TextBox ID="txtSHSYID" CssClass="text" runat="server" Text='<%# Eval("SYID")%>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            圈次
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtSHLaps" CssClass="text" runat="server" Text='<%# Eval("Laps")%>'></asp:TextBox>
                                        </td>
                                        <th>
                                            工作内容
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtSHContent" CssClass="text" runat="server" Text='<%# Eval("Content")%>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            主站
                                        </th>
                                        <td>
                                            <asp:DropDownList ID="ddlMainDW" Width="260px"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMainDW_SelectedIndexChanged">
                                                </asp:DropDownList>
                                        </td>
                                        <th>
                                            主站设备
                                        </th>
                                        <td>
                                            <asp:DropDownList ID="ddlMainSB" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            备站
                                        </th>
                                        <td>
                                            <asp:DropDownList ID="ddlBakDW" Width="260px"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBakDW_SelectedIndexChanged">
                                                </asp:DropDownList>
                                        </td>
                                        <th>
                                            备站设备
                                        </th>
                                        <td>
                                            <asp:DropDownList ID="ddlBakSB" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            实时开始处理时间
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtSHStartTime" runat="server" CssClass="text" Text='<%# Eval("StartTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        </td>
                                        <th>
                                            实时结束处理时间
                                        </th>
                                        <td>
                                            <asp:TextBox ID="txtSHEndTime" runat="server" CssClass="text" Text='<%# Eval("EndTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                <asp:CompareValidator ID="CompareValidator221" runat="server" ControlToCompare="txtSHStartTime"
                            ControlToValidate="txtSHEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                            ForeColor="Red" Operator="GreaterThan"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td colspan="2">
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
                    <tr>
                        <th>
                            指挥与监视
                        </th>
                        <td>
                            <asp:Repeater ID="rpDirectAndMonitor" runat="server" OnItemCommand="rpDirectAndMonitor_ItemCommand"
                                OnItemDataBound="rpDirectAndMonitor_ItemDataBound">
                                <HeaderTemplate>
                                    <table class="list">
                                        <tr>
                                            <th style="width: 150px;">
                                                对应试验ID
                                            </th>
                                            <th style="width: 150px;">
                                                开始时间
                                            </th>
                                            <th style="width: 150px;">
                                                结束时间
                                            </th>
                                            <th style="width: 150px;">
                                                实时演示任务
                                            </th>
                                            <th>
                                            </th>
                                        </tr>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDMSYID" CssClass="text" runat="server" Text='<%# Eval("SYID")%>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMStartTime" CssClass="text" runat="server" Text='<%# Eval("StartTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMEndTime" CssClass="text" runat="server" Text='<%# Eval("EndTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                <asp:CompareValidator ID="CompareValidatorc21" runat="server" ControlToCompare="txtDMStartTime"
                            ControlToValidate="txtDMEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                            ForeColor="Red" Operator="GreaterThan"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDMRTTask" runat="server">
                                                <asp:ListItem Value="有">有</asp:ListItem>
                                                <asp:ListItem Value="无">无</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn5" CausesValidation="False" CssClass="button" runat="server" CommandName="Add"
                                                Text="添加" />
                                            <asp:Button ID="btn6" CausesValidation="False" CssClass="button" runat="server" CommandName="Del"
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
                    <tr>
                        <th>
                            实时控制
                        </th>
                        <td>
                            <asp:Repeater ID="rpRealTimeControl" runat="server" OnItemCommand="rpRealTimeControl_ItemCommand">
                                <HeaderTemplate>
                                    <table class="list">
                                        <tr>
                                            <th style="width: 150px;">
                                                对应试验ID
                                            </th>
                                            <th style="width: 150px;">
                                                工作
                                            </th>
                                            <th style="width: 150px;">
                                                开始时间
                                            </th>
                                            <th style="width: 150px;">
                                                结束时间
                                            </th>
                                            <th>
                                            </th>
                                        </tr>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRCSYID" CssClass="text" runat="server" Text='<%# Eval("SYID")%>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRCWork" CssClass="text" runat="server" Text='<%# Eval("Work")%>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRCStartTime" CssClass="text" runat="server" Text='<%# Eval("StartTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRCEndTime" CssClass="text" runat="server" Text='<%# Eval("EndTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                <asp:CompareValidator ID="CompareValidatorcd21" runat="server" ControlToCompare="txtRCStartTime"
                            ControlToValidate="txtRCEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                            ForeColor="Red" Operator="GreaterThan"></asp:CompareValidator>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn7" CausesValidation="False" CssClass="button" runat="server" CommandName="Add"
                                                Text="添加" />
                                            <asp:Button ID="btn8" CausesValidation="False" CssClass="button" runat="server" CommandName="Del"
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
                    <tr>
                        <th>
                            试验评估
                        </th>
                        <td>
                            <asp:Repeater ID="rpSYEstimate" runat="server" OnItemCommand="SYEstimate_ItemCommand">
                                <HeaderTemplate>
                                    <table class="list">
                                        <tr>
                                            <th style="width: 150px;">
                                                对应试验ID
                                            </th>
                                            <th style="width: 150px;">
                                                开始时间
                                            </th>
                                            <th style="width: 150px;">
                                                完成时间
                                            </th>
                                            <th>
                                            </th>
                                        </tr>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtESYID" CssClass="text" runat="server" Text='<%# Eval("SYID")%>'></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEStartTime" CssClass="text" runat="server" Text='<%# Eval("StartTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEEndTime" CssClass="text" runat="server" Text='<%# Eval("EndTime")%>'
                                                onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn9" CausesValidation="False" CssClass="button" runat="server" CommandName="Add"
                                                Text="添加" />
                                            <asp:Button ID="btn10" CausesValidation="False" CssClass="button" runat="server"
                                                CommandName="Del" Text="删除" />
                                            <asp:CompareValidator ID="CompareValidator12" runat="server" ControlToCompare="txtEStartTime"
                                                ControlToValidate="txtEEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                                                ForeColor="Red" Operator="GreaterThan"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody> </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <div style="width: 750px; text-align: center;">
                <asp:Label ID="ltMessage" runat="server" CssClass="error" Text="试验内容与工作计划，所有字段都必须填写。"></asp:Label>
            </div>
            <div style="width: 100%; text-align: center">
                <asp:Button ID="btnSubmit" CssClass="button" OnClientClick="return CheckClientValidate();" runat="server" Text="保存计划" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnSaveTo" runat="server" OnClientClick="return CheckClientValidate();" CssClass="button" Text="另存计划" OnClick="btnSaveTo_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnReset" CssClass="button" runat="server" Text="重置" Width="65px" OnClick="btnReset_Click" CausesValidation="False" />&nbsp;&nbsp;
                <asp:Button ID="btnReturn" CssClass="button" runat="server" Text="返回" Width="65px" OnClick="btnReturn_Click" CausesValidation="False" />&nbsp;&nbsp;
                <asp:Button ID="btnSurePlan" CssClass="button" runat="server" Text="确认计划" onclick="btnSurePlan_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnCreateFile" class="button" runat="server" Text="生成文件" Width="65px" onclick="btnCreateFile_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnFormal" CssClass="button" runat="server" OnClick="btnFormal_Click" Text="转为正式计划" />
            </div>
            <div id="divFiles" runat="server" visible="false">
            <table>
                <tr style="width: 120px;height:24px;">
                    <th>
                        文件路径
                    </th>
                    <td class="style1">
                        <asp:Label ID="lblFilePath" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:LinkButton ID="lbtFilePath" runat="server" CausesValidation="false" 
                            onclick="lbtFilePath_Click" Visible="false">保存文件</asp:LinkButton>
                    </td>
                </tr>
            </table>
            </div>
            <div style="display: none">
                <asp:HiddenField ID="HfID" runat="server" />
                <asp:HiddenField ID="HfFileIndex" runat="server" />
                <asp:HiddenField ID="hfTaskID" runat="server" />
                <asp:HiddenField ID="hfSatID" runat="server" />
                <asp:HiddenField ID="hfStatus" runat="server" />
                <asp:HiddenField ID="hfURL" runat="server" />
                <asp:HiddenField ID="hfStationFile" runat="server" ClientIDMode="Static" />
                <asp:TextBox ID="txtIds" runat="server" ClientIDMode="Static"></asp:TextBox>
                <asp:Button ID="btnGetStationData" ClientIDMode="Static" class="button" runat="server"  CausesValidation="false"
                    OnClick="btnGetStationData_Click" Text="获取数据" />
            </div>
        </div>
        <div id="dialog-station" style="display: none" title="选择进出站及航捷数据">
            <asp:Repeater ID="rpDatas" runat="server">
                <HeaderTemplate>
                    <table class="list" style="width: 1500px">
                        <tr>
                            <th style="width: 20px;">
                                <input type="checkbox" onclick="checkAll(this)" />
                            </th>
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
                            <input type="checkbox" name="chkDelete" value="<%# Eval("rowIndex") %>" />
                        </td>
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
        <div id="dialog-form" style="display: none" title="提示信息">
            <p class="content">
            </p>
        </div>
    </asp:Panel>
</asp:Content>
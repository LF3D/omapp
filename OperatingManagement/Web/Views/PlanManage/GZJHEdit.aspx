<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GZJHEdit.aspx.cs"
    Inherits="OperatingManagement.Web.Views.PlanManage.GZJHEdit" %>

<%@ Register Src="../../ucs/ucTask.ascx" TagName="ucTask" TagPrefix="uc1" %>
<%@ Register Src="../../ucs/ucSatellite.ascx" TagName="ucSatellite" TagPrefix="uc2" %>
<%@ Register Src="../../ucs/ucTimer.ascx" TagName="ucTimer" TagPrefix="uc3" %>
<%@ Register src="../../ucs/ucOutTask.ascx" tagname="ucOutTask" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .text
        {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="planmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuPlan" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    计划管理 &gt; 地面站工作计划
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table cellpadding="0" class="edit1" style="width: 900px;">
        <tr>
            <th>
                上传进出站及航捷数据统计文件
            </th>
            <td align="left" colspan="3" style="width: 750px;">
                <asp:FileUpload ID="FileUpload1" class="upload" runat="server" Width="455px" />
                <asp:Button ID="btnUpdate" class="button" runat="server" Text="上传" CausesValidation="False"
                    OnClick="btnUpdate_Click" Width="54px" />
                <!--<asp:Label ID="lblUpload" CssClass="error" runat="server" Text="文件上传成功" Visible="false"></asp:Label>!-->
            </td>
        </tr>
        <tr>
            <th style="width: 150px;">
                任务代号(<span class="red">*</span>)
            </th>
            <td colspan="3">
                <asp:DropDownList ID="ddlMutiSatTask" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                计划开始时间
            </th>
            <td style="width: 300px;">
                <asp:TextBox ID="txtPlanStartTime" runat="server" CssClass="text" MaxLength="10"
                    ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPlanStartTime"
                    ErrorMessage="开始时间不能为空" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            <th style="width: 150px;">
                计划结束时间
            </th>
            <td>
                <asp:TextBox ID="txtPlanEndTime" runat="server" CssClass="text" MaxLength="10" ClientIDMode="Static"
                    onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPlanEndTime"
                    ErrorMessage="结束时间不能为空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txtPlanStartTime"
                    ControlToValidate="txtPlanEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                    ForeColor="Red" Operator="GreaterThan"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <th>
                计划序号
            </th>
            <td>
                <asp:TextBox ID="txtJXH" CssClass="text" runat="server" Text='<%# Eval("JXH")%>'
                    Enabled="False"></asp:TextBox>
                &nbsp;<span style="color: #3399FF;">自动生成</span>
            </td>
            <th>
                信息分类
            </th>
            <td>
                <asp:DropDownList ID="ddlXXFL" runat="server" DataTextField="Text" DataValueField="Value"
                    Width="154px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                备注
            </th>
            <td colspan="3">
                <asp:TextBox ID="txtNote" runat="server" CssClass="text" MaxLength="100" Width="607px"
                    Height="40px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <%--<asp:TextBox ID="txtSB" MaxLength="2" CssClass="text" runat="server" Text='<%# Eval("SB")%>'
                        onkeypress="return event.keyCode>=48&&event.keyCode<=57||event.keyCode==46" onpaste="return !clipboardData.getData('text').match(/\D/)"
                        ondragenter="return false" Style="ime-mode: Disabled"></asp:TextBox>--%>
    </table>
    <asp:Repeater ID="rpDatas" runat="server" OnItemCommand="rpDatas_ItemCommand" OnItemDataBound="rpDatas_ItemDataBound">
        <HeaderTemplate>
            <table class="edit1" style="width: 900px;" cellpadding="0" id="detailtable">
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <th>任务</th>
                <td colspan="3"><asp:DropDownList ID="ddlTask" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <th style="width: 150px;">
                    工作单位
                </th>
                <td style="width: 300px;">
                <asp:DropDownList ID="ddlDW" Width="260px"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDW_SelectedIndexChanged">
                    </asp:DropDownList>
                    <%--<asp:TextBox ID="txtDW" MaxLength="2" CssClass="text" runat="server" Text='<%# Eval("DW")%>'
                        onkeypress="return event.keyCode>=48&&event.keyCode<=57||event.keyCode==46" onpaste="return !clipboardData.getData('text').match(/\D/)"
                        ondragenter="return false" Style="ime-mode: Disabled"></asp:TextBox>--%>
                    &nbsp;
                </td>
                <th style="width: 150px;">
                    设备代号
                </th>
                <td>
                <asp:DropDownList ID="ddlSB" runat="server">
                    </asp:DropDownList>
                    <%--<asp:TextBox ID="txtSB" MaxLength="2" CssClass="text" runat="server" Text='<%# Eval("SB")%>'
                        onkeypress="return event.keyCode>=48&&event.keyCode<=57||event.keyCode==46" onpaste="return !clipboardData.getData('text').match(/\D/)"
                        ondragenter="return false" Style="ime-mode: Disabled"></asp:TextBox>--%>
                </td>
            </tr>
            <tr>
                <th>
                    工作方式
                </th>
                <td>
                    <asp:DropDownList ID="ddlFS" runat="server" DataTextField="Text" DataValueField="Value"
                        Width="154px">
                    </asp:DropDownList>
                </td>
                <th>
                    计划性质
                </th>
                <td>
                    <asp:DropDownList ID="ddlJXZ" runat="server" DataTextField="Text" DataValueField="Value"
                        Width="154px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    设备工作模式
                </th>
                <td>
                    <asp:DropDownList ID="ddlMS" runat="server" DataTextField="Text" DataValueField="Value"
                        Width="154px">
                    </asp:DropDownList>
                </td>
                <th>
                    本帧计划的圈标识
                </th>
                <td>
                    <asp:DropDownList ID="ddlQB" runat="server" DataTextField="Text" DataValueField="Value"
                        Width="154px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th style="width: 150px;">
                    工作性质
                </th>
                <td>
                    <asp:DropDownList ID="ddlGXZ" runat="server" DataTextField="Text" DataValueField="Value"
                        Width="154px">
                    </asp:DropDownList>
                </td>
                <th>
                    本帧计划飞行圈次
                </th>
                <td>
                    <asp:TextBox ID="txtQH" MaxLength="4" CssClass="text" runat="server" Text='<%# Eval("QH")%>'
                        onkeypress="return event.keyCode>=48&&event.keyCode<=57||event.keyCode==46" onpaste="return !clipboardData.getData('text').match(/\D/)"
                        ondragenter="return false" Style="ime-mode: Disabled"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    任务准备开始时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="txtPreStartTime" CssClass="text" runat="server" Text='<%# Eval("ZHB")%>'
                        onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                </td>
                <th></th>
                <td></td>
            </tr>
            <tr>
                <th>
                    跟踪开始时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="txtTrackStartTime" CssClass="text" runat="server"
                        Text='<%# Eval("GZK")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                </td>
                <th>
                    跟踪结束时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="txtTrackEndTime" CssClass="text" runat="server" Text='<%# Eval("GZJ")%>'
                        onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator11" runat="server" ControlToCompare="txtTrackStartTime"
                        ControlToValidate="txtTrackEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                        ForeColor="Red" Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <th>
                    开上行载波时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="txtWaveOnStartTime" CssClass="text" runat="server"
                        Text='FFFFFFFFFFFFFF' Enabled="False"></asp:TextBox>
                </td>
                <th>
                    关上行载波时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="txtWaveOffStartTime" CssClass="text" runat="server"
                        Text='FFFFFFFFFFFFFF' Enabled="False"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator12" runat="server" ControlToCompare="txtWaveOnStartTime"
                        ControlToValidate="txtWaveOffStartTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                        ForeColor="Red" Operator="GreaterThan" Type="Double" Enabled="false"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <th>
                    任务开始时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="txtStartTime" CssClass="text" runat="server" Text='<%# Eval("RK")%>'
                        onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                </td>
                <th>
                    任务结束时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="txtEndTime" CssClass="text" runat="server" Text='<%# Eval("JS")%>'
                        onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator13" runat="server" ControlToCompare="txtStartTime"
                        ControlToValidate="txtEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red"
                        Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <th>
                    信息类别标志
                </th>
                <td>
                    <asp:DropDownList ID="ddlBID" runat="server" DataTextField="Text" DataValueField="Value"
                        Width="154px">
                    </asp:DropDownList>
                </td>
                <th>
                    实时传送数据标志
                </th>
                <td>
                    <asp:TextBox ID="txtJSBZ" CssClass="text" runat="server" Text='<%# Eval("SBZ")%>'></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    数据传输开始时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="txtTransStartTime" CssClass="text" runat="server"
                        Text='<%# Eval("RTs")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                </td>
                <th>
                    数据传输结束时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="txtTransEndTime" CssClass="text" runat="server" Text='<%# Eval("RTe")%>'
                        onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator14" runat="server" ControlToCompare="txtTransStartTime"
                        ControlToValidate="txtTransEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                        ForeColor="Red" Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <th>
                    数据传输速率
                </th>
                <td>
                    <asp:TextBox ID="txtTransSpeedRate" MaxLength="4" CssClass="text" runat="server"
                        Text='<%# Eval("SL")%>' onkeypress="return event.keyCode>=48&&event.keyCode<=57||event.keyCode==46"
                        onpaste="return !clipboardData.getData('text').match(/\D/)" ondragenter="return false"
                        Style="ime-mode: Disabled"></asp:TextBox>
                </td>
                <th>
                    事后回放传送数据标志
                </th>
                <td>
                    <asp:TextBox ID="txtHBZ" CssClass="text" runat="server" Text='<%# Eval("HBZ")%>'></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    数据起始时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="DataStartTime" CssClass="text" runat="server" Text='<%# Eval("Ts")%>'
                        onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                </td>
                <th>
                    数据结束时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="DataEndTime" CssClass="text" runat="server" Text='<%# Eval("Te")%>'
                        onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator15" runat="server" ControlToCompare="DataStartTime"
                        ControlToValidate="DataEndTime" Display="Dynamic" ErrorMessage="结束时间应大于开始时间"
                        ForeColor="Red" Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <th>
                    事后回放信息类别标志
                </th>
                <td>
                    <asp:DropDownList ID="ddlSHBID" runat="server" DataTextField="Text" DataValueField="Value"
                        Width="154px">
                    </asp:DropDownList>
                </td>
                <th>
                    事后回放数据传输开始时间
                </th>
                <td>
                    <asp:TextBox MaxLength="14" ID="txtSHTransStartTime" CssClass="text" runat="server"
                        Text='<%# Eval("HRTs")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    事后回放数据传输速率
                </th>
                <td>
                    <asp:TextBox ID="txtHSL" MaxLength="4" CssClass="text" runat="server" Text='<%# Eval("SL")%>'
                        onkeypress="return event.keyCode>=48&&event.keyCode<=57||event.keyCode==46" onpaste="return !clipboardData.getData('text').match(/\D/)"
                        ondragenter="return false" Style="ime-mode: Disabled"></asp:TextBox>
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
    <table cellpadding="0" class="edit1" style="width: 900px;">
        <tr id="trMessage" runat="server" visible="false">
            <th style="width: 150px;">
            </th>
            <td colspan="3">
                <asp:Label ID="ltMessage" runat="server" CssClass="error" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <th style="width: 150px;">
                &nbsp;
            </th>
            <td colspan="3">
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" OnClick="btnSubmit_Click" OnClientClick="return CheckClientValidate();" Text="保存计划" />&nbsp;&nbsp;
                <asp:Button ID="btnSaveTo" runat="server" CssClass="button" OnClick="btnSaveTo_Click" OnClientClick="return CheckClientValidate();" Text="另存计划" />&nbsp;&nbsp;
                <asp:Button ID="btnReset" runat="server" CausesValidation="False" CssClass="button" OnClick="btnReset_Click" Text="重置" Width="65px" />&nbsp;&nbsp;
                <asp:Button ID="btnReturn" runat="server" CausesValidation="False" CssClass="button" OnClick="btnReturn_Click" Text="返回" Width="65px" />&nbsp;&nbsp;
                <asp:Button ID="btnSurePlan" CssClass="button" runat="server" Text="确认计划" onclick="btnSurePlan_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnCreateFile" class="button" runat="server" Text="生成文件" Width="65px" onclick="btnCreateFile_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnFormal" CssClass="button" runat="server" OnClick="btnFormal_Click" Text="转为正式计划" />
                <div style="display: none;">
                    <asp:HiddenField ID="HfID" runat="server" />
                    <asp:HiddenField ID="HfFileIndex" runat="server" />
                    <asp:HiddenField ID="hfTaskID" runat="server" />
                    <asp:HiddenField ID="hfSatID" runat="server" />
                    <asp:HiddenField ID="hfStatus" runat="server" />
                    <asp:HiddenField ID="hfURL" runat="server" />
                    <asp:HiddenField ID="hfStationFile" runat="server" ClientIDMode="Static" />
                    <asp:TextBox ID="txtIds" runat="server" ClientIDMode="Static"></asp:TextBox>
                    <asp:Button ID="btnGetStationData" ClientIDMode="Static" class="button" runat="server"
                        CausesValidation="false" OnClick="btnGetStationData_Click" Text="获取数据" />
                        <input type="hidden" id="hidtracktimeonblur" value="0" />
                        <input type="hidden" id="hidtranstimeonblur" value="0" />
                </div>
            </td>
        </tr>
        <div id="divFiles" runat="server" visible="false">
            <tr>
                <th>
                    文件路径
                </th>
                <td class="style1" colspan="3">
                    <asp:Label ID="lblFilePath" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <th></th>
                <td colspan="3">
                    <asp:LinkButton ID="lbtFilePath" runat="server" CausesValidation="false" 
                        onclick="lbtFilePath_Click" Visible="false">保存文件</asp:LinkButton>
                </td>
            </tr>
        </div>
    </table>
    <div id="dialog-station" style="display: none" title="选择进出站及航捷数据">
        <asp:Repeater ID="rpStation" runat="server">
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
</asp:Content>
<%@ Page MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" Language="C#"
    AutoEventWireup="true" CodeBehind="ZZGZJHEdit.aspx.cs" Inherits="OperatingManagement.Web.Views.PlanManage.ZZGZJHEdit" %>

<%@ Register Src="../../ucs/ucTask.ascx" TagName="ucTask" TagPrefix="uc1" %>
<%@ Register Src="../../ucs/ucSatellite.ascx" TagName="ucSatellite" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="planmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuPlan" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    计划管理 &gt; 总装地面站工作计划
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <div>
        <table cellpadding="0" class="edit1" style="width: 950px;">
            <tr>
                <td colspan="4">
                    <strong>计划基本信息</strong>
                </td>
            </tr>
            <tr>
                <th style="width: 100px;">
                    <asp:Button ID="btnGetPlanInfo" runat="server" CssClass="button" OnClick="txtGetPlanInfo_Click"
                        Text="选择测控资源使用计划" CausesValidation="False" />
                    <asp:HiddenField ID="hfSBJHID" runat="server" ClientIDMode="Static" />
                    <div style="display: none;">
                        <asp:Button ID="btnHidden" runat="server" ClientIDMode="Static" Text="btnHidden"
                            OnClick="btnHidden_Click" />
                    </div>
                </th>
                <td colspan="3">
                    <asp:LinkButton ID="btnSBJH" runat="server" ClientIDMode="Static" OnClick="btnSBJH_Click"
                        CausesValidation="False"></asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr>
                <th style="width: 100px;">
                    任务代号(<span class="red">*</span>)
                </th>
                <td style="width: 350px;">
                    <uc1:ucTask ID="ucTask1" runat="server" AllowBlankItem="False" />
                </td>
                <th style="width: 100px;">
                    卫星(<span class="red">*</span>)
                </th>
                <td>
                    <uc2:ucSatellite ID="ucSatellite1" runat="server" AllowBlankItem="False" />
                </td>
            </tr>
            <tr>
                <th style="width: 100px;">
                    计划开始时间
                </th>
                <td>
                    <asp:TextBox ID="txtPlanStartTime" runat="server" CssClass="text" MaxLength="10"
                        ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPlanStartTime"
                        ErrorMessage="开始时间不能为空" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <th style="width: 100px;">
                    计划结束时间
                </th>
                <td>
                    <asp:TextBox ID="txtPlanEndTime" runat="server" CssClass="text" MaxLength="10" 
                        ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPlanEndTime"
                        ErrorMessage="结束时间不能为空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" 
                        ControlToCompare="txtPlanStartTime" ControlToValidate="txtPlanEndTime" 
                        Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red" 
                        Operator="GreaterThan"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <th>
                    编号
                </th>
                <td>
                    <asp:TextBox ID="txtSequence" CssClass="text" runat="server" Enabled="false"></asp:TextBox>
                    &nbsp;<span style="color:#3399FF;">自动生成，不可编辑</span>
                </td>
                <th>
                    时间
                </th>
                <td>
                    <asp:TextBox ID="txtDatetime" CssClass="text" runat="server" Enabled="false"></asp:TextBox>
                    &nbsp;<span style="color:#3399FF;">自动生成，不可编辑</span>
                </td>
            </tr>
            <tr>
                <th>
                    工作单位
                </th>
                <td>
                    <asp:TextBox ID="txtStationName" CssClass="text" runat="server" Style="margin-left: 0px"></asp:TextBox>
                </td>
                <th>
                    设备代号
                </th>
                <td>
                    <asp:TextBox ID="txtEquipmentID" CssClass="text" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th style="width: 100px;">
                    备注
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtNote" runat="server" CssClass="text" MaxLength="100" Width="623px"
                        Height="40px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr style="display: none">
                <th style="width: 100px;">
                    任务个数
                </th>
                <td>
                    <asp:TextBox ID="txtTaskCount" CssClass="text" runat="server" ReadOnly="True"></asp:TextBox>
                </td>
                <th>
                </th>
                <td>
                </td>
            </tr>
        </table>
        <table id="detailtable" cellpadding="0" cellspacing="0" style="width: 950px; border-width: 0px;">
            <tr>
                <td style="padding: 0px 0px;">
                    <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound"
                        OnItemCommand="Repeater1_ItemCommand">
                        <ItemTemplate>
                            <table class="edit1" style="width: 950px;">
                                <tr>
                                    <td colspan="4">
                                    <asp:Label ID="ltTip" runat="server" CssClass="error" Text="以下信息必须填写. 不能为空"></asp:Label>
                                      <%--  <strong><span style="color:#3399FF;">时间格式均为：YYYYMMDDHHmmss。例如2000年1月2日下午3点4分5秒表示为字符串“20000102150405”</span></strong>--%>
                                    </td>
                                </tr>
                                 <tr>
                                    <th  colspan="4" style="text-align: center;">
                                        <b>任务基本信息</b>
                                    </th>
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        任务标志
                                    </th>
                                    <td style="width: 350px;">
                                        <asp:TextBox ID="txtTaskFlag" CssClass="text" runat="server" Text='<%# Eval("TaskFlag")%>'></asp:TextBox>
                                    </td>
                                    <th style="width: 100px;">
                                        工作方式
                                    </th>
                                    <td style="width: 350px;">
                                        <asp:DropDownList ID="ddlFS" runat="server" DataTextField="Text" DataValueField="Value"
                                            Width="154px">
                                        </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtWorkWay" CssClass="text" runat="server" Text='<%# Eval("WorkWay")%>'></asp:TextBox>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        计划性质
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlJXZ" runat="server" DataTextField="Text" DataValueField="Value"
                                            Width="154px">
                                        </asp:DropDownList>
                                        <%-- <asp:TextBox ID="txtPlanPropertiy" CssClass="text" runat="server" Text='<%# Eval("PlanPropertiy")%>'></asp:TextBox>--%>
                                    </td>
                                    <th style="width: 100px;">
                                        工作模式
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlMS" runat="server" DataTextField="Text" DataValueField="Value"
                                            Width="154px">
                                        </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtWorkMode" CssClass="text" runat="server" Text='<%# Eval("WorkMode")%>'></asp:TextBox>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        任务准备开始时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtPreStartTime" CssClass="text" runat="server" Text='<%# Eval("PreStartTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    </td>
                                    <th style="width: 100px;">
                                    </th>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        跟踪开始时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtTrackStartTime" CssClass="text" runat="server" Text='<%# Eval("TrackStartTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    </td>
                                     <th style="width: 100px;">
                                        跟踪结束时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtTrackEndTime" CssClass="text" runat="server" Text='<%# Eval("TrackEndTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator11" runat="server" 
                    ControlToCompare="txtTrackStartTime" ControlToValidate="txtTrackEndTime" 
                    Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red" 
                    Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                                    </td>
                                    
                                </tr>
                                <tr>
                                <th style="width: 100px;">
                                        开上行载波时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtWaveOnStartTime" CssClass="text" runat="server" Text='<%# Eval("WaveOnStartTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    </td>
                                    <th style="width: 100px;">
                                        关上行载波时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtWaveOffStartTime" CssClass="text" runat="server" Text='<%# Eval("WaveOffStartTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    <asp:CompareValidator ID="CompareValidator12" runat="server" 
                    ControlToCompare="txtWaveOnStartTime" ControlToValidate="txtWaveOffStartTime" 
                    Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red" 
                    Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                                    </td>
                                   
                                </tr>
                                <tr>
                                <th style="width: 100px;">
                                        任务开始时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtStartTime" CssClass="text" runat="server" Text='<%# Eval("StartTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    </td>
                                    <th>
                                        任务结束时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtEndTime" CssClass="text" runat="server" Text='<%# Eval("EndTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    <asp:CompareValidator ID="CompareValidator13" runat="server" 
                    ControlToCompare="txtStartTime" ControlToValidate="txtEndTime" 
                    Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red" 
                    Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="4" style="text-align: center;">
                                        <b>实时传输</b>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Repeater ID="rpReakTimeTransfor" runat="server" OnItemCommand="Repeater2_ItemCommand">
                                            <HeaderTemplate>
                                                <table class="list">
                                                    <tr>
                                                        <th style="width: 150px;">
                                                            格式标志
                                                        </th>
                                                        <th style="width: 150px;">
                                                            信息流标志
                                                        </th>
                                                        <th style="width: 150px;">
                                                            数据传输开始时间
                                                        </th>
                                                        <th style="width: 150px;">
                                                            数据传输结束时间
                                                        </th>
                                                        <th style="width: 150px;">
                                                            数据传输速率(BPS)
                                                        </th>
                                                        <th style="width: 100px;">
                                                        </th>
                                                    </tr>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtFormatFlag" CssClass="text" runat="server" Text='<%# Eval("FormatFlag")%>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtInfoFlowFlag" CssClass="text" runat="server" Text='<%# Eval("InfoFlowFlag")%>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="txtTransStartTime" CssClass="text" runat="server" Text='<%# Eval("TransStartTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="txtTransEndTime" CssClass="text" runat="server" Text='<%# Eval("TransEndTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                        <asp:CompareValidator ID="CompareValidator14" runat="server" 
                                                        ControlToCompare="txtTransStartTime" ControlToValidate="txtTransEndTime" 
                                                        Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red" 
                                                        Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTransSpeedRate" MaxLength="4" CssClass="text" runat="server" Text='<%# Eval("TransSpeedRate")%>'
                                                        onkeypress="return event.keyCode>=48&&event.keyCode<=57||event.keyCode==46" 
                                                        onpaste="return !clipboardData.getData('text').match(/\D/)"
                                                        ondragenter="return false" style="ime-mode:Disabled"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="Button2" CausesValidation="False" CssClass="button" CommandName="Add" runat="server" Text="添加" />
                                                        <asp:Button ID="Button3" CausesValidation="False" CssClass="button" CommandName="Del" runat="server" Text="删除" />
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
                                    <th colspan="4" style="text-align: center;">
                                        <b>事后回放</b>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Repeater ID="rpAfterFeedBack" runat="server" OnItemCommand="Repeater3_ItemCommand">
                                            <HeaderTemplate>
                                                <table class="list">
                                                    <tr>
                                                        <th style="width: 125px;">
                                                            格式标志
                                                        </th>
                                                        <th style="width: 125px;">
                                                            信息流标志
                                                        </th>
                                                        <th style="width: 125px;">
                                                            数据起始时间
                                                        </th>
                                                        <th style="width: 125px;">
                                                            数据结束时间
                                                        </th>
                                                        <th style="width: 125px;">
                                                            数据传输开始时间
                                                        </th>
                                                        <th style="width: 125px;">
                                                            数据传输速率
                                                        </th>
                                                        <th style="width: 100px;">
                                                        </th>
                                                    </tr>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="FormatFlag" Width="120px" CssClass="text" runat="server" Text='<%# Eval("FormatFlag")%>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="InfoFlowFlag" Width="120px" CssClass="text" runat="server" Text='<%# Eval("InfoFlowFlag")%>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="DataStartTime" Width="120px" CssClass="text" runat="server" Text='<%# Eval("DataStartTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="DataEndTime" Width="120px" CssClass="text" runat="server" Text='<%# Eval("DataEndTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                   <asp:CompareValidator ID="CompareValidator15" runat="server" 
                                                        ControlToCompare="DataStartTime" ControlToValidate="DataEndTime" 
                                                        Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red" 
                                                        Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="TransStartTime" Width="120px" CssClass="text" runat="server" Text='<%# Eval("TransStartTime")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TransSpeedRate" Width="120px" CssClass="text" runat="server" Text='<%# Eval("TransSpeedRate")%>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="Button5" CausesValidation="False" CssClass="button" CommandName="Add" runat="server" Text="添加" />
                                                        <asp:Button ID="Button6" CausesValidation="False" CssClass="button" CommandName="Del" runat="server" Text="删除" />
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
                                    <td colspan="4" style="text-align:center;">
                                        <asp:Button ID="Button1" CssClass="button" CausesValidation="False" CommandName="Add" runat="server" Text="添加任务" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="Button4" CssClass="button" CausesValidation="False" CommandName="Del" runat="server" Text="删除任务" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </div>
    <br />
   <div style="width: 750px; text-align: center;">
    <asp:Label ID="ltMessage" runat="server" CssClass="error" Text="任务基本信息字段都必须填写。"></asp:Label>
   </div>
    <div style="width: 750px; text-align: center;">
        <asp:Button ID="btnSubmit" runat="server" OnClientClick="return CheckClientValidate();" CssClass="button" Text="保存计划" OnClick="btnSubmit_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnSaveTo" runat="server" OnClientClick="return CheckClientValidate();" CssClass="button" Text="另存计划" OnClick="btnSaveTo_Click" />
    &nbsp;&nbsp;
        <asp:Button ID="btnReset" class="button" runat="server" Text="重置" Width="65px" 
                    onclick="btnReset_Click" CausesValidation="False" />
                    &nbsp;&nbsp; 
        <asp:Button ID="btnReturn" class="button" runat="server" 
                    Text="返回" Width="65px" 
                    onclick="btnReturn_Click" CausesValidation="False" />
    &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnFormal"  class="button" runat="server" onclick="btnFormal_Click"
                    Text="转为正式计划" />
    </div>
    <div style="display: none">
        <asp:HiddenField ID="HfID" runat="server" />
        <asp:HiddenField ID="HfFileIndex" runat="server" />
        <asp:HiddenField ID="hfTaskID" runat="server" />
        <asp:HiddenField ID="hfSatID" runat="server" />
        <asp:HiddenField ID="hfStatus" runat="server" />
        <asp:HiddenField ID="hfURL" runat="server" />
    </div>
    <div id="dialog-form" style="display: none" title="提示信息">
        <p class="content">
        </p>
    </div>
    <div id="dialog-sbjh" style="display: none" title="选择测控资源使用计划">
        <p class="content">
        </p>
        <asp:Repeater ID="rpDatas" runat="server">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <%--<th style="width:20px;"><input type="checkbox" onclick="checkAll(this)" /></th>--%>
                        <th style="width: 150px;">
                            计划编号
                        </th>
                        <th style="width: 150px;">
                            任务代号
                        </th>
                        <th style="width: 150px;">
                            计划类别
                        </th>
                        <th style="width: 150px;">
                            开始时间
                        </th>
                        <th style="width: 150px;">
                            结束时间
                        </th>
                        <th style="width: 100px;">
                            选择
                        </th>
                    </tr>
                    <tbody id="tbUsers">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <%--<td><input type="checkbox" <%# Eval("LoginName").ToString().Equals(this.Profile.UserName,StringComparison.InvariantCultureIgnoreCase)?"disabled=\"true\"":"" %> name="chkDelete" value="<%# Eval("Id") %>" /></td>--%>
                    <td>
                        <%# Eval("planid")%>
                    </td>
                    <td>
                        <%# Eval("taskid")%>
                    </td>
                    <td>
                        <%# Eval("plantype")%>
                    </td>
                    <td>
                        <%# Eval("starttime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                    </td>
                    <td>
                        <%# Eval("endtime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                    </td>
                    <td>
                        <button class="button" onclick="return SelectSBJH('<%# Eval("ID") %>',escape('<%# GetFileName(Eval("FileIndex")) %>'))">
                            选择</button>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <table class="listTitle">
            <tr>
                <td class="listTitle-c1">
                </td>
                <td class="listTitle-c2">
                    <om:CollectionPager ID="cpPager" runat="server" PageSize="5">
                    </om:CollectionPager>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
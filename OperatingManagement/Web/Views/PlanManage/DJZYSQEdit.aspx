<%@ Page MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" Language="C#"
    AutoEventWireup="true" CodeBehind="DJZYSQEdit.aspx.cs" Inherits="OperatingManagement.Web.Views.PlanManage.DJZYSQEdit" %>

<%@ Register Src="../../ucs/ucTask.ascx" TagName="ucTask" TagPrefix="uc1" %>
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
    计划管理 &gt; 测控资源使用申请
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <div>
        <table cellpadding="0" class="edit1" style="width: 950px;">
            <tr>
                <td colspan="4">
                    <strong>计划基本信息</strong>
                </td>
            </tr>
            <tr style="display:none;">
                <th style="width: 100px;">
                    <asp:Button ID="btnGetPlanInfo" runat="server" CssClass="button" OnClick="txtGetPlanInfo_Click"
                        Text="选择测控资源设用计划" CausesValidation="False" />
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
            <tr>
                <th style="width: 100px;">
                    任务(<span class="red">*</span>)
                </th>
                <td style="width: 350px;">

                    <asp:DropDownList ID="ddlTask" ClientIDMode="Static" runat="server" style="display:none;">
                    </asp:DropDownList>

                    <uc3:ucOutTask ID="ucOutTask1" runat="server" AllowBlankItem="False" />

                </td>
                <th>
                    航天器标识</th>
                <td>
                    <asp:TextBox ID="txtSCID" CssClass="text" runat="server" ClientIDMode="Static" ></asp:TextBox>
                    &nbsp;<span style="color:#3399FF;">保存时自动生成</span>
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
                    申请序列号
                </th>
                <td>
                    <asp:TextBox ID="txtSequence" CssClass="text" runat="server"  Enabled="false"></asp:TextBox>
                    &nbsp;<span style="color:#3399FF;">保存时自动生成</span>
                </td>
                <th>
                    申请数量</th>
                <td>
                    <asp:TextBox ID="txtTaskCount" CssClass="text" runat="server" ReadOnly="True"  Enabled="false"></asp:TextBox>
                     &nbsp;<span style="color:#3399FF;">保存时自动生成</span>
                </td>
            </tr>
            <tr>
                <th>
                    备注</th>
                <td colspan="3">
                    <asp:TextBox ID="txtNote" runat="server" CssClass="text" MaxLength="100" Width="623px"
                        Height="40px" TextMode="MultiLine"></asp:TextBox>
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
                                        申请性质
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlSXZ" runat="server" DataTextField="Text" DataValueField="Value"
                                            Width="154px">
                                        </asp:DropDownList>
                                        <%-- <asp:TextBox ID="txtPlanPropertiy" CssClass="text" runat="server" Text='<%# Eval("PlanPropertiy")%>'></asp:TextBox>--%>
                                    </td>
                                    <th style="width: 100px;">
                                    </th>
                                    <td style="width: 350px;">
                                        <asp:TextBox ID="txtSXH" CssClass="text" runat="server" Text='<%# Eval("SXH")%>' Visible="false"></asp:TextBox>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        任务类别
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtMLB" CssClass="text" runat="server" Text='<%# Eval("MLB")%>'>TT</asp:TextBox>
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
                                        工作单元
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlGZDY" runat="server" DataTextField="Text" DataValueField="Value"
                                            Width="154px" >
                                        </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtWorkMode" CssClass="text" runat="server" Text='<%# Eval("WorkMode")%>'></asp:TextBox>--%>
                                    </td>
                                    <th style="width: 100px;">
                                        设备代号
                                    </th>
                                    <td style="width: 350px;">
                                        <asp:DropDownList ID="ddlSBDH" runat="server"  DataTextField="Text" DataValueField="Value"
                                            Width="154px" style="display:none;" >
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtSBDH" CssClass="text" runat="server" Text='<%# Eval("SBDH")%>'></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        圈次
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtQC" CssClass="text" runat="server" Text='<%# Eval("QC")%>' 
                                        onkeypress="return event.keyCode>=48&&event.keyCode<=57||event.keyCode==46" 
                                                        onpaste="return !clipboardData.getData('text').match(/\D/)"
                                                        ondragenter="return false" style="ime-mode:Disabled"></asp:TextBox>
                                         <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtQC"
                                ErrorMessage="只能是数字" ForeColor="Red" ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
                                    </td>
                                    <th style="width: 100px;">
                                    圈标
                                    </th>
                                    <td>
                                    <asp:TextBox MaxLength="14" ID="txtQB" CssClass="text" runat="server" Text='<%# Eval("QB")%>' ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        测控事件类型
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlSHJ" runat="server" DataTextField="Text" DataValueField="Value"
                                            Width="154px">
                                        </asp:DropDownList>
                                    </td>
                                    <th style="width: 100px;">
                                        同时支持目标数
                                    </th>
                                    <td>
                                    <asp:TextBox MaxLength="14" ID="txtTNUM" CssClass="text" runat="server" Text='<%# Eval("TNUM")%>' 
                                    onkeypress="return event.keyCode>=48&&event.keyCode<=57||event.keyCode==46" 
                                                        onpaste="return !clipboardData.getData('text').match(/\D/)"
                                                        ondragenter="return false" style="ime-mode:Disabled"></asp:TextBox>
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTNUM"
                                ErrorMessage="只能是数字" ForeColor="Red" ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        任务准备开始时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtPreStartTime" CssClass="text" runat="server" Text='<%# Eval("ZHB")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    </td>
                                    <th style="width: 100px;">
                                        任务开始时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtStartTime" CssClass="text" runat="server" Text='<%# Eval("RK")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        跟踪开始时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtTrackStartTime" CssClass="text" runat="server" Text='<%# Eval("GZK")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    </td>
                                     <th style="width: 100px;">
                                        开上行载波时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtWaveOnStartTime" CssClass="text" runat="server" Text='FFFFFFFFFFFFFF' Enabled="False"></asp:TextBox>
                                    </td>
                                    
                                </tr>
                                <tr>
                                
                                    <th style="width: 100px;">
                                        关上行载波时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtWaveOffStartTime" CssClass="text" runat="server" Text='FFFFFFFFFFFFFF' Enabled="False"></asp:TextBox>
                                    <asp:CompareValidator ID="CompareValidator12" runat="server" 
                    ControlToCompare="txtWaveOnStartTime" ControlToValidate="txtWaveOffStartTime" 
                    Display="Dynamic" ErrorMessage="关上行载波时间应大于开上行载波时间" ForeColor="Red" 
                    Operator="GreaterThan" Type="Double" Enabled="false"></asp:CompareValidator>
                                    </td>
                                   <th style="width: 100px;">
                                        跟踪结束时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtTrackEndTime" CssClass="text" runat="server" Text='<%# Eval("GZJ")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator11" runat="server" 
                    ControlToCompare="txtTrackStartTime" ControlToValidate="txtTrackEndTime" 
                    Display="Dynamic" ErrorMessage="跟踪结束时间应大于跟踪开始时间" ForeColor="Red" 
                    Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>

                                    <th style="width: 100px;">
                                        任务结束时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtEndTime" CssClass="text" runat="server" Text='<%# Eval("JS")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    <asp:CompareValidator ID="CompareValidator13" runat="server" 
                                        ControlToCompare="txtStartTime" ControlToValidate="txtEndTime" 
                                        Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red" 
                                        Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                                    </td>
                                    <th></th>
                                    <td><span style="color:#3399FF;">任务准备开始时间与跟踪结束时间时间差应不大于3小时</span></td>
                                </tr>
                                <tr>
                                    <th colspan="4" style="text-align: center;">
                                        <b>实时传输</b>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Repeater ID="rpReakTimeTransfor" runat="server" OnItemCommand="Repeater2_ItemCommand"
                                         OnItemDataBound="Repeater2_ItemDataBound">
                                            <HeaderTemplate>
                                                <table class="list">
                                                    <tr>
                                                        <th align="left" style="width: 150px; ">
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
                                                        <asp:TextBox ID="txtFormatFlag" MaxLength="8" CssClass="text" runat="server" Text='<%# Eval("GBZ")%>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtInfoFlowFlag" CssClass="text" runat="server" Text='<%# Eval("XBZ")%>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="txtTransStartTime" CssClass="text" runat="server" Text='<%# Eval("RTs")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="txtTransEndTime" CssClass="text" runat="server" Text='<%# Eval("RTe")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                        <asp:CompareValidator ID="CompareValidator14" runat="server" 
                                                        ControlToCompare="txtTransStartTime" ControlToValidate="txtTransEndTime" 
                                                        Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red" 
                                                        Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTransSpeedRate" MaxLength="4" CssClass="text" runat="server" Text='<%# Eval("SL")%>'
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
                                                        <asp:TextBox ID="FormatFlag" Width="120px" CssClass="text"  MaxLength="8" runat="server" Text='<%# Eval("GBZ")%>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="InfoFlowFlag" Width="120px" CssClass="text" runat="server" Text='<%# Eval("XBZ")%>'></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="DataStartTime" Width="120px" CssClass="text" runat="server" Text='<%# Eval("Ts")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="DataEndTime" Width="120px" CssClass="text" runat="server" Text='<%# Eval("Te")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                   <asp:CompareValidator ID="CompareValidator15" runat="server" 
                                                        ControlToCompare="DataStartTime" ControlToValidate="DataEndTime" 
                                                        Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red" 
                                                        Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="TransStartTime" Width="120px" CssClass="text" runat="server" Text='<%# Eval("RTs")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TransSpeedRate" Width="120px" CssClass="text" runat="server" Text='<%# Eval("SL")%>'></asp:TextBox>
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
                                    <th colspan="4" style="text-align: center;">
                                        <b>工作点频</b>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Repeater ID="rpGZDP" runat="server" OnItemCommand="RepeaterGZDP_ItemCommand"
                                         OnItemDataBound="RepeaterGZDP_ItemDataBound">
                                            <HeaderTemplate>
                                                <table class="list">
                                                    <tr>
                                                        <th style="width: 100px;">
                                                            点频序号
                                                        </th>
                                                        <th style="width: 100px;">
                                                            频段选择
                                                        </th>
                                                        <th style="width: 100px;">
                                                            点频选择
                                                        </th>
                                                        <th>
                                                        </th>
                                                    </tr>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="txtFXH" CssClass="text" runat="server" Text='<%# Eval("FXH")%>' ></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    <asp:DropDownList ID="ddlPDXZ" runat="server" DataTextField="Text" DataValueField="Value"
                                                            Width="154px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="txtDPXZ" CssClass="text" runat="server" Text='<%# Eval("DPXZ")%>' ></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="Button15" CausesValidation="False" CssClass="button" CommandName="Add" runat="server" Text="添加" />
                                                        <asp:Button ID="Button16" CausesValidation="False" CssClass="button" CommandName="Del" runat="server" Text="删除" />
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
   <div style="width: 850px; text-align: center;">
    <asp:Label ID="ltMessage" runat="server" CssClass="error" Text="任务基本信息字段都必须填写。"></asp:Label>
   </div>
    <div style="width: 850px; text-align: center;">
        <asp:Button ID="btnSubmit" runat="server" OnClientClick="return CheckClientValidate();" CssClass="button" Text="保存计划" OnClick="btnSubmit_Click" />
        &nbsp;&nbsp;
        <asp:Button ID="btnSaveTo" runat="server" OnClientClick="return CheckClientValidate();" CssClass="button" Text="另存计划" OnClick="btnSaveTo_Click" />
    &nbsp;&nbsp;
        <asp:Button ID="btnReset" CssClass="button" runat="server" Text="重置" Width="65px" 
                    onclick="btnReset_Click" CausesValidation="False" />
                    &nbsp;&nbsp; 
        <asp:Button ID="btnReturn" CssClass="button" runat="server" 
                    Text="返回" Width="65px" 
                    onclick="btnReturn_Click" CausesValidation="False" />
    &nbsp;&nbsp;
                <asp:Button ID="btnFormal"  CssClass="button" runat="server" onclick="btnFormal_Click"
                    Text="转为正式计划" />
        &nbsp;&nbsp;
                <asp:Button ID="btnSurePlan"  CssClass="button" runat="server"
                    Text="确认计划" onclick="btnSurePlan_Click" />
        &nbsp;&nbsp;
                <asp:Button ID="btnWord"  CssClass="button" runat="server" onclick="btnWord_Click" CausesValidation="false"
                    Text="导出Word文档" />
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
        <asp:Button ID="btnGetStationData" ClientIDMode="Static" class="button" runat="server" CausesValidation="false"
                    OnClick="btnGetStationData_Click" Text="获取数据" />
        <input type="hidden" id="hidtracktimeonblur" value="0" />
        <input type="hidden" id="hidtranstimeonblur" value="0" />
    </div>
    <div id="dialog-form" style="display: none" title="提示信息">
        <p class="content">
        </p>
    </div>
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
                        
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </om:CollectionPager>
                </td>
            </tr>
        </table>
    </div>
    <div id="dialog-sbdh" style="display: none" title="选择喀什站对应的设备代号">
    <input id="radio1" type="radio" value="喀什站(TS-4217)" checked="checked" 
            name="kashi" />喀什站(TS-4217)<input id="radio2" 
            type="radio" value="喀什站(TY-4801)" name="kashi" />喀什站(TY-4801)
    </div>
</asp:Content>
<%@ Page MaintainScrollPositionOnPostback="true"  Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
CodeBehind="DJZYJHDetail.aspx.cs" Inherits="OperatingManagement.Web.Views.PlanManage.DJZYJHDetail" %>

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
    计划管理 &gt; 测控资源使用计划
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
                </td>
            </tr>
            <tr>
                <th style="width: 100px;">
                    计划开始时间
                </th>
                <td>
                    <asp:TextBox ID="txtPlanStartTime" runat="server" CssClass="text" MaxLength="10"
                        ClientIDMode="Static"></asp:TextBox>
                </td>
                <th style="width: 100px;">
                    计划结束时间
                </th>
                <td>
                    <asp:TextBox ID="txtPlanEndTime" runat="server" CssClass="text" MaxLength="10" 
                        ClientIDMode="Static"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    计划序列号
                </th>
                <td>
                    <asp:TextBox ID="txtSequence" CssClass="text" runat="server"  Enabled="false"></asp:TextBox>
                </td>
                <th>
                    计划数量</th>
                <td>
                    <asp:TextBox ID="txtTaskCount" CssClass="text" runat="server" ReadOnly="True"  Enabled="false"></asp:TextBox>
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
                    <asp:Repeater ID="Repeater1" runat="server" 
                        onitemdatabound="Repeater1_ItemDataBound">
                        <ItemTemplate>
                            <table class="edit1" style="width: 950px;">
                                 <tr>
                                    <th  colspan="4" style="text-align: center;">
                                        <b>任务基本信息</b>
                                    </th>
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        计划性质
                                    </th>
                                    <td>
                                         <asp:TextBox ID="txtPlanPropertiy" CssClass="text" runat="server" Text='<%# Eval("SXZ")%>'></asp:TextBox>
                                    </td>
                                    <th style="width: 100px;">
                                    答复标志
                                    </th>
                                    <td style="width: 350px;">
                                        <asp:TextBox ID="txtDF" CssClass="text" runat="server" Text='<%# Eval("DF")%>' ></asp:TextBox>
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
                                        <asp:TextBox ID="txtFS" CssClass="text" runat="server" Text='<%# Eval("FS")%>'></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        工作单元
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtGZDY" CssClass="text" runat="server" Text='<%# Eval("GZDY")%>'></asp:TextBox>
                                    </td>
                                    <th style="width: 100px;">
                                        设备代号
                                    </th>
                                    <td style="width: 350px;">
                                        <asp:TextBox ID="txtSBDH" CssClass="text" runat="server" Text='<%# Eval("SBDH")%>'></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="width: 100px;">
                                        圈次
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtQC" CssClass="text" runat="server" Text='<%# Eval("QC")%>'></asp:TextBox>
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
                                        <asp:TextBox MaxLength="14" ID="txtSHJ" CssClass="text" runat="server" Text='<%# Eval("SHJ")%>' ></asp:TextBox>
                                    </td>
                                    <th style="width: 100px;">
                                        同时支持目标数
                                    </th>
                                    <td>
                                    <asp:TextBox MaxLength="14" ID="txtTNUM" CssClass="text" runat="server" Text='<%# Eval("TNUM")%>'></asp:TextBox>
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
                                    </td>
                                   <th style="width: 100px;">
                                        跟踪结束时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtTrackEndTime" CssClass="text" runat="server" Text='<%# Eval("GZJ")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>

                                    <th style="width: 100px;">
                                        任务结束时间
                                    </th>
                                    <td>
                                        <asp:TextBox MaxLength="14" ID="txtEndTime" CssClass="text" runat="server" Text='<%# Eval("JS")%>' onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                                    </td>
                                    <th></th>
                                    <td></td>
                                </tr>
                                <tr>
                                    <th colspan="4" style="text-align: center;">
                                        <b>工作点频</b>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Repeater ID="rpGZDP" runat="server"   OnItemDataBound="RepeaterGZDP_ItemDataBound">
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
                                                    </tr>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="txtFXH" CssClass="text" runat="server" Text='<%# Eval("FXH")%>' ></asp:TextBox>
                                                    </td>
                                                    <td>
                                                       <asp:TextBox MaxLength="14" ID="txtPDXZ" CssClass="text" runat="server" Text='<%# Eval("PDXZ")%>' ></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox MaxLength="14" ID="txtDPXZ" CssClass="text" runat="server" Text='<%# Eval("DPXZ")%>' ></asp:TextBox>
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
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </div>
    <br />
   <div style="width: 750px; text-align: center;">
    <asp:Label ID="ltMessage" runat="server" CssClass="error" Text=""></asp:Label>
   </div>
    <div style="width: 750px; text-align: center;">
        &nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;
        <asp:Button ID="btnReturn" class="button" runat="server" 
                    Text="返回" Width="65px" 
                    onclick="btnReturn_Click" CausesValidation="False" />
        &nbsp;&nbsp;
                <asp:Button ID="btnWord"  class="button" runat="server" onclick="btnWord_Click" CausesValidation="false"
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
        <input type="hidden" id="hidtracktimeonblur" value="0" />
        <input type="hidden" id="hidtranstimeonblur" value="0" />
    </div>
    <div id="dialog-form" style="display: none" title="提示信息">
        <p class="content">
        </p>
    </div>
</asp:Content>
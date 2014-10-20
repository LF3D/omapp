<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="YJJHEdit.aspx.cs" Inherits="OperatingManagement.Web.Views.PlanManage.YJJHEdit" %>
<%@ Register src="../../ucs/ucTask.ascx" tagname="ucTask" tagprefix="uc1" %>
<%@ Register src="../../ucs/ucSatellite.ascx" tagname="ucSatellite" tagprefix="uc2" %>
<%@ Register src="../../ucs/ucTimer.ascx" tagname="ucTimer" tagprefix="uc3" %>
<%@ Register src="../../ucs/ucOutTask.ascx" tagname="ucOutTask" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="planmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuPlan" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    计划管理 &gt; 应用研究计划
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="edit1" style="width:800px;">
        <%# Eval("planid")%>
        <tr>
            <th style="width:130px;">任务(<span class="red">*</span>)</th>
            <td>
                <uc4:ucOutTask ID="ucOutTask1" runat="server" AllowBlankItem="False" />
            </td>
        </tr>
        <tr>
            <th>信息分类</th>
            <td>
                <asp:RadioButtonList ID="radBtnXXFL" runat="server" 
                    RepeatDirection="Horizontal" ClientIDMode="Static" BorderColor="White" 
                    BorderStyle="Double" BorderWidth="1px">
                    <asp:ListItem Value="ZJ" Selected="True">周计划</asp:ListItem>
                    <asp:ListItem Value="RJ">日计划</asp:ListItem> 
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th>计划序号</th>
            <td>
                <asp:TextBox ID="txtJXH" runat="server" Width="300px" CssClass="text" 
                    MaxLength="20" Enabled="False"></asp:TextBox>
                    &nbsp;<span style="color:#3399FF;">自动生成，不可编辑</span>
            </td>
        </tr>
        <tr>
            <th>系统名称</th>
            <td>
                <asp:DropDownList ID="ddlSysName" runat="server" Height="20px" Width="300px" ClientIDMode="Static" style="display:none;">
                </asp:DropDownList>
                <asp:TextBox ID="txtSysName" runat="server" Enabled="False" Width="300px" CssClass="text" ></asp:TextBox>&nbsp;
                <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtSysName"
                        ErrorMessage="系统名称不能为空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
        <asp:Repeater ID="rpTasks" runat="server" OnItemCommand="rpTasks_ItemCommand">
            <HeaderTemplate>
                <table class="edit1" style="width: 900px;" cellpadding="0" id="detailtable">
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <th  style="width:125px;">试验开始时间</th>
                    <td>
                        <asp:TextBox ID="txtStartTime" runat="server" Width="300px" CssClass="text" Text='<%# Eval("StartTime")%>' 
                            MaxLength="14" ClientIDMode="Static"   onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartTime"
                                ErrorMessage="开始时间不能为空" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                </tr>
                <tr>
                    <th>试验结束时间</th>
                    <td>
                        <asp:TextBox ID="txtEndTime" runat="server" Width="300px" CssClass="text" MaxLength="14" ClientIDMode="Static" 
                        Text='<%# Eval("EndTime")%>'   onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"></asp:TextBox>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEndTime"
                                ErrorMessage="结束时间不能为空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" 
                            ControlToCompare="txtStartTime" ControlToValidate="txtEndTime" 
                            Display="Dynamic" ErrorMessage="结束时间应大于开始时间" ForeColor="Red" 
                            Operator="GreaterThan" Type="Double"></asp:CompareValidator>
                        </td>
                </tr>
                <tr>
                    <th>系统任务</th>
                    <td>
                        <asp:TextBox ID="txtTask" runat="server" Width="390px"  CssClass="text" 
                            Text='<%# Eval("Task")%>' 
                            MaxLength="128" Height="47px" TextMode="MultiLine"></asp:TextBox>
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
            <th>备注</th>
            <td>
                <asp:TextBox ID="txtNote" runat="server" CssClass="text" MaxLength="50" 
                    Width="390px" Height="47px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr id="trMessage" runat="server" visible="false">
            <th></th>
            <td>
                <asp:Label ID="ltMessage" runat="server" CssClass="error" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <th>&nbsp;</th>
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="保存计划" onclick="btnSubmit_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnSaveTo" runat="server" CssClass="button" Text="另存计划" onclick="btnSaveTo_Click" />&nbsp;&nbsp; 
                <asp:Button ID="btnReset" CssClass="button" runat="server" Text="重置" Width="65px" onclick="btnReset_Click" CausesValidation="False" />&nbsp;&nbsp; 
                <asp:Button ID="btnReturn" CssClass="button" runat="server" Text="返回" Width="65px" onclick="btnReturn_Click" CausesValidation="False" />&nbsp;&nbsp;
                <asp:Button ID="btnSurePlan"  CssClass="button" runat="server" Text="确认计划" onclick="btnSurePlan_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnFormal"  CssClass="button" runat="server" onclick="btnFormal_Click" Text="转为正式计划" />&nbsp;&nbsp;
                <asp:Button class="button" ID="btnCreateFile" runat="server" Text="生成文件" Width="65px" onclick="btnCreateFile_Click" />
                <asp:HiddenField ID="HfID" runat="server" />
                <asp:HiddenField ID="HfFileIndex" runat="server" />
                <asp:HiddenField ID="hfTaskID" runat="server" />
                <asp:HiddenField ID="hfSatID" runat="server" />
                <asp:HiddenField ID="hfStatus" runat="server" />
                <asp:HiddenField ID="hfURL" runat="server" />
            </td>
        </tr>
        <div id="divFiles" runat="server" visible="false">
            <tr style="height:24px;">
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
                        onclick="lbtFilePath_Click">保存文件</asp:LinkButton>
                </td>
            </tr>
        </div>
    </table>
    <div id="dialog-form" style="display:none" title="提示信息">
	    <p class="content"></p>
    </div>
    <div id="dialog-sbjh" style="display: none" title="选择测控资源使用计划">
        <p class="content">
        </p>
        <asp:Repeater ID="rpDatas" runat="server" 
            onitemdatabound="rpDatas_ItemDataBound">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <th style="width: 100px;">
                            计划编号
                        </th>
                        <th style="width: 150px;">
                            任务代号
                        </th>
                        <th style="width: 200px;">
                            开始时间
                        </th>
                        <th style="width: 200px;">
                            结束时间
                        </th>
<%--                        <th style="width: 70px;">
                            选择
                        </th>--%>
                    </tr>
                    <tbody id="tbUsers">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("planid")%>
                    </td>
                    <td>
                        <%# Eval("taskid")%>
                    </td>
                    <td>
                        <%# Eval("starttime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                    </td>
                    <td>
                        <%# Eval("endtime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                    </td>
<%--                    <td>
                        <button class="button" onclick="return SelectSYJH('<%# Eval("ID") %>',escape('<%# GetFileName(Eval("FileIndex")) %>'))">
                            选择</button>
                    </td>--%>
                </tr>
                <tr>
                <td colspan="4">
                <asp:Repeater ID="rpSY" runat="server">
                            <HeaderTemplate>
                                <table class="list">
                                    <tr>
                                        <th style="width: 100px;">
                                            卫星名称
                                        </th>
                                        <th style="width: 110px;">
                                            开始时间
                                        </th>
                                        <th style="width: 110px;">
                                            结束时间
                                        </th>
                                        <th style="width: 150px;">
                                            系统名称
                                        </th>
                                        <th  style="width: 150px;">
                                            系统任务
                                        </th>
                                        <th></th>
                                    </tr>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSYSatName" Width="100px" CssClass="text" runat="server" Text='<%# Eval("SYSatName")%>'></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSYStartTime" Width="110px" CssClass="text" runat="server" Text='<%# Eval("SYStartTime")%>'></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSYEndTime" Width="110px" CssClass="text" runat="server" Text='<%# Eval("SYEndTime")%>'></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSYSysName" Width="150px" CssClass="text" runat="server" Text='<%# Eval("SYSysName")%>'></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSYSysTask" Width="150px" CssClass="text" runat="server" Text='<%# Eval("SYSysTask")%>'></asp:TextBox>
                                    </td>
                                    <td>
                                    <button class="button" onclick="return SelectSYJH('<%# Eval("SYSatName") %>','<%# Eval("SYStartTime") %>',
                                    '<%# Eval("SYEndTime") %>','<%# Eval("SYSysName") %>','<%# Eval("SYSysTask") %>' )">选择</button>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody> </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    
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
                    <om:CollectionPager ID="cpPager" runat="server" PageSize="1">
                    </om:CollectionPager>
                </td>
            </tr>
        </table>

    </div>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CenterOutputPolicyAdd.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.CenterOutputPolicyAdd" %>

<%@ Register src="../../ucs/ucTask.ascx" tagname="ucTask" tagprefix="uc1" %>
<%@ Register src="../../ucs/ucXYXSInfo.ascx" tagname="ucXYXSInfo" tagprefix="uc2" %>
<%@ Register src="../../ucs/ucInfoType.ascx" tagname="ucInfoType" tagprefix="uc3" %>
<%@ Register src="../../ucs/ucSatellite.ascx" tagname="ucSatellite" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="bizmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuBusiness" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    中心输出策略管理 &gt; 新增中心输出策略
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
   <%-- <script type="text/javascript">
        $(function () {
            $("#txtEffectTime").datepicker();
            $("#txtDefectTime").datepicker();
        });
    </script>--%>
    <table class="edit" style="width: 800px;">
        <tr>
            <th style="width: 150px;">
                任务代号(<span class="red">*</span>)
            </th>
            <td>
                <uc1:ucTask ID="dplTask" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                卫星名称(<span class="red">*</span>)
            </th>
            <td>
                <uc4:ucSatellite ID="dplSatellite" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                信源(<span class="red">*</span>)
            </th>
            <td>
                <uc2:ucXYXSInfo ID="dplSource" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                信息类别(<span class="red">*</span>)
            </th>
            <td>
                <uc3:ucInfoType ID="dplInfoType" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                信宿(<span class="red">*</span>)
            </th>
            <td>
                <uc2:ucXYXSInfo ID="dplDdestination" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                生效时间(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtEffectTime" runat="server" ClientIDMode="Static" CssClass="norText" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtEffectTime" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                失效时间(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtDefectTime" runat="server" ClientIDMode="Static" CssClass="norText" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtDefectTime" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtDefectTime" ControlToCompare="txtEffectTime" Type="Date"
                    Operator="GreaterThanEqual" ErrorMessage="失效时间应大于生效时间"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <th>
                描述
            </th>
            <td>
                <asp:TextBox ID="txtNote" runat="server" Width="320px" Height="120px" CssClass="text"
                    MaxLength="200" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr id="trMessage" runat="server" visible="false">
            <th>
                &nbsp;
            </th>
            <td>
                <asp:Label ID="lblMessage" runat="server" CssClass="error" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                &nbsp;
            </th>
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="提 交" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnReset" runat="server" CssClass="button" Text="清 除" OnClick="btnReset_Click"
                    CausesValidation="false" />
                <asp:Button ID="btnReturn" runat="server" CssClass="button" Text="返 回" OnClick="btnReturn_Click"
                    CausesValidation="false" />
            </td>
        </tr>
    </table> 
</asp:Content>

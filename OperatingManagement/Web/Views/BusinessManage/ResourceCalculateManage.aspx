<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ResourceCalculateManage.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.ResourceCalculateManage" %>
<%@ Import Namespace="OperatingManagement.DataAccessLayer.BusinessManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  <style type="text/css">
        .norText
        {
            width: 155px;
            margin: 0px;
            padding: 0px;
        }
        .norDpl
        {
            width: 160px;
            margin: 0px;
            padding: 0px;
        }
        .index_content_search
        {
            margin-top: 10px;
        }
        
        .index_content_search table
        {
            border: 1px solid #eeeeee;
            border-collapse: collapse;
            width: 100%;
        }
        
        .index_content_search table td
        {
            border: 1px solid #eeeeee;
            line-height: 26px;
            color: #333333;
            text-align: left;
            height: 26px;
        }
        .index_content_search table th
        {
            border: 1px solid #eeeeee;
            font-weight: bold;
            line-height: 26px;
            color: #333333;
            text-align: right;
            height: 26px;
        }
        .index_content_view
        {
            margin-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="resmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuRes" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 查询资源调度计算
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
<%--<script type="text/javascript">
    $(function () {
        $("#txtBeginTime").datepicker();
        $("#txtEndTime").datepicker();
    });
    </script>--%>
    <div class="index_content_search">
        <table cellspacing="0" cellpadding="0" class="searchTable">
            <tr>
                <th width="15%">
                    计算状态：
                </th>
                <td width="25%">
                    <asp:DropDownList ID="dplStatus" runat="server" CssClass="norDpl">
                    </asp:DropDownList>
                </td>
                <th width="15%">
                    计算结果文件来源：
                </th>
                <td width="25%">
                    <asp:DropDownList ID="dplResultFileSource" runat="server" CssClass="norDpl">
                    </asp:DropDownList>
                </td>
                <td width="20%">
                </td>
            </tr>
            <tr>
                <th width="15%">
                    起始时间：
                </th>
                <td width="25%">
                <asp:TextBox ID="txtBeginTime" runat="server" CssClass="norText" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtBeginTime" ErrorMessage="（必填）" ValidationGroup="SearchResourceCalculate"></asp:RequiredFieldValidator>
                </td>
                <th width="15%">
                    结束时间：
                </th>
                <td width="25%">
                <asp:TextBox ID="txtEndTime" runat="server" CssClass="norText" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtEndTime" ErrorMessage="（必填）" ValidationGroup="SearchResourceCalculate"></asp:RequiredFieldValidator>
                </td>
                <td width="20%">
                     <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="button" ValidationGroup="SearchResourceCalculate"
                        Text="查 询" />
                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" CssClass="button" Text="添 加" />
                </td>
            </tr>
        </table>
         <table cellspacing="0" cellpadding="0" style="margin-top:5px;" class="searchTable">
            <tr>
                <th width="15%">
                    上传计算结果文件：
                </th>
                <td width="65%">
                  <asp:FileUpload ID="fileUploadResultFile" runat="server" />
                </td>
                <td width="20%">
                    <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" CssClass="button"
                        Text="上 传" />
                </td>
            </tr>
        </table>
    </div>
    <div class="index_content_view">
        <asp:Repeater ID="rpResourceCalculateList" runat="server" onitemdatabound="rpResourceCalculateList_ItemDataBound">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <th style="width: 20%;">
                            资源需求文件
                        </th>
                        <th style="width: 20%;">
                            计算结果文件
                        </th>
                        <th style="width: 10%;">
                            计算结果文件来源
                        </th>
                        <th style="width: 8%;">
                            计算结果
                        </th>
                         <th style="width: 8%;">
                            计算状态
                        </th>
                        <th style="width: 8%;">
                            创建时间
                        </th>
                        <th style="width: 9%;">
                            资源需求文件
                        </th>
                        <th style="width: 9%;">
                            计算结果文件
                        </th>
                        <th style="width: 8%;">
                            计算结果
                        </th>
                    </tr>
                    <tbody id="tbResourceCalculateList">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("RequirementFileDisplayName")%>
                    </td>
                    <td>
                         <%# Eval("ResultFileDisplayName")%>
                    </td>
                    <td>
                         <%#SystemParameters.GetSystemParameterText(SystemParametersType.ResourceCalculateResultFileSource, Eval("ResultFileSource").ToString())%>
                    </td>
                    <td>
                        <%#SystemParameters.GetSystemParameterText(SystemParametersType.ResourceCalculateResult, Eval("CalculateResult").ToString())%>
                    </td>
                     <td>
                        <%#SystemParameters.GetSystemParameterText(SystemParametersType.ResourceCalculateStatus, Eval("Status").ToString())%>
                    </td>
                    <td>
                        <%# Eval("CreatedTime", "{0:" + this.SiteSetting.DateFormat + "}")%>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnRequirementFileDownload" runat="server" OnClick="lbtnRequirementFileDownload_Click" CommandArgument='<%# Eval("Id")%>'>下载</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnResultFileDownload" runat="server" OnClick="lbtnResultFileDownload_Click" CommandArgument='<%# Eval("Id")%>'>下载</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnCalculateResultView" runat="server" OnClick="lbtnCalculateResultView_Click" CommandArgument='<%# Eval("Id")%>'>查看</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody></table>
            </FooterTemplate>
        </asp:Repeater>
        <table class="listTitle">
            <tr>
                <td class="listTitle-c1">
                </td>
                <td class="listTitle-c2">
                    <om:CollectionPager ID="cpResourceCalculatePager" runat="server">
                    </om:CollectionPager>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

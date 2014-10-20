<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ResourceStatusEdit.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.ResourceStatusEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="resmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuRes" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 编辑资源状态
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <%-- <script type="text/javascript">
        $(function () {
            $("#txtBeginTime").datepicker();
            $("#txtEndTime").datepicker();
        });
    </script>--%>
    <table class="edit" style="width: 800px;">
        <tr>
            <th style="width: 150px;">
                状态类型(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplStatusType" runat="server" CssClass="norDpl">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                资源类型(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplResourceType" runat="server" CssClass="norDpl">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                资源编号(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtResourceCode" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtResourceCode" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="trHealthStatusFunctionType" runat="server">
            <th>
                功能类型(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplFunctionType" runat="server" CssClass="norDpl">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfv2" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="dplFunctionType" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="trHealthStatus" runat="server">
            <th>
                健康状态(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplHealthStatus" runat="server" CssClass="norDpl">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfv3" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="dplHealthStatus" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="trUseStatusUsedType" runat="server">
            <th>
                占用类型(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplUsedType" runat="server" CssClass="norDpl" AutoPostBack="True"
                    OnSelectedIndexChanged="dplUsedType_SelectedIndexChanged" CausesValidation="false">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="dplUsedType" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                起始时间(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtBeginTime" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"
                    CssClass="norText"></asp:TextBox>
                <asp:DropDownList ID="dplBeginTimeHour" runat="server" CssClass="norDpl" Width="60px"
                    Visible="false">
                </asp:DropDownList>
                <asp:DropDownList ID="dplBeginTimeMinute" runat="server" CssClass="norDpl" Width="60px"
                    Visible="false">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtBeginTime" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                结束时间(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtEndTime" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"
                    CssClass="norText"></asp:TextBox>
                <asp:DropDownList ID="dplEndTimeHour" runat="server" CssClass="norDpl" Width="60px"
                    Visible="false">
                </asp:DropDownList>
                <asp:DropDownList ID="dplEndTimeMinute" runat="server" CssClass="norDpl" Width="60px"
                    Visible="false">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtEndTime" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <%--<asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtEndTime" ControlToCompare="txtBeginTime" Type="Date" Operator="GreaterThanEqual"
                    ErrorMessage="起始时间应大于结束时间"></asp:CompareValidator>--%>
            </td>
        </tr>
        <tr id="trUseStatusUsedBy" runat="server">
            <th>
                服务对象(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtUsedBy" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtUsedBy" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="trUseStatusUsedCategory" runat="server">
            <th>
                服务种类(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtUsedCategory" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtUsedCategory" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="trUseStatusUsedFor" runat="server">
            <th>
                占用原因(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtUsedFor" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtUsedFor" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="trUseStatusCanBeUsed" runat="server">
            <th>
                是否可执行任务(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplCanBeUsed" runat="server" CssClass="norDpl">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="dplCanBeUsed" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                创建时间
            </th>
            <td>
                <asp:Label ID="lblCreatedTime" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <th>
                最后修改时间
            </th>
            <td>
                <asp:Label ID="lblUpdatedTime" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr id="trMessage" runat="server" visible="false">
            <th>
                &nbsp;
            </th>
            <td>
                <asp:Label ID="lblMessage" runat="server" CssClass="error" Text=""></asp:Label><asp:LinkButton
                    ID="lbtnReSubmit" runat="server" CommandName="ReSubmit" Visible="false" Text="继 续"
                    OnClick="btnSubmit_Click"></asp:LinkButton>
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

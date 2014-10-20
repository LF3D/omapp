<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ResourceRequirementAdd.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.ResourceRequirementAdd" %>

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
            width: 800px;
            margin: 10px 0px 10px 0px;
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
    业务管理 &gt; 新增资源调度计算
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
   <%-- <script type="text/javascript">
        $(function () {
            $("#txtTimeBenchmark").datepicker();
            $("#txtBeginTime").datepicker();
            $("#txtEndTime").datepicker();
        });
    </script>--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table class="edit" style="width: 800px;">
        <tr>
            <th style="width: 150px;">
                需求名称(<span class="red">*</span>)
            </th>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:Label ID="lblRequirementName" runat="server" Text=""></asp:Label>
                        <asp:HiddenField ID="hidWXBMIndex" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="dplSatName" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <th>
                优先级(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtPriority" runat="server" CssClass="norText"></asp:TextBox>（1-100的整数）
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtPriority" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Integer" Display="Dynamic"
                    MinimumValue="1" MaximumValue="100" ForeColor="Red" ControlToValidate="txtPriority"
                    ErrorMessage="（1-100的整数）"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <th>
                卫星编码(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplSatName" runat="server" CssClass="norDpl" AutoPostBack="True"
                    OnSelectedIndexChanged="dplSatName_SelectedIndexChanged" CausesValidation="false">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="dplSatName" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
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
        <tr>
            <th>
                持续时长(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtPersistenceTime" runat="server" CssClass="norText" MaxLength="9"></asp:TextBox>秒
                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtPersistenceTime" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="RangeValidator2" runat="server" Type="Integer" Display="Dynamic"
                    MinimumValue="1" MaximumValue="1000000000" ForeColor="Red" ControlToValidate="txtPersistenceTime"
                    ErrorMessage="（应为大于0的整数）"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <th>
                不可用设备(<span class="red">*</span>)
            </th>
            <td>
                <table width="100%">
                    <tr>
                        <th width="30%">
                            已添加不可用设备
                        </th>
                        <td width="70%">
                            <asp:Repeater ID="rpUnusedEquipmentList" runat="server">
                                <HeaderTemplate>
                                    <table class="list">
                                        <tr>
                                            <th style="width: 10%; text-align: center;">
                                                地面站编码
                                            </th>
                                            <th style="width: 10%; text-align: center;">
                                                地面站设备编码
                                            </th>
                                            <th style="width: 10%; text-align: center;">
                                                删除不可用设备
                                            </th>
                                        </tr>
                                        <tbody id="tbUnusedEquipmentList">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: center;">
                                            <%# Eval("GRCode")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("EquipmentCode")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:LinkButton ID="lbtnDeleteUnusedEquipment" runat="server" OnClick="lbtnDeleteUnusedEquipment_Click"
                                                OnClientClick="javascript:return confirm('是否删除不可用设备？')" CausesValidation="false"
                                                CommandName="delete" CommandArgument='<%# Eval("GRCode").ToString() + "$" + Eval("EquipmentCode").ToString()%>'>删除</asp:LinkButton>
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
                                        <om:CollectionPager ID="cpUnusedEquipmentPager" runat="server">
                                        </om:CollectionPager>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            地面站编码(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtGRCode" runat="server" CssClass="norText"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvGRCode" runat="server" Display="Dynamic" ValidationGroup="UnusedEquipment"
                                ForeColor="Red" ControlToValidate="txtGRCode" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            地面站设备编码(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtGREquipmentCode" runat="server" CssClass="norText"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvGREquipmentCode" runat="server" Display="Dynamic"
                                ValidationGroup="UnusedEquipment" ForeColor="Red" ControlToValidate="txtGREquipmentCode"
                                ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                        </th>
                        <td>
                            <asp:Button ID="btnAddUnusedEquipment" runat="server" CssClass="button" Text="添 加"
                                ValidationGroup="UnusedEquipment" OnClick="btnAddUnusedEquipment_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th>
                支持时段(<span class="red">*</span>)
            </th>
            <td>
                <table width="100%">
                    <tr>
                        <th width="30%">
                            已添加支持时段
                        </th>
                        <td width="70%">
                            <asp:Repeater ID="rpPeriodOfTimeList" runat="server">
                                <HeaderTemplate>
                                    <table class="list">
                                        <tr>
                                            <th style="width: 10%; text-align: center;">
                                                开始时间
                                            </th>
                                            <th style="width: 10%; text-align: center;">
                                                结束时间
                                            </th>
                                            <th style="width: 10%; text-align: center;">
                                                删除支持时段
                                            </th>
                                        </tr>
                                        <tbody id="tbPeriodOfTimeList">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: center;">
                                            <%# Convert.ToDateTime(Eval("BeginTime")).ToString("yyyyMMddHHmmss")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Convert.ToDateTime(Eval("EndTime")).ToString("yyyyMMddHHmmss")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:LinkButton ID="lbtnDeletePeriodOfTime" runat="server" OnClick="lbtnDeletePeriodOfTime_Click"
                                                OnClientClick="javascript:return confirm('是否删除支持时段？')" CausesValidation="false"
                                                CommandName="delete" CommandArgument='<%# Eval("BeginTime").ToString() + "$" + Eval("EndTime").ToString()%>'>删除</asp:LinkButton>
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
                                        <om:CollectionPager ID="cpPeriodOfTimePager" runat="server">
                                        </om:CollectionPager>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            开始时间(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtBeginTime" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})" CssClass="norText"></asp:TextBox>
                            <asp:DropDownList ID="dplBeginTimeHour" runat="server" CssClass="norDpl" Width="60px" Visible="false">
                            </asp:DropDownList>
                            <asp:DropDownList ID="dplBeginTimeMinute" runat="server" CssClass="norDpl" Width="60px" Visible="false">
                            </asp:DropDownList>
                            <asp:DropDownList ID="dplBeginTimeSecond" runat="server" CssClass="norDpl" Width="60px" Visible="false">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvBeginTime" runat="server" Display="Dynamic" ValidationGroup="PeriodOfTime"
                                ForeColor="Red" ControlToValidate="txtBeginTime" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            结束时间(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtEndTime" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})" CssClass="norText"></asp:TextBox>
                            <asp:DropDownList ID="dplEndTimeHour" runat="server" CssClass="norDpl" Width="60px" Visible="false">
                            </asp:DropDownList>
                            <asp:DropDownList ID="dplEndTimeMinute" runat="server" CssClass="norDpl" Width="60px" Visible="false">
                            </asp:DropDownList>
                            <asp:DropDownList ID="dplEndTimeSecond" runat="server" CssClass="norDpl" Width="60px" Visible="false">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEndTime" runat="server" Display="Dynamic" ValidationGroup="PeriodOfTime"
                                ForeColor="Red" ControlToValidate="txtEndTime" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                        <%--    <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtEndTime" ControlToCompare="txtBeginTime" Type="Date" Operator="GreaterThanEqual"
                                ValidationGroup="PeriodOfTime" ErrorMessage="起始时间应大于结束时间"></asp:CompareValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <th>
                        </th>
                        <td>
                            <asp:Button ID="btnAddPeriodOfTime" runat="server" CssClass="button" Text="添 加" ValidationGroup="PeriodOfTime"
                                OnClick="btnAddPeriodOfTime_Click" />
                        </td>
                    </tr>
                </table>
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
                <asp:Button ID="btnSave" runat="server" CssClass="button" Text="保 存" OnClick="btnSave_Click" />
            </td>
        </tr>
    </table>
    <div class="index_content_view">
        <asp:Repeater ID="rpResourceRequirementList" runat="server" OnItemDataBound="rpResourceRequirementList_ItemDataBound">
            <HeaderTemplate>
                <table class="list">
                    <tr>
                        <th style="width: 10%;">
                            需求名称
                        </th>
                        <th style="width: 10%;">
                            优先级
                        </th>
                        <th style="width: 15%;">
                            卫星编码
                        </th>
                        <th style="width: 15%;">
                            功能类型
                        </th>
                        <th style="width: 10%;">
                            持续时长
                        </th>
                        <th style="width: 15%;">
                            不可用设备
                        </th>
                        <th style="width: 15%;">
                            支持时段
                        </th>
                        <th style="width: 5%; display: none;">
                            编辑
                        </th>
                        <th style="width: 5%; display: none;">
                            删除
                        </th>
                    </tr>
                    <tbody id="tbResourceRequirementList">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("RequirementName")%>
                    </td>
                    <td>
                        <%# Eval("Priority")%>
                    </td>
                    <td>
                        <%# Eval("WXBM")%>
                    </td>
                    <td>
                        <%# Eval("FunctionType")%>
                    </td>
                    <td>
                        <%# Eval("PersistenceTime")%>
                    </td>
                    <td>
                        <asp:Label ID="lblUnusedEquipment" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPeriodOfTime" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="display: none;">
                        <asp:LinkButton ID="lbtnEditResourceRequirement" runat="server" CausesValidation="false"
                            OnClick="lbtnEditResourceRequirement_Click" CommandArgument='<%# Eval("RequirementName")%>'>编辑</asp:LinkButton>
                    </td>
                    <td style="display: none;">
                        <asp:LinkButton ID="lbtnDeleteResourceRequirement" runat="server" CausesValidation="false"
                            OnClick="lbtnDeleteResourceRequirement_Click" OnClientClick="javascript:return confirm('是否删除该资源需求？')"
                            CommandArgument='<%# Eval("RequirementName")%>'>删除</asp:LinkButton>
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
                    <om:CollectionPager ID="cpResourceRequirementPager" runat="server">
                    </om:CollectionPager>
                </td>
            </tr>
        </table>
        <table class="edit" style="width: 800px; margin-top: 10px;">
            <tr>
                <th style="width: 150px;">
                    时间基准(<span class="red">*</span>)
                </th>
                <td style="width: 350px;">
                    <asp:TextBox ID="txtTimeBenchmark" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})" CssClass="norText" ValidationGroup="Calculate"></asp:TextBox>
                    <asp:DropDownList ID="dplTimeBenchmarkHour" runat="server" CssClass="norDpl" Width="60px" Visible="false">
                    </asp:DropDownList>
                    <asp:DropDownList ID="dplTimeBenchmarkMinute" runat="server" CssClass="norDpl" Width="60px" Visible="false">
                    </asp:DropDownList>
                    <asp:DropDownList ID="dplTimeBenchmarkSecond" runat="server" CssClass="norDpl" Width="60px" Visible="false">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                        ValidationGroup="Calculate" ForeColor="Red" ControlToValidate="txtTimeBenchmark"
                        ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                </td>
                <td style="width: 300px;">
                    <asp:Button ID="btnCalculate" runat="server" CssClass="button" Text="计 算" OnClick="btnCalculate_Click"
                        ValidationGroup="Calculate" />
                    <asp:Button ID="btnReset" runat="server" CssClass="button" Text="清 除" OnClick="btnReset_Click"
                        CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

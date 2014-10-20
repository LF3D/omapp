<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="GDJHYB.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.GDJHYB" %>

<%@ Register Src="../../ucs/ucSatellite.ascx" TagName="ucSatellite" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            height: 20px;
        }
        .style2
        {
            width: 120px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="gdfx" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuGD" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 轨道分析 - 交会预报
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <%--<script type="text/javascript">
        $(function () {
            $("#txtCutMainReportBeginDate").datepicker();
            $("#txtCutMainLYDate").datepicker();
            $("#txtCutSubLYDate").datepicker();
        });
    </script>--%>
    <table class="edit" style="width: 800px; margin: 10px 0px;">
        <tr>
            <th style="width: 120px;">
                交会预报文件选项(<span class="red">*</span>)
            </th>
            <td>
                <asp:RadioButtonList ID="rblFileOption" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true" 
                    OnSelectedIndexChanged="rblFileOption_SelectedIndexChanged" BorderColor="White" 
                    BorderStyle="Double" BorderWidth="2px">
                    <asp:ListItem Text="文件上传" Value="0" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="手工录入" Value="1"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <div id="divFileUpload" runat="server" visible="true">
        <table id="tbCutMainUpload" class="edit" style="width: 800px;">
            <tr>
                <th class="style2" style="width:120px;">
                    CutMain文件(<span class="red">*</span>)
                </th>
                <td>
                    <asp:FileUpload ID="fuCutMainFile" ClientIDMode="Static" runat="server" />
                    <asp:RequiredFieldValidator ID="rvfCutMainFile" runat="server" Display="Dynamic"
                        ForeColor="Red" ControlToValidate="fuCutMainFile" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th class="style2">
                    CutSub文件(<span class="red">*</span>)
                </th>
                <td>
                    <asp:FileUpload ID="fuCutSubFile" ClientIDMode="Static" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" Display="Dynamic"
                        ForeColor="Red" ControlToValidate="fuCutSubFile" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th class="style2">
                    CutOptional文件(<span class="red">*</span>)
                </th>
                <td>
                    <asp:FileUpload ID="fuCutOptionalFile" ClientIDMode="Static" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" Display="Dynamic"
                        ForeColor="Red" ControlToValidate="fuCutOptionalFile" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </div>
    <div id="divFillIn" runat="server" visible="false">
        <asp:Menu ID="menuCut" runat="server" Orientation="Horizontal" 
            OnMenuItemClick="menuCut_MenuItemClick" BackColor="#99CCFF">
            <Items>
                <asp:MenuItem Text="CutMain" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="CutSub" Value="1"></asp:MenuItem>
                <asp:MenuItem Text="CutOptional" Value="2"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:MultiView ID="mvCut" runat="server" ActiveViewIndex="0">
            <asp:View ID="cutMain" runat="server">
                <table id="tbCutMainFillIn" class="edit" style="width: 800px;">
                    <tr>
                        <th style="width: 140px;">
                            预报起始历元日期(<span class="red">*</span>)
                        </th>
                        <td style="width: 280px;">
                            <asp:TextBox ID="txtCutMainReportBeginDate" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"
                                CssClass="norText"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCutMainReportBeginDate" runat="server" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMainReportBeginDate" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                        </td>
                        <th style="width: 120px;">
                            预报起始历元时刻(<span class="red">*</span>)
                        </th>
                        <td style="width: 280px;">
                            <asp:TextBox ID="txtCutMainReportBeginTimeMilliSecond" runat="server"
                                MaxLength="7" CssClass="norText"></asp:TextBox>毫秒
                            <asp:RequiredFieldValidator ID="rfvCutMainReportBeginTimeMilliSecond" runat="server"
                                Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMainReportBeginTimeMilliSecond"
                                ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMainReportBeginTimeMilliSecond" runat="server" Type="Double"
                                MaximumValue="999.999" MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMainReportBeginTimeMilliSecond"
                                ErrorMessage="（范围F6.3）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutMainReportBeginTimeMilliSecond" runat="server" ValidationExpression="^\d+(\.\d{1,3})?$" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtCutMainReportBeginTimeMilliSecond" ErrorMessage="（最多三位小数）"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            预报结束历元日期(<span class="red">*</span>)
                        </th>
                        <td style="width: 280px;">
                            <asp:TextBox ID="txtCutMainReportEndDate" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})"
                                CssClass="norText"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCutMainReportEndDate" runat="server" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMainReportEndDate" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                        </td>
                        <th style="width: 120px;">
                            预报结束历元时刻(<span class="red">*</span>)
                        </th>
                        <td style="width: 280px;">
                            <asp:TextBox ID="txtCutMainReportEndTimeMilliSecond" runat="server"
                                MaxLength="7" CssClass="norText"></asp:TextBox>毫秒
                            <asp:RequiredFieldValidator ID="rfvCutMainEndTimeMS" runat="server"
                                Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMainReportEndTimeMilliSecond"
                                ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMainEndTimeMS" runat="server" Type="Double"
                                MaximumValue="999.999" MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMainReportEndTimeMilliSecond"
                                ErrorMessage="（范围F6.3）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutMainEndTimeMS" runat="server" ValidationExpression="^\d+(\.\d{1,3})?$" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtCutMainReportEndTimeMilliSecond" ErrorMessage="（最多三位小数）"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            历元日期(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainLYDate" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})" CssClass="norText"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCutMainLYDate" runat="server" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMainLYDate" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                        </td>
                        <th>
                            历元时刻(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainLYTimeMilliSecond" runat="server" MaxLength="7" CssClass="norText"></asp:TextBox>毫秒
                            <asp:RequiredFieldValidator ID="rfvCutMainLYTimeMilliSecond" runat="server" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMainLYTimeMilliSecond" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMainLYTimeMilliSecond" runat="server" Type="Double"
                                MaximumValue="999.999" MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMainLYTimeMilliSecond"
                                ErrorMessage="（范围F6.3）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutMainLYTimeMilliSecond" runat="server" ValidationExpression="^\d+(\.\d{1,3})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMainLYTimeMilliSecond" ErrorMessage="（最多三位小数）"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            主星(<span class="red">*</span>)
                        </th>
                        <td colspan="3">
                            <asp:DropDownList ID="dplCutMainSatellite" runat="server" CssClass="norDpl" AutoPostBack="true"
                                OnSelectedIndexChanged="dplCutMainSatellite_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            NO(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainSatelliteNO" runat="server" Enabled="false" CssClass="norText"></asp:TextBox>
                        </td>
                        <th>
                            KK(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainSatelliteKK" runat="server" Enabled="false" CssClass="norText"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Sm(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainSatelliteSm" runat="server" Enabled="false" CssClass="norText"></asp:TextBox>米/千克
                        </td>
                        <th>
                            Ref(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainSatelliteRef" runat="server" Enabled="false" CssClass="norText"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            D1(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainD1" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutMainD1Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutMainD1" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtCutMainD1" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMainD1" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMainD1"
                                ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                             <asp:RegularExpressionValidator ID="revCutMainD1" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMainD1" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                        <th>
                            D2(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainD2" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutMainD2Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutMainD2" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtCutMainD2" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMainD2" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMainD2"
                                ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                           <asp:RegularExpressionValidator ID="revCutMainD2" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMainD2" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            D3(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainD3" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutMainD3Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutMainD3" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtCutMainD3" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMainD3" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMainD3"
                                ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutMainD3" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMainD3" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                        <th>
                            D4(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainD4" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutMainD4Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutMainD4" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtCutMainD4" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMainD4" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMainD4"
                                ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutMainD4" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMainD4" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            D5(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainD5" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutMainD5Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutMainD5" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtCutMainD5" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMainD5" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMainD5"
                                ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutMainD5" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMainD5" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                        <th>
                            D6(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMainD6" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutMainD6Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutMainD6" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtCutMainD6" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMainD6" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMainD6"
                                ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutMainD6" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMainD6" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            dR(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMaindR" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutMaindRUnit" runat="server" ClientIDMode="Static" Text="Km"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutMaindR" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtCutMaindR" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMaindR" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMaindR"
                                ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutMaindR" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMaindR" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                        <th>
                            KAE(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:RadioButtonList ID="rblCutMainKAE" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" 
                                OnSelectedIndexChanged="rblCutMainKAE_SelectedIndexChanged" BorderColor="White" 
                                BorderStyle="Double" BorderWidth="2px">
                                <asp:ListItem Text="考虑" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="不考虑" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            dA(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMaindA" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutMaindAUnit" runat="server" ClientIDMode="Static" Text="deg"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutMaindA" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtCutMaindA" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMaindA" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMaindA"
                                ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutMaindA" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMaindA" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                        <th>
                            dE(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutMaindE" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutMaindEUnit" runat="server" ClientIDMode="Static" Text="deg"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutMaindE" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtCutMaindE" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutMaindE" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ForeColor="Red" ControlToValidate="txtCutMaindE"
                                ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutMaindE" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutMaindE" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="cutSub" runat="server">
                <table id="tbCutSubFillIn" class="edit" style="width: 800px;">
                    <tr>
                        <th style="width: 140px;">
                            主星列表信息
                        </th>
                        <td colspan="3">
                            <asp:Repeater ID="rpCutSubList" runat="server">
                                <HeaderTemplate>
                                    <table class="list">
                                        <tr>
                                            <th style="width: 15%; text-align: center;">
                                                主星
                                            </th>
                                            <th style="width: 10%; text-align: center;">
                                                NO
                                            </th>
                                            <th style="width: 15%; text-align: center;">
                                                历元
                                            </th>
                                            <th style="width: 6%; text-align: center;">
                                                KK
                                            </th>
                                            <th style="width: 6%; text-align: center;">
                                                D1
                                            </th>
                                            <th style="width: 6%; text-align: center;">
                                                D2
                                            </th>
                                            <th style="width: 6%; text-align: center;">
                                                D3
                                            </th>
                                            <th style="width: 6%; text-align: center;">
                                                D4
                                            </th>
                                            <th style="width: 6%; text-align: center;">
                                                D5
                                            </th>
                                            <th style="width: 6%; text-align: center;">
                                                D6
                                            </th>
                                            <th style="width: 6%; text-align: center;">
                                                Sm
                                            </th>
                                            <th style="width: 6%; text-align: center;">
                                                Ref
                                            </th>
                                            <th style="width: 6%; text-align: center;">
                                                删除
                                            </th>
                                        </tr>
                                        <tbody id="tbCutSubList">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: center;">
                                            <%# Eval("SatelliteName")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("SatelliteNO")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("LYSK")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("KK")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("D1")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("D2")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("D3")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("D4")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("D5")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("D6")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("Sm")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <%# Eval("Ref")%>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:LinkButton ID="lbtnDeleteCutSub" runat="server" OnClick="lbtnDeleteCutSub_Click"
                                                OnClientClick="javascript:return confirm('是否删除该条主星记录？')" CausesValidation="false"
                                                CommandName="delete" CommandArgument='<%# Eval("Id").ToString()%>'>删除</asp:LinkButton>
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
                                        <om:CollectionPager ID="cpCuSubPager" runat="server">
                                        </om:CollectionPager>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 140px;">
                            历元日期(<span class="red">*</span>)
                        </th>
                        <td style="width: 260px;">
                            <asp:TextBox ID="txtCutSubLYDate" runat="server" ClientIDMode="Static" onfocus="WdatePicker({dateFmt:'yyyyMMddHHmmss'})" CssClass="norText"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCutSubLYDate" runat="server" Display="Dynamic"
                                ValidationGroup="AddCutSubItem" ForeColor="Red" ControlToValidate="txtCutSubLYDate"
                                ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                        </td>
                        <th style="width: 120px;">
                            历元时刻(<span class="red">*</span>)
                        </th>
                        <td style="width: 280px;">
                            <asp:TextBox ID="txtCutSubLYTimeMilliSecond" runat="server" MaxLength="7" CssClass="norText"></asp:TextBox>毫秒
                            <asp:RequiredFieldValidator ID="rfvCutSubLYTimeMilliSecond" runat="server" Display="Dynamic"
                                ValidationGroup="AddCutSubItem" ForeColor="Red" ControlToValidate="txtCutSubLYTimeMilliSecond"
                                ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutSubLYTimeMilliSecond" runat="server" Type="Double" MaximumValue="999.999"
                                MinimumValue="0.0" Display="Dynamic" ValidationGroup="AddCutSubItem" ForeColor="Red"
                                ControlToValidate="txtCutSubLYTimeMilliSecond" ErrorMessage="（范围F6.3）"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            主星(<span class="red">*</span>)
                        </th>
                        <td colspan="3">
                            <asp:DropDownList ID="dplCutSubSatellite" runat="server" CssClass="norDpl" AutoPostBack="true"
                                OnSelectedIndexChanged="dplCutSubSatellite_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            NO(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutSubSatelliteNO" runat="server" Enabled="false" CssClass="norText"></asp:TextBox>
                        </td>
                        <th>
                            KK(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutSubSatelliteKK" runat="server" Enabled="false" CssClass="norText"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Sm(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutSubSatelliteSm" runat="server" Enabled="false" CssClass="norText"></asp:TextBox>米/千克
                        </td>
                        <th>
                            Ref(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutSubSatelliteRef" runat="server" Enabled="false" CssClass="norText"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            D1(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutSubD1" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutSubD1Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutSubD1" runat="server" Display="Dynamic" ValidationGroup="AddCutSubItem"
                                ForeColor="Red" ControlToValidate="txtCutSubD1" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutSubD1" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ValidationGroup="AddCutSubItem" ForeColor="Red"
                                ControlToValidate="txtCutSubD1" ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutSubD1" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutSubD1" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                        <th>
                            D2(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutSubD2" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutSubD2Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutSubD2" runat="server" Display="Dynamic" ValidationGroup="AddCutSubItem"
                                ForeColor="Red" ControlToValidate="txtCutSubD2" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutSubD2" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ValidationGroup="AddCutSubItem" ForeColor="Red"
                                ControlToValidate="txtCutSubD2" ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutSubD2" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutSubD2" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            D3(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutSubD3" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutSubD3Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutSubD3" runat="server" Display="Dynamic" ValidationGroup="AddCutSubItem"
                                ForeColor="Red" ControlToValidate="txtCutSubD3" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutSubD3" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ValidationGroup="AddCutSubItem" ForeColor="Red"
                                ControlToValidate="txtCutSubD3" ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutSubD3" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutSubD3" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                        <th>
                            D4(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutSubD4" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutSubD4Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutSubD4" runat="server" Display="Dynamic" ValidationGroup="AddCutSubItem"
                                ForeColor="Red" ControlToValidate="txtCutSubD4" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutSubD4" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ValidationGroup="AddCutSubItem" ForeColor="Red"
                                ControlToValidate="txtCutSubD4" ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                             <asp:RegularExpressionValidator ID="revCutSubD4" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutSubD4" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            D5(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutSubD5" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutSubD5Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutSubD5" runat="server" Display="Dynamic" ValidationGroup="AddCutSubItem"
                                ForeColor="Red" ControlToValidate="txtCutSubD5" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutSubD5" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ValidationGroup="AddCutSubItem" ForeColor="Red"
                                ControlToValidate="txtCutSubD5" ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutSubD5" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutSubD5" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                        <th>
                            D6(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:TextBox ID="txtCutSubD6" runat="server" ClientIDMode="Static" CssClass="norText"></asp:TextBox>
                            <asp:Label ID="lblCutSubD6Unit" runat="server" ClientIDMode="Static" Text="米"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvCutSubD6" runat="server" Display="Dynamic" ValidationGroup="AddCutSubItem"
                                ForeColor="Red" ControlToValidate="txtCutSubD6" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutSubD6" runat="server" Type="Double" MaximumValue="9999999999.999999"
                                MinimumValue="0.0" Display="Dynamic" ValidationGroup="AddCutSubItem" ForeColor="Red"
                                ControlToValidate="txtCutSubD6" ErrorMessage="（范围F16.6）"></asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="revCutSubD6" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutSubD6" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                        </th>
                        <td colspan="3">
                            <asp:Button ID="btnAddCutSubItem" runat="server" CssClass="button" Text="添 加" ValidationGroup="AddCutSubItem"
                                OnClick="btnAddCutSubItem_Click" />
                            <asp:Button ID="btnResetCutSubItem" runat="server" CssClass="button" Text="清 除" OnClick="btnResetCutSubItem_Click" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="cutOptional" runat="server">
                <table id="tbCutOptionalFillIn" class="edit" style="width: 800px;">
                    <tr>
                        <th style="width: 140px;">
                            预报数据时间间隔(<span class="red">*</span>)
                        </th>
                        <td style="width: 260px;">
                            <asp:TextBox ID="txtCutOptionalTimeInterval" runat="server" ClientIDMode="Static"
                                CssClass="norText" Width="60px"></asp:TextBox>秒
                            <asp:RequiredFieldValidator ID="rfvCutOptionalTimeInterval" runat="server" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutOptionalTimeInterval" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvCutOptionalTimeInterval" runat="server" ControlToValidate="txtCutOptionalTimeInterval" Display="Dynamic"
                                ForeColor="Red" Type="Double" MinimumValue="1.0" MaximumValue="30.0" ErrorMessage="（范围1-30秒）"></asp:RangeValidator>
                             <asp:RegularExpressionValidator ID="revCutOptionalTimeInterval" runat="server" ValidationExpression="^\d+(\.\d{1,6})?$" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="txtCutOptionalTimeInterval" ErrorMessage="（最多六位小数）"></asp:RegularExpressionValidator>
                        </td>
                        <th style="width: 120px;">
                            第三体引力(<span class="red">*</span>)
                        </th>
                        <td style="width: 280px;">
                            <asp:RadioButtonList ID="rblCutOptionalGravitation" runat="server" 
                                RepeatDirection="Horizontal" BorderColor="White" BorderStyle="Double" 
                                BorderWidth="2px">
                                <asp:ListItem Text="考虑" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="不考虑" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            潮汐摄动(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:RadioButtonList ID="rblCutOptionalTide" runat="server" 
                                RepeatDirection="Horizontal" BorderColor="White" BorderStyle="Double" 
                                BorderWidth="2px">
                                <asp:ListItem Text="考虑" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="不考虑" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <th>
                            光压摄动(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:RadioButtonList ID="rblCutOptionalLight" runat="server" 
                                RepeatDirection="Horizontal" BorderColor="White" BorderStyle="Double" 
                                BorderWidth="2px">
                                <asp:ListItem Text="考虑" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="不考虑" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            大气阻尼摄动(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:RadioButtonList ID="rblCutOptionalEther" runat="server" 
                                RepeatDirection="Horizontal" BorderColor="White" BorderStyle="Double" 
                                BorderWidth="2px">
                                <asp:ListItem Text="考虑" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="不考虑" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <th>
                            后牛顿项(<span class="red">*</span>)
                        </th>
                        <td>
                            <asp:RadioButtonList ID="rblCutOptionalNewton" runat="server" 
                                RepeatDirection="Horizontal" BorderColor="White" BorderStyle="Double" 
                                BorderWidth="2px">
                                <asp:ListItem Text="考虑" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="不考虑" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
    <table class="edit" style="width: 800px; margin: 10px 0px;">
        <tr id="trMessage" runat="server" visible="false">
            <th>
                &nbsp;
            </th>
            <td>
                <asp:Label ID="lblMessage" runat="server" CssClass="error" Text="“相关文件”内容必须严格按照格式要求编写。"></asp:Label>
            </td>
        </tr>
        <div id="divCalResult" runat="server" visible="false">
            <tr style="height:24px;">
                <th>
                    计算结果文件路径
                </th>
                <td>
                    <asp:Label ID="lblResultFilePath" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr style="height:24px;">
                <th class="style1">
                    计算结果
                </th>
                <td class="style1">
                    <asp:Label ID="lblCalResult" runat="server" Text="等待计算" ForeColor="Red"></asp:Label>
                    &nbsp;
                    <asp:LinkButton ID="lbtUNWFileDownload" runat="server" OnClick="lbtUNWFileDownload_Click" CausesValidation="false">保存UNW结果文件</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtSTWFileDownload" runat="server" OnClick="lbtSTWFileDownload_Click" CausesValidation="false">保存STW结果文件</asp:LinkButton>
                    <!--<asp:LinkButton ID="lbtnViewResult" runat="server" 
                        OnClientClick="javascript:_resultDialog.dialog('open');return false;" 
                        CausesValidation="false" Visible="False">查看</asp:LinkButton> !-->
                    &nbsp;
                    <asp:LinkButton ID="lbtViewCurves" runat="server" OnClick="lbtViewCurves_Click" CausesValidation="false">查看曲线图</asp:LinkButton>
                </td>
            </tr>
        </div>
        <tr>
            <th style="width: 120px;">
                &nbsp;
            </th>
            <td>
                <asp:Button ID="btnCalculate" runat="server" Text="开始计算" CssClass="button" OnClick="btnCalculate_Click" />
                <asp:Button ID="btnResetAll" runat="server" Text="全部清除" CssClass="button" OnClick="btnResetAll_Click" CausesValidation="false" />
            </td>
        </tr>
    </table>
    <div id="dialog-form" style="display: none" title="提示信息">
        <p class="content">
            <b>CuMain文件路径：</b><asp:Literal ID="ltCutMainFilePath" runat="server"></asp:Literal><br />
            <b>CuSub文件路径：</b><asp:Literal ID="ltCutSubFilePath" runat="server"></asp:Literal><br />
            <b>CuOptinal文件路径：</b><asp:Literal ID="ltCutOptinalFilePath" runat="server"></asp:Literal><br />
            <b>CuMain文件内容：</b><asp:Literal ID="ltCutMainFile" runat="server"></asp:Literal><br />
            <b>CuSub文件内容：</b><asp:Literal ID="ltCutSubFile" runat="server"></asp:Literal><br />
            <b>CuOptinal文件内容：</b><asp:Literal ID="ltCutOptinalFile" runat="server"></asp:Literal><br />
        </p>
    </div>
    <div id="result-form" style="display: none" title="结果文件内容">
        <p class="content">
            <b>结果文件内容：</b><asp:Literal ID="ltResultFile" runat="server"></asp:Literal><br />
        </p>
    </div>
</asp:Content>

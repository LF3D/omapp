<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="GroundResourceAdd.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.GroundResourceAdd" %>
<%@ Import Namespace="OperatingManagement.DataAccessLayer.BusinessManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="resmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuRes" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 新增地面站资源
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="edit" style="width: 800px;">
        <tr>
            <th style="width: 200px;">
                设备名称(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtEquipmentName" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv1" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtEquipmentName" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                设备编号(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtEquipmentCode" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv2" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtEquipmentCode" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
         <tr>
            <th>
                地面站(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplGroundStation" runat="server" CssClass="norDpl">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfv3" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="dplGroundStation" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
         <tr>
            <th>
                是否光学设备(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplOpticalEquipment" runat="server" CssClass="norDpl">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfv4" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="dplOpticalEquipment" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                功能类型(<span class="red">*</span>)
            </th>
            <td>
                <asp:CheckBoxList ID="cblFunctionType" runat="server" BorderWidth="0" RepeatColumns="4" BorderStyle="None">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <th>
                经度坐标值(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtLongitude" runat="server" CssClass="norText"></asp:TextBox>度（地心系BLH，东经正值西经负值）
                <asp:RequiredFieldValidator ID="rfv6" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtLongitude" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rv1" runat="server" Display="Dynamic" MinimumValue="-180"
                    MaximumValue="180" ControlToValidate="txtLongitude" Type="Double" ForeColor="Red"
                    ErrorMessage="（-180至180）"></asp:RangeValidator>
                <asp:RegularExpressionValidator ID="rev1" runat="server" ValidationExpression="^(-?\d+)(\.\d{1,6})?$"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="txtLongitude" ErrorMessage="（最多含有六位小数的数字）"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                纬度坐标值(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtLatitude" runat="server" CssClass="norText"></asp:TextBox>度（地心系BLH，北纬正值南纬负值）
                <asp:RequiredFieldValidator ID="rfv7" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtLatitude" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rv2" runat="server" Display="Dynamic" MinimumValue="-90"
                    MaximumValue="90" ControlToValidate="txtLatitude" Type="Double" ForeColor="Red"
                    ErrorMessage="（-90至90）"></asp:RangeValidator>
                <asp:RegularExpressionValidator ID="rev2" runat="server" ValidationExpression="^(-?\d+)(\.\d{1,6})?$"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="txtLatitude" ErrorMessage="（最多含有六位小数的数字）"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                高程坐标值(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtGaoCheng" runat="server" CssClass="norText" MaxLength="12"></asp:TextBox>米（地心系BLH）
                <asp:RequiredFieldValidator ID="rfv8" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtGaoCheng" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="rev3" runat="server" ValidationExpression="^\d+(\.\d{1,1})?$"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="txtGaoCheng" ErrorMessage="（>=0且最多含有一位小数的数字）"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                信源信宿(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplXyxs" runat="server" CssClass="norDpl">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfv9" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="dplXyxs" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                扩展属性
            </th>
            <td>
                <asp:Repeater ID="rpZYSXList" runat="server" ViewStateMode="Enabled">
                    <HeaderTemplate>
                        <table class="list">
                            <tr>
                                <th style="width: 15%; text-align: center;">
                                    属性名称
                                </th>
                                <th style="width: 15%; text-align: center;">
                                    属性编码
                                </th>
                                <th style="width: 15%; text-align: center;">
                                    属性类型
                                </th>
                                <th style="width: 15%; text-align: center;">
                                    属性范围
                                </th>
                                <th style="width: 40%; text-align: center;">
                                    属性数值
                                </th>
                            </tr>
                            <tbody id="tbZYSXList">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="text-align: center;">
                                <%# Eval("PName")%>
                            </td>
                            <td style="text-align: center;">
                                <%# Eval("PCode")%>
                            </td>
                            <td style="text-align: center;">
                                <%# SystemParameters.GetSystemParameterText(SystemParametersType.ZYSXType, Eval("Type").ToString())%>
                            </td>
                            <td style="text-align: center;">
                                <%# Eval("Scope")%>
                            </td>
                            <td style="text-align: left;">
                               <asp:PlaceHolder ID="phPValueControls" runat="server"></asp:PlaceHolder>
                               <asp:HiddenField ID="hfPID" runat="server" Value='<%#Eval("ID")%>' />
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
                            <om:CollectionPager ID="cpZYSXPager" runat="server">
                            </om:CollectionPager>
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
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="提 交" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnReset" runat="server" CssClass="button" Text="清 除" OnClick="btnReset_Click"
                    CausesValidation="false" />
                <asp:Button ID="btnReturn" runat="server" CssClass="button" Text="返 回" OnClick="btnReturn_Click"
                    CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Content>

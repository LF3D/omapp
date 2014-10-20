<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SatEdit.aspx.cs"
    Inherits="OperatingManagement.Web.Views.BusinessManage.BDManage.SatEdit" %>
<%@ Import Namespace="OperatingManagement.DataAccessLayer.BusinessManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="bizmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuBusiness" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    基础数据管理 &gt; 编辑卫星
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="edit" style="width: 800px;">
        <tr>
            <th style="width: 200px;">
                卫星名称(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtWXMC" runat="server" Width="300px" CssClass="text" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv1" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtWXMC" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                卫星编码(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtWXBM" runat="server" Width="300px" CssClass="text" MaxLength="25"
                    Enabled="false"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv2" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtWXBM" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                国内六位编码(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtGNLBM" runat="server" Width="300px" CssClass="text" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv10" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtGNLBM" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="rv10" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtGNLBM" ErrorMessage="（请输入6位数字）" ValidationExpression="\d{1,6}"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                卫星标识(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtWXBS" runat="server" Width="300px" CssClass="text" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv3" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtWXBS" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                卫星状态(<span class="red">*</span>)
            </th>
            <td>
                <asp:RadioButtonList ID="rblState" runat="server" BorderColor="White" 
                    BorderStyle="Double" BorderWidth="2px" RepeatDirection="Horizontal" ClientIDMode="Static">
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th>
                面质比(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtMZB" runat="server" Width="300px" CssClass="text" MaxLength="8"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv4" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtMZB" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="rev3" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtMZB" ErrorMessage="（请输入8位内数字，精度最大3位）" ValidationExpression="\d{1,5}(.\d{1,3})?"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                表面反射系数(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtBMFSXS" runat="server" Width="300px" CssClass="text" MaxLength="8"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv5" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtBMFSXS" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtBMFSXS" ErrorMessage="（请输入8位内数字，精度最大3位）" ValidationExpression="\d{1,5}(.\d{1,3})?"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                形状(<span class="red">*</span>)
            </th>
            <td>
                <asp:RadioButtonList ID="rblShape" runat="server" BorderColor="White" 
                    BorderStyle="Double" BorderWidth="2px" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th>
                直径(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtD" runat="server" Width="300px" CssClass="text" MaxLength="8"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtD" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtD" ErrorMessage="（请输入8位内数字，精度最大3位）" ValidationExpression="\d{1,5}(.\d{1,3})?"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                长度(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtL" runat="server" Width="300px" CssClass="text" MaxLength="8" Text="0"></asp:TextBox>
                <span style="color:#3399FF;">球体时长度为0</span>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ForeColor="Red"
                    ControlToValidate="txtL" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtL" ErrorMessage="（请输入8位内数字，精度最大3位）" ValidationExpression="\d{1,5}(.\d{1,3})?"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                表面情况(<span class="red">*</span>)
            </th>
            <td>
                <asp:RadioButtonList ID="rblRG" runat="server" BorderColor="White" 
                    BorderStyle="Double" BorderWidth="2px" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
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
        <tr>
            <th>
                卫星功能
            </th>
            <td>
                <asp:HiddenField ID="hfWXGNs" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfSatID" runat="server"  ClientIDMode="Static" />
                <asp:Repeater ID="rpWXGNs" runat="server">
                    <HeaderTemplate>
                        <table class="list" id="tbWXGNs">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <ul class="taskList">
                                    <li>
                                        <input type="checkbox" id="chkGN<%# Eval("Id") %>" value="<%# Eval("Id") %>" />
                                        <span class="spanFNames" style="width:auto;"><%# Eval("FName") %></span>
                                    </li>
                                </ul>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>                    
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
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
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="提 交" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnReset" runat="server" CssClass="button" Text="重 置" OnClick="btnReset_Click"
                    CausesValidation="false" />
                <asp:Button ID="btnReturn" runat="server" CssClass="button" Text="返 回" OnClick="btnReturn_Click"
                    CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Content>

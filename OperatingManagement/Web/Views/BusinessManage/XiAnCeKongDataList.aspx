<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="XiAnCeKongDataList.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.XiAnCeKongDataList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="bizmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuBusiness" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
业务管理 &gt; 查看西安卫星测控中心数据
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:Panel ID="pnlData" runat="server">
        <table cellpadding="0"  class="edit" >
            <tr>
                <th align="right" class="style2">
                    开始日期：
                </th>
                <td class="style3">
                    <asp:TextBox ID="txtStartDate" runat="server"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                </td>
                <th align="right" class="style4">
                    结束日期：
                </th>
                <td>
                    <asp:TextBox ID="txtEndDate" runat="server"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right" class="style2">
                    数据类型：
                </th>
                <td class="style3">
                    <asp:DropDownList ID="ddlType" runat="server" Height="24px" Width="156px">
                        <asp:ListItem Value="tb_gdxa">飞行器轨道根数</asp:ListItem>
                        <asp:ListItem Value="tb_gdsh">事后精轨根数</asp:ListItem>
                        <asp:ListItem Value="tb_xdsc">星地时差</asp:ListItem>
                        <asp:ListItem Value="tb_T0">起飞0点T0</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="right" class="style4">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style5">
                </td>
                <td class="style6" colspan="3">
                    <asp:Button ID="btnSearch" CssClass="button" runat="server" OnClick="btnSearch_Click"
                        Text="查询" Width="69px" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnReset" CssClass="button" runat="server" Text="重置" Width="65px" />
                </td>
            </tr>
            <tr>
                <td class="style2" colspan="4">
                    &nbsp; &nbsp; &nbsp; &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2" colspan="4">
                    <asp:Repeater ID="rpDatas" runat="server">
                        <HeaderTemplate>
                            <table class="list">
                                <tr>
                                    <%--<th style="width:20px;"><input type="checkbox" onclick="checkAll(this)" /></th>--%>
                                    <th style="width: 150px;">
                                        信源
                                    </th>
                                    <th style="width: 150px;">
                                        信宿
                                    </th>
                                    <th style="width: 150px;">
                                        任务代码
                                    </th>
                                    <th style="width: 150px;">
                                        生成时间
                                    </th>
                                    <th style="width: 70px;">
                                        明细
                                    </th>
                                </tr>
                                <tbody id="tbUsers">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <%--<td><input type="checkbox" <%# Eval("LoginName").ToString().Equals(this.Profile.UserName,StringComparison.InvariantCultureIgnoreCase)?"disabled=\"true\"":"" %> name="chkDelete" value="<%# Eval("Id") %>" /></td>--%>
                                <td>
                                    <%# Eval("source")%>
                                </td>
                                <td>
                                    <%# Eval("destination")%>
                                </td>
                                <td>
                                    <%# Eval("taskid")%>
                                </td>
                                <td>
                                    <%# Eval("ctime", "{0:" + this.SiteSetting.DateTimeFormat + "}")%>
                                </td>
                                <td>
                                    <button class="button" onclick="return showDetail('<%# Eval("ID") %>')">
                                        明细</button>
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
                                <om:CollectionPager ID="cpPager" runat="server">
                                </om:CollectionPager>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

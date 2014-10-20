<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenaratedPlanList.aspx.cs" Inherits="OperatingManagement.Web.Views.PlanManage.GenaratedPlanList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="planmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuPlan" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    计划管理 &gt; 已生成计划列表
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
                    <asp:Repeater ID="rpDatas" runat="server">
                        <HeaderTemplate>
                            <table class="list">
                                <tr>
                                    <%--<th style="width:20px;"><input type="checkbox" onclick="checkAll(this)" /></th>--%>
                                    <th style="width: 150px;">
                                        计划类型
                                    </th>
                                    <th style="width: 150px;">
                                        任务代号
                                    </th>
                                    <th style="width: 150px;">
                                        开始时间
                                    </th>
                                    <th style="width: 150px;">
                                        结束时间
                                    </th>
                                    <th style="width: 70px;">
                                        编辑
                                    </th>
                                </tr>
                                <tbody id="tbUsers">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <%--<td><input type="checkbox" <%# Eval("LoginName").ToString().Equals(this.Profile.UserName,StringComparison.InvariantCultureIgnoreCase)?"disabled=\"true\"":"" %> name="chkDelete" value="<%# Eval("Id") %>" /></td>--%>
                                <td>
                                    <%# Eval("plantype")%>
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
                                <td>
                                    <button class="button" onclick="return showEdit('<%# Eval("PLANID") %>','<%# Eval("PLANTYPE") %>')">
                                        编辑</button>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    </asp:Content>

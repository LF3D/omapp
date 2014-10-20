<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ExperimentPlanDetail.aspx.cs"
    Inherits="OperatingManagement.Web.Views.PlanManage.ExperimentPlanDetail" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="planmanage" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuPlan" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MapPathContent" runat="server">
    计划管理 &gt; 查看试验计划明细
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="BodyContent" runat="server">
    <table cellpadding="0" class="style1 edit1"  style="width: 700px;">
    <tr>
            <th style="width: 120px;">
                编号
            </th>
            <td>
                <asp:TextBox ID="txtJHID" runat="server" CssClass="text" MaxLength="10"
                    Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th style="width: 120px;">
                时间
            </th>
            <td>
                <asp:TextBox ID="txtSYTime" runat="server" CssClass="text" MaxLength="10"
                    Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th style="width: 120px;">
                试验个数
            </th>
            <td>
                <asp:TextBox ID="txtSYCount" runat="server" CssClass="text" MaxLength="10" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr style = "display:none;">
            <th style="width: 120px;">
                &nbsp;
            </th>
            <td>
                     <asp:HiddenField ID="HfID" runat="server" />
                     <asp:HiddenField ID="HfFileIndex" runat="server" />
            </td>
        </tr>
    </table>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <table class="edit1" style="width: 700px;">
                <tr>
                    <th style="width: 120px;" rowspan="7">
                        试验
                    </th>
                    <th style="width: 100px;">
                        卫星名称
                    </th>
                    <td>
                        <asp:TextBox Width="200px" ID="txtSYSatName" CssClass="text" runat="server" Text='<%# Eval("SYSatName")%>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th style="width: 100px;">
                        试验类别
                    </th>
                    <td>
                        <asp:TextBox Width="200px" ID="txtSYType" CssClass="text" runat="server" Text='<%# Eval("SYType")%>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th style="width: 100px;">
                        试验项目
                    </th>
                    <td>
                        <asp:TextBox Width="200px" ID="txtSYItem" CssClass="text" runat="server" Text='<%# Eval("SYItem")%>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th style="width: 100px;">
                        开始时间
                    </th>
                    <td>
                        <asp:TextBox Width="200px" ID="txtStartTime" CssClass="text" runat="server" Text='<%# Eval("SYStartTime")%>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th style="width: 100px;">
                        结束时间
                    </th>
                    <td>
                        <asp:TextBox Width="200px" ID="txtEndTime" CssClass="text" runat="server" Text='<%# Eval("SYEndTime")%>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th style="width: 100px;">
                        系统名称
                    </th>
                    <td>
                        <asp:TextBox Width="200px" ID="txtSYSysName" CssClass="text" runat="server" Text='<%# Eval("SYSysName")%>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th style="width: 100px;">
                        系统任务
                    </th>
                    <td>
                        <asp:TextBox Width="300px" ID="txtSYSysTask" CssClass="text" runat="server" Text='<%# Eval("SYSysTask")%>' 
                            Height="47px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Panel ID ="pnlAll2" runat="server">
        <table class="edit1"  style="width: 700px;">
            <tr>
                <th style="width: 120px;">
                </th>
                <td align="left">
                    <asp:Button class="button" ID="btnCreateFile" runat="server" Text="生成文件" 
                        Width="65px" onclick="btnCreateFile_Click" />
                </td>
            </tr>
            <tr runat="server" id="trMsg" visible="false">
                <th></th>
                <td>
                    <asp:Label ID="lblMessage" runat="server" CssClass="error" Text=""></asp:Label>
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
    </asp:Panel>
</asp:Content>
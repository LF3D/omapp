<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResourceAdd.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.ResourceAdd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
<om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="resmanage" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
<om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuRes" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
业务管理 &gt; 新增资源
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
<table class="edit" style="width: 800px;">
        <tr>
            <th style="width: 150px;">
                资源类型(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplResourceType" runat="server" CssClass="norDpl">
                </asp:DropDownList>
            </td>
        </tr>
        <div id="divGroundResource" runat="server">
            <tr>
            <th>
                地面站名称(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtGRName" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtGRName" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
         <tr>
            <th>
                地面站编号(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtGRCode" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtGRCode" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                设备名称(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtEquipmentName" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtEquipmentName" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                设备编号(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtEquipmentCode" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtEquipmentCode" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
         <tr>
            <th>
                管理单位(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplOwner" runat="server" CssClass="norDpl">
                </asp:DropDownList>
            </td>
        </tr>
          <tr>
            <th>
                站址坐标(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplCoordinate" runat="server" CssClass="norDpl">
                </asp:DropDownList>
            </td>
        </tr>
          <tr>
            <th>
                功能类型(<span class="red">*</span>)
            </th>
            <td>
               <asp:CheckBoxList ID="cblFunctionType" runat="server">
               </asp:CheckBoxList>
            </td> 
        </tr>
        </div>
        <div id="divCommunicationResource" runat="server">
        <tr>
            <th>
                线路名称(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtRouteName" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtRouteName" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                方向(<span class="red">*</span>)
            </th>
            <td>
                <asp:DropDownList ID="dplDirection" runat="server" CssClass="norDpl">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                带宽(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="txtBandWidth" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtBandWidth" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        </div>
        <div id="divCenterResource" runat="server">
        <tr>
            <th>
                设备编号(<span class="red">*</span>)
            </th>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" CssClass="norText"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic"
                    ForeColor="Red" ControlToValidate="txtBandWidth" ErrorMessage="（必填）"></asp:RequiredFieldValidator>
            </td>
        </tr>
        </div>
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
                <asp:Button ID="btnReturn" runat="server" CssClass="button" Text="返 回" OnClick="btnReturn_Click"
                    CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Content>

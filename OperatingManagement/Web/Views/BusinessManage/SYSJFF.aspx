<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SYSJFF.aspx.cs" Inherits="OperatingManagement.Web.Views.BusinessManage.SYSJFF" %>
<%@ Register src="../../ucs/ucTask.ascx" tagname="ucTask" tagprefix="uc1" %>
<%@ Register src="../../ucs/ucSatellite.ascx" tagname="ucSatellite" tagprefix="uc2" %>
<%@ Register src="../../ucs/ucOutTask.ascx" tagname="ucOutTask" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigatorContent" runat="server">
    <om:PageNavigator ID="navMain" runat="server" CssName="menu-top" SelectedId="ykinfoman" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
    <om:PageMenu ID="PageMenu1" runat="Server" XmlFileName="menuYKInfo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MapPathContent" runat="server">
    业务管理 &gt; 试验数据分发
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="listTitle">
        <tr>
            <th style="text-align:left;"><span style="padding-left:5px">任务</span></th>
            <th style="text-align:left;"><span style="padding-left:5px">数据类型</span></th>
            <th style="text-align:left;"><span style="padding-left:5px">开始日期</span></th>
            <th style="text-align:left;"><span style="padding-left:5px">结束日期</span></th>
            <th></th>
        </tr>
        <tr>
            <td id="tdTask">
                <uc3:ucOutTask ID="ucOutTask1" runat="server" AllowBlankItem="False" /></td>
            <td id="tdData"><asp:DropDownList ID="ddlDataType" runat="server" ClientIDMode="Static">
                <asp:ListItem Value="0">天基目标观测试验数据</asp:ListItem>
                <asp:ListItem Value="1">空间机动试验数据</asp:ListItem>
                <asp:ListItem Value="2">仿真推演试验数据</asp:ListItem>
                </asp:DropDownList></td>
            <td><asp:TextBox ID="txtFrom" ClientIDMode="Static" CssClass="text" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" 
                runat="server"></asp:TextBox></td>
            <td><asp:TextBox ID="txtTo" ClientIDMode="Static" CssClass="text" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                runat="server"></asp:TextBox></td>
            <td><asp:Button CssClass="button" ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="查询" />
                &nbsp;&nbsp;<button class="button" onclick="return clearField();">清空</button>
                &nbsp;&nbsp;<asp:Button CssClass="button" 
                    ID="btnSend" runat="server" Text="发送数据" OnClientClick="return SendFile();" 
                    onclick="btnSend_Click"  />
                &nbsp;&nbsp;<asp:Button CssClass="button" ID="btnCreate" runat="server" 
                    Text="生成数据" OnClientClick="return createFile();" onclick="btnCreate_Click"  /></td>
        </tr>
    </table>
    <div runat="server" id="vYCData">
    <asp:Repeater ID="rpYCData" runat="server">
        <HeaderTemplate>
            <table class="list">
                <tr>
                    <th style="width:3%;"></th>
                    <th style="width:20%;">创建时间</th>
                    <th style="width:12%;">任务代号</th>
                    <th style="width:10%;">卫星编号</th>
                    <th style="width:10%;">数据类型</th>
                    <th style="width:20%;">开始星上时间</th>
                    <th style="width:20%;">结束星上时间</th>
                    <th style="width:5%;">备注</th>
                </tr>  
                <tbody id="tbYCData">        
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><input type="checkbox" name="chkSelect" value="<%# Eval("Id") + ";" + Eval("Stype") %>" /></td>
                <td><%# Eval("CTime", "{0:" + this.SiteSetting.DateTimeFormat + "}") %></td>
                <td><%# Eval("TaskID") %></td>
                <td><%# Eval("SatID") %></td>
                <td><%# Eval("Stype") %></td>
                <td><%# Eval("STTIMESTART", "{0:" + this.SiteSetting.DateTimeFormat + "}")%></td>
                <td><%# Eval("STTIMEEND", "{0:" + this.SiteSetting.DateTimeFormat + "}")%></td>
                <td><%# Eval("Reserve") %></td>
            </tr>            
        </ItemTemplate>
        <FooterTemplate>   
                </tbody>           
            </table>            
        </FooterTemplate>
    </asp:Repeater>
    <table>
        <tr>
            <td class="listTitle-c2">
                <om:CollectionPager ID="cpYCData" runat="server" ></om:CollectionPager>
            </td>
        </tr>
    </table>
    </div>
    <div runat="server" id="vUFData">
    <asp:Repeater ID="rpUFData" runat="server">
        <HeaderTemplate>
            <table class="list">
                <tr>
                    <th style="width:3%;"></th>
                    <th style="width:7%;">创建时间</th>
                    <th style="width:5%;">任务代号</th>
                    <th style="width:5%;">卫星编号</th>
                    <th style="width:10%;">数据接收开始时间</th>
                    <th style="width:10%;">数据接收结束时间</th>
                    <th style="width:5%;">用户帧代号</th>
                    <th style="width:3%;">延时标志</th>
                    <th style="width:7%;">数据开始时间</th>
                    <th style="width:7%;">数据结束时间</th>
                    <th style="width:10%;">目录</th>
                    <th style="width:10%;">数据文件名</th>
                    <th style="width:4%;">备注</th>
                </tr>  
                <tbody id="tbUFData">        
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><input type="checkbox" name="chkDelete" value="<%# Eval("Id") + ";" + Eval("UserID") %>" /></td>
                <td><%# Eval("CTime", "{0:" + this.SiteSetting.DateTimeFormat + "}") %></td>
                <td><%# Eval("TaskID") %></td>
                <td><%# Eval("SatID") %></td>
                <td><%# Eval("RECEIVEB", "{0:" + this.SiteSetting.DateTimeFormat + "}")%></td>
                <td><%# Eval("RECEIVEE", "{0:" + this.SiteSetting.DateTimeFormat + "}")%></td>
                <td><%# Eval("UserID") %></td>
                <td><%# Eval("DelaySI") %></td>
                <td><%# Eval("DATATIMEB", "{0:" + this.SiteSetting.DateTimeFormat + "}")%></td>
                <td><%# Eval("DATATIMEE", "{0:" + this.SiteSetting.DateTimeFormat + "}")%></td>
                <td><%# Eval("DIRECTORY")%></td>
                <td><%# Eval("FILENAME")%></td>
                <td><%# Eval("Reserve") %></td>
            </tr>            
        </ItemTemplate>
        <FooterTemplate>   
                </tbody>           
            </table>            
        </FooterTemplate>
    </asp:Repeater>
    <table>
        <tr>
            <td class="listTitle-c2">
                <om:CollectionPager ID="cpUFData" runat="server" ></om:CollectionPager>
            </td>
        </tr>
    </table>
    </div>
    <div runat="server" id="vFZData">
    <asp:Repeater ID="rpFZData" runat="server">
        <HeaderTemplate>
            <table class="list">
                <tr>
                    <th style="width:5px;"></th>
                    <th style="width:15%;">创建时间</th>
                    <th style="width:10%;">任务代号</th>
                    <th style="width:15%;">开始时间</th>
                    <th style="width:15%;">结束时间</th>
                    <th style="width:30%;">文件路径</th>
                    <th style="width:10%;">备注</th>
                </tr>  
                <tbody id="tbFZData">        
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><input type="checkbox" name="chkDelete" value="<%# Eval("Id") %>" /></td>
                <td><%# Eval("CTime","{0:"+this.SiteSetting.DateTimeFormat+"}") %></td>
                <td><%# Eval("TaskID") %></td>
                <td><%# Eval("STARTTIME", "{0:" + this.SiteSetting.DateTimeFormat + "}")%></td>
                <td><%# Eval("EndTIME", "{0:" + this.SiteSetting.DateTimeFormat + "}")%></td>
                <td><%# Eval("FileIndex")%></td>
                <td><%# Eval("Reserve")%></td>
            </tr>            
        </ItemTemplate>
        <FooterTemplate>   
                </tbody>           
            </table>            
        </FooterTemplate>
    </asp:Repeater>
    <table>
        <tr>
            <td class="listTitle-c2">
                <om:CollectionPager ID="cpFZData" runat="server" ></om:CollectionPager>
            </td>
        </tr>
    </table>
    </div>
    <div runat="server" id="vGDData">
        <asp:Repeater ID="rpGDData" runat="server">
            <HeaderTemplate>
                    <table class="list">
                        <tr>
                            <th style="width: 20px;">
                            </th>
                            <th style="width: 200px;">
                                信息标识
                            </th>
                            <th style="width: 100px;">
                                信息代号
                            </th>
                            <th style="width: 100px;">
                                卫星编号
                            </th>
                            <th>
                                创建时间
                            </th>
                            <th style="width: 100px;">
                                明细
                            </th>
                        </tr>
                        <tbody id="tbGDs">
                </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <input type="checkbox" name="chkDelete" value="<%# Eval("Id") %>" />
                    </td>
                    <td>
                        <%# Eval("DataName")%>
                    </td>
                    <td>
                        <%# Eval("ICODE")%>
                    </td>
                    <td>
                        <%# Eval("SatellteName")%>
                    </td>
                    <td>
                        <%# Eval("CTIME","{0:"+this.SiteSetting.DateTimeFormat+"}") %>
                    </td>
                    <td>
                        <button class="button" onclick="return showDetail('<%# Eval("Id") %>')">
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
                <td class="listTitle-c2">
                    <om:CollectionPager ID="cpGDData" runat="server"></om:CollectionPager>
                </td>
            </tr>
        </table>
    </div>
    <table class="listTitle">
        <tr id="trMessage" runat="server" visible="false">
            <td><asp:Label ID="lblMessage" runat="server" CssClass="error" Text=""></asp:Label></td>
        </tr>
        <tr><td>
                <div style="display:none;">
                <asp:HiddenField ID="hfycids" runat="server" ClientIDMode="Static"/>
                <asp:HiddenField ID="hfufids" runat="server" ClientIDMode="Static"/>
                <asp:HiddenField ID="hffzids" runat="server" ClientIDMode="Static"/>
                <asp:HiddenField ID="hfgdids" runat="server" ClientIDMode="Static"/>
                <asp:HiddenField ID="hfsendway" runat="server" ClientIDMode="Static"/>
                    <asp:Button ID="btnHidden" runat="server" ClientIDMode="Static" Text="btnHidden" 
                                OnClick="btnSend_Click" /></div></td></tr>
    </table>
    <div id="divFilePath"  runat="server"  visible="false">
    </div>
    <div id="SendPanel" style="display:none">
        <table id="rblProtocol">
            <tr>
                <td align="left"  style="text-align: left">
                <b>请选择要使用的发送协议：</b>
                <br />
                <input type="radio" name="rdProtocl" value="2" checked="checked" />Fep with Tcp
                <input type="radio" name="rdProtocl" value="1" />Fep with Udp
                <input type="radio" name="rdProtocl" value="0" />Ftp
                    <br />
                </td>
            </tr>
        </table>
    </div>
    <div id="dialog-form" style="display:none" title="提示信息">
	    <p class="content"></p>
    </div>
</asp:Content>

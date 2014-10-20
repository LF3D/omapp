<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PassportSSO.aspx.cs" Inherits="OperatingManagement.Web.Account.PassportSSO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="box login">
        <div class="b-r1c1">
            <div class="b-r1c2">
                <div class="b-r1c3">
                    &nbsp;</div>
            </div>
        </div>
        <div class="b-r2c1">
            <div class="b-r2c2">
                <div class="b-r2c3">
                    <div class="l-c-r1">
                        &nbsp;
                    </div>
                    <hr />
                    <div class="l-c-r2">
                        <div>
                            <b>用户名 </b>
                        </div>
                        <div>
                            <asp:TextBox ID="txtLoginName" runat="server" Width="300px" CssClass="text" ></asp:TextBox>
                        </div>
                        <div>
                            <b>密码 </b>
                        </div>
                        <div>
                            <asp:TextBox ID="txtPassword" runat="server" Width="300px" CssClass="text" TextMode="Password" ></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label CssClass="error" ID="lblMessage" runat="server" Visible="false"></asp:Label>
                        </div>
                        <div>
                            <asp:Button ID="btnLogin" runat="server" Text="登录" CssClass="button" 
                                onclick="btnLogin_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="b-r3c1">
            <div class="b-r3c2">
                <div class="b-r3c3 copyright">
                    CopyRight &copy; <%= this.CopyRight %> 2011</div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

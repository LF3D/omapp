<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Exp.aspx.cs" Inherits="OperatingManagement.Web.Views.Exp.Exp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <div class="box error-box">
           <div class="b-r1c1">
            <div class="b-r1c2">
                <div class="b-r1c3">&nbsp;</div>
            </div>
           </div>
           
           <div class="b-r2c1">
            <div class="b-r2c2">
                <div class="b-r2c3">
                     <div class="eb-c">
                        <div class="eb-c-r1"><b>系统异常信息</b></div>
                        <div class="eb-c-r1"><a onclick="window.history.back(-1)';">返回</a></div>
                        <hr />
                        <div class="eb-c-r2"><asp:Literal ID="ltError" runat="server" ></asp:Literal></div>
                     </div>
                </div>
            </div>
           </div>
           <div class="b-r3c1">
            <div class="b-r3c2">
                <div class="b-r3c3">&nbsp;</div>
            </div>
           </div>
        </div>
    </form>
</body>
</html>

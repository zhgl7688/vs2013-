<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebForm.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>登录</title>
</head>
<body style="background:#f3f7f9; overflow:hidden;">
    <div id="bgdiv" class="loginbgdiv">  
        <%--<div style="margin-top:50px;margin-left:200px;color:rgb(8,2,252);font-size:50px; font-family:黑色;font-weight:bold;">蜂窝软件有限公司 </div>--%>

    </div>
  
    <script type="text/javascript">
        var win = new RoadUI.Window();
        $(function ()
        {
            var left = $(window).width() - 600;
            var top = $(window).height() - 480;
            win.open({ url: "Login1.aspx", width: 350, height: 220, top: top, left: left, showico: false, title: "用户登录", resize: false, ismodal: false, showclose: false });
        });
    </script>
</body>
</html>

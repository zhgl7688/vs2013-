<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login1.aspx.cs" Inherits="WebForm.Login1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" />
</head>
<body >
    <form id="form1" runat="server">
    <br />
    <input type="hidden" id="Force" name="Force" value="0" />
    <table cellpadding="0" cellspacing="1" border="0" style="width:80%; margin:0 auto;" >
        <tr>
            <td style="width:70px; height:45px; text-align:right;">帐号：</td>
            <td><input type="text" class="form-control" id="Account" name="Account" value="" maxlength="50" style="width:170px;" /></td>
        </tr>
        <tr>
            <td style="height:45px; text-align:right;">密码：</td>
            <td><input type="password" class="form-control" id="Password" name="Password" maxlength="50" style="width:170px;" /></td>
        </tr>
        <tr id="novcode" style="display:none;">
            <td style="height:45px; text-align:right;">验证码：</td>
            <td><input type="text" class="form-control" id="VCode" name="VCode" maxlength="4" style="width:80px;float:left"  />
                <img alt="" src="VCode.ashx?<%=DateTime.Now.Ticks %>" onclick="cngimg();" style="float:left" id="VcodeImg" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <input type="submit" value="&nbsp;&nbsp;登&nbsp;&nbsp;录&nbsp;&nbsp;" id="loginbutton" name="loginbutton" onclick="return checkForm();" class="btn btn-info" />
            </td>
        </tr>
        <%if("1"!=Request.QueryString["session"]){%>
        <tr>
           
            <td colspan="2"  style="text-align:center; vertical-align:bottom; height:40px;"><a href="http://www.fowosoft.com/" target="_blank" ><span style="color:blue;">官网:fowosoft.com</span></a></td>
        </tr>
        <%} %>
    </table>
    </form>
    <script type="text/javascript">
        var isVcode = '1' == '<%=Session[FoWoSoft.Utility.Keys.SessionKeys.IsValidateCode.ToString()]%>';
        var isSessionLost = "1" == '<%=Request.QueryString["session"]%>';
        $(window).load(function ()
        {
            if (isVcode)
            {
                if (!isSessionLost)
                {
                    top.win.resize(350, 260);
                }
                $("#novcode").show();
            }
            <%=Script%>
        });
        function checkForm()
        {
            var form1 = document.forms[0];
            if ($.trim(form1.Account.value).length == 0)
            {
                alert("帐号不能为空!");
                form1.Account.focus();
                return false;
            }
            if ($.trim(form1.Password.value).length == 0)
            {
                alert("密码不能为空!");
                form1.Password.focus();
                return false;
            }
            if (isVcode && form1.VCode && $.trim(form1.VCode.value).length == 0)
            {
                alert("验证码不能为空!");
                form1.VCode.focus();
                return false;
            }
            return true;
        }
        function cngimg()
        {
            $('#VcodeImg').attr('src', 'VCode.ashx?' + new Date().toString());
        }
    </script>
</body>
</html>

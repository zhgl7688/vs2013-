<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="WebForm.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="Scripts/jquery-1.9.1.js"></script>
</head>
<body>
    <form id="form1" >
    <div>
        <input id="xxx" disabled="1" value="xxx1" />
    </div>
    </form>
</body>
</html>
<script>
    alert($('#xxx').attr("disabled")=='disabled');
    alert($('#xxx').disabled==false);
</script>
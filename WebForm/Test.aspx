﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="WebForm.Test" %>

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
        <a href="http://202.120.85.70/test1.ashx?all=50" target="_blank" >a1</a>
        <a href="http://202.120.85.70/test1.ashx?all=100"  target="_blank">a1</a>
        <a href="http://202.120.85.70/test1.ashx?all=150"  target="_blank">a1</a>
        <a href="http://202.120.85.70/test1.ashx?all=200"  target="_blank">a1</a>
        <a href="http://202.120.85.70/test1.ashx?all=250"  target="_blank">a1</a>
        <a href="http://202.120.85.70/test1.ashx?all=300"  target="_blank">a1</a>
        <a href="http://202.120.85.70/test1.ashx?all=350"  target="_blank">a1</a>
        <a href="http://202.120.85.70/test1.ashx?all=400"  target="_blank">a1</a>
        <a href="http://202.120.85.70/test1.ashx?all=450"  target="_blank">a1</a>
        <a href="http://202.120.85.70/test1.ashx?all=500"  target="_blank">a1</a>
        <a href="http://202.120.85.70/test1.ashx?all=550"  target="_blank">a1</a>

    </form>
</body>
</html>
<script>
    $.get("", function (data) {

    })
</script>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="houser.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="user-scalable=0, initial-scale=1.0 minimal-ui"/>
    <link href="Styles/reset.css" rel="stylesheet" type="text/css" />
    <link href="Styles/alt.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
        <div class="loginControls">
            <h3 class="menuLable">User Name</h3>
            <asp:TextBox ID="txtUserName" CssClass="menuTextBox" runat="server" PlaceHolderText="your username" />
            <h3 class="menuLable">Password</h3>
            <asp:TextBox ID="txtPassword" CssClass="menuTextBox" runat="server" TextMode="Password" pattern="[0-9]*" inputmode="numeric" />
            <asp:Button ID="btnSubmitLogin" CssClass="button" runat="server" Text="Login" onclick="btnSubmitLogin_Click" />
        </div>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="houser.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="loginControls">
            <asp:Button ID="btnSubmitLogin" CssClass="button" runat="server" Text="Login" onclick="btnSubmitLogin_Click" />
            <span class="menuLable">User Name
                <asp:TextBox ID="txtUserName" CssClass="menuTextBox" runat="server" PlaceHolderText="your username" />
            </span>
            <span class="menuLable">Password
                <asp:TextBox ID="txtPassword" CssClass="menuTextBox" runat="server" TextMode="Password" />
            </span>
        </div>
    </div>
    </form>
</body>
</html>

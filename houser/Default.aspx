<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="houser._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Houser App</title>
    <link href="Styles/style.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
</head>
<body>
    <form id="form1" runat="server" visible="true">
    <div class="pageWrapper">
    <div class="displayPanel">
    <div class="displayPanelPlaceholder"></div>
    </div>
    <div class="displayPanelSpacer"></div>
    <div class="menu">
        <asp:DropDownList ID="ddlSaleDate" class="dropDown" runat="server" 
            onselectedindexchanged="ddlSaleDate_SelectedIndexChanged" />
        <asp:Button ID="btnPopulateData" class="button" Text="Get Data" runat="server" Visible="true" 
            onclick="btnPopulateData_Click"/>
        <asp:CheckBox ID="chkNonLive" class="check" Text="  Get fresh data." runat="server" />
    </div>
    <div class="menuSpacer"></div>
    <div class="listings">
        <div class="listPlaceHolder"></div>
        <asp:Panel ID="pnlListingPanel" runat="server">
        </asp:Panel>
    </div>
    <div class="listingsSpacer"></div>
    <div id="displayData">
        <asp:Panel ID="displayPanel" runat="server">
        </asp:Panel>
    </div>
    <div id="map"></div>
    </form>
    <script>
        $(document).ready(function () {
            var self = $(".subjectProperty").parent();
            self.children().toggleClass("collapsed");
            self.children('.subjectProperty').toggleClass("collapsed");
            $(".subjectProperty").click(function () {
                var self = $(this).parent();
                self.children().toggleClass("collapsed");
                self.children('.subjectProperty').toggleClass("collapsed");
            });
        });
    </script>
    
  </body>
</html>

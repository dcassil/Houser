<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="houser._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Houser App</title>
    <link href="Styles/style.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script src="Scripts/jquery.simplemodal-1.4.2.js" type="text/javascript"></script>
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
            onselectedindexchanged="ddlSaleDate_SelectedIndexChanged" ToolTip="Select Sale Date" />
        <asp:Button ID="btnPopulateData" class="button" Text="Get Data" runat="server" Visible="true" 
            onclick="btnPopulateData_Click" ToolTip="Click to get properties for this sale date"/>
        <asp:CheckBox ID="chkNonLive" class="check" Text="Local Data Only" runat="server" ToolTip="Check this to show only local data" />
        <asp:CheckBox ID="chkTestMode" class="check" Text="  Test Mode" runat="server" ToolTip="Fetch only a few test listings" />
    </div>
    <div class="menuSpacer"></div>
    <div class="header">
    <div class="listingWrapper">
    <div class="indicatorSpacer"></div>
    <div class="listingPnlClass">
    <span class="propertyData">
    <span class="notes">Notes</span>
    <span class="address"><asp:Button ID="btnAddress" class="headerSortable" runat="server" Text="Address ▼" onclick="btnSortAddress_Click"/></span>
    <span class="minBidWrapper"><asp:Button ID="btnMinBid" class="headerSortable" runat="server" Text="Min Bid ▼" onclick="btnSortMinBid_Click"/></span>
    <span class="sqft"><asp:Button ID="btnSqft" class="headerSortable" runat="server" Text="SQFT ▼" onclick="btnSortSQFT_Click"/></span>
    <span class="beds"><asp:Button ID="btnBeds" class="headerSortable" runat="server" Text="Beds ▼" onclick="btnSortBeds_Click"/></span>
    <span class="baths"><asp:Button ID="btnBaths" class="headerSortable" runat="server" Text="Baths ▼" onclick="btnSortBaths_Click"/></span>
    </span>
    </div>
    </div>
    </div>
    <div class="headerSpacer"></div>
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

    <script>
        jQuery(function ($) {
            var account_number;
            var isSaved;
            var note;
            // Load dialog on page load
            //$('#basic-modal-content').modal();

            // Load dialog on click
            $('.notes').click(function (e) {
                account_number = $(this).attr('id');
                var notePad = "<div id=\"basic-modal-content\"><div class=\"center_title\">Notes</div><textArea id=\"note_text\" class=\"note_text_box\" name=\"" + account_number + "\" type=\"text\" rows=\"20\" cols=\"70\"></textarea><button class=\"button\" id=\"save_note\">save</button></div>"
                $('#basic-modal-content').remove();
                $('body').append(notePad);
                $('#basic-modal-content').modal();
                $("#save_note").click(function () {
                    note = $("#note_text").val();
                    $(".simplemodal-close").click();
                });
                $("#modalCloseImg").click(function () {
                    if ($("#note_text").val() != note) {
                        alert("You must save your changes");
                    }
                    return false;

                });
                return false;
            });


        });
    </script>
    
  </body>
</html>

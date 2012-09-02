<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="houser._Default" MaintainScrollPositionOnPostback="true" %>

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
        <span class="menuLable">Sale Date
            <asp:DropDownList ID="ddlSaleDate" class="dropDown" runat="server" ToolTip="Select Sale Date" />
        </span>
        <asp:Button ID="btnPopulateData" class="button" Text="Get Data" runat="server" Visible="true" 
        onclick="btnPopulateData_Click" ToolTip="Click to get properties for this sale date"/>
        <span class="menuLable">Select List
            <asp:DropDownList ID="ddlList" class="dropDown" runat="server"
            ToolTip="Select List" OnSelectedIndexChanged="ddlList_SelectedIndexChanged" AutoPostBack="True" />
        </span>
        <asp:CheckBox ID="chkNonLive" class="menuLable check" Text="Cached only" runat="server" ToolTip="Check this to show only local data" />
        <div class="loginControls">
            <asp:Button ID="btnSubmitLogin" CssClass="button" runat="server" Text="Login" onclick="btnSubmitLogin_Click" />
            <span class="menuLable">User Name
                <asp:TextBox ID="txtUserName" CssClass="menuTextBox" runat="server" PlaceHolderText="your suername" />
            </span>
            <span class="menuLable">Password
                <asp:TextBox ID="txtPassword" CssClass="menuTextBox" runat="server" TextMode="Password" />
            </span>
        </div>
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
    <span class="baths"><asp:Button ID="btnPpSqft" class="headerSortable" runat="server" Text="PpSqft ▼"/></span>
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
            var userID = <%=userID%>
            var account_number;
            var address;
            var isSaved;
            var note;
            var compData;
            var zoomLevel = 15;
            var mapMode = "roadmap";
            // ------- NOTES MODAL ---------
            // Load modal and get note on click
            $('.notes').click(function (e) {
                account_number = $(this).attr('id');
                var notePad = "<div id=\"basic-modal-content\"><div class=\"center_title\">Notes</div><textArea id=\"note_text\" class=\"note_text_box\" name=\"" + account_number + "\" type=\"text\" rows=\"20\" cols=\"70\"></textarea><button class=\"button\" id=\"save_note\">save</button></div>"
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/WebUtilities/NoteWebService.asmx/GetAccountNotes',
                    data: "{accountNumber: '" + account_number + "', userID: '" + userID + "'}",
                    dataType: "json",
                    async: false,
                    success: function (responce) {
                        note = responce;
                    },
                    error: function (error) {
                        note = error;
                    }
                });


                // close modal and save note on click
                $('#basic-modal-content').remove();
                $('body').append(notePad);
                $("#note_text").val(note.d);
                $('#basic-modal-content').modal();
                $("#save_note").click(function () {
                    note = $("#note_text").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '/WebUtilities/NoteWebService.asmx/SaveAccountNote',
                        data: "{accountNumber: '" + account_number + "', userID: '" + userID + "', note: '" + note + "'}",
                        dataType: "json",
                        async: false,
                        success: "",
                        error: ""
                    });
                    if (note == "") {
                        // var id = "#" + $("#note_text")[0].name;
                        // remove note exist class
                        $("#" + account_number).find("#" + account_number).removeClass("hasNote")
                    } else {
                        // add note exist class
                        $("#" + account_number).find("#" + account_number).addClass("hasNote")
                    }

                    // Close modal
                    $(".simplemodal-close").click();
                });
                return false;
            });
            // ----- END NOTES MODAL ------

            // ----- DETAIL PANEL -------
            $(".listingPanel").click(function () {
                $("body *").removeClass("selectedProperty");
                $(this).addClass("selectedProperty");
                $(".displayPanelPlaceholder").hide(40);
                // set account number.
                account_number = $(this).attr("ID");
                // set address.
                //need to url encode address for query string.
                address = encodeURI($(this).children(".propertyData").children(".address").html());

                // Create google map url
                var mapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + address + "%20Oklahoma%20City%20Oklahoma&zoom=" + zoomLevel + "&size=500x323&maptype=" + mapMode + "&markers=color:red%7Clabel:S%" + address + "&sensor=false"

                // remove details item containers
                $(".mapBox").remove();
                $(".compBox").remove();

                // Add the new details item containers
                $(".displayPanel").append('<div class="mapBox"></div>');
                $(".displayPanel").append('<div class="compBox"></div>');

                // Add details items
                // Map Image.
                $(".mapBox").append('<img class="mapImg" src="' + mapUrl + '" />');
                $(".mapBox").append('<div class="mapZoomIn">+</div>');
                $(".mapBox").append('<div class="mapZoomOut">-</div>');
                $(".mapBox").append('<div class="mapMode">mode</div>');

                $(".mapZoomIn").click(function () {
                    zoomLevel += 1;
                    $(".mapImg").remove();
                    mapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + address + "%20Oklahoma%20City%20Oklahoma&zoom=" + zoomLevel + "&size=500x323&maptype=" + mapMode + "&markers=color:red%7Clabel:S%" + address + "&sensor=false"
                    $(".mapBox").append('<img class="mapImg" src="' + mapUrl + '" />');
                });

                $(".mapZoomOut").click(function () {
                    zoomLevel -= 1;
                    $(".mapImg").remove();
                    mapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + address + "%20Oklahoma%20City%20Oklahoma&zoom=" + zoomLevel + "&size=500x323&maptype=" + mapMode + "&markers=color:red%7Clabel:S%" + address + "&sensor=false"
                    $(".mapBox").append('<img class="mapImg" src="' + mapUrl + '" />');
                });

                $(".mapMode").click(function () {
                    if (mapMode == "roadmap") {
                        mapMode = "satellite";
                    } else {
                        mapMode = "roadmap";
                    }
                    $(".mapImg").remove();
                    mapUrl = "http://maps.googleapis.com/maps/api/staticmap?center=" + address + "Oklahoma&zoom=" + zoomLevel + "&size=500x323&maptype=" + mapMode + "&markers=color:red%7Clabel:S%" + address + "&sensor=false"
                    $(".mapBox").append('<img class="mapImg" src="' + mapUrl + '" />');
                });


                // get comp properties.
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/WebUtilities/DetailsWebService.asmx/GetPropertyCompsByAccountNumber',
                    data: "{accountNumber: '" + account_number + "'}",
                    dataType: "json",
                    async: false,
                    success: function (responce) {
                        if (responce.d != "") {
                            compData = eval('(' + responce.d + ');');
                        } else {
                            compData = null;
                        }
                    },
                    error: function (error) {
                        compData = error;
                    }
                });
                // we are now returning a json string of comp properties when a property is clicked.  we will need to
                // assign these values to a list in the details panel.

                if (compData != null) {
                    var compTable = '<table class="compTable">';
                    compTable += '<th>Addrress</th>';
                    compTable += '<th>Sqft</th>';
                    compTable += '<th>Beds</th>';
                    compTable += '<th>Baths</th>';
                    compTable += '<th>LastSaleDate</th>';
                    compTable += '<th>LastSalePrice</th>';
                    compTable += '<th>$/Sqft</th>';
                    for (var i = 0; i < compData.Table.length; i++) {
                        if (i < 10) {
                            compTable += '<tr>';
                            compTable += '<td class="tAddress">' + compData.Table[i].Address + '</td>';
                            compTable += '<td class="tSqft">' + compData.Table[i].Sqft + '</td>';
                            compTable += '<td class="tBeds">' + compData.Table[i].Beds + '</td>';
                            compTable += '<td class="tBaths">' + compData.Table[i].Baths + '</td>';
                            compTable += '<td class="tLastSalePrice">' + compData.Table[i].LastSaleDate + '</td>';
                            compTable += '<td class="tLastSalePrice">$' + Number(compData.Table[i].LastSalePrice).toFixed(2) + '</td>';
                            compTable += '<td class="tPPSqftt">$' + Number(compData.Table[i].PricePerSqft).toFixed(2) + '</td>';
                            compTable += '</tr>';
                        }
                    }
                    compTable += '</table>';
                    $(".compBox").append('<div class="compBoxTitle">Property Comps</div>');
                    $(".compBox").append(compTable);
                }
            });
            //--------end details panel

            //-------- Add property to new list
            $(".addToReview").click(function () {
                account_number = $(this).closest("div").attr("id");
                var list = 1;
                // add remove class to indicate if prop is in the review list.
                if ($(this).hasClass("inReviewList")) {
                    $(this).removeClass("inReviewList");
                    $(this).text("Add to review list");
                    if ($("#ddlList").val() != "2") {
                        $(this).parent().parent().parent().remove();
                    }
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '/WebUtilities/DetailsWebService.asmx/RemovePropertyFromList',
                        data: "{accountNumber: '" + account_number + "', listID: '" + list + "', userID: '" + userID + "'}",
                        dataType: "json",
                        async: false,
                        success: "",
                        error: ""
                    });
                } else {
                    $(this).addClass("inReviewList");
                    $(this).text("Remove from list");
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '/WebUtilities/DetailsWebService.asmx/AddToReviewList',
                        data: "{accountNumber: '" + account_number + "', listID: '" + list + "', userID: '" + userID + "'}",
                        dataType: "json",
                        async: false,
                        success: "",
                        error: ""
                    });
                }


            });


        });
    </script>
    
  </body>
</html>

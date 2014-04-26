<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="alt.aspx.cs" Inherits="houser.print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Houser App</title>
    <link href="Styles/alt.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script src="Scripts/jquery.simplemodal-1.4.2.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
    
    </div>
    <script>
        jQuery(function ($) {
            var userID = <%=userID%>
            var saleDate = "<%=saleDate%>"
            var account_number;
            var address;
            var isSaved;
            var note;
            var propData;
            var zoomLevel = 15;
            var mapMode = "roadmap";
            var imgPath;
            var temp;

            // get properties.
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/WebUtilities/DetailsWebService.asmx/GetPropertiesBySaleDate',
                    data: "{sDate: '" + saleDate + "', list: '" + "1" + "', sUserID: '" + userID + "'}",
                    dataType: "json",
                    async: false,
                    success: function (responce) {
                        if (responce.d != "") {
                            propData = eval('(' + responce.d + ');');
                        } else {
                            propData = null;
                        }
                    },
                    error: function (error) {
                        propData = error;
                    }
                });

                if (propData != null) {
                     for (var i = 0; i < propData.length; i++) {
                        var propPage = '<div class="propPage">';
                        propPage += '<span><p class="address">' + propData[i].Address + '</p></span>'; 
                        var propTable = '<table class="propTable">';
                        propTable += '<tr class="row"><td>SQFT</td><td class="value">' + propData[i].Sqft + '</td></tr>';
                        propTable += '</table>';
                        propPage += propTable + "</div>";
                        $(".wrapper").append(propPage);
                        }
                }

        });
    </script>
    </form>
</body>
</html>

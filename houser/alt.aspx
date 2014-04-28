<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="alt.aspx.cs" Inherits="houser.print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Houser App</title>
    <link href="Styles/alt.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script src="Scripts/jquery.simplemodal-1.4.2.js" type="text/javascript"></script>
    <script src="Scripts/underscore.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
    
    </div>
    <script>
        jQuery(function ($) {
            _.templateSettings = {interpolate : /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
                evaluate    : /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
                escape      : /\{%-([\s\S]+?)%\}/g}; // excape HTML: {%- <script> %} prints &lt;script&gt;

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
//                     for (var i = 0; i < propData.length; i++) {
                    var template = $("#tmpPropertyData").html();
                    $(".wrapper").html(_.template(template,{propData:propData}));
                }

        });
    </script>
    <script type="text/html" id="tmpPropertyData">
    {% 
    _.each(propData, function(prop) {
    var address = prop.Address.split(",");
    %}
    <div class="propPage">
        <span>
            <p class="address"> {{ address[0] }} </p>
            <p class="city"> {{ address[1] }} </p>
        </span>
        <span class="propdata">
            <table class="propTable">
                <tr class="row">
                    <td class="tCell">
                        <span class="title">SQFT</span>
                    </td>
                    <td class="vCell">
                        <span class="value">{{prop.Sqft }}</span>
                    </td>
                </tr>
                <tr class="row">
                    <td class="tCell">
                        <span class="title">Beds</span>
                    </td>
                    <td class="vCell">
                        <span class="value">{{prop.Beds }}</span>
                    </td>
                </tr>
                <tr class="row">
                    <td class="tCell">
                        <span class="title">Baths</span>
                    </td>
                    <td class="vCell">
                        <span class="value">{{prop.Baths }}</span>
                    </td>
                </tr>
                <tr class="row">
                    <td class="tCell">
                        <span class="title">Price</span>
                    </td>
                    <td class="vCell">
                        <span class="value">{{prop.SalePrice }}</span>
                    </td>
                </tr>
            </table>
            
        </span>
        <span class="imgWrapper"><img class="img" src={{prop.ImgPath}} /></span>
        <div class="notes">
            <textarea rows="4" cols="20">{{prop.Note}}</textarea>
        </div>
        
        
    </div>
    {%
    });
    %}
    </script>
    </form>
</body>
</html>

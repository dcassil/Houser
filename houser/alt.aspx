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
    <div class="menu">
    <select class="list datesList">
        <option value="" disabled selected="Select Date">Select Date</option>
    </select>
    <select class="list propList">
        <option value="1" selected >Review List</option>
        <option value="2" >Full List</option>
    </select>
    </div>
    <div class="wrapper">
    
    </div>
    <script>
        var alt = {};
        alt.userID = -1;
        alt.saleDate = -1;
        alt.list = "1";
        alt.propData = {};
        // get properties.
        alt.getProperties = function(saleDate, list) {
            var data;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '/WebUtilities/DetailsWebService.asmx/GetPropertiesBySaleDate',
                data: "{sDate: '" + saleDate + "', list: '" + list + "', sUserID: '" + alt.userID + "'}",
                dataType: "json",
                async: false,
                success: function (responce) {
                    if (responce.d != "") {
                        data = eval('(' + responce.d + ');');
                    } else {
                        data = null;
                    }
                },
                error: function (error) {
                    data = error;
                }
            });
            return data;
        }
        alt.renderProperties = function(properties) {
            if (alt.propData != null) {
                var template = $("#tmpPropertyData").html();
                $(".wrapper").html();
                $(".wrapper").html(_.template(template,{propData:alt.propData}));
            }
        }
        jQuery(function ($) {
            _.templateSettings = {interpolate : /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
                evaluate    : /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
                escape      : /\{%-([\s\S]+?)%\}/g}; // excape HTML: {%- <script> %} prints &lt;script&gt;

            alt.userID = <%=userID%>
            alt.saleDate = "<%=saleDate%>"
            alt.propData = alt.getProperties(alt.saleDate, alt.list);
            alt.renderProperties(alt.propData);

            //UI functions
            $(".propList").on("change", function(){
                alt.list = $(".propList").val();
                alt.propData = alt.getProperties(alt.saleDate, alt.list);
                alt.renderProperties(alt.propData);
            });
            $(".datesList").on("change", function(){
                alt.saleDate = $(".datesList").val();
                alt.propData = alt.getProperties(alt.saleDate, alt.list);
                alt.renderProperties(alt.propData);
            });
        });
    </script>
    <script type="text/html" id="tmpPropertyData">
    {% 
    _.each(alt.propData, function(prop) {
    var address = prop.Address.split(",");
    %}
    <div class="propPage" ID={{prop.AccountNumber}}>
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
        <input type="button" value="Add to review list"/>
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

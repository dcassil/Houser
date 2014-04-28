<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="alt.aspx.cs" Inherits="houser.print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="user-scalable=0, initial-scale=1.0"></meta>
    <title>Houser App</title>
    <link href="Styles/alt.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script src="Scripts/libs/jquery/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="Scripts/libs/underscore/underscore.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="menuPlaceHolder"></div>
    <div class="menu">
    <select class="list datesList">
        <option value="" disabled selected="Select Date">Select Date</option>
    </select>
    <h3>Houser</h3>
    <select class="list propList">
        <option value="1" selected >Review List</option>
        <option value="2" >Full List</option>
    </select>
    </div>
    <div class="wrapper">
    
    </div>
    <script type="text/javascript">
        var xuid = <%=userID%>
        var xdate = "<%=saleDate %>"
    </script>
    <script src="Scripts/alt.js" type="text/javascript"></script>
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
        <input type="button" id="addToList" name="addToList" value="Add to review list"/>
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

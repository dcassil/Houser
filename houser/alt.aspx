<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="alt.aspx.cs" Inherits="houser.print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="viewport" content="user-scalable=0, initial-scale=1.0 minimal-ui"/>
    <title>Houser App</title>
    <link href="Styles/reset.css" rel="stylesheet" type="text/css" />
    <link href="Styles/alt.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script src="Scripts/libs/jquery/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="Scripts/libs/underscore/underscore.js" type="text/javascript"></script>
    <script src="Scripts/libs/panelSnap/jquery.panelSnap.js" type="text/javascript"></script>
</head>
<body onload="setTimeout(function() { window.scrollTo(0, 1) }, 100);" ontouchmove="alt.blockSwipe(event);">
    <form id="form1" runat="server">
    <div class="menuPlaceHolder"></div>
    <div class="menu">
    <select class="list datesList">
        <option value="" disabled selected="Select Date">Select Date</option>
    </select>
    <p class="progress"></p>
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
    <script src="Scripts/touch.js" type="text/javascript"></script>
    <script type="text/html" id="tmpPropertyData">
    {% 
    _.each(alt.propData, function(prop, i) {
    var address = prop.Address.split(",");
    %}
    <div class="propPage section" data={{i+1}} ID={{prop.AccountNumber}}>
        <span>
            <p class="address"><a href="http://maps.apple.com/?q={{address[0]}} {{address[1]}}">{{address[0]}}</a><span class="address city"> {{ address[1] }} </span></p>
            
        </span>
        <div class="column1">
            <dl>
                <dt>Assessed Value</dt>
                <dd>${{prop.SalePrice.toFixed(0)}}</dd>
                <dt>SQFT</dt>
                <dd>{{prop.Sqft}}</dd>
                <dt>Year Built</dt>
                <dd>{{prop.YearBuilt}}</dd>
            </dl>    
        </div>
        <div class="column2">
            <dl>
                <dt>Bedrooms</dt>
                <dd>{{prop.Beds }}</dd>
                <dt>Bathrooms</dt>
                <dd>{{prop.Baths }}</dd>
                <dt>Lot Size</dt>
                <dd>{{prop.LotSize }}</dd>
            </dl>    
        </div>
        <span class="imgWrapper"><img class="img" src={{prop.ImgPath}} /></span>
        {%if (alt.list === "2") { %}
            <input type="button" class="listButton" id="addToList" name="addToList" value="Add to review list"/>
        {% } else { %}
            <input type="button" class="listButton" id="removeFromList" name="removeFromList" value="Remove from review list"/>
        {% } %}
        <div class="notes" id={{prop.AccountNumber}}>
            <textarea rows="4" cols="20">{{prop.Note}}</textarea>
        </div>
        
        
    </div>
    {%
    });
    %}
    <div class="propPage section">
        <div class="emptyPanel">
            <asp:Button ID="btnSubmitLogOut" CssClass="button" runat="server" Text="log out" onclick="btnSubmitLogOut_Click" />
        </div>
    </div>
    </script>
    
    </form>
</body>
</html>

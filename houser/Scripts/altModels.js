/// <reference path="libs/backbone/backbone.js" />

var propertyModel = Backbone.Model.extend({
    initialize: function() {},
    xdata: {
        value: 0,
        sQFT: 0,
        beds: 0,
        baths: 0,
        lot: 0,
        yearBuit: "",
        accountNumber: ""
    }
});
var propertyCollection = Backbone.Collection.extend({
    model: propertyModel,
    type: "POST",
    contentType: "application/json; charset=utf-8",
    url: '/WebUtilities/DetailsWebService.asmx/GetPropertiesBySaleDate',
    data: "{sDate: '" + '5/8/2014 12:00:00 AM' + "', list: '" + '1' + "', sUserID: '" + '1' + "'}",
    dataType: "json",
    async: true
});
var properties = new propertyCollection();
properties.fetch({
    type: "POST",
    async: false,
    contentType: "application/json",
    data: "{sDate: '" + '5/8/2014 12:00:00 AM' + "', list: '" + '1' + "', sUserID: '" + '1' + "'}",
    parse: function (response) {
        return response.d;
    }
    
});
var beds = properties.models.pluck("Beds");
var baths = properties.models[0].pluck("Baths");
console.log(baths);


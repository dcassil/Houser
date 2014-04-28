        var alt = {};
        alt.userID = -1;
        alt.saleDates = {};
        alt.selectedDate = -1;
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
        alt.getSaleDates = function() {
            var dates;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '/WebUtilities/DetailsWebService.asmx/GetSherifSaleDates',
                dataType: "json",
                async: false,
                success: function (responce) {
                    if (responce.d != "") {
                        dates = responce.d;
                    } else {
                        dates = null;
                    }
                },
                error: function (error) {
                    dates = error;
                }
            });
            return dates;
        }
        alt.addToReviewList = function (accountNumber, userID) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '/WebUtilities/DetailsWebService.asmx/AddToReviewList',
                data: "{accountNumber: '" + accountNumber + "', listID: '" + "1" + "', userID: '" + userID + "'}",
                dataType: "json",
                async: false,
                success: function (responce) {
                    if (responce.d != "") {
                        //success
                    } else {
                        //fail
                    }
                },
                error: function (error) {
                    //fail
                }
            });
        }

        jQuery(function ($) {
            _.templateSettings = { interpolate: /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
                evaluate: /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
                escape: /\{%-([\s\S]+?)%\}/g
            }; // excape HTML: {%- <script> %} prints &lt;script&gt;

            alt.userID = xuid;
            alt.selectedDate = xdate;
            alt.propData = alt.getProperties(alt.selectedDate, alt.list);
            alt.renderProperties(alt.propData);
            alt.saleDates = alt.getSaleDates();

            //UI functions
            $.each(alt.saleDates, function (index, value) {
                $('.datesList').append($('<option/>', {
                    value: value,
                    text: value
                }));
            });
            $("body").on("click", '#addToList', function () {
                var account = $(this).parent().attr('id');
                alt.addToReviewList(account, alt.userID);
            });
            $(".propList").on("change", function () {
                alt.list = $(".propList").val();
                alt.propData = alt.getProperties(alt.selectedDate, alt.list);
                alt.renderProperties(alt.propData);
            });
            $(".datesList").on("change", function () {
                alt.selectedDate = $(".datesList").val();
                alt.propData = alt.getProperties(alt.selectedDate, alt.list);
                alt.renderProperties(alt.propData);
            });
        });
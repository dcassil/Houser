/// <reference path="touch.js" />
/// <reference path="libs/spin/spin.js" />
/// <reference path="libs/spin/spinOptions.js" />

        var alt = {};
        alt.userID = -1;
        alt.saleDates = {};
        alt.selectedDate = -1;
        alt.selectedList = "2";
        alt.propData = {};
        alt.swipeDirection = null;
        alt.totalProperties = 0;
        alt.currentProperty = 1;
        // get properties.
        alt.getProperties = function (saleDate, list, filterBy, value) {
            var data;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '/WebUtilities/DetailsWebService.asmx/GetPropertiesBySaleDate',
                data: "{sDate: '" + saleDate + "', list: '" + list + "', sUserID: '" + alt.userID + "'}",
                dataType: "json",
                async: true,
                success: function (responce) {
                    if (responce.d != "") {
                        alt.propData = eval('(' + responce.d + ');');
                        alt.setZillowUrl();
                        alt.renderProperties(alt.propData);
                    } else {
                        $(".wrapper").html("");
                        data = null;
                    }
                },
                error: function (error) {
                    data = error;
                }
            });
            return data;
        }
        alt.setZillowUrl = function () {
            for (var prop in alt.propData) {
                var zillowfiedAddress = alt.propData[prop].Address.trim().replace(/ /g, "-").replace(",", "-").replace("-(recalled)", "").replace("--", "-") + "_rb";
                var zillowUrl = "http://www.zillow.com/homes/";
                alt.propData[prop].zillowUrl = zillowUrl + zillowfiedAddress;
            }
        }
        alt.filterProperties = function (filterBy, value, properties) {
            var arr = [];
            if (filterBy && value) {
                var valueAndComparer = value.split(",");
                var index = 0;
                for (var i in properties) {
                    if (properties.hasOwnProperty(i)) {
                        if (valueAndComparer[0] === ">" && Number(properties[i][filterBy]) > Number(valueAndComparer[1])) {
                            arr[index] = properties[i];
                            index++;
                        }
                        if (valueAndComparer[0] === "<" && Number(properties[i][filterBy]) < Number(valueAndComparer[1])) {
                            arr[index] = properties[i];
                            index++;
                        }
                    }
                }
                return arr;
            } else {
                return properties;
            }
        }
        alt.renderProperties = function (properties) {
            if (properties != null) {
                alt.totalProperties = properties.length;
                var template = $("#tmpPropertyData").html();
                $(".wrapper").html();
                $(".wrapper").html(_.template(template, { propData: properties }));
                alt.fadeInOutProgress();
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
        alt.saveNote = function (note, accountID, userID) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '/WebUtilities/NoteWebService.asmx/SaveAccountNote',
                data: "{accountNumber: '" + accountID + "', userID: '" + userID + "', note: '" + note + "'}",
                dataType: "json",
                async: false
                });
        }
        alt.removeFromList = function (accountNumber, userID) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '/WebUtilities/DetailsWebService.asmx/RemovePropertyFromList',
                data: "{accountNumber: '" + accountNumber + "', listID: '" + "1" + "', userID: '" + userID + "'}",
                dataType: "json",
                async: false,
                success: "",
                error: ""
            });
        }
        alt.gestureStart = function (event) {
            var check = event;
        }
        alt.blockSwipe = function (event) {
            if (window.innerHeight > window.innerWidth) {
                // Tell Safari not to move the window.
                //event.preventDefault();
                // Parse the swipe event and find the direction
                alt.swipeDirection = touch.getSwipeDirection(event);
            }
        }
        alt.swipeStart = function (event) {
            touch.getStart(event);
        }
        alt.swipeEnd = function (event) {
            //get direction and scroll accordingly
            if (touch.swipeDirection !== null) {
                if (touch.swipeDirection === touch.down || touch.swipeDirection === touch.right) {
                    alt.scrollToNextProperty(event);
                }
                //get direction and scroll accordingly
                if (touch.swipeDirection === touch.up || touch.swipeDirection === touch.left) {
                    alt.scrollToPreviousProperty(event);
                }
                touch.swipeDirection = null;
                alt.swipeDirection = null;
            }
        }
        alt.scrollToNextProperty = function (event) {
            var $nextPage = $(event.target).parents(".propPage").next(".propPage");
            if ($nextPage.length > 0) {
                alt.currentProperty = $nextPage.attr("data");
                alt.fadeInOutProgress();
                var scrollTo = $nextPage.position().top;
                $('html, body').stop().animate({
                    scrollTop: (scrollTo - 30)
                }, 200);
            } else {
                $('html, body').stop().animate({
                    scrollTop: $(document).height()
                }, 200);
            }
        }
        alt.scrollToPreviousProperty = function (event) {
            var $prevPage = $(event.target).parents(".propPage").prev(".propPage");
            if ($prevPage.length > 0) {
                alt.currentProperty = $prevPage.attr("data");
                alt.fadeInOutProgress();
                var scrollTo = $prevPage.position().top;
                $('html, body').animate({
                    scrollTop: (scrollTo - 30)
                }, 200);
            } else {
                $('html, body').animate({
                    scrollTop: 0
                }, 200);
            }
        }
        alt.fadeInOutProgress = function () {
            $(".progress").html(alt.currentProperty + " / " + alt.totalProperties);
            $(".progress").removeClass("hide");

            //            $(".progress").stop(true, true).fadeIn(5000, function () {
            //                return true;
            //            });
            //            $(".progress").stop(true, true).fadeOut(2000, function () {
            //            });
        }
        jQuery(function ($) {
            _.templateSettings = { interpolate: /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
                evaluate: /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
                escape: /\{%-([\s\S]+?)%\}/g
            }; // excape HTML: {%- <script> %} prints &lt;script&gt;

            alt.userID = xuid;
            alt.saleDates = alt.getSaleDates();
            alt.selectedDate = alt.saleDates[0];
            alt.selectedList = $(".propList").val();
            var spinner = new Spinner(spinOptions).spin();
            $(".wrapper").html(spinner.el);
            alt.getProperties(alt.selectedDate, alt.selectedList);

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
                $(this).val("In Review");
            });
            $("body").on("click", '#removeFromList', function () {
                var account = $(this).parent().attr('id');
                alt.removeFromList(account, alt.userID);
                $(this).parent().remove();
            });

            $("body").on("click", '.clearFilter', function () {
                alt.renderProperties(alt.propData);
                $(".clearFilter").hide();
            });
            $("body").on("transitionend webkitTransitionEnd oTransitionEnd MSTransitionEnd", ".progress", function () {
                $(".progress").addClass("hide");
            });
            $("body").on("click", ".sortable", function () {
                var $self = $(this);
                if ($self.val() === "SQFT") {
                    var value = prompt("Filter by " + $self.val() + " with symbol like", ">, " + $self.parent().next("dd").html());
                    var propArray = alt.filterProperties("Sqft", value, alt.propData);
                    alt.renderProperties(propArray);
                    $(".clearFilter").css('visibility', 'visible');
                }
                if ($self.val() === "Year Built") {
                    var value = prompt("Filter by " + $self.val() + " with symbol like", ">, " + $self.parent().next("dd").html());
                    var propArray = alt.filterProperties("YearBuilt", value, alt.propData);
                    alt.renderProperties(propArray);
                    $(".clearFilter").css('visibility', 'visible');
                }
                if ($self.val() === "Lot Size") {
                    var value = prompt("Filter by " + $self.val() + " with symbol like", ">, " + $self.parent().next("dd").html());
                    var propArray = alt.filterProperties("LotSize", value, alt.propData);
                    alt.renderProperties(propArray);
                    $(".clearFilter").css('visibility', 'visible');
                }
                if ($self.val() === "Assessed Value") {
                    var value = prompt("Filter by " + $self.val() + " with symbol like", ">, " + $self.parent().next("dd").html());
                    var propArray = alt.filterProperties("SalePrice", value.replace("$", ""), alt.propData);
                    alt.renderProperties(propArray);
                    $(".clearFilter").css('visibility', 'visible');
                }
                if ($self.val() === "Bedrooms") {
                    var value = prompt("Filter by " + $self.val() + " with symbol like", ">, " + $self.parent().next("dd").html());
                    var propArray = alt.filterProperties("Beds", value, alt.propData);
                    alt.renderProperties(propArray);
                    $(".clearFilter").css('visibility', 'visible');
                }
                if ($self.val() === "Bathrooms") {
                    var value = prompt("Filter by " + $self.val() + " with symbol like", ">, " + $self.parent().next("dd").html());
                    var propArray = alt.filterProperties("Baths", value, alt.propData);
                    alt.renderProperties(propArray);
                    $(".clearFilter").css('visibility', 'visible');
                }
            });
            $(".propList").on("change", function () {
                var spinner = new Spinner(spinOptions).spin();
                $(".wrapper").html(spinner.el);
                alt.selectedList = $(".propList").val();
                alt.getProperties(alt.selectedDate, alt.selectedList);
            });
            $(".datesList").on("change", function () {
                var spinner = new Spinner(spinOptions).spin();
                $(".wrapper").html(spinner.el);
                alt.selectedDate = $(".datesList").val();
                alt.getProperties(alt.selectedDate, alt.selectedList);
            });
            $("body").on("blur", ".notes textarea", function () {
                var accountID = $(this).parent().attr("id");
                var note = $(this).val();
                alt.saveNote(note, accountID, alt.userID);
                $(this).css('height', '30px');
                var scrollTo = $(this).parents(".propPage").position().top;
                $('html, body').animate({
                    scrollTop: (scrollTo - 30)
                }, 300);
            });
            $("body").on("click", ".address", function () {
                $(this).next(".addressMenu").toggle();
            });
            $("body").on("focus", ".notes textarea", function () {
                $(this).css('height', '120px');
            });
        });
/// <reference path="libs/jquery/jquery-1.4.1-vsdoc.js" />

jQuery(function ($) {
    $(".settings-menu").on("click", '.switch.display-images .icon', function () {
        if ($(this).closest(".icon-off").length != 0) {
            $(this).closest(".icon-off").addClass("icon-on");
            $(this).closest(".icon-off").removeClass("icon-off");
        } else {
            $(this).closest(".icon-on").addClass("icon-off");
            $(this).closest(".icon-on").removeClass("icon-on"); ;
        }
    });
    $(".settings-menu").on("click", '.sub-select.list', function () {
        $(".settings-menu").append('<div class="list-menu"></div>');
        //$(".list-menu").addClass("show");
    });
});
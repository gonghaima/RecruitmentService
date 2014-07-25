/// <reference path="jquery-1.9.1.js" />
$(".showDiv").click(function () {
    $(".isHide").slideDown(1000);
    $(".showDiv").css("display", "none");
});

//var d = $('[name="Company"]').val();
//if (d != "")
//{
//    $(".isHide").slideDown(1000);
//}

$(document).ready(function () {
    

    if ($(".datepicker")[0]) {
        $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    };

    if ($("title").text().indexOf('Login') != -1) {
        $("#menu").hide();
    };

    $(".lstMenu").mouseover(function () {
        //$("header").css("margin-bottom", "100px"),
        $('.lstDropdown').css("display","block");
    });

    $(".lstDropdown").mouseout(function () {
        //$("header").css("margin-bottom", "0px"),
        $('.lstDropdown').css("display", "none");
    });

});


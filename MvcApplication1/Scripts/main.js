﻿/// <reference path="jquery-1.9.1.js" />
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
    }
});


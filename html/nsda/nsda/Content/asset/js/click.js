$(function () {
    $(".common-lists li").each(function (k, v) {
        $(v).click(function () {
            if (!$(v).is(".selected")) {
                $(this).addClass("selected").siblings().removeClass("selected");
            } else {
                $(this).removeClass("selected");
            }
        });
    });
});
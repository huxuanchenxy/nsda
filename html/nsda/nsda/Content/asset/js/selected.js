
$(".address-lists li").click(function () {
    $(this).addClass("selected").siblings().removeClass("selected")
})
$(".level-lists li").click(function () {
    $(this).addClass("selected").siblings().removeClass("selected")
})
$(".time-mon li").click(function () {
    $(this).addClass("selected").siblings().removeClass("selected")
})
$(".page a").click(function () {
    $(this).addClass("on").siblings().removeClass("on")
})
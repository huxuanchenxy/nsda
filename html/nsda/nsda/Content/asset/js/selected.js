
$(".address-lists li").click(function () {
    console.log($(this))
    $(this).addClass("selected").siblings().removeClass("selected")
})
$(".level-lists li").click(function () {
    console.log($(this))
    $(this).addClass("selected").siblings().removeClass("selected")
})
$(".time-mon li").click(function () {
    console.log($(this))
    $(this).addClass("selected").siblings().removeClass("selected")
})

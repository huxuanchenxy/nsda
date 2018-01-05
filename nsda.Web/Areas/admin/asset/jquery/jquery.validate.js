$.fn.IsSuccess = function () {
    var msg = "";
    var flag = true;
    $(this).find("[ischeck=yes]").each(function () {
        var obj = $(this);
        var regular = obj.attr("regular");
        if (regular != undefined) {
            var val = obj.val();
            if (obj.hasClass('fgselect')) {
                val = obj.attr('data-id');
                if (val == 0)
                    val = "";
            }
            switch (regular) {
                case "mobile":
                    {
                        if (!isMobile(val)) {
                            msg = "手机有误！";
                            flag = false;
                            showmsg(obj, msg);
                            return false;
                        }
                        break;
                    }
                case "pwd":
                    {
                        if (!isPwd(val)) {
                            msg = "必须是6-20位数字、大、小写字母、特殊符号中的二种及以上的组合！";
                            flag = false;
                            showmsg(obj, msg);
                            return false;
                        }
                        break;
                    }
                case "notnull":
                    {
                        if (isNotNull(val)) {
                            msg = "不能为空！";
                            flag = false;
                            showmsg(obj, msg);
                            return false;
                        }
                        break;
                    }
                case "num":
                    {
                        if (!isInt(val)) {
                            msg = "必须为数字！";
                            flag = false;
                            showmsg(obj, msg);
                            return false;
                        }
                        break;
                    }
                case "moneylt":
                    {
                        if (!isMoney(val)) {
                            msg = "必须为数字！";
                            flag = false;
                            showmsg(obj, msg);
                            return false;
                        }
                        else {
                            var min = obj.attr("min");
                            if (Number(val) < Number(min)) {
                                msg = "最小值为" + min + "！";
                                flag = false;
                                showmsg(obj, msg);
                                return false;
                            }
                        }
                        break;
                    }
                case "moneygt":
                    {
                        if (!isMoney(val)) {
                            msg = "必须为数字！";
                            flag = false;
                            showmsg(obj, msg);
                            return false;
                        }
                        else {
                            var max = obj.attr("max");
                            if (Number(val) < Number(max)) {
                                msg = "最大值为" + max;
                                flag = false;
                                showmsg(obj, msg);
                                return false;
                            }
                        }
                        break;
                    }
                case "numrange":
                    {
                        if (!isInt(val)) {
                            msg = "必须为数字！";
                            flag = false;
                            showmsg(obj, msg);
                            return false;
                        }
                        else {
                            var min = obj.attr("min");
                            var max = obj.attr("max");
                            if (Number(val) < Number(min) || Number(val) > Number(max)) {
                                msg = "必须在" + min + "至" + max;
                                flag = false;
                                showmsg(obj, msg);
                                return false;
                            }
                        }
                        break;
                    }
                case "email":
                    {
                        if (!isEmail(val)) {
                            msg = "邮箱有误！";
                            flag = false;
                            showmsg(obj, msg);
                            return false;
                        }
                        break;
                    }
                case "phone":
                    {
                        if (!isTel(val)) {
                            msg = "电话有误！";
                            flag = false;
                            showmsg(obj, msg);
                            return false;
                        }
                        break;
                    }
                case "uri":
                    {
                        if (!isUri(val)) {
                            msg = "网址有误！";
                            flag = false;
                            showmsg(obj, msg);
                            return false;
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    });

    return flag;

    function isMobile(mobile) {
        var yidong = /^170[356]\d{7}$|^(?:13[4-9]|147|178|15[0-27-9]|178|18[2-478])\d{8}$/,
            liantong = /^170[4789]\d{7}$|^(?:13[0-2]|145|15[56]|17[165]|18[56])\d{8}$/,
            dianxin = /^170[01]\d{7}$|^(?:133|153|173|177|18[019])\d{8}$/;
        var flag = false;
        switch (true) {
            case yidong.test(mobile):
                flag = true;
                break;
            case liantong.test(mobile):
                flag = true;
                break;
            case dianxin.test(mobile):
                flag = true;
                break;
            default:
                flag = false;
        }
        return flag;
    };

    function isNotNull(obj) {
        obj = $.trim(obj);
        if (obj.length == 0 || obj == null || obj == undefined) {
            return true;
        }
        else
            return false;
    }

    function isMoney(obj) {
        reg = /^[+]{0,1}(\d+)$|^[+]{0,1}(\d+\.\d+)$/;
        if (!reg.test(obj)) {
            return false;
        } else {
            return true;
        }
    }

    function isInt(obj) {
        reg = /^[-+]?\d+$/;
        if (!reg.test(obj)) {
            return false;
        } else {
            return true;
        }
    }

    function isEmail(email) {
        var reg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
        if (!reg.test(email)) {
            return false;
        } else {
            return true;
        }
    }

    function isTel(tel) {
        reg = /^(\d{3,4}\-)?[1-9]\d{6,7}$/;
        if (!reg.test(tel)) {
            return false;
        } else {
            return true;
        }
    }

    function isUri(uri) {
        reg = /^http:\/\/[a-zA-Z0-9]+\.[a-zA-Z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/;
        if (!reg.test(uri)) {
            return false;
        } else {
            return true;
        }
    }

    function isPwd(pwd) {
        var num = 0;
        var rule1 = /\d+/;
        var rule2 = /[a-z]+/;
        var rule3 = /[A-Z]+/;
        var rule4 = /[~!@@#\$%^&*\{\};,.\?\/'"]/;
        var rule5 = /^.{6,16}$/;
        var flag1 = rule1.test(pwd);
        var flag2 = rule2.test(pwd);
        var flag3 = rule3.test(pwd);
        var flag4 = rule4.test(pwd);

        var flag5 = rule5.test(pwd);

        if (flag1) {
            num = num + 1;
        }
        if (flag2) {
            num = num + 1;
        }
        if (flag3) {
            num = num + 1;
        }
        if (flag4) {
            num = num + 1;
        }

        if (!(num >= 2 && flag5)) {
            return false;
        } else {
            return true;
        }
    }
}

function showmsg(obj, msg) {
    try {
        removemsg(obj);
        obj.focus();
        var $poptip_error = $('<div class="poptip"><span class="poptip-arrow poptip-arrow-top"><em>◆</em></span>' + msg + '</div>').css("left", obj.offset().left + 'px').css("top", obj.offset().top + obj.parent().height() + 5 + 'px');
        $('body').append($poptip_error);
        if (obj.hasClass('fg-control') || obj.hasClass('fgselect')) {
            obj.parent().addClass('has-error');
        }
        if (obj.hasClass('fgselect')) {
            $('.input-error').remove();
        }
        obj.change(function () {
            if (obj.val()) {
                removemsg(obj);
            }
        });
        if (obj.hasClass('fgselect')) {
            $(document).click(function (e) {
                if (obj.attr('data-id')) {
                    removemsg(obj);
                }
                e.stopPropagation();
            });
        }
        return false;
    } catch (e) {
    }
}

function removemsg(obj) {
    obj.parent().removeClass('has-error');
    $('.poptip').remove();
    $('.input-error').remove();
}
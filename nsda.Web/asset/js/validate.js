(function (v) {
    var validate = {
    };
    validate.isMobile = function (mobile) {
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

    validate.isNotNull = function (obj) {
        obj = $.trim(obj);
        if (obj.length == 0 || obj == null || obj == undefined) {
            return true;
        }
        else
            return false;
    };

    validate.isMoney = function (obj) {
        reg = /^[+]{0,1}(\d+)$|^[+]{0,1}(\d+\.\d+)$/;
        if (!reg.test(obj)) {
            return false;
        } else {
            return true;
        }
    };

    validate.isInt = function (obj) {
        reg = /^[-+]?\d+$/;
        if (!reg.test(obj)) {
            return false;
        } else {
            return true;
        }
    };

    validate.isEmail = function (email) {
        var reg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
        if (!reg.test(email)) {
            return false;
        } else {
            return true;
        }
    };

    validate.isTel = function (tel) {
        reg = /^(\d{3,4}\-)?[1-9]\d{6,7}$/;
        if (!reg.test(tel)) {
            return false;
        } else {
            return true;
        }
    };

    validate.isUri = function (uri) {
        reg = /^http:\/\/[a-zA-Z0-9]+\.[a-zA-Z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/;
        if (!reg.test(uri)) {
            return false;
        } else {
            return true;
        }
    };
    
    validate.isPwd = function (pwd) {
        var num = 0;
        var rule1 = /\d+/;
        var rule2 = /[a-z]+/;
        var rule3 = /[A-Z]+/;
        var rule4 = /[~!@@#\$%^&*\{\};,.\?\/'"]/;
        var rule5 = /^.{6,32}$/;
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
    };

    v.validate = validate;
})(window);
$.mobile=function(textarea) {
    var yidong = /^170[356]\d{7}$|^(?:13[4-9]|147|178|15[0-27-9]|178|18[2-478])\d{8}$/,
        liantong = /^170[4789]\d{7}$|^(?:13[0-2]|145|15[56]|17[165]|18[56])\d{8}$/,
        dianxin = /^170[01]\d{7}$|^(?:133|153|154|173|177|18[019])\d{8}$/;

    var t = {
        cf: [],
        yd: [],
        lt: [],
        dx: [],
        qt: []
    }; 
    var r = []; 
    var arr = textarea.replace(/\r\n/g, ",").replace(/\n/g, ",").split(',');
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] !== "") {
            if (r.indexOf(arr[i], 0) == -1) {
                switch (true) {
                    case yidong.test(arr[i]):
                        t.yd.push(arr[i]);
                        r.push(arr[i]);
                        break;
                    case liantong.test(arr[i]):
                        t.lt.push(arr[i]);
                        r.push(arr[i]);
                        break;
                    case dianxin.test(arr[i]):
                        t.dx.push(arr[i]);
                        r.push(arr[i]);
                        break;
                    default:
                        t.qt.push(arr[i]);
                }
            } else {
                t.cf.push(arr[i]);
            }
        }
    }

    if (r.length > 0) {
        if (t.cf.length > 0 || t.qt.length > 0) {
            layermsg("检测到重复号码：" + t.cf.length + "个，错误号码：" + t.qt.length + "个，已为您自动过滤", 5);
        }
    } else {
    }
};

$.isMobile = function (mobile) {
    var yidong = /^170[356]\d{7}$|^(?:13[4-9]|147|178|15[0-27-9]|178|18[2-478])\d{8}$/,
        liantong = /^170[4789]\d{7}$|^(?:13[0-2]|145|15[56]|17[165]|18[56])\d{8}$/,
        dianxin = /^170[01]\d{7}$|^(?:133|153|173|177|18[019])\d{8}$/;
    var flag=false;
        switch (true) {
            case yidong.test(mobile):
                flag=true;
                break;
            case liantong.test(mobile):
                flag=true;
                break;
            case dianxin.test(mobile):
                flag=true;
                break;
            default:
                flag=false;
        }
        return flag;
};
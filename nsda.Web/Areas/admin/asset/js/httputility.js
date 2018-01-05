$.delete = function (options) {
    var defaults = {
        msg: "注：您确定要删除吗？数据删除后将无法恢复",
        url: "",
        param: [],
        type: "post",
        dataType: "json",
        callback: null
    };
    var options = $.extend(defaults, options);
    layerconfirm(options.msg, function (r) {
        if (r) {
            var postdata = options.param;
            postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
            $.ajax({
                url: options.url,
                data: postdata,
                type: options.type,
                dataType: options.dataType,
                beforeSend: function () {
                    loading(true);
                },
                success: function (data) {
                    loading(false);
                    if (data.flag) {
                        layermsg("操作成功", 1);
                        options.callback(data);
                        fgclose();
                    } else {
                        layermsg(data.msg, -1);
                    }
                },
                complete: function (data) {
                    loading(false);
                    if (data.responseText.indexOf('fgunloginfg') != -1) {
                        layerconfirm("由于您长时间没有操作,请重新登录？", function (r) {
                            if (r) {
                                window.location.href = "/login/login";
                            }
                        });
                        return;
                    }
                }
            });
        }
    });
}

$.confirm = function (options) {
    var defaults = {
        msg: "",
        url: "",
        param: [],
        type: "post",
        dataType: "json",
        callback: null
    };
    var options = $.extend(defaults, options);
    layerconfirm(options.msg, function (r) {
        if (r) {
            var postdata = options.param;
            postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
            $.ajax({
                url: options.url,
                data: postdata,
                type: options.type,
                dataType: options.dataType,
                beforeSend: function () {
                    loading(true);
                },
                success: function (data) {
                    loading(false);
                    if (data.flag) {
                        layermsg("操作成功", 1);
                        options.callback(data);
                        fgclose();
                    } else {
                        layermsg(data.msg, -1);
                    }
                },
                complete: function (data) {
                    loading(false);
                    if (data.responseText.indexOf('fgunloginfg') != -1) {
                        layerconfirm("由于您长时间没有操作,请重新登录？", function (r) {
                            if (r) {
                                window.location.href = "/login/login";
                            }
                        });
                        return;
                    }
                }
            });
        }
    });
}

$.post = function (options) {
    var defaults = {
        msg: "",
        url: "",
        param: [],
        type: "post",
        dataType: "json",
        callback: null
    };
    var options = $.extend(defaults, options);
    var postdata = options.param;
    postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    $.ajax({
        url: options.url,
        data: postdata,
        type: options.type,
        dataType: options.dataType,
        beforeSend: function () {
            loading(true);
        },
        success: function (data) {
            loading(false);
            if (data.flag) {
                layermsg("操作成功", 1);
                options.callback(data);
                fgclose();
            } else {
                layeralert(data.msg, -1);
            }
        },
        complete: function (data) {
            loading(false);
            if (data.responseText.indexOf('fgunloginfg') != -1) {
                layerconfirm("由于您长时间没有操作,请重新登录？", function (r) {
                    if (r) {
                        window.location.href = "/login/login";
                    }
                });
                return;
            }
        }
    });
}

$.get = function (options) {
    var defaults = {
        url: "",
        param: [],
        type: "get",
        dataType: "json",
        callback: null,
        async: false
    };
    var options = $.extend(defaults, options);
    $.ajax({
        url: options.url,
        data: options.param,
        type: options.type,
        dataType: options.dataType,
        async: options.async,
        success: function (data) {
            if (data.flag) {
                options.callback(data);
            }
        },
        complete: function (data) {
            if (data.responseText.indexOf('fgunloginfg') != -1) {
                layerconfirm("由于您长时间没有操作,请重新登录？", function (r) {
                    if (r) {
                        window.location.href = "/login/login";
                    }
                });
                return;
            }
        }
    });
}

$.complete = function (options) {
    var defaults = {
        msg: "注：您确定要将回款计划设定完成",
        url: "",
        param: [],
        type: "post",
        dataType: "json",
        callback: null
    };
    var options = $.extend(defaults, options);
    layerconfirm(options.msg, function (r) {
        if (r) {
            var postdata = options.param;
            postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
            $.ajax({
                url: options.url,
                data: postdata,
                type: options.type,
                dataType: options.dataType,
                beforeSend: function () {
                    loading(true);
                },
                success: function (data) {
                    loading(false);
                    if (data.flag) {
                        layermsg("操作成功", 1);
                        options.callback(data);
                        fgclose();
                    } else {
                        layermsg(data.msg, -1);
                    }
                },
                complete: function (data) {
                    loading(false);
                    if (data.responseText.indexOf('fgunloginfg') != -1) {
                        layerconfirm("由于您长时间没有操作,请重新登录？", function (r) {
                            if (r) {
                                window.location.href = "/login/login";
                            }
                        });
                        return;
                    }
                }
            });
        }
    });
}
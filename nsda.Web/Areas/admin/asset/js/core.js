$(function () {
    $(".fg-filter-text").click(function () {
        var $obj = $(this);
        if ($obj.next('.fg-filter-list').is(":hidden")) {
            $obj.css('border-bottom-color', '#1e89e1');
            $(".fg-filter-list").slideDown(10);
            $obj.addClass("active")
        } else {
            $obj.css('border-bottom-color', 'rgb(204, 204, 204)');
            $(".fg-filter-list").slideUp(10);
            $obj.removeClass("active")
        }
    });

    $(".left-nav li").click(function () {
        $(".left-nav li").removeClass("active").removeClass("hover");
        $(this).addClass("active")
    }).hover(function () {
        if (!$(this).hasClass("active")) {
            $(this).addClass("hover")
        }
    }, function () {
        $(this).removeClass("hover")
    });

    $(".m-check").click(function () {
        var $this = $(this);
        if ($this.hasClass("checked")) {
            $this.removeClass("checked");
            $this.prev().attr("checked", false);
        } else {
            $this.addClass("checked");
            $this.prev().attr("checked", "checked");
        }
        $this.prev().trigger("click");
    });

    $("#btncancel").click(function () {
        var obj = $(this).parent().parent();
        obj.prev().removeClass("active").css("border-bottom-color", "rgb(204, 204, 204)");
        obj.hide();
    });
});

$.fn.selectoption = function (options, data, value) {
    var $select = $(this);
    var html = '';
    html += '<div class="fgselect-text" style="color:#999;">' + options.desc + '</div>';
    html += '<div class="fgselect-option">';
    html += '<div class="fgselect-option-content" style="max-height: ' + options.height + '">' + $select.html() + '</div>';
    html += '</div>';
    $select.html(html);

    var $option_html = $($("<p>").append($select.find('.fgselect-option').clone()).html());
    $option_html.attr('id', $select.attr('id') + '-option');
    $select.find('.fgselect-option').remove();
    if ($option_html.length > 0) {
        $('body').find('#' + $select.attr('id') + '-option').remove();
    }
    $('body').prepend($option_html);

    var $option = $("#" + $select.attr('id') + "-option");

    if (data != null) {
        var $_html = $('<ul></ul>');
        if (data.length > 0) {
            $.each(data, function (i) {
                var row = data[i];
                $_html.append('<li data-id="' + row[options.id] + '">' + row[options.text] + '</li>');
            });
            $option.find('.fgselect-option-content').html($_html);
            $option.find('li').css('padding', "0 5px");
            $option.find('li').click(function (e) {
                var data_val = $(this).text();
                var data_id = $(this).attr('data-id');
                $select.attr("data-id", data_id).attr("data-val", data_val);
                $select.find('.fgselect-text').html(data_val).css('color', '#000');
                $option.slideUp(150);
                $select.trigger("change");
                e.stopPropagation();
            }).hover(function (e) {
                if (!$(this).hasClass('liactive')) {
                    $(this).toggleClass('on');
                }
                e.stopPropagation();
            });
        }
    }

    $select.unbind('click');

    $select.bind("click", function (e) {
        $(this).addClass('fgselect-focus');
        if ($option.is(":hidden")) {
            $select.find('.fgselect-option').hide();
            $('.fgselect-option').hide();
            var left = $select.offset().left;
            var top = $select.offset().top + 29;
            var width = $select.width();
            if (options.width) {
                width = options.width;
            }
            if (($option.height() + top) < $(window).height()) {
                $option.slideDown(150).css({ top: top, left: left, width: width });
            } else {
                var _top = (top - $option.height() - 32)
                $option.show().css({ top: _top, left: left, width: width });
                $option.attr('data-show', true);
            }
            $option.css('border-top', '1px solid #ccc');
            $option.find('li').removeClass('liactive');
            $option.find('[data-id=' + $select.attr('data-id') + ']').addClass('liactive');
            $option.find('.fgselect-option-search').find('input').select();
        } else {
            if ($option.attr('data-show')) {
                $option.hide();
            } else {
                $option.slideUp(150);
            }
        }
        e.stopPropagation();
    });

    $(document).click(function (e) {
        var e = e ? e : window.event;
        var tar = e.srcElement || e.target;
        if (!$(tar).hasClass('fg-control')) {
            if ($option.attr('data-show')) {
                $option.hide();
            } else {
                $option.slideUp(150);
            }
            $select.removeClass('fgselect-focus');
            e.stopPropagation();
        }
    });

    if (!$.isNullOrEmpty(value)) {

        var $select = $(this);
        var $option = $("#" + $select.attr('id') + "-option");
        $select.attr('data-id', value);
        var data_val = $option.find('ul').find('[data-id=' + value + ']').html();
        if (data_val) {
            $select.attr('data-val', data_val);
            $select.find('.fgselect-text').html(data_val).css('color', '#000');
            $option.find('ul').find('[data-id=' + value + ']').addClass('liactive')
        }
    }

    return $select;
}

$.fn.selectignore = function (options, data, ignoreId) {
    var value = '';
    var $select = $(this);
    var html = '';
    if (!options.isdefault) {
        html += '<div class="fgselect-text" style="color:#999;"></div>';
    }
    else {
        html += '<div class="fgselect-text" style="color:#999;">' + options.desc + '</div>';
    }
    html += '<div class="fgselect-option">';
    html += '<div class="fgselect-option-content" style="max-height: ' + options.height + '">' + $select.html() + '</div>';
    html += '</div>';
    $select.html(html);

    var $option_html = $($("<p>").append($select.find('.fgselect-option').clone()).html());
    $option_html.attr('id', $select.attr('id') + '-option');
    $select.find('.fgselect-option').remove();
    if ($option_html.length > 0) {
        $('body').find('#' + $select.attr('id') + '-option').remove();
    }
    $('body').prepend($option_html);

    var $option = $("#" + $select.attr('id') + "-option");

    if (data != null) {
        var $_html = $('<ul></ul>');
        if (data.length > 0) {
            $.each(data, function (i) {
                var row = data[i];
                if (row[options.id] != ignoreId) {
                    value = row[options.id];
                    $_html.append('<li data-id="' + row[options.id] + '">' + row[options.text] + '</li>');
                }
            });
            $option.find('.fgselect-option-content').html($_html);
            $option.find('li').css('padding', "0 5px");
            $option.find('li').click(function (e) {
                var data_val = $(this).text();
                var data_id = $(this).attr('data-id');
                $select.attr("data-id", data_id).attr("data-val", data_val);
                $select.find('.fgselect-text').html(data_val).css('color', '#000');
                $option.slideUp(150);
                $select.trigger("change");
                e.stopPropagation();
            }).hover(function (e) {
                if (!$(this).hasClass('liactive')) {
                    $(this).toggleClass('on');
                }
                e.stopPropagation();
            });
        }
    }

    $select.unbind('click');

    $select.bind("click", function (e) {
        $(this).addClass('fgselect-focus');
        if ($option.is(":hidden")) {
            $select.find('.fgselect-option').hide();
            $('.fgselect-option').hide();
            var left = $select.offset().left;
            var top = $select.offset().top + 29;
            var width = $select.width();
            if (options.width) {
                width = options.width;
            }
            if (($option.height() + top) < $(window).height()) {
                $option.slideDown(150).css({ top: top, left: left, width: width });
            } else {
                var _top = (top - $option.height() - 32)
                $option.show().css({ top: _top, left: left, width: width });
                $option.attr('data-show', true);
            }
            $option.css('border-top', '1px solid #ccc');
            $option.find('li').removeClass('liactive');
            $option.find('[data-id=' + $select.attr('data-id') + ']').addClass('liactive');
            $option.find('.fgselect-option-search').find('input').select();
        } else {
            if ($option.attr('data-show')) {
                $option.hide();
            } else {
                $option.slideUp(150);
            }
        }
        e.stopPropagation();
    });

    $(document).click(function (e) {
        var e = e ? e : window.event;
        var tar = e.srcElement || e.target;
        if (!$(tar).hasClass('fg-control')) {
            if ($option.attr('data-show')) {
                $option.hide();
            } else {
                $option.slideUp(150);
            }
            $select.removeClass('fgselect-focus');
            e.stopPropagation();
        }
    });

    if (!$.isNullOrEmpty(value)) {
        var $select = $(this);
        var $option = $("#" + $select.attr('id') + "-option");
        $select.attr('data-id', value);
        var data_val = $option.find('ul').find('[data-id=' + value + ']').html();
        if (data_val) {
            $select.attr('data-val', data_val);
            $select.find('.fgselect-text').html(data_val).css('color', '#000');
            $option.find('ul').find('[data-id=' + value + ']').addClass('liactive')
        }
    }

    return $select;
}

$.fn.selectcombox = function (options, value) {
    var $select = $(this);
    var html = '';
    if (!options.isdefault) {
        html += '<div class="fgselect-text" style="color:#999;"></div>';
    }
    else {
        html += '<div class="fgselect-text" style="color:#999;">' + options.desc + '</div>';
    }
    html += '<div class="fgselect-option">';
    html += '<div class="fgselect-option-content" style="max-height: ' + options.height + '">' + $select.html() + '</div>';
    html += '</div>';
    $select.html(html);

    var $option_html = $($("<p>").append($select.find('.fgselect-option').clone()).html());
    $option_html.attr('id', $select.attr('id') + '-option');
    $select.find('.fgselect-option').remove();
    if ($option_html.length > 0) {
        $('body').find('#' + $select.attr('id') + '-option').remove();
    }
    $('body').prepend($option_html);

    var $option = $("#" + $select.attr('id') + "-option");
    var val = "";
    $.get({
        url: options.url,
        param: options.param,
        callback: function (data) {
            if (data.flag) {
                var $_html = $('<ul></ul>');
                if (options.desc && options.isdefault) {
                    $_html.append('<li data-id="0">' + options.desc + '</li>');
                }
                if (data.data != null && data.data.length > 0) {
                    $.each(data.data, function (i) {
                        var row = data.data[i];
                        if (i == 0) {
                            if (!$.isNullOrEmpty(value)) {
                                val = value;
                            }
                            else {
                                val = row[options.id];
                            }
                        }
                        $_html.append('<li data-id="' + row[options.id] + '">' + row[options.text] + '</li>');
                    });
                    $option.find('.fgselect-option-content').html($_html);
                    $option.find('li').css('padding', "0 5px");
                    $option.find('li').click(function (e) {
                        var data_val = $(this).text();
                        var data_id = $(this).attr('data-id');
                        $select.attr("data-id", data_id).attr("data-val", data_val);
                        $select.find('.fgselect-text').html(data_val).css('color', '#000');
                        $option.slideUp(150);
                        $select.trigger("change");
                        e.stopPropagation();
                    }).hover(function (e) {
                        if (!$(this).hasClass('liactive')) {
                            $(this).toggleClass('on');
                        }
                        e.stopPropagation();
                    });
                }
            }
            else {
                return false;
            }
        }
    });

    $select.unbind('click');

    $select.bind("click", function (e) {
        $(this).addClass('fgselect-focus');
        if ($option.is(":hidden")) {
            $select.find('.fgselect-option').hide();
            $('.fgselect-option').hide();
            var left = $select.offset().left;
            var top = $select.offset().top + 29;
            var width = $select.width();
            if (options.width) {
                width = options.width;
            }
            if (($option.height() + top) < $(window).height()) {
                $option.slideDown(150).css({ top: top, left: left, width: width });
            } else {
                var _top = (top - $option.height() - 32)
                $option.show().css({ top: _top, left: left, width: width });
                $option.attr('data-show', true);
            }
            $option.css('border-top', '1px solid #ccc');
            $option.find('li').removeClass('liactive');
            $option.find('[data-id=' + $select.attr('data-id') + ']').addClass('liactive');
            $option.find('.fgselect-option-search').find('input').select();
        } else {
            if ($option.attr('data-show')) {
                $option.hide();
            } else {
                $option.slideUp(150);
            }
        }
        e.stopPropagation();
    });

    $(document).click(function (e) {
        var e = e ? e : window.event;
        var tar = e.srcElement || e.target;
        if (!$(tar).hasClass('fg-control')) {
            if ($option.attr('data-show')) {
                $option.hide();
            } else {
                $option.slideUp(150);
            }
            $select.removeClass('fgselect-focus');
            e.stopPropagation();
        }
    });

    if (!options.isdefault && !$.isNullOrEmpty(val))
        value = val;
    if (!$.isNullOrEmpty(value)) {
        var $select = $(this);
        var $option = $("#" + $select.attr('id') + "-option");
        $select.attr('data-id', value);
        var data_val = $option.find('ul').find('[data-id=' + value + ']').html();
        if (data_val) {
            $select.attr('data-val', data_val);
            $select.find('.fgselect-text').html(data_val).css('color', '#000');
            $option.find('ul').find('[data-id=' + value + ']').addClass('liactive')
        }
    }

    return $select;
}

$.fn.selecttree = function (options, value) {
    var $select = $(this);
    if ($select.find('.fgselect-text').length == 0) {
        var html = '';
        html += '<div class="fgselect-text"  style="color:#999;">' + options.desc + '</div>';
        html += '<div class="fgselect-option">';
        html += '<div class="fgselect-option-content" style="max-height: ' + options.height + '"></div>';
        html += '</div>';
        $select.append(html);
    }

    var $option_html = $($("<p>").append($select.find('.fgselect-option').clone()).html());
    $option_html.attr('id', $select.attr('id') + '-option');
    $select.find('.fgselect-option').remove();
    if (options.appendTo) {
        $(options.appendTo).prepend($option_html);
    } else {
        $('body').prepend($option_html);
    }
    var $option = $("#" + $select.attr('id') + "-option");
    var $option_content = $("#" + $select.attr('id') + "-option").find('.fgselect-option-content');

    loadtreeview(options.url);

    function loadtreeview(url) {
        $option_content.treeview({
            onnodeclick: function (item) {
                if (options.click) {
                    var i = "ok";
                    if (i = options.click(item), i == "false") return !1
                }
                $select.attr("data-id", item.id).attr("data-val", item.text);
                $select.find('.fgselect-text').html(item.text).css('color', '#000');
                $select.trigger("change");
                if (options.click) {
                    options.click(item);
                }
            },
            height: options.height,
            url: url,
            param: options.param,
            method: options.method,
            desc: options.desc
        });
    }

    if (options.icon) {
        $option.find('i').remove();
        $option.find('img').remove();
    }
    $select.find('.fgselect-text').unbind('click');
    $select.find('.fgselect-text').bind("click", function (e) {
        $(this).parent().addClass('fgselect-focus');
        if ($option.is(":hidden")) {
            $select.find('.fgselect-option').hide();
            $('.fgselect-option').hide();
            var left = $select.offset().left;
            var top = $select.offset().top + 29;
            var width = $select.width();
            if (options.width) {
                width = options.width;
            }
            if (($option.height() + top) < $(window).height()) {
                $option.slideDown(150).css({ top: top, left: left, width: width });
            } else {
                var _top = (top - $option.height() - 32);
                $option.show().css({ top: _top, left: left, width: width });
                $option.attr('data-show', true);
            }
            $option.css('border-top', '1px solid #ccc');
            if (options.appendTo) {
                $option.css("position", "inherit")
            }
            $option.find('.fgselect-option-search').find('input').select();
        } else {
            if ($option.attr('data-show')) {
                $option.hide();
            } else {
                $option.slideUp(150);
            }
        }
        e.stopPropagation();
    });
    $select.find('li div').click(function (e) {
        var e = e ? e : window.event;
        var tar = e.srcElement || e.target;
        if (!$(tar).hasClass('bbit-tree-ec-icon')) {
            $option.slideUp(150);
            e.stopPropagation();
        }
    });

    $(document).click(function (e) {
        var e = e ? e : window.event;
        var tar = e.srcElement || e.target;
        if (!$(tar).hasClass('bbit-tree-ec-icon') && !$(tar).hasClass('fg-control')) {
            if ($option.attr('data-show')) {
                $option.hide();
            } else {
                $option.slideUp(150);
            }
            $select.removeClass('fgselect-focus');
            e.stopPropagation();
        }
    });

    if (!$.isNullOrEmpty(value)) {
        var $select = $(this);
        var $option = $("#" + $select.attr('id') + "-option");
        $select.attr('data-id', value);
        var data_val = $option.find('ul').find('[data-id=' + value + ']').html();
        if (data_val) {
            $select.attr('data-val', data_val);
            $select.find('.fgselect-text').html(data_val).css('color', '#000');
            $option.find('ul').find('[data-id=' + value + ']').parent().parent().addClass('bbit-tree-selected');
        }
    }

    return $select;
}

$.fn.authbtn = function () {
    var $element = $(this);
    var menuid = tabiframeId().substr(6);
    var data = $.fn.foreach(top.buttondata, menuid);
    if (data != undefined) {
        $.each(data, function (i) {
            $element.find("#" + data[i].ButtonCode).attr('authyesorno', 'yes');
        });
    }
    $element.find('[authyesorno=no]').remove();
}

$.fn.foreach = function (data, menuid) {
    var reval = new Array();
    if (data != null) {
        $(data).each(function (i, v) {
            if (v.MenuId == menuid) {
                reval.push(v);
            }
        });
    }
    return reval;
}

$.fn.jqGridRowValue = function (key) {
    var $jgrid = $(this);
    var json = [];
    var selectedIds = $jgrid.jqGrid("getGridParam", "selarrrow");
    if (selectedIds != undefined && selectedIds != "") {
        for (var i = 0; i < selectedIds.length ; i++) {
            json.push($jgrid.jqGrid('getRowData', selectedIds[i])[key]);
        }
    } else {
        json.push($jgrid.jqGrid('getRowData', $jgrid.jqGrid('getGridParam', 'selrow'))[key]);
    }
    return String(json);
}

$.fn.jqGridRow = function () {
    var $jgrid = $(this);
    var json = [];
    var selectedIds = $jgrid.jqGrid("getGridParam", "selarrrow");
    if (selectedIds != undefined && selectedIds != "") {
        for (var i = 0; i < selectedIds.length ; i++) {
            json.push($jgrid.jqGrid('getRowData', selectedIds[i]));
        }
    } else {
        json.push($jgrid.jqGrid('getRowData', $jgrid.jqGrid('getGridParam', 'selrow')));
    }
    return json;
}

$.fn.jqGridRowId = function () {
    var $jgrid = $(this);
    var json = [];
    var selectedIds = $jgrid.jqGrid("getGridParam", "selarrrow");
    if (selectedIds != undefined && selectedIds != "") {
        for (var i = 0; i < selectedIds.length ; i++) {
            json.push($jgrid.jqGrid('getRowData', selectedIds[i]).Id);
        }
    } else {
        json.push($jgrid.jqGrid('getRowData', $jgrid.jqGrid('getGridParam', 'selrow')).Id);
    }
    return json;
}

function layeropen(options) {
    loading(true);
    var defaults = {
        id: null,
        title: '系统窗口',
        width: "100px",
        height: "100px",
        url: '',
        shade: 0.3,
        btn: ['确认', '关闭'],
        callback: null
    };
    var options = $.extend(defaults, options);
    var _url = options.url;
    var _width = top.$.windowWidth() > parseInt(options.width.replace('px', '')) ? options.width : top.$.windowWidth() + 'px';
    var _height = top.$.windowHeight() > parseInt(options.height.replace('px', '')) ? options.height : top.$.windowHeight() + 'px';
    top.layer.open({
        id: options.id,
        type: 2,
        shade: options.shade,
        title: options.title,
        fix: false,
        area: [_width, _height],
        content: _url,
        btn: options.btn,
        yes: function () {
            options.callback(options.id)
        }, cancel: function () {
            if (options.cancel != undefined) {
                options.cancel();
            }
            return true;
        }
    });
}

function layeralert(content, type) {
    if (type == -1) {
        type = 2;
    }
    top.layer.alert(content, {
        icon: type,
        title: "温馨提示"
    });
}

function layerconfirm(content, callback) {
    top.layer.confirm(content, {
        icon: 7,
        title: "温馨提示",
        btn: ['确认', '取消'],
    }, function () {
        callback(true);
    }, function () {
        callback(false)
    });
}

function layermsg(content, type) {
    if (type == -1) {
        type = 2;
    }
    top.layer.msg(content, { icon: type, time: 1500, shift: 5 });
}

function fgclose() {
    try {
        var index = top.layer.getFrameIndex(window.name);
        top.layer.close(index);
    } catch (e) {
    }
}

function reload() {
    location.reload();
    return false;
}

function formatdate(v, format) {
    if (!v) return "";
    var d = v;
    if (typeof v === 'string') {
        if (v.indexOf("/Date(") > -1)
            d = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
        else
            d = new Date(Date.parse(v.replace(/-/g, "/").replace("T", " ").split(".")[0]));
    }
    var o = {
        "M+": d.getMonth() + 1,
        "d+": d.getDate(),
        "h+": d.getHours(),
        "m+": d.getMinutes(),
        "s+": d.getSeconds(),
        "q+": Math.floor((d.getMonth() + 3) / 3),
        "S": d.getMilliseconds()
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}

$.parentiframe = function () {
    if ((top.frames[tabiframeId()].contentWindow != undefined) && ($.isbrowsername() == "Chrome" || $.isbrowsername() == "FF")) {
        return top.frames[tabiframeId()].contentWindow;
    }
    else {
        return top.frames[tabiframeId()];
    }
}

$.isbrowsername = function () {
    var userAgent = navigator.userAgent;
    var isOpera = userAgent.indexOf("Opera") > -1;
    if (isOpera) {
        return "Opera"
    };
    if (userAgent.indexOf("Firefox") > -1) {
        return "FF";
    }
    if (userAgent.indexOf("Chrome") > -1) {
        if (window.navigator.webkitPersistentStorage.toString().indexOf('DeprecatedStorageQuota') > -1) {
            return "Chrome";
        } else {
            return "360";
        }
    }
    if (userAgent.indexOf("Safari") > -1) {
        return "Safari";
    }
    if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
        return "IE";
    };
}

$.isNullOrEmpty = function (obj) {
    if (obj == "" || obj == null || obj == undefined) {
        return true;
    }
    else {
        return false;
    }
}

$.windowWidth = function () {
    return $(window).width();
}

$.windowHeight = function () {
    return $(window).height();
}

function toDecimal(num) {
    if (num == null) {
        return 0;
    }
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        return 0;
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        num = num.substring(0, num.length - (4 * i + 3)) + '' +
                num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num + '.' + cents);
}

function initresize() {
    $('.left-nav').height($(window).height() - 20);
    $('.nav-content').height($(window).height() - 20);
}

function loading(flag) {
    if (flag) {
        var html = '<div id="loading_bg"></div><div id="loading_fg"></div>';
        var $body = top.$('body');
        $body.append(html);
        var $loading_fg = top.$("#loading_fg");
        $loading_fg.css("left", ($body.width() - $loading_fg.width()) / 2 - 54);
        $loading_fg.css("top", ($body.height() - $loading_fg.height()) / 2);
    } else {
        top.$("#loading_fg").remove();
        top.$("#loading_bg").remove();
        top.$(".ajax-loader").remove();
    }
}

function tabiframeId() {
    var iframeId = top.$(".fg_iframe:visible").attr("id");
    return iframeId;
}

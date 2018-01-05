var menudata = [];
var buttondata = [];

(function ($) {
    fg = {
        newTab: function (item) {
            var dataId = item.id;
            var dataUrl = item.url;
            var menuName = item.title;
            var flag = true;
            if (dataUrl == undefined || $.trim(dataUrl).length == 0) {
                return false;
            }
         
            $('.menuTab').each(function () {
                var $this = $(this);
                if ($this.attr('data-menuid') == dataUrl) {
                    if (!$this.hasClass('active')) {
                        $this.addClass('active').siblings('.menuTab').removeClass('active');
                        $.fgtab.scrollToTab(this);
                        $('.mainContent .fg_iframe').each(function () {
                            var $this = $(this);
                            if ($this.attr('data-menuid') == dataUrl) {
                                $this.show().siblings('.fg_iframe').hide();
                                return false;
                            }
                        });
                    }
                    flag = false;
                    return false;
                }
            });
            if (flag) {
                if ($(".menuTab").length >= 12) {
                    layeralert("为保证系统效率,只允许同时运行12个功能窗口,请关闭一些窗口后重试！", 0);
                    return false;
                }
                var str = '<a href="javascript:;" class="active menuTab" data-menuid="' + dataUrl + '">' + menuName + ' <i class="fa fa-remove"></i></a>';
                $('.menuTab').removeClass('active');
                var str1 = '<iframe class="fg_iframe" id="iframe' + dataId + '" name="iframe' + dataId + '"  width="100%" height="100%" src="' + dataUrl + '" frameborder="0" data-menuid="' + dataUrl + '" seamless></iframe>';
                $('.mainContent').find('iframe.fg_iframe').hide();
                $('.mainContent').append(str1);
                loading(true);
                $('.mainContent iframe:visible').load(function () {
                    loading(false);
                });
                $('.menuTabs .page-tabs-content').append(str);
                $.fgtab.scrollToTab($('.menuTab.active'));
            }
        }
    }

    $.fgtab = {
        requestFullScreen: function () {
            var de = document.documentElement;
            if (de.requestFullscreen) {
                de.requestFullscreen();
            } else if (de.mozRequestFullScreen) {
                de.mozRequestFullScreen();
            } else if (de.webkitRequestFullScreen) {
                de.webkitRequestFullScreen();
            }
        },
        exitFullscreen: function () {
            var de = document;
            if (de.exitFullscreen) {
                de.exitFullscreen();
            } else if (de.mozCancelFullScreen) {
                de.mozCancelFullScreen();
            } else if (de.webkitCancelFullScreen) {
                de.webkitCancelFullScreen();
            }
        },
        refreshTab: function () {
            var currentId = $('.page-tabs-content').find('.active').attr('data-menuid');
            var target = $('.fg_iframe[data-menuid="' + currentId + '"]');
            var url = target.attr('src');
              target.attr('src', url).load(function () {
            });
        },
        activeTab: function () {
            var $this = $(this);
            var currentId = $this.attr('data-menuid');
            if (!$this.hasClass('active')) {
                $('.mainContent .fg_iframe').each(function () {
                    if ($(this).attr('data-menuid') == currentId) {
                        $(this).show().siblings('.fg_iframe').hide();
                        return false;
                    }
                });
                $this.addClass('active').siblings('.menuTab').removeClass('active');
                $.fgtab.scrollToTab(this);
            }
        },
        closeOtherTabs: function () {
            $('.page-tabs-content').children("[data-menuid]").find('.fa-remove').parents('a').not(".active").each(function () {
                $('.fg_iframe[data-menuid="' + $(this).attr('data-menuid') + '"]').remove();
                $(this).remove();
            });
            $('.page-tabs-content').css("margin-left", "0");
        },
        closeTab: function () {
            var $parent = $(this).parents('.menuTab');
            var closeTabId = $parent.attr('data-menuid');
            var currentWidth = $parent.width();
            if ($parent.hasClass('active')) {
                if ($parent.next('.menuTab').size()) {
                    var activeId = $parent.next('.menuTab:eq(0)').attr('data-menuid');
                    $parent.next('.menuTab:eq(0)').addClass('active');

                    $('.mainContent .fg_iframe').each(function () {
                        if ($(this).attr('data-menuid') == activeId) {
                            $(this).show().siblings('.fg_iframe').hide();
                            return false;
                        }
                    });
                    var marginLeftVal = parseInt($('.page-tabs-content').css('margin-left'));
                    if (marginLeftVal < 0) {
                        $('.page-tabs-content').animate({
                            marginLeft: (marginLeftVal + currentWidth) + 'px'
                        }, "fast");
                    }
                    $parent.remove();
                    $('.mainContent .fg_iframe').each(function () {
                        if ($(this).attr('data-menuid') == closeTabId) {
                            $(this).remove();
                            return false;
                        }
                    });
                }
                if ($parent.prev('.menuTab').size()) {
                    var activeId = $parent.prev('.menuTab:last').attr('data-menuid');
                    $parent.prev('.menuTab:last').addClass('active');
                    $('.mainContent .fg_iframe').each(function () {
                        if ($(this).attr('data-menuid') == activeId) {
                            $(this).show().siblings('.fg_iframe').hide();
                            return false;
                        }
                    });
                    $parent.remove();
                    $('.mainContent .fg_iframe').each(function () {
                        if ($(this).attr('data-menuid') == closeTabId) {
                            $(this).remove();
                            return false;
                        }
                    });
                }
            }
            else {
                $parent.remove();
                $('.mainContent .fg_iframe').each(function () {
                    if ($(this).attr('data-menuid') == closeTabId) {
                        $(this).remove();
                        return false;
                    }
                });
                $.fgtab.scrollToTab($('.menuTab.active'));
            }
            return false;
        },
        addTab: function () {
            $(".navbar-custom-menu>ul>li.open").removeClass("open");
            var dataId = $(this).attr('data-menuid');
            if (dataId != "") {
                top.$.cookie('sysmenuid', dataId, { path: "/" });
            }
            var dataUrl = $(this).attr('href');
            var menuName = $.trim($(this).text());
            var flag = true;
            if (dataUrl == undefined || $.trim(dataUrl).length == 0) {
                return false;
            }
            $('.menuTab').each(function () {
                if ($(this).attr('data-menuid') == dataUrl) {
                    if (!$(this).hasClass('active')) {
                        $(this).addClass('active').siblings('.menuTab').removeClass('active');
                        $.fgtab.scrollToTab(this);
                        $('.mainContent .fg_iframe').each(function () {
                            if ($(this).attr('data-menuid') == dataUrl) {
                                $(this).show().siblings('.fg_iframe').hide();
                                return false;
                            }
                        });
                    }
                    flag = false;
                    return false;
                }
            });
            if (flag) {
                if ($(".menuTab").length >= 12) {
                    layeralert("为保证系统效率,只允许同时运行12个功能窗口,请关闭一些窗口后重试！", 0)
                    return false;
                }
                var str = '<a href="javascript:;" class="active menuTab" data-menuid="' + dataUrl + '">' + menuName + ' <i class="fa fa-remove"></i></a>';
                $('.menuTab').removeClass('active');
                var str1 = '<iframe class="fg_iframe" id="iframe' + dataId + '" name="iframe' + dataId + '"  width="100%" height="100%" src="' + dataUrl + '" frameborder="0" data-menuid="' + dataUrl + '" seamless></iframe>';
                $('.mainContent').find('iframe.fg_iframe').hide();
                $('.mainContent').append(str1);
                loading(true);
                $('.mainContent iframe:visible').load(function () {
                    loading(false);
                });
                $('.menuTabs .page-tabs-content').append(str);
                $.fgtab.scrollToTab($('.menuTab.active'));
            }
            return false;
        },
        scrollTabRight: function () {
            var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
            var tabOuterWidth = $.fgtab.calSumWidth($(".content-tabs").children().not(".menuTabs"));
            var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
            var scrollVal = 0;
            if ($(".page-tabs-content").width() < visibleWidth) {
                return false;
            } else {
                var tabElement = $(".menuTab:first");
                var offsetVal = 0;
                while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {
                    offsetVal += $(tabElement).outerWidth(true);
                    tabElement = $(tabElement).next();
                }
                offsetVal = 0;
                while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                    offsetVal += $(tabElement).outerWidth(true);
                    tabElement = $(tabElement).next();
                }
                scrollVal = $.fgtab.calSumWidth($(tabElement).prevAll());
                if (scrollVal > 0) {
                    $('.page-tabs-content').animate({
                        marginLeft: 0 - scrollVal + 'px'
                    }, "fast");
                }
            }
        },
        scrollTabLeft: function () {
            var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
            var tabOuterWidth = $.fgtab.calSumWidth($(".content-tabs").children().not(".menuTabs"));
            var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
            var scrollVal = 0;
            if ($(".page-tabs-content").width() < visibleWidth) {
                return false;
            } else {
                var tabElement = $(".menuTab:first");
                var offsetVal = 0;
                while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {
                    offsetVal += $(tabElement).outerWidth(true);
                    tabElement = $(tabElement).next();
                }
                offsetVal = 0;
                if ($.fgtab.calSumWidth($(tabElement).prevAll()) > visibleWidth) {
                    while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                        offsetVal += $(tabElement).outerWidth(true);
                        tabElement = $(tabElement).prev();
                    }
                    scrollVal = $.fgtab.calSumWidth($(tabElement).prevAll());
                }
            }
            $('.page-tabs-content').animate({
                marginLeft: 0 - scrollVal + 'px'
            }, "fast");
        },
        scrollToTab: function (element) {
            var marginLeftVal = $.fgtab.calSumWidth($(element).prevAll()), marginRightVal = $.fgtab.calSumWidth($(element).nextAll());
            var tabOuterWidth = $.fgtab.calSumWidth($(".content-tabs").children().not(".menuTabs"));
            var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
            var scrollVal = 0;
            if ($(".page-tabs-content").outerWidth() < visibleWidth) {
                scrollVal = 0;
            } else if (marginRightVal <= (visibleWidth - $(element).outerWidth(true) - $(element).next().outerWidth(true))) {
                if ((visibleWidth - $(element).next().outerWidth(true)) > marginRightVal) {
                    scrollVal = marginLeftVal;
                    var tabElement = element;
                    while ((scrollVal - $(tabElement).outerWidth()) > ($(".page-tabs-content").outerWidth() - visibleWidth)) {
                        scrollVal -= $(tabElement).prev().outerWidth();
                        tabElement = $(tabElement).prev();
                    }
                }
            } else if (marginLeftVal > (visibleWidth - $(element).outerWidth(true) - $(element).prev().outerWidth(true))) {
                scrollVal = marginLeftVal - $(element).prev().outerWidth(true);
            }
            $('.page-tabs-content').animate({
                marginLeft: 0 - scrollVal + 'px'
            }, "fast");
        },
        calSumWidth: function (element) {
            var width = 0;
            $(element).each(function () {
                width += $(this).outerWidth(true);
            });
            return width;
        },
        init: function () {
            $('.menuItem').on('click', $.fgtab.addTab);
            $('.menuTabs').on('click', '.menuTab i', $.fgtab.closeTab);
            $('.menuTabs').on('click', '.menuTab', $.fgtab.activeTab);
            $('.tabLeft').on('click', $.fgtab.scrollTabLeft);
            $('.tabRight').on('click', $.fgtab.scrollTabRight);
            $('.tabReload').on('click', $.fgtab.refreshTab);
            $('.tabCloseCurrent').on('click', function () {
                $('.page-tabs-content').find('.active i').trigger("click");
            });
            $('.tabCloseAll').on('click', function () {
                $('.page-tabs-content').children("[data-menuid]").find('.fa-remove').each(function () {
                    $('.fg_iframe[data-menuid="' + $(this).attr('data-menuid') + '"]').remove();
                    $(this).parents('a').remove();
                });
                $('.page-tabs-content').children("[data-menuid]:first").each(function () {
                    $('.fg_iframe[data-menuid="' + $(this).attr('data-menuid') + '"]').show();
                    $(this).addClass("active");
                });
                $('.page-tabs-content').css("margin-left", "0");
            });
            $('.tabCloseOther').on('click', $.fgtab.closeOtherTabs);
            $('.fullscreen').on('click', function () {
                if (!$(this).attr('fullscreen')) {
                    $(this).attr('fullscreen', 'true');
                    $.fgtab.requestFullScreen();
                } else {
                    $(this).removeAttr('fullscreen')
                    $.fgtab.exitFullscreen();
                }
            });
        }
    };

    $.fg = {
        load: function () {
            $("body").removeClass("hold-transition")
            $("#content-wrapper").find('.mainContent').height($(window).height() - 100);
            $(window).resize(function (e) {
                $("#content-wrapper").find('.mainContent').height($(window).height() - 100);
            });
            $(".sidebar-toggle").click(function () {
                if (!$("body").hasClass("sidebar-collapse")) {
                    $("body").addClass("sidebar-collapse");
                } else {
                    $("body").removeClass("sidebar-collapse");
                }
            })
            $(window).load(function () {
                $(window).load(function () {
                    window.setTimeout(function () {
                        $('#ajax-loader').fadeOut();
                        loading(false);
                    }, 300);
                });
            });
        },
        foreach: function (data, action) {
            if (action == null) return;
            var reval = new Array();
            $(data).each(function (i, v) {
                if (action(v)) {
                    reval.push(v);
                }
            })
            return reval;
        },
        loadMenu: function () {
            var data = menudata;
            var html = "";
            $.each(data, function (i) {
                var row = data[i];
                if (row.ParentMenuId == "0") {
                    if (i == 0) {
                        html += '<li class="treeview active">';
                    } else {
                        html += '<li class="treeview">';
                    }
                    html += '<a href="#">'
                    html += '<i class="' + row.MenuIcon + '"></i><span>' + row.MenuName + '</span><i class="fa fa-angle-left pull-right"></i>'
                    html += '</a>'
                    var chdNodes = $.fg.foreach(data, function (v) { return v.ParentMenuId == row.MenuId });
                    if (chdNodes.length > 0) {
                        html += '<ul class="treeview-menu">';
                        $.each(chdNodes, function (i) {
                            var subcurrent = chdNodes[i];
                            var subchdNodes = $.fg.foreach(data, function (v) { return v.ParentMenuId == subcurrent.MenuId });
                            html += '<li>';
                            if (subchdNodes.length > 0) {
                                html += '<a href="#"><i class="' + subcurrent.MenuIcon + '"></i>' + subcurrent.MenuName + '';
                                html += '<i class="fa fa-angle-left pull-right"></i></a>';
                                html += '<ul class="treeview-menu">';
                                $.each(subchdNodes, function (i) {
                                    var subsubcurrent = subchdNodes[i];
                                    var threechdNodes = $.fg.foreach(data, function (v) { return v.ParentMenuId == subsubcurrent.MenuId });
                                    html += '<li>';
                                    if (threechdNodes.length > 0)
                                    {
                                        html += '<a href="#"><i class="' + subsubcurrent.MenuIcon + '"></i>' + subsubcurrent.MenuName + '';
                                        html += '<i class="fa fa-angle-left pull-right"></i></a>';
                                        html += '<ul class="treeview-menu">';
                                        $.each(threechdNodes, function (i) {
                                            var subthreechdNodes = threechdNodes[i];
                                            //html += '<li><a class="menuItem" data-menuid="' + subthreechdNodes.MenuId + '" href="' + subthreechdNodes.MenuUrl + '"><i class="' + subthreechdNodes.MenuIcon + '"></i>' + subthreechdNodes.MenuName + '</a></li>';
                                            html += '<li><a class="menuItem" data-menuid="' + subthreechdNodes.MenuId + '" href="' + subthreechdNodes.MenuUrl + '">' + subthreechdNodes.MenuName + '</a></li>';
                                        });
                                        html += '</ul>';
                                     }
                                    else
                                    {
                                        html += '<li><a class="menuItem" data-menuid="' + subsubcurrent.MenuId + '" href="' + subsubcurrent.MenuUrl + '"><i class="' + subsubcurrent.MenuIcon + '"></i>' + subsubcurrent.MenuName + '</a></li>';
                                    }
                                    html += '<li>';
                                });
                                html += '</ul>';

                            } else {
                                html += '<a class="menuItem" data-menuid="' + subcurrent.MenuId + '" href="' + subcurrent.MenuUrl + '"><i class="' + subcurrent.MenuIcon + '"></i>' + subcurrent.MenuName + '</a>';
                            }
                            html += '</li>';
                        });
                        html += '</ul>';
                    }
                    html += '</li>'
                }
            });
            $("#sidebar-menu").append(html);
            $("#sidebar-menu li a").click(function () {
                var d = $(this), e = d.next();
                if (e.is(".treeview-menu") && e.is(":visible")) {
                    e.slideUp(500, function () {
                        e.removeClass("menu-open")
                    }),
                    e.parent("li").removeClass("active")
                } else if (e.is(".treeview-menu") && !e.is(":visible")) {
                    var f = d.parents("ul").first(),
                    g = f.find("ul:visible").slideUp(500);
                    g.removeClass("menu-open");
                    var h = d.parent("li");
                    e.slideDown(500, function () {
                        e.addClass("menu-open"),
                        f.find("li.active").removeClass("active"),
                        h.addClass("active");

                        var _height1 = $(window).height() - $("#sidebar-menu >li.active").position().top - 41;
                        var _height2 = $("#sidebar-menu li > ul.menu-open").height() + 10
                        if (_height2 > _height1) {
                            $("#sidebar-menu >li > ul.menu-open").css({
                                overflow: "auto",
                                height: _height1
                            })
                        }
                    })
                }
                e.is(".treeview-menu");
            });
        }
    };

    $(function () {
        $.fg.load();
        $.get({
            url: "/home/authdata",
            callback: function (data) {
                menudata = data.data.menudata;
                buttondata = data.data.buttondata;
            }
        });

        $.fg.loadMenu();
        $.fgtab.init();
        $("#exit").click(function () {
            layerconfirm("注：您确定要安全退出本次登录吗？", function (r) {
                if (r) {
                    window.location.href = "/login/logout";
                }
            });
        });
    });

})(jQuery);



(function (w) {
    function pager(Pram) {
        this.wrap = $("<div>", { "class": "page" });
        this.model = {
            prev: $('<a>', { 'class': 'prev', 'href': 'javascript:void(0);' }).text('<'),
            next: $('<a>', { 'class': 'next', 'href': 'javascript:void(0);' }).text('>'),
            toPage: '',
            omit: '<span class="omit">...</span>',
            pageNum: $('<a>', { 'href': 'javascript:void(0);' })
        };
        this.pram = $.extend({
            parent: null,
            Size: 20,
            Align: 'left',
            maxPageNum: 5
        }, Pram);
    }
    pager.prototype.control = function (i, g, callback) {
        var m = this.model, pram = this.pram,
            $wrap = this.wrap.clone(), $prev = m.prev.clone(), $next = m.next.clone(),
            $col = [$prev];
        var v = Math.floor(pram.maxPageNum / 2)
        this.creatPageNum = function () {
            var _isF = (i > pram.maxPageNum - v + 1 && i + v - pram.maxPageNum <= g),
                _isL = (g - v > i && g > pram.maxPageNum),
                f = _isF ? (i - v) : 1,
                l = _isL ? i - v + pram.maxPageNum : pram.maxPageNum + 1;
            if (g - f < pram.maxPageNum) {
                f = g - pram.maxPageNum;
                f = i - v > v ? (f == 0 ? 1 : f) : 1;
            }
            if (i + v < pram.maxPageNum) {
                l = g <= pram.maxPageNum ? g : pram.maxPageNum + 1;
            } else if (i - v + pram.maxPageNum >= g) {
                l = g;
            } else if (i - v + pram.maxPageNum <= g) {
                l = i - v + pram.maxPageNum;
            }
            if (f != 1) {
                if (_isF) {
                    $col.push(m.pageNum.clone().text('1').on('click', function () {
                        callback(1);
                    }));
                    $col.push(m.omit);
                }
            }
            for (var t = f; t <= l; t++) {
                (function (t) {
                    var _num = m.pageNum.clone();
                    if (t == i) {
                        _num.attr('class', 'on');
                    } else {
                        _num.one('click', function () {
                            callback(t);
                        });
                    }
                    $col.push(_num.text(t));
                })(t);
            }
            if (f != 1) {
                if ((g - 1 > pram.maxPageNum && i < i + v) && (g - i >= pram.maxPageNum - v) && (g > l)) {
                    $col.push(m.omit);
                    $col.push(m.pageNum.clone().text(g).on('click', function () {
                        callback(g);
                    }));
                }
            }
        };

        this.creatPageNum();
        $col.push(m.toPage);
        $col.push($next);
        $wrap.html($col).find('.toPage').one('click', function () {
            var _num = $(this).prev('span').find('input').val();
            if (_num > g || _num < 1) return false;
            if ($.isNumeric(_num)) {
                callback(parseInt(_num));
            }
        });
        $prev.one('click', function () {
            var f = i == 1 ? 1 : i - 1;
            if (i == 1) return false;
            callback(f);
        });
        $next.one('click', function () {
            var l = i == g ? g : i + 1;
            if (i == l) return false;
            callback(l);
        });
        return $wrap;
    };
    pager.prototype.init = function (filter, pageNum, pageCount, callback) {
        $("#pager").html(this.control(pageNum, pageCount, callback));
    }
    w.pager = pager;
})(window);
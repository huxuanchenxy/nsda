     (function (w) {
         var playerevent = {
             param: {
                 TotalPage: 1,
                 PageIndex: 1,
                 PageSize: 10,
             }
         };
         playerevent.Load = function (filter) {             $.ajax({
                 url: "/commondata/listevent",
                 type: "get",
                 dataType: "json",
                 data: filter,
                 success: function (json) {                     if (json.records>0) {
                         var _pager = new w.pager({
                             'parent': $('#pager')
                         });
                         playerevent.param.TotalPage = json.total;
                         _pager.init(filter, playerevent.param.PageIndex, playerevent.param.TotalPage, function (p) {
                             playerevent.param.PageIndex = p;
                             playerevent.Load(playerevent.param)
                         });                         var html = '';
                         $.each(json.Data, function (k, val) {
                             
                         });
                         html += '';
                         $("#ss").html(html)
                     } else {
                         $("#ss").html('');
                         $("#pager").html("")
                     }
                 }
             })
         };
         w.playerevent = playerevent
     })(window);
playerevent.Load(playerevent.param);
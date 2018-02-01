$('#test1').on('click', function () {
    var ii = layer.load();
    //此处用setTimeout演示ajax的回调
    setTimeout(function () {
        layer.close(ii);
    }, 1000);
});
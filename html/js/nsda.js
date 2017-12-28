//课程

 $(function(){
    tabs($("#tabs01 li"), $('#container01 .con'));
})

var tabs = function(tab, con){
    tab.mouseenter(function(){
        var indx = tab.index(this);
        tab.removeClass('active1');
        $(this).addClass('active1');
        con.hide();
        con.eq(indx).show();
    })
}






























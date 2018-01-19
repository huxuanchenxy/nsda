var images = $('.bg>li');
var fong = $('.fg>li');
var btn = $('.link-dot>li');
var leftBtn = $('.leftBtn');
var rightBtn = $('.rightBtn');
var lwidths = images.innerWidth();
console.log(lwidths)
images.css('left',lwidths+'px').eq(0).css('left',0);
fong.css('left',lwidths+'px').eq(0).css('left',0);
console.dir(leftBtn);
var now = next = 0;
var flag = true;
function change(type = 'r'){
    if(!flag){
        return;
    }
    flag = false;
    if(type == 'r'){
        next++;
        if(next>=images.length){
            next = 0;
        }
        images.eq(next).css('left',lwidths);
        images.eq(now).animate({'left':-lwidths},2000);
        fong.eq(next).css('left',-lwidths);
        fong.eq(now).animate({'left':lwidths},2000);
    }
    if(type == 'l'){
        next--;
        if(next<0){
            next = images.length - 1;
        }
        images.eq(next).css('left',-lwidths);
        images.eq(now).animate({'left':lwidths},2000);
        fong.eq(next).css('left',lwidths);
        fong.eq(now).animate({'left':-lwidths},2000);
    }
    images.eq(next).animate({'left':0},2000,function () {
        flag=true;
    });
    fong.eq(next).animate({'left':0},2000,function () {
        flag=true;
    });
    btn.eq(now).removeClass('active');
    btn.eq(next).addClass('active');
    now = next;
}
    var t = setInterval(function(){
        change();
    },2000);

    $('.banner').mouseenter(function(){
        clearInterval(t);
    })
        .mouseleave(function() {
            t = setInterval(function(){
                change();
            },2000);
        });

    btn.mouseenter(function(){
        var index = $(this).index();
        next = index - 1;
        change();
    })
    leftBtn.click(function(){
        change('l');
    })
    rightBtn.click(function(){
        change('r');
    });


    var widths = $(".test-list .cen").innerWidth();
    $(".test-list .cen").css('left',widths+'px').eq(0).css('left',0);
    var now=0;
    var next=0;
    var flag=true;
    function move(type='r') {
        if(!flag){
            return;
        }
        flag=false;
        if(type=='r'){
            next++;
            if(next>=$(".test-list .cen").length){
                next=0;
            }
            $(".test-list .cen").eq(next).css('left',widths);
            $(".test-list .cen").eq(now).animate({'left':-widths},1000);
        }
        if(type=='l'){
            next--;
            if(next<0){
                next=$(".test-list .cen").length-1;
            }
            $(".test-list .cen").eq(next).css('left',-widths);
            $(".test-list .cen").eq(now).animate({'left':widths},1000);
        }
        $(".test-list .cen").eq(next).animate({'left':0},1000,function () {
            flag=true;
        });

        $(".testmsg-dot>li").eq(now).removeClass('active2');
        $(".testmsg-dot>li").eq(next).addClass('active2');
        now=next;
    }
        var t=setInterval(move('r'),2000);
        $('.test-list').mouseenter(function () {
            clearInterval(t);
        }).mouseleave(function () {
            t=setInterval(move('r'),2000);
        })
        $('.testmsg-dot>li').mouseenter(function () {

            var index=$(this).index();
            if(index>next){
                next=index-1;
                move('r');
            }
            if(index<next){
                next=index+1;
                move('l');
            }
        })








        var cwidths = $(".course-list .cour").innerWidth();
        $(".course-list .cour").css('left',cwidths+'px').eq(0).css('left',0);
        var now=0;
        var next=0;
        var flag=true;
        function cove(type='r') {
            if(!flag){
                return;
            }
            flag=false;
            if(type=='r'){
                next++;
                if(next>=$(".course-list .cour").length){
                    next=0;
                }
                $(".course-list .cour").eq(next).css('left',cwidths);
                $(".course-list .cour").eq(now).animate({'left':-cwidths},1000);
            }
            if(type=='l'){
                next--;
                if(next<0){
                    next=$(".course-list .cour").length-1;
                }
                $(".course-list .cour").eq(next).css('left',-cwidths);
                $(".course-list .cour").eq(now).animate({'left':cwidths},1000);
            }
            $(".course-list .cour").eq(next).animate({'left':0},1000,function () {
                flag=true;
            });

            $(".course-dot>li").eq(now).removeClass('act');
            $(".course-dot>li").eq(next).addClass('act');
            now=next;
        }
            var t=setInterval(cove('r'),2000);
            $('.course-list').mouseenter(function () {
                clearInterval(t);
            }).mouseleave(function () {
                t=setInterval(cove('r'),2000);
            })
            $('.course-dot>li').mouseenter(function () {
                var index=$(this).index();
                if(index>next){
                    next=index-1;
                    cove('r');
                }
                if(index<next){
                    next=index+1;
                    cove('l');
                }
            })



        }}}
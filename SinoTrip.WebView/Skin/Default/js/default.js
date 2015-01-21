

jQuery.extend({
    XswitchBox: function (objs) {
        var obj = {
            thislist: '.dist_list_box',
            parentbox: '.switch_dist_list',
            contentbox: '.switch_dist_content'
        }
        obj = jQuery.extend(obj, objs);


        $(obj.parentbox).each(function () {
            var $this = $(this);
            var $thisbutton = $this.find(obj.thislist).find("a");
            $thisbutton.css({ width: (100 / $thisbutton.length) - 1 + "%" })
        })
        var $contentbox = $(obj.contentbox);
        var _cw = $contentbox.first().width();
        var _ch = $contentbox.first().height();

        var $list = $contentbox.find("a");
        $list.css({ position: 'absolute' });
        var $this = $list.first();
        var _w = $this.outerWidth();
        var _h = $this.outerHeight();
        var _in_h = $this.height();
        var position = $this.position();

        var imgFormat = function (number) {
            var imgs = new Array();
            if (number < 4) {
                for (var i = 0; i < number; i++)
                    imgs[i] = i;
                return imgs;
            }
            if (number == 4)
                return [0, 3];
            if (number == 5)
                return [2];
            if (number > 5)
                return null;

        }
        $contentbox.each(function () {
            var _thisleft = position.left;
            var _thistop = position.top;
            var $imglist = $(this).find("a");
            var formats = imgFormat($imglist.length);
            var isb = false;
            var left_h = _h;

            $imglist.each(function (i) {
                if (formats != null && formats.indexOf(i) > -1) {
                    isb = true;
                    var _thish = _in_h + _h;
                    $(this).height(_thish);
                    $(this).find(".contentbox").height(_thish);
                    var $paroductimg = $(this).find("img[class=productimg]").height(_thish);
                    $paroductimg.attr("src", $paroductimg.attr("src").replace("320-240", "640-480"));

                } else {

                    isb = false;
                }


                if (i > 0) {
                    if (!isb) {
                        if (_ch > _thistop + _h + left_h) {
                            _thistop = _thistop + _h;
                        } else {
                            _thistop = position.top;
                            _thisleft = _thisleft + _w;
                        }
                        left_h = _h;
                    } else {

                        if (_ch > _thistop + _h + left_h) {
                            _thistop = _thistop + left_h;
                        } else {
                            _thistop = position.top;
                            _thisleft = _thisleft + _w;
                        }
                        left_h = _h * 2;
                    }
                } else {

                    if (!isb) {
                        left_h = _h;
                    } else {
                        left_h = _h * 2;
                    }

                }


                $(this).css({ left: _thisleft, top: _thistop });

            })
        })

        $(obj.contentbox).find("a").hover(function () {
            var _h = $(this).height();
            $(this).find(".productname,.price").hide();
            $(this).find(".contenttitle").height(_h).animate({ bottom: '0%' }, 200);
            $(this).find(".contenttitle_bj").addClass("mousemove").animate({ height: _h }, 200)

        }, function () {
            var $a = $(this).find(".productname,.price");
            $(this).find(".contenttitle").animate({ bottom: '-100%' }, 200);
            $(this).find(".contenttitle_bj").animate({ height: 30 }, 200, function () { $(this).removeClass("mousemove"); $a.show() })


        })
        var switchBox = function ($contentbox, origindex, thisindex) {
            var $contentlist = $contentbox.find(obj.contentbox)
            var $origbox = $contentlist.eq(origindex);
            var $thisbox = $contentlist.eq(thisindex);

            var origs = {};
            var thiss = {};
            if (origindex > thisindex) {
                origs = { left: '-100%' };
                thiss = { left: '100%' };
            } else {

                origs = { left: '100%' };
                thiss = { left: '-100%' };

            }

            $origbox.animate(origs, 300, function () { $(this).hide() });
            $thisbox.css(thiss).show()
            $thisbox.animate({ left: '0%' }, 300);
        }
        var $default_dist_list = $(obj.thislist);
        $default_dist_list.find("a").hover(function () {
            var $parentbox = $(this).parent(obj.thislist)
            var thisIndex = $(this).index();
            var origIndex = $parentbox.data("origIndex");
            if (origIndex == null)
                origIndex = 0;
            if (thisIndex == origIndex)
                return;
            $parentbox.data("origIndex", thisIndex);

            var $thisList = $parentbox.find("a");
            $thisList.eq(thisIndex).addClass("mouseover");
            $thisList.eq(origIndex).removeClass("mouseover");
            switchBox($parentbox.parents(obj.parentbox), origIndex, thisIndex)
        });

    }
});


    myFocus.set({
        id: 'myFocus', //焦点图盒子ID
        pattern: 'mF_rapoo', //风格应用的名称		
       // width: 950, //设置图片区域宽度(像素)
        height: 280, //设置图片区域高度(像素)
        waiting:0,
        txtHeight: 'default'//文字层高度设置(像素),'default'为默认高度，0为隐藏
    });

    jQuery.XswitchBox();





    $("#left_search_tab").find(".leftbutton").bind("click", function () {

        if ($(this).hasClass("current"))
            return false;
        var _index = $(this).index();
        $("#left_search_tab").find(".leftbutton").removeClass("current");
        $(this).addClass("current")
        $("#left_search_tab_content").find(".tabContentbox").hide().eq(_index).show();


    });

    $(".default_top_min_advert").find("td").hover(function () { $(this).addClass("over") }, function () { $(this).removeClass("over") })


    function SubmitTourSearch() {
        var str = "";
        str = jQuery.XfilledUrl("toursearch", true);
        if (str == "")
            return;

        var searchurl = $("#toursearch").attr("action");

        searchurl = searchurl + "?" + str;


        $("#toursearch").attr("action", searchurl);
        $("#toursearch").submit();

    }
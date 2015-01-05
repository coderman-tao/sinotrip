function buyItem(id) {

    var url = "/Mall/Tour/Tourfill.aspx?tourid=" + id + "&planid=" + $("#planselect").val();
    document.priceForm.action = url;
    document.priceForm.submit();
}


function getPricenoteContent(priceobj, priceextend) {

    var _feecontain = priceobj.feecontain.replace(/\r\n|\n/g, "<br>");
    var _nofeecontain = priceobj.feenocontain.replace(/\r\n|\n/g, "<br>");
    var _hotelmeal = "";
    var _hotelmealArray = new Array(planday);

    for (var i = 0; i < priceextend.length; i++) {
        if (priceextend[i].priceid != priceobj.id)
            continue;

        if (_hotelmealArray[priceextend[i].dataint - 1] == null)
            _hotelmealArray[priceextend[i].dataint - 1] = {};

        if (priceextend[i].type == M.ROOT_TRIP_MEAL) {
            var mealarray = _hotelmealArray[priceextend[i].dataint - 1].mealarray;
            _hotelmealArray[priceextend[i].dataint - 1]["meal" + priceextend[i].dictionary] = 1;
            continue;
        }

        if (priceextend[i].type == M.TOUR_TRIP_HOTEL) {
            _hotelmealArray[priceextend[i].dataint - 1].hotelnote = priceextend[i].data;
        }
    }




    for (var i = 0; i < planday; i++) {
        _hotelmeal += "第" + (i + 1) + "天";
        var _meal = "";
        if (_hotelmealArray != null && _hotelmealArray[i] != null) {
            _meal += (_hotelmealArray[i]["meal" + M.TRIP_MEAL_BREADFAST] != null && _hotelmealArray[i]["meal" + M.TRIP_MEAL_BREADFAST] == 1 ? " 早餐 " : "");
            _meal += (_hotelmealArray[i]["meal" + M.TRIP_MEAL_BREADFAST] != null && _hotelmealArray[i]["meal" + M.TRIP_MEAL_LOUCH] == 1 ? " 中餐 " : "");
            _meal += (_hotelmealArray[i]["meal" + M.TRIP_MEAL_BREADFAST] != null && _hotelmealArray[i]["meal" + M.TRIP_MEAL_DINNER] == 1 ? " 晚餐 " : "");
        }
        _meal = (_meal == "" ? " 全不含 " : _meal);
        _hotelmeal += " <span class=\"signStr\">餐：</span>";
        _hotelmeal += _meal;
        if (_hotelmealArray != null && _hotelmealArray[i] != null && _hotelmealArray[i].hotelnote != null && _hotelmealArray[i].hotelnote != "")
            _hotelmeal += "  <span class=\"signStr\">住宿：</span> " + _hotelmealArray[i].hotelnote;
        _hotelmeal += "<br>";

    }
    var _str = "<div class=\"TabContent noKeep\" style=\"display:none\">";
    _str += "<div style=\"padding: 15px;\"><div class=\"signNote\">费用包含</div><div style=\"display: \">" + _feecontain + "</div></div>";
    _str += "<div style=\"padding: 15px;\"><div class=\"signNote\">费用不包含</div><div style=\"display: \">" + _nofeecontain + "</div></div>";
    _str += "<div style=\"padding: 15px;\"><div class=\"signNote\">行程食宿</div><div style=\"display: \">" + _hotelmeal + "</div></div></div>";
    return _str;
}
var edate = new Date();
function timer() {
    var ts = (edate - (new Date())); //计算剩余的毫秒数
    var dd = parseInt(ts / 1000 / 60 / 60 / 24, 10); //计算剩余的天数
    var hh = parseInt(ts / 1000 / 60 / 60 % 24, 10); //计算剩余的小时数
    var mm = parseInt(ts / 1000 / 60 % 60, 10); //计算剩余的分钟数
    var ss = parseInt(ts / 1000 % 60, 10); //计算剩余的秒数
    dd = checkTime(dd);
    hh = checkTime(hh);
    mm = checkTime(mm);
    ss = checkTime(ss);
    $("#tickets").html(dd + "天" + hh + "时" + mm + "分" + ss + "秒");

}
function checkTime(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}  

function getPlanPrcie() {
    var data = {};
    data.id = id;
    data.planid = planid;
    data.productid = tourid;
    var url = "/AJAX/Sale/sale.aspx?action=price";
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (response) {
            var json = response.toJson();
            if (json.success) {
                json = json.msg.toJson();
                var buybtn = $("#buybutton");
                var nobtn = $("#nobuybotton");
                var sdate = new Date(parseInt(json.starttime) * 1000);
                edate = new Date(parseInt(json.endtime) * 1000);
                var now = new Date();
                $("#timeline").html(dateFormat("Y年m月d日H:i", json.starttime))
                $("#market_price").html(json.rrp);
                //timer(edate, "tickets");
                if (edate > now) {
                    setInterval(timer, 1000);
                }
                $("#web_price").html(json.price);
                $("#startdate").html(dateFormat("Y年m月d日", json.startdate));
                //dateFormat("Y年m月d日", ulist[i].deadline) 
                if (json.last <= 0 || edate <= now) {
                    buybtn.hide();
                    nobtn.show();
                    return;
                }
                if (sdate > now) {
                    buybtn.hide();
                    nobtn.html("抢购尚未开始");
                    nobtn.show();
                }

            }
            else {
                $("#buybutton").hide();
                $("#nobuybotton").show();

            }
        }
    });
}


$(function () {
    myFocus.set({
        id: 'tourFocus', //焦点图盒子ID
        pattern: 'mF_51xflash', //风格应用的名称		
        width: 625, //设置图片区域宽度(像素)
        height: 390, //设置图片区域高度(像素)
        wrap: false,
        txtHeight: 0//文字层高度设置(像素),'default'为默认高度，0为隐藏
    });


    $("#pricenoteTap").find("a").live("click", function () {
        var _index = $(this).index();
        $("#pricenoteTap").find("a").removeClass("current");
        $(this).addClass("current");
        $("#pricenoteContent").find(".TabContent").hide().eq(_index).show();

    })


    $("#pricebox").hover(function () { $(this).removeClass("notebox") }, function () { })


    /*行程须知备注等*/
    tournotelist = $("#tournotelist").find("a")
    tournotelist.click(function () {
        tournotelist.removeClass("current");
        $(this).addClass("current");
        // setNoteTab();
    }).hover(function () { $(this).addClass("over") }, function () { $(this).removeClass("over") })

    $(window).scroll(function () {
        setNoteTab();

    });
    $(window).resize(function () {
        setNoteTab();
    });
    getPlanPrcie();
});
/*行程须知备注等*/
function setNoteTab() {
    var t = $("#tournoteTab");
    var offset = t.offset();
    var ws = $(window).scrollTop()
    if (ws > offset.top) {
        $("#tournotelist").addClass("isPosition").css({ left: offset.left, width: t.width() });
    } else {
        $("#tournotelist").removeClass("isPosition");
    }

    var _thisindex = 0;
    $("#tournotelistbox").find(".tournotelist").each(function (i) {
        if (ws > $(this).offset().top - 30)
            _thisindex = i;
    })
    $("#tournotelist").find("a").removeClass("current").eq(_thisindex).addClass("current")

}




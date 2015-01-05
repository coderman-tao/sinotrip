function buyItem(id) {

    var url = "/Mall/Tour/Tourfill.aspx?tourid=" + id + "&planid=" + $("#planselect").val();
    document.priceForm.action = url;
    document.priceForm.submit();
}
function setPriceObj(value, index) {
    var priceSelect = $("#priceBody").find("select");
    var price = 0;
    priceSelect.each(function (i) {
        var n = parseInt($(this).val());
        var thisprice = planitem.Pricelist[i];
        price += (n * thisprice.price);
    });
    $("#planprice").html(parseFloat(price).toFixed(2));
}

function getPriceOption(index) {
    var priceSelect = $("#priceBody").find("select");
    var thisSelect = priceSelect.eq(index);
    var thisprice = planitem.Pricelist[index];
    var seat = 0;
    var human = 0;
    priceSelect.each(function (i) {
        if (i == index)
            return true;

        var item = planitem.Pricelist[i];
        if (item.pricetype == M.PRICE_TYPE_VARY)
            return true;

        human += parseInt($(this).val());
        if (item.seat == 1)
            seat += parseInt($(this).val());
    })

    var value = 0;
    var planseat = parseInt(planitem.PlanItem.seat) - parseInt(planitem.PlanItem.buyseat)
    if (planseat > 9)
        planseat = 9;

    if (thisprice.pricetype == M.PRICE_TYPE_VARY)
        value = human;
    else
        value = planseat - seat; ;

    var options = thisSelect.find("option");

    if (options.length > value) {
        for (var i = options.length; i > value; i--) {
            options.eq(i).remove();
        }
    }

    if (options.length <= value) {
        for (var i = options.length; i <= value; i++) {
            thisSelect.append("<option value=\"" + i + "\">" + i + "</option>");
        }
    }

}

function setPlanPrice(value) {

    var planindex = planlist.Planlist.indexOf(function (item) { return (item.id == value) });
    if (planindex < 0)
        return;

    var _planitem = planlist.Planlist[planindex]

    getPlanPrice(_planitem.id, _planitem.productid);

    var thisDate = $("#calendar").data("thisDate");
    var newDate = dateFormat("Y/m", _planitem.starttime)
    if (new Date(thisDate).getTime() != new Date(newDate + "/1").getTime()) {
        setTourCalendar(newDate + "/1", null);
    } else {
        $("#calendar").find("a").removeClass("current");
        $("#calendar").find("a[planindex=" + planindex + "]").addClass("current");

    }
}

function getPricenoteContent(priceobj, priceextend) {

    var _feecontain = priceobj.feecontain.replace(/\r\n|\n/g, "<br>");
    var _nofeecontain = priceobj.feenocontain.replace(/\r\n|\n/g, "<br>");
    var _hotelmeal = "";
    var _hotelmealArray = new Array(planday);

    if (priceextend != null) {
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


function getPlanPrice(value, tourid) {
    var data = {};
    data.planid = value;
    data.tourid = tourid;
    var url = "/AJAX/Tour.ashx?action=planprice"
    jQuery.Xload();
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (response) {
            jQuery.XloadHide();
            var json = response.toJson();
            planitem = json.result
            //填充价格

            $("#market_price").html(parseFloat(planitem.PlanItem.market).toFixed(2))
            $("#web_price").html(parseFloat(planitem.PlanItem.aduitprice).toFixed(2))
            $("#starttime").html(dateFormat("Y年m月d日", planitem.PlanItem.starttime));
            $("#discountBox").html("");

            var priceTableStr = "";
            var pricenoteTapStr = "";
            var pricenotecontentStr = "";




            for (var i = 0; (i < planitem.CouponList.length && i < 2); i++) {
                var _cp = planitem.CouponList[i];
                var _cn = "惠";
                var _cc = "下单优惠"
                var _title = "";

                if (_cp.type == M.COUPON_TYPE_QUAN) {
                    _cn = "券";
                    if (parseInt(_cp.money) <= 0) {
                        _cc = "立减券";
                        _title = "可使用" + _cp.credit + "优惠券"
                    } else {
                        _cc = "满减券";
                        _title = "可使用满" + _cp.money + "减" + _cp.credit + "优惠券"
                    }
                }
                if (_cp.type == M.COUPON_TYPE_QUAN) {
                    _cn = "减";
                    if (parseInt(_cp.money) <= 0) {
                        _cc = "下单立减";
                        _title = "下单立减" + _cp.credit;
                    } else
                        _cc = "下单满减";
                    _title = "下单满" + _cp.money + "立减" + _cp.credit
                }
                if (_cp.type == M.COUPON_TYPE_MA) {
                    continue;
                    _cn = "惠";
                    _cc = "折扣码";
                }

                $("#discountBox").append("<span class=\"discountBox couponetitle\" title=\"" + _title + "\"><span class=\"discountname\">" + _cn + "</span>√</span>");
            }

            for (var i = 0; i < planitem.Pricelist.length; i++) {

                if (planitem.Pricelist[i].name.indexOf("保险") >= 0 && parseFloat(planitem.Pricelist[i].price) == 0)
                    $("#discountBox").append("<span class=\"discountBox\" title=\"免费赠送旅游人身意外险\"><span class=\"discountname\">保</span>√</span>");

                if (tourtraffic == M.TRA_TYPE_QC && (planitem.Pricelist[i].pricetype == M.PRICE_TYPE_ADULT || planitem.Pricelist[i].pricetype == M.PRICE_TYPE_CHILD)) {
                    pricenoteTapStr += "<a class=\"noKeep\" href=\"javascript:void(0)\">" + planitem.Pricelist[i].name + "</a>";
                    pricenotecontentStr += getPricenoteContent(planitem.Pricelist[i], planitem.Tourextends);


                }
                priceTableStr += "<tr index=\"" + i + "\" priceid=\"" + planitem.Pricelist[i].id + "\">"
                priceTableStr += "<td>" + planitem.Pricelist[i].name + "</td>";
                priceTableStr += "<td class=\"td_right\">" + parseFloat(planitem.Pricelist[i].price).toFixed(2) + "</td>"
                priceTableStr += "<td><select name=\"pu" + planitem.Pricelist[i].id + "\" onfocus=\"getPriceOption(" + i + ")\"  onchange=\"setPriceObj(this.value," + i + ")\" style=\"width:35px\"><option value=\"0\">0</option></select> </td></tr>";
            }



            if ($("#discountBox").html() == "")
                $("#discountBox").append("<span class=\"discountBox\"><span class=\"discountname\" title=\"优质服务\">优</span>√</span>");
            $("#priceBody").html(priceTableStr)

            if (pricenoteTapStr != "") {
                $("#pricenoteTap").find("a[class=noKeep]").remove();
                $("#pricenoteTap").append(pricenoteTapStr);


                $("#pricenoteContent").find(".noKeep").remove();

                $("#pricenoteContent").append(pricenotecontentStr);

            }


            $("#pricenoteTap").find("a:first").addClass("current");
            $("#pricenoteContent").find(".TabContent:first").show();

        }

    });

}

function getPlanList(id) {
    var data = {};
    data.id = id;
    var url = "/AJAX/Tour.ashx?action=planlist"
    jQuery.Xload();
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (response) {
            jQuery.XloadHide();
            var json = response.toJson();
            planlist = json.result
            var planStr = "";
            var getplan = false;
            var thistime;
            var thisvalue;
            for (var i = 0; i < planlist.Planlist.length; i++) {

                var seat = parseInt(planlist.Planlist[i].seat) - parseInt(planlist.Planlist[i].buyseat);
                if (seat > 9)
                    seat = ">9";
                if (seat <= 0)
                    continue;

                if (!getplan) {
                    getplan = true;

                    getPlanPrice(planlist.Planlist[i].id, planlist.Planlist[i].productid);
                    thisvalue = planlist.Planlist[i].id;
                    thistime = dateFormat("Y/m/d", planlist.Planlist[i].starttime);
                }
                var aduitStr = planlist.Planlist[i].aduitprice;
                var childrenStr = planlist.Planlist[i].childrenprice;
                if (planlist.Planlist[i].aduitprice > 0) {
                    aduitStr = "成人:" + aduitStr;
                    if (childrenStr > 0)
                        childrenStr = " 儿童:" + childrenStr;
                    else
                        childrenStr = "";
                    planStr += "<option value=\"" + planlist.Planlist[i].id + "\">" + dateFormat("Y年m月d日", planlist.Planlist[i].starttime) + " 余位" + seat + " " + aduitStr + childrenStr + "  </option>"
                }
            }

            if (planStr == "") {
                $("#buybutton").hide();
                $("#nobuybotton").show();
                $("#pricenoteTap").find("a:first").addClass("current");
                $("#pricenoteContent").find(".TabContent:first").show();
            }

            $("#planselect").html(planStr);
            setTourCalendar(thistime, null);
        }
    });

}



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




function setTourCalendar(theTime, data) {


    var thisTime = ((theTime) ? new Date(theTime) : new Date())
    var thisMonth = thisTime.getMonth();
    var thisYear = thisTime.getFullYear();
    $("#calendarTxt").html(thisYear + " 年 " + (thisMonth + 1) + " 月");
    $("#calendar").data("thisDate", thisYear + "/" + (thisMonth + 1) + "/1")
    var daynumber = solarDays(thisYear, thisMonth); //获取当前月的天数
    var weekIndex = new Date(thisYear + "/" + (thisMonth + 1) + "/1").getDay(); //当月1号是周几

    if (weekIndex == 7) //如果是星期天从索引0开始
        weekIndex = 0;

    $("#calendar").find(".calendarList").find("a").each(function (i) {
        $(this).removeClass("current");
        var _dayHTML = "";
        if (i < weekIndex) {
            _dayHTML = "<span class=\"day\">-</span>";
            $(this).attr("planindex", -1);
            $(this).attr("title", "");
        } else if (daynumber > i - weekIndex) {

            _dayHTML = "<span class=\"day\">" + (i - weekIndex + 1) + "</span>";
            var intTime = new Date(thisYear, thisMonth, i - weekIndex + 1).getTime() / 1000;

            // var planindex = planlist.Planlist.indexOf(function (item) { return (item.starttime == intTime) })
            var planindex = -1;
            for (var j = 0; j < planlist.Planlist.length; j++) {
                if (planlist.Planlist[j].starttime == intTime) {
                    planindex = j;
                    if (parseInt(planlist.Planlist[j].seat) - parseInt(planlist.Planlist[j].buyseat) > 0)
                        break;
                }
            }




            if (planindex >= 0) {
                var seat = parseInt(planlist.Planlist[planindex].seat) - parseInt(planlist.Planlist[planindex].buyseat);

                var plannone = "";
                if (seat == 0 || new Date(thisYear, thisMonth, i - weekIndex + 2).getTime() <= new Date().getTime()) {
                    plannone = " plannone";
                    $(this).attr("planindex", -1);
                } else {
                    $(this).attr("planindex", planindex);

                }
                $(this).attr("title", (dateFormat("Y年m月d日", planlist.Planlist[planindex].starttime) + " 发团\n" + dateFormat("Y年m月d日", planlist.Planlist[planindex].endtime) + " 回团"));
                if (planlist.Planlist[planindex].id == $("#planselect").val())
                    $(this).addClass("current");

                if (seat > 9 && plannone == "")
                    seat = "余位>9";
                else if (seat != 0 && plannone == "")
                    seat = "余位" + seat;
                else
                    seat = "售罄";
                _dayHTML += "<span class=\"dayseat\">" + seat + "</span>";
                _dayHTML += "<span class=\"dayprice" + plannone + "\">￥" + planlist.Planlist[planindex].aduitprice + "</span>";

            } else {
                $(this).attr("planindex", -1);
                $(this).attr("title", "");
            }

            var hs = "";
            //获取节日索引
            var holidayObj = getHoliday(thisYear, thisMonth + 1, i - weekIndex + 1);

            if (holidayObj.ch != null && holidayObj.ch >= 0)//阳历
                hs += "<span class=\"dayNote\">" + cHolidayStr[holidayObj.ch] + "</span>";

            if (holidayObj.lh != null && holidayObj.lh >= 0 && hs == "") //阴历                   
                hs += "<span class=\"dayNote\">" + lHolidayStr[holidayObj.lh] + "</span>";

            _dayHTML += hs;

        } else {
            _dayHTML = "<span class=\"day\">-</span>";
            $(this).attr("planindex", -1);
            $(this).attr("title", "")
        }

        $(this).html(_dayHTML);

    });



}

function changeMonth(type) {
    var theTime = new Date($("#calendar").data("thisDate"));

    var thisTime = ((theTime) ? new Date(theTime) : new Date());

    var thisMonth = thisTime.getMonth()

    if (type == 0)
        thisTime.setMonth(thisMonth - 1);
    else
        thisTime.setMonth(thisMonth + 1);

    var thisYear = thisTime.getFullYear();
    var thisMonth = thisTime.getMonth();
    if (thisMonth == 0) {
        thisMonth = 12
        thisYear = thisYear - 1
    }
    setTourCalendar(new Date(thisYear, thisMonth, 1), null);
}




    myFocus.set({
        id: 'tourFocus', //焦点图盒子ID
        pattern: 'mF_51xflash', //风格应用的名称		
        width: 625, //设置图片区域宽度(像素)
        height: 390, //设置图片区域高度(像素)
        wrap: false,
        txtHeight: 0//文字层高度设置(像素),'default'为默认高度，0为隐藏
    });


    getPlanList(tourid)
    $("#pricenoteTap").find("a").live("click", function () {
        var _index = $(this).index();
        $("#pricenoteTap").find("a").removeClass("current");
        $(this).addClass("current");
        $("#pricenoteContent").find(".TabContent").hide().eq(_index).show();

    })
    $("#calendar").find("a").hover(
            function () {
                var planindex = parseInt($(this).attr("planindex"))
                var boxindex = $(this).index();
                if (planindex >= 0) {
                    $(this).addClass("hover");
                    var celendarlist = $("#calendar").find("a");
                    for (var i = 1; i < planday; i++) {
                        if (i + boxindex < 42) {
                            celendarlist.eq(i + boxindex).addClass("timein");
                        }
                    }
                }
            },
            function () {

                $("#calendar").find("a").removeClass("timein");
                if (parseInt($(this).attr("planindex")) >= 0)
                    $(this).removeClass("hover");

            })
            .click(function () {
                var planindex = $(this).attr("planindex");

                if (parseInt(planindex) >= 0) {

                    var _planitem = planlist.Planlist[planindex];

                    if ($("#planselect").val() == _planitem.id)
                        return false;

                    $("#planselect").val(_planitem.id);
                    getPlanPrice(_planitem.id, _planitem.productid);
                    $("#calendar").find("a").removeClass("current");
                    $(this).addClass("current");
                    setShine($("#pricebox"), "notebox", 300, 5)

                }
                return false;
            })

    $("#pricebox").hover(function () { $(this).removeClass("notebox") }, function () { })

    $("#calendar").find(".calendarButton").hover(function () { $(this).addClass("hover") }, function () { $(this).removeClass("hover") })


    /*行程须知备注等*/
    var tournotelist = $("#tournotelist").find("a");
    tournotelist.click(function () {
        tournotelist.removeClass("current");
        $(this).addClass("current");
        // setNoteTab();
    }).hover(function () { $(this).addClass("over") }, function () { $(this).removeClass("over") })
    setNoteTab();
    $(window).scroll(function () {
        setNoteTab();

    });
    $(window).resize(function () {
        setNoteTab();
    })

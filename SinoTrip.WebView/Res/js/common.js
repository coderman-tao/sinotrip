var QM_JUSER; //J

var validateRegExp = {
    intege: "^-?[1-9]\\d*$", //整数
    intege1: "^[1-9]\\d*$", //正整数
    intege2: "^-[1-9]\\d*$", //负整数
    num: "^([+-]?)\\d*\\.?\\d+$", //数字
    num1: "^[1-9]\\d*|0$", //正数（正整数 + 0）
    num2: "^-[1-9]\\d*|0$", //负数（负整数 + 0）
    ascii: "^[\\x00-\\xFF]+$", //仅ACSII字符
    chinese: "^[\\u4e00-\\u9fa5]+$", //仅中文
    date: "^\\d{4}(\\-|\\/|\.)\\d{1,2}\\1\\d{1,2}$", //日期
    email: "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", //邮件
    letter: "^[A-Za-z]+$", //字母
    letter_l: "^[a-z]+$", //小写字母
    letter_u: "^[A-Z]+$", //大写字母
    mobile: "^0?(13|15|18|14)[0-9]{9}$", //手机
    notempty: "^\\S+$", //非空
    password: "^.*[A-Za-z0-9\\w_-]+.*$", //密码
    fullNumber: "^[0-9]+$", //数字
    tel: "^[0-9\-()（）]{7,18}$", //电话号码的函数(包括验证国内区号,国际区号,分机号)
    url: "^http[s]?:\\/\\/([\\w-]+\\.)+[\\w-]+([\\w-./?%&=]*)?$", //url
    username: "^[A-Za-z0-9_\\-\\u4e00-\\u9fa5]+$", //用户名
    Eusername: "^[A-Za-z0-9]+$", //非中文的用户名
    name: "^[\u0391-\uFFE5\w]+$",
    idcard: "^(\\d{18,18}|\\d{15,15}|\\d{17,17}x)$",
    idCard1: "^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$",
    idCard2: "^\d{6}(18|19|20)?\d{2}(0[1-9]|1[12])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$"
};


/*************************************身份证判断*******************************************************/

var Wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1];    // 加权因子   
var ValideCode = [1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2];            // 身份证验证位值.10代表X   

function IdCardValidate(idCard) {
    idCard = trim(idCard.replace(/ /g, ""));               //去掉字符串头尾空格                     
    if (idCard.length == 15) {
        return isValidityBrithBy15IdCard(idCard);       //进行15位身份证的验证    
    } else if (idCard.length == 18) {
        var a_idCard = idCard.split("");                // 得到身份证数组   
        if (isValidityBrithBy18IdCard(idCard) && isTrueValidateCodeBy18IdCard(a_idCard)) {   //进行18位身份证的基本验证和第18位的验证
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}
/**  
* 判断身份证号码为18位时最后的验证位是否正确  
* @param a_idCard 身份证号码数组  
* @return  
*/
function isTrueValidateCodeBy18IdCard(a_idCard) {
    var sum = 0;                             // 声明加权求和变量   
    if (a_idCard[17].toLowerCase() == 'x') {
        a_idCard[17] = 10;                    // 将最后位为x的验证码替换为10方便后续操作   
    }
    for (var i = 0; i < 17; i++) {
        sum += Wi[i] * a_idCard[i];            // 加权求和   
    }
    valCodePosition = sum % 11;                // 得到验证码所位置   
    if (a_idCard[17] == ValideCode[valCodePosition]) {
        return true;
    } else {
        return false;
    }
}
/**  
* 验证18位数身份证号码中的生日是否是有效生日  
* @param idCard 18位书身份证字符串  
* @return  
*/
function isValidityBrithBy18IdCard(idCard18) {
    var year = idCard18.substring(6, 10);
    var month = idCard18.substring(10, 12);
    var day = idCard18.substring(12, 14);
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    // 这里用getFullYear()获取年份，避免千年虫问题   
    if (temp_date.getFullYear() != parseFloat(year)
          || temp_date.getMonth() != parseFloat(month) - 1
          || temp_date.getDate() != parseFloat(day)) {
        return false;
    } else {
        return true;
    }
}
/**  
* 验证15位数身份证号码中的生日是否是有效生日  
* @param idCard15 15位书身份证字符串  
* @return  
*/
function isValidityBrithBy15IdCard(idCard15) {
    var year = idCard15.substring(6, 8);
    var month = idCard15.substring(8, 10);
    var day = idCard15.substring(10, 12);
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    // 对于老身份证中的你年龄则不需考虑千年虫问题而使用getYear()方法   
    if (temp_date.getYear() != parseFloat(year)
              || temp_date.getMonth() != parseFloat(month) - 1
              || temp_date.getDate() != parseFloat(day)) {
        return false;
    } else {
        return true;
    }
}

function getBirthdatByIdNo(iIdNo) {
    var tmpStr = "";
    var idDate = "";
    var tmpInt = 0;
    var strReturn = "";
    iIdNo = trim(iIdNo);
    if ((iIdNo.length != 15) && (iIdNo.length != 18)) {
        strReturn = "输入的身份证号位数错误";
        return strReturn;
    }
    if (iIdNo.length == 15) {
        tmpStr = iIdNo.substring(6, 12);
        tmpStr = "19" + tmpStr;
        tmpStr = tmpStr.substring(0, 4) + "-" + tmpStr.substring(4, 6) + "-" + tmpStr.substring(6)
        return tmpStr;
    }
    else {
        tmpStr = iIdNo.substring(6, 14);
        tmpStr = tmpStr.substring(0, 4) + "-" + tmpStr.substring(4, 6) + "-" + tmpStr.substring(6)
        return tmpStr;
    }
}

//去掉字符串头尾空格   
function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}




function CtoH(str) {
    var result = "";
    for (var i = 0; i < str.length; i++) {
        if (str.charCodeAt(i) == 12288) {
            result += String.fromCharCode(str.charCodeAt(i) - 12256);
            continue;
        }
        if (str.charCodeAt(i) > 65280 && str.charCodeAt(i) < 65375)
            result += String.fromCharCode(str.charCodeAt(i) - 65248);
        else result += String.fromCharCode(str.charCodeAt(i));
    }
    return result;
}
/*****************************************时间格式化*******************************************************/

function getLocalTime(tickets) {
    return new Date(parseInt(tickets) * 1000).toLocaleString().replace(/年|月/g, "-").replace(/日/g, " ");
}

/**
* 和PHP一样的时间戳格式化函数
* @param  {string} format    格式如“Y-m-d H:i:s”
* @param  {int}    timestamp 要格式化的时间 默认为当前时间
* @return {string}           格式化的时间字符串
*/
function dateFormat(format, timestamp) {
    var a, jsdate = ((timestamp) ? new Date(timestamp * 1000) : new Date());
    var pad = function (n, c) {
        if ((n = n + "").length < c) {
            return new Array(++c - n.length).join("0") + n;
        } else {
            return n;
        }
    };
    var txt_weekdays = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    var txt_ordin = { 1: "st", 2: "nd", 3: "rd", 21: "st", 22: "nd", 23: "rd", 31: "st" };
    var txt_months = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    var f = {
        // Day
        d: function () { return pad(f.j(), 2) },
        D: function () { return f.l().substr(0, 3) },
        j: function () { return jsdate.getDate() },
        l: function () { return txt_weekdays[f.w()] },
        N: function () { return f.w() + 1 },
        S: function () { return txt_ordin[f.j()] ? txt_ordin[f.j()] : 'th' },
        w: function () { return jsdate.getDay() },
        z: function () { return (jsdate - new Date(jsdate.getFullYear() + "/1/1")) / 864e5 >> 0 },

        // Week
        W: function () {
            var a = f.z(), b = 364 + f.L() - a;
            var nd2, nd = (new Date(jsdate.getFullYear() + "/1/1").getDay() || 7) - 1;
            if (b <= 2 && ((jsdate.getDay() || 7) - 1) <= 2 - b) {
                return 1;
            } else {
                if (a <= 2 && nd >= 4 && a >= (6 - nd)) {
                    nd2 = new Date(jsdate.getFullYear() - 1 + "/12/31");
                    return date("W", Math.round(nd2.getTime() / 1000));
                } else {
                    return (1 + (nd <= 3 ? ((a + nd) / 7) : (a - (7 - nd)) / 7) >> 0);
                }
            }
        },

        // Month
        F: function () { return txt_months[f.n()] },
        m: function () { return pad(f.n(), 2) },
        M: function () { return f.F().substr(0, 3) },
        n: function () { return jsdate.getMonth() + 1 },
        t: function () {
            var n;
            if ((n = jsdate.getMonth() + 1) == 2) {
                return 28 + f.L();
            } else {
                if (n & 1 && n < 8 || !(n & 1) && n > 7) {
                    return 31;
                } else {
                    return 30;
                }
            }
        },

        // Year
        L: function () { var y = f.Y(); return (!(y & 3) && (y % 1e2 || !(y % 4e2))) ? 1 : 0 },
        //o not supported yet
        Y: function () { return jsdate.getFullYear() },
        y: function () { return (jsdate.getFullYear() + "").slice(2) },

        // Time
        a: function () { return jsdate.getHours() > 11 ? "pm" : "am" },
        A: function () { return f.a().toUpperCase() },
        B: function () {
            // peter paul koch:
            var off = (jsdate.getTimezoneOffset() + 60) * 60;
            var theSeconds = (jsdate.getHours() * 3600) + (jsdate.getMinutes() * 60) + jsdate.getSeconds() + off;
            var beat = Math.floor(theSeconds / 86.4);
            if (beat > 1000) beat -= 1000;
            if (beat < 0) beat += 1000;
            if ((String(beat)).length == 1) beat = "00" + beat;
            if ((String(beat)).length == 2) beat = "0" + beat;
            return beat;
        },
        g: function () { return jsdate.getHours() % 12 || 12 },
        G: function () { return jsdate.getHours() },
        h: function () { return pad(f.g(), 2) },
        H: function () { return pad(jsdate.getHours(), 2) },
        i: function () { return pad(jsdate.getMinutes(), 2) },
        s: function () { return pad(jsdate.getSeconds(), 2) },
        //u not supported yet

        // Timezone
        //e not supported yet
        //I not supported yet
        O: function () {
            var t = pad(Math.abs(jsdate.getTimezoneOffset() / 60 * 100), 4);
            if (jsdate.getTimezoneOffset() > 0) t = "-" + t; else t = "+" + t;
            return t;
        },
        P: function () { var O = f.O(); return (O.substr(0, 3) + ":" + O.substr(3, 2)) },
        //T not supported yet
        //Z not supported yet

        // Full Date/Time
        c: function () { return f.Y() + "-" + f.m() + "-" + f.d() + "T" + f.h() + ":" + f.i() + ":" + f.s() + f.P() },
        //r not supported yet
        U: function () { return Math.round(jsdate.getTime() / 1000) }
    };

    return format.replace(/[\\]?([a-zA-Z])/g, function (t, s) {
        if (t != s) {
            // escaped
            ret = s;
        } else if (f[s]) {
            // a date function exists
            ret = f[s]();
        } else {
            // nothing special
            ret = s;
        }
        return ret;
    });
}

Date.prototype.DateAdd = function (strInterval, Number) {
    var dtTmp = this;
    switch (strInterval) {
        case 's': return new Date(Date.parse(dtTmp) + (1000 * Number));
        case 'n': return new Date(Date.parse(dtTmp) + (60000 * Number));
        case 'h': return new Date(Date.parse(dtTmp) + (3600000 * Number));
        case 'd': return new Date(Date.parse(dtTmp) + (86400000 * Number));
        case 'w': return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
        case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
}

//---------------------------------------------------  
// 日期格式化  
// 格式 YYYY/yyyy/YY/yy 表示年份  
// MM/M 月份  
// W/w 星期  
// dd/DD/d/D 日期  
// hh/HH/h/H 时间  
// mm/m 分钟  
// ss/SS/s/S 秒  
//---------------------------------------------------
Date.prototype.Format = function (formatStr) {
    var str = formatStr;
    var Week = ['日', '一', '二', '三', '四', '五', '六'];

    var _Month = this.getMonth() + 1;

    str = str.replace(/yyyy|YYYY/, this.getFullYear());
    str = str.replace(/yy|YY/, (this.getYear() % 100) > 9 ? (this.getYear() % 100).toString() : '0' + (this.getYear() % 100));

    str = str.replace(/MM/, _Month > 9 ? _Month.toString() : '0' + _Month);
    str = str.replace(/M/g, _Month);

    str = str.replace(/w|W/g, Week[this.getDay()]);

    str = str.replace(/dd|DD/, this.getDate() > 9 ? this.getDate().toString() : '0' + this.getDate());
    str = str.replace(/d|D/g, this.getDate());

    str = str.replace(/hh|HH/, this.getHours() > 9 ? this.getHours().toString() : '0' + this.getHours());
    str = str.replace(/h|H/g, this.getHours());
    str = str.replace(/mm/, _Month > 9 ? _Month.toString() : '0' + _Month);
    str = str.replace(/m/g, _Month);

    str = str.replace(/ss|SS/, this.getSeconds() > 9 ? this.getSeconds().toString() : '0' + this.getSeconds());
    str = str.replace(/s|S/g, this.getSeconds());

    return str;
}


//度假旅游订单状态TOUR_ORDER_STATUS
function getTourOrderStatus(type) {

    var _index = TOUR_ORDER_STATUS.indexOf(function (item) { return (item.id == type) });
    if (_index >= 0)
        return TOUR_ORDER_STATUS[_index].value;
    else
        return "已完成";

}
function getCardType(type) {


    var _index = CARD_TYPE.indexOf(function (item) { return (item.id == type) });
    if (_index >= 0)
        return CARD_TYPE[_index].value;
    else
        return "";

}
/*****************************************通用方法****************************************************/
function MM_preloadImages() {
    var d = document;
    if (d.images) {
        if (!d.MM_p) d.MM_p = new Array();
        var i, j = d.MM_p.length, a = MM_preloadImages.arguments;
        for (i = 0; i < a.length; i++)
            if (a[i].indexOf("#") != 0) {
                d.MM_p[j] = new Image;
                d.MM_p[j++].src = a[i];
            }
    }
}
MM_preloadImages("/res/images/Qload.gif", "/res/images/Qload2.gif");
/*遮罩层的方法*/
function getXmask($Xmask, $target, $obj) {
    $Xmask.css({
        "width": $(document).width(),
        "height": $(document).height()
    });

    if ($obj) {
        $Xmask.css({
            "background": $obj.color,
            "z-index": $target.css("z-index") - 1,
            "opacity": $obj.opacity,
            "left": 0,
            "top": 0
        });
    }

}
jQuery.fn.extend({
    RollTitle: function (opt, callback) {
        if (!opt) var opt = {};
        var _this = this;
        _this.timer = null;
        _this.lineH = _this.find("li:first").height();
        _this.line = opt.line ? parseInt(opt.line, 15) : parseInt(_this.height() / _this.lineH, 10);
        _this.speed = opt.speed ? parseInt(opt.speed, 10) : 3000, //卷动速度，数值越大，速度越慢（毫秒 
_this.timespan = opt.timespan ? parseInt(opt.timespan, 13) : 5000; //滚动的时间间隔（毫秒 
        if (_this.line == 0) this.line = 1;
        _this.upHeight = 0 - _this.line * _this.lineH;
        _this.scrollUp = function () {
            _this.animate({
                marginTop: _this.upHeight
            }, _this.speed, function () {
                for (i = 1; i <= _this.line; i++) {
                    _this.find("li:first").appendTo(_this);
                }
                _this.css({ marginTop: 0 });
            });
        }
        _this.hover(function () {
            clearInterval(_this.timer);
        }, function () {
            _this.timer = setInterval(function () { _this.scrollUp(); }, _this.timespan);
        }).mouseout();
    },
    Xtip: function (obj) {
        var _s = '<div id=\"Xtip\" class="Xtipbox"></div>';
        if ($("#Xtip").length < 1)
            $("body").append(_s);
        $("#Xtip").html($(this).attr("data"));
        var offset = $(this).offset();
        var objs = {
            pageX: offset.left,
            pageY: offset.top
        }
        $.extend(objs, obj);

        var _t = objs.pageY + $(this).height();

        var _l = objs.pageX;
        if (_l + $("#Xtip").width() > $(window).width()) {

            _l = _l - (_l + $("#Xtip").width() - $(window).width()) - 100;
        }
        $("#Xtip").show().css({ top: _t, left: _l });

    },
    Xhide: function () {
        var _id = $(this).attr("id");
        if (_id == null || _id == "")
            return;

        $("#xwindow_" + _id).hide();

        $("#mask_window" + _id).hide();
    },
    Xwindow: function (obj) {
        var objs = {
            mask: true, //遮罩层
            opacity: 0.5, //透明度
            color: "#999999", //遮罩层颜色
            title: "",
            close: true,
            isdestory: false,
        }
        $.extend(objs, obj);
        var thisobj = $(this);
        var _id = $(this).attr("id");
        if (_id == null || _id == "")
            return;
        var _winobj = $("#xwindow_" + _id);
        if (_winobj.length == 0) {

            var _s = '<div class="Xwindow" id="xwindow_' + _id + '"><div class="Xcontent"><div class="Xtitle"><span class="XtitleStr"></span><span class="iconbtn"><span class="close" title="关闭">&nbsp;</span></span></div><div class="XBoxContent"></div>';
            _s += '<div class="XBoxbutton"></div></div></div>';
            _winobj = $(_s);
            $(this).show();
            _winobj.find(".XBoxContent").width($(this).width()).height($(this).height()).append($(this));
            $("body").append(_winobj);
        }

        _winobj.find(".XtitleStr").html(objs.title);

        if (objs.close) {
            _winobj.find(".iconbtn").show().find(".close").one("click", function () {
                if (objs.isdestory) {
                    _winobj.remove();
                }
                thisobj.Xhide();
            })
        } else {
            winobj.find(".iconbtn").hide()
        }

        if (objs.button == null || objs.button.constructor != Array) {
            _winobj.find(".XBoxbutton").hide();
        }



        _winobj.show().Xcenter();

        if (objs.mask == true) {
            if ($("#mask_window" + _id).length == 0)
                $("body").append("<div id=\"mask_window" + _id + "\" class=\"XMaskBox\"></div>");
            $Xmask = $("#mask_window" + _id);
            getXmask($Xmask, _winobj, objs);
            $Xmask.show();
        }

    },
    Xcheck: function () {
        var ct = $(this).attr("checkType");
        if (ct == null || ct == "")
            return;
        var cts = ct.split("|");
        if (cts.length == 0)
            cts[0] = ct;

        var _str = $(this).attr("err");
        if (_str == "" || _str == null)
            _str = "非法的输入项";
        for (var i = 0; i < cts.length; i++) {
            if (cts[i] == null || cts[i] == "")
                continue;

            var regu = validateRegExp[cts[i]]
            var re = new RegExp(regu);
            var _v = $(this).val();
            if (_v == $(this).attr("title"))
                _v = "";
            if (_v == "" && cts[i] != "notempty")
                continue;

            if (cts[i] == "iscard") {
                if (!IdCardValidate(_v))
                    throw { message: _str, obj: $(this) }
            } else if (_v.search(re) == -1) {

                throw { message: _str, obj: $(this) }
            }
        }
        var _min = parseInt($(this).attr("minL"));
        var _max = parseInt($(this).attr("maxL"));

        if (_min != 0 && _min != null) {
            if (_v.length < _min) {
                $(this).focus();
                throw { message: _str, obj: $(this) }
            }

        }
        if (_max != 0 && _max != null) {
            if (_v > _max) {
                $(this).focus();
                throw { message: _str, obj: $(this) }
            }
        }

        return true;

    }, Xcenter: function () {
        var _w = $(window).width();
        var _h = $(window).height();
        var _x = ((_w - $(this).outerWidth()) / 2) + $(window).scrollLeft();
        var _y = ((_h - $(this).outerHeight()) / 2) + $(window).scrollTop();
        $(this).css({ "left": _x, "top": _y });
    }
});

jQuery.extend({

    Xerr: function (err) {
        var thisobj = err.obj.nextAll(".errstr");
        if (thisobj.length > 0) {
            thisobj.html(err.message)
            err.obj.one("focus", function () { thisobj.html("") })

        } else {
            err.obj.focus();
            jQuery.Xmsg(err.message)

        }
    },
    Xcheck: function (classname) {
        if (classname == null || classname.length == 0)
            return;
        if (classname.charAt(0) != ".")
            classname = "." + classname;


        $(classname).each(function () {
            $(this).Xcheck();
        })
        return true;
    },
    XfilledUrl: function (classname, nocheck) {
        if (classname == null || classname.length == 0)
            return;
        if (classname.charAt(0) != ".")
            classname = "." + classname;

        var _data = "";
        $(classname).each(function () {

            var _name = $(this).attr("name");
            if (_name == "" || _name == null)
                return;
            if (!nocheck)
                $(this).Xcheck();
            var _type = $(this).attr("type");
            if ((_type == "radio" || _type == "checkbox") && !$(this).attr("checked"))
                return;

            if (parseInt($(this).val()) != 0 && !!$(this).val() && $(this).val() != $(this).attr("title"))
                _data += "&" + _name + "=" + $(this).val();

        });
        if (!!_data) {
            _data = _data.substr(1);
        }
        return _data;
    },
    Xfilled: function (classname, nocheck) {
        if (classname == null || classname.length == 0)
            return;
        if (classname.charAt(0) != ".")
            classname = "." + classname;

        var _data = {};
        $(classname).each(function () {
            var _name = $(this).attr("name");
            if (_name == "" || _name == null)
                return;
            if (!nocheck)
                $(this).Xcheck();
            var val = $(this).val();

            if (val == $(this).attr("title"))
                val = "";
            var _type = $(this).attr("type");
            if ((_type == "radio" || _type == "checkbox") && !$(this).attr("checked"))
                return;
            if (_data[_name] == null) {
                _data[_name] = val;
            } else {
                if (_data[_name].constructor == Array) {
                    _data[_name].push(val);
                } else {
                    var _array = new Array();
                    _array.push(_data[_name]);
                    _array.push(val);
                    _data[_name] = _array;
                }
            }
        })
        return _data;
    },
    XmsgHide: function () {
        $("#XmsgdBox").hide();
        $("#mask_msg").hide();
    },
    Xmsg: function (obj) {
        var objs = {
            mask: true, //遮罩层
            opacity: 0.5, //透明度
            color: "#999999", //遮罩层颜色
            title: "提示信息",
            msg: "确认要进行此操作吗",
            type: "warn"
        }
        if (typeof ("") == typeof (obj))
            objs.msg = obj;
        else
            $.extend(objs, obj);
        var _s = '<div class="XmsgdBox" id="XmsgdBox"><div class="Xcontent"><div class="XBoxContent warnIcon"><div class="XBoxTitle">sdf</div>';
        _s += '<div class="XBoxMsg">sdf</div></div><div class="XBoxbutton"></div>';
        _s += '</div></div>';

        if ($("#XmsgdBox").length < 1)
            $("body").append(_s);

        var Xcontent = $("#XmsgdBox").find(".Xcontent");
        var XBoxContent = $("#XmsgdBox").find(".XBoxContent");
        var XBoxTitle = $("#XmsgdBox").find(".XBoxTitle");

        var XBoxMsg = $("#XmsgdBox").find(".XBoxMsg");
        var XBoxbutton = $("#XmsgdBox").find(".XBoxbutton");


        XBoxTitle.html(objs.title);
        XBoxMsg.html(objs.msg);

        var button = $('<span class="Xbutton Xconfirm" >确认</span>')

        if (objs.button == null) {
            XBoxbutton.html(button);
            button.one("click", function () { jQuery.XmsgHide() })
        } else {
            if (objs.button.constructor == Array) {

                alert("数组")

            } else if (typeof (objs.button) == "object") {
                button.html(obj.button.name);
                XBoxbutton.html(button);
                button.one("click", function () { obj.button.btn(); jQuery.XmsgHide() })
            } else if (typeof (objs.button) == "function") {
                XBoxbutton.html(button);
                button.one("click", function () { objs.button(), jQuery.XmsgHide() })
            } else if (typeof (objs.button) == typeof ("")) {
                XBoxbutton.html("");
            }

        }
        $("#XmsgdBox").show().Xcenter();

        if (objs.mask == true) {
            if ($("#mask_msg").length == 0)
                $("body").append("<div id=\"mask_msg\" class=\"XMaskBox\"></div>");
            $Xmask = $("#mask_msg");

            getXmask($Xmask, $("#XmsgdBox"), objs);
            $Xmask.show();
        }

    },
    Xload: function (obj) {
        var objs = {
            mask: true, //遮罩层
            opacity: 0.5, //透明度
            color: "#999999", //遮罩层颜色
            msg: "...努力加载中..."
        }

        if (typeof ("") == typeof (obj))
            objs.msg = obj;
        else
            $.extend(objs, obj);

        var _s = '<div class="XloadBox" id="XloadBox"><div class=\"Xloadicon\"><div class=\"XloadiconMask\"></div></div><div class="XloadText"></div></div>';

        if ($("#XloadBox").length < 1)
            $("body").append(_s);

        var $Xload = $("#XloadBox");

        $Xload.find(".XloadText").html(objs.msg);

        $Xload.show().Xcenter();
        $("#XloadBox").data("show", true);
        var xding = function () {
            $Xload.find(".XloadiconMask").show().slideUp(1000, function () {
                if ($("#XloadBox").data("show") == true)
                    xding();
            });
        }
        xding();

        if (objs.mask == true) {
            if ($("#mask_load").length == 0)
                $("body").append("<div id=\"mask_load\" class=\"XMaskBox\"></div>");
            $Xmask = $("#mask_load");

            getXmask($Xmask, $Xload, objs);

            $Xmask.show();
        }
    },
    XloadHide: function () {
        $("#XloadBox").hide();
        $("#XloadBox").data("show", false);
        $("#mask_load").hide();
    }
});
/*****************************************通用方法****************************************************/




///设置DOM的闪动
function setShine(domobj, classname, sleep, number) {
    var _number = number;

    setTimeout(function () {
        domobj.addClass(classname);
        setTimeout(function () {
            domobj.removeClass(classname)
            _number--;
            if (_number > 0)
                setShine(domobj, classname, sleep, _number)
            else
                domobj.addClass(classname);

        }, sleep);
    }, sleep)
}



///根据价格公式计算价格
function getCountPrice(ptype, pvalue, type, value, cost, webprice) {
    var ttype = 0;
    var tvalue = 0;

    if (type != null && parseInt(type) != 0 && parseInt(type) != M.COUNTPRICE_TYPE_NONE) {
        ttype = type;
        tvalue = value;
    } else {
        ttype = ptype;
        tvalue = pvalue;
    }
    if (ttype != 0) {
        if (ttype == M.COUNTPRICE_TYPE_VAL)
            webprice = cost + tvalue;
        if (ttype == M.COUNTPRICE_TYPE_PER)
            webprice = cost + (cost * (tvalue / 100));
    }
    return webprice;
}


/*****************************************通用方法****************************************************/
function RndNum(n) {
    var rnd = "";
    for (var i = 0; i < n; i++)
        rnd += Math.floor(Math.random() * 10);
    return rnd;
}
///字符串格式化
String.prototype.format = function (args) {
    if (arguments.length > 0) {
        var result = this;
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                var reg = new RegExp("({" + key + "})", "g");
                result = result.replace(reg, args[key]);
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] == undefined) {
                    return "";
                }
                else {
                    var reg = new RegExp("({[" + i + "]})", "g");
                    result = result.replace(reg, arguments[i]);

                }
            }
        }
        return result;
    }
    else {
        return this;
    }
}
///对像数组indexOf
Array.prototype.indexOf = function (item, start, end, error) {
    if (!start || start < 0)
        start = 0;
    if (!end || end < start || end >= this.length)
        end = this.length - 1;
    for (var i = start; i <= end; i++) {
        if (typeof (item) == typeof (this.reverse)) {
            if (!!item(this[i], i, this))
                return i;

            continue;
        }

        if (this[i] == item)
            return i;
    }
    if (isNaN(error))
        error = -1;
    return error;
};

String.prototype.toJson = function () {
    return eval("(" + this + ")");
}


/*********************************************全局用户相关***************************************/


//var _cardtype = '<option value="">请选择</option><option value="' + M.CARD_TYPE_IDCARD + '">身份证</option>'

//_cardtype += '<option value="' + M.CARD_TYPE_OFFICER + '">军官证</option>'

//_cardtype += '<option value="' + M.CARD_TYPE_PASSPORT + '">护照</option>'

//_cardtype += '<option value="' + M.CARD_TYPE_HVPS + '">回乡证</option>'
//_cardtype += '<option value="' + M.CARD_TYPE_OTHER + '">其它证件</option>'
$(function () {
    $(".isNumber").keyup(function () {

        var val = $(this).val();
        if (!val.match(/^[\+\-]?\d*?\.?\d*?$/))
            $(this).val("");
    })
    $("input[type=text]").each(function () {
        var _title = $(this).attr("title")
        if (_title != "" && _title != null) {
            if ($(this).val() == "") {
                $(this).val(_title);
                $(this).addClass("titleStyle")
            }
            $(this).bind("focus", function () {
                if (_title == $(this).val()) {
                    $(this).val("");
                    $(this).removeClass("titleStyle")
                }
            }).bind("blur", function () {
                if ($(this).val() == _title || $(this).val() == "") {
                    $(this).val(_title).addClass("titleStyle")
                } else {
                    $(this).removeClass("titleStyle")
                }

            });

        }
    })

    $("input[type=text],input[type=password]").live("focus", function () {
        $(this).addClass("over")

    }).live("blur", function () {
        $(this).removeClass("over")

    })

})

///倒计时
function Timeing(seconds, str, str1, buttonObj) {

    var s = seconds;


    var ts = function () {
        if (s > 0) {
            setTimeout(function () {
                buttonObj.html(str.format(s));
                ts();
            }, 1000)
            s--;
        } else {
            buttonObj.html(str1);
            buttonObj.attr("wait", 0)
            buttonObj.removeClass("disabled")
        }
    }

    ts();
}
//发送短信验证码
function sendMobileCode(classname, exists, button) {

    var buttonObj = $(button)

    if (parseInt(buttonObj.attr("wait")) > 0)
        return;

    var data = {};
    try {
        data = jQuery.Xfilled(classname);
    } catch (e) {

        jQuery.Xerr(e);
        return false;
    }
    data.exists = exists; //当手机号码不存在时发送验证码,val = 0

    var url = "/AJAX/User.ashx?action=SendCode"
    jQuery.Xload();
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (response) {
            jQuery.XloadHide();
            var json = response.toJson();

            if (json.success) {
                var status = parseInt(json.result);

                if (status == exists) {
                    jQuery.Xmsg("发送成功")
                    buttonObj.attr("wait", 1)
                    buttonObj.addClass("disabled");
                    Timeing(180, '等待 {0} 秒', '获取验证码', buttonObj)
                }
                else {
                    jQuery.Xmsg("已存在相同的手机号码，建议通过密码找回功能找回该帐号")
                }


            } else {
                jQuery.Xmsg(json.msg)

            }

        }

    });

}



function CheckUser(obj, exists, str) {
    var thisobj = $(obj)
    var data = {};
    try {
        thisobj.Xcheck();
    } catch (e) {
        jQuery.Xerr(e);
        return false;
    }

    data[thisobj.attr("name")] = thisobj.val();
    data.exists = exists;
    var url = "/AJAX/User.ashx?action=CheckUser";
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (response) {
            var json = response.toJson();
            if (json.success) {
            } else {
                jQuery.Xerr({ obj: thisobj, message: str });
            }
        }
    });
}




/*用户登录-注册-密码找回*/
function resetPass(classname) {

    var data = {};
    try {
        data = jQuery.Xfilled(classname);
        var p;
        var passobj = $("." + classname + "[type=password]");
        passobj.each(function () {
            if (p == null)
                p = $(this).val();
            if (p != $(this).val()) {
                p = null;
                return false;
            }
        });
        if (p == null) {
            jQuery.Xerr({ obj: passobj, message: "两次密码输入不相同!" });
            return;
        }

    } catch (e) {
        jQuery.Xerr(e);
        return false;
    }
    var url = "/AJAX/User.ashx?action=ResetPW"
    jQuery.Xload();
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (response) {
            jQuery.XloadHide();
            var json = response.toJson();
            if (json.success) {
                jQuery.Xmsg({ msg: "密码重置成功！返回登录页进行登录！", button: function () { window.location.href = "login.aspx" } });
            } else {
                jQuery.Xmsg(json.msg)

            }


        }

    });
}
///注册
function ajaxReg(classname, isUnite) {
    var data = {};
    try {
        data = jQuery.Xfilled(classname);
    } catch (e) {
        jQuery.Xerr(e);
        return false;
    }

    if (isUnite) {
        $.each($("#BindUniteBox").data("UniteData"), function (key, value) {
            data[key] = value
        });
    }
    var url = "/AJAX/User.ashx?action=SimpleReg"
    jQuery.Xload();
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (response) {
            jQuery.XloadHide();
            var json = response.toJson();

            if (json.success) {
                if (isUnite)
                    $("#BindUniteBox").Xhide();
                else
                    $("#reglogin").Xhide();

                if (typeof (LoginReturn) == "function") {
                    LoginReturn(json);
                }
            } else {
                jQuery.Xmsg(json.msg)

            }


        }

    });
}

///登录
function ajaxLogin(classname) {
    var data = {};
    try {
        data = jQuery.Xfilled(classname);
    } catch (e) {
        jQuery.Xerr(e);
        return false;
    }

    var url = "/AJAX/User.ashx?action=Login"
    jQuery.Xload();
    jQuery.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function (response) {
            jQuery.XloadHide();
            var json = response.toJson();

            if (json.success) {
                $("#reglogin").Xhide();
                jQuery.Xmsg("登录成功，您可以进行后续操作");

                if (typeof (LoginReturn) == "function") {
                    LoginReturn(json);
                }
            } else {
                jQuery.Xmsg("登录失败！，请重新操作")

            }


        }

    });
}

var isPageLogin = false;
var isQ = true;
function showlogin() {
    $("#reglogin").Xwindow({ title: '您尚未登录' });
    try {
        if (isQ) {
            QcLoad("qqLoginBtn");
            isQ = false;
        }

    } catch (e) {

        return false;
    }

}

function QcLoad(btnid) {

    QcIsload = true
    ///浮动登录注册窗体
    QC.Login({
        //btnId：插入按钮的节点id，必选  
        btnId: btnid,
        //用户需要确认的scope授权项，可选，默认all  
        scope: "all",
        //按钮尺寸，可用值[A_XL| A_L| A_M| A_S|  B_M| B_S| C_S]，可选，默认B_S  
        size: "B_M",
        isbtn: true
    }, function (reqData, opts) {//登录成功            
        //退出 '<span><a href="javascript:QC.Login.signOut();">退出</a></span>'  
        if (QC.Login.check()) {//如果已登录  
            $("#reglogin").Xhide();
            QC.Login.getMe(function (openId, accessToken) {
                checkQL(reqData.figureurl, QC.String.escHTML(reqData.nickname), openId, accessToken);
            });
        }
    }, function (opts) {//注销成功  

        $("#BindUniteBox").Xhide();
    }

);
}
function showBindUnite(figure, nickname, obj) {
    $("#BindUniteBox").Xwindow({ title: '帐号绑定' });
    $("#BindUnitMsg").html(nickname + "，只差一步，完成登录设置<a href='javascript:void()' onclick='" + obj.btn + "' > 退出</a>");
    $("#BindUnitMsg").css('background', 'url(' + figure + ') no-repeat 10px center');
    $("#BindUniteBox").data("UniteData", obj);

}




//绑定用户
function BindUnite(classname) {
    var url = "/AJAX/User.ashx?action=BindUnite";

    var data = {};
    try {
        data = jQuery.Xfilled(classname);
    } catch (e) {
        jQuery.Xerr(e);
        return false;
    }
    $.each($("#BindUniteBox").data("UniteData"), function (key, value) {
        data[key] = value
    });

    jQuery.Xload();
    jQuery.ajax({
        url: url,
        type: "POST",
        data: data,
        success: function (response) {
            jQuery.XloadHide();
            var json = response.toJson();
            if (json.success) {
                $("#BindUniteBox").Xhide();
                if (typeof (LoginReturn) == "function") {
                    LoginReturn(json);
                }
            } else {

                jQuery.Xmsg("绑定失败！，用户名或密码错误")
            }
        }

    });



}

///qq联合登录
function checkQL(figure, nickname, openId, accessToken) {
    //alert(figure + " + " + nickname + " + " + openId + " + " + accessToken)
    var url = "/AJAX/User.ashx?action=UniteLogin"
    var data = {};
    data.qid = openId;
    data.qtoken = accessToken;
    jQuery.Xload();
    jQuery.ajax({
        url: url,
        type: "POST",
        data: data,
        success: function (response) {
            jQuery.XloadHide();
            var json = response.toJson();
            if (json.success) {
                $("#reglogin").Xhide();
                //jQuery.Xmsg("QQ联合登录成功，您可以进行后续操作")
                if (typeof (LoginReturn) == "function") {
                    LoginReturn(json);
                }
            } else {
                showBindUnite(figure, nickname, { qid: openId, qtoken: accessToken, btn: "QC.Login.signOut()" });

            }
        }

    });

}


/////机票
$(function () {
    $("#f_select_tab").find(".f_select_tabitem").bind("click", function () {
        $("#f_select_tab").find(".f_select_tabitem").removeClass("current");
        $(this).addClass("current");
        $("#isDomestic").val($(this).attr("data"));
        $("#DomesticFlightCity").hide();
        $("#inFlightCity").hide();
    })

    $(".XoverTip").live("mouseover", function () {
        $(this).Xtip();
    }).live("mouseout", function () {
        $("#Xtip").hide();
    }
    )


})


function searchFlightPage(classname) {

    var _data = "";


    var flighttype = $("#searchFlightPageBox").find("input[name=flighttype]:checked").val();

    var isDomestic = $("#searchFlightPageBox").find("input[name=isDomestic]").val();

    _data += "&flighttype=" + flighttype;
    _data += "&isDomestic=" + isDomestic;

    var $homecity = $("#searchFlightPageBox").find("input[name=homecity]");
    if ($homecity.val() == null || $homecity.val() == "" || $homecity.attr("title") == $homecity.val()) {
        $homecity.focus();
        return false;
    }

    var homecity = $homecity.attr("data").split("|");
    _data += "&homename=" + homecity[0];
    _data += "&homecity=" + homecity[1];

    var $destcity = $("#searchFlightPageBox").find("input[name=destcity]");
    if ($destcity.val() == null || $destcity.val() == "" || $destcity.attr("title") == $destcity.val() || $destcity.val() == $homecity.val()) {
        $destcity.focus();
        return false;
    }



    var destcity = $destcity.attr("data").split("|");
    _data += "&destname=" + destcity[0];
    _data += "&destcity=" + destcity[1];

    var $staredata = $("#searchFlightPageBox").find("input[name=staredata]");
    if ($staredata.val() == null || $staredata.val() == "" || $staredata.attr("title") == $staredata.val()) {
        jQuery.Xmsg("请选择出发时间!");
        return false;
    }


    var $backdate = $("#searchFlightPageBox").find("input[name=backdate]");
    if (flighttype == "1" && ($backdate.val() == null || $backdate.val() == "" || $backdate.attr("title") == $backdate.val())) {
        jQuery.Xmsg("请选择回程时间!");
        return false;
    }

    var staredata = $staredata.val();
    var backdate = $backdate.val();

    _data += "&staredata=" + staredata;
    _data += "&backdate=" + backdate;


    if (flighttype == "1" && new Date(backdate) <= new Date(staredata)) {
        jQuery.Xmsg("回程时间必须大于出发时间!");
        return false;
    }

    if (!!_data) {
        _data = _data.substr(1);
        var url = "/Mall/Flights/SearchResult.aspx?" + _data;
        document.flightSearchForm.action = url;
        document.flightSearchForm.submit();
    }


}

function ladingLand(citycode, landlist) {

    _landboxlist = $("#landboxlist").find(".landboxs").html("加载中...")
    data = {};
    data.citycode = citycode;
    var url = "/AJAX/HotleAjax.ashx?action=getAreaInfo";
    jQuery.ajax({
        url: url,
        type: "POST",
        data: data,
        success: function (response) {
            jQuery.XloadHide();
            var json = response.toJson();

            var lands = ["BUSINESS", "HOSPITAL", "PAVILIONHALL", "SCENICSPOTS", "SUBWAY", "UNIVERSITY", "TRAINAIR"];



            for (var i = 0; i < lands.length; i++) {
                var astr = "";
                var index = json.indexOf(function (item) { return (lands[i] == item.AreaCode) });
                if (index < 0) {
                    _landboxlist.eq(i).html(astr);
                    continue;
                }
                for (var j = 0; j < json[index].NewLandMarkInfos.length; j++) {
                    for (var k = 0; k < json[index].NewLandMarkInfos[j].LandMarkChildInfos.length; k++) {

                        astr += ("<a target=\"_blank\" href=\"/Mall/Hotel/Hotellist.aspx?inDate=" + $("#inDate").val() + "&outDate=" + $("#outDate").val() + "&citycode=" + citycode + "&landname=" + json[index].NewLandMarkInfos[j].LandMarkChildInfos[k].LandMarkChildName + "\"><span class=\"mapicon\"></span>" + json[index].NewLandMarkInfos[j].LandMarkChildInfos[k].LandMarkChildName + "</a>");
                    }
                }
                _landboxlist.eq(i).html(astr);
            }




        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            jQuery.XmsgHide();
            var _status = XMLHttpRequest.status;

        }
    });


}


function showHotelCity(isDomestic, thisdom, inputhidden, landlist) {
    var $this = $(thisdom);
    var _offset = $this.offset();

    var $citybox;

    isDomestic = (isDomestic == null || isDomestic == "0") ? 0 : 1;

    $citybox = (isDomestic == 0) ? $('#inHotelCity') : $('#inHotelCity');


    $citybox.show();
    $citybox.css({ "top": _offset.top + $this.outerHeight(), "left": _offset.left });
    $citybox.data("inputdemo", $this)

    var isSet = $citybox.data("isSet");
    if (!isSet || isSet == null) {
        $citybox.hover(function () {
            $(this).show();
        }, function () {
            $(this).hide();
            $this.blur();
        })
        $citybox.find(".close").click(function () {
            $citybox.hide();
            $this.blur();
        })
        var _tabs = $citybox.find(".h_citytabList").find("a");

        var _cityboxStr = "";
        var _data = (isDomestic == 0) ? hotleCity : hotleCity;

        _tabs.each(function () {

            var _objs = _data[$(this).html()];
            if (_objs == null)
                return;
            _cityboxStr += "<li>";
            for (var i = 0; i < _objs.length; i++) {
                _cityboxStr += "<a href=\"javascript:void(0)\" code=\"" + _objs[i].code + "\">" + _objs[i].name + "</a>";
            }
            _cityboxStr += "</li>";

        })
        $citybox.find(".citybox").html(_cityboxStr);

        var _citys = $citybox.find(".citybox").find("li");
        _citys.eq(0).show();
        _tabs.bind("click", function () {
            var _index = $(this).index();
            _tabs.removeClass("current");
            $(this).addClass("current");
            _citys.hide().eq(_index).show();
        })

        _citys.find("a").bind("click", function () {
            $citybox.data("inputdemo").val($(this).html()).attr("data", $(this).html() + "|" + $(this).attr("code")).blur();
            $citybox.hide();
            inputhidden.val($(this).attr("code"));
            if (landlist != null && landlist.length > 0) {
                ladingLand($(this).attr("code"), landlist)
            }


        })
        $citybox.data("isSet", true);
    }

}
function showFlightCity(isDomestic, thisdom) {
    var $this = $(thisdom);
    var _offset = $this.offset();

    var $citybox;

    isDomestic = (isDomestic == null || isDomestic == "0") ? 0 : 1;

    $citybox = (isDomestic == 0) ? $('#DomesticFlightCity') : $('#inFlightCity');


    $citybox.show();
    $citybox.css({ "top": _offset.top + $this.outerHeight(), "left": _offset.left });
    $citybox.data("inputdemo", $this)

    var isSet = $citybox.data("isSet");
    if (!isSet || isSet == null) {
        $citybox.hover(function () {
            $(this).show();
        }, function () {
            $(this).hide();
            $this.blur();
        })
        $citybox.find(".close").click(function () {
            $citybox.hide();
            $this.blur();
        })
        var _tabs = $citybox.find(".f_citytabList").find("a");

        var _cityboxStr = "";
        var _data = (isDomestic == 0) ? flights_domesticCity : flights_city;

        _tabs.each(function () {

            var _objs = _data[$(this).html()];
            if (_objs == null)
                return;
            _cityboxStr += "<li>";
            for (var i = 0; i < _objs.length; i++) {
                _cityboxStr += "<a href=\"javascript:void(0)\" code=\"" + _objs[i].code + "\">" + _objs[i].name + "</a>";
            }
            _cityboxStr += "</li>";

        })
        $citybox.find(".citybox").html(_cityboxStr);

        var _citys = $citybox.find(".citybox").find("li");
        _citys.eq(0).show();
        _tabs.bind("click", function () {
            var _index = $(this).index();
            _tabs.removeClass("current");
            $(this).addClass("current");
            _citys.hide().eq(_index).show();
        })

        _citys.find("a").bind("click", function () {
            $citybox.data("inputdemo").val($(this).html()).attr("data", $(this).html() + "|" + $(this).attr("code")).blur();
            $citybox.hide();
        })
        $citybox.data("isSet", true);
    }

}






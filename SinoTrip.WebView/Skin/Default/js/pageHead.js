
$(function () {
    $("#page_nav_list").find("a").bind("mousemove", function () {
        $(this).addClass("navover");
    }).bind("mouseout", function () {
        if (!$(this).hasClass("iscurr"))
            $(this).removeClass("navover");
    });



    if (parseInt(leftmeun) == 0) {

        $("#leftmenubuttom,.allsearchlab").click(function () {
            var isshow = $("#leftmenubuttom").data("isShow");
            if (!isshow) {
                $("#leftmenubuttom").data("isShow", true);
                $("#jd_menubox").show();
            } else {
                $("#leftmenubuttom").data("isShow", false);
                $("#jd_menubox").hide();
            }
        })


        $("#jd_menubox").hover(function () {
            $("#leftmenubuttom").data("isShow", true);
            $("#jd_menubox").show();
        }, function () {
            $("#leftmenubuttom").data("isShow", false);
            $("#jd_menubox").hide();


        })



    }


    var inputtext = "可输入产品关键字/产品编号/团期编号"
    $("#searchInput").bind("blur", function () {
        if ($(this).val() == "") {
            $(this).val(inputtext).addClass("curr");

        }
    }).bind("focus", function () {
        if ($(this).val() == inputtext)
            $(this).val("").removeClass("curr");
    }).bind("keyup", function (event) {
        if (event.keyCode == "13")
            SubmitPageTopSearch();
    });
   

    var $jdcounet = $("#jd_menubox").find(".jd_counetbox");
    var $menuitem = $("#jd_menubox").find(".menuItem");

    $jdcounet.each(function (item) {

        if ($(this).css("top") == "auto")
            $(this).css({ top: -50 * item });

    })

    $jdcounet.hover(function () {

        $(this).show();
    }, function () { $(this).hide() })



    $menuitem.hover(function () {

        var _index = $menuitem.index($(this))
        $jdcounet.eq(_index).show();
        $(this).addClass("menumove");
        $(this).find(".left_menutitle").addClass("thisover")


    }, function () {
        var _index = $menuitem.index($(this))
        $jdcounet.eq(_index).hide();
        $(this).removeClass("menumove");
        $(this).find(".left_menutitle").removeClass("thisover")



    })

})


function SubmitPageTopSearch() {

    var _input = $("#searchInput");
    var Key_Word = _input.val();
  
if (Key_Word == _input.attr("title"))
        return;
   
    Key_Word = Key_Word.replace(/(^\s*)|(\s*$)/g, "");
    var url = "";
    var _str = Key_Word.substr(1, Key_Word.length)   

    if (Key_Word == parseInt(Key_Word)) {

        url = "/tour/" + Key_Word + ".html";
        document.searchInputform.action = url;

    } else {
        url = "/productSearch.html?keyword=" + escape(Key_Word);
        
        document.searchInputform.action = url;
    }
    
    document.searchInputform.submit();



}
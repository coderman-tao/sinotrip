function getOrderStatus(obj) {

    var _str = getTourOrderStatus(obj.status);

    if (obj.status == M.ORDER_TOUR_STATUS_YQR || obj.status == M.ORDER_TOUR_STATUS_DSC) {
        if (parseFloat(obj.dealmoney) - parseFloat(obj.payment) > 0)
            return "<span class='orderprice'>订单已确认,等待付款</span>";
    }
    return "<span class='tourorderstatus_" + obj.status + "'>" + _str + "</span>";

}
function getOrderButton(obj) {
    if (obj.status != M.ORDER_TOUR_STATUS_YQX) {

        var _str = '<a href="/mall/tour/Itinerary.aspx?eid=' + obj.planoutsign + '" target="_blank"><span style="width:90px;" class="inputbutton green">行程单打印</span></a>';

        if (obj.SpareInt == M.PRODUCT_TYPE_QZ)
            _str = "";

        if (obj.status == M.ORDER_TOUR_STATUS_YSC) {
            if (!!obj.ErpElectron && obj.ErpElectron != "") {
                _str += '<a href="ErpPactshow.aspx?uuid=' + obj.ErpElectron + '&type=' + obj.ErpType + '" target="_blank"><span style="width:90px;" class="inputbutton green">合同下载</span></a>';
            } else {
                _str += '<a href="PactShow.aspx?type=' + obj.SpareInt + '&id=' + obj.erpid + '" target="_blank"><span style="width:90px;" class="inputbutton green">合同下载</span></a>';
            }

        }

        if (obj.status == M.ORDER_TOUR_STATUS_DSC) {

            _str += '<a href="CheckTourPact.aspx?id=' + obj.erpid + '" target="_blank" ><span style="width:90px;" class="inputbutton green">合同预览</span></a>';
        }

       
            if (parseFloat(obj.dealmoney) - parseFloat(obj.payment) > 0 && obj.status != M.ORDER_TOUR_STATUS_DQR) {

                if (obj.status == M.ORDER_TOUR_STATUS_YQR) {
                    _str += '<a href="CheckTourPact.aspx?id=' + obj.erpid + '" target="_blank" ><span style="width:90px;" class="inputbutton orange">确认支付</span></a>';
                } else {
                    _str += '<a href="/OrderPay/default.aspx?id=' + obj.erpid + '" target="_blank" ><span style="width:90px;" class="inputbutton orange">在线支付</span></a>';

                }

            } 
            
            
            if (obj.status == M.ORDER_TOUR_STATUS_YQR && parseFloat(obj.dealmoney) - parseFloat(obj.payment)==0) {

                _str += '<a href="CheckTourPact.aspx?id=' + obj.erpid + '" target="_blank" ><span style="width:90px;" class="inputbutton orange">确认合同</span></a>';
            }
       
    }
    $("#bottom_buttonbox").html(_str);
    $("#top_buttonbox").html(_str);


}


function getOrderListButton(obj) {
    var _pagename = "TourOrderItem";
    //if (obj.ErpType == M.PRODUCT_TYPE_GTLY)
    //   _pagename = "TourOrderItem";
    // else
    //   _pagename = "VisaOrderItem";

    var _str = "<div><a  target=\"_blank\" href=\"" + _pagename + ".aspx?id=" + obj.erpid + "\">订单详情</a></div>";

    if (obj.status == M.ORDER_TOUR_STATUS_YQX)
        return _str;


    if (obj.status == M.ORDER_TOUR_STATUS_YSC) {        
        if (!!obj.ErpElectron && obj.ErpElectron != "") {
            _str += '<div><a href="ErpPactshow.aspx?uuid=' + obj.ErpElectron + '&type=' + obj.ErpType + '" target="_blank">合同下载</a></div>';
        } else {
            _str += '<div><a href="PactShow.aspx?type=' + obj.SpareInt + '&id=' + obj.erpid + '" target="_blank">合同下载</a></div>';
        }

    }

    if (obj.status == M.ORDER_TOUR_STATUS_DSC) {

        _str += '<div><a href="CheckTourPact.aspx?id=' + obj.erpid + '" target="_blank" >合同预览</a></div>';
    }

    if (parseFloat(obj.dealmoney) - parseFloat(obj.payment) > 0 && obj.status != M.ORDER_TOUR_STATUS_DQR) {

        if (obj.status == M.ORDER_TOUR_STATUS_YQR) {
            _str += '<div><a href="CheckTourPact.aspx?id=' + obj.erpid + '" target="_blank" >确认支付</a></div>'; ;

        } else {
            _str += '<div><a href="/OrderPay/default.aspx?id=' + obj.erpid + '" target="_blank" >在线支付</a></div>';
        }

    }
    return _str;

}



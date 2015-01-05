function getFlaghtStatus(status) {
    var _str;
    if (status == "0")
        _str = "<span style=\"color:Red\">申请中</span>";
    else if (status == "3")
        _str = "<span style=\"color:Blue\">已出票</span>";
    else if (status == "4")
        _str = "<span style=\"color:Blue\">配送中</span>";
    else if (status == "5")
        _str = "<span style=\"color:Blue\">部分出票</span>";
    else if (status == "7")
        _str = "<span style=\"color:#666\">客户取消</span>";
    else if (status == "8")
        _str = "<span style=\"color:#666\">已取消</span>";
    else if (status == "9")
        _str = "<span style=\"color:Green\">已完成</span>";
    else
        _str = "<span style=\"color:#ff6600\">处理中</span>";
    return _str;
}
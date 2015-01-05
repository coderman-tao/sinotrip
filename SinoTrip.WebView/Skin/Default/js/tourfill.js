


function LoginReturn(obj) {
    setFillStep(1, false);

}


      /*初始化价格*/


      selectlist.each(function (j) {
          var _val = parseInt($(this).attr("orig")); //默认的数量
          //if (_val == 0)
          //   return true;
          for (var i = 0; i < _val + 1; i++) {
              $(this).append("<option value=\"" + i + "\">" + i + "</option>");
          }
          $(this).val(_val).data("val", _val);
          var _usertype = parseInt($(this).attr("usertype"));
          var _pricetype = parseInt($(this).attr("pricetype"));
          var _priceseat = parseInt($(this).attr("priceseat"));
          var _p = parseInt($(this).attr("price")) * _val;

          if (_pricetype == M.PRICE_TYPE_VARY) {
              pricevary += _p //附加服务总价
          } else {
              pricesum += _p //正规服务总价
              usernumber += _val;
              seatnumber += _val;

              if (_usertype == M.TOURIST_TYPE_HUMAN_AGED || _usertype == M.TOURIST_TYPE_HUMAN_ADULT) {
                  adultnumber += _val;
              }
          }
          mPricelist.eq(j).html("￥" + parseFloat(_p).toFixed(2));
      })

      $("#pricevary").html("￥" + parseFloat(pricevary).toFixed(2))

      $("#pricesum").html("￥" + parseFloat(pricesum).toFixed(2))

      $("#discount").html("￥" + parseFloat(discount).toFixed(2))

      $("#payPrice").html("￥" + parseFloat(pricesum + pricevary - discount).toFixed(2));

      /*价格下拉框动作*/
      selectlist.bind("focus", function () {
          var options = $(this).find("option");
          var _val = parseInt($(this).val());
          var _v = options.length;
          var _n = 0;
          if (parseInt($(this).attr("pricetype")) == M.PRICE_TYPE_VARY)
              _n = usernumber
          else
              _n = canSeat - (seatnumber - _val);

          if (_n + 1 > _v) {
              for (var i = _v; i <= _n; i++) {
                  $(this).append("<option value=\"" + i + "\">" + i + "</option>")
              }
          } else if (_n < _v) {
              for (var i = _v; i > _n; i--) {
                  options.eq(i).remove()
              }
          }
          $(this).val(_val)
      }).bind("change", function () {
          var _nv = parseInt($(this).val());
          var _val = parseInt($(this).data("val"))
          var _price = parseInt($(this).attr("price"));
          var _usertype = parseInt($(this).attr("usertype"));


          if (parseInt($(this).attr("pricetype")) == M.PRICE_TYPE_VARY) {
              pricevary = pricevary - (_val * _price) + (_nv * _price);
          } else {
              pricesum = pricesum - (_val * _price) + (_nv * _price);
              usernumber = usernumber + _nv - _val;

              seatnumber = seatnumber + _nv - _val;

              selectvary.each(function () {
                  if (parseInt($(this).val()) > usernumber)
                      $(this).val(usernumber)
              })

              if (_usertype == M.TOURIST_TYPE_HUMAN_AGED || _usertype == M.TOURIST_TYPE_HUMAN_ADULT)
                  adultnumber = adultnumber - _val + _nv;
          }

          var _index = selectlist.index($(this));
          mPricelist.eq(_index).html("￥" + parseFloat(_nv * _price).toFixed(2));
          $(this).data("val", _nv)
          $("#pricevary").html("￥" + parseFloat(pricevary).toFixed(2))
          $("#pricesum").html("￥" + parseFloat(pricesum).toFixed(2))
          $("#payPrice").html("￥" + parseFloat(pricesum + pricevary - discount).toFixed(2));

      });
     

      //拷贝用户信息
      function copyPactUser() {
          var useritem = $("#userinput").find("tr:first");
          $("#pactUserBox").find("input,select").each(function () {
              var name = $(this).attr("name").replace("pact", "user");
              var item = useritem.find("input[name=" + name + "],select[name=" + name + "]");
              if (item.length <= 0)
                  return;

              if ($(this).attr("type") == "radio" && !$(this).attr("checked"))
                  return;             
              item.val($(this).val());

          })
          useritem.find("input[type=radio]").attr("checked", "checked");
      }


      ///验证用户信息
      function checkUser() {
          try {
              var lx = $("#userinput").find("input[type=radio]:checked");
              if (lx.length == 0)
                  throw { message: "请选择一个成人做为此次出行的联系人！" };

              $("#userinput").find("tr").removeClass("usercheck").eq(lx.parents("tr").index()).find("input,select").addClass("usercheck");

              var aaa = jQuery.Xcheck(".usercheck");
          } catch (e) {
              jQuery.Xmsg(e.message);
              return false;
          }
          return true;
      }

     

      //设置用户输入框
      function setUserBox() {
          var _lx = '';
          var _trstr = "";
          var _setUserBox = function () {

              getProductLjCup(pt, tid, pid);
              $("#pactUserName").val(QM_JUSER.realname);
              $("#pactId").val(QM_JUSER.id)
              $("#pactUserMobile").val(QM_JUSER.mobile);
              $("input[type=radio][name=pactsex][value=" + QM_JUSER.sex + "]").attr("checked", "checked");
              $("#pactUserCard").val(QM_JUSER.idcard);
              $("#pactCardType").val(M.CARD_TYPE_IDCARD);
              $("#listContent").find("select").each(function () {
                  if (parseInt($(this).attr("pricetype")) == M.PRICE_TYPE_VARY)
                      return true;
                  var _val = $(this).val();
                  var priceid = $(this).attr("priceid");
                  var pricename = $(this).attr("pricename");
                  var _tbuser = $("#userinput").find("[priceid=" + priceid + "]")

                  var _usertype = $(this).attr("usertype");
                 
                 

                  if (_usertype == M.TOURIST_TYPE_HUMAN_AGED || _usertype == M.TOURIST_TYPE_HUMAN_ADULT) {
                      _lx = '<input type="radio" name="userlx" class="pactcotent" value="' + priceid + '_{0}"/>';
                  } else {
                      _lx = "";
                  }

                  var trstr = '<tr priceid="' + priceid + '"><td class="lx">' + _lx + '</td><td><input type="hidden" value="' +priceid+'_{0}" class="pactcotent" name = "userno"><input type="text" class="pactuser pactcotent" maxL="6" minL="2" err="联系人的姓名为必填项!2至6个中文字符" checkType="chinese|notempty" name="username"/></td><td>'
                  trstr += '<select class="pactuser pactcotent" err="请选择联系人的性别" checkType="notempty" name="usersex"><option value="">请选择</option><option value="0">男</option><option value="1">女</option></select></td>';
                  trstr += '<td><select class="pactuser pactcotent" err="请选择联系人的证件类型" checkType="notempty" name="usercardtype">' + _cardtype + '</select></td><td>';
                  trstr += '<input type="text" class="pactuser pactcotent" err="请填写联系人的证件号码" checkType="notempty" name="usercard"/></td><td><input type="text" class="pactuser pactcotent" err="请正确填写联系人手机号码！" checkType="mobile|notempty" name="usermobile"/></td><td>' + pricename + '</td></tr>';

                  if (_tbuser.length < _val) {
                      var _l = _tbuser.length;
                      if (_l > 0) {
                          var _last = _tbuser.last();

                          for (var i = _l; i < _val; i++) {
                              _tbuser.after(trstr.format(RndNum(10)));
                          }

                      } else {
                          for (var i = _l; i < _val; i++) {
                              $("#userinput").append(trstr.format(RndNum(10)));
                          }

                      }

                  } else if (_tbuser.length > _val) {
                      for (var i = _tbuser.length - 1; i >= _val; i--)
                          _tbuser.eq(i).remove();
                  }
              })
          }
          if (QM_JUSER == null) {
              var url = "/AJAX/User.ashx?action=GetUser"
              jQuery.Xload();
              jQuery.ajax({
                  url: url,
                  type: "POST",
                  success: function (response) {
                      jQuery.XloadHide();
                      var json = response.toJson();
                      if (json.success) {
                          QM_JUSER = json.result;
                          _setUserBox(QM_JUSER);
                         
                          setFillStep(1, true);
                          
                      } else {
                          showlogin();
                      }
                  }

              });

              return false;
          } else {
              _setUserBox(QM_JUSER)
              return true;

          }
          return false;
      }


      ///设置用户确认信息
      function setConfirmUser() {
          $("#pactUserBox").find("input").each(function () {
              var confirmitem = $("#confirmPactUser").find("span[item=" + $(this).attr("name") + "]");
              if (confirmitem.length <= 0)
                  return true;
              _val = "";
              if ($(this).attr("type") == "text")
                  _val = $(this).val();
              if ($(this).attr("type") == "radio") {
                  if (!$(this).attr("checked"))
                      return true;

                  _val = $(this).prev("span:first").html();
               
                  
              }

              confirmitem.html(_val)
          })

          $("#pactUserBox").find("select").each(function () {
              var confirmitem = $("#confirmPactUser").find("span[item=" + $(this).attr("name") + "]");
              if (confirmitem.length <= 0)
                  return true;
              confirmitem.html($(this).find("option[value=" + $(this).val() + "]").html());

          })

          var usertable = $($("#userinput").html());
          var inputlists = $("#userinput").find("td");
          usertable.find("td").each(function (i) {
              if (inputlists.eq(i).html() == "") {
                  $(this).html(Math.ceil((i + 1) / 7));

              }

              var item = inputlists.eq(i).find("input[type=text]:first,input[type=radio]:first")
              var _v = $(this).html();
              
              if (item.length > 0) {                  
                  if (item.attr("type") == "text")
                      _v = item.val();
                  else if (item.attr("type") == "radio") {
                      if (item.attr("checked"))
                          _v = "联系人";
                      else
                          _v = Math.ceil((i + 1) / 7); ;
                  }
              } else {
                  item = inputlists.eq(i).find("select:first")
                  if (item.length > 0) {
                      if (!!item.val())
                          _v = item.find("option[value=" + item.val() + "]").html()
                      else
                          _v = "";
                  }
              }
              $(this).html(_v)
          })

          $("#confirmTourist").html(usertable)


          //<div class="addstitle">☼<span class="boldStr" style=" color:#000">保险</span> <span>选购数：10</span> <span>已分配：10</span> <span>剩余：0</span></div>
          //<div class="addsbox"><span><input type="checkbox" />张三</span><span><input type="checkbox" />张三</span><span><input type="checkbox" />张三</span></div>
          var additiveStr = "";
          
          $("#additive").find("select").each(function () {
              var _n = parseInt($(this).val());
              if (_n <= 0)
                  return;
              var _name = $(this).attr("pricename");
              var _priceid = $(this).attr("priceid");
              var _thisuserinput = $("#userinput").find("tr");
              //if (_thisuserinput.length == _n)
              additiveStr += '<div class="addstitle">☼<span class="boldStr" style=" color:#000">' + _name + '</span>';
              additiveStr += '<span>选购数：<span item="total" priceid="' + _priceid + '">' + _n + '</span></span> <span>已分配：<span item="use" priceid="' + _priceid + '">' + _thisuserinput.length + '</span></span> ';
              additiveStr += '<span>剩余：<span item="surplus" priceid="' + _priceid + '">' + (_n - _thisuserinput.length < 0 ? 0 : _n - _thisuserinput.length) + '</span></span></div>';
              additiveStr += '<div class="addsbox">';

              _thisuserinput.each(function (j) {
                  var _username = $(this).find("input[name=username]").val();
                  if (!_username)
                      _username = "游客" + (j + 1);
                  if (j < _n)
                      additiveStr += '<span><input class="pactcotent" value="' + $(this).find("input[name=userno]").val() + '" type="checkbox" name="additive_' + _priceid + '" onchange="setAdditive(this)"  priceid="' + _priceid + '" checked/>' + _username + '</span>';
                  else
                      additiveStr += '<span><input class="pactcotent" value="' + $(this).find("input[name=userno]").val() + '" type="checkbox" name="additive_' + _priceid + '" onchange="setAdditive(this)"  priceid="' + _priceid + '" disabled="disabled"/>' + _username + '</span>';
              })

              additiveStr += '</div>';
          });
         
          if (!additiveStr) {
              $("#addserver").hide();

          } else {
              $("#addserver").show();
              $("#additiveList").html(additiveStr);
          }
         
      }
      //分配服务
      function setAdditive(item) {
          var itemobj = $(item);

          var usernumber = $("#userinput").find("tr").length;

          var _priceid = itemobj.attr("priceid");

          var _addtotal = $("#additive").find("select[priceid=" + _priceid + "]").val();

          var _adduse = $("#additiveList").find("input[type=checkbox][priceid="+_priceid+"]:checked")

          var _addsurplus = parseInt(_addtotal) - parseInt(_adduse.length);
          $("#additiveList").find("span[priceid=" + _priceid + "][item=use]").html(_adduse.length);
          $("#additiveList").find("span[priceid=" + _priceid + "][item=surplus]").html(_addsurplus);

          if (_addsurplus == 0)
              $("#additiveList").find("input[type=checkbox][priceid=" + _priceid + "][checked=false]").attr("disabled", "disabled");
          else
              $("#additiveList").find("input[type=checkbox][priceid=" + _priceid + "]").removeAttr("disabled");
      
      }


      function SubmitOrder() {
          var data = {};
          try {
              data = jQuery.Xfilled(".pactcotent",true);
          } catch (e) {
              jQuery.Xmsg(e.message);
              return false;
          }

          var url = "/AJAX/TourOrder.aspx?action=Submit"

          jQuery.Xload();

          jQuery.ajax({
              url: url,
              data: data,
              traditional: true,
              type: "POST",
              success: function (response) {

                  jQuery.XloadHide();
                  var json = response.toJson();
                  if (json.success) {
                      alert("下单成功，请保持电话畅通，耐心等待客服确认！");
                      window.location.href = "/UserWeb/";
                      //jQuery.Xmsg({ msg: "下单成功，请保持电话畅通，耐心等待客服确认！", button: function () { window.location.href = "/UserWeb/" } })
                  } else {
                      jQuery.Xmsg(json.msg)
                  }
              }
          });
      }

      ///步奏
      function setFillStep(isplus, nocheck) {
          if (isplus && !nocheck) {
              if (!$("#check_packname").attr("checked")) {
                  jQuery.Xmsg("您只有仔细阅读并同意订购协议后才能进行后续操作!");
                  return false;
              }
              switch (fillstep) {
                  case 1:
                      if (adultnumber <= 0) {
                          jQuery.Xmsg("出游成人数量不能小于1!");
                          return false;
                      }
                      if (!setUserBox())
                          return false;
                      break;
                  case 2:
                      if (!checkUser())
                          return false;
                      setConfirmUser();
                      break
                  case 3:
                      SubmitOrder();   
                      return false;    
              }
          }
          $("#flowPath").removeClass("step" + fillstep)
          $("#fill_" + fillstep).hide();
          if (isplus) {
              fillstep++
          } else {
              fillstep--
              if (fillstep == 2)
                  resCoupon();
          }
          $("#fill_" + fillstep).show();
          $("#flowPath").addClass("step" + fillstep)
          if (fillstep == 1)
              $("#stepButtonL").hide();
          else
              $("#stepButtonL").show();
          if (fillstep <= 3) {
              if (fillstep < 3)
                  $("#stepButtonR").html("下一步")
              else
                  $("#stepButtonR").html("提交订单");

          } else {
              fillstep = 3;
          }
      }

      function checkCupCode(forum, tourid, planid) {
          var _code = $("#cupcode").val();
          if (!_code)
              return;

          var cb = $("#codemsg").find("input[type=checkbox]:checked");
          if (cb.length > 0) {
              cb.removeAttr("checked");
              changeCoupone(cb)
              $("#codemsg").html("");
          }

          var data = {};
          data.forum = forum;
          data.productid = tourid;
          data.planid = planid;
          data.code = _code;         
          var url = "/AJAX/coupon.ashx?action=checkcouponcode"
          jQuery.ajax({
              url: url,
              data: data,
              type: "POST",
              success: function (response) {

                  var json = response.toJson();
                  var str = "<span style=\"color:Red\">当前验证码不可用!</span>";
                  if (json.success) {
                      var clist = json.result;
                      var trstr = "";
                      if (clist.length > 0) {
                          var i = 0;
                          var _note = "";
                          var _credit = "";
                          var disabled = "";
                          if (clist[i].mode != M.COUPON_MODE_PERSON) {
                              _note = "订单总额满"
                              _credit = "下单立减" + parseFloat(clist[i].credit).toFixed(2) + "元";
                          } else {
                              _note = "个人单价满"
                              _credit = "每人立减" + parseFloat(clist[i].credit).toFixed(2) + "元";
                          }
                          if (parseInt(clist[i].money) > 0)
                              _note += parseFloat(clist[i].money).toFixed(2) + "元";
                          else
                              _note = "";

                          _note = _note + _credit; 
                          if (clist[i].overlay == M.COUPON_OVERLAY_YES) {
                              _note += ",可与其它优惠叠加使用";
                          } else if ($("#couponBox").find("input[type=checkbox][overlay!=" + M.COUPON_OVERLAY_YES + "]:checked").length > 0) {
                              disabled = 'disabled="disabled"';
                          }
                          if (checkcoupon(clist[i].mode, clist[i].money, clist[i].credit) <= 0) {
                              disabled = 'disabled="disabled"';
                              _note += "<span style=\"color:Red\">不满足使用条件！</span>";
                          }




                          trstr += '<input index="' + i + '" ' + disabled + ' onchange = "changeCoupone($(this))"  class="couponRadio"  mode="' + clist[i].mode + '" overlay="' + clist[i].overlay + '"  money="' + clist[i].money + '" credit="' + clist[i].credit + '" type="checkbox" name="coupcodeid" value="' + clist[i].id + '">'
                          trstr += ' ' + clist[i].title;
                          trstr += "　" + _note;

                          str = "<span style=\"color:Green\">" + trstr + "</span>"
                      }
                      $("#codemsg").html(str)
                  } else {
                      str = "<span style=\"color:Red\">" + json.msg + "!</span>";
                  }
                  $("#codemsg").html(str)
              }

          });

      }

      function resCoupon() {
          var cls = $("#couponBox").find("input[type=checkbox]");
          discount = 0;
          $("#discount").html("￥" + parseFloat(discount).toFixed(2));
          $("#payPrice").html("￥" + parseFloat(pricesum + pricevary - discount).toFixed(2));
          cls.removeClass("pactcotent").removeAttr("disabled").removeAttr("checked").removeAttr("status");
          cls.each(function () {
              if (checkcoupon($(this).attr("mode"), $(this).attr("money"), $(this).attr("credit")) <= 0)
                  $(this).attr("disabled", "disabled").attr("status", "0");
          })
      }

      ///返回产品的立减信息
      function getProductLjCup(forum, tourid, planid) {

          if ($("#lijianbody").data("landCup"))              
              return false;
          
          var data = {};
          data.forum = forum;
          data.productid = tourid;
          data.planid = planid;
          data.type = M.COUPON_TYPE_LIJIAN;
          var url = "/AJAX/coupon.ashx?action=usercouponlist"
          jQuery.ajax({
              url: url,
              data: data,
              type: "POST",
              success: function (response) {
                  $("#lijianbody").data("landCup", true);
                  var json = response.toJson();
                  if (json.success) {

                      var clist = json.result.InvaildCoupon;
                      var ulist = json.result.Membercoupon;
                      var trstr = "";
                      var utrstr = "";
                      for (var i = 0; i < clist.length; i++) {
                          var _note = "";
                          var _credit = "";
                          if (clist[i].mode != M.COUPON_MODE_PERSON) {
                              _note = "订单总额满"
                              _credit = "下单立减" + parseFloat(clist[i].credit).toFixed(2) + "元";
                          } else {
                              _note = "个人单价满"
                              _credit = "每人立减" + parseFloat(clist[i].credit).toFixed(2) + "元";
                          }
                          if (parseInt(clist[i].money) > 0)
                              _note += parseFloat(clist[i].money).toFixed(2) + "元";
                          else
                              _note = "";

                          _note = _note + _credit;
                          if (clist[i].overlay == M.COUPON_OVERLAY_YES)
                              _note += ",可与其它优惠叠加使用";

                          trstr += "<tr>"
                          trstr += '<td><input index="'+i+'"  onchange = "changeCoupone($(this))"  class="couponRadio"  mode="' + clist[i].mode + '" overlay="' + clist[i].overlay + '"  money="' + clist[i].money + '" credit="' + clist[i].credit + '" type="checkbox" name="lijian" value="' + clist[i].id + '"></td>'
                          trstr += '<td>' + clist[i].title + '</td>';
                          trstr += '<td style="text-align:center">￥' + parseFloat(clist[i].credit).toFixed(2) + '</td>';
                          trstr += '<td>' + (parseInt(clist[i].money) <= 0 ? "无" : "满" + parseInt(clist[i].money)) + '</td>';
                          trstr += '<td>' + _note + '</td>';
                          trstr += '<td style="text-align:center">' + dateFormat("Y年m月d日", clist[i].deadline) + '</td>';
                          trstr += "</tr>"
                      }


                      for (var i = 0; i < ulist.length; i++) {
                          var _note = "";
                          var _credit = "";
                          if (ulist[i].mode != M.COUPON_MODE_PERSON) {
                              _note = "订单总额满"
                              _credit = "下单立减" + parseFloat(ulist[i].credit).toFixed(2) + "元";
                          } else {
                              _note = "个人单价满"
                              _credit = "每人立减" + parseFloat(ulist[i].credit).toFixed(2) + "元";
                          }
                          if (parseInt(ulist[i].money) > 0)
                              _note += parseFloat(ulist[i].money).toFixed(2) + "元";
                          else
                              _note = "";

                          _note = _note + _credit;
                          if (ulist[i].overlay == M.COUPON_OVERLAY_YES)
                              _note += ",<span style=\"color:Blue\">可叠加使用</span>";
                          utrstr += "<tr>"
                          utrstr += '<td><input index="' + i + '"  onchange ="changeCoupone($(this))" class="couponRadio"  mode="' + ulist[i].mode + '" overlay="' + ulist[i].overlay + '"  money="' + ulist[i].money + '" credit="' + ulist[i].credit + '"  type="checkbox" name="usercoupon" value="' + ulist[i].id + ',' + ulist[i].couponid + '"></td>'
                          utrstr += '<td>' + ulist[i].couponname + '</td>';
                          utrstr += '<td style="text-align:center">￥' + parseFloat(ulist[i].credit).toFixed(2) + '</td>';
                          utrstr += '<td>' + (parseInt(ulist[i].money) <= 0 ? "无" : "满" + parseInt(ulist[i].money)) + '</td>';
                          utrstr += '<td>' + _note + '</td>';
                          utrstr += '<td style="text-align:center">' + dateFormat("Y年m月d日", ulist[i].deadline) + '</td>';
                          utrstr += "</tr>"
                      }

                      if (trstr != "") {
                          $("#lijianbody").html(trstr)
                      } else {
                          $("#lijianbody").html("<tr><td colspan=\"6\"></td></tr>");
                          $("#lijianBox").hide();
                      }

                      if (utrstr != "") {
                          $("#userCupbody").html(utrstr)
                      } else {
                          $("#userCupbody").html("<tr><td colspan=\"6\">帐户下没有可用的优惠券信息</td></tr>");
                      }


                      resCoupon()

                  }
              }

          });

      }

      function checkcoupon(mode, money, credit) {
          mode = parseInt(mode);
          money = parseInt(money);
          credit = parseInt(credit);
          var thisval = 0;
          if (mode == M.COUPON_MODE_ORDER) {//按订单优惠
              if (money <= pricesum + pricevary)
                  thisval = credit;
              else
                  thisval = 0;
          } else if (mode == M.COUPON_MODE_PERSON) {

              selectlist.each(function (j) {
                  var _val = $(this).val(); //默认的数量
                  if (_val == 0)
                      return;
                  var ups = parseInt($(this).attr("price"));
                  if (ups >= money) {
                      thisval += credit * _val;
                  }

              });
          }
          return thisval;         
      }


      function changeCoupone(jcoupon) {

          var _val = jcoupon.val();
          var _name = jcoupon.attr("name");
          if (jcoupon.attr("checked")) {
              jcoupon.addClass("pactcotent");
              if (jcoupon.attr("overlay") != M.COUPON_OVERLAY_YES) {
                  $("#couponBox").find("input[type=checkbox][overlay!=" + M.COUPON_OVERLAY_YES + "][value!=" + _val + "]").attr("disabled", "disabled");
              } 
              discount += checkcoupon(jcoupon.attr("mode"), jcoupon.attr("money"), jcoupon.attr("credit"));
              
              $("#couponBox").find("input[type=checkbox][name=" + _name + "][index!=" + jcoupon.attr("index") + "]").attr("disabled", "disabled");
          } else {              
            if (jcoupon.attr("overlay") != M.COUPON_OVERLAY_YES) {
                $("#couponBox").find("input[type=checkbox][overlay!=" + M.COUPON_OVERLAY_YES + "][status!=0]").removeAttr("disabled");
             }
            discount -= checkcoupon(jcoupon.attr("mode"), jcoupon.attr("money"), jcoupon.attr("credit"));
            $("#couponBox").find("input[type=checkbox][name=" + _name + "][status!=0]").removeAttr("disabled");
            jcoupon.removeClass("pactcotent");
          }


        $("#discount").html("￥" + parseFloat(discount).toFixed(2));
         $("#payPrice").html("￥" + parseFloat(pricesum + pricevary - discount).toFixed(2));
      }


      $(function () {
          $("#pricenoteTap").find("li").live("click", function () {
              var _index = $(this).index();
              $("#pricenoteTap").find("li").removeClass("current");
              $(this).addClass("current");
              $("#pricenoteContent").find(".tapListContent").hide().eq(_index).show();
          });

        

      })

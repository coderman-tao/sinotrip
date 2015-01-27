using SinoTrip.API.Cartrip.Ucenter;
using SinoTrip.API.Cartrip.Ucenter.Api;
using SinoTrip.API.Cartrip.Ucenter.Client;
using SinoTrip.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinoTrip.WebView.ReturnPage.API
{
    /// <summary>
    /// uc 的摘要说明
    /// </summary>
    public class uc : UcApiBase
    {
        public override ApiReturn DeleteUser(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn RenameUser(int uid, string oldUserName, string newUserName)
        {
            throw new NotImplementedException();
        }

        public override UcTagReturns GetTag(string tagName)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn SynLogin(int uid, HttpContext context)
        {
            try
            {
                IUcClient client = new UcClient();
                UcUserInfo user = client.UserInfo(uid);
                LoggerCore.Debug("开始登录---------------------");
                if (user.Success)
                {
                    LoggerCore.Debug("成功---------------------------------");
                    context.Session["uid"] = user.Uid;
                    context.Session["username"] = user.UserName;
                    context.Session["email"] = user.Mail;
                    return ApiReturn.Success;
                }
                LoggerCore.Debug("失败---------------------------------");
                return ApiReturn.Failed;
            }
            catch (Exception)
            {
                return ApiReturn.Failed;
            }
        }

        public override ApiReturn SynLogout(HttpContext context)
        {
            try
            {
                context.Session.Abandon();
            }
            catch (Exception)
            {
                
                throw;
            }
            
            return ApiReturn.Success;
        }

        public override ApiReturn UpdatePw(string userName, string passWord)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateBadWords(UcBadWords badWords)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateHosts(UcHosts hosts)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateApps(UcApps apps)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateClient(UcClientSetting client)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateCredit(int uid, int credit, int amount)
        {
            throw new NotImplementedException();
        }

        public override UcCreditSettingReturns GetCreditSettings()
        {
            throw new NotImplementedException();
        }

        public override ApiReturn GetCredit(int uid, int credit)
        {
            throw new NotImplementedException();
        }

        public override ApiReturn UpdateCreditSettings(UcCreditSettings creditSettings)
        {
            throw new NotImplementedException();
        }
    }
}
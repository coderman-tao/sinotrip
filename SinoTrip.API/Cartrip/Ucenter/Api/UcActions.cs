using System.Collections.Generic;

namespace SinoTrip.API.Cartrip.Ucenter.Api
{
    internal static class UcActions
    {
        private static readonly IList<string> Items;

        /// <summary>
        /// �˽ӿڹ����������ӡ��� UCenter ���� test �Ľӿ�����ʱ��
        /// ����ɹ���ȡ���ӿڷ��ص� API_RETURN_SUCCEED ֵ����ʾ UCenter ��Ӧ��ͨѶ������
        /// </summary>
        public static string Test { get { return "test"; } }

        /// <summary>
        /// �� UCenter ɾ��һ���û�ʱ���ᷢ�� deleteuser �Ľӿ�����֪ͨ����Ӧ�ó���ɾ����Ӧ���û���
        /// ����Ĳ������� $get['ids'] �У�ֵΪ�ö��ŷָ����û� ID�����ɾ���ɹ������ API_RETURN_SUCCEED��
        /// </summary>
        public static string DeleteUser { get { return "deleteuser"; } }

        /// <summary>
        /// �� UCenter ����һ���û����û���ʱ���ᷢ�� renameuser �Ľӿ�����֪ͨ����Ӧ�ó��������
        /// ����Ĳ��� $get['uid'] ��ʾ�û� ID��$get['oldusername'] ��ʾ���û�����$get['newusername'] ��ʾ���û���������޸ĳɹ������ API_RETURN_SUCCEED��
        /// </summary>
        public static string RenameUser { get { return "renameuser"; } }

        /// <summary>
        /// ���û������û�����ʱ���˽ӿڸ������ UCenter �����������롣
        /// ����Ĳ��� $get['username'] ��ʾ�û�����$get['password'] ��ʾ�����롣����޸ĳɹ������ API_RETURN_SUCCEED��
        /// </summary>
        public static string UpdatePw { get { return "updatepw"; } }

        /// <summary>
        /// ���Ӧ�ó�����ڱ�ǩ���ܣ�����ͨ���˽ӿڰ�Ӧ�ó���ı�ǩ���ݴ��ݸ� UCenter��
        /// ����Ĳ������� $get['id'] �У�ֵΪ��ǩ���ơ�����������辭�� uc_serialize ����
        /// </summary>
        public static string GetTag { get { return "gettag"; } }

        /// <summary>
        /// ���Ӧ�ó�����Ҫ������Ӧ�ó������ͬ����¼���˲��ִ��븺����ָ���û��ĵ�¼״̬��
        /// ����Ĳ������� $get['uid'] �У�ֵΪ�û� ID���˽ӿ�Ϊ֪ͨ�ӿڣ���������ݡ�ͬ����¼��ʹ�� P3P ��׼��
        /// </summary>
        public static string SynLogin { get { return "synlogin"; } }

        /// <summary>
        /// ���Ӧ�ó�����Ҫ������Ӧ�ó������ͬ���˳���¼���˲��ִ��븺�����û��ĵ�¼��״̬��
        /// �˽ӿ�Ϊ֪ͨ�ӿڣ������������������ݡ�ͬ���˳���ʹ�� P3P ��׼��
        /// </summary>
        public static string SynLogout { get { return "synlogout"; } }

        /// <summary>
        /// �� UCenter �Ĵ���������ñ��ʱ���˽ӿڸ���֪ͨ����Ӧ�ó�����º�Ĵ�������������ݡ�
        /// ���������� POST ��ʽ�ύ���ӿڡ��ӿ����������� API_RETURN_SUCCEED��
        /// </summary>
        public static string UpdateBadWords { get { return "updatebadwords"; } }

        /// <summary>
        /// �� UCenter �������������ñ��ʱ���˽ӿڸ���֪ͨ����Ӧ�ó�����º�����������������ݡ�
        /// ���������� POST ��ʽ�ύ���ӿڡ��ӿ����������� API_RETURN_SUCCEED��
        /// </summary>
        public static string UpdateHosts { get { return "updatehosts"; } }

        /// <summary>
        /// �� UCenter ��Ӧ�ó����б���ʱ���˽ӿڸ���֪ͨ����Ӧ�ó�����º��Ӧ�ó����б�
        /// ���������� POST ��ʽ�ύ���ӿڡ��ӿ����������� API_RETURN_SUCCEED��
        /// </summary>
        public static string UpdateApps { get { return "updateapps"; } }

        /// <summary>
        /// �� UCenter �Ļ���������Ϣ���ʱ���˽ӿڸ���֪ͨ����Ӧ�ó�����º�Ļ����������ݡ�
        /// ���������� POST ��ʽ�ύ���ӿڡ��ӿ����������� API_RETURN_SUCCEED��
        /// </summary>
        public static string UpdateClient { get { return "updateclient"; } }

        /// <summary>
        /// ��ĳӦ��ִ���˻��ֶһ�����Ľӿں��� uc_credit_exchange_request() �󣬴˽ӿڸ���֪ͨ���һ���Ŀ��Ӧ�ó��������޸ĵ��û�����ֵ��
        /// ����Ĳ��� $get['credit'] ��ʾ���ֱ�ţ�$get['amount'] ��ʾ���ֵ�����ֵ��$get['uid'] ��ʾ�û� ID
        /// </summary>
        public static string UpdateCredit { get { return "updatecredit"; } }

        /// <summary>
        /// �˽ӿڸ����Ӧ�ó���Ļ������ô��ݸ� UCenter���Թ� UCenter �ڻ��ֶһ�������ʹ�á�
        /// �˽ӿ����������������������辭�� uc_serialize ����
        /// </summary>
        public static string GetCreditSettings { get { return "getcreditsettings"; } }

        /// <summary>
        /// �˽ӿڸ������ UCenter ���ֶһ����õĲ�����
        /// ����Ĳ������� $get['credit'] �У�ֵΪ���õĲ������顣�ӿ����������� API_RETURN_SUCCEED��
        /// </summary>
        public static string UpdateCreditSettings { get { return "updatecreditsettings"; } }

        /// <summary>
        /// �˽ӿ����ڰ�Ӧ�ó�����ָ���û��Ļ��ִ��ݸ� UCenter��
        /// ����Ĳ��� $get['uid'] Ϊ�û� ID��$get['credit'] Ϊ���ֱ�š��ӿ���������������ֵ��
        /// </summary>
        public static string GetCredit { get { return "getcredit"; } }

        static UcActions()
        {
            Items = new List<string>
                                  {
                                      DeleteUser,
                                      GetCreditSettings,
                                      GetTag,
                                      RenameUser,
                                      SynLogin,
                                      SynLogout,
                                      Test,
                                      UpdateApps,
                                      UpdateBadWords,
                                      UpdateClient,
                                      UpdateCredit,
                                      UpdateCreditSettings,
                                      UpdateHosts,
                                      UpdatePw,
                                      GetCredit,
                                  };
        }

        /// <summary>
        /// �Ƿ�������Action
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns></returns>
        public static bool Contains(string action)
        {
            return Items.Contains(action.ToLower());
        }
    }
}


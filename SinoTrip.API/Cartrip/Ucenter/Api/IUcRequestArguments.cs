using System.Collections.Specialized;

namespace SinoTrip.API.Cartrip.Ucenter.Api
{
    /// <summary>
    /// Requser����
    /// Dozer ��Ȩ����
    /// �����ơ��޸ģ����뱣���ҵ���ϵ��ʽ��
    /// http://www.dozer.cc
    /// mailto:dozer.cc@gmail.com
    /// </summary>
    public interface IUcRequestArguments
    {       
        /// <summary>
        /// Action
        /// </summary>
        string Action { get; }
        /// <summary>
        /// ʱ��
        /// </summary>
        long Time { get; }
        /// <summary>
        /// Query����
        /// </summary>
        NameValueCollection QueryString { get; }

        /// <summary>
        /// Form����
        /// </summary>
        string FormData { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsInvalidRequest { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsAuthracationExpiried { get; }
    }
}
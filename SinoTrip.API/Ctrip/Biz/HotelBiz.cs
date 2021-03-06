﻿using SinoTrip.API.Ctrip.Hotel.Module;
using SinoTrip.API.Ctrip.Model.Hotel;
using SinoTrip.API.Ctrip.Model.Hotel.Request;
using SinoTrip.API.Ctrip.Model.Hotel.Response;
using SinoTrip.API.Ctrip.SDK;
using SinoTrip.FrameWork.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace SinoTrip.API.Ctrip.Biz
{
    public class HotelBiz
    {
        private static readonly string SID = ConfigurationManager.AppSettings["ctripWebID"];
        private static readonly string UID = ConfigurationManager.AppSettings["ctripID"];
        private static readonly string KEY = ConfigurationManager.AppSettings["ctripKey"];

        /// <summary>
        /// 酒店列表查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public OTA_HotelSearchRS QueryHotel(OTA_HotelSearchRQ req)
        {
            OTA_HotelSearchRS rep = new OTA_HotelSearchRS();

            if (req.AreaID == null && req.HotelCityCode == null && (req.HotelName == null || req.HotelName == "") && (req.HotelStarLevel == null || req.StarScore == null))
            {
                rep.ValidateResult = false;
                rep.ValidateResultMessage = "请求信息不全";
                return rep;
            }

            StringBuilder reqXml = new StringBuilder();
            reqXml.AppendFormat("<ns:OTA_HotelSearchRQ Version=\"1.0\" PrimaryLangID=\"zh\" xsi:schemaLocation=\"http://www.opentravel.org/OTA/2003/05 OTA_HotelSearchRQ.xsd\" xmlns=\"http://www.opentravel.org/OTA/2003/05\">");
            reqXml.Append("<ns:Criteria>");
            reqXml.AppendFormat("<ns:Criterion>");
            reqXml.Append("<ns:HotelRef ");
            if (req.HotelCityCode != null)
            {
                reqXml.AppendFormat(" HotelCityCode=\"{0}\"", req.HotelCityCode);
            }
            if (req.AreaID != null)
            {
                reqXml.AppendFormat(" AreaID=\"{0}\"", req.AreaID);
            }
            if (!string.IsNullOrEmpty(req.HotelName))
            {
                reqXml.AppendFormat(" HotelName=\"{0}\"", req.HotelName);
            }
            reqXml.Append(" />");

            if (req.PositionTypeCode != null)
            {
                reqXml.AppendFormat("<ns:Position PositionTypeCode=\"{0}\" />", req.PositionTypeCode);
            }
            if (req.HotelStarLevel != null && req.StarScore != null)
            {
                reqXml.AppendFormat("<ns:Award Provider=\"{0}\" Rating=\"{1}\"/>", req.HotelStarLevel, req.StarScore);
            }

            reqXml.Append("</ns:Criterion>");
            reqXml.Append("</ns:Criteria>");
            reqXml.Append("</ns:OTA_HotelSearchRQ>");
            req.RequestContent = reqXml.ToString();
            var remote = ConfigurationManager.AppSettings["ctrip_Api"];
            APICommon apicommon = new APICommon(SID, UID, KEY, remote);
            string repXml = apicommon.GetResult(req);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(repXml);
            GetHeaderResult(xmlDoc, rep);
            List<CtripHotelRepEntity> hotelList = new List<CtripHotelRepEntity>();
            List<ZoneTypeInfo> zoneinfos;
            ZoneTypeInfo zoneInfo;
            var hotelNodes = xmlDoc.SelectNodes("/Response/HotelResponse/OTA_HotelSearchRS/Properties/Property");
            CtripHotelRepEntity entity = null;
            foreach (XmlElement el in hotelNodes)
            {
                entity = new CtripHotelRepEntity();
                entity.AddressLine = el.SelectSingleNode("Address/AddressLine").InnerText.Trim();
                entity.AreaID = el.GetAttribute("AreaID").Trim().ToInt32();
                entity.BrandCode = el.GetAttribute("BrandCode") == "" ? 0 : el.GetAttribute("BrandCode").ToInt32();
                entity.CityName = el.SelectSingleNode("Address/CityName").InnerText.Trim();
                entity.HotelName = el.GetAttribute("HotelName").Trim();
                entity.PostCode = el.SelectSingleNode("Address/PostalCode").InnerText.Trim();
                entity.HotelCode = el.GetAttribute("HotelCode").Trim().ToInt32();
                entity.HotelId = el.GetAttribute("HotelId").Trim().ToInt32();
                entity.HotelCityCode = el.GetAttribute("HotelCityCode").Trim().ToInt32();

                var awardNodes = el.SelectNodes("Award");
                foreach (XmlElement al in awardNodes)
                {
                    string provider = al.GetAttribute("Provider");
                    if (provider != "" && provider == "HotelStarRate")
                    {
                        entity.HotelStarRate = al.GetAttribute("Rating").Trim().ToDecimal();
                    }
                    if (provider != "" && provider == "CtripStarRate")
                    {
                        entity.CtripStarRate = al.GetAttribute("Rating").Trim().ToDecimal();
                    }
                    if (provider != "" && provider == "CtripRecommendRate")
                    {
                        entity.CtripRecommendRate = al.GetAttribute("Rating").Trim().ToDecimal();
                    }
                    if (provider != "" && provider == "CtripCommRate")
                    {
                        entity.CtripCommRate = al.GetAttribute("Rating").Trim().ToDecimal();
                    }
                    if (provider != "" && provider == "CommSurroundingRate")
                    {
                        entity.CommSurroundingRate = al.GetAttribute("Rating").Trim().ToDecimal();
                    }
                    if (provider != "" && provider == "CommFacilityRate")
                    {
                        entity.CommFacilityRate = al.GetAttribute("Rating").Trim().ToDecimal();
                    }
                    if (provider != "" && provider == "CommCleanRate")
                    {
                        entity.CommCleanRate = al.GetAttribute("Rating").Trim().ToDecimal();
                    }
                    if (provider != "" && provider == "CommServiceRate")
                    {
                        entity.CommServiceRate = al.GetAttribute("Rating").Trim().ToDecimal();
                    }
                }

                var relativePositionNodes = el.SelectNodes("RelativePosition");
                List<RelativePosition> relatives = new List<RelativePosition>();
                foreach (XmlElement rl in relativePositionNodes)
                {
                    relatives.Add(new RelativePosition()
                    {
                        Distance = rl.GetAttribute("Distance").Trim().ToDecimal(),
                        Name = rl.GetAttribute("Name").Trim(),
                        UnitOfMeasureCode = rl.GetAttribute("UnitOfMeasureCode").Trim().ToInt32()
                    });
                }
                entity.RelativePositions = relatives;


                List<VendorMessage> vendorMessages = new List<VendorMessage>();

                var VendorMessageNodes = el.SelectNodes("VendorMessages/VendorMessage");

                if (VendorMessageNodes != null && VendorMessageNodes.Count > 0)
                {
                    VendorMessage vendorMessage = null;
                    foreach (XmlNode v in VendorMessageNodes)
                    {
                        vendorMessage = new VendorMessage();
                        vendorMessage.InfoType = ((XmlElement)v).GetAttribute("InfoType").ToInt32();
                        var vText = v.SelectSingleNode("SubSection/Paragraph/Text");
                        if (vText != null)
                        {
                            vendorMessage.Paragraph = vText.InnerText.Trim();
                        }
                        vendorMessages.Add(vendorMessage);
                    }

                    entity.VendorMessages = vendorMessages;
                }

                XmlNode imgText = el.SelectSingleNode("VendorMessages/VendorMessage/SubSection/Paragraph/Text");
                if (imgText != null)
                {
                    entity.ImgText = imgText.InnerText;
                }
                else
                {
                    entity.ImgText = "";
                }
                XmlNode positionNode = el.SelectSingleNode("Position");
                entity.Longitude = ((XmlElement)positionNode).GetAttribute("Longitude");
                entity.Latitude = ((XmlElement)positionNode).GetAttribute("Latitude");
                entity.PositionTypeCode = ((XmlElement)positionNode).GetAttribute("PositionTypeCode");

                var zongTypeNodes = el.SelectNodes("TPA_Extensions/Zone/ZoneType");
                zoneinfos = new List<ZoneTypeInfo>();
                if (zongTypeNodes != null)
                {

                    foreach (XmlElement xe in zongTypeNodes)
                    {
                        zoneInfo = new ZoneTypeInfo();
                        zoneInfo.ZoneID = xe.GetAttribute("ZoneID").Trim().ToInt32();
                        zoneInfo.ZoneName = xe.GetAttribute("ZoneName").Trim();
                        zoneinfos.Add(zoneInfo);
                    }
                }
                entity.Zones = zoneinfos;

                hotelList.Add(entity);
            }
            rep.HotelList = hotelList;
            return rep;
        }

        /// <summary>
        /// 根据请求的Hotel Code返回相应的酒店静态信息，
        /// 并根据请求返回酒店信息，酒店设施，酒店区域，酒店多媒体信息
        /// </summary>
        /// <returns></returns>
        public OTA_HotelDescriptiveInfoRS OTA_HotelDescriptiveInfo(OTA_HotelDescriptiveInfoRQ req)
        {
            var rep = new OTA_HotelDescriptiveInfoRS();
            StringBuilder reqXml = new StringBuilder();
            reqXml.AppendFormat("<OTA_HotelDescriptiveInfoRQ Version=\"1.0\" xsi:schemaLocation=\"http://www.opentravel.org/OTA/2003/05 OTA_HotelDescriptiveInfoRQ.xsd\" xmlns=\"http://www.opentravel.org/OTA/2003/05\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
            reqXml.Append("<HotelDescriptiveInfos>");

            foreach (var item in req.DescriptiveInfos)
            {
                reqXml.AppendFormat("<HotelDescriptiveInfo HotelCode=\"{0}\" PositionTypeCode=\"502\">", item.HotelCode);
                if (item.SendHotelData != null)
                {
                    reqXml.AppendFormat("<HotelInfo SendData=\"{0}\"/>", item.SendHotelData.ToString().ToLower());
                }
                if (item.SendGuestRooms != null)
                {
                    reqXml.AppendFormat("<FacilityInfo SendGuestRooms=\"{0}\"/>", item.SendGuestRooms.ToString().ToLower());
                }
                if (item.SendAttractions != null || item.SendRecreations != null)
                {
                    reqXml.Append("<AreaInfo");
                    if (item.SendAttractions != null)
                    {
                        reqXml.AppendFormat(" SendAttractions=\"{0}\"", item.SendAttractions.ToString().ToLower());
                    }
                    if (item.SendRecreations != null)
                    {
                        reqXml.AppendFormat(" SendRecreations=\"{0}\"", item.SendRecreations.ToString().ToLower());
                    }
                    reqXml.Append(" />");
                }
                if (item.SendMultimediaObjectsData != null)
                {
                    reqXml.AppendFormat("<MultimediaObjects SendData=\"{0}\"/>", item.SendMultimediaObjectsData.ToString().ToLower());
                }
                reqXml.Append("</HotelDescriptiveInfo>");
            }

            reqXml.Append("</HotelDescriptiveInfos>");
            reqXml.Append("</OTA_HotelDescriptiveInfoRQ>");
            req.RequestContent = reqXml.ToString();
            var remote = ConfigurationManager.AppSettings["ctrip_Api"];
            APICommon apicommon = new APICommon(SID, UID, KEY, remote);
            string repXml = apicommon.GetResult(req);


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(repXml);
            GetHeaderResult(xmlDoc, rep);
            List<HotelDescriptiveInfo> hotelDescList = new List<HotelDescriptiveInfo>();

            var descriptiveContentNodes = xmlDoc.SelectNodes("Response/HotelResponse/OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents/HotelDescriptiveContent");
            foreach (XmlNode descriptiveContentNode in descriptiveContentNodes)
            {
                HotelDescriptiveInfo hotelDesc = new HotelDescriptiveInfo();
                XmlElement descElement = (XmlElement)descriptiveContentNode;
                hotelDesc.AreaID = descElement.GetAttribute("AreaID").ToInt32();
                hotelDesc.BrandCode = descElement.GetAttribute("BrandCode").ToInt32();
                hotelDesc.HotelCode = descElement.GetAttribute("HotelCode").ToInt32();
                hotelDesc.HotelCityCode = descElement.GetAttribute("HotelCityCode").ToInt32();
                hotelDesc.HotelId = hotelDesc.HotelCode;
                hotelDesc.HotelName = descElement.GetAttribute("HotelName").Trim();
                XmlNode hotelInfoNode = descriptiveContentNode.SelectSingleNode("HotelInfo");
                var dt = ((XmlElement)hotelInfoNode).GetAttribute("WhenBuilt").Trim();
                hotelDesc.WhenBuild = ((XmlElement)hotelInfoNode).GetAttribute("WhenBuilt").Trim().ToDateTime();
                var lastUpdate = ((XmlElement)hotelInfoNode).GetAttribute("LastUpdated");
                if (string.IsNullOrEmpty(lastUpdate))
                {
                    hotelDesc.LastUpdated = DateTime.Parse("1900-1-1");
                }
                else
                {
                    hotelDesc.LastUpdated = ((XmlElement)hotelInfoNode).GetAttribute("LastUpdated").Trim().ToDateTime();
                }

                // 地址信息
                var addressNode = hotelInfoNode.SelectSingleNode("Address");
                string address = "";
                if (addressNode.SelectSingleNode("AddressLine") != null)
                {
                    address = addressNode.SelectSingleNode("AddressLine").InnerText;
                }
                hotelDesc.Address = address;

                // 区域信息
                var zoneNodes = addressNode.SelectNodes("Zone");
                hotelDesc.ZoneName = "";
                hotelDesc.ZoneId = 0;
                foreach (XmlNode zn in zoneNodes)
                {

                    if (zn.SelectSingleNode("ZoneCode").InnerText.Trim().ToInt32() > 0)
                    {
                        hotelDesc.ZoneId = zn.SelectSingleNode("ZoneCode").InnerText.Trim().ToInt32();
                        hotelDesc.ZoneName = zn.SelectSingleNode("ZoneName").InnerText.Trim();
                        break;
                    }
                }

                string postCode = "";
                XmlNode postCodeNode = addressNode.SelectSingleNode("PostalCode");
                if (postCodeNode != null)
                {
                    postCode = postCodeNode.InnerText;
                }
                hotelDesc.PostCode = postCode;

                hotelDesc.HotelCityName = "";
                XmlNode hotelCityNameNode = addressNode.SelectSingleNode("CityName");
                if (hotelCityNameNode != null)
                {
                    hotelDesc.HotelCityName = hotelCityNameNode.InnerText.Trim();
                }

                // 一些常用的酒店标签和分类
                var segmentNodes = descriptiveContentNode.SelectNodes("HotelInfo/CategoryCodes/SegmentCategory");
                if (segmentNodes != null)
                {
                    List<string> segmentCategoryList = new List<string>();
                    foreach (XmlNode sn in segmentNodes)
                    {
                        segmentCategoryList.Add(sn.InnerText.Trim());
                    }
                    hotelDesc.SegmentCategoryList = segmentCategoryList;
                }

                XmlNode positionNode = hotelInfoNode.SelectSingleNode("Position");
                hotelDesc.Latitude = ((XmlElement)positionNode).GetAttribute("Latitude").Trim();
                hotelDesc.Longitude = ((XmlElement)positionNode).GetAttribute("Longitude").Trim();
                hotelDesc.PositionTypeCode = ((XmlElement)positionNode).GetAttribute("PositionTypeCode").Trim();

                // 酒店服务和设备
                XmlNodeList serviceNodeList = hotelInfoNode.SelectNodes("Services/Service");
                List<Facility> serviceTypeList = new List<Facility>();
                foreach (XmlNode node in serviceNodeList)
                {
                    serviceTypeList.Add(new Facility()
                    {
                        ID = ((XmlElement)node).GetAttribute("ID").Trim().ToInt32(),
                        Code = ((XmlElement)node).GetAttribute("Code").Trim().ToInt32(),
                        DescriptiveText = node.SelectSingleNode("DescriptiveText").InnerText.Trim()
                    });
                }

                hotelDesc.FacilityList = serviceTypeList;

                // 房型信息列表
                var GuestRoomNodeList = descriptiveContentNode.SelectNodes("FacilityInfo/GuestRooms/GuestRoom");
                List<HotelRoom> guestRoomList = new List<HotelRoom>();
                foreach (XmlNode node in GuestRoomNodeList)
                {
                    HotelRoom room = new HotelRoom();
                    room.RoomTypeName = ((XmlElement)node).GetAttribute("RoomTypeName").Trim();
                    var typeNode = (XmlElement)node.SelectSingleNode("TypeRoom");
                    room.StandardOccupancy = typeNode.GetAttribute("StandardOccupancy").Trim().ToInt32();
                    room.Size = typeNode.GetAttribute("Size").Trim();
                    room.BedTypeCode = typeNode.GetAttribute("BedTypeCode").Trim();
                    room.Floor = typeNode.GetAttribute("Floor").Trim();
                    room.Quantity = typeNode.GetAttribute("Quantity").Trim().ToInt32();
                    room.RoomTypeCode = typeNode.GetAttribute("RoomTypeCode").ToInt32();
                    room.NonSmoking = typeNode.GetAttribute("NonSmoking").Trim().ToLower() == "false" ? 0 : 1;
                    room.RoomSize = typeNode.GetAttribute("RoomSize").Trim().ToInt32();


                    // 酒店房间描述
                    XmlNode descriptionNode = node.SelectSingleNode("Features/Feature/DescriptiveText");
                    if (descriptionNode != null)
                    {
                        room.DescriptiveText = descriptionNode.InnerText.Trim();
                    }

                    // 房间内设施
                    var amenityNodeList = node.SelectNodes("Amenities/Amenity");
                    List<RoomFacility> hotelAmenties = new List<RoomFacility>();
                    RoomFacility amentity = null;
                    foreach (XmlNode an in amenityNodeList)
                    {
                        amentity = new RoomFacility();
                        amentity.Code = ((XmlElement)an).GetAttribute("RoomAmenityCode").Trim();
                        XmlNode descriptiveTextNode = an.SelectSingleNode("DescriptiveText");
                        amentity.DescriptiveText = descriptiveTextNode.InnerText;
                        hotelAmenties.Add(amentity);
                    }

                    room.Amenities = hotelAmenties;

                    // 行政区信息,地标类区域和建筑列表
                    var refPointNodes = descriptiveContentNode.SelectNodes("AreaInfo/RefPoints/RefPoint");
                    List<AreaInfo> refPoints = new List<AreaInfo>();
                    AreaInfo refPoint = null;
                    foreach (XmlNode p in refPointNodes)
                    {
                        refPoint = new AreaInfo();
                        XmlElement pe = (XmlElement)p;
                        refPoint.Distance = pe.GetAttribute("Distance").ToDecimal();
                        refPoint.Latitude = pe.GetAttribute("Latitude").Trim();
                        refPoint.Longitude = pe.GetAttribute("Longitude").Trim();
                        refPoint.Name = pe.GetAttribute("Name").Trim();
                        refPoint.RefPointCategoryCode = pe.GetAttribute("RefPointCategoryCode").Trim().ToInt32();
                        refPoint.RefPointName = pe.GetAttribute("RefPointName").Trim();
                        refPoint.UnitOfMeasureCode = pe.GetAttribute("UnitOfMeasureCode").Trim().ToInt32();
                        var descTextNode = p.SelectSingleNode("DescriptiveText");
                        refPoint.DescriptiveText = descTextNode.InnerText;
                        refPoints.Add(refPoint);
                    }
                    hotelDesc.AreaInfoList = refPoints;

                    // 酒店星级、携程星级、推荐级别
                    // 酒店等级、评分列表
                    var awardsNodes = descriptiveContentNode.SelectNodes("AffiliationInfo/Awards/Award");
                    foreach (XmlElement el in awardsNodes)
                    {
                        string provider = el.GetAttribute("Provider").Trim();
                        decimal rating = el.GetAttribute("Rating").Trim().ToDecimal();

                        switch (provider)
                        {
                            case "HotelStarRate":
                                hotelDesc.HotelStarRate = rating;
                                break;
                            case "CtripStarRate":
                                hotelDesc.CtripStarRate = rating;
                                break;
                            case "CtripUserRate":
                                hotelDesc.CtripUserRate = rating;
                                break;
                            case "CtripCommRate":
                                hotelDesc.CtripCommRate = rating;
                                break;
                            case "CommSurroundingRate":
                                hotelDesc.CommSurroundingRate = rating;
                                break;
                            case "CommFacilityRate":
                                hotelDesc.CommFacilityRate = rating;
                                break;
                            case "CommCleanRate":
                                hotelDesc.CommCleanRate = rating;
                                break;
                            case "CommServiceRate":
                                hotelDesc.CommServiceRate = rating;
                                break;
                            case "HotelStarLicence":
                                hotelDesc.HotelStarLicence = rating;
                                break;
                        }
                    }
                    //hotelDesc.am
                    guestRoomList.Add(room);

                }

                hotelDesc.HotelRoomList = guestRoomList;

                var hotelTextNodes = descriptiveContentNode.SelectNodes("MultimediaDescriptions/MultimediaDescription/TextItems/TextItem");
                List<MultimediaTextDescription> multimediaTextList = new List<MultimediaTextDescription>();

                MultimediaTextDescription multimediaText;
                foreach (XmlNode textNode in hotelTextNodes)
                {
                    multimediaText = new MultimediaTextDescription();
                    multimediaText.Category = ((XmlElement)textNode).GetAttribute("Category").Trim().ToInt32();
                    var descNode = textNode.SelectSingleNode("Description");
                    if (descNode != null)
                    {
                        multimediaText.Text = descNode.InnerText.Trim();
                    }

                    var urlNode = textNode.SelectSingleNode("URL");
                    if (urlNode != null)
                    {
                        multimediaText.Text = urlNode.InnerText.Trim();
                    }

                    multimediaTextList.Add(multimediaText);
                }
                hotelDesc.MultimediaTextDescriptionList = multimediaTextList;

                var hotelImgNodes = descriptiveContentNode.SelectNodes("MultimediaDescriptions/MultimediaDescription/ImageItems/ImageItem");
                List<MultimediaImgDescription> multimediaImgList = new List<MultimediaImgDescription>();
                MultimediaImgDescription multimediaImg = null;
                foreach (XmlNode imgNode in hotelImgNodes)
                {
                    multimediaImg = new MultimediaImgDescription();
                    multimediaImg.Category = ((XmlElement)imgNode).GetAttribute("Category").Trim();
                    multimediaImg.Caption = ((XmlElement)imgNode.SelectSingleNode("Description")).GetAttribute("Caption").Trim();
                    multimediaImg.Url = imgNode.SelectSingleNode("ImageFormat/URL").InnerText.Trim();
                    multimediaImg.Description = ((XmlElement)imgNode.SelectSingleNode("Description")).InnerText.Trim();
                    multimediaImgList.Add(multimediaImg);
                }
                hotelDesc.MultimediaImgDescriptionList = multimediaImgList;

                var policyNode = descriptiveContentNode.SelectSingleNode("Policies/Policy");
                HotelPolicy hotelPolicy = new HotelPolicy();
                List<PolicyInfoCode> policies = new List<PolicyInfoCode>();
                PolicyInfoCode polocyCode;
                if (policyNode != null)
                {
                    var policyCodeNodes = policyNode.SelectNodes("PolicyInfoCodes/PolicyInfoCode");
                    if (policyCodeNodes != null && policyCodeNodes.Count > 0)
                    {
                        foreach (XmlElement xe in policyCodeNodes)
                        {
                            polocyCode = new PolicyInfoCode();
                            polocyCode.Code = xe.GetAttribute("Code").Trim();

                            var policyDescNodes = xe.SelectNodes("Description");
                            List<Tuple<string, string>> descriptions = new List<Tuple<string, string>>();
                            if (policyDescNodes != null && policyDescNodes.Count > 0)
                            {
                                foreach (XmlElement pd in policyDescNodes)
                                {
                                    descriptions.Add(new Tuple<string, string>(pd.GetAttribute("Name").Trim(), pd.SelectSingleNode("Text").InnerText.Trim()));
                                }
                            }
                            polocyCode.Descriptions = descriptions;
                            policies.Add(polocyCode);
                        }

                    }

                    var polocyInfoNode = policyNode.SelectSingleNode("PolicyInfo") as XmlElement;
                    PolicyInfo polocyInfo = new PolicyInfo();
                    if (polocyInfoNode != null)
                    {
                        polocyInfo.CheckOutTime = polocyInfoNode.GetAttribute("CheckInTime").Trim();
                        polocyInfo.CheckInTime = polocyInfoNode.GetAttribute("CheckOutTime").Trim();
                    }
                    hotelPolicy.PolicyInfo = polocyInfo;
                }
                hotelPolicy.PolicyInfoCodes = policies;
                hotelDesc.PolicyInfo = hotelPolicy;

                hotelDescList.Add(hotelDesc);
            }
            rep.DescriptiveInfos = hotelDescList;
            return rep;
        }

        /// <summary>
        /// 根据查询条件查询可用的Rate Plan，包括房价、可卖渠道。通常用于双方系统保持一致性
        /// 参数Start,End,HotelCode为必填；参数AvailRatesOnlyInd为空时值为false
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public OTA_HotelRatePlanRS OTA_HotelRatePlan(OTA_HotelRatePlanRQ req)
        {
            var rep = new OTA_HotelRatePlanRS();
            try
            {
                StringBuilder reqXml = new StringBuilder("<ns:OTA_HotelRatePlanRQ TimeStamp=\"2013-06-01T00:00:00.000+08:00\" Version=\"1.0\">");
                reqXml.Append("<ns:RatePlans>");
                foreach (var item in req.HotelRatePlanList)
                {
                    reqXml.Append("<ns:RatePlan>");
                    reqXml.AppendFormat("<ns:DateRange Start=\"{0}\" End=\"{1}\"/>", item.StartDate.ToString("yyyy-MM-dd"), item.EndDate.ToString("yyyy-MM-dd"));
                    reqXml.Append("<ns:RatePlanCandidates>");
                    reqXml.AppendFormat("<ns:RatePlanCandidate AvailRatesOnlyInd=\"{0}\" >", item.AvailRatesOnlyInd.ToString().ToLower());
                    reqXml.Append("<ns:HotelRefs>");
                    reqXml.AppendFormat("<ns:HotelRef HotelCode=\"{0}\"/>", item.HotelCode);
                    reqXml.Append("</ns:HotelRefs>");
                    reqXml.Append("</ns:RatePlanCandidate>");
                    reqXml.Append("</ns:RatePlanCandidates>");
                    reqXml.Append("<ns:TPA_Extensions RestrictedDisplayIndicator=\"false\"/>");
                    reqXml.Append("</ns:RatePlan>");
                }
                reqXml.Append("</ns:RatePlans>");
                reqXml.Append("</ns:OTA_HotelRatePlanRQ>");

                req.RequestContent = reqXml.ToString();
                var remote = ConfigurationManager.AppSettings["ctrip_Api"];
                APICommon apicommon = new APICommon(SID, UID, KEY, remote);
                string repXml = apicommon.GetResult(req);
                //LogHelper.Debug(repXml);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(repXml);
                GetHeaderResult(xmlDoc, rep);
                var hotelRatePlanNodes = xmlDoc.SelectNodes("Response/HotelResponse/OTA_HotelRatePlanRS/RatePlans");
                List<HotelRatePlan> hotelRatePlanList = new List<HotelRatePlan>();

                HotelRatePlan ratePlan = null;

                foreach (XmlNode hp in hotelRatePlanNodes)
                {
                    ratePlan = new HotelRatePlan();
                    ratePlan.HotelCode = ((XmlElement)hp).GetAttribute("HotelCode").Trim().ToInt32();
                    var roomRatePlanNodes = hp.SelectNodes("RatePlan");
                    List<RoomRatePlan> roomRatePlanList = new List<RoomRatePlan>();
                    RoomRatePlan roomRatePlan = null;
                    foreach (XmlNode rp in roomRatePlanNodes)
                    {
                        roomRatePlan = new RoomRatePlan();

                        XmlNode desNode = rp.SelectSingleNode("Description/Text");
                        if (desNode != null)
                        {
                            roomRatePlan.Description = desNode.InnerText.Trim();
                        }
                        roomRatePlan.MarketCode = ((XmlElement)rp).GetAttribute("MarketCode").Trim().ToInt32();
                        roomRatePlan.RatePlanCategory = ((XmlElement)rp).GetAttribute("RatePlanCategory").Trim().ToInt32();
                        roomRatePlan.HotelRoomCode = ((XmlElement)rp).GetAttribute("RatePlanCode").Trim().ToInt32();

                        var SellableProductNodes = rp.SelectNodes("SellableProducts/SellableProduct");
                        List<string> sellableProductList = new List<string>();
                        if (SellableProductNodes != null)
                        {
                            foreach (XmlNode n in SellableProductNodes)
                            {
                                sellableProductList.Add(((XmlElement)n).GetAttribute("InvCode").Trim());
                            }
                        }
                        roomRatePlan.SellableProductList = sellableProductList;

                        List<RoomRate> roomRateList = new List<RoomRate>();
                        var roomRateNodes = rp.SelectNodes("Rates/Rate");
                        RoomRate roomRate = null;
                        foreach (XmlNode rn in roomRateNodes)
                        {
                            roomRate = new RoomRate();
                            roomRate.StartTime = ((XmlElement)rn).GetAttribute("Start").Trim().ToDateTime();
                            roomRate.EndTime = ((XmlElement)rn).GetAttribute("End").Trim().ToDateTime();
                            roomRate.Status = ((XmlElement)rn).GetAttribute("Status").Trim();
                            roomRate.IsInstantConfirm = ((XmlElement)rn).GetAttribute("IsInstantConfirm").Trim() == "false" ? 0 : 1;

                            XmlElement baseByGuestAmtNode = (XmlElement)rn.SelectSingleNode("BaseByGuestAmts/BaseByGuestAmt");
                            roomRate.AmountBeforeTax = baseByGuestAmtNode.GetAttribute("AmountBeforeTax").Trim().ToDecimal();
                            roomRate.CurrencyCode = baseByGuestAmtNode.GetAttribute("CurrencyCode").Trim();
                            roomRate.NumberOfGuests = baseByGuestAmtNode.GetAttribute("NumberOfGuests").Trim().ToInt32();
                            string listprice = baseByGuestAmtNode.GetAttribute("ListPrice").Trim();

                            if (!string.IsNullOrEmpty(listprice))
                            {
                                roomRate.ListPrice = listprice.ToDecimal();
                            }

                            XmlNode CancelPenaltyNode = rn.SelectSingleNode("CancelPolicies/CancelPenalty");

                            if (CancelPenaltyNode != null)
                            {
                                roomRate.CancelPenaltyStartTime = ((XmlElement)CancelPenaltyNode).GetAttribute("Start").Trim().ToDateTime();
                                roomRate.CancelPenaltyEndTime = ((XmlElement)CancelPenaltyNode).GetAttribute("End").Trim().ToDateTime();

                                XmlNode AmountPercentNode = CancelPenaltyNode.SelectSingleNode("AmountPercent");
                                roomRate.CancelAmount = ((XmlElement)AmountPercentNode).GetAttribute("Amount").Trim().ToDecimal();
                                roomRate.CancelCurrencyCode = ((XmlElement)AmountPercentNode).GetAttribute("CurrencyCode").Trim();

                                XmlElement GuaranteePolicyEle = (XmlElement)rn.SelectSingleNode("GuaranteePolicies/GuaranteePolicy");
                                if (GuaranteePolicyEle != null)
                                {

                                    roomRate.GuaranteeCode = GuaranteePolicyEle.GetAttribute("GuaranteeCode").Trim();
                                    roomRate.HoldTime = GuaranteePolicyEle.GetAttribute("HoldTime").Trim();
                                }

                            }
                            else
                            {
                                roomRate.CancelPenaltyStartTime = DateTime.Parse("1900-1-1");
                                roomRate.CancelPenaltyEndTime = DateTime.Parse("1900-1-1");
                                roomRate.CancelAmount = 0;
                                roomRate.CancelCurrencyCode = "";
                                roomRate.GuaranteeCode = "";
                                roomRate.HoldTime = "1900-1-1";
                            }


                            XmlElement MealsIncludedEle = (XmlElement)rn.SelectSingleNode("MealsIncluded");
                            roomRate.Breakfast = MealsIncludedEle.GetAttribute("Breakfast").Trim();
                            roomRate.NumberOfBreakfast = MealsIncludedEle.GetAttribute("NumberOfBreakfast").Trim().ToInt32();


                            List<Pertain> pertainList = new List<Pertain>();
                            var feeNodes = rn.SelectNodes("Fees/Fee");
                            Pertain pertain = null;
                            foreach (XmlNode fn in feeNodes)
                            {
                                pertain = new Pertain();
                                pertain.Amount = ((XmlElement)fn).GetAttribute("Amount").Trim().ToDecimal();
                                pertain.ChargeUnit = ((XmlElement)fn).GetAttribute("ChargeUnit").Trim();
                                pertain.CurrencyCode = ((XmlElement)fn).GetAttribute("CurrencyCode").Trim();
                                pertain.Code = ((XmlElement)fn).GetAttribute("Code").Trim();
                                pertain.Description = fn.SelectSingleNode("Description/Text").InnerText.Trim();
                                pertainList.Add(pertain);
                            }

                            roomRate.PertainList = pertainList;

                            XmlNode RebatePromotionNode = rn.SelectSingleNode("TPA_Extensions/RebatePromotion");
                            if (RebatePromotionNode != null)
                            {
                                roomRate.StartPeriod = ((XmlElement)RebatePromotionNode).GetAttribute("StartPeriod").Trim().ToDateTime();
                                roomRate.EndPeriod = ((XmlElement)RebatePromotionNode).GetAttribute("EndPeriod").Trim().ToDateTime();
                                roomRate.BackCurrencyCode = ((XmlElement)RebatePromotionNode).GetAttribute("CurrencyCode").Trim();
                                roomRate.BackCode = ((XmlElement)RebatePromotionNode).GetAttribute("Code").Trim();
                                roomRate.ProgramName = ((XmlElement)RebatePromotionNode).GetAttribute("ProgramName").Trim();
                                roomRate.BackAmount = ((XmlElement)RebatePromotionNode).GetAttribute("Amount").Trim().ToDecimal();
                                roomRate.BackDescription = RebatePromotionNode.SelectSingleNode("Description/Text").InnerText.Trim();
                                roomRate.RoomCode = roomRatePlan.HotelRoomCode;
                            }
                            else
                            {
                                roomRate.StartPeriod = DateTime.Parse("1900-1-1");
                                roomRate.EndPeriod = DateTime.Parse("1900-1-1");
                                roomRate.BackCurrencyCode = "";
                                roomRate.BackCode = "";
                                roomRate.ProgramName = "";
                                roomRate.BackAmount = 0;
                                roomRate.BackDescription = "";
                                roomRate.RoomCode = 0;

                            }
                            roomRateList.Add(roomRate);

                        }
                        roomRatePlan.RoomRateList = roomRateList;
                        roomRatePlanList.Add(roomRatePlan);
                    }
                    ratePlan.RoomRatePlanList = roomRatePlanList;
                    hotelRatePlanList.Add(ratePlan);
                }
                rep.HotelRatePlanList = hotelRatePlanList;
            }
            catch
            {
                throw;
            }
            return rep;
        }

        /// <summary>
        /// 返回在查询时间戳到现在这个时间段、及要查酒店所在酒店城市ID号下的所有酒店价格缓存变化通知列表
        /// 若请求参数：HotelCode存在，则查询的是该酒店在该时间段内价格缓存变化列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public OTA_HotelCacheChangeRS OTA_HotelCacheChange(OTA_HotelCacheChangeRQ req)
        {
            var rep = new OTA_HotelCacheChangeRS();
            StringBuilder reqXml = new StringBuilder();
            reqXml.Append("<ns:OTA_HotelCacheChangeRQ Version=\"1.0\">");
            reqXml.AppendFormat("<ns:CacheSearchCriteria CacheFromTimestamp=\"{0}\">", req.CacheFromTimestamp.GetDateTimeFormats('s')[0].ToString());
            reqXml.AppendFormat("<ns:CacheSearchCriterion HotelCityCode=\"{0}\"", req.HotelCityCode);
            if (req.HotelCode > 0)
            {
                reqXml.AppendFormat(" HotelCode=\"{0}\"", req.HotelCode);
            }
            reqXml.Append("/>");
            reqXml.Append("</ns:CacheSearchCriteria>");
            reqXml.Append("</ns:OTA_HotelCacheChangeRQ>");
            req.RequestContent = reqXml.ToString();

            var remote = ConfigurationManager.AppSettings["ctrip_Api"];
            APICommon apicommon = new APICommon(SID, UID, KEY, remote);
            string repXml = apicommon.GetResult(req);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(repXml);

            GetHeaderResult(xmlDoc, rep);

            var rsNode = xmlDoc.SelectSingleNode("Response/HotelResponse/OTA_HotelCacheChangeRS");
            string timeSpan = ((XmlElement)rsNode).GetAttribute("TimeStamp");
            var changeNodes = rsNode.SelectNodes("CacheChangeInfo");

            var changeItems = new List<HotelCacheChange>();
            HotelCacheChange changeItem = null;
            foreach (XmlNode node in changeNodes)
            {
                changeItem = new HotelCacheChange();
                changeItem.HotelCode = ((XmlElement)node).GetAttribute("HotelCode").Trim();
                var timeSpanElement = (XmlElement)node.SelectSingleNode("TimeSpan");
                changeItem.StartTime = timeSpanElement.GetAttribute("Start").Trim().ToDateTime();
                changeItem.EndTime = timeSpanElement.GetAttribute("End").Trim().ToDateTime();
                var otherInfoNode = (XmlElement)node.SelectSingleNode("OtherInfo");
                changeItem.RatePlanCode = otherInfoNode.GetAttribute("RatePlanCode");
                changeItems.Add(changeItem);
            }
            rep.HotelCacheChangeList = changeItems;
            return rep;
        }

        /// <summary>
        /// 基于查询条件，查询酒店可卖的房间产品（RoomStay），包括价格信息、房型信息等
        /// 建议在开始预订时或提交订单时调用可订检查
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public OTA_HotelAvailRS OTA_HotelAvail(OTA_HotelAvailRQ req)
        {
            var rep = new OTA_HotelAvailRS();
            StringBuilder reqXml = new StringBuilder();
            reqXml.AppendFormat("<ns:OTA_HotelAvailRQ Version=\"1.0\" TimeStamp=\"{0}\">", "2012-04-20T00:00:00.000+08:00");
            reqXml.Append("<ns:AvailRequestSegments>");
            reqXml.Append("<ns:AvailRequestSegment>");
            reqXml.Append("<ns:HotelSearchCriteria>");
            reqXml.Append("<ns:Criterion>");

            reqXml.AppendFormat("<ns:HotelRef HotelCode=\"{0}\"/>", req.HotelCode);
            reqXml.AppendFormat("<ns:StayDateRange Start=\"{0}\" End=\"{1}\"/>", GetCtripTime(req.CheckInTime), GetCtripTime(req.CheckOutTime));
            reqXml.Append("<ns:RatePlanCandidates>");
            reqXml.AppendFormat("<ns:RatePlanCandidate RatePlanCode=\"{0}\"/>", req.HotelRoomCode);
            reqXml.Append("</ns:RatePlanCandidates>");
            reqXml.Append("<ns:RoomStayCandidates>");

            reqXml.AppendFormat("<ns:RoomStayCandidate Quantity=\"{0}\">", req.RoomCount);
            reqXml.Append("<ns:GuestCounts");
            if (req.IsPerRoom != null)
            {
                reqXml.AppendFormat("IsPerRoom=\"{0}\"", req.IsPerRoom.ToString().ToLower());
            }
            reqXml.Append(">");
            reqXml.AppendFormat("<ns:GuestCount Count=\"{0}\"/>", req.GuestCount);
            reqXml.Append("</ns:GuestCounts>");

            reqXml.Append("</ns:RoomStayCandidate>");
            reqXml.Append("</ns:RoomStayCandidates>");
            reqXml.Append("<ns:TPA_Extensions>");
            reqXml.AppendFormat("<ns:LateArrivalTime>{0}</ns:LateArrivalTime>", GetCtripTime(req.LastCheckInTime));
            reqXml.Append("</ns:TPA_Extensions>");

            reqXml.Append("</ns:Criterion>");
            reqXml.Append("</ns:HotelSearchCriteria>");
            reqXml.Append("</ns:AvailRequestSegment>");
            reqXml.Append("</ns:AvailRequestSegments>");
            reqXml.Append("</ns:OTA_HotelAvailRQ>");


            req.RequestContent = reqXml.ToString();

            var remote = ConfigurationManager.AppSettings["ctrip_Api"];
            APICommon apicommon = new APICommon(SID, UID, KEY, remote);
            string repXml = apicommon.GetResult(req);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(repXml);

            var errorsNode = xmlDoc.SelectSingleNode("Response/HotelResponse/OTA_HotelAvailRS/Errors");
            if (errorsNode != null)
            {
                return rep;
            }

            var roomStayNode = xmlDoc.SelectSingleNode("Response/HotelResponse/OTA_HotelAvailRS/RoomStays/RoomStay");
            rep.AvailabilityStatus = ((XmlElement)roomStayNode).GetAttribute("AvailabilityStatus").Trim();
            XmlElement totalNode = (XmlElement)roomStayNode.SelectSingleNode("Total");
            rep.TotalAmountBeforeTax = totalNode.GetAttribute("AmountBeforeTax").Trim().ToDecimal();
            rep.CurrencyCode = totalNode.GetAttribute("CurrencyCode").Trim();

            var ratePlanNode = roomStayNode.SelectSingleNode("RatePlans/RatePlan");

            rep.RatePlanCode = ((XmlElement)ratePlanNode).GetAttribute("RatePlanCode").Trim();
            rep.HotelRoomCode = rep.RatePlanCode;
            rep.RoomTypeCode = rep.RatePlanCode;
            rep.RatePlanName = ((XmlElement)ratePlanNode).GetAttribute("RatePlanName").Trim();
            rep.PrepaidIndicator = ((XmlElement)ratePlanNode).GetAttribute("PrepaidIndicator").Trim();

            rep.Breakfast = ((XmlElement)ratePlanNode.SelectSingleNode("MealsIncluded")).GetAttribute("Breakfast");

            var roomTypenode = roomStayNode.SelectSingleNode("RoomTypes/RoomType");
            rep.RoomTypeCode = ((XmlElement)roomTypenode).GetAttribute("RoomTypeCode").Trim();
            rep.RoomTypeName = ((XmlElement)roomTypenode).GetAttribute("RoomType").Trim();

            var roomTypeDescriptionTextNode = roomTypenode.SelectSingleNode("RoomDescription/Text");
            if (roomTypeDescriptionTextNode != null)
            {
                rep.RoomTypeDescriptiveText = roomTypeDescriptionTextNode.InnerText.Trim();
            }

            var ratesNodes = roomStayNode.SelectNodes("RoomRates/RoomRate/Rates/Rate");
            List<AvaiRoomRate> avaiRoomRates = new List<AvaiRoomRate>();
            AvaiRoomRate roomRate = null;
            foreach (XmlNode n in ratesNodes)
            {
                roomRate = new AvaiRoomRate();
                roomRate.EffectiveDate = ((XmlElement)n).GetAttribute("EffectiveDate").Trim().ToDateTime();
                roomRate.ExpireDate = ((XmlElement)n).GetAttribute("ExpireDate").Trim().ToDateTime();
                roomRate.MaxGuestApplicable = ((XmlElement)n).GetAttribute("MaxGuestApplicable").Trim();

                List<Pertain> pertains = new List<Pertain>();
                var feeNodes = n.SelectNodes("Fees/Fee");
                Pertain pertain = null;
                foreach (XmlNode f in feeNodes)
                {
                    pertain = new Pertain();
                    pertain.Amount = ((XmlElement)f).GetAttribute("Amount").Trim().ToDecimal();
                    pertain.ChargeUnit = ((XmlElement)f).GetAttribute("ChargeUnit").Trim();
                    pertain.Code = ((XmlElement)f).GetAttribute("Code").Trim();
                    pertain.CurrencyCode = ((XmlElement)f).GetAttribute("CurrencyCode").Trim();
                    XmlNode descText = f.SelectSingleNode("Description/Text");
                    if (descText != null)
                    {
                        pertain.Description = descText.InnerText.Trim();
                    }
                    pertains.Add(pertain);
                }
                roomRate.PertainList = pertains;
                avaiRoomRates.Add(roomRate);
            }

            rep.RoomRateList = avaiRoomRates;

            XmlNode guaranteePaymentNode = roomStayNode.SelectSingleNode("DepositPayments/GuaranteePayment");
            if (guaranteePaymentNode != null)
            {
                GuaranteePayment guaranteePayment = new GuaranteePayment();
                guaranteePayment.StartTime = ((XmlElement)guaranteePaymentNode).GetAttribute("Start").Trim().ToDateTime();
                guaranteePayment.EndTime = ((XmlElement)guaranteePaymentNode).GetAttribute("End").Trim().ToDateTime();
                guaranteePayment.GuaranteeCode = ((XmlElement)guaranteePaymentNode).GetAttribute("GuaranteeCode").Trim();
                //guaranteePayment.GuaranteeCurrencyCode = ((XmlElement)guaranteePaymentNode).GetAttribute("GuaranteeCode").Trim();
                XmlNode amountPercentNode = guaranteePaymentNode.SelectSingleNode("AmountPercent");
                guaranteePayment.GuaranteeAmount = ((XmlElement)amountPercentNode).GetAttribute("Amount").Trim().ToDecimal();
                guaranteePayment.GuaranteeCurrencyCode = ((XmlElement)amountPercentNode).GetAttribute("CurrencyCode").Trim();
                XmlNode g_text = amountPercentNode.SelectSingleNode("Description/Text");
                if (g_text != null)
                {
                    guaranteePayment.Description = g_text.InnerText.Trim();
                }
                rep.GuaranteePayment = guaranteePayment;
            }


            XmlNode cancelPenaltyNode = roomStayNode.SelectSingleNode("CancelPenalties/CancelPenalty");
            if (cancelPenaltyNode != null)
            {
                CancelPenalty cancelPenalty = new CancelPenalty();

                cancelPenalty.StartTime = ((XmlElement)cancelPenaltyNode).GetAttribute("Start").ToDateTime();
                cancelPenalty.EndTime = ((XmlElement)cancelPenaltyNode).GetAttribute("End").ToDateTime();
                cancelPenalty.GuaranteeCode = ((XmlElement)cancelPenaltyNode).GetAttribute("GuaranteeCode").Trim();
                XmlNode c_AmountPercentNode = cancelPenaltyNode.SelectSingleNode("AmountPercent");
                cancelPenalty.AmountPercent = ((XmlElement)c_AmountPercentNode).GetAttribute("Amount").Trim().ToDecimal();
                cancelPenalty.CurrencyCode = ((XmlElement)c_AmountPercentNode).GetAttribute("CurrencyCode").Trim();
                XmlNode c_node = c_AmountPercentNode.SelectSingleNode("Description/Text");
                if (c_node != null)
                {
                    cancelPenalty.Description = c_node.InnerText.Trim();
                }
                rep.CancelPenalty = cancelPenalty;
            }

            return rep;
        }


        string GetCtripTime(DateTime dt)
        {
            return string.Format("{0}.000+08:00", dt.GetDateTimeFormats('s')[0].ToString());
        }
        protected void GetHeaderResult(XmlDocument xmlDoc, BaseReturnEntity returnEntity)
        {

            try
            {
                XmlElement headerNode = (XmlElement)xmlDoc.SelectSingleNode("Response/Header");

                string ShouldRecordPerformanceTime = headerNode.GetAttribute("ShouldRecordPerformanceTime");
                string timestamp = headerNode.GetAttribute("Timestamp");
                string ReferenceID = headerNode.GetAttribute("ReferenceID");
                string RecentlyTime = headerNode.GetAttribute("RecentlyTime");
                string AccessCount = headerNode.GetAttribute("AccessCount");
                string CurrentCount = headerNode.GetAttribute("CurrentCount");
                string ResetTime = headerNode.GetAttribute("ResetTime");
                string ResultCode = headerNode.GetAttribute("ResultCode");
                string ResultMsg = string.IsNullOrEmpty(headerNode.GetAttribute("ResultMsg")) ? "" : headerNode.GetAttribute("ResultMsg").Trim();
                string ResultNo = string.IsNullOrEmpty(headerNode.GetAttribute("ResultCode")) ? "" : headerNode.GetAttribute("ResultCode").Trim();

                ReturnHeaderInfo headerInfo = new ReturnHeaderInfo(ReferenceID, ResultCode, ResultNo, ResultMsg, timestamp);
                headerInfo.ShouldRecordPerformanceTime = ShouldRecordPerformanceTime;
                headerInfo.AccessCount = !string.IsNullOrWhiteSpace(headerNode.GetAttribute("AccessCount")) ? Convert.ToInt32(headerNode.GetAttribute("AccessCount").Trim()) : 0;
                headerInfo.CurrentCount = !string.IsNullOrWhiteSpace(headerNode.GetAttribute("CurrentCount")) ? Convert.ToInt32(headerNode.GetAttribute("CurrentCount").Trim()) : 0;
                headerInfo.ResetTime = !string.IsNullOrWhiteSpace(headerNode.GetAttribute("ResetTime")) ? headerNode.GetAttribute("ResetTime").Trim() : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                headerInfo.RecentlyTime = !string.IsNullOrWhiteSpace(headerNode.GetAttribute("RecentlyTime")) ? headerNode.GetAttribute("RecentlyTime").Trim() : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                returnEntity.GetReturnHeaderInfo(headerInfo);

            }
            catch
            {
                throw;
            }

        }
    }
}

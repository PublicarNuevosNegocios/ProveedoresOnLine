using MarketPlace.Models.General;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Company
{
    public class ThirdKnowledgeQueryResultViewModel
    {
        public TDQueryModel oRelatedInfo { get; set; }
        //Cosl

        public string oRequestName { get; set; }
        public string RequestName
        {
            get
            {
                if (string.IsNullOrEmpty(oRequestName))
                {
                    oRequestName = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.RequestName)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oRequestName;
            }
        }
        public string oIdNumberRequest { get; set; }
        public string IdNumberRequest
        {
            get
            {
                if (string.IsNullOrEmpty(oIdNumberRequest))
                {
                    oIdNumberRequest = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdNumberRequest)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oIdNumberRequest;
            }
        }

        public string oQueryId { get; set; }
        public string QueryId 
        {
            get
            {
                if (string.IsNullOrEmpty(oQueryId))
                {
                    oQueryId = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.QueryId)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oQueryId;
            }
        }
        public string oIdGroup { get; set; }
        public string IdGroup 
        {
            get
            {
                if (string.IsNullOrEmpty(oIdGroup))
                {
                    oIdGroup = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.GroupNumber)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oIdGroup;
            }
        }
        public string oGroupName { get; set; }
        public string GroupName 
        {
            get
            {
                if (string.IsNullOrEmpty(oGroupName))
                {
                    oGroupName = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.GroupName)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oGroupName;
            }        
        }
        public string oPriority { get; set; }
        public string Priority 
        {
            get
            {
                if (string.IsNullOrEmpty(oPriority))
                {
                    oPriority = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Priotity)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oPriority;
            }  
        }
        public string oTypeDocument { get; set; }
        public string TypeDocument 
        {
            get
            {
                if (string.IsNullOrEmpty(oTypeDocument))
                {
                    oTypeDocument = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.TypeDocument)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oTypeDocument;
            }  
        }
        public string oIdentificationNumberResult { get; set; }
        public string IdentificationNumberResult 
        {
            get
            {
                if (string.IsNullOrEmpty(oIdentificationNumberResult))
                {
                    oIdentificationNumberResult = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdNumberResult)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oIdentificationNumberResult;
            }  
        }

        public string oNameResult { get; set; }
        public string NameResult 
        {
            get
            {
                if (string.IsNullOrEmpty(oNameResult))
                {
                    oNameResult = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.NameResult)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oNameResult;
            }  
        }
        public string oIdList { get; set; }
        public string IdList 
        {
            get
            {
                if (string.IsNullOrEmpty(oIdList))
                {
                    oIdList = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.IdList)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oIdList;
            }  
        }
        public string oListName { get; set; }
        public string ListName 
        {
            get
            {
                if (string.IsNullOrEmpty(oListName))
                {
                    oListName = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.ListName)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oListName;
            }  
        }
        public string oAlias { get; set; }
        public string Alias 
        {
            get
            {
                if (string.IsNullOrEmpty(oAlias))
                {
                    oAlias = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Alias)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oAlias;
            }  
        }
        public string oOffense { get; set; }
        public string Offense 
        {
            get
            {
                if (string.IsNullOrEmpty(oOffense))
                {
                    oOffense = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Offense)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oOffense;
            }  
        }

        public string oPeps { get; set; }
        public string Peps 
        {
            get
            {
                if (string.IsNullOrEmpty(oPeps))
                {
                    oPeps = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Peps)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oPeps;
            }          
        }
        public string oStatus { get; set; }
        public string Status 
        {
            get
            {
                if (string.IsNullOrEmpty(oStatus))
                {
                    oStatus = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Status)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oStatus;
            }  
        }
        public string oZone { get; set; }
        public string Zone 
        {
            get
            {
                if (string.IsNullOrEmpty(oZone))
                {
                    oZone = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Zone)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oZone;
            } 
        }
        public string oLink { get; set; }
        public string Link 
        {
            get
            {
                if (string.IsNullOrEmpty(oLink))
                {
                    oLink = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.Link)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oLink;
            } 
        }
        public string oMoreInfo { get; set; }
        public string MoreInfo 
        {
            get
            {
                if (string.IsNullOrEmpty(oMoreInfo))
                {
                    oMoreInfo = oRelatedInfo.RelatedQueryInfoModel.Where(x => x.ItemInfoType.ItemId == (int)enumThirdKnowledgeColls.MoreInfo)
                        .Select(x => x.Value).DefaultIfEmpty("").FirstOrDefault();
                }
                return oMoreInfo;
            } 
        }
        public string RegisterDate { get; set; }
        public string LastModifyDate { get; set; }
        public string Message { get; set; }
        public string FileURL { get; set; }

        public ThirdKnowledgeQueryResultViewModel(TDQueryModel oCollumnsResult)
        {
            oRelatedInfo = oCollumnsResult;
        }

        public ThirdKnowledgeQueryResultViewModel()
        {

        }
    }
}

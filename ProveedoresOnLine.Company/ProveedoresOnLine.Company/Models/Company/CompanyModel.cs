using ProveedoresOnLine.Company.Models.Role;
using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Models.Company
{
    public class CompanyModel
    {
        public string CompanyPublicId { get; set; }

        public string CompanyName { get; set; }

        public CatalogModel IdentificationType { get; set; }

        public string IdentificationNumber { get; set; }

        public CatalogModel CompanyType { get; set; }

        public bool Enable { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<GenericItemInfoModel> CompanyInfo { get; set; }

        public List<GenericItemModel> RelatedContact { get; set; }

        public List<GenericItemModel> RelatedRole { get; set; }

        public RoleCompanyModel RelatedCompanyRole { get; set; }

        public List<UserCompany> RelatedUser { get; set; }
    }
}

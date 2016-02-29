using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Admin
{
    public class AdminRoleCompanyViewModel
    {
        ProveedoresOnLine.Company.Models.Role.RoleCompanyModel RelatedRoleCompany { get; set; }

        public int TotalRows { get; set; }

        public string RoleCompanyId { get; set; }

        public string RoleCompanyName { get; set; }

        public string ParentRoleCompany { get; set; }

        public string RelatedCompanyPublicId { get; set; }

        public string RelatedCompanyName { get; set; }

        public bool Enable { get; set; }

        public string LastModify { get; set; }

        public string CreateDate { get; set; }

        public AdminRoleCompanyViewModel() { }

        public AdminRoleCompanyViewModel(ProveedoresOnLine.Company.Models.Role.RoleCompanyModel oRelatedRoleCompany, int oTotalRows)
        {
            TotalRows = oTotalRows;

            RelatedRoleCompany = oRelatedRoleCompany;

            RoleCompanyId = RelatedRoleCompany.RoleCompanyId.ToString();

            RoleCompanyName = RelatedRoleCompany.RoleCompanyName;

            ParentRoleCompany = RelatedRoleCompany.ParentRoleCompany.ToString();

            Enable = RelatedRoleCompany.Enable;

            LastModify = RelatedRoleCompany.LastModify.ToString();

            CreateDate = RelatedRoleCompany.CreateDate.ToString();

            RelatedCompanyPublicId = RelatedRoleCompany.RelatedCompany.CompanyPublicId;

            RelatedCompanyName = RelatedRoleCompany.RelatedCompany.CompanyName;
        }
    }
}

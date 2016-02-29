using ProveedoresOnLine.Company.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Admin
{
    public class AdminRoleModuleViewModel
    {
        ProveedoresOnLine.Company.Models.Role.RoleModuleModel RelatedRoleModule { get; set; }

        public int TotalRows { get; set; }

        #region RoleModule Info

        public string RoleModuleId { get; set; }

        public string RoleModule { get; set; }

        public string RoleModuleTypeId { get; set; }

        public string RoleModuleTypeName { get; set; }

        public bool Enable { get; set; }

        public string LastModify { get; set; }

        public string CreateDate { get; set; }

        #endregion

        public AdminRoleModuleViewModel() { }

        public AdminRoleModuleViewModel(ProveedoresOnLine.Company.Models.Role.RoleModuleModel oRelatedRoleModule)
        {
            RelatedRoleModule = oRelatedRoleModule;

            this.RoleModuleId = RelatedRoleModule.RoleModuleId.ToString();

            this.RoleModule = RelatedRoleModule.RoleModule;

            this.RoleModuleTypeId = RelatedRoleModule.RoleModuleType.ItemId.ToString();

            this.RoleModuleTypeName = RelatedRoleModule.RoleModuleType.ItemName;

            this.Enable = RelatedRoleModule.Enable;

            this.LastModify = RelatedRoleModule.LastModify.ToString();

            this.CreateDate = RelatedRoleModule.CreateDate.ToString();
        }
    }
}

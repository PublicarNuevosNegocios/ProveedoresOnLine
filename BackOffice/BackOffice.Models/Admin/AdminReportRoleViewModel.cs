using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Admin
{
    public class AdminReportRoleViewModel
    {
        ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedReporRole { get; set; }

        public string ReportRoleId { get; set; }

        public string ReportRole { get; set; }

        public string ReportRoleTypeId { get; set; }

        public string ReportRoleTypeName { get; set; }

        public bool Enable { get; set; }

        public string LastModify { get; set; }

        public string CreateDate { get; set; }

        public AdminReportRoleViewModel() { }

        public AdminReportRoleViewModel (ProveedoresOnLine.Company.Models.Util.GenericItemModel oRelatedReportRole)
        {
            RelatedReporRole = oRelatedReportRole;

            this.ReportRoleId = RelatedReporRole.ItemId.ToString();

            this.ReportRole = RelatedReporRole.ItemName;

            this.ReportRoleTypeId = RelatedReporRole.ItemType.ItemId.ToString();

            this.ReportRoleTypeName = RelatedReporRole.ItemType.ItemName;

            this.Enable = RelatedReporRole.Enable;

            this.LastModify = RelatedReporRole.LastModify.ToString();

            this.CreateDate = RelatedReporRole.CreateDate.ToString();
        }
    }
}

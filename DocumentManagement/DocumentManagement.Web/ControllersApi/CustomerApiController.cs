using DocumentManagement.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DocumentManagement.Web.ControllersApi
{
    public class CustomerApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public CustomerSearchModel CustomerSearch
            (string CustomerSearchVal, string SearchParam, int PageNumber, int RowCount)
        {
            CustomerSearchModel oReturn = new CustomerSearchModel();

            int oTotalRows;
            oReturn.RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerSearch
                (SearchParam, PageNumber, RowCount, out oTotalRows);

            oReturn.TotalRows = oTotalRows;


            return oReturn;

        }

        [HttpPost]
        [HttpGet]
        public FormSearchModel FormSearch
            (string FormSearchVal, string CustomerPublicId, string SearchParam, int PageNumber, int RowCount)
        {
            FormSearchModel oReturn = new FormSearchModel();

            int oTotalRows;
            oReturn.RelatedForm = DocumentManagement.Customer.Controller.Customer.FormSearch
                (CustomerPublicId, SearchParam, PageNumber, RowCount, out oTotalRows);

            oReturn.TotalRows = oTotalRows;


            return oReturn;

        }

        [HttpPost]
        [HttpGet]
        public StepSearchModel StepSearch
            (string StepSearchVal, string FormPublicId)
        {
            StepSearchModel oReturn = new StepSearchModel();

            oReturn.RelatedStep = DocumentManagement.Customer.Controller.Customer.StepGetByFormId(FormPublicId);

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public int StepCreate
            (string StepCreateVal, string FormPublicId, string Name, int Position)
        {
            if (!string.IsNullOrEmpty(StepCreateVal) && StepCreateVal == "true")
            {
                return DocumentManagement.Customer.Controller.Customer.StepCreate
                    (FormPublicId,
                    new DocumentManagement.Customer.Models.Form.StepModel()
                    {
                        Name = Name,
                        Position = Convert.ToInt32(Position),
                    });
            }
            else
            {
                return 0;
            }
        }

        [HttpPost]
        [HttpGet]
        public void StepModify
            (string StepModifyVal, string StepId, string Name, string Position)
        {
            if (!string.IsNullOrEmpty(StepModifyVal) && StepModifyVal == "true")
            {
                DocumentManagement.Customer.Controller.Customer.StepModify
                    (new DocumentManagement.Customer.Models.Form.StepModel()
                    {
                        StepId = Convert.ToInt32(StepId),
                        Name = Name,
                        Position = Convert.ToInt32(Position),
                    });
            }
        }

        [HttpPost]
        [HttpGet]
        public void StepDelete
            (string StepDeleteVal, string StepId)
        {
            if (!string.IsNullOrEmpty(StepDeleteVal) && StepDeleteVal == "true")
            {
                DocumentManagement.Customer.Controller.Customer.StepDelete(Convert.ToInt32(StepId));
            }
        }

        [HttpPost]
        [HttpGet]
        public FieldSearchModel FieldSearch
            (string FieldSearchVal, string StepId)
        {
            FieldSearchModel oReturn = new FieldSearchModel();

            oReturn.RelatedField = DocumentManagement.Customer.Controller.Customer.FieldGetByStepId(Convert.ToInt32(StepId));

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public FieldSearchModel GetFieldOptions
            (string GetFieldOptionsVal, string FormPublicId)
        {
            FieldSearchModel oReturn = new FieldSearchModel()
            {
                ProviderInfoType = new List<Customer.Models.Util.CatalogModel>(),
            };

            DocumentManagement.Customer.Models.Customer.CustomerModel oCustomerAux =
                DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId(FormPublicId);

            List<DocumentManagement.Customer.Models.Util.CatalogModel> oItemsAux =
                DocumentManagement.Customer.Controller.Customer.CatalogGetCustomerOptions();

            oItemsAux.Where(cat => cat.CatalogId == 3).OrderBy(itm => itm.ItemName).All(itm =>
            {
                if (!oCustomerAux.RelatedForm.Any(f => f.RelatedStep.Any(s => s.RelatedField.Any(fi => fi.ProviderInfoType.ItemId == itm.ItemId))))
                {
                    oReturn.ProviderInfoType.Add(new Customer.Models.Util.CatalogModel()
                    {
                        ItemId = itm.ItemId,
                        ItemName = itm.ItemName,
                    });
                }
                return true;
            });

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public int FieldCreate
            (string FieldSearchVal, string StepId, string Name, string ProviderInfoType, string IsRequired, string Position)
        {
            if (!string.IsNullOrEmpty(FieldSearchVal) && FieldSearchVal == "true")
            {
                return DocumentManagement.Customer.Controller.Customer.FieldCreate
                    (Convert.ToInt32(StepId),
                    new DocumentManagement.Customer.Models.Form.FieldModel()
                    {
                        Name = Name,
                        ProviderInfoType = new Customer.Models.Util.CatalogModel()
                        {
                            ItemId = Convert.ToInt32(ProviderInfoType),
                        },
                        IsRequired = Convert.ToBoolean(IsRequired),
                        Position = Convert.ToInt32(Position),
                    });
            }
            else
            {
                return 0;
            }
        }

        [HttpPost]
        [HttpGet]
        public void FieldDelete
            (string FieldDeleteVal, string FieldId)
        {
            if (!string.IsNullOrEmpty(FieldDeleteVal) && FieldDeleteVal == "true")
            {
                DocumentManagement.Customer.Controller.Customer.FieldDelete(Convert.ToInt32(FieldId));
            }
        }
    }
}
﻿using DocumentManagement.Customer.Models.Customer;
using DocumentManagement.Customer.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Customer.Controller
{
    public class Customer
    {
        #region Customer

        public static string CustomerUpsert(CustomerModel CustomerToUpsert)
        {
            return DAL.Controller.CustomerDataController.Instance.CustomerUpsert
                (CustomerToUpsert.CustomerPublicId,
                CustomerToUpsert.Name,
                CustomerToUpsert.IdentificationType.ItemId,
                CustomerToUpsert.IdentificationNumber);
        }

        public static List<Models.Customer.CustomerModel> CustomerSearch(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CustomerDataController.Instance.CustomerSearch
                (SearchParam,
                PageNumber,
                RowCount,
                out TotalRows);
        }

        public static Models.Customer.CustomerModel CustomerGetByFormId(string FormPublicId)
        {
            return DAL.Controller.CustomerDataController.Instance.CustomerGetByFormId(FormPublicId);
        }

        public static Models.Customer.CustomerModel CustomerGetById(string CustomerPublicId)
        {
            return DAL.Controller.CustomerDataController.Instance.CustomerGetById(CustomerPublicId);
        }

        #endregion

        #region Form

        public static string FormUpsert(string CustomerPublicId, FormModel FormToUpsert)
        {
            return DAL.Controller.CustomerDataController.Instance.FormUpsert
                (FormToUpsert.FormPublicId,
                CustomerPublicId,
                FormToUpsert.Name,
                FormToUpsert.TermsAndConditions,
                FormToUpsert.Logo);
        }

        public static int StepCreate(string FormPublicId, StepModel StepToUpsert)
        {
            return DAL.Controller.CustomerDataController.Instance.StepCreate
                (FormPublicId,
                StepToUpsert.Name,
                StepToUpsert.Position);
        }

        public static void StepCreate(int StepId)
        {
            DAL.Controller.CustomerDataController.Instance.StepDelete(StepId);
        }

        public static int FieldCreate(int StepId, FieldModel FieldToUpsert)
        {
            return DAL.Controller.CustomerDataController.Instance.FieldCreate
                (StepId,
                FieldToUpsert.Name,
                FieldToUpsert.ProviderInfoType.ItemId,
                FieldToUpsert.FieldType.ItemId,
                FieldToUpsert.IsRequired,
                FieldToUpsert.Position);
        }

        public static void FieldCreate(int FieldId)
        {
            DAL.Controller.CustomerDataController.Instance.FieldDelete(FieldId);
        }

        public static List<DocumentManagement.Customer.Models.Form.FormModel> FormSearch(string CustomerPublicId, string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CustomerDataController.Instance.FormSearch
                (CustomerPublicId,
                SearchParam,
                PageNumber,
                RowCount,
                out TotalRows);
        }

        #endregion
    }
}

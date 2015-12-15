using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderFormModel
    {
        public bool RenderScripts { get; set; }

        public DocumentManagement.Customer.Models.Customer.CustomerModel RealtedCustomer { get; set; }

        public DocumentManagement.Customer.Models.Form.FormModel RealtedForm { get; set; }

        public DocumentManagement.Customer.Models.Form.StepModel RealtedStep { get; set; }

        public DocumentManagement.Provider.Models.Provider.ProviderModel RealtedProvider { get; set; }

        public Dictionary<int, List<DocumentManagement.Provider.Models.Util.CatalogModel>> ProviderOptions { get; set; }

        private static List<string> oFieldTypes;
        public static List<string> FieldTypes
        {
            get
            {
                if (oFieldTypes == null)
                {
                    oFieldTypes = DocumentManagement.Models.General.InternalSettings.Instance
                        [DocumentManagement.Models.General.Constants.C_Settings_Field_Types].
                        Value.
                        Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                        ToList();
                }
                return oFieldTypes;
            }
        }

        private static Dictionary<string, List<int>> oFieldInfoTypes;
        public static Dictionary<string, List<int>> FieldInfoTypes
        {
            get
            {
                if (oFieldInfoTypes == null)
                {
                    oFieldInfoTypes = new Dictionary<string, List<int>>();

                    FieldTypes.All(ft =>
                    {
                        oFieldInfoTypes.Add
                            (ft,
                            DocumentManagement.Models.General.InternalSettings.Instance
                                [ft].
                                Value.
                                Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                                Select(x => Convert.ToInt32(x.Replace(" ", ""))).
                                ToList()
                            );
                        return true;
                    });
                }
                return oFieldInfoTypes;
            }
        }

        public string errorMessage { get; set; }

        /// <summary>
        /// InfoType, Item, Value
        /// </summary>
        public Tuple<int,string,string> PrevFormModel { get; set; }
    }
}

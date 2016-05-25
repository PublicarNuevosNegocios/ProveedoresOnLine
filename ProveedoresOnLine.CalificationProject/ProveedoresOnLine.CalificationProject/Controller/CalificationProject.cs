using ProveedoresOnLine.CalificationProject.Models.CalificationProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationProject.Controller
{
    public class CalificationProject
    {
        #region ConfigItem

        public ConfigItemModel CalificationProjectConfigItemUpsert(ConfigItemModel oConfigItemModel)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

            try
            {
                if (oConfigItemModel != null)
                {
                    oConfigItemModel.CalificationProjectConfigItemId = DAL.Controller.CalificationProjectDataController.Instance.CalificationProjectConfigItemUpsert
                        (oConfigItemModel.CalificationProjectConfigId,
                        oConfigItemModel.CalificationProjectConfigItemId,
                        oConfigItemModel.CalificationProjectConfigItemName,
                        oConfigItemModel.CalificationProjectConfigItemType.ItemId,
                        oConfigItemModel.Enable);

                    oLog.IsSuccess = true;
                }
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = oConfigItemModel;

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "CalificationProjectConfigItemId",
                    Value = oConfigItemModel.CalificationProjectConfigItemId.ToString(),
                });

                LogManager.ClientLog.AddLog(oLog);
            }

            return oConfigItemModel;
        }

        #endregion
    }
}

/*init compare Menu*/
function Compare_InitMenu(InitObject) {
    $('#' + InitObject.ObjId).accordion({
        animate: 'swing',
        header: 'label',
        active: InitObject.active
    });
}

var Compare_SearchObject = {

    ObjectId: '',
    RowCount: 0,
    CompareUrl: '',

    Init: function (vInitObject) {

        this.ObjectId = vInitObject.ObjectId;
        this.RowCount = vInitObject.RowCount;
        this.CompareUrl = vInitObject.CompareUrl;
    },

    RenderAsync: function () {

        //load grid comparison
        $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').kendoGrid({
            editable: false,
            navigatable: false,
            pageable: true,
            scrollable: true,
            selectable: true,
            toolbar: [
                { name: 'Search', template: $('#' + Compare_SearchObject.ObjectId + '_SearchGrid_Header_Template').html() },
            ],
            dataSource: {
                pageSize: Compare_SearchObject.RowCount,
                serverPaging: true,
                schema: {
                    total: function (data) {
                        if (data != null && data.length > 0) {
                            return data[0].TotalRows;
                        }
                        return 0;
                    },
                    model: {
                        id: 'CompareId',
                        fields: {
                            CompareId: { editable: false, nullable: true },
                            CompareName: { editable: false, nullable: true },
                            LastModify: { editable: false, nullable: true },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        var oSearchParam = $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').find('input[type=text]').val();

                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CompareApi?CMCompareSearch=true&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                            }
                        });
                    },
                },
            },
            change: function (arg) {
                var odataItem = this.dataItem(this.select());
                if (odataItem != null && odataItem.CompareId != null && odataItem.CompareId > 0) {
                    var oUrl = Compare_SearchObject.CompareUrl;
                    oUrl += '?CompareId=' + odataItem.CompareId;
                    window.location = oUrl;
                }
            },
            columns: [{
                field: 'CompareName',
                title: 'Nombre',
                width: '50px',
            }, {
                field: 'LastModify',
                title: 'Última modificación',
                width: '50px',
            }],
        });

        //add search methods
        $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').find('input[type=text]').keydown(function (e) {
            if (e.keyCode == 13) {
                //enter action search
                $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').data('kendoGrid').dataSource.read();
            }
        });

        $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').find('a').click(function (e) {
            //action search
            $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').data('kendoGrid').dataSource.read();
        });
    },
};


var Compare_DetailObject = {
    ObjectId: '',
    CompareId: '',
    CompareDetailUrl: '',
    RelatedCompany: new Array(),

    Init: function (vInitObject) {

        this.ObjectId = vInitObject.ObjectId;
        this.CompareId = vInitObject.CompareId;
        this.CompareDetailUrl = vInitObject.CompareDetailUrl;

        if (vInitObject.RelatedProvider != null) {
            $.each(vInitObject.RelatedProvider, function (item, value) {
                Compare_DetailObject.RelatedCompany[value.RelatedProvider.RelatedCompany.CompanyPublicId] = value;
            });
        }
    },

    GetHeaderTemplate: function (vColumName) {
        var oItemHtml = '';
        if (vColumName == 'EvaluationArea') {
            oItemHtml = 'Area de evaluación';
        }
        else {
            oItemHtml = $('#' + Compare_DetailObject.ObjectId + '_Company_Header_Template').html();

            oItemHtml = oItemHtml.replace(/{ProviderPublicId}/gi, Compare_DetailObject.RelatedCompany[vColumName].RelatedProvider.RelatedCompany.CompanyPublicId);
            oItemHtml = oItemHtml.replace(/{ProviderLogoUrl}/gi, Compare_DetailObject.RelatedCompany[vColumName].ProviderLogoUrl);
            oItemHtml = oItemHtml.replace(/{CompanyName}/gi, Compare_DetailObject.RelatedCompany[vColumName].RelatedProvider.RelatedCompany.CompanyName);
            oItemHtml = oItemHtml.replace(/{IdentificationType}/gi, Compare_DetailObject.RelatedCompany[vColumName].RelatedProvider.RelatedCompany.IdentificationType.ItemName);
            oItemHtml = oItemHtml.replace(/{IdentificationNumber}/gi, Compare_DetailObject.RelatedCompany[vColumName].RelatedProvider.RelatedCompany.IdentificationNumber);
            oItemHtml = oItemHtml.replace(/{ProviderRateClass}/gi, 'rateit');
            oItemHtml = oItemHtml.replace(/{ProviderRate}/gi, Compare_DetailObject.RelatedCompany[vColumName].ProviderRate.replace(/,/gi,'.'));

            //validate item certified
            if (Compare_DetailObject.RelatedCompany[vColumName].ProviderIsCertified != null && Compare_DetailObject.RelatedCompany[vColumName].ProviderIsCertified == true) {
                oItemHtml = oItemHtml.replace(/{ProviderIsCertified}/gi, '');
            }
            else {
                oItemHtml = oItemHtml.replace(/{ProviderIsCertified}/gi, 'none');
            }

            //validate black list
            if (Compare_DetailObject.RelatedCompany[vColumName].ProviderAlertRisk != Provider_SearchObject.BlackListStatusShowAlert) {
                oItemHtml = oItemHtml.replace(/{ProviderAlertRisk}/gi, 'none');
            }
            else {
                oItemHtml = oItemHtml.replace(/{ProviderAlertRisk}/gi, '');
            }
        }
        return oItemHtml;
    },

    GetItemTemplate: function (vColumName) {
        var oReturn = '';

        if (vColumName == 'EvaluationArea') {
            oReturn = $('#' + Compare_DetailObject .ObjectId+ '_EvaluationArea_Item_Template').html();
        }
        else {
            var oReturn = $('#' + Compare_DetailObject.ObjectId + '_Company_Item_Template').html();
            oReturn = oReturn.replace(/{ProviderPublicId}/gi, vColumName);
        }

        return oReturn;
    },

    CompareDetailSearch: function () {
        var oUrl = Compare_DetailObject.CompareDetailUrl;

        oUrl += '&Currency=' + $('#' + Compare_DetailObject.ObjectId + '_Currency').val();
        oUrl += '&Year=' + $('#' + Compare_DetailObject.ObjectId + '_Year').val();

        window.location = oUrl;
    },

    RemoveCompareProvider: function (vProviderPublicId) {
        if (Compare_DetailObject.CompareId != null && Compare_DetailObject.CompareId.length > 0) {
            //remove company from existing compare process
            $.ajax({
                url: BaseUrl.ApiUrl + '/CompareApi?CMCompareRemoveCompany=true&CompareId=' + Compare_DetailObject.CompareId + '&ProviderPublicId=' + vProviderPublicId,
                dataType: 'json',
                success: function (result) {
                    if (result != null) {
                        Compare_DetailObject.CompareDetailSearch();
                    }
                },
                error: function (result) {
                }
            });
        }
    },
};

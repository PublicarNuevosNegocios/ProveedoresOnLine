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
    SearchUrl: '',
    CompareStatus: '',
    CompareId: '',
    CompareUrl: '',
    SearchParam: '',
    SearchFilter: '',
    SearchOrderType: '',
    OrderOrientation: false,
    PageNumber: 0,
    RowCount: 0,

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
        this.CompareId = vInitObject.CompareId;
        this.CompareUrl = vInitObject.CompareUrl;
        this.SearchParam = vInitObject.SearchParam;
        this.SearchFilter = vInitObject.SearchFilter;
        this.SearchOrderType = vInitObject.SearchOrderType;
        this.OrderOrientation = vInitObject.OrderOrientation;
        this.PageNumber = vInitObject.PageNumber;
        this.RowCount = vInitObject.RowCount;
    },

    RenderAsync2: function () {
        //init Search input
        $('#' + Compare_SearchObject.ObjectId + '_txtSearchBox').keydown(function (e) {
            if (e.keyCode == 13) {
                //enter action search
                Compare_SearchObject.Search2();
            }
        });

        //init search orient controls
        $('input[name="Search_rbOrder"]').change(function () {
            if ($(this) != null && $(this).attr('searchordertype') != null && $(this).attr('orderorientation') != null) {
                Compare_SearchObject.Search2({
                    SearchOrderType: $(this).attr('searchordertype'),
                    OrderOrientation: $(this).attr('orderorientation')
                });
            }
        });
    },

    Search2: function (vSearchObject) {
        /*get serach param*/
        if (this.SearchParam != $('#' + Compare_SearchObject.ObjectId + '_txtSearchBox').val()) {
            /*Init pager*/
            this.PageNumber = 0;
        }
        this.SearchParam = $('#' + Compare_SearchObject.ObjectId + '_txtSearchBox').val();

        if (vSearchObject != null) {
            /*get filter values*/
            if (vSearchObject.SearchFilter != null) {
                if (vSearchObject.SearchFilter.Enable == true) {
                    this.SearchFilter += ',' + vSearchObject.SearchFilter.Value;
                }
                else {
                    this.SearchFilter = this.SearchFilter.replace(new RegExp(vSearchObject.SearchFilter.Value, 'gi'), '').replace(/,,/gi, '');
                }

                /*Init pager*/
                this.PageNumber = 0;
            }

            /*get order*/
            if (vSearchObject.SearchOrderType != null) {
                this.SearchOrderType = vSearchObject.SearchOrderType;
            }
            if (vSearchObject.OrderOrientation != null) {
                this.OrderOrientation = vSearchObject.OrderOrientation;
            }

            /*get page*/
            if (vSearchObject.PageNumber != null) {
                this.PageNumber = vSearchObject.PageNumber;
            }
        }
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
                title: 'Última Modificación',
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
            oItemHtml = 'ÁREA DE EVALUACIÓN';
        }
        else {
            oItemHtml = $('#' + Compare_DetailObject.ObjectId + '_Company_Header_Template').html();

            oItemHtml = oItemHtml.replace(/{ProviderPublicId}/gi, Compare_DetailObject.RelatedCompany[vColumName].RelatedProvider.RelatedCompany.CompanyPublicId);
            oItemHtml = oItemHtml.replace(/{ProviderLogoUrl}/gi, Compare_DetailObject.RelatedCompany[vColumName].ProviderLogoUrl);
            oItemHtml = oItemHtml.replace(/{CompanyName}/gi, Compare_DetailObject.RelatedCompany[vColumName].RelatedProvider.RelatedCompany.CompanyName);
            oItemHtml = oItemHtml.replace(/{IdentificationType}/gi, Compare_DetailObject.RelatedCompany[vColumName].RelatedProvider.RelatedCompany.IdentificationType.ItemName);
            oItemHtml = oItemHtml.replace(/{IdentificationNumber}/gi, Compare_DetailObject.RelatedCompany[vColumName].RelatedProvider.RelatedCompany.IdentificationNumber);
            oItemHtml = oItemHtml.replace(/{ProviderRateClass}/gi, 'rateit');
            oItemHtml = oItemHtml.replace(/{ProviderRate}/gi, Compare_DetailObject.RelatedCompany[vColumName].ProviderRate);
            oItemHtml = oItemHtml.replace(/{ProviderRateCount}/gi, Compare_DetailObject.RelatedCompany[vColumName].ProviderRateCount);

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
            oReturn = $('#' + Compare_DetailObject.ObjectId + '_EvaluationArea_Item_Template').html();
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
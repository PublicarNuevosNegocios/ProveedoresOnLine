/*init provider Menu*/
function Provider_InitMenu(InitObject) {
    $('#' + InitObject.ObjId).accordion({
        animate: 'swing',
        header: 'label',
        active: InitObject.active
    });
}

var Provider_SearchObject = {

    ObjectId: '',
    SearchUrl: '',
    CompareId: '',
    CompareUrl: '',
    SearchParam: '',
    SearchFilter: '',
    SearchOrderType: '',
    OrderOrientation: false,
    PageNumber: 0,
    RowCount: 0,

    BlackListStatusShowAlert: '',

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

        this.BlackListStatusShowAlert = vInitObject.BlackListStatusShowAlert;
    },

    RenderAsync: function () {

        //init Search input
        $('#' + Provider_SearchObject.ObjectId + '_txtSearchBox').keydown(function (e) {
            if (e.keyCode == 13) {
                //enter action search
                Provider_SearchObject.Search();
            }
        });


        //init search orient controls
        $('input[name="Search_rbOrder"]').change(function () {
            if ($(this) != null && $(this).attr('searchordertype') != null && $(this).attr('orderorientation') != null) {
                Provider_SearchObject.Search({
                    SearchOrderType: $(this).attr('searchordertype'),
                    OrderOrientation: $(this).attr('orderorientation')
                });
            }
        });
    },

    /*{SearchFilter{Enable,Value},SearchOrderType,OrderOrientation,PageNumber}*/
    Search: function (vSearchObject) {
        /*get serach param*/
        if (this.SearchParam != $('#' + Provider_SearchObject.ObjectId + '_txtSearchBox').val()) {
            /*Init pager*/
            this.PageNumber = 0;
        }
        this.SearchParam = $('#' + Provider_SearchObject.ObjectId + '_txtSearchBox').val();

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
        window.location = Provider_SearchObject.GetSearchUrl();
    },

    GetSearchUrl: function () {

        var oUrl = this.SearchUrl;

        oUrl += '?CompareId=' + this.CompareId;
        oUrl += '&SearchParam=' + this.SearchParam;
        oUrl += '&SearchFilter=' + this.SearchFilter;
        oUrl += '&SearchOrderType=' + this.SearchOrderType;
        oUrl += '&OrderOrientation=' + this.OrderOrientation;
        oUrl += '&PageNumber=' + this.PageNumber;

        return oUrl;
    },

    /*****************************Compare search methods************************************************/

    OpenCompare: function (vCompareId) {
        $.ajax({
            url: BaseUrl.ApiUrl + '/CompareApi?CMCompareGet=true&CompareId=' + vCompareId,
            dataType: 'json',
            success: function (result) {
                if (result != null) {
                    //set compare id
                    Provider_SearchObject.CompareId = result.CompareId;

                    //show compare action
                    $('.' + Provider_SearchObject.ObjectId + '_Compare_SelActionCompare').show();

                    //set compare name and show
                    $('#' + Provider_SearchObject.ObjectId + '_Compare_CompareName').val(result.CompareName);
                    $('#' + Provider_SearchObject.ObjectId + '_Compare_CompareName').show();

                    //clean compare items
                    $('#' + Provider_SearchObject.ObjectId + '_Compare_ItemContainer').html('');

                    //show all compare search button
                    $("a[href*='Provider_SearchObject.AddCompareProvider']").show();

                    //render compare items
                    $.each(result.RelatedProvider, function (item, value) {
                        //get item html
                        var oItemHtml = $('#' + Provider_SearchObject.ObjectId + '_Compare_Item_Template').html();

                        //replace provider info
                        oItemHtml = oItemHtml.replace(/{ProviderPublicId}/gi, value.RelatedProvider.RelatedCompany.CompanyPublicId);
                        oItemHtml = oItemHtml.replace(/{ProviderLogoUrl}/gi, value.ProviderLogoUrl);
                        oItemHtml = oItemHtml.replace(/{CompanyName}/gi, value.RelatedProvider.RelatedCompany.CompanyName);
                        oItemHtml = oItemHtml.replace(/{IdentificationType}/gi, value.RelatedProvider.RelatedCompany.IdentificationType.ItemName);
                        oItemHtml = oItemHtml.replace(/{IdentificationNumber}/gi, value.RelatedProvider.RelatedCompany.IdentificationNumber);

                        //validate item certified
                        if (value.ProviderIsCertified != null && value.ProviderIsCertified == true) {
                            oItemHtml = oItemHtml.replace(/{ProviderIsCertified}/gi, '');
                        }
                        else {
                            oItemHtml = oItemHtml.replace(/{ProviderIsCertified}/gi, 'none');
                        }

                        //validate black list
                        if (value.ProviderAlertRisk != Provider_SearchObject.BlackListStatusShowAlert) {
                            oItemHtml = oItemHtml.replace(/{ProviderAlertRisk}/gi, 'none');
                        }
                        else {
                            oItemHtml = oItemHtml.replace(/{ProviderAlertRisk}/gi, '');
                        }


                        $('#' + Provider_SearchObject.ObjectId + '_Compare_ItemContainer').append(oItemHtml);

                        //remove search result add comparison button
                        $("a[href*='Provider_SearchObject.AddCompareProvider(\\\'" + value.RelatedProvider.RelatedCompany.CompanyPublicId + "\\\')']").hide();
                    });

                    //init generic tooltip
                    Tooltip_InitGeneric();
                }
            },
            error: function (result) {
            }
        });
    },

    ShowCompareCreate: function (vProviderPublicId) {
        //clean compare name
        $('#' + Provider_SearchObject.ObjectId + '_Compare_Create_ToolTip_Name').val('');

        //open new compare dialog
        $('#' + Provider_SearchObject.ObjectId + '_Compare_Create_ToolTip').dialog({
            modal: true,
            buttons: {
                'Cancelar': function () {
                    $(this).dialog('close');
                },
                'Guardar': function () {
                    var oCompareName = $('#' + Provider_SearchObject.ObjectId + '_Compare_Create_ToolTip_Name').val();

                    if (oCompareName != null && oCompareName.replace(/ /gi, '') != '') {

                        //create new compare
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CompareApi?CMCompareUpsert=true&CompareId=&CompareName=' + oCompareName + '&ProviderPublicId=' + vProviderPublicId,
                            dataType: 'json',
                            success: function (result) {
                                if (result != null) {
                                    Provider_SearchObject.OpenCompare(result);
                                }
                                $('#' + Provider_SearchObject.ObjectId + '_Compare_Create_ToolTip').dialog('close');
                            },
                            error: function (result) {
                                $('#' + Provider_SearchObject.ObjectId + '_Compare_Create_ToolTip').dialog('close');
                            }
                        });
                    }
                }
            }
        });
    },

    ShowSearchCompare: function () {

        //load grid comparison
        $('#' + Provider_SearchObject.ObjectId + '_Compare_Search_ToolTip_Grid').kendoGrid({
            editable: false,
            navigatable: false,
            pageable: true,
            scrollable: true,
            selectable: true,
            toolbar: [
                { name: 'Search', template: $('#' + Provider_SearchObject.ObjectId + '_Compare_Search_ToolTip_Grid_Header_Template').html() },
            ],
            dataSource: {
                pageSize: Provider_SearchObject.RowCount,
                serverPaging: true,
                height: '400px',
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
                        var oSearchParam = $('#' + Provider_SearchObject.ObjectId + '_Compare_Search_ToolTip_Grid').find('input[type=text]').val();

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
                    //open selected compare
                    Provider_SearchObject.OpenCompare(odataItem.CompareId);
                    //close dialog
                    $('#' + Provider_SearchObject.ObjectId + '_Compare_Search_ToolTip').dialog('close');
                    //destroy kendo grid
                    $('#' + Provider_SearchObject.ObjectId + '_Compare_Search_ToolTip_Grid').data('kendoGrid').destroy();
                }
            },
            columns: [{
                field: 'CompareName',
                title: 'Nombre',
            }, {
                field: 'LastModify',
                title: 'Modificado',
                width: '110px',
            }],
        });

        //add search methods
        $('#' + Provider_SearchObject.ObjectId + '_Compare_Search_ToolTip_Grid').find('input[type=text]').keydown(function (e) {
            if (e.keyCode == 13) {
                //enter action search
                $('#' + Provider_SearchObject.ObjectId + '_Compare_Search_ToolTip_Grid').data('kendoGrid').dataSource.read();
            }
        });

        $('#' + Provider_SearchObject.ObjectId + '_Compare_Search_ToolTip_Grid').find('a').click(function (e) {
            //action search
            $('#' + Provider_SearchObject.ObjectId + '_Compare_Search_ToolTip_Grid').data('kendoGrid').dataSource.read();
        });

        //show open compare dialog
        $('#' + Provider_SearchObject.ObjectId + '_Compare_Search_ToolTip').dialog({
            width: 650,
            minWidth: 500,
            modal: true,
        });
    },

    UpdateCompare: function (vCompareName) {

    },

    AddCompareProvider: function (vProviderPublicId) {
        if (Provider_SearchObject.CompareId != null && Provider_SearchObject.CompareId.length > 0) {
            //add company to existing compare process
            $.ajax({
                url: BaseUrl.ApiUrl + '/CompareApi?CMCompareAddCompany=true&CompareId=' + Provider_SearchObject.CompareId + '&ProviderPublicId=' + vProviderPublicId,
                dataType: 'json',
                success: function (result) {
                    if (result != null) {
                        Provider_SearchObject.OpenCompare(Provider_SearchObject.CompareId);
                    }
                },
                error: function (result) {
                }
            });
        }
        else {
            //new compare process
            Provider_SearchObject.ShowCompareCreate(vProviderPublicId);
        }
    },

    RemoveCompareProvider: function (vProviderPublicId) {
        if (Provider_SearchObject.CompareId != null && Provider_SearchObject.CompareId.length > 0) {
            //remove company from existing compare process
            $.ajax({
                url: BaseUrl.ApiUrl + '/CompareApi?CMCompareRemoveCompany=true&CompareId=' + Provider_SearchObject.CompareId + '&ProviderPublicId=' + vProviderPublicId,
                dataType: 'json',
                success: function (result) {
                    if (result != null) {
                        Provider_SearchObject.OpenCompare(Provider_SearchObject.CompareId);
                    }
                },
                error: function (result) {
                }
            });
        }
    },

    GoToCompare: function () {

        if (Provider_SearchObject.CompareId != null && Provider_SearchObject.CompareId.length > 0) {

            var oUrl = this.CompareUrl;

            oUrl += '?CompareId=' + this.CompareId;

            window.location = oUrl;
        }
    },

    /*****************************Compare search methods end************************************************/
};

var Provider_FinancialObject = {

    ObjectId: '',
    QueryUrl: '',

    Init: function (vInitObject) {

        this.ObjectId = vInitObject.ObjectId;
        this.QueryUrl = vInitObject.QueryUrl;
    },

    BalanceSheet_Search: function (vViewName) {

        var oYear = $('#' + Provider_FinancialObject.ObjectId + '_Year').val();
        var oCurrency = $('#' + Provider_FinancialObject.ObjectId + '_Currency').val();
        var oUrl = Provider_FinancialObject.QueryUrl.replace(/V_ViewName/gi, vViewName).replace(/V_Year/gi, oYear).replace(/V_Currency/gi, oCurrency)

        window.location = oUrl;
    },
};

var Provider_TrackingObject = {
    ObjectId: '',
    ProviderPublicId: '',

    Init: function (vTrackingObject) {
        this.ObjectId = vTrackingObject.ObjectId;
        this.ProviderPublicId = vTrackingObject.ProviderPublicId
    },

    RenderAsync: function () {
        //load grid comparison
        $('#' + Provider_TrackingObject.ObjectId + '_SearchGrid').kendoGrid({
            editable: false,
            navigatable: false,
            pageable: false,
            scrollable: true,
            selectable: true,
            dataSource: {
                serverPaging: true,
                schema: {
                    total: function (data) {
                        if (data != null && data.length > 0) {
                            return data[0].TotalRows;
                        }
                        return 0;
                    },
                    model: {
                        id: 'ItemId',
                        fields: {
                            ItemId: { editable: false, nullable: false },
                            ItemName: { editable: false, nullable: false },
                            CreateDate: { editable: false, nullable: false },
                            ItemName: { editable: false, nullable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?GITrackingInfo=true&ProviderPublicId=' + Provider_TrackingObject.ProviderPublicId,
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
            columns: [{
                field: 'ItemName',
                title: 'Seguimiento',
                width: '50%',
            }, {
                field: 'ItemType.ItemName',
                title: 'Estado',
                width: '25%',
            }, {
                field: 'CreateDate',
                title: 'Fecha',
                width: '25%',
                template: "#= kendo.toString(kendo.parseDate(CreateDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
            }],
        });
    },
};

var Provider_SurveySearchObject = {

    ObjectId: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
    },

    RenderAsync: function () {
        //show generic progress bar
        ProgressBar_Generic_Show();
        //change event over order
        $('#' + Provider_SurveySearchObject.ObjectId + '_Order').change(function () {
            Provider_SurveySearchObject.Search(null);
        });
    },

    Search: function (vSearchObject) {
        var oUrl = this.SearchUrl;

        oUrl += '&SearchOrderType=' + $('#' + Provider_SurveySearchObject.ObjectId + '_Order').val().split('_')[0];
        oUrl += '&OrderOrientation=' + $('#' + Provider_SurveySearchObject.ObjectId + '_Order').val().split('_')[1];

        if (vSearchObject != null && vSearchObject.PageNumber != null) {
            oUrl += '&PageNumber=' + vSearchObject.PageNumber;
        }
        window.location = oUrl;
    },
};
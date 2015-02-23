﻿/*init provider Menu*/
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
        this.SearchParam = $('#' + Provider_SearchObject.ObjectId + '_txtSearchBox').val();

        if (vSearchObject != null) {
            /*get filter values*/
            if (vSearchObject.SearchFilter != null) {
                if (vSearchObject.SearchFilter.Enable == true) {
                    this.SearchFilter += vSearchObject.SearchFilter.Value + ',';
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

    /*****************************Compare methods************************************************/

    OpenCompare: function (vCompareId) {
        $.ajax({
            url: BaseUrl.ApiUrl + '/CompareApi?CMCompareGet=true&CompareId=' + vCompareId,
            dataType: 'json',
            success: function (result) {
                if (result != null) {
                    //set compare id
                    Provider_SearchObject.CompareId = result.CompareId;

                    //show compare action
                    $('#' + Provider_SearchObject.ObjectId + '_Compare_ActionCompare').show();

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

                        $('#' + Provider_SearchObject.ObjectId + '_Compare_ItemContainer').append(oItemHtml);

                        //validate black list
                        if (value.ProviderAlertRisk != Provider_SearchObject.BlackListStatusShowAlert) {
                            $('#' + Provider_SearchObject.ObjectId + '_Compare_Item_BlackList_' + value.RelatedProvider.RelatedCompany.CompanyPublicId).html('');
                        }

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
                width: '50px',
            }, {
                field: 'LastModify',
                title: 'Última modificación',
                width: '50px',
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
            buttons: {
                'Cancelar': function () {
                    $(this).dialog('close');
                }
            }
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

    /*****************************Compare methods end************************************************/
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

}
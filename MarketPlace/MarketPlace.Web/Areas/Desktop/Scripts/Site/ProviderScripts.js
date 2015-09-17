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
    ProjectPublicId: '',
    ProjectUrl: '',
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
        this.ProjectPublicId = vInitObject.ProjectPublicId;
        this.ProjectUrl = vInitObject.ProjectUrl;
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
        oUrl += '&ProjectPublicId=' + this.ProjectPublicId;
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
                        oItemHtml = oItemHtml.replace(/{ProviderRateClass}/gi, 'rateit');
                        oItemHtml = oItemHtml.replace(/{ProviderRate}/gi, value.ProviderRate);
                        oItemHtml = oItemHtml.replace(/{ProviderRateCount}/gi, value.ProviderRateCount);

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

                    //re-init all rates
                    $('.rateit').rateit();

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

    /*****************************Project search methods start************************************************/

    GoToProject: function () {
        if (Provider_SearchObject.ProjectPublicId != null && Provider_SearchObject.ProjectPublicId.length > 0) {
            var oProjectUrl = Provider_SearchObject.ProjectUrl.replace(/{ProjectPublicId}/gi, Provider_SearchObject.ProjectPublicId);
            window.location = oProjectUrl;

            Dialog_ShowMessage('Proceso de selección', 'Estamos evaluando todos los criterios del proceso de selección.', oProjectUrl);
        }
    },

    ShowProjectCreateFromCompare: function () {
        if (Provider_SearchObject.CompareId != null && Provider_SearchObject.CompareId.length > 0 && $('#' + Provider_SearchObject.ObjectId + '_Compare_ItemContainer').find('li').length > 0) {
            Provider_SearchObject.ShowProjectCreate();
        }
        else {
            Dialog_ShowMessage('Crear proceso de selección', 'Debe tener por lo menos un proveedor asociado a la comparacion para iniciar un proceso de selección.', null);
        }
    },

    ShowProjectCreate: function () {
        //clean input fields
        $('#' + Provider_SearchObject.ObjectId + '_Compare_CreateProject_ToolTip_Name').val('');
        $('#' + Provider_SearchObject.ObjectId + '_Compare_CreateProject_ToolTip_ProjectConfig').val('');

        //set current compare
        $('#' + Provider_SearchObject.ObjectId + '_Compare_CreateProject_ToolTip_CompareId').val(Provider_SearchObject.CompareId);

        //init form validator
        $('#' + Provider_SearchObject.ObjectId + '_Compare_CreateProject_ToolTip_Form').kendoValidator();

        //open new compare dialog
        $('#' + Provider_SearchObject.ObjectId + '_Compare_CreateProject_ToolTip').dialog({
            width: 555,
            minHeight: 258,
            modal: true,
            buttons: {
                'Cancelar': function () {
                    $(this).dialog('close');
                },
                'Guardar': function () {
                    //validate form
                    var validator = $('#' + Provider_SearchObject.ObjectId + '_Compare_CreateProject_ToolTip_Form').data("kendoValidator");
                    if (validator.validate()) {
                        //hide dialog actions
                        $(".ui-dialog-buttonpane button").css('display', 'none');

                        //save project
                        $.ajax({
                            type: "POST",
                            url: $('#' + Provider_SearchObject.ObjectId + '_Compare_CreateProject_ToolTip_Form').attr('action'),
                            data: $('#' + Provider_SearchObject.ObjectId + '_Compare_CreateProject_ToolTip_Form').serialize(),
                            success: function (result) {
                                $('#' + Provider_SearchObject.ObjectId + '_Compare_CreateProject_ToolTip').dialog("close");
                                var oProjectUrl = Provider_SearchObject.ProjectUrl.replace(/{ProjectPublicId}/gi, result);
                                Dialog_CreatedPS('Crear Proceso de Selección', 'Se ha creado el proceso de selección correctamente.', oProjectUrl);
                            },
                            error: function (result) {
                                $('#' + Provider_SearchObject.ObjectId + '_Compare_CreateProject_ToolTip').dialog("close");
                                Dialog_ShowMessage('Crear Proceso de Selección', 'Ha ocurrido un error creando el proceso de selección.', null);
                            }
                        });
                    }
                }
            }
        });
    },

    AddProjectProvider: function (vProviderPublicId) {
        if (Provider_SearchObject.ProjectPublicId != null && Provider_SearchObject.ProjectPublicId.length > 0) {
            //add company to existing compare process
            $.ajax({
                url: BaseUrl.ApiUrl + '/ProjectApi?ProjectAddCompany=true&ProjectPublicId=' + Provider_SearchObject.ProjectPublicId + '&ProviderPublicId=' + vProviderPublicId,
                dataType: 'json',
                success: function (result) {
                    if (result != null) {
                        Provider_SearchObject.OpenProject();
                    }
                },
                error: function (result) {
                    Dialog_ShowMessage('Proceso de selección', 'Se ha generado un error agregando al proveedor al proceso de selección, por favor intentelo nuevamente.', null);
                }
            });
        }
        else {
            //new compare process
            Dialog_ShowMessage('Proceso de selección', 'No hay ningun proceso de selección abierto.', null);
        }
    },

    RemoveProjectProvider: function (vProviderPublicId) {
        if (Provider_SearchObject.ProjectPublicId != null && Provider_SearchObject.ProjectPublicId.length > 0) {
            //add company to existing compare process
            $.ajax({
                url: BaseUrl.ApiUrl + '/ProjectApi?ProjectRemoveCompany=true&ProjectPublicId=' + Provider_SearchObject.ProjectPublicId + '&ProviderPublicId=' + vProviderPublicId,
                dataType: 'json',
                success: function (result) {
                    if (result != null) {
                        Provider_SearchObject.OpenProject();
                    }
                },
                error: function (result) {
                    Dialog_ShowMessage('Proceso de selección', 'Se ha generado un error removiendo al proveedor del proceso de selección, por favor intentelo nuevamente.', null);
                }
            });
        }
        else {
            //new compare process
            Dialog_ShowMessage('Proceso de selección', 'No hay ningun proceso de selección abierto.', null);
        }
    },

    OpenProject: function () {
        if (Provider_SearchObject.ProjectPublicId != null && Provider_SearchObject.ProjectPublicId.length > 0) {
            $.ajax({
                url: BaseUrl.ApiUrl + '/ProjectApi?ProjectGet=true&ProjectPublicId=' + Provider_SearchObject.ProjectPublicId,
                dataType: 'json',
                success: function (result) {
                    if (result != null) {
                        //clean compare items
                        $('#' + Provider_SearchObject.ObjectId + '_Project_ItemContainer').html('');

                        //render compare items
                        $.each(result.RelatedProjectProvider, function (item, value) {
                            //get item html
                            var oItemHtml = $('#' + Provider_SearchObject.ObjectId + '_Project_Item_Template').html();

                            //replace provider info

                            oItemHtml = oItemHtml.replace(/{ProviderPublicId}/gi, value.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId);
                            oItemHtml = oItemHtml.replace(/{ProviderLogoUrl}/gi, value.RelatedProvider.RelatedLiteProvider.ProviderLogoUrl);
                            oItemHtml = oItemHtml.replace(/{CompanyName}/gi, value.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyName);
                            oItemHtml = oItemHtml.replace(/{IdentificationType}/gi, value.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationType.ItemName);
                            oItemHtml = oItemHtml.replace(/{IdentificationNumber}/gi, value.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.IdentificationNumber);
                            oItemHtml = oItemHtml.replace(/{ProviderRateClass}/gi, 'rateit');
                            oItemHtml = oItemHtml.replace(/{ProviderRate}/gi, value.RelatedProvider.RelatedLiteProvider.ProviderRate);
                            oItemHtml = oItemHtml.replace(/{ProviderRateCount}/gi, value.RelatedProvider.RelatedLiteProvider.ProviderRateCount);

                            //validate item certified
                            if (value.RelatedProvider.RelatedLiteProvider.ProviderIsCertified != null && value.RelatedProvider.RelatedLiteProvider.ProviderIsCertified == true) {
                                oItemHtml = oItemHtml.replace(/{ProviderIsCertified}/gi, '');
                            }
                            else {
                                oItemHtml = oItemHtml.replace(/{ProviderIsCertified}/gi, 'none');
                            }

                            //validate black list
                            if (value.RelatedProvider.RelatedLiteProvider.ProviderAlertRisk != Provider_SearchObject.BlackListStatusShowAlert) {
                                oItemHtml = oItemHtml.replace(/{ProviderAlertRisk}/gi, 'none');
                            }
                            else {
                                oItemHtml = oItemHtml.replace(/{ProviderAlertRisk}/gi, '');
                            }

                            $('#' + Provider_SearchObject.ObjectId + '_Project_ItemContainer').append(oItemHtml);

                            //remove search result add project button
                            $("a[href*='Provider_SearchObject.AddProjectProvider(\\\'" + value.RelatedProvider.RelatedLiteProvider.RelatedProvider.RelatedCompany.CompanyPublicId + "\\\')']").hide();
                        });

                        //re-init all rates
                        $('.rateit').rateit();

                        //init generic tooltip
                        Tooltip_InitGeneric();
                    }
                },
                error: function (result) {
                }
            });
        }
    },

    /*****************************Project search methods end************************************************/
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
                                $('#ProviderStatus').html(result[0].Value);
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
                field: 'LargeValue',
                title: 'Seguimiento',
                width: '50%',
            }, {
                field: 'CreateDate',
                title: 'Fecha',
                width: '25%',
                template: function (dataItem) {
                    var TrackingDate;
                    if (dataItem.LargeValue != null) {
                        TrackingDate = new Date(dataItem.CreateDate);
                        TrackingDate = TrackingDate.getDate() + "/" + (TrackingDate.getMonth() + 1) + "/" + TrackingDate.getFullYear();
                    }
                    else {
                        TrackingDate = "";
                    }
                    return TrackingDate;
                }
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
        $('#' + Provider_SurveySearchObject.ObjectId + '_FilterId').click(function () {
            Provider_SurveySearchObject.Filter(null);
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

    Filter: function () {
        var oUrl = this.SearchUrl;
        debugger;
        oUrl += '&InitDate=' + $('#' + Provider_SurveySearchObject.ObjectId + '_InitDateId').val();
        oUrl += '&EndDate=' + $('#' + Provider_SurveySearchObject.ObjectId + '_EndDateId').val();
        window.location = oUrl;
    },
};

var Provider_SurveyReports = {
    ObjectId: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId
    },

    ShowProgramReport: function (vShowObject) {
        //get base html
        var DialogDiv = $('<div style="display:none" title="Generar Reporte">' + $('#' + Provider_SurveyReports.ObjectId).html() + '</div>');

        //show dialog
        DialogDiv.dialog({
            width: 500,
            minWidth: 300,
            modal: true,
            buttons: {
                'Cancelar': function () {
                    $(this).dialog("close");
                },
                'Generar Reporte': function () {
                    DialogDiv.find('#' + Provider_SurveyReports.ObjectId + '_Form').submit();
                    DialogDiv.dialog("close");
                    /* $.ajax({
                         type: "POST",
                         url: DialogDiv.find('#' + Provider_SurveyReports.ObjectId + '_Form').attr('action'),
                         data: DialogDiv.find('#' + Provider_SurveyReports.ObjectId + '_Form').serialize(),
                         success: function (result) {
                             alert(window.location.toString());
                             DialogDiv.dialog("close");
                             Dialog_ShowMessage('Generar Reporte de Promedio de Evaluaciones', 'Se ha generado el reporte.', null);//window.location.toString()
                         },
                         error: function (result) {
                             DialogDiv.dialog("close");
                             Dialog_ShowMessage('Generar Reporte de Promedio de Evaluaciones', 'Ha ocurrido un error generando el reporte', null);
                         }
                     });*/
                }
            }
        });
    }
};
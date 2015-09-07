var Project_ProjectFile = {
    ObjectId: '',
    ProjectPublicId: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.ProjectPublicId = vInitObject.ProjectPublicId;
    },

    RenderAsync: function () {
        //var oFileExit = true;
        $('#' + Project_ProjectFile.ObjectId)
        .kendoUpload({
            multiple: false,
            showFileList: false,
            localization: {
                "select": "Agregar"
            },
            async: {
                saveUrl: BaseUrl.ApiUrl + '/ProjectApi?ProjectUploadFile=true&ProjectPublicId=' + Project_ProjectFile.ProjectPublicId,
                autoUpload: true
            },
            success: function (e) {
                if (e.response != null && e.response.length > 0) {
                    //render uploaded files
                    $.each(e.response, function (item, value) {
                        var oFileItem = $('#' + Project_ProjectFile.ObjectId + '_FileItemTemplate').html();
                        oFileItem = oFileItem.replace(/{ServerUrl}/gi, value.ServerUrl);
                        oFileItem = oFileItem.replace(/{FileName}/gi, value.FileName);
                        oFileItem = oFileItem.replace(/{FileObjectId}/gi, value.FileObjectId);
                        $('#' + Project_ProjectFile.ObjectId + '_FileList').append(oFileItem);
                    });

                    //clean file list from kendo upload
                    $('.k-upload-files.k-reset').find('li').remove();

                    //init tooltips
                    Tooltip_InitGeneric();
                }
            },
        });
    },

    RemoveFile: function (vProjectInfoId) {
        $.ajax({
            url: BaseUrl.ApiUrl + '/ProjectApi?ProjectRemoveFile=true&ProjectPublicId=' + Project_ProjectFile.ProjectPublicId + '&ProjectInfoId=' + vProjectInfoId,
            dataType: 'json',
            success: function (result) {
                $('#' + Project_ProjectFile.ObjectId + '_File_' + vProjectInfoId).remove();
            },
            error: function (result) {
                Dialog_ShowMessage('Proceso de selección', 'Ha ocurrido un error borrando el archivo.', null);
            },
        });
    },
};

var Project_ProjectDetailObject = {
    ObjectId: '',
    ProjectPublicId: '',
    CustomEconomicActivity: '',
    ProjectRecalculateUrl: '',
    ProjectDetailUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.ProjectPublicId = vInitObject.ProjectPublicId;
        this.CustomEconomicActivity = vInitObject.CustomEconomicActivity;
        this.ProjectRecalculateUrl = vInitObject.ProjectRecalculateUrl;
        this.ProjectDetailUrl = vInitObject.ProjectDetailUrl;
    },

    RenderAsync: function () {
        //init edit form
        if ($('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog_Form').length > 0) {
            //init form validator
            $('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog_Form').kendoValidator();
        }

        if ($('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog_DefaultEconomicActivity').length > 0) {
            //init default activity ac
            $('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog_DefaultEconomicActivity').
                kendoMultiSelect({
                    placeholder: "Seleccione actividades economicas...",
                    dataTextField: "ActivityName",
                    dataValueField: "EconomicActivityId",
                    autoBind: false,
                    dataSource: {
                        type: "json",
                        serverFiltering: true,
                        transport: {
                            read: function (options) {
                                var oSearchParam = '';
                                if (options.data != null && options.data.filter != null && options.data.filter.filters != null && options.data.filter.filters.length > 0) {
                                    oSearchParam = options.data.filter.filters[0].value;
                                }

                                $.ajax({
                                    url: BaseUrl.ApiUrl + '/CompanyApi?CategorySearchByActivityAC=true&TreeId=4&SearchParam=' + oSearchParam,
                                    dataType: 'json',
                                    success: function (result) {
                                        options.success(result);
                                    },
                                    error: function (result) {
                                        options.error(result);
                                    }
                                });
                            },
                        }
                    },
                    value: $.parseJSON($('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog_DefaultEconomicActivityValue').val()),
                });
        }

        if ($('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog_CustomEconomicActivity').length > 0 && Project_ProjectDetailObject.CustomEconomicActivity.length > 0) {
            //init custom activity ac
            $('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog_CustomEconomicActivity').
                kendoMultiSelect({
                    placeholder: "Seleccione actividades economicas...",
                    dataTextField: "ActivityName",
                    dataValueField: "EconomicActivityId",
                    autoBind: false,
                    dataSource: {
                        type: "json",
                        serverFiltering: true,
                        transport: {
                            read: function (options) {
                                var oSearchParam = '';
                                if (options.data != null && options.data.filter != null && options.data.filter.filters != null && options.data.filter.filters.length > 0) {
                                    oSearchParam = options.data.filter.filters[0].value;
                                }

                                $.ajax({
                                    url: BaseUrl.ApiUrl + '/CompanyApi?CategorySearchByActivityAC=true&TreeId=' + Project_ProjectDetailObject.CustomEconomicActivity + '&SearchParam=' + oSearchParam,
                                    dataType: 'json',
                                    success: function (result) {
                                        options.success(result);
                                    },
                                    error: function (result) {
                                        options.error(result);
                                    }
                                });
                            },
                        }
                    },
                    value: $.parseJSON($('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog_CustomEconomicActivityValue').val()),
                });
        }

        //init close form
        if ($('#' + Project_ProjectDetailObject.ObjectId + '_CloseProjectDialog_Form').length > 0) {
            //init form validator
            $('#' + Project_ProjectDetailObject.ObjectId + '_CloseProjectDialog_Form').kendoValidator();
        }

        //init award form
        if ($('#' + Project_ProjectDetailObject.ObjectId + '_ProviderAwardDialog_Form').length > 0) {
            //init form validator
            $('#' + Project_ProjectDetailObject.ObjectId + '_ProviderAwardDialog_Form').kendoValidator();
        }

        //init approved form
        if ($('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_ApproveDialog_Form').length > 0) {
            $('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_ApproveDialog_Form').kendoValidator();
        }

        //init rejected form
        if ($('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_RejectDialog_Form').length > 0) {
            $('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_RejectDialog_Form').kendoValidator();
        }
    },

    ShowEditProject: function () {
        if ($('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog').length > 0) {
            //init dialog
            $('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog').dialog({
                modal: true,
                width: '500',
                buttons: {
                    'Cancelar': function () {
                        $(this).dialog('close');
                    },
                    'Guardar': function () {
                        //validate form
                        var validator = $('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog_Form').data("kendoValidator");
                        if (validator.validate()) {
                            //hide dialog actions
                            $(".ui-dialog-buttonpane button").css('display', 'none');

                            //save project
                            $.ajax({
                                type: "POST",
                                url: $('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog_Form').attr('action'),
                                data: $('#' + Project_ProjectDetailObject.ObjectId + '_EditProjectDialog_Form').serialize(),
                                success: function (result) {
                                    Dialog_ShowMessage('Proceso de selección', 'Se ha actualizado el proceso de selección correctamente.', Project_ProjectDetailObject.ProjectRecalculateUrl);
                                    window.location = Project_ProjectDetailObject.ProjectRecalculateUrl;
                                    $(this).dialog('close');
                                },
                                error: function (result) {
                                    Dialog_ShowMessage('Proceso de selección', 'Se ha actualizado el proceso de selección correctamente.', Project_ProjectDetailObject.ProjectRecalculateUrl);
                                    window.location = Project_ProjectDetailObject.ProjectRecalculateUrl;
                                    $(this).dialog('close');
                                }
                            });
                        }
                    }
                },
            });
        }
    },

    ShowRequestApprovalProject: function (vProviderPublicId) {
        if ($('#' + Project_ProjectDetailObject.ObjectId + '_RequestApprovalDialog').length > 0) {
            //init dialog
            $('#' + Project_ProjectDetailObject.ObjectId + '_RequestApprovalDialog').dialog({
                modal: true,
                buttons: {
                    'Cancelar': function () {
                        $(this).dialog('close');
                    },
                    'Solicitar aprobación': function () {
                        //hide dialog actions
                        $(".ui-dialog-buttonpane button").css('display', 'none');

                        //save project
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProjectApi?ProjectRequestApproval=true&ProjectPublicId=' + Project_ProjectDetailObject.ProjectPublicId + '&ProviderPublicId=' + vProviderPublicId,
                            dataType: 'json',
                            success: function (result) {
                                Dialog_ShowMessage('Proceso de aprobación', 'Se ha enviado el proceso de aprobación correctamente.', Project_ProjectDetailObject.ProjectDetailUrl);
                                window.location = Project_ProjectDetailObject.ProjectDetailUrl;
                                $(this).dialog('close');
                            },
                            error: function (result) {
                                Dialog_ShowMessage('Proceso de aprobación', 'Se ha enviado el proceso de aprobación correctamente.', Project_ProjectDetailObject.ProjectDetailUrl);
                                window.location = Project_ProjectDetailObject.ProjectDetailUrl;
                                $(this).dialog('close');
                            }
                        });
                    }
                },
            });
        }
    },

    ShowApproveProjectProvider: function () {
        if ($('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_ApproveDialog').length > 0) {
            //init dialog
            $('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_ApproveDialog').dialog({
                modal: true,
                width: '500',
                buttons: {
                    'Cancelar': function () {
                        $(this).dialog('close');
                    },
                    'Aprobar': function () {
                        //validate form
                        var validator = $('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_ApproveDialog_Form').data("kendoValidator");
                        if (validator.validate()) {
                            //hide dialog actions
                            $(".ui-dialog-buttonpane button").css('display', 'none');

                            //save project
                            $.ajax({
                                type: "POST",
                                url: $('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_ApproveDialog_Form').attr('action'),
                                data: $('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_ApproveDialog_Form').serialize(),
                                success: function (result) {
                                    Dialog_ShowMessage('Proceso de selección - aprobación', 'Se ha aprobado el proceso de selección correctamente.', Project_ProjectDetailObject.ProjectDetailUrl);
                                    window.location = Project_ProjectDetailObject.ProjectDetailUrl;
                                    $(this).dialog('close');
                                },
                                error: function (result) {
                                    Dialog_ShowMessage('Proceso de selección - aprobación', 'Ha ocurrido un error aprobando el proceso de selección.', Project_ProjectDetailObject.ProjectDetailUrl);
                                    window.location = Project_ProjectDetailObject.ProjectDetailUrl;
                                    $(this).dialog('close');
                                }
                            });
                        }
                    }
                },
            });
        }
    },

    ShowRejectProjectProvider: function () {
        if ($('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_RejectDialog').length > 0) {
            //init dialog
            $('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_RejectDialog').dialog({
                modal: true,
                width: '500',
                buttons: {
                    'Cancelar': function () {
                        $(this).dialog('close');
                    },
                    'Rechazar': function () {
                        //validate form
                        var validator = $('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_RejectDialog_Form').data("kendoValidator");
                        if (validator.validate()) {
                            //hide dialog actions
                            $(".ui-dialog-buttonpane button").css('display', 'none');

                            //save project
                            $.ajax({
                                type: "POST",
                                url: $('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_RejectDialog_Form').attr('action'),
                                data: $('#' + Project_ProjectDetailObject.ObjectId + '_ProjectProviderDetail_RejectDialog_Form').serialize(),
                                success: function (result) {
                                    Dialog_ShowMessage('Proceso de selección - rechazo', 'Se ha rechazado el proceso de selección correctamente.', Project_ProjectDetailObject.ProjectDetailUrl);
                                    window.location = Project_ProjectDetailObject.ProjectDetailUrl;
                                    $(this).dialog('close');
                                },
                                error: function (result) {
                                    Dialog_ShowMessage('Proceso de selección - rechazo', 'Ha ocurrido un error rechazando el proceso de selección.', Project_ProjectDetailObject.ProjectDetailUrl);
                                    window.location = Project_ProjectDetailObject.ProjectDetailUrl;
                                    $(this).dialog('close');
                                }
                            });
                        }
                    }
                },
            });
        }
    },

    ShowCloseProject: function () {
        if ($('#' + Project_ProjectDetailObject.ObjectId + '_CloseProjectDialog').length > 0) {
            //init dialog
            $('#' + Project_ProjectDetailObject.ObjectId + '_CloseProjectDialog').dialog({
                modal: true,
                width: '500',
                buttons: {
                    'Cancelar': function () {
                        $(this).dialog('close');
                    },
                    'Guardar': function () {
                        //validate form
                        var validator = $('#' + Project_ProjectDetailObject.ObjectId + '_CloseProjectDialog_Form').data("kendoValidator");
                        if (validator.validate()) {
                            //hide dialog actions
                            $(".ui-dialog-buttonpane button").css('display', 'none');

                            //save project
                            $.ajax({
                                type: "POST",
                                url: $('#' + Project_ProjectDetailObject.ObjectId + '_CloseProjectDialog_Form').attr('action'),
                                data: $('#' + Project_ProjectDetailObject.ObjectId + '_CloseProjectDialog_Form').serialize(),
                                success: function (result) {
                                    Dialog_ShowMessage('Proceso de selección', 'Se ha actualizado el proceso de selección correctamente.', Project_ProjectDetailObject.ProjectDetailUrl);
                                    window.location = Project_ProjectDetailObject.ProjectDetailUrl;
                                    $(this).dialog('close');
                                },
                                error: function (result) {
                                    Dialog_ShowMessage('Proceso de selección', 'Se ha actualizado el proceso de selección correctamente.', Project_ProjectDetailObject.ProjectDetailUrl);
                                    window.location = Project_ProjectDetailObject.ProjectDetailUrl;
                                    $(this).dialog('close');
                                }
                            });
                        }
                    }
                },
            });
        }
    },

    ShowAwardProject: function () {
        if ($('#' + Project_ProjectDetailObject.ObjectId + '_ProviderAwardDialog').length > 0) {
            //init dialog
            $('#' + Project_ProjectDetailObject.ObjectId + '_ProviderAwardDialog').dialog({
                modal: true,
                width: '500',
                buttons: {
                    'Cancelar': function () {
                        $(this).dialog('close');
                    },
                    'Adjudicar': function () {
                        //validate form
                        var validator = $('#' + Project_ProjectDetailObject.ObjectId + '_ProviderAwardDialog_Form').data("kendoValidator");
                        if (validator.validate()) {
                            //hide dialog actions
                            $(".ui-dialog-buttonpane button").css('display', 'none');

                            //save project
                            $.ajax({
                                type: "POST",
                                url: $('#' + Project_ProjectDetailObject.ObjectId + '_ProviderAwardDialog_Form').attr('action'),
                                data: $('#' + Project_ProjectDetailObject.ObjectId + '_ProviderAwardDialog_Form').serialize(),
                                success: function (result) {
                                    Dialog_ShowMessage('Adjudicación', 'Se ha adjudicado el proceso correctamente.', Project_ProjectDetailObject.ProjectDetailUrl);
                                    window.location = Project_ProjectDetailObject.ProjectDetailUrl;
                                    $(this).dialog('close');
                                },
                                error: function (result) {
                                    Dialog_ShowMessage('Adjudicación', 'Ha ocurrido un error adjudicando el proceso de selección.', Project_ProjectDetailObject.ProjectDetailUrl);
                                    window.location = Project_ProjectDetailObject.ProjectDetailUrl;
                                    $(this).dialog('close');
                                }
                            });
                        }
                    }
                },
            });
        }
    },
};

var Project_SearchObject = {
    ObjectId: '',
    SearchUrl: '',
    ProjectStatus: '',
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
        $('#' + Project_SearchObject.ObjectId + '_txtSearchBox').keydown(function (e) {
            if (e.keyCode == 13) {
                //enter action search
                Project_SearchObject.Search();
            }
        });

        //init search orient controls
        $('input[name="Search_rbOrder"]').change(function () {
            if ($(this) != null && $(this).attr('searchordertype') != null && $(this).attr('orderorientation') != null) {
                Project_SearchObject.Search({
                    SearchOrderType: $(this).attr('searchordertype'),
                    OrderOrientation: $(this).attr('orderorientation')
                });
            }
        });
    },

    /*{SearchFilter{Enable,Value},SearchOrderType,OrderOrientation,PageNumber}*/
    Search: function (vSearchObject) {
        /*get serach param*/
        if (this.SearchParam != $('#' + Project_SearchObject.ObjectId + '_txtSearchBox').val()) {
            /*Init pager*/
            this.PageNumber = 0;
        }
       
        this.SearchParam = $('#' + Project_SearchObject.ObjectId + '_txtSearchBox').val();
        
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
        window.location = Project_SearchObject.GetSearchUrl();
    },

    GetSearchUrl: function () {
        var oUrl = this.SearchUrl;

        oUrl += '?ProjectStatus =' + this.ProjectStatus;
        oUrl += '&SearchParam=' + this.SearchParam;
        oUrl += '&SearchFilter=' + this.SearchFilter;
        oUrl += '&SearchOrderType=' + this.SearchOrderType;
        oUrl += '&OrderOrientation=' + this.OrderOrientation;
        oUrl += '&PageNumber=' + this.PageNumber;

        return oUrl;
    }
}
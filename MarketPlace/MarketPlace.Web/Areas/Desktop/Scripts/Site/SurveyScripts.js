/*Init survey program object*/
var Survey_ProgramObject = {

    ObjectId: '',
    DateFormat: '',

    Init: function (vInitObject) {

        this.ObjectId = vInitObject.ObjectId;
        this.DateFormat = vInitObject.DateFormat;
    },

    /*
    SurveyStatus: current survey status, mandatory

    SurveyPublicId: survey public id to upsert
    ProviderPublicId: provider to associate evaluation, mandatory

    SurveyConfigId: survey configid associate
    SurveyConfigName: survey config name associate

    SurveyEvaluator: evaluator associate
    SurveyEvaluatorId: evaluator info id 

    SurveyIssueDate: issue date to send
    SurveyIssueDateId: issue date to send info id
    */
    ShowProgram: function (vShowObject) {
        //validate survey status
        if (vShowObject != null && vShowObject.ProviderPublicId != null && vShowObject.SurveyStatus == '1206001') {

            //get base html
            var DialogDiv = $('<div style="display:none" title="Programar evaluación de desempeño">' + $('#' + Survey_ProgramObject.ObjectId).html() + '</div>');

            //show dialog
            DialogDiv.dialog({
                width: 500,
                minWidth: 300,
                modal: true,
                buttons: {
                    'Cancelar': function () {
                        $(this).dialog("close");
                    },
                    'Guardar': function () {
                        var validator = DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Form').data("kendoValidator");
                        if (validator.validate()) {

                            $.ajax({
                                type: "POST",
                                url: DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Form').attr('action'),
                                data: DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Form').serialize(),
                                success: function (result) {
                                    DialogDiv.dialog("close");
                                    Dialog_ShowMessage('Programar evaluación de desempeño', 'Se ha programado la evaluación de desempeño.', window.location.toString());
                                },
                                error: function (result) {
                                    DialogDiv.dialog("close");
                                    Dialog_ShowMessage('Programar evaluación de desempeño', 'Ha ocurrido un error programando la evaluación de desempeño.', null);
                                }
                            });
                        }
                    }
                }
            });

            //init hidden values
            debugger;

            //provider id
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_ProviderPublicId').val(vShowObject.ProviderPublicId);

            //survey public id
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyPublicId').val('');
            if (vShowObject != null && vShowObject.SurveyPublicId != null) {
                //set current survey public id
                DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyPublicId').val(vShowObject.SurveyPublicId);

                //current survey config
                DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigId').val('');
                if (vShowObject != null && vShowObject.SurveyConfigId != null) {
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigId').val(vShowObject.SurveyConfigId);
                }
                //set survey names autocomplete
                if (vShowObject != null && vShowObject.SurveyConfigName != null) {
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigName').val(vShowObject.SurveyConfigName);
                }

                //set evaluator autocomplete
                if (vShowObject != null && vShowObject.SurveyEvaluator != null && vShowObject.SurveyEvaluatorId) {
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Evaluator').val(vShowObject.SurveyEvaluator);
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Evaluator').attr('name', DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Evaluator').attr('name') + vShowObject.SurveyEvaluatorId);
                }

                //set issuedate datepicker 
                if (vShowObject != null && vShowObject.SurveyIssueDate != null && vShowObject.SurveyIssueDateId) {
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_IssueDate').val(vShowObject.SurveyIssueDate);
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_IssueDate').attr('name', DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_IssueDate').attr('name') + vShowObject.SurveyIssueDateId);
                }

                //set contract name
                if (vShowObject != null && vShowObject.SurveyContract != null) {
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Contract').val(vShowObject.SurveyContract);
                }

                //set startdate datepicker
                if (vShowObject != null && vShowObject.SurveyStartDate != null && vShowObject.SurveyStartDateId) {
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_StartDate').val(vShowObject.SurveyStartDate);
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_StartDate').attr('name', DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_StartDate').attr('name') + vShowObject.SurveyStartDateId);
                }

                //set enddate datepicker
                if (vShowObject != null && vShowObject.SurveyEndDate != null && vShowObject.SurveyEndDateId) {
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_EndDate').val(vShowObject.SurveyEndDate);
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_EndDate').attr('name', DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_EndDate').attr('name') + vShowObject.SurveyEndDateId);
                }

                //set comments
                if (vShowObject != null && vShowObject.SurveyComments != null) {
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Comments').val(vShowObject.SurveyComments);
                }

                //remove init imputs
                DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Responsible').remove();
                DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Status').remove();
                DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Progress').remove();
                DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Rating').remove();
            }

            //init survey names autocomplete
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigName').kendoAutoComplete({
                minLength: 0,
                dataTextField: 'm_Item2',
                select: function (e) {
                    var selectedItem = this.dataItem(e.item.index());
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigId').val(selectedItem.m_Item1);
                },
                dataSource: {
                    type: 'json',
                    serverFiltering: true,
                    transport: {
                        read: function (options) {
                            $.ajax({
                                url: BaseUrl.ApiUrl + '/SurveyApi?SurveyConfigSearchAC=true&SearchParam=' + options.data.filter.filters[0].value,
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
                }
            }).focusout(function () {
                //validate survey config selected
                if (!$.isNumeric(DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigId').val())) {
                    if (vShowObject != null && vShowObject.SurveyConfigName != null && vShowObject.SurveyConfigId != null) {
                        DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigId').val(vShowObject.SurveyConfigId);
                        DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigName').val(vShowObject.SurveyConfigName);
                    }
                    else {
                        DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigId').val('');
                        DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigName').val('');
                    }
                }
            });

            //init survey evaluator autocomplete
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Evaluator').kendoAutoComplete({
                minLength: 0,
                dataSource: {
                    type: 'json',
                    serverFiltering: true,
                    transport: {
                        read: function (options) {
                            $.ajax({
                                url: BaseUrl.ApiUrl + '/CompanyApi?UserCompanySearchAC=true&SearchParam=' + options.data.filter.filters[0].value,
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
                }
            });

            //init issuedate datepicker 
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_IssueDate').kendoDatePicker({
                format: Survey_ProgramObject.DateFormat,
                min: new Date(),
            });

            //init startdate datepicker 
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_StartDate').kendoDatePicker({
                format: Survey_ProgramObject.DateFormat,
                min: new Date(),
            });

            //init enddate datepicker 
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_EndDate').kendoDatePicker({
                format: Survey_ProgramObject.DateFormat,
                min: new Date(),
            });

            //init form validator
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Form').kendoValidator();
        }
    },
};


var Survey_SaveObject = {

    ObjectId: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;

        //show generic progress bar
        ProgressBar_Generic_Show();
    },

    Save: function (vUrl) {
        $('#' + Survey_SaveObject.ObjectId + '_Form').attr('action', vUrl);
        $('#' + Survey_SaveObject.ObjectId + '_Form').submit();
    },

    Finalize: function (vUrl) {

        $('#' + Survey_SaveObject.ObjectId + '_Finalize_Dialog').dialog({
            width: 500,
            minWidth: 300,
            modal: true,
            buttons: {
                'Cancelar': function () {
                    $(this).dialog("close");
                },
                'Aceptar': function () {
                    $('#' + Survey_SaveObject.ObjectId + '_Form').attr('action', vUrl);
                    $('#' + Survey_SaveObject.ObjectId + '_Form').submit();
                },
            },
        });

    },
};

var Survey_File = {

    ObjectId: '',
    SurveyPublicId: '',
    SurveyConfigInfoId: '',
    ProviderPublicId: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SurveyPublicId = vInitObject.SurveyPublicId;
        this.SurveyConfigInfoId = vInitObject.SurveyConfigInfoId
        this.ProviderPublicId = vInitObject.ProviderPublicId
    },

    RenderAsync: function () {
        //var oFileExit = true;
        $('#' + Survey_File.ObjectId)
        .kendoUpload({
            multiple: false,
            async: {
                saveUrl: BaseUrl.ApiUrl + '/SurveyApi?SurveyUploadFile=true&SurveyPublicId=' + Survey_File.SurveyPublicId + '&SurveyConfigInfoId=' + Survey_File.SurveyConfigInfoId + '&ProviderPublicId=' + Survey_File.ProviderPublicId,
                autoUpload: true
            },
            success: function (e) {
                debugger;
                if (e.response != null && e.response.length > 0) {
                    //render uploaded files
                    $.each(e.response, function (item, value) {
                        debugger;
                        var oFileItem = $('#' + Survey_File.ObjectId + '_FileItemTemplate').html();

                        oFileItem = oFileItem.replace(/{ServerUrl}/gi, value.ServerUrl);
                        oFileItem = oFileItem.replace(/{FileName}/gi, value.FileName);
                        oFileItem = oFileItem.replace(/{FileObjectId}/gi, value.FileObjectId);

                        $('#' + Survey_File.ObjectId + '_FileList').append(oFileItem);
                    });
                    //clean file list from kendo upload
                    $('.k-upload-files.k-reset').find('li').remove();

                    //init tooltips
                    Tooltip_InitGeneric();
                }
            },
        });
    },

    RemoveFile: function (vSurveyInfoId) {
        $.ajax({
            url: BaseUrl.ApiUrl + '/SurveyApi?SurveyRemoveFile=true&SurveyPublicId=' + Survey_File.SurveyPublicId + '&SurveyInfoId=' + vSurveyInfoId,
            dataType: 'json',
            success: function (result) {
                debugger;
                $('#' + Survey_File.ObjectId + '_File_' + vSurveyInfoId).remove();
            },
            error: function (result) {
                Dialog_ShowMessage('Evaluación de Desempeño', 'Ha ocurrido un error borrando el archivo.', null);
            },
        });
    },
};
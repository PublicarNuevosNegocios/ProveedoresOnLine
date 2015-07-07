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
    ShowProgram: function (vShowObject, evaluatorIds) {

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

            //provider id
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_ProviderPublicId').val(vShowObject.ProviderPublicId);

            //survey public id
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyPublicId').val('');
            if (vShowObject != null && vShowObject.SurveyPublicId != null) {

                if (evaluatorIds != null) {
                    var divEvaluator = DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_EvaluatorDiv').html('');
                    $.each(evaluatorIds, function (item, value) {
                        var result = ' <li><label>Evaluador - ' + ':</label><input id="Survey_ProgramSurvey_Evaluator' + "_" + value.SurveyConfigItemInfoRol + '" placeholder="andres.perez@gmail.com" required validationmessage="Seleccione un evaluador" name="SurveyInfo_1204003_' + value + '_' + vShowObject.SurveyConfigId + '" /></li>'
                        DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_EvaluatorDiv').append(result);
                    });
                }

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

                //set expirationdate datepicjer
                debugger;
                if (vShowObject != null && vShowObject.SurveyExpirationDate != null && vShowObject.SurveyExpirationDateId) {
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_ExpirationDate').val(vShowObject.SurveyExpirationDate);
                    DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_ExpirationDate').attr('name', DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_ExpirationDate').attr('name') + vShowObject.SurveyExpirationDateId);
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
                vShowObject.SurveyConfigId = DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigId').val();
                $.ajax({
                    url: BaseUrl.ApiUrl + '/SurveyApi?SCSurveyConfigItemGetBySurveyConfigId=true&SurveyConfigId=' + vShowObject.SurveyConfigId + '&SurveyConfigItemType=' + '1202004',
                    dataType: 'json',
                    success: function (e) {
                        if (e != null && e.length > 0) {
                            //Render Roles
                            var divEvaluator = DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_EvaluatorDiv').html('');
                            var area = null;
                            $.each(e, function (item, value) {
                                debugger;
                                if (area == value.AreaName) {
                                    var result = '<li><label>' + value.SurveyConfigItemInfoRolName + ':</label><input id="Survey_ProgramSurvey_Evaluator' +
                                        "_" + value.SurveyConfigItemInfoRolId +
                                        '" placeholder="andres.perez@gmail.com" required validationmessage="Seleccione un evaluador" name="SurveyInfo_1204003_0_' +
                                        value.SurveyConfigItemInfoRolId + '_' + value.AreaId + '" /></li>'
                                }
                                else {
                                    var result = '<li><label>' + value.AreaName + '</label></li>' +
                                                '<li><label>' + value.SurveyConfigItemInfoRolName + ':</label><input id="Survey_ProgramSurvey_Evaluator' +
                                                 "_" + value.SurveyConfigItemInfoRolId +
                                                 '" placeholder="andres.perez@gmail.com" required validationmessage="Seleccione un evaluador" name="SurveyInfo_1204003_0_' +
                                                 value.SurveyConfigItemInfoRolId + '_' + value.AreaId + '" /></li>'

                                    area = value.AreaName;
                                }

                                DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_EvaluatorDiv').append(result);

                                //init survey evaluator autocomplete
                                DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Evaluator_' + value.SurveyConfigItemInfoRolId).kendoAutoComplete({
                                    minLength: 0,
                                    open: function (e) {
                                        valid = false;
                                    },
                                    select: function (e) {
                                        debugger;
                                        valid = true;
                                    },
                                    close: function (e) {
                                        // if no valid selection - clear input
                                        if (!valid) this.value('');
                                    },
                                    dataSource: {
                                        type: 'json',
                                        serverFiltering: true,
                                        transport: {
                                            read: function (options) {
                                                $.ajax({
                                                    url: BaseUrl.ApiUrl + '/CompanyApi?UserCompanySearchByRoleAC=true&RolId=' + value.SurveyConfigItemInfoRol + '&SearchParam=' + options.data.filter.filters[0].value,
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
                            });
                            //init tooltips
                            Tooltip_InitGeneric();
                        }
                    },
                    error: function (result) {
                        options.error(result);
                    }
                });
            });

            //init issuedate datepicker 
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_IssueDate').kendoDatePicker({
                format: Survey_ProgramObject.DateFormat,
                min: new Date(),
            });

            //init expirationdate datepicker
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_ExpirationDate').kendoDatePicker({
                format: Survey_ProgramObject.DateFormat,
                min: new Date(),
            });

            //init startdate datepicker 
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_StartDate').kendoDatePicker({
                format: Survey_ProgramObject.DateFormat,
            });

            //init enddate datepicker 
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_EndDate').kendoDatePicker({
                format: Survey_ProgramObject.DateFormat,
            });

            //init form validator
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Form').kendoValidator();
        }
    },
};





/*Print View EvaluationProgramSurvey*/
var Survey_Evaluation_ProgramObject = {
    ObjectId: '',
    DateFormat: '',
    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.DateFormat = vInitObject.DateFormat;
    },

    RenderEvaluation: function () {
        //Autocomplete        
        $('#' + Survey_Evaluation_ProgramObject.ObjectId + '_SurveyName').kendoAutoComplete({

            minLength: 0,
            dataTextField: 'm_Item2',
            select: function (e) {
                var selectedItem = this.dataItem(e.item.index());
                $('#' + Survey_Evaluation_ProgramObject.ObjectId + '_SurveyConfigId').val(selectedItem.m_Item1);

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
            var IdSurvey = $('#' + Survey_Evaluation_ProgramObject.ObjectId + '_SurveyConfigId').val();
            $.ajax({
                url: BaseUrl.ApiUrl + '/SurveyApi?SCSurveyConfigItemGetBySurveyConfigId=true&SurveyConfigId=' + IdSurvey + '&SurveyConfigItemType=' + '1202004',
                dataType: 'json',
                success: function (e) {
                    if (e != null && e.length > 0) {
                        //Render Roles
                        var divEvaluator = $('#' + Survey_ProgramObject.ObjectId + '_EvaluatorDiv').html('');
                        var area = null;
                        $.each(e, function (item, value) {
                            var result = '<li><label>' + value.AreaName + '</label></li>' +
                                        '<li><label>' + value.SurveyConfigItemInfoRolName + ':</label><input id="Survey_ProgramSurvey_Evaluator' +
                                         "_" + value.SurveyConfigItemInfoRolId +
                                         '" placeholder="andres.perez@gmail.com" required validationmessage="Seleccione un evaluador" name="SurveyInfo_1204003_0_' +
                                         value.SurveyConfigItemInfoRolId + '_' + value.AreaId + '" /></li>'

                            area = value.AreaName;
                            $('#' + Survey_Evaluation_ProgramObject.ObjectId + '_EvaluatorDiv').append(result);
                            //init survey evaluator autocomplete
                            $('#' + Survey_Evaluation_ProgramObject.ObjectId + '_Evaluator_' + value.SurveyConfigItemInfoRolId).kendoAutoComplete({
                                minLength: 0,
                                open: function (e) {
                                    valid = false;
                                },
                                select: function (e) {
                                    debugger;
                                    valid = true;
                                },
                                close: function (e) {
                                    if (!valid) this.value('');
                                },
                                dataSource: {
                                    type: 'json',
                                    serverFiltering: true,
                                    transport: {
                                        read: function (options) {
                                            $.ajax({
                                                url: BaseUrl.ApiUrl + '/CompanyApi?UserCompanySearchByRoleAC=true&RolId=' + value.SurveyConfigItemInfoRol + '&SearchParam=' + options.data.filter.filters[0].value,
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
                        });
                        //init tooltips
                        Tooltip_InitGeneric();
                    }
                },
                error: function (result) {
                    options.error(result);
                }
            });
        });
        //Init DatePickers
        $('#' + Survey_Evaluation_ProgramObject.ObjectId + '_IssueDate').kendoDatePicker({
            format: Survey_Evaluation_ProgramObject.DateFormat,
            min: new Date(),
        });
        $('#' + Survey_Evaluation_ProgramObject.ObjectId + '_ExpirationDate').kendoDatePicker({
            format: Survey_Evaluation_ProgramObject.DateFormat,
            min: new Date(),
        });
        $('#' + Survey_Evaluation_ProgramObject.ObjectId + '_StartDate').kendoDatePicker({
            format: Survey_Evaluation_ProgramObject.DateFormat,
            min: new Date(),
        });
        $('#' + Survey_Evaluation_ProgramObject.ObjectId + '_EndDate').kendoDatePicker({
            format: Survey_Evaluation_ProgramObject.DateFormat,
            min: new Date(),
        });

        //init form validator
        $('#' + Survey_ProgramObject.ObjectId + '_Form').kendoValidator();


    }//RenderEValuation
}




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

                if (e.response != null && e.response.length > 0) {
                    //render uploaded files
                    $.each(e.response, function (item, value) {

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

                $('#' + Survey_File.ObjectId + '_File_' + vSurveyInfoId).remove();
            },
            error: function (result) {
                Dialog_ShowMessage('Evaluación de Desempeño', 'Ha ocurrido un error borrando el archivo.', null);
            },
        });
    },
};
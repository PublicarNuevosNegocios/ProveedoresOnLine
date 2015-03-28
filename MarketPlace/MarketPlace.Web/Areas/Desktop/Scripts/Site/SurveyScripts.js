/*Init survey program object*/
var Survey_ProgramObject = {

    ObjectId: '',
    DateFormat: '',

    Init: function (vInitObject) {

        this.ObjectId = vInitObject.ObjectId;
        this.DateFormat = vInitObject.DateFormat;
    },

    ShowProgram: function (vShowObject) {
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



                        //$.ajax({
                        //    url: BaseUrl.ApiUrl + '/SurveyApi?SurveyConfigSearchAC=true&SearchParam=' + options.data.filter.filters[0].value,
                        //    dataType: 'json',
                        //    success: function (result) {
                        //        options.success(result);
                        //    },
                        //    error: function (result) {
                        //        options.error(result);
                        //    }
                        //});

                        //    var url = "path/to/your/script.php"; // the script where you handle the form input.

                        //    $.ajax({
                        //        type: "POST",
                        //        url: url,
                        //        data: $("#idForm").serialize(), // serializes the form's elements.
                        //        success: function(data)
                        //        {
                        //            alert(data); // show response from the php script.
                        //        }
                        //    });

                        //    return false; // avoid to execute the actual submit of the form.
                        //});
                        //$(this).dialog("close");
                    }
                }
            }
        });

        //init hidden values
        DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyPublicId').val('');
        if (vShowObject != null && vShowObject.SurveyPublicId != null) {
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyPublicId').val(vShowObject.SurveyPublicId);
        }
        else {
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Responsible').remove();
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Status').remove();
        }

        DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_ProviderPublicId').val('');
        if (vShowObject != null && vShowObject.ProviderPublicId != null) {
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_ProviderPublicId').val(vShowObject.ProviderPublicId);
        }

        DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigId').val('');
        if (vShowObject != null && vShowObject.SurveyConfigId != null) {
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigId').val(vShowObject.SurveyConfigId);
        }

        //init survey names autocomplete
        if (vShowObject != null && vShowObject.SurveyConfigName != null) {
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_SurveyConfigName').val(vShowObject.SurveyConfigName);
        }
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

        //init evaluator autocomplete
        if (vShowObject != null && vShowObject.SurveyEvaluator != null && vShowObject.SurveyEvaluatorId) {
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Evaluator').val(vShowObject.SurveyEvaluator);
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Evaluator').attr('name', DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Evaluator').attr('name') + vShowObject.SurveyEvaluatorId);
        }

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
        if (vShowObject != null && vShowObject.SurveyIssueDate != null && vShowObject.SurveyIssueDateId) {
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_IssueDate').val(vShowObject.SurveyIssueDate);
            DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_IssueDate').attr('name', DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_IssueDate').attr('name') + vShowObject.SurveyIssueDateId);
        }
        DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_IssueDate').kendoDatePicker({
            format: Survey_ProgramObject.DateFormat,
            min: new Date(),
        });

        //init form validator
        DialogDiv.find('#' + Survey_ProgramObject.ObjectId + '_Form').kendoValidator();
    },
};



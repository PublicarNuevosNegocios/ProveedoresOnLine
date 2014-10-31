//init customer search grid
function CustomerSearchGrid(vidDiv) {

    //configure grid
    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $('#' + vidDiv + '_Header').html() }],
        pageable: true,
        dataSource: {
            pageSize: 20,
            serverPaging: true,
            schema: {
                total: function (data) {
                    if (data != null) {
                        return data.TotalRows;
                    }
                    return 0;
                }
            },
            transport: {
                read: function (options) {
                    var oSearchParam = $('#' + vidDiv + '_txtSearch').val();

                    $.ajax({
                        url: BaseUrl.ApiUrl + '/CustomerApi?CustomerSearchVal=true&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                        dataType: "json",
                        type: "POST",
                        success: function (result) {
                            options.success(result.RelatedCustomer);
                        },
                        error: function (result) {
                            options.error(result);
                        }
                    });
                }
            },
        },
        change: function (arg) {
            $.map(this.select(), function (item) {

                if ($(item).find('td').first().length > 0 && $(item).find('td').first().text().length > 0) {
                    window.location = BaseUrl.SiteUrl + 'Customer/UpsertCustomer?CustomerPublicId=' + $(item).find('td').first().text();
                }
            });

        },
        selectable: true,
        columns: [{
            field: "CustomerPublicId",
            title: "Id",
        }, {
            field: "Name",
            title: "Comprador"
        }, {
            field: "IdentificationType.ItemName",
            title: "Tipo identificación"
        }, {
            field: "IdentificationNumber",
            title: "Identificación"
        }],
    });

    //add search button event
    $('#' + vidDiv + '_SearchButton').click(function () {
        $('#' + vidDiv).getKendoGrid().dataSource.read();
    });
}

//init form search grid
function FormSearchGrid(vidDiv, vCustomerPublicId) {

    //configure grid
    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $('#' + vidDiv + '_Header').html() }],
        pageable: true,
        dataSource: {
            pageSize: 20,
            serverPaging: true,
            schema: {
                total: function (data) {
                    if (data != null) {
                        return data.TotalRows;
                    }
                    return 0;
                }
            },
            transport: {
                read: function (options) {
                    var oSearchParam = $('#' + vidDiv + '_txtSearch').val();

                    $.ajax({
                        url: BaseUrl.ApiUrl + '/CustomerApi?FormSearchVal=true&CustomerPublicId=' + vCustomerPublicId + '&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                        dataType: "json",
                        type: "POST",
                        success: function (result) {
                            options.success(result.RelatedForm);
                        },
                        error: function (result) {
                            options.error(result);
                        }
                    });
                }
            },
        },
        change: function (arg) {          
            $.map(this.select(), function (item) {                
                if ($(item).find('#dialogRefId').first().length > 0 && $(item).find('td').first().text().length > 0) {
                    window.location = BaseUrl.SiteUrl + 'Customer/UpsertForm?CustomerPublicId=' + vCustomerPublicId + '&FormPublicId=' + $(item).find('td').first().text();
                }
            });
        },
        selectable: "row multiple",
        columns: [{
            field: "FormPublicId",
            title: "Id",
        }, {
            field: "Name",
            title: "Formulario"
        }, {
            field: 'ProviderInfoId',
            title: 'Duplicar Formulario',
            template: '<a id="dialogRefId" href="javascript:DialogForm.Init(\'${FormPublicId}\');">Duplicar</a>'
        }
        ],
    });

    //add search button event
    $('#' + vidDiv + '_SearchButton').click(function () {
        $('#' + vidDiv).getKendoGrid().dataSource.read();
    });
}

var DialogForm = {
    formPublicId: '',
    Init: function (vFormPublicId) {
        $('#formPublicId').val(vFormPublicId);
        $('#DuplicateFormId').dialog({ title: "Duplicar Formulario" });
    },
}


//field step object
var FormUpsertObject = {
    /*step field info*/
    idDivStep: '',
    idDivField: '',
    FormPublicId: '',

    /*init meeting calendar variables*/
    Init: function (vInitObject) {
        this.idDivStep = vInitObject.idDivStep;
        this.idDivField = vInitObject.idDivField;
        this.FormPublicId = vInitObject.FormPublicId;
    },

    /*render grids*/
    RenderAsync: function () {

        this.RenderStepGrid();
        this.RenderFieldGrid();

    },

    RenderStepGrid: function () {

        //configure step grid
        $('#' + FormUpsertObject.idDivStep).kendoGrid({
            toolbar: [{ template: $('#' + FormUpsertObject.idDivStep + '_Header').html() }],
            pageable: false,
            dataSource: {
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?StepSearchVal=true&FormPublicId=' + FormUpsertObject.FormPublicId,
                            dataType: "json",
                            type: "POST",
                            success: function (result) {
                                options.success(result.RelatedStep);
                            },
                            error: function (result) {
                                options.error(result);
                            }
                        });
                    }
                },
            },
            change: function (arg) {
                $.map(this.select(), function (item) {

                    if ($(item).find('td').first().length > 0 && $(item).find('td').first().text().length > 0) {
                        //change current step id
                        $('#' + FormUpsertObject.idDivField + '_StepId').val($($(item).find('td')[0]).text());
                        $('#' + FormUpsertObject.idDivField).getKendoGrid().dataSource.read();
                    }
                });

            },
            selectable: true,
            columns: [{
                field: "StepId",
                title: "Id",
            }, {
                field: "Name",
                title: "Paso"
            }, {
                field: "Position",
                title: "Orden"
            }, {
                field: "StepId",
                title: "Editar",
                template: '<a href="javascript:FormUpsertObject.RenderStepUpdate(\'${StepId}\');">Editar</a><br/><a href="javascript:FormUpsertObject.StepDelete(\'${StepId}\');">Borrar</a>'
            }],
        });
    },

    RenderStepCreate: function () {

        $('#' + FormUpsertObject.idDivStep + '_Upsert_StepId').val('');
        $('#' + FormUpsertObject.idDivStep + '_Upsert').dialog();

    },

    RenderStepUpdate: function (vStepId) {
        var grid = $('#' + FormUpsertObject.idDivStep).data("kendoGrid");
        var selectedItem = grid.dataItem(grid.select());

        if (selectedItem != null && selectedItem.StepId != null) {

            //show edit popup
            $('#' + FormUpsertObject.idDivStep + '_Upsert_StepId').val(selectedItem.StepId);
            $('#' + FormUpsertObject.idDivStep + '_Upsert_Name').val(selectedItem.Name);
            $('#' + FormUpsertObject.idDivStep + '_Upsert_Position').val(selectedItem.Position);

            $('#' + FormUpsertObject.idDivStep + '_Upsert').dialog();
        }
    },

    StepUpsert: function () {

        $('#' + FormUpsertObject.idDivStep + '_Upsert').dialog("close");

        var oStepId = $('#' + FormUpsertObject.idDivStep + '_Upsert_StepId').val();
        var oName = $('#' + FormUpsertObject.idDivStep + '_Upsert_Name').val();
        var oPosition = $('#' + FormUpsertObject.idDivStep + '_Upsert_Position').val();

        if (oName != null && oName.length > 0 && oPosition != null && oPosition.length > 0) {

            if (oStepId != null && oStepId.length > 0) {
                //update
                $.ajax({
                    url: BaseUrl.ApiUrl + '/CustomerApi?StepModifyVal=true&StepId=' + oStepId + '&Name=' + oName + '&Position=' + oPosition,
                    dataType: "json",
                    type: "POST",
                    success: function (result) {
                        $('#' + FormUpsertObject.idDivStep).getKendoGrid().dataSource.read();
                        $('#divGridStep_Upsert_Name').val('');
                        $('#divGridStep_Upsert_Position').val('');
                    },
                    error: function (result) {
                        $('#' + FormUpsertObject.idDivStep).getKendoGrid().dataSource.read();
                        alert('Se ha generado un error:' + result);
                    }
                });
            }
            else {
                //create
                $.ajax({
                    url: BaseUrl.ApiUrl + '/CustomerApi?StepCreateVal=true&FormPublicId=' + FormUpsertObject.FormPublicId + '&Name=' + oName + '&Position=' + oPosition,
                    dataType: "json",
                    type: "POST",
                    success: function (result) {
                        $('#' + FormUpsertObject.idDivStep).getKendoGrid().dataSource.read();
                        $('#divGridStep_Upsert_Name').val('');
                        $('#divGridStep_Upsert_Position').val('');
                    },
                    error: function (result) {
                        $('#' + FormUpsertObject.idDivStep).getKendoGrid().dataSource.read();
                        alert('Se ha generado un error:' + result);
                    }
                });
            }
        }
        else {

            alert('los campos nombre y posicion son obligatorios');
        }

    },

    StepDelete: function (vStepId) {

        if (vStepId != null && vStepId.length > 0) {

            $('#' + FormUpsertObject.idDivStep + '_Delete').dialog({
                modal: true,
                buttons: {
                    "Borrar": function () {
                        //delete
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?StepDeleteVal=true&StepId=' + vStepId,
                            dataType: "json",
                            type: "POST",
                            success: function (result) {
                                $('#' + FormUpsertObject.idDivStep).getKendoGrid().dataSource.read();
                            },
                            error: function (result) {
                                $('#' + FormUpsertObject.idDivStep).getKendoGrid().dataSource.read();
                                alert('Se ha generado un error:' + result);
                            }
                        });
                        $(this).dialog("close");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        else {
            alert('El paso ' + vStepId + ' no es valido');
        }
    },

    RenderFieldGrid: function () {

        //configure field grid
        $('#' + FormUpsertObject.idDivField).kendoGrid({
            toolbar: [{ template: $('#' + FormUpsertObject.idDivField + '_Header').html() }],
            pageable: false,
            dataSource: {
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?FieldSearchVal=true&StepId=' + $('#' + FormUpsertObject.idDivField + '_StepId').val(),
                            dataType: "json",
                            type: "POST",
                            success: function (result) {
                                options.success(result.RelatedField);
                            },
                            error: function (result) {
                                options.error(result);
                            }
                        });
                    }
                },
            },
            columns: [{
                field: "FieldId",
                title: "Id",
            }, {
                field: "Name",
                title: "Campo"
            }, {
                field: "ProviderInfoType.ItemId",
                title: "Tipo de campo Id"
            }, {
                field: "ProviderInfoType.ItemName",
                title: "Tipo de campo"
            }, {
                field: "Position",
                title: "Orden"
            }, {
                field: "IsRequired",
                title: "Obligatorio"
            }, {
                field: "FieldId",
                title: "Borrar",
                template: '<a href="javascript:FormUpsertObject.FieldDelete(\'${FieldId}\');">Borrar</a>'
            }],
        });
    },

    RenderFieldCreate: function () {

        $.ajax({
            url: BaseUrl.ApiUrl + '/CustomerApi?GetFieldOptionsVal=true&FormPublicId=' + FormUpsertObject.FormPublicId,
            dataType: "json",
            type: "POST",
            success: function (result) {
                if (result != null && result.ProviderInfoType != null && result.ProviderInfoType.length > 0) {

                    //clean fields
                    $('#' + FormUpsertObject.idDivField + '_Create_Name').val('');
                    $('#' + FormUpsertObject.idDivField + '_Create_ProviderInfoType').val('');
                    $('#' + FormUpsertObject.idDivField + '_Create_Position').val('');
                    $('#' + FormUpsertObject.idDivField + '_Create_IsRequired').attr('checked', false);

                    //fill items to select
                    $('#' + FormUpsertObject.idDivField + '_Create_ProviderInfoType').find('option').remove();
                    $.each(result.ProviderInfoType, function (item, value) {
                        $('#' + FormUpsertObject.idDivField + '_Create_ProviderInfoType').append('<option value="' + value.ItemId + '">' + value.ItemName + '</option>')
                    });

                    $('#' + FormUpsertObject.idDivField + '_Create').dialog();
                }
                else {
                    alert('No quedan mas campos por agregar.');
                }
            },
            error: function (result) {
                alert('Se ha generado un error:' + result);
            }
        });
    },

    FieldCreate: function () {
        $('#' + FormUpsertObject.idDivField + '_Create').dialog("close");

        var oName = $('#' + FormUpsertObject.idDivField + '_Create_Name').val();
        var oProviderInfoType = $('#' + FormUpsertObject.idDivField + '_Create_ProviderInfoType').val();
        var oPosition = $('#' + FormUpsertObject.idDivField + '_Create_Position').val();

        var oIsRequired = '' + $('#' + FormUpsertObject.idDivField + '_Create_IsRequired:checked').is(":checked") + '';

        if (oName != null && oName.length > 0 && oProviderInfoType != null && oProviderInfoType.length > 0 && oPosition != null && oPosition.length > 0 && oIsRequired != null && oIsRequired.length > 0) {

            $.ajax({
                url: BaseUrl.ApiUrl + '/CustomerApi?FieldSearchVal=true&StepId=' + $('#' + FormUpsertObject.idDivField + '_StepId').val() + '&Name=' + oName + '&ProviderInfoType=' + oProviderInfoType + '&IsRequired=' + oIsRequired + '&Position=' + oPosition,
                dataType: "json",
                type: "POST",
                success: function (result) {
                    $('#' + FormUpsertObject.idDivField).getKendoGrid().dataSource.read();
                },
                error: function (result) {
                    $('#' + FormUpsertObject.idDivField).getKendoGrid().dataSource.read();
                    alert('Se ha generado un error:' + result);
                }
            });

        }
        else {

            alert('los campos nombre y posicion son obligatorios');
        }

    },

    FieldDelete: function (vFieldId) {

        if (vFieldId != null && vFieldId.length > 0) {

            $('#' + FormUpsertObject.idDivField + '_Delete').dialog({
                modal: true,
                buttons: {
                    "Borrar": function () {
                        //delete
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?FieldDeleteVal=true&FieldId=' + vFieldId,
                            dataType: "json",
                            type: "POST",
                            success: function (result) {
                                $('#' + FormUpsertObject.idDivField).getKendoGrid().dataSource.read();
                            },
                            error: function (result) {
                                $('#' + FormUpsertObject.idDivField).getKendoGrid().dataSource.read();
                                alert('Se ha generado un error:' + result);
                            }
                        });
                        $(this).dialog("close");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        else {
            alert('El paso ' + vStepId + ' no es valido');
        }
    },

};


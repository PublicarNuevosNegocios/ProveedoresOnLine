function ProviderSearchGrid(vidDiv, cmbForm, cmbCustomer, chkName) {

    //configure grid
    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $('#' + vidDiv + '_Header').html() }],
        pageable: true,
        scrollable: true,
        dataSource: {
            pageSize: 20,
            serverPaging: true,
            schema: {
                total: function (data) {
                    if (data != null && data.length > 0) {
                        return data[0].oTotalRows;
                    }
                    return 0;
                }
            },
            transport: {
                read: function (options) {
                    var oSearchParam = $('#' + vidDiv + '_txtSearch').val();
                    var oCustomerParam = $('#' + cmbCustomer + ' ' + 'option:selected').val();
                    var oFormParam = $('#' + cmbForm + ' ' + 'option:selected').val();
                    var oUniqueParam = $('#' + chkName).prop('checked');
                    if (oFormParam == null) {
                        oFormParam = "";
                    }

                    $.ajax({
                        url: BaseUrl.ApiUrl + '/ProviderApi?ProviderSearchVal=true&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize + '&CustomerPublicId=' + oCustomerParam + '&FormPublicId=' + oFormParam + '&Unique=' + oUniqueParam,
                        dataType: "json",
                        type: "POST",
                        success: function (result) {
                            options.success(result.RelatedProvider)
                        },
                        error: function (result) {
                            options.error(result);
                        }
                    });
                }
            },
        },
        columns: [{
            field: "RelatedProvider.ProviderPublicId",
            title: "Id Proveedor",
            width: 100
        }, {
            field: "RelatedProvider.Name",
            title: "Razón Social",
            width: 200
        }, {
            field: "RelatedProvider.IdentificationType.ItemName",
            title: "Tipo identificación",
            width: 150
        }, {
            field: "RelatedProvider.IdentificationNumber",
            title: "Númer identificación",
            width: 150
        },{
            field: "checkDigit",
            title: "Dígito Verificación",
            width: 150
        }, {
            field: "RelatedProvider.CustomerName",
            title: "Comprador",
            width: 200
        }, {
            field: "RelatedProvider.Email",
            title: "Email",
            width: 200
        }, {
            field: "FormUrl",
            title: "URL",
            width: 200,
            template: $('#' + vidDiv + '_FormUrl').html(),
        }, {
            field: "RelatedProvider.CustomerCount",
            title: "# Comp. Relacionados",
            width: 150
        }, {
            field: "LastModifyUser",
            title: "Usuario que actualizó",
            width: 200
        }, {
            field: "lastModify",
            title: "Ultima actualización",
            width: 200
        }, {
            field: "Edit",
            title: "Edit",
            template: '<a id="dialogRefId" href="javascript:EditDialog(\'${RelatedProvider.ProviderPublicId}\', \'${RelatedProvider.IdentificationType.ItemId}\', \'${RelatedProvider.IdentificationNumber}\', \'${RelatedProvider.Email}\',  \'${RelatedProvider.CustomerPublicId}\', \'${RelatedProvider.Name}\', \'${CustomerInfoTypeId}\', \'${checkDigit}\', \'${checkDigitInfoId}\');">Editar</a>',
            width: 150
        }],
    });
    //add search button event
    $('#' + vidDiv + '_SearchButton').click(function () {
        $('#' + vidDiv).getKendoGrid().dataSource.read();
    });
    $('#' + cmbCustomer).change(function () {
        initCmb('Form', cmbCustomer);
    });
}

function EditDialog(ProviderPublicId, IdentificationType, IdentificationNumber, Email, CustomerPublicId, ProviderName, infoId, checkDigit, checkDigitInfoIdedit) {
    $('#EditProviderDialog').show;
    $('#EditProviderDialog').dialog({ title: "Editar Proveedor" });
    $('#RazonSocial').val(ProviderName);
    $('#ProviderPublicIdEdit').val(ProviderPublicId);
    $('#TipoIdentificacion').val(IdentificationType);
    $('#NumeroIdentificacion').val(IdentificationNumber);
    $('#checkDigit').val(checkDigit);
    $('#ProviderCustomerIdEdit').val(CustomerPublicId);
    $('#Email').val(Email);
    $('#ProviderInfoIdEdit').val(infoId);
    $('#checkDigitInfoIdEdit').val(checkDigitInfoIdedit);
    
}

function initCmb(cmbForm, cmbCustomer) {
    var CustomerPublicId = $('#' + cmbCustomer + ' ' + 'option:selected').val();
    var htmlCmbForm = $('#' + cmbForm).html();
    $.ajax({
        url: BaseUrl.ApiUrl + '/ProviderApi/FormSearch?CustomerPublicId=' + CustomerPublicId + '&SearchParam=' + ' ' + '&PageNumber=' + 0 + '&RowCount=' + 20,
        dataType: "json",
        type: "POST",
        success: function (result) {
            $('#' + cmbForm).html('');
            $('#' + cmbForm).append('<option value="' + "" + '">' + " " + '</option>')
            for (item in result.RelatedForm) {                
                $('#' + cmbForm).append('<option value="' + result.RelatedForm[item].FormPublicId + '">' + result.RelatedForm[item].Name + '</option>')
            }
        },
        error: function (result) {
            options.error(result);
        }
    });
}

var L_AdminLogProvider = {
    DivId: '',
    ProviderPublicId: '',
    DateFormat: '',

    Init: function (vInitObject) {
        this.DivId = vInitObject.DivId;
        this.ProviderPublicId = vInitObject.ProviderPublicId;
        this.DateFormat = vInitObject.DateFormat;
    },

    RenderAsync: function () {
        $('#' + L_AdminLogProvider.DivId).kendoGrid({
            navigatable: true,
            pageable: false,
            scrollable: true,
            dataSource: {
                schema: {
                    total: function (data) {
                        if (data != null && data.length > 0) {
                            return data[0].oTotalRows;
                        }
                        return 0;
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?ProviderLog=true&ProviderPublicId=' + L_AdminLogProvider.ProviderPublicId,
                            dataType: 'json',
                            success: function (result) {                                
                                if (result != null) {
                                    options.success(result);
                                }                                
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            },
                        });
                    },
                },
            },
            columns: [{
                field: 'User',
                title: 'Usuario',
            }, {
                field: 'CreateDate',
                title: 'Fecha de Creación',
                format: L_AdminLogProvider.DateFormat,
            }, {
                field: 'RelatedLogInfo[0].Value',
                title: 'Campo Editado',
            }, {
                field: 'Source',
                title: 'Url Formulario',
                template: '<a target="_blank" href="${Source}">Ir al Formulario</a>'
            }]
        });
    },
};

function ChangesControl(vidDiv, SearchParam)
{    
    //configure grid
    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $('#' + vidDiv + '_Header').html() }],
        pageable: true,
        scrollable: true,
        dataSource: {
            pageSize: 20,
            serverPaging: true,
            schema: {
                total: function (data) {
                    if (data != null && data.length > 0) {
                        return data[0].oTotalRows;
                    }
                    return 0;
                }
            },
            transport: {
                read: function (options) {
                    var oSearchParam = $('#' + vidDiv + '_txtSearch').val();
                    $.ajax({
                        url: BaseUrl.ApiUrl + '/ProviderApi?ChangesControlSearch=true&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                        dataType: "json",
                        type: "POST",
                        success: function (result) {                            
                            options.success(result)
                        },
                        error: function (result) {                            
                            options.error(result);
                        }
                    });
                }
            },
        },
        columns: [{
            field: "Name",
            title: "Razon Social",
            width: 300
        }, {
            field: "IdentificationType.ItemName",
            title: "Tipo de Identificación",
            width: 100
        }, {
            field: "IdentificationNumber",
            title: "Núm. identificación",
            width: 150
        }, {
            field: "Status.ItemName",
            title: "Estado",
            width: 100
        }, {
            field: "LastModify",
            title: "Fecha de Modificación",
            width: 200
        }, {
            field: "FormUrl",
            title: "URL",
            width: 200,
            template: $('#' + vidDiv + '_FormUrl').html(),
        }],
    });
    //add search button event
    $('#' + vidDiv + '_SearchButton').click(function () {
        $('#' + vidDiv).getKendoGrid().dataSource.read();
    });

    $('#' + vidDiv + '_txtSearch').keypress(function (event) {        
        if (event.keyCode == 13) {
            event.preventDefault();
            $('#' + vidDiv + '_SearchButton').click();
        }
    });
}
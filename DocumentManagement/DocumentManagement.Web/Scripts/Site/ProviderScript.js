﻿function ProviderSearchGrid(vidDiv, cmbForm, cmbCustomer, chkName) {

    //configure grid
    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $('#' + vidDiv + '_Header').html() }],
        pageable: true,
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
                    if (oSearchParam != '' || oCustomerParam != '' || oFormParam != '' || oUniqueParam != '') {
                        options.data.page = 1;
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
        }, {
            field: "RelatedProvider.Name",
            title: "Razón Social"
        }, {
            field: "RelatedProvider.IdentificationType.ItemName",
            title: "Tipo identificación"
        }, {
            field: "RelatedProvider.IdentificationNumber",
            title: "Númer identificación"
        }, {
            field: "RelatedProvider.CustomerName",
            title: "Comprador"
        }, {
            field: "RelatedProvider.Email",
            title: "Email"
        }, {
            field: "FormUrl",
            title: "URL",
            width: 100,
            template: $('#' + vidDiv + '_FormUrl').html(),
        }, {
            field: "RelatedProvider.CustomerCount",
            title: "# Comp. Relacionados",
        }, {
            field: "codSalesforce",
            title: "URL SalesForce",
            width: 300,               
            template: '<a href="${codSalesforce}" target="_blank">Ver lead en Salesfoce</a>',
        }, {
            field: "Edit",
            title: "Edit",
            width: 100,
            template: '<a id="dialogRefId" href="javascript:EditDialog(\'${RelatedProvider.ProviderPublicId}\', \'${RelatedProvider.IdentificationType.ItemId}\', \'${RelatedProvider.IdentificationNumber}\', \'${RelatedProvider.Email}\', \'${codSalesforce}\', \'${RelatedProvider.CustomerPublicId}\', \'${RelatedProvider.Name}\', \'${CustomerInfoTypeId}\');">Editar</a>'
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

function EditDialog(ProviderPublicId, IdentificationType, IdentificationNumber, Email,SalesForceCode, CustomerPublicId, ProviderName, infoId)
{    
    $('#EditProviderDialog').show;
    $('#EditProviderDialog').dialog({ title: "Editar Proveedor" });
    
    $('#RazonSocial').val(ProviderName);
    $('#ProviderPublicIdEdit').val(ProviderPublicId);
    $('#TipoIdentificacion').val(IdentificationType);
    $('#NumeroIdentificacion').val(IdentificationNumber);
    $('#ProviderCustomerIdEdit').val(CustomerPublicId);    
    $('#Email').val(Email);
    $('#ProviderInfoIdEdit').val(infoId); 
    
    $('#SalesForceCode').val(SalesForceCode.replace(/https:\/\/na2.salesforce.com\//gi,''));
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
function ProviderSearchGrid(vidDiv, cmbForm, cmbCustomer, chkName) {
    
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
                        url: 'api/ProviderApi?ProviderSearchVal=true&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize + '&CustomerPublicId=' + oCustomerParam + '&FormPublicId=' + oFormParam + '&Unique=' + oUniqueParam,
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
        },{
            field: "RelatedProvider.IdentificationType.ItemName",
            title: "Tipo identificación"
        }, {
            field: "RelatedProvider.IdentificationNumber",
            title: "Númer identificación"
        }, {
            field: "RelatedProvider.CustomerName",
            title: "Comprador"
        },
           {   
            field: "RelatedProvider.Email",
            title: "Email"
        }, {
            field: "FormUrl",
            title: "URL",
            width: 400,
            template: "<a target=\"_blank\" href=\"" + "#=FormUrl#" + "\">" + "#=FormUrl#" + "</a>"
        }, {
            field: "RelatedProvider.CustomerCount",
            title: "# Comp. Relacionados",                      
        }, {
            field: "codSalesforce",
            title: "Número Campaña"
        }
        ],
    });
    //add search button event
    $('#' + vidDiv + '_SearchButton').click(function () {        
        $('#' + vidDiv).getKendoGrid().dataSource.read();
    });
    $('#' + cmbCustomer).change(function () {        
        initCmb('Form', cmbCustomer);
    });
}

function initCmb(cmbForm, cmbCustomer) {
    var CustomerPublicId = $('#' + cmbCustomer + ' ' + 'option:selected').val();
    var htmlCmbForm = $('#' + cmbForm).html();

    $.ajax({
        url: 'api/ProviderApi/FormSearch?CustomerPublicId=' + CustomerPublicId + '&SearchParam=' + ' ' + '&PageNumber=' + 0 + '&RowCount=' + 20,
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
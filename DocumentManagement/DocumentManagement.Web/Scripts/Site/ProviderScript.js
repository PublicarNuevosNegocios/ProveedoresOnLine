function ProviderSearchGrid(vidDiv, cmbForm, cmbCustomer) {
    
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
                    debugger;
                    var oSearchParam = $('#' + vidDiv + '_txtSearch').val();                    
                    var oCustomerParam = $('#' + cmbCustomer + ' ' + 'option:selected').val();
                    var oFormParam = $('#' + cmbForm + ' ' + 'option:selected').val();
                    if (oFormParam == null) {
                        oFormParam = "";
                    }

                    $.ajax({
                        url: '/api/ProviderApi?ProviderSearchVal=true&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize + '&CustomerPublicId=' + oCustomerParam + '&FormPublicId=' + oFormParam,
                        dataType: "json",
                        type: "POST",
                        success: function (result) {
                            debugger;
                            options.success(result.RelatedProvider)
                        },
                        error: function (result) {
                            debugger;
                            options.error(result);
                        }
                    });
                }
            },
        },      
        columns: [{
            field: "RelatedProvider.ProviderPublicId",
            title: "Id",
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
        }],
    });
    //add search button event
    $('#' + vidDiv + '_SearchButton').click(function () {        
        $('#' + vidDiv).getKendoGrid().dataSource.read();
    });
    $('#'+ cmbCustomer).change(function () {        
        var CustomerPublicId = $('#'+ cmbCustomer + ' '+ 'option:selected').val();
        var htmlCmbForm = $('#' + cmbForm).html();

        $.ajax({
            url: '/api/ProviderApi/FormSearch?CustomerPublicId=' + CustomerPublicId + '&SearchParam=' + ' ' + '&PageNumber=' + 0 + '&RowCount=' + 20,
            dataType: "json",
            type: "POST",
            success: function (result) {
                $('#' + cmbForm).html('');                
                for (item in result.RelatedForm) {                    
                    $('#' + cmbForm).append('<option value="' + result.RelatedForm[item].FormPublicId + '">' + result.RelatedForm[item].Name + '</option>')
                }
            },
            error: function (result) {                
                options.error(result);
            }
        });       
    });
}

function initCmb(cmbForm) {
    debugger;
    var CustomerPublicId = $('#' + cmbCustomer + ' ' + 'option:selected').val();
    var htmlCmbForm = $('#' + cmbForm).html();

    $.ajax({
        url: '/api/ProviderApi/FormSearch?CustomerPublicId=' + CustomerPublicId + '&SearchParam=' + ' ' + '&PageNumber=' + 0 + '&RowCount=' + 20,
        dataType: "json",
        type: "POST",
        success: function (result) {
            $('#' + cmbForm).html('');            
            for (item in result.RelatedForm) {                
                $('#' + cmbForm).append('<option value="' + result.RelatedForm[item].FormPublicId + '">' + result.RelatedForm[item].Name + '</option>')
            }
        },
        error: function (result) {
            options.error(result);
        }
    });
}
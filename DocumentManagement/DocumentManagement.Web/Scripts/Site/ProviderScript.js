function ProviderSearchGrid(vidDiv) {
    debugger;
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
                    var oCustomerParam = $('#' + vidDiv + '_txtSearch').val();
                    var oFormParam = $('#' + vidDiv + '_txtSearch').val();

                    $.ajax({
                        url: '/api/ProviderApi?ProviderSearchVal=true&SearchParam=' + oSearchParam + '&PageNumber=' +  1 + '&RowCount=' + options.data.pageSize,
                        dataType: "json",
                        type: "POST",
                        success: function (result) {
                            debugger;
                            options.success(result.RelatedCustomer);
                        },
                        error: function (result) {
                            debugger;
                            options.error(result);
                        }
                    });
                }
            },
        },
        change: function (arg) {
            $.map(this.select(), function (item) {

                if ($(item).find('td').first().length > 0 && $(item).find('td').first().text().length > 0) {
                    window.location = '/Customer/UpsertCustomer?CustomerPublicId=' + $(item).find('td').first().text();
                }
            });
        },
        selectable: true,
        columns: [{
            field: "CustomerPublicId",
            title: "Razón Social",
        }, {
            field: "Name",
            title: "Tipo identificación"
        }, {
            field: "IdentificationType.ItemName",
            title: "Númer identificación"
        }, {
            field: "IdentificationNumber",
            title: "Comprador"
        }, {
            field: "IdentificationNumber",
            title: "URL"
        }, {
            field: "IdentificationNumber",
            title: "Email"

        }],
    });

    //add search button event
    $('#' + vidDiv + '_SearchButton').click(function () {
        $('#' + vidDiv).getKendoGrid().dataSource.read();
    });
}

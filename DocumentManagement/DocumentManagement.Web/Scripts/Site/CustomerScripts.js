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
                        url: '/api/CustomerApi?SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
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
                    window.location = '/Customer/UpsertCustomer?CustomerPublicId='  + $(item).find('td').first().text();
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
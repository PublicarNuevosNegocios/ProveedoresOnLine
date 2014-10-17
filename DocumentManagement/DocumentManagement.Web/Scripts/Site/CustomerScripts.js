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
                        url: '/api/CustomerApi?CustomerSearchVal=true&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
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

//init customer search grid
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
                        url: '/api/CustomerApi?FormSearchVal=true&CustomerPublicId=' + vCustomerPublicId  + '&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
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

                if ($(item).find('td').first().length > 0 && $(item).find('td').first().text().length > 0) {
                    window.location = '/Customer/UpsertForm?CustomerPublicId=' + vCustomerPublicId + '&FormPublicId=' + $(item).find('td').first().text();
                }
            });

        },
        selectable: true,
        columns: [{
            field: "FormPublicId",
            title: "Id",
        }, {
            field: "Name",
            title: "Formulario"
        }],
    });

    //add search button event
    $('#' + vidDiv + '_SearchButton').click(function () {
        $('#' + vidDiv).getKendoGrid().dataSource.read();
    });
}
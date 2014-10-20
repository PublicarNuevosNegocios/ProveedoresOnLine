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
                    window.location = '/Customer/UpsertCustomer?CustomerPublicId=' + $(item).find('td').first().text();
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
                        url: '/api/CustomerApi?FormSearchVal=true&CustomerPublicId=' + vCustomerPublicId + '&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
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

//init Step Field search grid
function StepFieldSearchGrid(vidDivStep, vidDivField, vFormPublicId) {

    //configure step grid
    $('#' + vidDivStep).kendoGrid({
        toolbar: [{ template: $('#' + vidDivStep + '_Header').html() }],
        pageable: false,
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        url: '/api/CustomerApi?StepSearchVal=true&FormPublicId=' + vFormPublicId,
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

                //inputStepId

                ////add search button event
                //$('#' + vidDiv + '_SearchButton').click(function () {
                //    $('#' + vidDiv).getKendoGrid().dataSource.read();
                //});

                //if ($(item).find('td').first().length > 0 && $(item).find('td').first().text().length > 0) {
                //    window.location = '/Customer/UpsertForm?CustomerPublicId=' + vCustomerPublicId + '&FormPublicId=' + $(item).find('td').first().text();
                //}
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
        }],
    });

    //configure field grid
    $('#' + vidDivField).kendoGrid({
        toolbar: [{ template: $('#' + vidDivField + '_Header').html() }],
        pageable: false,
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax({
                        url: '/api/CustomerApi?FieldSearchVal=true&StepId=' + $('#' + vidDivField + '_StepId').val(),
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
        //change: function (arg) {
        //    $.map(this.select(), function (item) {

        //        //inputStepId

        //        ////add search button event
        //        //$('#' + vidDiv + '_SearchButton').click(function () {
        //        //    $('#' + vidDiv).getKendoGrid().dataSource.read();
        //        //});

        //        //if ($(item).find('td').first().length > 0 && $(item).find('td').first().text().length > 0) {
        //        //    window.location = '/Customer/UpsertForm?CustomerPublicId=' + vCustomerPublicId + '&FormPublicId=' + $(item).find('td').first().text();
        //        //}
        //    });
        //},
        //selectable: true,
        columns: [{
            field: "FieldId",
            title: "Id",
        }, {
            field: "Name",
            title: "Campo"
        }, {
            field: "ProviderInfoType.ItemName",
            title: "Tipo de campo"
        }, {
            field: "Position",
            title: "Orden"
        }, {
            field: "IsRequired",
            title: "Obligatorio"
        }],
    });
}




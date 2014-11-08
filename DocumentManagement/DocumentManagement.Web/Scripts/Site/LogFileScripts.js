function ProviderFileSearchGrid(vidDiv, vData) {

    $('#' + vidDiv).kendoGrid({
        dataSource: {
            type: 'json',
            data: vData,
        },
        columns: [{
            field: 'User',
            title: 'Usuario',
        }, {
            field: 'CreateDate',
            title: 'Creacion'
        }, {
            field: 'Url',
            title: ' ',
            template: '<a target="_blank" href="${Url}">Ver Archivo</a>'
        }]
    });

}


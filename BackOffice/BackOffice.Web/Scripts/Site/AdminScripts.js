var Admin_GeolocalizationObject = {    
    ObjectId: '',
    AdminOptions: new Array(),

    Init: function(vInitObject){
        this.ObjectId = vInitObject.ObjectId;
        if (vInitObject.UtilOptions != null) {
            $.each(vInitObject.UtilOptions, function (item, value) {
                Admin_GeolocalizationObject.AdminOptions[value.Key] = value.Value;
            });
        }
    },
    
    RenderAsync: function () {
        debugger;
        $('#' + Admin_GeolocalizationObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' }
            ],
            dataSource: {
                pageSize: 20,
                serverPaging: true,
                schema: {
                    total: function (data) {
                        if (data && data.length > 0) {
                            return data[0].AllTotalRows;
                        }
                        return 0;
                    },
                    model: {
                        id: 'GIT_CountryId',
                        fields: {
                            GIT_Country: { editable: false, nullable: true },
                           
                            AG_City: { editable: true },
                            AG_CityId: { editable: false },

                            GI_CapitalType: { editable: true },
                            GI_CapitalTypeId: { editable: false },

                            GI_DirespCode: { editable: true },
                            GI_DirespCodeId: { editable: false },

                            GIT_State: { editable: true },
                            GIT_StateId: { editable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {                        
                        $.ajax({                            
                            url: BaseUrl.ApiUrl + '/UtilApi?GetAllGeography=true&SearchParam=&CityId=' + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                            dataType: 'json',
                            success: function (result) {
                                debugger;
                                options.success(result);
                            },
                            error: function (result) {
                                debugger;
                                options.error(result);
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?GetAllGeography=true&SearchParam=' + 'a' + '&CityId=' + 'a',
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?GetAllGeography=true&SearchParam=' + 'a' + '&CityId=' + 'a',
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'GIT_CountryId',
                title: 'Id Pais',
                width: '50px',
            }, {
                field: 'GIT_Country',
                title: 'País',
                width: '100px',
            }, {
                field: 'GIT_StateId',
                title: 'Estado (Dpto.) Id',
                width: '50px',
            },{
                field: 'GIT_State',
                title: 'Estado (Dpto.)',
                width: '100px',
            },{
                field: 'AG_CityId',
                title: 'Id Ciudad',
                width: '50px',
            },{
                field: 'AG_City',
                title: 'Ciudad',
                width: '100px',
            },{
                field: 'GI_CapitalType',
                title: 'Tipo de Capital',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.GI_CapitalType != null) {
                        $.each(Admin_GeolocalizationObject.AdminOptions[107], function (item, value) {
                            if (dataItem.GI_CapitalType == value.ItemId) {
                                oReturn = value.ItemName;
                            }
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    $('<input required data-bind="value:' + options.field + '"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            dataSource: Admin_GeolocalizationObject.AdminOptions[107],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            },{
                field: 'GI_DirespCode',
                title: 'Código Ciudad',
                width: '100px',
            }],
        });
    },
}
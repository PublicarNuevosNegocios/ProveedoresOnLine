var Admin_CategoryObject = {    
    ObjectId: '',
    AdminOptions: new Array(),
    CategoryType: '', 

    Init: function(vInitObject){
        this.ObjectId = vInitObject.ObjectId;
        this.CategoryType = vInitObject.CategoryType
        if (vInitObject.UtilOptions != null) {
            $.each(vInitObject.UtilOptions, function (item, value) {
                Admin_CategoryObject.AdminOptions[value.Key] = value.Value;
            });
        }
    },
    
    RenderAsync: function () {
        debugger;
        $('#' + Admin_CategoryObject.ObjectId).kendoGrid({
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
                            GIT_Country: { editable: true, nullable: false },
                           
                            GIT_CountryDirespCode: { editable: true, nullable: true },
                            GIT_CountryDirespCodeId: { editable: false},

                            GIT_CountryType: { editable: true, nullable: true },
                            GIT_CountryTypeId: { editable: false },

                            GIT_CountryEnable: { editable: true, type: 'boolean', defaultValue: true },

                            AG_City: { editable: true, nullable: false },
                            AG_CityId: { editable: false},

                            GI_CapitalType: { editable: true, nullable: false },
                            GI_CapitalTypeId: { editable: false },

                            GI_CityDirespCode: { editable: true, nullable: false },
                            GI_CityDirespCodeId: { editable: false },

                            GI_CityEnable: { editable: true, type: 'boolean', defaultValue: true },

                            GIT_State: { editable: true, nullable: false },
                            GIT_StateId: { editable: false },

                            GIT_StateDirespCode: { editable: true, nullable: false },
                            GIT_StateDirespCodeId: { editable: false },

                            GIT_StateEnable: { editable: true, type: 'boolean', defaultValue: true },
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
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType,
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
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType,
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
            columns: [ {
                field: 'GIT_Country',
                title: 'País',
                width: '100px',
            }, {
                field: 'GIT_CountryDirespCode',
                title: 'País DirespCode',
                width: '100px',
            },{
                field: 'GIT_CountryEnable',
                title: 'Enable Pais',
                width: '50px',
            },{
                field: 'GIT_State',
                title: 'Estado (Dpto.)',
                width: '100px',
            }, {
                field: 'GIT_StateDirespCode', 
                title: 'Estado DirespCode',
                width: '100px',
            }, {
                field: 'GIT_StateEnable',
                title: 'Enable Estado (Dpto)',
                width: '50px',
            }, {
                field: 'AG_City',
                title: 'Ciudad',
                width: '100px',
            }, {
                field: 'GI_CityDirespCode',
                title: 'Ciudad DirespCode',
                width: '100px',
            }, {
                field: 'GI_CapitalType',
                title: 'Tipo de Capital',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.GI_CapitalType != null) {
                        $.each(Admin_CategoryObject.AdminOptions[107], function (item, value) {
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
                            dataSource: Admin_CategoryObject.AdminOptions[107],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            },{
                field: 'GI_CityEnable',
                title: 'Enable Ciudad',
                width: '50px',
            },],
        });
    },
}
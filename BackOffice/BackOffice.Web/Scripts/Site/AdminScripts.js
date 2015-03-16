var Admin_CategoryObject = {
    ObjectId: '',
    AdminOptions: new Array(),
    CategoryType: '',
    TreeId: '',
    DateFormat: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.CategoryType = vInitObject.CategoryType;
        this.TreeId = vInitObject.TreeId != "0" ? vInitObject.TreeId : "4";
        this.DateFormat = vInitObject.DateFormat;
        if (vInitObject.UtilOptions != null) {
            $.each(vInitObject.UtilOptions, function (item, value) {
                Admin_CategoryObject.AdminOptions[value.Key] = value.Value;
            });
        }
    },

    RenderAsync: function () {
        if (Admin_CategoryObject.CategoryType == "AdminGeo") {
            Admin_CategoryObject.RenderGeoAsync();
        }
        else if (Admin_CategoryObject.CategoryType == "AdminBank") {
            Admin_CategoryObject.RenderBankAsync();
        }
        else if (Admin_CategoryObject.CategoryType == "AdminCompanyRules") {
            Admin_CategoryObject.RenderCompanyRulesAsync();
        }
        else if (Admin_CategoryObject.CategoryType == "AdminRules") {
            Admin_CategoryObject.RenderRulesAsync();
        }
        else if (Admin_CategoryObject.CategoryType == "AdminResolution") {
            Admin_CategoryObject.RenderResolutionAsync();
        }
        else if (Admin_CategoryObject.CategoryType == "AdminEcoAcEstandar") {
            Admin_CategoryObject.RenderActivityStandarAsync();
        }
        else if (Admin_CategoryObject.CategoryType == "AdminEcoGroupEstandar") {
            Admin_CategoryObject.RenderGroupStandarAsync();
        }
        else if (Admin_CategoryObject.CategoryType == "AdminTree") {
            Admin_CategoryObject.RenderTreeAsync();
        }
        else if (Admin_CategoryObject.CategoryType == "AdminTRM") {
            Admin_CategoryObject.RenderTRMAsync();
        }

        //config keyboard
        Admin_CategoryObject.ConfigKeyBoard();
    },

    ConfigKeyBoard: function () {
        $(document.body).keydown(function (e) {
            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Admin_CategoryObject.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Admin_CategoryObject.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Admin_CategoryObject.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    RenderGeoAsync: function (param) {

        if (param != true) {
            var vSearchParam = '';
        }
        else {
            var vSearchParam = $('#SearchBoxId').val();
        }

        $('#' + Admin_CategoryObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar:
                [

                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' },
                { name: "SearchBox", template: "<input id='SearchBoxId' type='text'value=''>" },
                { name: "SearchButton", template: "<a id='Buscar' href='javascript: Admin_CategoryObject.RenderGeoAsync(" + "true" + ");'>Buscar</a" }
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
                            GIT_CountryDirespCodeId: { editable: false },

                            GIT_CountryType: { editable: true, nullable: true },
                            GIT_CountryTypeId: { editable: false },

                            GIT_CountryEnable: { editable: true, type: 'boolean', defaultValue: true },

                            AG_City: { editable: true, nullable: false },
                            AG_CityId: { editable: false },

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
                            url: BaseUrl.ApiUrl + '/UtilApi?GetAllGeography=true&SearchParam=' + vSearchParam + '&CityId=' + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize + '&IsAutoComplete=false',
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    create: function (options) {

                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', '0');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
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
                                Message('error', '');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'GIT_Country',
                title: 'País',
                width: '90px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.GIT_Country != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.GIT_Country;
                    }
                    return oReturn;
                },
                editor: function (container, options) {

                    // create an input element
                    var input = $('<input/>');
                    var isSelected = false;
                    // set its name to the field to which the column is bound ('name' in this case)
                    input.attr('value', options.model[options.field]);
                    // append it to the container
                    input.appendTo(container);
                    // initialize a Kendo UI AutoComplete
                    input.kendoAutoComplete({
                        dataTextField: 'GIT_Country',

                        change: function (e) {
                            if (isSelected == false) {
                                options.model['GIT_Country'] = e.sender._old;
                                options.model.dirty = true;
                            }
                        },

                        select: function (e) {
                            var selectedItem = this.dataItem(e.item.index());
                            isSelected = true;
                            //set server fiel name
                            options.model['GIT_CountryId'] = selectedItem.GIT_CountryId;
                            options.model['GIT_Country'] = selectedItem.GIT_Country;

                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: 'json',
                            serverFiltering: true,
                            transport: {
                                read: function (options) {

                                    $.ajax({
                                        //url: BaseUrl.ApiUrl + '/UtilApi?GetAllGeography=true&SearchParam=&CityId=' + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                                        url: BaseUrl.ApiUrl + '/UtilApi?GetAllGeography=true&SearchParam=' + options.data.filter.filters[0].value + '&CityId=' + '&PageNumber=0' + '&RowCount=65000&IsAutoComplete=true',
                                        dataType: 'json',
                                        success: function (result) {
                                            options.success(result);
                                        },
                                        error: function (result) {
                                            options.error(result);
                                        }
                                    });
                                },
                            }
                        }
                    });
                },
            }, {
                field: 'GIT_CountryDirespCode',
                title: 'País DirespCode',
                width: '130px',
            }, {
                field: 'GIT_CountryEnable',
                title: 'Enable Pais',
                width: '120px',
            }, {
                field: 'GIT_State',
                title: 'Estado (Dpto.)',
                width: '120px',
            }, {
                field: 'GIT_StateDirespCode',
                title: 'Estado DirespCode',
                width: '140px',
            }, {
                field: 'GIT_StateEnable',
                title: 'Enable Estado (Dpto)',
                width: '150px',
            }, {
                field: 'AG_City',
                title: 'Ciudad',
                width: '150px',
            }, {
                field: 'GI_CityDirespCode',
                title: 'Ciudad DirespCode',
                width: '150px',
            }, {
                field: 'GI_CapitalType',
                title: 'Tipo de Capital',
                width: '150px',
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
            }, {
                field: 'GI_CityEnable',
                title: 'Enable Ciudad',
                width: '110px',
            }, ],
        });
    },

    RenderBankAsync: function (param) {
        if (param != true) {
            var vSearchParam = '';
        }
        else {
            var vSearchParam = $('#SearchBoxId').val();
        }
        $('#' + Admin_CategoryObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar:
                [{ name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' },
                { name: "SearchBox", template: "<input id='SearchBoxId' type='text'value=''>" },
                { name: "SearchButton", template: "<a id='Buscar' href='javascript: Admin_CategoryObject.RenderBankAsync(" + "true" + ");'>Buscar</a" }],
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
                        id: 'B_BankId',
                        fields: {
                            GIT_Country: { editable: true, nullable: false },
                            GIT_CountryId: { editable: true, nullable: false },

                            B_Bank: { editable: true, nullable: false, validation: { required: true } },

                            B_BankCode: { editable: true, nullable: false },
                            B_BankCodeId: { editable: false },

                            B_City: { editable: true, nullable: false },
                            B_CityId: { editable: false },

                            B_BankEnable: { editable: true, type: 'boolean', defaultValue: true },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByBankAdmin=true&SearchParam=' + vSearchParam + '&CityId=' + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize + '&IsAutoComplete=false',
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', '0');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
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
                                Message('error', '');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'B_Bank',
                title: 'Banco',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.B_Bank != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.B_Bank;
                    }
                    return oReturn;
                },
                editor: function (container, options) {

                    // create an input element
                    var input = $('<input/>');
                    var isSelected = false;
                    // set its name to the field to which the column is bound ('name' in this case)
                    input.attr('value', options.model[options.field]);
                    // append it to the container
                    input.appendTo(container);
                    // initialize a Kendo UI AutoComplete
                    input.kendoAutoComplete({
                        dataTextField: 'ItemName',

                        change: function (e) {

                            if (isSelected == false) {
                                options.model['B_Bank'] = e.sender._old;
                                options.model.dirty = true;
                            }
                        },
                        select: function (e) {

                            var selectedItem = this.dataItem(e.item.index());

                            isSelected = true;
                            //set server fiel name
                            options.model['B_BankId'] = selectedItem.ItemId;
                            options.model['B_Bank'] = selectedItem.ItemName;

                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: 'json',
                            serverFiltering: true,
                            transport: {
                                read: function (options) {

                                    $.ajax({
                                        //url: BaseUrl.ApiUrl + '/UtilApi?GetAllGeography=true&SearchParam=&CityId=' + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                                        url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByBank=true&SearchParam=' + options.data.filter.filters[0].value,
                                        dataType: 'json',
                                        success: function (result) {
                                            options.success(result);
                                        },
                                        error: function (result) {
                                            options.error(result);
                                        }
                                    });
                                },
                            }
                        }
                    });
                },
            }, {
                field: 'B_BankCode',
                title: 'Código',
                width: '50px',
            }, {
                field: 'B_City',
                title: 'País',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.B_City != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.B_City;
                    }
                    return oReturn;
                },
                editor: function (container, options) {

                    // create an input element
                    var input = $('<input/>');
                    // set its name to the field to which the column is bound ('name' in this case)
                    input.attr('value', options.model[options.field]);
                    // append it to the container
                    input.appendTo(container);
                    // initialize a Kendo UI AutoComplete
                    input.kendoAutoComplete({
                        dataTextField: 'GIT_Country',

                        select: function (e) {

                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name
                            options.model['B_CityId'] = selectedItem.GIT_CountryId;
                            options.model['B_City'] = selectedItem.GIT_Country;

                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: 'json',
                            serverFiltering: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        //url: BaseUrl.ApiUrl + '/UtilApi?GetAllGeography=true&SearchParam=&CityId=' + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                                        url: BaseUrl.ApiUrl + '/UtilApi?GetAllGeography=true&SearchParam=' + options.data.filter.filters[0].value + '&CityId=' + '&PageNumber=0' + '&RowCount=65000&IsAutoComplete=true',
                                        dataType: 'json',
                                        success: function (result) {
                                            options.success(result);
                                        },
                                        error: function (result) {
                                            options.error(result);
                                            Message('error', '');
                                        }
                                    });
                                },
                            }
                        }
                    });
                },
            }, {
                field: 'B_BankEnable',
                title: 'Enable',
                width: '50px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem.B_BankEnable == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }],
        });
    },

    RenderCompanyRulesAsync: function (param) {
        if (param != true) {
            var vSearchParam = '';
        }
        else {
            var vSearchParam = $('#SearchBoxId').val();
        }
        $('#' + Admin_CategoryObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar:
                [{ name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' },
                { name: "SearchBox", template: "<input id='SearchBoxId' type='text'value=''>" },
                { name: "SearchButton", template: "<a id='Buscar' href='javascript: Admin_CategoryObject.RenderCompanyRulesAsync(" + "true" + ");'>Buscar</a" }],
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
                        id: 'CR_CompanyRuleId',
                        fields: {
                            CR_CompanyRule: { editable: true, nullable: false },
                            CR_CompanyRuleEnable: { editable: true, type: 'boolean', defaultValue: true },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByCompanyRulesAdmin=true&SearchParam=' + vSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', '0');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
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
                                Message('error', '');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'CR_CompanyRule',
                title: 'Empresa Certificadora',
                width: '150px',
            }, {
                field: 'CR_CompanyRuleEnable',
                title: 'Enable',
                width: '50px',
            }, ],
        });
    },

    RenderRulesAsync: function (param) {
        if (param != true) {
            var vSearchParam = '';
        }
        else {
            var vSearchParam = $('#SearchBoxId').val();
        }
        $('#' + Admin_CategoryObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar:
                [{ name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' },
                { name: "SearchBox", template: "<input id='SearchBoxId' type='text'value=''>" },
                { name: "SearchButton", template: "<a id='Buscar' href='javascript: Admin_CategoryObject.RenderRulesAsync(" + "true" + ");'>Buscar</a" }],
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
                        id: 'R_RuleId',
                        fields: {
                            R_Rule: { editable: true, nullable: false },
                            R_RuleEnable: { editable: true, type: 'boolean', defaultValue: true },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByRulesAdmin=true&SearchParam=' + vSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', '0');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
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
                                Message('error', '');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'R_Rule',
                title: 'Certificado',
            }, {
                field: 'R_RuleEnable',
                title: 'Habilitado',
            }, ],
        });
    },

    RenderResolutionAsync: function (param) {
        if (param != true) {
            var vSearchParam = '';
        }
        else {
            var vSearchParam = $('#SearchBoxId').val();
        }
        $('#' + Admin_CategoryObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar:
                [{ name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' },
                { name: "SearchBox", template: "<input id='SearchBoxId' type='text'value=''>" },
                { name: "SearchButton", template: "<a id='Buscar' href='javascript: Admin_CategoryObject.RenderResolutionAsync(" + "true" + ");'>Buscar</a" }],
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
                        id: 'RS_ResolutionId',
                        fields: {
                            RS_Resolution: { editable: true, nullable: false },
                            RS_ResolutionEnable: { editable: true, type: 'boolean', defaultValue: true },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByResolutionAdmin=true&SearchParam=' + vSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', '0');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
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
                                Message('error', '');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'RS_Resolution',
                title: 'Resolucion',
            }, {
                field: 'RS_ResolutionEnable',
                title: 'Habilitado',
            }, ],
        });
    },

    RenderActivityStandarAsync: function (param) {
        if (param != true) {
            var vSearchParam = '';
        }
        else {
            var vSearchParam = $('#SearchBoxId').val();
        }

        $('#' + Admin_CategoryObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar:
                [

                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' },
                { name: "SearchBox", template: "<input id='SearchBoxId' type='text'value=''>" },
                { name: "SearchButton", template: "<a id='Buscar' href='javascript: Admin_CategoryObject.RenderActivityStandarAsync(" + "true" + ");'>Buscar</a" }
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
                        id: 'ECS_EconomyActivityId',
                        fields: {
                            ECS_EconomyActivity: { editable: true, nullable: false },

                            ECS_TypeId: { editable: false, nullable: false },
                            ECS_Type: { editable: true, nullable: false },

                            ECS_CategoryId: { editable: false, nullable: false },
                            ECS_Category: { editable: true, nullable: false },

                            ECS_GroupId: { editable: false, nullable: false },
                            ECS_Group: { editable: true, nullable: false },

                            ECS_ProviderTypeId: { editable: false, nullable: false },
                            ECS_ProviderType: { editable: true, nullable: false },

                            ECS_ProviderTypeJoin: { editable: true, nullable: false },

                            ECS_GroupName: { editable: true, nullable: false },
                            ECS_Enable: { editable: true, type: 'boolean', defaultValue: true },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByEcoActivityAdmin=true&SearchParam=' + vSearchParam + '&CityId=' + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize + '&IsAutoComplete=false' + '&TreeId=' + Admin_CategoryObject.TreeId,
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', '0');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    update: function (options) {

                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
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
                                Message('error', '');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'ECS_EconomyActivity',
                title: 'Actividad Económica',
                width: '150px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.ECS_EconomyActivity != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.ECS_EconomyActivity;
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    var isSelected = false;
                    // create an input element
                    var input = $('<input/>');
                    // set its name to the field to which the column is bound ('name' in this case)
                    input.attr('value', options.model[options.field]);
                    // append it to the container
                    input.appendTo(container);
                    // initialize a Kendo UI AutoComplete
                    input.kendoAutoComplete({
                        dataTextField: 'ActivityName',
                        change: function (e) {
                            if (isSelected == false) {
                                options.model['ECS_EconomyActivity'] = e.sender._old;
                                options.model.dirty = true;
                            }
                        },
                        select: function (e) {
                            isSelected = true;
                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name
                            options.model['ECS_EconomyActivityId'] = selectedItem.EconomicActivityId;
                            options.model['ECS_EconomyActivity'] = selectedItem.ActivityName;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: 'json',
                            serverFiltering: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByActivity=true&IsDefault=false&SearchParam=' + options.data.filter.filters[0].value,
                                        dataType: 'json',
                                        success: function (result) {
                                            options.success(result);
                                        },
                                        error: function (result) {
                                            options.error(result);
                                            Message('error', '');
                                        }
                                    });
                                },
                            }
                        }
                    });
                },
            }, {
                field: 'ECS_Type',
                title: 'Tipo',
                width: '150px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.ECS_Type != null) {
                        $.each(Admin_CategoryObject.AdminOptions[103], function (item, value) {
                            if (dataItem.ECS_Type == value.ItemId) {
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
                            dataSource: Admin_CategoryObject.AdminOptions[103],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'ECS_ProviderTypeJoin',
                title: 'Tipo de Proveedor',
                width: '380px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.ECS_ProviderTypeJoin != null && dataItem.ECS_ProviderTypeJoin.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        $.each(dataItem.ECS_ProviderTypeJoin, function (item, value) {
                            oReturn = oReturn + value.ItemName + ',';
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {

                    //get current values
                    var oCurrentValue = new Array();
                    if (options.model[options.field] != null) {
                        $.each(options.model[options.field], function (item, value) {
                            oCurrentValue.push({
                                ItemId: value.ItemId,
                                ItemName: value.ItemName,
                            });
                        });
                    }
                    //init multiselect
                    $('<select id="' + Admin_CategoryObject.ObjectId + '_ProviderTypeMultiselect" multiple="multiple" />')
                        .appendTo(container)
                        .kendoMultiSelect({
                            minLength: 0,
                            dataValueField: 'ItemId',
                            dataTextField: 'ItemName',
                            autoBind: false,
                            itemTemplate: $('#' + Admin_CategoryObject.ObjectId + '_MultiAC_ItemTemplate').html(),
                            value: oCurrentValue,
                            change: function () {
                                //get selected values
                                if ($('#' + Admin_CategoryObject.ObjectId + '_ProviderTypeMultiselect').length > 0) {
                                    options.model[options.field] = $('#' + Admin_CategoryObject.ObjectId + '_ProviderTypeMultiselect').data('kendoMultiSelect')._dataItems;
                                    options.model.dirty = true;
                                }
                            },                            
                            dataSource: {
                                type: "json",
                                serverFiltering: true,
                                transport: {
                                    read: function (options) {
                                        options.success(Admin_CategoryObject.AdminOptions[610]);
                                    },
                                },
                            },
                        });

                    //remove attribute role from input for space search
                    var inputAux = $('#' + Admin_CategoryObject.ObjectId + '_ProviderTypeMultiselect').data("kendoMultiSelect").input;
                    $(inputAux).attr('role', '');
                },
            }, {
                field: 'ECS_Category',
                title: 'Categoría',
                width: '150px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.ECS_Category != null) {
                        $.each(Admin_CategoryObject.AdminOptions[104], function (item, value) {
                            if (dataItem.ECS_Category == value.ItemId) {
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
                            dataSource: Admin_CategoryObject.AdminOptions[104],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'ECS_GroupName',
                title: 'Grupo',
                width: '150px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.ECS_GroupName != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.ECS_GroupName;
                    }
                    return oReturn;
                },
                editor: function (container, options) {

                    // create an input element
                    var input = $('<input/>');
                    // set its name to the field to which the column is bound ('name' in this case)
                    input.attr('value', options.model[options.field]);
                    // append it to the container
                    input.appendTo(container);
                    // initialize a Kendo UI AutoComplete
                    input.kendoAutoComplete({
                        dataTextField: 'G_Group',
                        select: function (e) {

                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name                            
                            options.model['ECS_Group'] = selectedItem.G_GroupId;
                            options.model['ECS_GroupName'] = selectedItem.G_Group;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: 'json',
                            serverFiltering: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: BaseUrl.ApiUrl + '/UtilApi?CategotySearchByGroupStandarAdmin=true&SearchParam=' + options.data.filter.filters[0].value + '&PageNumber=0' + '&RowCount=650000&TreeId=7',
                                        dataType: 'json',
                                        success: function (result) {
                                            options.success(result);
                                        },
                                        error: function (result) {
                                            options.error(result);
                                            Message('error', '');
                                        }
                                    });
                                },
                            }
                        }
                    });
                },
            }, {
                field: 'ECS_Enable',
                title: 'Enable',
                width: '110px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem.ECS_Enable == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, ],
        });
    },

    RenderGroupStandarAsync: function (param) {
        if (param != true) {
            var vSearchParam = '';
        }
        else {
            var vSearchParam = $('#SearchBoxId').val();
        }
        $('#' + Admin_CategoryObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar:
                [{ name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' },
                { name: "SearchBox", template: "<input id='SearchBoxId' type='text'value=''>" },
                { name: "SearchButton", template: "<a id='Buscar' href='javascript: Admin_CategoryObject.RenderGroupStandarAsync(" + "true" + ");'>Buscar</a" }],
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
                        id: 'G_GroupId',
                        fields: {
                            G_Group: { editable: true, nullable: false },
                            G_GroupEnable: { editable: true, type: 'boolean', defaultValue: true },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategotySearchByGroupStandarAdmin=true&SearchParam=' + vSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize + '&TreeId=7',
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', '0');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
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
                                Message('error', '');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'G_Group',
                title: 'Grupo',
            }, {
                field: 'G_GroupEnable',
                title: 'Habilitado',
            }, ],
        });
    },

    RenderTreeAsync: function (param) {
        if (param != true) {
            var vSearchParam = '';
        }
        else {
            var vSearchParam = $('#SearchBoxId').val();
        }
        $('#' + Admin_CategoryObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar:
                [{ name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' },
                { name: "SearchBox", template: "<input id='SearchBoxId' type='text'value=''>" },
                { name: "SearchButton", template: "<a id='Buscar' href='javascript: Admin_CategoryObject.RenderTreeAsync(" + "true" + ");'>Buscar</a" }],
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
                        id: 'T_TreeId',
                        fields: {
                            T_TreeId: { editable: false, nullable: false },
                            T_TreeName: { editable: true, nullable: false },
                            T_TreeEnable: { editable: true, type: 'boolean', defaultValue: true },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByTreeAdmin=true&SearchParam=' + vSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', '0');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
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
                                Message('error', '');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'T_TreeId',
                title: 'Id',
            }, {
                field: 'T_TreeName',
                title: 'Nombre del Árbol',
            }, {
                field: "T_TreeId",
                title: "Agregar Items",
                template: $("#templateName").html()
            }, {
                field: 'T_TreeEnable',
                title: 'Habilitado',
            }, ],
        });
    },

    RenderTRMAsync: function (param) {
        if (param != true) {
            var vSearchParam = '';
        }
        else {
            var vSearchParam = $('#SearchBoxId').val();
        }
        $('#' + Admin_CategoryObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            scrollable: true,
            toolbar:
                [{ name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' }],
            dataSource: {
                pageSize: 20000,
                serverPaging: true,
                schema: {
                    total: function (data) {
                        if (data && data.length > 0) {
                            return data[0].AllTotalRows;
                        }
                        return 0;
                    },
                    model: {
                        id: 'C_CurrentExchangeId',
                        fields: {
                            C_CurrentExchangeId: { editable: false, nullable: false },
                            C_IssueDate: { editable: true, nullable: false },
                            C_MoneyTypeFromId: { editable: true, nullable: false },
                            C_MoneyTypeToId: { editable: true, nullable: false },
                            C_MoneyTypeToName: { editable: true, nullable: false },
                            C_Rate: { editable: true, nullable: false },
                            C_CreateDate: { editable: false, nullable: false },
                            C_LastModify: { editable: false, nullable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByTRMAdmin=true',
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?CategoryUpsert=true&CategoryType=' + Admin_CategoryObject.CategoryType + '&TreeId=' + Admin_CategoryObject.TreeId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'El registro se ha guardado correctamente con el id ' + result.C_CurrentExchangeId);
                                $('#' + Admin_CategoryObject.ObjectId).data('kendoGrid').dataSource.read();
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', '');
                            }
                        });
                    },
                    update: function (options) {
                        Message('error', 'La TRM no puede ser editada.');
                    }
                },
            },
            columns: [{
                field: 'C_IssueDate',
                title: 'TRM Fecha',
                width: '160px',
                format: Admin_CategoryObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                }
            }, {
                field: 'C_MoneyTypeFromId',
                title: 'Conversión desde',
                width: '150px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.C_MoneyTypeFromId != null) {
                        $.each(Admin_CategoryObject.AdminOptions[108], function (item, value) {
                            if (dataItem.C_MoneyTypeFromId == value.ItemId) {
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
                            dataSource: Admin_CategoryObject.AdminOptions[108],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'C_MoneyTypeToId',
                title: 'Moneda Destino',
                width: '150px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.C_MoneyTypeToId != null) {
                        $.each(Admin_CategoryObject.AdminOptions[108], function (item, value) {
                            if (dataItem.C_MoneyTypeToId == value.ItemId) {
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
                            dataSource: Admin_CategoryObject.AdminOptions[108],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: "C_Rate",
                title: "Tarifa",
            }, ],
        });
    },
}

/*Message*/
function Message(style, idfield) {
    if ($('div.message').length) {
        $('div.message').remove();
    }

    var mess = '';
    if (style == 'error') {
        if (idfield.length > 0) {
            mess = 'Error en la fila con el id ' + idfield + '.';
        }
        else {
            mess = 'Hay un error!';
        }
    } else {
        if (idfield == '0') {
            mess = 'Se creó el registro.';
        }
        else if (idfield == '') {
            mess = 'Operación exitosa.';
        } else {
            mess = 'Se editó la fila con el id ' + idfield + '.';
        }
    }

    $('<div class="message m_' + style + '">' + mess + '</div>').css({
        top: $(window).scrollTop() + 'px'
    }).appendTo('body').slideDown(200).delay(3000).fadeOut(300, function () {
        $(this).remove();
    });
}

/*Customer search object*/
var Customer_SearchObject = {
    ObjectId: '',
    SearchFilter: '',
    PageSize: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchFilter = vInitObject.SearchFilter;
        this.PageSize = vInitObject.PageSize;
    },

    RenderAsync: function () {
        //init input
        $('#' + Customer_SearchObject.ObjectId + '_txtSearch').keypress(function (e) {
            if (e.which == 13) {
                Customer_SearchObject.SearchEvent(null, null);
            }
        });


        //init grid
        $('#' + Customer_SearchObject.ObjectId).kendoGrid({
            editable: false,
            navigatable: false,
            pageable: true,
            scrollable: true,
            selectable: true,
            dataSource: {
                pageSize: Customer_SearchObject.PageSize,
                serverPaging: true,
                schema: {
                    total: function (data) {
                        if (data != null && data.length > 0) {
                            return data[0].TotalRows;
                        }
                        return 0;
                    }
                },
                transport: {
                    read: function (options) {

                        var oSearchParam = $('#' + Customer_SearchObject.ObjectId + '_txtSearch').val();

                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SMCustomerSearch=true&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                            dataType: 'json',
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
            change: function (arg) {
                $.map(this.select(), function (item) {
                    if ($(item).find('td').length >= 2 && $($(item).find('td')[1]).text().length > 0) {
                        window.location = BaseUrl.SiteUrl + 'Customer/GICustomerUpsert?CustomerPublicId=' + $($(item).find('td')[1]).text().replace(/ /gi, '');
                    }
                });
            },
            columns: [{
                field: 'ImageUrl',
                title: 'Logo',
                template: '<img style="width:50px;height:50px;" src="${ImageUrl}" />',
                width: '36px',
                attributes: { style: "text-align:center;" },
            }, {
                field: 'CustomerPublicId',
                title: 'Id',
                width: '36px',
            }, {
                field: 'CustomerName',
                title: 'Nombre',
                width: '128px',
            }, {
                field: 'CustomerType',
                title: 'Tipo',
                width: '36px',
            }, {
                field: 'IdentificationType',
                title: 'Identification',
                template: '${IdentificationType} ${IdentificationNumber}',
                width: '50px',
            }, {
                field: 'Enable',
                title: 'Habilitado',
                width: '34px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.Enable == true) {
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

    SearchEvent: function (vSearchFilter, vSelected) {
        if (vSearchFilter != null && vSelected != null) {
            if (vSelected == true) {
                Customer_SearchObject.SearchFilter = vSearchFilter + ',' + Customer_SearchObject.SearchFilter;
            }
            else {
                Customer_SearchObject.SearchFilter = Customer_SearchObject.SearchFilter.replace(new RegExp(vSearchFilter, 'gi'), '');
            }
        }
        var oSearchParam = $('#' + Customer_SearchObject.ObjectId + '_txtSearch').val();
        window.location = BaseUrl.SiteUrl + 'Customer/Index?SearchParam=' + oSearchParam + '&SearchFilter=' + Customer_SearchObject.SearchFilter;
    },
};

/*Company Rules object*/
var Customer_RulesObject = {
    ObjectId: '',
    CustomerPublicId: '',
    RoleCompanyList: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.CustomerPublicId = vInitObject.CustomerPublicId;
        this.RoleCompanyList = vInitObject.RoleCompanyList;
    },

    RenderAsync: function () {

        Customer_RulesObject.Render_CustomerUserRules();

        Customer_RulesObject.ConfigEvents();
    },

    ConfigKeyBoard: function () {

        //init keyboard tooltip
        $('.divGrid_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {

            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Customer_RulesObject.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Customer_RulesObject.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Customer_RulesObject.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function () {

        //config grid infro visible enable event
        $('#' + Customer_RulesObject.ObjectId + '_ViewEnable').change(function () {
            $('#' + Customer_RulesObject.ObjectId).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnableInfo: function () {
        return $('#' + Customer_RulesObject.ObjectId + '_ViewEnable').length > 0 ? $('#' + Customer_RulesObject.ObjectId + '_ViewEnable').is(':checked') : true;
    },

    Render_CustomerUserRules: function () {
        $('#' + Customer_RulesObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar datos del listado' },
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Customer_RulesObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Customer_RulesObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "RoleCompanyId",
                        fields: {
                            UserCompanyId: { editable: false, nullable: true },

                            RoleCompanyName: { editable: true, validation: { required: true } },
                            User: { editable: true, validation: { required: true } },
                            UserCompanyEnable: { editable: true, type: 'boolean', defaultValue: true },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?UserRolesByCustomer=true&CustomerPublicId=' + Customer_RulesObject.CustomerPublicId + '&ViewEnable=' + Customer_RulesObject.GetViewEnableInfo(),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?UserCompanyUpsert=true&CustomerPublicId=' + Customer_RulesObject.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se creó el registro.');

                                $('#' + Customer_RulesObject.ObjectId).data('kendoGrid').dataSource.read();
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?UserCompanyUpsert=true&CustomerPublicId=' + Customer_RulesObject.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.UserCompanyId + '.');

                                $('#' + Customer_RulesObject.ObjectId).data('kendoGrid').dataSource.read();
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.UserCompanyId + '.');
                            },
                        });
                    },
                },
                requestStart: function () {
                    kendo.ui.progress($("#loading"), true);
                },
                requestEnd: function () {
                    kendo.ui.progress($("#loading"), false);
                }
            },
            columns: [{
                field: 'UserCompanyEnable',
                title: 'Habilitado',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.UserCompanyEnable == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'RoleCompanyId',
                title: 'Cargo',
                width: '150px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción';
                    $.each(Customer_RulesObject.RoleCompanyList, function (item, value) {
                        if (value.RoleId == dataItem.RoleCompanyId) {
                            oReturn = value.RoleName;
                        }
                    });

                    return oReturn;
                },
                editor: function (container, options) {
                    $('<input required data-bind="value:' + options.field + '"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            dataSource: Customer_RulesObject.RoleCompanyList,
                            dataTextField: 'RoleName',
                            dataValueField: 'RoleId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'User',
                title: 'Usuario',
                width: '200px',
            }, {
                field: 'UserCompanyId',
                title: 'Id',
                width: '100px',
            }, ],
        });

    },
};

/*Customer survey config object*/
var Customer_SurveyObject = {
    ObjectId: '',
    CustomerPublicId: '',
    PageSize: '',
    SurveyGroup: '',
    SurveyConfigItemUpsertUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.CustomerPublicId = vInitObject.CustomerPublicId;
        this.PageSize = vInitObject.PageSize;
        this.SurveyGroup = vInitObject.SurveyGroup;
        this.SurveyConfigItemUpsertUrl = vInitObject.SurveyConfigItemUpsertUrl;
    },

    RenderAsync: function () {
        //render grid
        Customer_SurveyObject.RenderSurveyConfig();

        //focus on the grid
        $('#' + Customer_SurveyObject.ObjectId).data("kendoGrid").table.focus();

        //config keyboard
        Customer_SurveyObject.ConfigKeyBoard();

        //Config Events
        Customer_SurveyObject.ConfigEvents();
    },

    ConfigKeyBoard: function () {

        //init keyboard tooltip
        $('.divGrid_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {

            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Customer_SurveyObject.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Customer_SurveyObject.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Customer_SurveyObject.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function () {
        //config grid visible enables event
        $('#' + Customer_SurveyObject.ObjectId + '_ViewEnable').change(function () {
            $('#' + Customer_SurveyObject.ObjectId).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnable: function () {
        return $('#' + Customer_SurveyObject.ObjectId + '_ViewEnable').length > 0 ? $('#' + Customer_SurveyObject.ObjectId + '_ViewEnable').is(':checked') : true;
    },

    GetSearchParam: function () {
        return $('#' + Customer_SurveyObject.ObjectId + '_txtSearch').val();
    },

    Search: function () {
        $('#' + Customer_SurveyObject.ObjectId).data('kendoGrid').dataSource.read();
    },

    RenderSurveyConfig: function () {
        $('#' + Customer_SurveyObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar datos del listado' },
                { name: 'cancel', text: 'Descartar' },
                { name: 'Search', template: $('#' + Customer_SurveyObject.ObjectId + '_SearchTemplate').html() },
                { name: 'ViewEnable', template: $('#' + Customer_SurveyObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Customer_SurveyObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                pageSize: Customer_SurveyObject.PageSize,
                serverPaging: true,
                schema: {
                    total: function (data) {
                        if (data != null && data.length > 0) {
                            return data[0].TotalRows;
                        }
                        return 0;
                    },
                    model: {
                        id: "SurveyConfigId",
                        fields: {
                            SurveyConfigId: { editable: false, nullable: true },
                            SurveyName: { editable: true, validation: { required: true } },
                            SurveyEnable: { editable: true, type: 'boolean', defaultValue: true },

                            Group: { editable: true },
                            GroupName: { editable: true },
                            GroupId: { editable: false },

                            StepEnable: { editable: true, type: 'boolean', defaultValue: true },
                            StepEnableId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigSearch=true&CustomerPublicId=' + Customer_SurveyObject.CustomerPublicId + '&SearchParam=' + Customer_SurveyObject.GetSearchParam() + '&Enable=' + Customer_SurveyObject.GetViewEnable() + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigUpsert=true&CustomerPublicId=' + Customer_SurveyObject.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se creó el registro.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigUpsert=true&CustomerPublicId=' + Customer_SurveyObject.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.SurveyConfigId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.SurveyConfigId + '.');
                            },
                        });
                    },
                },
                requestStart: function () {
                    kendo.ui.progress($("#loading"), true);
                },
                requestEnd: function () {
                    kendo.ui.progress($("#loading"), false);
                }
            },
            editable: "popup",
            columns: [{
                field: 'SurveyEnable',
                title: 'Visible marketplace',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.SurveyEnable == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'Group',
                title: 'Grupo',
                width: '150px',
                template: '${GroupName}',
                editor: function (container, options) {
                    if (Customer_SurveyObject.SurveyGroup != null && Customer_SurveyObject.SurveyGroup.length > 0) {

                        $('<input data-bind="value:' + options.field + '"/>')
                            .appendTo(container)
                            .kendoDropDownList({
                                dataSource: {
                                    type: 'json',
                                    serverFiltering: true,
                                    transport: {
                                        read: function (options) {
                                            $.ajax({
                                                url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchBySurveyGroupAC=true&TreeId=' + Customer_SurveyObject.SurveyGroup + '&SearchParam=&RowCount=100',
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
                                },
                                dataTextField: 'ItemName',
                                dataValueField: 'ItemId',
                                optionLabel: 'Seleccione una opción',
                                select: function (e) {
                                    var selectedItem = this.dataItem(e.item.index());
                                    //set server fiel name
                                    options.model['GroupName'] = selectedItem.ItemName;
                                    //enable made changes
                                    options.model.dirty = true;
                                },
                            });
                    }
                    else {
                        $('<label>Este comprador no tiene grupo de encuesta relacionado</label>').appendTo(container);
                    }
                },
            }, {
                field: 'SurveyName',
                title: 'Nombre',
                width: '200px',
            }, {
                field: 'StepEnable',
                title: 'Paso a paso',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.StepEnable == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'SurveyConfigId',
                title: 'Id',
                width: '100px',
            }, {
                title: "&nbsp;",
                width: "200px",
                command: [{
                    name: 'edit',
                    text: 'Editar'
                }, {
                    name: 'Detail',
                    text: 'Ver detalle',
                    click: function (e) {
                        // e.target is the DOM element representing the button
                        var tr = $(e.target).closest("tr"); // get the current table row (tr)
                        // get the data bound to the current table row
                        var data = this.dataItem(tr);
                        //validate SurveyConfigId attribute
                        if (data.SurveyConfigId != null && data.SurveyConfigId.length > 0) {
                            window.location = Customer_SurveyObject.SurveyConfigItemUpsertUrl.replace(/\${SurveyConfigId}/gi, data.SurveyConfigId);
                        }
                    }
                }],
            }],
        });
    },
};

/*Customer survey config object*/
var Customer_SurveyItemObject = {
    ObjectId: '',
    CustomerPublicId: '',
    SurveyConfigId: '',
    HasEvaluations: false,
    PageSize: '',
    CustomerOptions: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.CustomerPublicId = vInitObject.CustomerPublicId;
        this.SurveyConfigId = vInitObject.SurveyConfigId;
        this.HasEvaluations = vInitObject.HasEvaluations;
        this.PageSize = vInitObject.PageSize;
        if (vInitObject.CustomerOptions != null) {
            $.each(vInitObject.CustomerOptions, function (item, value) {
                Customer_SurveyItemObject.CustomerOptions[value.Key] = value.Value;
            });
        }
    },

    RenderAsync: function (vRenderObject) {

        //render grid by type
        if (vRenderObject.SurveyItemType == 1202001) {
            Customer_SurveyItemObject.RenderSurveyItemEvaluationArea(vRenderObject);
        }
        else if (vRenderObject.SurveyItemType == 1202002) {
            Customer_SurveyItemObject.RenderSurveyItemQuestion(vRenderObject);
        }
        else if (vRenderObject.SurveyItemType == 1202003) {
            Customer_SurveyItemObject.RenderSurveyItemAnswer(vRenderObject);
        }

        //focus on the grid
        $('#' + Customer_SurveyItemObject.ObjectId + '_' + vRenderObject.SurveyItemType).data("kendoGrid").table.focus();

        //Config Events
        Customer_SurveyItemObject.ConfigEvents(vRenderObject.SurveyItemType);
    },

    ConfigEvents: function (vSurveyItemType) {
        //config grid visible enables event
        $('#' + Customer_SurveyItemObject.ObjectId + '_' + vSurveyItemType).find('#' + Customer_SurveyItemObject.ObjectId + '_ViewEnable').change(function () {
            $('#' + Customer_SurveyItemObject.ObjectId + '_' + vSurveyItemType).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnable: function (vSurveyItemType) {
        return $('#' + Customer_SurveyItemObject.ObjectId + '_' + vSurveyItemType).find('#' + Customer_SurveyItemObject.ObjectId + '_ViewEnable').length > 0 ? $('#' + Customer_SurveyItemObject.ObjectId + '_' + vSurveyItemType).find('#' + Customer_SurveyItemObject.ObjectId + '_ViewEnable').is(':checked') : true;
    },

    //vRenderObject.SurveyItemType item type
    //vRenderObject.ParentSurveyConfigItem parent item
    //vRenderObject.Title parent name
    RenderSurveyItemEvaluationArea: function (vRenderObject) {

        $('#' + Customer_SurveyItemObject.ObjectId + '_' + vRenderObject.SurveyItemType).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar datos del listado' },
                { name: 'cancel', text: 'Descartar' },
                {
                    name: 'title',
                    template: function () {
                        return $('#' + Customer_SurveyItemObject.ObjectId + '_TitleTemplate').html().replace(/\${Title}/gi, vRenderObject.Title);
                    }
                },
                { name: 'ViewEnable', template: $('#' + Customer_SurveyItemObject.ObjectId + '_ViewEnablesTemplate').html() },
            ],
            dataSource: {
                pageSize: Customer_SurveyItemObject.PageSize,
                schema: {
                    model: {
                        id: "SurveyConfigItemId",
                        fields: {
                            SurveyConfigItemId: { editable: false, nullable: true },
                            SurveyConfigItemName: { editable: true, validation: { required: true } },
                            SurveyConfigItemTypeId: { editable: false, nullable: true },
                            ParentSurveyConfigItem: { editable: true, nullable: true },
                            SurveyConfigItemEnable: { editable: true, type: 'boolean', defaultValue: true },

                            SurveyConfigItemInfoOrder: { editable: true, validation: { required: true } },
                            SurveyConfigItemInfoOrderId: { editable: false },

                            SurveyConfigItemInfoWeight: { editable: !Customer_SurveyItemObject.HasEvaluations, type: 'number', validation: { required: true, min: 0, max: 100 } },
                            SurveyConfigItemInfoWeightId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemGetBySurveyConfigId=true&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId + '&ParentSurveyConfigItem=' + vRenderObject.ParentSurveyConfigItem + '&Enable=' + Customer_SurveyItemObject.GetViewEnable(vRenderObject.SurveyItemType),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemUpsert=true&CustomerPublicId=' + Customer_SurveyItemObject.CustomerPublicId + '&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se creó el registro.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemUpsert=true&CustomerPublicId=' + Customer_SurveyItemObject.CustomerPublicId + '&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.SurveyConfigItemId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.SurveyConfigItemId + '.');
                            },
                        });
                    },
                },
                requestStart: function () {
                    kendo.ui.progress($("#loading"), true);
                },
                requestEnd: function () {
                    kendo.ui.progress($("#loading"), false);
                }
            },
            editable: {
                mode: "popup",
                window: {
                    title: "Area de evaluación",
                }
            },
            edit: function (e) {
                if (e.model.isNew()) {
                    // set survey item type
                    e.model.SurveyConfigItemTypeId = vRenderObject.SurveyItemType;
                    e.model.ParentSurveyConfigItem = vRenderObject.ParentSurveyConfigItem;
                }
            },
            columns: [{
                field: 'SurveyConfigItemEnable',
                title: 'Visible marketplace',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.SurveyConfigItemEnable == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'SurveyConfigItemName',
                title: 'Nombre',
                width: '200px',
                editor: function (container, options) {
                    $('<textarea data-bind="value: ' + options.field + '" style="height: 115px"></textarea>').appendTo(container);
                },
            }, {
                field: 'SurveyConfigItemInfoOrder',
                title: 'Orden',
                width: '50px',
                format: '{0:n0}'
            }, {
                field: 'SurveyConfigItemInfoWeight',
                title: 'Peso',
                width: '50px',
                format: '{0:n0}'
            }, {
                field: 'SurveyConfigItemId',
                title: 'Id',
                width: '100px',
            }, {
                title: "&nbsp;",
                width: "200px",
                command: [{
                    name: 'edit',
                    text: 'Editar'
                }, {
                    name: 'Detail',
                    text: 'Ver detalle',
                    click: function (e) {
                        // e.target is the DOM element representing the button
                        var tr = $(e.target).closest("tr"); // get the current table row (tr)
                        // get the data bound to the current table row
                        var data = this.dataItem(tr);
                        //validate SurveyConfigItemTypeId attribute
                        if (data.SurveyConfigItemTypeId != null && data.SurveyConfigItemTypeId.length > 0) {
                            //is in evaluation area show question
                            Customer_SurveyItemObject.RenderAsync({
                                SurveyItemType: '1202002',
                                ParentSurveyConfigItem: data.SurveyConfigItemId,
                                Title: data.SurveyConfigItemName,
                            });
                        }
                    }
                }],
            }],
        });
    },

    //vRenderObject.SurveyItemType item type
    //vRenderObject.ParentSurveyConfigItem parent item
    //vRenderObject.Title parent name
    RenderSurveyItemQuestion: function (vRenderObject) {
        if ($('#' + Customer_SurveyItemObject.ObjectId + '_' + vRenderObject.SurveyItemType).data("kendoGrid")) {
            //destroy kendo grid if exist

            // destroy the Grid
            $('#' + Customer_SurveyItemObject.ObjectId + '_' + vRenderObject.SurveyItemType).data("kendoGrid").destroy();
            // empty the Grid content (inner HTML)
            $('#' + Customer_SurveyItemObject.ObjectId + '_' + vRenderObject.SurveyItemType).empty();

            //empty answer grid
            // destroy the Grid
            $('#' + Customer_SurveyItemObject.ObjectId + '_1202003').data("kendoGrid").destroy();
            // empty the Grid content (inner HTML)
            $('#' + Customer_SurveyItemObject.ObjectId + '_1202003').empty();
        }

        $('#' + Customer_SurveyItemObject.ObjectId + '_' + vRenderObject.SurveyItemType).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar datos del listado' },
                { name: 'cancel', text: 'Descartar' },
                {
                    name: 'title',
                    template: function () {
                        return $('#' + Customer_SurveyItemObject.ObjectId + '_TitleTemplate').html().replace(/\${Title}/gi, vRenderObject.Title);
                    }
                },
                { name: 'ViewEnable', template: $('#' + Customer_SurveyItemObject.ObjectId + '_ViewEnablesTemplate').html() },
            ],
            dataSource: {
                pageSize: Customer_SurveyItemObject.PageSize,
                schema: {
                    model: {
                        id: "SurveyConfigItemId",
                        fields: {
                            SurveyConfigItemId: { editable: false, nullable: true },
                            SurveyConfigItemName: { editable: true, validation: { required: true } },
                            SurveyConfigItemTypeId: { editable: false, nullable: true },
                            ParentSurveyConfigItem: { editable: true, nullable: true },
                            SurveyConfigItemEnable: { editable: true, type: 'boolean', defaultValue: true },

                            SurveyConfigItemInfoOrder: { editable: true, validation: { required: true } },
                            SurveyConfigItemInfoOrderId: { editable: false },

                            SurveyConfigItemInfoWeight: { editable: !Customer_SurveyItemObject.HasEvaluations, type: 'number', validation: { required: true, min: 0, max: 100 } },
                            SurveyConfigItemInfoWeightId: { editable: false },

                            SurveyConfigItemInfoHasDescription: { editable: true, type: 'boolean', defaultValue: true },
                            SurveyConfigItemInfoHasDescriptionId: { editable: false },

                            SurveyConfigItemInfoIsMandatory: { editable: true, type: 'boolean', defaultValue: true },
                            SurveyConfigItemInfoIsMandatoryId: { editable: false },

                            SurveyConfigItemInfoQuestionTypeId: { editable: true, validation: { required: true } },
                            SurveyConfigItemInfoQuestionType: { editable: true },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemGetBySurveyConfigId=true&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId + '&ParentSurveyConfigItem=' + vRenderObject.ParentSurveyConfigItem + '&Enable=' + Customer_SurveyItemObject.GetViewEnable(vRenderObject.SurveyItemType),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemUpsert=true&CustomerPublicId=' + Customer_SurveyItemObject.CustomerPublicId + '&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se creó el registro.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemUpsert=true&CustomerPublicId=' + Customer_SurveyItemObject.CustomerPublicId + '&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.SurveyConfigItemId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.SurveyConfigItemId + '.');
                            },
                        });
                    },
                },
                requestStart: function () {
                    kendo.ui.progress($("#loading"), true);
                },
                requestEnd: function () {
                    kendo.ui.progress($("#loading"), false);
                }
            },
            editable: {
                mode: "popup",
                window: {
                    title: "Preguntas",
                }
            },
            edit: function (e) {
                if (e.model.isNew()) {
                    // set survey item type
                    e.model.SurveyConfigItemTypeId = vRenderObject.SurveyItemType;
                    e.model.ParentSurveyConfigItem = vRenderObject.ParentSurveyConfigItem;
                }
            },
            columns: [{
                field: 'SurveyConfigItemEnable',
                title: 'Visible marketplace',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.SurveyConfigItemEnable == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'SurveyConfigItemInfoQuestionType',
                title: 'Tipo de Pregunta',
                width: '190px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.SurveyConfigItemInfoQuestionType != null) {
                        $.each(Customer_SurveyItemObject.CustomerOptions[118], function (item, value) {
                            if (dataItem.SurveyConfigItemInfoQuestionType == value.ItemId) {
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
                            dataSource: Customer_SurveyItemObject.CustomerOptions[118],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'SurveyConfigItemName',
                title: 'Nombre',
                width: '200px',
                editor: function (container, options) {
                    $('<textarea data-bind="value: ' + options.field + '" style="height: 115px"></textarea>').appendTo(container);
                },
            }, {
                field: 'SurveyConfigItemInfoOrder',
                title: 'Orden',
                width: '80px',
                format: '{0:n0}'
            }, {
                field: 'SurveyConfigItemInfoWeight',
                title: 'Peso',
                width: '50px',
                format: '{0:n0}'
            }, {
                field: 'SurveyConfigItemInfoHasDescription',
                title: 'Mostrar descripción',
                width: '150px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.SurveyConfigItemInfoHasDescription == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'SurveyConfigItemInfoIsMandatory',
                title: 'Obligatorio',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.SurveyConfigItemInfoIsMandatory == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'SurveyConfigItemId',
                title: 'Id',
                width: '50px',
            }, {
                title: "&nbsp;",
                width: "150px",
                command: [{
                    name: 'edit',
                    text: 'Editar'
                }, {
                    name: 'Detail',
                    text: 'Ver detalle',
                    click: function (e) {
                        // e.target is the DOM element representing the button
                        var tr = $(e.target).closest("tr"); // get the current table row (tr)
                        // get the data bound to the current table row
                        var data = this.dataItem(tr);
                        //validate SurveyConfigItemTypeId attribute
                        if (data.SurveyConfigItemTypeId != null && data.SurveyConfigItemTypeId.length > 0 && data.SurveyConfigItemInfoQuestionType == '118001') {
                            //is in question show answer
                            Customer_SurveyItemObject.RenderAsync({
                                SurveyItemType: '1202003',
                                ParentSurveyConfigItem: data.SurveyConfigItemId,
                                Title: data.SurveyConfigItemName,
                            });
                        }
                        else if (data.SurveyConfigItemInfoQuestionType != '118001') {
                            Message('success', 'Los campos de tipo pregunta no poseen atributos de tipo respuesta.');
                        }
                    }
                }],
            }],
        });
    },

    //vRenderObject.SurveyItemType item type
    //vRenderObject.ParentSurveyConfigItem parent item
    //vRenderObject.Title parent name
    RenderSurveyItemAnswer: function (vRenderObject) {

        if ($('#' + Customer_SurveyItemObject.ObjectId + '_' + vRenderObject.SurveyItemType).data("kendoGrid")) {
            //destroy kendo grid if exist

            // destroy the Grid
            $('#' + Customer_SurveyItemObject.ObjectId + '_' + vRenderObject.SurveyItemType).data("kendoGrid").destroy();
            // empty the Grid content (inner HTML)
            $('#' + Customer_SurveyItemObject.ObjectId + '_' + vRenderObject.SurveyItemType).empty();
        }

        $('#' + Customer_SurveyItemObject.ObjectId + '_' + vRenderObject.SurveyItemType).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar datos del listado' },
                { name: 'cancel', text: 'Descartar' },
                {
                    name: 'title',
                    template: function () {
                        return $('#' + Customer_SurveyItemObject.ObjectId + '_TitleTemplate').html().replace(/\${Title}/gi, vRenderObject.Title);
                    }
                },
                { name: 'ViewEnable', template: $('#' + Customer_SurveyItemObject.ObjectId + '_ViewEnablesTemplate').html() },
            ],
            dataSource: {
                pageSize: Customer_SurveyItemObject.PageSize,
                schema: {
                    model: {
                        id: "SurveyConfigItemId",
                        fields: {
                            SurveyConfigItemId: { editable: false, nullable: true },
                            SurveyConfigItemName: { editable: true, validation: { required: true } },
                            SurveyConfigItemTypeId: { editable: false, nullable: true },
                            ParentSurveyConfigItem: { editable: true, nullable: true },
                            SurveyConfigItemEnable: { editable: true, type: 'boolean', defaultValue: true },

                            SurveyConfigItemInfoOrder: { editable: true, validation: { required: true } },
                            SurveyConfigItemInfoOrderId: { editable: false },

                            SurveyConfigItemInfoWeight: { editable: !Customer_SurveyItemObject.HasEvaluations, type: 'number', validation: { required: true, min: 0, max: 100 } },
                            SurveyConfigItemInfoWeightId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemGetBySurveyConfigId=true&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId + '&ParentSurveyConfigItem=' + vRenderObject.ParentSurveyConfigItem + '&Enable=' + Customer_SurveyItemObject.GetViewEnable(vRenderObject.SurveyItemType),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemUpsert=true&CustomerPublicId=' + Customer_SurveyItemObject.CustomerPublicId + '&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se creó el registro.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemUpsert=true&CustomerPublicId=' + Customer_SurveyItemObject.CustomerPublicId + '&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.SurveyConfigItemId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.SurveyConfigItemId + '.');
                            },
                        });
                    },
                },
                requestStart: function () {
                    kendo.ui.progress($("#loading"), true);
                },
                requestEnd: function () {
                    kendo.ui.progress($("#loading"), false);
                }
            },
            editable: {
                mode: "popup",
                window: {
                    title: "Respuestas",
                }
            },
            edit: function (e) {
                if (e.model.isNew()) {
                    // set survey item type
                    e.model.SurveyConfigItemTypeId = vRenderObject.SurveyItemType;
                    e.model.ParentSurveyConfigItem = vRenderObject.ParentSurveyConfigItem;
                }
            },
            columns: [{
                field: 'SurveyConfigItemEnable',
                title: 'Visible marketplace',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.SurveyConfigItemEnable == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'SurveyConfigItemName',
                title: 'Nombre',
                width: '200px',
                editor: function (container, options) {
                    $('<textarea data-bind="value: ' + options.field + '" style="height: 115px"></textarea>').appendTo(container);
                },
            }, {
                field: 'SurveyConfigItemInfoOrder',
                title: 'Orden',
                width: '50px',
                format: '{0:n0}'
            }, {
                field: 'SurveyConfigItemInfoWeight',
                title: 'Peso',
                width: '50px',
                format: '{0:n0}'
            }, {
                field: 'SurveyConfigItemId',
                title: 'Id',
                width: '100px',
            }, {
                title: "&nbsp;",
                width: "200px",
                command: [{
                    name: 'edit',
                    text: 'Editar'
                }],
            }],
        });
    },
};

var Customer_ProjectConfig = {
    ObjectId: '',
    CustomerPublicId: '',
    ProjectConfigId: '',
    PageSize: '',
    EvaluationItemUpsertUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.CustomerPublicId = vInitObject.CustomerPublicId;
        this.ProjectConfigId = vInitObject.ProjectConfigId;
        this.PageSize = vInitObject.PageSize;
        this.EvaluationItemUpsertUrl = vInitObject.EvaluationItemUpsertUrl;
    },

    RenderAsync: function (vRenderObject) {

        Customer_ProjectConfig.RenderProjectConfig();

        //focus on the grid
        $('#' + Customer_ProjectConfig.ObjectId).data("kendoGrid").table.focus();

        //config keyboard
        Customer_ProjectConfig.ConfigKeyBoard();

        //Config Events
        Customer_ProjectConfig.ConfigEvents();
    },

    ConfigKeyBoard: function () {

        //init keyboard tooltip
        $('.divGrid_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {

            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Customer_ProjectConfig.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Customer_ProjectConfig.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Customer_ProjectConfig.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function () {
        //config grid visible enables event
        $('#' + Customer_ProjectConfig.ObjectId + '_ViewEnable').change(function () {
            $('#' + Customer_ProjectConfig.ObjectId).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnable: function () {
        return $('#' + Customer_ProjectConfig.ObjectId + '_ViewEnable').length > 0 ? $('#' + Customer_ProjectConfig.ObjectId + '_ViewEnable').is(':checked') : true;
    },

    GetSearchParam: function () {
        return $('#' + Customer_ProjectConfig.ObjectId + '_txtSearch').val();
    },

    Search: function () {
        $('#' + Customer_ProjectConfig.ObjectId).data('kendoGrid').dataSource.read();
    },

    RenderProjectConfig: function () {
        $('#' + Customer_ProjectConfig.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar datos del listado' },
                { name: 'Search', template: $('#' + Customer_ProjectConfig.ObjectId + '_SearchTemplate').html() },
                { name: 'ViewEnable', template: $('#' + Customer_ProjectConfig.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Customer_ProjectConfig.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                pageSize: Customer_ProjectConfig.PageSize,
                serverPaging: true,
                schema: {
                    total: function (data) {
                        if (data && data.length > 0) {
                            return data[0].TotalRows;
                        }
                        return 0;
                    },
                    model: {
                        id: "ProjectProviderId",
                        fields: {
                            ProjectProviderId: { editable: false, nullable: true },
                            ProjectProviderName: { editable: true, validation: { required: true } },
                            ProjectProviderEnable: { editable: true, type: 'boolean', defaultValue: true }
                        },
                    },
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCProjectConfigSearch=true&CustomerPublicId=' + Customer_ProjectConfig.CustomerPublicId + '&ViewEnable=' + Customer_ProjectConfig.GetViewEnable() + '&SearchParam=' + Customer_ProjectConfig.GetSearchParam() + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCProjectConfigUpsert=true&CustomerPublicId=' + Customer_ProjectConfig.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                $('#' + Customer_ProjectConfig.ObjectId).data('kendoGrid').dataSource.read();
                                Message('success', 'Se creó el registro.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCProjectConfigUpsert=true&CustomerPublicId=' + Customer_ProjectConfig.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                $('#' + Customer_ProjectConfig.ObjectId).data('kendoGrid').dataSource.read();
                                Message('success', 'Se editó la fila con el id ' + options.data.ProjectProviderId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.ProjectProviderId + '.');
                            },
                        });
                    },
                },
                requestStart: function () {
                    kendo.ui.progress($("#loading"), true);
                },
                requestEnd: function () {
                    kendo.ui.progress($("#loading"), false);
                },
            },
            editable: {
                mode: "popup",
                window: {
                    title: "Precalificación",
                }
            },
            columns: [{
                field: 'ProjectProviderEnable',
                title: 'Habilitado',
                width: '82px',
            }, {
                field: 'ProjectProviderName',
                title: 'Configuración',
                width: '300px',
            }, {
                field: 'ProjectProviderId',
                title: 'Id',
                width: '60px',
            }, {
                title: "&nbsp;",
                width: "200px",
                command: [{
                    name: 'edit',
                    text: 'Editar'
                }, {
                    name: 'Detail',
                    text: 'Ver detalle',
                    click: function (e) {
                        // e.target is the DOM element representing the button
                        var tr = $(e.target).closest("tr"); // get the current table row (tr)
                        // get the data bound to the current table row
                        var data = this.dataItem(tr);
                        //validate SurveyConfigId attribute
                        if (data.ProjectProviderId != null && data.ProjectProviderId > 0) {
                            window.location = Customer_ProjectConfig.EvaluationItemUpsertUrl.replace(/\${ProjectProviderId}/gi, data.ProjectProviderId);
                        }
                    }
                }],
            }],
        });
    },
};

var Customer_EvaluationItemObject = {
    ObjectId: '',
    CustomerPublicId: '',
    ProjectConfigId: '',
    PageSize: '',
    ProjectConfigOptionsList: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.CustomerPublicId = vInitObject.CustomerPublicId;
        this.ProjectConfigId = vInitObject.ProjectConfigId;
        this.PageSize = vInitObject.PageSize;
        this.ProjectConfigOptionsList = vInitObject.ProjectConfigOptionsList;

        if (vInitObject.ProjectConfigOptionsList != null) {
            $.each(vInitObject.ProjectConfigOptionsList, function (item, value) {
                Customer_EvaluationItemObject.ProjectConfigOptionsList[value.Key] = value.Value;
            });
        }
    },

    RenderAsync: function (vRenderObject) {
        if (vRenderObject.EvaluationItemType == 1401001) {
            Customer_EvaluationItemObject.RenderEvaluationArea(vRenderObject);

            //focus on the grid
            $('#' + Customer_EvaluationItemObject.ObjectId + '_' + vRenderObject.EvaluationItemType).data("kendoGrid").table.focus();

            //config keyboard
            Customer_EvaluationItemObject.ConfigKeyBoard(vRenderObject.EvaluationItemType);

            //Config Events
            Customer_EvaluationItemObject.ConfigEvents(vRenderObject.EvaluationItemType);
        }
        else if (vRenderObject.EvaluationItemType == 1401002) {
            Customer_EvaluationItemObject.RenderEvaluationCriteria(vRenderObject);
        }
    },

    ConfigKeyBoard: function (EvaluationItemType) {

        //init keyboard tooltip
        $('.divGrid_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {

            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Customer_EvaluationItemObject.ObjectId + '_' + EvaluationItemType).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Customer_EvaluationItemObject.ObjectId + '_' + EvaluationItemType).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Customer_EvaluationItemObject.ObjectId + '_' + EvaluationItemType).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function (EvaluationItemType) {
        //config grid visible enables event
        $('#' + Customer_EvaluationItemObject.ObjectId + '_ViewEnable').change(function () {
            $('#' + Customer_EvaluationItemObject.ObjectId + '_' + EvaluationItemType).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnable: function () {
        return $('#' + Customer_EvaluationItemObject.ObjectId + '_ViewEnable').length > 0 ? $('#' + Customer_EvaluationItemObject.ObjectId + '_ViewEnable').is(':checked') : true;
    },

    GetSearchParam: function () {
        return $('#' + Customer_EvaluationItemObject.ObjectId + '_txtSearch').val();
    },

    Search: function () {
        $('#' + Customer_EvaluationItemObject.ObjectId).data('kendoGrid').dataSource.read();
    },

    RenderEvaluationArea: function (vRenderObject) {
        $('#' + Customer_EvaluationItemObject.ObjectId + '_' + vRenderObject.EvaluationItemType).kendoGrid({
            editable: true,
            navigatable: false,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'Search', template: $('#' + Customer_EvaluationItemObject.ObjectId + '_SearchTemplate').html() },
                { name: 'ViewEnable', template: $('#' + Customer_EvaluationItemObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Customer_EvaluationItemObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "EvaluationItemId",
                        fields: {
                            EvaluationItemId: { editable: false, nullable: true },
                            EvaluationItemName: { editable: true, validation: { required: true } },
                            EvaluationItemEnable: { editable: true, type: 'boolean', defaultValue: true },
                            EvaluationItemTypeId: { editable: false, nullable: true },
                            EvaluationItemTypeName: { editable: false, nullable: true },
                            ParentEvaluationItem: { editable: true, nullable: true },

                            EvaluatorTypeId: { editable: false, nullable: true },
                            EvaluatorType: { editable: true },

                            EvaluatorId: { editable: false, nullable: true },
                            Evaluator: { editable: true },

                            UnitId: { editable: false, nullable: true },
                            Unit: { editable: true },

                            OrderId: { editable: false, nullable: true },
                            Order: { editable: true },

                            ApprovePercentageId: { editable: false, nullable: true },
                            ApprovePercentage: { editable: true },
                        },
                    },
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCEvaluationItemSearch=true&ProjectConfigId=' + Customer_EvaluationItemObject.ProjectConfigId + '&ParentEvaluationItem=&EvaluationItemType=' + vRenderObject.EvaluationItemType + '&SearchParam=' + Customer_EvaluationItemObject.GetSearchParam() + '&ViewEnable=' + Customer_EvaluationItemObject.GetViewEnable(),
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCEvaluationItemUpsert=true&CustomerPublicId=' + Customer_EvaluationItemObject.CustomerPublicId + '&ProjectConfigId=' + Customer_EvaluationItemObject.ProjectConfigId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se creó el registro.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCEvaluationItemUpsert=true&CustomerPublicId=' + Customer_EvaluationItemObject.CustomerPublicId + '&ProjectConfigId=' + Customer_EvaluationItemObject.ProjectConfigId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.ProjectConfigId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.ProjectConfigId + '.');
                            },
                        });
                    },
                },
                requestStart: function () {
                    kendo.ui.progress($("#loading"), true);
                },
                requestEnd: function () {
                    kendo.ui.progress($("#loading"), false);
                },
            },
            editable: {
                mode: "popup",
                window: {
                    title: "Area de configuración",
                }
            },
            edit: function (e) {
                if (e.model.isNew()) {
                    // set survey item type
                    e.model.EvaluationItemTypeId = vRenderObject.EvaluationItemType;
                    e.model.ParentEvaluationItem = vRenderObject.ParentEvaluationItem;
                }
            },
            columns: [{
                field: 'EvaluationItemEnable',
                title: 'Habilitado',
                width: '88px',
            }, {
                field: 'EvaluationItemName',
                title: 'Area',
                width: '200px',
            }, {
                field: 'EvaluatorType',
                title: 'Tipo de Evaluador',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.EvaluatorType != null) {
                        $.each(Customer_EvaluationItemObject.ProjectConfigOptionsList[1405], function (item, value) {
                            if (dataItem.EvaluatorType == value.ItemId) {
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
                            dataSource: Customer_EvaluationItemObject.ProjectConfigOptionsList[1405],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
                width: '160px',
            }, {
                field: 'Evaluator',
                title: 'Evaluador',
                width: '190px',
            }, {
                field: 'Unit',
                title: 'Unidad',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.Unit != null) {
                        $.each(Customer_EvaluationItemObject.ProjectConfigOptionsList[1403], function (item, value) {
                            if (dataItem.Unit == value.ItemId) {
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
                            dataSource: Customer_EvaluationItemObject.ProjectConfigOptionsList[1403],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
                width: '90px',
            }, {
                field: 'ApprovePercentage',
                title: '% de Aprobación',
                template: function (dataItem) {
                    var oReturn = 'No aplica.';
                    if (dataItem != null && dataItem.Unit == '1403002') {
                        oReturn = dataItem.ApprovePercentage;
                    }
                    return oReturn;
                },
                width: '130px',
            }, {
                field: 'Order',
                title: 'Orden',
                width: '70px',
            }, {
                field: 'EvaluationItemId',
                title: 'Id',
                width: '70px',
            }, {
                title: "Acciones;",
                width: "200px",
                command: [{
                    name: 'edit',
                    text: 'Editar'
                }, {
                    name: 'Detail',
                    text: 'Ver detalle',
                    click: function (e) {
                        // e.target is the DOM element representing the button
                        var tr = $(e.target).closest("tr"); // get the current table row (tr)
                        // get the data bound to the current table row
                        var data = this.dataItem(tr);
                        //validate SurveyConfigItemTypeId attribute
                        if (data.EvaluationItemTypeId != null && data.EvaluationItemTypeId > 0) {
                            //is in evaluation area show question
                            Customer_EvaluationItemObject.RenderAsync({
                                EvaluationItemType: '1401002',
                                ParentEvaluationItem: data.EvaluationItemId,
                            });
                        }
                    }
                }],
            }, ],
        });
    },

    RenderEvaluationCriteria: function (vRenderObject) {

        $.ajax({
            url: BaseUrl.ApiUrl + '/CustomerApi?PCEvaluationItemSearch=true&ProjectConfigId=' + Customer_EvaluationItemObject.ProjectConfigId + '&ParentEvaluationItem=' + vRenderObject.ParentEvaluationItem + '&EvaluationItemType=' + vRenderObject.EvaluationItemType + '&SearchParam=' + Customer_EvaluationItemObject.GetSearchParam() + '&ViewEnable=' + Customer_EvaluationItemObject.GetViewEnable(),
            dataType: 'json',
            success: function (result) {
                result.forEach(function (val) {
                    if (val.Unit == "1403001") {
                        $('#' + Customer_EvaluationItemObject.ObjectId + '_' + val.Unit).dialog({
                            width: 500,
                        });
                    } else if (val.Unit == "1403002") {
                        $('#' + Customer_EvaluationItemObject.ObjectId + '_' + val.Unit).dialog({
                            width: 500,
                        });
                    } else if (val.Unit == "1403003") {
                        $('#' + Customer_EvaluationItemObject.ObjectId + '_' + val.Unit).dialog({
                            width: 500,
                        });
                    }
                });
            },
            error: function (result) {

            }
        });
    },
};
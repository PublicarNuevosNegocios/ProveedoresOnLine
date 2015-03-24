﻿/*Customer search object*/
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
                width: '40px',
                attributes: { style: "text-align:center;" },
            }, {
                field: 'CustomerPublicId',
                title: 'Id',
                width: '50px',
            }, {
                field: 'CustomerName',
                title: 'Nombre',
                width: '50px',
            }, {
                field: 'CustomerType',
                title: 'Tipo',
                width: '50px',
            }, {
                field: 'IdentificationType',
                title: 'Identification',
                template: '${IdentificationType} ${IdentificationNumber}',
                width: '50px',
            }, {
                field: 'Enable',
                title: 'Habilitado',
                width: '100px',
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?UserRolesByCustomer=true&CustomerPublicId=' + Customer_RulesObject.CustomerPublicId,
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
                            GroupId: { editable: true },
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
                field: 'SurveyConfigId',
                title: 'Id',
                width: '100px',
            }, {
                title: "&nbsp;",
                width: "250px",
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
    PageSize: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.CustomerPublicId = vInitObject.CustomerPublicId;
        this.SurveyConfigId = vInitObject.SurveyConfigId;
        this.PageSize = vInitObject.PageSize;
    },

    RenderAsync: function (vRenderObject) {

        //render grid by type
        Customer_SurveyItemObject.RenderSurveyItem(vRenderObject);

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
    RenderSurveyItem: function (vRenderObject) {

        $('#' + Customer_SurveyItemObject.ObjectId + '_' + vRenderObject.SurveyItemType).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar datos del listado' },
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Customer_SurveyItemObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Customer_SurveyItemObject.ObjectId + '_ShortcutToolTipTemplate').html() },
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
                            SurveyConfigItemInfoOrderId: { editable: true },

                            SurveyConfigItemInfoWeight: { editable: true, type: 'number', validation: { required: true, min: 0, max: 100 } },
                            SurveyConfigItemInfoWeightId: { editable: true },
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
            editable: "popup",
            edit: function(e) {
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
            }, {
                field: 'SurveyConfigItemInfoOrder',
                title: 'Orden',
                width: '200px',
                format: '{0:n0}'
            }, {
                field: 'SurveyConfigItemInfoWeight',
                title: 'Peso',
                width: '100px',
                format: '{0:n0}'
            }, {
                field: 'SurveyConfigItemId',
                title: 'Id',
                width: '100px',
            }, {
                title: "&nbsp;",
                width: "250px",
                command: [{
                    name: 'edit',
                    text: 'Editar'
                }, {
                    name: 'Detail',
                    text: 'Ver detalle',
                    click: function (e) {
                        //// e.target is the DOM element representing the button
                        //var tr = $(e.target).closest("tr"); // get the current table row (tr)
                        //// get the data bound to the current table row
                        //var data = this.dataItem(tr);
                        ////validate SurveyConfigId attribute
                        //if (data.SurveyConfigId != null && data.SurveyConfigId.length > 0) {
                        //    window.location = Customer_SurveyObject.SurveyConfigItemUpsertUrl.replace(/\${SurveyConfigId}/gi, data.SurveyConfigId);
                        //}
                    }
                }],
            }],
        });
    },
};


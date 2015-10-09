/*Generic provider submit form*/
function Customer_SubmitForm(SubmitObject) {
    if (SubmitObject.StepValue != null && SubmitObject.StepValue.length > 0 && $('#StepAction').length > 0) {
        $('#StepAction').val(SubmitObject.StepValue);
    }
    $('#' + SubmitObject.FormId).submit();
    Message('success', null);
}

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
    RoleCompanyList: new Array(),

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
        this.RoleCompanyList = vInitObject.RoleCompanyList;
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
        else if (vRenderObject.SurveyItemType == 1202004) {
            Customer_SurveyItemObject.RenderSurveyItemRol(vRenderObject);
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

                            SurveyConfigItemInfoAreaHasDescription: { editable: true, type: 'boolean', defaultValue: true },
                            SurveyConfigItemInfoAreaHasDescriptionId: { editable: false },

                            SurveyConfigItemInfoWeight: { editable: !Customer_SurveyItemObject.HasEvaluations, type: 'number', validation: { required: true, min: 0, max: 100 } },
                            SurveyConfigItemInfoWeightId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemGetBySurveyConfigId=true&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId + '&ParentSurveyConfigItem=' + vRenderObject.ParentSurveyConfigItem + '&SurveyConfigItemType=' + vRenderObject.SurveyItemType + '&Enable=' + Customer_SurveyItemObject.GetViewEnable(vRenderObject.SurveyItemType),
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
                field: 'SurveyConfigItemInfoAreaHasDescription',
                title: 'Descripción',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.SurveyConfigItemInfoAreaHasDescription == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'SurveyConfigItemInfoWeight',
                title: 'Peso',
                width: '50px',
                format: '{0:n1}'
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
                }, {
                    name: 'Add Role',
                    text: 'Agregar Roles',
                    click: function (e) {
                        // e.target is the DOM element representing the button
                        var tr = $(e.target).closest("tr"); // get the current table row (tr)
                        // get the data bound to the current table row
                        var data = this.dataItem(tr);
                        //validate SurveyConfigItemTypeId attribute
                        if (data.SurveyConfigItemTypeId != null && data.SurveyConfigItemTypeId.length > 0) {
                            //is in evaluation area show question
                            Customer_SurveyItemObject.RenderAsync({
                                SurveyItemType: '1202004',
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
                pageSize: 100,
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemGetBySurveyConfigId=true&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId + '&ParentSurveyConfigItem=' + vRenderObject.ParentSurveyConfigItem + '&SurveyConfigItemType=' + vRenderObject.SurveyItemType + '&Enable=' + Customer_SurveyItemObject.GetViewEnable(vRenderObject.SurveyItemType),
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
                            optionLabel: 'Seleccione una opción',
                            select: function (e) {
                                if (this.dataItem(e.item.index()).ItemId == 118002) {
                                    $('[data-container-for="SurveyConfigItemInfoWeight"]').hide();
                                    options.model.SurveyConfigItemInfoIsMandatory = false;
                                    $('[data-container-for="SurveyConfigItemInfoHasDescription"]').hide();
                                    options.model.SurveyConfigItemInfoHasDescription = false;
                                    $('[data-container-for="SurveyConfigItemInfoIsMandatory"]').hide();
                                }
                                else {
                                    $('[data-container-for="SurveyConfigItemInfoWeight"]').show();
                                    $('[data-container-for="SurveyConfigItemInfoHasDescription"]').show();
                                    $('[data-container-for="SurveyConfigItemInfoIsMandatory"]').show();
                                }
                            },
                        });

                    if (options.model[options.field] == 118002) {
                        $('[data-container-for="SurveyConfigItemInfoWeight"]').hide();
                        options.model.SurveyConfigItemInfoIsMandatory = false;
                        $('[data-container-for="SurveyConfigItemInfoHasDescription"]').hide();
                        options.model.SurveyConfigItemInfoHasDescription = false;
                        $('[data-container-for="SurveyConfigItemInfoIsMandatory"]').hide();
                    }
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
                format: '{0:n1}',
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemGetBySurveyConfigId=true&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId + '&ParentSurveyConfigItem=' + vRenderObject.ParentSurveyConfigItem + '&SurveyConfigItemType=' + vRenderObject.SurveyItemType + '&Enable=' + Customer_SurveyItemObject.GetViewEnable(vRenderObject.SurveyItemType),
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

    //vRenderObject.SurveyItemType item type
    //vRenderObject.ParentSurveyConfigItem parent item
    //vRenderObject.Title parent name
    RenderSurveyItemRol: function (vRenderObject) {
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
                            SurveyConfigItemEnable: { editable: true, type: 'boolean', defaultValue: true },

                            SurveyConfigItemInfoRol: { editable: true, validation: { required: true } },
                            SurveyConfigItemInfoRolId: { editable: false },

                            SurveyConfigItemInfoRolWeight: { editable: !Customer_SurveyItemObject.HasEvaluations, type: 'number', validation: { required: true, min: 0, max: 100 } },
                            SurveyConfigItemInfoRolWeightId: { editable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SCSurveyConfigItemGetBySurveyConfigId=true&SurveyConfigId=' + Customer_SurveyItemObject.SurveyConfigId + '&ParentSurveyConfigItem=' + vRenderObject.ParentSurveyConfigItem + '&SurveyConfigItemType=' + vRenderObject.SurveyItemType + '&Enable=' + Customer_SurveyItemObject.GetViewEnable(vRenderObject.SurveyItemType),
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
                    title: "Rol",
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
                field: 'SurveyConfigItemInfoRol',
                title: 'Rol',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.SurveyConfigItemInfoRol != null) {
                        $.each(Customer_SurveyItemObject.RoleCompanyList, function (item, value) {
                            if (dataItem.dirty != null && dataItem.dirty == true) {
                                oReturn = '<span class="k-dirty"></span>';
                            }
                            else if (value.RoleId == dataItem.SurveyConfigItemInfoRol) {
                                oReturn = value.RoleName;
                            }
                            else if (dataItem.SurveyConfigItemInfoRol == '') {
                                oReturn = '<label class="PlaceHolder">Ej: Evaluador</label>';
                            }
                        });
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
                        dataTextField: 'RoleName',
                        select: function (e) {
                            var selectedItem = this.dataItem(e.item.index());
                            //set server field name
                            options.model[options.field] = selectedItem.RoleName;
                            options.model['SurveyConfigItemInfoRol'] = selectedItem.RoleId;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: 'json',
                            serverFiltering: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: BaseUrl.ApiUrl + '/CustomerApi?RoleCompanyGetByPublicId=true&ViewEnable=true' + '&CustomerPublicId=' + Customer_SurveyItemObject.CustomerPublicId,
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
                field: 'SurveyConfigItemInfoRolWeight',
                title: 'Peso',
                width: '50px',
                format: '{0:n0}'
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

var Customer_ProjectModule = {
    ObjectId: '',
    AutoCompleteId: '',
    ControlToRetornACId: '',
    CustomerPublicId: '',
    ProjectConfigId: '',
    EvaluationItemId: '',
    EvaluationItemInfoId: '',
    PageSize: '',
    EvaluationItemUpsertUrl: '',
    EvaluationCriteriaUpsertUrl: '',
    RoleCompanyList: new Array(),
    ProjectConfigOptionsList: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.AutoCompleteId = vInitObject.AutoCompleteId;
        this.ControlToRetornACId = vInitObject.ControlToRetornACId;
        this.CustomerPublicId = vInitObject.CustomerPublicId;
        this.ProjectConfigId = vInitObject.ProjectConfigId;
        this.EvaluationItemId = vInitObject.EvaluationItemId;
        this.EvaluationItemInfoId = vInitObject.EvaluationItemInfoId;
        this.PageSize = vInitObject.PageSize;
        this.EvaluationItemUpsertUrl = vInitObject.EvaluationItemUpsertUrl;
        this.EvaluationCriteriaUpsertUrl = vInitObject.EvaluationCriteriaUpsertUrl;

        if (vInitObject.RoleCompanyList != null) {
            this.RoleCompanyList = vInitObject.RoleCompanyList;
        }

        if (vInitObject.ProjectConfigOptionsList != null) {
            $.each(vInitObject.ProjectConfigOptionsList, function (item, value) {
                Customer_ProjectModule.ProjectConfigOptionsList[value.Key] = value.Value;
            });
        }
    },

    RenderAsync: function (vRenderObject) {
        if (vRenderObject.EvaluationItemType == '0') {
            //Render project config
            Customer_ProjectModule.RenderProjectConfig();
        }
        else if (vRenderObject.EvaluationItemType == 1401001) {
            //Render evaluation item
            Customer_ProjectModule.RenderEvaluationItem(vRenderObject);
        }
        else if (vRenderObject.EvaluationItemType == 1401002) {
            //Render evaluation criteria
            Customer_ProjectModule.RenderEvaluationCriteria;
        }
        else if (vRenderObject.EvaluationItemType == 1401003) {
            Customer_ProjectModule.RenderProjectConfigExperiences();
        }

        //Render config options
        Customer_ProjectModule.ConfigKeyBoard();
        Customer_ProjectModule.ConfigEvents();
        Customer_ProjectModule.GetViewEnable();
        Customer_ProjectModule.GetSearchParam();
    },

    RenderAutocomplete: function () {
        Customer_ProjectModule.AutoComplete(Customer_ProjectModule.AutoCompleteId, Customer_ProjectModule.ControlToRetornACId);
    },

    AutoComplete: function (acId, ControlToRetornACId) {
        var acValue = $('#' + acId).val();
        $('#' + acId).kendoAutoComplete({
            dataTextField: "ItemName",
            select: function (e) {
                var selectedItem = this.dataItem(e.item.index());
                //set server fiel name
                $('#' + ControlToRetornACId).val(selectedItem.ItemId);
            },
            dataSource: {
                type: "json",
                serverFiltering: true,
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/UtilApi?GetHSEQCategory=true&SearchParam=' + options.data.filter.filters[0].value,
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

    ConfigKeyBoard: function () {
        //init keyboard tooltip
        $('.divGrid_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {
            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Customer_ProjectModule.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Customer_ProjectModule.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Customer_ProjectModule.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function () {
        //config grid visible enables event
        $('#' + Customer_ProjectModule.ObjectId + '_ViewEnable').change(function () {
            $('#' + Customer_ProjectModule.ObjectId).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnable: function () {
        return $('#' + Customer_ProjectModule.ObjectId + '_ViewEnable').length > 0 ? $('#' + Customer_ProjectModule.ObjectId + '_ViewEnable').is(':checked') : true;
    },

    GetSearchParam: function () {
        return $('#' + Customer_ProjectModule.ObjectId + '_txtSearch').val();
    },

    Search: function () {
        $('#' + Customer_ProjectModule.ObjectId).data('kendoGrid').dataSource.read();
    },

    RenderProjectConfig: function () {
        $('#' + Customer_ProjectModule.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: true,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'Search', template: $('#' + Customer_ProjectModule.ObjectId + '_SearchTemplate').html() },
                { name: 'ViewEnable', template: $('#' + Customer_ProjectModule.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Customer_ProjectModule.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                pageSize: Customer_ProjectModule.PageSize,
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCProjectConfigSearch=true&CustomerPublicId=' + Customer_ProjectModule.CustomerPublicId + '&ViewEnable=' + Customer_ProjectModule.GetViewEnable() + '&SearchParam=' + Customer_ProjectModule.GetSearchParam() + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCProjectConfigUpsert=true&CustomerPublicId=' + Customer_ProjectModule.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                $('#' + Customer_ProjectModule.ObjectId).data('kendoGrid').dataSource.read();
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCProjectConfigUpsert=true&CustomerPublicId=' + Customer_ProjectModule.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                $('#' + Customer_ProjectModule.ObjectId).data('kendoGrid').dataSource.read();
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
                            window.location = Customer_ProjectModule.EvaluationItemUpsertUrl.replace(/\${ProjectProviderId}/gi, data.ProjectProviderId);
                        }
                    }
                }],
            }],
        });
    },

    RenderEvaluationItem: function (vRenderObject) {
        $('#' + Customer_ProjectModule.ObjectId).kendoGrid({
            editable: true,
            navigatable: false,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'Search', template: $('#' + Customer_ProjectModule.ObjectId + '_SearchTemplate').html() },
                {
                    name: 'title',
                    template: function () {
                        return $('#' + Customer_ProjectModule.ObjectId + '_TitleTemplate').html().replace(/\${Title}/gi, vRenderObject.Title);
                    }
                },
                { name: 'ViewEnable', template: $('#' + Customer_ProjectModule.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Customer_ProjectModule.ObjectId + '_ShortcutToolTipTemplate').html() },
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

                            EA_EvaluatorTypeId: { editable: false },
                            EA_EvaluatorType: { editable: true },
                            EA_EvaluatorName: { editable: true, validation: { required: true } },

                            EA_EvaluatorId: { editable: false, nullable: true },
                            EA_Evaluator: { editable: true },

                            EA_UnitId: { editable: false, nullable: true },
                            EA_Unit: { editable: true },

                            EA_OrderId: { editable: false, nullable: true },
                            EA_Order: { editable: true },

                            EA_ApprovePercentageId: { editable: false, nullable: true },
                            EA_ApprovePercentage: { editable: true },
                        },
                    },
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCEvaluationItemSearch=true&ProjectConfigId=' + Customer_ProjectModule.ProjectConfigId + '&ParentEvaluationItem=&EvaluationItemType=' + vRenderObject.EvaluationItemType + '&SearchParam=' + Customer_ProjectModule.GetSearchParam() + '&ViewEnable=' + Customer_ProjectModule.GetViewEnable(),
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCEvaluationItemUpsert=true&CustomerPublicId=' + Customer_ProjectModule.CustomerPublicId + '&ProjectConfigId=' + Customer_ProjectModule.ProjectConfigId,
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?PCEvaluationItemUpsert=true&CustomerPublicId=' + Customer_ProjectModule.CustomerPublicId + '&ProjectConfigId=' + Customer_ProjectModule.ProjectConfigId,
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
                field: 'EA_EvaluatorType',
                title: 'Tipo de Evaluador',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.EA_EvaluatorType != null) {
                        $.each(Customer_ProjectModule.ProjectConfigOptionsList[1405], function (item, value) {
                            if (dataItem.EA_EvaluatorType == value.ItemId) {
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
                            dataSource: Customer_ProjectModule.ProjectConfigOptionsList[1405],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
                width: '160px',
            }, {
                field: 'EA_Evaluator',
                title: 'Evaluador',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción';
                    $.each(Customer_ProjectModule.RoleCompanyList, function (item, value) {
                        if (value.RoleId == dataItem.EA_Evaluator) {
                            oReturn = value.RoleName;
                        }
                    });
                    return oReturn;
                },
                editor: function (container, options) {
                    if (options.model.EA_EvaluatorType == '1405001') {
                        $('<input required data-bind="value:' + options.field + '"/>')
                                .appendTo(container)
                                .kendoDropDownList({
                                    dataSource: Customer_ProjectModule.RoleCompanyList,
                                    dataTextField: 'RoleName',
                                    dataValueField: 'RoleId',
                                    optionLabel: 'Seleccione una opción'
                                });
                    }
                    else {
                        $('<input type="text" class="k-input k-textbox" name="EA_Evaluator" required data-bind="value:' + options.field + '"/>')
                        .appendTo(container);
                    }
                },
                width: '190px',
            }, {
                field: 'EA_Unit',
                title: 'Unidad',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.EA_Unit != null) {
                        $.each(Customer_ProjectModule.ProjectConfigOptionsList[1403], function (item, value) {
                            if (dataItem.EA_Unit == value.ItemId) {
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
                            dataSource: Customer_ProjectModule.ProjectConfigOptionsList[1403],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
                width: '90px',
            }, {
                field: 'EA_ApprovePercentage',
                title: '% de Aprobación',
                template: function (dataItem) {
                    var oReturn = 'No aplica.';
                    if (dataItem != null && dataItem.EA_Unit == '1403002') {
                        oReturn = dataItem.EA_ApprovePercentage;
                    }
                    return oReturn;
                },
                width: '130px',
            }, {
                field: 'EA_Order',
                title: 'Orden',
                width: '70px',
            }, {
                field: 'EvaluationItemId',
                title: 'Id',
                width: '70px',
            }, {
                title: "Acciones",
                width: "200px",
                command: [{
                    name: 'edit',
                    text: 'Editar'
                }, {
                    name: 'Detail',
                    text: 'Ver Criterios',
                    click: function (e) {
                        // e.target is the DOM element representing the button
                        var tr = $(e.target).closest("tr"); // get the current table row (tr)
                        // get the data bound to the current table row
                        var data = this.dataItem(tr);

                        //validate SurveyConfigId attribute
                        //if (data.id != null && data.id > 0 && data.EvaluationItemId != null && data.EvaluationItemId > 0) {
                        //    window.location = Customer_ProjectModule.EvaluationCriteriaUpsertUrl.replace(/\${ProjectProviderId}/gi, Customer_ProjectModule.ProjectConfigId).replace(/\${EvaluationItemId}/gi, data.EvaluationItemId);
                        //}

                        //validate SurveyConfigItemTypeId attribute
                        if (data.EvaluationItemTypeId != null && data.EvaluationItemTypeId > 0) {
                            //is in evaluation area show question
                            vRenderObject.ParentEvaluationItem = data.EvaluationItemId;
                            vRenderObject.EvaluationItemType = '1401002';
                            vRenderObject.Title = data.EvaluationItemName;
                            Customer_EvaluationItemObject.RenderAsync(vRenderObject);
                        }
                    }
                }, {
                    name: 'Add Criteria',
                    text: 'Agregar Criterio',
                    click: function (e) {
                        // e.target is the DOM element representing the button
                        var tr = $(e.target).closest("tr");// get the current table row (tr)
                        // get the data bound to the current table row
                        var data = this.dataItem(tr);

                        //Redirect SurveyCriteriaUpsert
                        if (data.id != null && data.id > 0 && data.EvaluationItemId != null && data.EvaluationItemId > 0) {
                            window.location = Customer_ProjectModule.EvaluationCriteriaUpsertUrl.replace(/\${ProjectProviderId}/gi, Customer_ProjectModule.ProjectConfigId).replace(/\${EvaluationItemId}/gi, data.EvaluationItemId);
                        }
                    },
                }],
            }, ],
        });
    },
};

var ThirdKnowledgeObject = {
    ObjectId: '',
    CustomerPublicId: '',
    IsEnable: '',
    DateFormat: '',
    ThirdKnowledgeOptions: new Array(),
    PlanPublicId: '',
    PeriodPublicId: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId,
        this.CustomerPublicId = vInitObject.CustomerPublicId,
        this.IsEnable = vInitObject.Enable,
        this.DateFormat = vInitObject.DateFormat

        if (vInitObject.ThirdKnowledgeOptions != null) {
            $.each(vInitObject.ThirdKnowledgeOptions, function (item, value) {
                ThirdKnowledgeObject.ThirdKnowledgeOptions[value.Key] = value.Value;
            });
        }
    },

    RenderAsync: function (vRenderObject) {
        if (vRenderObject.ThirdKnowledgeType == 1601001) {
            ThirdKnowledgeObject.RenderPlan(vRenderObject);
        }
        if (vRenderObject.ThirdKnowledgeType == 1601002) {
            ThirdKnowledgeObject.RenderPeriods(vRenderObject);
        }
        if (vRenderObject.ThirdKnowledgeType == 1601003) {
            ThirdKnowledgeObject.RenderPeriodDetail(vRenderObject);
        }
    },

    RenderPlan: function (vRenderObject) {
        var isEdit = true;
        if (ThirdKnowledgeObject.PlanPublicId != "") {
            isEdit = false;
        }
        $('#' + ThirdKnowledgeObject.ObjectId + '_' + vRenderObject.ThirdKnowledgeType).kendoGrid({
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
                        return $('#' + ThirdKnowledgeObject.ObjectId + '_TitleTemplate').html().replace(/\${Title}/gi, vRenderObject.Title);
                    }
                },
                { name: 'ViewEnable', template: $('#' + ThirdKnowledgeObject.ObjectId + '_ViewEnablesTemplate').html() },
            ],
            dataSource: {
                pageSize: ThirdKnowledgeObject.PageSize,
                schema: {
                    model: {
                        id: "PlanPublicId",
                        fields: {
                            PlanPublicId: { editable: false, nullable: false },
                            QueriesByPeriod: { editable: true, nullable: false, validation: { requires: true } },
                            InitDate: { editable: true, nullable: false, validation: { requires: true } },
                            EndDate: { editable: true, nullable: false, validation: { requires: true } },
                            Status: { editable: true, nullable: false, validation: { requires: true } },
                            DaysByPeriod: { editable: true, nullable: false, validation: { requires: true } },
                            Enable: { editable: true, type: 'boolean', defaultValue: true },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?TDGetAllByCustomer=true&CustomerPublicId=' + ThirdKnowledgeObject.CustomerPublicId + '&Enable=' + ThirdKnowledgeObject.GetViewEnable(vRenderObject.ThirdKnowledgeType),
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?TDPlanUpsert=true&PlanPublicId=' + ThirdKnowledgeObject.PlanPublicId + "&CustomerPublicId=" + ThirdKnowledgeObject.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data),
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?TDPlanUpsert=true&PlanPublicId=' + ThirdKnowledgeObject.PlanPublicId + "&CustomerPublicId=" + ThirdKnowledgeObject.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.PlanPublicId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.SurveyConfigItemId + '.');
                            },
                        });
                    },
                },
                requestStart: function () {
                    kendo.ui.progress($("#TKloading"), true);
                },
                requestEnd: function () {
                    kendo.ui.progress($("#TKloading"), false);
                }
            },
            editable: {
                mode: "popup",
                window: {
                    title: "Asignación de plan",
                }
            },
            edit: function (e) {
                if (e.model.isNew()) {
                    // set survey item type
                    vRenderObject.PlanPublicId = e.model.PlanPublicId;
                }
                else {
                    $('[data-container-for="InitDate"]').hide();
                    $('[data-container-for="EndDate"]').hide();
                    $('[data-container-for="DaysByPeriod"]').hide();
                }
            },
            columns: [{
                field: 'QueriesByPeriod',
                title: 'Consultas por periodo',
                width: '160px',
            }, {
                field: 'InitDate',
                title: 'Fecha Inicial',
                width: '160px',
                format: ThirdKnowledgeObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                },
            }, {
                field: 'EndDate',
                title: 'Fecha Final',
                width: '160px',
                format: ThirdKnowledgeObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                }
            }, {
                field: 'Status',
                title: 'Estado',
                width: '90px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.Status != null) {
                        $.each(ThirdKnowledgeObject.ThirdKnowledgeOptions[101], function (item, value) {
                            if (dataItem.Status == value.ItemId) {
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
                            dataSource: ThirdKnowledgeObject.ThirdKnowledgeOptions[101],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'DaysByPeriod',
                title: 'Días por Periodo',
                width: '120px',
            }, {
                field: 'Enable',
                title: 'Habilitado Marketplace',
                width: '150px',
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
            },
                 {
                     title: "Acciones",
                     width: "200px",
                     command: [{
                         name: 'edit',
                         text: 'Editar',
                     }, {
                         name: 'Detail',
                         text: 'Ver Detalle',
                         click: function (e) {
                             // e.target is the DOM element representing the button
                             var tr = $(e.target).closest("tr"); // get the current table row (tr)
                             // get the data bound to the current table row
                             var data = this.dataItem(tr);

                             //validate Plan attribute
                             if (data.PlanPublicId != null) {
                                 vRenderObject.PlanPublicId = data.PlanPublicId;
                                 vRenderObject.ThirdKnowledgeType = '1601002';
                                 ThirdKnowledgeObject.RenderAsync(vRenderObject);
                             }
                         }
                     }, ],
                 }
            ]
        })
    },

    RenderPeriods: function (vRenderObject) {
        $('#' + ThirdKnowledgeObject.ObjectId + '_' + vRenderObject.ThirdKnowledgeType).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [{
                name: 'title',
                template: function () {
                    return $('#' + ThirdKnowledgeObject.ObjectId + '_TitleTemplate').html().replace(/\${Title}/gi, vRenderObject.Title);
                }
            },
                { name: 'ViewEnable', template: $('#' + ThirdKnowledgeObject.ObjectId + '_ViewEnablesTemplate').html() },
            ],
            dataSource: {
                pageSize: ThirdKnowledgeObject.PageSize,
                schema: {
                    model: {
                        id: "PeriodPublicId",
                        fields: {
                            PeriodPublicId: { editable: false, nullable: false },
                            AssignedQueries: { editable: true, nullable: false, validation: { requires: true } },
                            PeriodInitDate: { editable: true, nullable: false, validation: { requires: true } },
                            PeriodEndDate: { editable: true, nullable: false, validation: { requires: true } },
                            TotalQueries: { editable: true, nullable: false, validation: { requires: true } },
                            PeriodEnable: { editable: true, type: 'boolean', defaultValue: true },
                            PeriodLastModify: { editable: true, nullable: false, validation: { requires: true } },
                            PeriodCreateDate: { editable: true, nullable: false, validation: { requires: true } },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?TDGetPeriodsByPlanPublicId=true&PlanPublicId=' + vRenderObject.PlanPublicId + '&Enable=' + "true",
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?TDPlanUpsert=true&PlanPublicId=' + ThirdKnowledgeObject.PlanPublicId + "&CustomerPublicId=" + ThirdKnowledgeObject.CustomerPublicId,
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?TDPlanUpsert=true&PlanPublicId=' + ThirdKnowledgeObject.PlanPublicId,
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
                    title: "Asignación de plan",
                }
            },
            edit: function (e) {
                if (e.model.isNew()) {
                    // set survey item type
                    e.model.ThirdKnowledgeType = vRenderObject.ThirdKnowledgeType;
                }
            },
            columns: [{
                field: 'AssignedQueries',
                title: 'Consultas Asignadas',
                width: '150px',
            }, {
                field: 'PeriodInitDate',
                title: 'Inicio',
                width: '170px',
                format: ThirdKnowledgeObject.DateFormat,
                editor:
                    function timeEditor(container, options) {
                        var input = $('<input type="date" name="'
                            + options.field
                            + '" value="'
                            + options.model.get(options.field)
                            + '" />');
                        input.appendTo(container);
                    }
            }, {
                field: 'PeriodEndDate',
                title: 'Fin',
                width: '170px',
                format: ThirdKnowledgeObject.DateFormat,
                editor:
                    function timeEditor(container, options) {
                        var input = $('<input type="date" name="'
                            + options.field
                            + '" value="'
                            + options.model.get(options.field)
                            + '" />');
                        input.appendTo(container);
                    }
            }, {
                field: 'TotalQueries',
                title: 'Consultas Realizadas',
                width: '200px',
            }, {
                field: 'PeriodEnable',
                title: 'Habilitado Marketplace',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem.PeriodEnable == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'PeriodLastModify',
                title: 'Última Modificación',
                width: '170px',
                format: ThirdKnowledgeObject.DateFormat,
                editor:
                    function timeEditor(container, options) {
                        var input = $('<input type="date" name="'
                            + options.field
                            + '" value="'
                            + options.model.get(options.field)
                            + '" />');
                        input.appendTo(container);
                    }
            },
                {
                    title: "Acciones",
                    width: "200px",
                    command: [
                    {
                        name: 'Detail',
                        text: 'Ver Detalle',
                        click: function (e) {
                            // e.target is the DOM element representing the button
                            var tr = $(e.target).closest("tr"); // get the current table row (tr)

                            // get the data bound to the current table row
                            var data = this.dataItem(tr);

                            //validate Plan attribute
                            if (data.PeriodPublicId != null) {
                                vRenderObject.ThirdKnowledgeType = '1601003';
                                vRenderObject.Title = "Log";
                                vRenderObject.PeriodPublicId = data.PeriodPublicId;
                                ThirdKnowledgeObject.RenderAsync(vRenderObject);
                            }
                        }
                    }, ],
                }
            ]
        })
    },

    RenderPeriodDetail: function (vRenderObject) {
        $('#' + ThirdKnowledgeObject.ObjectId + '_' + vRenderObject.ThirdKnowledgeType).kendoGrid({
            editable: false,
            navigatable: true,
            pageable: false,
            scrollable: true,
            dataSource: {
                pageSize: ThirdKnowledgeObject.PageSize,
                schema: {
                    model: {
                        id: "PeriodPublicId",
                        fields: {
                            QueryPublicId: { editable: false, nullable: false },
                            QuerySearchType: { editable: false, nullable: false },
                            QueryUser: { editable: false, nullable: false },
                            QueryState: { editable: false, nullable: false },
                            QueryCreateDate: { editable: false, nullable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?TDGetQueriesByPeriodPublicId=true&PeriodPublicId=' + vRenderObject.PeriodPublicId + '&Enable=' + "true",
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    }
                },
                requestStart: function () {
                    kendo.ui.progress($("#loading"), true);
                },
                requestEnd: function () {
                    kendo.ui.progress($("#loading"), false);
                }
            },

            columns: [{
                field: 'QueryType',
                title: 'Tipo de consulta',
                width: '150px',
            }, {
                field: 'QueryUser',
                title: 'Responsable',
                width: '150px',
            }, {
                field: 'QueryStatus',
                title: 'Estado',
                width: '150px',
            }, {
                field: 'QueryCreateDate',
                title: 'Fecha',
                width: '170px',
                format: ThirdKnowledgeObject.DateFormat,
                editor:
                    function timeEditor(container, options) {
                        var input = $('<input type="date" name="'
                            + options.field
                            + '" value="'
                            + options.model.get(options.field)
                            + '" />');
                        input.appendTo(container);
                    }
            }]
        })
    },

    GetViewEnable: function (vThirdKnowledgeType) {
        return $('#' + ThirdKnowledgeObject.ObjectId + '_' + vThirdKnowledgeType).find('#' + ThirdKnowledgeObject.ObjectId + '_ViewEnable').length > 0 ? $('#' + ThirdKnowledgeObject.ObjectId + '_' + vThirdKnowledgeType).find('#' + ThirdKnowledgeObject.ObjectId + '_ViewEnable').is(':checked') : true;
    },
};

var Customer_AditionalDocumentsObject = {
    ObjectId: '',
    CustomerPublicId: '',
    ModulesList: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.CustomerPublicId = vInitObject.CustomerPublicId;

        this.ModulesList = vInitObject.Modules;
        //if (vInitObject.Modules != null) {
        //    $.each(vInitObject.Modules, function (key, value) {
        //        Customer_AditionalDocumentsObject.ModulesList[value.Key] = value.Value;
        //    });
        //}
    },

    RenderAsync: function () {
        Customer_AditionalDocumentsObject.Render_CustomerAditionalDocuments();

        Customer_AditionalDocumentsObject.ConfigEvents();
    },

    ConfigKeyBoard: function () {
        //init keyboard tooltip
        $('.divGrid_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {
            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Customer_AditionalDocumentsObject.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Customer_AditionalDocumentsObject.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Customer_AditionalDocumentsObject.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function () {
        //config grid infro visible enable event
        $('#' + Customer_AditionalDocumentsObject.ObjectId + '_ViewEnable').change(function () {
            $('#' + Customer_AditionalDocumentsObject.ObjectId).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnableInfo: function () {
        return $('#' + Customer_AditionalDocumentsObject.ObjectId + '_ViewEnable').length > 0 ? $('#' + Customer_AditionalDocumentsObject.ObjectId + '_ViewEnable').is(':checked') : true;
    },

    Render_CustomerAditionalDocuments: function () {
        $('#' + Customer_AditionalDocumentsObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar datos del listado' },
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Customer_AditionalDocumentsObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Customer_AditionalDocumentsObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "AditionalDataId",
                        fields: {
                            AditionalDataId: { editable: false, nullable: true },

                            Title: { editable: true, validation: { required: true } },

                            AditionalDataTypeId: { editable: false },
                            AditionalDataType: { editable: true, validation: { required: true } },

                            ModuleId: { editable: false },
                            Module: { editable: true, validation: { required: true } },

                            Enable: { editable: true, type: 'boolean', defaultValue: true },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?GetAditionalDocument=true&CustomerPublicId=' + Customer_AditionalDocumentsObject.CustomerPublicId + '&ViewEnable=' + Customer_AditionalDocumentsObject.GetViewEnableInfo(),
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
                            url: BaseUrl.ApiUrl + '/CustomerApi?ADAditionalDocumemntsUpsert=true&CustomerPublicId=' + Customer_AditionalDocumentsObject.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se creó el registro.');

                                $('#' + Customer_AditionalDocumentsObject.ObjectId).data('kendoGrid').dataSource.read();
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            },
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?ADAditionalDocumemntsUpsert=true&CustomerPublicId=' + Customer_AditionalDocumentsObject.CustomerPublicId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó el registro.');

                                $('#' + Customer_AditionalDocumentsObject.ObjectId).data('kendoGrid').dataSource.read();
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error al actualizar el registro.');
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
                field: 'Enable',
                title: 'Habilitado',
                width: '100px',
            }, {
                field: 'Title',
                title: 'Titulo',
                width: '100px',
            }, {
                field: 'AditionalDataType',
                title: 'Tipo de archivo',
                width: '150px',
            }, {
                field: 'Module',
                title: 'Modulo',
                width: '200px',
                //template: function (dataItem) {
                //    var oReturn = 'Seleccione una opción.';
                //    if (dataItem != null && dataItem.Module != null) {
                //        $.each(Customer_AditionalDocumentsObject.ModulesList, function (item, value) {
                //            debugger;
                //            if (dataItem.Module == value.Value[0].ItemId) {
                //                oReturn = value.Value[0].ItemName;
                //            }
                //        });
                //    }
                //    return oReturn;
                //},
                //editor: function (container, options) {
                //    debugger;
                //    $('<input required data-bind="value:' + options.field + '"/>')
                //        .appendTo(container)
                //        .kendoDropDownList({
                //            dataSource: Customer_AditionalDocumentsObject.ModulesList,
                //            dataTextField: '',
                //            dataValueField: '',
                //            optionLabel: 'Seleccione una opción'
                //        });
                //},
            }, {
                field: 'AditionalDataId',
                title: 'Id',
                width: '100px',
            }, ],
        });
    },
};
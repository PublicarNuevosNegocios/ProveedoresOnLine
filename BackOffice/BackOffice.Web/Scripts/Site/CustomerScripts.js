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
                width: '50px',
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
        $('#' + Customer_RulesObject.ObjectId + '_kbtooltip').tooltip();

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
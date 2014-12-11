/*init provider Menu*/
function Provider_InitMenu(InitObject) {
    $('#' + InitObject.ObjId).accordion({
        animate: 'swing',
        header: 'label',
        active: InitObject.active
    });
}

/*Generic provider submit form*/
function Provider_SubmitForm(SubmitObject) {
    if (SubmitObject.StepValue != null && SubmitObject.StepValue.length > 0 && $('#StepAction').length > 0) {
        $('#StepAction').val(SubmitObject.StepValue);
    }
    $('#' + SubmitObject.FormId).submit();
}

/*CompanyContactObject*/
var Provider_CompanyContactObject = {

    ObjectId: '',
    ProviderPublicId: '',
    ContactType: '',
    DateFormat: '',
    ProviderOptions: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.ProviderPublicId = vInitObject.ProviderPublicId;
        this.ContactType = vInitObject.ContactType;
        this.DateFormat = vInitObject.DateFormat;
        if (vInitObject.ProviderOptions != null) {
            $.each(vInitObject.ProviderOptions, function (item, value) {
                Provider_CompanyContactObject.ProviderOptions[value.Key] = value.Value;
            });
        }
    },

    RenderAsync: function () {
        if (Provider_CompanyContactObject.ContactType == 204001) {
            Provider_CompanyContactObject.RenderCompanyContact();
        }
        else if (Provider_CompanyContactObject.ContactType == 204002) {
            Provider_CompanyContactObject.RenderPersonContact();
        }
        else if (Provider_CompanyContactObject.ContactType == 204003) {
            Provider_CompanyContactObject.RenderBranch();
        }
        else if (Provider_CompanyContactObject.ContactType == 204004) {
            Provider_CompanyContactObject.RenderDistributor();
        }
    },

    RenderCompanyContact: function () {
        $('#' + Provider_CompanyContactObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' }
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "ContactId",
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            CC_CompanyContactType: { editable: true },
                            CC_CompanyContactTypeId: { editable: false },

                            CC_Value: { editable: true },
                            CC_ValueId: { editable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactGetByType=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                field: 'ContactId',
                title: 'Id',
                width: "50px",
            }, {
                field: 'ContactName',
                title: 'Nombre',
            }, {
                field: 'CC_CompanyContactType',
                title: 'Tipo de contacto',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.CC_CompanyContactType != null) {
                        $.each(Provider_CompanyContactObject.ProviderOptions[209], function (item, value) {
                            if (dataItem.CC_CompanyContactType == value.ItemId) {
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
                            dataSource: Provider_CompanyContactObject.ProviderOptions[209],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'CC_Value',
                title: 'Valor',
            }, {
                field: 'Enable',
                title: 'Habilitado',
                width: "100px",
            }],
        });
    },

    RenderPersonContact: function () {
        $('#' + Provider_CompanyContactObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' }
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "ContactId",
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            CP_PersonContactType: { editable: true },
                            CP_PersonContactTypeId: { editable: false },

                            CP_IdentificationType: { editable: true },
                            CP_IdentificationTypeId: { editable: false },

                            CP_IdentificationNumber: { editable: true, validation: { required: true } },
                            CP_IdentificationNumberId: { editable: false },

                            CP_IdentificationCity: { editable: true, validation: { required: false } },
                            CP_IdentificationCityId: { editable: false },

                            CP_IdentificationFile: { editable: true },
                            CP_IdentificationFileId: { editable: false },

                            CP_Name: { editable: true, validation: { required: true } },
                            CP_NameId: { editable: false },

                            CP_Phone: { editable: true, validation: { required: false } },
                            CP_PhoneId: { editable: false },

                            CP_Email: { editable: true, validation: { required: false, email: true } },
                            CP_EmailId: { editable: false },

                            CP_Negotiation: { editable: true, validation: { required: false } },
                            CP_NegotiationId: { editable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactGetByType=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                field: 'ContactId',
                title: 'Id',
                width: "50px",
            }, {
                field: 'ContactName',
                title: 'Nombre',
                width: "200px",
            }, {
                field: 'CP_PersonContactType',
                title: 'Tipo de representante',
                width: "200px",
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.CP_PersonContactType != null) {
                        $.each(Provider_CompanyContactObject.ProviderOptions[210], function (item, value) {
                            if (dataItem.CP_PersonContactType == value.ItemId) {
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
                            dataSource: Provider_CompanyContactObject.ProviderOptions[210],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'CP_IdentificationType',
                title: 'Tipo de identificación',
                width: "200px",
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.CP_IdentificationType != null) {
                        $.each(Provider_CompanyContactObject.ProviderOptions[101], function (item, value) {
                            if (dataItem.CP_IdentificationType == value.ItemId) {
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
                            dataSource: Provider_CompanyContactObject.ProviderOptions[101],
                            dataTextField: "ItemName",
                            dataValueField: "ItemId",
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'CP_IdentificationNumber',
                title: 'Número de identificación',
                width: "200px",
            }, {
                field: 'CP_IdentificationCity',
                title: 'Ciudad de expedicion del documento',
                width: "200px",
            }, {
                field: 'CP_Phone',
                title: 'Telefono',
                width: "200px",
            }, {
                field: 'CP_Email',
                title: 'Correo electronico',
                width: "200px",
            }, {
                field: 'CP_Negotiation',
                title: 'Capacidad de negociación',
                width: "200px",
            }, {
                field: 'CP_IdentificationFile',
                title: 'Doc representante legal.',
                width: "400px",
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CP_IdentificationFile != null && dataItem.CP_IdentificationFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyContactObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyContactObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${CP_IdentificationFile}/gi, dataItem.CP_IdentificationFile);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyContactObject.ProviderPublicId,
                            autoUpload: true
                        },
                        success: function (e) {
                            if (e.response != null && e.response.length > 0) {
                                //set server fiel name
                                options.model[options.field] = e.response[0].ServerName;
                                //enable made changes
                                options.model.dirty = true;
                            }
                        },
                        complete: function (e) {
                            //enable lost focus
                            oFileExit = true;
                        },
                        select: function (e) {
                            //disable lost focus while upload file
                            oFileExit = false;
                        },
                    });
                    $(container).focusout(function () {
                        if (oFileExit == false) {
                            //mantain file input focus
                            $('#files').focus();
                        }
                    });
                },
            }, {
                field: 'Enable',
                title: 'Habilitado',
                width: "100px",
            }],
        });
    },

    RenderBranch: function () {
        $('#' + Provider_CompanyContactObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' }
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "ContactId",
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            BR_Representative: { editable: true },
                            BR_RepresentativeId: { editable: false },

                            BR_Address: { editable: true, validation: { required: true } },
                            BR_AddressId: { editable: false },

                            BR_City: { editable: false },
                            BR_CityId: { editable: false },
                            BR_CityName: { editable: true, validation: { required: true } },

                            BR_Phone: { editable: true, validation: { required: false } },
                            BR_PhoneId: { editable: false },

                            BR_Fax: { editable: true },
                            BR_FaxId: { editable: false, validation: { required: true } },

                            BR_Email: { editable: true, validation: { required: false, email: true } },
                            BR_EmailId: { editable: false },

                            BR_Website: { editable: true, validation: { required: false } },
                            BR_WebsiteId: { editable: false },

                            BR_Latitude: { editable: true, validation: { required: false } },
                            BR_LatitudeId: { editable: false },

                            BR_Longitude: { editable: true, validation: { required: false } },
                            BR_LongitudeId: { editable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactGetByType=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                field: 'ContactId',
                title: 'Id',
                width: "50px",
            }, {
                field: 'ContactName',
                title: 'Nombre',
                width: "200px",
            }, {
                field: 'BR_Representative',
                title: 'Representante',
                width: "200px",
            }, {
                field: 'BR_Address',
                title: 'Dirección',
                width: "200px",
            }, {
                field: 'BR_CityName',
                title: 'Ciudad',
                width: "350px",
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.BR_CityName != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.BR_CityName;
                    }
                    return oReturn;
                },
                editor: function (container, options) {

                    // create an input element
                    var input = $("<input/>");
                    // set its name to the field to which the column is bound ('name' in this case)
                    input.attr("value", options.model[options.field]);
                    // append it to the container
                    input.appendTo(container);
                    // initialize a Kendo UI AutoComplete
                    input.kendoAutoComplete({
                        dataTextField: "ItemName",
                        select: function (e) {
                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name
                            options.model[options.field] = selectedItem.ItemName;
                            options.model['BR_City'] = selectedItem.ItemId;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: "json",
                            serverFiltering: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByGeography=true&SearchParam=' + options.data.filter.filters[0].value + '&CityId=',
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
                field: 'BR_Phone',
                title: 'Teléfono',
                width: "200px",
            }, {
                field: 'BR_Fax',
                title: 'Fax',
                width: "200px",
            }, {
                field: 'BR_Email',
                title: 'Correo electrónico',
                width: "200px",
            }, {
                field: 'BR_Website',
                title: 'Página web',
                width: "200px",
            }, {
                field: 'BR_Latitude',
                title: 'Latitud',
                width: "200px",
            }, {
                field: 'BR_Longitude',
                title: 'Longitud',
                width: "200px",
            }, {
                field: 'Enable',
                title: 'Habilitado',
                width: "100px",
            }],
        });
    },

    RenderDistributor: function () {
        $('#' + Provider_CompanyContactObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' }
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "ContactId",
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            DT_DistributorType: { editable: true },
                            DT_DistributorTypeId: { editable: false },

                            DT_Representative: { editable: true },
                            DT_RepresentativeId: { editable: false },

                            DT_Email: { editable: true, validation: { required: false, email: true } },
                            DT_EmailId: { editable: false },

                            DT_Phone: { editable: true, validation: { required: false } },
                            DT_PhoneId: { editable: false },

                            DT_City: { editable: false },
                            DT_CityId: { editable: false },
                            DT_CityName: { editable: true, validation: { required: true } },

                            DT_DateIssue: { editable: true, validation: { required: true } },
                            DT_DateIssueId: { editable: false },

                            DT_DueDate: { editable: true },
                            DT_DueDateId: { editable: false, validation: { required: true } },

                            DT_DistributorFile: { editable: true },
                            DT_DistributorFileId: { editable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactGetByType=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                field: 'ContactId',
                title: 'Id',
                width: "50px",
            }, {
                field: 'ContactName',
                title: 'Razón social',
                width: "200px",
            }, {
                field: 'DT_DistributorType',
                title: 'Tipo de distribuidor',
                width: "200px",
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.DT_DistributorType != null) {
                        $.each(Provider_CompanyContactObject.ProviderOptions[211], function (item, value) {
                            if (dataItem.DT_DistributorType == value.ItemId) {
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
                            dataSource: Provider_CompanyContactObject.ProviderOptions[211],
                            dataTextField: "ItemName",
                            dataValueField: "ItemId",
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'DT_Representative',
                title: 'Representante comercial',
                width: "200px",
            }, {
                field: 'DT_Email',
                title: 'Correo electronico',
                width: "200px",
            }, {
                field: 'DT_Phone',
                title: 'Telefono',
                width: "200px",
            }, {
                field: 'DT_CityName',
                title: 'Ciudad',
                width: "350px",
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.DT_CityName != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.DT_CityName;
                    }
                    return oReturn;
                },
                editor: function (container, options) {

                    // create an input element
                    var input = $("<input/>");
                    // set its name to the field to which the column is bound ('name' in this case)
                    input.attr("value", options.model[options.field]);
                    // append it to the container
                    input.appendTo(container);
                    // initialize a Kendo UI AutoComplete
                    input.kendoAutoComplete({
                        dataTextField: "ItemName",
                        select: function (e) {
                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name
                            options.model[options.field] = selectedItem.ItemName;
                            options.model['DT_City'] = selectedItem.ItemId;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: "json",
                            serverFiltering: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByGeography=true&SearchParam=' + options.data.filter.filters[0].value + '&CityId=',
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
                field: 'DT_DateIssue',
                title: 'Fecha de expedición',
                width: '200px',
                format: Provider_CompanyContactObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'DT_DueDate',
                title: 'Fecha de vencimiento',
                width: "200px",
                format: Provider_CompanyContactObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'DT_DistributorFile',
                title: 'Doc soporte.',
                width: "400px",
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.DT_DistributorFile != null && dataItem.DT_DistributorFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyContactObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyContactObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${DT_DistributorFile}/gi, dataItem.DT_DistributorFile);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyContactObject.ProviderPublicId,
                            autoUpload: true
                        },
                        success: function (e) {
                            if (e.response != null && e.response.length > 0) {
                                //set server fiel name
                                options.model[options.field] = e.response[0].ServerName;
                                //enable made changes
                                options.model.dirty = true;
                            }
                        },
                        complete: function (e) {
                            //enable lost focus
                            oFileExit = true;
                        },
                        select: function (e) {
                            //disable lost focus while upload file
                            oFileExit = false;
                        },
                    });
                    $(container).focusout(function () {
                        if (oFileExit == false) {
                            //mantain file input focus
                            $('#files').focus();
                        }
                    });
                },
            }],
        });
    },
};

/*CompanyComercialObject*/
var Provider_CompanyComercialObject = {

    ObjectId: '',
    ProviderPublicId: '',
    ComercialType: '',
    DateFormat: '',
    ProviderOptions: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.ProviderPublicId = vInitObject.ProviderPublicId;
        this.ContactType = vInitObject.ContactType;
        this.DateFormat = vInitObject.DateFormat;
        if (vInitObject.ProviderOptions != null) {
            $.each(vInitObject.ProviderOptions, function (item, value) {
                Provider_CompanyComercialObject.ProviderOptions[value.Key] = value.Value;
            });
        }
    },

    RenderAsync: function () {
        if (Provider_CompanyComercialObject.ComercialType == 301001) {
            Provider_CompanyComercialObject.RenderExperience();
        }
    },

    RenderExperience: function () {
        //$('#' + Provider_CompanyComercialObject.ObjectId).kendoGrid({
        //    editable: true,
        //    navigatable: true,
        //    pageable: false,
        //    scrollable: true,
        //    toolbar: [
        //        { name: 'create', text: 'Nuevo' },
        //        { name: 'save', text: 'Guardar' },
        //        { name: 'cancel', text: 'Descartar' }
        //    ],
        //    dataSource: {
        //        schema: {
        //            model: {
        //                id: "ContactId",
        //                fields: {
        //                    ContactId: { editable: false, nullable: true },
        //                    ContactName: { editable: true, validation: { required: true } },
        //                    Enable: { editable: true, type: "boolean", defaultValue: true },

        //                    CC_CompanyContactType: { editable: true },
        //                    CC_CompanyContactTypeId: { editable: false },

        //                    CC_Value: { editable: true },
        //                    CC_ValueId: { editable: false },
        //                }
        //            }
        //        },
        //        transport: {
        //            read: function (options) {
        //                $.ajax({
        //                    url: BaseUrl.ApiUrl + '/ProviderApi?GIContactGetByType=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
        //                    dataType: 'json',
        //                    success: function (result) {
        //                        options.success(result);
        //                    },
        //                    error: function (result) {
        //                        options.error(result);
        //                    }
        //                });
        //            },
        //            create: function (options) {
        //                $.ajax({
        //                    url: BaseUrl.ApiUrl + '/ProviderApi?GIContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
        //                    dataType: 'json',
        //                    type: 'post',
        //                    data: {
        //                        DataToUpsert: kendo.stringify(options.data)
        //                    },
        //                    success: function (result) {
        //                        options.success(result);
        //                    },
        //                    error: function (result) {
        //                        options.error(result);
        //                    }
        //                });
        //            },
        //            update: function (options) {
        //                $.ajax({
        //                    url: BaseUrl.ApiUrl + '/ProviderApi?GIContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
        //                    dataType: 'json',
        //                    type: 'post',
        //                    data: {
        //                        DataToUpsert: kendo.stringify(options.data)
        //                    },
        //                    success: function (result) {
        //                        options.success(result);
        //                    },
        //                    error: function (result) {
        //                        options.error(result);
        //                    }
        //                });
        //            },
        //        },
        //    },
        //    columns: [{
        //        field: 'ContactId',
        //        title: 'Id',
        //        width: "50px",
        //    }, {
        //        field: 'ContactName',
        //        title: 'Nombre',
        //    }, {
        //        field: 'CC_CompanyContactType',
        //        title: 'Tipo de contacto',
        //        template: function (dataItem) {
        //            var oReturn = 'Seleccione una opción.';
        //            if (dataItem != null && dataItem.CC_CompanyContactType != null) {
        //                $.each(Provider_CompanyContactObject.ProviderOptions[209], function (item, value) {
        //                    if (dataItem.CC_CompanyContactType == value.ItemId) {
        //                        oReturn = value.ItemName;
        //                    }
        //                });
        //            }
        //            return oReturn;
        //        },
        //        editor: function (container, options) {
        //            $('<input required data-bind="value:' + options.field + '"/>')
        //                .appendTo(container)
        //                .kendoDropDownList({
        //                    dataSource: Provider_CompanyContactObject.ProviderOptions[209],
        //                    dataTextField: 'ItemName',
        //                    dataValueField: 'ItemId',
        //                    optionLabel: 'Seleccione una opción'
        //                });
        //        },
        //    }, {
        //        field: 'CC_Value',
        //        title: 'Valor',
        //    }, {
        //        field: 'Enable',
        //        title: 'Habilitado',
        //        width: "100px",
        //    }],
        //});
    },
};


/*CompanyCertificationObject*/
var Provider_CompanyCertificationObject = {

    ObjectId: '',
    ProviderPublicId: '',
    CertificationType: '',
    DateFormat: '',
    CertificationOptionList: new Array(),

    Init: function (vInitiObject) {
        this.ObjectId = vInitiObject.ObjectId;
        this.ProviderPublicId = vInitiObject.ProviderPublicId;
        this.CertificationType = vInitiObject.CertificationType;
        this.DateFormat = vInitiObject.DateFormat;
        $.each(vInitiObject.CertificationOptionList, function (item, value) {
            Provider_CompanyCertificationObject.CertificationOptionList[value.Key] = value.Value;
        });
    },

    RenderAsync: function () {
        if (Provider_CompanyCertificationObject.CertificationType == 701001) {
            Provider_CompanyCertificationObject.RenderCompanyCertification();
        }
        else if (Provider_CompanyCertificationObject.CertificationType == 701002) {
            Provider_CompanyCertificationObject.RenderCompanyHealthyPolitics();
        }
        else if (Provider_CompanyCertificationObject.CertificationType == 701003) {
            Provider_CompanyCertificationObject.RenderCompanyRiskPolicies();
        }
    },

    RenderCompanyCertification: function () {
        $('#' + Provider_CompanyCertificationObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' }
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "CertificationId",
                        fields: {
                            CertificationId: { editable: false, nullable: true },
                            CertificationName: { editable: true },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            C_CertificationCompany: { editable: true },
                            C_CertificationCompanyId: { editable: false },
                            C_CertificationCompanyName: { editable: true },

                            C_Rule: { editable: true },
                            C_RuleId: { editable: false },
                            C_RuleName: { editable: true },

                            C_StartDateCertification: { editable: true },
                            C_StartDateCertificationId: { editable: false },

                            C_EndDateCertification: { editable: true },
                            C_EndDateCertificationId: { editable: false },

                            C_CCS: { editable: true },
                            C_CCSId: { editable: false },

                            C_CertificationFile: { editable: true },
                            C_CertificationFileId: { editable: false },

                            C_Scope: { editable: true },
                            C_ScopeId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?HICertificationGetByType=true&ProviderPublicId=' + Provider_CompanyCertificationObject.ProviderPublicId + '&CertificationType=' + Provider_CompanyCertificationObject.CertificationType,
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                            },
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?HICertificationUpsert=true&ProviderPublicId=' + Provider_CompanyCertificationObject.ProviderPublicId + '&CertificationType=' + Provider_CompanyCertificationObject.CertificationType,
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
                            },
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?HICertificationUpsert=true&ProviderPublicId=' + Provider_CompanyCertificationObject.ProviderPublicId + '&CertificationType=' + Provider_CompanyCertificationObject.CertificationType,
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
                            },
                        });
                    },
                },
            },
            columns: [{
                field: 'CertificationId',
                title: 'Id',
                width: '50px',
            }, {
                field: 'CertificationName',
                title: 'Nombre',
            }, {
                field: 'C_CertificationCompanyName',
                title: 'Empresa Certificadora',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.C_CertificationCompanyName != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.C_CertificationCompanyName;
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    // create an input element
                    var input = $("<input/>");
                    // set its name to the field to which the column is bound ('name' in this case)
                    input.attr("value", options.model[options.field]);
                    // append it to the container
                    input.appendTo(container);
                    // initialize a Kendo UI AutoComplete
                    input.kendoAutoComplete({
                        dataTextField: "ItemName",
                        select: function (e) {
                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name
                            options.model[options.field] = selectedItem.ItemName;
                            options.model['C_CertificationCompany'] = selectedItem.ItemId;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: "json",
                            serverFiltering: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByCompanyRule=true&SearchParam=' + options.data.filter.filters[0].value,
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
                field: 'C_RuleName',
                title: 'Norma',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.C_RuleName != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.C_RuleName;
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    // create an input element
                    var input = $("<input/>");
                    // set its name to the field to which the column is bound ('name' in this case)
                    input.attr("value", options.model[options.field]);
                    // append it to the container
                    input.appendTo(container);
                    // initialize a Kendo UI AutoComplete
                    input.kendoAutoComplete({
                        dataTextField: "ItemName",
                        select: function (e) {
                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name
                            options.model[options.field] = selectedItem.ItemName;
                            options.model['C_Rule'] = selectedItem.ItemId;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: "json",
                            serverFiltering: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByRule=true&SearchParam=' + options.data.filter.filters[0].value,
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
                field: 'C_StartDateCertification',
                title: 'Fecha Certificación',
                width: '100px',
                format: Provider_CompanyCertificationObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'C_EndDateCertification',
                title: 'Fecha Caducidad',
                width: '100px',
                format: Provider_CompanyCertificationObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'C_CCS',
                title: '% CCS',
                width: '60px',
            }, {
                field: 'C_CertificationFile',
                title: 'Archivo Certificación',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.C_CertificationFile != null && dataItem.C_CertificationFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyCertificationObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyCertificationObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${C_CertificationFile}/gi, dataItem.C_CertificationFile);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyCertificationObject.ProviderPublicId,
                            autoUpload: true
                        },
                        success: function (e) {
                            if (e.response != null && e.response.length > 0) {
                                //set server fiel name
                                options.model[options.field] = e.response[0].ServerName;
                                //enable made changes
                                options.model.dirty = true;
                            }
                        },
                        complete: function (e) {
                            //enable lost focus
                            oFileExit = true;
                        },
                        select: function (e) {
                            //disable lost focus while upload file
                            oFileExit = false;
                        },
                    });
                    $(container).focusout(function () {
                        if (oFileExit == false) {
                            //mantain file input focus
                            $('#files').focus();
                        }
                    });
                },
            }, {
                field: 'C_Scope',
                title: 'Alcance'
            }, {
                field: 'Enable',
                title: 'Habilitado'
            }],
        });
    },

    RenderCompanyHealthyPolitics: function () {

    },

    RenderCompanyRiskPolicies: function () {

    },
};

var Provider_LegalInfoObject = {

    AutoCompleteId: '',
    ControlToRetornACId: '',
    ObjectId: '',
    ProviderPublicId: '',
    LegalInfoType: '',
    ChaimberOfComerceOptionList: new Array(),

    Init: function (vInitiObject) {
        debugger;
        this.AutoCompleteId = vInitiObject.AutoCompleteId;
        this.ControlToRetornACId = vInitiObject.ControlToRetornACId;
        this.ObjectId = vInitiObject.ObjectId;
        this.ProviderPublicId = vInitiObject.ProviderPublicId;
        this.LegalInfoType = vInitiObject.LegalInfoType;
        this.ChaimberOfComerceOptionList = vInitiObject.ChaimberOfComerceOptionList;

        //Load AutoComplete 
        Provider_LegalInfoObject.AutoComplete(vInitiObject.AutoCompleteId, vInitiObject.ControlToRetornACId);

        $.each(vInitiObject.ChaimberOfComerceOptionList, function (item, value) {
            Provider_LegalInfoObject.ChaimberOfComerceOptionList[value.Key] = value.Value;
        });
    },

    RenderAsync: function () {
        if (Provider_LegalInfoObject.LegalInfoType == 601001) {
            Provider_LegalInfoObject.RenderChaimberOfComerce();
        }
        else if (Provider_LegalInfoObject.LegalInfoType == 601002) {
            Provider_LegalInfoObject.RenderUniqueRegister();
        }
        else if (Provider_LegalInfoObject.LegalInfoType == 601003) {
            Provider_LegalInfoObject.RenderCIFIN();
        }
        else if (Provider_LegalInfoObject.LegalInfoType == 601004) {
            Provider_LegalInfoObject.RenderSARLAFT();
        }
        else if (Provider_LegalInfoObject.LegalInfoType == 601005) {
            Provider_LegalInfoObject.RenderResolutions();
        }
    },

    RenderChaimberOfComerce: function () {
        $('#' + Provider_LegalInfoObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar cambios' },
                { name: 'cancel', text: 'Descartar cambios' }
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "LegalId",
                        fields: {
                            LegalId: { editable: false, nullable: true },
                            LegalName: { editable: true },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            CP_PartnerName: { editable: true },
                            CP_PartnerNameId: { editable: false },

                            CP_PartnerIdentificationNumber: { editable: true },
                            CP_PartnerIdentificationNumberId: { editable: false },

                            CP_PartnerRank: { editable: true },
                            CP_PartnerRankId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoGetByType=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType,
                            dataType: 'json',
                            success: function (result) {
                                debugger;
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                            },
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                debugger;
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                            },
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '',
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
                            },
                        });
                    },
                },
            },
            columns: [
                {
                    field: 'LegalId',
                    title: 'Id',
                }, {
                    field: 'CP_PartnerName',
                    title: 'Nombre',
                }, {
                    field: 'CP_PartnerIdentificationNumber',
                    title: 'Número de Identificación',
                }, {
                    field: 'CP_PartnerRank',
                    title: 'Cargo',
                }],
        });
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
                            url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByGeography=true&SearchParam=' + options.data.filter.filters[0].value + '&CityId=',
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

}

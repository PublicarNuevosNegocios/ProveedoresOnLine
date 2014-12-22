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
                        id: 'ContactId',
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: 'boolean', defaultValue: true },

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
                width: '50px',
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
                width: '100px',
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
                        id: 'ContactId',
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: 'boolean', defaultValue: true },

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
                width: '50px',
            }, {
                field: 'ContactName',
                title: 'Nombre',
                width: '200px',
            }, {
                field: 'CP_PersonContactType',
                title: 'Tipo de representante',
                width: '200px',
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
                width: '200px',
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
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'CP_IdentificationNumber',
                title: 'Número de identificación',
                width: '200px',
            }, {
                field: 'CP_IdentificationCity',
                title: 'Ciudad de expedicion del documento',
                width: '200px',
            }, {
                field: 'CP_Phone',
                title: 'Telefono',
                width: '200px',
            }, {
                field: 'CP_Email',
                title: 'Correo electronico',
                width: '200px',
            }, {
                field: 'CP_Negotiation',
                title: 'Capacidad de negociación',
                width: '200px',
            }, {
                field: 'CP_IdentificationFile',
                title: 'Doc representante legal.',
                width: '400px',
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
                width: '100px',
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
                        id: 'ContactId',
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: 'boolean', defaultValue: true },

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
                width: '50px',
            }, {
                field: 'ContactName',
                title: 'Nombre',
                width: '200px',
            }, {
                field: 'BR_Representative',
                title: 'Representante',
                width: '200px',
            }, {
                field: 'BR_Address',
                title: 'Dirección',
                width: '200px',
            }, {
                field: 'BR_CityName',
                title: 'Ciudad',
                width: '350px',
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
                    var input = $('<input/>');
                    // set its name to the field to which the column is bound ('name' in this case)
                    input.attr('value', options.model[options.field]);
                    // append it to the container
                    input.appendTo(container);
                    // initialize a Kendo UI AutoComplete
                    input.kendoAutoComplete({
                        dataTextField: 'ItemName',
                        select: function (e) {
                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name
                            options.model[options.field] = selectedItem.ItemName;
                            options.model['BR_City'] = selectedItem.ItemId;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: 'json',
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
                width: '200px',
            }, {
                field: 'BR_Fax',
                title: 'Fax',
                width: '200px',
            }, {
                field: 'BR_Email',
                title: 'Correo electrónico',
                width: '200px',
            }, {
                field: 'BR_Website',
                title: 'Página web',
                width: '200px',
            }, {
                field: 'BR_Latitude',
                title: 'Latitud',
                width: '200px',
            }, {
                field: 'BR_Longitude',
                title: 'Longitud',
                width: '200px',
            }, {
                field: 'Enable',
                title: 'Habilitado',
                width: '100px',
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
                        id: 'ContactId',
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: 'boolean', defaultValue: true },

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
                width: '50px',
            }, {
                field: 'ContactName',
                title: 'Razón social',
                width: '200px',
            }, {
                field: 'DT_DistributorType',
                title: 'Tipo de distribuidor',
                width: '200px',
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
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'DT_Representative',
                title: 'Representante comercial',
                width: '200px',
            }, {
                field: 'DT_Email',
                title: 'Correo electronico',
                width: '200px',
            }, {
                field: 'DT_Phone',
                title: 'Telefono',
                width: '200px',
            }, {
                field: 'DT_CityName',
                title: 'Ciudad',
                width: '350px',
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
                    var input = $('<input/>');
                    // set its name to the field to which the column is bound ('name' in this case)
                    input.attr('value', options.model[options.field]);
                    // append it to the container
                    input.appendTo(container);
                    // initialize a Kendo UI AutoComplete
                    input.kendoAutoComplete({
                        dataTextField: 'ItemName',
                        select: function (e) {
                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name
                            options.model[options.field] = selectedItem.ItemName;
                            options.model['DT_City'] = selectedItem.ItemId;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: 'json',
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
                width: '200px',
                format: Provider_CompanyContactObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'DT_DistributorFile',
                title: 'Doc soporte.',
                width: '400px',
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
            }, {
                field: 'Enable',
                title: 'Habilitado',
                width: '100px',
            }],
        });
    },
};

/*CompanyCommercialObject*/
var Provider_CompanyCommercialObject = {

    ObjectId: '',
    ProviderPublicId: '',
    CommercialType: '',
    DateFormat: '',
    ProviderOptions: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.ProviderPublicId = vInitObject.ProviderPublicId;
        this.CommercialType = vInitObject.CommercialType;
        this.DateFormat = vInitObject.DateFormat;
        if (vInitObject.ProviderOptions != null) {
            $.each(vInitObject.ProviderOptions, function (item, value) {
                Provider_CompanyCommercialObject.ProviderOptions[value.Key] = value.Value;
            });
        }
    },

    RenderAsync: function () {
        if (Provider_CompanyCommercialObject.CommercialType == 301001) {
            Provider_CompanyCommercialObject.RenderExperience();
        }
    },

    RenderExperience: function () {
        $('#' + Provider_CompanyCommercialObject.ObjectId).kendoGrid({
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
                        id: 'CommercialId',
                        fields: {
                            CommercialId: { editable: false, nullable: true },
                            CommercialName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: 'boolean', defaultValue: true },

                            EX_ContractType: { editable: true },
                            EX_ContractTypeId: { editable: false },

                            EX_Currency: { editable: true },
                            EX_CurrencyId: { editable: false },

                            EX_DateIssue: { editable: true },
                            EX_DateIssueId: { editable: false },

                            EX_DueDate: { editable: true },
                            EX_DueDateId: { editable: false },

                            EX_Client: { editable: true },
                            EX_ClientId: { editable: false },

                            EX_ContractNumber: { editable: true },
                            EX_ContractNumberId: { editable: false },

                            EX_ContractValue: { editable: true },
                            EX_ContractValueId: { editable: false },

                            EX_Phone: { editable: true },
                            EX_PhoneId: { editable: false },

                            EX_BuiltArea: { editable: true },
                            EX_BuiltAreaId: { editable: false },

                            EX_BuiltUnit: { editable: true },
                            EX_BuiltUnitId: { editable: false },

                            EX_ExperienceFile: { editable: true },
                            EX_ExperienceFileId: { editable: false },

                            EX_ContractSubject: { editable: true },
                            EX_ContractSubjectId: { editable: false },

                            EX_EconomicActivity: { editable: true },

                            EX_CustomEconomicActivity: { editable: true },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?CICommercialGetByType=true&ProviderPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId + '&CommercialType=' + Provider_CompanyCommercialObject.CommercialType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?CICommercialUpsert=true&ProviderPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId + '&CommercialType=' + Provider_CompanyCommercialObject.CommercialType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?CICommercialUpsert=true&ProviderPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId + '&CommercialType=' + Provider_CompanyCommercialObject.CommercialType,
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
                field: 'CommercialId',
                title: 'Id',
                width: '50px',
            }, {
                field: 'CommercialName',
                title: 'Nombre',
                width: '200px',
            }, {
                field: 'EX_ContractType',
                title: 'Tipo de contrato',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.EX_ContractType != null) {
                        $.each(Provider_CompanyCommercialObject.ProviderOptions[303], function (item, value) {
                            if (dataItem.EX_ContractType == value.ItemId) {
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
                            dataSource: Provider_CompanyCommercialObject.ProviderOptions[303],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'EX_DateIssue',
                title: 'Inicio',
                width: '200px',
                format: Provider_CompanyCommercialObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'EX_DueDate',
                title: 'Fin',
                width: '200px',
                format: Provider_CompanyCommercialObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'EX_Client',
                title: 'Cliente',
                width: '200px',
            }, {
                field: 'EX_ContractNumber',
                title: 'Número de contrato',
                width: '200px',
            }, {
                field: 'EX_ContractValue',
                title: 'Valor de contrato',
                width: '400px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.EX_Currency != null && dataItem.EX_ContractValue != null) {

                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }

                        oReturn = oReturn + dataItem.EX_ContractValue + ' ';
                        $.each(Provider_CompanyCommercialObject.ProviderOptions[108], function (item, value) {
                            if (dataItem.EX_Currency == value.ItemId) {
                                oReturn = oReturn + value.ItemName;
                            }
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    $('<input style="width:45%;" required data-bind="value:' + options.field + '"/><span>&nbsp;</span>')
                        .appendTo(container);
                    $('<input style="width:45%;" required data-bind="value:EX_Currency"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            dataSource: Provider_CompanyCommercialObject.ProviderOptions[108],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción',
                        });
                },
            }, {
                field: 'EX_Phone',
                title: 'Telefono',
                width: '200px',
            }, {
                field: 'EX_BuiltArea',
                title: 'Area contruida (m2)',
                width: '200px',
            }, {
                field: 'EX_BuiltUnit',
                title: 'Unidades construidas',
                width: '200px',
            }, {
                field: 'EX_ContractSubject',
                title: 'Objeto del contrato',
                width: '400px',
                editor: function (container, options) {
                    $('<textarea data-bind="value: ' + options.field + '"></textarea>')
                        .appendTo(container);
                },
            }, {
                field: 'EX_EconomicActivity',
                title: 'Actividad economica',
                width: '400px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.EX_EconomicActivity != null && dataItem.EX_EconomicActivity.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        $.each(dataItem.EX_EconomicActivity, function (item, value) {
                            oReturn = oReturn + value.ActivityName + ',';
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    $('<select multiple="multiple" data-bind="value:' + options.field + '" />')
                        .appendTo(container)
                        .kendoMultiSelect({
                            minLength: 2,
                            autoBind: false,
                            dataTextField: 'ActivityName',
                            //value: options.model[options.field],
                            itemTemplate: $('#' + Provider_CompanyCommercialObject.ObjectId + '_MultiAC_ItemTemplate').html(),
                            dataSource: {
                                type: "json",
                                serverFiltering: true,
                                schema: {
                                    model: {
                                        id: 'EconomicActivityId',
                                        fields: {
                                            EconomicActivityId: { type: 'string', nullable: false },
                                            ActivityName: { type: 'string', nullable: false },
                                            ActivityType: { type: 'string', nullable: true },
                                            ActivityGroup: { type: 'string', nullable: true },
                                            ActivityCategory: { type: 'string', nullable: true },
                                        }
                                    }
                                },
                                transport: {
                                    read: function (options) {
                                        if (options.data != null && options.data.filter != null && options.data.filter.filters != null && options.data.filter.filters.length > 0 && options.data.filter.filters[0].value != null && options.data.filter.filters[0].value.length > 0) {
                                            $.ajax({
                                                url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByActivity=true&IsDefault=true&SearchParam=' + options.data.filter.filters[0].value,
                                                dataType: 'json',
                                                success: function (result) {
                                                    options.success(result);
                                                },
                                                error: function (result) {
                                                    options.error(result);
                                                }
                                            });
                                        }
                                        else {
                                            options.success([]);
                                        }
                                    },
                                },
                            },
                        });
                },
            }, {
                field: 'EX_CustomEconomicActivity',
                title: 'Actividad economica personalizada',
                width: '400px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.EX_CustomEconomicActivity != null && dataItem.EX_CustomEconomicActivity.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        $.each(dataItem.EX_CustomEconomicActivity, function (item, value) {
                            oReturn = oReturn + value.ActivityName + ',';
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    $('<select multiple="multiple" data-bind="value:' + options.field + '" />')
                        .appendTo(container)
                        .kendoMultiSelect({
                            minLength: 2,
                            autoBind: false,
                            dataTextField: 'ActivityName',
                            //value: options.model[options.field],
                            itemTemplate: $('#' + Provider_CompanyCommercialObject.ObjectId + '_MultiAC_ItemTemplate').html(),
                            dataSource: {
                                type: "json",
                                serverFiltering: true,
                                schema: {
                                    model: {
                                        id: 'EconomicActivityId',
                                        fields: {
                                            EconomicActivityId: { type: 'string', nullable: false },
                                            ActivityName: { type: 'string', nullable: false },
                                            ActivityType: { type: 'string', nullable: true },
                                            ActivityGroup: { type: 'string', nullable: true },
                                            ActivityCategory: { type: 'string', nullable: true },
                                        }
                                    }
                                },
                                transport: {
                                    read: function (options) {
                                        if (options.data != null && options.data.filter != null && options.data.filter.filters != null && options.data.filter.filters.length > 0 && options.data.filter.filters[0].value != null && options.data.filter.filters[0].value.length > 0) {
                                            $.ajax({
                                                url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByActivity=true&IsDefault=false&SearchParam=' + options.data.filter.filters[0].value,
                                                dataType: 'json',
                                                success: function (result) {
                                                    options.success(result);
                                                },
                                                error: function (result) {
                                                    options.error(result);
                                                }
                                            });
                                        }
                                        else {
                                            options.success([]);
                                        }
                                    },
                                },
                            },
                        });
                },
            }, {
                field: 'EX_ExperienceFile',
                title: 'Doc soporte.',
                width: '400px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.EX_ExperienceFile != null && dataItem.EX_ExperienceFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyCommercialObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyCommercialObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${EX_ExperienceFile}/gi, dataItem.EX_ExperienceFile);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId,
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
                width: '100px',
            }],
        });
    },
};

/*CompanyHSEQObject*/
var Provider_CompanyHSEQObject = {

    ObjectId: '',
    ProviderPublicId: '',
    AutoCompleteId: '',
    ControlToReturnACId: '',
    HSEQType: '',
    DateFormat: '',
    HSEQOptionList: new Array(),

    Init: function (vInitiObject) {
        this.ObjectId = vInitiObject.ObjectId;
        this.ProviderPublicId = vInitiObject.ProviderPublicId;
        this.AutoCompleteId = vInitiObject.AutoCompleteId;
        this.ControlToReturnACId = vInitiObject.ControlToRetornACId;
        this.HSEQType = vInitiObject.HSEQType;
        this.DateFormat = vInitiObject.DateFormat;
        Provider_CompanyHSEQObject.AutoComplete(vInitiObject.AutoCompleteId, vInitiObject.ControlToRetornACId);
        $.each(vInitiObject.HSEQOptionList, function (item, value) {
            Provider_CompanyHSEQObject.HSEQOptionList[value.Key] = value.Value;
        });
    },

    RenderAsync: function () {
        if (Provider_CompanyHSEQObject.HSEQType == 701001) {
            Provider_CompanyHSEQObject.RenderCompanyCertification();
        }
        else if (Provider_CompanyHSEQObject.HSEQType == 701002) {
            Provider_CompanyHSEQObject.RenderCompanyHealthyPolitics();
        }
        else if (Provider_CompanyHSEQObject.HSEQType == 701003) {
            Provider_CompanyHSEQObject.RenderCompanyRiskPolicies();
        }
    },

    RenderCompanyCertification: function () {
        $('#' + Provider_CompanyHSEQObject.ObjectId).kendoGrid({
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
                            CertificationName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            C_CertificationCompany: { editable: true },
                            C_CertificationCompanyId: { editable: false },
                            C_CertificationCompanyName: { editable: true, validation: { required: true } },

                            C_Rule: { editable: true },
                            C_RuleId: { editable: false },
                            C_RuleName: { editable: true, validation: { required: true } },

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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQGetByType=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQUpsert=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQUpsert=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
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
                format: Provider_CompanyHSEQObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'C_EndDateCertification',
                title: 'Fecha Caducidad',
                width: '100px',
                format: Provider_CompanyHSEQObject.DateFormat,
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
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
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
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId,
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
        $('#' + Provider_CompanyHSEQObject.ObjectId).kendoGrid({
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

                            CH_Year: { editable: true, validation: { required: true } },
                            CH_YearId: { editable: false },

                            CH_PoliticsSecurity: { editable: true },
                            CH_PoliticsSecurityId: { editable: false },

                            CH_PoliticsNoAlcohol: { editable: true },
                            CH_PoliticsNoAlcoholId: { editable: false },

                            CH_ProgramOccupationalHealth: { editable: true },
                            CH_ProgramOccupationalHealthId: { editable: false },

                            CH_RuleIndustrialSecurity: { editable: true },
                            CH_RuleIndustrialSecurityId: { editable: false },

                            CH_MatrixRiskControl: { editable: true },
                            CH_MatrixRiskControlId: { editable: false },

                            CH_CorporateSocialResponsability: { editable: true },
                            CH_CorporateSocialResponsabilityId: { editable: false },

                            CH_ProgramEnterpriseSecurity: { editable: true },
                            CH_ProgramEnterpriseSecurityId: { editable: false },

                            CH_PoliticsRecruiment: { editable: true },
                            CH_PoliticsRecruimentId: { editable: false },

                            CH_CertificationsForm: { editable: true },
                            CH_CertificationsFormId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQGetByType=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQUpsert=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQUpsert=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
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
                field: 'CH_Year',
                title: 'Año',
            }, {
                field: 'CH_PoliticsSecurity',
                title: 'Política de Seguridad, Salud y Ambiente',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CH_PoliticsSecurity != null && dataItem.CH_PoliticsSecurity.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${FileUrl}/gi, dataItem.CH_PoliticsSecurity);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId,
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
                field: 'CH_PoliticsNoAlcohol',
                title: 'Política de no alcohol, Drogas y Fumadores',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CH_PoliticsNoAlcohol != null && dataItem.CH_PoliticsNoAlcohol.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${FileUrl}/gi, dataItem.CH_PoliticsNoAlcohol);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId,
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
                field: 'CH_ProgramOccupationalHealth',
                title: 'Programa de Salud Ocupacional',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CH_ProgramOccupationalHealth != null && dataItem.CH_ProgramOccupationalHealth.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${FileUrl}/gi, dataItem.CH_ProgramOccupationalHealth);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId,
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
                field: 'CH_RuleIndustrialSecurity',
                title: 'Reglamento de Higiene y Seguridad Industrial',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CH_RuleIndustrialSecurity != null && dataItem.CH_RuleIndustrialSecurity.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${FileUrl}/gi, dataItem.CH_RuleIndustrialSecurity);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId,
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
                field: 'CH_MatrixRiskControl',
                title: 'Matriz de Identificación de Peligros, Evaluación y Control de Riesgos',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CH_MatrixRiskControl != null && dataItem.CH_MatrixRiskControl.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${FileUrl}/gi, dataItem.CH_MatrixRiskControl);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId,
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
                field: 'CH_CorporateSocialResponsability',
                title: 'Responsabilidad Social Empresarial',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CH_CorporateSocialResponsability != null && dataItem.CH_CorporateSocialResponsability.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${FileUrl}/gi, dataItem.CH_CorporateSocialResponsability);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId,
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
                field: 'CH_ProgramEnterpriseSecurity',
                title: 'Programa de Seguridad Empresarial y Logística',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CH_ProgramEnterpriseSecurity != null && dataItem.CH_ProgramEnterpriseSecurity.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${FileUrl}/gi, dataItem.CH_ProgramEnterpriseSecurity);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId,
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
                field: 'CH_PoliticsRecruiment',
                title: 'Política de Contratación de Personal',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CH_PoliticsRecruiment != null && dataItem.CH_PoliticsRecruiment.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${FileUrl}/gi, dataItem.CH_PoliticsRecruiment);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId,
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
                field: 'CH_CertificationsForm',
                title: 'Formulario Certificaciones',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CH_CertificationsForm != null && dataItem.CH_CertificationsForm.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${FileUrl}/gi, dataItem.CH_CertificationsForm);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId,
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

    RenderCompanyRiskPolicies: function () {
        $('#' + Provider_CompanyHSEQObject.ObjectId).kendoGrid({
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

                            CR_Year: { editable: true, validation: { required: true } },
                            CR_YearId: { editable: false },

                            CR_ManHoursWorked: { editable: true },
                            CR_ManHoursWorkedId: { editable: false },

                            CR_Fatalities: { editable: true },
                            CR_FatalitiesId: { editable: false },

                            CR_NumberAccident: { editable: true },
                            CR_NumberAccidentId: { editable: false },

                            CR_NumberAccidentDisabling: { editable: true },
                            CR_NumberAccidentDisablingId: { editable: false },

                            CR_DaysIncapacity: { editable: true },
                            CR_DaysIncapacityId: { editable: false },

                            CR_CertificateAccidentARL: { editable: true },
                            CR_CertificateAccidentARLId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQGetByType=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQUpsert=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQUpsert=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
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
                field: 'CR_Year',
                title: 'Año',
                width: '80px',
            }, {
                field: 'CR_ManHoursWorked',
                title: 'Horas Hombre Trabajadas',
            }, {
                field: 'CR_Fatalities ',
                title: 'Fatalidades',
            }, {
                field: 'CR_NumberAccident',
                title: 'Número Total de Incidentes (excluye Accidentes Incapacitantes)',
            }, {
                field: 'CR_NumberAccidentDisabling ',
                title: 'Número de Accidentes Incapacitantes',
            }, {
                field: 'CR_DaysIncapacity',
                title: 'Días de Incapacidad',
            }, {
                field: 'CR_CertificateAccidentARL',
                title: 'Certificado de accidentalidad',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CR_CertificateAccidentARL != null && dataItem.CR_CertificateAccidentARL.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${FileUrl}/gi, dataItem.CR_CertificateAccidentARL);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId,
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
                            url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByARL=true&SearchParam=' + options.data.filter.filters[0].value + '&CityId=',
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
};

/*CompanyFinancialObject*/
var Provider_CompanyFinancialObject = {

    ObjectId: '',
    ProviderPublicId: '',
    FinancialType: '',
    DateFormat: '',
    ProviderOptions: new Array(),
    CurrentAccounts: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.ProviderPublicId = vInitObject.ProviderPublicId;
        this.FinancialType = vInitObject.FinancialType;
        this.DateFormat = vInitObject.DateFormat;
        if (vInitObject.ProviderOptions != null) {
            $.each(vInitObject.ProviderOptions, function (item, value) {
                Provider_CompanyFinancialObject.ProviderOptions[value.Key] = value.Value;
            });
        }
    },

    RenderAsync: function () {
        if (Provider_CompanyFinancialObject.FinancialType == 501001) {
            Provider_CompanyFinancialObject.RenderBalanceSheet();
        }
        else if (Provider_CompanyFinancialObject.FinancialType == 501004) {
            Provider_CompanyFinancialObject.RenderBankInfo();
        }
    },

    RenderBalanceSheet: function () {
        $('#' + Provider_CompanyFinancialObject.ObjectId).kendoGrid({
            editable: false,
            navigatable: false,
            pageable: false,
            scrollable: true,
            toolbar: '<a class="k-button" href="javascript:Provider_CompanyFinancialObject.RenderBalanceSheetDetail(null);">Nuevo</a>',
            dataSource: {
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialGetByType=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType,
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
            columns: [{
                field: 'FinancialId',
                title: 'Id',
                width: '50px',
            }, {
                field: 'FinancialName',
                title: 'Nombre',
                width: '200px',
            }, {
                field: 'SH_Year',
                title: 'Año',
                width: '200px',
            }, {
                field: 'SH_BalanceSheetFile',
                title: 'Doc soporte.',
                width: '400px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.SH_BalanceSheetFile != null && dataItem.SH_BalanceSheetFile.length > 0) {
                        oReturn = oReturn + $('#' + Provider_CompanyFinancialObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyFinancialObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${SH_BalanceSheetFile}/gi, dataItem.EX_ExperienceFile);

                    return oReturn;
                },
            }, {
                field: 'Enable',
                title: 'Habilitado',
                width: '100px',
            }],
        });
    },

    RenderBalanceSheetDetail: function (dataItem) {

        var oFiancialId = (dataItem != null ? dataItem.FinancialId : '0');

        $.ajax({
            url: BaseUrl.ApiUrl + '/ProviderApi?FIBalanceSheetGetByFinancial=true&FinancialId=' + oFiancialId,
            dataType: 'json',
            success: function (result) {
                if (result != null && result.length > 0) {
                    //save current accounts
                    Provider_CompanyFinancialObject.CurrentAccounts = result;

                    //insert form
                    var oFormHtml = $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form').html();
                    oFormHtml = oFormHtml.replace(/\${FinancialId}/gi, oFiancialId);
                    $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').html(oFormHtml);

                    //init form controls
                    if (dataItem != null) {
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_FinancialName_' + oFiancialId).val(dataItem.FinancialName);
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_Enable_' + oFiancialId).prop('checked', dataItem.Enable);

                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_YearId_' + oFiancialId).val(dataItem.SH_YearId);
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_Year_' + oFiancialId).val(dataItem.SH_Year);

                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_CurrencyId_' + oFiancialId).val(dataItem.SH_CurrencyId);
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_Currency_' + oFiancialId).val(dataItem.SH_Currency);

                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFile_' + oFiancialId).val(dataItem.SH_BalanceSheetFile);
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFileId_' + oFiancialId).val(dataItem.SH_BalanceSheetFileId);
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFileLink_' + oFiancialId).attr('href', dataItem.SH_BalanceSheetFile);
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').find('iframe').attr('src', BaseUrl.PreviewPdfUrl.replace(/\${FilePath}/gi, dataItem.SH_BalanceSheetFile));
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFileLink_' + oFiancialId).show();
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').find('iframe').show();
                    }
                    else {
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_FinancialName_' + oFiancialId).val('');
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_Enable_' + oFiancialId).prop('checked', true);

                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_YearId_' + oFiancialId).val('');
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_Year_' + oFiancialId).val('');

                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_CurrencyId_' + oFiancialId).val('');
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_Currency_' + oFiancialId).val('');

                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFile_' + oFiancialId).val('');
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFileId_' + oFiancialId).val('');
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFileLink_' + oFiancialId).attr('href', '');
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').find('iframe').attr('src', '');
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFileLink_' + oFiancialId).hide();
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').find('iframe').hide();
                    }
                    $('<input type="file" id="files" name="files"/>')
                        .appendTo($('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFileUpload_' + oFiancialId))
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId,
                            autoUpload: true
                        },
                        success: function (e) {
                            if (e.response != null && e.response.length > 0) {
                                //set server fiel name
                                    $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFile_' + oFiancialId).val(e.response[0].ServerName);
                                    $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFileLink_' + oFiancialId).attr('href', e.response[0].ServerName);
                                    $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').find('iframe').attr('src', BaseUrl.PreviewPdfUrl.replace(/\${FilePath}/gi, e.response[0].ServerName));
                                    $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFileLink_' + oFiancialId).show();
                                    $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').find('iframe').show();
                            }
                        },
                    });

                    //init accounts object
                    Provider_CompanyFinancialObject.RenderBalanceSheetDetailAccounts($('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_Accounts_' + oFiancialId), Provider_CompanyFinancialObject.CurrentAccounts);
                    //calc total values
                    Provider_CompanyFinancialObject.CalculateBalanceSheet();
                        }
                },
            error: function (result) {
                alert(result);
            }
        });
    },

    RenderBalanceSheetDetailAccounts: function (container, lstAccounts) {
        if (container != null && $(container).length > 0 && lstAccounts != null && lstAccounts.length > 0) {
            $.each(lstAccounts, function (item, value) {
                if (value.ChildBalanceSheet != null && value.ChildBalanceSheet.length > 0) {
                    //parent node
                    var ParentHtml = $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_ParentAccount').html();
                    ParentHtml = ParentHtml.replace(/\${AccountName}/gi, value.RelatedAccount.ItemName);
                    ParentHtml = ParentHtml.replace(/\${AccountId}/gi, value.RelatedAccount.ItemId);
                    $(container).append(ParentHtml);
                    Provider_CompanyFinancialObject.RenderBalanceSheetDetailAccounts($('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_ParentAccount_' + value.RelatedAccount.ItemId), value.ChildBalanceSheet);
                }
                else {
                    //last node
                    var ChildHtml = $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_ChildAccount').html();
                    ChildHtml = ChildHtml.replace(/\${AccountName}/gi, value.RelatedAccount.ItemName);
                    ChildHtml = ChildHtml.replace(/\${AccountId}/gi, value.RelatedAccount.ItemId);
                    ChildHtml = ChildHtml.replace(/\${Value}/gi, Number(value.RelatedBalanceSheetDetail != null ? value.RelatedBalanceSheetDetail.Value : '0'));

                    if ($(container).find('ul').length == 0) {
                        $(container).append('<ul></ul>');
                        }

                    $(container).find('ul').append(ChildHtml);
                    }
            });

            //append on focus out event
            $('.' + Provider_CompanyFinancialObject.ObjectId + '_Detail_ChildAccount_selector').focusout(function () {
                if ($.isNumeric($(this).val()) == false) {
                    $(this).val(0);
                            }
                Provider_CompanyFinancialObject.CalculateBalanceSheet();
                        });
        }
            },

    CalculateBalanceSheet: function () {
        Provider_CompanyFinancialObject.CalculateBalanceSheetDetail(Provider_CompanyFinancialObject.CurrentAccounts);
    },

    CalculateBalanceSheetDetail: function (lstAccounts) {

        var SumResult = new Number();

        if (lstAccounts != null && lstAccounts.length > 0) {
            $.each(lstAccounts, function (item, value) {
                if (value.ChildBalanceSheet != null && value.ChildBalanceSheet.length > 0) {
                    value.ChildSum = Provider_CompanyFinancialObject.CalculateBalanceSheetDetail(value.ChildBalanceSheet);
                    SumResult = SumResult + Number(value.ChildSum);
                    $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_ParentAccount_' + value.RelatedAccount.ItemId + '_Total').html(value.ChildSum);
                }
                else {
                    SumResult = SumResult + Number($('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_ChildAccount_' + value.RelatedAccount.ItemId).val());
                        }
            });
                    }
        return SumResult;
                            },
    
    CancelBalanceSheetDetail: function () {
        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').fadeOut("slow", function () {
            $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').html('');
                        });
                    },

    SaveBalanceSheetDetail: function (vFinancialId) {
        if (Provider_CompanyFinancialObject.ValidateBalanceSheetDetail() == true) {
            $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_' + vFinancialId).submit();
        }
                },

    ValidateBalanceSheetDetail: function () {
        var oReturn = true;

        //validate balance values
        //Provider_CompanyFinancialObject.CalculateBalanceSheet();

        return oReturn;
            },
            columns: [{
                field: 'AccountName',
                title: 'Cuenta',
                width: '200px',
            }, {
                field: 'BalanceItemValue',
                title: 'Valor',
                width: '200px',
            }],
        });
    },

    RenderBankInfo: function () {
        $('#' + Provider_CompanyFinancialObject.ObjectId).kendoGrid({
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
                        id: 'FinancialId',
                        fields: {
                            FinancialId: { editable: false, nullable: true },
                            FinancialName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: 'boolean', defaultValue: true },

                            IB_Bank: { editable: true },
                            IB_BankId: { editable: false },
                            IB_BankName: { editable: true },

                            IB_AccountType: { editable: true },
                            IB_AccountTypeId: { editable: false },

                            IB_AccountNumber: { editable: true },
                            IB_AccountNumberId: { editable: false },

                            IB_AccountHolder: { editable: true },
                            IB_AccountHolderId: { editable: false },

                            IB_ABA: { editable: true },
                            IB_ABAId: { editable: false },

                            IB_Swift: { editable: true },
                            IB_SwiftId: { editable: false },

                            IB_IBAN: { editable: true },
                            IB_IBANId: { editable: false },

                            IB_Customer: { editable: true },
                            IB_CustomerId: { editable: false },

                            IB_AccountFile: { editable: true },
                            IB_AccountFileId: { editable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialGetByType=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType,
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                            }
                        });
                    },
                    //create: function (options) {
                    //    $.ajax({
                    //        url: BaseUrl.ApiUrl + '/ProviderApi?CICommercialUpsert=true&ProviderPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId + '&CommercialType=' + Provider_CompanyCommercialObject.CommercialType,
                    //        dataType: 'json',
                    //        type: 'post',
                    //        data: {
                    //            DataToUpsert: kendo.stringify(options.data)
                    //        },
                    //        success: function (result) {
                    //            options.success(result);
                    //        },
                    //        error: function (result) {
                    //            options.error(result);
                    //        }
                    //    });
                    //},
                    //update: function (options) {
                    //    $.ajax({
                    //        url: BaseUrl.ApiUrl + '/ProviderApi?CICommercialUpsert=true&ProviderPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId + '&CommercialType=' + Provider_CompanyCommercialObject.CommercialType,
                    //        dataType: 'json',
                    //        type: 'post',
                    //        data: {
                    //            DataToUpsert: kendo.stringify(options.data)
                    //        },
                    //        success: function (result) {
                    //            options.success(result);
                    //        },
                    //        error: function (result) {
                    //            options.error(result);
                    //        }
                    //    });
                    //},
                },
            },
            columns: [{
                field: 'FinancialId',
                title: 'Id',
                width: '50px',
            }, {
                field: 'FinancialName',
                title: 'Nombre',
                width: '200px',
            }, {
                field: 'SH_Year',
                title: 'Año',
                width: '200px',
            }, {
                field: 'SH_BalanceSheetFile',
                title: 'Doc soporte.',
                width: '400px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.SH_BalanceSheetFile != null && dataItem.SH_BalanceSheetFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyFinancialObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyFinancialObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${SH_BalanceSheetFile}/gi, dataItem.EX_ExperienceFile);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId,
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
                width: '100px',
            }],
        });
    },
};


var Provider_LegalInfoObject = {

    AutoCompleteId: '',
    ControlToRetornACId: '',
    ObjectId: '',
    ProviderPublicId: '',
    LegalInfoType: '',
    ChaimberOfComerceOptionList: new Array(),
    LegalId: '',
    DateFormat: '',

    Init: function (vInitiObject) {

        this.AutoCompleteId = vInitiObject.AutoCompleteId;
        this.ControlToRetornACId = vInitiObject.ControlToRetornACId;
        this.ObjectId = vInitiObject.ObjectId;
        this.ProviderPublicId = vInitiObject.ProviderPublicId;
        this.LegalInfoType = vInitiObject.LegalInfoType;
        this.ChaimberOfComerceOptionList = vInitiObject.ChaimberOfComerceOptionList;
        this.LegalId = vInitiObject.LegalId;
        this.DateFormat = vInitiObject.DateFormat;
        //Load AutoComplete 
        Provider_LegalInfoObject.AutoComplete(vInitiObject.AutoCompleteId, vInitiObject.ControlToRetornACId);

        $.each(vInitiObject.ChaimberOfComerceOptionList, function (item, value) {
            Provider_LegalInfoObject.ChaimberOfComerceOptionList[value.Key] = value.Value;
        });
    },

    RenderAsync: function () {
        if (Provider_LegalInfoObject.LegalInfoType == 601007) {
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
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                            },
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                    field: 'CD_PartnerName',
                    title: 'Nombre',
                }, {
                    field: 'CD_PartnerIdentificationNumber',
                    title: 'Número de Identificación',
                }, {
                    field: 'CD_PartnerRank',
                    title: 'Cargo',
                }, {
                    field: 'Enable',
                    title: 'Habilitado',
                    width: '100px',
                }],
        });
    },

    RenderUniqueRegister: function () {
        $('#' + Provider_LegalInfoObject.ObjectId).kendoGrid({
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
                        id: "LegalId",
                        fields: {
                            LegalId: { editable: false, nullable: true },
                            LegalName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            R_PersonType: { editable: true, validation: { required: true } },
                            R_PersonTypeId: { editable: false },

                            R_LargeContributor: { editable: true, type: "boolean", defaultValue: true, validation: { required: true } },
                            R_LargeContributorId: { editable: false },

                            R_LargeContributorReceipt: { editable: true, validation: { required: true } },
                            R_LargeContributorReceiptId: { editable: false },

                            R_LargeContributorDate: { editable: true },
                            R_LargeContributorDateId: { editable: false },

                            R_SelfRetainer: { editable: true, validation: { required: true }, type: "boolean", defaultValue: true },
                            R_SelfRetainerId: { editable: false },

                            R_SelfRetainerReciept: { editable: true, validation: { required: true } },
                            R_SelfRetainerRecieptId: { editable: false },

                            R_SelfRetainerDate: { editable: true },
                            R_SelfRetainerDateId: { editable: false },

                            R_EntityType: { editable: true, validation: { required: true } },
                            R_EntityTypeId: { editable: false },

                            R_IVA: { editable: true, type: "boolean", defaultValue: true, validation: { required: true } },
                            R_IVAId: { editable: false },

                            R_TaxPayerType: { editable: true, validation: { required: true } },
                            R_TaxPayerTypeId: { editable: false },

                            R_ICA: { editable: true, validation: { required: true } },
                            R_ICAId: { editable: false },

                            R_RUTFile: { editable: true, validation: { required: true } },
                            R_RUTFileId: { editable: false },

                            R_LargeContributorFile: { editable: true },
                            R_LargeContributorFileId: { editable: false },

                            R_SelfRetainerFile: { editable: true },
                            R_SelfRetainerFileId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoGetByType=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                field: 'LegalId',
                title: 'Id',
                width: '50px',
            }, {
                field: 'LegalName',
                title: 'Nombre',
                width: '200px',
            }, {
                field: 'R_PersonType',
                title: 'Tipo de Persona',
                width: '200px',
                template: function (dataItem) {

                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.R_PersonType != null) {
                        $.each(Provider_LegalInfoObject.ChaimberOfComerceOptionList[213], function (item, value) {
                            if (dataItem.R_PersonType == value.ItemId) {
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
                            dataSource: Provider_LegalInfoObject.ChaimberOfComerceOptionList[213],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'R_LargeContributor',
                title: 'Gran Contribuyente',
                width: '200px',
            }, {
                field: 'R_LargeContributorReceipt',
                title: 'Gran Contribuyente Recibo',
                width: '200px',
            }, {
                field: 'R_LargeContributorDate',
                title: 'Gran Contribuyente Fecha',
                width: '100px',
                format: Provider_LegalInfoObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'R_SelfRetainer',
                title: 'Autorretenedor',
                width: '200px',
            }, {
                field: 'R_SelfRetainerReciept',
                title: 'Autorretenedor Recibo',
                width: '200px',
            }, {
                field: 'R_SelfRetainerDate',
                title: 'Autorretenedor Fecha',
                width: '200px',
                format: Provider_LegalInfoObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'R_EntityType',
                title: 'Tipo de Entidad',
                width: '200px',
                template: function (dataItem) {

                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.R_EntityType != null) {
                        $.each(Provider_LegalInfoObject.ChaimberOfComerceOptionList[214], function (item, value) {
                            if (dataItem.R_EntityType == value.ItemId) {
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
                            dataSource: Provider_LegalInfoObject.ChaimberOfComerceOptionList[214],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'R_IVA',
                title: 'IVA',
                width: '200px',
            }, {
                field: 'R_TaxPayerType',
                title: 'Tipo de Régimen',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.R_TaxPayerType != null) {
                        $.each(Provider_LegalInfoObject.ChaimberOfComerceOptionList[215], function (item, value) {
                            if (dataItem.R_TaxPayerType == value.ItemId) {
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
                            dataSource: Provider_LegalInfoObject.ChaimberOfComerceOptionList[215],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'R_ICA',
                title: 'ICA',
                width: '350px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.R_ICA != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.R_ICA;
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
                        dataTextField: 'ActivityName',
                        select: function (e) {
                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name
                            options.model[options.field] = selectedItem.ActivityName;
                            options.model['R_ICAName'] = selectedItem.EconomicActivityId;
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
                                        }
                                    });
                                },
                            }
                        }
                    });
                },
            }, {
                field: 'R_RUTFile',
                title: 'RUT Anexo',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.R_RUTFile != null && dataItem.R_RUTFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_LegalInfoObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_LegalInfoObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${R_RUTFile}/gi, dataItem.R_RUTFile);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId,
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
                field: 'R_LargeContributorFile',
                title: 'Gran Contribuyente Anexo',
                width: '200px',
                template: function (dataItem) {

                    var oReturn = '';
                    if (dataItem != null && dataItem.R_LargeContributorFile != null && dataItem.R_LargeContributorFile.length > 0) {

                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_LegalInfoObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_LegalInfoObject.ObjectId + '_NoFile').html();
                    }
                    oReturn = oReturn.replace(/\${R_RUTFile}/gi, dataItem.R_LargeContributorFile);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId,
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
                field: 'R_SelfRetainerFile',
                title: 'Autorretenedor Anexo',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.R_SelfRetainerFile != null && dataItem.R_SelfRetainerFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_LegalInfoObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_LegalInfoObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${R_RUTFile}/gi, dataItem.R_SelfRetainerFile);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId,
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
                width: '200px',
            }],
        });
    },

    RenderCIFIN: function () {
        $('#' + Provider_LegalInfoObject.ObjectId).kendoGrid({
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
                        id: "LegalId",
                        fields: {
                            LegalId: { editable: false, nullable: true },
                            LegalName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            CF_QueryDate: { editable: true },
                            CF_QueryDateId: { editable: false },

                            CF_ResultQuery: { editable: true },
                            CF_ResultQueryId: { editable: false },

                            CF_AutorizationFile: { editable: true },
                            CF_AutorizationFileId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoGetByType=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                field: 'LegalId',
                title: 'Id',
                width: '50px',
            }, {
                field: 'CF_QueryDate',
                title: 'Fecha de Consulta',
                width: '200px',
                format: Provider_LegalInfoObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'CF_ResultQuery',
                title: 'Resultado de la Consulta',
                width: '200px',
            }, {
                field: 'CF_AutorizationFile',
                title: 'Archivo de Autorización',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CF_AutorizationFile != null && dataItem.CF_AutorizationFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_LegalInfoObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_LegalInfoObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${R_RUTFile}/gi, dataItem.CF_AutorizationFile);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId,
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
                width: '200px',
            }],
        });
    },

    RenderSARLAFT: function () {
        $('#' + Provider_LegalInfoObject.ObjectId).kendoGrid({
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
                        id: "LegalId",
                        fields: {
                            LegalId: { editable: false, nullable: true },
                            LegalName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            SF_ProcessDate: { editable: true },
                            SF_ProcessDateId: { editable: false },

                            SF_PersonType: { editable: true },
                            SF_PersonTypeId: { editable: false },

                            SF_SARLAFTFile: { editable: true },
                            SF_SARLAFTFileId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoGetByType=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                field: 'LegalId',
                title: 'Id',
                width: '50px',
            }, {
                field: 'SF_ProcessDate',
                title: 'Fecha de Diligenciamiento',
                width: '200px',
                format: Provider_LegalInfoObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'SF_PersonType',
                title: 'Tipo de Persona',
                width: '200px',
                template: function (dataItem) {

                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.SF_PersonType != null) {
                        $.each(Provider_LegalInfoObject.ChaimberOfComerceOptionList[213], function (item, value) {
                            if (dataItem.SF_PersonType == value.ItemId) {
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
                            dataSource: Provider_LegalInfoObject.ChaimberOfComerceOptionList[213],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'SF_SARLAFTFile',
                title: 'Archivo Anexo',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.SF_SARLAFTFile != null && dataItem.SF_SARLAFTFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_LegalInfoObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_LegalInfoObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${R_RUTFile}/gi, dataItem.SF_SARLAFTFile);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId,
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
                width: '200px',
            }],
        });
    },

    RenderResolutions: function () {
        $('#' + Provider_LegalInfoObject.ObjectId).kendoGrid({
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
                        id: "LegalId",
                        fields: {
                            LegalId: { editable: false, nullable: true },
                            LegalName: { editable: true, validation: { required: true } },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            RS_EntityType: { editable: true, validation: { required: true } },
                            RS_EntityTypeId: { editable: false },

                            RS_ResolutionFile: { editable: true, validation: { required: true } },
                            RS_ResolutionFileId: { editable: false },

                            RS_StartDate: { editable: true, validation: { required: true } },
                            RS_StartDateId: { editable: false },

                            RS_EndDate: { editable: true },
                            RS_EndDateId: { editable: false },

                            RS_Description: { editable: true },
                            RS_DescriptionId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoGetByType=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                field: 'LegalId',
                title: 'Id',
                width: '50px',
            }, {
                field: 'RS_EntityType',
                title: 'Tipo de Entidad',
                width: '300px',
                template: function (dataItem) {

                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.RS_EntityType != null) {
                        $.each(Provider_LegalInfoObject.ChaimberOfComerceOptionList[218], function (item, value) {
                            if (dataItem.RS_EntityType == value.ItemId) {
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
                            dataSource: Provider_LegalInfoObject.ChaimberOfComerceOptionList[218],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'RS_StartDate',
                title: 'Fecha Inicial',
                width: '200px',
                format: Provider_LegalInfoObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'RS_EndDate',
                title: 'Fecha Final',
                width: '200px',
                format: Provider_LegalInfoObject.DateFormat,
                editor: function (container, options) {
                    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
                        .appendTo(container)
                        .kendoDateTimePicker({});
                },
            }, {
                field: 'RS_Description',
                title: 'Descripción',
                width: '300px',
            }, {
                field: 'RS_ResolutionFile',
                title: 'Archivo Anexo',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.RS_ResolutionFile != null && dataItem.RS_ResolutionFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_LegalInfoObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_LegalInfoObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${R_RUTFile}/gi, dataItem.RS_ResolutionFile);

                    return oReturn;
                },
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files"/>')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId,
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
                width: '200px',
            }, ],
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

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
    debugger;
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
    ContactOptionList: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.ProviderPublicId = vInitObject.ProviderPublicId;
        this.ContactType = vInitObject.ContactType;
        $.each(vInitObject.ContactOptionList, function (item, value) {
            Provider_CompanyContactObject.ContactOptionList[value.Key] = value.Value;
        });
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
                        $.each(Provider_CompanyContactObject.ContactOptionList[209], function (item, value) {
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
                            dataSource: Provider_CompanyContactObject.ContactOptionList[209],
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
                        $.each(Provider_CompanyContactObject.ContactOptionList[210], function (item, value) {
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
                            dataSource: Provider_CompanyContactObject.ContactOptionList[210],
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
                        $.each(Provider_CompanyContactObject.ContactOptionList[101], function (item, value) {
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
                            dataSource: Provider_CompanyContactObject.ContactOptionList[101],
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
                width: "200px",
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

                            BR_City: { editable: true, validation: { required: true } },
                            BR_CityId: { editable: false },

                            BR_Phone: { editable: true },
                            BR_PhoneId: { editable: false },

                            BR_Fax: { editable: true },
                            BR_FaxId: { editable: false },

                            BR_Email: { editable: true },
                            BR_EmailId: { editable: false },

                            BR_Website: { editable: true },
                            BR_WebsiteId: { editable: false },

                            BR_GeoLocation: { editable: true },
                            BR_GeoLocationId: { editable: false },
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
                field: 'BR_Representative',
                title: 'Representante',
            }, {
                field: 'BR_Address',
                title: 'Dirección',
            }, {
                field: 'BR_City',
                title: 'Ciudad',
            }, {
                field: 'BR_Phone',
                title: 'Teléfono',
            }, {
                field: 'BR_Fax',
                title: 'Fax',
            }, {
                field: 'BR_Email',
                title: 'Correo electrónico',
            }, {
                field: 'BR_Website',
                title: 'Página web',
            }, {
                field: 'BR_GeoLocation',
                title: 'Geolocalización',
            }, {
                field: 'Enable',
                title: 'Habilitado',
                width: "100px",
            }],
        });
    },
};



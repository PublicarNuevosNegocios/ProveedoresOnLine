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
        if (Provider_CompanyContactObject.ContactType == 11001) {
            Provider_CompanyContactObject.RenderCompanyContact();
        }
        else if (Provider_CompanyContactObject.ContactType == 11002) {
            Provider_CompanyContactObject.RenderPersonContact();
        }
    },

    RenderCompanyContact: function () {
        $('#' + Provider_CompanyContactObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo contacto' },
                { name: 'save', text: 'Guardar cambios' },
                { name: 'cancel', text: 'Descartar cambios' }
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?ContactGetByType=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?ContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?ContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
            }, {
                field: 'ContactName',
                title: 'Nombre',
            }, {
                field: 'CC_CompanyContactType',
                title: 'Tipo de contacto',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.CC_CompanyContactType != null) {
                        $.each(Provider_CompanyContactObject.ContactOptionList[12], function (item, value) {
                            if (dataItem.CC_CompanyContactType == value.ItemId) {
                                oReturn = value.ItemName;
                            }
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    $('<input data-bind="value:' + options.field + '"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            dataSource: Provider_CompanyContactObject.ContactOptionList[12],
                            dataTextField: "ItemName",
                            dataValueField: "ItemId"
                        });
                },
            }, {
                field: 'CC_Value',
                title: 'Valor',
            }, {
                field: 'Enable',
                title: 'Habilitado',
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
                { name: 'create', text: 'Nuevo contacto' },
                { name: 'save', text: 'Guardar cambios' },
                { name: 'cancel', text: 'Descartar cambios' }
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

                            CP_IdentificationNumber: { editable: true },
                            CP_IdentificationNumberId: { editable: false },

                            CP_IdentificationCity: { editable: true },
                            CP_IdentificationCityId: { editable: false },

                            CP_IdentificationFile: { editable: true },
                            CP_IdentificationFileId: { editable: false },

                            CP_Name: { editable: true },
                            CP_NameId: { editable: false },

                            CP_Phone: { editable: true },
                            CP_PhoneId: { editable: false },

                            CP_Email: { editable: true },
                            CP_EmailId: { editable: false },

                            CP_Negotiation: { editable: true },
                            CP_NegotiationId: { editable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?ContactGetByType=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
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
                        debugger;
                        //$.ajax({
                        //    url: BaseUrl.ApiUrl + '/ProviderApi?ContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
                        //    dataType: 'json',
                        //    type: 'post',
                        //    data: {
                        //        DataToUpsert: kendo.stringify(options.data)
                        //    },
                        //    success: function (result) {
                        //        options.success(result);
                        //    },
                        //    error: function (result) {
                        //        options.error(result);
                        //    }
                        //});
                    },
                    update: function (options) {
                        //$.ajax({
                        //    url: BaseUrl.ApiUrl + '/ProviderApi?ContactUpsert=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType,
                        //    dataType: 'json',
                        //    type: 'post',
                        //    data: {
                        //        DataToUpsert: kendo.stringify(options.data)
                        //    },
                        //    success: function (result) {
                        //        options.success(result);
                        //    },
                        //    error: function (result) {
                        //        options.error(result);
                        //    }
                        //});
                    },
                },
            },
            columns: [{
                field: 'ContactId',
                title: 'Id',
            }, {
                field: 'ContactName',
                title: 'Nombre',
            }, {
                field: 'CP_PersonContactType',
                title: 'Tipo de representante',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.CP_PersonContactType != null) {
                        $.each(Provider_CompanyContactObject.ContactOptionList[19], function (item, value) {
                            if (dataItem.CP_PersonContactType == value.ItemId) {
                                oReturn = value.ItemName;
                            }
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    $('<input data-bind="value:' + options.field + '"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            dataSource: Provider_CompanyContactObject.ContactOptionList[19],
                            dataTextField: "ItemName",
                            dataValueField: "ItemId"
                        });
                },
            }, {
                field: 'CP_IdentificationType',
                title: 'Tipo de identificación',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.CP_IdentificationType != null) {
                        $.each(Provider_CompanyContactObject.ContactOptionList[3], function (item, value) {
                            if (dataItem.CP_IdentificationType == value.ItemId) {
                                oReturn = value.ItemName;
                            }
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    $('<input data-bind="value:' + options.field + '"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            dataSource: Provider_CompanyContactObject.ContactOptionList[3],
                            dataTextField: "ItemName",
                            dataValueField: "ItemId"
                        });
                },
            }, {
                field: 'CP_IdentificationNumber',
                title: 'Número de identificación',
            }, {
                field: 'CP_IdentificationCity',
                title: 'Ciudad de expedicion del documento',
            }, {
                field: 'CP_Phone',
                title: 'Telefono',
            }, {
                field: 'CP_Email',
                title: 'Correo electronico',
            }, {
                field: 'CP_Negotiation',
                title: 'Capacidad de negociación',
            }, {
                field: 'CP_IdentificationFile',
                title: 'Doc representante legal.',
                width: "500px",
                template: '<a href="#:CP_IdentificationFile#">Ver archivo</a>',
                editor: function (container, options) {
                    var oFileExit = true;
                    $('<input type="file" id="files" name="files" />')
                    .appendTo(container)
                    .kendoUpload({
                        multiple: false,
                        async: {
                            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + Provider_CompanyContactObject.ProviderPublicId,
                            autoUpload: true
                        },
                        success: function (e) {
                            alert(e.response[0].ServerName)
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
            }],
        });
    },
};

function checkOffset() {
    if ($('.POBOProviderActions').offset().top + $('.POBOProviderActions').height()
                >= $('#POBORenderFooter').offset().top - 10)
        $('.POBOProviderActions').css('position', 'absolute');
    if ($(document).scrollTop() + window.innerHeight < $('#POBORenderFooter').offset().top)
        $('.POBOProviderActions').css('position', 'fixed');
    $('.POBOProviderActions').text($(document).scrollTop() + window.innerHeight);
}

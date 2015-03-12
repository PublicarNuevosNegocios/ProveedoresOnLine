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
    Message('success', null);
}

function Provider_Navigate(Url, GridName, ButtonClass) {
    $('#' + GridName.GridName).data("kendoGrid").saveChanges();
    $('.' + ButtonClass.ButtonClass).attr('href', Url.Url);

    window.location = $('.' + ButtonClass.ButtonClass).attr("href");

}

/*Provider search object*/
var Provider_SearchObject = {
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
        $('#' + Provider_SearchObject.ObjectId + '_txtSearch').keypress(function (e) {
            if (e.which == 13) {
                Provider_SearchObject.SearchEvent(null, null);
            }
        });


        //init grid
        $('#' + Provider_SearchObject.ObjectId).kendoGrid({
            editable: false,
            navigatable: false,
            pageable: true,
            scrollable: true,
            selectable: true,
            dataSource: {
                pageSize: Provider_SearchObject.PageSize,
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

                        var oSearchParam = $('#' + Provider_SearchObject.ObjectId + '_txtSearch').val();

                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?SMProviderSearch=true&SearchParam=' + oSearchParam + '&SearchFilter=' + Provider_SearchObject.SearchFilter + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
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
                        window.location = BaseUrl.SiteUrl + 'Provider/GIProviderUpsert?ProviderPublicId=' + $($(item).find('td')[1]).text().replace(/ /gi, '');
                    }
                });
            },
            columns: [{
                field: 'ImageUrl',
                title: 'Logo',
                template: '<img style="width:50px;height:50px;" src="${ImageUrl}" />',
                width: '50px',
                attributes: { style: "text-align:center;" },
            }, {
                field: 'ProviderPublicId',
                title: 'Id',
                width: '50px',
            }, {
                field: 'ProviderName',
                title: 'Nombre',
                width: '50px',
            }, {
                field: 'ProviderType',
                title: 'Tipo',
                width: '50px',
            }, {
                field: 'IdentificationType',
                title: 'Identification',
                template: '${IdentificationType} ${IdentificationNumber}',
                width: '50px',
            }, {
                field: 'IsOnRestrictiveList',
                title: 'Estado Listas Restrictivas',
                width: '155px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.IsOnRestrictiveList == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
                Provider_SearchObject.SearchFilter = vSearchFilter + ',' + Provider_SearchObject.SearchFilter;
            }
            else {
                Provider_SearchObject.SearchFilter = Provider_SearchObject.SearchFilter.replace(new RegExp(vSearchFilter, 'gi'), '');
            }
        }
        var oSearchParam = $('#' + Provider_SearchObject.ObjectId + '_txtSearch').val();
        window.location = BaseUrl.SiteUrl + 'Provider/Index?SearchParam=' + oSearchParam + '&SearchFilter=' + Provider_SearchObject.SearchFilter;
    },
};

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

        //focus on the grid
        $('#' + Provider_CompanyContactObject.ObjectId).data("kendoGrid").table.focus();

        //config keyboard
        Provider_CompanyContactObject.ConfigKeyBoard();

        //Config Events
        Provider_CompanyContactObject.ConfigEvents();
    },

    ConfigKeyBoard: function () {

        //init keyboard tooltip
        $('#' + Provider_CompanyContactObject.ObjectId + '_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {
            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Provider_CompanyContactObject.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Provider_CompanyContactObject.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Provider_CompanyContactObject.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function () {

        //config grid visible enables event
        $('#' + Provider_CompanyContactObject.ObjectId + '_ViewEnable').change(function () {
            $('#' + Provider_CompanyContactObject.ObjectId).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnable: function () {

        return $('#' + Provider_CompanyContactObject.ObjectId + '_ViewEnable').length > 0 ? $('#' + Provider_CompanyContactObject.ObjectId + '_ViewEnable').is(':checked') : true;
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyContactObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyContactObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: 'ContactId',
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true },
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactGetByType=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType + '&ViewEnable=' + Provider_CompanyContactObject.GetViewEnable(),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
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
                                Message('success', 'Se creó el registro.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
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
                                Message('success', 'Se editó la fila con el id ' + options.data.ContactId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.ContactId + '.');
                            }
                        });
                    },
                },
            },
            columns: [{
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
            }, {
                field: 'ContactName',
                title: 'Nombre',
                width: '180px',
            }, {
                field: 'CC_CompanyContactType',
                title: 'Tipo de contacto',
                width: '190px',
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
                title: 'Información',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem.CC_Value == '') {
                        if (dataItem.CC_CompanyContactType == "209001") {
                            oReturn = '<label class="PlaceHolder">Ej: 7560000</label>'
                        }
                        else if (dataItem.CC_CompanyContactType == "209002") {
                            oReturn = '<label class="PlaceHolder">Ej: 3161234567</label>'
                        }
                        else if (dataItem.CC_CompanyContactType == "209003") {
                            oReturn = '<label class="PlaceHolder">Ej: www.prueba.com</label>'
                        }
                        else if (dataItem.CC_CompanyContactType == "209004") {
                            oReturn = '<label class="PlaceHolder">Ej: 110221</label>'
                        }
                        else if (dataItem.CC_CompanyContactType == "209005") {
                            oReturn = '<label class="PlaceHolder">Ej: 7560000</label>'
                        }
                        else if (dataItem.CC_CompanyContactType == "209006") {
                            oReturn = '<label class="PlaceHolder">Ej: prueba@prueba.com</label>'
                        }
                    }
                    else {
                        oReturn = dataItem.CC_Value;
                    }

                    return oReturn;
                },
                width: '190px',
            }, {
                field: 'ContactId',
                title: 'Id Interno',
                width: '78px',
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyContactObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyContactObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: 'ContactId',
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true },
                            Enable: { editable: true, type: 'boolean', defaultValue: true },

                            CP_PersonContactType: { editable: true },
                            CP_PersonContactTypeId: { editable: false },

                            CP_IdentificationType: { editable: true },
                            CP_IdentificationTypeId: { editable: false },

                            CP_IdentificationNumber: { editable: true, validation: { required: false } },
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactGetByType=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType + '&ViewEnable=' + Provider_CompanyContactObject.GetViewEnable(),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
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
                                Message('success', 'Se creó el registro.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
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
                                Message('success', 'Se editó la fila con el id ' + options.data.ContactId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.ContactId + '.');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
                width: '180px',
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
                width: '180px',
            }, {
                field: 'CP_IdentificationCity',
                title: 'Ciudad de expedicion del documento',
                width: '180px',
            }, {
                field: 'CP_Phone',
                title: 'Telefono',
                width: '150px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.CP_Phone == '') {
                        oReturn = '<label class="PlaceHolder">Ej: (57) 3213232 ext 22</label>';
                    }
                    else {
                        oReturn = dataItem.CP_Phone;
                    }

                    return oReturn;
                },
            }, {
                field: 'CP_Email',
                title: 'Correo electronico',
                width: '200px',
            }, {
                field: 'CP_Negotiation',
                title: 'Capacidad de negociación',
                width: '190px',
            }, {
                field: 'CP_IdentificationFile',
                title: 'Doc representante legal.',
                width: '292px',
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
                field: 'ContactId',
                title: 'Id Interno',
                width: '78px',
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyContactObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyContactObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: 'ContactId',
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true },
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

                            BR_IsPrincipal: { editable: true, type: 'boolean', defaultValue: true },
                            BR_IsPrincipalId: { editable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactGetByType=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType + '&ViewEnable=' + Provider_CompanyContactObject.GetViewEnable(),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
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
                                Message('success', 'Se creó el registro.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
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
                                Message('success', 'Se editó la fila con el id ' + options.data.ContactId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.ContactId + '.');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                alt: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'BR_IsPrincipal',
                title: 'Sucursal Principal',
                width: '136px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.BR_IsPrincipal == true) {
                        oReturn = '<div class="POBOMainBranch">Si</div>';
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }, {
                field: 'BR_Representative',
                title: 'Representante',
                width: '229px',
            }, {
                field: 'BR_Address',
                title: 'Dirección',
                width: '200px',
            }, {
                field: 'BR_CityName',
                title: 'Ciudad',
                width: '180px',
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
                width: '120px',
            }, {
                field: 'BR_Fax',
                title: 'Fax',
                width: '120px',
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
                width: '70px',
            }, {
                field: 'BR_Longitude',
                title: 'Longitud',
                width: '80px',
            }, {
                field: 'ContactName',
                title: 'Nombre',
                width: '160px',
            }, {
                field: 'ContactId',
                title: 'Id Interno',
                width: '78px',
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyContactObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyContactObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: 'ContactId',
                        fields: {
                            ContactId: { editable: false, nullable: true },
                            ContactName: { editable: true },
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?GIContactGetByType=true&ProviderPublicId=' + Provider_CompanyContactObject.ProviderPublicId + '&ContactType=' + Provider_CompanyContactObject.ContactType + '&ViewEnable=' + Provider_CompanyContactObject.GetViewEnable(),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
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
                                Message('success', 'Se creó el registro.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
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
                                Message('success', 'Se editó la fila con el id ' + options.data.ContactId + '.');

                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.ContactId + '.');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'ContactName',
                title: 'Razón social',
                width: '200px',
            }, {
                field: 'DT_DistributorType',
                title: 'Tipo de distribuidor',
                width: '180px',
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
                width: '170px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.DT_Phone == '') {
                        oReturn = '<label class="PlaceHolder">Ej: (57) 3213232 ext 22</label>';
                    }
                    else {
                        oReturn = dataItem.DT_Phone;
                    }

                    return oReturn;
                },
            }, {
                field: 'DT_CityName',
                title: 'Ciudad',
                width: '180px',
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
                width: '160px',
                format: Provider_CompanyContactObject.DateFormat,
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
                field: 'DT_DistributorFile',
                title: 'Doc soporte.',
                width: '292px',
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
                field: 'ContactId',
                title: 'Id Interno',
                width: '78px',
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

        //config keyboard
        Provider_CompanyCommercialObject.ConfigKeyBoard();

        //Config Events
        Provider_CompanyCommercialObject.ConfigEvents();
    },

    ConfigKeyBoard: function () {

        //init keyboard tooltip
        $('#' + Provider_CompanyCommercialObject.ObjectId + '_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {

            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Provider_CompanyCommercialObject.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Provider_CompanyCommercialObject.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Provider_CompanyCommercialObject.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function () {
        //config grid visible enables event
        $('#' + Provider_CompanyCommercialObject.ObjectId + '_ViewEnable').change(function () {
            $('#' + Provider_CompanyCommercialObject.ObjectId).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnable: function () {
        return $('#' + Provider_CompanyCommercialObject.ObjectId + '_ViewEnable').length > 0 ? $('#' + Provider_CompanyCommercialObject.ObjectId + '_ViewEnable').is(':checked') : true;
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyCommercialObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyCommercialObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: 'CommercialId',
                        fields: {
                            CommercialId: { editable: false, nullable: true },
                            CommercialName: { editable: true },
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

                            EX_ExperienceFile: { editable: true },
                            EX_ExperienceFileId: { editable: false },

                            EX_ContractSubject: { editable: true },
                            EX_ContractSubjectId: { editable: false },

                            EX_EconomicActivity: { editable: true, defaultValue: null },

                            EX_CustomEconomicActivity: { editable: true, defaultValue: null },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?CICommercialGetByType=true&ProviderPublicId=' + Provider_CompanyCommercialObject.ProviderPublicId + '&CommercialType=' + Provider_CompanyCommercialObject.CommercialType + '&ViewEnable=' + Provider_CompanyCommercialObject.GetViewEnable(),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
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
                                Message('success', 'Se creó el registro.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
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
                                Message('success', 'Se editó la fila con el id ' + options.data.CommercialId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.CommercialId + '.');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'EX_Client',
                title: 'Cliente',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.EX_Client == '') {
                        oReturn = '<label class="PlaceHolder">NOMBRES Y APELLIDOS</label>'
                    }
                    else {
                        oReturn = dataItem.EX_Client;
                    }

                    return oReturn;
                },
            }, {
                field: 'EX_ContractType',
                title: 'Tipo de contrato',
                width: '180px',
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
                field: 'EX_ContractNumber',
                title: 'Número de contrato',
                width: '160px',
            }, {
                field: 'EX_ContractSubject',
                title: 'Objeto del contrato',
                width: '400px',
                editor: function (container, options) {
                    $('<textarea data-bind="value: ' + options.field + '"></textarea>')
                        .appendTo(container);
                },
            }, {
                field: 'EX_Currency',
                title: 'Modeda',
                width: '180px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.EX_Currency != null) {
                        $.each(Provider_CompanyCommercialObject.ProviderOptions[108], function (item, value) {
                            if (dataItem.EX_Currency == value.ItemId) {
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
                            dataSource: Provider_CompanyCommercialObject.ProviderOptions[108],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'EX_ContractValue',
                title: 'Valor de contrato',
                width: '180px',
            }, {
                field: 'EX_Phone',
                title: 'Telefono',
                width: '170px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.EX_Phone == '') {
                        oReturn = '<label class="PlaceHolder">(57)7655454</label>'
                    }
                    else {
                        oReturn = dataItem.EX_Phone;
                    }

                    return oReturn;
                },
            }, {
                field: 'EX_DateIssue',
                title: 'Inicio',
                width: '160px',
                format: Provider_CompanyCommercialObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                },
            }, {
                field: 'EX_DueDate',
                title: 'Fin',
                width: '160px',
                format: Provider_CompanyCommercialObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                },
            }, {
                field: 'EX_EconomicActivity',
                title: 'Maestra Estandar',
                width: '380px',
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

                    //get current values
                    var oCurrentValue = new Array();
                    if (options.model[options.field] != null) {
                        $.each(options.model[options.field], function (item, value) {
                            oCurrentValue.push({
                                EconomicActivityId: value.EconomicActivityId,
                                ActivityName: value.ActivityName,
                                ActivityType: value.ActivityType,
                                ActivityGroup: value.ActivityGroup,
                                ActivityCategory: value.ActivityCategory
                            });
                        });
                    }

                    //init multiselect
                    $('<select id="' + Provider_CompanyCommercialObject.ObjectId + '_EconomicActivityMultiselect" multiple="multiple" />')
                        .appendTo(container)
                        .kendoMultiSelect({
                            minLength: 2,
                            dataValueField: 'EconomicActivityId',
                            dataTextField: 'ActivityName',
                            autoBind: false,
                            itemTemplate: $('#' + Provider_CompanyCommercialObject.ObjectId + '_MultiAC_ItemTemplate').html(),
                            value: oCurrentValue,
                            change: function () {
                                //get selected values
                                if ($('#' + Provider_CompanyCommercialObject.ObjectId + '_EconomicActivityMultiselect').length > 0) {
                                    options.model[options.field] = $('#' + Provider_CompanyCommercialObject.ObjectId + '_EconomicActivityMultiselect').data('kendoMultiSelect')._dataItems;
                                    options.model.dirty = true;
                                }
                            },
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
                                                    options.success([]);
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

                    //remove attribute role from input for space search
                    var inputAux = $('#' + Provider_CompanyCommercialObject.ObjectId + '_EconomicActivityMultiselect').data("kendoMultiSelect").input;
                    $(inputAux).attr('role', '');
                },
            }, {
                field: 'EX_CustomEconomicActivity',
                title: 'Maestra personalizada',
                width: '380px',
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

                    //get current values
                    var oCurrentValue = new Array();

                    if (options.model[options.field] != null) {
                        $.each(options.model[options.field], function (item, value) {
                            oCurrentValue.push({
                                EconomicActivityId: value.EconomicActivityId,
                                ActivityName: value.ActivityName,
                                ActivityType: value.ActivityType,
                                ActivityGroup: value.ActivityGroup,
                                ActivityCategory: value.ActivityCategory
                            });
                        });
                    }

                    //init multiselect
                    $('<select id="' + Provider_CompanyCommercialObject.ObjectId + '_CustomEconomicActivityMultiselect" multiple="multiple" />')
                        .appendTo(container)
                        .kendoMultiSelect({
                            minLength: 2,
                            dataValueField: 'EconomicActivityId',
                            dataTextField: 'ActivityName',
                            autoBind: false,
                            itemTemplate: $('#' + Provider_CompanyCommercialObject.ObjectId + '_MultiAC_ItemTemplate').html(),
                            value: oCurrentValue,
                            change: function () {
                                //get selected values
                                if ($('#' + Provider_CompanyCommercialObject.ObjectId + '_CustomEconomicActivityMultiselect').length > 0) {
                                    options.model[options.field] = $('#' + Provider_CompanyCommercialObject.ObjectId + '_CustomEconomicActivityMultiselect').data('kendoMultiSelect')._dataItems;
                                    options.model.dirty = true;
                                }
                            },
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
                                                    options.success([]);
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

                    //remove attribute role from input for space search
                    var inputAux = $('#' + Provider_CompanyCommercialObject.ObjectId + '_CustomEconomicActivityMultiselect').data("kendoMultiSelect").input;
                    $(inputAux).attr('role', '');
                },
            }, {
                field: 'EX_ExperienceFile',
                title: 'Doc soporte.',
                width: '292px',
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
                field: 'CommercialName',
                title: 'Nombre',
                width: '160px',
            }, {
                field: 'CommercialId',
                title: 'Id Interno',
                width: '78px',
            }, ],
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
    CompanyRiskType: '',
    DateFormat: '',
    HSEQOptionList: new Array(),
    YearOptionList: new Array(),

    Init: function (vInitiObject) {
        this.ObjectId = vInitiObject.ObjectId;
        this.ProviderPublicId = vInitiObject.ProviderPublicId;
        this.AutoCompleteId = vInitiObject.AutoCompleteId;
        this.ControlToReturnACId = vInitiObject.ControlToRetornACId;
        this.HSEQType = vInitiObject.HSEQType;
        this.CompanyRiskType = vInitiObject.CompanyRiskType;
        this.DateFormat = vInitiObject.DateFormat;
        Provider_CompanyHSEQObject.AutoComplete(vInitiObject.AutoCompleteId, vInitiObject.ControlToRetornACId);
        $.each(vInitiObject.HSEQOptionList, function (item, value) {
            Provider_CompanyHSEQObject.HSEQOptionList[value.Key] = value.Value;
        });
        if (vInitiObject.YearOptionList != null) {
            $.each(vInitiObject.YearOptionList, function (item, value) {
                Provider_CompanyHSEQObject.YearOptionList[value.Key] = value.Value;
            });
        }
    },

    RenderAsync: function () {
        if (Provider_CompanyHSEQObject.HSEQType == 701001) {
            Provider_CompanyHSEQObject.RenderCompanyCertification();
        }
        else if (Provider_CompanyHSEQObject.HSEQType == 701002) {
            Provider_CompanyHSEQObject.RenderCompanyHealthyPolitics();
        }
        else if (Provider_CompanyHSEQObject.HSEQType == 701004) {
            Provider_CompanyHSEQObject.RenderCompanyRiskPolicies();
        }

        //config keyboard
        Provider_CompanyHSEQObject.ConfigKeyBoard();

        //Config Events
        Provider_CompanyHSEQObject.ConfigEvents();
    },

    ConfigKeyBoard: function () {

        //init keyboard tooltip
        $('#' + Provider_CompanyHSEQObject.ObjectId + '_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {
            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Provider_CompanyHSEQObject.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Provider_CompanyHSEQObject.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Provider_CompanyHSEQObject.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function () {
        //config grid visible enables event
        $('#' + Provider_CompanyHSEQObject.ObjectId + '_ViewEnable').change(function () {
            $('#' + Provider_CompanyHSEQObject.ObjectId).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnable: function () {
        return $('#' + Provider_CompanyHSEQObject.ObjectId + '_ViewEnable').length > 0 ? $('#' + Provider_CompanyHSEQObject.ObjectId + '_ViewEnable').is(':checked') : true;
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyHSEQObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyHSEQObject.ObjectId + '_ShortcutToolTipTemplate').html() },
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
                            C_CertificationCompanyName: { editable: true, validation: { required: true } },

                            C_Rule: { editable: true, validation: { required: true } },
                            C_RuleId: { editable: false },
                            C_RuleName: { editable: true },

                            C_StartDateCertification: { editable: true },
                            C_StartDateCertificationId: { editable: false },

                            C_EndDateCertification: { editable: true },
                            C_EndDateCertificationId: { editable: false },

                            C_CCS: { editable: true, validation: { required: true }, type: "number" },
                            C_CCSId: { editable: false },

                            C_CertificationFile: { editable: true },
                            C_CertificationFileId: { editable: false },

                            C_Scope: { editable: true, validation: { required: true } },
                            C_ScopeId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQGetByType=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType + '&ViewEnable=' + Provider_CompanyHSEQObject.GetViewEnable(),
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQUpsert=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQUpsert=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.CertificationId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.CertificationId + '.');
                            },
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'C_CertificationCompanyName',
                title: 'Empresa Certificadora',
                width: '190px',
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
                width: '190px',
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
                field: 'C_Scope',
                title: 'Alcance',
                width: '550px',
            }, {
                field: 'C_StartDateCertification',
                title: 'Fecha Certificación',
                width: '160px',
                format: Provider_CompanyHSEQObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                },
            }, {
                field: 'C_EndDateCertification',
                title: 'Fecha Caducidad',
                width: '160px',
                format: Provider_CompanyHSEQObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                },
            }, {
                field: 'C_CCS',
                title: '% CCS',
                width: '80px',
            }, {
                field: 'C_CertificationFile',
                title: 'Archivo Certificación',
                width: '292px',
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
                field: 'CertificationName',
                title: 'Nombre',
                width: '190px',
            }, {
                field: 'CertificationId',
                title: 'Id Interno',
                width: '78px',
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyHSEQObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyHSEQObject.ObjectId + '_ShortcutToolTipTemplate').html() },
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQGetByType=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType + '&ViewEnable=' + Provider_CompanyHSEQObject.GetViewEnable(),
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQUpsert=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQUpsert=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.CertificationId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.CertificationId + '.');
                            },
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'CH_Year',
                title: 'Año',
                width: '120px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.CH_Year != null) {
                        $.each(Provider_CompanyHSEQObject.YearOptionList, function (item, value) {
                            if (dataItem.CH_Year == value) {
                                oReturn = value;
                            }
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    $('<input required data-bind="value:' + options.field + '"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            dataSource: Provider_CompanyHSEQObject.YearOptionList,
                            dataTextField: '',
                            dataValueField: '',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'CH_PoliticsSecurity',
                title: 'Política de Seguridad, Salud y Ambiente',
                width: '292px',
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
                width: '292px',
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
                title: 'Sistema de Gestión de Seguridad y Salud en el Trabajo',
                width: '292px',
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
                width: '292px',
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
                width: '292px',
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
                width: '292px',
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
                width: '292px',
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
                width: '292px',
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
                width: '292px',
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
            }, {
                field: 'CertificationId',
                title: 'Id Interno',
                width: '78px',
            }, ],
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
                { name: 'save', text: 'Guardar datos del listado' },
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyHSEQObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyHSEQObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "CertificationId",
                        fields: {
                            CertificationId: { editable: false, nullable: true },
                            CertificationName: { editable: true },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            CA_Year: { editable: true, validation: { required: true }, type: "number" },
                            CA_YearId: { editable: false },

                            CA_ManHoursWorked: { editable: true, type: "number" },
                            CA_ManHoursWorkedId: { editable: false },

                            CA_Fatalities: { editable: true, type: "number" },
                            CA_FatalitiesId: { editable: false },

                            CA_NumberAccident: { editable: true, type: "number" },
                            CA_NumberAccidentId: { editable: false },

                            CA_NumberAccidentDisabling: { editable: true, type: "number" },
                            CA_NumberAccidentDisablingId: { editable: false },

                            CA_DaysIncapacity: { editable: true, type: "number" },
                            CA_DaysIncapacityId: { editable: false },

                            CA_CertificateAccidentARL: { editable: true },
                            CA_CertificateAccidentARLId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQGetByType=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType + '&ViewEnable=' + Provider_CompanyHSEQObject.GetViewEnable(),
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQUpsert=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.HSEQType,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se creó el registro.');
                                Provider_CompanyHSEQObject.ObtainData();
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
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
                                Message('success', 'Se editó la fila con el id ' + options.data.CertificationId + '.');
                                Provider_CompanyHSEQObject.ObtainData();
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.CertificationId + '.');
                            },
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'CA_Year',
                title: 'Año',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.CA_Year != null) {
                        $.each(Provider_CompanyHSEQObject.YearOptionList, function (item, value) {
                            if (dataItem.CA_Year == value) {
                                oReturn = value;
                            }
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    $('<input required data-bind="value:' + options.field + '"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            dataSource: Provider_CompanyHSEQObject.YearOptionList,
                            dataTextField: '',
                            dataValueField: '',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'CA_ManHoursWorked',
                title: 'Horas Hombre Trabajadas',
                width: '200px',
            }, {
                field: 'CA_Fatalities ',
                title: 'Fatalidades',
                width: '160px',
            }, {
                field: 'CA_NumberAccident',
                title: 'Total de Incidentes (excluye Accidentes Incapacitantes)',
                width: '292px',
            }, {
                field: 'CA_NumberAccidentDisabling ',
                title: 'Número de Accidentes Incapacitantes',
                width: '260px',
            }, {
                field: 'CA_DaysIncapacity',
                title: 'Días de Incapacidad',
                width: '160px',
            }, {
                field: 'CA_CertificateAccidentARL',
                title: 'Certificado de accidentalidad',
                width: '250px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.CA_CertificateAccidentARL != null && dataItem.CA_CertificateAccidentARL.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyHSEQObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyHSEQObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${FileUrl}/gi, dataItem.CA_CertificateAccidentARL);

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
                field: 'CertificationId',
                title: 'Id Interno',
                width: '78px',
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

    ObtainData: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/ProviderApi?HIHSEQGetByType=true&ProviderPublicId=' + Provider_CompanyHSEQObject.ProviderPublicId + '&HSEQType=' + Provider_CompanyHSEQObject.CompanyRiskType + "&ViewEnable=true",
            dataType: 'json',
            success: function (result) {
                if (result != null && result.length > 0) {
                    $('#lblLTIFResult').html(result[0].CR_LTIFResult);
                }
            },
            error: function (result) {
                Message('error', result);
            },
        });

    },
};

/*CompanyFinancialObject*/
var Provider_CompanyFinancialObject = {

    //init properties
    ObjectId: '',
    ProviderPublicId: '',
    FinancialType: '',
    DateFormat: '',
    ProviderOptions: new Array(),
    YearOptionList: new Array(),
    CurrentAccounts: new Array(),

    //internal process properties
    ValueAccounts: new Array(),
    FormulaAccounts: new Array(),
    ValidateFormulaAccounts: new Array(),

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
        if (vInitObject.YearOptionList != null) {
            $.each(vInitObject.YearOptionList, function (item, value) {
                Provider_CompanyFinancialObject.YearOptionList[value.Key] = value.Value;
            });
        }
    },

    RenderAsync: function () {
        if (Provider_CompanyFinancialObject.FinancialType == 501001) {
            //balance sheet
            Provider_CompanyFinancialObject.RenderBalanceSheet();
        }
        else if (Provider_CompanyFinancialObject.FinancialType == 501002) {
            //tax
            Provider_CompanyFinancialObject.RenderTaxesInfo();
        }
        else if (Provider_CompanyFinancialObject.FinancialType == 501003) {
            //income statemente
            Provider_CompanyFinancialObject.RenderIncomeStatementInfo();
        }
        else if (Provider_CompanyFinancialObject.FinancialType == 501004) {
            //bank
            Provider_CompanyFinancialObject.RenderBankInfo();
        }

        //config keyboard
        Provider_CompanyFinancialObject.ConfigKeyBoard();

        //Config Events
        Provider_CompanyFinancialObject.ConfigEvents();
    },

    ConfigKeyBoard: function () {

        //init keyboard tooltip
        $('#' + Provider_CompanyFinancialObject.ObjectId + '_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {
            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Provider_CompanyFinancialObject.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Provider_CompanyFinancialObject.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Provider_CompanyFinancialObject.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function () {

        //config grid visible enables event
        $('#' + Provider_CompanyFinancialObject.ObjectId + '_ViewEnable').change(function () {
            $('#' + Provider_CompanyFinancialObject.ObjectId).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnable: function () {

        return $('#' + Provider_CompanyFinancialObject.ObjectId + '_ViewEnable').length > 0 ? $('#' + Provider_CompanyFinancialObject.ObjectId + '_ViewEnable').is(':checked') : true;
    },

    /***************************Start balance sheet functions***********************************************/

    RenderBalanceSheet: function () {
        $('#' + Provider_CompanyFinancialObject.ObjectId).kendoGrid({
            editable: false,
            navigatable: false,
            pageable: false,
            scrollable: true,
            selectable: true,
            toolbar: [
                { name: 'create', template: '<a class="k-button" href="javascript:Provider_CompanyFinancialObject.RenderBalanceSheetDetail(null);">Nuevo</a>' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyFinancialObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyFinancialObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialGetByType=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType + '&ViewEnable=' + Provider_CompanyFinancialObject.GetViewEnable(),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            }
                        });
                    },
                },
            },
            change: function (e) {
                var selectedRows = this.select();
                for (var i = 0; i < selectedRows.length; i++) {
                    Provider_CompanyFinancialObject.RenderBalanceSheetDetail(this.dataItem(selectedRows[i]));
                }
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'SH_Year',
                title: 'Año',
                width: '180px',
            }, {
                field: 'SH_BalanceSheetFile',
                title: 'Doc soporte.',
                width: '292px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.SH_BalanceSheetFile != null && dataItem.SH_BalanceSheetFile.length > 0) {
                        oReturn = oReturn + $('#' + Provider_CompanyFinancialObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyFinancialObject.ObjectId + '_NoFile').html();
                    }
                    oReturn = oReturn.replace(/\${SH_BalanceSheetFile}/gi, dataItem.SH_BalanceSheetFile);

                    return oReturn;
                },
            }, {
                field: 'FinancialName',
                title: 'Nombre',
                width: '180px',
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
                    var oFormHtml = $('#' + Provider_CompanyFinancialObject.ObjectId + '_Template_Form').html();
                    oFormHtml = oFormHtml.replace(/\${FinancialId}/gi, oFiancialId);
                    $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').html(oFormHtml);
                    $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').hide();

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
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFileLink_' + oFiancialId).show();
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
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_SH_BalanceSheetFileLink_' + oFiancialId).hide();
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
                                    $('#' + Provider_CompanyFinancialObject.ObjectId + ' _Detail_Form_SH_BalanceSheetFileLink_' + oFiancialId).show();
                                }
                            },
                        });

                    //init formula variables
                    Provider_CompanyFinancialObject.ValueAccounts = new Array();
                    Provider_CompanyFinancialObject.FormulaAccounts = new Array();
                    Provider_CompanyFinancialObject.ValidateFormulaAccounts = new Array();

                    //init accounts object
                    Provider_CompanyFinancialObject.RenderBalanceSheetDetailAccounts($('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail_Form_Accounts_' + oFiancialId), Provider_CompanyFinancialObject.CurrentAccounts);
                    //calc formula fields
                    //Provider_CompanyFinancialObject.EvalBalanceSheetFormula();

                    $('#' + Provider_CompanyFinancialObject.ObjectId + '_Detail').fadeIn('slow');
                }
            },
            error: function (result) {
                Message('error', result);
            }
        });
    },

    RenderBalanceSheetDetailAccounts: function (container, lstAccounts) {

        if (container != null && $(container).length > 0 && lstAccounts != null && lstAccounts.length > 0) {

            $.each(lstAccounts, function (item, value) {
                if (value.AccountIsParetn) {
                    //master account

                    //get master template
                    var ParentHtml = $('#' + Provider_CompanyFinancialObject.ObjectId + '_Template_AccountParent').html();
                    ParentHtml = ParentHtml.replace(/\${AccountName}/gi, value.RelatedAccount.ItemName);
                    ParentHtml = ParentHtml.replace(/\${AccountId}/gi, value.RelatedAccount.ItemId);
                    //add parent account html
                    $(container).append(ParentHtml);
                    //render child accounts
                    Provider_CompanyFinancialObject.RenderBalanceSheetDetailAccounts($('#' + Provider_CompanyFinancialObject.ObjectId + '_AccountContent_' + value.RelatedAccount.ItemId), value.ChildBalanceSheet);
                }
                else {
                    //child account

                    //get child container
                    var ChildContainer = container;

                    //get child content
                    var ChildValueHtml = $('#' + Provider_CompanyFinancialObject.ObjectId + '_Template_AccountType_' + value.AccountType).html();
                    ChildValueHtml = ChildValueHtml.replace(/\${AccountName}/gi, value.RelatedAccount.ItemName);
                    ChildValueHtml = ChildValueHtml.replace(/\${AccountId}/gi, value.RelatedAccount.ItemId);
                    ChildValueHtml = ChildValueHtml.replace(/\${AccountUnit}/gi, value.AccountUnit);

                    if (value.ChildBalanceSheet != null && value.ChildBalanceSheet.length > 0) {

                        //render subitems

                        //get subitem template
                        var SubItemHtml = $('#' + Provider_CompanyFinancialObject.ObjectId + '_Template_AccountChild').html();
                        SubItemHtml = SubItemHtml.replace(/\${AccountName}/gi, value.RelatedAccount.ItemName);
                        SubItemHtml = SubItemHtml.replace(/\${AccountId}/gi, value.RelatedAccount.ItemId);
                        //add child account html
                        $(container).append(SubItemHtml);

                        //get subitems container
                        ChildContainer = $('#' + Provider_CompanyFinancialObject.ObjectId + '_AccountContent_' + value.RelatedAccount.ItemId);

                        //add account html content for title account types
                        if (value.AccountType == 2) {
                            //add account html content
                            $(ChildContainer).append(ChildValueHtml);
                        }

                        //render subitems accounts
                        Provider_CompanyFinancialObject.RenderBalanceSheetDetailAccounts(ChildContainer, value.ChildBalanceSheet);

                        //add account html content for value account types
                        if (value.AccountType == 0 || value.AccountType == 1) {
                            //add account html content
                            $(ChildContainer).append(ChildValueHtml);
                        }
                    }
                    else {
                        //no subitems

                        //add account html content
                        $(ChildContainer).append(ChildValueHtml);
                    }
                }

                //set account value
                if (value.AccountType == 0 || value.AccountType == 1) {
                    if (value.RelatedBalanceSheetDetail != null && value.RelatedBalanceSheetDetail.Value != null) {
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_AccountContent_Value_' + value.RelatedAccount.ItemId).val(value.RelatedBalanceSheetDetail.Value);
                    }
                    else {
                        $('#' + Provider_CompanyFinancialObject.ObjectId + '_AccountContent_Value_' + value.RelatedAccount.ItemId).val(0);
                    }
                }

                //add item to formula variables
                if (value.AccountType == 0) {
                    //formula field list
                    Provider_CompanyFinancialObject.FormulaAccounts.push(new Object({
                        AccountId: value.RelatedAccount.ItemId,
                        Control: $('#' + Provider_CompanyFinancialObject.ObjectId + '_AccountContent_Value_' + value.RelatedAccount.ItemId),
                        Formula: value.AccountFormula,
                    }));
                }

                if (value.AccountValidateFormula != null) {

                    Provider_CompanyFinancialObject.ValidateFormulaAccounts.push(new Object({
                        AccountId: value.RelatedAccount.ItemId,
                        Control: $('#' + Provider_CompanyFinancialObject.ObjectId + '_AccountContent_Value_' + value.RelatedAccount.ItemId),
                        Formula: value.AccountValidateFormula,
                    }));
                }

                //value field list
                Provider_CompanyFinancialObject.ValueAccounts[value.RelatedAccount.ItemId] = $('#' + Provider_CompanyFinancialObject.ObjectId + '_AccountContent_Value_' + value.RelatedAccount.ItemId);
            });

            //append on focus out event
            $('.' + Provider_CompanyFinancialObject.ObjectId + '_AccountContent_Value_Selector').focusout(function () {
                if ($.isNumeric($(this).val()) == false) {
                    $(this).val(0);
                }
                Provider_CompanyFinancialObject.EvalBalanceSheetFormula();
            });
        }
    },

    EvalBalanceSheetFormula: function () {

        if (Provider_CompanyFinancialObject.FormulaAccounts != null && Provider_CompanyFinancialObject.FormulaAccounts.length > 0) {

            $.each(Provider_CompanyFinancialObject.FormulaAccounts, function (item, value) {

                if (value.Formula != null && value.Formula.length > 0) {
                    //get formula exclude averange calc and spaces
                    var oFormulaToEval = value.Formula.toLowerCase().replace(/ /gi, '').replace(/prom/gi, '');

                    //get variables in formula [AccountId]
                    var olstFormulaVariables = oFormulaToEval.match(new RegExp('[\\[\\d\\]]+', 'gi'));

                    //replace all variable for values
                    $.each(olstFormulaVariables, function (item, value) {
                        //get current account id
                        var oAccountId = value.replace(/\[/gi, '').replace(/\]/gi, '');

                        if (Provider_CompanyFinancialObject.ValueAccounts[oAccountId] != null && Provider_CompanyFinancialObject.ValueAccounts[oAccountId].length > 0) {
                            //replace input value into formula expression
                            oFormulaToEval = oFormulaToEval.replace(new RegExp('\\[' + oAccountId + '\\]', 'gi'), Provider_CompanyFinancialObject.ValueAccounts[oAccountId].val());
                        }
                    });
                    //eval formula and show in input value

                    var oResult = eval(oFormulaToEval);

                    if ($.isNumeric(oResult)) {
                        value.Control.val(oResult);
                    }
                    else {
                        value.Control.val('0');
                    }
                }
            });
        }
    },

    ValidateBalanceSheetDetail: function () {
        var oReturn = true;

        if (Provider_CompanyFinancialObject.ValidateFormulaAccounts != null && Provider_CompanyFinancialObject.ValidateFormulaAccounts.length > 0) {

            $.each(Provider_CompanyFinancialObject.ValidateFormulaAccounts, function (item, value) {

                if (value.Formula != null && value.Formula.length > 0) {
                    //get formula exclude averange calc and spaces
                    var oFormulaToEval = value.Formula.toLowerCase().replace(/ /gi, '').replace(/prom/gi, '');

                    //get variables in formula [AccountId]
                    var olstFormulaVariables = oFormulaToEval.match(new RegExp('[\\[\\d\\]]+', 'gi'));

                    //replace all variable for values
                    $.each(olstFormulaVariables, function (item, value) {
                        //get current account id
                        var oAccountId = value.replace(/\[/gi, '').replace(/\]/gi, '');

                        if (Provider_CompanyFinancialObject.ValueAccounts[oAccountId] != null && Provider_CompanyFinancialObject.ValueAccounts[oAccountId].length > 0) {
                            //replace input value into formula expression
                            oFormulaToEval = oFormulaToEval.replace(new RegExp('\\[' + oAccountId + '\\]', 'gi'), Provider_CompanyFinancialObject.ValueAccounts[oAccountId].val());
                        }
                    });
                    //eval formula and show in input value
                    var oResult = eval(oFormulaToEval);

                    if (oResult == false) {
                        oReturn = false;
                    }
                }
            });
        }

        return oReturn;
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
        else {
            Message('error', 'El balance no cumple con las validaciones, por favor revise los datos.');
        }
    },

    /***************************End balance sheet functions***********************************************/

    RenderTaxesInfo: function () {
        $('#' + Provider_CompanyFinancialObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyFinancialObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyFinancialObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: 'FinancialId',
                        fields: {
                            FinancialId: { editable: false, nullable: true },
                            FinancialName: { editable: true },
                            Enable: { editable: true, type: 'boolean', defaultValue: true },

                            TX_Year: { editable: true, validation: { required: true }, type: "number" },
                            TX_YearId: { editable: false },

                            TX_TaxFile: { editable: true },
                            TX_TaxFileId: { edtiable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialGetByType=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType + '&ViewEnable=' + Provider_CompanyFinancialObject.GetViewEnable(),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialUpsert=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType,
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
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialUpsert=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.FinancialId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.FinancialId + '.');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'TX_Year',
                title: 'Año',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.TX_Year != null) {
                        $.each(Provider_CompanyFinancialObject.YearOptionList, function (item, value) {
                            if (dataItem.TX_Year == value) {
                                oReturn = value;
                            }
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    $('<input required data-bind="value:' + options.field + '"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            dataSource: Provider_CompanyFinancialObject.YearOptionList,
                            dataTextField: '',
                            dataValueField: '',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'TX_TaxFile',
                title: 'Impuesto',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.TX_TaxFile != null && dataItem.TX_TaxFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyFinancialObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyFinancialObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${Url_File}/gi, dataItem.TX_TaxFile);

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
                field: 'FinancialId',
                title: 'Id Interno',
                width: '78px',
            }],
        });
    },

    RenderIncomeStatementInfo: function () {
        $('#' + Provider_CompanyFinancialObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar' },
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyFinancialObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyFinancialObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: 'FinancialId',
                        fields: {
                            FinancialId: { editable: false, nullable: true },
                            FinancialName: { editable: true },
                            Enable: { editable: true, type: 'boolean', defaultValue: true },

                            IS_Year: { editable: true },
                            IS_YearId: { editable: false },

                            IS_GrossIncome: { editable: true },
                            IS_GrossIncomeId: { edtiable: false },

                            IS_NetIncome: { editable: true },
                            IS_NetIncomeId: { editable: false },

                            IS_GrossEstate: { editable: true },
                            IS_GrossEstateId: { editable: false },

                            IS_LiquidHeritage: { editable: true },
                            IS_LiquidHeritageId: { editable: false },

                            IS_FileIncomeStatement: { editable: true },
                            IS_FileIncomeStatementId: { editable: false },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialGetByType=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType + '&ViewEnable=' + Provider_CompanyFinancialObject.GetViewEnable(),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialUpsert=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType,
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
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialUpsert=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.FinancialId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.FinancialId + '.');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'IS_Year',
                title: 'Año',
                width: '80px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.IS_Year != null) {
                        $.each(Provider_CompanyFinancialObject.YearOptionList, function (item, value) {
                            if (dataItem.IS_Year == value) {
                                oReturn = value;
                            }
                        });
                    }
                    return oReturn;
                },
                editor: function (container, options) {
                    $('<input required data-bind="value:' + options.field + '"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            dataSource: Provider_CompanyFinancialObject.YearOptionList,
                            dataTextField: '',
                            dataValueField: '',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'IS_GrossIncome',
                title: 'Ingresos Brutos',
                width: '160px',
            }, {
                field: 'IS_NetIncome',
                title: 'Ingresos Netos',
                width: '160px',
            }, {
                field: 'IS_GrossEstate',
                title: 'Patrimonio Bruto',
                width: '160px',
            }, {
                field: 'IS_LiquidHeritage',
                title: 'Patrimonio Líquido',
                width: '160px',
            }, {
                field: 'IS_FileIncomeStatement',
                title: 'Declaración de Renta',
                width: '292px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.IS_FileIncomeStatement != null && dataItem.IS_FileIncomeStatement.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyFinancialObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyFinancialObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${Url_File}/gi, dataItem.IS_FileIncomeStatement);

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
                field: 'FinancialId',
                title: 'Id Interno',
                width: '78px',
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_CompanyFinancialObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_CompanyFinancialObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: 'FinancialId',
                        fields: {
                            FinancialId: { editable: false, nullable: true },
                            FinancialName: { editable: true },
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialGetByType=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType + '&ViewEnable=' + Provider_CompanyFinancialObject.GetViewEnable(),
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', result);
                            }
                        });
                    },
                    create: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialUpsert=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType,
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
                            }
                        });
                    },
                    update: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?FIFinancialUpsert=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId + '&FinancialType=' + Provider_CompanyFinancialObject.FinancialType,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.FinancialId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.FinancialId + '.');
                            }
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'IB_BankName',
                title: 'Banco',
                width: '190px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.IB_BankName != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.IB_BankName;
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
                            options.model['IB_Bank'] = selectedItem.ItemId;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: "json",
                            serverFiltering: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByBank=true&SearchParam=' + options.data.filter.filters[0].value,
                                        dataType: 'json',
                                        success: function (result) {
                                            if (result != null) {
                                                if (result.length > 0) {
                                                    options.success(result);
                                                }
                                            }
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
                field: 'IB_AccountType',
                title: 'Tipo de Cuenta',
                width: '150px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.IB_AccountTypeId != null) {
                        $.each(Provider_CompanyFinancialObject.ProviderOptions[1001], function (item, value) {
                            if (dataItem.IB_AccountType == value.ItemId) {
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
                            dataSource: Provider_CompanyFinancialObject.ProviderOptions[1001],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'IB_AccountNumber',
                title: 'Número de Cuenta',
                width: '180px',
            }, {
                field: 'IB_AccountHolder',
                title: 'Titular de la Cuenta',
                width: '200px',
            }, {
                field: 'IB_ABA',
                title: 'ABA',
                width: '120px'
            }, {
                field: 'IB_Swift',
                title: 'SWIFT',
                width: '120px',
            }, {
                field: 'IB_IBAN',
                title: 'IBAN',
                width: '120px',
            }, {
                field: 'IB_Customer',
                title: 'Comprador',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.IB_Customer != null) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        else {
                            oReturn = '';
                        }
                        oReturn = oReturn + dataItem.IB_Customer;
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
                        dataTextField: 'CP_Customer',
                        select: function (e) {
                            isSelected = true;
                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name
                            options.model[options.field] = selectedItem.CP_CustomerPublicId;
                            options.model['IB_Customer'] = selectedItem.CP_Customer;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: 'json',
                            serverFiltering: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: BaseUrl.ApiUrl + '/ProviderApi?GetAllCustomers=true&ProviderPublicId=' + Provider_CompanyFinancialObject.ProviderPublicId,
                                        dataType: 'json',
                                        success: function (result) {
                                            options.success(result);
                                        },
                                        error: function (result) {
                                            options.error(result);
                                            Message('error', result);
                                        }
                                    });
                                },
                            }
                        }
                    });
                },
            }, {
                field: 'IB_AccountFile',
                title: 'Certificado',
                width: '292px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem != null && dataItem.IB_AccountFile != null && dataItem.IB_AccountFile.length > 0) {
                        if (dataItem.dirty != null && dataItem.dirty == true) {
                            oReturn = '<span class="k-dirty"></span>';
                        }
                        oReturn = oReturn + $('#' + Provider_CompanyFinancialObject.ObjectId + '_File').html();
                    }
                    else {
                        oReturn = $('#' + Provider_CompanyFinancialObject.ObjectId + '_NoFile').html();
                    }

                    oReturn = oReturn.replace(/\${Url_File}/gi, dataItem.IB_AccountFile);

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
                field: 'FinancialId',
                title: 'Id Interno',
                width: '78px',
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

        //config keyboard
        Provider_LegalInfoObject.ConfigKeyBoard();

        //Config Events
        Provider_LegalInfoObject.ConfigEvents();
    },

    ConfigKeyBoard: function () {

        //init keyboard tooltip
        $('#' + Provider_LegalInfoObject.ObjectId + '_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {
            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + Provider_LegalInfoObject.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + Provider_LegalInfoObject.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + Provider_LegalInfoObject.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function () {

        //config grid visible enables event
        $('#' + Provider_LegalInfoObject.ObjectId + '_ViewEnable').change(function () {
            $('#' + Provider_LegalInfoObject.ObjectId).data('kendoGrid').dataSource.read();
        });
    },

    GetViewEnable: function () {
        return $('#' + Provider_LegalInfoObject.ObjectId + '_ViewEnable').length > 0 ? $('#' + Provider_LegalInfoObject.ObjectId + '_ViewEnable').is(':checked') : true;
    },

    RenderChaimberOfComerce: function () {
        $('#' + Provider_LegalInfoObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
            scrollable: true,
            toolbar: [
                { name: 'create', text: 'Nuevo' },
                { name: 'save', text: 'Guardar datos del listado' },
                { name: 'cancel', text: 'Descartar cambios' },
                { name: 'ViewEnable', template: $('#' + Provider_LegalInfoObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_LegalInfoObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "LegalId",
                        fields: {
                            LegalId: { editable: false, nullable: true },
                            LegalName: { editable: true },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            CD_PartnerName: { editable: true, validation: { required: true } },
                            CD_PartnerNameId: { editable: false },

                            CD_PartnerIdentificationNumber: { editable: true, validation: { required: true } },
                            CD_PartnerIdentificationNumberId: { editable: false },

                            CD_PartnerRank: { editable: true, validation: { required: true } },
                            CD_PartnerRankId: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoGetByType=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&ViewEnable=' + Provider_LegalInfoObject.GetViewEnable(),
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.LegalId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.LegalId + '.');
                            },
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'CD_PartnerName',
                title: 'Nombre',
                width: '180px',
                template: function (dataItem) {
                    var oReturn = '';
                    if (dataItem.CD_PartnerName == undefined || dataItem.CD_PartnerName == '') {
                        if (dataItem.CP_PartnerName == '') {
                            oReturn = '<label class="PlaceHolder">NOMBRES Y APELLIDOS</label>';
                        }
                    }
                    else {
                        oReturn = dataItem.CD_PartnerName;
                    }

                    return oReturn;
                },
            }, {
                field: 'CD_PartnerIdentificationNumber',
                title: 'Número de Identificación',
                width: '180px',
            }, {
                field: 'CD_PartnerRank',
                title: 'Cargo',
                width: '200px',
                template: function (dataItem) {
                    var oReturn = 'Seleccione una opción.';
                    if (dataItem != null && dataItem.CD_PartnerRank != null) {
                        $.each(Provider_LegalInfoObject.ChaimberOfComerceOptionList[219], function (item, value) {
                            if (dataItem.CD_PartnerRank == value.ItemId) {
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
                            dataSource: Provider_LegalInfoObject.ChaimberOfComerceOptionList[219],
                            dataTextField: 'ItemName',
                            dataValueField: 'ItemId',
                            optionLabel: 'Seleccione una opción'
                        });
                },
            }, {
                field: 'LegalId',
                title: 'Id Interno',
                width: '78px',
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_LegalInfoObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_LegalInfoObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "LegalId",
                        fields: {
                            LegalId: { editable: false, nullable: true },
                            LegalName: { editable: true },
                            Enable: { editable: true, type: "boolean", defaultValue: true },

                            R_PersonType: { editable: true, validation: { required: true } },
                            R_PersonTypeId: { editable: false },

                            R_LargeContributor: { editable: true, type: "boolean", defaultValue: true },
                            R_LargeContributorId: { editable: false },

                            R_LargeContributorReceipt: { editable: true },
                            R_LargeContributorReceiptId: { editable: false },

                            R_LargeContributorDate: { editable: true },
                            R_LargeContributorDateId: { editable: false },

                            R_SelfRetainer: { editable: true, type: "boolean", defaultValue: true },
                            R_SelfRetainerId: { editable: false },

                            R_SelfRetainerReciept: { editable: true },
                            R_SelfRetainerRecieptId: { editable: false },

                            R_SelfRetainerDate: { editable: true },
                            R_SelfRetainerDateId: { editable: false },

                            R_EntityType: { editable: true, validation: { required: true } },
                            R_EntityTypeId: { editable: false },

                            R_IVA: { editable: true, type: "boolean", defaultValue: true },
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoGetByType=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&ViewEnable=' + Provider_LegalInfoObject.GetViewEnable(),
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.LegalId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.LegalId + '.');
                            },
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'R_PersonType',
                title: 'Tipo de Persona',
                width: '180px',
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
                width: '190px',
            }, {
                field: 'R_LargeContributorReceipt',
                title: 'Gran contribuyente resolución',
                width: '190px',
            }, {
                field: 'R_LargeContributorDate',
                title: 'Gran Contribuyente Fecha',
                width: '190px',
                format: Provider_LegalInfoObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                },
            }, {
                field: 'R_SelfRetainer',
                title: 'Autorretenedor',
                width: '120px',
            }, {
                field: 'R_SelfRetainerReciept',
                title: 'Autorretenedor Resolucion',
                width: '160px',
            }, {
                field: 'R_SelfRetainerDate',
                title: 'Autorretenedor Fecha',
                width: '160px',
                format: Provider_LegalInfoObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                },
            }, {
                field: 'R_EntityType',
                title: 'Tipo de Entidad',
                width: '190px',
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
                width: '80px',
            }, {
                field: 'R_TaxPayerType',
                title: 'Tipo de Régimen',
                width: '190px',
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
                        dataTextField: 'I_ICACode',
                        select: function (e) {
                            var selectedItem = this.dataItem(e.item.index());
                            //set server fiel name
                            options.model[options.field] = selectedItem.I_ICA;
                            options.model['R_ICAName'] = selectedItem.I_ICAId;
                            //enable made changes
                            options.model.dirty = true;
                        },
                        dataSource: {
                            type: 'json',
                            serverFiltering: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: BaseUrl.ApiUrl + '/UtilApi?CategorySearchByICA=true&SearchParam=' + options.data.filter.filters[0].value + '&PageNumber=0&RowCount=0',
                                        dataType: 'json',
                                        success: function (result) {
                                            if (result.length > 0) {
                                                options.success(result);
                                            }
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
                width: '292px',
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
                width: '292px',
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
                width: '292px',
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
                field: 'LegalName',
                title: 'Nombre',
                width: '200px',
            }, ],
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_LegalInfoObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_LegalInfoObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "LegalId",
                        fields: {
                            LegalId: { editable: false, nullable: true },
                            LegalName: { editable: true },
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoGetByType=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&ViewEnable=' + Provider_LegalInfoObject.GetViewEnable(),
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.LegalId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.LegalId + '.');
                            },
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'CF_QueryDate',
                title: 'Fecha de Consulta',
                width: '160px',
                format: Provider_LegalInfoObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                },
            }, {
                field: 'CF_ResultQuery',
                title: 'Resultado de la Consulta',
                width: '200px',
            }, {
                field: 'CF_AutorizationFile',
                title: 'Archivo de Autorización',
                width: '292px',
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
                field: 'LegalId',
                title: 'Id Interno',
                width: '78px',
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_LegalInfoObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_LegalInfoObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "LegalId",
                        fields: {
                            LegalId: { editable: false, nullable: true },
                            LegalName: { editable: true },
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoGetByType=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&ViewEnable=' + Provider_LegalInfoObject.GetViewEnable(),
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.LegalId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.LegalId + '.');
                            },
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
            }, {
                field: 'SF_ProcessDate',
                title: 'Fecha de Diligenciamiento',
                width: '200px',
                format: Provider_LegalInfoObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
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
                field: 'LegalId',
                title: 'Id Interno',
                width: '78px',
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
                { name: 'cancel', text: 'Descartar' },
                { name: 'ViewEnable', template: $('#' + Provider_LegalInfoObject.ObjectId + '_ViewEnablesTemplate').html() },
                { name: 'ShortcutToolTip', template: $('#' + Provider_LegalInfoObject.ObjectId + '_ShortcutToolTipTemplate').html() },
            ],
            dataSource: {
                schema: {
                    model: {
                        id: "LegalId",
                        fields: {
                            LegalId: { editable: false, nullable: true },
                            LegalName: { editable: true },
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoGetByType=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&ViewEnable=' + Provider_LegalInfoObject.GetViewEnable(),
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
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
                            url: BaseUrl.ApiUrl + '/ProviderApi?LILegalInfoUpsert=true&ProviderPublicId=' + Provider_LegalInfoObject.ProviderPublicId + '&LegalInfoType=' + Provider_LegalInfoObject.LegalInfoType + '&LegalId=' + Provider_LegalInfoObject.LegalId,
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                Message('success', 'Se editó la fila con el id ' + options.data.LegalId + '.');
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.LegalId + '.');
                            },
                        });
                    },
                },
            },
            columns: [{
                field: 'Enable',
                title: 'Visible en Market Place',
                width: '155px',
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
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                },
            }, {
                field: 'RS_EndDate',
                title: 'Fecha Final',
                width: '200px',
                format: Provider_LegalInfoObject.DateFormat,
                editor: function timeEditor(container, options) {
                    var input = $('<input type="date" name="'
                        + options.field
                        + '" value="'
                        + options.model.get(options.field)
                        + '" />');
                    input.appendTo(container);
                },
            }, {
                field: 'RS_Description',
                title: 'Alcance',
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
                field: 'LegalId',
                title: 'Id Interno',
                width: '78px',
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

var Provider_CustomerInfoObject = {

    ObjectId: '',
    ProviderPublicId: '',
    ProviderCustomerInfoType: '',
    ProviderOptions: new Array(),

    Init: function (vInitiObject) {
        this.ObjectId = vInitiObject.ObjectId;
        this.ProviderPublicId = vInitiObject.ProviderPublicId;
        this.ProviderCustomerInfoType = vInitiObject.ProviderCustomerInfoType;
        $.each(vInitiObject.ProviderOptions, function (item, value) {
            Provider_CustomerInfoObject.ProviderOptions[value.Key] = value.Value;
        });
    },

    RenderAsync: function () {
        if (Provider_CustomerInfoObject.ProviderCustomerInfoType == 901001) {
            Provider_CustomerInfoObject.RenderCustomerByProvider();
        }
    },

    RenderCustomerByProvider: function () {
        $('#' + Provider_CustomerInfoObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: false,
            pageable: false,
            scrollable: true,
            selectable: true,
            toolbar: [
                { name: 'create_customer', template: '<a class="k-button" href="javascript:Provider_CustomerInfoObject.CreateCustomerByProviderStatus();">Agregar Comprador</a>' },
                { name: 'create_tracking', template: '<a class="k-button" href="javascript:Provider_CustomerInfoObject.CreateCustomerByProviderTracking(null);">Agregar Seguimiento</a>' },
            ],
            dataSource: {
                schema: {
                    model: {
                        fields: {
                            CP_CustomerProviderId: { editable: false },
                            CP_CustomerPublicId: { editable: false },
                            CP_Customer: { editable: false },
                            CP_Status: { editable: false },
                            CP_Enable: { editable: true, type: "boolean" },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?CPCustomerProviderStatus=true&ProviderPublicId=' + Provider_CustomerInfoObject.ProviderPublicId + '&vCustomerRelated=1&vAddCustomer=0',
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
                },
            },
            change: function (e) {
                var selectedRows = this.select();
                for (var i = 0; i < selectedRows.length; i++) {
                    Provider_CustomerInfoObject.RenderCustomerByProviderDetail(this.dataItem(selectedRows[i]).CP_CustomerProviderId);
                }
            },
            columns: [{
                field: 'CP_Customer',
                title: 'Comprador',
                width: '100px',
            }, {
                field: 'CP_Status',
                title: 'Estado',
                width: '100px',
            }, {
                field: 'CP_CustomerPublicId',
                title: 'Id Comprador',
                width: '100px',
            }, {
                field: 'CP_Enable',
                title: 'Asociado',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.CP_Enable == true) {
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

    RenderCustomerByProviderDetail: function (oData) {
        $('#' + Provider_CustomerInfoObject.ObjectId + '_Detail').kendoGrid({
            editable: false,
            navigatable: false,
            pageable: false,
            scrollable: true,
            selectable: true,
            dataSource: {
                schema: {
                    model: {
                        fields: {
                            CPI_CustomerProviderInfoId: { editable: false },
                            CPI_TrackingType: { editable: false },
                            CPI_Tracking: { editable: false },
                            CPI_LastModify: { editable: false },
                            CPI_Enable: { editable: false },
                        },
                    }
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/ProviderApi?CPCustomerProviderInfo=true&CustomerProviderId=' + oData,
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
                },
            },
            change: function (e) {
                var selectedRows = this.select();
                for (var i = 0; i < selectedRows.length; i++) {
                    Provider_CustomerInfoObject.CreateCustomerByProviderTracking(this.dataItem(selectedRows[i]));
                }
            },
            columns: [{
                field: 'CPI_TrackingType',
                title: 'Tipo de Seguimiento',
                width: '100px',
            }, {
                field: 'CPI_Tracking',
                title: 'Seguimiento',
                width: '100px',
            }, {
                field: 'CPI_LastModify',
                title: 'Fecha de Edición',
                width: '100px',
            }, {
                field: 'CPI_CustomerProviderInfoId',
                title: 'Id',
                width: '50px',
            }],
        });
    },

    CreateCustomerByProviderStatus: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/ProviderApi?CPCustomerProviderStatus=true&ProviderPublicId=' + Provider_CustomerInfoObject.ProviderPublicId + '&vCustomerRelated=0&vAddCustomer=1',
            dataType: "json",
            type: "POST",
            success: function (result) {
                $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Dialog').html('');
                for (var i = 0; i < result.length; i++) {
                    if (result[i].CP_Enable == 'True') {
                        $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Dialog').append('<li class="CompanyCheck"><input id="' + result[i].CP_CustomerPublicId + '" type="checkbox" checked /></li>')
                    }
                    else {
                        $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Dialog').append('<li class="CompanyCheck"><input id="' + result[i].CP_CustomerPublicId + '" type="checkbox" /></li>')
                    }
                    $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Dialog').append('<li class="Company">' + result[i].CP_Customer + '</li>')
                    $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Dialog').append('<li><input id="PublicId" type="hidden" value="' + result[i].CP_CustomerPublicId + '" /></li>')
                }
            },
            error: function (result) {
                options.error(result);
            }
        });
        $('#' + Provider_CustomerInfoObject.ObjectId + '_Dialog').dialog();
    },

    UpsertCustomerByProvider: function () {

        var oCompanyPublicList = new Array();
        var oEnable = 0;
        $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Dialog input:checked').each(function () {
            oCompanyPublicList.push($(this).attr('id'));
        });

        //update
        $.ajax({
            url: BaseUrl.ApiUrl + '/ProviderApi?UpsertCustomerByProvider=true&oProviderPublicId=' + Provider_CustomerInfoObject.ProviderPublicId + '&oCompanyPublicList=' + oCompanyPublicList + '&oEnable=1',
            dataType: "json",
            type: "POST",
            success: function (result) {
                Message('success', 'Se creó el registro.');

                $('#' + Provider_CustomerInfoObject.ObjectId + '_Dialog').dialog("close");

                Provider_CustomerInfoObject.RenderCustomerByProvider();
            },
            error: function (result) {
            }
        });
    },

    CreateCustomerByProviderTracking: function (TrackingInfo) {

        $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Tracking').text('');
        $('#' + Provider_CustomerInfoObject.ObjectId + '_Internal_Tracking').text('');
        $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_Tracking').text('');

        $.ajax({
            url: BaseUrl.ApiUrl + '/ProviderApi?CPCustomerProviderStatus=true&ProviderPublicId=' + Provider_CustomerInfoObject.ProviderPublicId + '&vCustomerRelated=1&vAddCustomer=0',
            dataType: "json",
            type: "POST",
            success: function (result) {
                $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Tracking').text('');
                for (var i = 0; i < result.length; i++) {
                    $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Tracking').append('<li class="CompanyCheck"><input id="' + result[i].CP_CustomerPublicId + '" type="checkbox" /></li>')
                    $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Tracking').append('<li class="Company">' + result[i].CP_Customer + '</li>')
                    $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Tracking').append('<li><input id="PublicId" type="hidden" value="' + result[i].CP_CustomerPublicId + '" /></li>')
                }
                return result;
            },
            error: function (result) {
                $('#' + Provider_CustomerInfoObject.ObjectId + '_Internal_Tracking').text('');
                $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_Tracking').text('');
            }
        });

        if (TrackingInfo != null) {
            if (TrackingInfo.CPI_TrackingType != null && TrackingInfo.CPI_TrackingType == "Seguimientos internos") {
                $('#' + Provider_CustomerInfoObject.ObjectId + '_Internal_Tracking').append(TrackingInfo.CPI_Tracking)
            }
            else if (TrackingInfo.CPI_TrackingType != null && TrackingInfo.CPI_TrackingType == "Seguimientos del comprador") {
                $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_Tracking').append(TrackingInfo.CPI_Tracking)
            }
        }

        $('#' + Provider_CustomerInfoObject.ObjectId + '_Tracking_Dialog').dialog();
    },

    UpsertCustomerByProviderTraking: function () {

        var oCompanyPublicList = new Array();
        $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Tracking input:checked').each(function () {
            oCompanyPublicList.push($(this).attr('id'));
        });
        var oCustomers = $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_List_Tracking').val();
        var oInternalTracking = $('#' + Provider_CustomerInfoObject.ObjectId + '_Internal_Tracking').val();
        var oExternalTracking = $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_Tracking').val();
        var oStatusId = $('#' + Provider_CustomerInfoObject.ObjectId + '_Status').val();

        //update
        $.ajax({
            url: BaseUrl.ApiUrl + '/ProviderApi?UpsertCustomerInfoByProvider=true&oProviderPublicId=' + Provider_CustomerInfoObject.ProviderPublicId + '&oCompanyPublicList=' + oCompanyPublicList + '&oStatusId=' + oStatusId + '&oInternalTracking=' + oInternalTracking + '&oExternalTracking=' + oExternalTracking,
            dataType: "json",
            type: "POST",
            success: function (result) {
                Message('success', 'Se creó el registro.');

                $('#' + Provider_CustomerInfoObject.ObjectId + '_Tracking_Dialog').dialog("close");

                $('#' + Provider_CustomerInfoObject.ObjectId + '_Internal_Tracking').val('');
                $('#' + Provider_CustomerInfoObject.ObjectId + '_Customer_Tracking').val('');

                Provider_CustomerInfoObject.RenderCustomerByProvider();
            },
            error: function (result) {
                Message('error', result);
            }
        });
    },
}



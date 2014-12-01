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
    CompanyContactTypeData: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.ProviderPublicId = vInitObject.ProviderPublicId;
        this.ContactType = vInitObject.ContactType;
        this.CompanyContactTypeData = vInitObject.CompanyContactTypeData;
    },

    RenderAsync: function () {
        $('#' + Provider_CompanyContactObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: true,
            pageable: false,
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
                            CC_CompanyContactTypeId: { editable: false },
                            CC_CompanyContactType: { editable: true },
                            CC_ValueId: { editable: false },
                            CC_Value: { editable: true },
                            Enable: { editable: true, type: "boolean", defaultValue: true }
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
                        $.each(Provider_CompanyContactObject.CompanyContactTypeData, function (item, value) {
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
                            dataSource: Provider_CompanyContactObject.CompanyContactTypeData,
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
};

function checkOffset() {
    if ($('.POBOProviderActions').offset().top + $('.POBOProviderActions').height()
                >= $('#POBORenderFooter').offset().top - 10)
        $('.POBOProviderActions').css('position', 'absolute');
    if ($(document).scrollTop() + window.innerHeight < $('#POBORenderFooter').offset().top)
        $('.POBOProviderActions').css('position', 'fixed');
    $('.POBOProviderActions').text($(document).scrollTop() + window.innerHeight);
}

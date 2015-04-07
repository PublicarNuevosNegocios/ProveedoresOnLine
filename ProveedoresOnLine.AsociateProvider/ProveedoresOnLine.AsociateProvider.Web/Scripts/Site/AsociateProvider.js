var AsociateProviderObject = {
    ObjectId: '',
    PageSize: '',
    
    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.PageSize = vInitObject.PageSize;
    },

    RenderAsync: function () {
        AsociateProviderObject.RenderAsociateProvider();
    },

    RenderAsociateProvider: function (param) {

        if (param == true) {
            var vSearchParam = $('#SearchBoxId').val();
        }
        else {
            var vSearchParam = '';
        }

        $('#' + AsociateProviderObject.ObjectId).kendoGrid({
            editable: true,
            navigatable: false,
            pageable: true,
            scrollable: true,
            toolbar: [
               { name: 'save', text: 'Guardar' },
               { name: 'cancel', text: 'Descartar' },
               { name: "SearchBox", template: "<input id='SearchBoxId' type='text'value=''>" },
               { name: "SearchButton", template: "<a id='Buscar' href='javascript: AsociateProviderObject.RenderAsociateProvider(" + "true" + ");'>Buscar</a" }
            ],
            dataSource: {
                pageSize: AsociateProviderObject.PageSize,
                serverPaging: true,
                schema: {
                    total: function (data) {
                        if (data && data.length > 0) {
                            return data[0].TotalRows;
                        }
                        return 0;
                    },
                    model: {
                        id: "AP_AsociateProviderId",
                        fields: {
                            AP_AsociateProviderId: { editable: false, nullable: true },
                            AP_BO_ProviderPublicId: { editable: false },
                            AP_DM_ProviderPublicId: { editable: false },
                            AP_BO_ProviderName: { editable: false },
                            AP_Email: { editable: true, validation: { required: false, email: true } },
                            AP_LastModify: { editable: false },
                        },
                    },
                },
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/AsociateProviderApi?GetAllAsociateProvider=true&SearchParam=' + vSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                            dataType: 'json',
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
                            url: BaseUrl.ApiUrl + '/AsociateProviderApi?AsociateProviderUpsert=true',
                            dataType: 'json',
                            type: 'post',
                            data: {
                                DataToUpsert: kendo.stringify(options.data)
                            },
                            success: function (result) {
                                options.success(result);
                                $('#' + AsociateProviderObject.ObjectId).data('kendoGrid').dataSource.read();
                            },
                            error: function (result) {
                                options.error(result);
                            }
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
            columns: [{
                field: 'AP_BO_ProviderPublicId',
                title: 'Id Back Office',
            }, {
                field: 'AP_DM_ProviderPublicId',
                title: 'Id Document Management',
            }, {
                field: 'AP_BO_ProviderName',
                title: 'Proveedor',
            }, {
                field: 'AP_Email',
                title: 'Certificador',
            }, {
                field: 'AP_LastModify',
                title: 'Fecha de Edición',
            }, {
                field: 'AP_AsociateProviderId',
                title: 'Id',
                width: '78px',
            }],
        });
    },
}

/*Message*/
function Message(style, msjText) {
    if ($('div.message').length) {
        $('div.message').remove();
    }

    var mess = '';

    if (msjText != null) {
        mess = msjText;
    }
    else if (style == 'error') {
        mess = 'Hay un error!';
    } else {
        mess = 'Operación exitosa.';
    }

    $('<div class="message m_' + style + '">' + mess + '</div>').css({
        top: $(window).scrollTop() + 'px'
    }).appendTo('body').slideDown(200).delay(3000).fadeOut(300, function () {
        $(this).remove();
    });
}
var AsociateProviderObject = {
    ObjectId: '',
    PageSize: '',
    
    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.PageSize = vInitObject.PageSize;
    },

    RenderAsync: function () {
        AsociateProviderObject.RenderAsociateProvider();

        AsociateProviderObject.ConfigKeyBoard();

        AsociateProviderObject.ConfigEvents();
    },

    ConfigKeyBoard: function () {

        //init keyboard tooltip
        $('.divGrid_kbtooltip').tooltip();

        $(document.body).keydown(function (e) {
            if (e.altKey && e.shiftKey && e.keyCode == 71) {
                //alt+shift+g

                //save
                $('#' + AsociateProviderObject.ObjectId).data("kendoGrid").saveChanges();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 78) {
                //alt+shift+n

                //new field
                $('#' + AsociateProviderObject.ObjectId).data("kendoGrid").addRow();
            }
            else if (e.altKey && e.shiftKey && e.keyCode == 68) {
                //alt+shift+d

                //new field
                $('#' + AsociateProviderObject.ObjectId).data("kendoGrid").cancelChanges();
            }
        });
    },

    ConfigEvents: function () {
        //config grid visible enables event
        $('#' + AsociateProviderObject.ObjectId + '_ViewEnable').change(function () {
            $('#' + AsociateProviderObject.ObjectId).data('kendoGrid').dataSource.read();
        });
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
               { name: 'create', text: 'Nuevo' },
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
                            return data[0].AllTotalRows;
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
                            AP_Email: { editable: true },
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
                                Message('error', '');
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
                                Message('success', 'Se editó la fila con el id ' + options.data.AP_AsociateProviderId + '.');
                                $('#' + AsociateProviderObject.ObjectId).data('kendoGrid').dataSource.read();
                            },
                            error: function (result) {
                                options.error(result);
                                Message('error', 'Error en la fila con el id ' + options.data.AP_AsociateProviderId + '.');
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
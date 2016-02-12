/*Init survey program object*/
var Third_KnowledgeSimpleSearchObject = {
    ObjectId: '',
    Url: '',
    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.Url = vInitObject.Url
    },

    SimpleSearch: function () {
        debugger;
        Third_KnowledgeSimpleSearchObject.Loading_Generic_Show();
        debugger;
        if ($('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Form').length > 0) {
            $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').html('')
            var validator = $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_EditProjectDialog_Form').data("kendoValidator");

            $.ajax({
                type: "POST",
                url: $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Form').attr('action'),
                data: $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Form').serialize(),
                success: function (result) {
                    Third_KnowledgeSimpleSearchObject.Loading_Generic_Hidden();
                    debugger;
                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').html('')
                    var tittlestDiv = '';
                    var resultDiv = '';
                    if (result.RelatedSingleSearch != null && result.RelatedSingleSearch.length > 0) {
                        $.each(result.RelatedSingleSearch, function (item, value) {
                            resultDiv = '';

                            tittlestDiv = '<div class="col-sm-1 col-lg-1 POMPProviderBoxInfo"><strong>Prioridad</strong></div>'
                                        + '<div class="col-sm-1 col-lg-1 POMPProviderBoxInfo"><strong>Estado</strong></div>'
                                        + '<div class="col-sm-4 col-lg-4 POMPProviderBoxInfo"><strong>Nombre</strong></div>'
                                        + '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo"><strong>No. Identificación</strong></div>'
                                        + '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo"><strong>Alias</strong></div>'
                                        + '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo"></div>';

                            resultDiv += '<div class="row text-center">';
                            resultDiv += '<div id="POMPSSResultName" class="col-xs-12 text-left"  style="padding-top:7px"><strong>' + value.m_Item1 + '</strong></div>'
                            resultDiv += '</div>';
                            resultDiv += '<div class="conatiner-fluid POMPTKDetailContainer">';
                            resultDiv += '<div class="row text-center">';
                            resultDiv += '<br/>';
                            resultDiv += tittlestDiv;
                            resultDiv += '</div>';
                            resultDiv += '</div>';
                            resultDiv += '<br/>';
                            resultDiv += '<div class="conatiner-fluid POMPTKDetailContainer">';
                            $.each(value.m_Item2, function (item, value) {
                                resultDiv += '<div class="row text-center">';
                                if (value.Priority != null) {
                                    resultDiv += '<div class="col-sm-1 col-lg-1">' + value.Priority + '</div>';
                                }
                                if (value.Status != null) {
                                    var statusName = "";
                                    if (value.Status == "True") {
                                        statusName = "Activo";
                                    }
                                    else {
                                        statusName = "Inactivo";
                                    }
                                    resultDiv += '<div class="col-sm-1 col-lg-1 POMPProviderBoxInfo">' + statusName + '</div>';
                                }

                                if (value.NameResult != null) {
                                    resultDiv += '<div class="col-sm-4 col-lg-4 POMPProviderBoxInfo">' + value.NameResult + '</div>';
                                }

                                if (value.IdentificationResult != null) {
                                    resultDiv += '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo">' + value.IdentificationResult + '</div>';
                                }

                                if (value.Alias != null) {
                                    resultDiv += '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo">' + value.Alias + '</div>';
                                }

                                if (value.QueryBasicPublicId != null) {
                                    resultDiv += '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo">' + '<a target = "_blank" href="' + Third_KnowledgeSimpleSearchObject.Url + '?QueryBasicPublicId=' + value.QueryBasicPublicId + '&ReturnUrl=null">' + "Ver Detalle" + '</a>' + '</div>';
                                }
                                resultDiv += '</div>';
                                resultDiv += '<div class="row text-center">';
                                resultDiv += '<hr class="Tk-DetailSingleSearchSeparator"/>';
                                resultDiv += '</div>';
                            });
                            resultDiv += '</div><br/><br/>';

                            $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').append(resultDiv);
                        });
                    }
                    else {
                        resultDiv = '<div class="row"><div class="col-md-11 col-sm-11 text-center"><br/><br/><br/><label>LA BÚSQUEDA</label><br/><label class="POMPNoresultText">no arrojó coincidencias</label></div></div>',
                        $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').append(resultDiv);
                    }

                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Queries').html('');
                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Queries').append(result.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel[0].TotalQueries);

                },
                error: function (result) {
                    Third_KnowledgeSimpleSearchObject.Loading_Generic_Hidden();
                },
            })
        }
    },

    Loading_Generic_Show: function () {
        kendo.ui.progress($("#loading"), true);
    },
    Loading_Generic_Hidden: function () {
        kendo.ui.progress($("#loading"), false);
    },
};

var Third_KnowledgeMasiveSearchObject = {
    ObjectId: '',
    CompanyPublicId: '',
    PeriodPublicId: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId
        this.CompanyPublicId = vInitObject.CompanyPublicId
        this.PeriodPublicId = vInitObject.PeriodPublicId,
        Third_KnowledgeMasiveSearchObject.LoadFile();
    },

    LoadFile: function () {
        $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_FileUpload').kendoUpload({
            multiple: false,
            localization: {
                "select": "Selecionar Archivos"
            },
            async: {
                saveUrl: BaseUrl.ApiUrl + '/ThirdKnowledgeApi?TKLoadFile=true&CompanyPublicId=' + Third_KnowledgeMasiveSearchObject.CompanyPublicId + '&PeriodPublicId=' + Third_KnowledgeMasiveSearchObject.PeriodPublicId,
                autoUpload: true
            },
            success: function (e) {
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_MessageResult').html('');
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_MessageResult').append(e.response.LoadMessage);
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_Queries').html('');
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_Queries').append(e.response.AdditionalInfo);

                Header_NewNotification();
            },
            error: function (e) {
            }
        });
    },
};

var Third_KnowledgeSearchObject = {
    ObjectId: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
    },

    RenderAsync: function () {
        //Change event
        $('#' + Third_KnowledgeSearchObject.ObjectId + '_FilterId').click(function () {
            Third_KnowledgeSearchObject.Search(null);
        });
    },

    Search: function (vSearchObject) {
        var oUrl = this.SearchUrl;
        oUrl += '?InitDate=' + $('#' + Third_KnowledgeSearchObject.ObjectId + '_InitDateId').val();
        oUrl += '&EndDate=' + $('#' + Third_KnowledgeSearchObject.ObjectId + '_EndDateId').val();

        if (vSearchObject != null && vSearchObject.PageNumber != null) {
            oUrl += '&PageNumber=' + vSearchObject.PageNumber;
        }
        window.location = oUrl;
    },
};

var Third_KnowledgeDetailSearch = {
    QueryPublicId: ''
    , PageNumber: 0
    , RowCount: 0
    , InitDate: ''
    , EndDate: ''
    , Enable: ''
    , IsSuccess: ''


    , Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;

        this.QueryPublicId = vInitObject.QueryPublicId;
        this.InitDate = vInitObject.InitDate;
        this.EndDate = vInitObject.EndDate;
        this.PageNumber = vInitObject.PageNumber;
        this.RowCount = vInitObject.RowCount;

        this.Enable = vInitObject.Enable;
        this.IsSuccess = vInitObject.IsSuccess;

    },
    RenderAsync: function () {
        //Change event
        $('#' + Third_KnowledgeDetailSearch.ObjectId + '_FilterId').click(function () {
            Third_KnowledgeDetailSearch.Search(vInitObject);
        });
    },
    Search: function (vSearchObject) {
        $.ajax({
            url: BaseUrl.ApiUrl + '/ThirdKnowledgeApi?TKThirdKnowledgeDetail=true&QueryPublicId=' + vSearchObject.QueryPublicId + '&InitDate=' + vSearchObject.InitDate + '&EndDate=' + vSearchObject.EndDate + '&Enable=' + 1 + '&IsSuccess=' + 'no' + '&PageNumber=' + vSearchObject.PageNumber,
            dataType: 'json',
            success: function (result) {
                if (result != null) {
                    var InitDate = result.RelatedThidKnowledgeSearch.InitDate;
                    var EndDate = result.RelatedThidKnowledgeSearch.EndDate;
                    debugger;
                    $("#TKDetailContainer").empty();
                    result.Group.forEach(function (group) {

                        if (group.m_Item1 == "SIN COINCIDENCIAS") {
                            var titulo = '<div class="row"><div class="col-sm-12 col-lg-12 POMPTKDetailTitle"><strong>' + group.m_Item1 + '</strong></div></div>';
                            $("#TKDetailContainer").append(titulo);
                            var content_title =
                                     '<div class="row">'
                                    + '<div class="col-sm-4 POMPProviderBoxInfo text-left"><strong>Nombre Consultado</strong></div>'
                                    + '<div class="col-sm-4 POMPProviderBoxInfo text-left"><strong>Identificación Consultada</strong></div>'
                                    + '</div><br/>';
                            $("#TKDetailContainer").append(content_title);

                            group.m_Item2.forEach(function (content) {
                                debugger;
                                var content_info = '<div class="row POMPBorderbottom">';
                                $.each(content.DetailInfo, function (item, value) {
                                    debugger;
                                    if (value.Value != "SIN COINCIDENCIAS") {
                                        content_info += '<div class="col-sm-4 POMPProviderBoxInfo text-left"><p>' + value.Value + '</p></div>';
                                    }                                   
                                });
                                content_info += '<div class="col-sm-2 POMPProviderBoxInfo text-center"><p>' +
                                                '<a href="' + BaseUrl.SiteUrl + '/ThirdKnowledge/TKDetailSingleSearch?QueryBasicPublicId=' + content.QueryBasicPublicId +
                                                '&ReturnUrl='+
                                                '&InitDate=' + InitDate +
                                                '&EndDate=' + EndDate +
                                                '">Ver Detalle</a></p></div>';
                                content_info += '</div><br />';
                                $("#TKDetailContainer").append(content_info);
                            });
                        }
                        else {
                            var titulo = '<div class="row"><div class="col-sm-12 col-lg-12 POMPTKDetailTitle"><strong>' + group.m_Item1 + '</strong></div></div>';
                            $("#TKDetailContainer").append(titulo);
                            var content_title = '<br /><div class="row"><div class="col-sm-1 POMPProviderBoxInfo text-center"><strong>Prioridad</strong></div><div class="col-sm-1 POMPProviderBoxInfo text-center"><strong>Estado</strong></div><div class="col-sm-4 POMPProviderBoxInfo text-center"><strong>Nombre</strong></div><div class="col-sm-2 POMPProviderBoxInfo text-center"><strong>Identificación</strong></div><div class="col-sm-2 POMPProviderBoxInfo text-center"><strong>Alias</strong></div></div><br />';
                            $("#TKDetailContainer").append(content_title);
                            group.m_Item2.forEach(function (content) {
                                var status = "Inactivo";
                                if (content.Status == "True") {
                                    status = "Activo";
                                }
                                var content_info = '<div class="row POMPBorderbottom">' +
                                                   '<div class="col-sm-1 POMPProviderBoxInfo text-center"><p>' + content.Priority + '</p></div>' +
                                                   '<div class="col-sm-1 POMPProviderBoxInfo text-center"><p>' + status + '</p></div>' +
                                                   '<div class="col-sm-4 POMPProviderBoxInfo text-center"><p>' + content.NameResult + '</p></div>' +
                                                   '<div class="col-sm-2 POMPProviderBoxInfo text-center"><p>' + content.IdentificationResult + '</p></div>' +
                                                   '<div class="col-sm-2 POMPProviderBoxInfo text-center"><p>' + content.Alias + '</p></div>' +
                                                   '<div class="col-sm-2 POMPProviderBoxInfo text-center"><p>' + 
                                                   '<a href="/ThirdKnowledge/TKDetailSingleSearch?QueryBasicPublicId=' + content.QueryPublicId +
                                                   '&ReturnUrl='+
                                                   '&InitDate=' + InitDate +
                                                   '&EndDate=' + EndDate +
                                                   '">Ver Detalle</a></p></div>' +
                                                   '</div><br />';
                                $("#TKDetailContainer").append(content_info);
                            });
                        }
                    });
                }
            },
            error: function (result) {
            }
        });
    },
}

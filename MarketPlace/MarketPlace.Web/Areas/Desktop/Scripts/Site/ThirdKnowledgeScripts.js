/*Init survey program object*/
var Third_KnowledgeSimpleSearchObject = {
    ObjectId: '',
    Url:'',
    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.Url = vInitObject.Url
    },

    SimpleSearch: function () {

        Third_KnowledgeSimpleSearchObject.Loading_Generic_Show();
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
                            debugger;
                            resultDiv = '';
                            
                            tittlestDiv = '<div class="col-md-2 col-lg-2"><strong>Prioridad</strong></div><div class="col-md-2 col-lg-2"><strong>Estado</strong></div><div class="col-md-2 col-lg-2"><strong>Nombre</strong></div><div class="col-md-2 col-lg-2"><strong>Identificación</strong></div><div class="col-md-2 col-lg-2"><strong>Alias</strong></div><div class="col-md-2 col-lg-2"></div>';
                            resultDiv += '<div class="row text-center">';
                                resultDiv += '<div id="POMPResultName" class="text-left"><p>' + value.m_Item1 + '</p></div>'
                            resultDiv += '</div>';
                            resultDiv += '<div class="row text-center">';
                                resultDiv += '<br/>';
                                resultDiv += tittlestDiv;
                            resultDiv += '</div>';
                            resultDiv += '<br/>';
                            $.each(value.m_Item2, function (item, value) {
                                debugger;
                                resultDiv += '<div class="row text-center">';
                                    if (value.Priority != null) {
                                        resultDiv += '<div class="col-md-2 col-lg-2">' + value.Priority + '</div>';
                                    }
                                    if (value.Status != null) {
                                        var statusName = "";
                                        if (value.Status == "True") {
                                            statusName = "Activo";
                                        }
                                        else {
                                            statusName = "Inactivo";
                                        }
                                        resultDiv += '<div class="col-md-2 col-lg-2">' + statusName + '</div>';
                                    }

                                    if (value.NameResult != null) {
                                        resultDiv += '<div class="col-md-2 col-lg-2">' + value.NameResult + '</div>';
                                    }
                                    
                                    if (value.IdentificationResult != null) {
                                        resultDiv += '<div class="col-md-2 col-lg-2">' + value.IdentificationResult + '</div>';
                                    }

                                    if (value.Alias != null) {
                                        resultDiv += '<div class="col-md-2 col-lg-2">' + value.Alias + '</div>';
                                    }

                                    if (value.QueryBasicPublicId != null) {
                                        resultDiv += '<div class="col-md-2 col-lg-2">' + '<a target = "_blank" href="' + Third_KnowledgeSimpleSearchObject.Url + '?QueryBasicPublicId=' + value.QueryBasicPublicId + '&ReturnUrl=null">' + "Ver Detalle" + '</a>' + '</div>';
                                    }
                                    resultDiv += '</div>';
                                    resultDiv += '<div class="row text-center">';
                                        resultDiv += '<hr class="Tk-DetailSingleSearchSeparator"/>';
                                    resultDiv += '</div>';
                            });
                            resultDiv += '<br/><br/>';
                                                        
                            $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').append(resultDiv);
                        });                        
                    }
                    else {
                        resultDiv = '<div class="POMPResultSection text-center"><label>' + "La búsqueda no arrojó ninguna coincidencia " + "</label>"
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


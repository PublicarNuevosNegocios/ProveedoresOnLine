/*Init survey program object*/
var Third_KnowledgeSimpleSearchObject = {
    ObjectId: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
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
                    debugger;
                    Third_KnowledgeSimpleSearchObject.Loading_Generic_Hidden();

                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').html('')
                    var resultDiv = '';
                    if (result.RelatedThidKnowledgeSearch.CollumnsResult != null && result.RelatedThidKnowledgeSearch.CollumnsResult.RelatedQueryBasicInfoModel.length > 0) {
                        $.each(result.RelatedThidKnowledgeSearch.CollumnsResult.RelatedQueryBasicInfoModel, function (item, value) {                            
                            resultDiv = 
                             '<div class="POMPContainerResult"><div id="POMPResultName"><p>' + value.NameResult + '</p><a target = "_blank" href="' + '/ThirdKnowledge/TKDetailSingleSearch?QueryBasicPublicId=' + value.QueryBasicPublicId + '">' + "Ver Detalle" + '</a></div>'
                            if (value.IdentificationResult != null) {
                                resultDiv += '<div class="POMPResultSection"><label>' + "Número de Identificación " + "</label><p>" + value.IdentificationResult + '</p></div>'
                            }
                            if (value.Alias != null) {
                                resultDiv += '<div class="POMPResultSection"><label>' + "Alias " + "</label><p>" + value.Alias + '</p></div>'
                            }
                            if (value.Offense != null) {
                                resultDiv += '<div class="POMPResultSection"><label>' + "Cargo o Delito " + "</label><p>" + value.Offense + '</p></div>'
                            }                            
                            if (value.Peps != null) {
                                debugger;

                                resultDiv += '<div class="POMPResultSection"><label>' + "Peps " + "</label><p>" + value.Peps + '</p></div>'
                            }
                            if (value.Priority != null) {
                                resultDiv += '<div class="POMPResultSection"><label>' + "Prioridad " + "</label><p>" + value.Priority + '</p></div>'
                            }
                            if (value.Status != null) {
                                
                                var statusName = "";

                                if (value.Status == "True") {
                                    statusName = "Activo";
                                }
                                else {
                                    statusName = "Inactivo";
                                }

                                resultDiv += '<div class="POMPResultSection"><label>' + "Estado " + "</label><p>" + statusName + '</p></div>'
                            }                            
                                                        
                            $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').append(resultDiv);
                        });                        
                    }
                    else {
                        resultDiv = '<div class="POMPResultSection"><label>' + "La búsqueda no arrojó ninguna coincidencia " + "</label>"
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
                debugger;
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_MessageResult').html('');
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_MessageResult').append(e.response.LoadMessage);
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_Queries').html('');
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_Queries').append(e.response.AdditionalInfo);
                debugger;
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


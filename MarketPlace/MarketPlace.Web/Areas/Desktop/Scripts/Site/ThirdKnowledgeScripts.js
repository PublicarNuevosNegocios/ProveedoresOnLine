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

            //save project
            $.ajax({
                type: "POST",
                url: $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Form').attr('action'),
                data: $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Form').serialize(),
                success: function (result) {
                    Third_KnowledgeSimpleSearchObject.Loading_Generic_Hidden();

                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').html('')
                    var resultDiv = '';
                    if (result.RelatedThirdKnowledge != null && result.RelatedThirdKnowledge.CollumnsResult != null) {
                        {
                            debugger;
                            $.each(result.RelatedThirdKnowledge.CollumnsResult, function (item, value) {
                                if (item != 0 && item != 1) {
                                    resultDiv = '<div class="POMPContainerResult"><div id="POMPResultName"><p>' + value[4] + '</p></div>' +
                                     '<div class="POMPResultSection"><p>' + value[3] + '</p></div>' +
                                     '<div class="POMPResultSection"><p>' + value[1] + '</p></div>' +
                                     '<div class="POMPResultSection"><p>' + value[6] + '</p></div></div>'
                                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').append(resultDiv);
                                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Queries').html('');
                                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Queries').append(result.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel[0].TotalQueries);
                                }
                            });
                        }
                    }
                },
                error: function (result) {
                    Third_KnowledgeSimpleSearchObject.Loading_Generic_Hidden();

                }
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
                "select": "Agregar"
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


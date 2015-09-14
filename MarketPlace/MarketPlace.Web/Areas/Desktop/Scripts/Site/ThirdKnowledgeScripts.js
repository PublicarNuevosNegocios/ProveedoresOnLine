/*Init survey program object*/
var Third_KnowledgeSimpleSearchObject = {
    ObjectId: 'ThirdKnowledge',

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
                        debugger;
                        if (result.RelatedThirdKnowledge.CollumnsResult.length == 1) {
                            resultDiv = '<div class=""><p>' + result.RelatedThirdKnowledge.CollumnsResult[0][0] + '</p></div>';
                            $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').append(resultDiv);
                            $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Queries').html('');
                            $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Queries').append(result.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel[0].TotalQueries);
                        }
                        else {
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


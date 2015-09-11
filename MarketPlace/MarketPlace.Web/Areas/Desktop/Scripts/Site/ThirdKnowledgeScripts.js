/*Init survey program object*/
var Third_KnowledgeSimpleSearchObject = {
    ObjectId: 'ThirdKnowledge',   

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;        
    },

    SimpleSearch: function () {
        debugger;
        if ($('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Form').length > 0) {

            var validator = $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_EditProjectDialog_Form').data("kendoValidator");
           
                //save project
            $.ajax({
                type: "POST",
                url: $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Form').attr('action'),
                data: $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Form').serialize(),
                success: function (result) {
                    debugger;
                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').html('')
                    var resultDiv = '';
                    if (result.RelatedThirdKnowledge != null && result.RelatedThirdKnowledge.CollumnsResult != null) {                        
                        $.each(result.RelatedThirdKnowledge.CollumnsResult, function (item, value) {
                            debugger;                            
                            if (item != 0 && item != 1) {
                                resultDiv = '<div class="POMPContainerResult"><div id="POMPResultName"><p>' + value[4] + '</p></div>' +
                                 '<div class="POMPResultSection"><p>' + value[3] + '</p></div>' +                                
                                 '<div class="POMPResultSection"><p>' + value[1] + '</p></div>' +                                
                                 '<div class="POMPResultSection"><p>' + value[6] + '</p></div></div>'
                                $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').append(resultDiv);
                            }                        
                    });
                    }                    
                },
                error: function (result) {
                    debugger;
                    //Dialog_ShowMessage('Proceso de selección', 'Se ha actualizado el proceso de selección correctamente.', Third_KnowledgeSimpleSearchObject.ProjectRecalculateUrl);
                    //window.location = Third_KnowledgeSimpleSearchObject.ProjectRecalculateUrl;
                    //$(this).dialog('close');
                }
            })            
        }
    }
};


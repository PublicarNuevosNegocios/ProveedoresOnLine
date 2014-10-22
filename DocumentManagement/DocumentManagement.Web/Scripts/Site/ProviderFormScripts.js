//postback form
function PF_PostBackForm(vidForm, NewStepId) {
    var strUrl = $("#" + vidForm).attr('action').replace(/{{NewStepId}}/, NewStepId);
    $("#" + vidForm).attr('action', strUrl);
    $("#" + vidForm).submit();
}

//init spiner
function PF_InitSpinner(vidDiv) {
    $("#" + vidDiv).spinner();
}


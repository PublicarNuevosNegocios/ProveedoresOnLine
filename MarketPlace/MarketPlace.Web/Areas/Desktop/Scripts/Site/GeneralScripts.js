/*init base url*/
var BaseUrl = {   
    ApiUrl: '',
    SiteUrl: '',
    PreviewPdfUrl: '',
    Init: function (vInitObject) {
        this.ApiUrl = vInitObject.ApiUrl;
        this.SiteUrl = vInitObject.SiteUrl;
        this.PreviewPdfUrl = vInitObject.PreviewPdfUrl;
    },
};

/*show hide user menu*/
function Header_ShowHideUserMenu(divId) {
    $('#' + divId).toggle('slow');
}

/*Show notifications*/
function Header_ShowHideNotifications(DivId, User, companyPublicId) {
    $('#' + DivId).text('');
    $.ajax({
        url: BaseUrl.ApiUrl + '/CompanyApi?NGetNotifications=true&User=' + User + '&CompanyPublicId=' + companyPublicId + '&Enable=true',
        dataType: 'json',
        success: function (result) {
            if (result != null) {
                $('#' + DivId).text('');
                for (var i = 0; i < result.length; i++) {
                    debugger;
                    $('#' + DivId).append('<div><a href="' + result[i].Url + '">' + result[i].Label + '</a></div>');
                }
            }
        },
        error: function (result) {
            $('#' + DivId).text('');
        }
    });

    $('#' + DivId).toggle('slow');
}

function Header_NewNotification() {
    $('#OldNotification').hide();
    $('#NewNotification').show();
}

function Header_OldNotification() {
    $('#OldNotification').show();
    $('#NewNotification').hide();
}

/*init generic tooltip*/
function Tooltip_InitGeneric() {
    $('.SelGenericTooltip').tooltip();
}

/*show process message*/
function Dialog_ShowMessage(vTitle, vMessage, vRedirectUrl) {
    var DialogDiv = $('#Generic_MessageDialog').html();
    DialogDiv = DialogDiv.replace(/\${Title}/gi, vTitle);
    DialogDiv = DialogDiv.replace(/\${Message}/gi, vMessage);
    $(DialogDiv).dialog({
        width: 300,
        modal: true,
        closeOnEscape: false,
        open: function (event, ui) { $(".ui-dialog-titlebar-close", ui.dialog || ui).hide(); },
        buttons: {
            'Cerrar': function () {
                $(this).dialog("close");
                if (vRedirectUrl != null && vRedirectUrl.length > 0) {
                    window.location = vRedirectUrl;
                }
            },
        },
    });
}

/*show process message*/
function Dialog_CreatedPS(vTitle, vMessage, vRedirectUrl) {
    var DialogDiv = $('#Generic_MessageDialog').html();
    DialogDiv = DialogDiv.replace(/\${Title}/gi, vTitle);
    DialogDiv = DialogDiv.replace(/\${Message}/gi, vMessage);
    $(DialogDiv).dialog({
        width: 300,
        modal: true,
        closeOnEscape: false,
        open: function (event, ui) { $(".ui-dialog-titlebar-close", ui.dialog || ui).hide(); },
        buttons: {
            'Iniciar proceso de selección': function () {
                $(this).dialog("close");
                if (vRedirectUrl != null && vRedirectUrl.length > 0) {
                    window.location = vRedirectUrl;
                }
            },
        },
    });
}

/*show generic progressbar*/
function ProgressBar_Generic_Show() {
    $('.selProgressBar').kendoProgressBar({
        type: 'percent',
        animation: {
            duration: 600
        },
    });

    $.each($('.selProgressBar'), function (item, value) {
        $(value).data("kendoProgressBar").value($(value).attr('data-value'));
    });
}

//date validation
function dateValidation(date_a, date_b) {
    var auxfec1 = date_a;
    var auxfec2 = date_b;
    if (auxfec1 > auxfec2) {
        return (false);
    }
    if (auxfec1 == auxfec2) {
        return (true);
    }
    return (true);
}
//show modal
function showModal(response) {
    if (response != null) {
        $("#msg").html(response);
        $("#modal").show().kendoWindow({
            modal: true,
            title: "Evaluación."
        }).data("kendoWindow").center().open();
    }
}
//date validation
$('#Survey_ProgramSurvey_StartDate').focusout(function () {
    if ($('#Survey_ProgramSurvey_StartDate').val() != '' && $('#Survey_ProgramSurvey_EndDate').val() != '') {
        if (dateValidation($('#Survey_ProgramSurvey_StartDate').val(), $('#Survey_ProgramSurvey_EndDate').val())) {
        }
        else {
            $('#Survey_ProgramSurvey_StartDate').focus();
            $('#Survey_ProgramSurvey_StartDate').val($('#Survey_ProgramSurvey_EndDate').val());
            Dialog_ShowMessage("Evaluación", "La fecha inicial no debe ser superior a la fecha final.", '');
        }
    }
});
$('#Survey_ProgramSurvey_EndDate').focusout(function () {
    if ($('#Survey_ProgramSurvey_StartDate').val() != '' && $('#Survey_ProgramSurvey_EndDate').val() != '') {
        if (dateValidation($('#Survey_ProgramSurvey_StartDate').val(), $('#Survey_ProgramSurvey_EndDate').val())) {
        }
        else {
            $('#Survey_ProgramSurvey_EndDate').focus();
            $('#Survey_ProgramSurvey_StartDate').val($('#Survey_ProgramSurvey_EndDate').val());
            Dialog_ShowMessage("Evaluación", "La fecha inicial no debe ser superior a la fecha final.", '');
        }
    }
});

$(document).ready(function () {
    var position = 'expanded';

    $(".POMPManualLink").click(function () {
        if (position == 'expanded') {
            $(this).animate({ height: '148px' });
            position = 'collapsed';
        } else {
            $(this).animate({ height: '90px' });
            position = 'expanded';
        }
    });
});
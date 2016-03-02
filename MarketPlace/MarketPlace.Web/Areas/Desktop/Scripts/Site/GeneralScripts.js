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
            if (result != null && result.length > 0) {
                $.each(result, function (item, value) {
                    $('#' + DivId).append('<li class="row list-group-item text-left POMP_NotifyItemList"><i class="glyphicon glyphicon-info-sign POMP_NotifyItemListIcon"></i><a onclick="javascript: DeleteNotifications(' + value.NotificationId + ', \'' + value.Url + '\')">' + value.Label + '</a></li>');
                });
            }
            else {
                $('#' + DivId).text('');
                $('#' + DivId).append('<div class="POMPNotificationsList"><li class="row list-group-item text-left POMP_NotifyItemList"><i class="glyphicon glyphicon-thumbs-up POMP_NotifyItemListIcon"></i>&nbsp;&nbsp;<span>Usted no tiene notificaciones</span></li></div>');
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

function DeleteNotifications(NotificationId, UrlRedirect) {
    if (NotificationId > 0) {
        $.ajax({
            url: BaseUrl.ApiUrl + '/CompanyApi?NDeleteNotifications=true&NotificationId=' + NotificationId,
            dataType: 'json',
            success: function (result) {},
            error: function (result) {}
        });

        window.location.replace(UrlRedirect);
    }
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
    $("#POMPManualTittle").click(function () {
        if (position == 'collapsed') {
            $("#POMPManuals").animate({ bottom: '-=45px' });
            position = 'expanded';
        } else {
            $("#POMPManuals").animate({ bottom: '+=45px' });

            position = 'collapsed';
        }
    });
});

var ReportViewerObj = {    
    RenderReportViewer: function (vInitObject) {
        debugger;
        var Form = '';
        var cmbToAppend = '<select name= "'+ vInitObject.ObjectId +'_cmbFormat">';        
        $.each(vInitObject.Options, function (item, value)
        {
            debugger;
            if (value == "EXCEL" || value == "Excel" ) {
                cmbToAppend += '<option value="xlsx">' + value + '</option>';
            }
            else if (value == "Pdf" || value == "PDF") {
                cmbToAppend += ' <option value="pdf">' + value + '</option>';
            }            
        });        
        cmbToAppend += '</select>';

        cmbToAppend += '<input type="hidden" name="DownloadReport" id="DownloadReport" value="true" />';
         
        Form = $('#' + vInitObject.ObjectId + '_DialogForm').empty();
        Form = $('#' + vInitObject.ObjectId + '_DialogForm').append(cmbToAppend);

        $('#' + vInitObject.ObjectId + '_Dialog').dialog({
            tittle:vInitObject.Tittle,
            modal: true,
            buttons: {               
                'Descargar': function () {
                    $('#' + vInitObject.ObjectId + '_DialogForm').submit();
                    $(this).dialog("close");
                }
            }
        });
    }
}
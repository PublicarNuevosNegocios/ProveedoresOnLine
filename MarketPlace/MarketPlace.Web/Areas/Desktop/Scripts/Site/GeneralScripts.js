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


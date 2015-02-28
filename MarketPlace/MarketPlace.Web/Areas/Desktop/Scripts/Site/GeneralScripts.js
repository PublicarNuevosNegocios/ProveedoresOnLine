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
function Tooltip_InitGeneric()
{
    $('.SelGenericTooltip').tooltip();
}


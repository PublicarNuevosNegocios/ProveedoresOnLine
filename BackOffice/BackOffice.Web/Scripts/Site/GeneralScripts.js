/*init base url*/
var BaseUrl = {
    ApiUrl: '',
    SiteUrl: '',
    Init: function (vInitObject) {
        this.ApiUrl = vInitObject.ApiUrl;
        this.SiteUrl = vInitObject.SiteUrl;
    },
};

/*show hide user menu*/
function Header_ShowHideUserMenu(divId) {
    $('#' + divId).toggle('slow');
}

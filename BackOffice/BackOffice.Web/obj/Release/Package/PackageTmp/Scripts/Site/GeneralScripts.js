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

/*UploadFile Generic Function*/
function UploadFile(initObject) {
    var oFileExit = true;
    $('#GenericLoadFile')
    .kendoUpload({
        multiple: false,
        async: {            
            saveUrl: BaseUrl.ApiUrl + '/FileApi?FileUpload=true&CompanyPublicId=' + initObject.ProviderPublicId,
            autoUpload: true
        },
        success: function (e) {            
            if (e.response != null && e.response.length > 0) {
                //set server fiel name
                $('#' + initObject.ControlellerResponseId).val(e.response[0].ServerName);
            }
        },
        complete: function (e) {            
            //enable lost focus
            oFileExit = true;
        },
        select: function (e) {
            //disable lost focus while upload file
            oFileExit = false;
        },
    });    
};
            

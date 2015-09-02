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
/*UploadFile TemplateForm Function*/
function UploadTemplateFormFile(initObject) {
    var oFileExit = true;
    $('#TemplateFormLoadFile')
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


            
/*Message*/
function Message(style, msjText) {
    if ($('div.message').length) {
        $('div.message').remove();
    }

    var mess = '';

    if (msjText != null) {
        mess = msjText;
    }
    else if(style == 'error') {
        mess = 'Hay un error!';
    } else {
        mess = 'Operación exitosa.';
    }

    $('<div class="message m_' + style + '">' + mess + '</div>').css({
        top: $(window).scrollTop() + 'px'
    }).appendTo('body').slideDown(200).delay(3000).fadeOut(300, function () {
        $(this).remove();
    });
}
/*init base url*/
var BaseUrl = {
    ApiUrl: '',
    SiteUrl: '',

    Init: function (vInitObject) {
        this.ApiUrl = vInitObject.ApiUrl;
        this.SiteUrl = vInitObject.SiteUrl;
    },
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
    else if (style == 'error') {
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
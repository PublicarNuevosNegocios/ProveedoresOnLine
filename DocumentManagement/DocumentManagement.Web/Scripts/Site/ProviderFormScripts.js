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

//init progressbar
function PF_InitProgressBar(vidDiv, vProgress, vLabel) {
    $("#" + vidDiv).progressbar({
        value: vProgress
    });

    $("#" + vidDiv + '_Label').text(vLabel);
}

//partners object
var PF_PartnerFormObject = {

    DivId: '',
    PartnerData: new Array(),

    Init: function (vInitObject) {

        this.DivId = vInitObject.DivId;
        this.PartnerData = vInitObject.PartnerData;
    },

    //init Partners grid
    RenderAsync: function () {
        $('#' + PF_PartnerFormObject.DivId).kendoGrid({
            toolbar: [{ template: '<a href="javascript:PF_PartnerFormObject.ShowCreate();">Agregar</a>' }],
            dataSource: {
                type: 'json',
                data: PF_PartnerFormObject.PartnerData,
            },
            columns: [{
                field: 'IdentificationNumber',
                title: 'Identificación',
            }, {
                field: 'FullName',
                title: 'Nombres y apellidos'
            }, {
                field: 'ParticipationPercent',
                title: '(%)'
            }, {
                field: 'ProviderInfoId',
                title: ' ',
                template: '<a href="javascript:PF_PartnerFormObject.ShowDelete(${ProviderInfoId});">Borrar</a>'
            }]
        });
    },

    ShowCreate: function () {
        $('#' + PF_PartnerFormObject.DivId + '_Create').dialog();
    },

    Create: function () {

        var oReq = '';
        oReq = oReq + '{ProviderInfoId:0,';
        oReq = oReq + 'IdentificationNumber:"' + $('#' + PF_PartnerFormObject.DivId + '_IdentificationNumber').val() + '",';
        oReq = oReq + 'FullName:"' + $('#' + PF_PartnerFormObject.DivId + '_FullName').val() + '",';
        oReq = oReq + 'ParticipationPercent:"' + $('#' + PF_PartnerFormObject.DivId + '_ParticipationPercent').val() + '",';
        oReq = oReq + 'IsDelete:"false"}';

        $('#' + PF_PartnerFormObject.DivId + '-').val(oReq);

        PF_PostBackForm('FrmGenericStep', '');
    },

    ShowDelete: function (ProviderInfoId) {

        var oReq = '';
        oReq = oReq + '{ProviderInfoId:"' + ProviderInfoId + '",';
        oReq = oReq + 'IdentificationNumber:"",';
        oReq = oReq + 'FullName:"",';
        oReq = oReq + 'ParticipationPercent:"",';
        oReq = oReq + 'IsDelete:"true"}';

        $('#' + PF_PartnerFormObject.DivId + '-').val(oReq);
        $('#' + PF_PartnerFormObject.DivId + '-').attr('name', $('#' + PF_PartnerFormObject.DivId + '-').attr('name') + ProviderInfoId);

        PF_PostBackForm('FrmGenericStep', '');
    },
};

//init autocomplete control
function PF_InitAutocomplete(acId, acData) {
    $('#' + acId).autocomplete(
	{
	    source: acData,
	    minLength: 0,
	});
}

//partners object
var AP_ProviderNotesObject = {

    DivId: '',
    NotesData: new Array(),

    Init: function (vInitObject) {

        this.DivId = vInitObject.DivId;
        this.NotesData = vInitObject.NotesData;
    },

    //init Partners grid
    RenderAsync: function () {
        $('#' + AP_ProviderNotesObject.DivId).kendoGrid({
            //toolbar: [{ template: '<a href="javascript:AP_ProviderNotesObject.ShowCreate();">Agregar</a>' }],
            dataSource: {
                type: 'json',
                data: AP_ProviderNotesObject.PartnerData,
            },
            columns: [{
                field: 'LargeValue',
                title: 'Nota',
            }, {
                field: 'CreateDate',
                title: 'Fecha'
            }]
        });
    },
};







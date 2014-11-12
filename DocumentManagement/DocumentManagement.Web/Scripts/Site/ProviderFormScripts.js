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

var PF_ValidateFormObject = {

    FormId: '',

    Init: function (vInitObject) {
        this.FormId = vInitObject.FormId;
    },

    RenderAsync: function () {
        $('#' + PF_ValidateFormObject.FormId).validate({
            errorClass: 'error help-inline',
            validClass: 'success',
            errorElement: 'span',
            highlight: function (element, errorClass, validClass) {
                $(element).parents("div.control-group").addClass(errorClass).removeClass(validClass);

            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).parents(".error").removeClass(errorClass).addClass(validClass);
            },
        });
    },

    AddRule: function (vRuleValues) {
        $('#' + vRuleValues.idDiv).rules("add", {
            required: vRuleValues.Required,
            email: (vRuleValues.Type == 'email'),
            number: (vRuleValues.Type == 'number'),
            messages: {
                required: 'El campo es obligatorio.',
                email: 'El formato del campo no es correcto.',
                number: 'El campo solo admite valores numéricos.'
            },
        });

    },
};

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
            toolbar: [{ template: '<a class="AddMultipleFile" href="javascript:PF_PartnerFormObject.ShowCreate();">Agregar</a>' }],
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
                title: 'Porcentaje de participación (%)'
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

        if (ProviderInfoId != null) {
            $('#' + PF_PartnerFormObject.DivId + '_Delete').dialog({
                modal: true,
                buttons: {
                    "Borrar": function () {
                        //delete
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?StepDeleteVal=true&StepId=' + ProviderInfoId,
                            dataType: "json",
                            type: "POST",
                            success: function (result) {
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
                            error: function (result) {

                                alert('Se ha generado un error:' + result);
                            }
                        });
                        $(this).dialog("close");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        else {
            alert('no se puede borrar ' + ProviderInfoId);
        }
    },
};

//init autocomplete control
function PF_InitAutocomplete(acId, acData) {
    debugger;
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
                data: AP_ProviderNotesObject.NotesData,
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

//postback form
function PF_LegalTermsChange(vidInput) {

    var oReq = '';

    oReq = oReq + '{CheckData:' + $('#' + vidInput + '_CheckData').prop('checked') + ',';
    oReq = oReq + 'CheckCommercial:' + $('#' + vidInput + '_CheckCommercial').prop('checked') + ',';
    oReq = oReq + 'CheckRestrictiveList:' + $('#' + vidInput + '_CheckRestrictiveList').prop('checked') + '}';

    $("#" + vidInput).val(oReq);
}

//Multiple File
var PF_MultipleFileObject = {

    DivId: '',
    MultipleData: new Array(),

    Init: function (vInitObject) {

        this.DivId = vInitObject.DivId;
        this.MultipleData = vInitObject.MultipleData;
    },

    //init Multiple File grid
    RenderAsync: function () {
        $('#' + PF_MultipleFileObject.DivId).kendoGrid({
            toolbar: [
                {
                    template: '<input type="file" id="' + PF_MultipleFileObject.DivId + '" name="' + PF_MultipleFileObject.DivId + '" /><a class="AddMultipleFile" href="javascript:PF_MultipleFileObject.Create();">Agregar archivo</a>'
                }
            ],
            dataSource: {
                type: 'json',
                data: PF_MultipleFileObject.MultipleData,
            },
            columns: [{
                field: 'FileName',
                title: 'Archivo',
            }, {
                field: 'ProviderInfoUrl',
                title: ' ',
                template: '<a href="${ProviderInfoUrl}" target="_blank">Ver archivo</a>'
            }, {
                field: 'ProviderInfoId',
                title: ' ',
                template: '<a href="javascript:PF_MultipleFileObject.ShowDelete(${ProviderInfoId});">Borrar</a>'
            }]
        });
    },

    Create: function () {

        var oReq = '';
        oReq = oReq + '{ProviderInfoId:0,';
        oReq = oReq + 'ProviderInfoUrl:"' + $('#' + PF_MultipleFileObject.DivId).val() + '",';
        oReq = oReq + 'IsDelete:"false"}';

        $('#' + PF_MultipleFileObject.DivId + '-').val(oReq);

        PF_PostBackForm('FrmGenericStep', '');
    },

    ShowDelete: function (ProviderInfoId) {

        if (ProviderInfoId != null) {
            $('#' + PF_MultipleFileObject.DivId + '_Delete').dialog({
                modal: true,
                buttons: {
                    "Borrar": function () {
                        //delete
                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?StepDeleteVal=true&StepId=' + ProviderInfoId,
                            dataType: "json",
                            type: "POST",
                            success: function (result) {
                                var oReq = '';
                                oReq = oReq + '{IsDelete:"true",';
                                oReq = oReq + 'ProviderInfoId:"' + ProviderInfoId + '",';
                                oReq = oReq + 'ProviderInfoUrl:""}';

                                $('#' + PF_MultipleFileObject.DivId + '-').val(oReq);
                                $('#' + PF_MultipleFileObject.DivId + '-').attr('name', $('#' + PF_MultipleFileObject.DivId + '-').attr('name') + ProviderInfoId);

                                PF_PostBackForm('FrmGenericStep', '');
                            },
                            error: function (result) {

                                alert('Se ha generado un error:' + result);
                            }
                        });
                        $(this).dialog("close");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        else {
            alert('no se puede borrar ' + ProviderInfoId);
        }
    },
}

var PF_MultipleFileACObject = {
    
    DivId: '',
    MultipleData: new Array(),
    DialogId: '',
    ocData: new Array(),

    Init: function (vInitObject) {
        debugger;

        this.DivId = vInitObject.DivId;
        this.MultipleData = vInitObject.MultipleData;
        this.DialogId = vInitObject.DialogId;
        this.ocData = vInitObject.ocData;
    },

    //init Multiple File grid
    RenderAsync: function () {
        $('#' + PF_MultipleFileACObject.DivId).kendoGrid({
            toolbar: [
                {
                    template: '<a class="AddMultipleFile" href="javascript:PF_MultipleFileACObject.OpenDialog();">Agregar archivo</a>'
                }
            ],
            dataSource: {
                type: 'json',
                data: PF_MultipleFileACObject.MultipleData,
            },
            columns: [{
                field: 'FileName',
                title: 'Archivo',
            }, {
                field: 'ProviderInfoUrl',
                title: ' ',
                template: '<a href="${ProviderInfoUrl}" target="_blank">Ver archivo</a>'
            }, {
                field: 'ProviderInfoId',
                title: ' ',
                template: '<a href="javascript:PF_MultipleFileObject.ShowDelete(${ProviderInfoId});">Borrar</a>'
            }]
        });
    },

    Create: function () {

    },

    OpenDialog: function () {
        debugger;
        $('#' + this.DialogId).dialog();
        //$('#' + this.DialogId).html('');        
        //$('#' + this.DialogId).append('<li><label>Etiqueta</label></li>')
        //$('#' + this.DialogId).append('<li><input type="Text" id="' + PF_MultipleFileACObject.DivId + '" name="' + PF_MultipleFileACObject.DivId + '" /></li>')
        $('#' + this.DialogId).append('<li><input type="file" id="' + PF_MultipleFileACObject.DivId + '" name="' + PF_MultipleFileACObject.DivId + '" /></li>');
        $('#' + this.DialogId).append('<a class="" href="javascript:PF_MultipleFileACObject.Create();">Agregar</a><ul>')

        //PF_InitAutocomplete(PF_MultipleFileACObject.DivId, PF_MultipleFileACObject.ocData);
    },
}
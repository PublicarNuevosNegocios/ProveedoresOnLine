//postback form
function PF_PostBackForm(vidForm, NewStepId, IsSync) {
    
    var strUrl = $("#" + vidForm).attr('action').replace(/{{NewStepId}}/, NewStepId).replace(/{{IsSync}}/, IsSync);
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
            //required: vRuleValues.Required,
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
    IsModified: '',
    ProviderPublicId: '',

    Init: function (vInitObject) {
        
        this.DivId = vInitObject.DivId;
        this.PartnerData = vInitObject.PartnerData;
        this.IsModified = vInitObject.IsModified;
        this.ProviderPublicId = vInitObject.ProviderPublicId;
    },

    //init Partners grid
    RenderAsync: function () {
        
        var grid =
            $('#' + PF_PartnerFormObject.DivId).kendoGrid({
                toolbar: [{ template: '<a class="AddMultipleFile" href="javascript:PF_PartnerFormObject.ShowCreate();">Agregar</a>' }],
                dataSource: {
                    type: 'json',
                    data: PF_PartnerFormObject.PartnerData,
                },
                columns: [{
                    field: 'IdentificationNumber',
                    title: 'Identificación',
                    template: function (dataItem) {
                        var oReturn = '';
                        if (dataItem.IsRowModifed == true) {
                            oReturn = "<div >&nbsp; <label style='color:red'> " + dataItem.IdentificationNumber + " </label> </div>"
                        }
                        else {
                            oReturn = "<div >&nbsp; <label style='color:black'> " + dataItem.IdentificationNumber + "</label> </div>"
                        }
                        return oReturn;
                    },
                }, {
                    field: 'FullName',
                    title: 'Nombres y apellidos',
                    template: function (dataItem) {
                        var oReturn = '';
                        if (dataItem.IsRowModifed == true) {
                            oReturn = "<div >&nbsp; <label style='color:red'> " + dataItem.FullName + " </label> </div>"
                        }
                        else {
                            oReturn = "<div >&nbsp; <label style='color:black'> " + dataItem.FullName + "</label> </div>"
                        }
                        return oReturn;
                    },
                }, {
                    field: 'ParticipationPercent',
                    title: 'Porcentaje de participación (%)'
                }, {
                    field: 'ProviderInfoId',
                    title: ' ',
                    template: '<a href="javascript:PF_PartnerFormObject.ShowDelete(${ProviderInfoId});">Borrar</a>'
                }, {
                    title: 'Sincronizar',
                    template: function (dataItem) {
                        var oReturn = '';
                        if (dataItem.IsRowModifed == true) {
                            oReturn = '<a href="javascript:PF_PartnerFormObject.Sync(' + "'" + dataItem.ProviderInfoId + "'" + ',' + "'" + dataItem.IdentificationNumber + "'" + ',' + "'" + dataItem.FullName + "'" + ',' + "'" + dataItem.ParticipationPercent + "'" + ',' + "'" + PF_PartnerFormObject.ProviderPublicId + "'" + ');">Sincronizar</a>'

                        }
                        return oReturn;
                    },
                }, ]
            });
        if (this.IsModified == "True") {
            $('#' + PF_PartnerFormObject.DivId).data("kendoGrid").showColumn(4);
        }
        else {
            $('#' + PF_PartnerFormObject.DivId).data("kendoGrid").hideColumn(4);
        }
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
                            url:  + '/CustomerApi?StepDeleteVal=true&StepId=' + ProviderInfoId,
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

    Sync: function (ProviderInfoId, IdentificationNumber, FullName, ParticipationPercent, ProviderPublicId) {
        
        var oReq = '';
        oReq = oReq + '{ProviderInfoId:"' + ProviderInfoId + '",';
        oReq = oReq + 'IdentificationNumber:"' + IdentificationNumber + '",';
        oReq = oReq + 'FullName:"' + FullName + '",';
        oReq = oReq + 'ParticipationPercent:"' + ParticipationPercent + '",';
        oReq = oReq + 'IsSync:"true",';
        oReq = oReq + 'ProviderPublicId:"' + ProviderPublicId + '",';
        oReq = oReq + 'IsDelete:"false"}';

        $('#' + PF_PartnerFormObject.DivId + '-').val(oReq);

        var strUrl = BaseUrl.SiteUrl + "/ProviderForm/SyncPartnersGrid?ProviderPublicId=" + ProviderPublicId + "&IdentificationNumber=" + IdentificationNumber + "&FullName=" + FullName + "&ProviderInfoId=" + ProviderInfoId
        $("#" + "FrmGenericStep").attr('action', strUrl);

        $("#" + "FrmGenericStep").submit();
    }
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

    //oReq = oReq + '{CheckCommercial:' + $('#' + vidInput + '_CheckCommercial').prop('checked') + ',';
    oReq = oReq + '{CheckLegalTerms:' + $('#' + vidInput + '_CheckLegalTerms').prop('checked') + '}';

    $("#" + vidInput).val(oReq);
}

//Multiple File
var PF_MultipleFileObject = {

    DivId: '',
    MultipleData: new Array(),
    ACData: new Array(),
    IsModified: '',    
    ProviderPublicId: '',

    Init: function (vInitObject) {
        
        this.DivId = vInitObject.DivId;
        this.MultipleData = vInitObject.MultipleData;
        this.ACData = vInitObject.ACData;
        this.IsModified = vInitObject.IsModified == "True" ? "red" : "black";
        this.ProviderPublicId = vInitObject.ProviderPublicId;
    },

    //init Multiple File grid
    RenderAsync: function () {
        $('#' + PF_MultipleFileObject.DivId).kendoGrid({
            toolbar: [
                {
                    template: '<a class="AddMultipleFile" href="javascript:PF_MultipleFileObject.ShowCreate();">Agregar archivo</a>'
                }
            ],
            dataSource: {
                type: 'json',
                data: PF_MultipleFileObject.MultipleData,
            },
            columns: [{
                field: 'Name',
                title: 'Nombre',
                template: function (dataItem) {
                    
                    var oReturn = '';
                    if (dataItem.IsRowModifed == true) {
                        oReturn = "<div >&nbsp; <label style='color:red'> " + dataItem.Name+ "</label> </div>"
                    }
                    else {
                        oReturn = "<div >&nbsp; <label style='color:blue'> " + dataItem.Name + "</label> </div>"
                    }
                    return oReturn;
                },                
            }, {
                field: 'ProviderInfoUrl',
                title: ' ',
                template: '<a href="${ProviderInfoUrl}" " target="_blank">Ver archivo</a>'
            }, {
                field: 'ProviderInfoId',
                title: ' ',
                template: '<a href="javascript:PF_MultipleFileObject.ShowDelete(${ProviderInfoId});">Borrar</a>'
            }, {
                title: 'Sincronizar',
                template: function (dataItem) {
                    
                    var oReturn = '';
                    if (dataItem.IsRowModifed == true) {
                        oReturn = '<a href="javascript:PF_MultipleFileObject.MultilpeFileSync(' + "'" + dataItem.ProviderInfoId + "'" + ',' + "'" + dataItem.ProviderInfoUrl + "'" + ',' + "'" + dataItem.Name + "'" + ',' + "'" + PF_MultipleFileObject.ProviderPublicId + "'" + ',' + "'" + dataItem.ItemInfoType + "'" + ');">Sincronizar</a>'

                    }
                    return oReturn;
                },
            },]
        });
    },

    ShowCreate: function (ProviderInfoId) {
        $('#' + PF_MultipleFileObject.DivId + '_Create').dialog({
            dialogClass: "DialogCreate",
            modal: true,
            buttons: {
                "Crear": function () {
                    //create
                    PF_MultipleFileObject.Create();
                    $(this).dialog("close");
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });

        $('#' + PF_MultipleFileObject.DivId + '_Create').html($('#' + PF_MultipleFileObject.DivId + '_Create').html().replace(/pform/gi, 'form'));
        PF_InitAutocomplete(PF_MultipleFileObject.DivId + '--Name', this.ACData);
    },

    Create: function () {
        $('#' + PF_MultipleFileObject.DivId + '_Form').append('<input type="hidden" name="UpsertAction" id="UpsertAction" value="true" />');
        $('#' + PF_MultipleFileObject.DivId + '_Form').attr('action', $('#FrmGenericStep').attr('action'));
        PF_PostBackForm(PF_MultipleFileObject.DivId + '_Form', '');
    },

    ShowDelete: function (ProviderInfoId) {

        if (ProviderInfoId != null) {
            $('#' + PF_MultipleFileObject.DivId + '_Delete').dialog({
                dialogClass: "DialogDelete",
                modal: true,
                buttons: {
                    "Borrar": function () {
                        //delete
                        var oReq = '';
                        oReq = oReq + '{IsDelete:"true",';
                        oReq = oReq + 'ProviderInfoId:"' + ProviderInfoId + '",';
                        oReq = oReq + 'Name:"",';
                        oReq = oReq + 'ProviderInfoUrl:""}';

                        $('#' + PF_MultipleFileObject.DivId + '-').val(oReq);
                        $('#' + PF_MultipleFileObject.DivId + '-').attr('name', $('#' + PF_MultipleFileObject.DivId + '-').attr('name') + ProviderInfoId);

                        PF_PostBackForm('FrmGenericStep', '');

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

    MultilpeFileSync: function (ProviderInfoId, ProviderInfoUrl, Name, ProviderPublicId, ItemType)
    {
        var oReq = '';
        oReq = oReq + '{IsDelete:"false",';
        oReq = oReq + 'ProviderInfoId:"' + ProviderInfoId + '",';
        oReq = oReq + 'Name:"' + Name + '",';
        oReq = oReq + 'ProviderInfoUrl:"' +ProviderInfoUrl + '"}';

        $('#' + PF_MultipleFileObject.DivId + '-').val(oReq);
        $('#' + PF_MultipleFileObject.DivId + '-').attr('name', $('#' + PF_MultipleFileObject.DivId + '-').attr('name') + ProviderInfoId);

        var strUrl = BaseUrl.SiteUrl+"/ProviderForm/SyncMultipleFileGrid?ProviderPublicId=" + ProviderPublicId + "&ProviderInfoUrl=" + ProviderInfoUrl + "&Name=" + Name + "&ProviderInfoId=" + ProviderInfoId + "&ItemType=" + ItemType
        $("#" + "FrmGenericStep").attr('action', strUrl);

        $("#" + "FrmGenericStep").submit();
    }
};


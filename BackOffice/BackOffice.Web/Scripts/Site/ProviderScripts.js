/*init provider Menu*/
function Provider_InitMenu(InitObject) {
    $('#' + InitObject.ObjId).accordion({
        animate: 'swing',
        header: 'label',
        active: InitObject.active
    });
}

/*Generic provider submit form*/
function Provider_SubmitForm(SubmitObject) {
    if (SubmitObject.StepValue != null && SubmitObject.StepValue.lenght > 0 && $('#StepAction').length > 0) {
        $('#StepAction').val(SubmitObject.StepValue);
    }
    $('#' + SubmitObject.FormId).submit();
}

/*CompanyContactObject*/

var Provider_CompanyContactObject = {

    ObjectId: '',
    GridData: new Array(),

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.GridData = vInitObject.GridData;
    },

    RenderAsync: function () {
    },
};
var Project_ProjectFile = {

    ObjectId: '',
    ProjectPublicId: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.ProjectPublicId = vInitObject.ProjectPublicId;
    },

    RenderAsync: function () {
        //var oFileExit = true;
        $('#' + Project_ProjectFile.ObjectId)
        .kendoUpload({
            multiple: false,
            async: {
                saveUrl: BaseUrl.ApiUrl + '/ProjectApi?ProjectUploadFile=true&ProjectPublicId=' + Project_ProjectFile.ProjectPublicId,
                autoUpload: true
            },
            success: function (e) {
                if (e.response != null && e.response.length > 0) {
                    //set server fiel name
                    //$('#' + initObject.ControlellerResponseId).val(e.response[0].ServerName);

                    //render uploaded files
                    //$.each(e.response, function (item, value) {
                    //    var oFileItem = $('#' + Project_ProjectFile.ObjectId + '_FileItem').html();

                    //    $('#' + Project_ProjectFile.ObjectId + '_FileList').append(oFileItem);

                    //});

                    //$('.k-upload-files.k-reset').find('li').remove();
                }
            },
            complete: function (e) {
                //enable lost focus
                //oFileExit = true;
            },
            select: function (e) {
                //disable lost focus while upload file
                //oFileExit = false;
            },
        });

    },
};
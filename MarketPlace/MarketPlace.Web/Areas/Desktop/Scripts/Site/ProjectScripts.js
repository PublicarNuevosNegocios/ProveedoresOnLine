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
                    //render uploaded files
                    $.each(e.response, function (item, value) {
                        var oFileItem = $('#' + Project_ProjectFile.ObjectId + '_FileItemTemplate').html();

                        oFileItem = oFileItem.replace(/{ServerUrl}/gi, value.ServerUrl);
                        oFileItem = oFileItem.replace(/{FileName}/gi, value.FileName);
                        oFileItem = oFileItem.replace(/{FileObjectId}/gi, value.FileObjectId);

                        $('#' + Project_ProjectFile.ObjectId + '_FileList').append(oFileItem);
                    });
                    //clean file list from kendo upload
                    $('.k-upload-files.k-reset').find('li').remove();
                }
            },
        });
    },
    
    RemoveFile: function (vProjectInfoId) {
        $.ajax({
            url: BaseUrl.ApiUrl + '/ProjectApi?ProjectRemoveFile=true&ProjectPublicId=' + Project_ProjectFile.ProjectPublicId + '&ProjectInfoId=' + vProjectInfoId,
            dataType: 'json',
            success: function (result) {
                $('#' + Project_ProjectFile.ObjectId + '_File_' + vProjectInfoId).remove();
            },
            error: function (result) {
                Dialog_ShowMessage('Proceso de selección', 'Ha ocurrido un error borrando el archivo.', null);
            },
        });
    },
};
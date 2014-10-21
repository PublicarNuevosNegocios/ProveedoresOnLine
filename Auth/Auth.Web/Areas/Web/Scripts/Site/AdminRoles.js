//init Schedule Available grid
function ProfileAutorizationListGrid(vidDiv, vProfileData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#templateCreate").html() }],
        dataSource: {
            type: "json",
            data: vProfileData,
        },
        columns: [{
            field: "RoleName",
            title: "Rol",
        }, {
            field: "UserEmail",
            title: "Correo electrónico"
        }, {
            field: "ProfileRoleId",
            title: "&nbsp;",
            template: $("#templateDelete").html()
        }],
    });
}

//Valitation, Email 
function ValidEmailAutorizationProfileList(ControlId) {
    $('#' + ControlId).validate({
        //debug: true,
        errorClass: 'error help-inline',
        validClass: 'success',
        errorElement: 'span',
        highlight: function (element, errorClass, validClass) {
            $(element).parents("div.control-group").addClass(errorClass).removeClass(validClass);

        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).parents(".error").removeClass(errorClass).addClass(validClass);
        },
        rules: {
            UserEmail: {
                email: true
            }
        },
        messages: {
            UserEmail:
                {
                    required: " Debe ingresar una dirección de correo electrónico",
                    email: " Debe ingresar un email valido"
                }
        }
    });
}
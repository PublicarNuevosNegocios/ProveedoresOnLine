//init Schedule Available grid
function AutorizationListGrid(vidDiv, vAutorizationData) {

    $('#' + vidDiv).kendoGrid({
        toolbar: [{ template: $("#templateCreate").html() }],
        dataSource: {
            type: "json",
            data: vAutorizationData,
        },
        columns: [{
            field: "Application",
            title: "Aplicativo",
        }, {
            field: "Role",
            title: "Rol",
        }, {
            field: "UserEmail",
            title: "Correo electrónico"
        }, {
            field: "ApplicationRoleId",
            title: "&nbsp;",
            template: $("#templateDelete").html()
        }],
    });
}

//Valitation, Email 
function ValidateEmail(ControlId) {
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
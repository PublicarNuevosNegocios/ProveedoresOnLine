/*Init survey program object*/
var Third_KnowledgeSimpleSearchObject = {
    ObjectId: '',
    Url: '',
    ReSearch: false,
    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.Url = vInitObject.Url;
        this.ReSearch = vInitObject.ReSearch;

        if (this.ReSearch == "True") {
            //Call Research Function
            Third_KnowledgeSimpleSearchObject.SimpleSearch();
        }
    },

    SimpleSearch: function () {
        Third_KnowledgeSimpleSearchObject.Loading_Generic_Show();
        if ($('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Form').length > 0) {
            $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').html('')
            var validator = $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_EditProjectDialog_Form').data("kendoValidator");

            $.ajax({
                type: "POST",
                url: $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Form').attr('action'),
                data: $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Form').serialize(),
                success: function (result) {
                    Third_KnowledgeSimpleSearchObject.Loading_Generic_Hidden();                    
                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').html('')
                    var tittlestDiv = '';
                    var resultDiv = '';
                    //&& result.RelatedThidKnowledgeSearch.length > 0
                    if (result.RelatedThidKnowledgeSearch != null) {
                        $.each(result.RelatedThidKnowledgeSearch.CollumnsResult.RelatedQueryBasicInfoModel, function (item, value) {
                            if (value.DetailInfo[2].Value == "SIN COINCIDENCIAS") {
                                resultDiv = '';
                                resultDiv += '<div class="row">' +
                                '<div class="col-sm-12 col-lg-12 POMPTKDetailTitle"><strong>SIN COINCIDENCIAS</strong></div>' +
                                '</div>' +
                                '<br />' +
                                '<div class="row">' +
                                '<div class="col-sm-4 POMPProviderBoxInfo text-left"><strong>Nombre Consultado</strong></div>' +
                                '<div class="col-sm-4 POMPProviderBoxInfo text-left"><strong>Identificación Consultada</strong></div>' +
                                '</div><br />';
                                resultDiv += '<div class="row POMPBorderbottom">';
                                resultDiv += '<div class="col-sm-4 POMPProviderBoxInfo text-left"><p>';
                                resultDiv += value.DetailInfo[0].Value + '</p></div>';
                                resultDiv += '<div class="col-sm-4 POMPProviderBoxInfo text-left"><p>';
                                resultDiv += value.DetailInfo[1].Value + '</p></div>';
                                resultDiv += '  <div class="col-sm-4 POMPProviderBoxInfo text-right"><p>' +
                                            '<a target = "_blank" href="' + BaseUrl.SiteUrl + 'ThirdKnowledge/TKDetailSingleSearch?QueryBasicPublicId=' + value.QueryBasicPublicId +
                                            '">Ver Detalle</a>' +
                                            '</p></div></div> <br /> <br /> <br />';
                                $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').append(resultDiv);
                            }
                            else {                                
                                resultDiv = '';
                                tittlestDiv = '<div class="col-sm-1 col-lg-1 POMPProviderBoxInfo"><strong>Prioridad</strong></div>'
                                            + '<div class="col-sm-1 col-lg-1 POMPProviderBoxInfo"><strong>Estado</strong></div>'
                                            + '<div class="col-sm-4 col-lg-4 POMPProviderBoxInfo"><strong>Nombre</strong></div>'
                                            + '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo"><strong>No. Identificación</strong></div>'
                                            + '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo"><strong>Alias</strong></div>'
                                            + '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo"></div>';

                                resultDiv += '<div class="row text-center">';
                                resultDiv += '<div id="POMPSSResultName" class="col-xs-12 text-left"  style="padding-top:7px"><strong>' + value.m_Item1 + '</strong></div>'
                                resultDiv += '</div>';
                                resultDiv += '<div class="conatiner-fluid POMPTKDetailContainer">';
                                resultDiv += '<div class="row text-center">';
                                resultDiv += '<br/>';
                                resultDiv += tittlestDiv;
                                resultDiv += '</div>';
                                resultDiv += '</div>';
                                resultDiv += '<br/>';
                                resultDiv += '<div class="conatiner-fluid POMPTKDetailContainer">';
                                $.each(value.m_Item2, function (item, value) {
                                    resultDiv += '<div class="row text-center">';
                                    if (value.Priority != null) {
                                        resultDiv += '<div class="col-sm-1 col-lg-1">' + value.Priority + '</div>';
                                    }
                                    if (value.Status != null) {
                                        var statusName = "";
                                        if (value.Status == "True") {
                                            statusName = "Activo";
                                        }
                                        else {
                                            statusName = "Inactivo";
                                        }
                                        resultDiv += '<div class="col-sm-1 col-lg-1 POMPProviderBoxInfo">' + statusName + '</div>';
                                    }

                                    if (value.NameResult != null) {
                                        resultDiv += '<div class="col-sm-4 col-lg-4 POMPProviderBoxInfo">' + value.NameResult + '</div>';
                                    }

                                    if (value.IdentificationResult != null) {
                                        resultDiv += '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo">' + value.IdentificationResult + '</div>';
                                    }

                                    if (value.Alias != null) {
                                        resultDiv += '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo">' + value.Alias + '</div>';
                                    }

                                    if (value.QueryBasicPublicId != null) {
                                        resultDiv += '<div class="col-sm-2 col-lg-2 POMPProviderBoxInfo">' + '<a target = "_blank" href="' + Third_KnowledgeSimpleSearchObject.Url + '?QueryBasicPublicId=' + value.QueryBasicPublicId + '&ReturnUrl=null">' + "Ver Detalle" + '</a>' + '</div>';
                                    }
                                    resultDiv += '</div>';
                                    resultDiv += '<div class="row text-center">';
                                    resultDiv += '<hr class="Tk-DetailSingleSearchSeparator"/>';
                                    resultDiv += '</div>';
                                });
                                resultDiv += '</div><br/><br/>';

                                $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').append(resultDiv);
                            }
                        });
                    }
                    else {
                        resultDiv = '<div class="row"><div class="col-md-11 col-sm-11 text-center"><br/><br/><br/><label>LA BÚSQUEDA</label><br/><label class="POMPNoresultText">no arrojó coincidencias</label></div></div>',
                        $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_DivResult').append(resultDiv);
                    }

                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Queries').html('');
                    $('#' + Third_KnowledgeSimpleSearchObject.ObjectId + '_Queries').append(result.RelatedThirdKnowledge.CurrentPlanModel.RelatedPeriodModel[0].TotalQueries);

                },
                error: function (result) {
                    Third_KnowledgeSimpleSearchObject.Loading_Generic_Hidden();
                },
            })
        }
    },

    Loading_Generic_Show: function () {
        kendo.ui.progress($("#loading"), true);
    },

    Loading_Generic_Hidden: function () {
        kendo.ui.progress($("#loading"), false);
    },
};

var Third_KnowledgeMasiveSearchObject = {
    ObjectId: '',
    CompanyPublicId: '',
    PeriodPublicId: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId
        this.CompanyPublicId = vInitObject.CompanyPublicId
        this.PeriodPublicId = vInitObject.PeriodPublicId,
        Third_KnowledgeMasiveSearchObject.LoadFile();
    },

    LoadFile: function () {
        $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_FileUpload').kendoUpload({
            multiple: false,
            localization: {
                "select": "Selecionar Archivos"
            },
            async: {
                saveUrl: BaseUrl.ApiUrl + '/ThirdKnowledgeApi?TKLoadFile=true&CompanyPublicId=' + Third_KnowledgeMasiveSearchObject.CompanyPublicId + '&PeriodPublicId=' + Third_KnowledgeMasiveSearchObject.PeriodPublicId,
                autoUpload: true
            },
            success: function (e) {
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_MessageResult').html('');
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_MessageResult').append(e.response.LoadMessage);
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_Queries').html('');
                $('#' + Third_KnowledgeMasiveSearchObject.ObjectId + '_Queries').append(e.response.AdditionalInfo);

                Header_NewNotification();
            },
            error: function (e) {
            }
        });
    },
};

var Third_KnowledgeDetailSearch = {
    QueryPublicId: ''
    , PageNumber: 0
    , RowCount: 0
    , SearchUrl: ''
    , InitDate: ''
    , EndDate: ''
    , Enable: ''
    , IsSuccess: ''


    , Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
        this.QueryPublicId = vInitObject.QueryPublicId;
        this.InitDate = vInitObject.InitDate;
        this.EndDate = vInitObject.EndDate;
        this.PageNumber = vInitObject.PageNumber;
        this.RowCount = vInitObject.RowCount;
        this.Enable = vInitObject.Enable;
        this.IsSuccess = vInitObject.IsSuccess;

    },

    RenderAsync: function () {
        //Change event

        $('#' + Third_KnowledgeDetailSearch.ObjectId + '_FilterId').click(function () {
            Third_KnowledgeDetailSearch.Search(vInitObject);
        });
    },

    Search: function (vSearchObject) {
        var oUrl = this.SearchUrl + '?QueryPublicId=' + vSearchObject.QueryPublicId;
        oUrl += '&InitDate=' + '';
        oUrl += '&EndDate=' + '';

        if (vSearchObject != null && vSearchObject.PageNumber != null) {
            oUrl += '&PageNumber=' + vSearchObject.PageNumber;
        }
        window.location = oUrl;
    },
}

var Third_KnowledgeSearch = {
    CustomerPublicId: ''
    , PageNumber: 0
    , RowCount: 0
    , SearchUrl: ''
    , InitDate: ''
    , EndDate: ''
    , SearchType: ''
    , Status: ''

    , Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
        this.CustomerPublicId = vInitObject.CustomerPublicId;
        this.InitDate = vInitObject.InitDate;
        this.EndDate = vInitObject.EndDate;
        this.PageNumber = vInitObject.PageNumber;
        this.RowCount = vInitObject.RowCount;
        this.SearchType = vInitObject.SearchType;
        this.Status = vInitObject.Status;
    },
    RenderAsync: function () {
        //Change event
        $('#' + Third_KnowledgeSearch.ObjectId + '_FilterId').click(function () {
            Third_KnowledgeSearch.Search(null);
        });
    },
    Search: function (vSearchObject) {
        if (vSearchObject != null) {
            var oUrl = this.SearchUrl + '?';
            if (vSearchObject != null && vSearchObject.PageNumber != null)
                oUrl += '&PageNumber=' + vSearchObject.PageNumber;
            else
                oUrl += '&PageNumber=' + '';
            if (vSearchObject != null && vSearchObject.InitDate != null)
                oUrl += '&InitDate=' + vSearchObject.InitDate;
            else
                oUrl += '&InitDate=' + '';
            if (vSearchObject != null && vSearchObject.EndDate != null)
                oUrl += '&EndDate=' + vSearchObject.EndDate;
            else
                oUrl += '&EndDate=' + '';
            if (vSearchObject != null && vSearchObject.SearchType != null)
                oUrl += '&SearchType=' + vSearchObject.SearchType;
            else
                oUrl += '&SearchType=' + '';
            if (vSearchObject != null && vSearchObject.Status != null)
                oUrl += '&Status=' + vSearchObject.Status;
            else
                oUrl += '&Status=' + '';
            window.location = oUrl;
        }
        else {
            var oUrl = this.SearchUrl;
            oUrl += '?InitDate=' + $('#' + Third_KnowledgeSearch.ObjectId + '_InitDateId').val();
            oUrl += '&EndDate=' + $('#' + Third_KnowledgeSearch.ObjectId + '_EndDateId').val();
            oUrl += '&SearchType=' + $('#' + Third_KnowledgeSearch.ObjectId + '_QueryType').val();
            oUrl += '&Status=' + $('#' + Third_KnowledgeSearch.ObjectId + '_QueryStatus').val();
            oUrl += '&PageNumber=0';
            window.location = oUrl;
        }
    },

    Third_Knowledge_ReSearchMasive: function (vReSearchObj) {
        Third_KnowledgeSimpleSearchObject.Loading_Generic_Show();
        $.ajax({
            type: "POST",
            url: BaseUrl.ApiUrl + '/ThirdKnowledgeApi?TKReSearchMasive=true&CompanyPublicId=' + vReSearchObj.CustomerPublicId + '&PeriodPublicId=' + vReSearchObj.PediodPublicId + '&FileName=' + vReSearchObj.FileName,
            success: function (result) {
                Third_KnowledgeSimpleSearchObject.Loading_Generic_Hidden();
                Dialog_ShowMessage("Carga Exitosa", "El archivo es correcto, en unos momentos recibirá un correo con el respectivo resultado de la validación.", window.location.href);

            },
            error: function (result) {
                Third_KnowledgeSimpleSearchObject.Loading_Generic_Hidden();
                Dialog_ShowMessage("Error", "Ocurrió un problema al realizar la consulta, por favor verifique su conexión a internet.", window.location.href);
            },
        })
    }
}

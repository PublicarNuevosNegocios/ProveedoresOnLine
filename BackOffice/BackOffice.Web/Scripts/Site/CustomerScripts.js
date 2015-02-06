/*Customer search object*/
var Customer_SearchObject = {
    ObjectId: '',
    SearchFilter: '',
    PageSize: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchFilter = vInitObject.SearchFilter;
        this.PageSize = vInitObject.PageSize;
    },

    RenderAsync: function () {
        //init input
        $('#' + Customer_SearchObject.ObjectId + '_txtSearch').keypress(function (e) {
            if (e.which == 13) {
                Customer_SearchObject.SearchEvent(null, null);
            }
        });


        //init grid
        $('#' + Customer_SearchObject.ObjectId).kendoGrid({
            editable: false,
            navigatable: false,
            pageable: true,
            scrollable: true,
            selectable: true,
            dataSource: {
                pageSize: Customer_SearchObject.PageSize,
                serverPaging: true,
                schema: {
                    total: function (data) {
                        if (data != null && data.length > 0) {
                            return data[0].TotalRows;
                        }
                        return 0;
                    }
                },
                transport: {
                    read: function (options) {

                        var oSearchParam = $('#' + Customer_SearchObject.ObjectId + '_txtSearch').val();

                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CustomerApi?SMCustomerSearch=true&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
                            dataType: 'json',
                            success: function (result) {
                                options.success(result);
                            },
                            error: function (result) {
                                options.error(result);
                            }
                        });
                    },
                },
            },
            change: function (arg) {
                $.map(this.select(), function (item) {
                    if ($(item).find('td').length >= 2 && $($(item).find('td')[1]).text().length > 0) {
                        window.location = BaseUrl.SiteUrl + 'Provider/GIProviderUpsert?ProviderPublicId=' + $($(item).find('td')[1]).text().replace(/ /gi, '');
                    }
                });
            },
            columns: [{
                field: 'ImageUrl',
                title: 'Logo',
                template: '<img style="width:50px;height:50px;" src="${ImageUrl}" />',
                width: '50px',
            }, {
                field: 'ProviderPublicId',
                title: 'Id',
                width: '50px',
            }, {
                field: 'ProviderName',
                title: 'Nombre',
                width: '50px',
            }, {
                field: 'ProviderType',
                title: 'Tipo',
                width: '50px',
            }, {
                field: 'IdentificationType',
                title: 'Identification',
                template: '${IdentificationType} ${IdentificationNumber}',
                width: '50px',
            }, {
                field: 'Enable',
                title: 'Habilitado',
                width: '100px',
                template: function (dataItem) {
                    var oReturn = '';

                    if (dataItem.Enable == true) {
                        oReturn = 'Si'
                    }
                    else {
                        oReturn = 'No'
                    }
                    return oReturn;
                },
            }],
        });
    },

    SearchEvent: function (vSearchFilter, vSelected) {
        if (vSearchFilter != null && vSelected != null) {
            if (vSelected == true) {
                Customer_SearchObject.SearchFilter = vSearchFilter + ',' + Customer_SearchObject.SearchFilter;
            }
            else {
                Customer_SearchObject.SearchFilter = Customer_SearchObject.SearchFilter.replace(new RegExp(vSearchFilter, 'gi'), '');
            }
        }
        var oSearchParam = $('#' + Customer_SearchObject.ObjectId + '_txtSearch').val();
        window.location = BaseUrl.SiteUrl + 'Customer/Index?SearchParam=' + oSearchParam + '&SearchFilter=' + Customer_SearchObject.SearchFilter;
    },
};

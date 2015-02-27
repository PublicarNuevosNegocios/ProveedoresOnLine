/*init compare Menu*/
function Compare_InitMenu(InitObject) {
    $('#' + InitObject.ObjId).accordion({
        animate: 'swing',
        header: 'label',
        active: InitObject.active
    });
}

var Compare_SearchObject = {

    ObjectId: '',
    RowCount: 0,
    CompareUrl: '',

    Init: function (vInitObject) {

        this.ObjectId = vInitObject.ObjectId;
        this.RowCount = vInitObject.RowCount;
        this.CompareUrl = vInitObject.CompareUrl;
    },

    RenderAsync: function () {

        //load grid comparison
        $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').kendoGrid({
            editable: false,
            navigatable: false,
            pageable: true,
            scrollable: true,
            selectable: true,
            toolbar: [
                { name: 'Search', template: $('#' + Compare_SearchObject.ObjectId + '_SearchGrid_Header_Template').html() },
            ],
            dataSource: {
                pageSize: Compare_SearchObject.RowCount,
                serverPaging: true,
                schema: {
                    total: function (data) {
                        if (data != null && data.length > 0) {
                            return data[0].TotalRows;
                        }
                        return 0;
                    },
                    model: {
                        id: 'CompareId',
                        fields: {
                            CompareId: { editable: false, nullable: true },
                            CompareName: { editable: false, nullable: true },
                            LastModify: { editable: false, nullable: true },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        var oSearchParam = $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').find('input[type=text]').val();

                        $.ajax({
                            url: BaseUrl.ApiUrl + '/CompareApi?CMCompareSearch=true&SearchParam=' + oSearchParam + '&PageNumber=' + (new Number(options.data.page) - 1) + '&RowCount=' + options.data.pageSize,
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
                var odataItem = this.dataItem(this.select());
                if (odataItem != null && odataItem.CompareId != null && odataItem.CompareId > 0) {
                    var oUrl = Compare_SearchObject.CompareUrl;
                    oUrl += '?CompareId=' + odataItem.CompareId;
                    window.location = oUrl;
                }
            },
            columns: [{
                field: 'CompareName',
                title: 'Nombre',
                width: '50px',
            }, {
                field: 'LastModify',
                title: 'Última modificación',
                width: '50px',
            }],
        });

        //add search methods
        $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').find('input[type=text]').keydown(function (e) {
            if (e.keyCode == 13) {
                //enter action search
                $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').data('kendoGrid').dataSource.read();
            }
        });

        $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').find('a').click(function (e) {
            //action search
            $('#' + Compare_SearchObject.ObjectId + '_SearchGrid').data('kendoGrid').dataSource.read();
        });
    },
};


var Compare_DetailObject = {
    ObjectId: '',
    GridColumns: [],
    GridData: [],

    Init: function (vInitObject) {

        this.ObjectId = vInitObject.ObjectId;
        this.GridColumns = vInitObject.GridColumns;
        this.GridData = vInitObject.GridData;
    },

    RenderAsync: function () {
        $('#' + Compare_DetailObject.ObjectId).kendoGrid({
            editable: false,
            navigatable: false,
            pageable: false,
            scrollable: true,
            selectable: true,
            dataSource: {
                data: Compare_DetailObject.GridData,
            },
            columns: Compare_DetailObject.GridColumns,
        });
    },

    GetHeaderTemplate: function(vProviderPublicId)
    {
        return vProviderPublicId;
    },

    GetItemTemplate: function (vProviderPublicId)
    {
        debugger;
        var oReturn = $('#CMPBalance_Company_Item_Template').html();

        oReturn = oReturn.replace(/{ProviderPublicId}/gi, vProviderPublicId);

        return oReturn;
    },
};
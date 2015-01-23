var Provider_SearchObject = {

    ObjectId: '',
    SearchUrl: '',
    SearchParam: '',
    SearchFilter: '',
    SearchOrderType: '',
    OrderOrientation: false,
    PageNumber: 0,

    Init: function (vInitObject) {

        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
        this.SearchParam = vInitObject.SearchParam;
        this.SearchFilter = vInitObject.SearchFilter;
        this.SearchOrderType = vInitObject.SearchOrderType;
        this.OrderOrientation = vInitObject.OrderOrientation;
        this.PageNumber = vInitObject.PageNumber;
    },

    RenderAsync: function () {
    },

    /*{SearchFilter{Enable,Value},SearchOrderType,OrderOrientation,PageNumber}*/
    Search: function (vSearchObject) {

        /*get serach param*/
        this.SearchParam = $('#' + Provider_SearchObject.ObjectId + '_txtSearchBox').val();

        if (vSearchObject != null) {
            /*get filter values*/
            if (vSearchObject.SearchFilter != null) {
                if (vSearchObject.SearchFilter.Enable == true) {
                    this.SearchFilter += ',' + vSearchObject.SearchFilter.Value;
                }
                else {
                    this.SearchFilter = this.SearchFilter.replace(new RegExp(vSearchObject.SearchFilter.Value, gi), '');
                }
            }

            /*get order*/
            if (vSearchObject.SearchOrderType != null) {
                this.SearchOrderType = vSearchObject.SearchOrderType;
            }
            if (vSearchObject.OrderOrientation != null) {
                this.SearchOrderType = vSearchObject.OrderOrientation;
            }

            /*get page*/
            if (vSearchObject.PageNumber != null) {
                this.PageNumber = vSearchObject.PageNumber;
            }
        }
        window.location = GetSearchUrl();
    },

    GetSearchUrl: function () {

        var oUrl = this.SearchUrl;

        oUrl += '?SearchParam=' + this.SearchUrl;
        oUrl += '&SearchFilter=' + this.SearchFilter;
        oUrl += '&SearchOrderType=' + this.SearchOrderType;
        oUrl += '&OrderOrientation=' + this.OrderOrientation;
        oUrl += '&PageNumber=' + this.PageNumber;

        return oUrl;
    },

};
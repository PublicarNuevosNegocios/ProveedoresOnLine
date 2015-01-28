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

        //init search orient controls
        $('input[name="Search_rbOrder"]').change(function () {
            if ($(this) != null && $(this).attr('searchordertype') != null && $(this).attr('orderorientation') != null) {
                Provider_SearchObject.Search({
                    SearchOrderType: $(this).attr('searchordertype'),
                    OrderOrientation: $(this).attr('orderorientation')
                });
            }
        });
    },

    /*{SearchFilter{Enable,Value},SearchOrderType,OrderOrientation,PageNumber}*/
    Search: function (vSearchObject) {
        /*get serach param*/
        this.SearchParam = $('#' + Provider_SearchObject.ObjectId + '_txtSearchBox').val();

        if (vSearchObject != null) {
            /*get filter values*/
            if (vSearchObject.SearchFilter != null) {
                if (vSearchObject.SearchFilter.Enable == true) {
                    this.SearchFilter += vSearchObject.SearchFilter.Value + ',';
                }
                else {
                    this.SearchFilter = this.SearchFilter.replace(new RegExp(vSearchObject.SearchFilter.Value, 'gi'), '').replace(/,,/gi, '');
                }

                /*Init pager*/
                this.PageNumber = 0;
            }

            /*get order*/
            if (vSearchObject.SearchOrderType != null) {
                this.SearchOrderType = vSearchObject.SearchOrderType;
            }
            if (vSearchObject.OrderOrientation != null) {
                this.OrderOrientation = vSearchObject.OrderOrientation;
            }

            /*get page*/
            if (vSearchObject.PageNumber != null) {
                this.PageNumber = vSearchObject.PageNumber;
            }
        }
        window.location = Provider_SearchObject.GetSearchUrl();
    },

    GetSearchUrl: function () {

        var oUrl = this.SearchUrl;

        oUrl += '?SearchParam=' + this.SearchParam;
        oUrl += '&SearchFilter=' + this.SearchFilter;
        oUrl += '&SearchOrderType=' + this.SearchOrderType;
        oUrl += '&OrderOrientation=' + this.OrderOrientation;
        oUrl += '&PageNumber=' + this.PageNumber;

        return oUrl;
    },

};

var Provider_GeneralInfoObject = {

    ObjectId: '',
    UlrContent: '',
   
}
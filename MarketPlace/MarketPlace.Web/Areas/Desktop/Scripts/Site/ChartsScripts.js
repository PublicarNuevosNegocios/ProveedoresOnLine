/*Init charts program object*/
var Survey_ChartsObject = {
    ObjectId: '',
    SurveyResoinsable: '',
    SearchUrl: '',
    UserEmail: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SurveyResoinsable = vInitObject.SurveyResoinsable;
        this.SearchUrl = vInitObject.SearchUrl;
        this.UserEmail = vInitObject.UserEmail;
    },

    RenderAsync: function () {
        if (Survey_ChartsObject.SurveyResoinsable == 'true') {
            //Survey_ChartsObject.RenderChatrSurveyByResponsable();
        }        
    },    

    RenderChatrSurveyByResponsable: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/SurveyApi?GetSurveyByResponsable=true',
            dataType: "json",
            async: false,
            success: function (result) {
                var data = new google.visualization.DataTable();

                data.addColumn('string', 'Estado');
                data.addColumn('number', 'Cantidad');
                $.each(result, function (item, value) {
                    data.addRows([[item, value]]);
                });
                var options = {
                    //title: 'Evaluaciones de Desempeño por estado Año en curso',
                    is3D: true,
                    chartArea:{left:0,top:0,width:"100%",height:"100%"}
                  ,height: "100%"
                  ,width: "100%"
                };

                function selectHandler() {
                    var selectedItem = chart.getSelection()[0];
                    if (selectedItem) {
                        var topping = data.getValue(selectedItem.row, 0);
                        var SearchFilter = 0;
                        if (topping == "Programada") {
                            SearchFilter = 1206001;
                        }
                        else if (topping == "Enviada") {
                            SearchFilter = 1206002;
                        }
                        else if (topping == "En progreso") {
                            SearchFilter = 1206003;
                        }
                        else if (topping == "Finalizada") {
                            SearchFilter = 1206004;
                        }
                        window.location = Survey_ChartsObject.GetSearchUrl(SearchFilter, Survey_ChartsObject.UserEmail);
                    }
                }
                var chart = new google.visualization.PieChart(document.getElementById(Survey_ChartsObject.ObjectId));
                google.visualization.events.addListener(chart, 'select', selectHandler);                
                chart.draw(data, options);
                function resize() {                    
                    // change dimensions if necessary
                    chart.draw(data, options);
                }
                if (window.addEventListener) {
                    window.addEventListener('resize', resize);
                }
                else {
                    window.attachEvent('onresize', resize);
                }
                
            }
        });
    },

    GetSearchUrl: function (SearchFilter, UserEmail) {

        var oUrl = this.SearchUrl;

        oUrl += '?CompareId=';
        oUrl += '&ProjectPublicId=';
        oUrl += '&SearchParam=';

        if (UserEmail != 0) {
            oUrl += '&SearchFilter=,111011;' + SearchFilter + ',111014;' + UserEmail;
        }
        else {
            oUrl += '&SearchFilter=,111011;' + SearchFilter
        }        
        oUrl += '&SearchOrderType=113002';
        oUrl += '&OrderOrientation=false';
        oUrl += '&PageNumber=0';

        return oUrl;
    },
};

var Providers_ChartsObject = {
    ObjectId: '',
    SearchUrl: '',


    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
    },

    RenderAsync: function () {

    },

    RenderChatrProvidersByStatus: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/ProviderApi?GetProvidersByState=true',
            dataType: "json",
            async: false,
            success: function (result) {
                var data = new google.visualization.DataTable();

                data.addColumn('string', 'Estado');
                data.addColumn('number', 'Cantidad');
                $.each(result, function (item, value) {
                    data.addRows([[item, value]]);
                });
                var options = {                   
                    is3D: true,
                    chartArea: { left: 0, top: 0, width: "100%", height: "100%" }
                  , height: "100%"
                  , width: "100%"
                };

                function selectHandler() {
                    var selectedItem = chart.getSelection()[0];
                    if (selectedItem) {
                        var topping = data.getValue(selectedItem.row, 0);
                        var SearchFilter = 0;
                        if (topping == "En creación") {
                            SearchFilter = 902001;
                        }
                        else if (topping == "En proceso") {
                            SearchFilter = 902002;
                        }
                        else if (topping == "En actualización") {
                            SearchFilter = 902003;
                        }
                        else if (topping == "Validado doc. básica") {
                            SearchFilter = 902004;
                        }
                        else if (topping == "Validado doc. completa") {
                            SearchFilter = 902005;
                        }
                        window.location = Providers_ChartsObject.GetSearchUrl(SearchFilter);
                        
                    }
                }
       
                var chart = new google.visualization.PieChart(document.getElementById(Providers_ChartsObject.ObjectId));
                google.visualization.events.addListener(chart, 'select', selectHandler);
                chart.draw(data, options);
                function resize() {
                    // change dimensions if necessary
                    chart.draw(data, options);
                }
                if (window.addEventListener) {
                    window.addEventListener('resize', resize);
                }
                else {
                    window.attachEvent('onresize', resize);
                }

            }
        });
    },

    GetSearchUrl: function (SearchFilter) {

        var oUrl = this.SearchUrl;

        oUrl += '?CompareId=';
        oUrl += '&ProjectPublicId=';
        oUrl += '&SearchParam=';


            oUrl += '&SearchFilter=,112001;' + SearchFilter
               
        oUrl += '&SearchOrderType=113002';
        oUrl += '&OrderOrientation=false';
        oUrl += '&PageNumber=0';

        return oUrl;
    },

};
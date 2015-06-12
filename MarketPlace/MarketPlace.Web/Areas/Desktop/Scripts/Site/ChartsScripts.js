/*Init charts program object*/
var Survey_ChartsObject = {
    ObjectId: '',
    SurveyResoinsable: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SurveyResoinsable = vInitObject.SurveyResoinsable;
        this.SearchUrl = vInitObject.SearchUrl;
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
                        window.location = Survey_ChartsObject.GetSearchUrl(SearchFilter);
                    }
                }
                var chart = new google.visualization.PieChart(document.getElementById(Survey_ChartsObject.ObjectId));
                google.visualization.events.addListener(chart, 'select', selectHandler);
                chart.draw(data, options);
            }
        });
    },

    GetSearchUrl: function (SearchFilter) {

        var oUrl = this.SearchUrl;

        oUrl += '?CompareId=';
        oUrl += '&ProjectPublicId=';
        oUrl += '&SearchParam=';
        oUrl += '&SearchFilter=,111011;' + SearchFilter;
        oUrl += '&SearchOrderType=113002';
        oUrl += '&OrderOrientation=false';
        oUrl += '&PageNumber=0';

        return oUrl;
    },
};
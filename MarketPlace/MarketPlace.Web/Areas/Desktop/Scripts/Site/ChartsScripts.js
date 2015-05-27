/*Init charts program object*/
var Survey_ChartsObject = {
    ObjectId: '',
    SurveyResoinsable: '',
    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SurveyResoinsable = vInitObject.SurveyResoinsable;
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
                    title: 'Evaluiaciones de Desempeño por estado Año en curso',
                    is3D: true,
                };

                function selectHandler() {
                    var selectedItem = chart.getSelection()[0];
                    if (selectedItem) {
                        var topping = data.getValue(selectedItem.row, 0);
                        debugger;
                        alert('The user selected ' + topping);
                    }
                }
                var chart = new google.visualization.PieChart(document.getElementById(Survey_ChartsObject.ObjectId));
                google.visualization.events.addListener(chart, 'select', selectHandler);
                chart.draw(data, options);
            }
        });
    },

};
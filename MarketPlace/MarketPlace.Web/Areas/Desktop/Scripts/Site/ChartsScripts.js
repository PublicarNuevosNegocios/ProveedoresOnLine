﻿//**** SURVEY CHARTS ****//
var Survey_ChartsObject = {
    ObjectId: '',
    SurveyResponsible: '',
    SearchUrl: '',
    UserEmail: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SurveyResponsible = vInitObject.SurveyResponsible;
        this.SearchUrl = vInitObject.SearchUrl;
        this.UserEmail = vInitObject.UserEmail;
    },

    RenderChartSurveyByResponsable: function () {
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
                    is3D: true,
                    chartArea: { left: 0, top: 0, width: "100%", height: "100%" }
                  , height: "100%"
                  , width: "100%"
                   , colors: ['#FF6961', '#77DD77', '#966FD6', '#FDFD96', '#FFD1DC', '#03C03C', '#779ECB', '#C23B22']
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

var SurveyByName_ChartsObject = {
    ObjectId: '',
    SurveyName: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SurveyName = vInitObject.SurveyName;
        this.SearchUrl = vInitObject.SearchUrl;
    },

    RenderChart: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/SurveyApi?GetSurveyByName=true',
            dataType: "json",
            async: false,
            success: function (result) {
                var data = new google.visualization.DataTable();
                
                data.addColumn('string', 'Tipo');
                data.addColumn('number', 'Cantidad');
                data.addColumn('number', 'filtro');
                $.each(result, function (item, value) {
                    data.addRows([[value.m_Item2, value.m_Item3, value.m_Item1]]);
                });

                var options = {
                    is3D: true,
                    chartArea: { left: 0, top: 0, width: "100%", height: "100%" },
                    height: "100%",
                    width: "100%",
                   colors: ['#FF6961', '#77DD77', '#966FD6', '#FDFD96', '#FFD1DC', '#03C03C', '#779ECB', '#C23B22']
                    
                };

                function selectHandler() {
                    var selectedItem = chart.getSelection()[0];                    
                    if (selectedItem) {                        
                        var SearchFilter = data.getValue(selectedItem.row, 2);
                        window.location = SurveyByName_ChartsObject.GetSearchUrl(SearchFilter);
                    }
                }
                var chart = new google.visualization.PieChart(document.getElementById(SurveyByName_ChartsObject.ObjectId));
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


        oUrl += '&SearchFilter=,111012;' + SearchFilter

        oUrl += '&SearchOrderType=113002';
        oUrl += '&OrderOrientation=false';
        oUrl += '&PageNumber=0';

        return oUrl;
    },
};

var SurveyByEvaluators_ChartsObject = {
    ObjectId: '',
    SearchUrl: '',
    DashboardId: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
        this.DashboardId = vInitObject.DashboardId;
    },

    RenderChatrSurveyByEvaluator: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/SurveyApi?GetSurveyByEvaluators=true',
            dataType: "json",
            async: false,
            success: function (result) {

                var data = new google.visualization.DataTable();
                data.addColumn('string', 'Mail');
                data.addColumn('string', 'Estado');
                data.addColumn('string', 'Mes');
                data.addColumn('number', 'Cantidad');
                data.addColumn({ type: 'string', role: 'annotation' });
                data.addColumn('number', 'UserId');
                $.each(result, function (item, value) {
                    data.addRows([[value.m_Item1, value.m_Item2, value.m_Item3, value.m_Item4, value.m_Item2, value.m_Item5]]);
                });

                var dashboard = new google.visualization.Dashboard(document.getElementById(SurveyByEvaluators_ChartsObject.DashboardId));

                var vBarChart = new google.visualization.ChartWrapper({
                    'chartType': 'BarChart',
                    'bars': 'horizontal',
                    'containerId': document.getElementById(SurveyByEvaluators_ChartsObject.ObjectId),
                    'options': {
                        chartArea: { left: 150, top: 0, width: "70%", height: "80%" },
                        height: "100%",
                        width: "100%"
                    },
                    'view': {
                        'columns': [0, 3, 4]
                    }

                });
                // Create a range slider, passing some options
                var barFilterMonth = new google.visualization.ControlWrapper({
                    'controlType': 'CategoryFilter',
                    'containerId': 'filter_month',
                    'options': {
                        'filterColumnLabel': 'Mes'
                    }
                });
                var barFilterState = new google.visualization.ControlWrapper({
                    'controlType': 'CategoryFilter',
                    'containerId': 'filter_state',
                    'options': {
                        'filterColumnLabel': 'Estado'
                    }
                });
                dashboard.bind(barFilterMonth, vBarChart);
                dashboard.bind(barFilterState, vBarChart);


                google.visualization.events.addListener(vBarChart, 'ready', function () {
                    google.visualization.events.addListener(vBarChart, 'select', selectHandler);
                });

                function selectHandler() {
                    var selectedItem = vBarChart.getChart().getSelection();
                    if (selectedItem) {
                        var SearchFilter = data.getValue(selectedItem[0].row, 5);
                        window.location = SurveyByEvaluators_ChartsObject.GetSearchUrl(SearchFilter);

                    }
                }


                google.visualization.events.addListener(vBarChart, 'select', selectHandler);


                dashboard.draw(data);
                function resize() {
                    // change dimensions if necessary
                    dashboard.draw(data);
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


        oUrl += '&SearchFilter=,111014;' + SearchFilter

        oUrl += '&SearchOrderType=113002';
        oUrl += '&OrderOrientation=false';
        oUrl += '&PageNumber=0';

        return oUrl;
    },
};

var SurveyByMonth_ChartsObject = {
    ObjectId: '',
    DashboardId: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.DashboardId = vInitObject.DashboardId;
        this.SearchUrl = vInitObject.SearchUrl;
    },

    RenderChatrSurveyByMonth: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/SurveyApi?GetSurveyByMonth=true',
            dataType: "json",
            async: false,
            success: function (result) {
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'Estado');
                data.addColumn('number', 'Cantidad');
                data.addColumn('string', 'Mes');
                data.addColumn({ type: 'string', role: 'tooltip' });
                data.addColumn('string', "EstadoId");
                $.each(result, function (item, value) {
                    data.addRows([[value.m_Item1, value.m_Item2, value.m_Item3, value.m_Item1, value.m_Item4]]);
                });
                var dashboard = new google.visualization.Dashboard(document.getElementById(SurveyByMonth_ChartsObject.DashboardId));

                var pieChart = new google.visualization.ChartWrapper({
                    'chartType': 'PieChart',
                    'containerId': document.getElementById(SurveyByMonth_ChartsObject.ObjectId),
                    'options': {
                        pieSliceTextStyle: {
                            color: 'black',
                        },
                        slices: {
                            0: { color: '#779ECB' },
                            1: { color: '#FDFD96' },
                        },
                        pieHole: 0.4
                        , chartArea: { left: 0, top: 0, width: "100%", height: "100%" }
                        , height: "100%"
                        , width: "100%"
                    },
                    'view': { 'columns': [0, 1] },

                });

                var filterMonth = new google.visualization.ControlWrapper({
                    'controlType': 'CategoryFilter',
                    'containerId': 'filter_div',
                    'options': {
                        'filterColumnLabel': 'Mes'
                    }
                });
                dashboard.bind(filterMonth, pieChart);


                google.visualization.events.addListener(pieChart, 'ready', function () {
                    google.visualization.events.addListener(pieChart, 'select', selectHandler);
                });

                function selectHandler() {                    
                    var selectedItem = pieChart.getChart().getSelection();
                    if (selectedItem) {
                        var SearchFilter = data.getValue(selectedItem[0].row, 4);
                        window.location = SurveyByMonth_ChartsObject.GetSearchUrl(SearchFilter);
                    }
                }

                dashboard.draw(data);
                function resize() {
                    // change dimensions if necessary
                    dashboard.draw(data);
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


        oUrl += '&SearchFilter=,111011;' + SearchFilter

        oUrl += '&SearchOrderType=113002';
        oUrl += '&OrderOrientation=false';
        oUrl += '&PageNumber=0';

        return oUrl;
    }
};

//**** PROVIDERS CHARTS ****//
var Providers_ChartsObject = {
    ObjectId: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
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
                    opacity: 0.2,
                    is3D: true,
                    chartArea: { left: 0, top: 0, width: "100%", height: "100%" }
                  , height: "100%"
                  , width: "100%"
                    , colors: ['#FF6961', '#77DD77', '#966FD6', '#FDFD96', '#FFD1DC', '#03C03C', '#779ECB', '#C23B22']
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

//**** PROJECT CHARTS ****//
var ProjectByStatus_ChartsObject = {
    ObjectId: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
    },

    RenderChatrProjectByStatus: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/ProjectApi?GetProjectByState=true',
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
                    legend: 'none',
                    series: { 0: { color: 'orange', opacity: 0.2 }, 1: { color: 'blue', opacity: 0.2 } },
                    is3D: true,
                    chartArea: { left: 30, top: 10, width: "100%", height: "80%" }
                  , height: "100%"
                  , width: "100%"
                    , colors: ['#FF6961', '#77DD77', '#966FD6', '#FDFD96', '#FFD1DC', '#03C03C', '#779ECB', '#C23B22']
                };

                var chart = new google.visualization.ColumnChart(document.getElementById(ProjectByStatus_ChartsObject.ObjectId));

                chart.draw(data, options);
                function resize() {
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
};

var ProjectByResponsible_ChartsObject = {
    ObjectId: '',
    DashboardId: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
        this.DashboardId = vInitObject.DashboardId;
    },

    RenderChart: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/ProjectApi?GetProjectByResponsible=true',
            dataType: "json",
            async: false,
            success: function (result) {

                var data = new google.visualization.DataTable();                
                data.addColumn('string', 'Responsable');
                data.addColumn('number', 'Cantidad');
                $.each(result, function (item, value) {                    
                    data.addRows([[item, value]]);
                });

                var options = {
                    bars: 'horizontal',
                    legend: { position: "none" },
                    series: { 0: { color: 'orange', visibleInLegend: false }, 1: { color: 'blue', visibleInLegend: false } },
                    is3D: true,
                    chartArea: { left: 200, top: 10, width: "100%", height: "90%" },
                    height: "100%",
                    width: "60%"
                    , colors: ['#FF6961', '#77DD77', '#966FD6', '#FDFD96', '#FFD1DC', '#03C03C', '#779ECB', '#C23B22']
                    
                };

                var vBarChart = new google.visualization.BarChart(document.getElementById(ProjectByResponsible_ChartsObject.ObjectId));

                vBarChart.draw(data, options);
                function resize() {
                    vBarChart.draw(data, options);
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
};
//**** SURVEY CHARTS ****//
var Survey_ChartsObject = {
    ObjectId: '',
    SurveyResponsible: '',
    SearchUrl: '',
    UserEmail: '',
    DashboardId:'',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SurveyResponsible = vInitObject.SurveyResponsible;
        this.SearchUrl = vInitObject.SearchUrl;
        this.UserEmail = vInitObject.UserEmail;
        this.DashboardId = vInitObject.DashboardId;
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
                data.addColumn('number', 'Year');
                $.each(result, function (item, value) {
                    data.addRows([[value.m_Item1, value.m_Item2, value.m_Item3]]);
                });               
                var dashboard = new google.visualization.Dashboard(document.getElementById(Survey_ChartsObject.DashboardId));
                var vBarChart = new google.visualization.ChartWrapper({
                    'chartType': 'PieChart',
                    'containerId': document.getElementById(Survey_ChartsObject.ObjectId),
                    'options': {
                        colors: ['#FF6961', '#77DD77', '#966FD6', '#FDFD96', '#FFD1DC', '#03C03C', '#779ECB', '#C23B22'],
                        is3D: true,
                        chartArea: { left: 90, top: 0, width: "88%", height: "98%" },
                        height: "100%",
                        width: "100%"
                    },
                    'view': {
                        'columns': [0, 1]
                    }
                });
                // Create a range slider, passing some options
                var barFilterYear = new google.visualization.ControlWrapper({
                    'controlType': 'CategoryFilter',
                    'containerId': 'filter_year_sv',
                   
                    'options': {
                        'filterColumnLabel': 'Year',
                        'ui': {
                            'label': 'Año'
                        }
                    }
                });
                dashboard.bind(barFilterYear, vBarChart);

                google.visualization.events.addListener(vBarChart, 'ready', function () {
                    google.visualization.events.addListener(vBarChart, 'select', selectHandler);
                });

                function selectHandler() {
                    var selectedItem = vBarChart.getChart().getSelection();
                    if (selectedItem) {
                        var topping = data.getValue(selectedItem[0].row, 0);
                        var SearchFilter = 0;
                        if (topping == "Programada") {
                            SearchFilter = 1206001;
                        }
                        else if (topping == "Enviada") {
                            SearchFilter = 1206002;
                        }
                        else if (topping == "En Progreso") {
                            SearchFilter = 1206003;
                        }
                        else if (topping == "Finalizada") {
                            SearchFilter = 1206004;
                        }
                        else if (topping == "Vencida") {
                            SearchFilter = 1206005;
                        }
                        window.location = Survey_ChartsObject.GetSearchUrl(SearchFilter, Survey_ChartsObject.UserEmail);
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
    DashboardId: '',
    SurveyName: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SurveyName = vInitObject.SurveyName;
        this.SearchUrl = vInitObject.SearchUrl;
        this.DashboardId = vInitObject.DashboardId;
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
                data.addColumn('number', 'Year');
                $.each(result, function (item, value) {
                    data.addRows([[value.m_Item2, value.m_Item3, value.m_Item1, value.m_Item4]]);
                });

                var dashboard = new google.visualization.Dashboard(document.getElementById(SurveyByName_ChartsObject.DashboardId));

                var vBarChart = new google.visualization.ChartWrapper({
                    'chartType': 'PieChart',
                    'containerId': document.getElementById(SurveyByName_ChartsObject.ObjectId),
                    'options': {
                        colors: ['#FF6961', '#77DD77', '#966FD6', '#FDFD96', '#FFD1DC', '#03C03C', '#779ECB', '#C23B22'],
                        is3D: true,
                        chartArea: { left: 150, top: 0, width: "70%", height: "80%" },
                        height: "100%",
                        width: "100%"
                    },
                    'view': {
                        'columns': [0, 1]
                    }
                });

                // Create a range slider, passing some options
                var barFilterYearnm = new google.visualization.ControlWrapper({
                    'controlType': 'CategoryFilter',
                    'containerId': 'filter_year_nm',

                    'options': {
                        'filterColumnLabel': 'Year',
                        'ui': {
                            'label': 'Año'
                        }
                    }
                });

                dashboard.bind(barFilterYearnm, vBarChart);


                google.visualization.events.addListener(vBarChart, 'ready', function () {
                    google.visualization.events.addListener(vBarChart, 'select', selectHandler);
                });

                function selectHandler() {

                    var selectedItem = vBarChart.getChart().getSelection();
                    if (selectedItem) {
                        var SearchFilter = data.getValue(selectedItem[0].row, 2);
                        window.location = SurveyByName_ChartsObject.GetSearchUrl(SearchFilter);
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
                data.addColumn('number', 'Year');
                $.each(result, function (item, value) {
                    data.addRows([[value.m_Item1, value.m_Item2, value.m_Item3, value.m_Item4, value.m_Item2, value.m_Item5, value.m_Item6]]);
                });

                var dashboard = new google.visualization.Dashboard(document.getElementById(SurveyByEvaluators_ChartsObject.DashboardId));

                var vBarChart = new google.visualization.ChartWrapper({
                    'is3D': true,
                    'chartType': 'PieChart',
                    'bars': 'horizontal',
                    'containerId': document.getElementById(SurveyByEvaluators_ChartsObject.ObjectId),
                    'options': {
                        is3D: true,
                        chartArea: { left: 10, top: 28, width: "98%", height: "80%" },
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
                        ,
                        'ui': {
                            'label': 'Mes'
                        }
                    }
                });
                var barFilterState = new google.visualization.ControlWrapper({
                    'controlType': 'CategoryFilter',
                    'containerId': 'filter_state',
                    'options': {
                        'filterColumnLabel': 'Estado'
                        ,
                        'ui': {
                            'label': 'Estado'
                        }
                    }
                });
                var barFilterYear = new google.visualization.ControlWrapper({
                    'controlType': 'CategoryFilter',
                    'containerId': 'filter_year',
                    'options': {
                        'filterColumnLabel': 'Year'
                        ,
                        'ui': {
                            'label': 'Año'
                        }
                    }
                });
                dashboard.bind(barFilterMonth, vBarChart);
                dashboard.bind(barFilterState, vBarChart);
                dashboard.bind(barFilterYear, vBarChart);
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
                data.addColumn('number', 'Year');
                $.each(result, function (item, value) {
                    data.addRows([[value.m_Item1, value.m_Item2, value.m_Item3, value.m_Item1, value.m_Item4, value.m_Item5]]);
                });
                var dashboard = new google.visualization.Dashboard(document.getElementById(SurveyByMonth_ChartsObject.DashboardId));

                var pieChart = new google.visualization.ChartWrapper({
                    'chartType': 'PieChart',
                    'containerId': document.getElementById(SurveyByMonth_ChartsObject.ObjectId),
                    'options': {
                        colors: ['#FF6961', '#77DD77', '#966FD6', '#FDFD96', '#FFD1DC', '#03C03C', '#779ECB', '#C23B22'],
                        is3D: false,
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
                        'filterColumnLabel': 'Mes',
                        'ui': {
                            'label': 'Mes'
                        }
                    }
                });
                var filterYear = new google.visualization.ControlWrapper({
                    'controlType': 'CategoryFilter',
                    'containerId': 'filter_div_year',
                    'options': {
                        'filterColumnLabel': 'Year',
                        'ui': {
                            'label': 'Año'
                        }
                    }
                });
                dashboard.bind(filterMonth, pieChart);
                dashboard.bind(filterYear, pieChart);

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
                        if (topping == "En Creación Nacional") {
                            SearchFilter = 902001;
                        }
                        else if (topping == "En Proceso Nacional") {
                            SearchFilter = 902002;
                        }
                        else if (topping == "En Actualización") {
                            SearchFilter = 902003;
                        }
                        else if (topping == "Validado Documentación Básica Nacional") {
                            SearchFilter = 902004;
                        }
                        else if (topping == "Validado Documentación Completa Nacional") {
                            SearchFilter = 902005;
                        }
						else if (topping == "En Creación Extranjero") {
                            SearchFilter = 902006;
                        }
						else if (topping == "En Proceso Extranjero") {
                            SearchFilter = 902007;
                        }
						else if (topping == "Validado Documentación Completa Extranjero") {
                            SearchFilter = 902008;
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

//-National Providers-//
var NationalProviders_ChartsObject = {
    ObjectId: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
    },

    RenderChatrNationalProvidersByStatus: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/ProviderApi?GetNationalProvidersByState=true',
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
                        if (topping == "En Creación Nacional") {
                            SearchFilter = 902001;
                        }
                        else if (topping == "En Proceso Nacional") {
                            SearchFilter = 902002;
                        }
                        else if (topping == "En Actualización") {
                            SearchFilter = 902003;
                        }
                        else if (topping == "Validado Documentación Básica Nacional") {
                            SearchFilter = 902004;
                        }
                        else if (topping == "Validado Documentación Completa Nacional") {
                            SearchFilter = 902005;
                        }
                        else if (topping == "Imposible Contactar Nacional") {
                            SearchFilter = 902009;
                        }
                        else if (topping == "Inactivo Nacional") {
                            SearchFilter = 902011;
                        }
                        window.location = NationalProviders_ChartsObject.GetSearchUrl(SearchFilter);

                    }
                }

                var chart = new google.visualization.PieChart(document.getElementById(NationalProviders_ChartsObject.ObjectId));
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

//-Alien Providers-//
var AlienProviders_ChartsObject = {
    ObjectId: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
    },

    RenderChatrAlienProvidersByStatus: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/ProviderApi?GetAlienProvidersByState=true',
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
                        if (topping == "En Creación Extranjero") {
                            SearchFilter = 902006;
                        }
                        else if (topping == "En Proceso Extranjero") {
                            SearchFilter = 902007;
                        }
                        else if (topping == "Validado Documentación Completa Extranjero") {
                            SearchFilter = 902008;
                        }
                        else if (topping == "Imposible Contactar Extranjero") {
                            SearchFilter = 902010;
                        }
                        else if (topping == "Inactivo Extranjero") {
                            SearchFilter = 902012;
                        }

                        window.location = AlienProviders_ChartsObject.GetSearchUrl(SearchFilter);

                    }
                }

                var chart = new google.visualization.PieChart(document.getElementById(AlienProviders_ChartsObject.ObjectId));
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
    DashboardId: '',
    SearchUrl: '',

    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SearchUrl = vInitObject.SearchUrl;
        this.DashboardId = vInitObject.DashboardId;
    },

    RenderChatrProjectByStatus: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/SurveyApi?GetSurveyByResponsable=true',
            dataType: "json",
            async: false,
            success: function (result) {
                var data = new google.visualization.DataTable();

                data.addColumn('string', 'Estado');
                data.addColumn('number', 'Cantidad');
                data.addColumn('number', 'Year');
                $.each(result, function (item, value) {
                    data.addRows([[value.m_Item1, value.m_Item2, value.m_Item3]]);
                });
                var dashboard = new google.visualization.Dashboard(document.getElementById(ProjectByStatus_ChartsObject.DashboardId));
                var vBarChart = new google.visualization.ChartWrapper({
                    'chartType': 'BarChart',
                    'containerId': document.getElementById(ProjectByStatus_ChartsObject.ObjectId),
                    'options': {
                        colors: ['#FF6961', '#77DD77', '#966FD6', '#FDFD96', '#FFD1DC', '#03C03C', '#779ECB', '#C23B22'],
                        is3D: true,
                        chartArea: { left: 150, top: 0, width: "70%", height: "80%" },
                        height: "100%",
                        width: "100%"
                    },
                    'view': {
                        'columns': [0, 1]
                    }
                });
                // Create a range slider, passing some options
                var barFilterYear = new google.visualization.ControlWrapper({
                    'controlType': 'CategoryFilter',
                    'containerId': 'filter_pjs_year',
                    'options': {
                        'filterColumnLabel': 'Year',
                        'ui': {
                            'label': 'Año'
                        }
                    }
                });
                dashboard.bind(barFilterYear, vBarChart);


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
                data.addColumn('number', 'Year');
                $.each(result, function (item, value) {
                    data.addRows([[value.m_Item1, value.m_Item2, value.m_Item3]]);
                });


                var dashboard = new google.visualization.Dashboard(document.getElementById(ProjectByResponsible_ChartsObject.DashboardId));
                var vBarChart = new google.visualization.ChartWrapper({
                    'chartType': 'PieChart',
                    'containerId': document.getElementById(ProjectByResponsible_ChartsObject.ObjectId),
                    'options': {
                        colors: ['#FF6961', '#77DD77', '#966FD6', '#FDFD96', '#FFD1DC', '#03C03C', '#779ECB', '#C23B22'],
                        is3D: true,
                        chartArea: { left: 150, top: 0, width: "70%", height: "80%" },
                        height: "100%",
                        width: "100%"
                    },
                    'view': {
                        'columns': [0, 1]
                    }
                });
                // Create a range slider, passing some options
                var barFilterYear = new google.visualization.ControlWrapper({
                    'controlType': 'CategoryFilter',
                    'containerId': 'filter_pjr_year',

                    'options': {
                        'filterColumnLabel': 'Year',
                        'ui': {
                            'label': 'Año'
                        }
                    }
                });
                dashboard.bind(barFilterYear, vBarChart);

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
};

//*** ThirKnowLedge Chart ***//

var TK_GetPeriodsByPlan_ChartsObject = {
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
            url: BaseUrl.ApiUrl + '/ThirdKnowledgeApi?GetPeriodsByPlan=true',
            dataType: "json",
            async: false,
            success: function (result) {
                
                var data = new google.visualization.DataTable();
                var total_query = 0;
                data.addColumn('string', 'Periodo');
                data.addColumn('number', 'Consultas Realizadas');
                data.addColumn({ type: 'string', role: 'annotation' });
                data.addColumn('number', 'Consultas Restantes');
                data.addColumn({ type: 'string', role: 'annotation' });
                $.each(result, function (item, value) {
                    if (value.m_Item2 > 0) {
                        total_query = value.m_Item3;
                        var temp = value.m_Item3 - value.m_Item2;
                        if (Number(temp) < 0) { temp = 0; }
                        if (Number(temp)==0) {
                            data.addRows([[value.m_Item1, value.m_Item2, String(value.m_Item2), Number(temp), '']]);
                        }
                        if (Number(temp) > 0) {
                            data.addRows([[value.m_Item1, value.m_Item2, String(value.m_Item2), Number(temp), String(temp)]]);
                        }
                        
                    }
                });

                var options = {
                    title: 'Limite de consultas por periodo: '+ total_query,
                    subtitle: 'Based on a scale of 1 to 10',
                    legend: { position: 'top', alignment: 'start' },
                    isStacked: true,
                    chartArea: { left: 125, top: 30, width: "100%", height: "90%" },
                    width: "100%",
                    colors: ['#FF6961', '#77DD77']
                };

                var chart = new google.visualization.BarChart(document.getElementById(TK_GetPeriodsByPlan_ChartsObject.ObjectId));

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
    }
};
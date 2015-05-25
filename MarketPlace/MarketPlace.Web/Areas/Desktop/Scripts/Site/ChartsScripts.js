/*Init charts program object*/
var Survey_ProgramObject = {    
    ObjectId: '',
    SurveyResoinsable:'',
    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
        this.SurveyResoinsable = vInitObject.SurveyResoinsable;
    },

    RenderAsync: function () {
        debugger;
        if (Survey_ProgramObject.SurveyResoinsable == 'true') {
            Survey_ProgramObject.RenderChatrSurveyByResponsable();
        }
    },

    RenderChatrSurveyByResponsable: function () {
        debugger;
      var jsonData = $.ajax({
            url: BaseUrl.ApiUrl + '/SurveyApi?GetSurveyByResponsable=true&ProjectPublicId=' + Provider_SearchObject.ProjectPublicId + '&ProviderPublicId=' + vProviderPublicId,
            dataType: 'json',
            //success: function (result) {
            //    if (result != null) {
            //        Provider_SearchObject.OpenProject();
            //    }
            //},
            //error: function (result) {
            //    Dialog_ShowMessage('Proceso de selección', 'Se ha generado un error agregando al proveedor al proceso de selección, por favor intentelo nuevamente.', null);
            //}
      }).responseText;

      var data = new google.visualization.DataTable(jsonData);
      var chart = new google.visualization.PieChart(document.getElementById('SurveyResponsableStatusChartId'));
      chart.draw(data, { width: 400, height: 240 });

    },
};
//**** REPORT ****//
var Survey_Report = {
    ObjectId: '',


    Init: function (vInitObject) {
        this.ObjectId = vInitObject.ObjectId;
    },

    ListSurveyGeneralInfoReport: function () {
        $.ajax({
            url: BaseUrl.ApiUrl + '/ReportApi?SurveyGeneralInfoReport=true&algo=' + "Algo",
            dataType: "json",
            async: false,            
            success: function (result) {
                alert(result);

            }, error: function (result){
                alert("Error");
            },
        });
    },

};

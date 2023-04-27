angular.module("General").controller('CatalogTypeMeasureController', function (models, $scope, $http, $window, notify, $rootScope) {
    var arch = new FileReader();
    $scope.myFiles = [];
    $scope.valResult = {};
    $scope.description = "";
    $scope.factorUtilidad = 0.0;
    // $scope.newTypeMeasure = new models.TypeMeasure();

    $scope.AddTypeMeasure = function () {
        $window.location = '../../CatalogTypeMeasure/AddTypeMeasure';
    };

    $scope.SaveAddTypeMeasure = function () {
        $scope.SaveAdd();

    };

    $scope.SaveAdd = function () {
        $("#btnSave").button("loading");
        $http({
            method: 'POST',
            url: '../../../CatalogTypeMeasure/SaveAddTypeMeasure',
            params: {

                nombreMedida: $scope.nombreMedida
            }
        }).
            success(function (data, status, headers, config) {
                $("#btnSave").button("reset");
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.id = data.oData.idTypeMeasure;
                    $window.location = '../../../CatalogTypeMeasure/Index';
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).
            error(function (data, status, headers, config) {
                $("#btnSave").button("reset");
                notify("Ocurrío un error.", $rootScope.error);
            });
    };


    $scope.openModalMsg = function () {
        $("#modalMsg").modal("show");
    };

    $scope.closeModalMsg = function () {
        $("#msgModalMsg").text("");
        $("#modalMsg").modal("hide");
    };
});
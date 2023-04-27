angular.module("General").controller('CatalogTypeServiceController', function (models, $scope, $http, $window, notify, $rootScope) {
    var arch = new FileReader();
    $scope.myFiles = [];
    $scope.valResult = {};
    $scope.description = "";
    $scope.factorUtilidad = 0.0;
    // $scope.newTypeService = new models.typeService();

    $scope.AddTypeService = function () {
        $window.location = '../../CatalogTypeService/AddTypeService';
    };

    $scope.SaveAddTypeService = function () {
        $scope.SaveAdd();

    };

    $scope.SaveAdd = function () {
        $("#btnSave").button("loading");
        $http({
            method: 'POST',
            url: '../../../CatalogTypeService/SaveAddTypeService',
            params: {

                description: $scope.description,
                factorUtilidad: $scope.factorUtilidad
            }
        }).
            success(function (data, status, headers, config) {
                $("#btnSave").button("reset");
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.id = data.oData.idTypeService;
                    $window.location = '../../../CatalogTypeService/Index';
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

    let input = document.getElementById("factorUtilidad");

    input.addEventListener("blur", function () {
        this.value = parseFloat(this.value).toFixed(2);       
    });
});
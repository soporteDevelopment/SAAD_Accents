angular.module("General").controller('CatalogBrandController', function (models, $scope, $http, $window, notify, $rootScope) {
    $scope.itemsPerPage = 1000;
    $scope.currentPage = 0;

    $scope.SaveBrand = function () {
        if ($scope.frmCatalogBrand.$valid) {

            $("#btnSave").button("loading");
            $http({
                method: 'POST',
                url: '../../../CatalogBrand/Patch',
                data: {
                    Name: $scope.name,
                    Description: $scope.description,
                    idProvider: $scope.idProvider,
                    idCatalogBrand: $scope.idCatalogBrand
                }
            }).
                success(function (data, status, headers, config) {
                    $("#btnSave").button("reset");
                    if (data.success == 1) {
                        notify(data.oData.Message, $rootScope.success);
                        window.location = "../../../CatalogBrand/List";
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
        }
    };
    $scope.GetProviders = function () {
        $("#search").button("loading");

        $http({
            method: 'GET',
            url: '../../../CatalogProvider/GetAll',
            params: {
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {
                $("#search").button("reset");

                if (data.success == 1) {
                    if (data.oData.Providers.length > 0) {
                        $scope.Providers = data.oData.Providers;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.providers = data.oData.Providers;
                        $scope.total = data.oData.Count;
                    }
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).
            error(function (data, status, headers, config) {
                $("#search").button("reset");
                notify("Ocurrío un error.", $rootScope.error);
            });
    };
   
});
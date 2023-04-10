angular.module("General").controller('MaterialsController', function (models, materialValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
        }
    };

    $scope.nextPageDisabled = function () {
        return $scope.currentPage === $scope.pageCount() - 1 ? "disabled" : "";
    };

    $scope.pageCount = function () {
        return Math.ceil($scope.total / $scope.itemsPerPage);
    };

    $scope.range = function () {
        var rangeSize = 5;
        var ret = [];
        var start;
        var result = $scope.pageCount();

        if ($scope.pageCount() == 0)
            return ret;

        start = $scope.currentPage;
        if (start > $scope.pageCount() - rangeSize) {
            start = $scope.pageCount() - rangeSize;
        }

        for (var i = start; i < start + rangeSize; i++) {
            if (i >= 0) {
                ret.push(i);
            }
        }

        return ret;
    };

    $scope.setPage = function (n) {

        var i = $scope.pageCount();

        if (n >= 0 && n < $scope.pageCount()) {
            $scope.currentPage = n;
        }
    };

    $scope.$watch("currentPage", function (newValue, oldValue) {

        $scope.listMaterials();

    });

    $scope.onLoad = function () {

        $scope.listMaterials();

    };

    $scope.listMaterials = function () {

        $http({
            method: 'POST',
            url: '../../../Materials/ListMaterials',
            params: {
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                if (data.oData.Materials.length > 0) {

                    $scope.Materials = data.oData.Materials;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Materials = data.oData.Materials;
                    $scope.total = data.oData.Count;

                }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

        });
    };

    $scope.valResult = {};

    $scope.newMaterial = new models.Material();

    $scope.AddMaterial = function () {

        $window.location = '../../Materials/AddMaterial';

    };

    $scope.SaveAddMaterial = function () {

        $scope.valResult = materialValidator.validate($scope.newMaterial);

        if ($scope.newMaterial.$isValid) {

            $scope.SaveAdd();

        }
    };

    $scope.SaveAdd = function () {

        $("#SaveAddMaterial").button("loading");

        $http({
            method: 'POST',
            url: '../../../Materials/SaveAddMaterial',
            params: {
                name: $scope.newMaterial.Material,               
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveAddMaterial").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $window.location = '../../../Materials/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveAddMaterial").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.UpdateMaterial = function (ID) {

        $window.location = '../../Materials/UpdateMaterial?idMaterial=' + ID;

    };

    $scope.SaveUpdateMaterial = function (ID) {

        $scope.valResult = materialValidator.validate($scope.newMaterial);

        if ($scope.newMaterial.$isValid) {

            $scope.SaveUpdate(ID);
        }

    },

    $scope.SaveUpdate = function (ID) {

        $("#SaveUpdateMaterial").button("loading");

        $http({
            method: 'POST',
            url: '../../../Materials/SaveUpdateMaterial',
            params: {
                idMaterial: ID,
                name: $scope.newMaterial.Material,               
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveUpdateMaterial").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $window.location = '../../../Materials/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveUpdateMaterial").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

});
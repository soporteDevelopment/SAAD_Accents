angular.module("General").controller('MetropolitanAreasController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.saving = false;

    $scope.init = function(){
        $scope.getMetropolitanArea();
        $scope.getStates();
    }

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.getMetropolitanArea();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.getMetropolitanArea();
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
            $scope.getMetropolitanArea();
        }
    };

    $scope.getMetropolitanArea = function () {
        $http({
            method: 'GET',
            url: '../../../MetropolitanAreas/Get',
            params: {
                idTown: null,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    if (data.oData.Towns.length > 0) {
                        $scope.metropolitanAreas = data.oData.Towns;
                        $scope.total = data.oData.Total;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.metropolitanAreas = data.oData.Towns;
                        $scope.total = data.oData.Total;
                    }
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.saveMetropolitanArea = function () {
        $scope.saving = true;

        $http({
            method: 'POST',
            url: '../../../MetropolitanAreas/SaveAdd',
            params: {
                idMunicipio: $scope.idTown
            }
        }).
            success(function (data, status, headers, config) {
                $scope.saving = false;
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.getMetropolitanArea();
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).
            error(function (data, status, headers, config) {
                $scope.saving = false;
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.deleteMetropolitanArea = function (ID) {
        $http({
            method: 'DELETE',
            url: '../../../MetropolitanAreas/Delete',
            params: {
                id: ID
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.getMetropolitanArea();
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.getStates = function () {
        $http({
            method: 'GET',
            url: '../../../Users/GetStates'
        }).
            success(function (data, status, headers, config) {
                $scope.states = data;
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.getTowns = function () {
        $http({
            method: 'GET',
            url: '../../../Users/GetTowns',
            params: {
                idState: $scope.idState
            }
        }).
            success(function (data, status, headers, config) {
                $scope.towns = data;
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

});
angular.module("General").controller('BannersController', function (models, $scope, $http, $window, notify, $rootScope) {
    $scope.active = true;
    $scope.items = new Array();

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    //Código para el paginado
    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.Get();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.Get();
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
        if (n >= 0 && n < $scope.pageCount()) {
            $scope.currentPage = n;
            $scope.Get();
        }
    };

    $scope.Get = function () {
        $("#search").button("loading");

        $http({
            method: 'GET',
            url: '../../../Banners/Get',
            params: {
                name: $scope.name,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {
                $("#search").button("reset");

                if (data.success == 1) {
                    if (data.oData.banners.length > 0) {
                        $scope.banners = data.oData.banners;
                        $scope.total = data.oData.total;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.banners = data.oData.banners;
                        $scope.total = data.oData.total;
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

    $scope.Update = function (id) {
        window.location = "../../../Banners/Update?id=" + id;
    };

    $scope.Delete = function (id) {
        $http({
            method: 'DELETE',
            url: '../../../Banners/Delete',
            params: {
                id: id
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.Get();
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

    $scope.ZoomImage = function (imagen) {
        $scope.ZoomUrlImage = imagen;

        $("#modalZoomImage").modal("show");
    };


});

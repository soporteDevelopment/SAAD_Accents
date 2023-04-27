angular.module("General").controller('CatalogProviderController', function (models, $scope, $http, $window, notify, $rootScope) {
    $scope.active = true;

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
            url: '../../../CatalogProvider/GetAll',
            params: {
                Nombre: $scope.name,
                Correo: $scope.email,
                Telefono: $scope.phone,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {
                $("#search").button("reset");

                if (data.success == 1) {
                    if (data.oData.Providers.length > 0) {
                        $scope.providers = data.oData.Providers;
                        $scope.total = data.oData.Count;
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

    $scope.Update = function (id) {
        window.location = "../../../CatalogProvider/Update?id=" + id;
    };
    $scope.Delete = function (id) {
        $http({
            method: 'DELETE',
            url: '../../../CatalogProvider/Delete',
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
   
});

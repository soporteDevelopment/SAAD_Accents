angular.module("General").controller('CatalogsController', function (models, $scope, $http, $window, notify, $rootScope) {
    $scope.active = true;
    $scope.items = new Array();

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    $scope.Init = function () {
        $scope.GetProviders();
        $scope.GetCategories();
    };

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
            url: '../../../Catalogs/ListCatalogs',
            params: {
                code: $scope.code,
                model: $scope.model,
                volumen: $scope.volumen,
                idProvider: $scope.idProvider,
                idCategory: $scope.idCategory,
                idCatalogBrand:$scope.idCatalogBrand,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {
                $("#search").button("reset");

                if (data.success == 1) {
                    if (data.oData.Catalogs.length > 0) {
                        $scope.catalogs = data.oData.Catalogs;
                        $scope.total = data.oData.total;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.catalogs = data.oData.Catalogs;
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

    $scope.GetProviders = function () {
        $http({
            method: 'GET',
            url: '../../../CatalogProvider/GetActives',
            params: {
            }
        }).
            success(function (data, status, headers, config) {
                $scope.Providers = data.oData.Providers;
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.GetCategories = function () {
        $http({
            method: 'GET',
            url: '../../../CatalogCategory/GetAll'
        }).
            success(function (data, status, headers, config) {
                $scope.Categories = data.oData.Categories;
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.Update = function (id) {
        window.location = "../../../Catalogs/UpdateCatalog?id=" + id;
    };

    $scope.PrintTicket = function (id) {
        var amount = 1;
        angular.forEach($scope.catalogs, function (value, key) {
            if (value.idCatalogo == id) {
                amount = value.Cantidad;
                value.Cantidad = 0;
            }
        });

        if (amount > 0) {
            var win = window.open("../../../Catalogs/PrintTicket?idCatalogo=" + id + "&Cantidad=" + amount);
        }
    };

    $scope.Delete = function (id) {
        $http({
            method: 'DELETE',
            url: '../../../Catalogs/Delete',
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

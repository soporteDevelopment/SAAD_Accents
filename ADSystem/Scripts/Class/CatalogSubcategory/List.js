angular.module("General").controller('SubcategoriesController', function (models, subcategoryValidator, $scope, $http, $window, notify, $rootScope) {
    var arch = new FileReader();
    $scope.myFiles = [];

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

    $scope.listSubcategories = function () {
        $http({
            method: 'POST',
            url: '../../../CatalogSubcategory/ListSubcategories',
            params: {
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage,
                idCategory: $scope.idCategory
            }
        }).
        success(function (data, status, headers, config) {
            if (data.success == 1) {
                if (data.oData.Subcategories.length > 0) {
                    $scope.Subcategories = data.oData.Subcategories;
                    $scope.total = data.oData.Count;
                } else {
                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Subcategories = data.oData.Subcategories;
                    $scope.total = data.oData.Count;
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

    $scope.AddSubcategory = function (ID) {
        $window.location = '../../CatalogSubcategory/AddSubcategory?idCategory=' + ID;
    };

    $scope.UpdateSubcategory = function (ID) {
        $window.location = '../../CatalogSubcategory/UpdateSubcategory?idSubcategory=' + ID;
    };

    $scope.IndexSubcategories = function (ID) {
        $window.location = "../../../CatalogSubcategory/Index?idControl=" + ID;
    };

    $scope.DeleteSubcategory = function (ID) {
        $http({
            method: 'POST',
            url: '../../../CatalogSubcategory/DeleteSubcategory',
            params: {
                idSubcategory: ID
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.listSubcategories();
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
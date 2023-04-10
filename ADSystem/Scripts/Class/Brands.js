angular.module("General").controller('BrandsController', function (models, brandValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.brand = "";
    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.selectedController = 0;

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listBrands();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listBrands();
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
            $scope.listBrands();
        }

    };
    
    $scope.listBrands = function () {

        $http({
            method: 'POST',
            url: '../../../Brands/ListBrands',
            params: {
                brand: ($scope.brand == "" || $scope.brand == null) ? "" : $scope.brand,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                $scope.Brands = data.oData.Brands;
                $scope.total = data.oData.Count;

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });
    };
    //Redirecciona al momento de guardar
    $scope.AddBrand = function () {

        $window.location = '../../Brands/AddBrand';

    };

    $scope.valResult = {};

    $scope.newBrand = new models.Brand();

    $scope.SaveAddBrand = function () {

        $scope.valResult = brandValidator.validate($scope.newBrand);

        if ($scope.newBrand.$isValid) {

            $scope.SaveAdd();

        }

    },

    $scope.SaveAdd = function () {

        $http({
            method: 'POST',
            url: '../../../Brands/SaveAddBrand',
            params: {
                name: $scope.newBrand.name,
                description: $scope.newBrand.description
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $window.location = '../../../Brands/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.UpdateBrand = function (ID) {

        $window.location = '../../Brands/UpdateBrand?idBrand=' + ID;

    };

    $scope.SaveUpdateBrand = function (ID) {

        $scope.valResult = brandValidator.validate($scope.newBrand);

        if ($scope.newBrand.$isValid) {

            $scope.SaveUpdate(ID);

        }

    };

    $scope.SaveUpdate = function (ID) {

        $http({
            method: 'POST',
            url: '../../../Brands/SaveUpdateBrand',
            params: {
                idBrand: ID,
                name: $scope.newBrand.name,
                description: $scope.newBrand.description
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               notify(data.oData.Message, $rootScope.success);

               $window.location = '../../../Brands/Index';

           } else if (data.failure == 1) {

               notify(data.oData.Error, $rootScope.error);

           } else if (data.noLogin == 1) {

               window.location = "../../../Access/Close"

           }

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });
    };

});
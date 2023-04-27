angular.module("General").controller('SalesController', function ($scope, $http, $window, notify, $rootScope) {
       
    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
        
    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listEraserSales();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listEraserSales();
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
            $scope.listEraserSales();
        }
    };

    $scope.listEraserSales = function () {

        $("#searchSales").button("loading");

        $http({
            method: 'POST',
            url: '../../../Sales/GetEraserSales',
            params: {
                costumer: ($scope.sCustomer == "" || $scope.sCustomer == null) ? "" : $scope.sCustomer,
                project: ($scope.sProject == "" || $scope.sProject == null) ? "" : $scope.sProject,
                amazonas: $scope.sBranchAma,
                guadalquivir: $scope.sBranchGua,
                textura: $scope.sBranchTex,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            $("#searchSales").button("reset");

            if (data.success == 1) {

                if (data.oData.Sales.length > 0) {

                    $scope.Sales = data.oData.Sales;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Sales = data.oData.Sales;
                    $scope.total = data.oData.Count;

                }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#searchSales").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });
    };

    $scope.GetEraserSales = function (idEraserSale) {

        window.location = "../../../Sales/GetEraserSale?idEraserSale=" + idEraserSale;

    };

    $scope.DeleteEraserSale = function (idEraserSale, idBranch) {

        $http({
            method: 'POST',
            url: "../../../Sales/DeleteEraserSale",
            params: {
                idEraserSale: idEraserSale,
                idBranch: idBranch
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               notify(data.oData.Message, $rootScope.success);

               $scope.listEraserSales();

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

    $scope.PrintEraserSale = function (idEraserSale) {

        var URL = "../../../Sales/PrintEraserSale?idEraserSale=" + idEraserSale;

        var win = window.open(URL, '_blank');
        win.focus();

    };

});

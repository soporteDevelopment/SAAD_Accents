angular.module("General").controller('PhysicalController', function ($scope, $http, $window, notify, $rootScope) {
    
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

    $scope.listPhysicalCustomers = function () {

        $("#searchPhysicalCostumer").button("loading");

        $http({
            method: 'POST',
            url: '../../../Customers/ListPhysicalCustomers',
            params: {
                costumer: $scope.searchPhysicalCostumer,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            $("#searchPhysicalCostumer").button("reset");

            if (data.success == 1) {

                if (data.oData.Customers.length > 0) {

                    $scope.PCustomers = data.oData.Customers;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.PCustomers = data.oData.Customers;
                    $scope.total = data.oData.Count;

                }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#searchPhysicalCostumer").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });
    };

    $scope.UpdatePhysicalCustomer = function (ID) {

        $window.location = '../../Customers/UpdatePhysicalCustomer?idCustomer=' + ID;

    };

    //Delete Physical Customer

    $scope.DeletePhysicalCustomer = function (idCustomer) {

        var r = confirm("Esta seguro que desea eliminar al cliente?");

        if (r == true) {

            $http({
                method: 'POST',
                url: '../../../Customers/DeletePhysicalCustomer',
                params: {
                    idCustomer: idCustomer
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    $scope.listPhysicalCustomers();

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });
        }
    };

    $scope.$watch("currentPage", function (newValue, oldValue) {

        $scope.listPhysicalCustomers();

    });

});


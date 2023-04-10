angular.module("General")
    .controller('TotalSalesController', function (models, $scope, $http, $window, notify, $rootScope, $filter) {

        $scope.itemsPerPage = 500;
        $scope.currentPage = 0;
        $scope.total = 0;
        $scope.branch = 1;

        $scope.months = [
            { "Month": 1, "Name": "Enero" },
            { "Month": 2, "Name": "Febrero" },
            { "Month": 3, "Name": "Marzo" },
            { "Month": 4, "Name": "Abril" },
            { "Month": 5, "Name": "Mayo" },
            { "Month": 6, "Name": "Junio" },
            { "Month": 7, "Name": "Julio" },
            { "Month": 8, "Name": "Agosto" },
            { "Month": 9, "Name": "Septiembre" },
            { "Month": 10, "Name": "Octubre" },
            { "Month": 11, "Name": "Noviembre" },
            { "Month": 12, "Name": "Diciembre" },
        ];


        $scope.years = [
            2016,
            2017,
            2018,
            2019,
            2020,
            2021,
            2022,
            2023
        ]

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
                $scope.GenerarReporteVentas();
            }
        };

        $scope.LoadSales = function () {
            var sinceDate = (($scope.sinceMonth < 10) ? "0" + $scope.sinceMonth : $scope.sinceMonth) + "/01/" + $scope.sinceYear;
            var untilDate = (($scope.untilMonth < 10) ? "0" + $scope.untilMonth : $scope.untilMonth) + "/01/" + $scope.untilYear;
           
            $http({
                method: 'GET',
                url: '../../../SalesIntelligence/TotalSales',
                params: {
                    startDate: sinceDate,
                    endDate: untilDate
                }
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {                        
                        if (data.oData.Sales.length == 0) { notify('No se encontraron registros.', $rootScope.error); }
                        $scope.sales = data.oData.Sales;
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

        $scope.LoadUsers = function () {

            $http({
                method: 'POST',
                url: '../../../Users/ListTotalUsers'
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        $scope.users = data.oData.Users;
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

        $scope.DownloadSales = function () {
            var sinceDate = (($scope.sinceMonth < 10) ? "0" + $scope.sinceMonth : $scope.sinceMonth) + "/01/" + $scope.sinceYear;
            var untilDate = (($scope.untilMonth < 10) ? "0" + $scope.untilMonth : $scope.untilMonth) + "/01/" + $scope.untilYear;

            window.location = "/SalesIntelligence/TotalSalesXLS?startDate=" + sinceDate + "&endDate=" + untilDate;
        };

        $scope.ValidateDate = function (date) {
            var result = date;

            if (typeof date === 'object') {
                result = (((parseInt(date.getMonth() + 1)) < 10)
                            ? "0" + (date.getMonth() + 1)
                            : (date.getMonth() + 1)) +
                            "/" +
                            (((parseInt(date.getDate())) < 10) ? "0" + (date.getDate()) : (date.getDate())) +
                            "/" +
                            date.getFullYear();
            }

            return result;
        };

        $scope.setSelecBranch = function (a, b) {
            var index = $scope.arrayObjectIndexOf($scope.Branches, a, b);
            $scope.Branch = $scope.Branches[index];
        };
    });

jQuery.fn.reset = function () {
    $(this).each(function () { this.reset(); });
}
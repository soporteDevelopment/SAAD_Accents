angular.module("General")
    .controller('ViewsController', function (models, $scope, $http, $window, notify, $rootScope, $filter) {

        $scope.itemsPerPage = 500;
        $scope.currentPage = 0;
        $scope.total = 0;
        $scope.branch = 1;

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
            var idBranch = null;
            if ($scope.branch != 1) {
                idBranch = $scope.branch;
            }
            $http({
                method: 'GET',
                url: '../../../SalesIntelligence/Views',
                params: {
                    idSeller: $scope.seller,                    
                    startDate: $scope.dateSince,
                    endDate: $scope.dateUntil,
                    idBranch: idBranch,
                }
            }).
                success(function (data, status, headers, config) {                    
                    if (data.success == 1) {                        
                        if (data.oData.Views.length == 0) { notify('No se encontraron registros.', $rootScope.error); }
                        $scope.views = data.oData.Views;                        
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
            var idBranch = null;
            if ($scope.branch != 1) {
                idBranch = $scope.branch;
            }
            var startDate = $scope.ValidateDate($scope.dateSince);
            var endDate = $scope.ValidateDate($scope.dateUntil);

            window.location = "/SalesIntelligence/ViewsXLS?idSeller=" + $scope.seller + "&startDate=" + startDate + "&endDate=" + endDate + "&idBranch=" + idBranch;
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

        $scope.dateToday = new Date();

        $scope.toggleMin = function () {
            $scope.minDate = $scope.minDate ? null : new Date();
        };

        $scope.toggleMin();

        $scope.openSince = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedSince = true;
            $scope.openedUntil = false;
        };

        $scope.openUntil = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedSince = false;
            $scope.openedUntil = true;
        };

        $scope.openDatePayment = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedDatePayment = true;
        };

        $scope.dateOptions = {
            formatYear: 'yyyy',
            startingDay: 1
        };

        $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM/dd/yyyy'];
        $scope.format = $scope.formats[4];

    });

jQuery.fn.reset = function () {
    $(this).each(function () { this.reset(); });
}
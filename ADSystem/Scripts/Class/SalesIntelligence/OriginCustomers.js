angular.module("General")
    .controller('OriginCustomersController', function (models, $scope, $http, $window, notify, $rootScope, $filter) {

        $scope.LoadSales = function () {
            $("#searchReport").button("loading");

            $http({
                method: 'GET',
                url: '../../../SalesIntelligence/OriginCustomers',
                params: {
                    startDate: $scope.dateSince,
                    endDate: $scope.dateUntil
                }
            }).
                success(function (data, status, headers, config) {
                    $("#searchReport").button("reset");
                    if (data.success == 1) {
                        if (data.oData.SalesCustomers.length == 0) { notify('No se encontraron registros.', $rootScope.error); }
                        $scope.views = data.oData.SalesCustomers;
                    } else if (data.failure == 1) {
                        notify(data.oData.Error, $rootScope.error);
                    } else if (data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    }
                }).
                error(function (data, status, headers, config) {
                    $("#searchReport").button("reset");
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

            window.location = "/SalesIntelligence/OriginCustomersXLS?idSeller=" + $scope.seller + "&startDate=" + startDate + "&endDate=" + endDate + "&idBranch=" + idBranch;
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
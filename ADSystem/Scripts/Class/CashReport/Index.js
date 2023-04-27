angular.module("General").controller('ReportsController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.items = new Array();

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    //Código para el paginado
    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.GetReports();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.GetReports();
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
            $scope.GetReports();
        }
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
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM/dd/yyyy'];
    $scope.format = $scope.formats[4];

    $scope.GetReports = function () {

        $("#search").button("loading");

        $http({
            method: 'GET',
            url: '../../../CashReport/Get',
            params: {
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                remission : $scope.remission,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {
                $("#search").button("reset");

                if (data.success == 1) {
                    if (data.oData.Reports.length > 0) {
                        $scope.Reports = data.oData.Reports;
                        $scope.total = data.oData.Count;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.Reports = data.oData.Reports;
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

    $scope.DetailReport = function (id) {
        $scope.includeURLDetail = "GetDetail?id=" + id;
        $("#modalDetailReport").modal("show");
    };

    $scope.PrintReport = function (id) {
        var URL = '../../../CashReport/Print?id=' + id;

        var win = window.open(URL, '_blank');
        win.focus();
    };

    $scope.InitDetail = function (detail) {
        $scope.items = detail;
    };

    $scope.loadInit = function (report) {
        $scope.report = report;
    };

    $scope.UpdateStatus = function (id, status) {
        if (status != true) {
            $http({
                method: 'PATCH',
                url: '../../../CashReport/UpdateStatus',
                params: {
                    id: id
                }
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        angular.forEach($scope.Reports, function (report, key) {
                            if (report.idReporteCaja == id) {
                                report.Revisado = data.oData.Reviewed;
                            }
                        });
                    } else if (data.failure == 1) {
                        notify(data.oData.Error, $rootScope.error);
                    } else if (data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    }
                }).
                error(function (data, status, headers, config) {
                    notify("Ocurrío un error.", $rootScope.error);
                });
        }
    };
});

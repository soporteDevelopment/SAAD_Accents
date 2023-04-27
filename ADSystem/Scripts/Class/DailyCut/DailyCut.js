angular.module("General").controller('BranchOfficeController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.number = "";
    $scope.searchPayment = "false";
    $scope.branchOffices = [];
    $scope.sellers = [];
    $scope.totalPayment = 0;
    $scope.totalPaymentDay = 0;
    $scope.idBranchOffice = 1;


    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
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

    $scope.loadsellers = function () {

        $http({
            method: 'POST',
            url: '../../../Users/ListAllUsers'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.sellers = data.oData.Users;

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
    $scope.loadBranchsOffice = function () {

        $http({
            method: 'GET',
            url: '../../../BranchOffice/Get' 
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.branchOffices = data.oData.Branchs;
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
    $scope.GetReports = function () {

        $("#search").button("loading");

        $http({
            method: 'GET',
            url: '../../../SalesIntelligence/DailyCuttings',
            params: {
                queryDate: $scope.dateSince,
                idBranchOffice: $scope.idBranchOffice,
                idUser: ($scope.seller == "" || $scope.seller == null) ? "" : $scope.seller.idUsuario,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {
                $("#search").button("reset");

                if (data.success == 1) {
                    if (data.oData.Reports.length > 0) {
                        $scope.payments = data.oData.Reports;
                        $scope.total = data.oData.Count;
                        $scope.totalPaymentDay = 0;
                        for (var i = 0; i < $scope.payments.length; i++) {
                            for (var d = 0; d < $scope.payments[i].TerminalsBanks.length; d++) {
                                for (var e = 0; e < $scope.payments[i].TerminalsBanks[d].DailyCuttings.length; e++) {
                                    $scope.totalPaymentDay = $scope.totalPaymentDay + $scope.payments[i].TerminalsBanks[d].DailyCuttings[e].Cantidad;
                                }
                            }
                        }

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
    $scope.DownloadPayments = function () {
        var sinceDate = new Date($scope.dateSince).toDateString();
        var idUsuario = ($scope.seller == "" || $scope.seller == null) ? "" : $scope.seller.idUsuario;
       
        window.location = "/SalesIntelligence/DailyCutXLS?queryDate=" + sinceDate + "&idBranchOffice=" + $scope.idBranchOffice + "&idUser=" + idUsuario;
    };
    $scope.calculateTotalPayment = function (payments) {
        var totalPayment = 0;

        for (var i = 0; i < payments.length; i++) {
            totalPayment = totalPayment + payments[i].Cantidad;
        }

        return totalPayment;
        
    };
    
});

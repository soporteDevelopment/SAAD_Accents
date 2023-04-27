angular.module("General").controller('RepairsController', function (models, $scope, $http, $window, notify, $rootScope) {

    $scope.items = new Array();

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    $scope.InitList = function() {
        $scope.LoadSellers();
    };

    //Código para el paginado
    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.GetRepairs();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.GetRepairs();
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
            $scope.GetRepairs();
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

    $scope.GetRepairs = function () {

        $("#search").button("loading");

        $http({
            method: 'POST',
            url: '../../../Repairs/ListRepairs',
            params: {
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                number: $scope.number,
                iduser: ($scope.seller == "" || $scope.seller == null) ? "" : $scope.seller.idUsuario,
                code: ($scope.code == "" || $scope.code == null) ? "" : $scope.code,
                status: ($scope.status == undefined) ? 2 : $scope.status,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            $("#search").button("reset");

            if (data.success == 1) {

                if (data.oData.Repairs.length > 0) {

                    $scope.Repairs = data.oData.Repairs;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Repairs = data.oData.Repairs;
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

    $scope.LoadSellers = function () {

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

    $scope.DetailRepair = function (idRepair) {

        $scope.includeURLDetail = "RepairById?idRepair=" + idRepair;

        $("#modalDetailRepair").modal("show");

    };

    $scope.ChangeStatus = function (ID, status) {

        $scope.idRepair = ID;
        $scope.status = status;

        $("#openModalStatus").modal("show");

    };

    $scope.UpdateStatus = function () {

        $http({
            method: 'POST',
            url: '../../../Repairs/UpdateStatus',
            params: {
                idRepair: $scope.idRepair,
                status: $scope.status,
                comments: $scope.comments
            }
        }).
        success(function (data, status, headers, config) {

            $("#openModalStatus").modal("hide");

            if (data.success == 1) {

                $scope.GetRepairs();

                notify(data.oData.Message, $rootScope.success);

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

    $scope.UpdateRepair = function(idRepair) {
        $scope.includeURLRepair = "UpdateRepair?idRepair=" + idRepair;

        $("#modalUpdateRepair").modal("show");
    }

    $scope.PrintRepair = function (idRepair) {

        var URL = "../../../Repairs/Print?idRepair=" + idRepair;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.InitDetail = function (detail) {

        $scope.items = detail;

    };
});

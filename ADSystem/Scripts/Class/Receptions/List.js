angular.module("General").controller('ReceptionsController', function (models, $scope, $http, $window, notify, $rootScope) {
        
    $scope.items = new Array();

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    $scope.InitList = function () {

        $scope.LoadSellers();

    },

    //Código para el paginado
    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.GetReceptions();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.GetReceptions();
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
            $scope.GetReceptions();
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

    $scope.GetReceptions = function () {

        $("#search").button("loading");

        $http({
            method: 'POST',
            url: '../../../Receptions/Get',
            params: {
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                transfer: $scope.transfer,
                iduser: ($scope.seller == "" || $scope.seller == null) ? "" : $scope.seller.idUsuario,
                code: ($scope.code == "" || $scope.code == null) ? "" : $scope.code,
                status: ($scope.status == undefined) ? 1 : $scope.status,
                amazonas: $scope.amazonas,
                guadalquivir: $scope.guadalquivir,
                textura: $scope.textura,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            $("#search").button("reset");

            if (data.success == 1) {

                if (data.oData.Receptions.length > 0) {

                    $scope.Receptions = data.oData.Receptions;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Receptions = data.oData.Receptions;
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

    $scope.DetailReception = function (idReception) {

        $scope.includeURLDetail = "GetDetailReception?idReception=" + idReception;

        $("#modalDetailReception").modal("show");

    };

    $scope.ChangeStatus = function (ID, status) {

        $scope.idReception = ID;
        $scope.status = status;

        $("#openModalStatus").modal("show");

    };

    $scope.UpdateStatus = function () {

        $http({
            method: 'POST',
            url: '../../../Receptions/UpdateStatus',
            params: {
                idReception: $scope.idReception,
                status: $scope.status,
                comments: $scope.comments
            }
        }).
        success(function (data, status, headers, config) {

            $("#openModalStatus").modal("hide");

            if (data.success == 1) {

                $scope.GetReceptions();

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

    $scope.InitDetail = function (detail) {

        $scope.items = detail;

    };

    $scope.EditReception = function (idReception) {

        window.location = "../../../Receptions/Update?idReception=" + idReception;

    };

});

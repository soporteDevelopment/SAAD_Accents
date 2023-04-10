angular.module("General").controller('QuotationsController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.number = "";
    $scope.searchPayment = "false";

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

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listQuotations();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listQuotations();
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
            $scope.listQuotations();
        }
    };

    $scope.listQuotations = function (a) {

        if (a == 0) {
            $scope.currentPage = 0;
        }

        $("#searchQuotations").button("loading");

        $http({
            method: 'POST',
            url: '../../../Quotations/GetQuotations',
            params: {
                allTime: ($scope.searchSince == 2) ? true : false,
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                number: $scope.number,
                costumer: ($scope.sCustomer == "" || $scope.sCustomer == null) ? "" : $scope.sCustomer,
                iduser: ($scope.sseller == "" || $scope.sseller == null) ? "" : $scope.sseller.idUsuario,
                project: ($scope.sProject == "" || $scope.sProject == null) ? "" : $scope.sProject,
                amazonas: $scope.sBranchAma,
                guadalquivir: $scope.sBranchGua,
                textura: $scope.sBranchTex,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {

                $("#searchQuotations").button("reset");

                if (data.success == 1) {

                    if (data.oData.Quotations.length > 0) {

                        $scope.Quotations = data.oData.Quotations;
                        $scope.total = data.oData.Count;

                    } else {

                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.Quotations = data.oData.Quotations;
                        $scope.total = data.oData.Count;

                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }


            }).
            error(function (data, status, headers, config) {

                $("#searchQuotations").button("reset");

                notify("Ocurrío un error.", $rootScope.error);


            });
    };

    $scope.GetQuotation = function (idQuotation, idQuotationType) {
        if (idQuotationType == 1) {
            window.location = "../../../Quotations/GetQuotation?idQuotation=" + idQuotation;
        }
        else if (idQuotationType == 2) {
            window.location = "../../../Quotations/GetQuotationFromOutProductToSale?idQuotation=" + idQuotation;
        } else {
            window.location = "../../../Quotations/GetQuotationUnify?idQuotation=" + idQuotation;
        }
    };

    $scope.GetQuotationInDollar = function (idQuotation) {
        window.location = "../../../Quotations/GetQuotationInDollar?idQuotation=" + idQuotation;
    };

    $scope.GetEraserQuotation = function (idQuotation, idQuotationType) {
        if (idQuotationType == 1) {
            window.location = "../../../Quotations/GetEraserQuotation?idQuotation=" + idQuotation;
        } else if (idQuotationType == 2) {
            window.location = "../../../Quotations/GetEraserQuotationView?idQuotation=" + idQuotation;
        } else if (idQuotationType == 3) {
            window.location = "../../../Quotations/GetEraserQuotationUnify?idQuotation=" + idQuotation;
        }
    };

    $scope.GetEraserQuotationDollar = function (idQuotation, idSucursal) {
        window.location = "../../../Quotations/GetEraserQuotationDollar?idQuotation=" + idQuotation;
    };

    $scope.DeleteQuotation = function (idQuotation) {

        var r = confirm("Está seguro que desea eliminar esta cotización?");

        if (r == true) {

            $http({
                method: 'POST',
                url: "../../../Quotations/DeleteQuotation",
                params: {
                    idQuotation: idQuotation
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        notify(data.oData.Message, $rootScope.success);

                        $scope.listQuotations();

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

    $scope.PrintQuotation = function (idQuotation) {
        var URL = "../../../Quotations/PrintQuotation?idQuotation=" + idQuotation;

        var win = window.open(URL, '_blank');
        win.focus();
    };

    $scope.SendMail = function (idQuotation) {

        $scope.mailSale = idQuotation;

        $("#modalSendMail").modal("show");
    },

        $scope.AcceptSendMail = function () {

            if ($scope.txtSendMail != undefined && $scope.txtSendMail.length > 0) {

                $http({
                    method: 'POST',
                    url: '../../../Quotations/SendMailQuotationSaleAgain',
                    data: {
                        idQuotation: $scope.mailSale,
                        email: $scope.txtSendMail
                    }
                }).success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        $scope.txtSendMail = "";

                        notify(data.oData.Message, $rootScope.success);

                    } else if (data.failure == 1) {

                        notify(data.oData.Error, $rootScope.error);

                    } else if (data.noLogin == 1) {

                        window.location = "../../../Access/Close"

                    }

                }).error(function (data, status, headers, config) {

                    notify("Ocurrío un error.", $rootScope.error);

                });

            } else {

                notify("Ingrese una cuenta de correo", $rootScope.error);

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
    $scope.openPaymentDate = function (a, $event) {
        $event.preventDefault();
        $event.stopPropagation();

        angular.forEach($scope.TypesPayment, function (value, key) {

            if (value.idTypePayment != a.item.idTypePayment) {

                value.bCalendar = false;

            }

            angular.forEach(value.HistoryCredit, function (obj, key) {

                obj.bCalendar = false;

            });


        });

        a.item.bCalendar = true;
    };
    $scope.openPaymentDateHist = function (a, $event) {
        $event.preventDefault();
        $event.stopPropagation();

        angular.forEach($scope.TypesPayment, function (value, key) {

            if (value.idTypePayment != a.item.idTypePayment) {

                value.bCalendar = false;

            }

            angular.forEach(value.HistoryCredit, function (obj, key) {

                obj.bCalendar = false;

            });

        });

        a.hist.bCalendar = true;
    };

    $scope.openedPaymentDateDetail = {
        opened: false
    };

    $scope.open1 = function () {
        $scope.openedPaymentDateDetail.opened = true;
    };

    $scope.openMaxPayment = function (a, $event) {
        $event.preventDefault();
        $event.stopPropagation();

        a.item.openedMaxPayment = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM/dd/yyyy'];
    $scope.format = $scope.formats[4];

    $scope.getDate = function (date) {
        var res = "";

        if (date != null) {
            if (date.length > 10) {
                res = date.substring(6, 19);
            }
        }

        return res;
    };
    $scope.CreateDuplicate = function (idQuotation) {
        window.location = "../../../Quotations/CreateDuplicateQuotation?idQuotation=" + idQuotation;
    };
});

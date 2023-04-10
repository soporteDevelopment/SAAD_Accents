angular.module("General").controller('SalesOnLineController', function (models, $scope, $http, $window, notify, $rootScope) {
    $scope.includeURL = null;
    $scope.shoppingCart = {};
    $scope.active = true;
    $scope.items = new Array();

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    $scope.idStatus = 1;
    $scope.status = null;
    $scope.customer = "";

    $scope.optionsStatus = [
        { value: 1, text: "PENDIENTE POR CONFIRMAR" },
        { value: 2, text: "COMPRA CONFIRMADA" },
        { value: 3, text: "LISTO PARA RECOGER EN SUCURSAL" },
        { value: 4, text: "COMPRA ENVIADA" },
        { value: 5, text: "COMPRA ENTREGADA" },
        { value: 6, text: "COMPRA CANCELADA" }
    ]

    //Código para el paginado
    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.Get();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.Get();
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
            $scope.Get();
        }
    };

    $scope.getSales = function () {
        $("#search").button("loading");

        $scope.sales = null;
        $scope.total = 0;

        $http({
            method: 'GET',
            url: '../../../SalesOnLine/GetAll',
            params: {
                start: $scope.dateSince,
                end: $scope.dateUntil,
                status: $scope.idStatus,
                customer: $scope.customer,
                user: $scope.user,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {
                $("#search").button("reset");

                if (data.success == 1) {
                    if (data.oData.sales.length > 0) {
                        $scope.sales = data.oData.sales;
                        $scope.total = data.oData.total;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.customers = data.oData.customers;
                        $scope.total = data.oData.total;
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

    $scope.openModalStatus = function (id, idStatus, idBranch) {
        $scope.idSale = id;
        $scope.status = idStatus;
        $scope.branch = idBranch;
        $("#openModalStatus").modal("show");
    };

    $scope.updateStatus = function () {
        $http({
            method: 'PATCH',
            url: '../../../SalesOnLine/Status',
            params: {
                id: $scope.idSale,
                status: $scope.status,
                branch: $scope.branch
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $("#openModalStatus").modal("hide");
                    $scope.getSales();
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

    $scope.updateBillNumber = function () {
        $http({
            method: 'PATCH',
            url: '../../../SalesOnLine/Bill',
            params: {
                id: $scope.sale.IdVenta,
                bill: $scope.bill
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
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

    $scope.updateAssignedUser = function () {
        $http({
            method: 'PATCH',
            url: '../../../SalesOnLine/AssignedUser',
            params: {
                id: $scope.sale.IdVenta,
                idUser: $scope.idUser
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
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

    $scope.updateSendingData = function () {
        $http({
            method: 'PATCH',
            url: '../../../SalesOnLine/UpdateSendingData',
            params: {
                id: $scope.sale.IdVenta,
                sendingProvider: $scope.sendingProvider,
                guideNumber: $scope.guideNumber
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
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

    $scope.print = function (id) {
        var URL = "../../../SalesOnLine/Print?id=" + id;
        var win = window.open(URL, '_blank');
        win.focus();
    };

    $scope.loadUsers = function () {
        $http({
            method: 'POST',
            url: '../../../Users/ListAllUsers'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.Users = data.oData.Users;
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

    //Detail ShoppingCart
    $scope.getDetail = function (id) {

        $http({
            method: 'GET',
            url: '../../../SalesOnLine/Get',
            params: {
                id: id
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {
                    if (data.oData.sale != null && data.oData.sale != undefined) {
                        $scope.sale = data.oData.sale;
                        $scope.idUser = data.oData.sale.IdVendedor;
                        $scope.bill = data.oData.sale.NumeroFactura;
                        $scope.loadUsers();
                        $("#modalSaleOnLine").modal("show");
                    } else {
                        notify('No existe carrito de compras.', $rootScope.error);
                    }
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
});

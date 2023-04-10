angular.module("General").controller('CatalogViewController', function (models, $scope, $http, $window, notify, $rootScope) {
    $scope.status = 2;
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

    $scope.GetSellers = function () {
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

    $scope.GetViews = function () {
        $("#search").button("loading");

        $http({
            method: 'GET',
            url: '../../../CatalogView/GetAll',
            params: {
                DateSince: $scope.dateSince,
                DateUntil: $scope.dateUntil,
                Number: $scope.number,
                Customer: $scope.customer,
                CatalogCode: $scope.catalogCode,
                Status: $scope.status,
                Page: $scope.currentPage,
                PageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {
                $("#search").button("reset");

                if (data.success == 1) {
                    if (data.oData.Catalogs.length > 0) {
                        $scope.Catalogs = data.oData.Catalogs;
                        $scope.total = data.oData.Count;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.Catalogs = data.oData.Catalogs;
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

    $scope.GetDetail = function (id) {
        var d = new Date();
        $scope.includeURLCatalogViewDetail = "GetDetailById?id=" + id + "&date=" + d.getTime();
        $("#ModalDetailCatalogView").modal("show");
    };

    $scope.Print = function (id) {
        var URL = '../../../CatalogView/Print?id=' + id;
        var win = window.open(URL, '_blank');
        win.focus();
    };

    $scope.InitDetail = function (detail) {
        $scope.User = {
            idUser: ""
        };
        $scope.GetSellers();
        $scope.items = detail;
    };

    $scope.SetStatus = function (id) {
        $scope.idView = id;
        $scope.status = null;

        $("#ModalCatalogViewStatus").modal("show");
    };

    $scope.UpdateStatus = function () {
        if (status != true) {
            $http({
                method: 'PATCH',
                url: '../../../CatalogView/UpdateStatus',
                params: {
                    id: $scope.idView,
                    status: $scope.status
                }
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        $scope.GetViews();
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

    $scope.SaveReturn = function (id) {
        var catalogs = new Array();

        angular.forEach($scope.items, function (value, key) {
            if (value.CantidadDevolucion > 0) {
                var catalog = [value.idDetalleVista, value.CantidadDevolucion];
                catalogs.push(catalog);
            }
        });

        if (catalogs.length > 0) {
            $http({
                method: 'POST',
                url: '../../../CatalogView/Return',
                data: {
                    id: id,
                    catalogs: catalogs
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
        }

        $("#ModalDetailCatalogView").modal("hide");
    };

    $scope.SendEmail = function (id) {
        var r = confirm("¡Desea enviar de nuevamente el correo al cliente?");

        if (r == true) {
            $http({
                method: 'GET',
                url: '../../../CatalogView/Email?id=' + id
            }).success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
        }
    };
});

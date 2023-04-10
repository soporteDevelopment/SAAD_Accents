angular.module("General").controller('OutProductsController', function (models, ServiceSaleValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.selectedCode = "";
    $scope.branch = "";
    $scope.datetime = new Date();
    $scope.items = new Array();
    $scope.customer = {

        type: "moral"

    };
    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.remision = "";
    $scope.cliente = "";
    $scope.fecha = null;
    $scope.cantidad = 0;
    $scope.estatus = "";
    $scope.outProducts = new Array();
    $scope.includeURLMoral = "";
    $scope.includeURLPhysical = "";
    $scope.barcode = {};
    $scope.branchID = null;
    $scope.Supervisor = {

        idSupervisor: ""

    };

    //Codigo para el scaner
    angular.element(document).ready(function () {
        angular.element(document).keydown(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);

            if (code == 13)// Enter key hit
            {
                $scope.GetProductForId();
                $scope.barcode = "";
            }
            else {
                $scope.barcode = $scope.barcode + String.fromCharCode(code);
            }
        });
    });

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listViews();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listViews();
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
            $scope.listViews();
        }
    };

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

    $scope.LoadUsers = function () {

        $http({
            method: 'POST',
            url: '../../../Users/ListAllUsers'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.usersOne = data.oData.Users;

                    if ($scope.outSellerOne != undefined) {

                        $scope.setSelecUsersOne($scope.outSellerOne);

                        $scope.LoadUsersButNot();
                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.LoadCustomers = function () {

        $scope.LoadPhysicalCustomers();
        $scope.LoadMoralCustomers();

    };

    $scope.LoadPhysicalCustomers = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllPhysicalCustomers'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.physicalCustomers = data.oData.Customers;

                    if ($scope.customer.type == "physical") {

                        ($scope.outCustomer != undefined || $scope.outCustomer != null) ? $scope.setSelecCustomerPhysical($scope.outCustomer) : "";

                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.LoadMoralCustomers = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllMoralCustomers'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.moralCustomers = data.oData.Customers;

                    if ($scope.customer.type == "moral") {

                        ($scope.outCustomer != undefined || $scope.outCustomer != null) ? $scope.setSelecCustomerMoral($scope.outCustomer) : "";

                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.LoadOffices = function () {

        $http({
            method: 'POST',
            url: '../../../Offices/ListAllOffices'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.offices = data.oData.Offices;

                    if ($scope.customer.type == "office") {

                        ($scope.outOffice != undefined || $scope.outOffice != null) ? $scope.setSelecCustomerOffice($scope.outCustomer) : "";

                    }

                    ($scope.outOffice != undefined || $scope.outOffice != null) ? $scope.setSelecOffices($scope.outOffice) : "";

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    //Eliminar
    $scope.loadViewDetails = function (val) {
        $scope.selectedView = val.idVista;
        $scope.loadDetails();
    };

    $scope.CloseViewDetails = function () {
        $("#modalViewDetails").modal("hide");
    };

    //Eliminar
    $scope.loadDetails = function () {
        $scope.listOfDetails = null;

        $http({
            method: 'POST',
            url: '../../../OutProducts/GetOutProductsDetails',
            params: {
                idVista: $scope.selectedView
            }

        }).success(function (data, status, headers, config) {
            if (data.success == 1) {
                $scope.listOfDetails = data.oData.outProductsDetails;

                $("#modalViewDetails").modal("show");

            } else if (data.failure == 1) {
                notify(data.oData.Error, $rootScope.error);
            } else if (data.noLogin == 1) {
                window.location = "../../../Access/Close"
            }
        }).error(function (data, status, headers, config) {
            notify("Ocurrío un error.", $rootScope.error);
        });
    };

    //Eliminar
    $scope.Vistas = function () {

        $http({
            method: 'POST',
            url: '../../../OutProducts/GetVistas',
            params: {
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).success(function (data, status, headers, config) {
            if (data.success == 1) {
                if (data.oData.listOfVistas.length > 0) {
                    $scope.listOfVistas = data.oData.listOfVistas;
                    //$scope.total = data.oData.Count;
                } else {
                    notify('No se encontraron registros.', $rootScope.error);
                }
            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }
        }).error(function (data, status, headers, config) {
            notify("Ocurrío un error.", $rootScope.error);
        });

    };

    //Metodo consumido al usar los filtros del listado
    $scope.listViews = function (a, b) {

        if (a != undefined && b != undefined) {

            $scope.currentPage = 0;
            $scope.itemsPerPage = 20;
        }

        $("#searchViews").button("Buscando...");

        $http({
            method: 'POST',
            url: '../../../OutProducts/ListOutProducts',
            params: {
                allTime: ($scope.searchSince == 2) ? true : false,
                fechaInicial: $scope.dateSince,
                fechaFinal: $scope.dateUntil,
                cliente: ($scope.sCustomer == "" || $scope.sCustomer == null) ? "" : $scope.sCustomer,
                iduser: ($scope.sseller == "" || $scope.sseller == null) ? "" : $scope.sseller.idUsuario,
                producto: ($scope.selectCode == "" || $scope.selectCode == null) ? "" : $scope.selectCode,
                remision: ($scope.sRemision == "" || $scope.sRemision == null) ? "" : $scope.sRemision,
                project: ($scope.sProject == "" || $scope.sProject == null) ? "" : $scope.sProject,
                status: ($scope.sStatusOutProduct == undefined) ? 1 : $scope.sStatusOutProduct,
                amazonas: $scope.sBranchAma,
                guadalquivir: $scope.sBranchGua,
                textura: $scope.sBranchTex,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {

                $("#searchViews").button("reset");

                if (data.success == 1) {

                    if (data.oData.outProducts.length > 0) {

                        $scope.Products = data.oData.outProducts;
                        $scope.total = data.oData.Count;

                    } else {

                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.Products = data.oData.outProducts;
                        $scope.total = data.oData.Count;

                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                $("#searchProduct").button("reset");

                notify("Ocurrío un error.", $rootScope.error);

            });
    };

    $scope.Print = function () {

        var URL = '../../../OutProducts/PrintOutProducts?remision=' + $scope.oremision;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.PrintPending = function (remision) {

        var URL = '../../../OutProducts/PrintPendingOutProducts?remision=' + remision;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.ChangeStatus = function (ID, status) {
        $scope.idOutProductsStatus = ID;
        $scope.statusOutProducts = status;

        if (status == 1) {
            $("#openModalStatus").modal("show");
        } else {
            notify("La Salida a Vista ya se encuentra Cancelada", $rootScope.error);
        }
    };

    $scope.SetStatus = function () {

        $http({
            method: 'POST',
            url: '../../../OutProducts/SetStatus',
            params: {
                idOutProducts: $scope.idOutProductsStatus,
                status: $scope.statusOutProducts
            }
        }).
            success(function (data, status, headers, config) {

                $("#openModalStatus").modal("hide");

                if (data.success == 1) {

                    $scope.listViews();

                    notify(data.oData.Message, $rootScope.success);

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.DetailOutProducts = function (remision) {
        var d = new Date();

        $scope.includeURL = "DetailOutProducts?remision=" + remision + "&date=" + d.getTime();

        $scope.Supervisor = {
            idSupervisor: ""
        };

        $("#modalDetailOutProductsOnLine").modal("show");
    };

    $scope.PrintOutProducts = function (remision) {
        var URL = '../../../OutProducts/PrintOutProducts?remision=' + remision;

        var win = window.open(URL, '_blank');
        win.focus();
    };

    $scope.init = function (detail, branchID) {
        $scope.branchID = branchID;
        $scope.items = detail;
    };

    $scope.saveUpdateStock = function (ID) {
        $("#saveUpdateStock").button("loading");

        if ($scope.validateAmount()) {
            $scope.saveUpdateReturnStock(ID);
        }

        $("#saveUpdateStock").button("reset");
    };

    $scope.saveUpdateReturnStock = function (ID) {

        var products = new Array();

        angular.forEach($scope.items, function (value, key) {
            if (value.numDevolucion > 0) {
                var product = [value.idProducto, value.numDevolucion];
                products.push(product);
            }
        });

        if ($scope.Supervisor.idSupervisor == undefined || $scope.Supervisor.idSupervisor == null || $scope.Supervisor.idSupervisor == "") {
            notify("Indique quien realizó la verificación.", $rootScope.error);
        } else {
            if (products.length > 0) {
                $http({
                    method: 'POST',
                    url: '../../../OutProducts/UpdateStockReturnOutProducts',
                    data: {
                        idOutProducts: ID,
                        lProducts: products,
                        idUser: $scope.Supervisor.idSupervisor.idUsuario
                    }
                }).
                    success(function (data, status, headers, config) {
                        if (data.success == 1) {
                            notify(data.oData.Message, $rootScope.success);
                            $scope.saveUpdateSalesStock(ID);
                        } else if (data.failure == 1) {
                            notify(data.oData.Error, $rootScope.error);
                        } else if (data.noLogin == 1) {
                            window.location = "../../../Access/Close";
                        }
                    }).
                    error(function (data, status, headers, config) {
                        notify("Ocurrío un error.", $rootScope.error);
                    });
            } else {
                $scope.saveUpdateSalesStock(ID);
            }

            $("#modalDetailOutProductsOnLine").modal("hide");
        }
    };

    $scope.saveUpdateSalesStock = function (ID) {
        var sProducts = "";

        angular.forEach($scope.items, function (value, key) {
            if (value.numVenta > 0) {
                if (sProducts != "") {
                    sProducts = sProducts + "|" + value.idProducto + "-" + value.numVenta;
                } else {
                    sProducts = "" + value.idProducto + "-" + value.numVenta + "";
                }
            }

        });

        if (sProducts.length > 0) {
            $window.location = "../../../Sales/IndexOutProductSale?idOutProduct=" + ID + "&idUserChecker=" + $scope.Supervisor.idSupervisor.idUsuario + "&lProducts=" + sProducts;
        }
    };

    $scope.SendMail = function (remision) {
        $scope.mailRemision = remision;
        $("#modalSendMail").modal("show");
    };

    $scope.AcceptSendMail = function () {
        if ($scope.txtSendMail != undefined && $scope.txtSendMail.length > 0) {

            $http({
                method: 'POST',
                url: '../../../OutProducts/SendMailOutProductsAgain',
                data: {
                    remision: $scope.mailRemision,
                    email: $scope.txtSendMail
                }
            }).success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.txtSendMail = "";
                    notify(data.oData.Message, $rootScope.success);
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }

            }).error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });

        } else {
            notify("Ingrese una cuenta de correo", $rootScope.error);
        }
    };

    $scope.GetProductForId = function () {
        var res = $scope.barcode.match(/^\d{12}$/);

        if (($scope.barcode.length == 12) && (res.length > 0))// Enter key hit
        {
            res = res[0].substr(0, res[0].length - 1);
            res = parseInt(res);

            var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: res }));

            if (index >= 0) {
                if ($("#dev" + $scope.items[index].idDetalleVista).val() == "") {
                    $("#dev" + $scope.items[index].idDetalleVista).val(0);
                }

                if ($scope.items[index].Pendiente > ($scope.items[index].numDevolucion + $scope.items[index].numVenta)) {
                    var value = parseInt($("#dev" + $scope.items[index].idDetalleVista).val());
                    $("#dev" + $scope.items[index].idDetalleVista).val(value + 1);

                    $scope.items[index].numDevolucion = parseInt($("#dev" + $scope.items[index].idDetalleVista).val());
                }
            }

            $scope.LostFocus();
        }
    };

    $scope.LostFocus = function () {
        $('input').each(function () {
            $(this).trigger('blur');
        });
    };

    $scope.EditHeaderView = function (idView) {
        $scope.includeURLEditView = "../../OutProducts/EditHeaderView?idView=" + idView;
        $("#modalEditView").modal("show");
    };

    $scope.createQuotations = function (remision) {
        window.location = "../../../Quotations/CreateQuotationFromOutProduct?remision=" + remision;
    }

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

    $scope.validateAmount = function () {
        result = true;
        angular.forEach($scope.items, function (value, key) {
            var total = value.numDevolucion + value.numVenta;
            if ((value.Pendiente < total) || (isNaN(total))) {
                value.numDevolucion = 0;
                value.numVenta = 0;
                notify("Valide las cantidades de articulo " + value.Codigo, $rootScope.error);
                result = false;
            }
        });

        return result;
    };
});

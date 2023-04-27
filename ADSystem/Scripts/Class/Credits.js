angular.module("General").controller('CreditsController', function (models, CreditValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.openedPaymentDate = false;
    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.customer = "moral";
    $scope.typeCredit = "remision";
    $scope.includeURL = "";

    $scope.items = new Array();

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listCredits();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listCredits();
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
            $scope.listCredits();
        }
    };

    $scope.listCredits = function () {

        $http({
            method: 'POST',
            url: '../../../Credits/ListCredits',
            params: {
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                remision: $scope.sRemision,
                costumer: ($scope.sCustomer == "" || $scope.sCustomer == null) ? "" : $scope.sCustomer,
                codigo: ($scope.sCodigo == "" || $scope.sCodigo == null) ? "" : $scope.sCodigo,
                status: ($scope.sStatusCredit == undefined) ? 1 : $scope.sStatusCredit,
                amazonas: $scope.sBranchAma,
                guadalquivir: $scope.sBranchGua,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    if (data.oData.Credits.length > 0) {

                        $scope.Credits = data.oData.Credits;
                        $scope.total = data.oData.Count;

                    } else {

                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.Credits = data.oData.Credits;
                        $scope.total = data.oData.Count;

                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

            });
    };

    $scope.valResult = {};

    $scope.newCredit = new models.Credit();

    $scope.AddCredit = function () {

        $window.location = '../../Credits/AddCredit';

    };

    $scope.DetailSaleForRemision = function () {

        $http({
            method: 'POST',
            url: '../../../Sales/DetailSaleForRemision',
            data: {
                remision: $scope.remision
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.items.length = 0;

                    if (data.oData.Sale != null) {

                        angular.forEach(data.oData.Sale.oDetail, function (value, key) {

                            $scope.items.push({
                                idProducto: value.idProducto,
                                idServicio: value.idServicio,
                                Codigo: value.Codigo,
                                Descripcion: value.Descripcion,
                                Precio: value.Precio,
                                Cantidad: value.Cantidad,
                                Descuento: (data.oData.Sale.Descuento > 0) ? value.Descuento + data.oData.Sale.Descuento : value.Descuento,
                                Checked: false,
                                Amazonas: 0,
                                Guadalquivir: 0,
                                Textura: 0,
                                Service: (value.idServicio > 0) ? true : false,
                                CantDev: 0,
                                NotaCredito: value.NotaCredito
                            });

                        });

                    }

                    $scope.sale = data.oData.Sale;

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

        $scope.remision = "";

        $scope.LoadPhysicalCustomers();
        $scope.LoadMoralCustomers();
        $scope.LoadOfficeCustomers();

    };

    $scope.LoadPhysicalCustomers = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllPhysicalCustomers'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.physicalCustomers = data.oData.Customers;

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

    $scope.LoadOfficeCustomers = function () {

        $http({
            method: 'POST',
            url: '../../../Offices/ListAllOffices'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.officeCustomers = data.oData.Offices;

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

    $scope.SaveAddCredit = function () {
        $scope.valResult = CreditValidator.validate($scope.newCredit);

        if ($scope.newCredit.$isValid) {
            $scope.SaveAdd();
        }
    };

    $scope.SaveAdd = function () {
        var creditNoteType = 2;

        if ($scope.typeCredit == 'costumer' && (($scope.physicalCustomer == undefined) && ($scope.moralCustomer == undefined) && ($scope.officeCustomer == undefined))) {
            notify("Seleccione un cliente.", $rootScope.error);
        } else if ($scope.typeCredit == 'remision' && $scope.remision == '') {
            notify("Indique la remisión.", $rootScope.error);
        } else { 
            if ($scope.typeCredit == 'costumer') {
                creditNoteType = 5;
            } else {
                creditNoteType = 2;
            }

            var products = new Array();

            angular.forEach($scope.items, function (value, key) {
                if (value.Checked == true) {
                    if (value.Service == false) {
                        products.push({
                            idProducto: value.idProducto,
                            Amazonas: value.Amazonas,
                            Guadalquivir: value.Guadalquivir,
                            Textura: value.Textura,
                            Tipo: 1
                        });
                    } else {
                        products.push({
                            idServicio: value.idServicio,
                            Cantidad: value.CantDev,
                            Tipo: 2
                        });
                    }
                }
            });

            if ($scope.typeCredit == 'remision' && (products.length == null || products.length == 0)) {
                notify("Seleccione los productos o servicios.", $rootScope.error);
            } else {

                $("#SaveAddCredit").button("loading");

                $http({
                    method: 'POST',
                    url: '../../../Credits/SaveAddCredit',
                    data: {
                        idCreditNoteType: creditNoteType,
                        idSale: ($scope.sale != undefined) ? $scope.sale.idVenta : null,
                        idCustomerP: ($scope.typeCredit == 'costumer' && $scope.customer == "physical") ? $scope.physicalCustomer.idCliente : null,
                        idCustomerM: ($scope.typeCredit == 'costumer' && $scope.customer == "moral") ? $scope.moralCustomer.idCliente : null,
                        idOffice: ($scope.typeCredit == 'costumer' && $scope.customer == "office") ? $scope.officeCustomer.idDespacho : null,
                        idSeller: $scope.idSeller,
                        amount: $scope.newCredit.Amount,
                        dtDate: $scope.dateSince,
                        comments: $scope.comments,
                        lProducts: products
                    }
                }).
                    success(function (data, status, headers, config) {

                        $("#SaveAddCredit").button("reset");

                        if (data.success == 1) {

                            $scope.remision = data.oData.sRemision;
                            $scope.finalDate = data.oData.finalDate;
                            $scope.idCreditNote = data.oData.idCreditNote;

                            $('#modalCredit').on('hidden.bs.modal', function (e) {

                                location.reload();

                            });

                            $("#modalCredit").modal("show");

                        } else if (data.failure == 1) {

                            notify(data.oData.Error, $rootScope.error);

                        } else if (data.noLogin == 1) {

                            window.location = "../../../Access/Close"

                        }

                    }).
                    error(function (data, status, headers, config) {

                        $("#SaveAddCredit").button("reset");

                        notify("Ocurrío un error.", $rootScope.error);

                    });
            }

        }

    };

    $scope.openModalStatus = function (ID, status) {

        $scope.idCreditNote = ID;
        $scope.statusCredit = status;

        $("#openModalStatus").modal("show");

    };

    $scope.SetStatus = function () {

        $http({
            method: 'POST',
            url: '../../../Credits/UpdateStatus',
            data: {
                idCreditNote: $scope.idCreditNote,
                status: $scope.statusCredit
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

                $("#openModalStatus").modal("hide");

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.calculateAmount = function () {

        $scope.newCredit.Amount = 0;

        angular.forEach($scope.items, function (value, key) {

            if (value.Checked == true) {

                var cantDev = 0;

                if (value.Service == false) {

                    cantDev = value.Amazonas + value.Guadalquivir + value.Textura;

                } else {

                    cantDev = value.CantDev;

                }

                var cost = cantDev * value.Precio;

                var percentage = value.Descuento / 100;

                var discount = cost * percentage;

                $scope.newCredit.Amount = $scope.newCredit.Amount + (cost - (discount));

            }

        });

    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM/dd/yyyy'];
    $scope.format = $scope.formats[4];

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

    $scope.openPaymentDate = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedPaymentDate = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.PrintCredit = function (idCreditNote) {
        var now = new Date();
        var URL = '../../../Credits/PrintCredit?idCreditNote=' + idCreditNote + "&update=" + now;
        var win = window.open(URL, '_blank');
        win.focus();
    };

    $scope.DetailCredit = function (idNotaCredito) {
        $scope.includeURL = "DetailCredit?idNotaCredito=" + idNotaCredito;
        $("#modalDetailCredit").modal("show");
    };

    $scope.GetDetail = function (id) {
        $http({
            method: 'GET',
            url: '../../../Credits/GetDetail',
            params: {
                idNotaCredito: id
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.products = data.oData.Detail; 
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

});
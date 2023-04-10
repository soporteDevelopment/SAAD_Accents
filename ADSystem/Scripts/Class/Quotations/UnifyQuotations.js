angular.module("General").controller('QuotationController', function (models, ServiceSaleValidator, GUID, GetDiscount, GetDiscountOffice, GetDiscountSpecial, $scope, $http, $window, notify, $rootScope, FileUploader) {

    $scope.selectedCode = "";
    $scope.branch = "";
    $scope.branchID = 0;
    $scope.customer = "";
    $scope.items = new Array();
    $scope.lProductsOut = new Array();
    $scope.subTotal = 0;
    $scope.checkedDiscount = false;
    $scope.checkedIVA = false;
    $scope.IVATasa = 0;
    $scope.discount = 0;
    $scope.total = 0;
    $scope.countService = 0;
    $scope.resCredit = 0;
    $scope.msgTypesPayment = false;
    $scope.includeURLMoral = "";
    $scope.includeURLPhysical = "";
    $scope.barcode = "";
    $scope.paymentLeft = 0;
    $scope.DatePaymentResult = "";
    $scope.Math = window.Math;
    $scope.myFiles = [];

    $scope.branchadd = {
        prod: "A"
    };
    $scope.branchID = 2;

    //Codigo para el scaner
    //angular.element(document).ready(function () {

    //    angular.element(document).keydown(function (e) {

    //        var code = (e.keyCode ? e.keyCode : e.which);

    //        if (code == 13)// Enter key hit
    //        {

    //            $scope.GetProductForId();

    //            $scope.barcode = "";

    //        }
    //        else {

    //            $scope.barcode = $scope.barcode + String.fromCharCode(code);

    //        }

    //    });

    //});   

    //Inicializa la variable CUSTOMER que se utiliza al momento de hacer una venta en el listado de tipo de cliente
    $scope.customer = {
        type: "moral"
    };

    //Indica en que sucursal está logueado el usuario al momento de hacer la venta
    $scope.SetBranch = function (ID) {

        if (ID == 1) {

            setTimeout(function () {

                $("#openModal").modal("show");

            }, 0);

        } else {

            $scope.branchID = ID;

        }

    };

    //Obtiene la información de una sucursal
    $scope.GetBranchInfo = function (IDBranch) {

        $http({
            method: 'GET',
            url: '../../../Branch/GetById',
            params: { id: IDBranch }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {
                    if (data.oData.Branch.IVATasa > $scope.IVATasa) {
                        $scope.branch = data.oData.Branch.Nombre;
                        $scope.IVATasa = data.oData.Branch.IVATasa;
                        $scope.branchID = data.oData.Branch.idSucursal;
                    }

                    if ($scope.checkedIVA) {
                        $scope.CalculateTotalCost();
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

    $scope.GetNumberQuotation = function () {

        $http({
            method: 'POST',
            url: '../../../Quotations/GeneratePrevNumberRem'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.number = data.oData.sNumber;

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

    $scope.AddMoralCustomer = function () {
        var now = new Date();

        $scope.includeURLMoral = "../../Customers/PartialAddMoralCustomer?update=" + now;

        $("#openModalAddMoralCustomer").modal("show");

        $('#openModalAddMoralCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadCustomers();
        })

    };

    $scope.AddPhysicalCustomer = function () {
        var now = new Date();

        $scope.includeURLPhysical = "../../Customers/PartialAddPhysicalCustomer?update=" + now;

        $("#openModalAddPhysicalCustomer").modal("show");

        $('#openModalAddPhysicalCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadCustomers();
        })

    };

    $scope.AddOffice = function () {
        var now = new Date();

        $scope.includeURLOffice = "../../Offices/PartialAddOffice?update=" + now;

        $("#openModalAddOfficeCustomer").modal("show");

        $('#openModalAddOfficeCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadOffices();
        })

    };

    $scope.GetProduct = function () {

        $http({
            method: 'POST',
            url: '../../../Products/GetProductQuotation',
            params: {
                idProduct: ($scope.selectedCode == "" || $scope.selectedCode == null) ? "" : $scope.selectedCode.originalObject.idProducto
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    var result = _.find($scope.items, function (chr) {
                        return chr.idProducto == data.oData.Product.idProducto && chr.idSucursal == $scope.branchID;
                    });

                    if (result == undefined) {
                        $scope.items.push({
                            idProducto: data.oData.Product.idProducto,
                            idProveedor: data.oData.Product.idProveedor,
                            imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                            codigo: data.oData.Product.Codigo,
                            desc: data.oData.Product.Descripcion,
                            prec: data.oData.Product.PrecioVenta,
                            descuento: 0,
                            existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                return chr.idSucursal == $scope.branchID;

                            }), 'Existencia'),
                            stock: data.oData.Product.Stock,
                            cantidad: 1,
                            costo: 0,
                            servicio: false,
                            credito: false,
                            comentarios: "",
                            idSucursal: $scope.branchID,
                            idView: "",
                            idPromocion: null,
                            idTipoPromocion: null,
                            idViewPadre: null,
                            idProductoPadre: null,
                            tienePromocion: false
                        });

                        $scope.ValidateStockQuotation(data.oData.Product.idProducto, $scope.branchID);
                    } else {
                        var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto, idSucursal: $scope.branchID }));
                        $scope.items[index].cantidad++;

                        if ($scope.items[index].idView > 0) {
                            $scope.ValidateStock(result.idProducto, result.idSucursal, result.idView)
                        } else {
                            $scope.ValidateStockQuotation(data.oData.Product.idProducto, $scope.branchID);
                        }
                    }

                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }

                $scope.selectedCode = "";
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
                $scope.selectedCode = "";
            });

        $("#product_value").val("");
    };

    $scope.GetStockProduct = function (idProduct) {

        $http({
            method: 'POST',
            url: '../../../Products/GetProduct',
            params: {
                idProduct: idProduct
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify("<p>" + data.oData.Product._Existencias[0].Sucursal + ":" + data.oData.Product._Existencias[0].Existencia
                        + "<br/>" + data.oData.Product._Existencias[1].Sucursal + ":" + data.oData.Product._Existencias[1].Existencia
                        + "<br/>" + data.oData.Product._Existencias[2].Sucursal + ":" + data.oData.Product._Existencias[2].Existencia
                        + "<br/> EN VISTA:" + data.oData.Product.Vista + "</p>", $rootScope.error);

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

                $scope.selectedCode = "";

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $scope.selectedCode = "";

            });

        $("#product_value").val("");

    };

    $scope.GetAmountProductByView = function (idVista, idProduct) {

        return $.ajax({
            method: 'POST',
            url: '../../../Products/GetAmountProductByView',
            async: false,
            data: {
                viewID: idVista,
                productID: idProduct
            }
        });

    };

    $scope.GetProductForIdAndView = function (idBranch, idView, idProduct) {

        return $.ajax({
            method: 'POST',
            url: '../../../Products/GetProductForIdAndView',
            async: false,
            data: {
                idBranch: idBranch,
                idView: idView,
                idProduct: idProduct
            }
        });
    };

    $scope.GetProductForIdAndBranch = function (idBranch, idProduct) {

        return $.ajax({
            method: 'POST',
            url: '../../../Products/GetProductForIdAndBranch',
            async: false,
            data: {
                idBranch: idBranch,
                idProduct: idProduct
            }
        });
    };

    $scope.GetStockProductQuotation = function (idProduct) {

        $http({
            method: 'POST',
            url: '../../../Products/GetProductQuotation',
            params: {
                idProduct: idProduct,

            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {
                    notify("<p>" + data.oData.Product._Existencias[0].Sucursal + ":" + data.oData.Product._Existencias[0].Existencia
                        + "<br/>" + data.oData.Product._Existencias[1].Sucursal + ":" + data.oData.Product._Existencias[1].Existencia
                        + "<br/>" + data.oData.Product._Existencias[2].Sucursal + ":" + data.oData.Product._Existencias[2].Existencia
                        + "</p>", $rootScope.error);
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }

                $scope.selectedCode = "";
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
                $scope.selectedCode = "";
            });

        $("#product_value").val("");
    };

    $scope.CalculateTotalCost = function () {

        $scope.subTotal = 0;
        $scope.CantidadProductos = 0;
        $scope.totalCreditNotes = 0;
        $scope.resCredit = 0;
        $scope.discount = ($scope.discount != null) ? $scope.discount : 0;

        angular.forEach($scope.items, function (value, key) {

            if (value.credito != true) {

                var cost = value.cantidad * value.prec;

                var percentage = value.descuento / 100;

                var discount = cost * percentage;

                value.costo = cost - (discount);
                $scope.subTotal = $scope.subTotal + value.costo;
                $scope.CantidadProductos = $scope.CantidadProductos + parseInt(value.cantidad);

            } else {

                $scope.totalCreditNotes = $scope.totalCreditNotes + (value.cantidad * value.prec);

            }

        });

        $scope.IVA = ((($scope.subTotal + $scope.totalCreditNotes) - (($scope.subTotal) * ($scope.discount / 100)))) * ($scope.IVATasa / 100);

        if ($scope.IVA < 0) {

            $scope.IVA = 0;
        }

        if ($scope.checkedDiscount == true) {

            var percentage = $scope.discount / 100;

            angular.forEach($scope.items, function (value, key) {

                if (value.descuento > 0 || (value.servicio == true && value.desc == 'INSTALACIONES')) {

                    var cost = value.cantidad * value.prec;

                    var percentage = value.descuento / 100;

                    var discount = cost * percentage;

                    value.costo = cost - (discount);

                }

            });

            var discount = 0;

            discount = ($scope.subTotal) * percentage;

            if ($scope.checkedIVA == true) {

                $scope.total = (($scope.subTotal + $scope.totalCreditNotes) - discount) + $scope.IVA;

            } else {

                $scope.total = ($scope.subTotal + $scope.totalCreditNotes) - discount;
            }

        } else {

            $scope.checkedDiscount = false;
            $scope.discount = 0;

            if ($scope.checkedIVA == true) {

                $scope.total = (($scope.subTotal + $scope.totalCreditNotes) + $scope.IVA);

            } else {

                $scope.total = $scope.subTotal + $scope.totalCreditNotes;

            }
        }

        if ($scope.total < 0) {

            $scope.resCredit = $scope.total * -1;

        }

    };

    $scope.CalculateTotalCostWithoutDiscount = function () {

        $scope.subTotal = 0;
        $scope.CantidadProductos = 0;
        $scope.totalCreditNotes = 0;
        $scope.resCredit = 0;
        $scope.discount = ($scope.discount != null) ? $scope.discount : 0;

        angular.forEach($scope.items, function (value, key) {

            if (value.credito != true) {

                var cost = value.cantidad * value.prec;

                var percentage = value.descuento / 100;

                var discount = cost * percentage;

                value.costo = cost - (discount);
                $scope.subTotal = $scope.subTotal + value.costo;
                $scope.CantidadProductos = $scope.CantidadProductos + parseInt(value.cantidad);

            } else {

                $scope.totalCreditNotes = $scope.totalCreditNotes + (value.cantidad * value.prec);

            }

        });

        if (($scope.checkedDiscount == true)) {

            $scope.IVA = ((($scope.subTotal + $scope.totalCreditNotes) - (($scope.subTotal) * ($scope.discount / 100)))) * ($scope.IVATasa / 100);

            if ($scope.IVA < 0) {

                $scope.IVA = 0;
            }

            var percentage = $scope.discount / 100;

            angular.forEach($scope.items, function (value, key) {

                if (value.descuento > 0 || (value.servicio == true && value.desc == 'INSTALACIONES')) {

                    var cost = value.cantidad * value.prec;

                    var percentage = value.descuento / 100;

                    var discount = cost * percentage;

                    value.costo = cost - (discount);

                }

            });

            var discount = 0;

            discount = ($scope.subTotal + $scope.totalCreditNotes) * percentage;

            if ($scope.checkedIVA == true) {

                $scope.total = (($scope.subTotal + $scope.totalCreditNotes) - discount) + $scope.IVA;

            } else {

                $scope.total = ($scope.subTotal + $scope.totalCreditNotes) - discount;
            }

        } else {

            $scope.checkedDiscount = false;
            $scope.discount = 0;

            $scope.IVA = (($scope.subTotal - (($scope.subTotal) * ($scope.discount / 100)) + $scope.totalCreditNotes)) * ($scope.IVATasa / 100);

            if ($scope.IVA < 0) {

                $scope.IVA = 0;
            }

            if ($scope.checkedIVA == true) {

                $scope.total = (($scope.subTotal + $scope.totalCreditNotes) + $scope.IVA);

            } else {

                $scope.total = $scope.subTotal + $scope.totalCreditNotes;

            }

        }

        if ($scope.total < 0) {

            $scope.resCredit = $scope.total * -1;

        }

    };

    //Realizar el cálculo del Total 
    $scope.CalculateTotalForDetail = function (subTotal, discount, totalNotasCredito) {

        subTotal = subTotal - (subTotal * (discount / 100));

        subTotal = parseFloat(subTotal);

        totalNotasCredito = parseFloat(totalNotasCredito);

        subTotal = subTotal + totalNotasCredito;

        $scope.IVADetail = subTotal * ($scope.IVATasa / 100);

        if ($scope.IVADetail < 0) {

            $scope.IVADetail = 0;

        }

    };

    $scope.ValidateStockOutProduct = function (ID, branchID, viewID) {
        if (viewID > 0) {
            $scope.ValidateStock(ID, branchID, viewID);
        } else {
            $scope.ValidateStockQuotation(ID, branchID);
        }
    };

    $scope.ValidateStock = function (ID, branchID, viewID) {

        var service = _.result(_.find($scope.items, function (chr) {
            return chr.idProducto == ID
        }), 'servicio');

        if (service == false) {

            var exist = _.result(_.find($scope.items, function (chr) {
                return chr.idProducto == ID && chr.idSucursal == branchID;
            }), 'existencia');

            exist = (exist == undefined) ? 0 : exist;

            var stock = _.result(_.find($scope.items, function (chr) {
                return chr.idProducto == ID;
            }), 'stock');

            var amount = 0;

            if (viewID != undefined && viewID != null && viewID > 0) {
                amount = _.result(_.find($scope.items, function (chr) {
                    return chr.idProducto == ID && chr.idView == viewID;
                }), 'cantidad');
            } else {
                amount = _.result(_.find($scope.items, function (chr) {
                    return chr.idProducto == ID && chr.idSucursal == branchID;
                }), 'cantidad');
            }

            if (exist < amount) {
                var index = 0;

                if (viewID != undefined && viewID != null && viewID > 0) {
                    index = _.indexOf($scope.items, _.find($scope.items, function (chr) {
                        return chr.idProducto == ID && chr.idView == viewID;
                    }));
                } else {
                    index = _.indexOf($scope.items, _.find($scope.items, function (chr) {
                        return chr.idProducto == ID && chr.idSucursal == branchID;
                    }));
                }

                amountView = 0;

                angular.forEach($scope.items, function (value, key) {
                    if (value.idProducto == ID && value.idSucursal == branchID && value.idView != viewID) {
                        amountView = amountView + value.cantidad;
                    }
                });

                pendingAmount = 0;

                if (viewID != undefined && viewID != null && viewID > 0) {
                    pendingAmount = $scope.GetAmountProductByView(viewID, ID);
                    pendingAmount = pendingAmount.responseJSON.oData.Amount;
                }

                if (viewID != undefined && viewID != null && viewID > 0) {
                    exist = $scope.GetProductForIdAndView(branchID, viewID, ID);
                    exist = exist.responseJSON.oData.Stock;
                }

                amountView = ((exist - amountView) < 0) ? 0 : exist - amountView;

                $scope.items[index].cantidad = pendingAmount + amountView;

                if ($scope.items[index].idVista > 0) {
                    $scope.GetStockProductOutProduct(ID, $scope.items[index].idVista);
                } else {
                    $scope.GetStockProduct(ID);
                }
            }
        }

        $scope.CalculateTotalCost();

    };

    $scope.ValidateStockQuotation = function (ID, idBranch) {

        var service = _.result(_.find($scope.items, function (chr) {
            return chr.idProducto == ID;
        }), 'servicio');

        if (service == false) {

            var exist = _.result(_.find($scope.items, function (chr) {
                return chr.idProducto == ID && chr.idSucursal == idBranch && chr.idView == "";
            }), 'existencia');

            exist = (exist == undefined) ? 0 : exist;

            var stock = _.result(_.find($scope.items, function (chr) {
                return chr.idProducto == ID && chr.idSucursal == idBranch && chr.idView == "";
            }), 'stock');

            var amount = _.result(_.find($scope.items, function (chr) {
                return chr.idProducto == ID && chr.idSucursal == idBranch && chr.idView == "";
            }), 'cantidad');

            if (exist < amount) {
                var index = _.indexOf($scope.items, _.find($scope.items, function (chr) {
                    return chr.idProducto == ID && chr.idSucursal == idBranch && chr.idView == "";
                }));

                $scope.items[index].cantidad = exist;

                $scope.GetStockProductQuotation(ID);

                if (exist == 0) {
                    $scope.items.splice(index, 1);
                }
            }
        }

        $scope.CalculateTotalCost();

    };

    $scope.DeleteProduct = function (ID, branchID, viewID) {

        _.remove($scope.items, function (n) {

            return n.idProducto == ID && n.idSucursal == branchID && n.idView == viewID;

        });

        $scope.CalculateTotalCost();

    };

    $scope.OpenModalService = function () {

        $("#modalService").modal("show");

        $("#modalService").on("show.bs.modal", function (e) {

            $scope.LoadServices();

            $scope.newService = new models.ServiceSale();
        });

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

    $scope.valResult = {};

    $scope.newService = new models.ServiceSale();

    $scope.AddService = function () {

        $scope.valResult = ServiceSaleValidator.validate($scope.newService);

        if ($scope.newService.$isValid) {

            $scope.SaveService();

        }

    };

    $scope.SaveService = function () {

        $scope.items.push({
            idProducto: $scope.newService.descService.idServicio + '-' + GUID.New(),
            idProveedor: null,
            imagen: $scope.newService.imagen,
            codigo: "SERVICIO",
            desc: $scope.newService.descService.Descripcion,
            prec: $scope.newService.salePriceService,
            descuento: 0,
            existencia: $scope.newService.amountService,
            stock: $scope.newService.amountService,
            cantidad: $scope.newService.amountService,
            costo: 0,
            servicio: true,
            credito: false,
            comentarios: $scope.newService.commentsService,
            idSucursal: $scope.branchID,
            idPromocion: null,
            idTipoPromocion: null,
            idVistaPadre: null,
            idProductoPadre: null,
            tienePromocion: false
        });

        $scope.descService = "";
        $scope.newService.imagen = "";
        $scope.newService.salePriceService = 0;
        $scope.newService.amountService = 0;
        $scope.newService.commentsService = "";

        $("#holder").css("background-image", "");

        $scope.CalculateTotalCost();

        $("#modalService").modal("hide");

        $scope.ValidateService($scope.newService.descService.idServicio);

    };

    $scope.ValidateService = function (idService) {

        $http({
            method: 'POST',
            url: '../../../Services/GetInstallationService',
            data: {
                idService: idService
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1 && data.oData != null) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

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

                    window.location = "../../../Access/Close";

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

                    window.location = "../../../Access/Close";

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

                    window.location = "../../../Access/Close";

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.LoadUsersButNot = function () {

        $http({
            method: 'GET',
            url: '../../../Users/ListAllUsersButNot',
            params: {
                idUser: $scope.sellerOne.idUsuario
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.usersTwo = data.oData.Users;

                    if ($scope.outSellerTwo != undefined) {

                        $scope.setSelecUsersTwo($scope.outSellerTwo);

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

    $scope.LoadServices = function () {

        $http({
            method: 'POST',
            url: '../../../Services/ListAllServices'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.listServices = data.oData.Services;

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

    $scope.ResumeQuotation = function (send) {

        var showModal = false;

        $scope.send = send;

        $("#txtValidation").empty();

        if ($scope.customer.type == "moral") {

            if (($scope.moralCustomer == undefined) || ($scope.moralCustomer == null)) {

                $("#txtValidation").append("Seleccione un cliente moral  </br>");

                showModal = true;

            }

        }

        if ($scope.customer.type == "physical") {

            if (($scope.physicalCustomer == undefined) || ($scope.physicalCustomer == null)) {

                $("#txtValidation").append("Seleccione un cliente físico  </br>");

                showModal = true;

            }

        }

        if ($scope.checkedOffices == true) {

            if (($scope.office == undefined) || ($scope.office == null)) {

                $("#txtValidation").append("Seleccione un despacho  </br>");

                showModal = true;

            }

        }

        if (($scope.sellerOne == undefined) || ($scope.sellerOne == null)) {

            $("#txtValidation").append("Seleccione un vendedor  </br>");

            showModal = true;

        }

        if ($scope.items.length == 0) {

            $("#txtValidation").append("Ingrese un producto a la venta </br>");

            showModal = true;

        }

        angular.forEach($scope.items, function (value, key) {
            if (value.cantidad <= 0) {
                $("#txtValidation").append("No se permiten cantidades menores o iguales a 0");

                showModal = true;
            }
        });

        if (showModal == true) {

            $("#modalValidation").modal("show");

        } else {

            $scope.SaveQuotation();

        }

    };

    $scope.SaveQuotation = function () {

        var products = new Array();

        var amount = 0;

        angular.forEach($scope.items, function (value, key) {

            amount = amount + parseInt(value.cantidad);

            if (value.servicio == true) {

                products.push({
                    idProducto: null,
                    codigo: value.codigo,
                    imagen: value.imagen,
                    desc: value.desc,
                    prec: value.prec,
                    descuento: parseFloat(value.descuento),
                    cantidad: value.cantidad,
                    idServicio: value.idProducto.split("-")[0],
                    credito: value.credito,
                    comentarios: value.comentarios,
                    tipo: 2,
                    idSucursal: $scope.branchID,
                    idVista: null
                });

            } else {

                if (value.idTipoPromocion == 3) {
                    products.push({
                        idProducto: value.idProducto,
                        codigo: value.codigo,
                        imagen: value.imagen,
                        desc: value.desc,
                        prec: value.prec,
                        descuento: parseFloat(value.descuento),
                        cantidad: value.cantidad,
                        idServicio: null,
                        credito: value.credito,
                        comentarios: value.comentarios,
                        tipo: 1,
                        idSucursal: value.idSucursal,
                        idVista: value.idView,
                        idPromocion: value.idPromocion,
                        costoPromocion: value.costo,
                        idTipoPromocion: value.idTipoPromocion,
                        idProductoPadre: value.idProductoPadre
                    });
                } else {
                    products.push({
                        idProducto: value.idProducto,
                        codigo: value.codigo,
                        imagen: value.imagen,
                        desc: value.desc,
                        prec: value.prec,
                        descuento: parseFloat(value.descuento),
                        cantidad: value.cantidad,
                        idServicio: null,
                        credito: value.credito,
                        comentarios: value.comentarios,
                        tipo: 1,
                        idSucursal: value.idSucursal,
                        idVista: value.idView,
                        idPromocion: null,
                        costoPromocion: null,
                        idTipoPromocion: null,
                        idProductoPadre: null
                    });
                }

            }

        });

        var typeCustomer = null;

        switch ($scope.customer.type) {

            case "physical":
                typeCustomer = 0;
                break;
            case "moral":
                typeCustomer = 1;
                break;
            case "office":
                typeCustomer = 2;
                break;

        }

        $http({
            method: 'POST',
            url: '../../../Quotations/SaveQuotationFromUnifiedView',
            data: {
                number: $scope.number,
                idUser1: $scope.sellerOne.idUsuario,
                idUser2: ($scope.sellerTwo == undefined) ? null : $scope.sellerTwo.idUsuario,
                idCustomerP: ($scope.customer.type == "physical") ? $scope.physicalCustomer.idCliente : null,
                idCustomerM: ($scope.customer.type == "moral") ? $scope.moralCustomer.idCliente : null,
                idOffice: ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
                project: $scope.project,
                typeCustomer: typeCustomer,
                idOfficeReference: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null,
                idBranch: $scope.branchID,
                dateSale: $scope.dateTime,
                amountProducts: amount,
                subtotal: $scope.subTotal,
                discount: ($scope.checkedDiscount == true) ? parseFloat($scope.discount) : 0,
                IVA: ($scope.checkedIVA == true) ? 1 : 0,
                IVATasa: $scope.IVATasa,
                total: $scope.total,
                lProducts: products,
                comments: $scope.comments,
                send: $scope.send
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    window.location = "../../../Quotations/Index";

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

    $scope.setIdBranch = function () {

        if ($scope.branchadd.prod == "A") { $scope.branchID = 2; } else if ($scope.branchadd.prod == "G") { $scope.branchID = 3; } else { $scope.branchID = 4; }
    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {

        if (myArray != undefined) {

            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }

    };

    $scope.DetailSale = function (remision) {

        $scope.includeURL = "DetailSale?remision=" + remision;

        $("#modalDetailSaleOnLine").modal("show");

    };

    $scope.init = function (detail) {

        $scope.items = detail;

    };

    $scope.LoadDetailQuotation = function (outProduct) {
        angular.forEach(outProduct.oDetail, function (detail, dKey) {
            var branchID = detail.idSucursal;
            var branchName = detail.Sucursal;
            angular.forEach(detail.oDetail, function (value, key) {
                $scope.GetProductForQuotation(value, branchID, branchName);
            });
        });
    }

    $scope.GetProductForQuotation = function (prod, branchID, branchName) {

        if (prod.idProducto != null) {

            $http({
                method: 'GET',
                url: '../../../Products/GetProductQuotationOriginView',
                params: {
                    idProduct: prod.idProducto,
                    idView: prod.idVista
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        var result = _.result(_.find($scope.items, function (chr) {
                            return chr.idProducto == data.oData.Product.idProducto && chr.idView == prod.idVista;
                        }), 'idProducto');

                        if (result == undefined) {

                            var discount = 0;

                            $scope.items.push({
                                idProducto: data.oData.Product.idProducto,
                                idProveedor: data.oData.Product.idProveedor,
                                imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                codigo: data.oData.Product.Codigo,
                                desc: data.oData.Product.Descripcion,
                                prec: data.oData.Product.PrecioVenta,
                                descuento: (prod.Descuento == undefined) ? 0 : prod.Descuento,
                                existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {
                                    return chr.idSucursal == branchID;
                                }), 'Existencia'),
                                stock: data.oData.Product.Stock,
                                cantidad: (prod.Cantidad == undefined) ? 0 : prod.Cantidad,
                                costo: 0,
                                servicio: false,
                                credito: false,
                                comentarios: prod.Comentarios,
                                Sucursal: branchName,
                                idSucursal: branchID,
                                idView: prod.idVista,
                                remision: prod.Remision,
                                idPromocion: data.oData.idPromocion,
                                idTipoPromocion: data.oData.idTipoPromocion,
                                idViewPadre: null,
                                idProductoPadre: null,
                                tienePromocion: false
                            });

                        } else {

                            var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto, idView: prod.idVista }));

                            $scope.items[index].cantidad++;

                        }

                        $scope.GetStockProductOutProduct(data.oData.Product.idProducto, branchID, prod.idVista);

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

    $scope.GetStockProductOutProduct = function (ID, idView) {
        if (ID != undefined) {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductForOutProduct',
                params: {
                    idView: idView,
                    idProduct: ID
                }
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        notify("<p>" + data.oData.Product._Existencias[0].Sucursal + ":" + data.oData.Product._Existencias[0].Existencia
                            + "<br/>" + data.oData.Product._Existencias[1].Sucursal + ":" + data.oData.Product._Existencias[1].Existencia
                            + "<br/>" + data.oData.Product._Existencias[2].Sucursal + ":" + data.oData.Product._Existencias[2].Existencia
                            + "<br/> EN VISTA:" + data.oData.Product.Vista + "</p>", $rootScope.error);
                    } else if (data.failure == 1) {
                        notify(data.oData.Error, $rootScope.error);
                    } else if (data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    }

                    $scope.selectedCode = "";
                    $scope.CalculateTotalCost();
                }).
                error(function (data, status, headers, config) {
                    notify("Ocurrío un error.", $rootScope.error);
                    $scope.selectedCode = "";
                });

            $("#product_value").val("");
        }
    };

    $scope.LoadInformation = function (model) {

        angular.forEach(model.oDetail, function (value, key) {
            $scope.GetBranchInfo(value.idSucursal);
        });


        var credit = "";
        //Select type Customer
        $scope.customer.type = (model.TipoCliente == 0) ? "physical" : (model.TipoCliente == 1) ? "moral" : "office";

        //Select customer
        $scope.outCustomer = model.Customer;

        //Select office        
        $scope.checkedOffices = (model.idDespachoReferencia != undefined && model.idDespachoReferencia.length != 0) ? true : false;

        $scope.outOffice = "'" + model.idDespachoReferencia + "'";

        //Select seller
        $scope.outSellerOne = model.Vendedor;
        $scope.outSellerTwo = model.Vendedor2;

        $scope.idUserChecker = model.idUsuario;

        if (model.Descuento > 0) {
            $scope.checkedDiscount = true;
            $scope.discount = parseFloat(model.Descuento);
        }

        $scope.checkedIVA = (model.IVA == 1) ? true : false;
    };

    $scope.setSelecCustomerPhysical = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.physicalCustomers, parseInt(a), 'idCliente');

        $scope.physicalCustomer = $scope.physicalCustomers[index];
    };

    $scope.setSelecCustomerMoral = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.moralCustomers, parseInt(a), 'idCliente');

        $scope.moralCustomer = $scope.moralCustomers[index];
    };

    $scope.setSelecCustomerOffice = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.offices, parseInt(a), 'idDespacho');

        $scope.officeCustomer = $scope.offices[index];

        $scope.CalculateTotalCost();
    };

    $scope.setSelecOffices = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.offices, parseInt(a), 'idDespacho');

        $scope.office = $scope.offices[index];
    };

    $scope.setSelecUsersOne = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.usersOne, a, 'NombreCompleto');

        $scope.sellerOne = $scope.usersOne[index];
    };

    $scope.setSelecUsersTwo = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.usersTwo, a, 'NombreCompleto');

        $scope.sellerTwo = $scope.usersTwo[index];
    };

    $scope.setSelectService = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.listServices, a, 'Descripcion');

        $scope.newService.descService = $scope.listServices[index];
    };

    $scope.getDate = function (date) {

        var res = "";

        if (date != null) {

            if (date.length > 10) {

                res = date.substring(6, 19);

            }

        }

        return res;
    };

    $scope.SendMail = function (idSale) {

        $scope.mailSale = idSale;

        $("#modalSendMail").modal("show");

    },

        $scope.AcceptSendMail = function () {

            if ($scope.txtSendMail != undefined && $scope.txtSendMail.length > 0) {

                $http({
                    method: 'POST',
                    url: '../../../Sales/SendMailSaleAgain',
                    data: {
                        idSale: $scope.mailSale,
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
        },

        $scope.OpenCommentsModal = function (ID, comments) {

            $scope.productItem = ID;
            $scope.commentsItem = comments;

            $("#modalComments").modal("show");

        },

        $scope.SaveComments = function () {

            var index = _.findIndex($scope.items, function (o) { return o.idProducto == $scope.productItem });

            if (index > -1) {

                $scope.items[index].comentarios = $scope.commentsItem;

            }

            $("#modalComments").modal("hide");

        },

        $scope.EditSale = function (idSale) {

            window.location = "../../../Sales/EditSale?idSale=" + idSale;

        };

    //Carga de imágenes
    $scope.itemsToPrint = new Array();

    var arch = new FileReader();
    $scope.myFiles = [];

    $scope.iniAddProduct = function () {
        inicio();
    }

    function inicio() {
        fileMethod();
    };

    function fileMethod() {
        document.getElementById('holder').addEventListener('dragover', permitirDrop, false);
        document.getElementById('holder').addEventListener('drop', drop, false);
    };

    function drop(ev) {
        ev.preventDefault();
        arch.addEventListener('load', leer, false);
        arch.readAsDataURL(ev.dataTransfer.files[0]);
        $scope.myFiles = ev.dataTransfer.files;

        var file = $scope.myFiles.item(0);
        if ('name' in file) {
            $("#imgName").val(file.name);
        }
        else {
            $("#imgName").val(file.name);
        }

        $scope.rd = 0;
    };

    function permitirDrop(ev) {
        ev.preventDefault();
    };

    function leer(ev) {
        $("#imgProduct").remove();
        document.getElementById('holder').style.backgroundImage = "url('" + ev.target.result + "')";
    };

    $scope.SaveUploadImg = function (option) {
        var formData = new FormData();
        for (var i = 0; i < $scope.myFiles.length; i++) {
            formData.append('file', $scope.myFiles[i]);
        }

        if ($scope.myFiles.length == null || $scope.myFiles.length == 0 || $scope.myFiles.length == undefined) {
            $scope.openModalMsg();
            return;
        }

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '../../Services/UploadFile?qtyRotates=' + $scope.rd, true);
        xhr.onload = function () {
            var response = JSON.parse(xhr.responseText);
            if (xhr.readyState == 4 && xhr.status == 200) {
                $scope.newService.imagen = "../Content/Services/" + response.fileName;
                notify(response.message, $rootScope.success);
            } else {
                notify(response.message, $rootScope.error);
            }
        };

        xhr.send(formData);

    };

    $scope.GetAccounts = function () {

        $http({
            method: 'POST',
            url: '../../../Sales/GetAccounts'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.accounts = data.oData.Accounts;

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $scope.selectedCode = "";

            });

    };

    $scope.ChangeTextButtons = function () {
        $("#modalDeleteItem .confirmbutton-yes").text("Enviar");
        $("#modalDeleteItem .confirmbutton-no").text("Solo guardar")
    };

    //Cargar Editar Imagen
    $scope.iniEditProduct = function () {
        inicioEdit();
    }

    function inicioEdit() {
        fileEditMethod();
    };

    function fileEditMethod() {
        document.getElementById('editHolder').addEventListener('dragover', allowEditDrop, false);
        document.getElementById('editHolder').addEventListener('drop', dropEdit, false);
    };

    function dropEdit(ev) {
        ev.preventDefault();
        arch.addEventListener('load', readEdit, false);
        arch.readAsDataURL(ev.dataTransfer.files[0]);
        $scope.myFiles = ev.dataTransfer.files;

        var file = $scope.myFiles.item(0);
        if ('name' in file) {
            $("#edithImgName").val(file.name);
        }
        else {
            $("#edithImgName").val(file.name);
        }

        $scope.rd = 0;
    };

    function allowEditDrop(ev) {
        ev.preventDefault();
    };

    function readEdit(ev) {
        document.getElementById('editHolder').style.backgroundImage = "url('" + ev.target.result + "')";
    };

    $scope.ChangeTextButtons = function () {
        $("#modalDeleteItem .confirmbutton-yes").text("Enviar");
        $("#modalDeleteItem .confirmbutton-no").text("Solo guardar")
    };

    //--------------------------------------------------

    $scope.EditService = function (item) {

        $scope.newService.idService = item.idProducto;
        $scope.setSelectService(item.desc);
        $scope.newService.salePriceService = item.prec;
        $scope.newService.amountService = item.cantidad;
        $scope.newService.commentsService = item.comentarios;
        $scope.newService.imagen = item.imagen;

        document.getElementById('editHolder').style.backgroundImage = "url('" + item.imagen + "')";

        $("#modalEditService").modal("show");
    };

    $scope.SaveEditService = function () {

        var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: $scope.newService.idService }));

        $scope.items[index].imagen = ($scope.newService.imagen.length <= 40) ? "../Content/Services/" + $scope.newService.imagen : $scope.newService.imagen;
        $scope.items[index].codigo = "SERVICIO";
        $scope.items[index].desc = $scope.newService.descService.Descripcion;
        $scope.items[index].prec = $scope.newService.salePriceService;
        $scope.items[index].existencia = $scope.newService.amountService;
        $scope.items[index].stock = $scope.newService.amountService;
        $scope.items[index].cantidad = $scope.newService.amountService;
        $scope.items[index].comentarios = $scope.newService.commentsService;

        $("#editHolder").css("background-image", "");

        $scope.CalculateTotalCost();

        $("#modalEditService").modal("hide");

        $scope.ValidateService($scope.newService.descService.idServicio);
    };

    $scope.RotateImage = function () {

        if ($scope.myFiles.length > 0) {
            $scope.rd += 1;
            var deg = 90 * $scope.rd;
            var deg = 90 * $scope.rd;
            $('#holder').css({
                '-webkit-transform': 'rotate(' + deg + 'deg)',  //Safari 3.1+, Chrome  
                '-moz-transform': 'rotate(' + deg + 'deg)',     //Firefox 3.5-15  
                '-ms-transform': 'rotate(' + deg + 'deg)',      //IE9+  
                '-o-transform': 'rotate(' + deg + 'deg)',       //Opera 10.5-12.00  
                'transform': 'rotate(' + deg + 'deg)'          //Firefox 16+, Opera 12.50+  
            });

            if ($scope.rd > 3) {
                $scope.rd = 0;
            }
        }
    };

    //Promotions
    $scope.GetAvailableProducts = function (idProduct, idView) {
        $scope.availableProducts = [];
        angular.forEach($scope.items, function (value, key) {
            if (value.idProducto != idProduct
                && value.servicio == false
                && value.tienePromocion == false) {
                $scope.availableProducts.push(value);
            } else if (value.idProducto == idProduct
                && value.idView != idView
                && value.servicio == false
                && value.tienePromocion == false) {
                $scope.availableProducts.push(value);
            }
        });
    }

    $scope.OpenPromotionModal = function (item) {
        $scope.product = item;

        $scope.GetAvailableProducts(item.idProducto, item.idView);

        $("#modalPromotions").modal("show");
    };

    $scope.SaveCombo = function () {

        $http({
            method: 'GET',
            url: '../../../Promotions/MixedComboPromotion'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    if (data.oData.Promotion != undefined) {
                        angular.forEach($scope.items, function (value, key) {
                            if (value.idProductoPadre == $scope.product.idProducto && value.idViewPadre == $scope.product.idView) {
                                value.idPromocion = null;
                                value.idTipoPromocion = null;
                                value.idViewPadre = null;
                                value.idProductoPadre = null;
                                value.tienePromocion = false;
                            }
                        });

                        angular.forEach($scope.items, function (value, key) {
                            //Si es el producto seleccionado y el precio de este es menor al producto padre
                            if (value.idProducto == $scope.relatedProduct.idProducto && value.idView == $scope.relatedProduct.idView) {
                                if ($scope.relatedProduct.prec < $scope.product.prec) {
                                    value.descuento = 50;
                                    value.idPromocion = data.oData.Promotion.idPromocion;
                                    value.idTipoPromocion = data.oData.Promotion.idTipoPromocion;
                                    value.idViewPadre = $scope.product.idView;
                                    value.tienePromocion = true;
                                    value.idProductoPadre = $scope.product.idProducto;

                                    angular.forEach($scope.items, function (item, key) {
                                        if (item.idProducto == $scope.product.idProducto && item.idView == $scope.product.idView) {
                                            item.tienePromocion = true;
                                        }
                                    });
                                }
                            } else if (value.idProducto == $scope.product.idProducto && value.idView == $scope.product.idView) {
                                if ($scope.product.prec < $scope.relatedProduct.prec) {
                                    value.descuento = 50;
                                    value.idPromocion = data.oData.Promotion.idPromocion;
                                    value.idTipoPromocion = data.oData.Promotion.idTipoPromocion;
                                    value.idViewPadre = $scope.relatedProduct.idView;
                                    value.tienePromocion = true;
                                    value.idProductoPadre = $scope.relatedProduct.idProducto;

                                    angular.forEach($scope.items, function (item, key) {
                                        if (item.idProducto == $scope.relatedProduct.idProducto && item.idView == $scope.relatedProduct.idView) {
                                            item.tienePromocion = true;
                                        }
                                    });
                                }
                            }
                        });
                    }

                    $scope.CalculateTotalCost();
                    $("#modalPromotions").modal("hide");

                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }

                $scope.selectedCode = "";
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
                $scope.selectedCode = "";
            });
    };

});

angular.module("General").controller('EditSalesController', function (models, ServiceSaleValidator, PaymentValidator, DetailSaleFacturaValidator, GUID, GetDiscount, GetDiscountOffice, GetDiscountSpecial, $scope, $http, $window, notify, $rootScope, FileUploader) {

    $scope.selectedCode = "";
    $scope.branch = "";
    $scope.customer = { type: "" };
    $scope.items = new Array();
    $scope.lProductsOut = new Array();
    $scope.subTotal = 0;
    $scope.checkedDiscount = false;
    $scope.checkedIVA = false;
    $scope.discount = 0;
    $scope.total = 0;
    $scope.countService = 0;
    $scope.resCredit = 0;
    $scope.msgTypesPayment = false;
    $scope.includeURLMoral = "";
    $scope.includeURLPhysical = "";
    $scope.barcode = "";
    $scope.typeBill = 0;
    $scope.paymentLeft = 0;
    $scope.DatePaymentResult = "";
    $scope.Math = window.Math;
    $scope.amountDetailBill = 0;
    $scope.paymentTAX = {};
    $scope.paymentTAXCredit = {};
    $scope.unified = 1;
    $scope.bankAccountStatus = [];
    $scope.terminalTypeStatus = [];

    $scope.typesPaymentItems = new Array();
    $scope.paymentMonths = new Array();
    $scope.Allpayments = {};
    $scope.type = 0;

    $scope.productOne = {};
    $scope.productTwo = {};

    $scope.LoadInformation = function (typeCustomer, customer, idOffice, seller, sale, discount, iva) {

        //Select type Customer
        $scope.customer.type = (typeCustomer == 0) ? "physical" : (typeCustomer == 1) ? "moral" : "office";

        //Select customer
        $scope.outCustomer = customer;

        //Select office
        $scope.checkedOffices = (idOffice != undefined && idOffice.length != 0) ? true : false;

        $scope.outOffice = idOffice;

        //Select seller
        $scope.outSellerOne = seller[0];
        $scope.outSellerTwo = seller[1];

        $scope.remision = sale.Remision;
        $scope.branch = sale.Sucursal;
        $scope.branchID = 2;
        $scope.idSale = sale.idVenta;
        $scope.project = sale.Proyecto;

        if (discount > 0) {
            $scope.checkedDiscount = true;
            $scope.discount = parseFloat(discount);
        }

        if (iva == 1) {
            $scope.checkedIVA = true;
        }
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

    $scope.SetBranchName = function (branchName, IDBranch) {
        $scope.branch = branchName;
        $scope.branchID = IDBranch;
        $("#openModal").modal("hide");
        $scope.GetNumberRem();
    };

    $scope.AddMoralCustomer = function () {
        var now = new Date();

        $scope.includeURLMoral = "../../Customers/PartialAddMoralCustomer?update=" + now;

        $("#openModalAddMoralCustomer").modal("show");

        $('#openModalAddMoralCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadCustomers();
        });
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

    $scope.loadSales = function (sale) {
        $scope.discount = sale.Descuento;

        angular.forEach(sale.oDetail, function (value, key) {
            if (value.idProducto == null && value.idServicio > 0) {
                $scope.items.push({
                    idProducto: value.idServicio + "-" + GUID.New(),
                    idSucursal: value.idSucursal,
                    idCredito: null,
                    idProveedor: null,
                    imagen: "../Content/Services/" + value.Imagen,
                    codigo: value.Descripcion,
                    desc: value.Descripcion,
                    prec: value.Precio,
                    descuento: value.Descuento,
                    existencia: value.Cantidad,
                    stock: value.Cantidad,
                    cantidad: value.Cantidad,
                    costo: 0,
                    servicio: true,
                    credito: false,
                    comentarios: value.Comentarios,
                    idPromocion: null,
                    idTipoPromocion: null,
                    idProductoPadre: null
                });
            } else if (value.idProducto == null && value.idCredito > 0) {
                $scope.items.push({
                    idProducto: value.idCredito,
                    idSucursal: value.idSucursal,
                    idCredito: null,
                    idProveedor: null,
                    imagen: "",
                    codigo: value.Credito,
                    desc: "Nota de crédito",
                    prec: value.Precio,
                    descuento: value.Descuento,
                    existencia: 1,
                    stock: 1,
                    cantidad: 1,
                    costo: 0,
                    servicio: false,
                    credito: true,
                    comentarios: value.Comentarios,
                    idPromocion: null,
                    idTipoPromocion: null,
                    idProductoPadre: null
                });
            } else {
                $scope.GetProductForSale(value);
            }

        });

        $scope.CalculateTotalCost();
    };

    $scope.GetProductForSale = function (prod) {
        if (prod.idProducto != null) {
            $http({
                method: 'POST',
                url: '../../../Products/GetProduct',
                params: {
                    idProduct: prod.idProducto
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        var branchID = (prod.idSucursal > 1) ? prod.idSucursal : $scope.branchID;

                        var result = _.result(_.find($scope.items, function (chr) {
                            return chr.idProducto == data.oData.Product.idProducto && chr.idSucursal == branchID
                        }), 'idProducto');

                        if (result == undefined) {

                            var discount = 0;

                            if (prod.idTipoPromocion == 3) {
                                $scope.items.push({
                                    idProducto: data.oData.Product.idProducto,
                                    idSucursal: branchID,
                                    idProveedor: data.oData.Product.idProveedor,
                                    imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                    codigo: data.oData.Product.Codigo,
                                    desc: data.oData.Product.Descripcion,
                                    prec: data.oData.Product.PrecioVenta,
                                    descuento: prod.Descuento,
                                    existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                        return chr.idSucursal == branchID;

                                    }), 'Existencia'),
                                    stock: data.oData.Product.Stock,
                                    cantidad: prod.Cantidad,
                                    costo: 0,
                                    servicio: false,
                                    credito: false,
                                    comentarios: "",
                                    idPromocion: prod.idPromocion,
                                    idTipoPromocion: prod.idTipoPromocion,
                                    idProductoPadre: prod.idProductoPadre
                                });
                            } else {
                                $scope.items.push({
                                    idProducto: data.oData.Product.idProducto,
                                    idSucursal: branchID,
                                    idProveedor: data.oData.Product.idProveedor,
                                    imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                    codigo: data.oData.Product.Codigo,
                                    desc: data.oData.Product.Descripcion,
                                    prec: data.oData.Product.PrecioVenta,
                                    descuento: prod.Descuento,
                                    existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {
                                        return chr.idSucursal == branchID;
                                    }), 'Existencia'),
                                    stock: data.oData.Product.Stock,
                                    cantidad: prod.Cantidad,
                                    costo: 0,
                                    servicio: false,
                                    credito: false,
                                    comentarios: "",
                                    idPromocion: null,
                                    idTipoPromocion: null,
                                    idProductoPadre: null
                                });
                            }

                        } else {
                            var index = _.indexOf($scope.items, _.find($scope.items, function (chr) {
                                return chr.idProducto == data.oData.Product.idProducto && chr.idSucursal == branchID;
                            }));

                            $scope.items[index].cantidad++;
                        }

                        $scope.ValidateStock(data.oData.Product.idProducto, prod.idSucursal);
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

    $scope.GetProduct = function () {
        $http({
            method: 'POST',
            url: '../../../Products/GetProduct',
            params: {
                idProduct: ($scope.selectedCode == "" || $scope.selectedCode == null) ? "" : $scope.selectedCode.originalObject.idProducto
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    var result = _.result(_.find($scope.items, function (chr) {
                        return chr.idProducto == data.oData.Product.idProducto && chr.idSucursal == $scope.branchID
                    }), 'idProducto');

                    if (result == undefined) {
                        $scope.items.push({
                            idProducto: data.oData.Product.idProducto,
                            idSucursal: $scope.branchID,
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
                            idPromocion: null,
                            idTipoPromocion: null,
                            idProductoPadre: null
                        });
                    } else {
                        var index = _.indexOf($scope.items, _.find($scope.items, function (chr) {
                            return chr.idProducto == data.oData.Product.idProducto && chr.idSucursal == $scope.branchID;
                        }));

                        $scope.items[index].cantidad++;
                    }

                    $scope.ValidateStock(data.oData.Product.idProducto, $scope.branchID);
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
    $scope.getStatusTerminalType = function () {
        $http({
            method: 'GET',
            url: '../../../CatalogTerminalType/Get'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.terminalTypeStatus = data.oData.Status;
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
    $scope.getStatusBankAccount = function () {
        $http({
            method: 'GET',
            url: '../../../CatalogBankAccount/Get'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.bankAccountStatus = data.oData.Status;
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

    $scope.CalculateTotalCost = function () {
        $scope.subTotal = 0;
        $scope.CantidadProductos = 0;
        $scope.totalCreditNotes = 0;
        $scope.resCredit = 0;
        $scope.discount = ($scope.discount != null) ? $scope.discount : 0;

        angular.forEach($scope.items, function (value, key) {
            var cost = 0;
            var percentage = 0;
            var discount = 0;

            if (value.credito != true) {
                //Se aplica promocion
                cost = value.cantidad * value.prec;
                percentage = value.descuento / 100;
                discount = cost * percentage;

                value.costo = cost - (discount);
                $scope.subTotal = $scope.subTotal + value.costo;
                $scope.CantidadProductos = $scope.CantidadProductos + parseInt(value.cantidad);

            } else {
                $scope.totalCreditNotes = $scope.totalCreditNotes + (value.cantidad * value.prec);
            }
        });

        $scope.IVA = ((($scope.subTotal + $scope.totalCreditNotes) - (($scope.subTotal) * ($scope.discount / 100)))) * .16;

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
            var cost = 0;
            var percentage = 0;
            var discount = 0;

            if (value.credito != true) {
                cost = value.cantidad * value.prec;
                percentage = value.descuento / 100;
                discount = cost * percentage;

                value.costo = cost - (discount);
                $scope.subTotal = $scope.subTotal + value.costo;
                $scope.CantidadProductos = $scope.CantidadProductos + parseInt(value.cantidad);
            } else {
                $scope.totalCreditNotes = $scope.totalCreditNotes + (value.cantidad * value.prec);
            }
        });

        if ($scope.checkedDiscount == true) {
            $scope.IVA = ((($scope.subTotal + $scope.totalCreditNotes) - (($scope.subTotal) * ($scope.discount / 100)))) * .16;

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

            $scope.IVA = (($scope.subTotal - (($scope.subTotal) * ($scope.discount / 100)) + $scope.totalCreditNotes)) * .16;

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

        $scope.IVADetail = subTotal * .16;

        if ($scope.IVADetail < 0) {
            $scope.IVADetail = 0;
        }
    };

    $scope.ValidateStock = function (ID, idBranch) {
        var service = _.result(_.find($scope.items, function (chr) {
            return chr.idProducto == ID
        }), 'servicio');

        if (service == false) {
            var exist = _.result(_.find($scope.items, function (chr) {
                return chr.idProducto == ID && chr.idSucursal == idBranch;
            }), 'existencia');

            exist = (exist == undefined) ? 0 : exist;

            var amount = _.result(_.find($scope.items, function (chr) {
                return chr.idProducto == ID && chr.idSucursal == idBranch;
            }), 'cantidad');

            if (exist < amount) {
                var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: ID, idSucursal: idBranch }));

                if (exist == 0 && index > -1) {
                    $scope.items.splice(index, 1);
                } else if (index > -1) {
                    $scope.items[index].cantidad = exist;
                }

                $scope.GetStockProduct(ID);
            }
        }

        $scope.CalculateTotalCost();
    };

    $scope.DeleteProduct = function (prod) {
        _.remove($scope.items, function (n) {
            return n.idProducto == prod.idProducto && n.idSucursal == prod.idSucursal;
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
            idSucursal: $scope.branchID,
            idProveedor: null,
            idCredito: null,
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
            idPromocion: null,
            idTipoPromocion: null,
            idProductoPadre: null
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

    $scope.ResumeSale = function () {

        var showModal = false;

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

        if (showModal == true) {
            $("#modalValidation").modal("show");
        } else {
            $scope.SaveSale();
            this.getStatusBankAccount();
            this.getStatusTerminalType();
        }
    };

    $scope.SaveSale = function () {
        $("#modalPayment").modal("show");
        $scope.AddTypePayment();
    };

    $scope.GetCreditNote = function (item) {
        $("#getCreditNote").button("loading");

        $http({
            method: 'POST',
            url: '../../../Credits/GetCreditNote',
            data: {
                remision: item.creditnote
            }
        }).
            success(function (data, status, headers, config) {
                $("#getCreditNote").button("reset");

                if (data.success == 1) {
                    angular.forEach($scope.typesPaymentItems, function (value, key) {
                        if (value.creditnote == item.creditnote) {
                            value.idCreditNote = data.oData.CreditNote.idNotaCredito;
                            value.amount = data.oData.CreditNote.Cantidad
                        }
                    });
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).
            error(function (data, status, headers, config) {
                $("#getCreditNote").button("reset");

                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.GetCreditNoteAddPayment = function () {
        $("#getCreditNote").button("loading");

        $http({
            method: 'POST',
            url: '../../../Credits/GetCreditNote',
            data: {
                remision: $scope.newPayment.creditnote
            }
        }).
            success(function (data, status, headers, config) {
                $("#getCreditNote").button("reset");

                if (data.success == 1) {
                    $scope.newPayment.idCreditNote = data.oData.CreditNote.idNotaCredito;
                    $scope.newPayment.paymentAmount = data.oData.CreditNote.Cantidad;
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).
            error(function (data, status, headers, config) {
                $("#getCreditNote").button("reset");
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.AddTypePayment = function () {
        var totalForPay = 0;

        if ($scope.typesPaymentItems.length > 0) {
            totalForPay = $scope.total - _.sumBy($scope.typesPaymentItems, function (o) { return o.amount; });

            if (totalForPay > 0) {
                $scope.typesPaymentItems.push({
                    typesPayment: 0,
                    typesCard: 0,
                    bank: "",
                    holder: "",
                    numCheck: "",
                    numIFE: "",
                    amount: totalForPay,
                    amountIVA: 0,
                    dateMaxPayment: "",
                    openedMaxPayment: false,
                    IVA: false,
                    creditnote: null,
                    idCreditNote: null,
                    paymentMonth: null,
                    idCatalogTerminalType: null,
                    idCatalogBankAccount: null
                });
            }
        } else {
            totalForPay = $scope.total;

            $scope.typesPaymentItems.push({
                typesPayment: 0,
                typesCard: 0,
                bank: "",
                holder: "",
                numCheck: "",
                numIFE: "",
                amount: totalForPay,
                amountIVA: 0,
                dateMaxPayment: "",
                openedMaxPayment: false,
                IVA: false,
                creditnote: null,
                idCreditNote: null,
                paymentMonth: null,
                idCatalogTerminalType: null,
                idCatalogBankAccount: null
            });
        }
    };

    $scope.AddModifyTypePayment = function () {

        var totalForPay = 0;

        if ($scope.typesPaymentItems.length > 0) {
            totalForPay = $scope.total - _.sumBy($scope.typesPaymentItems, function (o) { return o.amount; });

            if (totalForPay > 0) {
                $scope.typesPaymentItems.push({
                    typesPayment: 0,
                    typesCard: 0,
                    bank: "",
                    holder: "",
                    numCheck: "",
                    numIFE: "",
                    amount: totalForPay,
                    amountIVA: 0,
                    dateMaxPayment: "",
                    openedMaxPayment: false,
                    IVA: false,
                    creditnote: null,
                    idCreditNote: null,
                    paymentMonth: null,
                    idCatalogTerminalType: null,
                    idCatalogBankAccount: null
                });
            }
        } else {
            totalForPay = $scope.total;

            $scope.typesPaymentItems.push({
                typesPayment: 0,
                typesCard: 0,
                bank: "",
                holder: "",
                numCheck: "",
                numIFE: "",
                amount: totalForPay,
                amountIVA: 0,
                dateMaxPayment: "",
                openedMaxPayment: false,
                IVA: false,
                creditnote: null,
                idCreditNote: null,
                paymentMonth: null,
                idCatalogTerminalType: null,
                idCatalogBankAccount: null
            });
        }
    };

    $scope.RemoveTypePayment = function () {
        var a = this;

        _.remove($scope.typesPaymentItems, function (n) {
            return n == a.item;
        });
    };

    $scope.CalculateTotalCostTypesPayment = function (payment) {
        if (payment.IVA) {
            payment.amountIVA = (payment.amount * .16);
            payment.amount = payment.amount + payment.amountIVA;
            $scope.total = $scope.total + payment.amountIVA;
        } else {
            $scope.total = $scope.total - payment.amountIVA;
            payment.amount = payment.amount - payment.amountIVA;
            payment.amountIVA = 0;
        }
    }

    $scope.SavePayment = function () {

        $scope.resCredit = $scope.total - _.sumBy($scope.typesPaymentItems, function (o) { return o.amount; });

        var paymentEmpty = _.find($scope.typesPaymentItems, function (o) { return o.typesPayment == 0; });

        var hasPaymentError = false;

        for (var i = 0; i < $scope.typesPaymentItems.length; i++) {
            if (($scope.typesPaymentItems[i].typesPayment == 5 || $scope.typesPaymentItems[i].typesPayment == 6) &&
                $scope.typesPaymentItems[i].idCatalogTerminalType == null) {
                notify("Seleccionar una terminal.", $rootScope.error);
                hasPaymentError = true;
            }
            else if ($scope.typesPaymentItems[i].typesPayment == 7 && $scope.typesPaymentItems[i].idCatalogBankAccount == null) {
                notify("Seleccionar banco destino.", $rootScope.error);
                hasPaymentError = true;
            }
        }

        if (($scope.resCredit > 0) || (paymentEmpty != undefined)) {

            $scope.msgTypesPayment = true;

            $scope.missing = $scope.resCredit;

        } else if (hasPaymentError == false) {

            $scope.buttonDisabled = true;

            $("#modalPayment").modal("hide");

            var products = new Array();

            var amount = 0;

            angular.forEach($scope.items, function (value, key) {

                amount = amount + parseInt(value.cantidad);

                if (value.servicio == true) {

                    products.push({

                        idProducto: null,
                        idSucursal: value.idSucursal,
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
                        idPromocion: null,
                        costoPromocion: null,
                        idTipoPromocion: null,
                        idProductoPadre: null
                    });

                } else if (value.credito == true) {

                    products.push({
                        idProducto: value.idProducto,
                        idSucursal: value.idSucursal,
                        codigo: value.codigo,
                        imagen: value.imagen,
                        desc: value.desc,
                        prec: value.prec,
                        descuento: parseFloat(value.descuento),
                        cantidad: value.cantidad,
                        idServicio: null,
                        credito: value.credito,
                        comentarios: value.comentarios,
                        tipo: 3,
                        idPromocion: null,
                        costoPromocion: null,
                        idTipoPromocion: null,
                        idProductoPadre: null
                    });

                } else {

                    if (value.idTipoPromocion == 3) {
                        products.push({
                            idProducto: value.idProducto,
                            idSucursal: value.idSucursal,
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
                            idPromocion: value.idPromocion,
                            costoPromocion: value.costo,
                            idTipoPromocion: value.idTipoPromocion,
                            idProductoPadre: value.idProductoPadre
                        });
                    } else {
                        products.push({
                            idProducto: value.idProducto,
                            idSucursal: value.idSucursal,
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
                url: '../../../Sales/SaveEditSale',
                data: {
                    idSale: $scope.idSale,
                    idBranch: $scope.unified,
                    idUser1: $scope.sellerOne.idUsuario,
                    idUser2: ($scope.sellerTwo == undefined) ? null : $scope.sellerTwo.idUsuario,
                    idCustomerP: ($scope.customer.type == "physical") ? $scope.physicalCustomer.idCliente : null,
                    idCustomerM: ($scope.customer.type == "moral") ? $scope.moralCustomer.idCliente : null,
                    idOffice: ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
                    project: $scope.project,
                    typeCustomer: typeCustomer,
                    idOfficeReference: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null,
                    amountProducts: amount,
                    subtotal: $scope.subTotal,
                    discount: ($scope.checkedDiscount == true) ? parseFloat($scope.discount) : 0,
                    IVA: ($scope.checkedIVA == true) ? 1 : 0,
                    total: $scope.total,
                    lProducts: products,
                    lTypePayment: $scope.typesPaymentItems
                }
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        notify(data.oData.Message, $rootScope.success);
                        $('#modalPrintSale').on('hidden.bs.modal', function (e) {
                            window.location = "../../../Sales/Index";
                        });

                        $("#modalPrintSale").modal("show");

                        if ($scope.resCredit < 0) {
                            var credito = Math.abs($scope.resCredit);
                            $scope.GenerateCredit(credito);
                        }
                    } else if (data.failure == 1) {
                        notify(data.oData.Error, $rootScope.error);
                    } else if (data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    }

                    $scope.buttonDisabled = false;
                }).
                error(function (data, status, headers, config) {
                    notify("Ocurrío un error.", $rootScope.error);
                    $scope.buttonDisabled = false;
                });
        }

    };

    $scope.Print = function () {
        var URL = '../../../Sales/PrintSale?remision=' + $scope.remision;

        var win = window.open(URL, '_blank');
        win.focus();
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

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
        if (myArray != undefined) {
            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }
    };

    $scope.CalculateTotalCost = function () {
        $scope.subTotal = 0;
        $scope.CantidadProductos = 0;
        $scope.totalCreditNotes = 0;
        $scope.resCredit = 0;
        $scope.discount = ($scope.discount != null) ? $scope.discount : 0;

        angular.forEach($scope.items, function (value, key) {
            var cost = 0;
            var percentage = 0;
            var discount = 0;

            if (value.credito != true) {
                cost = value.cantidad * value.prec;
                percentage = value.descuento / 100;
                discount = cost * percentage;

                value.costo = cost - (discount);
                $scope.subTotal = $scope.subTotal + value.costo;
                $scope.CantidadProductos = $scope.CantidadProductos + parseInt(value.cantidad);
            } else {
                $scope.totalCreditNotes = $scope.totalCreditNotes + (value.cantidad * value.prec);
            }
        });

        $scope.IVA = ((($scope.subTotal + $scope.totalCreditNotes) - (($scope.subTotal) * ($scope.discount / 100)))) * .16;

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

            var cost = 0;
            var percentage = 0;
            var discount = 0;

            if (value.credito != true) {
                cost = value.cantidad * value.prec;
                percentage = value.descuento / 100;
                discount = cost * percentage;

                value.costo = cost - (discount);
                $scope.subTotal = $scope.subTotal + value.costo;
                $scope.CantidadProductos = $scope.CantidadProductos + parseInt(value.cantidad);
            } else {

                $scope.totalCreditNotes = $scope.totalCreditNotes + (value.cantidad * value.prec);

            }

        });

        if ($scope.checkedDiscount == true) {

            $scope.IVA = ((($scope.subTotal + $scope.totalCreditNotes) - (($scope.subTotal) * ($scope.discount / 100)))) * .16;

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

            $scope.IVA = (($scope.subTotal - (($scope.subTotal) * ($scope.discount / 100)) + $scope.totalCreditNotes)) * .16;

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

    $scope.GetTypesPaymentForSale = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/GetTypesPaymentForSale',
            params: {
                idSale: idSale,
            }
        }).
            then(function (response) {

                if (response.data.success == 1) {
                    var suma = 0;

                    if (response.data.oData.TypesPayment.length > 0) {
                        angular.forEach(response.data.oData.TypesPayment, function (value, key) {
                            if (value.HistoryCredit.length > 0) {
                                for (var i = 0; i <= value.HistoryCredit.length - 1; i++) {
                                    suma = suma - value.HistoryCredit[i].Cantidad;
                                }

                                value.amount = value.amount + suma;
                            }
                        });

                        $scope.TypesPayment = response.data.oData.TypesPayment;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);
                    }
                } else if (response.data.failure == 1) {
                    notify(response.data.oData.Error, $rootScope.error);
                } else if (response.data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            })
    };

    $scope.GenerateCredit = function (credito) {
        $http({
            method: 'POST',
            url: '../../../Credits/SaveAddCredit',
            data: {
                idCreditNoteType: 1,
                idSale: $scope.idSale,
                idCustomerP: ($scope.customer.type == "physical") ? $scope.physicalCustomer.idCliente : null,
                idCustomerM: ($scope.customer.type == "moral") ? $scope.moralCustomer.idCliente : null,
                idSeller: $scope.sellerOne.idUsuario,
                amount: credito,
                dtDate: $scope.dateTime,
                comments: $scope.remision,
                lProducts: null
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {
                    $scope.resCredit = Math.abs($scope.resCredit);
                    $scope.remisionCredit = data.oData.sRemision;
                    $scope.finalDateCredit = data.oData.finalDate;
                    $scope.idCreditNote = data.oData.idCreditNote;

                    $("#modalCreditNote").modal("show");
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

    $scope.OpenCommentsModal = function (ID, idBranch, comments) {
        $scope.productItem = ID;
        $scope.idBranchProduct = idBranch;
        $scope.commentsItem = comments;

        $("#modalComments").modal("show");
    };

    $scope.SaveComments = function () {
        var index = _.findIndex($scope.items, function (o) { return o.idProducto == $scope.productItem && o.idSucursal == $scope.idBranchProduct});

        if (index > -1) {
            $scope.items[index].comentarios = $scope.commentsItem;
        }

        $("#modalComments").modal("hide");
    };

    //Carga de imágenes
    $scope.itemsToPrint = new Array();

    var arch = new FileReader();
    $scope.myFiles = [];

    $scope.iniAddProduct = function () {
        inicio();
    };

    function inicio() {
        fileMethod();
    };

    function fileMethod() {
        document.getElementById('holder').addEventListener('dragover', permitirDrop, false);
        document.getElementById('holder').addEventListener('drop', drop, false);
    };

    // upload on file select or drop
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

    $scope.RotateImage = function () {

        if ($scope.myFiles.length > 0) {
            $scope.rd += 1;
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

    $scope.setSelectService = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.listServices, a, 'Descripcion');

        $scope.newService.descService = $scope.listServices[index];
    };

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

    //Mixed Combo Promotion
    $scope.OpenModalMixedComboPromotion = function () {
        $("#ModalMixedComboPromotion").modal("show");
    };

    $scope.GetPaymentMonths = function () {
        $http({
            method: 'GET',
            url: '../../../PaymentMonth/GetAll',
            params: {}
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.paymentMonths = data.oData.PaymentMonths;
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

angular.module("General").controller('SalesController', function (models, ServiceSaleValidator, PaymentValidator, DetailSaleFacturaValidator, GUID, GetDiscount, GetDiscountOffice, GetDiscountSpecial, $scope, $http, $window, notify, $rootScope, FileUploader) {

    $scope.selectedCode = "";
    $scope.branch = "";
    $scope.customer = "";
    $scope.items = new Array();
    $scope.lProductsOut = new Array();
    $scope.subTotal = 0;
    $scope.checkedDiscount = false;
    $scope.checkedIVA = false;
    $scope.checkedIVAPayment = false;
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
    $scope.Bill = models.Bill();
    $scope.paymentTAX = {};
    $scope.paymentTAXCredit = {};
    $scope.Dollar = 0.0;
    $scope.bankAccountStatus = [];
    $scope.terminalTypeStatus = [];

    $scope.typesPaymentItems = new Array();
    $scope.paymentMonths = new Array();
    $scope.Allpayments = {};

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    //Asigna el tipo de cambio
    $scope.SetValueDollars = function () {
        setTimeout(function () {
            $("#openModalDollar").modal("show");
        }, 1);        
    };

    $scope.ModalSetDollar = function () {
        $("#openModalDollar").modal("hide");
        $scope.SetDollar();
    };
       
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
                    $scope.branch = data.oData.Branch.Nombre;
                    $scope.IVATasa = data.oData.Branch.IVATasa;
                    $scope.branchID = data.oData.Branch.idSucursal;

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
            url: '../../../Products/GetProduct',
            params: {
                idProduct: ($scope.selectedCode == "" || $scope.selectedCode == null) ? "" : $scope.selectedCode.originalObject.idProducto
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                var result = _.result(_.find($scope.items, function (chr) {

                    return chr.idProducto == data.oData.Product.idProducto

                }), 'idProducto');

                if (result == undefined) {
                    0

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
                        comentarios: ""
                    });

                } else {

                    var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));

                    $scope.items[index].cantidad++;

                }

                $scope.ValidateStock(data.oData.Product.idProducto);

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
   
    $scope.GetProductQuotation = function () {

        $http({
            method: 'POST',
            url: '../../../Products/GetProductQuotation',
            params: {
                idProduct: ($scope.selectedCode == "" || $scope.selectedCode == null) ? "" : $scope.selectedCode.originalObject.idProducto
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                var result = _.result(_.find($scope.items, function (chr) {

                    return chr.idProducto == data.oData.Product.idProducto

                }), 'idProducto');

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
                        comentarios: ""
                    });

                } else {

                    var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));

                    $scope.items[index].cantidad++;

                }

                $scope.ValidateStockQuotation(data.oData.Product.idProducto);

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

    $scope.GetStockProductQuotation = function (idProduct) {

        $http({
            method: 'POST',
            url: '../../../Products/GetProductQuotation',
            params: {
                idProduct: idProduct
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

    $scope.GetProductForId = function () {

        var res = $scope.barcode.match(/^\d{12}$/);

        if (($scope.barcode.length == 12) && (res.length > 0))// Enter key hit
        {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductForId',
                params: {
                    idProduct: $scope.barcode
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    var result = _.result(_.find($scope.items, function (chr) {

                        return chr.idProducto == data.oData.Product.idProducto

                    }), 'idProducto');

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
                            comentarios: ""
                        });

                    } else {

                        var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));

                        $scope.items[index].cantidad++;

                    }

                    $scope.ValidateStock(data.oData.Product.idProducto);

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

            $scope.barcode = "";

        }

    };

    $scope.SetDollar = function () {
        angular.forEach($scope.items, function (value, key) {
            if (value.servicio == true && value.nodolar == 0) {
                value.prec = value.dolar * $scope.Dollar;
            } 
        });

        $scope.CalculateTotalCost();
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

        if (($scope.checkedDiscount = true) && ($scope.discount > 0)) {

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

            discount = (($scope.subTotal) + $scope.totalCreditNotes) * percentage;

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

    $scope.ValidateStock = function (ID) {

        var service = _.result(_.find($scope.items, function (chr) {

            return chr.idProducto == ID

        }), 'servicio');

        if (service == false) {

            var exist = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'existencia');

            exist = (exist == undefined) ? 0 : exist;

            var stock = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'stock');

            var amount = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'cantidad');

            if (exist < amount) {

                var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: ID }));

                if (exist == 0 && index > -1) {
                    $scope.items.splice(index, 1);
                } else {
                    $scope.items[index].cantidad = exist;
                }

                if ($scope.idView > 0) {
                    $scope.GetStockProductOutProduct(ID);
                } else {
                    $scope.GetStockProduct(ID);
                }

            }
        }

        $scope.CalculateTotalCost();

    };

    $scope.ValidateStockQuotation = function (ID) {

        var service = _.result(_.find($scope.items, function (chr) {

            return chr.idProducto == ID

        }), 'servicio');

        if (service == false) {

            var exist = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'existencia');

            exist = (exist == undefined) ? 0 : exist;

            var stock = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'stock');

            var amount = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'cantidad');

            if (exist < amount) {

                var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: ID }));

                if (exist == 0 && index > -1) {
                    $scope.items.splice(index, 1);
                } else {
                    $scope.items[index].cantidad = exist;
                }

                $scope.GetStockProductQuotation(ID);

            }
        }

        $scope.CalculateTotalCost();

    };

    $scope.DeleteProduct = function (ID) {

        _.remove($scope.items, function (n) {

            return n.idProducto == ID;

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

    $scope.OpenModalCredit = function () {

        $("#modalCredit").modal("show");

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
            dolar: $scope.newService.salePriceService,
            nodolar: 1
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

    $scope.AddCredit = function () {

        $http({
            method: 'POST',
            url: '../../../Credits/GetCreditNote',
            data: {
                remision: $scope.searchStr
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               if (_.find($scope.items, function (o) { return o.codigo == data.oData.CreditNote.RemisionCredito; }) == undefined) {

                   $scope.items.push({
                       idProducto: data.oData.CreditNote.idNotaCredito,
                       idProveedor: null,
                       imagen: "",
                       codigo: data.oData.CreditNote.RemisionCredito,
                       desc: "Nota de crédito. " + data.oData.CreditNote.RemisionCredito,
                       prec: -data.oData.CreditNote.Cantidad,
                       descuento: 0,
                       existencia: 1,
                       stock: 1,
                       cantidad: 1,
                       costo: 0,
                       servicio: false,
                       credito: true,
                       comentarios: ""
                   });

                   $scope.CalculateTotalCost();

               }

               $scope.selectedRemision = "";

               $("#modalCredit").modal("hide");

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

    $scope.RemoveTypePayment = function () {

        var a = this;

        _.remove($scope.typesPaymentItems, function (n) {
            return n == a.item;
        });

    };

    $scope.CalculateTotalCostTypesPayment = function (payment) {

        var IVATypesPayment = 0;
        var IVA = 0;

        if (payment.IVA) {
            $scope.checkedIVAPayment = true;

            payment.amountIVA = (payment.amount * ($scope.IVATasa / 100));
            
            payment.amount = payment.amount + payment.amountIVA;

            $scope.total = $scope.total + payment.amountIVA;

        } else {
            $scope.checkedIVAPayment = false;

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
                        codigo: value.codigo,
                        imagen: value.imagen,
                        desc: value.desc,
                        prec: value.prec,
                        descuento: parseFloat(value.descuento),
                        cantidad: value.cantidad,
                        idServicio: value.idProducto.split("-")[0],
                        credito: value.credito,
                        comentarios: value.comentarios,
                        tipo: 2
                    });

                } else if (value.credito == true) {

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
                        tipo: 3
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
                        tipo: 1
                    });

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

            var ivaPayment = 0;
            if ($scope.checkedIVAPayment == true) {                
                ivaPayment = 1;
            } else if ($scope.checkedIVA == true) {                
                ivaPayment = 1;
            }

            $http({
                method: 'POST',
                url: '../../../Sales/SaveDollarSale',
                data: {
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
                    IVA: ivaPayment,
                    total: $scope.total,
                    lProducts: products,
                    lProductsOut: $scope.lProductsOut,
                    lTypePayment: $scope.typesPaymentItems,
                    idView: ($scope.idView > 0) ? $scope.idView : null,
                    idQuotation: ($scope.idQuotation != undefined) ? $scope.idQuotation : null
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.remision = data.oData.sRemision;

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

    $scope.GetNumberRem = function () {

        $http({
            method: 'POST',
            url: '../../../Sales/GeneratePrevNumberRem',
            params: {
                idBranch: $scope.branchID
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.remision = data.oData.sRemision;

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

    $scope.Print = function () {

        var URL = '../../../Sales/PrintSale?remision=' + $scope.remision;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.GetNoIFE = function () {

        $scope.numIFE = ($scope.customer.type == "physical") ? $scope.physicalCustomer.NoIFE : null

    };

    $scope.getStates = function () {

        $http({
            method: 'GET',
            url: '../../../Users/GetStates'
        }).
        success(function (data, status, headers, config) {

            $scope.States = data;

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.getTowns = function () {

        $http({
            method: 'GET',
            url: '../../../Users/GetTowns',
            params: {
                idState: $scope.stateBill.idEstado
            }
        }).
        success(function (data, status, headers, config) {

            $scope.Towns = data;

            if ($scope.customer.type == "moral") {

                if ($scope.moralCustomer != undefined) {

                    $scope.setSelecTown($scope.moralCustomer.idMunicipio);

                }

            } else {

                if ($scope.physicalCustomer != undefined) {

                    $scope.setSelecTown($scope.physicalCustomer.idMunicipio);

                }

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.getTownsBill = function () {

        $http({
            method: 'GET',
            url: '../../../Users/GetTowns',
            params: {
                idState: $scope.Bill.stateBill.idEstado
            }
        }).
        success(function (data, status, headers, config) {

            $scope.Towns = data;

            if ($scope.customer.type == "moral") {

                if ($scope.moralCustomer != undefined) {

                    $scope.setSelecTown($scope.moralCustomer.idMunicipio);

                }

            } else {

                if ($scope.physicalCustomer != undefined) {

                    $scope.setSelecTown($scope.physicalCustomer.idMunicipio);

                }

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.setSelecState = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.States, parseInt(a), 'idEstado');

        if ($scope.States != undefined) {

            $scope.stateBill = $scope.States[index];

            if ($scope.stateBill != undefined) {

                $scope.getTowns();

            }

        }

    };

    $scope.setSelecTown = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Towns, parseInt(a), 'idMunicipio');

        if (index != undefined) {

            $scope.townBill = $scope.Towns[index];

        }

    };

    $scope.Bill = function () {

        $("#modalBill").modal("show");

    };

    $scope.BillDeposit = function () {

        $("#modalDeposit").modal("hide")

        $("#modalBill").modal("show");

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {

        if (myArray != undefined) {

            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }

    };

    $scope.SendBill = function () {

        var typeCustomer = 0;

        if ($scope.typeBill == 1) {

            typeCustomer = 2;

        } else if ($scope.customer.type == "moral") {

            typeCustomer = 0;

        } else {

            typeCustomer = 1;

        }

        $http({
            method: 'POST',
            url: '../../../Sales/SendBill',
            params: {
                remision: $scope.remision,
                typeCustomer: typeCustomer,
                idCustomer: ($scope.customer.type == "moral") ? $scope.moralCustomer.idCliente : $scope.physicalCustomer.idCliente,
                concept: $scope.conceptBill,
                name: $scope.nameBill,
                phone: $scope.phoneBill,
                mail: $scope.mailBill,
                rfc: $scope.rfcBill,
                street: $scope.streetBill,
                outNum: $scope.outNumberBill,
                inNum: $scope.intNumberBill,
                suburb: $scope.suburbBill,
                idTown: ($scope.townBill != undefined) ? $scope.townBill.idMunicipio : null,
                cp: $scope.cpBill,
                town: ($scope.townBill != undefined) ? $scope.townBill.Municipio : null,
                state: ($scope.stateBill != undefined) ? $scope.stateBill.Estado : null
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $http.post('../../../Sales/BillSale', { remision: $scope.remision }).then(function () { });

               notify(data.oData.Message, $rootScope.success);

               $("#modalBill").modal("hide");

               $("#modalPrintSale").modal("show");

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

    $scope.SendBillPayment = function (remision) {

        $http({
            method: 'POST',
            url: '../../../Sales/SendBillPayment',
            params: {
                remision: remision,
                amount: $scope.amountDetailBill,
                idCustomer: $scope.idCustomerBill,
                concept: $scope.Bill.conceptBill,
                name: $scope.Bill.nameBill,
                phone: $scope.Bill.phoneBill,
                mail: $scope.Bill.mailBill,
                rfc: $scope.Bill.rfcBill,
                street: $scope.Bill.streetBill,
                outNum: $scope.Bill.outNumberBill,
                inNum: $scope.Bill.intNumberBill,
                suburb: $scope.Bill.suburbBill,
                idTown: ($scope.Bill.townBill != undefined) ? $scope.Bill.townBill.idMunicipio : null,
                cp: $scope.Bill.cpBill,
                town: ($scope.Bill.townBill != undefined) ? $scope.Bill.townBill.Municipio : null,
                state: ($scope.Bill.stateBill != undefined) ? $scope.Bill.stateBill.Estado : null
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               notify(data.oData.Message, $rootScope.success);

               $("#modalBill").modal("hide");

               $("#modalPrintSale").modal("show");

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

    $scope.greaterThan = function (prop, val) {
        return function (item) {
            return item[prop] > val;
        }
    }

    $scope.valResult = {};

    $scope.newPayment = new models.Payment();
    
    $scope.loadSalesFromQuotationForSale = function (outProduct) {

        angular.forEach(outProduct, function (value, key) {

            if (value.idProducto == null && value.idServicio > 0) {

                $scope.items.push({
                    idProducto: value.idServicio + "-" + GUID.New(),
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
                    dolar: value.Precio,
                    nodolar: 0
                });

            } else if (value.idProducto == null && value.idCredito > 0) {

                $scope.items.push({
                    idProducto: value.idNotaCredito,
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
                    comentarios: value.Comentarios
                });

            } else {

                $scope.GetProductForQuotationForSale(value);

            }

        });

        $scope.CalculateTotalCost();

    };

    $scope.GetProductForQuotation = function (prod) {

        if (prod.idProducto != null) {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductQuotation',
                params: {
                    idProduct: prod.idProducto
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    var result = _.result(_.find($scope.items, function (chr) {

                        return chr.idProducto == data.oData.Product.idProducto

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

                                return chr.idSucursal == $scope.branchID;

                            }), 'Existencia'),
                            stock: data.oData.Product.Stock,
                            cantidad: (prod.Cantidad == undefined) ? 0 : prod.Cantidad,
                            costo: 0,
                            servicio: false,
                            credito: false,
                            comentarios: prod.Comentarios
                        });

                    } else {

                        var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));

                        $scope.items[index].cantidad++;

                    }

                    $scope.ValidateStock(data.oData.Product.idProducto);

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

    $scope.GenerateCredit = function (credito) {

        $http({
            method: 'POST',
            url: '../../../Credits/SaveAddCredit',
            data: {
                idCreditNoteType: 1,
                idSale: null,
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

    $scope.GenerateCreditFromPayment = function (credito) {

        $http({
            method: 'POST',
            url: '../../../Credits/SaveAddCreditFromPayment',
            data: {
                idSale: $scope.idSaleModifyPayment,
                amount: credito,
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

    $scope.GenerateCreditFromPaymentCredit = function (idSale, credito) {

        $http({
            method: 'POST',
            url: '../../../Credits/SaveAddCreditFromPayment',
            data: {
                idSale: idSale,
                amount: credito,
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.resCredit = Math.abs(credito);
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

    $scope.PrintCredit = function (idCreditNote) {

        var URL = '../../../Credits/PrintCredit?idCreditNote=' + idCreditNote;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.LoadInformation = function (typeCustomer, customer, idOffice, seller, discount, iva, idBranch) {

        $scope.GetBranchInfo(idBranch);

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

        if (discount > 0) {
            $scope.checkedDiscount = true;
            $scope.discount = parseFloat(discount);
        }

        if (iva == 1) {
            $scope.checkedIVA = true;
        }

    };

    $scope.GetDataBill = function () {

        if ($scope.typeBill == 1) {

            $http({
                method: 'POST',
                url: '../../../Sales/GetDatasBill',
                data: {
                    rfc: $scope.rfcBill
                }
            }).
           success(function (data, status, headers, config) {

               if (data.success == 1 && data.oData.DataBill != null) {

                   $scope.conceptBill = "";
                   $scope.nameBill = data.oData.DataBill.Nombre;
                   $scope.phoneBill = data.oData.DataBill.Telefono;
                   $scope.mailBill = data.oData.DataBill.Correo;
                   $scope.rfcBill = data.oData.DataBill.RFC;
                   $scope.streetBill = data.oData.DataBill.Calle;
                   $scope.outNumberBill = data.oData.DataBill.NumExt;
                   $scope.intNumberBill = data.oData.DataBill.NumInt;
                   $scope.suburbBill = data.oData.DataBill.Colonia;
                   $scope.setSelecState(data.oData.DataBill.idEstado);
                   $scope.setSelecTown(data.oData.DataBill.idMunicipio);
                   $scope.cpBill = data.oData.DataBill.CP;

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

    $scope.GetDataBillPayment = function () {

        $http({
            method: 'POST',
            url: '../../../Sales/GetDatasBill',
            data: {
                rfc: $scope.Bill.rfcBill
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1 && data.oData.DataBill != null) {

               $scope.Bill.conceptBill = "";
               $scope.Bill.nameBill = data.oData.DataBill.Nombre;
               $scope.Bill.phoneBill = data.oData.DataBill.Telefono;
               $scope.Bill.mailBill = data.oData.DataBill.Correo;
               $scope.Bill.rfcBill = data.oData.DataBill.RFC;
               $scope.Bill.streetBill = data.oData.DataBill.Calle;
               $scope.Bill.outNumberBill = data.oData.DataBill.NumExt;
               $scope.Bill.intNumberBill = data.oData.DataBill.NumInt;
               $scope.Bill.suburbBill = data.oData.DataBill.Colonia;
               $scope.setSelecState(data.oData.DataBill.idEstado);
               $scope.setSelecTown(data.oData.DataBill.idMunicipio);
               $scope.Bill.cpBill = data.oData.DataBill.CP;

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

    $scope.GetInformationBill = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/GetInformationBill',
            data: {
                idSale: idSale
            }
        }).
           success(function (data, status, headers, config) {

               if (data.success == 1 && data.oData.DataBill != null) {

                   $scope.remisionBill = data.oData.DataBill.Remision;
                   $scope.typeCustomerBill = data.oData.DataBill.TypeCustomer;
                   $scope.idCustomerBill = data.oData.DataBill.idCustomer;
                   $scope.conceptBill = "";
                   $scope.nameBill = data.oData.DataBill.Nombre;
                   $scope.phoneBill = data.oData.DataBill.Telefono;
                   $scope.mailBill = data.oData.DataBill.Correo;
                   $scope.rfcBill = data.oData.DataBill.RFC;
                   $scope.streetBill = data.oData.DataBill.Calle;
                   $scope.outNumberBill = data.oData.DataBill.NumExt;
                   $scope.intNumberBill = data.oData.DataBill.NumInt;
                   $scope.suburbBill = data.oData.DataBill.Colonia;
                   $scope.setSelecState(data.oData.DataBill.idEstado);
                   $scope.setSelecTown(data.oData.DataBill.idMunicipio);
                   $scope.cpBill = data.oData.DataBill.CP;

               } else if (data.failure == 1) {

                   notify(data.oData.Error, $rootScope.error);

               } else if (data.noLogin == 1) {

                   window.location = "../../../Access/Close";

               }

           }).
           error(function (data, status, headers, config) {

               notify("Ocurrío un error.", $rootScope.error);

           });

    },

    $scope.GetInformationBillPayment = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/GetInformationBill',
            data: {
                idSale: idSale
            }
        }).
           success(function (data, status, headers, config) {

               if (data.success == 1 && data.oData.DataBill != null) {

                   $scope.Bill.remisionBill = data.oData.DataBill.Remision;
                   $scope.Bill.typeCustomerBill = data.oData.DataBill.TypeCustomer;
                   $scope.Bill.idCustomerBill = data.oData.DataBill.idCustomer;
                   $scope.Bill.conceptBill = "";
                   $scope.Bill.nameBill = data.oData.DataBill.Nombre;
                   $scope.Bill.phoneBill = data.oData.DataBill.Telefono;
                   $scope.Bill.mailBill = data.oData.DataBill.Correo;
                   $scope.Bill.rfcBill = data.oData.DataBill.RFC;
                   $scope.Bill.streetBill = data.oData.DataBill.Calle;
                   $scope.Bill.outNumberBill = data.oData.DataBill.NumExt;
                   $scope.Bill.intNumberBill = data.oData.DataBill.NumInt;
                   $scope.Bill.suburbBill = data.oData.DataBill.Colonia;
                   $scope.setSelecState(data.oData.DataBill.idEstado);
                   $scope.setSelecTown(data.oData.DataBill.idMunicipio);
                   $scope.Bill.cpBill = data.oData.DataBill.CP;

               } else if (data.failure == 1) {

                   notify(data.oData.Error, $rootScope.error);

               } else if (data.noLogin == 1) {

                   window.location = "../../../Access/Close";

               }

           }).
           error(function (data, status, headers, config) {

               notify("Ocurrío un error.", $rootScope.error);

           });

    },

    $scope.OpenAddTax = function (idSale) {

        $scope.updateTime = Date.now();

        $scope.includeURLAddTax = "OpenAddTax?idSale=" + idSale + "&updated=" + $scope.updateTime;

        $("#modalAddTax").modal("show");

    };

    $scope.SaveAddTax = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/AddTax',
            data: {
                idSale: idSale, amount: this.amount, typePayment: this.typesPayment,
                typeCard: this.typesCard, bank: this.bank, holder: this.holder, check: this.numCheck, noIFE: this.numIFE
            }
        }).
           success(function (data, status, headers, config) {

               if (data.success == 1) {

                   $("#modalBill").modal("show");

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

    $scope.SaveAddTaxPayment = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/SaveAddTaxPayment',
            data: {
                idSale: idSale,
                idPayment: $scope.paymentTAX.idPayment,
                amount: $scope.paymentTAX.amount + $scope.paymentTAX.amountIVA,
                amountIVA: $scope.paymentTAX.amountIVA
            }
        }).
           success(function (data, status, headers, config) {

               if (data.success == 1) {

                   $("#modalBill").modal("hide");

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

    $scope.SaveAddTaxPaymentCredit = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/SaveAddTaxPaymentCredit',
            data: {
                idSale: idSale,
                idCreditHistory: $scope.paymentTAXCredit.idPayment,
                amount: $scope.paymentTAXCredit.amount + $scope.paymentTAXCredit.amountIVA,
                amountIVA: $scope.paymentTAXCredit.amountIVA
            }
        }).
           success(function (data, status, headers, config) {

               if (data.success == 1) {

                   $("#modalBillCredit").modal("hide");

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

    $scope.newDetailFactura = new models.DetailSaleFactura();

    $scope.SaveNumberBill = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/SetNumberBill',
            data: {
                idSale: idSale,
                numberBill: $scope.newDetailFactura.numberBill
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

    $scope.ChangeTextButtons = function () {
        $("#modalDeleteItem .confirmbutton-yes").text("Enviar");
        $("#modalDeleteItem .confirmbutton-no").text("Solo guardar")
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

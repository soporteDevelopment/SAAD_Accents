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
    $scope.IVATasa = 0;
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
    $scope.myFiles = [];
    $scope.bankAccountStatus = [];
    $scope.terminalTypeStatus = [];

    $scope.branchadd = {
        prod: "A"
    };
    $scope.branchID = 2;
    $scope.currentBranchID = 2;

    $scope.typesPaymentItems = new Array();
    $scope.paymentMonths = new Array();
    $scope.Allpayments = {};

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;


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

    //Inicializa la variable CUSTOMER que se utiliza al momento de hacer una venta en el listado de tipo de cliente

    $scope.customer = {
        type: "moral"
    };

    //Indica en que sucursal está logueado el usuario al momento de hacer la venta

    $scope.SetBranch = function (ID) {
        $scope.branchID = ID;
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
                        $scope.currentBranchID = data.oData.Branch.idSucursal;
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

                    return chr.idProducto == data.oData.Product.idProducto && chr.idSucursal == $scope.branchID

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
                        comentarios: "",
                        Sucursal: "",
                        idSucursal: $scope.branchID,
                        idVista: "",
                        remision: ""
                    });

                } else {

                    var index = _.indexOf($scope.items, _.find($scope.items, function (chr) {

                        return chr.idProducto == data.oData.Product.idProducto && chr.idSucursal == $scope.branchID

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

    $scope.CalculateTotalCostForEraserSale = function () {

        if ($scope.discount > 0) {
            $scope.checkedDiscount = true;
        }

        $scope.CalculateTotalCostWithoutDiscount();
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

    $scope.ValidateStock = function (ID, branchID) {

        var service = _.result(_.find($scope.items, function (chr) {

            return chr.idProducto == ID

        }), 'servicio');

        if (service == false) {

            var exist = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID && chr.idSucursal == branchID;

            }), 'existencia');

            exist = (exist == undefined) ? 0 : exist;

            var amount = 0;

            amount = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID && chr.idSucursal == branchID;

            }), 'cantidad');

            if (exist < amount) {

                var index = 0

                index = _.indexOf($scope.items, _.find($scope.items, function (chr) {
                    return chr.idProducto == ID && chr.idSucursal == branchID;
                }));
                
                if (exist == 0 && index > -1) {
                    $scope.items.splice(index, 1);
                } else {
                    $scope.items[index].cantidad = exist;
                }

                $scope.GetStockProduct(ID);
            }
        }

        $scope.GetBranchInfo(branchID);

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
            comentarios: $scope.newService.commentsService
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

        angular.forEach($scope.items, function (value, key) {
            if (value.cantidad <= 0) {
                $("#txtValidation").append("No se permiten cantidades menores o iguales a 0");

                showModal = true;
            }
        });

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
                        tipo: 2,
                        idSucursal: 0,
                        idVista: 0
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
                        tipo: 3,
                        idSucursal: 0,
                        idVista: 0
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
                        idVista: value.idView
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
                url: '../../../Sales/SaveSaleAllStore',
                data: {
                    idUser1: $scope.sellerOne.idUsuario,
                    idUser2: ($scope.sellerTwo == undefined) ? null : $scope.sellerTwo.idUsuario,
                    idCustomerP: ($scope.customer.type == "physical") ? $scope.physicalCustomer.idCliente : null,
                    idCustomerM: ($scope.customer.type == "moral") ? $scope.moralCustomer.idCliente : null,
                    idOffice: ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
                    project: $scope.project,
                    typeCustomer: typeCustomer,
                    idOfficeReference: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null,
                    idBranch: $scope.currentBranchID,
                    dateSale: $scope.dateTime,
                    amountProducts: amount,
                    subtotal: $scope.subTotal,
                    discount: ($scope.checkedDiscount == true) ? parseFloat($scope.discount) : 0,
                    IVA: ivaPayment,                    
                    total: $scope.total,
                    lProducts: products,
                    lTypePayment: $scope.typesPaymentItems
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
            url: '../../../Sales/GeneratePrevNumberRemC'
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

    $scope.PrintSale = function (remision) {

        var URL = '../../../Sales/PrintSale?remision=' + remision;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.PrintSaleWithBankInformation = function (remision) {

        var URL = '../../../Sales/PrintSaleWithBankInformation?remision=' + remision;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.PrintDeposit = function () {

        var URL = '../../../Sales/PrintDeposit?remision=' + $scope.remision + '&deposit=' + $scope.deposit;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.PrintDepositOnList = function (deposit) {

        var URL = '../../../Sales/PrintDepositOnline?idSale=' + $scope.newPayment.idSalePayment + '&deposit=' + deposit;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.GetNoIFE = function () {

        $scope.numIFE = ($scope.customer.type == "physical") ? $scope.physicalCustomer.NoIFE : null

    };

    $scope.setIdBranch = function () {

        if ($scope.branchadd.prod == "A") { $scope.branchID = 2; } else if ($scope.branchadd.prod == "G") { $scope.branchID = 3; } else { $scope.branchID = 4; }
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

    $scope.DetailSale = function (remision) {

        $scope.includeURL = "DetailSale?remision=" + remision;

        $("#modalDetailSaleOnLine").modal("show");

    };

    $scope.DetailSaleBill = function (item) {

        $scope.amountDetailBill = item.amount;

        $scope.paymentTAX = {
            idPayment: item.idTypePayment,
            amount: item.amount,
            amountIVA: item.amount * .16,
            IVA: item.IVA
        }

        $("#modalBill").modal("show");

    };

    $scope.DetailSaleBillCredit = function (item) {

        $scope.amountDetailBill = item.amount;

        $scope.paymentTAXCredit = {
            idPayment: item.idTypePayment,
            amount: item.amount,
            amountIVA: item.amount * .16,
            IVA: item.IVA
        }

        $("#modalBillCredit").modal("show");
    }

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

    $scope.PaymentHistorySale = function (idSale) {

        $scope.updateTime = Date.now();

        $scope.includeURLPayment = "InsertPayment?idSale=" + idSale + "&updated=" + $scope.updateTime;

        $("#modalPaymentSaleOnLine").modal("show");

    };

    $scope.init = function (detail) {

        $scope.items = detail;

    };

    $scope.initPayment = function (history) {

        this.history = history;

    };

    $scope.ChangeStatus = function (ID, status) {

        $scope.idSaleStatus = ID;
        $scope.statusSale = status;

        $("#openModalStatus").modal("show");

    };

    $scope.SetStatus = function () {

        $http({
            method: 'POST',
            url: '../../../Sales/SetStatus',
            params: {
                idSale: $scope.idSaleStatus,
                status: $scope.statusSale,
                comments: $scope.comments
            }
        }).
        success(function (data, status, headers, config) {

            $("#openModalStatus").modal("hide");

            if (data.success == 1) {

                $scope.listSales();

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

    $scope.SetStatusCredit = function (IDSale, ID, dateModify) {

        $http({
            method: 'POST',
            url: '../../../Sales/SetStatusCredit',
            params: {
                idCredit: ID,
                status: this.statusCredit,
                dateModify: dateModify
            }
        }).
        success(function (data, status, headers, config) {

            $("#openModalStatusCredit").modal("hide");

            if (data.success == 1) {

                $scope.PaymentHistorySale(IDSale);

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

    $scope.SetStatusCreditDetail = function (IDSale, ID, dateModify, account, voucher) {

        $http({
            method: 'POST',
            url: '../../../Sales/SetStatusCredit',
            params: {
                idCredit: ID,
                status: this.statusCredit,
                dateModify: dateModify,
                idCuenta: account,
                voucher: voucher
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

    $scope.valResult = {};

    $scope.newPayment = new models.Payment();

    $scope.SaveInsertPayment = function () {

        $scope.valResult = PaymentValidator.validate($scope.newPayment);

        if ($scope.newPayment.$isValid) {

            $scope.AddInsertPayment();

        }

    };

    $scope.AddInsertPayment = function () {

        $("#btnAbonar").button("loading");

        var left = this.newPayment.paymentLeft - this.newPayment.paymentAmount;

        $http({
            method: 'POST',
            url: '../../../Sales/SaveInsertPayment',
            params: {
                idSale: this.newPayment.idSalePayment,
                amount: this.newPayment.paymentAmount,
                left: left,
                comments: this.newPayment.paymentComment,
                dtInsert: this.newPayment.paymentDate,
                typePayment: this.newPayment.typesPayment,
                typeCard: (this.newPayment.typesPayment == 5) ? this.newPayment.typesCard : null,
                bank: (this.newPayment.typesPayment == 1) ? this.newPayment.bank : null,
                holder: (this.newPayment.typesPayment == 1) ? this.newPayment.holder : null,
                check: (this.newPayment.typesPayment == 1) ? this.newPayment.numCheck : null,
                noIFE: (this.numIFE == 1) ? this.newPayment.numIFE : null,
            }
        }).
        success(function (data, status, headers, config) {

            $("#btnAbonar").button("reset");

            if (data.success == 1) {

                $scope.newPayment.paymentAmount = "";
                $scope.newPayment.paymentComment = "";
                $scope.newPayment.bank = "";
                $scope.newPayment.holder = "";
                $scope.newPayment.numCheck = "";
                $scope.newPayment.numIFE = "";

                notify(data.oData.Message, $rootScope.success);

                $("#modalPaymentSaleOnLine").modal("hide");

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close";

            }

        }).
        error(function (data, status, headers, config) {

            $("#btnAbonar").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

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

    $scope.PrintCredit = function (idCreditNote) {

        var URL = '../../../Credits/PrintCredit?idCreditNote=' + idCreditNote;

        var win = window.open(URL, '_blank');
        win.focus();

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

    $scope.setSelectService = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.listServices, a, 'Descripcion');

        $scope.newService.descService = $scope.listServices[index];
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

    },

    $scope.SaveAddTaxPaymentCredit = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/SaveAddTaxPaymentCredit',
            data: {
                idSale: idSale,
                idPayment: $scope.paymentTAXCredit.idPayment,
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

    $scope.ModalPaymentWay = function (idSale, Total) {

        $scope.idSaleModifyPayment = idSale;
        $scope.total = Total;

        $scope.typesPaymentItems = new Array();

        $scope.AddModifyTypePayment();

        $("#modalPayment").modal("show");

    },

    $scope.SaveDatePaymentDetail = function (a) {

        if (a.item.DatePayment == "") {
            notify("La fecha es requerida.", $rootScope.error);
        } else {

            $http({
                method: 'POST',
                url: '../../../Sales/SaveDatePaymentDetail',
                data: {
                    idTypePayment: a.item.idTypePayment,
                    status: $("#sStatusSale" + a.item.idTypePayment).val(),
                    date: a.item.DatePayment,
                    idCuenta: a.item.idCuenta,
                    voucher: a.item.voucher
                }
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
    },

    $scope.OpenCommentsModal = function (ID, idBranch, comments) {
        $scope.productItem = ID;
        $scope.idBranch = idBranch;
        $scope.commentsItem = comments;

        $("#modalComments").modal("show");
    },

    $scope.SaveComments = function () {
        var index = _.findIndex($scope.items, function (o) {
            return o.idProducto == $scope.productItem && o.idSucursal == $scope.branchItem
        });

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
    }

    function fileMethod() {
        document.getElementById('holder').addEventListener('dragover', permitirDrop, false);
        document.getElementById('holder').addEventListener('drop', drop, false);
    }

    // upload on file select or drop
    function drop(ev) {
        ev.preventDefault();
        arch.addEventListener('load', leer, false);
        arch.readAsDataURL(ev.dataTransfer.files[0]);
        $scope.myFiles = ev.dataTransfer.files;

        var file = $scope.myFiles.item(0);
        $("#imgName").val(file.name);

        $scope.rd = 0;
    }

    function permitirDrop(ev) {
        ev.preventDefault();
    }

    function leer(ev) {
        $("#imgProduct").remove();
        document.getElementById('holder').style.backgroundImage = "url('" + ev.target.result + "')";
    }

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

        $scope.items[index].imagen = $scope.newService.imagen;
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

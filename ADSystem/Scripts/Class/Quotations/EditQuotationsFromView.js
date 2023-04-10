angular.module("General").controller('QuotationsController', function (models, ServiceSaleValidator, PaymentValidator, DetailSaleFacturaValidator, GUID, GetDiscount, GetDiscountOffice, GetDiscountSpecial, $scope, $http, $window, notify, $rootScope, FileUploader) {
    $scope.idQuotation = null;
    $scope.selectedCode = "";
    $scope.branch = "";
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
    $scope.includeURLMoral = "";
    $scope.includeURLPhysical = "";
    $scope.barcode = "";
    $scope.Math = window.Math;
    $scope.myFiles = [];
    $scope.availableProducts = [];

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

    $scope.SetBranchName = function (branchName, IDBranch) {
        $scope.branch = branchName;
        $scope.branchID = IDBranch;
        $("#openModal").modal("hide");
        $scope.GetNumberRem();
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

        if ($scope.checkedDiscount == true) {

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

                $scope.GetStockProductOutProduct(ID);
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
            idSucursal: $scope.branchID,
            idPromocion: null,
            idTipoPromocion: null,
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

            $("#txtValidation").append("Ingrese un producto a la cotización </br>");

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

            $scope.SaveUpdateEraserQuotations();

        }

        $("#modalDeleteItem .confirmbutton-yes").text("Si");
        $("#modalDeleteItem .confirmbutton-no").text("No");
    };

    $scope.SaveUpdateEraserQuotations = function () {
        var showModal = false;

        if ($scope.items.length == 0) {
            $("#txtValidation").append("Ingrese un producto a la cotización </br>");

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
                        idPromocion: null,
                        costoPromocion: null,
                        idTipoPromocion: null,
                        idProductoPadre: null
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
                            idPromocion: value.idPromocion,
                            costoPromocion: value.costo,
                            idTipoPromocion: value.idTipoPromocion,
                            idProductoPadre: value.idProductoPadre,
                            idSucursal: $scope.branchID
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
                            idPromocion: null,
                            costoPromocion: null,
                            idTipoPromocion: null,
                            idProductoPadre: null,
                            idSucursal: $scope.branchID
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
                url: '../../../Quotations/SaveEraserQuotationFromView',
                data: {
                    idQuotation: $scope.idQuotation,
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
                    idView: $scope.viewID
                }
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        notify(data.oData.Message, $rootScope.success);
                        window.location = "../../../Quotations/ListQuotations";
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
    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
        if (myArray != undefined) {
            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }
    };

    $scope.greaterThan = function (prop, val) {
        return function (item) {
            return item[prop] > val;
        }
    };

    $scope.init = function (detail) {
        $scope.items = detail;
    };

    $scope.loadDetailFromQuotation = function (outProduct) {
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
                    idSucursal: value.idSucursal,
                    idPromocion: null,
                    idTipoPromocion: null,
                    idProductoPadre: null,
                    tienePromocion: false
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
                    comentarios: value.Comentarios,
                    idSucursal: value.idSucursal,
                    idPromocion: null,
                    idTipoPromocion: null,
                    idProductoPadre: null,
                    tienePromocion: false
                });
            } else {
                $scope.GetProductForQuotation(value);
            }
        });

        $scope.CalculateTotalCost();
    };

    $scope.GetProductOutProduct = function (prod) {

        if (prod.idProducto != null) {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductForOutProduct',
                params: {
                    idView: $scope.viewID,
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
                                prec: prod.Precio,
                                descuento: discount,
                                existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                    return chr.idSucursal == $scope.branchID;

                                }), 'Existencia'),
                                stock: data.oData.Product.Stock,
                                cantidad: prod.Cantidad,
                                costo: 0,
                                servicio: false,
                                credito: false,
                                comentarios: prod.Comentarios,
                                idSucursal: prod.idSucursal,
                                idPromocion: null,
                                idTipoPromocion: null,
                                idProductoPadre: null,
                                tienePromocion: false
                            });

                            $scope.lProductsOut.push({
                                idProducto: data.oData.Product.idProducto,
                                codigo: data.oData.Product.Codigo,
                                desc: data.oData.Product.Descripcion,
                                prec: prod.Precio,
                                descuento: 0,
                                cantidad: prod.Cantidad,
                                costo: 0,
                                idServicio: null,
                                credito: false,
                                comentarios: prod.Comentarios,
                                idSucursal: prod.idSucursal
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

    $scope.GetProductForQuotation = function (prod) {

        if (prod.idProducto != null) {

            $http({
                method: 'GET',
                url: '../../../Products/GetProductQuotationOriginView',
                params: {
                    idProduct: prod.idProducto,
                    idView: $scope.viewID
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
                                comentarios: prod.Comentarios,
                                idSucursal: prod.idSucursal,
                                idPromocion: null,
                                idTipoPromocion: null,
                                idProductoPadre: null,
                                tienePromocion: false
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

    $scope.GetStockProductOutProduct = function (ID) {

        if (ID != undefined) {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductForOutProduct',
                params: {
                    idView: $scope.viewID,
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

                }).
                error(function (data, status, headers, config) {

                    notify("Ocurrío un error.", $rootScope.error);

                    $scope.selectedCode = "";

                });

            $("#product_value").val("");

        }
    };

    $scope.LoadInformation = function (typeCustomer, customer, idOffice, seller, credit, discount, iva, IVATasa) {

        $scope.IVATasa = IVATasa;

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

        //Credit

        if (credit != '') {
            $scope.searchStr = credit;
            $scope.AddCredit();
        }

        if (discount > 0) {
            $scope.checkedDiscount = true;
            $scope.discount = parseFloat(discount);
        }

        if (iva == 1) {
            $scope.checkedIVA = true;
        }
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

    // Carga de imagen
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

    //Promotions
    $scope.GetAvailableProducts = function (idProduct) {
        $scope.availableProducts = [];
        angular.forEach($scope.items, function (value, key) {
            var index = _.findIndex($scope.items, function (o) { return o.idProductoPadre == value.idProducto });
            if (value.idProducto != idProduct
                && value.idPromocion == null
                && value.servicio == false
                && index == -1) {
                $scope.availableProducts.push(value);
            }
        });
    }

    $scope.OpenPromotionModal = function (item) {
        $scope.product = item;

        $scope.GetAvailableProducts(item.idProducto);

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
                            if (value.idProductoPadre == $scope.product.idProducto) {
                                value.idPromocion = null;
                                value.idTipoPromocion = null;
                                value.idProductoPadre = null;
                            }
                        });

                        angular.forEach($scope.items, function (value, key) {
                            //Si es el producto seleccionado y el precio de este es menor al producto padre
                            if (value.idProducto == $scope.relatedProduct.idProducto) {
                                if ($scope.relatedProduct.prec < $scope.product.prec) {
                                    value.descuento = 50;
                                    value.idPromocion = data.oData.Promotion.idPromocion;
                                    value.idTipoPromocion = data.oData.Promotion.idTipoPromocion;
                                    value.idProductoPadre = $scope.product.idProducto;
                                    value.tienePromocion = true;

                                    angular.forEach($scope.items, function (item, key) {
                                        if (item.idProducto == $scope.product.idProducto) {
                                            item.tienePromocion = true;
                                        }
                                    });
                                }
                            } else if (value.idProducto == $scope.product.idProducto) {
                                if ($scope.product.prec < $scope.relatedProduct.prec) {
                                    value.descuento = 50;
                                    value.idPromocion = data.oData.Promotion.idPromocion;
                                    value.idTipoPromocion = data.oData.Promotion.idTipoPromocion;
                                    value.idProductoPadre = $scope.relatedProduct.idProducto;
                                    value.tienePromocion = true;

                                    angular.forEach($scope.items, function (item, key) {
                                        if (item.idProducto == $scope.relatedProduct.idProducto) {
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

angular.module("General").controller('OutProductsController', function (models, ServiceSaleValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.selectedCode = "";
    $scope.branch = "";
    $scope.datetime = new Date();
    $scope.items = new Array();
    $scope.subTotal = 0;
    $scope.checked = false;
    $scope.total = 0;
    $scope.countService = 0;
    $scope.CantidadProductos = 0;
    $scope.customer = {

        type: "moral"

    };
    $scope.cantidad = 0;
    $scope.estatus = "";
    $scope.outProducts = new Array();
    $scope.includeURLMoral = "";
    $scope.includeURLPhysical = "";
    $scope.barcode = {};
    $scope.Supervisor = {
        idSupervisor: ""
    };

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

        if ($scope.branchID != 1) {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductOutProduct',
                params: {
                    idBranch: $scope.branchID,
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
                            imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                            codigo: data.oData.Product.Codigo,
                            desc: data.oData.Product.Descripcion,
                            prec: data.oData.Product.PrecioVenta,
                            existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                return chr.idSucursal == $scope.branchID;

                            }), 'Existencia'),
                            stock: data.oData.Product.Stock,
                            cantidad: 1,
                            costo: 0,
                            servicio: false,
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

                    window.location = "../../../Access/Close"

                }

                $scope.selectedCode = "";

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $scope.selectedCode = "";

            });

        } else {

            notify("Seleccione una sucursal.", $rootScope.error);

        }

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

                window.location = "../../../Access/Close"

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
                    idProduct: res
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
                            imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                            codigo: data.oData.Product.Codigo,
                            desc: data.oData.Product.Descripcion,
                            prec: data.oData.Product.PrecioVenta,
                            existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                return chr.idSucursal == $scope.branchID;

                            }), 'Existencia'),
                            stock: data.oData.Product.Stock,
                            cantidad: 1,
                            costo: 0,
                            servicio: false,                           
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

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

            $scope.barcode = "";

        }

    };

    $scope.GetProductOutProduct = function (prod) {

        if (prod.idProducto != null) {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductOutProduct',
                params: {
                    idBranch: $scope.branchID,
                    idProduct: prod.idProducto
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
                            imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                            codigo: data.oData.Product.Codigo,
                            desc: data.oData.Product.Descripcion,
                            prec: prod.Precio,
                            descuento: 0,
                            existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                return chr.idSucursal == $scope.branchID;

                            }), 'Existencia'),
                            stock: data.oData.Product.Stock,
                            cantidad: prod.Cantidad,
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

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

        }
    };

    $scope.CalculateTotalCost = function () {

        $scope.subTotal = 0;
        $scope.CantidadProductos = 0;

        angular.forEach($scope.items, function (value, key) {
            value.costo = value.cantidad * value.prec;
            $scope.subTotal = $scope.subTotal + value.costo;
            $scope.CantidadProductos = $scope.CantidadProductos + parseInt(value.cantidad);
        });

        $scope.total = $scope.subTotal;
    };

    $scope.ValidateStock = function (ID) {

        var service = _.result(_.find($scope.items, function (chr) {

            return chr.idProducto == ID

        }), 'servicio');

        if (service == false) {

            var exist = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'existencia');

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

                $scope.GetStockProduct(ID);

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
            idProducto: $scope.newService.descService.idServicio,
            imagen: "",
            codigo: $scope.newService.descService.Descripcion,
            desc: $scope.newService.descService.Descripcion,
            prec: $scope.newService.salePriceService,
            existencia: $scope.newService.amountService,
            stock: $scope.newService.amountService,
            cantidad: $scope.newService.amountService,
            costo: 0,
            servicio: true,
            comentarios: $scope.newService.commentsService
        });

        $scope.descService = "";
        $scope.newService.salePriceService = 0;
        $scope.newService.amountService = 0;
        $scope.newService.commentsService = "";

        $scope.CalculateTotalCost();

        $("#modalService").modal("hide");
    };

    $scope.ConfirmOutProducts = function () {

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

        if ($scope.customer.type == "office") {
            if (($scope.officeCustomer == undefined) || ($scope.officeCustomer == null)) {
                $("#txtValidation").append("Seleccione un despacho  </br>");
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

        if (($scope.sellerThree == undefined) || ($scope.sellerThree == null)) {
            $("#txtValidation").append("Seleccione quien verificó la salida de los productos </br>");
            showModal = true;
        }

        if ($scope.items.length == 0) {
            $("#txtValidation").append("Ingrese un producto </br>");
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
            $scope.SaveAddOutProducts();
        }
    };

    $scope.SaveAddOutProducts = function () {

        var r = confirm("Esta seguro que desea finalizar la salida a vista?");

        if (r == true) {

            var idCustomerMoral = null;
            var idCustomerPhysical = null
            var idSellertwo = null;
            var idDespacho = null;

            if ($scope.moralCustomer != undefined && $scope.moralCustomer.idCliente != undefined) {
                idCustomerMoral = $scope.moralCustomer.idCliente;
            } else if ($scope.physicalCustomer != undefined) {
                idCustomerPhysical = $scope.physicalCustomer.idCliente;
            }
            
            if ($scope.sellerTwo != undefined) {
                idSellertwo = $scope.sellerTwo.idUsuario;
            }

            $scope.buttonDisabled = true;

            var products = new Array();

            var amount = 0;

            angular.forEach($scope.items, function (value, key) {


                if (!value.servicio) {

                    amount = amount + parseInt(value.cantidad);

                }                              

                products.push({
                    idProducto: value.idProducto,
                    imagen: "",
                    codigo: value.codigo,
                    desc: value.desc,
                    prec: value.prec,
                    existencia: value.existencia,
                    stock: value.stock,
                    cantidad: value.cantidad,
                    costo: value.costo,
                    servicio: value.servicio,
                    comentarios: value.comentarios
                });

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
                url: '../../../OutProducts/SaveAddOutProducts',
                data: {
                    idEraserView: ($scope.idView == undefined)? null : $scope.idView,
                    idClienteFisico: idCustomerPhysical,//$scope.newBrand.description,
                    idClienteMoral: idCustomerMoral,//$scope.newBrand.description,
                    idOffice: ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
                    project: $scope.project,
                    typeCustomer: typeCustomer,
                    idOfficeReference: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null,
                    idSucursal: $scope.branchID,//$scope.newBrand.name,
                    Fecha: $scope.datetime,
                    CantidadProductos: amount,
                    Subtotal: $scope.subTotal,
                    Total: $scope.total,
                    idVendedor: $scope.sellerOne.idUsuario,
                    idVendedor2: idSellertwo,
                    idVerificador: $scope.sellerThree.idUsuario,
                    Items: products,
                    freight: ($scope.checkedFreight == undefined) ? false : $scope.checkedFreight
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.oremision = data.oData.sRemision;

                    notify(data.oData.Message, $rootScope.success);

                    $('#modalPrint').on('hidden.bs.modal', function (e) {

                        window.location = "../../../OutProducts/Index";

                    });

                    $("#modalPrint").modal("show");

                    //$window.location = '../../../OutProducts/Index';

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

                $scope.buttonDisabled = false;

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $scope.buttonDisabled = false;

            });

        }

    };

    $scope.SaveEraserOutProduct = function () {

        var r = confirm("Está seguro de guardar en borrador la salida a vista?");

        if (r == true) {

            var idCustomerMoral = null;
            var idCustomerPhysical = null;
            var idSellertwo = null;

            if ($scope.moralCustomer != undefined && $scope.moralCustomer.idCliente != undefined) {
                idCustomerMoral = $scope.moralCustomer.idCliente;
            } else if ($scope.physicalCustomer != undefined) {
                idCustomerPhysical = $scope.physicalCustomer.idCliente;
            }

            if ($scope.sellerTwo != undefined) {
                idSellertwo = $scope.sellerTwo.idUsuario;
            }

            $scope.buttonDisabled = true;

            var products = new Array();

            var amount = 0;

            angular.forEach($scope.items, function (value, key) {

                if (!value.servicio) {

                    amount = amount + parseInt(value.cantidad);

                }

                products.push({
                    idProducto: value.idProducto,
                    imagen: "",
                    codigo: value.codigo,
                    desc: value.desc,
                    prec: value.prec,
                    existencia: value.existencia,
                    stock: value.stock,
                    cantidad: value.cantidad,
                    costo: value.costo,
                    servicio: value.servicio,
                    comentarios: value.comentarios
                });

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
                url: '../../../OutProducts/SaveAddEraserOutProducts',
                data: {
                    idClienteFisico: idCustomerPhysical,//$scope.newBrand.description,
                    idClienteMoral: idCustomerMoral,//$scope.newBrand.description,
                    idOffice: ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
                    project: $scope.project,
                    typeCustomer: typeCustomer,
                    idOfficeReference: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null,
                    idSucursal: $scope.branchID,//$scope.newBrand.name,
                    Fecha: $scope.datetime,
                    CantidadProductos: amount,
                    Subtotal: $scope.subTotal,
                    Total: $scope.total,
                    idVendedor: $scope.sellerOne.idUsuario,
                    idVendedor2: idSellertwo,
                    flete: ($scope.checkedFreight == undefined) ? false : $scope.checkedFreight,
                    Items: products
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    location.reload();

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

                $scope.buttonDisabled = false;

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $scope.buttonDisabled = false;

            });

        }

    };

    $scope.SaveUpdateEraserOutProducts = function () {

        var r = confirm("Está seguro de guardar en borrador la salida a vista?");

        if (r == true) {

            var idCustomerMoral = null;
            var idCustomerPhysical = null;
            var idSellertwo = null;
            var idDespacho = null;

            if ($scope.moralCustomer != undefined && $scope.moralCustomer.idCliente != undefined) {
                idCustomerMoral = $scope.moralCustomer.idCliente;
            } else if ($scope.physicalCustomer != undefined) {
                idCustomerPhysical = $scope.physicalCustomer.idCliente;
            }

            

            if ($scope.office != undefined && $scope.office.idDespacho != undefined) {

                if ($scope.checkedOffices == true) {

                    idDespacho = $scope.office.idDespacho;

                } else {

                    idDespacho = null;

                }
                
            }

            if ($scope.sellerTwo != undefined) {
                idSellertwo = $scope.sellerTwo.idUsuario;
            }

            $scope.buttonDisabled = true;

            var products = new Array();

            var amount = 0;

            angular.forEach($scope.items, function (value, key) {

                amount = amount + parseInt(value.cantidad);

                products.push({
                    idProducto: value.idProducto,
                    imagen: "",
                    codigo: value.codigo,
                    desc: value.desc,
                    prec: value.prec,
                    existencia: value.existencia,
                    stock: value.stock,
                    cantidad: value.cantidad,
                    costo: value.costo,
                    servicio: value.servicio,
                    comentarios: value.comentarios
                });

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
                url: '../../../OutProducts/SaveUpdateEraserOutProducts',
                data: {
                    idEraserView: $scope.idView,
                    idClienteFisico: idCustomerPhysical,//$scope.newBrand.description,
                    idClienteMoral: idCustomerMoral,//$scope.newBrand.description,
                    idOffice: ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
                    project: $scope.project,
                    typeCustomer: typeCustomer,
                    idOfficeReference: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null,
                    idSucursal: $scope.branchID,//$scope.newBrand.name,
                    Fecha: $scope.datetime,
                    CantidadProductos: amount,
                    Subtotal: $scope.subTotal,
                    Total: $scope.total,
                    idVendedor: $scope.sellerOne.idUsuario,
                    idVendedor2: idSellertwo,
                    Items: products
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    window.location = "../../../OutProducts/ListEraserOutProducts"

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

                $scope.buttonDisabled = false;

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $scope.buttonDisabled = false;

            });

        }

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

               window.location = "../../../Access/Close"

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

               window.location = "../../../Access/Close"

           }

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };
    
    $scope.PendingOutProducts = function (count) {

        if (count > 0) {

            $http({
                method: 'POST',
                url: '../../../OutProducts/GetPendingOutProducts'
            }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {

                    $scope.Pendings = data.oData.Pendings;

                    $("#modalPendingOutProducts").modal("show");

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
   
    $scope.GetNumberRem = function () {

        $http({
            method: 'POST',
            url: '../../../OutProducts/GeneratePrevNumberRem',
            params: {
                idBranch: $scope.branchID
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.oremision = data.oData.sRemision;

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

    $scope.init = function (detail) {

        $scope.items = detail;

    };

    $scope.saveUpdateStock = function (ID) {

        $("#saveUpdateStock").button("loading");

        $scope.saveUpdateReturnStock(ID);

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

        if ($scope.Supervisor.idSupervisor.idUsuario == undefined) {

            alert("Indique quien realizó la verificación.");

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

                        window.location = "../../../Access/Close"

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

    },

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

                   window.location = "../../../Access/Close"

               }

           }).error(function (data, status, headers, config) {

               notify("Ocurrío un error.", $rootScope.error);

           });

       } else {

           notify("Ingrese una cuenta de correo", $rootScope.error);

       }
   }

    //Update EraserOutProduct

    $scope.LoadInformation = function (typeCustomer, customer, idOffice, seller) {

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

    $scope.loadSalesFromOutProduct = function (outProduct) {

        angular.forEach(outProduct.oDetail, function (value, key) {

            $scope.GetProductOutProduct(value);

        });

        angular.forEach(outProduct.oServicios, function (value, key) {

            $scope.items.push({
                idProducto: value.idServicio,
                imagen: "",
                codigo: value.Descripcion,
                desc: value.Descripcion,
                prec: value.Precio,
                descuento: 0,
                existencia: value.Cantidad,
                stock: value.Cantidad,
                cantidad: value.Cantidad,
                costo: 0,
                servicio: true,
                credito: false,
                comentarios: value.Comentarios
            });

        });

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {

        if (myArray != undefined) {

            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }

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

    $scope.CleanAnotherCustomer = function (typeCustomer) {

        switch (typeCustomer) {

            case 1:

                    $scope.physicalCustomer = undefined;
                    $scope.officeCustomer = undefined;

                break;

            case 2:

                    $scope.moralCustomer = undefined;
                    $scope.officeCustomer = undefined;

                break;

            case 3:

                    $scope.moralCustomer = undefined;
                    $scope.physicalCustomer = undefined;

                break;

        }

    },

    $scope.createQuotations = function (remision) {

        window.location = "../../../Quotations/CreateQuotationFromOutProduct?remision=" + remision;

    }

});

angular.module("General").controller('GiftsTableController', function (GetDiscount, GetDiscountOffice, GetDiscountSpecial, $scope, $http, $window, notify, $rootScope) {

    //Vairables generales
    $scope.items = new Array();
    $scope.detail = "";
    $scope.actualDateTime;
    $scope.sellerOne;
    $scope.sellerTwo;
    $scope.remision = "";
    $scope.branch = "";
    $scope.branchID = 0;
    $scope.selectedCode = "";
    $scope.openedDateWedding = false;
    $scope.dateWedding = new Date();

    $scope.typesPaymentItems = new Array();

    //Funciones Generales

    $scope.LoadGiftsTable = function (seller, idGiftsTable, idCreditNote, idBoyfriend, Detail, actualDateTime) {

        $scope.idGiftsTable = idGiftsTable;
        $scope.idCreditNote = idCreditNote;
        $scope.idBoyfriend = idBoyfriend;

        $scope.detail = Detail;

        $scope.actualDateTime = actualDateTime;

        $scope.LoadUsers(seller);
        $scope.LoadServices();
        $scope.LoadPhysicalCustomers();
        $scope.LoadMoralCustomers();
        $scope.LoadOffices();

        //SelectSeller
        $scope.sellerOne = '';

        //Seleccionar la sucursal
        setTimeout(function () {

            $("#openModal").modal("show");

        }, 0);

    };

    $scope.LoadProducts = function () {

        angular.forEach($scope.detail, function (value, key) {

            if (value.Tipo == 2) {

                $scope.items.push({
                    idProducto: value.idServicio,
                    imagen: "",
                    codigo: value.Descripcion,
                    desc: value.Descripcion,
                    prec: value.Precio,
                    descuento: value.Descuento,
                    porcentaje: value.Porcentaje,
                    existencia: value.Cantidad,
                    stock: value.Cantidad,
                    cantidad: value.Cantidad,
                    costo: 0,
                    servicio: true,
                    credito: false,
                    comentarios: value.Comentario,
                    tipo:2

                });

            } else if (value.Tipo == 3) {

                $scope.items.push({
                    idProducto: value.idNotaCredito,
                    imagen: "",
                    codigo: value.Credito,
                    desc: "Nota de crédito",
                    prec: value.Precio,
                    descuento: value.Descuento,
                    porcentaje: 0,
                    existencia: 1,
                    stock: 1,
                    cantidad: 1,
                    costo: 0,
                    servicio: false,
                    credito: true,
                    comentarios: value.Comentario,
                    tipo: 3
                });

            } else {

                $scope.GetProductForSale(value);

            }

        });

        $scope.CalculateTotalCost();

    }
    
    $scope.OpenModalService = function () {

        $("#modalService").modal("show");

        $("#modalService").on("show.bs.modal", function (e) {

            $scope.LoadServices();

            $scope.newService = new models.ServiceSale();
        });

    };

    $scope.LoadUsers = function (seller) {

        $http({
            method: 'POST',
            url: '../../../Users/ListAllUsers'
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.usersOne = data.oData.Users;

               if (seller != undefined) {

                   $scope.setSelecUsersOne(seller);

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

    $scope.setSelecUsersOne = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.usersOne, a, 'NombreCompleto');

        $scope.sellerOne = $scope.usersOne[index];

    };

    $scope.setSelecUsersTwo = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.usersTwo, a, 'NombreCompleto');

        $scope.sellerTwo = $scope.usersTwo[index];

    };

    $scope.LoadPhysicalCustomers = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllPhysicalCustomers'
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.physicalCustomers = data.oData.Customers;

               if($scope.idBoyfriend != undefined){

                   var index = $scope.arrayObjectIndexOf($scope.physicalCustomers, $scope.idBoyfriend, 'idCliente');

                   $scope.physicalCustomer = $scope.physicalCustomers[index];

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

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {

        if (myArray != undefined) {

            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }

    };

    $scope.SetBranchName = function (branchName, IDBranch) {

        $scope.branch = branchName;

        $scope.branchID = IDBranch;

        $("#openModal").modal("hide");

        $scope.GetNumberRem();

        $scope.LoadProducts();

    };

    $scope.GetNumberRem = function () {

        $http({
            method: 'POST',
            url: '../../../GiftsTable/GeneratePrevNumberRemForSale',
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

    //Se pueden agregar productos que esten en:
    //Salida a Vista
    //Cotizacion
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

                    $scope.items.push({
                        idProducto: data.oData.Product.idProducto,
                        imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                        codigo: data.oData.Product.Codigo,
                        desc: data.oData.Product.Descripcion,
                        prec: data.oData.Product.PrecioVenta,
                        descuento: 0,
                        porcentaje: 0,
                        existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                            return chr.idSucursal == $scope.branchID;

                        }), 'Existencia'),
                        stock: data.oData.Product.Stock,
                        cantidad: 1,
                        costo: 0,
                        servicio: false,
                        credito: false,
                        comentarios: "",
                        tipo: 1
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

    $scope.GetProductForSale = function (product) {

        $http({
            method: 'POST',
            url: '../../../Products/GetProduct',
            params: {
                idProduct: product.idProducto
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
                        descuento: 0,
                        porcentaje: product.Porcentaje,
                        existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                            return chr.idSucursal == $scope.branchID;

                        }), 'Existencia'),
                        stock: data.oData.Product.Stock,
                        cantidad: product.Cantidad,
                        costo: 0,
                        servicio: false,
                        credito: false,
                        comentarios: "",
                        tipo: 1
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

    //Agregar un servicio
    $scope.AddService = function () {

        if ($scope.addServiceForm.$valid) {

            $scope.SaveService();

        }

    };

    $scope.SaveService = function () {

        $scope.items.push({
            idProducto: $scope.newService.descService.idServicio,
            imagen: "",
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
            tipo: 1
        });

        $scope.descService = "";
        $scope.newService.salePriceService = 0;
        $scope.newService.amountService = 0;
        $scope.newService.commentsService = "";

        $scope.CalculateTotalCost();

        $("#modalService").modal("hide");

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

    //Valida que la EXISTENCIA sea menor o igual a la CANTIDAD DE PRODUCTOS que se quiere adquirir
    $scope.ValidateStock = function (ID) {

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

            //Si la EXISTENCIA es menos a la CANTIDAD INGRESADA se muestra mensaje de existencia al vendedor
            $scope.GetStockProduct(ID);

        }

        $scope.CalculateTotalCost();

    };

    //Muestra mensaje con existencias
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

    };

    //Realiza el cálculo del Total de la Mesa de Regalos
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

        $scope.IVA = (($scope.subTotal - (($scope.subTotal) * ($scope.discount / 100)) + $scope.totalCreditNotes)) * .16;

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

                $scope.total = (($scope.subTotal - discount) + $scope.totalCreditNotes) + $scope.IVA;

            } else {

                $scope.total = ($scope.subTotal - discount) + $scope.totalCreditNotes;
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

            $scope.IVA = (($scope.subTotal - (($scope.subTotal) * ($scope.discount / 100)) + $scope.totalCreditNotes)) * .16;

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

            discount = ($scope.subTotal + $scope.totalCreditNotes )* percentage;

            if ($scope.checkedIVA == true) {

                $scope.total = (($scope.subTotal - discount) + $scope.totalCreditNotes) + $scope.IVA;

            } else {

                $scope.total = ($scope.subTotal - discount) + $scope.totalCreditNotes;
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

    //Borra un artículo de la lista
    $scope.DeleteProduct = function (ID) {

        _.remove($scope.items, function (n) {

            return n.idProducto == ID;

        });

        $scope.CalculateTotalCost();

    };

    //Abre el modal para agregar un comentario
    $scope.OpenCommentsModal = function (ID, comments) {

        $scope.productItem = ID;
        $scope.commentsItem = comments;

        $("#modalComments").modal("show");

    };

    $scope.SaveComments = function () {

        var index = _.findIndex($scope.items, function (o) { return o.idProducto == $scope.productItem });

        if (index > -1) {

            $scope.items[index].comentarios = $scope.commentsItem;

        }

        $("#modalComments").modal("hide");

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
                    IVA: false
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
                IVA: false
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
                    IVA: false
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
                IVA: false
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

        if (($scope.resCredit > 0) || (paymentEmpty != undefined)) {

            $scope.msgTypesPayment = true;

            $scope.missing = $scope.resCredit;

        } else {

            $scope.buttonDisabled = true;

            $("#modalPayment").modal("hide");

            var products = new Array();

            var amount = 0;

            angular.forEach($scope.items, function (value, key) {

                amount = amount + parseInt(value.cantidad);

                products.push({
                    idProducto: (value.servicio == true) ? null : value.idProducto,
                    Codigo: value.codigo,
                    Descripcion: value.desc,
                    Precio: value.prec,
                    Descuento: parseFloat(value.descuento),
                    Cantidad: value.cantidad,
                    Porcentaje: (value.porcentaje == null) ? 0 : value.porcentaje,
                    idServicio: (value.servicio == true) ? value.idProducto : null,
                    idCredito: null,
                    Comentarios: value.comentarios,
                    Tipo: value.Tipo
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
                url: '../../../GiftsTable/SaveGiftsTableForSale',
                data: {
                    idMesaRegalo: $scope.idGiftsTable,
                    idUsuario1: $scope.sellerOne.idUsuario,
                    idUsuario2: ($scope.sellerTwo == undefined) ? null : $scope.sellerTwo.idUsuario,
                    idClienteFisico: ($scope.customer.type == "physical") ? $scope.physicalCustomer.idCliente : null,
                    idClienteMoral: ($scope.customer.type == "moral") ? $scope.moralCustomer.idCliente : null,
                    idDespacho: ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
                    TipoCliente: typeCustomer,
                    idDespachoReferencia: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null,
                    idSucursal: $scope.branchID,
                    Fecha: $scope.dateTime,
                    CantidadProductos: amount,
                    Subtotal: $scope.subTotal,
                    Descuento: ($scope.checkedDiscount == true) ? parseFloat($scope.discount) : 0,
                    IVA: ($scope.checkedIVA == true) ? 1 : 0,
                    Total: $scope.total,
                    oDetail: products,
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

});
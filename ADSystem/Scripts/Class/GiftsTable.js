angular.module("General").controller('GiftsTableController', function (GetDiscount, GetDiscountOffice, GetDiscountSpecial, $scope, $http, $window, notify, $rootScope) {

    //Vairables generales
    $scope.items = new Array();
    $scope.actualDateTime;
    $scope.sellerOne;
    $scope.sellerTwo;
    $scope.remision = "";
    $scope.branch = "";
    $scope.branchID = 0;
    $scope.selectedCode = "";
    $scope.openedDateWedding = false;
    $scope.dateWedding = new Date();

    //Funciones Generales
    $scope.OpenModalService = function () {

        $("#modalService").modal("show");

        $("#modalService").on("show.bs.modal", function (e) {

            $scope.LoadServices();

            $scope.newService = new models.ServiceSale();
        });

    };

    $scope.LoadBoyfriend = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllPhysicalCustomers'
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.boyfriendSelect = data.oData.Customers;

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

    $scope.LoadGirlfriend = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllPhysicalCustomersButNot',
            params: {
                idCustomer: $scope.boyfriend.idCliente,
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.girlfriendSelect = data.oData.Customers;

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

    $scope.AddBoyfriendCustomer = function () {
        var now = new Date();

        $scope.includeURLBoyfriend = "../../Customers/PartialAddPhysicalCustomer?update=" + now;

        $("#openModalAddBoyfriendCustomer").modal("show");

        $('#openModalAddBoyfriendCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadBoyfriend();
        })
    };

    $scope.AddGirlfriendCustomer = function () {
        var now = new Date();

        $scope.includeURLGirlfriend = "../../Customers/PartialAddPhysicalCustomer?update=" + now;

        $("#openModalAddGirlfriendCustomer").modal("show");

        $('#openModalAddGirlfriendCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadGirlfriend();
        })
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

    $scope.setSelectBoyfriend = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.boyfriendSelect, parseInt(a), 'idCliente');

        $scope.boyfriend = $scope.boyfriendSelect[index];

        

    };

    $scope.setSelectGirlfriend = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.girlfriendSelect, parseInt(a), 'idCliente');

        $scope.girlfriend = $scope.girlfriendSelect[index];

        

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

    //Agregar un servicio
    $scope.AddService = function () {

        if ($scope.addServiceForm.$valid) {

            $scope.SaveService();

        }

    };

    $scope.SaveService = function () {

        $scope.items.push({
            idProducto: $scope.newService.descService.idServicio + "-" + Date.now(),
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
            comentarios: $scope.newService.commentsService
        });

        $scope.descService = "";
        $scope.newService.salePriceService = 0;
        $scope.newService.amountService = 0;
        $scope.newService.commentsService = "";

        $scope.CalculateTotalCost();

        $("#modalService").modal("hide");

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

            discount = ($scope.subTotal + $scope.totalCreditNotes) * percentage;

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

    },

    //Fecha de la boda  
    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };

    $scope.toggleMin();   

    $scope.openDateWedding = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedDateWedding = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM/dd/yyyy'];
    $scope.format = $scope.formats[4];

    //Da formato a las fechas JSON
    $scope.getDate = function (date) {

        var res = "";

        if (date != null) {

            if (date.length > 10) {

                res = date.substring(6, 19);

            }

        }

        return res;

    };

    //Crear una Mesa de Regalos
    $scope.LoadVariablesInit = function (seller, actualDateTime) {

        $scope.actualDateTime = actualDateTime;

        //Llenar combos        
        $scope.LoadBoyfriend();
        $scope.LoadUsers(seller);
        $scope.LoadServices();
        $scope.LoadOffices();

        //SelectSeller
        $scope.sellerOne = '';

        //Seleccionar la sucursal
        setTimeout(function () {

            $("#openModal").modal("show");

        }, 0);

    }

    $scope.SetBranchName = function (branchName, IDBranch) {

        $scope.branch = branchName;

        $scope.branchID = IDBranch;

        $("#openModal").modal("hide");

        $scope.GetNumberRem();

    };

    $scope.GetNumberRem = function () {

        $http({
            method: 'POST',
            url: '../../../GiftsTable/GeneratePrevNumberRem',
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

    $scope.SaveGiftsTable = function () {

        var products = new Array();

        var amount = 0;

        angular.forEach($scope.items, function (value, key) {

            amount = amount + parseInt(value.cantidad);

            if (value.servicio == true) {

                products.push({

                    idProducto: null,
                    Descripcion: value.desc,
                    Precio: value.prec,
                    Descuento: parseFloat(value.descuento),
                    Cantidad: value.cantidad,
                    idServicio: value.idProducto.split("-")[0],
                    Comentario: value.comentarios,
                    Tipo: 2

                });

            } else {

                products.push({

                    idProducto: value.idProducto,
                    Descripcion: value.desc,
                    Precio: value.prec,
                    Descuento: parseFloat(value.descuento),
                    Cantidad: value.cantidad,
                    idServicio: null,
                    Comentario: value.comentarios,
                    Tipo: 1

                });

            }

        });

        var notifyCouple = 0;

        if (($scope.notifyBoyfriend == true) && ($scope.notifyGirlfriend == true)) {
            notifyCouple = 3;
        } else if (($scope.notifyBoyfriend == true)) {
            notifyCouple = 2;
        } else if (($scope.notifyGirlfriend == true)) {
            notifyCouple = 1;
        }

        $http({
            method: 'POST',
            url: '../../../GiftsTable/SaveGiftsTable',
            data: {
                idNovio: $scope.boyfriend.idCliente,
                idNovia: $scope.girlfriend.idCliente,
                FechaBoda: $scope.dateWedding,
                LugarBoda: $scope.location,
                Latitud: $scope.latitud,
                Longitud: $scope.longitud,
                HoraBoda: $scope.hourWedding + $scope.minuteWedding,
                idVendedor1: $scope.sellerOne.idUsuario,
                idVendedor2: ($scope.sellerTwo == undefined) ? null : $scope.sellerTwo.idUsuario,
                idSucursal: $scope.branchID,
                idDespachoReferencia: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null,
                CantidadProductos: amount,
                Subtotal: $scope.subTotal,
                IVA: ($scope.checkedIVA == true) ? 1 : 0,
                Descuento: ($scope.checkedDiscount == true) ? parseFloat($scope.discount) : 0,
                Total: $scope.total,
                Comentarios: $scope.comments,
                Notificar: notifyCouple,
                Fecha: $scope.actualDateTime,
                lDetail: products
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.remision = data.oData.sRemision;

               notify(data.oData.Message, $rootScope.success);

               $('#modalPrint').on('hidden.bs.modal', function (e) {

                   window.location = "../../../GiftsTable/Index";

               });

               $("#modalPrint").modal("show");

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

    $scope.Print = function () {

        var URL = '../../../GiftsTable/PrintGiftsTable?remision=' + $scope.remision;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    //Listado Mesa de Regalos

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

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    //Código para el paginado

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listGiftsTable();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listGiftsTable();
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
            $scope.listGiftsTable();
        }

    };

    $scope.listGiftsTable = function () {

        $("#btnGetList").button("loading");

        $http({
            method: 'POST',
            url: '../../../GiftsTable/GetGiftsTable',
            params: {
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                remision: $scope.sRemision,
                costumer: ($scope.sCustomer == "" || $scope.sCustomer == null) ? "" : $scope.sCustomer,
                codigo: ($scope.sCodigo == "" || $scope.sCodigo == null) ? "" : $scope.sCodigo,
                status: ($scope.sStatusCredit == undefined) ? 1 : $scope.sStatusCredit,
                amazonas: $scope.sBranchAma,
                guadalquivir: $scope.sBranchGua,
                textura: $scope.sBranchTex,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            $("#btnGetList").button("reset");

            if (data.success == 1) {

                if (data.oData.GiftsTable.length > 0) {

                    $scope.GiftsTable = data.oData.GiftsTable;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.GiftsTable = data.oData.GiftsTable;
                    $scope.total = data.oData.Count;

                }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close";

            }

        }).
        error(function (data, status, headers, config) {

            $("#btnGetList").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });
    };

    $scope.DetailGiftsTable = function (idGiftsTable) {

        $scope.includeURL = "GetDetailGiftsTable?idGiftsTable=" + idGiftsTable;

        $("#modalDetailGiftsTable").modal("show");

    };

    $scope.ItemsDetailGiftsTable = function (detail, sales) {

        $scope.items = detail;
        $scope.sales = sales;

    };

    $scope.CalculateTotalForDetail = function (subTotal, discount) {

        subTotal = subTotal - (subTotal * (discount / 100));

        subTotal = parseFloat(subTotal);       

        $scope.IVADetail = subTotal * .16;

        if ($scope.IVADetail < 0) {

            $scope.IVADetail = 0;

        }

    };

    $scope.GiftsTableForSale = function (idGiftsTable) {

        var URL = '../../../GiftsTable/GiftsTableForSale?idGiftsTable=' + idGiftsTable;

        window.location = URL;

    };

    $scope.SendMail = function (remision) {

        $scope.remision = remision;

        $("#modalSendMail").modal("show");

    };

    $scope.AcceptSendMail = function () {

        if ($scope.txtSendMail != undefined && $scope.txtSendMail.length > 0) {

            $http({
                method: 'POST',
                url: '../../../GiftsTable/SendGiftsTableAgain',
                data: {
                    remision: $scope.remision,
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

    $scope.ChangeStatus = function (ID, status) {

        $scope.idSaleStatus = ID;
        $scope.statusSale = status;

        $("#openModalStatus").modal("show");

    };

    $scope.SetStatus = function () {

        $http({
            method: 'POST',
            url: '../../../GiftsTable/UpdateStatusGiftsTable',
            params: {
                idSale: $scope.idSaleStatus,
                status: $scope.statusSale
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

    $scope.ChangeStatusSale = function (ID, status) {

        $scope.idSale = ID;
        $scope.statusSale = status;

        $("#openModalStatusSale").modal("show");

    };

    $scope.SetStatusSale = function () {

        $http({
            method: 'POST',
            url: '../../../GiftsTable/UpdateStatusGiftsTableSale',
            params: {
                idSale: $scope.idSale,
                status: $scope.statusSale
            }
        }).
        success(function (data, status, headers, config) {

            $("#openModalStatusSale").modal("hide");

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

    //Modificar una Mesa de Regalos

    //Eliminar una Mesa de Regalos

    //Cerrar una Mesa de Regalos

    $scope.CloseGiftsTable = function () {



    }

    //Imprimir 

    $scope.Print = function (remision) {

        var URL = '../../../GiftsTable/PrintGiftsTable?remision=' + remision;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.PrintSale = function (remision) {

        var URL = '../../../GiftsTable/PrintGiftsTableSale?remision=' + remision;

        var win = window.open(URL, '_blank');
        win.focus();

    }

});
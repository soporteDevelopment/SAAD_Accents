angular.module("General").controller('RepairsController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.branchID = 2;

    //Codigo para el scaner
    angular.element(document).ready(function () {
        angular.element(document).keydown(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);

            if (code == 13)// Enter key hit
            {
                $scope.GetProduct();
                $scope.barcode = "";
            }
            else {
                $scope.barcode = $scope.barcode + String.fromCharCode(code);
            }
        });
    });

    $scope.items = new Array();

    $scope.LoadInit = function (seller) {
        $scope.sellerName = seller;

        $scope.LoadSellers();

        $scope.GetNumberRepair();
    };

    $scope.SetIdBranch = function (branchID) {
        $scope.branchID = branchID;
    };

    $scope.LoadSellers = function () {

        $http({
            method: 'POST',
            url: '../../../Users/ListAllUsers'
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.Users = data.oData.Users;

               if ($scope.sellerName != undefined) {

                   $scope.SetSelecUsersOne($scope.sellerName);
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

    $scope.SetSelecUsersOne = function (a) {

        var index = $scope.ArrayObjectIndexOf($scope.Users, a, 'NombreCompleto');

        $scope.seller = $scope.Users[index];

    };

    $scope.ArrayObjectIndexOf = function (myArray, searchTerm, property) {

        if (myArray != undefined) {

            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }

    };

    $scope.GetNumberRepair = function () {

        $http({
            method: 'POST',
            url: '../../../Repairs/GeneratePrevNumber'
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.number = data.oData.Number;

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

    $scope.GetProduct = function () {

        var res = $scope.barcode.match(/^\d{12}$/);

        if (($scope.barcode.length == 12) && (res.length > 0))// Enter key hit
        {
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
                            idView: "",
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

            $scope.LostFocus();
        }

    };

    $scope.GetProductForCode = function () {

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
                            idView: "",
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

        $scope.LostFocus();

    };

    $scope.ValidateStock = function (ID, branchID) {

        var exist = _.result(_.find($scope.items, function (chr) {

            return chr.idProducto == ID && chr.idSucursal == branchID;

        }), 'existencia');

        exist = (exist == undefined) ? 0 : exist;

        var amount = _.result(_.find($scope.items, function (chr) {

            return chr.idProducto == ID && chr.idSucursal == branchID;

        }), 'cantidad');

        if (exist < amount) {

            var index = _.indexOf($scope.items, _.find($scope.items, function (chr) {
                return chr.idProducto == ID && chr.idSucursal == branchID;
            }));

            if (exist == 0 && index > -1) {
                $scope.items.splice(index, 1);
            } else {
                $scope.items[index].cantidad = exist;
            }

            $scope.GetStockProduct(ID);

        }

    };

    $scope.GetStockProduct = function (idProduct) {

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

    $scope.DeleteProduct = function (ID, idBranch) {

        _.remove($scope.items, function (n) {

            return n.idProducto == ID && n.idSucursal == idBranch;

        });

    };

    $scope.SaveRepair = function () {
        var lDetail = new Array();

        var amount = 0;

        angular.forEach($scope.items, function (value, key) {

            amount = amount + parseInt(value.cantidad);

            lDetail.push({
                idProducto: value.idProducto,
                idSucursal: value.idSucursal,
                Cantidad: value.cantidad,
                Pendiente: value.cantidad,
                Comentarios: value.comentarios
            });

        });

        var repair = {
            Numero: $scope.number,
            idUsuario: $scope.seller.idUsuario,
            FechaSalida: $scope.dateTimeOut,
            FechaRegreso: $scope.dateTimeIn,
            Responsable: $scope.responsible,
            Destino: $scope.destiny,
            Comentarios: $scope.comments,
            Estatus: 2,
            CreadoPor: $scope.seller.idUsuario,
            Creado: $scope.dateTimeOut,
            lDetalle: lDetail
        }

        $http({
            method: 'POST',
            url: '../../../Repairs/SaveAddRepair',
            data: {
                'repair': repair
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                window.location = "../../../Repairs/Index";

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
        $scope.branchItem = idBranch;
        $scope.commentsItem = comments;

        $("#modalComments").modal("show");

    };

    $scope.SaveComments = function () {
        var index = _.findIndex($scope.items, function (o) {
            return o.idProducto == $scope.productItem && o.idSucursal == $scope.branchItem
        });

        if (index > -1) {
            $scope.items[index].comentarios = $scope.commentsItem;
        }

        $("#modalComments").modal("hide");
    };

    $scope.LostFocus = function () {

        $('input').each(function () {
            $(this).trigger('blur');
        });

    };

    $scope.dateToday = new Date();

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };

    $scope.toggleMin();

    $scope.openBack = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedBack = true;
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

});

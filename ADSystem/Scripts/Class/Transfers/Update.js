angular.module("General").controller('TransfersController', function ($scope, $http, $window, notify, $rootScope) {

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

    $scope.items = new Array();

    $scope.LoadInit = function (transfer) {

        $scope.idTransfer = transfer.idTransferencia;
        $scope.sellerName = transfer.Usuario;

        $scope.LoadSellers();

        $scope.number = transfer.Numero;
        $scope.branch = transfer.SucursalOrigen;
        $scope.branchID = transfer.idSucursalOrigen;
        $scope.branchDestiny = transfer.idSucursalDestino;
        $scope.comments = transfer.Comentarios;

        angular.forEach(transfer.lDetail, function (value, key) {

            $scope.GetProductInit(value.idProducto, (value.Cantidad - value.CantidadEnviada));

        });

    };
    
    $scope.SetBranchName = function (branchName, IDBranch) {

        $scope.branch = branchName;

        $scope.branchID = IDBranch;

        $("#openModal").modal("hide");

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

    $scope.GetProductInit = function (idProduct, amount) {

        $http({
            method: 'POST',
            url: '../../../Products/GetProduct',
            params: {
                idProduct: idProduct
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
                        cantidad: amount,
                        costo: 0,
                        servicio: false,
                        credito: false,
                        comentarios: ""
                    });

                } else {

                    var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));

                    $scope.items[index].cantidad = $scope.items[index].cantidad + amount;

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

            $scope.selectedCode = "";

        });

    };

    $scope.GetProduct = function () {

        var res = $scope.barcode.match(/^\d{12}$/);

        if (($scope.barcode.length == 12) && (res.length > 0))// Enter key hit
        {
            $http({
                method: 'POST',
                url: '../../../Products/GetProductForId',
                params: {
                    idProduct: (res != null && res != "") ? res : ""
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

            $scope.selectedCode = "";

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

            $scope.selectedCode = "";

        });

        $("#product_value").val("");

        $scope.LostFocus();

    };

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

    $scope.DeleteProduct = function (ID) {

        _.remove($scope.items, function (n) {

            return n.idProducto == ID;

        });

    };

    $scope.SaveTransfer = function () {
        var invalid = false;

        if ($scope.branchDestiny != null && $scope.branchDestiny != undefined && $scope.branchDestiny != ""
            && ($scope.branchID != $scope.branchDestiny) && $scope.items.length > 0) {

                var lDetail = new Array();

                var amount = 0;

                angular.forEach($scope.items,
                    function(value, key) {

                        amount = amount + parseInt(value.cantidad);

                        lDetail.push({
                            idProducto: value.idProducto,
                            Cantidad: value.cantidad,
                            Costo: value.prec,
                            Comentarios: value.comentarios,
                        });

                    });

                transfer = {
                    idTransferencia: $scope.idTransfer,
                    Numero: $scope.number,
                    idUsuario: $scope.seller.idUsuario,
                    idSucursalOrigen: $scope.branchID,
                    idSucursalDestino: $scope.branchDestiny,
                    Fecha: $scope.dateTime,
                    CantidadProductos: amount,
                    Comentarios: $scope.comments
                }

                $http({
                    method: 'POST',
                    url: '../../../Transfers/SaveUpdate',
                    data: {
                        'transfer': transfer,
                        'lDetail': lDetail
                    }
                }).success(function(data, status, headers, config) {

                    if (data.success == 1) {

                        notify(data.oData.Message, $rootScope.success);

                        window.location = "../../../Transfers/Index";

                    } else if (data.failure == 1) {

                        notify(data.oData.Error, $rootScope.error);

                    } else if (data.noLogin == 1) {

                        window.location = "../../../Access/Close";

                    }

                }).error(function(data, status, headers, config) {

                    notify("Ocurrío un error.", $rootScope.error);

                });

        } else {
            notify("Revise la información de la transferencia", $rootScope.error);
        }

    };

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

    $scope.LostFocus = function () {

        $('input').each(function () {
            $(this).trigger('blur');
        });

    };

});

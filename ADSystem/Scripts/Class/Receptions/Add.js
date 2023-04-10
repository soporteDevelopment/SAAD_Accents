angular.module("General").controller('ReceptionsController', function ($scope, $http, $window, notify, $rootScope) {


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

    $scope.LostFocus = function () {
        $('input').each(function () {
            $(this).trigger('blur');
        });
    };

    $scope.items = new Array();

    $scope.LoadInit = function (seller) {

        $scope.sellerName = seller;

        $scope.LoadSellers();

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

    $scope.GetNumberReception = function () {

        $http({
            method: 'POST',
            url: '../../../Receptions/GeneratePrevNumber',
            params: {
                idBranch: $scope.branchID
            }
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

    $scope.GetTransfer = function () {

        if ($scope.transfernumber != undefined && $scope.transfernumber != "") {

            $scope.items = [];

            $http({
                method: 'POST',
                url: '../../../Transfers/GetByNumber',
                params: {
                    number: $scope.transfernumber
                }
            }).success(function(data, status, headers, config) {

                if (data.success == 1) {

                    if (data.oData.Transfer != undefined) {

                        $scope.transfer = data.oData.Transfer;
                        $scope.branch = data.oData.Transfer.SucursalDestino;

                        data.oData.Transfer.lDetail.forEach(function(product) {

                            $scope.items.push({
                                idProducto: product.idProducto,
                                imagen: product.Imagen,
                                codigo: product.Codigo,
                                desc: product.Descripcion,
                                prec: product.Costo,
                                enviado: product.Cantidad,
                                recibido: 0
                            });

                        });

                        $scope.branchID = $scope.transfer.idSucursalDestino;

                        $scope.GetNumberReception();

                        $scope.LostFocus();

                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

                $scope.selectedCode = "";

            }).error(function(data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $scope.selectedCode = "";

            });

            $("#product_value").val("");

        } else {
            alert("Ingrese el número de transferencia.");
        }

    };

    $scope.ValidateReception = function () {
        
        var keepGoing = true;

        angular.forEach($scope.items, function (value, key) {
            //Valida cantidades mayores a las enviadas
            if (keepGoing) {
                if (value.recibido > value.enviado || value.recibido < 0) {
                    keepGoing = false;
                }
            }

        });

        if (keepGoing) {
            //Valida diferencias negativas
            angular.forEach($scope.items, function (value, key) {

                if (keepGoing) {
                    if (value.recibido < value.enviado) {
                        keepGoing = false;
                    }
                }

            });

            if (keepGoing) {
                $scope.SaveReception();
            } else {
                var r = confirm("Existen diferencias, ¿desea guardar la recepción?");
                if (r == true) {
                    $scope.SaveReception();
                }
            }
        } else {
            alert("Revise las cantidades recibidas.");
        }
    };

    $scope.SaveReception = function () {

        var lDetail = new Array();
        var lHistoryTransfer = new Array();
        var lHistoryReception = new Array();

        var amount = 0;

        angular.forEach($scope.items,
                    function (value, key) {
                        amount = amount + parseInt(value.recibido);

                        lDetail.push({
                            idProducto: value.idProducto,
                            Cantidad: value.enviado,
                            CantidadRecibida: value.recibido,
                            Costo: value.prec,
                            Comentarios: value.comentarios
                        });

                        lHistoryTransfer.push([
                            value.idProducto,
                            parseInt(value.recibido)
                        ]);

                        lHistoryReception.push([
                            value.idProducto,
                            parseInt(value.recibido)
                        ]);
                    });

        reception = {
            Numero: $scope.number,
            idTransferencia: $scope.transfer.idTransferencia,
            idUsuario: $scope.seller.idUsuario,
            idSucursal: $scope.transfer.idSucursalDestino,
            Fecha: $scope.dateTime,
            CantidadProductos: amount,
            Comentarios: $scope.comments
        }

        $("#saveReception").button("loading");
        $http({
            method: 'POST',
            url: '../../../Receptions/Add',
            data: {
                'reception': reception,
                'lDetail': lDetail,
                'aHistoryReception': lHistoryReception,
                'aHistoryTransfer': lHistoryTransfer
            }
        }).success(function (data, status, headers, config) {
            $("#saveReception").button("reset");
            if (data.success == 1) {
                notify(data.oData.Message, $rootScope.success);
                window.location = "../../../Receptions/Index";
            } else if (data.failure == 1) {
                notify(data.oData.Error, $rootScope.error);
            } else if (data.noLogin == 1) {
                window.location = "../../../Access/Close";
            }
        }).error(function (data, status, headers, config) {
            $("#saveReception").button("reset");
            notify("Ocurrío un error.", $rootScope.error);
        });

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

    $scope.GetProductForId = function () {

        var res = $scope.barcode.match(/^\d{12}$/);

        if (($scope.barcode.length == 12) && (res.length > 0))// Enter key hit
        {
            res = res[0].substr(0, res[0].length - 1);
            res = parseInt(res);

            var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: res }));

            if (index >= 0) {
                if ($("#rec" + $scope.items[index].idProducto).val() == "") {
                    $("#rec" + $scope.items[index].idProducto).val(0);
                }

                if ($scope.items[index].enviado > $scope.items[index].recibido) {
                    var value = parseInt($("#rec" + $scope.items[index].idProducto).val());
                    $("#rec" + $scope.items[index].idProducto).val(value + 1);

                    $scope.items[index].recibido = parseInt($scope.items[index].recibido) + 1;
                }
            }

            $scope.LostFocus();
        }

    };

});

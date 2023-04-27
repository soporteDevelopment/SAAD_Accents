angular.module("General").controller('QuotationsPrint', function (models, $scope, $http, $window, notify, $rootScope) {

    $scope.cantidadProductos = 0;
    $scope.sumaSubTotal = 0;
    $scope.discount = 0;
    $scope.ivaTotal = 0;
    $scope.total = 0;

    $scope.init = function (model) {        
        $scope.items = new Array();

        angular.forEach(model.lCotizaciones, function (quotation, quotationKey) {            
            angular.forEach(quotation.oDetail, function (value, key) {

                var result = undefined;

                if ($scope.items.length > 0) {
                    result = _.result(_.find($scope.items, function (chr) {
                        return chr.idProducto == value.idProducto
                    }), 'idProducto');
                }

                if (result == undefined) {

                    if (value.idProducto == null && value.idServicio > 0) {
                        $scope.items.push({
                            idProducto: value.idServicio,
                            Imagen: value.Imagen,
                            Codigo: value.Descripcion,
                            Descripcion: value.Descripcion,
                            Precio: value.Precio,
                            Descuento: value.Descuento,
                            Cantidad: value.Cantidad,
                            Comentarios: value.Comentarios
                        });

                    } else {

                        $scope.items.push({
                            idProducto: value.idProducto,
                            Imagen: value.Imagen,
                            Codigo: value.Codigo,
                            Descripcion: value.Descripcion,
                            Precio: value.Precio,
                            Descuento: value.Descuento,
                            Cantidad: value.Cantidad,
                            Comentarios: value.Comentarios
                        });

                    }

                }else{
                    var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: value.idProducto }));
                    $scope.items[index].Cantidad = $scope.items[index].Cantidad + value.Cantidad;
                }

            });
        });

        angular.forEach($scope.items, function (value, key) {
            $scope.cantidadProductos = $scope.cantidadProductos + value.Cantidad;
            $scope.sumaSubTotal = $scope.sumaSubTotal + ((value.Cantidad * value.Precio) - ((value.Cantidad * value.Precio) * (value.Descuento / 100)));
        });

        if (model.Descuento > 0) {
            $scope.discount = $scope.sumaSubTotal * (model.Descuento / 100);
        }

        if (model.IVA == 1) {                        
            $scope.ivaTotal = ($scope.sumaSubTotal - $scope.discount) * (model.IVATasa / 100);            
            $scope.total = ($scope.sumaSubTotal - $scope.discount) + $scope.ivaTotal;
        } else {
            $scope.ivaTotal = 0;
            $scope.total = $scope.sumaSubTotal - $scope.discount;
        }

        setTimeout(function () {
            window.print();
        }, 5000);
    },
    $scope.getDate = function (date) {
        var res = "";

        if (date.length > 10) {
            res = date.substring(6, 19);
        }

        return res;
    }
});

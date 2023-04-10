angular.module("General").controller('SalesPrint', function (models, $scope, $http, $window, notify, $rootScope) {

    $scope.init = function (detail) {
        $scope.items = new Array();

        angular.forEach(detail, function (value, key) {
            if (value.idProducto == null && value.idServicio > 0) {
                $scope.items.push({
                    idProducto: value.idServicio,
                    Imagen: "../Content/Services/" + value.Imagen,
                    Codigo: value.Descripcion,
                    Descripcion: value.Descripcion,
                    Precio: value.Precio,
                    Descuento: value.Descuento,
                    Cantidad: value.Cantidad,
                    idPromocion: value.idPromocion,
                    CostoPromocion: value.CostoPromocion,
                    Comentarios: value.Comentarios
                });
            } else if (value.idProducto == null) {
                $scope.items.push({
                    idProducto: value.idProducto,
                    Imagen: "",
                    Codigo: value.Descripcion,
                    Descripcion: value.Descripcion,
                    Precio: value.Precio,
                    Descuento: value.Descuento,
                    Cantidad: value.Cantidad,
                    idPromocion: value.idPromocion,
                    CostoPromocion: value.CostoPromocion,
                    Comentarios: value.Comentarios
                });
            } else {
                $scope.items.push({
                    idProducto: value.idProducto,
                    Imagen: value.oProducto.urlImagen,
                    Codigo: value.Codigo,
                    Descripcion: value.Descripcion,
                    Precio: value.Precio,
                    Descuento: value.Descuento,
                    Cantidad: value.Cantidad,
                    idPromocion: value.idPromocion,
                    CostoPromocion: value.CostoPromocion,
                    Comentarios: value.Comentarios
                });
            }
        });
    },

    $scope.GetTypesPaymentForPrint = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/GetTypesPaymentForPrint',
            params: {
                idSale: idSale,
            }
        }).
            then(function (response) {
                if (response.data.success == 1) {
                    var suma = 0;

                    if (response.data.oData.TypesPayment.length > 0) {
                        $scope.TypesPayment = response.data.oData.TypesPayment;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);
                    }

                    setTimeout(function () {
                        window.print();
                    }, 3000);

                } else if (response.data.failure == 1) {
                    notify(response.data.oData.Error, $rootScope.error);
                } else if (response.data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            });
    };

    $scope.getDate = function (date) {
        var res = "";

        if (date.length > 10) {
            res = date.substring(6, 19);
        }

        return res;
    };
});

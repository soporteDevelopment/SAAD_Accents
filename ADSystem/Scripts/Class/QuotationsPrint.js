angular.module("General").controller('QuotationsPrint', function (models, $scope, $http, $window, notify, $rootScope) {

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
                    Comentarios: value.Comentarios
                });

            }

        });

        setTimeout(function () {

            window.print();

        }, 3000);

    },

    $scope.getDate = function (date) {

        var res = "";

        if (date.length > 10) {

            res = date.substring(6, 19);

        }

        return res;

    }

});

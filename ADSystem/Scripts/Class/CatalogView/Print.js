angular.module("General").controller('PrintController', function (models, $scope, $http, $window, notify, $rootScope) {
    $scope.Init = function (detail) {
        $scope.items = [];
        angular.forEach(detail, function (value, key) {
            $scope.items.push({
                Imagen: value.Catalogo.Imagen,
                Codigo: value.Catalogo.Codigo,
                Modelo: value.Catalogo.Modelo,
                Volumen: value.Catalogo.Volumen,
                Cantidad: value.Cantidad,
                Precio: parseFloat(value.Precio)
            });
        });
    };
});

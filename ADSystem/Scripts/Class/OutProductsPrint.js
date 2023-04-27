angular.module("General").controller('OutProductsPrint', function (models, $scope, $http, $window, notify, $rootScope) {

    $scope.init = function (detail, services) {
        $scope.items = detail;
        
        angular.forEach(services, function (value, key) {

            $scope.items.push({
                urlImagen: value.urlImagen,
                Codigo: value.Descripcion,
                Descripcion: value.Descripcion,
                Cantidad: value.Cantidad,
                Precio: parseFloat(value.Precio)
            });

        });
    }

    $scope.initPending = function (detail, services) {

        $scope.items = detail;

        angular.forEach(services, function (value, key) {

            $scope.items.push({
                urlImagen: value.urlImagen,
                Codigo: value.Descripcion,
                Descripcion: value.Descripcion,
                Cantidad: value.Cantidad,
                Precio: parseFloat(value.Precio)
            });

        });

        $scope.CantidadProductos = _.sumBy($scope.items, function (item) { return item.Cantidad; });

        $scope.Subtotal = _.sumBy($scope.items, function (item) { return (item.Precio * item.Cantidad); });

        $scope.Total = _.sumBy($scope.items, function (item) { return (item.Precio * item.Cantidad); });

    }
    
});

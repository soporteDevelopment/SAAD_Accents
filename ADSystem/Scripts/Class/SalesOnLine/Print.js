angular.module("General").controller('SalesOnLine', function (models, $scope, $http, $window, notify, $rootScope) {
        
    $scope.init = function (sale) {
        $scope.name = sale.Cliente.Nombre + " " + sale.Cliente.Apellidos;
        $scope.address = sale.Cliente.Calle + " " + sale.Cliente.NumInt + " " + sale.Cliente.NumExt + " " + sale.Cliente.Colonia + " " + sale.Cliente.Estado + " " + sale.Cliente.Cp;
        $scope.email = sale.Cliente.Correo;
        $scope.phone = sale.Cliente.Telefono;

        $scope.items = sale.Productos;

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

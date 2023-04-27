angular.module("General").controller('OutUnifyProductsPrint', function (models, $scope, $http, $window, notify, $rootScope) {

    $scope.Total = 0;
    $scope.CantidadTotalProductos = 0;
    $scope.CantidadProductos = 0;

    $scope.init = function (model) {

        $scope.views = model;
    }
    
    $scope.initPending = function (view) {
        
        view.CantidadProductos = _.sumBy(view.oDetail, function (item) { return item.Cantidad; });
        
        $scope.CantidadTotalProductos = $scope.CantidadTotalProductos + _.sumBy(view.oDetail, function (item) { return item.Cantidad; });

        $scope.Subtotal = _.sumBy($scope.items, function (item) { return (item.Precio * item.Cantidad); });
        $scope.Total = $scope.Total + view.Restante;
    }
    
});

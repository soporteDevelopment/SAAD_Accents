angular.module("General").controller('UpdateController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.type = 0;
    $scope.idRepair = null;

    $scope.Init = function (repair) {
        $scope.idRepair = repair.idReparacion;
        $scope.destiny = repair.idReparacion,
        $scope.number = repair.Numero;
        $scope.dateTimeOut = repair.FechaSalida;
        $scope.destiny = repair.Destino;
        $scope.responsible = repair.Responsable;
        $scope.dateTimeIn = repair.FechaRegreso;
        $scope.status = repair.Estatus;
        $scope.comments = repair.Comentarios;

        $scope.items = new Array();

        angular.forEach(repair.lDetalle, function (value, key) {

            $scope.items.push({
                idDetalleReparacion: value.idDetalleReparacion,
                idProducto: value.idProducto,
                Producto: value.Producto,
                idSucursal: null,
                Cantidad: value.Cantidad,
                Pendiente: value.Pendiente,
                Ingreso: 0
            });

        });
    };

    $scope.SaveUpdate = function () {
        $("#SaveUpdate").button("loading");
        
        var repair = {
            idReparacion: $scope.idRepair,
            FechaSalida: $scope.dateTimeOut,
            FechaRegreso: $scope.dateTimeIn,
            Responsable: $scope.responsible,
            Destino: $scope.destiny,
            Comentarios: $scope.comments
        }

        $http({
                method: 'PATCH',
                url: '../../../Repairs/SaveUpdateRepair',
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
});

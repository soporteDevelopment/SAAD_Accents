angular.module("General").controller('DetailController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.type = 0;
    $scope.idRepair = null;

    $scope.Init = function (idRepair, detail) {

        $scope.idRepair = idRepair;

        $scope.items = new Array();

        angular.forEach(detail, function (value, key) {

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
        $("#SaveUpdateStock").button("loading");

        var lhistory = new Array();

        angular.forEach($scope.items, function (value, key) {
            if (value.Ingreso > 0) {
                lhistory.push({
                    idDetalleReparacion: value.idDetalleReparacion,
                    Cantidad: value.Ingreso,
                    idSucursal: value.idSucursal
                });
            }
        });

        $http({
            method: 'POST',
            url: '../../../Repairs/SaveHistoryRepair',
            data: {
                idRepair: $scope.idRepair,
                lhistory: lhistory
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    window.location = "../../../Repairs/Index";

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

                $("#SaveUpdateStock").button("reset");

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $("#SaveUpdateStock").button("reset");

            });
    }
});

angular.module("General").controller('CommissionsController', function (GUID, $scope, $http, $window, notify, $rootScope) {

    $scope.Commissions = new Array();
    $scope.idUser = 0;

    $scope.Init = function (idUser) {
        $scope.idUser = idUser;
        $scope.GetCommissions();
    };

    $scope.GetCommissions = function () {
        $http({
            method: 'GET',
            url: '../../../Commissions/Get',
            params: {
                idUser: $scope.idUser
            }
        }).
        success(function (data, status, headers, config) {
            if (data.success == 1) {
                $scope.Commissions = [];
                if (data.oData.Commissions.length > 0) {
                    angular.forEach(data.oData.Commissions, function (value, key) {
                        $scope.Commissions.push({
                            idComision: value.idComision,
                            idUsuario: value.idUsuario,
                            LimiteInferior: value.LimiteInferior,
                            LimiteSuperior: value.LimiteSuperior,
                            SueldoMensual: value.SueldoMensual,
                            PorcentajeComision: value.PorcentajeComision,
                            SueldoComision: value.SueldoComision,
                            BonoUno: value.BonoUno,
                            BonoDos: value.BonoDos,
                            BonoTres: value.BonoTres
                        });
                    });
                } else {
                    notify('No se encontraron registros.', $rootScope.error);
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

    $scope.Add = function () {
        $http({
            method: 'POST',
            url: '../../../Commissions/Add',
            params: {
                idUsuario: $scope.idUser,
                LimiteInferior: $scope.LimiteInferior,
                LimiteSuperior: $scope.LimiteSuperior,
                SueldoMensual: $scope.SueldoMensual,
                PorcentajeComision: $scope.PorcentajeComision,
                SueldoComision: 0,
                BonoUno: $scope.BonoUno,
                BonoDos: $scope.BonoDos,
                BonoTres: $scope.BonoTres
            }
        }).
        success(function (data, status, headers, config) {
            if (data.success == 1) {
                notify(data.oData.Message, $rootScope.success);
                $scope.CleanCommission();
                $scope.GetCommissions();
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

    $scope.Update = function (commission) {
        $http({
            method: 'PATCH',
            url: '../../../Commissions/Update',
            params: {
                idComision: commission.idComision,
                idUsuario: $scope.idUser,
                LimiteInferior: commission.LimiteInferior,
                LimiteSuperior: commission.LimiteSuperior,
                SueldoMensual: commission.SueldoMensual,
                PorcentajeComision: commission.PorcentajeComision,
                SueldoComision: 0,
                BonoUno: commission.BonoUno,
                BonoDos: commission.BonoDos,
                BonoTres: commission.BonoTres
            }
        }).
        success(function (data, status, headers, config) {
            if (data.success == 1) {
                $scope.CleanCommission();
                $scope.GetCommissions();
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

    $scope.Remove = function (idCommission) {
        $http({
            method: 'DELETE',
            url: '../../../Commissions/Delete',
            params: {
                idCommission: idCommission
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.CleanCommission();
                    $scope.GetCommissions();
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

    $scope.CleanCommission = function () {
        $scope.LimiteInferior = 0;
        $scope.LimiteSuperior = 0;
        $scope.SueldoMensual = 0;
        $scope.PorcentajeComision = 0;
        $scope.BonoUno = 0;
        $scope.BonoDos = 0;
        $scope.BonoTres = 0;
    }
});
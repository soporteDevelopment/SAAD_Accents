angular.module("General").controller('GiftsTableSalePrint', function ($scope, $http, $window, notify, $rootScope) {

    $scope.init = function (detail) {

        $scope.items = detail;

        setTimeout(function () { 

            window.print();

        }, 3000);

    },

    $scope.getTypesPaymentForSale = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../GiftsTable/GetTypesPaymentForSale',
            params: {
                idSale: idSale,
            }
        }).
        then(function (response) {

            if (response.data.success == 1) {

                var suma = 0;

                if (response.data.oData.TypesPayment.length > 0) {

                    angular.forEach(response.data.oData.TypesPayment, function (value, key) {

                        if (value.HistoryCredit.length > 0) {

                            for (var i = 0; i <= value.HistoryCredit.length - 1; i++) {

                                suma = suma - value.HistoryCredit[i].Cantidad;

                            }

                            value.amount = value.amount + suma;

                        }

                    });

                    $scope.TypesPayment = response.data.oData.TypesPayment;

                    setTimeout(function () {

                        window.print();

                    }, 3000);

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                }

            } else if (response.data.failure == 1) {

                notify(response.data.oData.Error, $rootScope.error);

            } else if (response.data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        })
        .catch(function (response) {

        })

    };

    $scope.getDate = function (date) {

        var res = "";

        if (date.length > 10) {

            res = date.substring(6, 19);

        }

        return res;

    }

});

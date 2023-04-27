angular.module("General").controller('SalesDepositPrint', function (models, $scope, $http, $window, notify, $rootScope) {

    $scope.deposit = 0;
    $scope.remaining = 0;

    $scope.init = function (detail) {

        $scope.items = detail;

    },

    $scope.getTypesPaymentForSale = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/GetTypesPaymentForSale',
            params: {
                idSale: idSale,
            }
        }).
        then(function (response) {

            if (response.data.success == 1) {

                if (response.data.oData.TypesPayment.length > 0) {

                    $scope.TypesPayment = response.data.oData.TypesPayment;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                }
                
            } else if (response.data.failure == 1) {

                notify(response.data.oData.Error, $rootScope.error);

            } else if (response.data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        })
        .catch(function(response) {
            
        })
        .finally(function () {

            window.print();

        });        

    };

    $scope.getDate = function(date){

        var res = "";

        if (date.length > 10) {

            res = date.substring(6, 19);


        }

        return res;

    }
    
});

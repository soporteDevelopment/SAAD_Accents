angular.module("General").controller('PrintTransfer', function (models, $scope, $http, $window, notify, $rootScope) {

    $scope.init = function (detail) {

        $scope.items = detail;

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

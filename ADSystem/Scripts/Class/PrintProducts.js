angular.module("General").controller('PrintProducts', function (models, $scope, $http, $window, $rootScope) {

    $scope.init = function (products) {

        $scope.items = products;

        setTimeout(function () {

            window.print();

        }, 3000);

    }

});

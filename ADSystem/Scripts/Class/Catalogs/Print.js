angular.module("General").controller('CatalogsController', function ($scope, $http, $window, $rootScope) {
    $scope.init = function (products) {
        $scope.catalogs = catalogs;

        setTimeout(function () {
            window.print();
        }, 3000);
    }
});

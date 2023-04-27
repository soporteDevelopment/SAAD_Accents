angular.module("General").controller('PrintController', function ($scope, $http, $window, notify, $rootScope) {
    $scope.loadInit = function (report) {
        $scope.report = report;

        setTimeout(function () {
            window.print();
        }, 3000);
    }
});

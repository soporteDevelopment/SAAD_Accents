angular.module("General").controller('ReportsController', function ($scope, $http, $window, notify, $rootScope) {
    $scope.loadInit = function (report) {
        $scope.report = report;
    }
});

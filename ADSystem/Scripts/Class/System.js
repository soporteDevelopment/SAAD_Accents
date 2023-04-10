angular.module("General").controller('SystemController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.SaveChange = function () {

        $("#SaveChange").button("loading");

        var values = { "Host": $scope.Host, "Name": $scope.Name, "Password": $scope.Password, "Port": $scope.Port, "TimeOut": $scope.TimeOut };

        $http({
            method: 'POST',
            url: '../../../System/SaveChange',
            params: {
                Host: $scope.Host,
                Name: $scope.Name,
                Password: $scope.Password,
                Port: $scope.Port,
                TimeOut: $scope.TimeOut
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveChange").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveChange").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });
    };
    
});
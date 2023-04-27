angular.module("General").controller('EventsController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.openedDateEvent = false;
    $scope.openedRememberDate = false;
    $scope.Color = "#616161";
    $scope.Type = 1;

    $scope.LoadInit = function () {

        $scope.LoadUsers();

    };

    $scope.LoadUsers = function () {

        $http({
            method: 'GET',
            url: '../../../Users/ListAllUsersButNot'
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.Users = data.oData.Users;

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

    $scope.AddEvent = function () {

        if ($scope.frmEvent.$valid) {

            var users = [];

            angular.forEach($scope.Users, function (value, key) {
                users.push(value.idUsuario);
            });

            $.ajax({
                url: "/Events/AddEvent",
                method: 'POST',
                data: {
                    "Nombre": $scope.EventName,
                    "Fecha": $("#EventDate").val(),
                    "TodoDia": $scope.AllDay,
                    "HoraInicio": $scope.EventTimeStart,
                    "HoraFin": $scope.EventTimeEnd,
                    "Color": ($scope.Type == 1) ? $scope.EventColor : "#D50000",
                    "Fondo": ($scope.Type == 1) ? $scope.EventColor : "#D50000",
                    "Fuente": "#FFFFFF",
                    "Lugar": $scope.EventZone,
                    "FechaRecordatorio": $("#RememberDate").val(),
                    "HoraRecordatorio": $scope.EventTimeRemember,
                    "Tipo": $scope.Type,
                    "users": users
                },
                success: function (response) {
                    if (response.success == 1) {
                        window.location = "/Home/Index";
                    } else if (response.failure == 1) {
                        notify(response.oData.Error, $rootScope.error);
                    } else if (response.noLogin == 1) {
                        window.location = "/Access/Close";
                    }
                },
                error: function () {
                    alert('Ocurrío un error');
                }
            });
        }

    };

    $scope.Back = function () {
        window.location = "/Home/Index";
    };

    $scope.dateToday = new Date();

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };

    $scope.toggleMin();

    $scope.openDateEvent = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedDateEvent = true;
    };

    $scope.openRememberDate = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedRememberDate = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM-dd-yyyy'];
    $scope.format = $scope.formats[4];

    $scope.FormatDate = function (value) {
        if (!value) return ''
        var date = new Date(parseInt(value.substr(6)));

        return date.getFullYear() + "-" + (((parseInt(date.getMonth() + 1)) < 10) ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1)) + "-" + (((parseInt(date.getDate())) < 10) ? "0" + (date.getDate()) : (date.getDate()));
    };

    $scope.getDate = function (date) {
        var res = "";
        if (date != null) {
            if (date.length > 10) {
                res = date.substring(6, 19);
            }
        }
        return res;
    };

    $scope.Colors = [
        {
            Color: "#33B679",
            Lugar: "Amazonas"
        },
        {
            Color: "#FD67D4",
            Lugar: "Calzada"
        },
        {
            Color: "#FFF401",
            Lugar: "Textura"
        },
        {
            Color: "#D50000",
            Lugar: "Otro"
        },
    ];

});


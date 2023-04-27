angular.module("General").controller('ActionsController', function (models, actionValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
        }
    };

    $scope.nextPageDisabled = function () {
        return $scope.currentPage === $scope.pageCount() - 1 ? "disabled" : "";
    };

    $scope.pageCount = function () {
        return Math.ceil($scope.total / $scope.itemsPerPage);
    };

    $scope.range = function () {

        var rangeSize = 5;
        var ret = [];
        var start;
        var result = $scope.pageCount();

        if ($scope.pageCount() == 0) {

            return ret;

        }

        start = $scope.currentPage;
        if (start > $scope.pageCount() - rangeSize) {

            start = $scope.pageCount() - rangeSize;

        }

        for (var i = start; i < start + rangeSize; i++) {

            if (i >= 0) {
                ret.push(i);
            }

        }

        return ret;

    };

    $scope.setPage = function (n) {

        var i = $scope.pageCount();

        if (n >= 0 && n < $scope.pageCount()) {

            $scope.currentPage = n;

        }
    };

    $scope.$watch("currentPage", function (newValue, oldValue) {

        $scope.listActions();

    });

    $scope.listActions = function () {

        $("#searchActions").button('loading');

        $http({
            method: 'POST',
            url: '../../../Actions/ListActions',
            params: {
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage,
                module: ($scope.selectedController != "" && $scope.selectedController != null) ? $scope.selectedController.title : "",
            }
        }).
        success(function (data, status, headers, config) {

            $("#searchActions").button('reset');

            if (data.success == 1) {

                if (data.oData.Actions.length > 0) {

                    $scope.Actions = data.oData.Actions;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Actions = data.oData.Actions;
                    $scope.total = data.oData.Count;

                }


            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#searchActions").button('reset');

            notify("Ocurrío un error.", $rootScope.error);

        });
    };

    $scope.AddAction = function () {

        $window.location = '../../Actions/AddAction';

    };

    $scope.GetControllers = function () {

        $http({
            method: 'POST',
            url: '../../../Actions/GetControllers'
        }).
       success(function (data, status, headers, config) {

           $scope.Controllers = data.oData.Controllers;

           $scope.setSelecControl($scope.selectedControlId);

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    $scope.valResult = {};

    $scope.newAction = new models.action();

    $scope.SaveAddAction = function () {

        $scope.valResult = actionValidator.validate($scope.newAction);

        if ($scope.newAction.$isValid) {

            $scope.SaveAdd();

        }

    },

    $scope.SaveAdd = function () {

        var parentmenu = false;
        var childmenu = false;
        var idControl = 0;

        if ($scope.newAction.parentmenuBool == "true") {

            parentmenu = true;

        } else {

            childmenu = true;

        }

        $("#SaveAddAction").button('loading');

        $http({
            method: 'POST',
            url: '../../../Actions/SaveAddAction',
            params: {
                accion: $scope.newAction.name,
                descripcion: $scope.newAction.description,
                menuPadre: parentmenu,
                menuHijo: childmenu,
                ajax: $scope.newAction.ajax,
                idControl: $scope.newAction.parentmenu.idControl
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveAddAction").button('reset');

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $scope.listActions();

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveAddAction").button('reset');

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.UpdateAction = function (ID) {

        $window.location = '../../Actions/UpdateAction?idAction=' + ID;

    };

    $scope.SaveUpdateAction = function (ID) {

        $scope.valResult = actionValidator.validate($scope.newAction);
        if ($scope.newAction.$isValid) {

            $scope.SaveUpdate(ID);

        }

    };

    $scope.SaveUpdate = function (ID) {

        var parentmenu = false;
        var childmenu = false;
        var idControl = 0;

        if ($scope.newAction.parentmenuBool == "true") {

            parentmenu = true;

        } else {

            childmenu = true;

        }

        $("#SaveUpdateAction").button('loading');

        $http({
            method: 'POST',
            url: '../../../Actions/SaveUpdateAction',
            params: {
                idAction: ID,
                accion: $scope.newAction.name,
                descripcion: $scope.newAction.description,
                menuPadre: parentmenu,
                menuHijo: childmenu,
                ajax: $scope.newAction.ajax,
                idControl: $scope.newAction.parentmenu.idControl
            }
        }).
       success(function (data, status, headers, config) {

           $("#SaveUpdateAction").button('reset');

           if (data.success == 1) {

               notify(data.oData.Message, $rootScope.success);

               $scope.listActions();

           } else if (data.failure == 1) {

               notify(data.oData.Error, $rootScope.error);

           } else if (data.noLogin == 1) {

               window.location = "../../../Access/Close"

           }

       }).
       error(function (data, status, headers, config) {

           $("#SaveUpdateAction").button('reset');

           notify("Ocurrío un error.", $rootScope.error);

       });
    };

    $scope.setSelecControl = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Controllers, parseInt(a), 'idControl');

        $scope.newAction.parentmenu = $scope.Controllers[index];

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {

        for (var i = 0, len = myArray.length; i < len; i++) {
            if (myArray[i][property] === searchTerm) return i;
        }

        return -1;
    };

});
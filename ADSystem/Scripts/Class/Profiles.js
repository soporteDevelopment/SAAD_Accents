
angular.module("General").controller('ProfilesController', function ($scope, $http, $window, notify, $rootScope) {

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

        if ($scope.pageCount() == 0)
            return ret;

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

        $scope.listProfiles();

    });

    $scope.onLoad = function () {
        $scope.listProfiles();
    };

    $scope.listProfiles = function () {

        $http({
            method: 'POST',
            url: '../../../Profiles/ListProfiles',
            params: {
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                if (data.oData.Profiles.length > 0) {

                    $scope.Profiles = data.oData.Profiles;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Profiles = data.oData.Profiles;
                    $scope.total = data.oData.Count;

                }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });
    };

    $scope.AddProfile = function () {

        $window.location = '../../../Profiles/AddProfile'

    };

    $scope.SaveAddProfile = function () {

        $("#SaveAddProfile").button('loading');

        var selectedControl = _.pluck($scope.list, 'lAcciones');

        var selectedAction = new Array();

        _.forEach(selectedControl, function (n, key) {

            _.forEach(_.pluck(_.where(n, { 'Selected': true }), 'idAction'), function (x, y) {
                selectedAction.push(x);
            });

        });

        $http({
            method: 'POST',
            url: '../../../Profiles/SaveAddProfile',
            data: {
                profile: $scope.name,
                lActions: selectedAction
            }
        }).
       success(function (data, status, headers, config) {

           $("#SaveAddProfile").button('reset');

           if (data.success == 1) {

               notify(data.oData.Message, $rootScope.success);

               window.location = '../../../Profiles/Index';

           } else if (data.failure == 1) {

               notify(data.oData.Error, $rootScope.error);

           } else if (data.noLogin == 1) {

               window.location = "../../../Access/Close"

           }

       }).
       error(function (data, status, headers, config) {

           $("#SaveAddProfile").button('reset');

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    $scope.UpdateProfile = function (idPerfil) {

        $window.location = '../../../Profiles/UpdateProfile?idProfile=' + idPerfil;

    }

    $scope.SaveUpdateProfile = function (ID) {       

        var selectedControl = _.pluck($scope.list, 'lAcciones');

        var selectedAction = new Array();

        _.forEach(selectedControl, function (n, key) {

            _.forEach(_.pluck(_.where(n, { 'Selected': true }), 'idAction'), function (x, y) {
                selectedAction.push(x);
            });

        });

        $("#SaveUpdateProfile").button('loading');

        $http({
            method: 'POST',
            url: '../../../Profiles/SaveUpdateProfile',
            data: {
                idProfile: ID,
                lActions: selectedAction
            }
        }).
       success(function (data, status, headers, config) {

           $("#SaveUpdateProfile").button('reset');

           if (data.success == 1) {

               notify(data.oData.Message, $rootScope.success);

               window.location = '../../../Profiles/Index';

           } else if (data.failure == 1) {

               notify(data.oData.Error, $rootScope.error);

           } else if (data.noLogin == 1) {

               window.location = "../../../Access/Close"

           }

       }).
       error(function (data, status, headers, config) {

           $("#SaveUpdateProfile").button('reset');

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    $scope.SelectAllActions = function () {

        $scope.list.forEach(function (item, index) {
            
            item.lAcciones.forEach(function (subItem, subIndex) {

                subItem.Selected = true;

            });
            
        });

    }

});
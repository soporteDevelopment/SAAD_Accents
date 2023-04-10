angular.module("General").controller('ServicesController', function (models, ServiceValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.Service = "";

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.itemsToPrint = new Array();

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listServices();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listServices();
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
            $scope.listServices();
        }
    };
    $scope.$watch("currentPage", function (newValue, oldValue) {

        $scope.listServices();

    });

    $scope.onLoad = function () {

        $scope.listServices();

    };

    $scope.listServices = function (currentPage) {

        if (currentPage != undefined) {
            $scope.currentPage = currentPage;
        }

        $("#searchService").button("loading");

        $http({
            method: 'POST',
            url: '../../../Services/ListServices',
            params: {
                service: $scope.Service,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {

            $("#searchService").button("reset");

            if (data.success == 1) {

                if (data.oData.Services.length > 0) {

                    $scope.Services = data.oData.Services;
                    $scope.total = data.oData.Count;

                    $scope.itemsToPrint.length = 0;

                    angular.forEach(data.oData.Services, function (value, key) {
                        $scope.itemsToPrint.push({
                            Service: value.Service
                        });

                    });

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Services = data.oData.Services;
                    $scope.total = data.oData.Count;

                }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
            error(function (data, status, headers, config) {

                $("#searchService").button("reset");

                notify("Ocurrío un error.", $rootScope.error);

        });
    };

    $scope.valResult = {};

    $scope.newService = new models.Service();

    $scope.AddService = function () {

        $window.location = '../../Services/AddService';

    };

    $scope.SaveAddService = function () {

        $scope.valResult = ServiceValidator.validate($scope.newService);

        if ($scope.newService.$isValid) {

            $scope.SaveAdd();

        }
    };

    $scope.SaveAdd = function () {

        $("#SaveAddService").button("loading");

        $http({
            method: 'POST',
            url: '../../../Services/SaveAddService',
            params: {
                description: $scope.newService.Description,
                installation: $scope.newService.Installation
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveAddService").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $window.location = '../../../Services/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveAddService").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.UpdateService = function (ID) {

        $window.location = '../../Services/UpdateService?idService=' + ID;

    };

    $scope.SaveUpdateService = function (ID) {

        $scope.valResult = ServiceValidator.validate($scope.newService);

        if ($scope.newService.$isValid) {

            $scope.SaveUpdate(ID);
        }

    },

    $scope.SaveUpdate = function (ID) {

        $("#SaveUpdateService").button("loading");

        $http({
            method: 'POST',
            url: '../../../Services/SaveUpdateService',
            params: {
                idService: ID,
                description: $scope.newService.Description,
                installation: $scope.newService.Installation
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveUpdateService").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $window.location = '../../../Services/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveUpdateService").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

});
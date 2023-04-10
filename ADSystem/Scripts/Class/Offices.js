angular.module("General").controller('OfficesController', function (models, officeValidator, $scope, $http, $window, notify, $rootScope) {

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

        $scope.listOffices();

    });

    $scope.onLoad = function () {

        $scope.listOffices();

    };

    $scope.listOffices = function () {

        $http({
            method: 'POST',
            url: '../../../Offices/ListOffices',
            params: {
                offices: $scope.searchOffices,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            $("#searchOffices").button("reset");

            if (data.success == 1) {

                if (data.oData.Offices.length > 0) {

                    $scope.Offices = data.oData.Offices;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Offices = data.oData.Offices;
                    $scope.total = data.oData.Count;

                }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

        });
    };

    $scope.valResult = {};

    $scope.newOffice = new models.Office();

    $scope.AddOffice = function () {

        $window.location = '../../Offices/AddOffice';

    };

    $scope.SaveAddOffice = function () {

        if ($scope.addOfficeCustomerForm.$valid) {

            $scope.SaveAdd();

        }
    };

    $scope.SaveAdd = function () {

        $("#SaveAddOffice").button("loading");

        $http({
            method: 'POST',
            url: '../../../Offices/SaveAddOffice',
            params: {
                name: $scope.newOffice.Name,
                phone: ($scope.newOffice.Phone == null) ? "" : $scope.newOffice.Phone,
                street: ($scope.newOffice.Street == null) ? "" : $scope.newOffice.Street,
                extNum: ($scope.newOffice.ExtNum == null) ? "" : $scope.newOffice.ExtNum,
                intNum: ($scope.newOffice.IntNum == null) ? "" : $scope.newOffice.IntNum,
                neighborhood: ($scope.newOffice.Neighborhood == null) ? "" : $scope.newOffice.Neighborhood,
                idTown: ($scope.newOffice.idTown == undefined) ? 999 : $scope.newOffice.idTown.idMunicipio,
                cp: ($scope.newOffice.CP == undefined) ? 0 : $scope.newOffice.CP,
                email: $scope.newOffice.Email,
                percentage: ($scope.newOffice.Percentage == undefined) ? 0 : $scope.newOffice.Percentage,
                idOrigin: $scope.idOrigin
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveAddOffice").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $window.location = '../../../Offices/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveAddOffice").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.SaveAddOfficeDialog = function () {

        if ($scope.addOfficeCustomerForm.$valid) {

            $scope.SaveAddDialog();

        }
    };   

    $scope.SaveAddDialog = function () {

        $("#SaveAddOffice").button("loading");

        $http({
            method: 'POST',
            url: '../../../Offices/SaveAddOffice',
            params: {
                name: $scope.newOffice.Name,
                phone: ($scope.newOffice.Phone == null) ? "" : $scope.newOffice.Phone,
                street: ($scope.newOffice.Street == null) ? "" : $scope.newOffice.Street,
                extNum: ($scope.newOffice.ExtNum == null) ? "" : $scope.newOffice.ExtNum,
                intNum: ($scope.newOffice.IntNum == null) ? "" : $scope.newOffice.IntNum,
                neighborhood: ($scope.newOffice.Neighborhood == null) ? "" : $scope.newOffice.Neighborhood,
                idTown: ($scope.newOffice.idTown == undefined) ? 999 : $scope.newOffice.idTown.idMunicipio,
                cp: ($scope.newOffice.CP == undefined) ? 0 : $scope.newOffice.CP,
                email: $scope.newOffice.Email,
                percentage: ($scope.newOffice.Percentage == undefined) ? 0 : $scope.newOffice.Percentage,
                idOrigin: $scope.idOrigin
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveAddOffice").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $("#openModalAddOfficeCustomer").modal("hide");

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveAddOffice").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.UpdateOffice = function (ID) {

        $window.location = '../../Offices/UpdateOffice?idOffice=' + ID;

    };

    $scope.SaveUpdateOffice = function (ID) {

        if ($scope.addOfficeCustomerForm.$valid) {

            $scope.SaveUpdate(ID);
        }

    },

    $scope.SaveUpdate = function (ID) {

        $("#SaveUpdateOffice").button("loading");

        $http({
            method: 'POST',
            url: '../../../Offices/SaveUpdateOffice',
            params: {
                idOffice: ID,
                name: $scope.newOffice.Name,
                phone: ($scope.newOffice.Phone == null) ? "" : $scope.newOffice.Phone,
                street: ($scope.newOffice.Street == null) ? "" : $scope.newOffice.Street,
                extNum: ($scope.newOffice.ExtNum == null) ? "" : $scope.newOffice.ExtNum,
                intNum: ($scope.newOffice.IntNum == null) ? "" : $scope.newOffice.IntNum,
                neighborhood: ($scope.newOffice.Neighborhood == null) ? "" : $scope.newOffice.Neighborhood,
                idTown: ($scope.newOffice.idTown == undefined) ? 999 : $scope.newOffice.idTown.idMunicipio,
                cp: ($scope.newOffice.CP == undefined) ? 0 : $scope.newOffice.CP,
                email: $scope.newOffice.Email,
                percentage: ($scope.newOffice.Percentage == undefined) ? 0 : $scope.newOffice.Percentage,
                idOrigin: $scope.newOffice.idOrigin
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveUpdateOffice").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $window.location = '../../../Offices/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveUpdateOffice").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    //Delete Office

    $scope.DeleteOffice = function (idDespacho) {

        var r = confirm("Esta seguro que desea eliminar el despacho?");

        if (r == true) {

            $http({
                method: 'POST',
                url: '../../../Offices/DeleteOffice',
                params: {
                    idDespacho: idDespacho
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    $scope.listOffices();

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

        }
    };

    $scope.getStates = function () {

        $http({
            method: 'GET',
            url: '../../../Users/GetStates'
        }).
        success(function (data, status, headers, config) {

            $scope.States = data;

            if ($scope.selectedState > 0) {

                $scope.setSelecState($scope.selectedState);

                $scope.getTowns();

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.getTowns = function () {

        $http({
            method: 'GET',
            url: '../../../Users/GetTowns',
            params: {
                idState: $scope.state.idEstado
            }
        }).
        success(function (data, status, headers, config) {

            $scope.Towns = data;

            if ($scope.selectedTown > 0) {

                $scope.setSelecTown($scope.selectedTown);

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.setSelecState = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.States, parseInt(a), 'idEstado');

        $scope.state = $scope.States[index];

    };

    $scope.setSelecTown = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Towns, parseInt(a), 'idMunicipio');

        $scope.newOffice.idTown = $scope.Towns[index];

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
        for (var i = 0, len = myArray.length; i < len; i++) {
            if (myArray[i][property] === searchTerm) return i;
        }
        return -1;
    };
    $scope.getOriginCustomer = function () {
        $http({
            method: 'GET',
            url: '../../../CustomerOrigin/Get'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.origins = data.oData.Origins;
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

});
angular.module("General").controller('ProvidersController', function (models, providerValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.Empresa = "";
    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.selectedBrandId = 0;
    //$scope.idBrand = 0;

    $scope.Init = function () {
        //$scope.GetTypeService();
    };

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listProviders();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listProviders();
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
            $scope.listProviders();
        }
    };

    $scope.$watch("currentPage", function (newValue, oldValue) {

        $scope.getBrandCatalogs();

    });

    $scope.onLoad = function () {

        $scope.listProviders();

    };

    $scope.GetTypeService = function () {       
       
        if ($scope.boolPedidosMedida) {
            $http({
                method: 'GET',
                url: '../../../CatalogTypeService/GetAll'
            }).
                success(function (data, status, headers, config) {
                    $scope.TipoServicios = data.oData.typeServices;
                }).
                error(function (data, status, headers, config) {
                    notify("Ocurrío un error.", $rootScope.error);
                });
        } else {

            $scope.idTipoServicio = "";
            $scope.TipoServicios = "";
        }

     
        
    };

    $scope.listProviders = function () {

        $("#searchProvider").button("loading");

        $http({
            method: 'POST',
            url: '../../../Providers/ListProviders',
            params: {
                provider: ($scope.Empresa == "" || $scope.Empresa == null) ? "" : $scope.Empresa,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {

                $("#searchProvider").button("reset");

                if (data.success == 1) {

                    if (data.oData.Providers.length > 0) {

                        $scope.Providers = data.oData.Providers;
                        $scope.total = data.oData.Count;

                    } else {

                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.Providers = data.oData.Providers;
                        $scope.total = data.oData.Count;

                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                $("#searchProvider").button("reset");

                notify("Ocurrío un error.", $rootScope.error);

            });
    };

    $scope.AddProvider = function () {

        $window.location = '../../Providers/AddProvider';

    };

    $scope.valResult = {};

    $scope.newProvider = new models.Provider();

    $scope.SaveAddProvider = function () {

        $scope.valResult = providerValidator.validate($scope.newProvider);

        if ($scope.newProvider.$isValid) {

            $scope.SaveAdd();

        }

    },

        $scope.SaveAdd = function () {

        console.log("$scope.idTipoServicio && $scope.boolPedidosMedida", $scope.idTipoServicio , $scope.boolPedidosMedida)

        if ($scope.boolPedidosMedida && !$scope.idTipoServicio) {

            notify("Tipo de Servicio es requerido", $rootScope.error);
            return false;
        } 
            
        $("#SaveAddProvider").button("loading");

        $http({
            method: 'POST',
            url: '../../../Providers/SaveAddProvider',
            params: {
                company: $scope.newProvider.Company,
                rsocial: $scope.rsocial,
                ncommerce: $scope.ncommerce,
                domFiscal: $scope.domFiscal,
                domEntrega: $scope.domEntrega,
                nacionalidad: $scope.nacionalidad,
                empRFC: $scope.empRFC,
                sitioWeb: $scope.sitioWeb,
                sitioWebCat: $scope.sitioWebCat,
                userWeb: $scope.userWeb,
                passWeb: $scope.passWeb,
                name: $scope.newProvider.Name,
                email: $scope.email,
                telephone: $scope.telephone,
                credit: $scope.credit,
                creditDays: $scope.creditDays,
                bankMEX: $scope.bankMEX,
                holderMEX: $scope.holderMEX,
                clabeMEX: $scope.clabeMEX,
                countMEX: $scope.countMEX,
                branchMEX: $scope.branchMEX,
                rfcMEX: $scope.rfcMEX,
                dataINT: $scope.dataINT,
                freight: $scope.freight,
                costImport: $scope.costImport,
                regimenFiscal: $scope.regimenFiscal,
                proveedorPedidosMedida: $scope.boolPedidosMedida,
                idTipoServicio: $scope.idTipoServicio
            }
        }).
            success(function (data, status, headers, config) {

                $("#SaveAddProvider").button("reset");

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    window.location = '../../../Providers/Index';

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                $("#SaveAddProvider").button("reset");

                notify("Ocurrío un error.", $rootScope.error);

            });

        };

    $scope.UpdateProvider = function (ID) {
        $window.location = '../../Providers/UpdateProvider?idProvider=' + ID;

    };

    $scope.SaveUpdateProvider = function (idProvider, idBankMXN, idBankINT) {

        $scope.valResult = providerValidator.validate($scope.newProvider);

        if ($scope.newProvider.$isValid) {

            $scope.SaveUpdate(idProvider, idBankMXN, idBankINT);

        }


    },

        $scope.SaveUpdate = function (idProvider, idBankMXN, idBankINT) {

        if ($scope.boolPedidosMedida && !$scope.idTipoServicio) {
            notify("Tipo de Servicio es requerido", $rootScope.error);
            return false;
        }

            $("#SaveUpdateProvider").button("loading");

            $http({
                method: 'POST',
                url: '../../../Providers/SaveUpdateProvider',
                params: {
                    idProvider: idProvider,
                    idBankMxn: idBankMXN,
                    idBankInt: idBankINT,
                    company: $scope.newProvider.Company,
                    rsocial: $scope.rsocial,
                    ncommerce: $scope.ncommerce,
                    domFiscal: $scope.domFiscal,
                    domEntrega: $scope.domEntrega,
                    nacionalidad: $scope.nacionalidad,
                    empRFC: $scope.empRFC,
                    sitioWeb: $scope.sitioWeb,
                    sitioWebCat: $scope.sitioWebCat,
                    userWeb: $scope.userWeb,
                    passWeb: $scope.passWeb,
                    name: $scope.newProvider.Name,
                    email: $scope.email,
                    telephone: $scope.telephone,
                    credit: $scope.credit,
                    creditDays: $scope.creditDays,
                    bankMEX: $scope.bankMEX,
                    holderMEX: $scope.holderMEX,
                    clabeMEX: $scope.clabeMEX,
                    countMEX: $scope.countMEX,
                    branchMEX: $scope.branchMEX,
                    rfcMEX: $scope.rfcMEX,
                    dataINT: $scope.dataINT,
                    freight: $scope.freight,
                    costImport: $scope.costImport,
                    regimenFiscal: $scope.regimenFiscal,
                    proveedorPedidosMedida: $scope.boolPedidosMedida,
                    idTipoServicio: $scope.idTipoServicio
                }
            }).
                success(function (data, status, headers, config) {

                    $("#SaveUpdateProvider").button("reset");

                    if (data.success == 1) {

                        notify(data.oData.Message, $rootScope.success);

                        window.location = '../../../Providers/Index';

                    } else if (data.failure == 1) {

                        notify(data.oData.Error, $rootScope.error);

                    } else if (data.noLogin == 1) {

                        window.location = "../../../Access/Close"

                    }

                }).
                error(function (data, status, headers, config) {

                    $("#SaveUpdateProvider").button("reset");

                    notify("Ocurrío un error.", $rootScope.error);

                });

        };

    $scope.UpdateStatusProvider = function (ID, Estatus) {

        $http({
            method: 'POST',
            url: '../../../Providers/UpdateStatusProvider',
            params: {
                idProvider: ID,
                bStatus: Estatus
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

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


    $scope.getBrandCatalogs = function () {

        $http({
            method: 'POST',
            url: '../../../Providers/GetAllBrandForProviders'
        }).
            success(function (data, status, headers, config) {

                $scope.Brands = data.oData.Brands;

                if ($scope.selectedBrandId > 0) {

                    $scope.setSelectBrand($scope.selectedBrandId);

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.setSelectBrand = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Brands, parseInt(a), 'idMarca');

        $scope.newProvider.Brand = $scope.Brands[index];

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
        for (var i = 0, len = myArray.length; i < len; i++) {
            if (myArray[i][property] === searchTerm) return i;
        }
        return -1;
    };


    $scope.InitData = (data) => {
        console.log(data);
        const { idTipoServicio, ProveedorPedidosMedida } = data;
        $scope.boolPedidosMedida = ProveedorPedidosMedida;
        $scope.idTipoServicio = idTipoServicio;
        console.log($scope.boolPedidosMedida);
        if ($scope.boolPedidosMedida) {
            $scope.GetTypeService();
        }

    }

});



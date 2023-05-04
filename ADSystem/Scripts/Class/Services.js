angular.module("General").controller('ServicesController', function (models, ServiceValidator, $scope, $timeout, $http, $window, notify, $rootScope, GUID) {

    $scope.Service = "";
    $scope.items = new Array();
    $scope.itemsToSend = new Array();
    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.itemsToPrint = new Array();
    $scope.itemSelected = ""
    $scope.showButtonAdd = ""

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listServices();
        }
    };

    $scope.OpenModalAddMeasure = () => {
        $scope.newService.typeMeasures = [];
        $scope.newService.valueMeasure = "";
        $("#openModalMeasures").modal("show");
    };

    $scope.LoadTypeMeasure = function () {

        $http({
            method: 'GET',
            url: '../../../CatalogTypeMeasure/GetAll',

        }).
            success(function (data, status, headers, config) {

                console.log(data.oData.typeMeasures)
                if (data.success == 1) {

                    $scope.listTypeMeasures = data.oData.typeMeasures;

                    console.log("$scope.listTypeMeasures ", $scope.listTypeMeasures)



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

    $scope.AddMeasureToService = () => {

        if ($scope.newService.typeMeasures.idTipoMedida && $scope.newService.valueMeasure) {

            if ($scope.items.length > 0) {
                if ($scope.items.some((item) => {
                    if (item.idMeasureType == $scope.newService.typeMeasures.idTipoMedida) {
                        return true;
                    }
                })) {
                    notify('Ya existe un registro con el mismo tipo de medida', $rootScope.error);
                    return false;
                } else {
                    $scope.items.push({
                        idMeasureType: $scope.newService.typeMeasures.idTipoMedida,
                        measureDesc: $scope.newService.typeMeasures.NombreMedida,
                        measureValue: $scope.newService.valueMeasure,
                    });
                }
            } else {
                $scope.items.push({
                    idMeasureType: $scope.newService.typeMeasures.idTipoMedida,
                    measureDesc: $scope.newService.typeMeasures.NombreMedida,
                    measureValue: $scope.newService.valueMeasure,
                });
            }



            $scope.newService.typeMeasures = [];
            $scope.newService.valueMeasure = "";

        } else {

            notify('Verifique que la información este correctamente', $rootScope.error);
        }
    };


    $scope.EditMeasureType = function (item) {
        $scope.itemSelected = item;
        $scope.setSelectMeasureType(item.measureDesc);
        $scope.newService.valueMeasure = item.measureValue;
        $scope.newService.typeMeasures.idTipoMedida = item.idMeasureType;

        $("#modalEditMeasureType").modal("show");
    };

    $scope.setSelectMeasureType = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.listTypeMeasures, a, 'NombreMedida');

        $scope.newService.typeMeasures = $scope.listTypeMeasures[index];
    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {

        if (myArray != undefined) {

            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }

    };

    $scope.DeleteMeasureType = (id) => {
        _.remove($scope.items, function (n) {
            return n.idMeasureType == id;
        });
    }


    $scope.SaveEditMeasureService = () => {

        angular.forEach($scope.items, function (item, index) {
            if (item.idMeasureType === $scope.itemSelected.idMeasureType) {
                $scope.items[index].measureDesc = $scope.newService.typeMeasures.NombreMedida;
                $scope.items[index].measureValue = $scope.newService.valueMeasure;
            }
        });

        $("#modalEditMeasureType").modal("hide");
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

        let confirmText = "¿Estás seguro?, verifique que la información este correctamente";
        if (confirm(confirmText) == true) {
            $("#SaveAddService").button("loading");

            if ($scope.items) {
                angular.forEach($scope.items, function (value, key) {
                    $scope.itemsToSend.push({
                        idTipoMedida: value.idMeasureType,
                        Valor: value.measureValue,
                        NombreMedida: value.measureDesc
                    });

                });
            }

            $http({
                method: 'POST',
                url: '../../../Services/SaveAddService',
                data: JSON.stringify($scope.itemsToSend),
                params: {
                    description: $scope.newService.Description,
                    installation: $scope.newService.Installation,
                }
            }).
                success(function (data, status, headers, config) {

                    $("#SaveAddService").button("reset");

                    if (data.success == 1) {

                        $timeout(function () {
                            notify(data.oData.Message, $rootScope.success, { timeOut: 5000 })
                        }, 1000).then(function () {
                            $window.location = '../../../Services/Index';
                        });


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
        }




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


        let confirmText = "Se realizarón cambios en la información. ¿Esta seguro de guardar el cambio?";

            if (confirm(confirmText) == true) {

                $("#SaveUpdateService").button("loading");

                if ($scope.items) {

                    angular.forEach($scope.items, function (value, key) {
                        $scope.itemsToSend.push({
                            idTipoMedida: value.idMeasureType,
                            Valor: value.measureValue,
                            NombreMedida: value.measureDesc
                        });

                    });
                }

                $http({
                    method: 'POST',
                    url: '../../../Services/SaveUpdateService',
                    data: JSON.stringify($scope.itemsToSend),
                    params: {
                        idService: ID,
                        description: $scope.newService.Description,
                        installation: $scope.newService.Installation
                    }
                }).
                    success(function (data, status, headers, config) {

                        $("#SaveUpdateService").button("reset");

                        if (data.success == 1) {

                            $timeout(function () {
                                notify(data.oData.Message, $rootScope.success, { timeOut: 5000 })
                            }, 1000).then(function () {
                                $window.location = '../../../Services/Index';
                            });

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


            }



        };

    $scope.InitData = (data) => {

        if (data.oMedidasEstandar.length) {
            $scope.showButtonAdd = false;
            data.oMedidasEstandar.forEach((item) => {
                $scope.items.push({
                    idMeasureType: item.idTipoMedida,
                    measureDesc: item.NombreMedida,
                    measureValue: item.Valor,
                });
            })
        } else {
            $scope.showButtonAdd = true;

        }
    }

    let input = document.getElementById("valueMeasure");
    let inputEdit = document.getElementById("valueMeasureEdit");

    if (input) {
        input.addEventListener("blur", function () {
            const regex = /^[0-9]*\.?[0-9]*$/;
            const isValid = regex.test(this.value);

            if (!isValid) {
                // Limpiar el valor del input
                $("#valueMeasure").val('')
            } else {

                if (this.value == "" || this.value == ".") {
                    $("#valueMeasure").val('')
                    return false;
                }
                this.value = parseFloat(this.value).toFixed(2);

            }
        });
    }
    if (inputEdit) {
        inputEdit.addEventListener("blur", function () {
            const regex = /^[0-9]*\.?[0-9]*$/;
            const isValid = regex.test(this.value);

            if (!isValid) {
                // Limpiar el valor del input
                $("#valueMeasureEdit").val('')
            } else {

                if (this.value == "" || this.value == ".") {
                    $("#valueMeasureEdit").val('')
                    return false;
                }
                this.value = parseFloat(this.value).toFixed(2);

            }
        });
    }
    
});
angular.module("General").controller('CatalogViewController', function (models, $scope, $http, $window, notify, $rootScope) {
    $scope.selectedCatalog = "";
    $scope.number = "";
    $scope.branch = "";
    $scope.datetime = new Date();
    $scope.items = new Array();
    $scope.subTotal = 0;
    $scope.total = 0;
    $scope.amountCatalogs = 0;
    $scope.customer = {
        type: "moral"
    };
    $scope.includeURLMoral = "";
    $scope.includeURLPhysical = "";

    $scope.SetBranch = function () {
        setTimeout(function () {
            $("#openModal").modal("show");
        }, 0);
    };

    $scope.SetBranchName = function (branchName, IDBranch) {
        $scope.branch = branchName;
        $scope.branchID = IDBranch;
        $("#openModal").modal("hide");
        $scope.GetPreviousNumber();
    };

    $scope.AddMoralCustomer = function () {
        var now = new Date();

        $scope.includeURLMoral = "../../Customers/PartialAddMoralCustomer?update=" + now;

        $("#openModalAddMoralCustomer").modal("show");

        $('#openModalAddMoralCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadCustomers();
        });
    };

    $scope.AddPhysicalCustomer = function () {
        var now = new Date();

        $scope.includeURLPhysical = "../../Customers/PartialAddPhysicalCustomer?update=" + now;

        $("#openModalAddPhysicalCustomer").modal("show");

        $('#openModalAddPhysicalCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadCustomers();
        });
    };

    $scope.AddOffice = function () {
        var now = new Date();

        $scope.includeURLOffice = "../../Offices/PartialAddOffice?update=" + now;

        $("#openModalAddOfficeCustomer").modal("show");

        $('#openModalAddOfficeCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadOffices();
        });
    };

    $scope.GetPreviousNumber = function () {
        $http({
            method: 'GET',
            url: '../../../CatalogView/GetPreviousNumber'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.number = data.oData.Number;
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

    $scope.LoadCustomers = function () {
        $scope.LoadPhysicalCustomers();
        $scope.LoadMoralCustomers();
    };

    $scope.LoadPhysicalCustomers = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllPhysicalCustomers'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.physicalCustomers = data.oData.Customers;
                    if ($scope.customer.type == "physical") {
                        ($scope.outCustomer != undefined || $scope.outCustomer != null) ? $scope.setSelecCustomerPhysical($scope.outCustomer) : "";
                    }
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

    $scope.LoadMoralCustomers = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllMoralCustomers'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.moralCustomers = data.oData.Customers;
                    if ($scope.customer.type == "moral") {
                        ($scope.outCustomer != undefined || $scope.outCustomer != null) ? $scope.setSelecCustomerMoral($scope.outCustomer) : "";
                    }
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

    $scope.LoadOffices = function () {
        $http({
            method: 'POST',
            url: '../../../Offices/ListAllOffices'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.offices = data.oData.Offices;

                    if ($scope.customer.type == "office") {
                        ($scope.outOffice != undefined || $scope.outOffice != null) ? $scope.setSelecCustomerOffice($scope.outCustomer) : "";
                    }

                    ($scope.outOffice != undefined || $scope.outOffice != null) ? $scope.setSelecOffices($scope.outOffice) : "";
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

    $scope.GetCatalogByCode = function () {

        if ($scope.branchID != 1) {

            $http({
                method: 'GET',
                url: '../../../Catalogs/GetById',
                params: {
                    id: ($scope.selectedCatalog == "" || $scope.selectedCatalog == null) ? "" : $scope.selectedCatalog.originalObject.idCatalogo
                }
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        var result = _.result(_.find($scope.items, function (chr) {
                            return chr.idCatalogo == data.oData.Catalog.idCatalogo
                        }), "idCatalogo");

                        if (result == undefined) {
                            $scope.items.push({
                                idCatalogo: data.oData.Catalog.idCatalogo,
                                Imagen: '/Content/Catalogs/' + data.oData.Catalog.Imagen,
                                Codigo: data.oData.Catalog.Codigo,
                                Modelo: data.oData.Catalog.Modelo,
                                Volumen: data.oData.Catalog.Volumen,
                                Precio: data.oData.Catalog.Precio,
                                Cantidad: 1,
                                Costo: 0,
                                Comentarios: ""
                            });
                        } else {
                            var index = _.indexOf($scope.items, _.find($scope.items, { idCatalogo: data.oData.Catalog.idCatalogo }));
                            $scope.items[index].Cantidad++;
                        }

                        $scope.GetStockByBranch(data.oData.Catalog.idCatalogo);

                    } else if (data.failure == 1) {
                        notify(data.oData.Error, $rootScope.error);
                    } else if (data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    }

                    $scope.selectedCatalog = "";
                }).
                error(function (data, status, headers, config) {
                    notify("Ocurrío un error.", $rootScope.error);
                    $scope.selectedCatalog = "";
                });
        } else {
            notify("Seleccione una sucursal.", $rootScope.error);
        }

        $("#code_value").val("");
    };

    $scope.GetStockByBranch = function (idCatalog) {

        $http({
            method: 'GET',
            url: '../../../BranchCatalogs/GetStockByBranch',
            params: {
                idCatalog: idCatalog,
                idBranch: $scope.branchID
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    var result = _.find($scope.items, function (chr) {
                        return chr.idCatalogo == idCatalog
                    });

                    if (result.Cantidad > data.oData.Stock) {
                        var index = _.indexOf($scope.items, _.find($scope.items, { idCatalogo: idCatalog }));

                        if (data.oData.Stock == 0 && index > -1) {
                            $scope.items.splice(index, 1);
                        } else {
                            $scope.items[index].Cantidad = data.oData.Stock;
                        }
                        $scope.ValidateStock(idCatalog);
                    }

                    $scope.CalculateTotalCost();
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }

                $scope.selectedCode = "";
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.ValidateStock = function (idCatalog) {

        $http({
            method: 'GET',
            url: '../../../BranchCatalogs/GetStock',
            params: {
                idCatalog: idCatalog
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify("<p>Amazonas:" + data.oData.Stock.Amazonas
                        + "<br/>Guadalquivir:" + data.oData.Stock.Guadalquivir
                        + "<br/>Textura:" + data.oData.Stock.Textura
                        + "<br/>EN VISTA:" + data.oData.Stock.Vista + "</p>", $rootScope.error);
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }

                $scope.selectedCode = "";
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.OpenCommentsModal = function (idCatalog, comments) {
        $scope.catalogItem = idCatalog;
        $scope.commentsItem = comments;

        $("#modalComments").modal("show");
    };

    $scope.SaveComments = function () {
        var index = _.findIndex($scope.items, function (o) { return o.idCatalogo == $scope.catalogItem });

        if (index > -1) {
            $scope.items[index].Comentarios = $scope.commentsItem;
        }

        $("#modalComments").modal("hide");
    };

    $scope.DeleteCatalog = function (idCatalog) {
        _.remove($scope.items, function (n) {
            return n.idCatalogo == idCatalog;
        });

        $scope.CalculateTotalCost();
    };

    $scope.CalculateTotalCost = function () {
        $scope.subTotal = 0;
        $scope.amountCatalogs = 0;

        angular.forEach($scope.items, function (value, key) {
            value.Costo = value.Cantidad * value.Precio;
            $scope.subTotal = $scope.subTotal + value.Costo;
            $scope.amountCatalogs = $scope.amountCatalogs + parseInt(value.Cantidad);
        });

        $scope.total = $scope.subTotal;
    };

    $scope.ConfirmAndValidate = function () {
        var showModal = false;

        $("#txtValidation").empty();
        if ($scope.customer.type == "moral") {
            if (($scope.moralCustomer == undefined) || ($scope.moralCustomer == null)) {
                $("#txtValidation").append("Seleccione un cliente moral  </br>");
                showModal = true;
            }
        }

        if ($scope.customer.type == "physical") {
            if (($scope.physicalCustomer == undefined) || ($scope.physicalCustomer == null)) {
                $("#txtValidation").append("Seleccione un cliente físico  </br>");
                showModal = true;
            }
        }

        if ($scope.customer.type == "office") {
            if (($scope.officeCustomer == undefined) || ($scope.officeCustomer == null)) {
                $("#txtValidation").append("Seleccione un despacho  </br>");
                showModal = true;
            }
        }

        if ($scope.items.length == 0) {
            $("#txtValidation").append("Ingrese un producto </br>");
            showModal = true;
        }

        angular.forEach($scope.items, function (value, key) {
            if (value.Cantidad <= 0) {
                $("#txtValidation").append("No se permiten cantidades menores o iguales a 0");
                showModal = true;
            }
        });

        if (showModal == true) {
            $("#modalValidation").modal("show");
        } else {
            $scope.SaveAdd();
        }
    };

    $scope.SaveAdd = function () {
        $scope.buttonDisabled = true;

        var detail = new Array();
        var amount = 0;
        angular.forEach($scope.items, function (value, key) {
            detail.push({
                idCatalogo: value.idCatalogo,
                Precio: value.Precio,
                Cantidad: value.Cantidad,
                idSucursal: $scope.branchID,
                Comentarios: value.Comentarios
            });

            amount = amount + value.Cantidad;
        });

        var typeCustomer = null;

        switch ($scope.customer.type) {
            case "physical":
                typeCustomer = 0;
                break;
            case "moral":
                typeCustomer = 1;
                break;
            case "office":
                typeCustomer = 2;
                break;
        }

        $http({
            method: 'POST',
            url: '../../../CatalogView/Post',
            data: {
                idClienteFisico: ($scope.customer.type == "physical") ? $scope.physicalCustomer.idCliente : null,
                idClienteMoral: ($scope.customer.type == "moral") ? $scope.moralCustomer.idCliente : null,
                idDespacho: ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
                TipoCliente: typeCustomer,
                idSucursal: $scope.branchID,
                Fecha: $scope.datetime,
                CantidadProductos: amount,
                Subtotal: $scope.subTotal,
                Total: $scope.total,
                Detalle: detail
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.idCatalogView = data.oData.idCatalogView;
                    notify(data.oData.Message, $rootScope.success);

                    $('#modalPrint').on('hidden.bs.modal', function (e) {
                        window.location = "../../../CatalogView/Index";
                    });

                    $("#modalPrint").modal("show");
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }

                $scope.buttonDisabled = false;
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
                $scope.buttonDisabled = false;
            });
    };

    $scope.Print = function () {
        var URL = '../../../CatalogView/Print?id=' + $scope.idCatalogView;
        var win = window.open(URL, '_blank');
        win.focus();
    };

    $scope.setSelectCustomerPhysical = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.physicalCustomers, parseInt(a), 'idCliente');
        $scope.physicalCustomer = $scope.physicalCustomers[index];
    };

    $scope.setSelectCustomerMoral = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.moralCustomers, parseInt(a), 'idCliente');
        $scope.moralCustomer = $scope.moralCustomers[index];
    };

    $scope.setSelectCustomerOffice = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.offices, parseInt(a), 'idDespacho');
        $scope.officeCustomer = $scope.offices[index];
    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
        if (myArray != undefined) {
            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }
    };

    $scope.CleanAnotherCustomer = function (typeCustomer) {
        switch (typeCustomer) {
            case 1:
                $scope.physicalCustomer = undefined;
                $scope.officeCustomer = undefined;
                break;
            case 2:
                $scope.moralCustomer = undefined;
                $scope.officeCustomer = undefined;
                break;
            case 3:
                $scope.moralCustomer = undefined;
                $scope.physicalCustomer = undefined;
                break;
        }
    };
});

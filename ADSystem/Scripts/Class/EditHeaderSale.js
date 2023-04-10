angular.module("General").controller('EditSalesController', function (GetDiscount, GetDiscountOffice, GetDiscountSpecial, $scope, $http, $window, notify, $rootScope) {

    $scope.type = 0;

    $scope.LoadInformation = function (typeCustomer, customer, idOffice, seller, discount, iva) {
        //Select type Customer
        $scope.type = (typeCustomer == 0) ? "physical" : (typeCustomer == 1) ? "moral" : "office";

        //Select customer
        $scope.outCustomer = customer;

        //Select office
        $scope.checkedOffices = (idOffice != undefined && idOffice.length != 0) ? true : false;
        $scope.outOffice = idOffice;

        //Select seller
        $scope.outSellerOne = seller[0];
        $scope.outSellerTwo = seller[1];

        if (discount > 0) {
            $scope.checkedDiscount = true;
            $scope.discount = parseFloat(discount);
        }

        if (iva == 1) {
            $scope.checkedIVA = true;
        }
    };

    $scope.Init = function (detail) {
        $scope.items = detail;
    };

    $scope.LoadCustomers = function () {
        $scope.LoadPhysicalCustomers();
        $scope.LoadMoralCustomers();
        $scope.LoadOffices();
    };

    $scope.LoadPhysicalCustomers = function () {
        $http({
            method: 'POST',
            url: '../../../Customers/ListAllPhysicalCustomers'
        }).
       success(function (data, status, headers, config) {
           if (data.success == 1) {

               $scope.physicalCustomers = data.oData.Customers;

               if ($scope.type == "physical") {
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

               if ($scope.type == "moral") {
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

               if ($scope.type == "office") {
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

    $scope.LoadUsers = function () {
        $http({
            method: 'POST',
            url: '../../../Users/ListAllUsers'
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {
               $scope.usersOne = data.oData.Users;
               if ($scope.outSellerOne != undefined) {
                   $scope.setSelecUsersOne($scope.outSellerOne);
                   $scope.LoadUsersButNot();
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

    $scope.LoadUsersButNot = function () {
        $http({
            method: 'GET',
            url: '../../../Users/ListAllUsersButNot',
            params: {
                idUser: $scope.sellerOne.idUsuario
            }
        }).
       success(function (data, status, headers, config) {
           if (data.success == 1) {
               $scope.usersTwo = data.oData.Users;
               if ($scope.outSellerTwo != undefined) {
                   $scope.setSelecUsersTwo($scope.outSellerTwo);
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

    $scope.setSelecCustomerPhysical = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.physicalCustomers, parseInt(a), 'idCliente');
        $scope.physicalCustomer = $scope.physicalCustomers[index];
    };

    $scope.setSelecCustomerMoral = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.moralCustomers, parseInt(a), 'idCliente');
        $scope.moralCustomer = $scope.moralCustomers[index];
    };

    $scope.setSelecCustomerOffice = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.offices, parseInt(a), 'idDespacho');
        $scope.officeCustomer = $scope.offices[index];
        $scope.CalculateTotalCost();
    };

    $scope.setSelecOffices = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.offices, parseInt(a), 'idDespacho');
        $scope.office = $scope.offices[index];
    };

    $scope.setSelecUsersOne = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.usersOne, a, 'NombreCompleto');
        $scope.sellerOne = $scope.usersOne[index];
    };

    $scope.setSelecUsersTwo = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.usersTwo, a, 'NombreCompleto');
        $scope.sellerTwo = $scope.usersTwo[index];
    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
        if (myArray != undefined) {
            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }
            return -1;
        }
    };

    $scope.CalculateTotalCost = function () {
        $scope.subTotal = 0;
        $scope.CantidadProductos = 0;
        $scope.totalCreditNotes = 0;
        $scope.discount = ($scope.discount != null) ? $scope.discount : 0;

        angular.forEach($scope.items, function (value, key) {
            if (value.idCredito == null) {
                var cost = value.Cantidad * value.Precio;
                var percentage = value.Descuento / 100;
                var discount = cost * percentage;

                value.Costo = cost - (discount);
                $scope.subTotal = $scope.subTotal + value.Costo;
                $scope.CantidadProductos = $scope.CantidadProductos + parseInt(value.Cantidad);
            } else {
                $scope.totalCreditNotes = value.Cantidad * value.Precio;
            }

        });

        $scope.IVA = (($scope.subTotal - (($scope.subTotal) * ($scope.discount / 100)) + $scope.totalCreditNotes)) * .16;

        if ($scope.IVA < 0) {
            $scope.IVA = 0;
        }

        if (($scope.checkedDiscount = true) && ($scope.discount > 0)) {
            var percentage = $scope.discount / 100;
            var discount = $scope.subTotal * percentage;

            if ($scope.checkedIVA == true) {
                $scope.total = (($scope.subTotal - discount) + $scope.totalCreditNotes) + $scope.IVA;
            } else {
                $scope.total = ($scope.subTotal - discount) + $scope.totalCreditNotes;
            }
        } else {
            $scope.checkedDiscount = false;
            $scope.discount = 0;

            if ($scope.checkedIVA == true) {
                $scope.total = (($scope.subTotal + $scope.totalCreditNotes) + $scope.IVA);

            } else {
                $scope.total = $scope.subTotal + $scope.totalCreditNotes;
            }
        }

        if ($scope.total < 0) {
            $scope.resCredit = $scope.total * -1;
        }
    };

    $scope.CalculateTotalForDetail = function (subTotal, discount, totalNotasCredito) {
        subTotal = subTotal - (subTotal * (discount / 100));
        subTotal = parseFloat(subTotal);
        totalNotasCredito = parseFloat(totalNotasCredito);
        subTotal = subTotal + totalNotasCredito;
        $scope.IVA = subTotal * .16;

        if ($scope.IVA < 0) {
            $scope.IVA = 0;
        } 
    };

    $scope.GetTypesPaymentForSale = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/GetTypesPaymentForSale',
            params: {
                idSale: idSale,
            }
        }).
            then(function (response) {
                if (response.data.success == 1) {
                    var suma = 0;
                    if (response.data.oData.TypesPayment.length > 0) {
                        angular.forEach(response.data.oData.TypesPayment, function (value, key) {
                            if (value.HistoryCredit.length > 0) {
                                for (var i = 0; i <= value.HistoryCredit.length - 1; i++) {
                                    suma = suma - value.HistoryCredit[i].Cantidad;
                                }
                                value.amount = value.amount + suma;
                            }
                        });

                        $scope.TypesPayment = response.data.oData.TypesPayment;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);
                    }
                } else if (response.data.failure == 1) {
                    notify(response.data.oData.Error, $rootScope.error);
                } else if (response.data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            });
    };

    $scope.SaveEditSale = function (idVenta) {
        if ($scope.frmLogin.$valid) {
            $("#saveEditSale").button("loading");
            var products = new Array();
            angular.forEach($scope.items, function (value, key) {
                products.push({
                    idDetalleVenta: value.idDetalleVenta,
                    Descuento: parseFloat(value.Descuento)
                });
            });

            var typeCustomer = null;

            switch ($scope.type) {
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
                url: '../../../Sales/SaveEditHeaderSale',
                data: {
                    idVenta: idVenta,
                    idUsuario1: $scope.sellerOne.idUsuario,
                    idUsuario2: ($scope.sellerTwo == undefined) ? null : $scope.sellerTwo.idUsuario,
                    idClienteFisico: ($scope.type == "physical") ? $scope.physicalCustomer.idCliente : null,
                    idClienteMoral: ($scope.type == "moral") ? $scope.moralCustomer.idCliente : null,
                    idDespacho: ($scope.type == "office") ? $scope.officeCustomer.idDespacho : null,
                    Proyecto: $scope.project,
                    TipoCliente: typeCustomer,
                    idDespachoReferencia: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null,
                    Subtotal: $scope.subTotal,
                    Descuento: ($scope.checkedDiscount == true) ? parseFloat($scope.discount) : 0,
                    IVA: ($scope.checkedIVA == true) ? 1 : 0,
                    Total: $scope.total,
                    oDetail: products
                }
            }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $("#modalEditSale").modal("hide");   
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }

                $("#saveEditSale").button("reset");                
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
                $("#saveEditSale").button("reset");
            });
        }
    }
});

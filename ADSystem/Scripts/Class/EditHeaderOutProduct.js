angular.module("General").controller('EditViewController', function ($scope, $http, $window, notify, $rootScope) {

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

    $scope.SaveEditView = function (idVista) {

        if ($scope.frmLogin.$valid) {

            $("#saveEditView").button("loading");

            var products = new Array();            

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
                url: '../../../OutProducts/SaveEditHeaderView',
                data: {
                    idVista: idVista,
                    idUsuario1: $scope.sellerOne.idUsuario,
                    idUsuario2: ($scope.sellerTwo == undefined) ? null : $scope.sellerTwo.idUsuario,
                    idClienteFisico: ($scope.type == "physical") ? $scope.physicalCustomer.idCliente : null,
                    idClienteMoral: ($scope.type == "moral") ? $scope.moralCustomer.idCliente : null,
                    idDespacho: ($scope.type == "office") ? $scope.officeCustomer.idDespacho : null,
                    Proyecto: $scope.project,
                    TipoCliente: typeCustomer,
                    idDespachoReferencia: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null                    
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        notify(data.oData.Message, $rootScope.success);

                        window.location = "../../../OutProducts/ListOutProducts";

                    } else if (data.failure == 1) {

                        notify(data.oData.Error, $rootScope.error);

                    } else if (data.noLogin == 1) {

                        window.location = "../../../Access/Close"

                    }

                    $("#saveEditView").button("reset");

                }).
                error(function (data, status, headers, config) {

                    notify("Ocurrío un error.", $rootScope.error);

                    $("#saveEditView").button("reset");

                });

        }
    }
});

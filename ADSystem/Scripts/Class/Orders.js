angular.module("General").controller('OrdersController', function (models, orderValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.selectedOrder = "";
    $scope.selectedBill = "";
    $scope.selectedProvider = "";
    $scope.selectedCodigo = "";

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    $scope.searchType = 1;
    $scope.status = 1;
    $scope.currentCoin = 1;
    //$scope.maxDate = moment(new Date.now()).format('YYYY-MM-DD')
    //var today = new Date().toISOString().slice(0, 10).toString();
    var today = new Date();
    $scope.maxDate = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + (today.getDate() + 1);

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

        $scope.listOrders();
        $scope.coinList();

    });

    $scope.LoadCoin = function (typeCoin) {        
        $scope.currentCoin = typeCoin;
    }

    $scope.coinList = function () {        
        $http({
            method: 'GET',
            url: '../../../Orders/ListCoinType',
            params: {
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {
                    if (data.oData.CoinTypes.length > 0) {                        
                        $scope.ListCoinType = data.oData.CoinTypes;
                    } else {                        
                        notify('No se encontraron registros.', $rootScope.error);
                    }
                } else {                   
                    notify('No se encontraron registros.', $rootScope.error);
                }
                    

            }).
            error(function (data, status, headers, config) {                

                notify("Ocurrío un error.", $rootScope.error);

            });
    }

    $scope.listOrders = function () {

        $("#searchOrden").button("loading");

        if ($scope.searchSince != 2) {
            var everything = 4;
            $scope.searchType = everything;
        }

        var status;
        switch ($scope.status) {
            case '1': status = null; // todo
                break;
            case '2': status = 0; // pendiente
                break;
            case '3': status = 1; // liberado
                break;
            default: status = null;
                break;
        }

        $http({
            method: 'POST',
            url: '../../../Orders/ListOrders',
            params: {
                searchType: $scope.searchType,
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                status: status,
                order: ($scope.selectedOrder == "" || $scope.selectedOrder == null) ? "" : $scope.selectedOrder.title,
                bill: ($scope.selectedBill == "" || $scope.selectedBill == null) ? "" : $scope.selectedBill.title,
                brand: ($scope.selectedProvider == "" || $scope.selectedProvider == null) ? "" : $scope.selectedProvider.title,
                code: ($scope.selectedCodigo == "" || $scope.selectedCodigo == null) ? "" : $scope.selectedCodigo.title,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {
                $("#searchOrden").button("reset");

                if (data.success == 1) {

                    if (data.oData.Orders.length > 0) {

                        $scope.Orders = data.oData.Orders;
                        $scope.total = data.oData.Count;

                    } else {

                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.Orders = data.oData.Orders;
                        $scope.total = data.oData.Count;

                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                $("#searchOrden").button("reset");

                notify("Ocurrío un error.", $rootScope.error);

            });
    };

    $scope.valResult = {};

    $scope.newOrder = new models.Order();

    $scope.AddOrder = function () {

        $window.location = '../../Orders/AddOrder?idOrder=' + ID;

    };

    $scope.SaveAddOrder = function () {        

        $scope.valResult = orderValidator.validate($scope.newOrder);

        if (!$scope.order && !$scope.factura) {
            $scope.valResult["errors"][$scope.valResult["errors"].length] = { errorMessage: "El campo Órden ó Factura es requerido.", propertyName: "order/factura" };
            $scope.newOrder.$isValid = false;
        }
        if ($scope.newOrder.$isValid) {

            $scope.SaveAdd();

        }

    };

    $scope.SaveAdd = function () {
        
        $("#SaveAddOrder").button('loading');
        $http({
            method: 'POST',
            url: '../../../Orders/SaveAddOrder',
            params: {
                order: $scope.order,
                factura: $scope.factura,
                idEmpresa: $scope.newOrder.Provider.idProveedor,
                fechaCompra: $scope.datePurchase,
                dollar: $scope.dollar == undefined? 0 : $scope.dollar.replace("$", ""),
                fechaEntrega: $scope.dateDeliver,
                fechaCaptura: $scope.dateCapture,
                idCoin: $scope.currentCoin,
                add: $scope.add,                
            }
        }).
            success(function (data, status, headers, config) {

                $("#SaveAddOrder").button('reset');

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    $window.location = '../../../Orders/Index';

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                $("#SaveAddOrder").button('reset');

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.UpdateOrder = function (ID) {

        $window.location = '../../../Orders/UpdateOrder?idOrder=' + ID;

    };

    $scope.SaveUpdateOrder = function (id) {        

        $scope.valResult = orderValidator.validate($scope.newOrder);

        if (!$scope.order && !$scope.factura) {
            $scope.valResult["errors"][$scope.valResult["errors"].length] = { errorMessage: "El campo Órden ó Factura es requerido.", propertyName: "order/factura" };
            $scope.newOrder.$isValid = false;
        }

        if ($scope.newOrder.$isValid) {

            $scope.SaveUpdate(id);

        }

    };

    $scope.SaveUpdate = function (id) {

        $("#SaveUpdateOrder").button('loading');

        $http({
            method: 'POST',
            url: '../../../Orders/SaveUpdateOrder',
            params: {
                idOrder: id,
                order: $scope.order,
                factura: $scope.factura,
                idEmpresa: $scope.newOrder.Provider.idProveedor,
                fechaCompra: $scope.datePurchase,
                fechaEntrega: $scope.dateDeliver,
                fechaCaptura: $scope.dateCapture,
                idCoin: $scope.currentCoin,
                add: $scope.add
            }
        }).
            success(function (data, status, headers, config) {

                $("#SaveUpdateOrder").button('reset');

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    $window.location = '../../../Orders/Index';

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                $("#SaveUpdateOrder").button('reset');

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.DeleteOrder = function (idOrder) {

        $http({
            method: 'POST',
            url: '../../../Orders/DeleteOrder',
            params: {
                idOrder: idOrder
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    setTimeout(function () {
                        $scope.reloadRoute();
                    }, 2500);

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

    $scope.UpdateOrderStatus = function (idOrder, status) {

        $http({
            method: 'POST',
            url: '../../../Orders/UpdateOrderStatus',
            params: {
                idOrder: idOrder,
                status: (status == 0) ? 1 : 0
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);

                    window.location.reload();
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

    $scope.PrintAllTicketsOfOrder = function (idOrder) {

        var win = window.open('../../products/PrintAllTicketsOfOrder?idOrder=' + idOrder, '_blank');
        win.focus();

    };

    $scope.datePurchase = new Date();

    $scope.dateDeliver = new Date();

    $scope.dateCapture = new Date();

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };

    $scope.toggleMin();

    $scope.openSince = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedSince = true;
        $scope.openedUntil = false;
    };

    $scope.openUntil = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedSince = false;
        $scope.openedUntil = true;
    };

    $scope.openPurchase = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedCapture = false;
        $scope.openedDeliver = false;
        $scope.openedPurchase = true;
    };

    $scope.$watch("datePurchase", function (newValue, oldValue) {

        var prueba = new Date($scope.datePurchase);

        $http({
            method: 'POST',
            url: '../../../Orders/CheckCurrency',
            params: { date: prueba }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.dollar = "$" + parseFloat(data.oData.conversion).toFixed(2);
                } else {                    
                    notify("Ocurrío un error en la conversión del dolar", $rootScope.error);
                }
            }).
            error(function (data, status, headers, config) {                
                notify("Ocurrío un error en la conversión del dolar", $rootScope.error);

            });
    });

    $scope.openDeliver = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedPurchase = false;
        $scope.openedCapture = false;
        $scope.openedDeliver = true;
    };

    $scope.openCapture = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedPurchase = false;
        $scope.openedDeliver = false;
        $scope.openedCapture = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM/dd/yyyy'];
    $scope.format = $scope.formats[4];

    $scope.IndexOrdersProducts = function (idOrder, idProvider) {

        window.location = '../../../Orders/IndexOrdersProducts?idOrder=' + idOrder + '&idProvider=' + idProvider;

    };

    $scope.setSelecProvider = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Enterprise, parseInt(a), 'idProveedor');

        $scope.newOrder.Provider = $scope.Enterprise[index];

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
        for (var i = 0, len = myArray.length; i < len; i++) {
            if (myArray[i][property] === searchTerm) return i;
        }
        return -1;
    };

    $scope.reloadRoute = function () {
        window.location.reload();
    };

});
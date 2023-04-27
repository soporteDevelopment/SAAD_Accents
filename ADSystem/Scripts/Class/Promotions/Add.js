angular.module("General").controller('PromotionsController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.typePromotions = [];
    $scope.products = [];

    $scope.LoadInit = function () {
        $scope.GetTypePromotions();
    },

        $scope.GetTypePromotions = function () {
            $http({
                method: 'GET',
                url: '../../../TypePromotions/Get',
                params: {}
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        $scope.typePromotions = data.oData.TypePromotions;
                    } else if (data.failure == 1) {
                        notify(data.oData.Error, $rootScope.error);
                    } else if (data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    }
                }).
                error(function (data, status, headers, config) {
                    notify("Ocurrío un error.", $rootScope.error);
                    $scope.selectedCode = "";
                });
        };

    //Obtener productos sin promocion activa
    $scope.GetProduct = function () {

        $http({
            method: 'POST',
            url: '../../../Products/GetProduct',
            params: {
                idProduct: ($scope.selectedCode == "" || $scope.selectedCode == null) ? "" : $scope.selectedCode.originalObject.idProducto
            }
        }).
        success(function (data, status, headers, config) {
            if (data.success == 1) {
                var result = _.result(_.find($scope.products, function (chr) {

                    return chr.idProducto == data.oData.Product.idProducto

                }), 'idProducto');

                if (result == undefined) {
                    $scope.products.push({
                        idProducto: data.oData.Product.idProducto,
                        idProveedor: data.oData.Product.idProveedor,
                        imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                        codigo: data.oData.Product.Codigo,
                        desc: data.oData.Product.Descripcion,
                        prec: data.oData.Product.PrecioVenta,                       
                        cantidad: 1,
                    });
                } else {
                    var index = _.indexOf($scope.products, _.find($scope.products, { idProducto: data.oData.Product.idProducto }));
                    $scope.products[index].cantidad++;
                }
            } else if (data.failure == 1) {
                notify(data.oData.Error, $rootScope.error);
            } else if (data.noLogin == 1) {
                window.location = "../../../Access/Close";
            }

            $scope.selectedCode = "";
        }).
        error(function (data, status, headers, config) {
            notify("Ocurrío un error.", $rootScope.error);
            $scope.selectedCode = "";
        });
    };

    $scope.Add = function () {
        var products = new Array();

        angular.forEach($scope.products, function (value, key) {
            products.push({
                idProducto: value.idProducto,
                Cantidad: value.cantidad,
            });
        });

        var promotion = {
            idTipoPromocion: $scope.TypePromotion,
            Descripcion: $scope.Description,
            FechaInicio: $scope.SinceDate,
            FechaFin: $scope.UntilDate,
            Descuento: $scope.Discount,
            Costo: $scope.Cost,
            Active: true,
            DetallePromociones: products
        }

        $http({
            method: 'POST',
            url: '../../../Promotions/Post',
            data: {
                'promotion': promotion
            }
        }).
        success(function (data, status, headers, config) {
            if (data.success == 1) {
                notify(data.oData.Message, $rootScope.success);
                window.location = "../../../Promotions/Index";
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

    $scope.DeleteProduct = function (ID) {
        _.remove($scope.products, function (n) {
            return n.idProducto == ID;
        });
    };

    $scope.Back = function () {
        window.location = "/Home/Index";
    };

    $scope.dateToday = new Date();

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };

    $scope.toggleMin();

    $scope.openSinceDate = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedSinceDate = true;
    };

    $scope.openUntilDate = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedUntilDate = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM-dd-yyyy'];
    $scope.format = $scope.formats[4];

    $scope.FormatDate = function (value) {
        if (!value) return ''
        var date = new Date(parseInt(value.substr(6)));

        return date.getFullYear() + "-" + (((parseInt(date.getMonth() + 1)) < 10) ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1)) + "-" + (((parseInt(date.getDate())) < 10) ? "0" + (date.getDate()) : (date.getDate()));
    };
});

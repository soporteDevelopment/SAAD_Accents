angular.module("General").controller('ProductsController', function (models, ProductValidator, $scope, $http, $window, notify, $rootScope, FileUploader) {

    $scope.rd = 0;

    $scope.Description = "";
    $scope.Codigo = "";
    $scope.Category = "";
    $scope.Color = "";
    $scope.Material = "";
    $scope.selectedProvider = "";
    $scope.includeURL = "";

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.selectedProveedorId = 0;
    $scope.selectedCategoryId = 0;
    $scope.selectedSubcategoryId = 0;
    $scope.selectedMaterialId = 0;
    $scope.productId = 0;
    $scope.imgName = "";
    $scope.imgType = 'file';
    $scope.guidImage = '';
    $scope.itemsToPrint = new Array();
    var isUploaded = false;
    
    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listProducts();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listProducts();
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
            $scope.listProducts();
        }
    };

    $scope.pageInit = function (currentPage, itemsPerPage) {
        $scope.currentPage = currentPage;
        $scope.itemsPerPage = itemsPerPage;
        $scope.listProducts();
    };
    
    $scope.listProducts = function (currentPage) {

        if (currentPage != undefined) {
            $scope.currentPage = currentPage;
        }

        $("#searchProduct").button("loading");

        $http({
            method: 'POST',
            url: '../../../Products/ListProductsPrintBarCode',
            params: {
                idProduct: ($scope.idProduct == "" || $scope.idProduct == null) ? null : $scope.idProduct,
                description: ($scope.Description == "" || $scope.Description == null) ? "" : $scope.Description,
                code: ($scope.Codigo == "" || $scope.Codigo == null) ? "" : $scope.Codigo,
                cost: ($scope.productPrecio == undefined) ? null : $scope.productPrecio,
                category: ($scope.Category == "" || $scope.Category == null) ? "" : $scope.Category.Nombre,
                color: ($scope.Color == "" || $scope.Color == null) ? "" : $scope.Color,
                material: ($scope.Material == "" || $scope.Material == null) ? "" : $scope.Material,
                order: ($scope.Orden == "" || $scope.Orden == null) ? "" : $scope.Orden,
                brand: ($scope.selectedProvider == "" || $scope.selectedProvider == null) ? "" : $scope.selectedProvider.title,
                branch: $scope.branch,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            $("#searchProduct").button("reset");

            if (data.success == 1) {

                if (data.oData.Products.length > 0) {

                    $scope.Products = data.oData.Products;
                    $scope.total = data.oData.Count;

                    $scope.itemsToPrint.length = 0;

                    angular.forEach(data.oData.Products, function (value, key) {

                        $scope.itemsToPrint.push({

                            idProducto: value.idProducto,
                            urlImagen: value.urlImagen,
                            Codigo: value.Codigo,
                            Descripcion: value.Descripcion,
                            Precio: value.PrecioVenta,
                            Proveedor: value.Proveedor,
                            Marca: value.Marca,
                            Copias: 0

                        });

                    });

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Products = data.oData.Products;
                    $scope.total = data.oData.Count;

                }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#searchProduct").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });
    };

    $scope.GetCategories = function () {

        $http({
            method: 'POST',
            url: '../../../Products/GetCategories'
        }).
       success(function (data, status, headers, config) {

           $scope.Categories = data.oData.Category;

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    $scope.setSelecProveedor = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Providers, parseInt(a), 'idProveedor');

        $scope.newProduct.ProveedorId = $scope.Providers[index];

    };

    $scope.setSelecCategory = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Categories, parseInt(a), 'idCategoria');

        $scope.newProduct.CategoryId = $scope.Categories[index];

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
        for (var i = 0, len = myArray.length; i < len; i++) {
            if (myArray[i][property] === searchTerm) return i;
        }
        return -1;
    };

    $scope.printProducts = function () {

        var products = "";

        angular.forEach($scope.itemsToPrint, function (value, key) {

            if (value.Copias > 0) {

                var element = "";

                element = value.idProducto + "," + value.Copias;

                if (products.length == 0) {

                    products = element;

                } else {

                    products = products + "|" + element;

                }
            }

        });

        var win = window.open('../../products/PrintTicketsProducts?lProducts=' + products, '_blank');
        win.focus();

    },

    $scope.printAllProducts = function () {

        var win = window.open('../../products/PrintAllTicketsProducts?amount=' + $scope.numberAllProduct, '_blank');
        win.focus();

    }

});

jQuery.fn.reset = function () {
    $(this).each(function () { this.reset(); });
}
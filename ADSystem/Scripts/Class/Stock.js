angular.module("General").controller('StockController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.itemsPerPage = 40;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.invZero = false;
    $scope.invStoZero = false;

    $scope.itemsPerPageStock = 40;
    $scope.currentPageStock = 0;
    $scope.totalStock = 0;

    $scope.idProduct = null;
    $scope.branch = "";
    $scope.IDBranch = 0;
    $scope.idStock = 1;

    $scope.amountNegatives = 0;
    $scope.amountPositives = 0;
    $scope.amountOldStock = 0;
    $scope.amountNewStock = 0;
    $scope.costOldStock = 0;
    $scope.costNewStock = 0;

    //Codigo para el scaner
    angular.element(document).ready(function () {
        angular.element(document).keydown(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);

            if (code == 13)// Enter key hit
            {
                $scope.DescriptionStock = "";
                $scope.idProduct = "";
                $scope.CodigoStock = "";
                $scope.productPrecioStock = undefined;
                $scope.CategoryStock = "";
                $scope.ColorStock = "";
                $scope.MaterialStock = "";
                $scope.selectedProviderStock = "";

                $scope.VerifyProduct();
                $scope.barcode = "";
            }
            else {
                $scope.barcode = $scope.barcode + String.fromCharCode(code);
            }
        });
    });

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

    $scope.listStocks = function () {

        $("#searchStock").button("loading");

        $http({
            method: 'POST',
            url: '../../../Stock/GetStocks',
            params: {
                dtDateSince: $scope.dateSince,
                amazonas: $scope.sBranchAma,
                guadalquivir: $scope.sBranchGua,
                textura: $scope.sBranchTex,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            $("#searchStock").button("reset");

            if (data.success == 1) {

                if (data.oData.Stocks.length > 0) {

                    $scope.Stocks = data.oData.Stocks;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Stocks = data.oData.Stocks;
                    $scope.total = data.oData.Count;

                }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#searchStock").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });
    };

    $scope.listProducts = function (page) {

        if (page != undefined) {
            $scope.currentPage = page;
        }

        $("#searchProduct").button("loading");

        $http({
            method: 'POST',
            url: '../../../Products/ListProductsForBranch',
            params: {
                idStock: $scope.idStock,
                branch: ($scope.IDBranch),
                description: ($scope.Description == "" || $scope.Description == null) ? "" : $scope.Description,
                code: ($scope.Codigo == "" || $scope.Codigo == null) ? "" : $scope.Codigo,
                cost: ($scope.productPrecio == undefined) ? null : $scope.productPrecio,
                category: ($scope.Category == "" || $scope.Category == null) ? "" : $scope.Category.Nombre,
                color: ($scope.Color == "" || $scope.Color == null) ? "" : $scope.Color,
                material: ($scope.Material == "" || $scope.Material == null) ? "" : $scope.Material,
                brand: ($scope.selectedProvider == "" || $scope.selectedProvider == null) ? "" : $scope.selectedProvider.title,
                stockZero: $scope.invZero,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage,
                orderASC: ($scope.orderASC == undefined) ? false : $scope.orderASC
            }
        }).
        success(function (data, status, headers, config) {

            $("#searchProduct").button("reset");

            if (data.success == 1) {

                if (data.oData.Products.length > 0) {

                    $scope.Products = data.oData.Products;
                    $scope.total = data.oData.Count;

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

    $scope.prevPageStock = function () {
        if ($scope.currentPageStock > 0) {
            $scope.currentPageStock--;
            $scope.listProductsStock();
        }
    };

    $scope.prevPageDisabledStock = function () {
        return $scope.currentPageStock === 0 ? "disabled" : "";
    };

    $scope.nextPageStock = function () {
        if ($scope.currentPageStock < $scope.pageCountStock() - 1) {
            $scope.currentPageStock++;
            $scope.listProductsStock();
        }
    };

    $scope.nextPageDisabledStock = function () {
        return $scope.currentPageStock === $scope.pageCountStock() - 1 ? "disabled" : "";
    };

    $scope.pageCountStock = function () {
        return Math.ceil($scope.totalStock / $scope.itemsPerPageStock);
    };

    $scope.rangeStock = function () {
        var rangeSize = 5;
        var ret = [];
        var start;
        var result = $scope.pageCountStock();

        if ($scope.pageCountStock() == 0)
            return ret;

        start = $scope.currentPageStock;
        if (start > $scope.pageCountStock() - rangeSize) {
            start = $scope.pageCountStock() - rangeSize;
        }

        for (var i = start; i < start + rangeSize; i++) {
            if (i >= 0) {
                ret.push(i);
            }
        }

        return ret;
    };

    $scope.setPageStock = function (n) {

        var i = $scope.pageCountStock();

        if (n >= 0 && n < $scope.pageCountStock()) {
            $scope.currentPageStock = n;
            $scope.listProductsStock();
        }
    };

    $scope.setPagination = function () {

        $scope.itemsPerPageStock = 20;
        $scope.currentPageStock = 0;
        $scope.totalStock = 0;

    };

    $scope.listProductsStock = function (page) {

        if (page != undefined) {
            $scope.currentPageStock = page;
            $scope.idProduct = null;
        }

        $("#searchProductStock").button("loading");

        var url = '../../../Stock/ListProductsStock';

        if ($scope.difference) {

            url = '../../../Stock/ListProductsStockDif';

        }

        $http({
            method: 'POST',
            url: url,
            params: {
                idStock: $scope.idStock,
                description: ($scope.DescriptionStock == "" || $scope.DescriptionStock == null) ? "" : $scope.DescriptionStock,
                idProduct: ($scope.idProduct == "" || $scope.idProduct == null || $scope.idProduct == 0) ? null : $scope.idProduct,
                code: ($scope.CodigoStock == "" || $scope.CodigoStock == null) ? "" : $scope.CodigoStock,
                cost: ($scope.productPrecioStock == undefined) ? null : $scope.productPrecioStock,
                category: ($scope.CategoryStock == "" || $scope.CategoryStock == null) ? "" : $scope.CategoryStock.Nombre,
                color: ($scope.ColorStock == "" || $scope.ColorStock == null) ? "" : $scope.ColorStock,
                material: ($scope.MaterialStock == "" || $scope.MaterialStock == null) ? "" : $scope.MaterialStock,
                brand: ($scope.selectedProviderStock == "" || $scope.selectedProviderStock == null) ? "" : $scope.selectedProviderStock.title,
                stockZero: $scope.invStoZero,
                page: $scope.currentPageStock,
                pageSize: $scope.itemsPerPageStock
            }
        }).
        success(function (data, status, headers, config) {

            $("#searchProductStock").button("reset");

            if (data.success == 1) {

                if (data.oData.Products.length > 0) {

                    $scope.ProductsStock = data.oData.Products;
                    $scope.totalStock = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.ProductsStock = data.oData.Products;
                    $scope.totalStock = data.oData.Count;

                }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close";

            }

        }).
        error(function (data, status, headers, config) {

            $("#searchProductStock").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });
    };

    $scope.onInit = function () {

        $scope.GetCategories();

        setTimeout(function () {
            $scope.openModalSetBanch();
        }, 0);

        $scope.listProducts();

    };

    $scope.onInitUpdate = function (idStock, idBranch) {

        $scope.idStock = idStock;
        $scope.IDBranch = idBranch;

        $scope.GetCategories();

        $scope.listProducts();

        $scope.GetInformationStock($scope.idStock, $scope.IDBranch);

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

    $scope.openModalSetBanch = function () {

        $("#modalSetBranch").modal("show");

    };

    $scope.StartStock = function () {

        $scope.branch = "";

        if ($scope.IDBranch == 2) {

            $scope.branch = "RÍO AMAZONAS";

        } else if ($scope.IDBranch == 3) {

            $scope.branch = "RÍO GUADALQUIVIR";

        } else {

            $scope.branch = "TEXTURA";

        }

        $http({
            method: 'POST',
            url: '../../../Stock/AddStock',
            params: {
                idSucursal: $scope.IDBranch,
                FechaInicio: $scope.dateStart,
                FechaFin: $scope.dateStart,
                Estatus: 1,
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                $scope.idStock = data.oData.idStock;

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

        $("#modalSetBranch").modal("hide");

    };

    $scope.openModalPrintBarCode = function (idProduct) {

        $scope.idProduct = idProduct;

        $("#modalPrintBarcode").modal("show");

    };

    $scope.PrintBarCode = function () {

        var products = $scope.idProduct + "," + $scope.amountImpressions;

        var win = window.open('../../products/PrintTicketsProducts?lProducts=' + products, '_blank');
        win.focus();

        $("#modalPrintBarcode").modal("hide");

    };

    $scope.VerifyProduct = function (product) {
        var res = $scope.barcode.match(/^\d{12}$/);

        if (($scope.barcode.length == 12) && (res.length > 0)) // Enter key hit
        {
            $http({
                method: 'POST',
                url: '../../../Stock/VerifyProduct',
                params: {
                    idStock: $scope.idStock,
                    idBranch: $scope.IDBranch,
                    idProduct: res
                }
            }).success(function (data, status, headers, config) {
                if (data.success == 1) {

                    $scope.DescriptionStock = "";
                    $scope.idProduct = data.oData.idProduct;
                    $scope.CodigoStock = "";
                    $scope.productPrecioStock = null;
                    $scope.CategoryStock = "";
                    $scope.ColorStock = "";
                    $scope.MaterialStock = "";
                    $scope.selectedProviderStock = "";
                    $scope.currentPageStock = 0;

                    $scope.listProductsStock();
                    $scope.GetInformationStock();

                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close"
                }
            }).error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
        }
    };

    $scope.UpdateProductStock = function (product) {

        $http({
            method: 'POST',
            url: '../../../Stock/UpdateProductStock',
            params: {
                idProductosInventario: product.idProductosInventario,
                idInventario: $scope.idStock,
                idSucursal: $scope.IDBranch,
                idProducto: product.idProducto,
                CantidadAnterior: product.CantidadAnterior,
                CantidadActual: product.CantidadActual,
                Precio: product.PrecioVenta,
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.listProductsStock();
               $scope.GetInformationStock();

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

    $scope.RefreshProduct = function (product) {

        $http({
                method: 'POST',
                url: '../../../Stock/RefreshProduct',
                params: {
                    idInventario: $scope.idStock,
                    idSucursal: $scope.IDBranch,
                    idProducto: product.idProducto,
                    CantidadAnterior: 0,
                    CantidadActual: product.Cantidad,
                    Precio: product.PrecioVenta,
                }
            }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify("Artículo contabilizado", $rootScope.success);

                    $scope.listProductsStock();
                    $scope.GetInformationStock();
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

    $scope.GetInformationStock = function () {

        $http({
            method: 'POST',
            url: '../../../Stock/GetInformationStock',
            params: {
                idStock: $scope.idStock,
                idBranch: $scope.IDBranch
            }
        }).
       success(function (data, status, headers, config) {

           $scope.amountNegatives = data.oData.InformationStock.AmountNegatives;
           $scope.amountPositives = data.oData.InformationStock.AmountPositives;
           $scope.amountOldStock = data.oData.InformationStock.AmountOldStock;
           $scope.amountNewStock = data.oData.InformationStock.AmountNewStock;
           $scope.costOldStock = data.oData.InformationStock.CostOldStock;
           $scope.costNewStock = data.oData.InformationStock.CostNewStock;

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    $scope.GetInformationStockList = function (idStock, idBranch) {

        $http({
            method: 'POST',
            url: '../../../Stock/GetInformationStock',
            params: {
                idStock: idStock,
                idBranch: idBranch
            }
        }).
       success(function (data, status, headers, config) {

           $scope.amountNegatives = data.oData.InformationStock.AmountNegatives;
           $scope.amountPositives = data.oData.InformationStock.AmountPositives;
           $scope.amountOldStock = data.oData.InformationStock.AmountOldStock;
           $scope.amountNewStock = data.oData.InformationStock.AmountNewStock;
           $scope.costOldStock = data.oData.InformationStock.CostOldStock;
           $scope.costNewStock = data.oData.InformationStock.CostNewStock;

           $("#modalDetailStock").modal("show");

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    $scope.GetCompleteInformationStock = function (idStock) {

        $http({
            method: 'POST',
            url: '../../../Stock/GetCompleteInformationStock',
            params: {
                idStock: idStock
            }
        }).
       success(function (data, status, headers, config) {

           $scope.amountNegatives = data.oData.InformationStock.AmountNegatives;
           $scope.amountPositives = data.oData.InformationStock.AmountPositives;
           $scope.amountOldStock = data.oData.InformationStock.AmountOldStock;
           $scope.amountNewStock = data.oData.InformationStock.AmountNewStock;
           $scope.costOldStock = data.oData.InformationStock.CostOldStock;
           $scope.costNewStock = data.oData.InformationStock.CostNewStock;

           $("#modalDetailStock").modal("show");

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    $scope.SaveStock = function () {

        $scope.exportDifference();

        var r = confirm("Está seguro que desea actualizar el inventario?");

        if (r == true) {

            $http({
                method: 'POST',
                url: '../../../Stock/UpdateStock',
                params: {
                    idInventario: $scope.idStock,
                    idSucursal: $scope.IDBranch,
                    DiferenciasNegativas: $scope.amountNegatives,
                    DiferenciasPositivas: $scope.amountPositives,
                    ArticulosInventarioAnterior: $scope.amountOldStock,
                    ArticulosInventarioActual: $scope.amountNewStock,
                    CostoInventarioAnterior: $scope.costOldStock,
                    CostoInventarioActual: $scope.costNewStock,
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    window.location = "../../../Stock/List"

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

    $scope.UpdateProduct = function (ID) {

        $window.location = '../../Stock/UpdateProduct?idProduct=' + ID + '&idStock=' + $scope.idStock + '&idBranch=' + $scope.IDBranch;

    };

    $scope.dateToday = new Date();

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };

    $scope.toggleMin();

    $scope.openStart = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedStart = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        minMode: 'month',
        startingDay: 1
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM/dd/yyyy'];
    $scope.format = $scope.formats[4];

    $scope.getDate = function (date) {

        var res = "";

        if (date.length > 10) {

            res = date.substring(6, 19);

        }

        return res;

    };

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

    $scope.openPaymentDate = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedPaymentDate = true;
    };

    $scope.continueStock = function (idStock, idSucursal) {

        $window.location = '../../Stock/ContinueStock?idStock=' + idStock + '&idBranch=' + idSucursal;

    };

    $scope.exportDifference = function () {

        window.location = "../../../Stock/ExportDifference?idStock=" + $scope.idStock;

    };

    $scope.exportCountingProducts = function () {

        var idStock = $scope.idStock;
        var branch = ($scope.IDBranch);
        var description = ($scope.Description == "" || $scope.Description == null) ? "" : $scope.Description;
        var code = ($scope.Codigo == "" || $scope.Codigo == null) ? "" : $scope.Codigo;
        var cost = ($scope.productPrecio == undefined) ? null : $scope.productPrecio;
        var category = ($scope.Category == "" || $scope.Category == null) ? "" : $scope.Category.Nombre;
        var color = ($scope.Color == "" || $scope.Color == null) ? "" : $scope.Color;
        var material = ($scope.Material == "" || $scope.Material == null) ? "" : $scope.Material;
        var brand = ($scope.selectedProvider == "" || $scope.selectedProvider == null) ? "" : $scope.selectedProvider.title;
        var page = $scope.currentPage;
        var pageSize = $scope.itemsPerPage;
        var orderASC = ($scope.orderASC == undefined) ? false : $scope.orderASC;

        var URL = "../../../Stock/ExportCountingProducts?idStock=" + idStock + "&branch=" + branch + "&description=" + description + "&code=" + code + "&cost=" + cost + "&category=" + category + "&color=" + color + "&material=" + material + "&brand=" + brand + "&page=" + page + "&pageSize=" + pageSize + "&orderASC=" + orderASC;

        var win = window.open(URL, '_blank');
        win.focus();
    };

    $scope.ZoomImage = function (urlImage) {
        $scope.ZoomUrlImage = urlImage;

        $("#modalZoomImage").modal("show");
    }

    $scope.PrintMissingProducts = function (idStock) {

        var URL = '../../../Stock/PrintMissingProducts?idStock=' + idStock;

        var win = window.open(URL, '_blank');
        win.focus();

    };

});
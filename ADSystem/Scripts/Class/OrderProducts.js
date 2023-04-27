angular.module("General").controller('OrderProductsController', function (models, productOrderValidator, OrderProductValidator, UpdateOrderProductValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.rd = 0;

    $scope.valResult = {};

    $scope.newProductOrder = new models.ProductOrder();

    $scope.newProductOrder.selectedCodigo = "";

    $scope.newProduct = 0;
    $scope.idProductoOrder = 0;
    $scope.productId = 0;
    $scope.imgName = "";
    $scope.guidImage = "";
    $scope.newProduct.Code = 0;
    $scope.idOrder = 0;
    $scope.idProvider = 0;

    $scope.selectedCategoryId = 0;

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    var arch = new FileReader();
    $scope.myFiles = [];
    $scope.imgType = 'file';
    var isUploaded = false;

    $scope.SetValues = function (idOrder, idProvider) {
        $scope.idOrder = idOrder;
        $scope.idProvider = idProvider;
        $scope.newProductOrder.branchQuantityAm = 0;
        $scope.newProductOrder.branchQuantityGua = 0;
        $scope.newProductOrder.branchQuantityTex = 0;
        $scope.newOrderProducts.ProductbranchQuantityAm = 0;
        $scope.newOrderProducts.ProductbranchQuantityGua = 0;
        $scope.newOrderProducts.ProductbranchQuantityTex = 0;
    }

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listOrderProducts();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listOrderProducts();
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
            $scope.listOrderProducts();
        }
    };

    $scope.$watch("currentPage", function (newValue, oldValue) {
        $scope.ListProviders();
        $scope.GetCategories();
        $scope.GetMaterials();
    });

    $scope.$watch('imgType', function (imgType) {
        if (imgType == "file") {
            $("#file-content").show();
            $("#url-content").hide();
        }
        if (imgType == "url") {
            $("#file-content").hide();
            $("#url-content").show();
        }
    });

    $scope.iniAddProduct = function () {
        inicio();
    }

    $scope.listOrderProducts = function (currentPage) {

        if (currentPage != undefined) {
            $scope.currentPage = currentPage;
        }

        $http({
            method: 'POST',
            url: '../../../Orders/ListOrdersProducts',
            params: {
                idOrder: $scope.idOrder,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                if (data.oData.Orders.length > 0) {

                    $scope.OrderProducts = data.oData.Orders;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.OrderProducts = data.oData.Orders;
                    $scope.total = data.oData.Count;

                }

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

    $scope.IndexOrdersProducts = function (ID) {

        $window.location = '../../Orders/IndexOrdersProducts?idOrder=' + ID + '&idProvider=' + $scope.idProvider;

    };

    $scope.AddOrderProduct = function () {

        window.location = '../../Orders/AddOrderProduct';

    };

    $scope.valResult = {};

    $scope.newProductOrder = new models.ProductOrder();

    $scope.SaveAddOrderProduct = function (ID) {

        $scope.newProductOrder.branchQuantityAm = ($scope.newProductOrder.branchQuantityAm!=null)?$scope.newProductOrder.branchQuantityAm.toString():0;
        $scope.newProductOrder.branchQuantityGua = ($scope.newProductOrder.branchQuantityGua !=null) ? $scope.newProductOrder.branchQuantityGua.toString():0;
        $scope.newProductOrder.branchQuantityTex = ($scope.newProductOrder.branchQuantityTex != null) ?$scope.newProductOrder.branchQuantityTex.toString():0;

        $scope.valResult = productOrderValidator.validate($scope.newProductOrder);

        if ($scope.newProductOrder.$isValid) {
            var quantity = parseInt($scope.newProductOrder.quantity)
            var sumAllquantity = parseInt($scope.newProductOrder.branchQuantityAm) + parseInt($scope.newProductOrder.branchQuantityGua) + parseInt($scope.newProductOrder.branchQuantityTex);

            if (quantity < sumAllquantity) {
                $scope.valResult["errors"][$scope.valResult["errors"].length] = { errorMessage: "La cantidad adquirida es menor a la suma de las sucursales.", propertyName: "quantity" };
                $scope.newProductOrder.$isValid = false;
            }
            if (quantity > sumAllquantity) {
                $scope.valResult["errors"][$scope.valResult["errors"].length] = { errorMessage: "La cantidad adquirida es mayor a la suma de las sucursales.", propertyName: "quantity" };
                $scope.newProductOrder.$isValid = false;
            }
        }

        if ($scope.newProductOrder.$isValid) {

            $scope.SaveAddExistente(ID);
        }

    };

    $scope.SaveAddExistente = function (ID) {

        $("#SaveAddOrderProduct").button('loading');

        $http({
            method: 'POST',
            url: '../../../Orders/SaveAddOrderProduct',
            params: {
                idOrder: ID,
                idProduct: ($scope.newProductOrder.selectedCodigo == "" || $scope.newProductOrder.selectedCodigo == null) ? "" : $scope.newProductOrder.selectedCodigo.originalObject.idProducto,
                cantidad: $scope.newProductOrder.quantity,
                precioCompra: $scope.newProductOrder.purchasePrice,
                precioVenta: $scope.newProductOrder.salePrice,
                existenciaAmazonas: $scope.newProductOrder.branchQuantityAm,
                existenciaGuadalquivir: $scope.newProductOrder.branchQuantityGua,
                existenciaTextura: $scope.newProductOrder.branchQuantityTex
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveAddOrderProduct").button('reset');

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                window.location = '../../../Orders/IndexOrdersProducts?idOrder=' + ID + '&idProvider=' + $scope.idProvider;

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveAddOrderProduct").button('reset');

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.UpdateOrderProduct = function (idOrder, idProduct) {

        $window.location = '../../../Orders/UpdateOrderProduct?idOrder=' + idOrder + '&idProduct=' + idProduct;

    };

    $scope.SaveUpdateOrderProduct = function (idOrder, idProduct) {
        
        $scope.valResult = UpdateOrderProductValidator.validate($scope);
        if ($scope.$isValid) {

            var ProductbranchQuantityAm = parseInt($scope.lBranches[0].Cantidad.toString());
            var ProductbranchQuantityGua = parseInt($scope.lBranches[1].Cantidad.toString());
            var ProductbranchQuantityTex = parseInt($scope.lBranches[2].Cantidad.toString());
            if (isNaN(ProductbranchQuantityAm)) {
                ProductbranchQuantityAm = 0;
            }
            if (isNaN(ProductbranchQuantityGua)) {
                ProductbranchQuantityGua = 0;
            }
            if (isNaN(ProductbranchQuantityTex)) {
                ProductbranchQuantityTex = 0;
            }
            var quantity = parseInt($scope.quantity);
            var sumAllquantity = ProductbranchQuantityAm + ProductbranchQuantityGua + ProductbranchQuantityTex;
            if (quantity < sumAllquantity) {
                $scope.valResult["errors"][0] = { errorMessage: "La cantidad adquirida es menor a la suma de las sucursales.", propertyName: "quantity" };                
                $scope.$isValid = false;
            }
            if (quantity > sumAllquantity) {
                $scope.valResult["errors"][0] = { errorMessage: "La cantidad adquirida es mayor a la suma de las sucursales.", propertyName: "quantity" };                
                $scope.$isValid = false;
            }
        }
        
        if ($scope.$isValid) {
            $("#SaveUpdateOrderProduct").button("loading");
            $http({
                method: 'POST',
                url: '../../../Orders/SaveUpdateOrderProduct',
                data: {
                    idOrder: idOrder,
                    idProduct: idProduct,
                    upProduct: $scope.product,
                    nameProducto: $scope.nameProduct.replace(/'/g, ''),
                    cantidad: $scope.quantity,
                    precioCompra: $scope.purchasePrice,
                    precioVenta: $scope.salePrice,
                    lCantidadProductos: $scope.lBranches
                }
            }).
            success(function (data, status, headers, config) {

                $("#SaveUpdateOrderProduct").button("reset");

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    window.location = '../../../Orders/IndexOrdersProducts?idOrder=' + idOrder + '&idProvider=' + $scope.idProvider;

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                $("#SaveUpdateOrderProduct").button("reset");

                notify("Ocurrío un error.", $rootScope.error);

            });
        }

    };

    $scope.setSelecProduct = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Views, parseInt(a), 'idProducto');

        $scope.product = $scope.Views[index];

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {

        for (var i = 0, len = myArray.length; i < len; i++) {
            if (myArray[i][property] === searchTerm) return i;
        }
        return -1;

    };

    $scope.newOrderProducts = new models.OrderProducts();

    $scope.SaveAdd = function () {

        $scope.newOrderProducts.ProductbranchQuantityAm = ($scope.newOrderProducts.ProductbranchQuantityAm != null) ? $scope.newOrderProducts.ProductbranchQuantityAm.toString() : 0;
        $scope.newOrderProducts.ProductbranchQuantityGua = ($scope.newOrderProducts.ProductbranchQuantityGua != null)? $scope.newOrderProducts.ProductbranchQuantityGua.toString() : 0;
        $scope.newOrderProducts.ProductbranchQuantityTex = ($scope.newOrderProducts.ProductbranchQuantityTex != null)? $scope.newOrderProducts.ProductbranchQuantityTex.toString() : 0;
        $scope.valResult = OrderProductValidator.validate($scope.newOrderProducts);

        if ($scope.newOrderProducts.$isValid) {
            var quantity = parseInt($scope.newOrderProducts.Productquantity)
            var sumAllquantity = parseInt($scope.newOrderProducts.ProductbranchQuantityAm) + parseInt($scope.newOrderProducts.ProductbranchQuantityGua) + parseInt($scope.newOrderProducts.ProductbranchQuantityTex);
            if (quantity < sumAllquantity) {
                $scope.valResult["errors"][$scope.valResult["errors"].length] = {
                    errorMessage: "La cantidad adquirida es menor a la suma de las sucursales.", propertyName: "quantity"
                };
                $scope.newOrderProducts.$isValid = false;
            }
            if (quantity > sumAllquantity) {
                $scope.valResult["errors"][$scope.valResult["errors"].length] = {
                    errorMessage: "La cantidad adquirida es mayor a la suma de las sucursales.", propertyName: "quantity"
                };
                $scope.newOrderProducts.$isValid = false;
            }
        }
        var timems = 0;
        if ($scope.myFiles.length != null && $scope.myFiles.length != 0 && $scope.myFiles.length != undefined && $scope.newOrderProducts.$isValid) {
            $scope.SaveUploadImg();
        } else {
            if ($scope.newOrderProducts.$isValid) {
                $scope.SaveAddProduct();
            }            
        }
       

    };

    $scope.SaveAddProduct = function () {

        $("#AddProduct").button('loading');

        $http({
            method: 'POST',
            url: '../../../Products/SaveAddProduct',
            params: {
                Name: $scope.Name.replace(/'/g, ''),
                ComercialName: "",
                Description: ($scope.Description == undefined) ? "" : $scope.Description.replace(/'/g, ''),
                SalePrice: $scope.newOrderProducts.SalePrice,
                ProveedorId: $scope.idProvider,
                CategoryId: $scope.newOrderProducts.CategoryId.idCategoria,
                SubCategoryId: ($scope.SubcategoryId == undefined) ? null : $scope.SubcategoryId.idSubcategoria,
                Color: $scope.Color,
                MaterialId: ($scope.MaterialId == 0 || $scope.MaterialId == undefined) ? null : $scope.MaterialId.idMaterial,
                Measure: $scope.Measure,
                Weight: ($scope.Weight == undefined) ? null : $scope.Weight,
                Code: $scope.newOrderProducts.Code,
                Comments: $scope.Comments,
                Filename: $scope.guidImage,
                urlImage: $("#imgUrl").val(),
                ImgType: $scope.imgType,
                Extension: $scope.extension
            }
        }).
        success(function (data, status, headers, config) {

            $("#AddProduct").button('reset');

            if (data.success == 1) {

                $scope.idProductoOrder = data.oData.idProducto,
                $scope.quantity = $scope.newOrderProducts.Productquantity,
                $scope.purchasePrice = $scope.newOrderProducts.ProductpurchasePrice,
                $scope.salePrice = $scope.newOrderProducts.SalePrice,
                $scope.branchQuantityAm = $scope.newOrderProducts.ProductbranchQuantityAm,
                $scope.branchQuantityGua = $scope.newOrderProducts.ProductbranchQuantityGua
                $scope.branchQuantityTex = $scope.newOrderProducts.ProductbranchQuantityTex

                $scope.SaveAddOrderAddProduct($scope.idOrder)

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#AddProduct").button('reset');

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.SaveAddOrderAddProduct = function (ID) {

        $http({
            method: 'POST',
            url: '../../../Orders/SaveAddOrderProduct',
            params: {
                idOrder: ID,
                idProduct: $scope.idProductoOrder,
                cantidad: $scope.quantity,
                precioCompra: $scope.purchasePrice,
                precioVenta: $scope.salePrice,
                existenciaAmazonas: $scope.branchQuantityAm,
                existenciaGuadalquivir: $scope.branchQuantityGua,
                existenciaTextura: $scope.branchQuantityTex
            }
        }).
        success(function (data, status, headers, config) {

            $("#addProductForm").reset();
            document.getElementById('holder').style.backgroundImage = "none";
            document.getElementById('holderUrl').style.backgroundImage = "none";
            $scope.myFiles = [];

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                window.location = '../../../Orders/IndexOrdersProducts?idOrder=' + ID + '&idProvider=' + $scope.idProvider;

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

    $scope.DeleteProductOrder = function (idOrder, idProduct){

        $http({
            method: 'POST',
            url: '../../../Orders/DeleteProductOrder',
            params: {
                idOrder: idOrder,
                idProduct: idProduct
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $scope.listOrderProducts(idOrder);

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

    $scope.ListProviders = function () {

        $http({
            method: 'POST',
            url: '../../../Providers/ListAllProviders'
        }).
        success(function (data, status, headers, config) {

            $scope.Providers = data.oData.Providers;

        }).
        error(function (data, status, headers, config) {

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

    $scope.GetSubcategories = function () {

        $scope.SubcategoryId = undefined;

        $http({
            method: 'POST',
            url: '../../../Products/GetSubcategories',
            params: {
                idCategory: $scope.newOrderProducts.CategoryId.idCategoria
            }
        }).
       success(function (data, status, headers, config) {

           $scope.Subcategories = data.oData.Subcategories;

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    $scope.GetMaterials = function () {

        $http({
            method: 'POST',
            url: '../../../Products/GetMaterials'
        }).
       success(function (data, status, headers, config) {

           $scope.Materials = data.oData.Materials;

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    function inicio() {
        fileMethod();
        urlMethod();
    }

    function fileMethod() {
        document.getElementById('holder').addEventListener('dragover', permitirDrop, false);
        document.getElementById('holder').addEventListener('drop', drop, false);
    }

    function urlMethod() {

        $("#holderUrl").on('dragover', function (e) {
            e.preventDefault();
            return false;
        });

        $("#holderUrl").on('drop', function (e) {
            e.preventDefault();
            e.originalEvent.dataTransfer.items[0].getAsString(function (url) {
                $("#imgUrl").val(url);
                $("#imgProductUpd").remove();
                document.getElementById('holderUrl').style.backgroundImage = "url('" + url + "')";
            });
        });

    }
    function drop(ev) {
        ev.preventDefault();
        arch.addEventListener('load', leer, false);
        arch.readAsDataURL(ev.dataTransfer.files[0]);
        $scope.myFiles = ev.dataTransfer.files;

        var file = $scope.myFiles.item(0);
        if ('name' in file) {
            $("#imgName").val(file.name);            
        }
        else {
            $("#imgName").val(file.name);
        }

        $scope.rd = 0;
}

    function permitirDrop(ev) {
        ev.preventDefault();
    }

    function leer(ev) {
        $("#imgProduct").remove();
        document.getElementById('holder').style.backgroundImage = "url('" + ev.target.result + "')";
    }

    $scope.SaveUploadImg = function () {
        var formData = new FormData();
        for (var i = 0; i < $scope.myFiles.length; i++) {
            formData.append('file', $scope.myFiles[i]);
        }
        // now post a new XHR request
        if ($scope.myFiles.length == null || $scope.myFiles.length == 0 || $scope.myFiles.length == undefined) {
            $("#msgModalMsg").text("Seleccione la imagen que desea cargar");
            $scope.openModalMsg();
            return;
        }
        var xhr = new XMLHttpRequest();
        xhr.open('POST', "../../products/UploadFile?productId=" + $scope.productId + "&option=ADD" + "&qtyRotates=" + $scope.rd, true);
        xhr.onload = function () {
            var response = JSON.parse(xhr.responseText);
            isUploaded = response.isUploaded;
            if (xhr.readyState == 4 && xhr.status == 200 && isUploaded) {
                notify(response.message, $rootScope.success);
                $scope.guidImage = response.fileName;
                $scope.extension = response.extension;
                if ($scope.newOrderProducts.$isValid) {
                    $scope.SaveAddProduct();
                }
            } else {
                notify(response.message, $rootScope.error);
            }
        };

        xhr.send(formData);
    }

    $scope.openModalMsg = function () {

        $("#modalMsg").modal("show");

    };

    $scope.closeModalMsg = function () {

        $("#msgModalMsg").text("");
        $("#modalMsg").modal("hide");

    }

    $scope.RotateImage = function () {

        if ($scope.myFiles.length > 0) {
            $scope.rd += 1;
            var deg = 90 * $scope.rd;
            $('#holder').css({
                '-webkit-transform': 'rotate(' + deg + 'deg)',  //Safari 3.1+, Chrome  
                '-moz-transform': 'rotate(' + deg + 'deg)',     //Firefox 3.5-15  
                '-ms-transform': 'rotate(' + deg + 'deg)',      //IE9+  
                '-o-transform': 'rotate(' + deg + 'deg)',       //Opera 10.5-12.00  
                'transform': 'rotate(' + deg + 'deg)'          //Firefox 16+, Opera 12.50+  
            });

            if ($scope.rd > 3) {
                $scope.rd = 0;
            }
        }
    }

});

jQuery.fn.reset = function () {
    $(this).each(function () { this.reset(); });
}
angular.module("General").controller('ProductsController', function (models, ProductValidator, ProductEditValidator, ProductOnlineValidator, $scope, $http, $window, notify, $rootScope, FileUploader) {

    $scope.rd = 0;

    $scope.Description = "";
    $scope.Codigo = "";
    $scope.Category = "";
    $scope.Color = "";
    $scope.Material = "";
    $scope.selectedProvider = "";
    $scope.includeURL = "";
    $scope.branch = "";

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
    $scope.orderBy = 3;
    $scope.ascending = true;
    $scope.itemsToPrint = new Array();
    var isUploaded = false;

    var arch = new FileReader();
    $scope.myFiles = [];

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

    $scope.loadFilters = function () {
        $scope.ListProviders();
        $scope.GetCategories();
        $scope.GetSubcategoriesIni($scope.selectedCategoryId);
        $scope.GetMaterials();
    };

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

    $scope.pageInit = function (currentPage, itemsPerPage) {

        $scope.currentPage = currentPage;
        $scope.itemsPerPage = itemsPerPage;

        $scope.listProducts();

    };

    $scope.iniAddProduct = function () {
        inicio();
    }

    $scope.iniUpdProduct = function (getProductId, TipoImagen, guid, extension, urlImage) {
        inicio();
        $scope.productId = getProductId;
        $scope.imgType = (TipoImagen == 1) ? 'file' : 'url';
        $scope.guidImage = guid;
        $('#holder').css('background-image', 'url(' + "../Content/Products/" + guid + extension + ')');
        $('#holderUrl').css('background-image', 'url(' + urlImage + ')');
    };

    $scope.listProducts = function (currentPage) {

        if (currentPage != undefined) {
            $scope.currentPage = currentPage;
        }

        $("#searchProduct").button("loading");

        $http({
            method: 'POST',
            url: '../../../Products/ListProducts',
            params: {
                idProduct: ($scope.idProduct == "" || $scope.idProduct == null) ? null : $scope.idProduct,
                description: ($scope.Description == "" || $scope.Description == null) ? "" : $scope.Description,
                code: ($scope.Codigo == "" || $scope.Codigo == null) ? "" : $scope.Codigo,
                cost: ($scope.productPrecio == undefined) ? null : $scope.productPrecio,
                category: ($scope.Category == "" || $scope.Category == null) ? "" : $scope.Category.Nombre,
                subcategory: ($scope.Subcategory == "" || $scope.Subcategory == null) ? "" : $scope.Subcategory.Nombre,
                color: ($scope.Color == "" || $scope.Color == null) ? "" : $scope.Color,
                material: ($scope.Material == "" || $scope.Material == null) ? "" : $scope.Material,
                brand: ($scope.selectedProvider == "" || $scope.selectedProvider == null) ? "" : $scope.selectedProvider.title,
                branch: $scope.branch,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage,
                orderBy: $scope.orderBy,
                ascending: $scope.ascending
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

    $scope.ShowAlert = function (ID) {

        alert(ID);
    };

    $scope.AddProduct = function () {

        $window.location = '../../Products/AddProduct';

    };

    $scope.valResult = {};

    $scope.newProduct = new models.Products();

    $scope.SaveAdd = function () {

        $scope.valResult = ProductValidator.validate($scope.newProduct);

        if ($scope.myFiles.length != null && $scope.myFiles.length != 0 && $scope.myFiles.length != undefined && $scope.newProduct.$isValid) {
            $scope.SaveUploadImg('ADD');
        } else {
            if ($scope.newProduct.$isValid) {
                $scope.SaveAddProduct();
            }
        }

    };

    $scope.SaveAddProduct = function () {

        $("#AddProduct").button('loading');

        $http({
            method: 'POST',
            url: '../../../Products/SaveAddProduct',
            data: {
                Name: $scope.newProduct.Name,
                ComercialName: "",
                Description: $scope.newProduct.Description,
                BuyPrice: $scope.newProduct.BuyPrice,
                SalePrice: $scope.newProduct.SalePrice,
                ProveedorId: $scope.newProduct.ProveedorId.idProveedor,
                CategoryId: $scope.newProduct.CategoryId.idCategoria,
                SubCategoryId: ($scope.newProduct.SubcategoryId == 0 || $scope.newProduct.SubcategoryId == undefined) ? null : $scope.newProduct.SubcategoryId.idSubcategoria,
                Color: $scope.newProduct.Color,
                MaterialId: ($scope.newProduct.MaterialId == 0 || $scope.newProduct.MaterialId == undefined) ? null : $scope.newProduct.MaterialId.idMaterial,
                Measure: $scope.newProduct.Measure,
                Weight: ($scope.newProduct.Weight == undefined) ? null : $scope.newProduct.Weight,
                Code: $scope.newProduct.Code,
                Comments: $scope.newProduct.Comments,
                Stock: $scope.lBranches,
                Filename: $scope.guidImage,
                urlImage: $('#holderUrl').css('background-image'),
                ImgType: $scope.imgType,
                Extension: $scope.extension
            }
        }).
            success(function (data, status, headers, config) {

                $("#AddProduct").button('reset');

                if (data.success == 1) {

                    $("#addProductForm").reset();
                    document.getElementById('holder').style.backgroundImage = "none";
                    document.getElementById('holderUrl').style.backgroundImage = "none";
                    $scope.myFiles = [];

                    notify(data.oData.Message, $rootScope.success);

                    window.location = '../../../Products/Index';

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

    $scope.UpdateProduct = function (ID) {
        let apiURL = '../../../Transfers/GetByProduct?idProduct=' + ID;
        $http
            .get(apiURL)
            .then(
                res => {
                    if (res.data.success == 1) {
                        if (!res.data.oData.PendingTransfers) {
                            $window.location = '../../Products/UpdateProduct?idProduct=' + ID;
                        } else {
                            notify("Producto con transacción pendiente", $rootScope.error);
                        }
                    } else if (res.data.failure == 1) {
                        notify(res.data.oData.Error, $rootScope.error);
                    } else if (res.data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    };
                },
                msg => {
                    // Error
                    notify(msg.oData.Error, $rootScope.error);
                }
            );
    };

    $scope.SaveUpdate = function () {

        $scope.valResult = ProductEditValidator.validate($scope.newProduct);

        if ($scope.myFiles.length != null && $scope.myFiles.length != 0 && $scope.myFiles.length != undefined && $scope.newProduct.$isValid) {
            $scope.SaveUploadImg('UPD');
        } else {
            if ($scope.newProduct.$isValid) {
                $scope.SaveUpdateProduct();
            }
        }

    };

    $scope.SaveUpdateOnLine = function (ID) {

        $scope.valResult = ProductOnlineValidator.validate($scope.newProduct);

        if ($scope.newProduct.$isValid) {

            $scope.SaveUpdateProductOnLine(ID);

        }

    };

    $scope.SaveUpdateStock = function (ID) {

        $scope.valResult = ProductValidator.validate($scope.newProduct);

        if ($scope.newProduct.$isValid) {

            $scope.SaveUpdateProductStock(ID);

        }

    };

    $scope.SaveUpdateProduct = function () {

        $("#UpdateProduct").button('loading');

        $http({
            method: 'POST',
            url: '../../../Products/SaveUpdateProduct',
            data: {
                idProduct: $scope.productId,
                Name: $scope.newProduct.Name,
                ComercialName: "",
                Description: $scope.newProduct.Description,
                BuyPrice: $scope.newProduct.BuyPrice,
                SalePrice: $scope.newProduct.SalePrice,
                ProveedorId: $scope.newProduct.ProveedorId.idProveedor,
                CategoryId: $scope.newProduct.CategoryId.idCategoria,
                SubCategoryId: ($scope.newProduct.SubcategoryId == 0 || $scope.newProduct.SubcategoryId == undefined) ? null : $scope.newProduct.SubcategoryId.idSubcategoria,
                Color: $scope.newProduct.Color,
                MaterialId: ($scope.newProduct.MaterialId == 0 || $scope.newProduct.MaterialId == undefined) ? null : $scope.newProduct.MaterialId.idMaterial,
                Measure: $scope.newProduct.Measure,
                Weight: ($scope.newProduct.Weight == undefined) ? null : $scope.newProduct.Weight,
                Code: $scope.newProduct.Code,
                Comments: $scope.newProduct.Comments,
                Justify: $scope.newProduct.Justify,
                Stock: $scope.lBranches,
                urlImage: $('#holderUrl').css('background-image'),
                ImgType: $scope.imgType,
                ImageName: $scope.guidImage
            }
        }).
            success(function (data, status, headers, config) {

                $("#UpdateProduct").button('reset');

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    window.location = '../../../Products/Index';

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                $("#UpdateProduct").button('reset');

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.SaveUpdateProductOnLine = function (ID) {
        $scope.valResult = ProductOnlineValidator.validate($scope.newProduct);

        if ($scope.newProduct.$isValid) {

            $("#SaveUpdateProductOnLine").button('loading');

            $http({
                method: 'POST',
                url: '../../../Products/SaveUpdateProductOnLine',
                data: {
                    idProduct: ID,
                    Name: $scope.newProduct.Name,
                    Description: $scope.newProduct.Description,
                    ProveedorId: $scope.newProduct.ProveedorId.idProveedor,
                    SalePrice: $scope.newProduct.SalePrice,
                    Code: $scope.newProduct.Code,
                    Comments: $scope.newProduct.Comments,
                    Justify: $scope.newProduct.Justify,
                    Stock: $scope.newProduct.lBranches
                }
            }).success(function (data, status, headers, config) {

                $("#SaveUpdateProductOnLine").button('reset');

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);
                    window.location = '../../../Products/Index';


                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).error(function (data, status, headers, config) {

                $("#UpdateProduct").button('reset');

                notify("Ocurrío un error.", $rootScope.error);

            });
        }
    };

    $scope.SaveUpdateProductStock = function (ID, idStock, idBranch) {

        $("#SaveUpdateProductStock").button('loading');

        $http({
            method: 'POST',
            url: '../../../Products/SaveUpdateProduct',
            data: {
                idProduct: ID,
                Name: $scope.newProduct.Name,
                ComercialName: "",
                Description: $scope.newProduct.Description,
                BuyPrice: $scope.newProduct.BuyPrice,
                SalePrice: $scope.newProduct.SalePrice,
                ProveedorId: $scope.newProduct.ProveedorId.idProveedor,
                CategoryId: $scope.newProduct.CategoryId.idCategoria,
                SubCategoryId: ($scope.newProduct.SubcategoryId == 0 || $scope.newProduct.SubcategoryId == undefined) ? null : $scope.newProduct.SubcategoryId.idSubcategoria,
                Color: $scope.newProduct.Color,
                MaterialId: ($scope.newProduct.MaterialId == 0 || $scope.newProduct.MaterialId == undefined) ? null : $scope.newProduct.MaterialId.idMaterial,
                Measure: $scope.newProduct.Measure,
                Weight: ($scope.newProduct.Weight == undefined) ? null : $scope.newProduct.Weight,
                Code: $scope.newProduct.Code,
                Comments: $scope.newProduct.Comments,
                Stock: $scope.lBranches,
                urlImage: $("#urlImagen").val(),
                ImgType: $scope.imgType,
                ImageName: $scope.guidImage
            }
        }).
            success(function (data, status, headers, config) {

                $("#SaveUpdateProductStock").button('reset');

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    window.location = '../../../Stock/ContinueStock?idStock=' + idStock + '&idBranch=' + idBranch;

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }

            }).
            error(function (data, status, headers, config) {

                $("#SaveUpdateProductStock").button('reset');

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.UpdateProductOnLine = function (ID) {
        let apiURL = '../../../Transfers/GetByProduct?idProduct=' + ID;

        $http
            .get(apiURL)
            .then(
                res => {
                    if (res.data.success == 1) {
                        if (!res.data.oData.PendingTransfers) {
                            $scope.includeURL = "UpdateProductOnLine?idProduct=" + ID;
                            $("#modalUpdateProductOnLine").modal("show");
                        } else {
                            notify("Producto con transacción pendiente", $rootScope.error);
                        }
                    } else if (res.data.failure == 1) {
                        notify(res.data.oData.Error, $rootScope.error);
                    } else if (res.data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    };
                },
                msg => {
                    // Error
                    notify(msg.oData.Error, $rootScope.error);
                }
            );
    };

    $scope.ValidateUpdateStatusProducto = function (ID, Estatus) {
        let apiURL = '../../../Transfers/GetByProduct?idProduct=' + ID;
        $http
            .get(apiURL)
            .then(
                res => {
                    if (res.data.success == 1) {
                        if (!res.data.oData.PendingTransfers) {
                            $scope.UpdateStatusProducto(ID, Estatus);
                        } else {
                            notify("Producto con transacción pendiente", $rootScope.error);
                        }
                    } else if (res.data.failure == 1) {
                        notify(res.data.oData.Error, $rootScope.error);
                    } else if (res.data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    };
                },
                msg => {
                    // Error
                    notify(msg.oData.Error, $rootScope.error);
                }
            );
    };

    $scope.UpdateStatusProducto = function (ID, Estatus) {

        $http({
            method: 'POST',
            url: '../../../Products/UpdateStatusProducto',
            params: {
                idProducto: ID,
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

    $scope.ListProviders = function () {

        $http({
            method: 'POST',
            url: '../../../Providers/ListAllProviders',
            params: {
            }
        }).
            success(function (data, status, headers, config) {
                $scope.Providers = data.oData.Providers;
                if ($scope.selectedProveedorId > 0) {

                    $scope.setSelecProveedor($scope.selectedProveedorId);

                }
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

                if ($scope.selectedCategoryId > 0) {

                    $scope.setSelecCategory($scope.selectedCategoryId);

                }
            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.GetSubcategories = function () {

        $http({
            method: 'POST',
            url: '../../../Products/GetSubcategories',
            params: {
                idCategory: $scope.newProduct.CategoryId.idCategoria
            }
        }).
            success(function (data, status, headers, config) {

                $scope.Subcategories = data.oData.Subcategories;

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.GetSubcategoriesOnList = function () {

        $http({
            method: 'POST',
            url: '../../../Products/GetSubcategories',
            params: {
                idCategory: $scope.Category.idCategoria
            }
        }).
            success(function (data, status, headers, config) {

                $scope.Subcategories = data.oData.Subcategories;

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.GetSubcategoriesUpdate = function () {

        var CategoryId = null;
        $http({
            method: 'POST',
            url: '../../../Products/GetSubcategories',
            params: {
                idCategory: $scope.newProduct.CategoryId.idCategoria
            }
        }).
            success(function (data, status, headers, config) {

                $scope.Subcategories = data.oData.Subcategories;

                if ($scope.selectedSubcategoryId > 0) {

                    $scope.setSelecSubcategorie($scope.selectedSubcategoryId);

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.GetSubcategoriesIni = function (Id) {

        var CategoryId = null;
        $http({
            method: 'POST',
            url: '../../../Products/GetSubcategories',
            params: {
                idCategory: Id
            }
        }).
            success(function (data, status, headers, config) {

                $scope.Subcategories = data.oData.Subcategories;

                if ($scope.selectedSubcategoryId > 0) {

                    $scope.setSelecSubcategorie($scope.selectedSubcategoryId);

                }

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

                if ($scope.selectedMaterialId > 0) {

                    $scope.setSelecMaterial($scope.selectedMaterialId);

                }

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

    $scope.setSelecSubcategorie = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Subcategories, parseInt(a), 'idSubcategoria');

        $scope.newProduct.SubcategoryId = $scope.Subcategories[index];

    };

    $scope.setSelecMaterial = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Materials, parseInt(a), 'idMaterial');

        $scope.newProduct.MaterialId = $scope.Materials[index];

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
        for (var i = 0, len = myArray.length; i < len; i++) {
            if (myArray[i][property] === searchTerm) return i;
        }
        return -1;
    };

    function inicio() {
        fileMethod();
        urlMethod();
    };

    function fileMethod() {
        document.getElementById('holder').addEventListener('dragover', permitirDrop, false);
        document.getElementById('holder').addEventListener('drop', drop, false);
    };

    function urlMethod() {

        $("#holderUrl").on('dragover', function (e) {
            e.preventDefault();
            return false;
        });

        $("#holderUrl").on('drop', function (e) {
            e.preventDefault();
            e.originalEvent.dataTransfer.items[0].getAsString(function (url) {
                document.getElementById('holderUrl').style.backgroundImage = "url('" + url + "')";
            });
        });
    };

    function drop(ev) {
        ev.preventDefault();
        arch.addEventListener('load', leer, false);
        arch.readAsDataURL(ev.dataTransfer.files[0]);
        $scope.myFiles = ev.dataTransfer.files;
        $scope.rd = 0;
    };

    function permitirDrop(ev) {
        ev.preventDefault();
    };

    function leer(ev) {
        document.getElementById('holder').style.backgroundImage = "url('" + ev.target.result + "')";
    };

    $scope.SaveUploadImg = function (option) {

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
        xhr.open('POST', "../../products/UploadFile?productId=" + $scope.productId + "&option=" + option + "&qtyRotates=" + $scope.rd, true);

        xhr.onload = function () {
            var response = JSON.parse(xhr.responseText);
            isUploaded = response.isUploaded;
            if (xhr.readyState == 4 && xhr.status == 200 && isUploaded) {
                notify(response.message, $rootScope.success);
                $scope.guidImage = response.fileName;
                $scope.extension = response.extension;
                if (option == 'ADD') {
                    $scope.SaveAddProduct();
                } else if (option == 'UPD') {
                    $scope.SaveUpdateProduct();
                }

            } else {
                notify(response.message, $rootScope.error);
            }
        };

        xhr.send(formData);
    };

    $scope.openModalMsg = function () {
        $("#modalMsg").modal("show");
    };

    $scope.closeModalMsg = function () {
        $("#msgModalMsg").text("");
        $("#modalMsg").modal("hide");
    };

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

        },

        $scope.ZoomImage = function (urlImage) {
            $scope.ZoomUrlImage = urlImage;

            $("#modalZoomImage").modal("show");
        },

    //Category 
    $scope.productCategory = new models.ProductCategory();

    $scope.UpdateCategoryOnLine = function (id) {
        $scope.includeCategoryURL = "UpdateCategoryOnLine?id=" + id;

        $("#modalUpdateCategoryOnLine").modal("show");
    };

    $scope.SaveUpdateCategoryOnLine = function (id) {
        $("#btnUpdateCategoryOnLine").button('loading');

        $http({
            method: 'PATCH',
            url: '../../../Products/SaveUpdateCategoryOnLine',
            data: {
                id: id,
                categoryId: $scope.productCategory.Category.idCategoria,
                subcategoryId: ($scope.productCategory.Subcategory != null) ? $scope.productCategory.Subcategory.idSubcategoria : null
            }
        }).success(function (data, status, headers, config) {
            $("#btnUpdateCategoryOnLine").button('reset');

            if (data.success == 1) {
                notify(data.oData.Message, $rootScope.success);
                window.location = '../../../Products/Index';
            } else if (data.failure == 1) {
                notify(data.oData.Error, $rootScope.error);
            } else if (data.noLogin == 1) {
                window.location = "../../../Access/Close"
            }
        }).error(function (data, status, headers, config) {
            $("#btnUpdateCategoryOnLine").button('reset');

            notify("Ocurrío un error.", $rootScope.error);
        });
    };

    $scope.InitCategory = function (categoryId, subcategoryId) {
        $scope.Subcategories = [];
        var indexCategory = $scope.arrayObjectIndexOf($scope.Categories, parseInt(categoryId), 'idCategoria');
        $scope.productCategory.Category = $scope.Categories[indexCategory];

        if (categoryId > 0) {
            $scope.GetSubcategoriesForUpdate(subcategoryId);            
        }        
    };

    $scope.GetSubcategoriesForUpdate = function (idSubcategory) {
        $("#btnUpdateCategoryOnLine").button('loading');

        $http({
            method: 'POST',
            url: '../../../Products/GetSubcategories',
            params: {
                idCategory: $scope.productCategory.Category.idCategoria
            }
        }).
        success(function (data, status, headers, config) {
            $("#btnUpdateCategoryOnLine").button('reset');
            $scope.Subcategories = data.oData.Subcategories;

            if (idSubcategory > 0) {
                var indexSubcategory = $scope.arrayObjectIndexOf($scope.Subcategories, parseInt(idSubcategory), 'idSubcategoria');
                $scope.productCategory.Subcategory = $scope.Subcategories[indexSubcategory];
            }

        }).
        error(function (data, status, headers, config) {
            $("#btnUpdateCategoryOnLine").button('reset');
            notify("Ocurrío un error.", $rootScope.error);
        });
    };
});

jQuery.fn.reset = function () {
    $(this).each(function () { this.reset(); });
}
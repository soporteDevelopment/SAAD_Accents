angular.module("General").controller('CatalogsController', function (models, $scope, $http, $window, notify, $rootScope) {
    var arch = new FileReader();
    $scope.myFiles = [];
    $scope.Amazonas = 0;
    $scope.Guadalquivir = 0;
    $scope.Textura = 0;

    $scope.Init = function () {
        $scope.GetProviders();
        $scope.GetCategories();
    };

    $scope.Save = function () {
        if ($scope.frmCatalog.$valid) {

            $("#btnSave").button("loading");

            $http({
                method: 'POST',
                url: '../../../Catalogs/SaveAddCatalog',
                data: {
                    modelo: $scope.model,
                    volumen: $scope.volumen,
                    precio: $scope.price,
                    idProveedor: $scope.idProvider,
                    idCatalogoMarca: $scope.idCatalogBrand,
                    idCategoria: $scope.idCategory,
                    idSubcategoria: $scope.idSubcategory
                }
            }).
                success(function (data, status, headers, config) {
                    $("#btnSave").button("reset");

                    if (data.success == 1) {
                        notify(data.oData.Message, $rootScope.success);
                        $scope.idCatalog = data.oData.idCatalog;
                        $scope.saveBranchCatalogs();
                    } else if (data.failure == 1) {
                        notify(data.oData.Error, $rootScope.error);
                    } else if (data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    }
                }).
                error(function (data, status, headers, config) {
                    $("#btnSave").button("reset");
                    notify("Ocurrío un error.", $rootScope.error);
                });
        }
    };

    $scope.saveBranchCatalogs = function () {
        var branches = [
            {
                idSucursal: 2,
                idCatalogo: $scope.idCatalog,
                Cantidad: $scope.Amazonas
            },
            {
                idSucursal: 3,
                idCatalogo: $scope.idCatalog,
                Cantidad: $scope.Guadalquivir
            },
            {
                idSucursal: 4,
                idCatalogo: $scope.idCatalog,
                Cantidad: $scope.Textura
            }
        ];

        $http({
            method: 'POST',
            url: '../../../BranchCatalogs/SaveAddBranchCatalog',
            data: {
                branchCatalogs: branches
            }
        }).
            success(function (data, status, headers, config) {
                $("#btnSave").button("reset");

                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    if ($scope.myFiles.length != null && $scope.myFiles.length > 0 && $scope.myFiles.length != undefined) {
                        $scope.saveUploadImg();
                    } else {
                        window.location = '../../../Catalogs/Index';
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

    $scope.GetProviders = function () {
        $http({
            method: 'GET',
            url: '../../../CatalogProvider/GetActives',
            params: {
            }
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
            method: 'GET',
            url: '../../../CatalogCategory/GetAll'
        }).
            success(function (data, status, headers, config) {
                $scope.Categories = data.oData.Categories;
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.GetSubcategories = function () {
        $http({
            method: 'GET',
            url: '../../../CatalogSubCategory/GetAll',
            params: {
                idCategory: $scope.idCategory
            }
        }).
            success(function (data, status, headers, config) {
                $scope.Subcategories = data.oData.Subcategories;
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.GetCatalogBrands = function () {
        $("#search").button("loading");

        $http({
            method: 'GET',
            url: '../../../CatalogBrand/GetAll',
            params: {
                idProvider: $scope.idProvider,
                page: 0,
                pageSize: 10000
            }
        }).
            success(function (data, status, headers, config) {
                $("#search").button("reset");

                if (data.success == 1) {
                    if (data.oData.Brands.length > 0) {
                        $scope.brands = data.oData.Brands;
                        $scope.total = data.oData.Count;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.brands = data.oData.Brands;
                        $scope.total = data.oData.Count;
                    }
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).
            error(function (data, status, headers, config) {
                $("#search").button("reset");
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    //Image  
    $scope.fileMethod = function () {
        document.getElementById('holder').addEventListener('dragover', permitirDrop, false);
        document.getElementById('holder').addEventListener('drop', drop, false);
    };

    function drop(ev) {
        ev.preventDefault();
        arch.addEventListener('load', leer, false);
        arch.readAsDataURL(ev.dataTransfer.files[0]);
        $scope.myFiles = ev.dataTransfer.files;
    };

    function permitirDrop(ev) {
        ev.preventDefault();
    };

    function leer(ev) {
        document.getElementById('holder').style.backgroundImage = "url('" + ev.target.result + "')";
    };

    $scope.saveUploadImg = function (option) {

        var formData = new FormData();
        for (var i = 0; i < $scope.myFiles.length; i++) {
            formData.append('file', $scope.myFiles[i]);
        }

        if ($scope.myFiles.length == null || $scope.myFiles.length == 0 || $scope.myFiles.length == undefined) {
            $("#msgModalMsg").text("Seleccione la imagen que desea cargar");
            $scope.openModalMsg();
            return;
        }
        var xhr = new XMLHttpRequest();
        xhr.open('POST', "../../Catalogs/UploadFile?id=" + $scope.idCatalog);

        xhr.onload = function () {
            var response = JSON.parse(xhr.responseText);
            if (response.success == 1) {
                notify(response.oData.Message, $rootScope.success);
                window.location = '../../../Catalogs/Index';
            } else {
                notify(response.oData.Message, $rootScope.error);
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

});

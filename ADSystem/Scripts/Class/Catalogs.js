angular.module("General").controller('CatalogsController', function (models, catalogValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.idSubcategory = 0;
    $scope.selectedCategoryId = 0;
    $scope.selectedSubcategoryId = 0;

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

        $scope.listCatalogs();
        $scope.GetCategories();
        $scope.GetSubcategoriesIni($scope.selectedCategoryId);

    });

    $scope.onLoad = function () {

        $scope.listCatalogs();

    };

    $scope.listCatalogs = function () {

        $http({
            method: 'POST',
            url: '../../../Catalogs/ListCatalogs',
            params: {
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                if (data.oData.Catalogs.length > 0) {

                    $scope.Catalogs = data.oData.Catalogs;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Catalogs = data.oData.Catalogs;
                    $scope.total = data.oData.Count;

                }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

        });
    };

    $scope.valResult = {};

    $scope.newCatalog = new models.Catalog();

    $scope.AddCatalog = function () {

        $window.location = '../../Catalogs/AddCatalog';

    };

    $scope.SaveAddCatalog = function () {

        $scope.valResult = catalogValidator.validate($scope.newCatalog);

        if ($scope.newCatalog.$isValid) {

            $scope.SaveAdd();

        }
    };

    $scope.SaveAdd = function () {

        $("#SaveAddCatalog").button("loading");

        $http({
            method: 'POST',
            url: '../../../Catalogs/SaveAddCatalog',
            params: {
                Name: $scope.newCatalog.Name,
                Volumen: $scope.newCatalog.volumen,
                idCategory: $scope.newCatalog.Category.idCategoria,
                idSubcategory: ($scope.idSubcategory == 0 || $scope.idSubcategory == undefined) ? null : $scope.idSubcategory.idSubcategoria
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveAddCatalog").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $window.location = '../../../Catalogs/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveAddCatalog").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.UpdateCatalog = function (ID) {

        $window.location = '../../Catalogs/UpdateCatalog?idCatalog=' + ID;

    };

    $scope.SaveUpdateCategory = function (ID) {

        $scope.valResult = catalogValidator.validate($scope.newCatalog);

        if ($scope.newCatalog.$isValid) {

            $scope.SaveUpdate(ID);
        }

    },

    $scope.SaveUpdate = function (ID) {

        $("#SaveUpdateCatalog").button("loading");

        $http({
            method: 'POST',
            url: '../../../Catalogs/SaveUpdateCatalog',
            params: {
                idCatalog: ID,
                name: $scope.newCatalog.Name,
                Volumen: $scope.volumen,
                idCategory: $scope.newCatalog.Category.idCategoria,
                idSubcategory: ($scope.idSubcategory == 0 || $scope.idSubcategory == undefined) ? null : $scope.idSubcategory.idSubcategoria
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveUpdateCatalog").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $window.location = '../../../Catalogs/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveUpdateCatalog").button("reset");

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

               $scope.setSelectCategory($scope.selectedCategoryId);

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
                idCategory: $scope.newCatalog.Category.idCategoria
            }
        }).
       success(function (data, status, headers, config) {

           $scope.Subcategories = data.oData.Subcategories;

           if ($scope.selectedSubcategoryId > 0) {

               $scope.setSelecSubcategories($scope.selectedSubcategoryId);

           }

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
                idCategory: $scope.newCatalog.Category.idCategoria
            }
        }).
       success(function (data, status, headers, config) {

           $scope.Subcategories = data.oData.Subcategories;

           if ($scope.selectedSubcategoryId > 0) {

               $scope.setSelecSubcategories($scope.selectedSubcategoryId);

           }

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    $scope.GetSubcategoriesIni = function (Id) {

        $http({
            method: 'POST',
            url: '../../../Products/GetSubcategories',
            data: {
                idCategory: Id
            }
        }).
       success(function (data, status, headers, config) {

           $scope.Subcategories = data.oData.Subcategories;

           if ($scope.selectedSubcategoryId > 0) {

               $scope.setSelecSubcategories($scope.selectedSubcategoryId);

           }

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };
    
    $scope.setSelectCategory = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Categories, parseInt(a), 'idCategoria');

        $scope.newCatalog.Category = $scope.Categories[index];

    };


    $scope.setSelecSubcategories = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Subcategories, parseInt(a), 'idSubcategoria');

        $scope.idSubcategory = $scope.Subcategories[index];

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
        for (var i = 0, len = myArray.length; i < len; i++) {
            if (myArray[i][property] === searchTerm) return i;
        }
        return -1;
    };

});
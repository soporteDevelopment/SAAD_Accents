angular.module("General").controller('CategoriesController', function (models, categoryValidator, $scope, $http, $window, notify, $rootScope) {
    var arch = new FileReader();
    $scope.myFiles = [];

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

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

        $scope.listCategories();

    });

    $scope.onLoad = function () {

        $scope.listCategories();

    };

    $scope.listCategories = function () {
        $("#searchCategories").button("loading");
        $http({
            method: 'POST',
            url: '../../../Categories/ListCategories',
            params: {
                category: $scope.category,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    if (data.oData.Categories.length > 0) {

                        $scope.Categories = data.oData.Categories;
                        $scope.total = data.oData.Count;

                    } else {

                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.Categories = data.oData.Categories;
                        $scope.total = data.oData.Count;

                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }
                $("#searchCategories").button("reset");
            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);
                $("#searchCategories").button("reset");
            });
    };

    $scope.AddCategory = function () {

        $window.location = '../../Categories/AddCategory';

    };

    $scope.valResult = {};

    $scope.newCategory = new models.category();

    $scope.SaveAddCategory = function () {
        $scope.valResult = categoryValidator.validate($scope.newCategory);
        if ($scope.newCategory.$isValid) {
            $scope.SaveAdd();
        }
    };

    $scope.SaveAdd = function () {

        $("#SaveAddCategory").button("loading");

        $http({
            method: 'POST',
            url: '../../../Categories/SaveAddCategory',
            params: {
                Name: $scope.newCategory.name,
                Description: $scope.newCategory.description
            }
        }).
            success(function (data, status, headers, config) {
                $("#SaveAddCategory").button("reset");
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.id = data.oData.idCategory;
                    if ($scope.myFiles.length != null && $scope.myFiles.length != undefined && $scope.myFiles.length > 0) {
                        $scope.SaveUploadImg();
                    } else {
                        $window.location = '../../../Categories/Index';
                    }
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }

            }).
            error(function (data, status, headers, config) {

                $("#SaveAddCategory").button("reset");

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.UpdateCategory = function (ID) {

        $window.location = '../../Categories/UpdateCategory?idCategory=' + ID;

    };

    $scope.SaveUpdateCategory = function (ID) {

        $scope.valResult = categoryValidator.validate($scope.newCategory);
        if ($scope.newCategory.$isValid) {

            $scope.SaveUpdate(ID);

        }

    };

    $scope.SaveUpdate = function (ID) {

        $("#SaveUpdateCategory").button("loading");

        $http({
            method: 'POST',
            url: '../../../Categories/SaveUpdateCategory',
            params: {
                idCategory: ID,
                Name: $scope.newCategory.name,
                Description: $scope.newCategory.description
            }
        }).
            success(function (data, status, headers, config) {
                $("#SaveUpdateCategory").button("reset");
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.id = ID;
                    if ($scope.myFiles.length != null && $scope.myFiles.length != undefined && $scope.myFiles.length > 0) {
                        $scope.SaveUploadImg();
                    } else {
                        $window.location = '../../../Categories/Index';
                    }
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close"
                }
            }).
            error(function (data, status, headers, config) {

                $("#SaveUpdateCategory").button("reset");

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.IndexSubcategories = function (ID) {

        $window.location = "../../../Subcategories/Index?idCategory=" + ID;

    };

    $scope.DeleteCategory = function (ID) {

        $http({
            method: 'POST',
            url: '../../../Categories/DeleteCategory',
            params: {
                idCategory: ID
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                    $scope.listCategories();

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

    //Image  
    $scope.loadImage = function (imagen) {
        $('#holder').css('background-image', 'url(' + "../Content/Categories/" + imagen + ')');
    };

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

    $scope.SaveUploadImg = function (option) {

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
        xhr.open('POST', "../../Categories/UploadFile?id=" + $scope.id);

        xhr.onload = function () {
            var response = JSON.parse(xhr.responseText);
            if (response.success == 1) {
                notify(response.oData.Message, $rootScope.success);
                $window.location = '../../../Categories/Index';
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
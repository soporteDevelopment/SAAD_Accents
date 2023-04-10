angular.module("General").controller('SubcategoriesController', function (models, subcategoryValidator, $scope, $http, $window, notify, $rootScope) {
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

    $scope.listSubcategories = function () {

        $http({
            method: 'POST',
            url: '../../../Subcategories/ListSubcategories',
            params: {
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage,
                idCategory: $scope.idCategory
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                if (data.oData.Subcategories.length > 0) {

                    $scope.Subcategories = data.oData.Subcategories;
                    $scope.total = data.oData.Count;

                } else {

                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Subcategories = data.oData.Subcategories;
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

    $scope.AddSubcategory = function (ID) {

        $window.location = '../../Subcategories/AddSubcategory?idCategory=' + ID;

    };

    $scope.valResult = {};

    $scope.newSubcategory = new models.subcategory();

    $scope.SaveAddSubcategory = function () {

        $scope.valResult = subcategoryValidator.validate($scope.newSubcategory);
        if ($scope.newSubcategory.$isValid) {

            $scope.SaveAdd();

        }

    },

    $scope.SaveAdd = function () {

        $("#SaveAddSubcategory").button("loading");

        $http({
            method: 'POST',
            url: '../../../Subcategories/SaveAddSubcategory',
            params: {
                Name: $scope.newSubcategory.name,
                Description: $scope.newSubcategory.description,
                idCategory: $scope.idCategory
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveAddSubcategory").button("reset");

            if (data.success == 1) {
                notify(data.oData.Message, $rootScope.success);
                $scope.id = data.oData.idSubcategory;
                if ($scope.myFiles.length != null && $scope.myFiles.length != undefined && $scope.myFiles.length > 0) {
                    $scope.SaveUploadImg();
                } else {
                    $window.location = "../../../Subcategories/Index?idCategory=" + $scope.idCategory;
                }
            } else if (data.failure == 1) {
                notify(data.oData.Error, $rootScope.error);
            } else if (data.noLogin == 1) {
                window.location = "../../../Access/Close"
            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveAddSubcategory").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.UpdateSubcategory = function (ID) {

        $window.location = '../../Subcategories/UpdateSubcategory?idSubcategory=' + ID;

    };

    $scope.SaveUpdateSubcategory = function (ID) {

        $scope.valResult = subcategoryValidator.validate($scope.newSubcategory);
        if ($scope.newSubcategory.$isValid) {
            $scope.SaveUpdate(ID);
        }

    },

    $scope.SaveUpdate = function (ID) {

        $("#SaveUpdateSubcategory").button("loading");

        $http({
            method: 'POST',
            url: '../../../Subcategories/SaveUpdateSubcategory',
            params: {
                idSubcategory: ID,
                Name: $scope.newSubcategory.name,
                Description: $scope.newSubcategory.description
            }
        }).
       success(function (data, status, headers, config) {

           $("#SaveUpdateSubcategory").button("reset");

           if (data.success == 1) {
               notify(data.oData.Message, $rootScope.success);
               $scope.id = ID;
               if ($scope.myFiles.length != null && $scope.myFiles.length != undefined && $scope.myFiles.length > 0) {
                   $scope.SaveUploadImg();
               } else {
                   $window.location = "../../../Subcategories/Index?idCategory=" + $scope.idCategory;
               }
           } else if (data.failure == 1) {
               notify(data.oData.Error, $rootScope.error);
           } else if (data.noLogin == 1) {
               window.location = "../../../Access/Close"
           }

       }).
       error(function (data, status, headers, config) {
           $("#SaveUpdateSubcategory").button("reset");
           notify("Ocurrío un error.", $rootScope.error);
       });

    };

    $scope.IndexSubcategories = function (ID) {
        $window.location = "../../../Subcategories/Index?idControl=" + ID;
    };

    $scope.DeleteSubcategory = function (ID) {

        $http({
            method: 'POST',
            url: '../../../Subcategories/DeleteSubcategory',
            params: {
                idSubcategory: ID
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.listSubcategories();
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

    //Image  
    $scope.loadImage = function (imagen) {
        $('#holder').css('background-image', 'url(' + "../Content/Subcategories/" + imagen + ')');
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
        xhr.open('POST', "../../Subcategories/UploadFile?id=" + $scope.id);

        xhr.onload = function () {
            var response = JSON.parse(xhr.responseText);
            if (response.success == 1) {
                notify(response.oData.Message, $rootScope.success);
                $window.location = "../../../Subcategories/Index?idCategory=" + $scope.idCategory;
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
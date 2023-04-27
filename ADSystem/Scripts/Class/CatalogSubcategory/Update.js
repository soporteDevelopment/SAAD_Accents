angular.module("General").controller('SubcategoriesController', function (models, subcategoryValidator, $scope, $http, $window, notify, $rootScope) {
    var arch = new FileReader();
    $scope.myFiles = [];
    $scope.valResult = {};
    $scope.newSubcategory = new models.subcategory();

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
            url: '../../../CatalogSubcategory/SaveUpdateSubcategory',
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
                   $window.location = "../../../CatalogSubcategory/Index?idCategory=" + $scope.idCategory;
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

    $scope.IndexSubcategories = function () {
        $window.location = "../../../CatalogSubcategory/Index?idCategory=" + $scope.idCategory;
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
        xhr.open('POST', "../../CatalogSubcategory/UploadFile?id=" + $scope.id);

        xhr.onload = function () {
            var response = JSON.parse(xhr.responseText);
            if (response.success == 1) {
                notify(response.oData.Message, $rootScope.success);
                $window.location = "../../../CatalogSubcategory/Index?idCategory=" + $scope.idCategory;
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
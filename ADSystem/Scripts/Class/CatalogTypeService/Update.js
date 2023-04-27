angular.module("General").controller('CatalogTypeServiceController', function (models, $scope, $http, $window, notify, $rootScope) {
    var arch = new FileReader();
    $scope.myFiles = [];
    $scope.valResult = {};
   // $scope.newTypeService = new models.typeService();

    $scope.SaveUpdateTypeService = function () {
        $scope.valResult = typeServiceValidator.validate($scope.newTypeService);
        if ($scope.newTypeService.$isValid) {
            $scope.SaveUpdate();
        }
    };

    $scope.SaveUpdate = function () {
        $("#SaveUpdateTypeService").button("loading");
       

        $http({
            method: 'PATCH',
            url: '../../../CatalogTypeService/SaveUpdateTypeService',
            params: {
                idTypeService: $scope.id,
                Description: $scope.description,
                FactorUtilidad: $scope.factorUtilidad
            }
        }).
            success(function (data, status, headers, config) {
                $("#SaveUpdateTypeService").button("reset");
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.id = id;
                    $window.location = '../../../CatalogTypeService/Index';
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close"
                }
            }).
            error(function (data, status, headers, config) {
                $("#SaveUpdateTypeService").button("reset");
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
        xhr.open('POST', "../../CatalogCategory/UploadFile?id=" + $scope.id);

        xhr.onload = function () {
            var response = JSON.parse(xhr.responseText);
            if (response.success == 1) {
                notify(response.oData.Message, $rootScope.success);
                $window.location = '../../../CatalogCategory/Index';
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

    let input = document.getElementById("factorUtilidad");

    input.addEventListener("blur", function () {
        const regex = /^[0-9]*\.?[0-9]*$/;
        const isValid = regex.test(this.value);

        if (!isValid) {
            // Limpiar el valor del input
            $("#factorUtilidad").val('')
        } else {
            this.value = parseFloat(this.value).toFixed(2);

        }
    });
});
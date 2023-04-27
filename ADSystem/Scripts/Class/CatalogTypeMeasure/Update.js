angular.module("General").controller('CatalogTypeMeasureController', function (models, $scope, $http, $window, notify, $rootScope) {
    var arch = new FileReader();
    $scope.myFiles = [];
    $scope.valResult = {};
    console.log(models);
  //  $scope.newTypeMeasure = new models.TypeMeasure();
    //alert();

    //$scope.SaveUpdateTypeMeasure = function () {
    //    console.log("sadfadsf")
    //    $scope.valResult = TypeMeasureValidator.validate($scope.newTypeMeasure);
    //    //if ($scope.newTypeMeasure.$isValid) {
    //        $scope.SaveUpdate();
    //    //}
    //};

    $scope.SaveUpdate = function () {
        $("#SaveUpdateTypeMeasure").button("loading");

        $http({
            method: 'PATCH',
            url: '../../../CatalogTypeMeasure/SaveUpdateTypeMeasure',
            params: {
                idtypemeasure: $scope.id,
                nombremedida: $scope.nombreMedida
            }
        }).
            success(function (data, status, headers, config) {
                $("#saveupdatetypemeasure").button("reset");
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.id = id;
                    $window.location = '../../../CatalogTypeMeasure/index';
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.nologin == 1) {
                    window.location = "../../../access/close"
                }
            }).
            error(function (data, status, headers, config) {
                $("#saveupdatetypemeasure").button("reset");
                notify("ocurrío un error.", $rootscope.error);
            });
    };

    ////Image  
 

    //$scope.fileMethod = function () {
    //    document.getElementById('holder').addEventListener('dragover', permitirDrop, false);
    //    document.getElementById('holder').addEventListener('drop', drop, false);
    //};

    //function drop(ev) {
    //    ev.preventDefault();
    //    arch.addEventListener('load', leer, false);
    //    arch.readAsDataURL(ev.dataTransfer.files[0]);
    //    $scope.myFiles = ev.dataTransfer.files;
    //};

    //function permitirDrop(ev) {
    //    ev.preventDefault();
    //};

    //function leer(ev) {
    //    document.getElementById('holder').style.backgroundImage = "url('" + ev.target.result + "')";
    //};

    $scope.openModalMsg = function () {
        $("#modalMsg").modal("show");
    };

    $scope.closeModalMsg = function () {
        $("#msgModalMsg").text("");
        $("#modalMsg").modal("hide");
    };
});
angular.module("General").controller('CatalogTextilesController', function (models, $scope, $http, $window, notify, $rootScope) {

    $scope.msgErrorsFinal = new Array;

    const CleanInputFile = () => {

        let fileInput = document.getElementById("excel-file");
        fileInput.value = '';
    }

    $scope.SaveAdd = function () {

        $("#upload-btn").button("loading");

        let input = document.getElementById("excel-file");
        let file = input.files[0];
        if (file) {

            reader = new FileReader();
            reader.readAsBinaryString(file);
            reader.onload = function () {
                data = reader.result;
                workbook = XLSX.read(data, { type: "binary" });
                worksheet = workbook.Sheets[workbook.SheetNames[0]];
                json = XLSX.utils.sheet_to_json(worksheet);
                jsonData = JSON.stringify(json);

                $http({
                    method: 'POST',
                    url: '../../../CatalogTextiles/CargarTextiles',
                    data: jsonData
                }).
                    success(function (data, status, headers, config) {
                        $("#upload-btn").button("reset");
                        if (data.success == 1) {
                            notify(data.oData.Message, $rootScope.success);                                 
                            CleanInputFile();
                        } else if (data.failure == 1) {
                            CleanInputFile();
                            $scope.msg = data.oData.Error;
                            const str = $scope.msg.split(".");
                            $scope.msgErrorsFinal = str;
                            $("#openErrorModal").modal("show");
                        } else if (data.noLogin == 1) {
                            window.location = "../../../Access/Close";
                        }
                    }).
                    error(function (data, status, headers, config) {
                        CleanInputFile();
                        $("#upload-btn").button("reset");
                        notify("Ocurrío un error.", $rootScope.error);
                    });
            };
        } else {
            $("#upload-btn").button("reset");

            notify("No se ha seleccionado ningún archivo.", $rootScope.error);
        }


    }
});
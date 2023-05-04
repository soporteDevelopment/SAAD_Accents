angular.module("General").controller('ProviderPreQuotationController', function ($scope, $http, GUID, notify, $rootScope) {

    $scope.providers = new Array();
    $scope.typePdfAttach = 1;
    $scope.typePdfPreQuotation = 2;
    $scope.isVisibleButtonFabrics = false;
    $scope.isVisibleButtonMeasuress = false;

    /*
    Carga inicial con la información del servicio  
    */
    $scope.LoadInformationPrequotation = (detailPrequotation) => {

        const { date, number, oDetail: { measures, Descripcion, fabrics, PDF, Comentarios, idPreCotizacion, Imagen, Cantidad, idPreCotizacionDetalle } } = detailPrequotation;

        if (fabrics?.length) {
            $scope.isVisibleButtonFabrics = true;
        }
        if (measures?.length) {
            $scope.isVisibleButtonMeasures = true;
        }

        $scope.number = number;
        $scope.dateTime = $scope.FormatDate(new Date(date));
        $scope.image = Imagen;
        $scope.amount = Cantidad;
        $scope.description = Descripcion;
        $scope.detailMeasures = measures;
        $scope.detailFabrics = fabrics;
        $scope.comments = Comentarios;
        $scope.pdfAttach = PDF;
        $scope.preQuotationId = idPreCotizacion;
        $scope.preQuotationDetailId = idPreCotizacionDetalle;
    };


    /*
    Formatear fecha de precotización MM/dd/yyyy HH:mm
    */

    $scope.FormatDate = (date) => {

        let month = date.getMonth() + 1; // Obtén el mes (agrega 1 ya que los meses comienzan desde 0)
        let day = date.getDate(); // Obtén el día
        let year = date.getFullYear(); // Obtén el año
        let hour = date.getHours(); // Obtén las horas
        let minutes = date.getMinutes(); // Obtén los minutos

        // Formatea la fecha
        let formattedDate = month.toString().padStart(2, '0') + '/' + day.toString().padStart(2, '0') + '/' + year + ' ' + hour.toString().padStart(2, '0') + ':' + minutes.toString().padStart(2, '0');

        return formattedDate;
    }

    /*
    Información del tipo de servicio de proveedores
    */
    $scope.LoadProviders = function () {

        $http({
            method: 'GET',
            url: '../../../CatalogTypeService/GetAll'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {
                    const { typeServices } = data.oData;
                    $scope.typeServicesList = typeServices;

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

    /*
    Obtiene información de proveedores por el tipo de servicio seleccionado
    */
    $scope.GetProviderByTypeService = () => {
        
        if ($scope.descServiceType) {
            $http({
                method: 'GET',
                url: '../../../Providers/GetProviderByTypeService',
                params: { idTipoServicio: $scope.descServiceType.idTipoServicio }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {
                        const { Providers } = data.oData;
                        $scope.providerList = Providers;

                    } else if (data.failure == 1) {

                        notify(data.oData.Error, $rootScope.error);

                    } else if (data.noLogin == 1) {

                        window.location = "../../../Access/Close";
                    }

                }).
                error(function (data, status, headers, config) {

                    notify("Ocurrío un error.", $rootScope.error);

                });
        }
        //else {
        //    $scope.providerList = "";
        //    $scope.providerList = [];
        //}
    }

    /*
    Muestra el detalle de las medidas
    */
    $scope.OpenMeasuresDetail = () => {

        $("#openModalDetailMeasures").modal('show');

    };

    /*
    Muestra el detalle de las telas
    */
    $scope.OpenFabricsDetail = () => {

        $("#openModalDetailFabrics").modal('show');
    }

    /*
    Descarga de archivos pdf -- typeFile = 1 (adjunto) , typeFile = 2 (pdf precotización)
    */
    $scope.DownloadPdf = (typeFile) => {

        if (typeFile === $scope.typePdfAttach) {

            if ($scope.pdfAttach) {
                window.open($scope.pdfAttach, 'Download');
            } else {
                notify("No existe un archivo para visualizar.", $rootScope.error);
            }

        } else if (typeFile === $scope.typePdfPreQuotation) {

            $http({
                method: 'POST',
                url: '../../../PreQuotations/DownloadPDF',
                params: { idPreQuotation: $scope.preQuotationId },
                responseType: 'arraybuffer'
            }).
                success(function (data, status, headers, config) {
                    const blobFile = new Blob([data], { type: 'application/pdf' });
                    const url = URL.createObjectURL(blobFile);
                    const a = document.createElement('a');
                    a.href = url;
                    a.download = `Precotización.pdf`;;
                    //a.download = `Precotización-Folio ${preQuotationNumber}.pdf`;;
                    document.body.appendChild(a);
                    a.click();
                    document.body.removeChild(a);
                    URL.revokeObjectURL(url);

                }).
                error(function (data, status, headers, config) {
                    notify("Ocurrío un error.", $rootScope.error);
                });
        }
    }

    /*
    Se agregan proveedores al servicio
    */
    $scope.AddProvider = () => {

        if ($scope.descServiceType && $scope.descProvider) {

            //Datos del tipo de servicio seleccionado
            const { idTipoServicio, Descripcion } = $scope.descServiceType;
            //Datos del provedor
            const { Nombre, Telefono, Correo, idProveedor } = $scope.descProvider;

            //validación para que no pueda agregar un registro con el mismo tipo de servicio y proveedor
            if ($scope.providers.length) {
                if ($scope.providers.some((item) => (item.idProveedor == idProveedor && item.idTipoServicio == idTipoServicio))) {
                    notify('Ya existe un registro con el mismo tipo de servicio y proveedor', $rootScope.error);                    
                    return false;
                }
            }

            $scope.providers.push({
                idItemProvider: `${idProveedor}-${GUID}`,
                idPreCotizacionDetalle: $scope.preQuotationDetailId,
                idTipoServicio: idTipoServicio,
                descriptionServiceType: Descripcion,
                idProveedor: idProveedor,
                email: Correo,
                name: Nombre,
                phone: Telefono
            })

            $scope.descServiceType = "";
            $scope.descProvider = "";
            $scope.providerList = "";


        } else {

            notify("Seleccione correctamente la información para poder agregar un proveedor.", $rootScope.error);
        }


    }

    /*
    Se eliminan proveedores 
    */
    $scope.DeleteProvider = (idProvedorItem) => {

        _.remove($scope.providers, function (element) {
            return element.idItemProvider == idProvedorItem;

        });

    }

    /*
    Se envian y se guardan los proveedores seleccionados
    */
    $scope.SendDataProvider = (confirm) => {

        if (confirm) {

            //$scope.sending = true;
            $("#sending").button("loading");
            $http({
                method: 'POST',
                url: '../../../ProviderPreQuotation/AddProviderService',
                data: JSON.stringify($scope.providers),
                params: { idPreQuotation: $scope.preQuotationId }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        notify(data.oData.Message, $rootScope.success, { timeOut: 5000 });

                        setTimeout(function () {
                            window.location = "../../../PreQuotations/ListPreQuotations";
                        }, 1000);

                    } else if (data.failure == 1) {

                        $("#sending").button("reset");
                        notify(data.oData.Error, $rootScope.error);

                    } else if (data.noLogin == 1) {

                        window.location = "../../../Access/Close";
                    }
                }).
                error(function (data, status, headers, config) {

                    notify("Ocurrío un error.", $rootScope.error);
                    $("#sending").button("reset");
                });
        }

    }

});
angular.module("General").controller('ProviderPreQuotationController', function (models, $scope, $http, GUID, notify, $rootScope, $timeout) {

    $scope.isVisibleButtonFabrics = false;
    $scope.isVisibleButtonMeasuress = false;
    $scope.fabricsProvider = new Array;
    $scope.typePdfAttach = 1;
    $scope.typePdfPreQuotation = 2;
    $scope.typePdfGenerateOrder = 3;

    //Inicializa la informacion al cargar la vista. 
    $scope.LoadInformation = (detail) => {
        $scope.number = detail.number;
        $scope.dateTime = $scope.FormatDate(new Date(detail.date));
        $scope.comments = detail.oDetail.comments;
        $scope.detail = detail.oDetail;
        $scope.preQuotationId = detail.oDetail.idPreCotizacion;
        $scope.preQuotationDetailId = detail.oDetail.idPreCotizacionDetalle;
        $scope.detailmeasures = detail.oDetail.measures;
        $scope.detailfabrics = detail.oDetail.fabrics;
        $scope.providers = detail.providers;
       
        //Validacion para mostrar botones de telas y medidas.
        if ($scope.detailfabrics?.length) {
            $scope.isVisibleButtonFabrics = true;
        }

        if ($scope.detailmeasures?.length) {
            $scope.isVisibleButtonMeasures = true;
        }
    };

    ////Actualiza la informacion del proveedor.
    $scope.SelectProvider = () => {
        $("#openModalUpdate").modal("hide");
    }


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

    //Genera el array, con la informacion de proveedores para enviarlo al servicio. 
    $scope.SendDataProvider = () => {

        let data = generateData();

        if (data.length) {
            $http({
                method: 'POST',
                url: '../../../ProviderPreQuotation/AssignProviderPreQuotation',
                data: data
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        notify(data.oData.Message, $rootScope.success, { timeOut: 5000 });

                        setTimeout(function () {
                            window.location = "../../../PreQuotations/ListPreQuotations";
                        }, 2000);

                    } else if (data.failure == 1) {
                        notify(data.oData.Error, $rootScope.error);
                    } else if (data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    }
                }).
                error(function (data, status, headers, config) {
                    notify("Ocurrío un error.", $rootScope.error);
                });
        } else {
            notify("Es necesario que seleccione al menos un proveedor", $rootScope.error);

        }

        
    }

    function generateData() {
        let data = new Array;

        $scope.providers.forEach(p => {
            let check = document.getElementById("check" + p.idProveedor).checked;

            if (check) {
                let array = {
                    "idPreCotDetalleProveedores": p.idPreCotDetalleProveedores,
                    "idPreCotizacionDetalle": p.idPreCotizacionDetalle,
                    "idTipoServicio": p.idTipoServicio,
                    "idProveedor": p.idProveedor,
                    "CostoFabricacion": p.CostoFabricacion,
                    "DiasFabricacion": p.DiasFabricacion,
                    "ComentariosProveedor": p.ComentariosProveedor,
                    "ComentariosComprador": p.ComentariosComprador,
                    "Enviado": p.Enviado,
                    "TelasProveedores": p.TelasProveedores,
                }

                data.push(array);
            } 
        });

        return data;
    }

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
        } else if (typeFile === $scope.typePdfGenerateOrder) {
            $("#btnGenerateOrder").button("loading");

            $http({
                method: 'POST',
                url: '../../../PreQuotations/GenerateOrder',
                params: { idPreQuotationDetail: $scope.preQuotationDetailId, idPreQuotation: $scope.preQuotationId },
                responseType: 'arraybuffer'
            }).
                success(function (data, status, headers, config) {

                    var today = new Date();
                    let time = today.getHours() + "_" + today.getMinutes();

                    const blobFile = new Blob([data], { type: 'application/zip' });
                    const url = URL.createObjectURL(blobFile);
                    const a = document.createElement('a');
                    a.href = url;
                    a.download = "Ordenes_proveedores_" + time + ".zip";
                    //a.download = `Precotización-Folio ${preQuotationNumber}.pdf`;;
                    document.body.appendChild(a);
                    a.click();
                    document.body.removeChild(a);
                    URL.revokeObjectURL(url);
                    notify("Correcto", $rootScope.success, { timeOut: 5000 });

                    $("#btnGenerateOrder").button("reset");

                    setTimeout(function () {
                        window.location = "../../../PreQuotations/ListPreQuotations";
                    }, 2000);
                }).
                error(function (data, status, headers, config) {
                    $("#btnGenerateOrder").button("reset");
                    notify("Ocurrío un error.", $rootScope.error);
                });
        }
    }

    $scope.openModalUpdate = (currentProvider, ident) => {
        $scope.currentProviderId = currentProvider.idPreCotDetalleProveedores;
        $scope.cost = currentProvider.CostoFabricacion;
        $scope.days = currentProvider.DiasFabricacion;
        $scope.commentProvider = currentProvider.ComentariosProveedor;
        $scope.commentSeller = currentProvider.ComentariosComprador
        InitializeFabricData(currentProvider.TelasProveedores,ident );

        $('.disable').attr('disabled', true);
        $("#openModalUpdate").modal("show");
    }

    function InitializeFabricData(telas, ident) {
        if (telas) {
            telas.forEach(function (item, index) {                
                document.getElementById("fabric" + item.idTextiles).innerHTML = item.ValorMts;
                let nameFabric;
                let precioByMt;
               
                const dataFabric = $scope.detailfabrics.filter((ele) => ele.idTextiles === item.idTextiles);

                if (dataFabric) {

                    nameFabric = dataFabric[0].NombreTextiles
                    precioByMt = dataFabric[0].CostoPorMts

                }

                if (ident == 1) {
                    if (item.ValorMts != null || item.ValorMts != undefined) {


                        document.getElementById("label" + item.idTextiles).innerHTML = nameFabric;
                        document.getElementById("precio" + item.idTextiles).innerHTML = precioByMt;

                    } else {
                        document.getElementById("label" + item.idTextiles).innerHTML = "";
                        document.getElementById("precio" + item.idTextiles).innerHTML = "";


                    }
                }
                
                
                if (item.ValorMts != null || item.ValorMts != undefined) {
                    // Busca el item que coincida con el idTextiles y obtiene el costo, para realizar el calculo
                    let currentFabric = $scope.detailfabrics.filter(x => x.idTextiles == item.idTextiles);
                    let CostoPorMts = currentFabric[0].CostoPorMts;

                    let calculate = (item.ValorMts * CostoPorMts);
                    setValueMtsTotal(item.idTextiles, calculate);
                } else {
                    setValueMtsTotal(item.idTextiles, null);
                }

            });
        } else {
            setValueMtsTotal(null, null)
        }
    }

    function setValueMtsTotal(id, total) {        
        if (id != null) {
            if (total != null) {
                document.getElementById("inputFabric" + id).innerHTML = total.toLocaleString('es-MX', {
                    style: 'currency',
                    currency: 'MXN',
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2,
                });
            } else {
                $scope.detailfabrics.forEach(function (item) {
                    document.getElementById("inputFabric" + id).innerHTML = null;
                });
            }
        } else {
            $scope.detailfabrics.forEach(f => {
                document.getElementById("inputFabric" + f.idTextiles).innerHTML = null;
            });
        }
    }

    //Muestra el detalle de las medidas    
    $scope.OpenMeasuresDetail = () => {
        $("#openModalDetailMeasures").modal('show');
    };

    //Muestra el detalle de las telas
    $scope.OpenFabricsDetail = () => {
        $("#openModalDetailFabrics").modal('show');
    }

    //Muestra el detalle de las telas para agregar costo
    $scope.OpenModalDetailFabricsCost = (detail) => {

        $scope.detailCost = detail.Telas;

        $("#openModalDetailFabricsCost").modal('show');
    }
});
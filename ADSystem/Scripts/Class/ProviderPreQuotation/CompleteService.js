angular.module("General").controller('ProviderPreQuotationController', function (models, $scope, $http, GUID, notify, $rootScope, $timeout) {

    $scope.isVisibleButtonFabrics = false;
    $scope.isVisibleButtonMeasuress = false;
    $scope.fabricsProvider = new Array;
    $scope.typePdfAttach = 1;
    $scope.typePdfPreQuotation = 2;

    //Inicializa la informacion al cargar la vista. 
    $scope.LoadInformation = (detail) => {

        $scope.number = detail.number;
        $scope.dateTime = $scope.FormatDate(new Date(detail.date));
        $scope.comments = detail.oDetail.comments;
        $scope.detail = detail.oDetail;
        $scope.preQuotationId = detail.oDetail.idPreCotizacion;
        $scope.detailmeasures = detail.oDetail.measures;
        $scope.detailfabrics = detail.oDetail.fabrics;
        $scope.providers = detail.providers;

        console.log("detail.providers", detail.providers);

        //$scope.providers.forEach(p => {
        //    p.Telas = $scope.detailfabrics;
        //});

        //Validacion para mostrar botones de telas y medidas.
        if ($scope.detailfabrics?.length) {
            $scope.isVisibleButtonFabrics = true;
        }

        if ($scope.detailmeasures?.length) {
            $scope.isVisibleButtonMeasures = true;
        }
    };

    //Actualiza la informacion del proveedor.
    $scope.UpdateProviderData = () => {

        $scope.providers.forEach((item) => {
            if (item.idPreCotDetalleProveedores == $scope.currentProviderId) {
                item.CostoFabricacion = $scope.cost;
                item.DiasFabricacion = $scope.days;
                item.ComentariosProveedor = $scope.commentProvider;
                item.ComentariosComprador = $scope.commentSeller;
                item.TelasProveedores = SaveFabrics(item.TelasProveedores)
            }
        });

        $scope.CostoPorMts = null;
        $scope.fabricsProvider = new Array;
        setValueMtsTotal(null, null);

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
    //Genera array, con datos de las telas del proveedor. Inicializa el input de metros.
    function SaveFabrics(fabrics) {
        fabrics.forEach(function (item) {

            let value = document.getElementById("fabric" + item.idTextiles).value;

            //item.idPreCotProveedoresTelas 0,
            //item.idPreCotDetalleProveedores: 23,
            //item.idServicio: 1,
            //item.idProveedor: 529,
            //item.idTextiles: 4,
            item.ValorMts = value;

            //let obj = {
            //    idPreCotProveedoresTelas: 0,
            //    idPreCotDetalleProveedores: 23,
            //    idServicio: 1,
            //    idProveedor: 529,
            //    idTextiles: 4,
            //    ValorMts: 2
            //}
            //let obj = {
            //    idTextilesGuid: item.idTextiles + '-' + GUID.New(),
            //    idTextiles: item.idTextiles,
            //    NombreTextiles: item.NombreTextiles,
            //    CostoPorMts: item.CostoPorMts,
            //    ValorMts: (value.replace("$", "").replace(",", "") * 1)
            //}

            //$scope.fabricsProvider.push(obj);

            document.getElementById("fabric" + item.idTextiles).value = null;
        });

        return fabrics;
    }

    //Genera el array, con la informacion de proveedores para enviarlo al servicio. 
    $scope.SendDataProvider = () => {

        let data = generateData();

        console.log("DATA", data);

        $http({
            method: 'POST',
            url: '../../../ProviderPreQuotation/ComplementProvider',
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
    }

    function generateData() {
        let data = new Array;

        $scope.providers.forEach(p => {

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
                //"TelasProveedores": CostFabricsArray(p),
            }

            data.push(array);

        });

        return data;
    }

    //Genera el array de telas, que se une al array de proveedor para ser enviado al servicio
    function CostFabricsArray(provider) {
        let array = new Array;

        if (provider.TelasProveedores != undefined) {
            if (provider.TelasProveedores.length) {
                provider.TelasProveedores.forEach(f => {
                    let obj = {
                        "idPreCotProveedoresTelas": null,
                        "idPreCotDetalleProveedores": provider.idPreCotDetalleProveedores,
                        "idServicio": provider.idTipoServicio,
                        "idProveedor": provider.idProveedor,
                        "idTextiles": f.idTextiles,
                        "ValorMts": f.ValorMts
                    };

                    array.push(obj);
                });
            }
        }

        return array;
    }

    //Valida input numerico. 
    $scope.ValidateDays = (event) => {
        // Agregar un evento "onkeydown" al campo de entrada para permitir solo números y un punto decimal
        const codigoTecla = event.keyCode;
        const esNumero = (codigoTecla >= 48 && codigoTecla <= 57) || (codigoTecla >= 96 && codigoTecla <= 105);

        if (!esNumero && codigoTecla !== 8 && codigoTecla !== 46 && codigoTecla !== 37 && codigoTecla !== 39 && codigoTecla !== 9) {
            event.preventDefault();
        }
    }

    $scope.CalculateMts = (event, item) => {
        let value = event.target.value;

        //const regex = /^[0-9]*\.?[0-9]*$/;
        const regex = /^\d*\.?\d*$/;

        if (value == ".") {
            $("#fabric" + item.idTextiles).val('');
            return false;
        }

        if (regex.test(value)) {
            // El valor es válido, no hacer nada
            let calculate = (value * item.CostoPorMts);          
            document.getElementById("inputFabric" + item.idTextiles).innerHTML = calculate.toLocaleString('es-MX', {
            style: 'currency',
            currency: 'MXN',
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        });
        } else {
            // El valor no es válido, eliminar el último carácter ingresado
            $("#fabric" + item.idTextiles).val('');
            event.target.value = value.slice(0, -1);
        }      
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
        }
    }

    $scope.openModalUpdate = (currentProvider) => {
        $scope.currentProviderId = currentProvider.idPreCotDetalleProveedores;
        $scope.cost = currentProvider.CostoFabricacion;
        $scope.days = currentProvider.DiasFabricacion;
        $scope.commentProvider = currentProvider.ComentariosProveedor;
        $scope.commentSeller = currentProvider.ComentariosComprador
        InitializeFabricData(currentProvider.TelasProveedores);

        $("#openModalUpdate").modal("show");
    }

    function InitializeFabricData(telas) {
        console.log("telas", telas);
        if (telas) {
            telas.forEach(function (item) {
                document.getElementById("fabric" + item.idTextiles).value = item.ValorMts;

                console.log("item.ValorMts", item.ValorMts);
                if (item.ValorMts != null || item.ValorMts != undefined) {
                    console.log("$scope.detailfabrics", $scope.detailfabrics);
                    console.log("item.idTextiles", item.idTextiles);
                    // Busca el item que coincida con el idTextiles y obtiene el costo, para realizar el calculo
                    let currentFabric = $scope.detailfabrics.filter(x => x.idTextiles == item.idTextiles);
                    let CostoPorMts = currentFabric[0].CostoPorMts;
                    console.log("currentFabric", currentFabric);
                    console.log("CostoPorMts", CostoPorMts);

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
        console.log("id", id);
        console.log("total", total);
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
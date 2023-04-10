angular.module("General")
    .controller('ReportsController', function (models, $scope, $http, $window, notify, $rootScope, $filter) {

        $scope.itemsPerPage = 500;
        $scope.currentPage = 0;
        $scope.total = 0;
        $scope.comisiones = {};

        $scope.prevPage = function () {
            if ($scope.currentPage > 0) {
                $scope.currentPage--;
                $scope.GenerarReporteVentas();
            }
        };

        $scope.prevPageDisabled = function () {
            return $scope.currentPage === 0 ? "disabled" : "";
        };

        $scope.nextPage = function () {
            if ($scope.currentPage < $scope.pageCount() - 1) {
                $scope.currentPage++;
                $scope.GenerarReporteVentas();
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
                $scope.GenerarReporteVentas();
            }
        };

        $scope.setSelecBranch = function (a, b) {

            var index = $scope.arrayObjectIndexOf($scope.Branches, a, b);

            $scope.Branch = $scope.Branches[index];

        };

        $scope.GenerarReporteVentas = function (a, b) {

            $scope.opened = false;
            if (a != undefined && b != undefined) {
                $scope.currentPage = 0;
                $scope.itemsPerPage = 10;
            }

            $("#txtValidation").empty();

            if (($scope.dateSince == "") || ($scope.dateUntil == "")) {

                $("#txtValidation").append("Seleccione un rango de fechas para la búsqueda  </br>");
                $("#modalValidation").modal("show");

            } else {

                $("#searchReport").button("loading");

                $http({
                    method: 'POST',
                    url: '../../../Intelligence/GetSalesFoSpecialReport',
                    params: {
                        dateSince: $scope.dateSince,
                        dateUntil: $scope.dateUntil,
                        amazonas: $scope.sBranchAma,
                        guadalquivir: $scope.sBranchGua,
                        textura: $scope.sBranchTex,
                        page: $scope.currentPage,
                        pageSize: $scope.itemsPerPage
                    }
                }).
                success(function (data, status, headers, config) {

                    $("#searchReport").button("reset");

                    if (data.success == 1) {

                        if (data.oData.Sales != undefined) {

                            $scope.Sales = data.oData.Sales;
                            $scope.total = data.oData.Count;
                            $scope.CalculateTotal();
                            $scope.GetCommission();

                        } else {

                            notify('No se encontraron registros.', $rootScope.error);
                        }

                    } else if (data.failure == 1) {

                        notify('No se encontraron registros.', $rootScope.error);

                    } else if (data.noLogin == 1) {

                        window.location = "../../../Access/Close"

                    }

                }).
               error(function (data, status, headers, config) {

                   $("#searchReport").button("reset");

                   notify("Ocurrío un error.", $rootScope.error);

               });


            }
        };

        $scope.CalculateTotal = function () {

            var notacredito = 0;

            $scope.comisiones.TotalVentasBruto = 0;

            //TotalVentasBruto
            angular.forEach($scope.Sales, function (value, key) {
                if (!value.Omitir && !value.Compartida) {
                    $scope.comisiones.TotalVentasBruto = $scope.comisiones.TotalVentasBruto + value.ImportePagadoCliente;
                } else if (!value.Omitir && value.Compartida) {
                    $scope.comisiones.TotalVentasBruto = $scope.comisiones.TotalVentasBruto + (value.ImportePagadoCliente / 2);
                }
            });

            //TotalVentasNeto
            var totalsinIVA = 0;
            var totalservicios = 0;
            var totaltarjetacredito = 0;

            angular.forEach($scope.Sales, function (value, key) {

                if (!value.Omitir && !value.Compartida) {

                    angular.forEach(value.listTypePayment, function (payment, paymentkey) {
                        totalsinIVA = totalsinIVA + payment.amount;
                    });

                    //Quitar Total Pagado con Nota de Crédito        
                    angular.forEach(value.listTypePayment, function (payment, paymentkey) {

                        //No es crédito y está saldada
                        if (payment.typesPayment == 8 && payment.Estatus == 1) {

                            notacredito = notacredito + payment.amount;

                        } else if (payment.typesPayment == 2) {

                            //Si es crédito se obtiene el historial 
                            if (payment.HistoryCredit != undefined) {

                                angular.forEach(payment.HistoryCredit, function (history, historykey) {
                                    //Está saldado
                                    if (history.idFormaPago == 8 && history.Estatus == 1) {
                                        notacredito = notacredito + history.Cantidad;
                                    }

                                });
                            }

                        }

                    });

                    var deletePayment = 0.0;

                    //Se eliminan servicios de instalacion y flete
                    angular.forEach(value.oDetail, function (product, key) {
                        if (product.idServicio == 1 || product.idServicio == 11
                            || product.idServicio == 25 || product.idServicio == 60 || product.idServicio == 61) {
                            deletePayment = (product.Precio * product.Cantidad);
                        }
                    });

                    totalsinIVA = totalsinIVA - deletePayment;
                }
            });

            totalsinIVA = totalsinIVA - notacredito;

            $scope.comisiones.TotalVentasNeto = totalsinIVA;

            //TotalSaldado
            var totalsaldado = 0;

            angular.forEach($scope.Sales, function (value, key) {
                if (!value.Omitir && !value.Compartida) {
                    //Está saldada
                    if (value.Estatus == 1) {
                        angular.forEach(value.listTypePayment, function (payment, paymentkey) {
                            //No es crédito y está saldada
                            if (payment.typesPayment != 2 && payment.Estatus == 1) {
                                totalsaldado = totalsaldado + payment.amount;
                            } else if (payment.typesPayment == 2) {
                                //Si es crédito se obtiene el historial 
                                if (payment.HistoryCredit != undefined) {
                                    angular.forEach(payment.HistoryCredit, function (history, historykey) {
                                        //Está saldado
                                        if (history.Estatus == 1) {
                                            totalsaldado = totalsaldado + history.Cantidad;
                                        }
                                    });
                                }
                            }
                        });

                        var deletePayment = 0.0;

                        //Se eliminan servicios de instalacion y flete
                        angular.forEach(value.oDetail, function (product, key) {
                            if (product.idServicio == 1 || product.idServicio == 11
                                || product.idServicio == 25 || product.idServicio == 60 || product.idServicio == 61) {
                                deletePayment = (product.Precio * product.Cantidad);
                            }
                        });

                        totalsaldado = totalsaldado - deletePayment;
                    }
                }
            });

            $scope.comisiones.TotalSaldado = totalsaldado - totalservicios;

            //TotalPendientePorSaldar
            $scope.comisiones.TotalPendiente = $scope.comisiones.TotalVentasNeto - $scope.comisiones.TotalSaldado;

            $scope.GetCommissionForSeller();

        };

        //Obtiene el porcentaje de comisiones para el vendedor
        $scope.GetCommissionForSeller = function () {

            $http({
                method: 'POST',
                url: '../../../Intelligence/GetSpecialCommissionsForSeller',
                params: {
                    idSeller: 41,
                    total: $scope.comisiones.TotalVentasNeto
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.Bonos = [];
                    $scope.porcentajecomision = 0;
                    $scope.comisiones.ComisionTotal = 0;
                    $scope.comisiones.BonoUno = 0;
                    $scope.comisiones.BonoDos = 0;
                    $scope.comisiones.BonoTres = 0;

                    $scope.comisiones.PendientePagoBonos = ($scope.comisiones.BonoUno + $scope.comisiones.BonoDos + $scope.comisiones.BonoTres);

                    if (data.oData.Commissions != null) {

                        $scope.porcentajecomision = (data.oData.Commissions.PorcentajeComision / 100);
                        $scope.comisiones.ComisionTotal = ($scope.comisiones.TotalVentasNeto * (data.oData.Commissions.PorcentajeComision / 100));
                        $scope.comisiones.BonoUno = data.oData.Commissions.BonoUno;
                        $scope.comisiones.BonoDos = data.oData.Commissions.BonoDos;
                        $scope.comisiones.BonoTres = data.oData.Commissions.BonoTres;

                        $scope.comisiones.PendientePagoBonos = ($scope.comisiones.BonoUno + $scope.comisiones.BonoDos + $scope.comisiones.BonoTres);

                    } else {

                        notify("Sin comisiones", $rootScope.error);

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

        $scope.GetCommission = function () {

            $http({
                method: 'POST',
                url: '../../../Intelligence/GetSpecialCommissions',
                params: {
                    date: $scope.dateSince,
                    idSeller: 41
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.CommissionsPayment = data.oData.Commissions;
                    $scope.comisiones.ComisionPagada = data.oData.CommissionPayment;
                    $scope.Bonos = data.oData.Bonos;

                    if ($scope.Bonos.length > 0) {

                        var bonos = _.sumBy($scope.Bonos, function (o) { return o.Cantidad });
                        $scope.comisiones.PendientePagoBonos = $scope.comisiones.PendientePagoBonos - bonos;
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

        },

        $scope.CalculatePayment = function () {

            $scope.amountPayment = 0;
            var notacredito = 0;
            var totalsinIVA = 0;

            angular.forEach($scope.Sales, function (value, key) {

                var servicios = 0;

                if (value.Pagar) {

                    if (!value.Omitir && !value.Compartida) {

                        //Servicios que se omiten   
                        angular.forEach(value.oDetail, function (service, servicekey) {

                            if (service.idServicio > 0 && service.Omitir == true) {

                                servicios = servicios + ((service.Cantidad * service.Precio) - ((service.Cantidad * service.Precio) * (service.Descuento / 100)));

                            } else if (service.idProducto > 0 && service.Omitir == true) {

                                servicios = servicios + ((service.Cantidad * service.Precio) - ((service.Cantidad * service.Precio) * (service.Descuento / 100)));

                            } else if (service.idCredito > 0) {

                                servicios = servicios + ((service.Cantidad * -1) * service.Precio);

                            }

                        });

                        var subtotal = value.ImporteVenta - servicios;

                        //Quitar Total Pagado con Nota de Crédito   
                        angular.forEach(value.listTypePayment, function (payment, paymentkey) {

                            //No es crédito y está saldada
                            if (payment.typesPayment == 8 && payment.Estatus == 1) {

                                notacredito = notacredito + payment.amount;

                            } else if (payment.typesPayment == 2) {

                                //Si es crédito se obtiene el historial 
                                if (payment.HistoryCredit != undefined) {

                                    angular.forEach(payment.HistoryCredit, function (history, historykey) {

                                        //Está saldado
                                        if (history.idFormaPago == 8 && history.Estatus == 1) {
                                            notacredito = notacredito + history.Cantidad;
                                        }

                                    });
                                }

                            }

                        });

                        //Quitar el Descuento 
                        if (value.IntDescuento > 0) {

                            totalsinIVA = totalsinIVA + (subtotal - (subtotal * (value.IntDescuento / 100)));

                        } else {

                            totalsinIVA = totalsinIVA + subtotal;

                        }
                    }
                }
            });

            totalsinIVA = totalsinIVA - notacredito;

            $scope.totalSinIVA = totalsinIVA;
            $scope.amountPayment = (totalsinIVA * $scope.porcentajecomision).toFixed(2);

        }

        $scope.AddCommission = function () {
            
            $http({
                method: 'POST',
                url: '../../../Intelligence/AddSpecialCommission',
                data: {
                    idVendedor: 41,
                    TotalNetoVendido: $scope.totalSinIVA,
                    PorcentajeComision: ($scope.porcentajecomision * 100),
                    FormaPago: $scope.typePayment,
                    Cantidad: $scope.amountPayment,
                    Concepto: $scope.concept,
                    Fecha: $scope.dateSince,
                    FechaPago: $scope.datePayment,
                    Detalle: $scope.detail
                }
            }).
               success(function (data, status, headers, config) {

                   if (data.success == 1) {

                       notify(data.oData.Message, $rootScope.success);

                       $scope.GenerarReporteVentas();

                       $scope.concept = null;
                       $scope.typePayment = 0;
                       $scope.amountPayment = 0;
                       $scope.detail = null;

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

        $scope.init = function (detail) {

            $scope.items = detail;

        };

        $scope.exportCommision = function (idCommission) {

            window.location = "../../../Intelligence/ExportSpecialReport?idCommissionSale=" + idCommission;

        }

        $scope.dateToday = new Date();

        $scope.toggleMin = function () {
            $scope.minDate = $scope.minDate ? null : new Date();
        };

        $scope.toggleMin();

        $scope.openSince = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedSince = true;
            $scope.openedUntil = false;
        };

        $scope.openUntil = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedSince = false;
            $scope.openedUntil = true;
        };

        $scope.openDatePayment = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.openedDatePayment = true;
        };

        $scope.dateOptions = {
            formatYear: 'yyyy',
            startingDay: 1
        };

        $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM/dd/yyyy'];
        $scope.format = $scope.formats[4];

    });

jQuery.fn.reset = function () {
    $(this).each(function () { this.reset(); });
}
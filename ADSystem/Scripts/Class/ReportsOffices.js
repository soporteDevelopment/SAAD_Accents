angular.module("General")
    .controller('ReportsController', function (models, $scope, $http, $window, notify, $rootScope, $filter) {

        $scope.itemsPerPage = 500;
        $scope.currentPage = 0;
        $scope.total = 0;
        $scope.comisiones = {};
        $scope.percentage = 0;
        $scope.amountPayment = 0;
        $scope.percentageArteriors = 0;
        $scope.amountPaymentProducts = 0;
        $scope.amountPaymentArteriors = 0;

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

        $scope.LoadOffices = function () {
            $http({
                method: 'POST',
                url: '../../../Offices/ListAllOffices'
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        $scope.offices = data.oData.Offices;
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
                $("#txtValidation").append("Seleccione un rango de fechas para la busqueda  </br>");
                $("#modalValidation").modal("show");
            } else {
                $("#searchReport").button("loading");

                $http({
                    method: 'POST',
                    url: '../../../Intelligence/GetSalesReportOffices',
                    params: {
                        dateSince: $scope.dateSince,
                        dateUntil: $scope.dateUntil,
                        office: ($scope.office == undefined || $scope.office.idDespacho == undefined) ? "" : $scope.office.idDespacho,
                        customer: ($scope.sCustomer == "" || $scope.sCustomer == null) ? "" : $scope.sCustomer,
                        remission: $scope.remission,
                        amazonas: $scope.sBranchAma,
                        guadalquivir: $scope.sBranchGua,
                        textura: $scope.sBranchTex
                    }
                }).
                    success(function (data, status, headers, config) {

                        $("#searchReport").button("reset");

                        if (data.success == 1) {

                            if (data.oData.Sales != undefined) {

                                $scope.officepayment = $scope.office;

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

        $scope.DetailSaleReport = function (remision) {

            $scope.includeURL = "DetailSaleReportOffice?remision=" + remision;

            $("#modalDetailSaleReport").modal("show");

            $('#modalDetailSaleReport').on('hidden.bs.modal', function (e) {
                $scope.CalculateTotal();
            })

        };

        $scope.CloseDetailSaleReport = function () {

            $("#modalDetailSaleReport").modal("hide");

            $scope.CalculatePayment();
        };

        $scope.CalculateTotal = function () {
            var notacredito = 0;

            $scope.comisiones.TotalVentasBruto = 0;

            //TotalVentasBruto
            angular.forEach($scope.Sales, function (value, key) {
                if (!value.Omitir) {
                    $scope.comisiones.TotalVentasBruto = $scope.comisiones.TotalVentasBruto + value.ImportePagadoCliente;
                }
            });

            //TotalVentasNeto
            var totalsinIVA = 0;

            angular.forEach($scope.Sales, function (value, key) {

                if (!value.Omitir) {

                    angular.forEach(value.listTypePayment, function (payment, paymentkey) {
                        totalsinIVA = totalsinIVA + ((payment.amount / 116) * 100);
                    });

                    //Quitar Total Pagado con Nota de Crédito        
                    angular.forEach(value.listTypePayment, function (payment, paymentkey) {

                        //Si es una Nota de Crédito y está saldada
                        if (payment.typesPayment == 8 && payment.Estatus == 1) {
                            if (value.ImportePagadoCliente <= payment.amount) {
                                notacredito = notacredito + value.ImportePagadoCliente;
                            } else {
                                notacredito = notacredito + ((payment.amount / 116) * 100);
                            }
                            //Si es venta de credito
                        } else if (payment.typesPayment == 2) {
                            //Si crédito se obtiene el historial 
                            if (payment.HistoryCredit != undefined) {
                                angular.forEach(payment.HistoryCredit, function (history, historykey) {
                                    if (history.idFormaPago == 8 && history.Estatus == 1) {
                                        notacredito = notacredito + ((history.Cantidad / 116) * 100);
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


            if (totalsinIVA > notacredito) {
                totalsinIVA = totalsinIVA - notacredito;
            } else {
                totalsinIVA = 0;
            }

            $scope.comisiones.TotalVentasNeto = totalsinIVA;

            //TotalSaldado
            var totalsaldado = 0;

            angular.forEach($scope.Sales, function (value, key) {
                if (!value.Omitir) {
                    //Está saldada
                    if (value.Estatus == 1) {
                        angular.forEach(value.listTypePayment, function (payment, paymentkey) {
                            //No es crédito y está saldada
                            if (payment.typesPayment != 2 && payment.Estatus == 1) {
                                totalsaldado = totalsaldado + ((payment.amount / 116) * 100);
                            } else if (payment.typesPayment == 2) {
                                //Si es crédito se obtiene el historial 
                                if (payment.HistoryCredit != undefined) {
                                    angular.forEach(payment.HistoryCredit, function (history, historykey) {
                                        //Está saldado
                                        if (history.Estatus == 1) {
                                            totalsaldado = totalsaldado + ((history.Cantidad / 116) * 100);
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

            $scope.comisiones.TotalSaldado = (totalsaldado < notacredito) ? totalsaldado : totalsaldado - notacredito;;

            //TotalPendientePorSaldar
            $scope.comisiones.TotalPendiente = $scope.comisiones.TotalVentasNeto - $scope.comisiones.TotalSaldado;
        };

        $scope.GetCommission = function () {

            if ($scope.office != undefined) {

                $http({
                    method: 'POST',
                    url: '../../../Intelligence/GetCommissionsOffice',
                    params: {
                        idOffice: $scope.office.idDespacho,
                        dateSince: $scope.dateSince,
                        dateUntil: $scope.dateUntil,
                    }
                }).
                    success(function (data, status, headers, config) {

                        if (data.success == 1) {

                            $scope.CommissionsPayment = data.oData.Commissions;
                            $scope.comisiones.ComisionPagada = data.oData.CommissionPayment;

                        } else if (data.failure == 1) {

                            notify(data.oData.Error, $rootScope.error);

                        } else if (data.noLogin == 1) {

                            window.location = "../../../Access/Close"

                        }
                    }).
                    error(function (data, status, headers, config) {
                        notify("Ocurrío un error.", $rootScope.error);
                    });
            }
        };

        $scope.SetOmit = function (idSale, omit) {

            $http({
                method: 'POST',
                url: '../../../Intelligence/SetOmit',
                params: {
                    idSale: idSale,
                    omit: omit
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        angular.forEach($scope.Sales, function (value, key) {

                            if (value.idVenta == idSale) {
                                (value.Omitir == true) ? value.Omitir = false : value.Omitir = true;
                            }

                        });

                        $scope.CalculateTotal();

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

        $scope.SetPercentage = function (e) {

            angular.forEach($scope.Sales, function (value, key) {

                angular.forEach(value.oDetail, function (service, servicekey) {

                    if (service.idDetalleVenta == e.item.idDetalleVenta) {
                        service.Porcentaje = e.item.Porcentaje;
                    }

                });

            });

            $scope.CalculateTotal();
        };

        $scope.SetOmitItem = function (idDetail, omit) {

            $http({
                method: 'POST',
                url: '../../../Intelligence/SetOmitItem',
                params: {
                    idDetail: idDetail,
                    omit: omit
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        angular.forEach($scope.items, function (value, key) {

                            if (value.idDetalleVenta == idDetail) {

                                (value.Omitir == true) ? value.Omitir = false : value.Omitir = true;
                            }

                        });

                        angular.forEach($scope.Sales, function (value, key) {

                            angular.forEach(value.oDetail, function (detail, detailKey) {

                                if (detail.idDetalleVenta == idDetail) {

                                    (detail.Omitir == true) ? detail.Omitir = false : detail.Omitir = true;
                                }

                            });

                        });

                        $scope.CalculatePayment();

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

        $scope.CalculatePayment = function () {

            $scope.amountPayment = 0;
            var subtotal = 0;
            var arteriorsTotal = 0;
            var sumproductTotal = 0;

            angular.forEach($scope.Sales, function (value, key) {

                var arteriors = 0;
                var sumproduct = 0;
                var notacredito = 0;

                if (!value.Pagado) {

                    if (!value.Omitir) {

                        //Quitar Total Pagado con Nota de Crédito  
                        angular.forEach(value.listTypePayment, function (payment, paymentkey) {
                            //Es crédito y está saldada
                            if (payment.typesPayment == 8 && payment.Estatus == 1) {
                                if (value.ImportePagadoCliente <= payment.amount) {
                                    notacredito = notacredito + value.ImportePagadoCliente;
                                } else {
                                    notacredito = notacredito + payment.amount;
                                }

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

                        var percentage = 0;

                        if (notacredito > 0) {
                            percentage = notacredito / value.ImportePagadoCliente;
                        }

                        angular.forEach(value.oDetail, function (service, servicekey) {
                            if (service.oProducto != null) {
                                if (service.oProducto.idProveedor == 114) {
                                    var total = ((service.Cantidad * service.Precio) - ((service.Cantidad * service.Precio) * (service.Descuento / 100)));

                                    arteriors = arteriors + (((total / 116) * 100) * (service.Porcentaje / 100));
                                } else if (service.idProducto > 0 && service.Omitir != true && service.Porcentaje > 0) {
                                    var total = ((service.Cantidad * service.Precio) - ((service.Cantidad * service.Precio) * (service.Descuento / 100)));

                                    sumproduct = sumproduct + (((total / 116) * 100) * (service.Porcentaje / 100));
                                }
                            } else if (service.idServicio > 0 && service.Omitir != true && service.Porcentaje > 0) {
                                var total = ((service.Cantidad * service.Precio) - ((service.Cantidad * service.Precio) * (service.Descuento / 100)));

                                sumproduct = sumproduct + (((total / 116) * 100) * (service.Porcentaje / 100));
                            }
                        });

                        var porcentajeNotaCredito = (sumproduct * percentage);

                        arteriorsTotal = arteriorsTotal + (arteriors - (arteriors * (((notacredito * 100) / value.ImportePagadoCliente) / 100)));
                        sumproductTotal = sumproductTotal + (sumproduct - percentage);
                        subtotal = subtotal + ((sumproduct - porcentajeNotaCredito) + arteriors);
                    }
                }

                var porcentajeNotaCredito = (sumproduct * percentage);;

                value.ComisionGeneral = sumproduct - porcentajeNotaCredito;
                value.ComisionArterios = arteriors - (arteriors * percentage);
                value.ComisionTotal = (sumproduct - porcentajeNotaCredito) + arteriors;
                value.TotalNotaCredito = notacredito;
            });

            $scope.amountPayment = subtotal;
            $scope.amountPaymentProducts = sumproductTotal;
            $scope.amountPaymentArteriors = arteriorsTotal;
        };

        $scope.AddCommission = function () {

            var salespaidout = [];

            angular.forEach($scope.Sales, function (value, key) {
                if (value.Pagar) {
                    salespaidout.push(value)
                }
            });

            $scope.amountPayment = parseFloat($scope.amountPayment);
            $scope.amountPaymentProducts = (($scope.amountPaymentProducts != undefined) ? parseFloat($scope.amountPaymentProducts) : 0);
            $scope.amountPaymentArteriors = (($scope.amountPaymentArteriors != undefined) ? parseFloat($scope.amountPaymentArteriors) : 0);

            if ($scope.officepayment == undefined || $scope.officepayment.idDespacho == undefined) {

                notify("Seleccione un despacho.", $rootScope.error);

            } else {
                $http({
                    method: 'POST',
                    url: '../../../Intelligence/AddCommissionOffice',
                    data: {
                        idDespacho: $scope.officepayment.idDespacho,
                        FormaPago: $scope.typePayment,
                        Cantidad: parseFloat($scope.amountPayment),
                        Concepto: $scope.concept,
                        Fecha: $scope.dateSince,
                        FechaPago: $scope.datePayment,
                        Total: parseFloat($scope.amountPayment),
                        Detalle: $scope.detail,
                        sales: salespaidout
                    }
                }).
                    success(function (data, status, headers, config) {

                        if (data.success == 1) {

                            notify(data.oData.Message, $rootScope.success);

                            $scope.GenerarReporteVentas();

                            $scope.concept = null;
                            $scope.typePayment = 0;
                            $scope.amountPayment = 0;
                            $scope.amountPaymentProducts = 0;
                            $scope.amountPaymentArteriors = 0;
                            $scope.percentage = 0;
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
            }

        };

        $scope.GetTypesPaymentForSale = function (idSale) {

            $http({
                method: 'POST',
                url: '../../../Sales/GetTypesPaymentForSale',
                params: {
                    idSale: idSale,
                }
            }).
                then(function (response) {

                    if (response.data.success == 1) {

                        var suma = 0;

                        if (response.data.oData.TypesPayment.length > 0) {

                            angular.forEach(response.data.oData.TypesPayment, function (value, key) {

                                if (value.HistoryCredit.length > 0) {

                                    for (var i = 0; i <= value.HistoryCredit.length - 1; i++) {

                                        suma = suma - value.HistoryCredit[i].Cantidad;

                                    }

                                    value.amount = value.amount + suma;

                                }

                            });

                            $scope.TypesPayment = response.data.oData.TypesPayment;

                        } else {

                            notify('No se encontraron registros.', $rootScope.error);

                        }

                    } else if (response.data.failure == 1) {

                        notify(response.data.oData.Error, $rootScope.error);

                    } else if (response.data.noLogin == 1) {

                        window.location = "../../../Access/Close";

                    }

                })

        };

        $scope.init = function (detail) {
            $scope.items = detail;
        };

        $scope.exportCommision = function (idCommission) {
            window.location = "../../../Intelligence/ExportReportOffice?idCommissionSale=" + idCommission;
        };

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
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

        $scope.LoadUsers = function () {

            $http({
                method: 'POST',
                url: '../../../Users/ListTotalUsers'
            }).
           success(function (data, status, headers, config) {

               if (data.success == 1) {

                   $scope.users = data.oData.Users;

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
                    url: '../../../Intelligence/GetSummarySales',
                    params: {
                        dateSince: $scope.dateSince,
                        dateUntil: $scope.dateUntil,
                        seller: ($scope.seller == undefined || $scope.seller.idUsuario == undefined) ? "" : $scope.seller.idUsuario,
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

                            $scope.comisiones.CommissionOffice = data.oData.CommissionOffice;

                            $scope.comisiones.CommissionSeller = data.oData.CommissionSeller;

                            $scope.CalculateTotal();

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

            $scope.includeURL = "DetailSummarySaleReport?remision=" + remision;

            $("#modalDetailSaleReport").modal("show");

            $('#modalDetailSaleReport').on('hidden.bs.modal', function (e) {
                $scope.CalculateTotal();
            })

        };

        $scope.CalculateTotal = function () {

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
            //var totalservicios = 0;

            angular.forEach($scope.Sales, function (value, key) {

                //var servicios = 0;

                //if (!value.Omitir && !value.Compartida) {

                //    //Servicios que se omiten   
                //    angular.forEach(value.oDetail, function (service, servicekey) {

                //        //service.idServicio == 1 || service.idServicio == 11

                //        if (service.idServicio > 0 && service.Omitir == true) {

                //            servicios = servicios + ((service.Cantidad * service.Precio) - ((service.Cantidad * service.Precio) * (service.Descuento / 100)));

                //            totalservicios = totalservicios + servicios;

                //        } else if (service.idProducto > 0 && service.Omitir == true) {

                //            servicios = servicios + ((service.Cantidad * service.Precio) - ((service.Cantidad * service.Precio) * (service.Descuento / 100)));

                //        } else if (service.idCredito > 0) {

                //            servicios = servicios + ((service.Cantidad * -1) * service.Precio);

                //        }

                //    });

                //    var subtotal = value.ImporteVenta - servicios;

                //    //Quitar el IVA    
                //    if (value.IntDescuento > 0) {

                //        totalsinIVA = totalsinIVA + (subtotal - (subtotal * (value.IntDescuento / 100)));

                //    } else {

                //        totalsinIVA = totalsinIVA + subtotal;
                //    }

                //} else if (!value.Omitir && value.Compartida) {

                //    //Servicios que se omiten   
                //    angular.forEach(value.oDetail, function (service, servicekey) {

                //        if (service.idServicio > 0 && service.Omitir == true) {

                //            servicios = servicios + (((service.Cantidad * service.Precio) - ((service.Cantidad * service.Precio) * (service.Descuento / 100))) / 2);

                //        } else if (service.idProducto > 0 && service.Omitir == true) {

                //            servicios = servicios + ((service.Cantidad * service.Precio) - ((service.Cantidad * service.Precio) * (service.Descuento / 100)));

                //        } else if (service.idCredito > 0) {

                //            servicios = servicios + ((service.Cantidad * -1) * service.Precio);

                //        }

                //    });

                //    var subtotal = value.ImporteVenta - servicios;

                //    //Quitar el IVA   
                //    if (value.IntDescuento > 0) {

                //        totalsinIVA = totalsinIVA + ((subtotal - (subtotal * (value.IntDescuento / 100))) / 2);

                //    } else {

                //        totalsinIVA = totalsinIVA + (subtotal/2);

                //    }

                //}

                if (!value.Omitir && !value.Compartida) {

                    angular.forEach(value.listTypePayment, function (payment, paymentkey) {

                        //Si tiene IVA se le resta
                        if (value.IVA) {

                            totalsinIVA = totalsinIVA + ((payment.amount / 116) * 100);

                        } else {

                            totalsinIVA = totalsinIVA + payment.amount;

                        }

                    });

                } else if (!value.Omitir && value.Compartida) {

                    angular.forEach(value.listTypePayment, function (payment, paymentkey) {

                        //Si tiene IVA se le resta
                        if (value.IVA) {

                            totalsinIVA = totalsinIVA + (((payment.amount / 116) * 100) / 2);

                        } else {

                            totalsinIVA = totalsinIVA + (payment.amount / 2);

                        }

                    });

                }

            });

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

                                //Si tiene IVA se le resta
                                if (value.IVA) {

                                    totalsaldado = totalsaldado + ((payment.amount / 116) * 100);
                                                                       

                                } else {

                                    totalsaldado = totalsaldado + payment.amount;

                                }

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

                    }

                } else if (!value.Omitir && value.Compartida) {

                    if (value.Estatus == 1) {

                        angular.forEach(value.listTypePayment, function (payment, paymentkey) {

                            //No es crédito y está saldada
                            if (payment.typesPayment != 2 && payment.Estatus == 1) {

                                //Si tiene IVA se le resta
                                if (value.IVA) {

                                    totalsaldado = totalsaldado + (((payment.amount / 116) * 100) / 2);


                                } else {

                                    totalsaldado = totalsaldado + (payment.amount / 2);

                                }

                            } else if (payment.typesPayment == 2) {

                                //Si es crédito se obtiene el historial 
                                if (payment.HistoryCredit != undefined) {

                                    angular.forEach(payment.HistoryCredit, function (history, historykey) {

                                        //Está saldado
                                        if (history.Estatus == 1) {
                                            totalsaldado = totalsaldado + (history.Cantidad/2);
                                        }

                                    });
                                }

                            }

                        });

                    }

                }
            });

            $scope.comisiones.TotalSaldado = totalsaldado - totalservicios;

            //TotalPendientePorSaldar
            $scope.comisiones.TotalPendiente = $scope.comisiones.TotalVentasNeto - $scope.comisiones.TotalSaldado;

        };        

        $scope.GetTypesPaymentForPrint = function (idSale) {

            $http({
                method: 'POST',
                url: '../../../Sales/GetTypesPaymentForPrint',
                params: {
                    idSale: idSale,
                }
            }).
            then(function (response) {

                if (response.data.success == 1) {

                    var suma = 0;

                    if (response.data.oData.TypesPayment.length > 0) {
                        
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

            window.location = "../../../Intelligence/ExportReport?idCommissionSale=" + idCommission;

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
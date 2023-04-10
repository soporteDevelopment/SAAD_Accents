angular.module("General").controller('EntriesController', function (models, entryValidator, egressValidator, $scope, $http, $window, notify, $rootScope) {

    $scope.entries = new Array();
    $scope.egresses = new Array();
    $scope.sellers = new Array();
    $scope.entryId = null;
    $scope.openedDateEntry = false;
    $scope.totalEntries = 0;
    $scope.totalEgresses = 0;
    $scope.selectedEntries = [],
        $scope.selectedEgress = [],
        $scope.amountEntry = 0;
    $scope.amountEgress = 0;
    $scope.sale = undefined;

    $scope.loadInit = function () {
        $("#dateSince").datetimepicker({
            locale: 'ES',
            format: 'MM-DD-YYYY'
        });
        $("#dateUntil").datetimepicker({
            locale: 'ES',
            format: 'MM-DD-YYYY'
        });
        var today = new Date();

        $scope.dateSince = (today.getMonth() + 1) + "/" + today.getDate() + "/" + today.getFullYear();
        $scope.dateUntil = (today.getMonth() + 1) + "/" + today.getDate() + "/" + today.getFullYear();
        $scope.listPendingSales();
        $scope.listEntries();
        $scope.includeEgressOnLine = "../../Egresses/Add";
        $scope.includeInternalEgressOnLine = "../../Egresses/AddInternal";
        $scope.includeReportOnLine = "../../CashReport/Add";
    };

    $scope.listEntries = function () {
        $("#search").button("loading");

        $http({
            method: 'GET',
            url: '../../../Entries/ListActivesEntries',
            params: { remission: $scope.remissionEntry }
        }).
            success(function (data, status, headers, config) {
                $("#search").button("reset");

                if (data.success == 1) {
                    if (data.oData.Entries.length > 0) {
                        $scope.entries = data.oData.Entries;
                        $scope.totalEntries = data.oData.Total;
                    } else {
                        $scope.entries = null;
                        $scope.totalEntries = 0;
                    }

                    $scope.listEgresses();
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).
            error(function (data, status, headers, config) {
                $("#search").button("reset");

                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.listEgresses = function () {
        $http({
            method: 'GET',
            url: '../../../Egresses/ListActivesEgresses',
            params: { remission: $scope.remissionEgress }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    if (data.oData.Egresses.length > 0) {
                        $scope.egresses = data.oData.Egresses;
                        $scope.totalEgresses = data.oData.Total;
                    } else {
                        $scope.egresses = null;
                        $scope.totalEgresses = 0;
                    }
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

    $scope.loadSellers = function () {
        $http({
            method: 'POST',
            url: '../../../Users/ListAllUsers'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.sellers = data.oData.Users;
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

    $scope.entry = new models.Entry();

    $scope.modalAddEntry = function () {
        $scope.entryId = null;
        $scope.includeEntryOnLine = "../../Entries/Add?" + Date.now();
        $scope.loadSellers();
        $scope.cleanEntry();
        $("#modalEntry").modal("show");
    };

    $scope.getPayments = function () {
        $http({
            method: 'GET',
            url: '../../../sales/GetPaymentsBySale',
            params: { id: ($scope.entry.idSale != undefined && $scope.entry.idSale != null) ? $scope.entry.idSale.originalObject.idVenta : null }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    if (data.oData.Payments.length > 0) {
                        $scope.payments = data.oData.Payments;
                    } else {
                        notify("No existen pagos en efectivo", $rootScope.error);
                    }
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

    $scope.cleanAmount = function () {
        $scope.entry.Amount = 0;
        $scope.payments = [];
    };

    $scope.calculateTotal = function () {
        $scope.entry.Amount = 0;

        $scope.payments.forEach(function (item) {
            if (item.Selected) {
                $scope.entry.Amount = $scope.entry.Amount + item.Amount;
            }
        });
    };

    $scope.saveAddEntry = function () {

        var payments = [];

        $scope.payments.forEach(function (item) {
            if (item.Selected) {
                payments.push(item);
            }            
        });

        $http({
            method: 'POST',
            url: '../../../Entries/SaveAdd',
            data: {
                EntregadaPor: $scope.entry.DeliveredBy,
                EntregadaOtro: $scope.entry.DeliveredAnotherBy,
                Tipo: $scope.entry.Type,
                idVenta: ($scope.entry.idSale != undefined && $scope.entry.idSale != null) ? $scope.entry.idSale.originalObject.idVenta : null,
                Cantidad: $scope.entry.Amount,
                Comentarios: $scope.entry.Comments,
                payments: payments
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.cleanEntry();
                    $scope.listEntries();
                    $("#modalEntry").modal("hide");
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

    $scope.modalUpdateEntry = function (id) {
        $scope.entryId = id;
        $scope.loadSellers();
        $scope.includeEntryOnLine = "../../Entries/Update?id=" + $scope.entryId;
        $("#modalEntry").modal("show");
    };

    $scope.saveUpdate = function () {
        $http({
            method: 'PATCH',
            url: '../../../Entries/SaveUpdate',
            params: {
                idEntrada: $scope.entryId,
                EntregadaPor: $scope.entry.DeliveredBy,
                EntregadaOtro: $scope.entry.DeliveredAnotherBy,
                Tipo: $scope.entry.Type,
                idVenta: ($scope.entry.idSale != undefined && $scope.entry.idSale != null) ? $scope.entry.idSale.originalObject.idVenta : null,
                Cantidad: $scope.entry.Amount,
                Comentarios: $scope.entry.Comments
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.cleanEntry();
                    $scope.listEntries();
                    $("#modalEntry").modal("hide");
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

    $scope.cleanEntry = function () {
        $scope.entry.DeliveredBy = null;
        $scope.entry.DeliveredAnotherBy = null;
        $scope.entry.Type = 1;
        $scope.entry.idSale = null;
        $scope.entry.Amount = null;
        $scope.entry.Comments = null;
        $scope.payments = [];
    };

    $scope.cleanEgress = function () {
        $scope.egress.DeliveredBy = null;
        $scope.egress.DeliveredAnotherBy = null;
        $scope.egress.Type = 1;
        $scope.egress.idSale = null;
        $scope.egress.Amount = null;
        $scope.egress.Comments = null;
    }

    $scope.listPendingSales = function () {
        $http({
            method: 'GET',
            url: '../../../Sales/GetPendingSales',
            params: {}
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    if (data.oData.Sales.length > 0) {
                        $scope.sales = data.oData.Sales;
                    } else {
                        $scope.sales = null;
                    }
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

    $scope.egress = new models.Egress();

    $scope.modalAddEgress = function () {
        $scope.cleanEgress();
        $scope.loadSellers();
        $("#modalEgress").modal("show");
    };

    $scope.setEgress = function (idEntry, idSale) {
        $scope.cleanEgress();
        $scope.loadSellers();
        $scope.egress.Type = (idSale > 0) ? 1 : 2;
        $scope.egress.idEntry = idEntry;
        $scope.egress.idSale = idSale;
        $("#modalEgress").modal("show");
    };

    $scope.setInternalEgress = function (idEntry, idSale) {
        $scope.cleanEgress();
        $scope.loadSellers();
        $scope.egress.Type = (idSale > 0) ? 1 : 2;
        $scope.egress.idEntry = idEntry;
        $scope.egress.idSale = idSale;
        $("#modalInternalEgress").modal("show");
    };

    $scope.saveEgressAdd = function () {
        $http({
            method: 'POST',
            url: '../../../Egresses/SaveAdd',
            params: {
                RecibidaPor: $scope.egress.ReceivedBy,
                RecibidaOtro: $scope.egress.ReceivedAnotherBy,
                idEntrada: $scope.egress.idEntry,
                idVenta: ($scope.egress.idSale != undefined && $scope.egress.idSale != null) ? $scope.egress.idSale.originalObject.idVenta : null,
                Cantidad: $scope.egress.Amount,
                Comentarios: $scope.egress.Comments,
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.listEntries();
                    $scope.cleanEgress();
                    $("#modalEgress").modal("hide");
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

    $scope.saveInternalEgressAdd = function () {
        $http({
            method: 'POST',
            url: '../../../Egresses/SaveAdd',
            params: {
                RecibidaPor: $scope.egress.ReceivedBy,
                RecibidaOtro: $scope.egress.ReceivedAnotherBy,
                idEntrada: $scope.egress.idEntry,
                idVenta: $scope.egress.idSale,
                Cantidad: $scope.egress.Amount,
                Comentarios: $scope.egress.Comments,
                fkSalida: $scope.egress.fkEgress
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.listEntries();
                    $scope.cleanEgress();
                    $("#modalInternalEgress").modal("hide");
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

    $scope.cancelEntry = function (id) {
        $http({
            method: 'PATCH',
            url: '../../../Entries/Cancel',
            params: {
                id: id
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.listEntries();
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

    $scope.removeEntry = function (id) {
        var response = confirm("¿Desea borrar el registro?");

        if (response) {
            $http({
                method: 'PATCH',
                url: '../../../Entries/Delete',
                params: {
                    id: id
                }
            }).success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.listEntries();
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
        }
    };

    $scope.removeEgress = function (id) {
        var response = confirm("¿Desea borrar el registro?");

        if (response) {
            $http({
                method: 'PATCH',
                url: '../../../Egresses/Delete',
                params: {
                    id: id
                }
            }).success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.listEntries();
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
        }
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

    $scope.openDateEntry = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedDateEntry = !$scope.openedDateEntry;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM/dd/yyyy'];
    $scope.format = $scope.formats[4];

    $scope.getDate = function (date) {
        var res = "";
        if (date != null) {
            if (date.length > 10) {
                res = date.substring(6, 19);
            }
        }

        return res;
    };

    //Report
    $scope.cashreport = new models.CashReport();

    $scope.modalAddReport = function () {
        $scope.cashreport.Comments = "";
        $("#modalReport").modal("show");
    };

    $scope.selectEntry = function (e, idEntry, amount) {
        if (e.target.checked) {
            $scope.selectedEntries.push(idEntry);
            $scope.amountEntry = $scope.amountEntry + amount;
        } else {
            $scope.selectedEntries.splice($scope.selectedEntries.indexOf(idEntry), 1);;
            $scope.amountEntry = $scope.amountEntry - amount;
        }
    }

    $scope.selectEgress = function (e, idEgress, amount, internalEgresses) {
        if (e.target.checked) {
            $scope.selectedEgress.push(idEgress);
            $scope.amountEgress = $scope.amountEgress + amount;

            if (internalEgresses != null && internalEgresses != undefined) {
                internalEgresses.forEach(function (element) {
                    $scope.amountEgress = $scope.amountEgress + element.Cantidad;
                })
            }
        } else {
            $scope.selectedEgress.splice($scope.selectedEgress.indexOf(idEgress), 1);
            $scope.amountEgress = $scope.amountEgress - amount;

            if (internalEgresses != null && internalEgresses != undefined) {
                internalEgresses.forEach(function (element) {
                    $scope.amountEgress = $scope.amountEgress - element.Cantidad;
                })
            }
        }
    }

    $scope.saveAdd = function () {
        $http({
            method: 'POST',
            url: '../../../CashReport/SaveAdd',
            params: {
                Comentarios: $scope.cashreport.Comments,
                CantidadIngreso: $scope.amountEntry,
                CantidadEgreso: $scope.amountEgress
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $scope.saveRelationReportEntries(data.oData.idReportEntries);
                    $scope.saveRelationReportEgresses(data.oData.idReportEntries);
                    $("#modalReport").modal("hide");
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

    $scope.saveRelationReportEntries = function (idReport) {
        if ($scope.selectedEntries.length > 0) {
            $http({
                method: 'PATCH',
                url: '../../../Entries/ReportEntries',
                params: {
                    idReport: idReport,
                    entries: $scope.selectedEntries
                }
            }).success(function (data, status, headers, config) {
                $scope.listEntries();
                if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
        }
    };

    $scope.saveRelationReportEgresses = function (idReport) {
        if ($scope.selectedEgress.length > 0) {
            $http({
                method: 'PATCH',
                url: '../../../Egresses/ReportEgresses',
                params: {
                    idReport: idReport,
                    egresses: $scope.selectedEgress
                }
            }).success(function (data, status, headers, config) {
                $scope.listEgresses();
                if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
        }
    };
});
angular.module("General").controller('PreQuotationsController', function (models, $scope, $http, $window, notify, $rootScope, $timeout) {

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;

    //Get list pre quotations
    $scope.listPreQuotations = function () {

        $("#sending").button("loading");

        $http({
            method: 'POST',
            url: '../../../PreQuotations/GetPreQuotations',
            params: {
                allTime: ($scope.searchSince == 2) ? true : false,
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                number: $scope.number,
                costumer: ($scope.sCustomer == "" || $scope.sCustomer == null) ? "" : $scope.sCustomer,
                iduser: ($scope.sseller == "" || $scope.sseller == null) ? "" : $scope.sseller.idUsuario,
                project: ($scope.sProject == "" || $scope.sProject == null) ? "" : $scope.sProject,
                amazonas: $scope.sBranchAma,
                guadalquivir: $scope.sBranchGua,
                textura: $scope.sBranchTex,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {

                $("#searchPreQuotations").button("reset");

                if (data.success == 1) {

                    if (data.oData.PreQuotation.length > 0) {

                        $scope.PreQuotations = data.oData.PreQuotation;
                        $scope.total = data.oData.Count;
                        //console.log('PreQuotations', data.oData.PreQuotation);

                    } else {

                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.PreQuotations = data.oData.PreQuotation;
                        $scope.total = data.oData.Count;

                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close"

                }
            }).
            error(function (data, status, headers, config) {

                $("#searchPreQuotations").button("reset");

                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    //cone preQuotation
    $scope.Clone = function (idPreQuotation) {

        window.location = "../../../PreQuotations/ClonePreQuotation?idPreQuotation=" + idPreQuotation;
        //console.log("clone", clone);
        //console.log("idPreQuotation", idPreQuotation);
        //if (clone) {
        //    $("#searchPreQuotations").button("loading");

        //    $http({
        //        method: 'POST',
        //        url: '../../../PreQuotations/ClonePreQuotation',
        //        data: {
        //            id: idPreQuotation
        //        }
        //    }).
        //        success(function (data, status, headers, config) {

        //            $("#searchPreQuotations").button("reset");

        //            if (data.success == 1) {

        //                notify("La pre cotización fue clonada", $rootScope.success);
        //                $scope.listPreQuotations();

        //            } else if (data.failure == 1) {

        //                notify(data.oData.Error, $rootScope.error);

        //            } else if (data.noLogin == 1) {

        //                window.location = "../../../Access/Close"

        //            }
        //        }).
        //        error(function (data, status, headers, config) {

        //            $("#searchPreQuotations").button("reset");

        //            notify("Ocurrío un error.", $rootScope.error);
        //        });
        //}
    };

    //Get sellers
    $scope.loadsellers = function () {

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

    $scope.openModalDetail = function (data) {
        //console.log("Data", data)
        const { oDetail, Numero, idPreCotizacion } = data;

        if ($scope.folio != "") {
            $scope.preQuotationId = idPreCotizacion;
            $scope.detailPrequotation = oDetail;
            $scope.folio = Numero;

            $("#openModalDetail").modal("show");
        }

    }

    $scope.DownloadPDF = (preQuotationID, preQuotationNumber) => {

        $http({
            method: 'POST',
            url: '../../../PreQuotations/DownloadPDF',
            params: { idPreQuotation: preQuotationID },
            responseType: 'arraybuffer'
        }).
            success(function (data, status, headers, config) {
                const blobFile = new Blob([data], { type: 'application/pdf' });
                const url = URL.createObjectURL(blobFile);
                const a = document.createElement('a');
                a.href = url;
                a.download = `Precotización-Folio ${preQuotationNumber}.pdf`;;
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
                URL.revokeObjectURL(url);

            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
    }

    //Envia la prectizacion a ser una cotizacion
    $scope.SendQuotation = (idPrequotation) => {
        //console.log("idPrequotation", idPrequotation);
        if (confirm("¿Desea enviar esta precotización a cotización?")) {
            $http({
                method: 'POST',
                url: '../../../PreQuotations/SendCotizacion',
                params: {
                    id: idPrequotation
                }
            }).
                success(function (data, status, headers, config) {

                    notify(data.oData.Message, $rootScope.success);
                    $scope.listPreQuotations();
                }).
                error(function (data, status, headers, config) {

                    $("#searchPreQuotations").button("reset");

                    notify("Ocurrío un error.", $rootScope.error);
                });
        }
    }

    //Eliminar la prectizacion
    $scope.DeletePreQuotation = (idPrequotation) => {
        //console.log("idPrequotation", idPrequotation);
        if (confirm("¿Desea eliminar esta precotización?")) {
            $http({
                method: 'POST',
                url: '../../../PreQuotations/DeletePreQuotation',
                params: {
                    id: idPrequotation
                }
            }).
                success(function (data, status, headers, config) {

                    notify(data.oData.Message, $rootScope.success);
                    $scope.listPreQuotations();
                }).
                error(function (data, status, headers, config) {

                    $("#searchPreQuotations").button("reset");

                    notify("Ocurrío un error.", $rootScope.error);
                });
        }
    }

    //Paging
    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listPreQuotations();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listPreQuotations();
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
            $scope.listPreQuotations();
        }
    };

    //Datepicker
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

    $scope.GetEraserPreQuotation = (id) => {
        window.location = "../../../PreQuotations/GetEraserPreQuotation?id=" + id;
    }

    $scope.GoToViewPageProvider = (detailId) => {
        window.location = "../../../ProviderPreQuotation/Index?id=" + $scope.preQuotationId + "&detailId=" + detailId;
    }

    $scope.GoToViewPageComplete = (detailId) => {
        window.location = "../../../ProviderPreQuotation/CompleteService?id=" + $scope.preQuotationId + "&detailId=" + detailId;
    }

    $scope.GoToViewPageAssign = (detailId) => {
        window.location = "../../../ProviderPreQuotation/AssignProvider?id=" + $scope.preQuotationId + "&detailId=" + detailId;
    }

    $scope.GoToViewPageGenerateOrder = (detailId) => {
        window.location = "../../../ProviderPreQuotation/GenerateOrderService?id=" + $scope.preQuotationId + "&detailId=" + detailId;
    }
});
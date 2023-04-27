angular.module("General").controller('QuotationsController', function ($scope, $http, $window, notify, $rootScope) {

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.Dollar = false;
    $scope.QuotationSelected = [];
    $scope.myFiles = [];

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

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listQuotations();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listQuotations();
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
            $scope.listQuotations();
        }
    };

    $scope.listQuotations = function (a) {

        if (a == 0) {
            $scope.currentPage = 0;
            $scope.QuotationSelected = [];
        }

        $("#searchQuotations").button("loading");

        $http({
            method: 'POST',
            url: '../../../UnifyQuotations/GetQuotations',
            params: {
                costumer: ($scope.sCustomer == "" || $scope.sCustomer == null) ? "" : $scope.sCustomer,
                iduser: ($scope.sseller == "" || $scope.sseller == null) ? "" : $scope.sseller.idUsuario,
                project: ($scope.sProject == "" || $scope.sProject == null) ? "" : $scope.sProject,
                amazonas: $scope.sBranchAma,
                guadalquivir: $scope.sBranchGua,
                textura: $scope.sBranchTex,
                dollar: $scope.Dollar,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            $("#searchQuotations").button("reset");

            if (data.success == 1) {

                if (data.oData.Quotations.length > 0) {

                    $scope.Quotations = data.oData.Quotations;
                    $scope.total = data.oData.Count;

                    if (a != 0) {
                        angular.forEach($scope.QuotationSelected,
                            function (value, key) {
                                angular.forEach($scope.Quotations,
                                    function (quotation, quotationkey) {
                                        if ((value.idCotizacion == quotation.idCotizacion))
                                        {
                                            quotation.Selected = true;
                                        }
                                    });
                            });
                    }

                } else {
                    notify('No se encontraron registros.', $rootScope.error);

                    $scope.Quotations = data.oData.Quotations;
                    $scope.total = data.oData.Count;
                }

            } else if (data.failure == 1) {
                notify(data.oData.Error, $rootScope.error);
            } else if (data.noLogin == 1) {
                window.location = "../../../Access/Close";
            }
        }).
        error(function (data, status, headers, config) {
            $("#searchQuotations").button("reset");
            notify("Ocurrío un error.", $rootScope.error);
        });
    };

    $scope.SelectAll = function () {

        $scope.Quotations.forEach(function (quotation, index) {
            if ($scope.SelectedAll) {
                quotation.Selected = true;
                $scope.buttonDisabled = false;
            } else {
                quotation.Selected = false;
                $scope.buttonDisabled = true;
                $scope.QuotationSelected = [];
            }
        });

        $scope.Quotations.forEach(function (quotation, index) {
            $scope.SelectQuotation(quotation);
        });
    },

    $scope.SelectQuotation = function (quotation) {

        var result = _.find($scope.QuotationSelected, function (chr) {

            return chr == quotation.idCotizacion

        });

        if (quotation.Selected && result == undefined) {
            $scope.QuotationSelected.push(quotation.idCotizacion);
        } else if (!quotation.Selected && result != undefined) {
            _.remove($scope.QuotationSelected, function (n) {
                return n === quotation.idCotizacion;
            });
        }
    },

    $scope.Validate = function (option) {
        //Validar que el cliente sea el mismo
        //Validar que el pryecto sea el mismo
        //Validar que la moneda sea la misma
        var quotationsSelected = new Array();

        angular.forEach($scope.QuotationSelected,
            function (value, key) {
                angular.forEach($scope.Quotations,
                    function (quotation, quotationkey) {
                        if (value == quotation.idCotizacion) {
                            quotationsSelected.push(quotation);
                        }
                    });
            });

        var valid = true;

        angular.forEach(quotationsSelected,
            function (value, key) {
                angular.forEach(quotationsSelected,
                    function (quotation, quotationkey) {

                        if (value.Proyecto == null || value.Proyecto == "") {
                            value.Proyecto = null;
                        }

                        if (quotation.Proyecto == null || quotation.Proyecto == "") {
                            quotation.Proyecto = null;
                        }

                        if ((value.idClienteFisico != quotation.idClienteFisico) ||
                            (value.idClienteMoral != quotation.idClienteMoral) ||
                            (value.idDespacho != quotation.idDespacho) ||
                            (value.idDespachoReferencia != quotation.idDespachoReferencia) ||
                            (value.Proyecto != quotation.Proyecto) ||
                            (value.Dolar != quotation.Dolar)
                        ) {
                            valid = false;
                        }

                    });
            });

        if (!valid) {
            alert("Los datos principales de las cotizaciones deben ser iguales.");
        } else {
            if (option == 1) {
                $scope.GenerateUnifiedQuotation();
            } else if (option == 2) {
                $scope.GenerateSaleUnifiedQuotation();
            } else {
                $scope.GenerateSaleUnifiedQuotationDollar();
            }
        }
    }

    $scope.GenerateUnifiedQuotation = function () {
        $("#btnPrintQuotations").button("loading");
        window.location = "../../../UnifyQuotations/GenerateUnifiedQuotation?lQuotations=" + JSON.stringify($scope.QuotationSelected);
    }

    $scope.GenerateSaleUnifiedQuotation = function () {
        $("#btnSaleQuotations").button("loading");
        window.location = "../../../UnifyQuotations/SaleUnifiedQuotation?lQuotations=" + JSON.stringify($scope.QuotationSelected);
    }

    $scope.GenerateSaleUnifiedQuotationDollar = function () {
        $("#btnSaleQuotations").button("loading");
        window.location = "../../../UnifyQuotations/SaleUnifiedQuotationDollar?lQuotations=" + JSON.stringify($scope.QuotationSelected);
    }

    $scope.RotateImage = function () {

        if ($scope.myFiles.length > 0) {
            $scope.rd += 1;
            var deg = 90 * $scope.rd;
            $('#holder').css({
                '-webkit-transform': 'rotate(' + deg + 'deg)',  //Safari 3.1+, Chrome  
                '-moz-transform': 'rotate(' + deg + 'deg)',     //Firefox 3.5-15  
                '-ms-transform': 'rotate(' + deg + 'deg)',      //IE9+  
                '-o-transform': 'rotate(' + deg + 'deg)',       //Opera 10.5-12.00  
                'transform': 'rotate(' + deg + 'deg)'          //Firefox 16+, Opera 12.50+  
            });

            if ($scope.rd > 3) {
                $scope.rd = 0;
            }
        }
    };

});

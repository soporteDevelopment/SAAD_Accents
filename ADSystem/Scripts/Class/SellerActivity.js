angular.module("General").controller('SellerActitvity', function (models, $scope, $http, $window, notify, $rootScope) {

    $scope.dateSince = null;
    $scope.dateUntil = null;
    $scope.seller = null;
    $scope.users = null;
    $scope.typereport = null;
    $scope.intelligence = null;

    $scope.loadusers = function () {

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

               window.location = "../../../Access/Close";

           }

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    $scope.loadreport = function () {

        var showModal = false;

        $("#txtValidation").empty();

        if ($scope.typereport == null) {
            
            $("#txtValidation").append("Seleccione un tipo de reporte </br>");

            showModal = true;

        }

        if ($scope.seller == null) {

            $("#txtValidation").append("Seleccione un vendedor </br>");

            showModal = true;

        }

        if (showModal == true) {

            $("#modalValidation").modal("show");

        } else {

            if ($scope.typereport == 3) {

                $scope.loadquotations();

            } else if ($scope.typereport == 2) {

                $scope.loadviews();

            } else if ($scope.typereport == 1) {

                $scope.loadsales();

            }

        }

    };

    $scope.loadquotations = function () {

        $http({
            method: 'POST',
            url: '../../../Intelligence/QuotationsReport',
            data: {
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                idseller: $scope.seller.idUsuario
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.quotations = data.oData.Quotations;
               $scope.costquotations = data.oData.CostQuotations;
               $scope.totalquotations = data.oData.TotalQuotations;
               $scope.quotationsToSale = data.oData.QuotationsToSale
               $scope.quotationsSaled = data.oData.QuotationsSaled;

               $scope.GetComments();

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

    $scope.loadsales = function () {

        $http({
            method: 'POST',
            url: '../../../Intelligence/SalesReport',
            data: {
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                idseller: $scope.seller.idUsuario
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.sales = data.oData.Sales;
               $scope.saled = data.oData.Saled;
               $scope.totalsales = data.oData.TotalSales;

               $scope.GetComments();

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

    $scope.loadviews = function () {

        $http({
            method: 'POST',
            url: '../../../Intelligence/OutProductsReport',
            data: {
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                idseller: $scope.seller.idUsuario
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.views = data.oData.Views;
               $scope.viewsSaled = data.oData.ViewsSaled;
               $scope.totalviews = data.oData.TotalViews;

               $scope.GetComments();

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

    $scope.AddComment = function () {

        $http({
            method: 'POST',
            url: '../../../Intelligence/SaveComments',
            data: {
                dtDate: $scope.dateSince,
                idseller: $scope.seller.idUsuario,
                comments: $scope.comment,
                typereport: $scope.typereport
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.comment = "";

               $scope.GetComments();

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

    $scope.GetComments = function () {

        $http({
            method: 'POST',
            url: '../../../Intelligence/GetComments',
            data: {
                dtDate: $scope.dateSince,
                idseller: $scope.seller.idUsuario,
                typereport: $scope.typereport
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.comments = data.oData.Comments;

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

    $scope.DeleteComment = function (idActivity) {

        $http({
            method: 'POST',
            url: '../../../Intelligence/DeleteComments',
            data: {
                idActivity: idActivity
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.GetComments();

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

});
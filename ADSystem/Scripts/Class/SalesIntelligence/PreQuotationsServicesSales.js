angular.module("General").controller('PreQuotationsServicesSalesController', function (models, $scope, $http, $window, notify, $rootScope, $filter) {


    $scope.DataInitialCat = () => {
        $scope.LoadServices();
        $scope.LoadProviders();
    }

    $scope.LoadProviders = () => {

        $http({
            method: 'POST',
            url: '../../../Providers/ListAllProviders',
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

    //Obtiene todos los servicios
    $scope.LoadServices = function () {

        $http({
            method: 'GET',
            url: '../../../Services/GetAllServices'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {
                   
                    $scope.listServices = data.oData.services;

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
    
    $scope.LoadServicesPreQuotations = function () {

        let paramsFilter = {            
            preQoutationNumber: $scope.preQuotationNumber ? $scope.preQuotationNumber : "",            
            startDate: $scope.dateSince,
            endDate: $scope.dateUntil,
            idService: $scope.descService ? $scope.descService.idServicio : null,
            idProvider: $scope.descProvider ? $scope.descProvider.idProveedor : null
        }
       
        $http({
            method: 'GET',
            url: '../../../SalesIntelligence/ServiceReport',
            params: paramsFilter
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    const { Reports } = data.oData;                   
                    if (!Reports.length) { notify('No se encontraron registros.', $rootScope.error); }
                    $scope.preQuotationsServices = Reports;
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

    $scope.DownloadPreQuotationsServicesReport = function () {

        var startDate = $scope.ValidateDate($scope.dateSince);
        var endDate = $scope.ValidateDate($scope.dateUntil);
        let preQoutationNumber = $scope.preQuotationNumber ? $scope.preQuotationNumber : "";
        let idService = $scope.descService ? $scope.descService.idServicio : null;
        let idProvider = $scope.descProvider ? $scope.descProvider.idProveedor : null;

        window.location = "/SalesIntelligence/PrequotationsServicesSalesXLS?startDate=" + startDate + "&endDate=" + endDate + "&preQoutationNumber=" + preQoutationNumber + "&idService=" + idService + "&idProvider=" + idProvider ;
    };

    $scope.ValidateDate = function (date) {
        var result = date;

        if (typeof date === 'object') {
            result = (((parseInt(date.getMonth() + 1)) < 10)
                ? "0" + (date.getMonth() + 1)
                : (date.getMonth() + 1)) +
                "/" +
                (((parseInt(date.getDate())) < 10) ? "0" + (date.getDate()) : (date.getDate())) +
                "/" +
                date.getFullYear();
        }

        return result;
    };

    $scope.setSelecBranchsetSelecBranch = function (a, b) {
        var index = $scope.arrayObjectIndexOf($scope.Branches, a, b);
        $scope.Branch = $scope.Branches[index];
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
});

jQuery.fn.reset = function () {
    $(this).each(function () { this.reset(); });
}
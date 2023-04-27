angular.module("General").controller('PreQuotationsSalesController', function (models, $scope, $http, $window, notify, $rootScope, $filter) {

    $scope.typeCustomer = null;
    $scope.physicalValue = 1;
    $scope.moralValue = 2;
    $scope.officeValue = 3;
    $scope.customerSelected = 0;
    $scope.customers = 0;

    $scope.LoadDataCatalogs = () => {

        $scope.LoadSellers();
    }

    //Obtiene todos los clientes morales
    $scope.LoadMoralCustomers = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllMoralCustomers'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {
                    $scope.customerList = data.oData.Customers;

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

    //Obtiene todos los clientes fisicos
    $scope.LoadPhysicalCustomers = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllPhysicalCustomers'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.customerList = data.oData.Customers;

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

    //Obtiene todos los despachos
    $scope.LoadOffices = function () {

        $http({
            method: 'POST',
            url: '../../../Offices/ListAllOffices'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.customerList = data.oData.Offices;


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

    //Obtiene todos los vendedores
    $scope.LoadSellers = function () {

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

    //Obtiene valor del tipo de cliente
    $('input[type=radio][name=type]').on('change', function () {

        $scope.customerList = "";
        $scope.typeCustomer = null;
        $scope.customers = 0;

        switch ($(this).val()) {
            case 'moral':
                $scope.LoadMoralCustomers();
                $scope.typeCustomer = $scope.moralValue;
                break;
            case 'physical':
                $scope.LoadPhysicalCustomers();
                $scope.typeCustomer = $scope.physicalValue;
                break;
            case 'office':
                $scope.LoadOffices();
                $scope.typeCustomer = $scope.officeValue;
                break;
        }
    });

    const tableContainer = document.querySelector('.table-container');
    console.log("tableContainer", tableContainer)
    let mouseDown = false;
    let startX, scrollLeft;

    let startDragging = function (e) {
        mouseDown = true;
        startX = e.pageX - tableContainer.offsetLeft;
        scrollLeft = tableContainer.scrollLeft;
    };
    let stopDragging = function (event) {
        mouseDown = false;
    };

    tableContainer.addEventListener('mousemove', (e) => {
        e.preventDefault();
        if (!mouseDown) { return; }
        const x = e.pageX - tableContainer.offsetLeft;
        const scroll = x - startX;
        tableContainer.scrollLeft = scrollLeft - scroll;
    });

    // Add the event listeners
    tableContainer.addEventListener('mousedown', startDragging, false);
    tableContainer.addEventListener('mouseup', stopDragging, false);
    tableContainer.addEventListener('mouseleave', stopDragging, false);

    $scope.LoadPreQuotations = function () {

        let paramsFilter = {
            idCustomer: $scope.customers ? ($scope.typeCustomer == $scope.officeValue ? $scope.customers?.idDespacho : $scope.customers?.idCliente) : null,
            typeCustomer: $scope.typeCustomer ? $scope.typeCustomer : null,
            preQoutationNumber: $scope.preQuotationNumber ? $scope.preQuotationNumber : "",
            project: $scope.project ? $scope.project : "",
            startDate: $scope.dateSince,
            endDate: $scope.dateUntil,
            idVendor: $scope.seller ? $scope.seller.idUsuario : null
        }

        $http({
            method: 'GET',
            url: '../../../SalesIntelligence/PreQuotationReport',
            params: paramsFilter
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    const { Reports } = data.oData;
                    if (!Reports.length) { notify('No se encontraron registros.', $rootScope.error); }
                    $scope.preQuotations = Reports;
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

    $scope.DownloadPreQuotationsReport = function () {
        var startDate = $scope.ValidateDate($scope.dateSince);
        var endDate = $scope.ValidateDate($scope.dateUntil);
        let customerId = $scope.customers ? ($scope.typeCustomer == $scope.officeValue ? $scope.customers?.idDespacho : $scope.customers?.idCliente) : null;
        let typeCustomer = $scope.typeCustomer ? $scope.typeCustomer : null;
        let preQoutationNumber = $scope.preQuotationNumber ? $scope.preQuotationNumber : "";
        let project = $scope.project ? $scope.project : "";
        let idVendor = $scope.seller ? $scope.seller.idUsuario : null

        window.location = "/SalesIntelligence/PreQuotationsReportXLS?startDate=" + startDate + "&endDate=" + endDate + "&preQoutationNumber=" + preQoutationNumber + "&project=" + project + "&typeCustomer=" + typeCustomer + "&idCustomer=" + customerId + "&idVendor=" + idVendor;
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
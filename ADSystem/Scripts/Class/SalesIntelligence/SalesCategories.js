angular.module("General")
	.controller('SalesCategoriesController', function (models, $scope, $http, $window, notify, $rootScope, $filter) {

		$scope.typeSale = 1;
		$scope.showServiceDetail = 0;
		$scope.categories = [];
		$scope.subcategories = [];

		$scope.LoadSales = function () {
			$http({
				method: 'GET',
				url: '../../../SalesIntelligence/SalesCategories',
				params: {
					startDate: $scope.dateSince,
					endDate: $scope.dateUntil,
					typeSale: $scope.typeSale,
					idCategory: $scope.idCategory,
					idSubcategory: $scope.idSubcategory,
					idService: $scope.idService
				}
			}).
				success(function (data, status, headers, config) {
					if (data.success == 1) {
						if ($scope.typeSale == 2) $scope.showServiceDetail = 1; else $scope.showServiceDetail = 0;
						if (data.oData.Sales.length == 0) { notify('No se encontraron registros.', $rootScope.error); }
						$scope.sales = data.oData.Sales;
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

		$scope.LoadServices = function () {
			$http({
				method: 'POST',
				url: '../../../Services/ListAllServices'
			}).
				success(function (data, status, headers, config) {
					if (data.success == 1) {
						$scope.services = data.oData.Services;
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

		$scope.GetCategories = function () {

			$http({
				method: 'POST',
				url: '../../../Products/GetCategories'
			}).
				success(function (data, status, headers, config) {
					$scope.categories = data.oData.Category;

					if ($scope.selectedCategoryId > 0) {
						$scope.setSelecCategory($scope.selectedCategoryId);
					}
				}).
				error(function (data, status, headers, config) {
					notify("Ocurrío un error.", $rootScope.error);
				});
		};

		$scope.GetSubcategories = function () {

			$http({
				method: 'POST',
				url: '../../../Products/GetSubcategories',
				params: {
					idCategory: $scope.idCategory
				}
			}).
				success(function (data, status, headers, config) {
					$scope.subcategories = data.oData.Subcategories;
				}).
				error(function (data, status, headers, config) {
					notify("Ocurrío un error.", $rootScope.error);
				});
		};

		$scope.DownloadSales = function () {
			var startDate = $scope.ValidateDate($scope.dateSince);
			var endDate = $scope.ValidateDate($scope.dateUntil);
			console.log('$scope.typeSale', $scope.typeSale);

			if ($scope.typeSale == "1") {
				window.location = "/SalesIntelligence/SalesProductsXLS?startDate=" + startDate + "&endDate=" + endDate + "&idCategory=" + $scope.idCategory + "&idSubcategory=" + $scope.idSubcategory;
			} else {
				window.location = "/SalesIntelligence/SalesServicesXLS?startDate=" + startDate + "&endDate=" + endDate + "&idService=" + $scope.idService;
			}
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

		$scope.setSelecBranch = function (a, b) {
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
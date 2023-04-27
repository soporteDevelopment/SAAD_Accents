angular.module("General").controller('CustomersController', function (models, MoralCustomerValidator, PhysicalCustomerValidator, $scope, $http, $window, notify, $rootScope) {

	$scope.tipoMoral = "M";
	$scope.tipoFisico = "F";
	$scope.months = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"];
	$scope.searchMoralCostumer = "";
	$scope.searchPhysicalCostumer = "";

	$scope.customer = 'physical';
	$scope.SelectedNationality = 0;
	$scope.SelectedGenre = 0;

	$scope.$watch("currentPage", function (newValue, oldValue) {
		$scope.GetNationalities();
		$scope.GetGenres();
	});

	$scope.$watch('customer', function (customer) {
		if (customer == "moral") {
			$("#moral-content").show();
			$("#physical-content").hide();
		}
		if (customer == "physical") {
			$("#moral-content").hide();
			$("#physical-content").show();
		}
	});

	$scope.ShowAlert = function (ID) {
		alert(ID);
	};

	$scope.AddMoralCustomer = function () {
		$window.location = '../../Customers/newMoralCustomer';
	};

	$scope.AddPhysicalCustomer = function () {
		$window.location = '../../Customers/newPhysicalCustomer';
	};

	$scope.valResult = {};

	$scope.newMoralCustomer = new models.MoralCustomers();

	$scope.SaveAddMoral = function (reload) {
		if ($scope.addMoralCustomerForm.$valid) {
			$scope.SaveAddMoralCustomer(reload);
		}
	};

	$scope.SaveAddMoralCustomer = function (reload) {
		$("#AddMoralCustomer").button("loading");

		$http({
			method: 'POST',
			url: '../../../Customers/SaveAddMoralCustomer',
			params: {
				Name: $scope.newMoralCustomer.Name,
				RFC: $scope.newMoralCustomer.RFC,
				CelPhone: $scope.newMoralCustomer.CelPhone,
				Phone: $scope.newMoralCustomer.Phone,
				Mail: $scope.newMoralCustomer.Mail,
				WebSite: $scope.newMoralCustomer.WebSite,
				Nationality: $scope.newMoralCustomer.Nationality.id,
				Street: $scope.newMoralCustomer.Street,
				OutNumber: $scope.newMoralCustomer.OutNumber,
				IntNumber: $scope.newMoralCustomer.IntNumber,
				Suburb: $scope.newMoralCustomer.Suburb,
				Town: ($scope.newMoralCustomer.Town == undefined || $scope.newMoralCustomer.Town == 0 || $scope.newMoralCustomer.Town == null) ? null : $scope.newMoralCustomer.Town.idMunicipio,
				PC: $scope.newMoralCustomer.PC,
				ContactName: $scope.newMoralCustomer.ContactName,
				ContactPhone: $scope.newMoralCustomer.ContactPhone,
				ContactMail: $scope.newMoralCustomer.ContactMail,
				Credit: $scope.newMoralCustomer.Credit,
				TimeOfCredit: $scope.newMoralCustomer.TimeOfCredit,
				CreditLimit: $scope.newMoralCustomer.CreditLimit,
				idOrigin: $scope.idOrigin
			}
		}).
			success(function (data, status, headers, config) {

				$("#AddMoralCustomer").button("reset");

				if (data.success == 1) {

					$("#addMoralCustomerForm").reset();

					notify(data.oData.Message, $rootScope.success);

					if (reload == true) {

						$window.location = '../../../Customers/Index';

					} else {

						$("#openModalAddMoralCustomer").modal("hide");

					}


				} else if (data.failure == 1) {

					notify(data.oData.Error, $rootScope.error);

				} else if (data.noLogin == 1) {

					window.location = "../../../Access/Close"

				}

			}).
			error(function (data, status, headers, config) {

				$("#AddMoralCustomer").button("reset");

				notify("Ocurrío un error.", $rootScope.error);

			});

	};

	$scope.SaveUpdateMoral = function (ID) {
		if ($scope.addMoralCustomerForm.$valid) {
			$scope.SaveUpdateMoralCustomer(ID);
		}
	};

	$scope.SaveUpdateMoralCustomer = function (ID) {

		$("#UpdateMoralCustomer").button("loading");

		$http({
			method: 'POST',
			url: '../../../Customers/SaveUpdateMoralCustomer',
			params: {
				idCustomer: ID,
				Name: $scope.newMoralCustomer.Name,
				RFC: $scope.newMoralCustomer.RFC,
				CelPhone: $scope.newMoralCustomer.CelPhone,
				Phone: $scope.newMoralCustomer.Phone,
				Mail: $scope.newMoralCustomer.Mail,
				WebSite: $scope.newMoralCustomer.WebSite,
				Nationality: $scope.newMoralCustomer.Nationality.id,
				Street: $scope.newMoralCustomer.Street,
				OutNumber: $scope.newMoralCustomer.OutNumber,
				IntNumber: $scope.newMoralCustomer.IntNumber,
				Suburb: $scope.newMoralCustomer.Suburb,
				Town: ($scope.newMoralCustomer.Town == undefined || $scope.newMoralCustomer.Town == 0 || $scope.newMoralCustomer.Town == null) ? null : $scope.newMoralCustomer.Town.idMunicipio,
				PC: $scope.newMoralCustomer.PC,
				ContactName: $scope.newMoralCustomer.ContactName,
				ContactPhone: $scope.newMoralCustomer.ContactPhone,
				ContactMail: $scope.newMoralCustomer.ContactMail,
				Credit: $scope.newMoralCustomer.Credit,
				TimeOfCredit: $scope.newMoralCustomer.TimeOfCredit,
				CreditLimit: $scope.newMoralCustomer.CreditLimit,
				idOrigin: $scope.newMoralCustomer.idOrigin
			}
		}).
			success(function (data, status, headers, config) {

				$("#UpdateMoralCustomer").button("reset");

				if (data.success == 1) {

					notify(data.oData.Message, $rootScope.success);

					$window.location = '../../../Customers/Index';

				} else if (data.failure == 1) {

					notify(data.oData.Error, $rootScope.error);

				} else if (data.noLogin == 1) {

					window.location = "../../../Access/Close"

				}

			}).
			error(function (data, status, headers, config) {

				$("#UpdateMoralCustomer").button("reset");

				notify("Ocurrío un error.", $rootScope.error);

			});

	};

	$scope.newPhysicalCustomer = new models.PhysicalCustomers();

	$scope.SaveAddPhysical = function (reload) {

		if ($scope.addPhysicalCustomerForm.$valid) {

			$scope.SaveAddPhysicalCustomer(reload);

		}

	};

	$scope.SaveAddPhysicalCustomer = function (reload) {

		$("#AddPhysicalCustomer").button("loading");

		$http({
			method: 'POST',
			url: '../../../Customers/SaveAddPhysicalCustomers',
			params: {
				Name: $scope.newPhysicalCustomer.Name,
				LastName: $scope.newPhysicalCustomer.LastName,
				Birthday: $scope.newPhysicalCustomer.Birthday,
				Genre: $scope.newPhysicalCustomer.Genre.id,
				Mail: $scope.newPhysicalCustomer.Mail,
				CelPhone: $scope.newPhysicalCustomer.CelPhone,
				Phone: $scope.newPhysicalCustomer.Phone,
				Street: $scope.newPhysicalCustomer.Street,
				OutNumber: $scope.newPhysicalCustomer.OutNumber,
				IntNumber: $scope.newPhysicalCustomer.IntNumber,
				Suburb: $scope.newPhysicalCustomer.Suburb,
				Town: ($scope.newPhysicalCustomer.Town == undefined || $scope.newPhysicalCustomer.Town == 0 || $scope.newPhysicalCustomer.Town == null) ? null : $scope.newPhysicalCustomer.Town.idMunicipio,
				PC: $scope.newPhysicalCustomer.PC,
				CardId: $scope.newPhysicalCustomer.CardId,
				RFC: $scope.newPhysicalCustomer.RFC,
				Credit: $scope.newPhysicalCustomer.Credit,
				TimeOfCredit: $scope.newPhysicalCustomer.TimeOfCredit,
				CreditLimit: $scope.newPhysicalCustomer.CreditLimit,
				IntermediaryName: $scope.newPhysicalCustomer.IntermediaryName,
				IntermediaryPhone: $scope.newPhysicalCustomer.IntermediaryPhone,
				idOrigin: $scope.idOrigin
			}
		}).
			success(function (data, status, headers, config) {

				$("#AddPhysicalCustomer").button("reset");

				if (data.success == 1) {

					$("#addPhysicalCustomerForm").reset();

					notify(data.oData.Message, $rootScope.success);

					if (reload == true) {

						$window.location = '../../../Customers/Index';

					} else {

						$("#openModalAddPhysicalCustomer").modal("hide");

					}

				} else if (data.failure == 1) {

					notify(data.oData.Error, $rootScope.error);

				} else if (data.noLogin == 1) {

					window.location = "../../../Access/Close"

				}

			}).
			error(function (data, status, headers, config) {

				$("#AddPhysicalCustomer").button("reset");

				notify("Ocurrío un error.", $rootScope.error);

			});

	};

	$scope.SaveUpdatePhysical = function (ID) {

		if ($scope.addPhysicalCustomerForm.$valid) {

			$scope.SaveUpdatePhysicalCustomer(ID);

		}

	};

	$scope.SaveUpdatePhysicalCustomer = function (ID) {

		$("#UpdatePhysicalCustomer").button("loading");

		$http({
			method: 'POST',
			url: '../../../Customers/SaveUpdatePhysicalCustomer',
			params: {
				idCustomer: ID,
				Name: $scope.newPhysicalCustomer.Name,
				LastName: $scope.newPhysicalCustomer.LastName,
				Birthday: $scope.newPhysicalCustomer.Birthday,
				Genre: $scope.newPhysicalCustomer.Genre.id,
				Mail: $scope.newPhysicalCustomer.Mail,
				CelPhone: $scope.newPhysicalCustomer.CelPhone,
				Phone: $scope.newPhysicalCustomer.Phone,
				Street: $scope.newPhysicalCustomer.Street,
				OutNumber: $scope.newPhysicalCustomer.OutNumber,
				IntNumber: $scope.newPhysicalCustomer.IntNumber,
				Suburb: $scope.newPhysicalCustomer.Suburb,
				Town: ($scope.newPhysicalCustomer.Town == undefined || $scope.newPhysicalCustomer.Town == 0 || $scope.newPhysicalCustomer.Town == null) ? null : $scope.newPhysicalCustomer.Town.idMunicipio,
				PC: $scope.newPhysicalCustomer.PC,
				CardId: $scope.newPhysicalCustomer.CardId,
				RFC: $scope.newPhysicalCustomer.RFC,
				Credit: $scope.newPhysicalCustomer.Credit,
				TimeOfCredit: $scope.newPhysicalCustomer.TimeOfCredit,
				CreditLimit: $scope.newPhysicalCustomer.CreditLimit,
				IntermediaryName: $scope.newPhysicalCustomer.IntermediaryName,
				IntermediaryPhone: $scope.newPhysicalCustomer.IntermediaryPhone,
				idOrigin: $scope.newPhysicalCustomer.idOrigin
			}
		}).
			success(function (data, status, headers, config) {

				$("#UpdatePhysicalCustomer").button("reset");

				if (data.success == 1) {

					notify(data.oData.Message, $rootScope.success);

					$window.location = '../../../Customers/Index';

				} else if (data.failure == 1) {

					notify(data.oData.Error, $rootScope.error);

				} else if (data.noLogin == 1) {

					window.location = "../../../Access/Close"

				}

			}).
			error(function (data, status, headers, config) {

				$("#UpdatePhysicalCustomer").button("reset");

				notify("Ocurrío un error.", $rootScope.error);

			});

	};

	$scope.GetNationalities = function () {

		$scope.NationalitiesData = [
			{ id: 0, name: 'Mexicano' },
			{ id: 1, name: 'Extranjero' }
		];
		$scope.setSelecNationality($scope.SelectedNationality);

	};

	$scope.GetGenres = function () {

		$scope.GenresData = [
			{ id: 0, name: 'Masculino' },
			{ id: 1, name: 'Femenino' }
		];

		$scope.setSelecGenre($scope.SelectedGenre);

	};

	$scope.setSelecNationality = function (a) {
		var index = $scope.arrayObjectIndexOf($scope.NationalitiesData, parseInt(a), 'id');
		$scope.newMoralCustomer.Nationality = $scope.NationalitiesData[index];
	};

	$scope.setSelecGenre = function (a) {
		var index = $scope.arrayObjectIndexOf($scope.GenresData, parseInt(a), 'id');

		$scope.newPhysicalCustomer.Genre = $scope.GenresData[index];

	};

	$scope.setSelecState = function (a) {

		var index = $scope.arrayObjectIndexOf($scope.States, parseInt(a), 'idEstado');

		$scope.state = $scope.States[index];

	};

	$scope.setSelecTown = function (a, b) {

		var index = $scope.arrayObjectIndexOf($scope.Towns, parseInt(a), 'idMunicipio');

		(b == "M") ? $scope.newMoralCustomer.Town = $scope.Towns[index] : $scope.newPhysicalCustomer.Town = $scope.Towns[index];

	};

	$scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
		for (var i = 0, len = myArray.length; i < len; i++) {
			if (myArray[i][property] === searchTerm) return i;
		}
		return -1;
	};

	$scope.arrayObjectIndex = function (myArray, searchTerm) {
		for (var i = 0, len = myArray.length; i < len; i++) {
			if (myArray[i] == searchTerm) return i;
		}
		return -1;
	};

	$scope.Birthday = new Date();

	$scope.toggleMin = function () {
		$scope.minDate = $scope.minDate ? null : new Date();
	};
	$scope.toggleMin();

	$scope.open = function ($event) {
		$event.preventDefault();
		$event.stopPropagation();

		$scope.opened = true;
	};

	$scope.dateOptions = {
		formatYear: 'yyyy',
		startingDay: 1
	};

	$scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate', 'MM/dd/yyyy'];
	$scope.format = $scope.formats[4];

	var tomorrow = new Date();
	tomorrow.setDate(tomorrow.getDate() + 1);
	var afterTomorrow = new Date();
	afterTomorrow.setDate(tomorrow.getDate() + 2);
	$scope.events =
		[
			{
				date: tomorrow,
				status: 'full'
			},
			{
				date: afterTomorrow,
				status: 'partially'
			}
		];

	$scope.getStates = function (tipo) {
		$http({
			method: 'GET',
			url: '../../../Users/GetStates'
		}).
			success(function (data, status, headers, config) {
				$scope.States = data;
				if ($scope.selectedState > 0) {
					$scope.setSelecState($scope.selectedState);
					$scope.getTowns(tipo);
				}
			}).
			error(function (data, status, headers, config) {
				notify("Ocurrío un error.", $rootScope.error);
			});
	};

	$scope.getTowns = function (tipo) {
		$http({
			method: 'GET',
			url: '../../../Users/GetTowns',
			params: {
				idState: $scope.state.idEstado
			}
		}).
			success(function (data, status, headers, config) {
				$scope.Towns = data;

				if ($scope.selectedTown > 0) {
					$scope.setSelecTown($scope.selectedTown, tipo);
				}
			}).
			error(function (data, status, headers, config) {
				notify("Ocurrío un error.", $rootScope.error);
			});
	};

	$scope.selectedTimeOfCreditMoral = function (timeCredit) {
		var a = $scope.arrayObjectIndex($scope.months, timeCredit);
		$scope.newMoralCustomer.TimeOfCredit = $scope.months[a];
	};

	$scope.selectedTimeOfCreditPhysical = function (timeCredit) {
		var a = $scope.arrayObjectIndex($scope.months, timeCredit);
		$scope.newPhysicalCustomer.TimeOfCredit = $scope.months[a];
	};

	$scope.getOriginCustomer = function () {
		$http({
			method: 'GET',
			url: '../../../CustomerOrigin/Get'
		}).
			success(function (data, status, headers, config) {
				if (data.success == 1) {
					$scope.origins = data.oData.Origins;
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

jQuery.fn.reset = function () {
	$(this).each(function () { this.reset(); });
}
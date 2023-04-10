angular.module("General").controller('UsersController', function (models, userValidator, userUpdateValidator , $scope, $http, $window, notify, $rootScope) {

    $scope.valResult = {};

    $scope.newUser = new models.User();

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;
    $scope.idUser = null;

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
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
        }
    };

    $scope.$watch("currentPage", function (newValue, oldValue) {

        $scope.listUsers();
        $scope.getBranches();

    });

    $scope.onLoad = function () {
        $scope.listUsers();
    };

    $scope.listUsers = function () {

        $http({
            method: 'POST',
            url: '../../../Users/ListUsers',
            params: {
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                 if (data.oData.Users.length > 0) {

                     $scope.Users = data.oData.Users;
                     $scope.total = data.oData.Count;

                 } else {

                     notify('No se encontraron registros.', $rootScope.error);

                     $scope.Users = data.oData.Users;
                     $scope.total = data.oData.Count;

                 }

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }
           
        }).
        error(function (data, status, headers, config) {

            notify(data.oData.Message, $rootScope.error);

        });
    };

    $scope.AddUser = function () {

        $window.location = '../../Users/AddUser';

    };

    $scope.getStates = function () {

        $http({
            method: 'GET',
            url: '../../../Users/GetStates'
        }).
        success(function (data, status, headers, config) {

            $scope.States = data;

            if ($scope.selectedState > 0) {

                $scope.setSelecState($scope.selectedState);

                $scope.getTowns();

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.getTowns = function () {

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

                $scope.setSelecTown($scope.selectedTown);

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.getProfiles = function () {

        $http({
            method: 'POST',
            url: '../../../Profiles/GetProfiles'
        }).
        success(function (data, status, headers, config) {
            $scope.Profiles = data;

            if ($scope.selectedProfile > 0) {

                $scope.setSelecProfile($scope.selectedProfile);

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.getBranches = function () {

        $http({
            method: 'POST',
            url: '../../../Users/GetBranch'
        }).
        success(function (data, status, headers, config) {
            $scope.Branches = data;

            if ($scope.selectedBranch > 0) {

                $scope.setSelecBranch($scope.selectedBranch);

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.SaveAddUser = function () {

        $scope.valResult = userValidator.validate($scope.newUser);

        if ($scope.newUser.$isValid) {

            $scope.SaveAdd();

        }
    };

    $scope.SaveAdd = function () {
        
        $("#SaveAddUser").button("loading");

        $http({
            method: 'POST',
            url: '../../../Users/SaveAddUser',
            params: {
                name: $scope.newUser.Name,
                lastname: $scope.newUser.LastName,
                dtBirth: $scope.dateBirth,
                street: $scope.street,
                extNum: $scope.extNum,
                intNum: $scope.intNum,
                district: $scope.district,
                idTown: ($scope.town == undefined) ? null : $scope.town.idMunicipio,
                CP: $scope.CP,
                telephone: $scope.telephone,
                telcel: $scope.telCel,
                mail: $scope.newUser.Mail,
                sex: $scope.sex,
                idAcademicDegree: ($scope.academicDegree == undefined) ? null : $scope.academicDegree.idNivelAcademico,
                grade: $scope.grade,
                idProfile: $scope.newUser.Profile.idPerfil,
                password: $scope.newUser.Password,
                branch: $scope.newUser.Branch.idSucursal,
                dtInput: $scope.dateInput,
                seller: $scope.seller,
                bill: $scope.bill,
                restricted: $scope.restricted,
                attentionTicket: $scope.attentionTicket
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveAddUser").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $window.location = '../../Users/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveAddUser").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.UpdateUser = function (ID) {

        $window.location = '../../Users/UpdateUser?idUser=' + ID;

    };

    $scope.Commissions = function (ID) {

        $window.location = '../../Commissions/Index?idUser=' + ID;

    };

    $scope.SaveUpdateUser = function (ID) {

        $scope.valResult = userUpdateValidator.validate($scope.newUserUpdate);

        if ($scope.newUserUpdate.$isValid) {

            $scope.SaveUpdate(ID);

        }
    };

    $scope.SaveUpdate = function (ID) {

        $("#SaveUpdateUser").button("loading");

        $http({
            method: 'POST',
            url: '../../../Users/SaveUpdateUser',
            params: {
                idUser: ID,
                name: $scope.newUserUpdate.Name,
                lastname: $scope.newUserUpdate.LastName,
                dtBirth: $scope.dateBirth,
                street: $scope.street,
                extNum: $scope.extNum,
                intNum: $scope.intNum,
                district: $scope.district,
                idTown: ($scope.town == undefined) ? null : $scope.town.idMunicipio,
                CP: $scope.CP,
                telephone: $scope.telephone,
                telcel: $scope.telCel,
                mail: $scope.newUserUpdate.Mail,
                sex: $scope.sex,
                idAcademicDegree: ($scope.academicDegree == undefined) ? null : $scope.academicDegree.idNivelAcademico,
                grade: $scope.grade,
                idProfile: $scope.newUserUpdate.Profile.idPerfil,
                branch: $scope.newUserUpdate.Branch.idSucursal,
                dtInput: $scope.dateInput,
                seller: ($scope.seller == undefined) ? false : $scope.seller,
                bill: $scope.bill,
                restricted: $scope.restricted,
                attentionTicket: $scope.attentionTicket
            }
        }).
        success(function (data, status, headers, config) {

            $("#SaveUpdateUser").button("reset");

            if (data.success == 1) {

                notify(data.oData.Message, $rootScope.success);

                $window.location = '../../Users/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            $("#SaveUpdateUser").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    };

    $scope.UpdatePassword = function (ID) {
        $scope.idUser = ID;
        $("#modalUpdatePassword").modal("show");
    };

    $scope.UpdatePasswordFromUser = function () {

        if ($scope.password == $scope.confirmPassword) {

            $("#btnChangePassword").button("loading");

            $http({
                method: 'POST',
                url: '../../../Users/UpdatePasswordFromUser',
                params: {
                    idUser: $scope.idUser,
                    password: $scope.password
                }
            }).
            success(function (data, status, headers, config) {

                $("#btnChangePassword").button("reset");

                if (data.success == 1) {

                    alert(data.oData.Message);

                } else if (data.failure == 1) {

                    alert(data.oData.Message);
                }

                $("#modalUpdatePassword").modal("hide");

                $scope.password = "";
                $scope.newPassword = "";
                $scope.confirmPassword = "";

            }).
            error(function (data, status, headers, config) {

                $("#btnChangePassword").button("reset");

                notify(data.oData.Message, $rootScope.error);

                $("#modalUpdatePassword").modal("hide");

                $scope.password = "";
                $scope.newPassword = "";
                $scope.confirmPassword = "";

            });

        } else {

            alert("La validación de la contraseña es incorrecta.");

        }

    };

    $scope.setSelecAcademicDegree = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.AcademicDegrees, parseInt(a), 'idNivelAcademico');

        $scope.academicDegree = $scope.AcademicDegrees[index];

    };
    
    $scope.setSelecProfile = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Profiles, parseInt(a), 'idPerfil');

        $scope.newUserUpdate.Profile = $scope.Profiles[index];

    };

    $scope.setSelecBranch = function(a){
    
        var index = $scope.arrayObjectIndexOf($scope.Branches, parseInt(a), 'idSucursal');

        $scope.newUserUpdate.Branch = $scope.Branches[index];

    };

    $scope.setSelecState = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.States, parseInt(a), 'idEstado');

        $scope.state = $scope.States[index];

    };

    $scope.setSelecTown = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.Towns, parseInt(a), 'idMunicipio');

        $scope.town = $scope.Towns[index];

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {
        for (var i = 0, len = myArray.length; i < len; i++) {
            if (myArray[i][property] === searchTerm) return i;
        }
        return -1;
    };

    $scope.UpdateStatusUser = function (idUser, Status) {

        $http({
            method: 'POST',
            url: '../../../Users/UpdateStatusUser',
            params: {
                idUser: idUser,
                status: (Status == 1) ? 0 : 1
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               notify(data.oData.Message, $rootScope.success);

               $scope.listUsers();

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

    $scope.dateBirth = new Date();
    $scope.dateInput = new Date();

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };
    $scope.toggleMin();

    $scope.openBirth = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedBirth = true;
        $scope.openedInput = false;
    };

    $scope.openInput = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedBirth = false;
        $scope.openedInput = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
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

});
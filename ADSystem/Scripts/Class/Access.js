var myInterceptor = function ($q) {
    return {
        request: function (config) {

            return config;
        },

        response: function (result) {

            return result;
        },
        responseError: function (rejection) {

            return $q.reject(rejection);
        }
    }
};

var access = angular.module('access', ['ngFluentValidation',  'ngProgress', 'ngAnimate', 'toaster', 'ui.bootstrap']).config(function ($httpProvider, ngProgressProvider) {

    $httpProvider.interceptors.push(myInterceptor);

    // Default color is firebrick
    ngProgressProvider.setColor('firebrick');
    // Default height is 2px
    ngProgressProvider.setHeight('2px');

});

access.factory('models', function () {

    var models = {};

    models.login = function () {
        this.User = null;
        this.Password = null;
    };

    return models;

});

access.factory('loginValidator', function (validator) {

    var loginValidator = s = new validator();

    s.ruleFor('User').notEmpty().withMessage('Invalid User');
    s.ruleFor('User').length(0,150);

    s.ruleFor('Password').notEmpty();
    s.ruleFor('Password').length(0, 20);
    
    return loginValidator;

});

access.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if(event.which === 13) {
                scope.$apply(function (){
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});

access.factory('notify', ['$window', 'toaster', function (win, toaster) {

    return function (msg, tipo) {

        if (tipo == 1)
            toaster.success(msg);

        else if (tipo == 0)
            toaster.error(msg);

    };
}]);

access.run(function ($rootScope) {
    $rootScope.success = 1;
    $rootScope.error = 0;
});

access.controller('AccessController', function (models, loginValidator, $scope, $http, $window, notify, $rootScope) {
       
    $scope.valResult = {};

    $scope.newLogin = new models.login();

    $scope.save = function () {
                
        if ($scope.frmLogin.$valid) {

            $scope.validateUser();

        }
       
    },

    $scope.validateUser = function () {

        $("#btnAccess").button("loading");

        $http({
            method: 'POST',
            url: '../../Access/ValidateUser',
            params: {
                user: $scope.newLogin.User,
                password: $scope.newLogin.Password
            }
        }).
        success(function (data) {

            $("#btnAccess").button("reset");

            if (data.success == 1) {

                $scope.redirectHome(1);

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data) {

            $("#btnAccess").button("reset");

            notify("Ocurrío un error.", $rootScope.error);

        });

    },
    
    $scope.redirectHome = function (ID) {

        $http({
            method: 'POST',
            url: '../../Access/SetBranch?idBranch=' + ID            
        }).
        success(function (data) {

            if (data.success == 1) {

                $window.location = '../../Home/Index';

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data) {

            notify("Ocurrío un error.", $rootScope.error);

        });

    },

    $scope.openModalRecoveredPassword = function () {

        $("#modalRecoverPassword").modal("show");

    },

     $scope.recoverPassword = function () {

         $http({
             method: 'POST',
             url: '../../../Access/RecoverPassword',
             params: {
                 email: $scope.email
             }
         }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                $("#modalRecoverPassword").modal("hide");

                $scope.email = "";

                notify(data.oData.Message, $rootScope.success);

            } else if (data.failure == 1) {
                              
                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close"

            }

        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

        });

     }
    
});
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
}


angular.module('General', ['ngFluentValidation', 'ngProgress', 'ngAnimate', 'toaster', 'ui.bootstrap', 'acute.select', 'angularFileUpload', 'angucomplete', 'ng-currency', 'angular.filter', 'ngFileUpload']).config(function ($httpProvider, ngProgressProvider) {
    $httpProvider.interceptors.push(myInterceptor);

    // Default color is firebrick
    ngProgressProvider.setColor('firebrick');
    // Default height is 2px
    ngProgressProvider.setHeight('2px');
});

angular.module('General').run(function ($rootScope) {
    $rootScope.success = 1;
    $rootScope.error = 0;
});

angular.module('General').run(function (acuteSelectService) {
    // Use the acute select service to set the template path for all instances
    acuteSelectService.updateSetting("templatePath", "/Scripts/template");
})

angular.module('General').factory('models', function () {

    var models = {};

    models.category = function () {
        this.name = null;
        this.description = null;
    };

    models.subcategory = function () {
        this.name = null;
        this.description = null;
    };

    models.action = function () {
        this.name = null;
        this.description = null;
        this.ajax = null;
        this.parentmenuBool = null;
        this.parentmenu = null;
    };

    models.Brand = function () {
        this.name = null;
        this.description = null;
    };

    models.Products = function () {
        this.Name = null;
        this.ComercialName = null;
        this.Description = null;
        this.BuyPrice = null;
        this.SalePrice = null;
        this.ProveedorId = null;
        this.CategoryId = null;
        this.SubcategoryId = null;
        this.Color = null;
        this.MaterialId = null;
        this.Measure = null;
        this.Weight = null;
        this.Code = null;
        this.lBranches = null;
        this.urlImagen = null;
        this.TipoImagen = null;
        this.Comments = null;
        this.Justify = null;
    }

    models.ProductCategory = function () {
        this.Category = null;
        this.Subcategory = null;
    }

    models.OrderProducts = function () {
        this.SalePrice = null;
        this.ProveedorId = null;
        this.CategoryId = null;
        this.Code = null;
        this.Productquantity = null;
        this.ProductpurchasePrice = null;
        this.ProductbranchQuantityAm = null;
        this.ProductbranchQuantityGua = null;
    }

    models.Provider = function () {
        this.Company = null;
        this.Name = null;
        this.Email = null;
    }

    models.MXNBankAccount = function () {
        this.idCuentaBancaria = null;
        this.Banco = null;
        this.Titular = null;
        this.CLABE = null;
        this.Cuenta = null;
        this.Sucursal = null;
        this.RFC = null;
    }

    models.MoralCustomers = function () {
        this.Name = null;
        this.RFC = null;
        this.CelPhone = null;
        this.Phone = null;
        this.Mail = null;
        this.WebSite = null;
        this.Nationality = null;
        this.Street = null;
        this.OutNumber = null;
        this.IntNumber = null;
        this.Suburb = null;
        this.Town = null;
        this.PC = null;
        this.ContactName = null;
        this.ContactPhone = null;
        this.ContactMail = null;
        this.Credit = null;
        this.TimeOfCredit = null;
        this.CreditLimit = null;
        this.idOrigin = null;
    }

    models.PhysicalCustomers = function () {
        this.Name = null;
        this.LastName = null;
        this.Birthday = null;
        this.Genre = null;
        this.Mail = null;
        this.CelPhone = null;
        this.Phone = null;
        this.Street = null;
        this.OutNumber = null;
        this.IntNumber = null;
        this.Suburb = null;
        this.Town = null;
        this.PC = null;
        this.CardId = null;
        this.RFC = null;
        this.Intermediary = null;
        this.Credit = null;
        this.TimeOfCredit = null;
        this.CreditLimit = null;
        this.idOrigin = null;
    }

    models.ProductOrder = function () {
        this.selectedCodigo = null;
        this.quantity = null;
        this.purchasePrice = null;
        this.branchQuantityAm = null;
        this.branchQuantityGua = null;
        this.branchQuantityTex = null;
    }

    models.Catalog = function () {
        this.Name = null;
        this.Brand = null;
        this.Category = null;
    }

    models.Order = function () {
        this.Order = null;
        this.Factura = null;
        this.Provider = null;
    }

    models.User = function () {
        this.Name = null;
        this.LastName = null;
        this.Profile = null;
        this.Branch = null;
        this.Mail = null;
        this.Password = null;
        this.ConfirmPassword = null;
    }

    models.Material = function () {
        this.Material = null;
    }

    models.Office = function () {
        this.Name = null;
        this.Phone = null;
        this.Street = null;
        this.ExtNum = null;
        this.IntNum = null;
        this.Neighborhood = null;
        this.idTown = null;
        this.CP = null;
        this.Email = null;
        this.Percentage = null;
        this.idOrigin = null;
    }

    models.Credit = function () {
        this.Amount = null;
    }

    models.ServiceSale = function () {
        this.idService = null;
        this.descService = null;
        this.imagen = null;
        this.salePriceService = null;
        this.amountService = null;
        this.commentsService = null;
    }

    models.ServicePreQuotation = function () {
        this.idService = null;
        this.descService = null;
        this.imagen = null;
        this.amountService = null;
        this.commentsService = null;
    }

    models.Payment = function () {
        this.idSalePayment = null;
        this.paymentAmount = null;
        this.paymentComment = null;
        this.typesPayment = null;
        this.paymentDate = null;
        this.typesCard = null;
        this.holder = null;
        this.bank = null;
        this.numCheck = null;
        this.numIFE = null;
        this.paymentLeft = null;
        this.creditnote = null;
        this.idCreditNote = null;
    }

    models.Service = function () {
        this.Description = null;
        this.Installation = null;
    }

    models.DetailSaleFactura = function () {
        this.Factura = null;
    }

    models.Bill = function () {
        this.typeBill = null;
        this.rfcBill = null;
        this.conceptBill = null;
        this.nameBill = null;
        this.phoneBill = null;
        this.mailBill = null;
        this.streetBill = null;
        this.outNumberBill = null;
        this.intNumberBill = null;
        this.suburbBill = null;
        this.stateBill = null;
        this.townBill = null;
        this.cpBill = null;
    }

    models.Entry = function () {
        this.Type = 1;
        this.idSale = null;
        this.Date = null;
        this.DeliveredBy = null;
        this.DeliveredAnotherBy = null;
        this.Amount = null;
        this.Comments = null;
    }

    models.Egress = function () {
        this.Date = null;
        this.Type = 1;
        this.idEntry = null;
        this.idSale = null;
        this.fkEgress = null;
        this.ReceivedBy = null;
        this.ReceivedAnotherBy = null;
        this.Amount = null;
        this.Comments = null;
    }

    models.CashReport = function () {
        this.Comments = null;
    }

    return models;
});

angular.module('General').factory('entryValidator', function (validator) {
    var entryValidator = s = new validator();

    s.ruleFor('DeliveredBy').notEmpty().withMessage('Entregado por, es requerido.');
    s.ruleFor('DeliveredBy').length(0, 150).withMessage('Longitud debe ser menor a 150 dígitos');
    s.ruleFor('Amount').notEmpty().withMessage('Cantidad, es requerido.');
    s.ruleFor('Comments').length(0, 350).withMessage('Longitud debe ser menor a 350 dígitos');

    return entryValidator;
});

angular.module('General').factory('egressValidator', function (validator) {
    var egressValidator = s = new validator();

    s.ruleFor('DeliveredBy').notEmpty().withMessage('Entregado por, es requerido.');
    s.ruleFor('DeliveredBy').length(0, 150).withMessage('Longitud debe ser menor a 150 dígitos');
    s.ruleFor('Amount').notEmpty().withMessage('Cantidad, es requerido.');
    s.ruleFor('Comments').length(0, 350).withMessage('Longitud debe ser menor a 350 dígitos');

    return egressValidator;
});

angular.module('General').factory('categoryValidator', function (validator) {
    var categoryValidator = s = new validator();

    s.ruleFor('name').notEmpty().withMessage('Nombre es requerido.');
    s.ruleFor('name').length(0, 100).withMessage('Longitud debe ser menor a 100 dígitos');

    return categoryValidator;
});

angular.module('General').factory('subcategoryValidator', function (validator) {
    var subcategoryValidator = s = new validator();

    s.ruleFor('name').notEmpty().withMessage('Nombre es requerido.');
    s.ruleFor('name').length(0, 100).withMessage('La longitud de Nombre debe ser menor a 100 dígitos');

    return subcategoryValidator;
});

angular.module('General').factory('ProductValidator', function (validator) {
    var ProductValidator = s = new validator();

    s.ruleFor('Code').notEmpty().withMessage('Código es requerido.');
    s.ruleFor('Code').length(0, 100).withMessage('La longitud de Código debe ser menor a 100 dígitos');
    s.ruleFor('SalePrice').notEmpty().withMessage('Precio de Venta es requerido.');
    s.ruleFor('ProveedorId').notEmpty().withMessage('Proveedor es requerido.');
    s.ruleFor('CategoryId').notEmpty().withMessage('Categoria es requerida.');

    return ProductValidator;
});

angular.module('General').factory('ProductEditValidator', function (validator) {
    var ProductValidator = s = new validator();

    s.ruleFor('Code').notEmpty().withMessage('Código es requerido.');
    s.ruleFor('Code').length(0, 100).withMessage('La longitud de Código debe ser menor a 100 dígitos');
    s.ruleFor('SalePrice').notEmpty().withMessage('Precio de Venta es requerido.');
    s.ruleFor('ProveedorId').notEmpty().withMessage('Proveedor es requerido.');
    s.ruleFor('CategoryId').notEmpty().withMessage('Categoria es requerida.');
    s.ruleFor('Justify').notEmpty().withMessage('Justificación es requerida.');

    return ProductValidator;
});

angular.module('General').factory('ProductOnlineValidator', function (validator) {
    var ProductValidator = s = new validator();

    s.ruleFor('Code').notEmpty().withMessage('Código es requerido.');
    s.ruleFor('Code').length(0, 100).withMessage('La longitud de Código debe ser menor a 100 dígitos');
    s.ruleFor('SalePrice').notEmpty().withMessage('Precio de Venta es requerido.');
    s.ruleFor('ProveedorId').notEmpty().withMessage('Proveedor es requerido.');
    s.ruleFor('Justify').notEmpty().withMessage('Justificación es requerida.');

    return ProductValidator;
});

angular.module('General').factory('OrderProductValidator', function (validator) {
    var OrderProductValidator = s = new validator();

    s.ruleFor('Code').notEmpty().withMessage('Código es requerido.');    
    s.ruleFor('Code').length(0, 100).withMessage('La longitud de Código debe ser menor a 100 dígitos');
    s.ruleFor('SalePrice').notEmpty().withMessage('Precio de Venta es requerido.');
    s.ruleFor('CategoryId').notEmpty().withMessage('Categoria es requerida.');
    s.ruleFor("Productquantity").notEmpty().withMessage('Cantidad de productos es requerida.');
    s.ruleFor("ProductpurchasePrice").notEmpty().withMessage('Precio de compra es requerida.');
    s.ruleFor("ProductbranchQuantityAm").notEmpty().withMessage('Cantidad de productos de Amazonas es requerida.');
    s.ruleFor("ProductbranchQuantityGua").notEmpty().withMessage('Cantidad de productos de Guadalquivir es requerida.');
    s.ruleFor("ProductbranchQuantityTex").notEmpty().withMessage('Cantidad de productos de Textura es requerida.');

    return OrderProductValidator;
});

angular.module('General').factory('actionValidator', function (validator) {
    var actionValidator = s = new validator();

    s.ruleFor('name').notEmpty().withMessage('Accion es requerido.');
    s.ruleFor('name').length(0, 100).withMessage('La longitud de la Accion debe ser menor a 100 dígitos');
    s.ruleFor('ajax').notEmpty();
    s.ruleFor('parentmenuBool').notEmpty();
    s.ruleFor('parentmenu').notEmpty();

    return actionValidator;
});

angular.module('General').factory('brandValidator', function (validator) {
    var brandValidator = s = new validator();

    s.ruleFor('name').notEmpty().withMessage('Marca es requerido.');
    s.ruleFor('name').length(0, 60).withMessage('La longitud de la Marca debe ser menor a 60 dígitos');

    return brandValidator;
});

angular.module('General').factory('catalogValidator', function (validator) {
    var catalogValidator = s = new validator();

    s.ruleFor('Name').notEmpty().withMessage('Catálogo es requerido.');
    s.ruleFor('Name').length(0, 60).withMessage('La longitud de Catálogo debe ser menor a 60 dígitos');
    s.ruleFor('Category').notEmpty().withMessage('Categoría es requerido.');


    return catalogValidator;
});

angular.module('General').factory('providerValidator', function (validator) {
    var ProviderValidator = s = new validator();

    s.ruleFor('Company').notEmpty().withMessage('Nombre de la Empresa es requerido');
    s.ruleFor('Company').length(0, 150).withMessage('La longitud de Nombre de la Empresa debe ser menor a 150 dígitos');

    return ProviderValidator;
});

angular.module('General').factory('MoralCustomerValidator', function (validator) {
    var MoralCustomerValidator = s = new validator();

    s.ruleFor('Name').notEmpty().withMessage('Nombre es requerido');
    s.ruleFor('Name').length(0, 100).withMessage('La longitud de Nombre debe ser menor a 100 dígitos');
    s.ruleFor('Mail').notEmpty().withMessage('Correo es requerido');

    return MoralCustomerValidator;
});

angular.module('General').factory('PhysicalCustomerValidator', function (validator) {

    var PhysicalCustomerValidator = s = new validator();

    s.ruleFor('Name').notEmpty().withMessage('Nombre es requerido');
    s.ruleFor('Name').length(0, 100).withMessage('La longitud de Nombre debe ser menor a 100 dígitos');
    s.ruleFor('Mail').notEmpty().withMessage('Correo es requerido');

    return PhysicalCustomerValidator;

});

angular.module('General').factory('productOrderValidator', function (validator) {
    var productOrderValidator = s = new validator();

    s.ruleFor('selectedCodigo').notEmpty().withMessage('Producto es requerido.');
    s.ruleFor('quantity').notEmpty().withMessage('Cantidad es requerido.');
    s.ruleFor('purchasePrice').notEmpty().withMessage('Precio de compra es requerido.');
    s.ruleFor('salePrice').notEmpty().withMessage('Precio de venta es requerido.');
    s.ruleFor('branchQuantityAm').notEmpty().withMessage('Cantidad asignada a Amazonas es requerido.');
    s.ruleFor('branchQuantityGua').notEmpty().withMessage('Cantidad asignada a Guadalquivir es requerido.');
    s.ruleFor('branchQuantityTex').notEmpty().withMessage('Cantidad asignada a Textura es requerido.');

    return productOrderValidator;
});

angular.module('General').factory('UpdateOrderProductValidator', function (validator) {
    var UpdateOrderProductValidator = s = new validator();

    s.ruleFor('nameProduct').notEmpty().withMessage('Nombre es requerido.');
    s.ruleFor('quantity').notEmpty().withMessage('Cantidad es requerido.');
    s.ruleFor('purchasePrice').notEmpty().withMessage('Precio de compra es requerido.');
    s.ruleFor('salePrice').notEmpty().withMessage('Precio de venta es requerido.');

    return UpdateOrderProductValidator;
});

angular.module('General').factory('orderValidator', function (validator) {
    var orderValidator = s = new validator();
        
    s.ruleFor('Provider').notEmpty().withMessage('Empresa es requerido');

    return orderValidator;
});

angular.module('General').factory('userValidator', function (validator) {
    var userValidator = s = new validator();

    s.ruleFor('Name').notEmpty().withMessage('Nombre es requerido');
    s.ruleFor('LastName').notEmpty().withMessage('Apellidos es requerido');
    s.ruleFor('Profile').notEmpty().withMessage('Perfil es requerido');
    s.ruleFor('Branch').notEmpty().withMessage('Sucursal es requerido');
    s.ruleFor('Mail').notEmpty().withMessage('Correo es requerido');
    s.ruleFor('Password').notEmpty().withMessage('Contraseña es requerida');
    s.ruleFor('ConfirmPassword').notEmpty().withMessage('Confirmar contraseña es requerida');

    return userValidator;
});

angular.module('General').factory('userUpdateValidator', function (validator) {
    var userUpdateValidator = s = new validator();

    s.ruleFor('Name').notEmpty().withMessage('Nombre es requerido');
    s.ruleFor('LastName').notEmpty().withMessage('Apellidos es requerido');
    s.ruleFor('Profile').notEmpty().withMessage('Perfil es requerido');
    s.ruleFor('Branch').notEmpty().withMessage('Sucursal es requerido');
    s.ruleFor('Mail').notEmpty().withMessage('Correo es requerido');

    return userUpdateValidator;
});

angular.module('General').factory('materialValidator', function (validator) {
    var materialValidator = s = new validator();

    s.ruleFor('Material').notEmpty().withMessage('Material es requerido');
    s.ruleFor('Material').length(0, 100).withMessage('La longitud debe ser menor a 100 dígitos');

    return materialValidator;
});

angular.module('General').factory('officeValidator', function (validator) {
    var officeValidator = s = new validator();

    s.ruleFor('Name').notEmpty().withMessage('Nombre es requerido');
    s.ruleFor('Email').notEmpty().withMessage('Correo es requerido');

    return officeValidator;
});

angular.module('General').factory('CreditValidator', function (validator) {
    var CreditValidator = s = new validator();

    s.ruleFor('Amount').notEmpty().withMessage('Cantidad es requerido');

    return CreditValidator;
});

angular.module('General').factory('ServiceSaleValidator', function (validator) {
    var ServiceSaleValidator = s = new validator();

    s.ruleFor('descService').notEmpty().withMessage('Servicio es requerido');
    s.ruleFor('salePriceService').notEmpty().withMessage('Precio es requerido');
    s.ruleFor('amountService').notEmpty().withMessage('Cantidad es requerido');

    return ServiceSaleValidator;
});

angular.module('General').factory('PaymentValidator', function (validator) {
    var PaymentValidator = s = new validator();

    s.ruleFor('paymentAmount').notEmpty().withMessage('Cantidad es requerido');
    s.ruleFor('typesPayment').notEmpty().withMessage('Forma de pago es requerido');
    s.ruleFor('paymentDate').notEmpty().withMessage('Fecha es requerido');

    return PaymentValidator;
});

angular.module('General').factory('ServiceValidator', function (validator) {
    var ServiceValidator = s = new validator();
    s.ruleFor('Description').notEmpty().withMessage('Descripción es requerida');
    return ServiceValidator;
});

angular.module('General').factory('DetailSaleFacturaValidator', function (validator) {
    var DetailSaleFacturaValidator = s = new validator();
    s.ruleFor('Factura').notEmpty();
    return DetailSaleFacturaValidator;
});

angular.module('General').controller('GeneralController', function ($scope, $timeout, ngProgress, toaster, $http, $window, notify, $rootScope) {
    $scope.show = false;

    $scope.color = ngProgress.color();
    $scope.height = ngProgress.height();

    ngProgress.start();
    $timeout(function () {
        ngProgress.complete();
        $scope.show = true;
    }, 2000);

    $scope.init = function () {
        $("#modalDeleteItem .confirmbutton-yes").text("Si");
        $("#modalDeleteItem .confirmbutton-no").text("No");
    }

    $scope.closeSession = function () {

        $window.location = '../../Access/Close';

    };

    $scope.homeRedirect = function () {
        $window.location = '../../Home/Index';
    };


    $scope.openModalUpdatePassword = function () {
        $("#modalUpdatePassword").modal("show");
    };

    $scope.updatePassword = function () {
        if ($scope.newPassword == $scope.confirmPassword) {

            $http({
                method: 'POST',
                url: '../../../Users/UpdatePassword',
                params: {
                    password: $scope.password,
                    newPassword: $scope.newPassword
                }
            }).
            success(function (data, status, headers, config) {

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

    $scope.updateUserActual = function () {
        $window.location = '../../Users/UpdateUserActual';
    };

    $scope.buttonDisabled = false;
});

angular.module('General').factory('notify', ['$window', 'toaster', function (win, toaster) {

    return function (msg, tipo) {

        if (tipo == 1)
            toaster.success({ title: "", body: msg, bodyOutputType: 'trustedHtml' });

        else if (tipo == 0)
            toaster.error({ title: "", body: msg, bodyOutputType: 'trustedHtml' });

    };
}]);

angular.module('General').factory('GUID', [function () {
        
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }

    function New() {
        return s4() + s4() + '' + s4() + '' + s4() + '' +
          s4() + '' + s4() + s4() + s4();

    }

    return {
        New: New
    }

}]);

angular.module('General').filter("jsDate", function () {
    return function (x) {
        if (x != null && x != false) {
            return new Date(parseInt(x.substr(6)));
        }
    };
});

angular.module('General').factory("GetDiscount", ['$http', function ($http) {
    return GetDiscount = function (amount) {
        return $http.post('../../../Discounts/GetDiscount', { amount: amount });        
    }
}]);

angular.module('General').factory("GetDiscountOffice", ['$http', function ($http) {
    return GetDiscountOffice = function (amount) {
        return $http.post('../../../Discounts/GetDiscountOffice', { amount: amount });
    }
}]);

angular.module('General').factory("GetDiscountSpecial", ['$http', function ($http) {
    return GetDiscountSpecial = function (amount) {
        return $http.post('../../../Discounts/GetDiscountSpecial', { amount: amount });
    }
}]);


/**
 * A generic confirmation for risky actions.
 * Usage: Add attributes: ng-really-message="Really?" ng-really-click="takeAction()" function
 */
angular.module('General').directive('ngReallyClick', [function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.bind('click', function () {
                var message = attrs.ngReallyMessage;

                $(".dial-text").html(message);
                $("#modalDeleteItem").modal("show");

                pop = $("#modalDeleteItem");

                pop.find('.confirmbutton-yes').click(function (e) {
                    scope.$apply(attrs.ngReallyClick);
                    e.stopImmediatePropagation();
                    e.preventDefault();
                    $("#modalDeleteItem").modal('hide');
                });

                pop.find('.confirmbutton-no').click(function (e) {

                    $("#modalDeleteItem").modal('hide');
                });

            });
        }
    }
}]);

angular.module('General').directive('ngReallyYes', [function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.bind('click', function () {
                var message = attrs.ngReallyMessage;

                $(".dial-text").html(message);
                $("#modalDeleteItem").modal("show");

                pop = $("#modalDeleteItem");

                pop.find('.confirmbutton-yes').click(function (e) {
                    scope.$apply(attrs.ngReallyYes);
                    e.stopImmediatePropagation();
                    e.preventDefault();
                    $("#modalDeleteItem").modal('hide');
                });

                pop.find('.confirmbutton-no').click(function (e) {
                    scope.$apply(attrs.ngReallyNo);
                    e.stopImmediatePropagation();
                    e.preventDefault();
                    $("#modalDeleteItem").modal('hide');
                });

            });
        }
    }
}]);

angular.module('General').directive('uppercase', function (uppercaseFilter, $parse) {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            var capitalize = function (inputValue) {

                if (inputValue != undefined || inputValue != null) {

                    var capitalized = inputValue.toUpperCase();
                    if (capitalized !== inputValue) {
                        modelCtrl.$setViewValue(capitalized);
                        modelCtrl.$render();
                    }
                    return capitalized;

                }
            }
            var model = $parse(attrs.ngModel);
            modelCtrl.$parsers.push(capitalize);
            capitalize(model(scope));
        }
    };
});

angular.module('General').directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            modelCtrl.$parsers.push(function (inputValue) {
                // this next if is necessary for when using ng-required on your input. 
                // In such cases, when a letter is typed first, this parser will be called
                // again, and the 2nd time, the value will be undefined
                if (inputValue == undefined) return ''
                var transformedInput = inputValue.replace(/[^0-9]/g, '');
                if (transformedInput != inputValue) {
                    modelCtrl.$setViewValue(transformedInput);
                    modelCtrl.$render();
                }

                return transformedInput;
            });
        }
    };
});

angular.module('General').directive('productssale', function ($compile) {
    function link(scope, element, attrs) {
        scope.$watch(
          function (scope) {
              return scope.$eval(attrs.productssale);
          },
          function (value) {
              element.append($compile(value)(scope));
          }
        );
    };

    return {
        link: link
    }
});

angular.module('General').directive('placeLocation', function () {
    return {
        restrict: 'E',
        template: "<div></div>",
        replace: true,
        link: function (scope, element, attrs) {
            var myLatLng = new google.maps.LatLng(20.638974, -103.396820);
            var mapOptions = {
                center: myLatLng,
                scrollwheel: false,
                zoom: 15
            };
            var map = new google.maps.Map(document.getElementById(attrs.id),
                mapOptions);
            var marker = new google.maps.Marker({
                position: myLatLng,
                map: map
            });

            //Autocomplete
            var input = document.getElementById('location');

            var autocomplete = new google.maps.places.Autocomplete(input);
            autocomplete.bindTo('bounds', map);

            var infowindow = new google.maps.InfoWindow();

            var marker = new google.maps.Marker({
                map: map,
                anchorPoint: new google.maps.Point(0, -29)
            });

            autocomplete.addListener('place_changed', function () {
                infowindow.close();
                marker.setVisible(false);
                var place = autocomplete.getPlace();
                if (!place.geometry) {
                    // User entered the name of a Place that was not suggested and
                    // pressed the Enter key, or the Place Details request failed.
                    window.alert("No details available for input: '" + place.name + "'");
                    return;
                }

                // If the place has a geometry, then present it on a map.
                if (place.geometry.viewport) {
                    map.fitBounds(place.geometry.viewport);
                } else {
                    map.setCenter(place.geometry.location);
                    map.setZoom(17);  // Why 17? Because it looks good.
                }
                marker.setIcon(/** @type {google.maps.Icon} */({
                    url: place.icon,
                    size: new google.maps.Size(71, 71),
                    origin: new google.maps.Point(0, 0),
                    anchor: new google.maps.Point(17, 34),
                    scaledSize: new google.maps.Size(35, 35)
                }));
                marker.setPosition(place.geometry.location);
                marker.setVisible(true);

                var address = '';
                if (place.address_components) {
                    address = [
                      (place.address_components[0] && place.address_components[0].short_name || ''),
                      (place.address_components[1] && place.address_components[1].short_name || ''),
                      (place.address_components[2] && place.address_components[2].short_name || '')
                    ].join(' ');
                }

                infowindow.setContent('<div><strong>' + place.name + '</strong><br>' + address);
                infowindow.open(map, marker);

                scope.latitud = place.geometry.location.lat();
                scope.longitud = place.geometry.location.lng();

                scope.location = place.name + " " + place.formatted_address;
            });

            //marker.setMap(map);

            scope.$watch('selected', function () {

                window.setTimeout(function () {

                    google.maps.event.trigger(map, 'resize');
                }, 100);

            });

        }
    }
});

angular.module('General').directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});




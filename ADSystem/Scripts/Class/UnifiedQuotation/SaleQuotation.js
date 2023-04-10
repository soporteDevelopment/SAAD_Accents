angular.module("General").controller('QuotationsController', function (models, ServiceSaleValidator, PaymentValidator, DetailSaleFacturaValidator, GUID, GetDiscount, GetDiscountOffice, GetDiscountSpecial, $scope, $http, $window, notify, $rootScope, FileUploader) {

    $scope.branch = "";
    $scope.branchID = 1;
    $scope.quotations = new Array();
    $scope.subTotal = 0;
    $scope.checkedDiscount = false;
    $scope.checkedIVA = false;
    $scope.checkedIVAPayment = false;
    $scope.IVATasa = 0;
    $scope.discount = 0;
    $scope.total = 0;
    $scope.includeURLMoral = "";
    $scope.includeURLPhysical = "";
    $scope.Math = window.Math;
    $scope.myFiles = [];
    $scope.bankAccountStatus = [];
    $scope.terminalTypeStatus = [];

    $scope.typesPaymentItems = new Array();
    $scope.paymentMonths = new Array();
    $scope.Allpayments = {};

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;
    $scope.total = 0;

    $scope.customer = {
        type: "moral"
    };    

    $scope.GetNumberRem = function () {
        $http({
            method: 'POST',
            url: '../../../Sales/GeneratePrevNumberRemC'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.remision = data.oData.sRemision;
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

    $scope.setIdBranch = function () {
        if ($scope.branchadd.prod == "A") { $scope.idSucursal = 2; } else if ($scope.branchadd.prod == "G") { $scope.idSucursal = 3; } else { $scope.idSucursal = 4; }
    };

    //Obtiene la información de una sucursal
    $scope.GetBranchInfo = function (IDBranch) {

        $http({
            method: 'GET',
            url: '../../../Branch/GetById',
            params: { id: IDBranch }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {
                    if (data.oData.Branch.IVATasa > $scope.IVATasa) {                        
                        $scope.branch = data.oData.Branch.Nombre;
                        $scope.IVATasa = data.oData.Branch.IVATasa;
                        $scope.branchID = data.oData.Branch.idSucursal;                        
                    }

                    if ($scope.checkedIVA) {
                        $scope.CalculateTotalCost();
                    }

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

    $scope.LoadQuotation = function (quotation) {
        $scope.idUsuario1 = quotation.idUsuario1;
        $scope.idUsuario2 = quotation.idUsuario2;
        $scope.idClienteFisico = quotation.idClienteFisico;
        $scope.idClienteMoral = quotation.idClienteMoral;
        $scope.idDespacho = quotation.idDespacho;
        $scope.project = quotation.Proyecto;
        $scope.checkedOffices = (quotation.idDespachoReferencia != null && quotation.idDespachoReferencia != undefined)
            ? true
            : false;
        $scope.idDespachoReferencia = quotation.idDespachoReferencia;
        $scope.branch = "TODAS";
        $scope.idSucursal = 1;
        $scope.TipoCliente = quotation.TipoCliente;
        $scope.checkedIVA = (quotation.IVA == 1) ? true : false;

        if (quotation.TipoCliente == '0') {
            $scope.customer.type = "physical";
            $scope.LoadPhysicalCustomers();
        } else if (quotation.TipoCliente == '1') {
            $scope.customer.type = "moral";
            $scope.LoadMoralCustomers();
        } else if (quotation.TipoCliente == '2') {
            $scope.customer.type = "office";
            $scope.LoadOffices();
        }

        $scope.LoadUsers();

        $scope.quotations.push({
            idCotizacion: 0,
            Numero: "",
            oDetail: new Array()
        });

        angular.forEach(quotation.lCotizaciones,
            function (quotation, quotationKey) {
                $scope.quotations.push({
                    idCotizacion: quotation.idCotizacion,
                    Numero: quotation.Numero,
                    oDetail: new Array()
                });
            });

        angular.forEach(quotation.lCotizaciones, function (quotation, quotationKey) {            
            if ($scope.checkedIVA) {                
                $scope.GetBranchInfo(quotation.idSucursal);
            }                        
            angular.forEach(quotation.oDetail, function (value, key) {
                if (value.idProducto != null && value.idProducto > 0) {
                    if (value.idVista > 0) {
                        $scope.GetProductForBranch(value.idSucursal, quotation.idCotizacion, value);
                    } else {
                        $scope.GetProductForQuotation(value.idSucursal, quotation.idCotizacion, value);
                    }
                } else {
                    angular.forEach($scope.quotations,
                        function (q, qKey) {
                            if (q.idCotizacion == quotation.idCotizacion) {
                                q.oDetail.push({
                                    idProducto: value.idServicio,
                                    idProveedor: null,
                                    imagen: "../Content/Services/" + value.Imagen,
                                    codigo: "SERVICIO",
                                    desc: value.Descripcion,
                                    prec: value.Precio,
                                    descuento: value.Descuento,
                                    existencia: value.Cantidad,
                                    stock: value.Cantidad,
                                    cantidad: value.Cantidad,
                                    costo: 0,
                                    servicio: true,
                                    credito: false,
                                    comentarios: value.Comentarios,
                                    Sucursal: value.Sucursal,
                                    idSucursal: value.idSucursal,
                                    idVista: value.idVista,
                                    idCotizacion: quotation.idCotizacion
                                });
                            }
                        });
                }
            });
        });

        angular.forEach(quotation.lCotizaciones, function (quotation, quotationKey) {
            angular.forEach(quotation.oDetail, function (value, key) {
                $scope.cantidadProductos = $scope.cantidadProductos + value.Cantidad;
                $scope.sumaSubTotal = $scope.sumaSubTotal + (value.Precio * value.Cantidad);
            });
        });

        $scope.CalculateTotalCost();
        $scope.LoadServices();
    };

    $scope.CalculateTotalCost = function () {

        $scope.subTotal = 0;
        $scope.CantidadProductos = 0;
        $scope.totalCreditNotes = 0;
        $scope.resCredit = 0;
        $scope.discount = ($scope.discount != null) ? $scope.discount : 0;

        angular.forEach($scope.quotations,
            function (quotation, quotationKey) {
                angular.forEach(quotation.oDetail,
                    function (detail, detailKey) {
                        var cost = detail.cantidad * detail.prec;
                        var percentage = detail.descuento / 100;
                        var discount = cost * percentage;
                        detail.costo = cost - (discount);
                        $scope.subTotal = $scope.subTotal + detail.costo;
                        $scope.CantidadProductos = $scope.CantidadProductos + parseInt(detail.cantidad);
                    });
            });

        $scope.IVA = ((($scope.subTotal + $scope.totalCreditNotes) - (($scope.subTotal) * ($scope.discount / 100)))) * ($scope.IVATasa / 100);

        if ($scope.IVA < 0) {
            $scope.IVA = 0;        }

        if ($scope.checkedDiscount == true) {

            var percentage = $scope.discount / 100;

            angular.forEach($scope.quotations,
                function (quotation, quotationKey) {
                    angular.forEach(quotation.oDetail,
                        function (detail, detailKey) {
                            if (detail.descuento > 0 || (detail.servicio == true && detail.desc == 'INSTALACIONES')) {
                                var cost = detail.cantidad * detail.prec;
                                var percentage = detail.descuento / 100;
                                var discount = cost * percentage;
                                detail.costo = cost - (discount);
                            }
                        });
                });

            var discount = 0;
            discount = ($scope.subTotal) * percentage;

            if ($scope.checkedIVA == true) {
                $scope.total = (($scope.subTotal + $scope.totalCreditNotes) - discount) + $scope.IVA;
            } else {
                $scope.total = ($scope.subTotal + $scope.totalCreditNotes) - discount;
            }

        } else {

            $scope.checkedDiscount = false;
            $scope.discount = 0;

            if ($scope.checkedIVA == true) {
                $scope.total = (($scope.subTotal + $scope.totalCreditNotes) + $scope.IVA);
            } else {
                $scope.total = $scope.subTotal + $scope.totalCreditNotes;
            }
        }

        if ($scope.total < 0) {
            $scope.resCredit = $scope.total * -1;
        }

    };

    $scope.CalculateTotalCostWithoutDiscount = function () {

        $scope.subTotal = 0;
        $scope.CantidadProductos = 0;
        $scope.totalCreditNotes = 0;
        $scope.resCredit = 0;
        $scope.discount = ($scope.discount != null) ? $scope.discount : 0;

        angular.forEach($scope.quotations,
            function (quotation, quotationKey) {
                angular.forEach(quotation.oDetail,
                    function (detail, detailKey) {
                        if (detail.credito != true) {
                            var cost = detail.cantidad * detail.prec;
                            var percentage = detail.descuento / 100;
                            var discount = cost * percentage;

                            detail.costo = cost - (discount);
                            $scope.subTotal = $scope.subTotal + detail.costo;
                            $scope.CantidadProductos = $scope.CantidadProductos + parseInt(detail.cantidad);
                        } else {
                            $scope.totalCreditNotes = $scope.totalCreditNotes + (detail.cantidad * detail.prec);
                        }
                    });
            });

        if ($scope.checkedDiscount == true) {

            $scope.IVA = ((($scope.subTotal + $scope.totalCreditNotes) - (($scope.subTotal) * ($scope.discount / 100)))) * ($scope.IVATasa / 100);

            if ($scope.IVA < 0) {

                $scope.IVA = 0;
            }

            var percentage = $scope.discount / 100;

            angular.forEach($scope.quotations,
                function (quotation, quotationKey) {
                    angular.forEach(quotation.oDetail,
                        function (detail, detailKey) {
                            if (detail.descuento > 0 || (detail.servicio == true && detail.desc == 'INSTALACIONES')) {
                                var cost = detail.cantidad * detail.prec;
                                var percentage = detail.descuento / 100;
                                var discount = cost * percentage;
                                detail.costo = cost - (discount);
                            }
                        });
                });

            var discount = 0;

            discount = ($scope.subTotal + $scope.totalCreditNotes) * percentage;

            if ($scope.checkedIVA == true) {

                $scope.total = (($scope.subTotal + $scope.totalCreditNotes) - discount) + $scope.IVA;

            } else {

                $scope.total = ($scope.subTotal + $scope.totalCreditNotes) - discount;
            }

        } else {

            $scope.checkedDiscount = false;
            $scope.discount = 0;

            $scope.IVA = (($scope.subTotal - (($scope.subTotal) * ($scope.discount / 100)) + $scope.totalCreditNotes)) * ($scope.IVATasa / 100);

            if ($scope.IVA < 0) {

                $scope.IVA = 0;
            }

            if ($scope.checkedIVA == true) {

                $scope.total = (($scope.subTotal + $scope.totalCreditNotes) + $scope.IVA);

            } else {

                $scope.total = $scope.subTotal + $scope.totalCreditNotes;

            }

        }

        if ($scope.total < 0) {
            $scope.resCredit = $scope.total * -1;
        }

    };

    //Realizar el cálculo del Total 
    $scope.CalculateTotalForDetail = function (subTotal, discount, totalNotasCredito) {
        subTotal = subTotal - (subTotal * (discount / 100));
        subTotal = parseFloat(subTotal);
        totalNotasCredito = parseFloat(totalNotasCredito);
        subTotal = subTotal + totalNotasCredito;
        $scope.IVADetail = subTotal * ($scope.IVATasa / 100);
        if ($scope.IVADetail < 0) {
            $scope.IVADetail = 0;
        }
    };

    $scope.LoadPhysicalCustomers = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllPhysicalCustomers'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.physicalCustomers = data.oData.Customers;

                    if ($scope.customer.type == "physical") {

                        ($scope.idClienteFisico != undefined || $scope.idClienteFisico != null) ? $scope.setSelecCustomerPhysical($scope.idClienteFisico) : "";

                    }

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

    $scope.LoadMoralCustomers = function () {

        $http({
            method: 'POST',
            url: '../../../Customers/ListAllMoralCustomers'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.moralCustomers = data.oData.Customers;

                    if ($scope.customer.type == "moral") {

                        ($scope.idClienteMoral != undefined || $scope.idClienteMoral != null) ? $scope.setSelecCustomerMoral($scope.idClienteMoral) : "";

                    }

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
    $scope.getStatusBankAccount = function () {
        $http({
            method: 'GET',
            url: '../../../CatalogBankAccount/Get'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.bankAccountStatus = data.oData.Status;
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
    $scope.getStatusTerminalType = function () {
        $http({
            method: 'GET',
            url: '../../../CatalogTerminalType/Get'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.terminalTypeStatus = data.oData.Status;
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

    $scope.LoadOffices = function () {

        $http({
            method: 'POST',
            url: '../../../Offices/ListAllOffices'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.offices = data.oData.Offices;

                    if ($scope.customer.type == "office") {

                        ($scope.idDespacho != undefined || $scope.idDespacho != null) ? $scope.setSelecCustomerOffice($scope.idDespacho) : "";

                    }

                    ($scope.idDespachoReferencia != undefined || $scope.idDespachoReferencia != null) ? $scope.setSelecOffices($scope.idDespachoReferencia) : "";

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

    $scope.LoadUsers = function () {

        $http({
            method: 'POST',
            url: '../../../Users/ListAllUsers'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.usersOne = data.oData.Users;

                    if ($scope.idUsuario1 != undefined) {

                        $scope.setSelecUsersOne($scope.idUsuario1);

                        $scope.LoadUsersButNot();
                    }

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

    $scope.LoadUsersButNot = function () {

        $http({
            method: 'GET',
            url: '../../../Users/ListAllUsersButNot',
            params: {
                idUser: $scope.sellerOne.idUsuario
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.usersTwo = data.oData.Users;

                    if ($scope.idUsuario2 != undefined) {

                        $scope.setSelecUsersTwo($scope.idUsuario2);

                    }

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

    $scope.GetProductForBranch = function (idBranch, idQuotation, product) {

        $http({
            method: 'POST',
            url: '../../../Products/GetForEraserProduct',
            params: {
                idView: product.idVista,
                idProduct: product.idProducto,
            }
        }).
        success(function (data, status, headers, config) {

            if (data.success == 1) {

                var result = _.result(_.find($scope.quotations, function (quotation) {
                    _.some(quotation.oDetail, function (qt) {
                        return qt.idProducto == product.idProducto &&
                            qt.idSucursal == idBranch &&
                            qt.idCotizacion == idQuotation;
                    });
                }));

                if (result == undefined) {

                    angular.forEach($scope.quotations,
                        function (quotation, quotationKey) {
                            if (quotation.idCotizacion == idQuotation) {
                                quotation.oDetail.push({
                                    idProducto: data.oData.Product.idProducto,
                                    idProveedor: data.oData.Product.idProveedor,
                                    imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                    codigo: data.oData.Product.Codigo,
                                    desc: data.oData.Product.Descripcion,
                                    prec: data.oData.Product.PrecioVenta,
                                    descuento: product.Descuento,
                                    existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                        return chr.idSucursal == idBranch;

                                    }), 'Existencia'),
                                    stock: data.oData.Product.Stock,
                                    cantidad: product.Cantidad,
                                    costo: 0,
                                    servicio: false,
                                    credito: false,
                                    comentarios: "",
                                    Sucursal: "",
                                    idSucursal: idBranch,
                                    idCotizacion: idQuotation,
                                    idVista: product.idVista
                                });
                            }
                        });

                } else {

                    angular.forEach($scope.quotations,
                        function (quotation, quotationKey) {
                            if (quotation.idCotizacion == idQuotation) {
                                angular.forEach(quotation.oDetail,
                                    function (detail, detailKey) {
                                        if (detail.idProducto == data.oData.Product.idProducto &&
                                            detail.idSucursal == idBranch) {
                                            detail.cantidad++;
                                        }
                                    });
                            }
                        });
                }

                $scope.ValidateStock(product.idProducto, idBranch, idQuotation);

            } else if (data.failure == 1) {

                notify(data.oData.Error, $rootScope.error);

            } else if (data.noLogin == 1) {

                window.location = "../../../Access/Close";

            }

            $scope.selectedCode = "";
        }).
        error(function (data, status, headers, config) {

            notify("Ocurrío un error.", $rootScope.error);

            $scope.selectedCode = "";

        });

        $("#product_value").val("");

    };

    $scope.GetProductForQuotation = function (idBranch, idQuotation, product) {

        $http({
            method: 'POST',
            url: '../../../Products/GetProductQuotation',
            params: {
                idProduct: product.idProducto
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    var result = _.result(_.find($scope.quotations, function (quotation) {
                        _.some(quotation.oDetail, function (qt) {
                            return qt.idProducto == product.idProducto &&
                                qt.idSucursal == idBranch &&
                                qt.idCotizacion == idQuotation;
                        });
                    }));

                    if (result == undefined) {

                        angular.forEach($scope.quotations,
                            function (quotation, quotationKey) {
                                if (quotation.idCotizacion == idQuotation) {
                                    quotation.oDetail.push({
                                        idProducto: data.oData.Product.idProducto,
                                        idProveedor: data.oData.Product.idProveedor,
                                        imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                        codigo: data.oData.Product.Codigo,
                                        desc: data.oData.Product.Descripcion,
                                        prec: data.oData.Product.PrecioVenta,
                                        descuento: product.Descuento,
                                        existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                            return chr.idSucursal == idBranch;

                                        }), 'Existencia'),
                                        stock: data.oData.Product.Stock,
                                        cantidad: product.Cantidad,
                                        costo: 0,
                                        servicio: false,
                                        credito: false,
                                        comentarios: "",
                                        Sucursal: "",
                                        idSucursal: idBranch,
                                        idCotizacion: idQuotation,
                                        idVista: null
                                    });
                                }
                            });

                    } else {

                        angular.forEach($scope.quotations,
                            function (quotation, quotationKey) {
                                if (quotation.idCotizacion == idQuotation) {
                                    angular.forEach(quotation.oDetail,
                                        function (detail, detailKey) {
                                            if (detail.idProducto == data.oData.Product.idProducto &&
                                                detail.idSucursal == idBranch) {
                                                detail.cantidad = detail.cantidad + product.Cantidad;
                                            }
                                        });
                                }
                            });
                    }

                    $scope.ValidateStock(product.idProducto, idBranch, idQuotation);

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

                $scope.selectedCode = "";
            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $scope.selectedCode = "";

            });

        $("#product_value").val("");

    };

    $scope.GetProduct = function () {

        if ($scope.idSucursal >= 2) {

            $http({
                method: 'POST',
                url: '../../../Products/GetProduct',
                params: {
                    idProduct: ($scope.selectedCode == "" || $scope.selectedCode == null)
                        ? ""
                        : $scope.selectedCode.originalObject.idProducto
                }
            }).success(function (data, status, headers, config) {

                if (data.success == 1) {

                    var result = _.result(_.find($scope.quotations,
                        function (quotation) {
                            _.some(quotation.oDetail,
                                function (qt) {
                                    return qt.idProducto == data.oData.Product.idProducto &&
                                        qt.idSucursal == $scope.idSucursal &&
                                        qt.idCotizacion == 0;
                                });
                        }));

                    if (result == undefined) {

                        angular.forEach($scope.quotations,
                            function (quotation, quotationKey) {
                                if (quotation.idCotizacion == 0) {
                                    quotation.oDetail.push({
                                        idProducto: data.oData.Product.idProducto,
                                        idProveedor: data.oData.Product.idProveedor,
                                        imagen: (data.oData.Product.TipoImagen == 1)
                                            ? '/Content/Products/' +
                                            data.oData.Product.NombreImagen +
                                            data.oData.Product.Extension
                                            : data.oData.Product.urlImagen,
                                        codigo: data.oData.Product.Codigo,
                                        desc: data.oData.Product.Descripcion,
                                        prec: data.oData.Product.PrecioVenta,
                                        descuento: 0,
                                        existencia: _.result(_.find(data.oData.Product._Existencias,
                                                function (chr) {

                                                    return chr.idSucursal == $scope.idSucursal;

                                                }),
                                            'Existencia'),
                                        stock: data.oData.Product.Stock,
                                        cantidad: 1,
                                        costo: 0,
                                        servicio: false,
                                        credito: false,
                                        comentarios: "",
                                        Sucursal: "",
                                        idSucursal: $scope.idSucursal,
                                        idCotizacion: 0
                                    });
                                }
                            });

                    } else {

                        angular.forEach($scope.quotations,
                            function (quotation, quotationKey) {
                                if (quotation.idCotizacion == 0) {
                                    angular.forEach(quotation.oDetail,
                                        function (detail, detailKey) {
                                            if (detail.idProducto == data.oData.Product.idProducto &&
                                                detail.idSucursal == $scope.idSucursal) {
                                                detail.cantidad++;
                                            }
                                        });
                                }
                            });
                    }

                    $scope.ValidateStock(data.oData.Product.idProducto, $scope.idSucursal, 0);

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

                $scope.selectedCode = "";

            }).error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $scope.selectedCode = "";

            });

            $("#product_value").val("");

        } else {
            notify("Seleccione una sucursal.", $rootScope.error);
        }

    };

    $scope.GetStockProduct = function (idProduct) {

        $http({
            method: 'POST',
            url: '../../../Products/GetProduct',
            params: {
                idProduct: idProduct
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify("<p>" + data.oData.Product._Existencias[0].Sucursal + ":" + data.oData.Product._Existencias[0].Existencia
                        + "<br/>" + data.oData.Product._Existencias[1].Sucursal + ":" + data.oData.Product._Existencias[1].Existencia
                        + "<br/>" + data.oData.Product._Existencias[2].Sucursal + ":" + data.oData.Product._Existencias[2].Existencia
                        + "<br/> EN VISTA:" + data.oData.Product.Vista + "</p>", $rootScope.error);

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

                $scope.selectedCode = "";

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $scope.selectedCode = "";

            });

        $("#product_value").val("");

    };

    $scope.GetProductForIdAndBranch = function (idBranch, idProduct) {

        return $.ajax({
            method: 'POST',
            url: '../../../Products/GetProductForIdAndBranch',
            async: false,
            data: {
                idBranch: idBranch,
                idProduct: idProduct
            }
        });
    };

    $scope.ValidateStock = function (ID, branchID, idQuotation) {

        var producto = null;

        angular.forEach($scope.quotations,
        function (quotation, quotationKey) {
            angular.forEach(quotation.oDetail,
                function (detail, detailKey) {
                    if (detail.idProducto == ID &&
                        detail.idSucursal == branchID &&
                        detail.idCotizacion == idQuotation) {
                        producto = detail;
                    }
                });
            });

        //Se verifica el total del inventario
        var stock = 0;

        angular.forEach($scope.quotations,
            function (quotation, quotationKey) {
                angular.forEach(quotation.oDetail,
                    function (detail, detailKey) {
                        if (detail.idProducto == ID &&
                            detail.idSucursal == branchID) {
                            stock = stock + parseInt(detail.cantidad);
                        }
                    });
            });

        if (producto.servicio == false) {

            producto.existencia = (producto.existencia == undefined) ? 0 : producto.existencia;

            var amount = 0;

            angular.forEach($scope.quotations,
                function (quotation, quotationKey) {
                    angular.forEach(quotation.oDetail,
                        function (detail, detailKey) {
                            if (detail.idProducto == ID &&
                                detail.idSucursal == branchID &&
                                detail.idCotizacion == idQuotation) {
                                amount = amount + detail.cantidad;
                            }
                        });
                });

            if (producto.existencia < amount) {
                angular.forEach($scope.quotations,
                    function (quotation, quotationKey) {
                        if (quotation.idCotizacion == idQuotation) {
                            angular.forEach(quotation.oDetail,
                                function (detail, detailKey) {
                                    if (detail.idProducto == ID &&
                                        detail.idSucursal == branchID) {
                                        detail.cantidad = producto.existencia;
                                    }
                                });
                        }
                    });

                $scope.GetStockProduct(ID);
            }

            if (stock > producto.existencia) {
                angular.forEach($scope.quotations,
                    function (quotation, quotationKey) {
                        angular.forEach(quotation.oDetail,
                            function (detail, detailKey) {
                                if (detail.idProducto == ID &&
                                    detail.idSucursal == branchID &&
                                    detail.idCotizacion == idQuotation) {
                                    detail.cantidad = 0;
                                }
                            });
                    });

                notify("Inventario insuficiente", $rootScope.error);
            }
        }

        $scope.CalculateTotalCost();
    };

    $scope.DeleteProduct = function (ID, branchID, idQuotation) {

        angular.forEach($scope.quotations,
            function (quotation, quotationKey) {
                if (quotation.idCotizacion == idQuotation) {
                    _.remove(quotation.oDetail, function (n) {
                        return n.idProducto == ID && n.idSucursal == branchID && n.idCotizacion == idQuotation;
                    });
                }
            });

        $scope.CalculateTotalCost();
    };

    $scope.ResumeSale = function () {

        var showModal = false;

        $("#txtValidation").empty();

        if ($scope.customer.type == "moral") {

            if (($scope.moralCustomer == undefined) || ($scope.moralCustomer == null)) {
                $("#txtValidation").append("Seleccione un cliente moral  </br>");
                showModal = true;
            }

        }

        if ($scope.customer.type == "physical") {

            if (($scope.physicalCustomer == undefined) || ($scope.physicalCustomer == null)) {
                $("#txtValidation").append("Seleccione un cliente físico  </br>");
                showModal = true;
            }

        }

        if ($scope.checkedOffices == true) {

            if (($scope.office == undefined) || ($scope.office == null)) {
                $("#txtValidation").append("Seleccione un despacho  </br>");
                showModal = true;
            }

        }

        if (($scope.sellerOne == undefined) || ($scope.sellerOne == null)) {
            $("#txtValidation").append("Seleccione un vendedor  </br>");
            showModal = true;
        }

        angular.forEach($scope.quotations,
            function (quotation, quotationKey) {
                angular.forEach(quotation.oDetail,
                    function (detail, detailKey) {
                        if (detail.cantidad <= 0) {
                            $("#txtValidation").append("No se permiten cantidades menores o iguales a 0");
                            showModal = true;
                        }
                    });
            });

        if (showModal == true) {

            $("#modalValidation").modal("show");

        } else {

            $scope.SaveSale();
            this.getStatusBankAccount();
            this.getStatusTerminalType();
        }

    };

    $scope.SaveSale = function () {
        $("#modalPayment").modal("show");
        $scope.AddTypePayment();
    };


    $scope.AddTypePayment = function () {

        var totalForPay = 0;

        if ($scope.typesPaymentItems.length > 0) {

            totalForPay = $scope.total - _.sumBy($scope.typesPaymentItems, function (o) { return o.amount; });

            if (totalForPay > 0) {

                $scope.typesPaymentItems.push({
                    typesPayment: 0,
                    typesCard: 0,
                    bank: "",
                    holder: "",
                    numCheck: "",
                    numIFE: "",
                    amount: totalForPay,
                    amountIVA: 0,
                    dateMaxPayment: "",
                    openedMaxPayment: false,
                    IVA: false,
                    creditnote: null,
                    idCreditNote: null,
                    paymentMonth: null,
                    idCatalogTerminalType: null,
                    idCatalogBankAccount: null
                });

            }

        } else {

            totalForPay = $scope.total;

            $scope.typesPaymentItems.push({
                typesPayment: 0,
                typesCard: 0,
                bank: "",
                holder: "",
                numCheck: "",
                numIFE: "",
                amount: totalForPay,
                amountIVA: 0,
                dateMaxPayment: "",
                openedMaxPayment: false,
                IVA: false,
                creditnote: null,
                idCreditNote: null,
                paymentMonth: null,
                idCatalogTerminalType: null,
                idCatalogBankAccount: null
            });

        }

    };

    $scope.AddModifyTypePayment = function () {

        var totalForPay = 0;

        if ($scope.typesPaymentItems.length > 0) {

            totalForPay = $scope.total - _.sumBy($scope.typesPaymentItems, function (o) { return o.amount; });

            if (totalForPay > 0) {

                $scope.typesPaymentItems.push({
                    typesPayment: 0,
                    typesCard: 0,
                    bank: "",
                    holder: "",
                    numCheck: "",
                    numIFE: "",
                    amount: totalForPay,
                    amountIVA: 0,
                    dateMaxPayment: "",
                    openedMaxPayment: false,
                    IVA: false,
                    creditnote: null,
                    idCreditNote: null,
                    paymentMonth: null
                });

            }

        } else {

            totalForPay = $scope.total;

            $scope.typesPaymentItems.push({
                typesPayment: 0,
                typesCard: 0,
                bank: "",
                holder: "",
                numCheck: "",
                numIFE: "",
                amount: totalForPay,
                amountIVA: 0,
                dateMaxPayment: "",
                openedMaxPayment: false,
                IVA: false,
                creditnote: null,
                idCreditNote: null,
                paymentMonth: null
            });

        }

    };

    $scope.RemoveTypePayment = function () {

        var a = this;

        _.remove($scope.typesPaymentItems, function (n) {
            return n == a.item;
        });

    };

    $scope.GetCreditNote = function (item) {

        $("#getCreditNote").button("loading");

        $http({
            method: 'POST',
            url: '../../../Credits/GetCreditNote',
            data: {
                remision: item.creditnote
            }
        }).
            success(function (data, status, headers, config) {

                $("#getCreditNote").button("reset");

                if (data.success == 1) {

                    angular.forEach($scope.typesPaymentItems, function (value, key) {
                        if (value.creditnote == item.creditnote) {
                            value.idCreditNote = data.oData.CreditNote.idNotaCredito;
                            value.amount = data.oData.CreditNote.Cantidad
                        }
                    });

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

            }).
            error(function (data, status, headers, config) {

                $("#getCreditNote").button("reset");

                notify("Ocurrío un error.", $rootScope.error);

            });
    };

    $scope.CalculateTotalCostTypesPayment = function (payment) {

        var IVATypesPayment = 0;
        var IVA = 0;

        if (payment.IVA) {

            $scope.checkedIVAPayment = true;

            payment.amountIVA = (payment.amount * ($scope.IVATasa / 100));

            payment.amount = payment.amount + payment.amountIVA;

            $scope.total = $scope.total + payment.amountIVA;

        } else {

            $scope.checkedIVAPayment = false;

            $scope.total = $scope.total - payment.amountIVA;

            payment.amount = payment.amount - payment.amountIVA;

            payment.amountIVA = 0;

        }

    }

    $scope.SavePayment = function () {

        $scope.resCredit = $scope.total - _.sumBy($scope.typesPaymentItems, function (o) { return o.amount; });

        var paymentEmpty = _.find($scope.typesPaymentItems, function (o) { return o.typesPayment == 0; });

        var hasPaymentError = false;

        for (var i = 0; i < $scope.typesPaymentItems.length; i++) {
            if (($scope.typesPaymentItems[i].typesPayment == 5 || $scope.typesPaymentItems[i].typesPayment == 6) &&
                $scope.typesPaymentItems[i].idCatalogTerminalType == null) {
                notify("Seleccionar una terminal.", $rootScope.error);
                hasPaymentError = true;
            }
            else if ($scope.typesPaymentItems[i].typesPayment == 7 && $scope.typesPaymentItems[i].idCatalogBankAccount == null) {
                notify("Seleccionar banco destino.", $rootScope.error);
                hasPaymentError = true;
            }
        }

        if (($scope.resCredit > 0) || (paymentEmpty != undefined)) {

            $scope.msgTypesPayment = true;

            $scope.missing = $scope.resCredit;

        } else if (hasPaymentError == false) {

            $scope.buttonDisabled = true;

            $("#modalPayment").modal("hide");

            var products = new Array();

            var amount = 0;

            angular.forEach($scope.quotations, function (quotation, quotationKey) {
                angular.forEach(quotation.oDetail, function (value, key) {

                    amount = amount + parseInt(value.cantidad);

                    if (value.servicio == true) {

                        products.push({
                            idProducto: null,
                            codigo: value.codigo,
                            imagen: value.imagen,
                            desc: value.desc,
                            prec: value.prec,
                            descuento: parseFloat(value.descuento),
                            cantidad: value.cantidad,
                            idServicio: (value.idCotizacion == 0) ? value.idProducto.split("-")[0] : value.idProducto,
                            credito: value.credito,
                            comentarios: value.comentarios,
                            tipo: 2,
                            idSucursal: 0,
                            idCotizacion: value.idCotizacion
                        });

                    } else if (value.credito == true) {

                        products.push({
                            idProducto: value.idProducto,
                            codigo: value.codigo,
                            imagen: value.imagen,
                            desc: value.desc,
                            prec: value.prec,
                            descuento: parseFloat(value.descuento),
                            cantidad: value.cantidad,
                            idServicio: null,
                            credito: value.credito,
                            comentarios: value.comentarios,
                            tipo: 3,
                            idSucursal: 0,
                            idCotizacion: value.idCotizacion
                        });

                    } else {

                        products.push({
                            idProducto: value.idProducto,
                            codigo: value.codigo,
                            imagen: value.imagen,
                            desc: value.desc,
                            prec: value.prec,
                            descuento: parseFloat(value.descuento),
                            cantidad: value.cantidad,
                            idServicio: null,
                            credito: value.credito,
                            comentarios: value.comentarios,
                            tipo: 1,
                            idSucursal: value.idSucursal,
                            idCotizacion: value.idCotizacion,
                            idVista: value.idVista
                        });
                    }
                });
            });

            var typeCustomer = null;

            switch ($scope.customer.type) {
                case "physical":
                    typeCustomer = 0;
                    break;
                case "moral":
                    typeCustomer = 1;
                    break;
                case "office":
                    typeCustomer = 2;
                    break;
            }

            var ivaPayment = 0;
            if ($scope.checkedIVAPayment == true) {
                ivaPayment = 1;
            } else if ($scope.checkedIVA == true) {
                ivaPayment = 1;
            }

            $http({
                method: 'POST',
                url: '../../../UnifyQuotations/SaveSale',
                data: {
                    idUser1: $scope.sellerOne.idUsuario,
                    idUser2: ($scope.sellerTwo == undefined) ? null : $scope.sellerTwo.idUsuario,
                    idCustomerP: ($scope.customer.type == "physical") ? $scope.physicalCustomer.idCliente : null,
                    idCustomerM: ($scope.customer.type == "moral") ? $scope.moralCustomer.idCliente : null,
                    idOffice: ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
                    project: $scope.project,
                    typeCustomer: typeCustomer,
                    idOfficeReference: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null,
                    idBranch: $scope.branchID,
                    dateSale: $scope.dateTime,
                    amountProducts: amount,
                    subtotal: $scope.subTotal,
                    discount: ($scope.checkedDiscount == true) ? parseFloat($scope.discount) : 0,
                    IVA: ivaPayment,
                    IVATasa: $scope.IVATasa,
                    total: $scope.total,
                    lProducts: products,
                    lTypePayment: $scope.typesPaymentItems
                }
            }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.remision = data.oData.sRemision;

                    notify(data.oData.Message, $rootScope.success);

                    $('#modalPrintSale').on('hidden.bs.modal', function (e) {
                        window.location = "../../../Sales/Index";
                    });

                    $("#modalPrintSale").modal("show");

                    if ($scope.resCredit < 0) {
                        var credito = Math.abs($scope.resCredit);

                        $scope.GenerateCredit(credito);
                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

                $scope.buttonDisabled = false;

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

                $scope.buttonDisabled = false;

            });

        }

    };

    $scope.GenerateCredit = function (credito) {

        $http({
            method: 'POST',
            url: '../../../Credits/SaveAddCredit',
            data: {
                idCreditNoteType: 1,
                idSale: null,
                idCustomerP: ($scope.customer.type == "physical") ? $scope.physicalCustomer.idCliente : null,
                idCustomerM: ($scope.customer.type == "moral") ? $scope.moralCustomer.idCliente : null,
                idSeller: $scope.sellerOne.idUsuario,
                amount: credito,
                dtDate: $scope.dateTime,
                comments: $scope.remision,
                lProducts: null
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.resCredit = Math.abs($scope.resCredit);
                    $scope.remisionCredit = data.oData.sRemision;
                    $scope.finalDateCredit = data.oData.finalDate;
                    $scope.idCreditNote = data.oData.idCreditNote;

                    $("#modalCreditNote").modal("show");

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

    $scope.GetNumberRem = function () {

        $http({
            method: 'POST',
            url: '../../../Sales/GeneratePrevNumberRemC'
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1) {

               $scope.remision = data.oData.sRemision;

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

    $scope.Print = function () {

        var URL = '../../../Sales/PrintSale?remision=' + $scope.remision;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.PrintSale = function (remision) {

        var URL = '../../../Sales/PrintSale?remision=' + remision;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.setSelecCustomerPhysical = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.physicalCustomers, parseInt(a), 'idCliente');

        $scope.physicalCustomer = $scope.physicalCustomers[index];

    };

    $scope.setSelecCustomerMoral = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.moralCustomers, parseInt(a), 'idCliente');

        $scope.moralCustomer = $scope.moralCustomers[index];

    };

    $scope.setSelecCustomerOffice = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.offices, parseInt(a), 'idDespacho');

        $scope.officeCustomer = $scope.offices[index];

        $scope.CalculateTotalCost();

    };

    $scope.setSelecOffices = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.offices, parseInt(a), 'idDespacho');

        $scope.office = $scope.offices[index];

    };

    $scope.setSelecUsersOne = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.usersOne, a, 'idUsuario');

        $scope.sellerOne = $scope.usersOne[index];

    };

    $scope.setSelecUsersTwo = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.usersTwo, a, 'idUsuario');

        $scope.sellerTwo = $scope.usersTwo[index];

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {

        if (myArray != undefined) {

            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }

    };

    //Cargar Editar Imagen
    $scope.iniEditProduct = function () {
        inicioEdit();
    }

    function inicioEdit() {
        fileEditMethod();
    };

    function fileEditMethod() {
        document.getElementById('editHolder').addEventListener('dragover', allowEditDrop, false);
        document.getElementById('editHolder').addEventListener('drop', dropEdit, false);
    };

    function dropEdit(ev) {
        ev.preventDefault();
        arch.addEventListener('load', readEdit, false);
        arch.readAsDataURL(ev.dataTransfer.files[0]);
        $scope.myFiles = ev.dataTransfer.files;

        var file = $scope.myFiles.item(0);
        if ('name' in file) {
            $("#edithImgName").val(file.name);
        }
        else {
            $("#edithImgName").val(file.name);
        }

        $scope.rd = 0;
    };

    function allowEditDrop(ev) {
        ev.preventDefault();
    };

    function readEdit(ev) {
        document.getElementById('editHolder').style.backgroundImage = "url('" + ev.target.result + "')";
    };

    $scope.ChangeTextButtons = function () {
        $("#modalDeleteItem .confirmbutton-yes").text("Enviar");
        $("#modalDeleteItem .confirmbutton-no").text("Solo guardar")
    };

    $scope.OpenModalService = function () {

        $("#modalService").modal("show");
        $scope.LoadServices();

        $scope.newService = new models.ServiceSale();
    };

    $scope.LoadServices = function () {

        $http({
            method: 'POST',
            url: '../../../Services/ListAllServices'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.listServices = data.oData.Services;

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

    $scope.AddService = function () {

        $scope.valResult = ServiceSaleValidator.validate($scope.newService);

        if ($scope.newService.$isValid) {

            $scope.SaveService();

        }

    };

    $scope.SaveService = function () {

        angular.forEach($scope.quotations,
            function (quotation, quotationKey) {
                if (quotation.idCotizacion == 0) {
                    quotation.oDetail.push({
                        idProducto: $scope.newService.descService.idServicio + '-' + GUID.New(),
                        idProveedor: null,
                        imagen: $scope.newService.imagen,
                        codigo: "SERVICIO",
                        desc: $scope.newService.descService.Descripcion,
                        prec: $scope.newService.salePriceService,
                        descuento: 0,
                        existencia: $scope.newService.amountService,
                        stock: $scope.newService.amountService,
                        cantidad: $scope.newService.amountService,
                        costo: 0,
                        servicio: true,
                        credito: false,
                        comentarios: $scope.newService.commentsService,
                        Sucursal: "",
                        idSucursal: $scope.idSucursal,
                        idCotizacion: 0
                    });
                }
            });

        $scope.descService = "";
        $scope.newService.imagen = "";
        $scope.newService.salePriceService = 0;
        $scope.newService.amountService = 0;
        $scope.newService.commentsService = "";

        $("#holder").css("background-image", "");

        $scope.CalculateTotalCost();

        $("#modalService").modal("hide");

        $scope.ValidateService($scope.newService.descService.idServicio);

    };

    $scope.ValidateService = function (idService) {

        $http({
            method: 'POST',
            url: '../../../Services/GetInstallationService',
            data: {
                idService: idService
            }
        }).
       success(function (data, status, headers, config) {

           if (data.success == 1 && data.oData != null) {

               notify(data.oData.Error, $rootScope.error);

           } else if (data.noLogin == 1) {

               window.location = "../../../Access/Close";

           }

       }).
       error(function (data, status, headers, config) {

           notify("Ocurrío un error.", $rootScope.error);

       });

    };

    //--------------------------------------------------
    $scope.EditService = function (item) {        

        $scope.newService = new models.ServiceSale();

        $scope.newService.idService = item.idProducto;
        $scope.setSelectService(item.desc);
        $scope.newService.salePriceService = item.prec;
        $scope.newService.amountService = item.cantidad;
        $scope.newService.commentsService = item.comentarios;
        $scope.newService.imagen = item.imagen;

        document.getElementById('editHolder').style.backgroundImage = "url('" + item.imagen + "')";

        $("#modalEditService").modal("show");
    };

    $scope.SaveEditService = function () {

        angular.forEach($scope.quotations,
            function (quotation, quotationKey) {
                angular.forEach(quotation.oDetail,
                    function (detail, detailKey) {
                        if (detail.idProducto == $scope.newService.idService) {
                            detail.imagen = $scope.newService.imagen;
                            detail.codigo = "SERVICIO";
                            detail.desc = $scope.newService.descService.Descripcion;
                            detail.prec = $scope.newService.salePriceService;
                            detail.existencia = $scope.newService.amountService;
                            detail.stock = $scope.newService.amountService;
                            detail.cantidad = $scope.newService.amountService;
                            detail.comentarios = $scope.newService.commentsService;
                        }
                    });
            });

        $("#editHolder").css("background-image", "");

        $scope.CalculateTotalCost();

        $("#modalEditService").modal("hide");

        $scope.ValidateService($scope.newService.descService.idServicio);
    };

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

    $scope.OpenCommentsModal = function (ID, branchID, quotationID, comments) {

        $scope.commentProductItem = ID;
        $scope.commentBranchID = branchID;
        $scope.commentQuotationID = quotationID
        $scope.comments = comments;

        $("#modalComments").modal("show");

    };

    $scope.SaveComments = function () {

        angular.forEach($scope.quotations,
            function (quotation, quotationKey) {
                if (quotation.idCotizacion == $scope.commentQuotationID) {
                    angular.forEach(quotation.oDetail,
                        function (detail, detailKey) {
                            if (detail.idProducto == $scope.commentProductItem &&
                                detail.idSucursal == $scope.commentBranchID) {
                                detail.comentarios = $scope.comments;
                            }
                        });
                }
            });

        $("#modalComments").modal("hide");

    };

    //Cargar Imagen
    $scope.itemsToPrint = new Array();

    var arch = new FileReader();
    $scope.myFiles = [];

    $scope.iniAddProduct = function () {
        inicio();
    }

    function inicio() {
        fileMethod();
    };

    function fileMethod() {
        document.getElementById('holder').addEventListener('dragover', permitirDrop, false);
        document.getElementById('holder').addEventListener('drop', drop, false);
    };

    //Upload on file select or drop
    function drop(ev) {
        ev.preventDefault();
        arch.addEventListener('load', leer, false);
        arch.readAsDataURL(ev.dataTransfer.files[0]);
        $scope.myFiles = ev.dataTransfer.files;

        var file = $scope.myFiles.item(0);
        if ('name' in file) {
            $("#imgName").val(file.name);
        }
        else {
            $("#imgName").val(file.name);
        }

        $scope.rd = 0;
    };

    function permitirDrop(ev) {
        ev.preventDefault();
    };

    function leer(ev) {
        $("#imgProduct").remove();
        document.getElementById('holder').style.backgroundImage = "url('" + ev.target.result + "')";
    };

    $scope.SaveUploadImg = function (option) {

        var formData = new FormData();
        for (var i = 0; i < $scope.myFiles.length; i++) {
            formData.append('file', $scope.myFiles[i]);
        }

        if ($scope.myFiles.length == null || $scope.myFiles.length == 0 || $scope.myFiles.length == undefined) {
            $scope.openModalMsg();
            return;
        }

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '../../Services/UploadFile?qtyRotates=' + $scope.rd, true);
        xhr.onload = function () {
            $(".loadImg").button("reset");
            var response = JSON.parse(xhr.responseText);
            if (xhr.readyState == 4 && xhr.status == 200) {
                $scope.newService.imagen = "../Content/Services/" + response.fileName;
                notify(response.message, $rootScope.success);
            } else {
                notify(response.message, $rootScope.error);
            }
        };

        $(".loadImg").button("loading");

        xhr.send(formData);
    };

    //Cargar Editar Imagen
    $scope.iniEditProduct = function () {
        inicioEdit();
    }

    function inicioEdit() {
        fileEditMethod();
    };

    function fileEditMethod() {
        document.getElementById('editHolder').addEventListener('dragover', allowEditDrop, false);
        document.getElementById('editHolder').addEventListener('drop', dropEdit, false);
    };

    function dropEdit(ev) {
        ev.preventDefault();
        arch.addEventListener('load', readEdit, false);
        arch.readAsDataURL(ev.dataTransfer.files[0]);
        $scope.myFiles = ev.dataTransfer.files;

        var file = $scope.myFiles.item(0);
        if ('name' in file) {
            $("#edithImgName").val(file.name);
        }
        else {
            $("#edithImgName").val(file.name);
        }

        $scope.rd = 0;
    };

    function allowEditDrop(ev) {
        ev.preventDefault();
    };

    function readEdit(ev) {
        document.getElementById('editHolder').style.backgroundImage = "url('" + ev.target.result + "')";
    };

    $scope.ChangeTextButtons = function () {
        $("#modalDeleteItem .confirmbutton-yes").text("Enviar");
        $("#modalDeleteItem .confirmbutton-no").text("Solo guardar")
    };

    $scope.dateToday = new Date();

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };

    $scope.toggleMin();

    $scope.openPaymentDate = function (a, $event) {
        $event.preventDefault();
        $event.stopPropagation();

        angular.forEach($scope.TypesPayment, function (value, key) {

            if (value.idTypePayment != a.item.idTypePayment) {

                value.bCalendar = false;

            }

            angular.forEach(value.HistoryCredit, function (obj, key) {

                obj.bCalendar = false;

            });


        });

        a.item.bCalendar = true;
    };

    $scope.openPaymentDateHist = function (a, $event) {
        $event.preventDefault();
        $event.stopPropagation();

        angular.forEach($scope.TypesPayment, function (value, key) {

            if (value.idTypePayment != a.item.idTypePayment) {

                value.bCalendar = false;

            }

            angular.forEach(value.HistoryCredit, function (obj, key) {

                obj.bCalendar = false;

            });

        });

        a.hist.bCalendar = true;
    };

    $scope.openedPaymentDateDetail = {
        opened: false
    };

    $scope.open1 = function () {
        $scope.openedPaymentDateDetail.opened = true;
    };

    $scope.openMaxPayment = function (a, $event) {
        $event.preventDefault();
        $event.stopPropagation();

        a.item.openedMaxPayment = true;
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

    $scope.setSelectService = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.listServices, a, 'Descripcion');

        $scope.newService.descService = $scope.listServices[index];
    };

    $scope.GetPaymentMonths = function () {
        $http({
            method: 'GET',
            url: '../../../PaymentMonth/GetAll',
            params: {}
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.paymentMonths = data.oData.PaymentMonths;
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
    $scope.getStatusBankAccount = function () {
        $http({
            method: 'GET',
            url: '../../../CatalogBankAccount/Get'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.bankAccountStatus = data.oData.Status;
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
    $scope.getStatusTerminalType = function () {
        $http({
            method: 'GET',
            url: '../../../CatalogTerminalType/Get'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.terminalTypeStatus = data.oData.Status;
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

angular.module("General").controller('QuotationsController', function (models, ServiceSaleValidator, PaymentValidator, DetailSaleFacturaValidator, GUID, GetDiscount, GetDiscountOffice, GetDiscountSpecial, $scope, $http, $window, notify, $rootScope, FileUploader) {

    $scope.branch = "";
    $scope.branchID = 0;
    $scope.Dolar = 0;
    $scope.customer = "";
    $scope.quotations = new Array();
    $scope.subTotal = 0;
    $scope.checkedDiscount = false;
    $scope.checkedIVA = false;
    $scope.IVATasa = 0;
    $scope.discount = 0;
    $scope.total = 0;
    $scope.includeURLMoral = "";
    $scope.includeURLPhysical = "";
    $scope.Math = window.Math;
    $scope.myFiles = [];

    $scope.typesPaymentItems = new Array();
    $scope.Allpayments = {};

    $scope.itemsPerPage = 20;
    $scope.total = 0;

    $scope.customer = {
        type: "moral"
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
        $scope.checkedOffices = (quotation.idDespachoReferencia != null && quotation.idDespachoReferencia != undefined) ? true : false;
        $scope.idDespachoReferencia = quotation.idDespachoReferencia;
        $scope.branch = "TODAS";
        $scope.idSucursal = 1;
        $scope.TipoCliente = quotation.TipoCliente;
        $scope.Direccion = quotation.oAddress;
        $scope.Dolar = quotation.Dolar;

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
            
            if (quotation.IVA == 1) {                
                $scope.GetBranchInfo(quotation.idSucursal);                
            }
           
            angular.forEach(quotation.oDetail, function (value, key) {
                if (value.idProducto != null && value.idProducto > 0) {
                    $scope.GetProductForQuotation(value.idSucursal, quotation.idCotizacion, value);
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

            $scope.IVA = 0;
        }

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

    $scope.GetProductForQuotation = function (idBranch, idQuotation, product) {

        if (product.idVista > 0) {

            $http({
                method: 'GET',
                url: '../../../Products/GetProductQuotationOriginView',
                params: {
                    idProduct: product.idProducto,
                    idView: product.idVista
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
                                            comentarios: product.Comentarios,
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

        } else {

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
                                            comentarios: product.Comentarios,
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
        }
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
        }

        $scope.CalculateTotalCost();
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

    $scope.PrintUnifiedQuotation = function () {

        var products = new Array();

        var amount = 0;

        angular.forEach($scope.quotations, function (quotation, quotationKey) {
            angular.forEach(quotation.oDetail, function (value, key) {

                amount = amount + parseInt(value.cantidad);

                if (value.servicio == true) {
                    products.push({
                        idProducto: null,
                        Codigo: value.codigo,
                        Imagen: value.imagen,
                        Descripcion: value.desc,
                        Precio: value.prec,
                        Descuento: parseFloat(value.descuento),
                        Cantidad: value.cantidad,
                        idServicio: value.idProducto,
                        Credito: value.credito,
                        Comentarios: value.comentarios,
                        Tipo: 2
                    });
                } else {
                    products.push({
                        idProducto: value.idProducto,
                        Codigo: value.codigo,
                        Imagen: value.imagen,
                        Descripcion: value.desc,
                        Precio: value.prec,
                        Descuento: parseFloat(value.descuento),
                        Cantidad: value.cantidad,
                        idServicio: null,
                        Credito: value.credito,
                        Comentarios: value.comentarios,
                        Tipo: 1
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

        var quotation = {
            idUsuario1: $scope.sellerOne.idUsuario,
            Usuario1: $scope.sellerOne.NombreCompleto,
            idUsuario2: ($scope.sellerTwo == undefined) ? null : $scope.sellerTwo.idUsuario,
            Usuario2: ($scope.sellerTwo == undefined) ? "" : $scope.sellerTwo.NombreCompleto,
            idClienteFisico: ($scope.customer.type == "physical") ? $scope.physicalCustomer.idCliente : null,
            idClienteMoral: ($scope.customer.type == "moral") ? $scope.moralCustomer.idCliente : null,
            ClienteFisico: ($scope.customer.type == "physical")
                ? $scope.physicalCustomer.Nombre
                : ($scope.customer.type == "moral")
                ? $scope.moralCustomer.Nombre
                    : $scope.officeCustomer.Nombre,
            oAddress: {
                Correo: $scope.Direccion.Correo,
                TelCasa: $scope.Direccion.TelCasa,
                TelCelular: $scope.Direccion.TelCelular,
                Direccion: $scope.Direccion.Direccion
            },
            idDespacho: ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
            Proyecto: $scope.project,
            idDespachoReferencia: ($scope.checkedOffices == true) ? $scope.office.idDespacho : null,
            Despacho: ($scope.customer.type == "office") ? $scope.officeCustomer.Nombre : null,
            DespachoReferencia: ($scope.checkedOffices == true) ? $scope.office.Nombre : null,
            idSucursal: 1,
            Sucursal: "TODAS",
            Fecha: $scope.dateTime,
            CantidadProductos: amount,
            Subtotal: $scope.subTotal,
            Descuento: ($scope.checkedDiscount == true) ? parseFloat($scope.discount) : 0,
            IVA: ($scope.checkedIVA == true) ? 1 : 0,
            IVATasa: $scope.IVATasa,
            Total: $scope.total,
            TipoCliente: typeCustomer,
            Comentarios: $scope.comments,
            Dolar: $scope.Dolar,
            lCotizaciones: [{
                oDetail: products
            }]
        };

        var $form = $("<form>", {
            action: "../../../UnifyQuotations/PrintUnifiedQuotation",
            method: "post",
            target: "Cotizacion"
        });

        $('<input>').attr({
            type: "hidden",
            name: "quotation",
            value: JSON.stringify(quotation)
        }).appendTo($form);

        window.open('', 'Cotizacion');
        $form.appendTo('body').submit();
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

    //--------------------------------------------------

    $scope.EditService = function (item) {

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

        var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: $scope.newService.idService }));

        $scope.items[index].imagen = ($scope.newService.imagen.length <= 40) ? "../Content/Services/" + $scope.newService.imagen : $scope.newService.imagen;
        $scope.items[index].codigo = "SERVICIO";
        $scope.items[index].desc = $scope.newService.descService.Descripcion;
        $scope.items[index].prec = $scope.newService.salePriceService;
        $scope.items[index].existencia = $scope.newService.amountService;
        $scope.items[index].stock = $scope.newService.amountService;
        $scope.items[index].cantidad = $scope.newService.amountService;
        $scope.items[index].comentarios = $scope.newService.commentsService;

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

});

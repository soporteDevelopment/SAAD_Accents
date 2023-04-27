angular.module("General").controller('SalesController', function (models, ServiceSaleValidator, PaymentValidator, DetailSaleFacturaValidator, GUID, GetDiscount, GetDiscountOffice, GetDiscountSpecial, $scope, $http, $window, notify, $rootScope, FileUploader) {

    $scope.selectedCode = "";
    $scope.branch = "";
    $scope.customer = "";
    $scope.items = new Array();
    $scope.lProductsOut = new Array();
    $scope.subTotal = 0;
    $scope.checkedDiscount = false;
    $scope.checkedIVA = false;
    $scope.checkedIVAPayment = false;
    $scope.IVATasa = 0;
    $scope.discount = 0;
    $scope.total = 0;
    $scope.countService = 0;
    $scope.resCredit = 0;
    $scope.msgTypesPayment = false;
    $scope.includeURLMoral = "";
    $scope.includeURLPhysical = "";
    $scope.barcode = "";
    $scope.typeBill = 0;
    $scope.paymentLeft = 0;
    $scope.DatePaymentResult = "";
    $scope.Math = window.Math;
    $scope.amountDetailBill = 0;
    $scope.hasIVA = 0;
    $scope.Bill = models.Bill();
    $scope.paymentTAX = {};
    $scope.paymentTAXCredit = {};
    $scope.myFiles = [];
    $scope.searchPayment = "false";
    $scope.bankAccountStatus = [];
    $scope.terminalTypeStatus = [];

    $scope.typesPaymentItems = new Array();
    $scope.paymentMonths = new Array();
    $scope.Allpayments = {};

    $scope.itemsPerPage = 20;
    $scope.currentPage = 0;


    $scope.productOne = {};
    $scope.productTwo = {};

    //Código para el paginado
    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
            $scope.listSales();
        }
    };

    $scope.prevPageDisabled = function () {
        return $scope.currentPage === 0 ? "disabled" : "";
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
            $scope.listSales();
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
            $scope.listSales();
        }
    };

    $scope.listSales = function () {

        $("#searchSales").button("loading");

        $http({
            method: 'POST',
            url: '../../../Sales/GetSales',
            params: {
                allTime: ($scope.searchSince == 2) ? true : false,
                searchPayment: $scope.searchPayment,
                dtDateSince: $scope.dateSince,
                dtDateUntil: $scope.dateUntil,
                remision: $scope.sRemision,
                costumer: ($scope.sCustomer == "" || $scope.sCustomer == null) ? "" : $scope.sCustomer,
                iduser: ($scope.sseller == "" || $scope.sseller == null) ? "" : $scope.sseller.idUsuario,
                codigo: ($scope.sCodigo == "" || $scope.sCodigo == null) ? "" : $scope.sCodigo,
                project: ($scope.sProject == "" || $scope.sProject == null) ? "" : $scope.sProject,
                billNumber: ($scope.sBillNumber == "" || $scope.sBillNumber == null) ? "" : $scope.sBillNumber,
                voucher: ($scope.sVoucher == "" || $scope.sVoucher == null) ? "" : $scope.sVoucher,
                status: ($scope.sStatusCredit == undefined) ? 1 : $scope.sStatusCredit,
                statusPayment: $scope.sStatusPayment,
                payment: $scope.sPayment,
                amazonas: $scope.sBranchAma,
                guadalquivir: $scope.sBranchGua,
                textura: $scope.sBranchTex,
                page: $scope.currentPage,
                pageSize: $scope.itemsPerPage
            }
        }).
            success(function (data, status, headers, config) {

                $("#searchSales").button("reset");

                if (data.success == 1) {

                    if (data.oData.Sales.length > 0) {

                        $scope.Sales = data.oData.Sales;
                        $scope.total = data.oData.Count;

                    } else {

                        notify('No se encontraron registros.', $rootScope.error);

                        $scope.Sales = data.oData.Sales;
                        $scope.total = data.oData.Count;

                    }

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

            }).
            error(function (data, status, headers, config) {

                $("#searchSales").button("reset");

                notify("Ocurrío un error.", $rootScope.error);

            });
    };
    //Inicializa la variable CUSTOMER que se utiliza al momento de hacer una venta en el listado de tipo de cliente
    $scope.customer = {
        type: "moral"
    };

    //Indica en que sucursal está logueado el usuario al momento de hacer la venta
    $scope.SetBranch = function (ID) {
        if (ID == 1) {
            setTimeout(function () {
                $("#openModal").modal("show");
            }, 0);
        } else {
            $scope.branchID = ID;
        }
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
                    $scope.branch = data.oData.Branch.Nombre;
                    $scope.IVATasa = data.oData.Branch.IVATasa;
                    $scope.branchID = data.oData.Branch.idSucursal;

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

    $scope.SetBranchName = function (branchName, IDBranch) {
        $scope.branch = branchName;
        $scope.branchID = IDBranch;
        $scope.GetBranchInfo(IDBranch);
        $("#openModal").modal("hide");
        $scope.GetNumberRem();
    };

    $scope.AddMoralCustomer = function () {
        var now = new Date();

        $scope.includeURLMoral = "../../Customers/PartialAddMoralCustomer?update=" + now;

        $("#openModalAddMoralCustomer").modal("show");

        $('#openModalAddMoralCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadCustomers();
        });
    };

    $scope.AddPhysicalCustomer = function () {
        var now = new Date();

        $scope.includeURLPhysical = "../../Customers/PartialAddPhysicalCustomer?update=" + now;

        $("#openModalAddPhysicalCustomer").modal("show");

        $('#openModalAddPhysicalCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadCustomers();
        })

    };

    $scope.AddOffice = function () {
        var now = new Date();

        $scope.includeURLOffice = "../../Offices/PartialAddOffice?update=" + now;

        $("#openModalAddOfficeCustomer").modal("show");

        $('#openModalAddOfficeCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadOffices();
        })

    };

    $scope.GetProduct = function () {

        $http({
            method: 'POST',
            url: '../../../Products/GetProduct',
            params: {
                idProduct: ($scope.selectedCode == "" || $scope.selectedCode == null) ? "" : $scope.selectedCode.originalObject.idProducto
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    var result = _.result(_.find($scope.items, function (chr) {
                        return chr.idProducto == data.oData.Product.idProducto && chr.idPromocion == null
                    }), 'idProducto');

                    if (result == undefined) {

                        $scope.items.push({
                            idProducto: data.oData.Product.idProducto,
                            idProveedor: data.oData.Product.idProveedor,
                            imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                            codigo: data.oData.Product.Codigo,
                            desc: data.oData.Product.Descripcion,
                            prec: data.oData.Product.PrecioVenta,
                            descuento: 0,
                            existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                return chr.idSucursal == $scope.branchID;

                            }), 'Existencia'),
                            stock: data.oData.Product.Stock,
                            cantidad: 1,
                            costo: 0,
                            servicio: false,
                            credito: false,
                            comentarios: "",
                            idPromocion: null,
                            idTipoPromocion: null,
                            idProductoPadre: null
                        });

                    } else {

                        var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));

                        $scope.items[index].cantidad++;

                    }

                    $scope.ValidateStock(data.oData.Product.idProducto);

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

    $scope.GetProductForEraser = function (idVista) {

        $http({
            method: 'POST',
            url: '../../../Products/GetForEraserProduct',
            params: {
                idView: idVista,
                idProduct: ($scope.selectedCode == "" || $scope.selectedCode == null) ? "" : $scope.selectedCode.originalObject.idProducto
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    var result = _.result(_.find($scope.items, function (chr) {
                        return chr.idProducto == data.oData.Product.idProducto && chr.idPromocion == null
                    }), 'idProducto');

                    if (result == undefined) {

                        $scope.items.push({
                            idProducto: data.oData.Product.idProducto,
                            idProveedor: data.oData.Product.idProveedor,
                            imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                            codigo: data.oData.Product.Codigo,
                            desc: data.oData.Product.Descripcion,
                            prec: data.oData.Product.PrecioVenta,
                            descuento: 0,
                            existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                return chr.idSucursal == $scope.branchID;

                            }), 'Existencia'),
                            stock: data.oData.Product.Stock,
                            cantidad: 1,
                            costo: 0,
                            servicio: false,
                            credito: false,
                            comentarios: "",
                            idPromocion: null,
                            idTipoPromocion: null,
                            idProductoPadre: null
                        });

                    } else {
                        var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));
                        $scope.items[index].cantidad++;
                    }

                    $scope.ValidateStock(data.oData.Product.idProducto);

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

    $scope.GetProductQuotation = function () {

        $http({
            method: 'POST',
            url: '../../../Products/GetProductQuotation',
            params: {
                idProduct: ($scope.selectedCode == "" || $scope.selectedCode == null) ? "" : $scope.selectedCode.originalObject.idProducto
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    var result = _.result(_.find($scope.items, function (chr) {
                        return chr.idProducto == data.oData.Product.idProducto && chr.idPromocion == null
                    }), 'idProducto');

                    if (result == undefined) {

                        $scope.items.push({
                            idProducto: data.oData.Product.idProducto,
                            idProveedor: data.oData.Product.idProveedor,
                            imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                            codigo: data.oData.Product.Codigo,
                            desc: data.oData.Product.Descripcion,
                            prec: data.oData.Product.PrecioVenta,
                            descuento: 0,
                            existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                return chr.idSucursal == $scope.branchID;

                            }), 'Existencia'),
                            stock: data.oData.Product.Stock,
                            cantidad: 1,
                            costo: 0,
                            servicio: false,
                            credito: false,
                            comentarios: "",
                            idPromocion: null,
                            idTipoPromocion: null,
                            idProductoPadre: null
                        });

                    } else {
                        var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));
                        $scope.items[index].cantidad++;
                    }

                    $scope.ValidateStockQuotation(data.oData.Product.idProducto);

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

    $scope.GetStockProductForEraserSale = function (idVista, idProduct) {

        $http({
            method: 'POST',
            url: '../../../Products/GetForEraserProduct',
            params: {
                idView: idVista,
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

    $scope.GetStockProductQuotation = function (idProduct) {

        $http({
            method: 'POST',
            url: '../../../Products/GetProductQuotation',
            params: {
                idProduct: idProduct
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify("<p>" + data.oData.Product._Existencias[0].Sucursal + ":" + data.oData.Product._Existencias[0].Existencia
                        + "<br/>" + data.oData.Product._Existencias[1].Sucursal + ":" + data.oData.Product._Existencias[1].Existencia
                        + "<br/>" + data.oData.Product._Existencias[2].Sucursal + ":" + data.oData.Product._Existencias[2].Existencia
                        + "</p>", $rootScope.error);

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

    $scope.GetProductForEraserSales = function (idVista, prod) {

        $http({
            method: 'POST',
            url: '../../../Products/GetForEraserProduct',
            params: {
                idView: idVista,
                idProduct: prod.idProducto
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    var result = _.result(_.find($scope.items, function (chr) {
                        return chr.idProducto == data.oData.Product.idProducto && chr.idPromocion == null
                    }), 'idProducto');

                    if (result == undefined) {

                        if (prod.idTipoPromocion == 3) {
                            $scope.items.push({
                                idProducto: data.oData.Product.idProducto,
                                idProveedor: data.oData.Product.idProveedor,
                                imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                codigo: data.oData.Product.Codigo,
                                desc: data.oData.Product.Descripcion,
                                prec: data.oData.Product.PrecioVenta,
                                descuento: prod.Descuento,
                                existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                    return chr.idSucursal == $scope.branchID;

                                }), 'Existencia'),
                                stock: data.oData.Product.Stock,
                                cantidad: 1,
                                costo: 0,
                                servicio: false,
                                credito: false,
                                comentarios: "",
                                idPromocion: prod.idPromocion,
                                idTipoPromocion: prod.idTipoPromocion,
                                idProductoPadre: prod.idProductoPadre
                            });
                        } else {
                            $scope.items.push({
                                idProducto: data.oData.Product.idProducto,
                                idProveedor: data.oData.Product.idProveedor,
                                imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                codigo: data.oData.Product.Codigo,
                                desc: data.oData.Product.Descripcion,
                                prec: data.oData.Product.PrecioVenta,
                                descuento: prod.Descuento,
                                existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                    return chr.idSucursal == $scope.branchID;

                                }), 'Existencia'),
                                stock: data.oData.Product.Stock,
                                cantidad: prod.Cantidad,
                                costo: 0,
                                servicio: false,
                                credito: false,
                                comentarios: "",
                                idPromocion: null,
                                idTipoPromocion: null,
                                idProductoPadre: null
                            });
                        }

                    } else {
                        var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));
                        $scope.items[index].cantidad++;
                    }

                    $scope.ValidateStockForEraserSale(idVista, data.oData.Product.idProducto);

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

    $scope.GetProductForId = function () {

        var res = $scope.barcode.match(/^\d{12}$/);

        if (($scope.barcode.length == 12) && (res.length > 0))// Enter key hit
        {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductForId',
                params: {
                    idProduct: $scope.barcode
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        var result = _.result(_.find($scope.items, function (chr) {
                            return chr.idProducto == data.oData.Product.idProducto && chr.idPromocion == null
                        }), 'idProducto');

                        if (result == undefined) {

                            $scope.items.push({
                                idProducto: data.oData.Product.idProducto,
                                idProveedor: data.oData.Product.idProveedor,
                                imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                codigo: data.oData.Product.Codigo,
                                desc: data.oData.Product.Descripcion,
                                prec: data.oData.Product.PrecioVenta,
                                descuento: 0,
                                existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                    return chr.idSucursal == $scope.branchID;

                                }), 'Existencia'),
                                stock: data.oData.Product.Stock,
                                cantidad: 1,
                                costo: 0,
                                servicio: false,
                                credito: false,
                                comentarios: "",
                                idPromocion: null,
                                idTipoPromocion: null,
                                idProductoPadre: null
                            });

                        } else {
                            var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));
                            $scope.items[index].cantidad++;
                        }

                        $scope.ValidateStock(data.oData.Product.idProducto);

                    } else if (data.failure == 1) {

                        notify(data.oData.Error, $rootScope.error);

                    } else if (data.noLogin == 1) {

                        window.location = "../../../Access/Close";

                    }

                }).
                error(function (data, status, headers, config) {

                    notify("Ocurrío un error.", $rootScope.error);

                });

            $scope.barcode = "";
        }
    };

    $scope.CalculateTotalCost = function () {

        $scope.subTotal = 0;
        $scope.CantidadProductos = 0;
        $scope.totalCreditNotes = 0;
        $scope.resCredit = 0;
        $scope.discount = ($scope.discount != null) ? $scope.discount : 0;

        angular.forEach($scope.items, function (value, key) {

            var cost = 0;
            var percentage = 0;
            var discount = 0;

            if (value.credito != true) {
                cost = value.cantidad * value.prec;
                percentage = value.descuento / 100;
                discount = cost * percentage;

                value.costo = cost - (discount);
                $scope.subTotal = $scope.subTotal + value.costo;
                $scope.CantidadProductos = $scope.CantidadProductos + parseInt(value.cantidad);
            } else {
                $scope.totalCreditNotes = $scope.totalCreditNotes + (value.cantidad * value.prec);
            }

        });

        $scope.IVA = ((($scope.subTotal + $scope.totalCreditNotes) - (($scope.subTotal) * ($scope.discount / 100)))) * ($scope.IVATasa / 100);

        if ($scope.IVA < 0) {

            $scope.IVA = 0;
        }

        if ($scope.checkedDiscount == true) {

            var percentage = $scope.discount / 100;

            angular.forEach($scope.items, function (value, key) {

                if (value.descuento > 0 || (value.servicio == true && value.desc == 'INSTALACIONES')) {

                    var cost = value.cantidad * value.prec;
                    var percentage = value.descuento / 100;
                    var discount = cost * percentage;
                    value.costo = cost - (discount);

                }

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

        angular.forEach($scope.items, function (value, key) {

            var cost = 0;
            var percentage = 0;
            var discount = 0;

            if (value.credito != true) {
                cost = value.cantidad * value.prec;
                percentage = value.descuento / 100;
                discount = cost * percentage;

                value.costo = cost - (discount);
                $scope.subTotal = $scope.subTotal + value.costo;
                $scope.CantidadProductos = $scope.CantidadProductos + parseInt(value.cantidad);
            } else {
                $scope.totalCreditNotes = $scope.totalCreditNotes + (value.cantidad * value.prec);
            }

        });

        if ($scope.checkedDiscount == true) {

            $scope.IVA = ((($scope.subTotal + $scope.totalCreditNotes) - (($scope.subTotal) * ($scope.discount / 100)))) * ($scope.IVATasa / 100);

            if ($scope.IVA < 0) {

                $scope.IVA = 0;
            }

            var percentage = $scope.discount / 100;

            angular.forEach($scope.items, function (value, key) {

                if (value.descuento > 0 || (value.servicio == true && value.desc == 'INSTALACIONES')) {

                    var cost = value.cantidad * value.prec;
                    var percentage = value.descuento / 100;
                    var discount = cost * percentage;
                    value.costo = cost - (discount);

                }

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

    $scope.ValidateStock = function (ID) {

        var service = _.result(_.find($scope.items, function (chr) {

            return chr.idProducto == ID

        }), 'servicio');

        if (service == false) {

            var exist = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'existencia');

            exist = (exist == undefined) ? 0 : exist;

            var stock = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'stock');

            var amount = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'cantidad');

            if (exist < amount) {

                var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: ID }));

                if (exist == 0 && index > -1) {
                    $scope.items.splice(index, 1);
                } else {
                    $scope.items[index].cantidad = exist;
                }

                if ($scope.idView > 0) {

                    $scope.GetStockProductOutProduct(ID);

                } else {

                    $scope.GetStockProduct(ID);

                }

                if (exist == 0) {
                    $scope.items.splice(index, 1);
                }

            }
        }

        $scope.CalculateTotalCost();
    };

    $scope.ValidateStockForEraserSale = function (idVista, ID) {

        var service = _.result(_.find($scope.items, function (chr) {

            return chr.idProducto == ID

        }), 'servicio');

        if (service == false) {

            var exist = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'existencia');

            exist = (exist == undefined) ? 0 : exist;

            var stock = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'stock');

            var amount = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'cantidad');

            if (exist < amount) {

                var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: ID }));

                if (exist == 0 && index > -1) {
                    $scope.items[index].cantidad = 0;
                } else {
                    $scope.items[index].cantidad = exist;
                }

                $scope.GetStockProductForEraserSale(idVista, ID);

            }
        }

        $scope.CalculateTotalCost();

    };

    $scope.ValidateStockQuotation = function (ID) {

        var service = _.result(_.find($scope.items, function (chr) {

            return chr.idProducto == ID

        }), 'servicio');

        if (service == false) {

            var exist = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'existencia');

            exist = (exist == undefined) ? 0 : exist;

            var stock = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'stock');

            var amount = _.result(_.find($scope.items, function (chr) {

                return chr.idProducto == ID

            }), 'cantidad');

            if (exist < amount) {

                var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: ID }));

                if (exist == 0 && index > -1) {
                    $scope.items.splice(index, 1);
                } else {
                    $scope.items[index].cantidad = exist;
                }

                $scope.GetStockProductQuotation(ID);

            }
        }

        $scope.CalculateTotalCost();

    };

    $scope.DeleteProduct = function (ID) {

        _.remove($scope.items, function (n) {
            return n.idProducto == ID;
        });

        //Se modifica descuento para calculo
        angular.forEach($scope.items, function (value, key) {
            if (value.idProductoPadre == ID) {
                value.descuento = 0;
            }
        });

        $scope.CalculateTotalCost();
    };

    $scope.OpenModalService = function () {

        $("#modalService").modal("show");

        $("#modalService").on("show.bs.modal", function (e) {

            $scope.LoadServices();

            $scope.newService = new models.ServiceSale();
        });

    };

    $scope.loadsellers = function () {

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

    $scope.OpenModalCredit = function () {

        $("#modalCredit").modal("show");

    };

    $scope.valResult = {};

    $scope.newService = new models.ServiceSale();

    $scope.AddService = function () {

        $scope.valResult = ServiceSaleValidator.validate($scope.newService);

        if ($scope.newService.$isValid) {

            $scope.SaveService();

        }

    };

    $scope.SaveService = function () {

        $scope.items.push({
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
            idPromocion: null,
            idTipoPromocion: null,
            idProductoPadre: null
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

    $scope.AddCredit = function () {

        $http({
            method: 'POST',
            url: '../../../Credits/GetCreditNote',
            data: {
                remision: $scope.searchStr
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    if (_.find($scope.items, function (o) { return o.codigo == data.oData.CreditNote.RemisionCredito; }) == undefined) {

                        $scope.items.push({
                            idProducto: data.oData.CreditNote.idNotaCredito,
                            idProveedor: null,
                            imagen: "",
                            codigo: data.oData.CreditNote.RemisionCredito,
                            desc: "Nota de crédito. " + data.oData.CreditNote.RemisionCredito,
                            prec: -data.oData.CreditNote.Cantidad,
                            descuento: 0,
                            existencia: 1,
                            stock: 1,
                            cantidad: 1,
                            costo: 0,
                            servicio: false,
                            credito: true,
                            comentarios: "",
                            idPromocion: null,
                            idTipoPromocion: null,
                            idProductoPadre: null
                        });

                        $scope.CalculateTotalCost();

                    }

                    $scope.selectedRemision = "";

                    $("#modalCredit").modal("hide");

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

    $scope.LoadCustomers = function () {

        $scope.LoadPhysicalCustomers();
        $scope.LoadMoralCustomers();

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

                        ($scope.outCustomer != undefined || $scope.outCustomer != null) ? $scope.setSelecCustomerPhysical($scope.outCustomer) : "";

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

                        ($scope.outCustomer != undefined || $scope.outCustomer != null) ? $scope.setSelecCustomerMoral($scope.outCustomer) : "";

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

                        ($scope.outOffice != undefined || $scope.outOffice != null) ? $scope.setSelecCustomerOffice($scope.outCustomer) : "";

                    }

                    ($scope.outOffice != undefined || $scope.outOffice != null) ? $scope.setSelecOffices($scope.outOffice) : "";

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

                    if ($scope.outSellerOne != undefined) {

                        $scope.setSelecUsersOne($scope.outSellerOne);

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

                    if ($scope.outSellerTwo != undefined) {

                        $scope.setSelecUsersTwo($scope.outSellerTwo);

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

        if ($scope.items.length == 0) {
            $("#txtValidation").append("Ingrese un producto a la venta </br>");
            showModal = true;
        }

        angular.forEach($scope.items, function (value, key) {
            if (value.cantidad <= 0) {
                $("#txtValidation").append("No se permiten cantidades menores o iguales a 0");
                showModal = true;
            }
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

    $scope.GetCreditNoteAddPayment = function () {

        $("#getCreditNote").button("loading");

        $http({
            method: 'POST',
            url: '../../../Credits/GetCreditNote',
            data: {
                remision: $scope.newPayment.creditnote
            }
        }).
            success(function (data, status, headers, config) {

                $("#getCreditNote").button("reset");

                if (data.success == 1) {

                    $scope.newPayment.idCreditNote = data.oData.CreditNote.idNotaCredito;
                    $scope.newPayment.paymentAmount = data.oData.CreditNote.Cantidad;

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

    $scope.RemoveTypePayment = function () {
        var a = this;

        _.remove($scope.typesPaymentItems, function (n) {
            return n == a.item;
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
            else if($scope.typesPaymentItems[i].typesPayment == 7 && $scope.typesPaymentItems[i].idCatalogBankAccount == null){
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

            angular.forEach($scope.items, function (value, key) {

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
                        idServicio: value.idProducto.split("-")[0],
                        credito: value.credito,
                        comentarios: value.comentarios,
                        tipo: 2,
                        idPromocion: null,
                        costoPromocion: null,
                        idTipoPromocion: null,
                        idProductoPadre: null,
                        idSucursal: $scope.branchID
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
                        idPromocion: null,
                        costoPromocion: null,
                        idTipoPromocion: null,
                        idProductoPadre: null,
                        idSucursal: $scope.branchID
                    });

                } else {

                    if (value.idTipoPromocion == 3) {
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
                            idPromocion: value.idPromocion,
                            costoPromocion: value.costo,
                            idTipoPromocion: value.idTipoPromocion,
                            idProductoPadre: value.idProductoPadre,
                            idSucursal: $scope.branchID
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
                            idPromocion: null,
                            costoPromocion: null,
                            idTipoPromocion: null,
                            idProductoPadre: null,
                            idSucursal: $scope.branchID
                        });
                    }
                }
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
                url: '../../../Sales/SaveSale',
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
                    //IVA: ($scope.checkedIVAPayment == true) ? 1 : 0,
                    IVA: ivaPayment,
                    total: $scope.total,
                    lProducts: products,
                    lProductsOut: $scope.lProductsOut,
                    lTypePayment: $scope.typesPaymentItems,
                    idView: ($scope.idView > 0) ? $scope.idView : null,
                    idQuotation: ($scope.idQuotation != undefined) ? $scope.idQuotation : null,
                    idEraserSale: ($scope.idEraserSale != undefined) ? $scope.idEraserSale : null,
                    idUserChecker: ($scope.idUserChecker != undefined) ? $scope.idUserChecker : null
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

    $scope.SaveEraserSale = function () {

        var r = confirm("Está seguro que desea guardar en borrador esta venta?");

        if (r == true) {

            var products = new Array();

            var amount = 0;

            angular.forEach($scope.items, function (value, key) {

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
                        idServicio: value.idProducto.split("-")[0],
                        credito: value.credito,
                        comentarios: value.comentarios,
                        tipo: 2,
                        idPromocion: null,
                        idTipoPromocion: null,
                        idProductoPadre: null
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
                        idPromocion: null,
                        idTipoPromocion: null,
                        idProductoPadre: null
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
                        idPromocion: value.idPromocion,
                        idTipoPromocion: value.idTipoPromocion,
                        idProductoPadre: value.idProductoPadre
                    });

                }

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

            $http({
                method: 'POST',
                url: '../../../Sales/SaveEraserSale',
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
                    IVA: ($scope.checkedIVA == true) ? 1 : 0,
                    IVATasa: $scope.IVATasa,
                    total: $scope.total,
                    lProducts: products,
                    idView: ($scope.idView > 0) ? $scope.idView : null
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        notify(data.oData.Message, $rootScope.success);

                        location.reload();

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

    };

    $scope.SaveUpdateEraserSale = function () {

        var r = confirm("Está seguro que desea guardar en borrador esta venta?");

        if (r == true) {

            var products = new Array();

            var amount = 0;

            angular.forEach($scope.items, function (value, key) {

                amount = amount + parseInt(value.cantidad);

                if (value.servicio == true) {

                    products.push({
                        idProducto: null,
                        codigo: value.codigo,
                        desc: value.desc,
                        prec: value.prec,
                        descuento: parseFloat(value.descuento),
                        cantidad: value.cantidad,
                        idServicio: value.idProducto.split("-")[0],
                        credito: value.credito,
                        comentarios: value.comentarios,
                        tipo: 2,
                        idPromocion: value.idPromocion,
                        idTipoPromocion: null,
                        idProductoPadre: null
                    });

                } else if (value.credito == true) {

                    products.push({

                        idProducto: null,
                        codigo: value.codigo,
                        desc: value.desc,
                        prec: value.prec,
                        descuento: parseFloat(value.descuento),
                        cantidad: value.cantidad,
                        idCredito: value.idProducto,
                        credito: value.credito,
                        comentarios: value.comentarios,
                        tipo: 3,
                        idPromocion: value.idPromocion,
                        idTipoPromocion: null,
                        idProductoPadre: null
                    });

                } else {

                    products.push({
                        idProducto: value.idProducto,
                        codigo: value.codigo,
                        desc: value.desc,
                        prec: value.prec,
                        descuento: parseFloat(value.descuento),
                        cantidad: value.cantidad,
                        idServicio: null,
                        credito: value.credito,
                        comentarios: value.comentarios,
                        tipo: 1,
                        idPromocion: value.idPromocion,
                        idTipoPromocion: value.idTipoPromocion,
                        idProductoPadre: value.idProductoPadre
                    });

                }

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

            $http({
                method: 'POST',
                url: '../../../Sales/SaveUpdateEraserSale',
                data: {
                    idEraserSale: $scope.idEraserSale,
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
                    IVA: ($scope.checkedIVA == true) ? 1 : 0,
                    total: $scope.total,
                    lProducts: products,
                    idView: ($scope.idView > 0) ? $scope.idView : null
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        notify(data.oData.Message, $rootScope.success);

                        window.location = "../../../Sales/Index";

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

    };

    $scope.GetNumberRem = function () {

        $http({
            method: 'POST',
            url: '../../../Sales/GeneratePrevNumberRem',
            params: {
                idBranch: $scope.branchID
            }
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

    $scope.PrintSaleWithBankInformation = function (remision) {

        var URL = '../../../Sales/PrintSaleWithBankInformation?remision=' + remision;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.PrintDeposit = function () {

        var URL = '../../../Sales/PrintDeposit?remision=' + $scope.remision + '&deposit=' + $scope.deposit;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.PrintDepositOnList = function (deposit) {

        var URL = '../../../Sales/PrintDepositOnline?idSale=' + $scope.newPayment.idSalePayment + '&deposit=' + deposit;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.GetNoIFE = function () {

        $scope.numIFE = ($scope.customer.type == "physical") ? $scope.physicalCustomer.NoIFE : null

    };

    $scope.getStates = function () {

        $http({
            method: 'GET',
            url: '../../../Users/GetStates'
        }).
            success(function (data, status, headers, config) {
                $scope.States = data;
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });

    };

    $scope.Bill = function () {

        $("#modalBill").modal("show");

    };

    $scope.BillDeposit = function () {

        $("#modalDeposit").modal("hide")

        $("#modalBill").modal("show");

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {

        if (myArray != undefined) {

            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }

    };

    $scope.SendBill = function () {
        $http({
            method: 'POST',
            url: '../../../Sales/SendBill',
            params: {
                remision: $scope.remision,
                rfc: $scope.rfcBill,
                name: $scope.nameBill,
                phone: $scope.phoneBill,
                mail: $scope.mailBill,
                street: $scope.streetBill,
                outNum: $scope.outNumberBill,
                inNum: $scope.intNumberBill,
                suburb: $scope.suburbBill,
                town: $scope.townBill,
                state: $scope.stateBill,
                cp: $scope.cpBill,
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $http.post('../../../Sales/BillSale', { remision: $scope.remision }).then(function () { });
                    notify(data.oData.Message, $rootScope.success);
                    $("#modalBill").modal("hide");
                    $("#modalPrintSale").modal("show");
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

    $scope.SendBillPayment = function (remision) {

        $http({
            method: 'POST',
            url: '../../../Sales/SendBillPayment',
            params: {
                remision: remision,
                amount: $scope.amountDetailBill,
                idCustomer: $scope.idCustomerBill,
                concept: $scope.Bill.conceptBill,
                name: $scope.Bill.nameBill,
                phone: $scope.Bill.phoneBill,
                mail: $scope.Bill.mailBill,
                rfc: $scope.Bill.rfcBill,
                street: $scope.Bill.streetBill,
                outNum: $scope.Bill.outNumberBill,
                inNum: $scope.Bill.intNumberBill,
                suburb: $scope.Bill.suburbBill,
                town: $scope.Bill.townBill,
                state: $scope.Bill.stateBill,
                cp: $scope.Bill.cpBill
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    notify(data.oData.Message, $rootScope.success);
                    $("#modalBill").modal("hide");
                    $("#modalPrintSale").modal("show");
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

    $scope.DetailSale = function (remision) {
        var now = new Date();
        $scope.includeURL = "DetailSale?remision=" + remision + "&update=" + now;
        $("#modalDetailSaleOnLine").modal("show");
    };

    $scope.DetailSaleBill = function (item) {

        $scope.amountDetailBill = item.amount;

        $scope.paymentTAX = {
            idPayment: item.idTypePayment,
            amount: item.amount,
            amountIVA: item.amount * ($scope.IVATasa / 100),
            IVA: item.IVA
        }

        $("#modalBill").modal("show");

    };

    $scope.DetailSaleBillCredit = function (item) {

        $scope.amountDetailBill = item.amount;

        $scope.paymentTAXCredit = {
            idPayment: item.idHitorialCredito,
            amount: item.Cantidad,
            amountIVA: item.Cantidad * ($scope.IVATasa / 100),
            IVA: item.IVA
        }

        $("#modalBillCredit").modal("show");
    }

    $scope.GetTypesPaymentForSale = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/GetTypesPaymentForSale',
            params: {
                idSale: idSale,
            }
        }).
            then(function (response) {
                if (response.data.success == 1) {
                    var suma = 0;

                    if (response.data.oData.TypesPayment.length > 0) {
                        angular.forEach(response.data.oData.TypesPayment, function (value, key) {

                            if (value.HistoryCredit.length > 0) {
                                for (var i = 0; i <= value.HistoryCredit.length - 1; i++) {
                                    suma = suma - value.HistoryCredit[i].Cantidad;
                                }

                                value.amount = value.amount + suma;

                                if (value.amount < 0) {
                                    value.amount = 0;
                                }
                            }
                        });

                        $scope.TypesPayment = response.data.oData.TypesPayment;
                    } else {
                        notify('No se encontraron registros.', $rootScope.error);
                    }
                } else if (response.data.failure == 1) {
                    notify(response.data.oData.Error, $rootScope.error);
                } else if (response.data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            })
    };

    $scope.greaterThan = function (prop, val) {
        return function (item) {
            return item[prop] > val;
        }
    }

    $scope.GetTypesPaymentForPrint = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/GetTypesPaymentForPrint',
            params: {
                idSale: idSale,
            }
        }).
            then(function (response) {
                if (response.data.success == 1) {

                    var suma = 0;

                    if (response.data.oData.TypesPayment.length > 0) {

                        $scope.TypesPayment = response.data.oData.TypesPayment;

                    } else {

                        notify('No se encontraron registros.', $rootScope.error);

                    }

                } else if (response.data.failure == 1) {

                    notify(response.data.oData.Error, $rootScope.error);

                } else if (response.data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

            })

    };

    $scope.PaymentHistorySale = function (idSale) {
        $scope.updateTime = Date.now();
        this.getStatusBankAccount();
        this.getStatusTerminalType();
        $scope.includeURLPayment = "InsertPayment?idSale=" + idSale + "&updated=" + $scope.updateTime;

        $("#modalPaymentSaleOnLine").modal("show");
    };

    $scope.init = function (detail) {
        $scope.items = detail;
    };

    $scope.initPayment = function (history) {
        this.history = history;
    };

    $scope.ChangeStatus = function (ID, status) {
        $scope.idSaleStatus = ID;
        $scope.statusSale = status;

        $("#openModalStatus").modal("show");
    };

    $scope.SetStatus = function () {

        $http({
            method: 'POST',
            url: '../../../Sales/SetStatus',
            params: {
                idSale: $scope.idSaleStatus,
                status: $scope.statusSale,
                comments: $scope.comments
            }
        }).
            success(function (data, status, headers, config) {
                $("#openModalStatus").modal("hide");

                if (data.success == 1) {
                    $scope.listSales();
                    notify(data.oData.Message, $rootScope.success);
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

    $scope.SetStatusCredit = function (IDSale, ID, dateModify) {

        $http({
            method: 'POST',
            url: '../../../Sales/SetStatusCredit',
            params: {
                idCredit: ID,
                status: this.statusCredit,
                dateModify: dateModify
            }
        }).
            success(function (data, status, headers, config) {

                $("#openModalStatusCredit").modal("hide");

                if (data.success == 1) {

                    $scope.PaymentHistorySale(IDSale);

                    notify(data.oData.Message, $rootScope.success);

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

    $scope.SetStatusCreditDetail = function (IDSale, ID, dateModify, account, voucher) {

        $http({
            method: 'POST',
            url: '../../../Sales/SetStatusCredit',
            params: {
                idCredit: ID,
                status: this.statusCredit,
                dateModify: dateModify,
                idCuenta: account,
                voucher: voucher
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

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

    $scope.valResult = {};

    $scope.newPayment = new models.Payment();

    $scope.SaveInsertPayment = function () {

        $scope.valResult = PaymentValidator.validate($scope.newPayment);

        if ($scope.newPayment.$isValid) {

            $scope.AddInsertPayment();

        }

    };

    $scope.AddInsertPayment = function () {

        var self = this;

        $("#btnAbonar").button("loading");

        var left = 0;
        var payment = 0;

        if (self.newPayment.paymentAmount > self.newPayment.paymentLeft) {
            left = 0;
            payment = self.newPayment.paymentLeft;
        } else {
            left = self.newPayment.paymentLeft - self.newPayment.paymentAmount;
            payment = self.newPayment.paymentAmount;
        }

        $http({
            method: 'POST',
            url: '../../../Sales/SaveInsertPayment',
            params: {
                idSale: self.newPayment.idSalePayment,
                amount: payment,
                left: left,
                comments: self.newPayment.paymentComment,
                dtInsert: self.newPayment.paymentDate,
                typePayment: self.newPayment.typesPayment,
                typeCard: (self.newPayment.typesPayment == 5) ? self.newPayment.typesCard : null,
                bank: (self.newPayment.typesPayment == 1) ? self.newPayment.bank : null,
                holder: (self.newPayment.typesPayment == 1) ? self.newPayment.holder : null,
                check: (self.newPayment.typesPayment == 1) ? self.newPayment.numCheck : null,
                noIFE: (self.numIFE == 1) ? self.newPayment.numIFE : null,
                idCreditNote: (self.newPayment.typesPayment == 8) ? self.newPayment.idCreditNote : null
            }
        }).
            success(function (data, status, headers, config) {

                $("#btnAbonar").button("reset");

                if (data.success == 1) {

                    if (self.newPayment.paymentAmount > self.newPayment.paymentLeft) {
                        var credit = self.newPayment.paymentAmount - self.newPayment.paymentLeft;
                        $scope.GenerateCreditFromPaymentCredit(self.newPayment.idSalePayment, Math.abs(credit));
                    }

                    $scope.newPayment.paymentAmount = "";
                    $scope.newPayment.paymentComment = "";
                    $scope.newPayment.bank = "";
                    $scope.newPayment.holder = "";
                    $scope.newPayment.numCheck = "";
                    $scope.newPayment.numIFE = "";
                    $scope.newPayment.creditnote = "";
                    $scope.newPayment.idCreditNote = null;

                    notify(data.oData.Message, $rootScope.success);

                    $("#modalPaymentSaleOnLine").modal("hide");

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

            }).
            error(function (data, status, headers, config) {

                $("#btnAbonar").button("reset");

                notify("Ocurrío un error.", $rootScope.error);

            });

    };

    $scope.loadSalesFromEraserSale = function (outProduct) {

        $scope.discount = outProduct.Descuento;

        angular.forEach(outProduct.oDetail, function (value, key) {

            if (value.idProducto == null && value.idServicio > 0) {

                $scope.items.push({
                    idProducto: value.idServicio + "-" + GUID.New(),
                    idProveedor: null,
                    imagen: "../Content/Services/" + value.Imagen,
                    codigo: value.Descripcion,
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
                    idPromocion: null,
                    idTipoPromocion: null,
                    idProductoPadre: null
                });

            } else if (value.idProducto == null && value.idCredito > 0) {

                $scope.items.push({
                    idProducto: value.idCredito,
                    idProveedor: null,
                    imagen: "",
                    codigo: value.Credito,
                    desc: "Nota de crédito",
                    prec: value.Precio,
                    descuento: value.Descuento,
                    existencia: 1,
                    stock: 1,
                    cantidad: 1,
                    costo: 0,
                    servicio: false,
                    credito: true,
                    comentarios: value.Comentarios,
                    idPromocion: null,
                    idTipoPromocion: null,
                    idProductoPadre: null
                });

            } else {

                $scope.GetProductForEraserSales(outProduct.idVista, value);

            }

        });

        $scope.CalculateTotalCost();

    };

    $scope.loadSalesFromQuotation = function (outProduct) {

        angular.forEach(outProduct, function (value, key) {

            if (value.idProducto == null && value.idServicio > 0) {

                $scope.items.push({
                    idProducto: value.idServicio + "-" + GUID.New(),
                    idProveedor: null,
                    imagen: "../Content/Services/" + value.Imagen,
                    codigo: value.Descripcion,
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
                    idPromocion: null,
                    idTipoPromocion: null,
                    idProductoPadre: null
                });

            } else if (value.idProducto == null && value.idCredito > 0) {

                $scope.items.push({
                    idProducto: value.idNotaCredito,
                    idProveedor: null,
                    imagen: "",
                    codigo: value.Credito,
                    desc: "Nota de crédito",
                    prec: value.Precio,
                    descuento: value.Descuento,
                    existencia: 1,
                    stock: 1,
                    cantidad: 1,
                    costo: 0,
                    servicio: false,
                    credito: true,
                    comentarios: value.Comentarios,
                    idPromocion: null,
                    idTipoPromocion: null,
                    idProductoPadre: null
                });

            } else {

                $scope.GetProductForQuotation(value);

            }

        });

        $scope.CalculateTotalCost();

    };

    $scope.loadSalesFromQuotationForSale = function (outProduct) {

        angular.forEach(outProduct, function (value, key) {

            if (value.idProducto == null && value.idServicio > 0) {

                $scope.items.push({
                    idProducto: value.idServicio + "-" + GUID.New(),
                    idProveedor: null,
                    imagen: "../Content/Services/" + value.Imagen,
                    codigo: value.Descripcion,
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
                    idPromocion: null,
                    idTipoPromocion: null,
                    idProductoPadre: null,
                    idVista: value.idVista
                });

            } else if (value.idProducto == null && value.idCredito > 0) {

                $scope.items.push({
                    idProducto: value.idNotaCredito,
                    idProveedor: null,
                    imagen: "",
                    codigo: value.Credito,
                    desc: "Nota de crédito",
                    prec: value.Precio,
                    descuento: value.Descuento,
                    existencia: 1,
                    stock: 1,
                    cantidad: 1,
                    costo: 0,
                    servicio: false,
                    credito: true,
                    comentarios: value.Comentarios,
                    idPromocion: null,
                    idTipoPromocion: null,
                    idProductoPadre: null,
                    idVista: value.idVista
                });

            } else {

                if ($scope.idView > 0) {
                    $scope.GetProductOutProduct(value);
                } else {
                    $scope.GetProductForQuotationForSale(value);
                }

            }

        });

        setTimeout(function () { $scope.VerifySpecialCombo(); }, 3000);

        $scope.CalculateTotalCost();

    };

    $scope.loadSalesFromOutProduct = function (outProduct) {

        angular.forEach(outProduct.oDetail, function (value, key) {
            $scope.GetProductOutProduct(value);
        });

        angular.forEach(outProduct.oServicios, function (value, key) {
            $scope.items.push({
                idProducto: value.idServicio,
                idProveedor: null,
                imagen: "../Content/Services/" + value.Imagen,
                codigo: value.Descripcion,
                desc: value.Descripcion,
                prec: value.Precio,
                descuento: 0,
                existencia: value.Cantidad,
                stock: value.Cantidad,
                cantidad: value.Cantidad,
                costo: 0,
                servicio: true,
                credito: false,
                comentarios: value.Comentarios,
                idPromocion: null,
                idTipoPromocion: null,
                idProductoPadre: null
            });
        });

    };

    $scope.GetProductOutProduct = function (prod) {

        if (prod.idProducto != null) {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductForOutProduct',
                params: {
                    idView: $scope.idView,
                    idProduct: prod.idProducto
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        var result = _.result(_.find($scope.items, function (chr) {

                            return chr.idProducto == data.oData.Product.idProducto

                        }), 'idProducto');

                        if (result == undefined) {

                            var discount = 0;

                            $scope.items.push({
                                idProducto: data.oData.Product.idProducto,
                                idProveedor: data.oData.Product.idProveedor,
                                imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                codigo: data.oData.Product.Codigo,
                                desc: data.oData.Product.Descripcion,
                                prec: data.oData.Product.PrecioVenta,
                                descuento: (prod.Descuento == undefined) ? 0 : prod.Descuento,
                                existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {
                                    return chr.idSucursal == $scope.branchID;
                                }), 'Existencia'),
                                stock: data.oData.Product.Stock,
                                cantidad: prod.Cantidad,
                                costo: 0,
                                servicio: false,
                                credito: false,
                                comentarios: "",
                                idPromocion: prod.idPromocion,
                                idTipoPromocion: prod.idTipoPromocion,
                                idProductoPadre: prod.idProductoPadre,
                                idVista: prod.idVista
                            });

                            $scope.lProductsOut.push({
                                idProducto: data.oData.Product.idProducto,
                                codigo: data.oData.Product.Codigo,
                                desc: data.oData.Product.Descripcion,
                                prec: prod.Precio,
                                descuento: 0,
                                cantidad: prod.Cantidad,
                                costo: 0,
                                idServicio: null,
                                credito: false,
                                comentarios: prod.Comentarios
                            });

                        } else {
                            var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));
                            $scope.items[index].cantidad++;
                        }

                        if (prod.idVista > 0) {
                            $scope.ValidateStockForEraserSale(prod.idVista, data.oData.Product.idProducto);
                        } else {
                            $scope.ValidateStock(data.oData.Product.idProducto);
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
        }
    };

    $scope.GetProductForQuotation = function (prod) {

        if (prod.idProducto != null) {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductQuotation',
                params: {
                    idProduct: prod.idProducto
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        var result = _.result(_.find($scope.items, function (chr) {

                            return chr.idProducto == data.oData.Product.idProducto && chr.idPromocion == null

                        }), 'idProducto');

                        if (result == undefined) {

                            var discount = 0;

                            $scope.items.push({
                                idProducto: data.oData.Product.idProducto,
                                idProveedor: data.oData.Product.idProveedor,
                                imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                codigo: data.oData.Product.Codigo,
                                desc: data.oData.Product.Descripcion,
                                prec: data.oData.Product.PrecioVenta,
                                descuento: (prod.Descuento == undefined) ? 0 : prod.Descuento,
                                existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                    return chr.idSucursal == $scope.branchID;

                                }), 'Existencia'),
                                stock: data.oData.Product.Stock,
                                cantidad: (prod.Cantidad == undefined) ? 0 : prod.Cantidad,
                                costo: 0,
                                servicio: false,
                                credito: false,
                                comentarios: prod.Comentarios,
                                idPromocion: prod.idPromocion,
                                idTipoPromocion: prod.idTipoPromocion,
                                idProductoPadre: prod.idProductoPadre
                            });

                        } else {

                            var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));

                            $scope.items[index].cantidad++;

                        }

                        $scope.ValidateStock(data.oData.Product.idProducto);

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
    };

    $scope.GetProductForQuotationForSale = function (prod) {

        if (prod.idProducto != null) {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductOutProduct',
                params: {
                    idBranch: $scope.branchID,
                    idProduct: prod.idProducto
                }
            }).
                success(function (data, status, headers, config) {

                    if (data.success == 1) {

                        var result = _.result(_.find($scope.items, function (chr) {

                            return chr.idProducto == data.oData.Product.idProducto && chr.idPromocion == null

                        }), 'idProducto');

                        if (result == undefined) {

                            if (prod.idTipoPromocion == 3) {
                                $scope.items.push({
                                    idProducto: data.oData.Product.idProducto,
                                    idProveedor: data.oData.Product.idProveedor,
                                    imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                    codigo: data.oData.Product.Codigo,
                                    desc: data.oData.Product.Descripcion,
                                    prec: data.oData.Product.PrecioVenta,
                                    descuento: (prod.Descuento == undefined) ? 0 : prod.Descuento,
                                    existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                        return chr.idSucursal == $scope.branchID;

                                    }), 'Existencia'),
                                    stock: data.oData.Product.Stock,
                                    cantidad: 1,
                                    costo: 0,
                                    servicio: false,
                                    credito: false,
                                    comentarios: "",
                                    idPromocion: prod.idPromocion,
                                    idTipoPromocion: prod.idTipoPromocion,
                                    idProductoPadre: prod.idProductoPadre
                                });
                            } else {
                                $scope.items.push({
                                    idProducto: data.oData.Product.idProducto,
                                    idProveedor: data.oData.Product.idProveedor,
                                    imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                                    codigo: data.oData.Product.Codigo,
                                    desc: data.oData.Product.Descripcion,
                                    prec: data.oData.Product.PrecioVenta,
                                    descuento: (prod.Descuento == undefined) ? 0 : prod.Descuento,
                                    existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                        return chr.idSucursal == $scope.branchID;

                                    }), 'Existencia'),
                                    stock: data.oData.Product.Stock,
                                    cantidad: prod.Cantidad,
                                    costo: 0,
                                    servicio: false,
                                    credito: false,
                                    comentarios: "",
                                    idPromocion: null,
                                    idTipoPromocion: null,
                                    idProductoPadre: null
                                });
                            }

                        } else {

                            var index = _.indexOf($scope.items, _.find($scope.items, { idProducto: data.oData.Product.idProducto }));

                            $scope.items[index].cantidad++;

                        }

                        $scope.ValidateStock(data.oData.Product.idProducto);

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
    };

    $scope.GetStockProductOutProduct = function (ID) {

        if (ID != undefined) {

            $http({
                method: 'POST',
                url: '../../../Products/GetProductForOutProduct',
                params: {
                    idView: $scope.idView,
                    idProduct: ID
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

    $scope.GenerateCreditFromPayment = function (credito) {

        $http({
            method: 'POST',
            url: '../../../Credits/SaveAddCreditFromPayment',
            data: {
                idSale: $scope.idSaleModifyPayment,
                amount: credito,
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

    $scope.GenerateCreditFromPaymentCredit = function (idSale, credito) {
        $http({
            method: 'POST',
            url: '../../../Credits/SaveAddCreditFromPayment',
            data: {
                idSale: idSale,
                amount: credito,
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.resCredit = Math.abs(credito);
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

    $scope.PrintCredit = function (idCreditNote) {

        var URL = '../../../Credits/PrintCredit?idCreditNote=' + idCreditNote;

        var win = window.open(URL, '_blank');
        win.focus();

    };

    $scope.LoadInformation = function (typeCustomer, customer, idOffice, seller, credit, discount, iva, idBranch, IVATasa) {

        if (idBranch == null) {
            $scope.IVATasa = IVATasa;
        } else {
            $scope.GetBranchInfo(idBranch);
        }

        //Select type Customer
        $scope.customer.type = (typeCustomer == 0) ? "physical" : (typeCustomer == 1) ? "moral" : "office";

        //Select customer
        $scope.outCustomer = customer;

        //Select office
        $scope.checkedOffices = (idOffice != undefined && idOffice.length != 0) ? true : false;

        $scope.outOffice = idOffice;

        //Select seller
        $scope.outSellerOne = seller[0];
        $scope.outSellerTwo = seller[1];

        //Credit
        if (credit != '') {
            $scope.searchStr = credit;
            $scope.AddCredit();
        }

        if (discount > 0) {
            $scope.checkedDiscount = true;
            $scope.discount = parseFloat(discount);
        }

        if (iva == 1) {
            $scope.checkedIVA = true;
        }
    };

    $scope.GetDataBill = function () {

        $http({
            method: 'POST',
            url: '../../../Sales/GetDatasBill',
            data: {
                rfc: $scope.rfcBill
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1 && data.oData.DataBill != null) {
                    $scope.nameBill = data.oData.DataBill.Nombre;
                    $scope.phoneBill = data.oData.DataBill.Telefono;
                    $scope.mailBill = data.oData.DataBill.Correo;
                    $scope.streetBill = data.oData.DataBill.Calle;
                    $scope.outNumberBill = data.oData.DataBill.NumExt;
                    $scope.intNumberBill = data.oData.DataBill.NumInt;
                    $scope.suburbBill = data.oData.DataBill.Colonia;
                    $scope.townBill = data.oData.DataBill.Municipio;
                    $scope.stateBill = data.oData.DataBill.Estado;
                    $scope.cpBill = data.oData.DataBill.CP;
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

    $scope.GetDataBillPayment = function () {
        $http({
            method: 'POST',
            url: '../../../Sales/GetDatasBill',
            data: {
                rfc: $scope.Bill.rfcBill
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1 && data.oData.DataBill != null) {
                    $scope.Bill.conceptBill = "";
                    $scope.Bill.nameBill = data.oData.DataBill.Nombre;
                    $scope.Bill.phoneBill = data.oData.DataBill.Telefono;
                    $scope.Bill.mailBill = data.oData.DataBill.Correo;
                    $scope.Bill.rfcBill = data.oData.DataBill.RFC;
                    $scope.Bill.streetBill = data.oData.DataBill.Calle;
                    $scope.Bill.outNumberBill = data.oData.DataBill.NumExt;
                    $scope.Bill.intNumberBill = data.oData.DataBill.NumInt;
                    $scope.Bill.suburbBill = data.oData.DataBill.Colonia;
                    $scope.Bill.townBill = data.oData.DataBill.Municipio;
                    $scope.Bill.stateBill = data.oData.DataBill.Estado;
                    $scope.Bill.cpBill = data.oData.DataBill.CP;
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



    };

    $scope.setSelecOffices = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.offices, parseInt(a), 'idDespacho');

        $scope.office = $scope.offices[index];

    };

    $scope.setSelecUsersOne = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.usersOne, a, 'NombreCompleto');

        $scope.sellerOne = $scope.usersOne[index];

    };

    $scope.setSelecUsersTwo = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.usersTwo, a, 'NombreCompleto');

        $scope.sellerTwo = $scope.usersTwo[index];

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

    $scope.GetInformationBill = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/GetInformationBill',
            data: {
                idSale: idSale
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1 && data.oData.DataBill != null) {

                    $scope.remisionBill = data.oData.DataBill.Remision;
                    $scope.typeCustomerBill = data.oData.DataBill.TypeCustomer;
                    $scope.idCustomerBill = data.oData.DataBill.idCustomer;
                    $scope.conceptBill = "";
                    $scope.nameBill = data.oData.DataBill.Nombre;
                    $scope.phoneBill = data.oData.DataBill.Telefono;
                    $scope.mailBill = data.oData.DataBill.Correo;
                    $scope.rfcBill = data.oData.DataBill.RFC;
                    $scope.streetBill = data.oData.DataBill.Calle;
                    $scope.outNumberBill = data.oData.DataBill.NumExt;
                    $scope.intNumberBill = data.oData.DataBill.NumInt;
                    $scope.suburbBill = data.oData.DataBill.Colonia;
                    $scope.setSelecState(data.oData.DataBill.idEstado);
                    $scope.setSelecTown(data.oData.DataBill.idMunicipio);
                    $scope.cpBill = data.oData.DataBill.CP;

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

    $scope.GetInformationBillPayment = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/GetInformationBill',
            data: {
                idSale: idSale
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1 && data.oData.DataBill != null) {

                    $scope.Bill.remisionBill = data.oData.DataBill.Remision;
                    $scope.Bill.typeCustomerBill = data.oData.DataBill.TypeCustomer;
                    $scope.Bill.idCustomerBill = data.oData.DataBill.idCustomer;
                    $scope.Bill.conceptBill = "";
                    $scope.Bill.nameBill = data.oData.DataBill.Nombre;
                    $scope.Bill.phoneBill = data.oData.DataBill.Telefono;
                    $scope.Bill.mailBill = data.oData.DataBill.Correo;
                    $scope.Bill.rfcBill = data.oData.DataBill.RFC;
                    $scope.Bill.streetBill = data.oData.DataBill.Calle;
                    $scope.Bill.outNumberBill = data.oData.DataBill.NumExt;
                    $scope.Bill.intNumberBill = data.oData.DataBill.NumInt;
                    $scope.Bill.suburbBill = data.oData.DataBill.Colonia;
                    $scope.setSelecState(data.oData.DataBill.idEstado);
                    $scope.setSelecTown(data.oData.DataBill.idMunicipio);
                    $scope.Bill.cpBill = data.oData.DataBill.CP;

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

    $scope.OpenAddTax = function (idSale) {
        $scope.updateTime = Date.now();
        $scope.includeURLAddTax = "OpenAddTax?idSale=" + idSale + "&updated=" + $scope.updateTime;
        $("#modalAddTax").modal("show");
    };

    $scope.SaveAddTax = function (idSale) {

        $("#addingTax").button("Buscando...");

        $http({
            method: 'POST',
            url: '../../../Sales/AddTax',
            data: {
                idSale: idSale, amount: this.amount, typePayment: this.typesPayment,
                typeCard: this.typesCard, bank: this.bank, holder: this.holder, check: this.numCheck, noIFE: this.numIFE
            }
        }).
            success(function (data, status, headers, config) {
                $("#addingTax").button("reset");
                if (data.success == 1) {
                    $scope.hasIVA = 1;
                    $("#modalBill").modal("show");
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).
            error(function (data, status, headers, config) {
                $("#addingTax").button("reset");
                notify("Ocurrío un error.", $rootScope.error);
            });
    };

    $scope.SaveAddTaxPayment = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/SaveAddTaxPayment',
            data: {
                idSale: idSale,
                idPayment: $scope.paymentTAX.idPayment,
                amount: $scope.paymentTAX.amount + $scope.paymentTAX.amountIVA,
                amountIVA: $scope.paymentTAX.amountIVA
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $("#modalBill").modal("hide");

                    notify(data.oData.Message, $rootScope.success);

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

    $scope.SaveAddTaxPaymentCredit = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/SaveAddTaxPaymentCredit',
            data: {
                idSale: idSale,
                idCreditHistory: $scope.paymentTAXCredit.idPayment,
                amount: $scope.paymentTAXCredit.amount + $scope.paymentTAXCredit.amountIVA,
                amountIVA: $scope.paymentTAXCredit.amountIVA
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $("#modalBillCredit").modal("hide");

                    notify(data.oData.Message, $rootScope.success);

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

    $scope.SendMail = function (idSale) {

        $scope.mailSale = idSale;

        $("#modalSendMail").modal("show");

    };

    $scope.AcceptSendMail = function () {
        if ($scope.txtSendMail != undefined && $scope.txtSendMail.length > 0) {
            $http({
                method: 'POST',
                url: '../../../Sales/SendMailSaleAgain',
                data: {
                    idSale: $scope.mailSale,
                    email: $scope.txtSendMail
                }
            }).success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.txtSendMail = "";
                    notify(data.oData.Message, $rootScope.success);
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
            });
        } else {
            notify("Ingrese una cuenta de correo", $rootScope.error);
        }
    };

    $scope.newDetailFactura = new models.DetailSaleFactura();

    $scope.SaveNumberBill = function (idSale) {

        $http({
            method: 'POST',
            url: '../../../Sales/SetNumberBill',
            data: {
                idSale: idSale,
                numberBill: $scope.newDetailFactura.numberBill
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

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

    $scope.ModalPaymentWay = function (idSale, Total) {
        $scope.idSaleModifyPayment = idSale;
        $scope.total = Total;

        $scope.typesPaymentItems = new Array();

        $scope.AddModifyTypePayment();

        this.getStatusBankAccount();
        this.getStatusTerminalType();

        $("#modalPayment").modal("show");
    };

    $scope.SaveModifyPaymentWay = function () {
        $scope.resCredit = $scope.total - _.sumBy($scope.typesPaymentItems, function (o) { return o.amount; });
        var paymentEmpty = _.find($scope.typesPaymentItems, function (o) { return o.typesPayment == 0; });

        if (($scope.resCredit > 0) || (paymentEmpty != undefined)) {
            $scope.msgTypesPayment = true;
            $scope.missing = $scope.resCredit;
        } else {
            $scope.buttonDisabled = true;

            $("#modalPayment").modal("hide");

            $http({
                method: 'POST',
                url: '../../../Sales/ModifyPaymentWay',
                data: {
                    idSale: $scope.idSaleModifyPayment,
                    lTypePayment: $scope.typesPaymentItems,
                }
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        notify(data.oData.Message, $rootScope.success);

                        if ($scope.resCredit < 0) {
                            var credito = Math.abs($scope.resCredit);
                            $scope.GenerateCreditFromPayment(credito);
                        } else {
                            $scope.listSales();
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
        }
    };

    $scope.SaveDatePaymentDetail = function (a) {

        if (a.item.DatePayment == "" && a.item.Estatus == 1) {
            notify("La fecha es requerida.", $rootScope.error);
        } else {

            $http({
                method: 'POST',
                url: '../../../Sales/SaveDatePaymentDetail',
                data: {
                    idTypePayment: a.item.idTypePayment,
                    status: $("#sStatusSale" + a.item.idTypePayment).val(),
                    date: a.item.DatePayment,
                    idCuenta: a.item.idCuenta,
                    voucher: a.item._voucher
                }
            }).success(function (data, status, headers, config) {

                if (data.success == 1) {

                    notify(data.oData.Message, $rootScope.success);

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                } else if (data.noLogin == 1) {

                    window.location = "../../../Access/Close";

                }

            }).error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });
        }
    };

    $scope.OpenCommentsModal = function (ID, comments) {

        $scope.productItem = ID;
        $scope.commentsItem = comments;

        $("#modalComments").modal("show");

    };

    $scope.SaveComments = function () {

        var index = _.findIndex($scope.items, function (o) { return o.idProducto == $scope.productItem });

        if (index > -1) {

            $scope.items[index].comentarios = $scope.commentsItem;

        }

        $("#modalComments").modal("hide");

    };

    $scope.EditSale = function (idSale) {
        var r = confirm("El inventario se modificará, desea continuar?");

        if (r == true) {
            window.location = "../../../Sales/EditSale?idSale=" + idSale;
        }
    };

    $scope.EditUnifySale = function (idSale) {
        var r = confirm("El inventario se modificará, desea continuar?");

        if (r == true) {
            window.location = "../../../Sales/EditUnifySale?idSale=" + idSale;
        }
    };

    $scope.EditHeaderSale = function (idSale) {
        var now = new Date();
        $scope.includeURLEditSale = "../../Sales/EditHeaderSale?idSale=" + idSale + "&update=" + now;
        $('#modalEditSale').on('hidden.bs.modal', function (e) {
            $scope.listSales();
        });
        $("#modalEditSale").modal("show");
    };

    //Carga de imágenes
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

    // upload on file select or drop
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
            var response = JSON.parse(xhr.responseText);
            if (xhr.readyState == 4 && xhr.status == 200) {
                $scope.newService.imagen = "../Content/Services/" + response.fileName;
                notify(response.message, $rootScope.success);
            } else {
                notify(response.message, $rootScope.error);
            }
        };

        xhr.send(formData);
    };

    $scope.GetAccounts = function () {

        $http({
            method: 'POST',
            url: '../../../Sales/GetAccounts'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.accounts = data.oData.Accounts;
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }
            }).
            error(function (data, status, headers, config) {
                notify("Ocurrío un error.", $rootScope.error);
                $scope.selectedCode = "";
            });
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

        if ($scope.newService.imagen != null) {
            $scope.items[index].imagen = ($scope.newService.imagen.length <= 40) ? "../Content/Services/" + $scope.newService.imagen : $scope.newService.imagen;
        }
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

    $scope.setSelectService = function (a) {
        var index = $scope.arrayObjectIndexOf($scope.listServices, a, 'Descripcion');
        $scope.newService.descService = $scope.listServices[index];
    };

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

    //Mixed Combo Promotion
    $scope.OpenModalMixedComboPromotion = function () {
        $("#ModalMixedComboPromotion").modal("show");
    };

    //Get Product One
    $scope.GetProductOneMixedCombo = function () {

        $http({
            method: 'POST',
            url: '../../../Products/GetProductByBranch',
            params: {
                idBranch: $scope.branchID,
                idProduct: ($scope.codeOne == "" || $scope.codeOne == null) ? "" : $scope.codeOne.originalObject.idProducto
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    if (data.oData.Product != undefined && data.oData.Product.Stock > 0) {

                        $scope.productOne = {
                            idProducto: data.oData.Product.idProducto,
                            idProveedor: data.oData.Product.idProveedor,
                            imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                            codigo: data.oData.Product.Codigo,
                            desc: data.oData.Product.Descripcion,
                            prec: data.oData.Product.PrecioVenta,
                            descuento: 0,
                            existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                return chr.idSucursal == $scope.branchID;

                            }), 'Existencia'),
                            stock: data.oData.Product.Stock,
                            cantidad: 1,
                            costo: 0,
                            servicio: false,
                            credito: false,
                            comentarios: "",
                            idPromocion: data.oData.Product.Promotion.idPromocion,
                            idTipoPromocion: data.oData.Product.Promotion.idTipoPromocion,
                            idProductoPadre: null
                        };
                    } else {
                        notify("Producto sin inventario", $rootScope.error);
                    }

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

    //Get Product Two
    $scope.GetProductTwoMixedCombo = function () {

        $http({
            method: 'POST',
            url: '../../../Products/GetProductByBranch',
            params: {
                idBranch: $scope.branchID,
                idProduct: ($scope.codeTwo == "" || $scope.codeTwo == null) ? "" : $scope.codeTwo.originalObject.idProducto
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    if (data.oData.Product != undefined && data.oData.Product.Stock > 0) {

                        $scope.productTwo = {
                            idProducto: data.oData.Product.idProducto,
                            idProveedor: data.oData.Product.idProveedor,
                            imagen: (data.oData.Product.TipoImagen == 1) ? '/Content/Products/' + data.oData.Product.NombreImagen + data.oData.Product.Extension : data.oData.Product.urlImagen,
                            codigo: data.oData.Product.Codigo,
                            desc: data.oData.Product.Descripcion,
                            prec: data.oData.Product.PrecioVenta,
                            descuento: 0,
                            existencia: _.result(_.find(data.oData.Product._Existencias, function (chr) {

                                return chr.idSucursal == $scope.branchID;

                            }), 'Existencia'),
                            stock: data.oData.Product.Stock,
                            cantidad: 1,
                            costo: 0,
                            servicio: false,
                            credito: false,
                            comentarios: "",
                            idPromocion: data.oData.Product.Promotion.idPromocion,
                            idTipoPromocion: data.oData.Product.Promotion.idTipoPromocion,
                            idProductoPadre: $scope.productOne.idProducto
                        };
                    } else {
                        notify("Producto sin inventario", $rootScope.error);
                    }

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

    $scope.DeleteProductOne = function () {
        $scope.productOne = {};
    };

    $scope.DeleteProductTwo = function () {
        $scope.productTwo = {};
    };

    $scope.SaveMixedComboPromotion = function () {
        $("#ModalMixedComboPromotion").modal("hide");

        //Se aplica descuento y relacionan
        if ($scope.productOne.prec < $scope.productTwo.prec) {
            $scope.productOne.descuento = 50;
            $scope.productOne.idProductoPadre = $scope.productTwo.idProducto;
        } else {
            $scope.productTwo.descuento = 50;
            $scope.productTwo.idProductoPadre = $scope.productOne.idProducto;
        }

        //Se agregan los productos
        $scope.items.push($scope.productOne);
        $scope.items.push($scope.productTwo);

        $scope.CalculateTotalCost();
    };

    //Verificar articulos con promocion de Combo Especial
    $scope.VerifySpecialCombo = function () {
        //Se recorren los elementos para aplicar descuento
        angular.forEach($scope.items, function (productOne, key) {
            if (productOne.idPromocion > 0 && productOne.idTipoPromocion == 3 && productOne.idProductoPadre > 0) {
                //Se recorren los elementos para encontrar el producto relacionado
                angular.forEach($scope.items, function (productTwo, key) {
                    if (productOne.idProducto == productTwo.idProductoPadre) {
                        if (productOne.prec < productTwo.prec) {
                            productOne.descuento = 50;
                        } else {
                            productTwo.descuento = 50;
                        }
                    }
                });
            }
        });

        $scope.CalculateTotalCost();
    }

    $scope.CreditNoteSale = function (remision) {
        var now = new Date();
        $scope.includeCreditNoteSale = "PaymentDetailSale?remision=" + remision + "&update=" + now;
        $scope.paymentCreditNote = {
            Comments: ""
        };
        $("#modalCreditNoteSaleOnLine").modal("show");
    };

    $scope.CalculateTotalCreditNote = function () {
        $scope.totalCreditNote = 0;
        angular.forEach($scope.TypesPayment, function (value, key) {
            if (value.Seleccionado) {
                $scope.totalCreditNote = $scope.totalCreditNote + value.amount;
            }
        });
    };

    $scope.SaveCreditNote = function (idSale, idPhysicalClient, idMoralClient, idOfficeClient) {
        var payments = [];

        angular.forEach($scope.TypesPayment, function (value, key) {
            if (value.Seleccionado) {
                payments.push({
                    "idFormaPago": value.idTypePayment,
                    "History": false
                });
            }

            if (value.HistoryCredit.length > 0) {
                angular.forEach(value.HistoryCredit, function (hvalue, hkey) {
                    if (hvalue.Seleccionado) {
                        payments.push({
                            "idFormaPago": value.idHistorialCredito,
                            "History": true
                        });
                    }
                });
            }
        });

        if (payments.length > 0) {
            $http({
                method: 'POST',
                url: '../../../Credits/AddCreditNote',
                data: {
                    idVenta: idSale,
                    Cantidad: $scope.totalCreditNote,
                    Comentarios: $scope.paymentCreditNote.Comments,
                    idClienteFisico: (idPhysicalClient == "") ? null : idPhysicalClient,
                    idClienteMoral: (idMoralClient == "") ? null : idMoralClient,
                    idDespacho: (idOfficeClient == "") ? null : idOfficeClient,
                    FormaPago: payments
                }
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        notify(data.oData.Message, $rootScope.success);
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

    $scope.ValidateStockForQuotationOutProduct = function (item) {
        if (item.idVista > 0) {
            $scope.ValidateStockForEraserSale(item.idVista, item.idProducto);
        } else {
            $scope.ValidateStock(item.idProducto);
        }
    }

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
});

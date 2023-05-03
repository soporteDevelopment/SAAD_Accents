angular.module("General").controller('PreQuotationsController', function (models, $scope, $http, GUID, notify, $rootScope, $timeout) {

    $scope.branch = "";
    $scope.items = new Array;
    $scope.itemsFabrics = new Array;
    $scope.newService = new models.ServicePreQuotation();
    $scope.fileUrl = ""
    $scope.measures = new Array;
    $scope.measuresStandar = new Array;
    $scope.isEnabled = true;
    $scope.executeFunction = false;


    //Inicializa la variable CUSTOMER que se utiliza al momento de hacer una venta en el listado de tipo de cliente
    $scope.customer = {
        type: "moral"
    };

    //Indica en que sucursal está logueado el usuario al momento de hacer la cotización

    $scope.SetBranch = function (ID) {
        if (ID == 1) {
            setTimeout(function () {
                $("#openModal").modal("show");
            }, 0);
        } else {
            $scope.branchID = ID;
        }
    };

    //Cuando se da click desde el boton de cerrar del modal se genera número precotización y nombre sucursal
    $('#btnCloseModal').on('click', function () {        
        $scope.GetBranchInfo($scope.branchID);
        $scope.GetNumberRem();

    })
    
    //Actualiza la informacion de la sucursal
    $scope.SetBranchName = function (branchName, IDBranch) {
        $scope.GetBranchInfo(IDBranch);
        $("#openModal").modal("hide");
        $scope.GetNumberRem();
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
                    if ($scope.checkedIVA == true) {
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

    //Obtiene el numero de pre cotización
    $scope.GetNumberRem = function () {       
        $http({
            method: 'POST',
            url: '../../../PreQuotations/GeneratePrevNumberRem',
            params: {
                idBranch: $scope.branchID
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.number = data.oData.sNumber;
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

    //Obtiene todos los usuarios
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

    //Obtiene todos los usuarios menos el usuario actual que esta en sesion.
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

    //Obtiene todos los vendedores
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

    //Obtiene todos los clientes
    $scope.LoadCustomers = function () {

        $scope.LoadPhysicalCustomers();
        $scope.LoadMoralCustomers();

    };

    //Obtiene todos los clientes morales
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

    //Obtiene todos los clientes fisicos
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

    //Obtiene todos los despachos
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

    //Selecciona como vendedor al usuario actualmente en sesion. 
    $scope.setSelecUsersOne = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.usersOne, a, 'NombreCompleto');

        $scope.sellerOne = $scope.usersOne[index];

    };

    $scope.arrayObjectIndexOf = function (myArray, searchTerm, property) {

        if (myArray != undefined) {

            for (var i = 0, len = myArray.length; i < len; i++) {
                if (myArray[i][property] === searchTerm) return i;
            }

            return -1;
        }

    };

    //Agrega un nuevo servicio a la lista
    $scope.AddService = () => {

        if ($scope.newService.amountService === null || $scope.newService.amountService <= 0 || $scope.newService.amountService === undefined) {
            notify("La cantidad no puede estar vacia o en 0", $rootScope.error);
            return false;
        }

        if ($scope.newService.descService.MedidasEstandar > 0) {
            if ($scope.listMeasuresStandar == undefined || $scope.listMeasuresStandar == null || $scope.listMeasuresStandar.length == 0) {
                notify("Agregue medidas a este servicio", $rootScope.error);
                return false;
            }
        }

        $scope.items.push({
            idProduct: $scope.newService.descService.idServicio + '-' + GUID.New(),
            idService: $scope.newService.descService.idServicio,
            image: $scope.newService.imagen,
            pdf: $scope.pdfName,
            code: "SERVICIO",
            desc: $scope.newService.descService.Descripcion,
            existence: $scope.newService.amountService,
            stock: $scope.newService.amountService,
            amount: $scope.newService.amountService,
            service: true,
            credit: false,
            comments: $scope.newService.commentsService,
            idBranch: $scope.branchID,
            fabrics: $scope.itemsFabrics,
            measures: $scope.listMeasuresStandar,
            measureStandar: $scope.checkedStandarMeasures,
        });

        console.log("ITEMS", $scope.items)

        $scope.descService = "";
        $scope.newService.imagen = "";
        $scope.newService.salePriceService = 0;
        $scope.newService.amountService = 0;
        $scope.newService.commentsService = "";
        $("#holder").css("background-image", "");
        $("#editHolder").css("background-image", "");
        document.getElementById("fileInput").value = "";


        //Inicializa arrays
        $scope.pdfName = "";
        $scope.listMeasuresStandar = new Array;
        $scope.itemsFabrics = new Array;
        $scope.newService = new models.ServicePreQuotation();

    }

    //Elimina un item de la lista, segun su id
    $scope.DeleteProduct = function (ID) {

        if ($scope.items != null || $scope.items != undefined) {
            if ($scope.items.length == 1) {
                notify("No puede eliminar todos los servicios", $rootScope.error);
                return false;
            }
        }

        _.remove($scope.items, function (n) {

            return n.idProduct == ID;

        });
    };

    //Actualiza el item de la lista de servicios
    $scope.SaveEditService = function () {

        let validateValueEdit;
        $scope.listMeasuresStandar.forEach(function (list) {
            var value = document.getElementById("measureedit" + list.idTipoMedida).value;
            if (value !== "" && !/^[\d]+(\.[\d]+)?$/.test(value)) {
                validateValueEdit = true;
                return false;
            }
        });

        if (validateValueEdit) {
            notify("El valor debe ser un número decimal válido", $rootScope.error);
            return false;
        }

        let updateObject = {
            idProduct: $scope.idCurrentProduct,
            idService: $scope.newService.idService,
            image: $scope.newService.imagen,
            pdf: $scope.pdfName,
            code: "SERVICIO",
            desc: $scope.newService.descService.Descripcion,
            existence: $scope.newService.amountService,
            stock: $scope.newService.amountService,
            amount: $scope.newService.amountService,
            service: true,
            credit: false,
            comments: $scope.newService.commentsService,
            idBranch: $scope.branchID,
            fabrics: $scope.itemsFabrics,
            measures: $scope.listMeasuresStandar,
            measureStandar: $scope.checkedStandarMeasures,
        }
        
        $scope.items.forEach(function (i, index) {
            if (i.idProduct === $scope.idCurrentProduct) {
                $scope.items[index] = updateObject;
            }
        });
      
        $scope.CleanData();

        $("#modalEditService").modal("hide");
    }

    //Inicializa las variables para poder ver la info en el modal.
    $scope.EditService = function (item) {
       
        $scope.errorFile = false;
        $scope.idCurrentProduct = item.idProduct;
        $scope.setSelectService(item.desc);
        $scope.newService.idService = item.idService;
        LoadMeasuresStandar(item.measures, 2);
        LoadFabrics();

        $scope.pdfName = item.pdf;
        $scope.newService.amountService = item.amount;
        $scope.newService.commentsService = item.comments;
        $scope.newService.imagen = item.image;
        $scope.fileUrl = item.pdf;
        $scope.listMeasuresStandar = item.measures;
        $scope.itemsFabrics = item.fabrics;
        $scope.checkedStandarMeasures = item.measureStandar;

        if (item.image != null) {

            document.getElementById('editHolder').style.backgroundImage = "url('" + item.image + "')";
        }

        $('#navEdit a:first').tab('show')

        $("#modalEditService").modal("show");
    };



    $('#modalEditService').on('show.bs.modal', function () {
        $timeout(function () {
            $scope.listMeasuresStandar.forEach(function (item, index) {
                let input = document.getElementById("measureedit" + item.idTipoMedida);
                input.value = item.Valor;

                // Agregar un evento "onkeydown" al campo de entrada para permitir solo números y un punto decimal
                input.onkeydown = function (event) {

                    const codigoTecla = event.keyCode;
                    const esNumero = (codigoTecla >= 48 && codigoTecla <= 57) || (codigoTecla >= 96 && codigoTecla <= 105);
                    const esPuntoDecimal = codigoTecla === 46 || codigoTecla === 110 || codigoTecla === 190;
                    const tienePuntoDecimal = event.target.value.includes('.');

                    if (!esNumero && !esPuntoDecimal && codigoTecla !== 8 && codigoTecla !== 46 && codigoTecla !== 37 && codigoTecla !== 39 && codigoTecla !== 9) {
                        event.preventDefault();
                    } else if (esPuntoDecimal && tienePuntoDecimal) {
                        event.preventDefault();
                    }
                };

                // Agregar un evento "onkeyup" al campo de entrada para establecer la variable en "false" si se elimina el punto decimal
                input.onkeyup = function (event) {

                    var char = event.which || event.keyCode; // Obtener el código de la tecla presionada
                    if (char == 8 || char == 46) {
                        var valor = input.value;
                        if (valor.indexOf(".") == -1) {
                            seIngresoPunto = false; // Establecer la variable en "false" si se elimina el punto decimal
                        }
                    }
                };

                // Agregar un evento "onblur" al campo de entrada para validar el contenido
                input.onblur = function () {

                    var valor = input.value;
                    if (valor !== "" && !/^[\d]+(\.[\d]+)?$/.test(valor)) {
                        /* input.value = ""; // Borrar el contenido si no es un número decimal válido*/
                        //alert("El valor debe ser un número decimal válido"); // Mostrar un mensaje de error
                        return false;
                    }
                };

            });
        }, 800).then(function () {
            // Aquí se ejecuta la función de devolución de llamada después de que se complete el tiempo de espera
            $scope.listMeasuresStandar.forEach(function (item) {
                $("#measureedit" + item.idTipoMedida).trigger("keydown");
                $("#measureedit" + item.idTipoMedida).trigger("blur");
                $("#measureedit" + item.idTipoMedida).trigger('keyup');
            });
        });
    });



    //Selecciona el servicio y su nombre.
    $scope.setSelectService = function (a) {

        var index = $scope.arrayObjectIndexOf($scope.listServices, a, 'Descripcion');

        $scope.newService.descService = $scope.listServices[index];
    };

    //Proceso para generar un nuevo cliente moral
    $scope.AddMoralCustomer = function () {
        var now = new Date();

        $scope.includeURLMoral = "../../Customers/PartialAddMoralCustomer?update=" + now;
        $("#openModalAddMoralCustomer").modal("show");
        $('#openModalAddMoralCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadCustomers();
        });
    };

    //Proceso para generar un nuevo cliente fisico
    $scope.AddPhysicalCustomer = function () {
        var now = new Date();

        $scope.includeURLPhysical = "../../Customers/PartialAddPhysicalCustomer?update=" + now;

        $("#openModalAddPhysicalCustomer").modal("show");

        $('#openModalAddPhysicalCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadCustomers();
        })

    };

    //Proceso para generar un nuevo despacho
    $scope.AddOffice = function () {
        var now = new Date();

        $scope.includeURLOffice = "../../Offices/PartialAddOffice?update=" + now;

        $("#openModalAddOfficeCustomer").modal("show");

        $('#openModalAddOfficeCustomer').on('hidden.bs.modal', function (e) {
            $scope.LoadOffices();
        })

    };

    //Abre el modal para agregar medidas, incializa la informacion de medidas estandar 
    $scope.OpenModalStandarMeasurements = () => {
        LoadMeasuresStandar();
    }

    //Obtiene las medidas estandar en base al id del servicio
    function LoadMeasuresStandar(measures, indicator) {

        $http({
            method: 'GET',
            url: '../../../Services/GetServicesMeasureStandar',
            params: {
                idService: $scope.newService.descService.idServicio
            }
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.listMeasuresStandar = data.oData.services;

                    if ($scope.listMeasuresStandar.length) {

                        $scope.checkedStandarMeasures = true;

                        //Se genera un nuevo array con las medidas estandar. el valor de la medida se incializa en 0 para su modificacion. 
                        $scope.listMeasuresStandar.forEach(function (item, index) {

                            let obj = {
                                idMeasure: item.idTipoMedida,
                                value: item.Valor
                            }

                            $scope.measuresStandar.push(obj);
                            //item.Valor = 0           

                            if (measures != null || measures != undefined) {

                                if (measures.length) {
                                    measures.forEach((ele) => {
                                        if (ele.idTipoMedida === item.idTipoMedida) {
                                            item.Valor = ele.Valor
                                        }

                                    })
                                }
                            }
                        });

                    }

                    if (indicator !== 2) {
                        $("#openModalStandarMeasures").modal("show");
                        //Se inicializa en false el check de medidas estandar
                        $scope.checkedStandarMeasures = true;
                    } else {
                        $scope.checkedStandarMeasures = false;
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

    $('#openModalStandarMeasures').on('show.bs.modal', function () {
        $timeout(function () {
            $scope.listMeasuresStandar.forEach(function (item, index) {
                let input = document.getElementById("measure" + item.idTipoMedida);
                input.value = item.Valor;

                // Agregar un evento "onkeydown" al campo de entrada para permitir solo números y un punto decimal
                input.onkeydown = function (event) {

                    const codigoTecla = event.keyCode;
                    const esNumero = (codigoTecla >= 48 && codigoTecla <= 57) || (codigoTecla >= 96 && codigoTecla <= 105);
                    const esPuntoDecimal = codigoTecla === 46 || codigoTecla === 110 || codigoTecla === 190;
                    const tienePuntoDecimal = event.target.value.includes('.');

                    if (!esNumero && !esPuntoDecimal && codigoTecla !== 8 && codigoTecla !== 46 && codigoTecla !== 37 && codigoTecla !== 39 && codigoTecla !== 9) {
                        event.preventDefault();
                    } else if (esPuntoDecimal && tienePuntoDecimal) {
                        event.preventDefault();
                    }
                };

                // Agregar un evento "onkeyup" al campo de entrada para establecer la variable en "false" si se elimina el punto decimal
                input.onkeyup = function (event) {

                    var char = event.which || event.keyCode; // Obtener el código de la tecla presionada
                    if (char == 8 || char == 46) {
                        var valor = input.value;
                        if (valor.indexOf(".") == -1) {
                            seIngresoPunto = false; // Establecer la variable en "false" si se elimina el punto decimal
                        }
                    }
                };

                // Agregar un evento "onblur" al campo de entrada para validar el contenido
                input.onblur = function () {
                    var valor = input.value;
                    if (valor !== "" && !/^[\d]+(\.[\d]+)?$/.test(valor)) {
                        return false;
                    }
                };

            });
        }, 800).then(function () {
            // Aquí se ejecuta la función de devolución de llamada después de que se complete el tiempo de espera
            $scope.listMeasuresStandar.forEach(function (item) {
                $("#measure" + item.idTipoMedida).trigger("keydown");
                $("#measure" + item.idTipoMedida).trigger("blur");
                $("#measure" + item.idTipoMedida).trigger('keyup');
            });

            $scope.isEnabled = false;
        });
    });



    $('#openModalStandarMeasures').on('hide.bs.modal', function () {
        $scope.isEnabled = true;

    });
    //Agrega los valores nuevos a su medida correspondiente
    $scope.AddMeasure = () => {

        let validateValue;
        $scope.listMeasuresStandar.forEach(function (list, index) {
            var value = document.getElementById("measure" + list.idTipoMedida).value;
            if (value !== "" && !/^[\d]+(\.[\d]+)?$/.test(value)) {
                validateValue = true
                return false;
            } else {
                list.Valor = value * 1;
            }
        });

        if (validateValue) {
            notify("El valor debe ser un número decimal válido", $rootScope.error);
            return false;
        }

        //Valida si el valor de los input viene vacio.
        if ($scope.listMeasuresStandar.some(function (val) {
            return val.Valor * 1 === 0 || val.Valor == "";
        })) {
            notify("Las medidas no pueden estar vacias", $rootScope.error);
            return false;
        }

        notify("Se agregaron medidas", $rootScope.success);

        $("#openModalStandarMeasures").modal("hide");
    }

    //Si el usuario selecciona utilizar las medidas estandar, se agrega el valor de medida estandar a la medida correspondiente
    $scope.setMeasuresStandar = function () {
        if ($scope.checkedStandarMeasures) {
            $scope.listMeasuresStandar.forEach(function (list, index) {
                $scope.measuresStandar.forEach(me => {
                    if (list.idTipoMedida === me.idMeasure) {
                        list.Valor = me.value;
                        //list.Valor = parseInt(me.value);
                        document.getElementById("measure" + list.idTipoMedida).value = list.Valor;
                        document.getElementById("measureedit" + list.idTipoMedida).value = list.Valor;
                    }
                });
            });
        } else {
            $scope.listMeasuresStandar.forEach(m => {
                m.Valor = 0;
                document.getElementById("measure" + m.idTipoMedida).value = m.Valor;
                document.getElementById("measureedit" + m.idTipoMedida).value = m.Valor;
            });
        }
    }

    //Abre el modal de telas, inicializa la info de telas.
    $scope.OpenModalFabricType = () => {
        LoadFabrics();
        $("#openModalFabric").modal("show");
    }

    //Obtiene todas las telas
    function LoadFabrics() {
        $http({
            method: 'GET',
            url: '../../../CatalogTextiles/GetAllTextiles'
        }).
            success(function (data, status, headers, config) {

                if (data.success == 1) {

                    $scope.listFabrics = data.oData.textiles;

                } else if (data.failure == 1) {

                    notify(data.oData.Error, $rootScope.error);

                }

            }).
            error(function (data, status, headers, config) {

                notify("Ocurrío un error.", $rootScope.error);

            });
    }

    //Agrega una nueva tela a la lista
    $scope.AddFabricType = () => {
        if ($scope.fabric != null || $scope.fabric != undefined) {
            $scope.itemsFabrics.push({
                idFabricItem: $scope.fabric.idTextiles + '-' + GUID.New(),
                idFabric: $scope.fabric.idTextiles,
                fabric: $scope.fabric.NombreTela,
                brand: $scope.fabric.Marca,
                price: $scope.fabric.Precio
            });
        }
        
        $scope.fabric = null;
    }

    //Elimina una tela de la lista
    $scope.DeleteFabric = (idFabric) => {
        _.remove($scope.itemsFabrics, function (n) {
            return n.idFabricItem == idFabric;
        });
    }

    //Oculta el modal de telas
    $scope.SaveDataFabricType = () => {
        notify("Se agregaron telas", $rootScope.success);
        $("#openModalFabric").modal("hide");
    }

    //Abre el modal de imagen
    $scope.OpenModalUploadImage = () => {
        $("#openModalUploadImage").modal("show");
    }

    //Proceso imagen nueva
    var arch = new FileReader();
    $scope.myFiles = [];

    $scope.iniAddImage = () => {
        inicio();
    }

    function inicio() {
        fileMethod();
    };

    function fileMethod() {
        document.getElementById('holder').addEventListener('dragover', permitirDrop, false);
        document.getElementById('holder').addEventListener('drop', drop, false);
        document.getElementById('editHolder').addEventListener('dragover', permitirDrop, false);
        document.getElementById('editHolder').addEventListener('drop', drop, false);
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
        document.getElementById('editHolder').style.backgroundImage = "url('" + ev.target.result + "')";
    };

    $scope.RotateImage = () => {

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

            $('#editHolder').css({
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

    //Guarda la nueva imagen
    $scope.SaveUploadImg = function (option) {

        $scope.isEnabledImage = true;

        var formData = new FormData();
        for (var i = 0; i < $scope.myFiles.length; i++) {
            formData.append('file', $scope.myFiles[i]);
        }

        if ($scope.myFiles.length == null || $scope.myFiles.length == 0 || $scope.myFiles.length == undefined) {
            /*$scope.openModalMsg();*/
            $scope.isEnabledImage = false;
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
                $scope.isEnabledImage = false;
            } else {
                $scope.isEnabledImage = false;
                notify(response.message, $rootScope.error);
            }
        };

        $(".loadImg").button("loading");

        xhr.send(formData);

        $("#openModalUploadImage").modal("hide");
    };

    $scope.OpenModalUploadPdf = () => {
        $("#openModalUploadPdf").modal("show");
    }

    $scope.ValidateFile = () => {
        $scope.newService.imagen = "";

        $("#openModalUploadPdf").modal("show");
    }

    //Guarda un nuevo pdf
    $scope.SaveFilePdf = (edit) => {

        try {
            $scope.isEnabledPDF = true;
            $scope.errorFile = false;


            let fileInput;
            if (edit) {
                fileInput = document.getElementById("fileInputEdit");
            } else {
                fileInput = document.getElementById("fileInput");
            }

            if (ValidatePDF(fileInput)) {
                $scope.errorFile = true;
                $scope.isEnabledPDF = false;

                return false;
            }

            let allowedExtensions = /(\.pdf)$/i;

            if (!allowedExtensions.exec(fileInput.value)) {
                fileInput.value = '';

                notify("Solo se permiten archivos PDF.", $rootScope.error);
                $scope.isEnabledPDF = false;
                return false;
            } else {

                let xhr = new XMLHttpRequest();
                let formData = new FormData();
                formData.append('file', fileInput.files[0]);

                xhr.open('POST', '../../Services/UploadPDF');
                xhr.onload = function (response) {
                    var response = JSON.parse(xhr.responseText);
                    if (xhr.readyState == 4 && xhr.status == 200) {
                        notify(response.message, $rootScope.success);
                        $("#openModalUploadPdf").modal("hide");
                        $scope.pdfName = "../Content/PDF/" + response.fileName
                        $scope.fileUrl = "../Content/PDF/" + response.fileName
                        $scope.isEnabledPDF = false;
                    } else {
                        $scope.isEnabledPDF = false;
                        notify(response.message, $rootScope.error);
                    }
                };
                xhr.send(formData);


            }

            $scope.isEnabledPDF = false;
        } catch (ex) {
            $scope.isEnabledPDF = false;
            notify("Ocurrio un error", $rootScope.error);
        }


    }

    //Muestra como vista previa el pdf actual
    $scope.DownloadFilePdf = () => {
        window.open($scope.fileUrl, 'Download');
    }

    //Envia la precotizacion y su detalle al servicio. se valida que un ciente este seleccionado
    $scope.ResumePreQuotation = (confirm) => {
        if (confirm) {

            if (validateCustomer()) {
                notify("Agregue datos de cliente", $rootScope.error);
                return false;
            }

            if ($scope.items == null || $scope.items == undefined || $scope.items.length == 0) {
                notify("Por favor agregue al menos un servicio", $rootScope.error);
                return false
            }

            const preQuotationArray = generateArray();
            
            $http({
                method: 'POST',
                url: '../../../PreQuotations/SavePreQuotation',
                data: preQuotationArray
            }).
                success(function (data, status, headers, config) {
                    if (data.success == 1) {
                        window.location = "../../../PreQuotations/ListPreQuotations";
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
        }
    };

    $scope.summaryFabrics = (fabrics) => {
        let fabricEdit = "";
        if (fabrics != null || fabrics != undefined) {
            if (fabrics) {
                fabrics.forEach(function (f, index) {
                    let space = (index + 1 == fabrics.length) ? ". " : ", ";
                    fabricEdit += f.fabric + space;
                });
            }
        }

        return fabricEdit;
    }

    $scope.summaryMeasures = (measures) => {
        let measuresEdit = "";
        if (measures != null || measures != undefined) {
            if (measures) {
                measures.forEach(function (m, index) {
                    let space = (index + 1 == measures.length) ? ". " : ", ";
                    measuresEdit += m.NombreMedida + ": " + m.Valor + space;
                });
            }
        }

        return measuresEdit;
    }

    //obtiene tab actual si es resumen ejecuta una funcion
    $('a[data-toggle="tab"]').on('click', function (e) {
        if ($(this).attr('href') === '#summary') {
            // Aquí puede llamar a su función
            $scope.executeFunction = true;
        } else {
            $scope.executeFunction = false;
        }
    });

    //Genera el array para enviar al servicio
    function generateArray() {

        return {
            "idPreCotizacion": 0,
            "Numero": $scope.number,
            "idUsuario1": $scope.sellerOne.idUsuario,
            "Usuario1": null,
            "idUsuario2": ($scope.sellerTwo == undefined) ? null : $scope.sellerTwo.idUsuario,
            "Usuario2": null,
            "idClienteFisico": ($scope.customer.type == "physical") ? $scope.physicalCustomer.idCliente : null,
            "ClienteFisico": null,
            "idClienteMoral": ($scope.customer.type == "moral") ? $scope.moralCustomer.idCliente : null,
            "ClienteMoral": null,
            "idDespacho": ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
            "Despacho": null,
            "Proyecto": $scope.project,
            "idDespachoReferencia": ($scope.customer.type == "office") ? $scope.officeCustomer.idDespacho : null,
            "DespachoReferencia": null,
            "idSucursal": $scope.branchID,
            "Sucursal": null,
            "Fecha": $scope.dateTime,
            "CantidadProductos": $scope.items.length ?? 0,
            "Total": null,
            "Estatus": null,
            "sEstatus": null,
            "NumberFactura": null,
            "TipoCliente": typeCustomer(),
            "oDetail": detailArray(),
            "oClienteFisico": null,
            "oClienteMoral": null,
            "oDespacho": null,
            "SumSubtotal": null,
            "Comentarios": $scope.comments,
            "Editar": false,
            "Selected": false,
            "Customer": null
        }
    }

    //Genera el array para el detalle de la precotizacion
    function detailArray() {
        let preQuotationDetail = [];
        if ($scope.items.length > 0) {
            $scope.items.forEach(e => preQuotationDetail.push({
                "idServicio": e.idService,
                "Descripcion": e.desc,
                "Comentarios": e.comments,
                "Cantidad": e.amount,
                "Imagen": e.image,
                "PDF": e.pdf,
                "fabrics": fabricsArray(e),
                "measures": measuresArray(e)
            }));
        }
        return preQuotationDetail;
    }

    //Genera el array de telas 
    function fabricsArray(item) {
        let array = [];
        if (item.fabrics != null || item.fabrics != undefined) {
            if (item.fabrics.length > 0) {
                item.fabrics.forEach(f => array.push({
                    "idServicio": item.idService,
                    "idTextiles": f.idFabric,
                    "CostoPorMts": f.price,
                    "ValorMts": null,
                    "CostoTotal": null
                }))
            }
        }
        return array;
    }

    //Genera el array de medidas
    function measuresArray(item) {
        let array = [];
        if (item.measures != null || item.measures != undefined) {
            if (item.measures.length > 0) {
                item.measures.forEach(m => array.push({
                    "idServicio": item.idService,
                    "idTipoMedida": m.idTipoMedida,
                    "Valor": m.Valor
                }))
            }
        }
        return array;
    }

    //Valida el tipo de cliente y lo retorna
    function typeCustomer() {
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
        return typeCustomer;
    }

    //Valida que se tenga un cliente seleccionado
    function validateCustomer() {
        if ($scope.physicalCustomer === undefined && $scope.moralCustomer === undefined && $scope.officeCustomer === undefined) {
            return true;
        }

        return false;
    }

    $scope.CleanData = () => {
        $scope.descService = "";
        $scope.newService.imagen = "";
        $scope.newService.salePriceService = 0;
        $scope.newService.amountService = 0;
        $scope.newService.commentsService = "";

        $("#holder").css("background-image", "");
        $("#editHolder").css("background-image", "");
        document.getElementById("fileInputEdit").value = "";

        //Inicializa arrays
        $scope.pdfName = null;
        $scope.itemsFabrics = new Array;
        $scope.newService = new models.ServicePreQuotation();
        $scope.listMeasuresStandar = new Array;
    }

    $scope.CloseEditModal = () => {
        $scope.CleanData();
        $("#modalEditService").modal("hide");
    }

    function ValidatePDF (file)  {

        let errorFile = false;

        let _file = file.files[0];

        let fileSize = _file.size; // tamaño en bytes

        let maxSize = 4194304; // 10 MG en bytes

        if (fileSize > maxSize) {
           errorFile = true;
        }

        return errorFile;
    }
});
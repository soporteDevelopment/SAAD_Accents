angular.module("General").controller('TicketsController', function (models, $scope, $http, $window, notify, $rootScope) {
    var arch = new FileReader();
    $scope.myFile = [];
    $scope.pendingStatusTickets = [];
    $scope.pendingStatusTicket = 1;
    $scope.activeStatusTicket = [];
    $scope.activeStatusTicket = 2;
    $scope.closedStatusTicket = [];
    $scope.closedStatusTicket = 3;
    $scope.status = [];
    $scope.priorities = [];
    $scope.updateStatus = {};

    $scope.getPendingStatusTickets = function () {
        $http({
            method: 'GET',
            url: '../../../Tickets/GetAllByStatus?idUser=' + $scope.createdBy + '&idStatusCredit=' + $scope.pendingStatusTicket + '&idAssignedUser=' + $scope.assignedBy
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.pendingStatusTickets = data.oData.Tickets;
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

    $scope.getActiveStatusTickets = function () {
        $http({
            method: 'GET',
            url: '../../../Tickets/GetAllByStatus?idUser=' + $scope.createdBy + '&idStatusCredit=' + $scope.activeStatusTicket + '&idAssignedUser=' + $scope.assignedBy
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.activeStatusTickets = data.oData.Tickets;
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

    $scope.getClosedStatusTickets = function () {
        $http({
            method: 'GET',
            url: '../../../Tickets/GetAllByStatus?idUser=' + $scope.createdBy + '&idStatusCredit=' + $scope.closedStatusTicket + '&idAssignedUser=' + $scope.assignedBy
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.closedStatusTickets = data.oData.Tickets;
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

    $scope.SaveAdd = function () {
        if ($scope.frmTicket.$valid) {

            $("#btnSave").button("loading");

            $http({
                method: 'POST',
                url: '../../../Tickets/Post',
                data: {
                    Title: $scope.title,
                    Description: $scope.description,
                    CreatedBy: $scope.idUser,
                    idPriorityTicket: $scope.idPriorityTicket,
                    idTypeTicket: $scope.idTypeTicket,
                    idAssignedUser: $scope.assigned

                }
            }).
                success(function (data, status, headers, config) {
                    $("#btnSave").button("reset");
                    if (data.success == 1) {
                        notify(data.oData.Message, $rootScope.success);
                        $('#ticket').modal('hide');
                        var ticket = data.oData.Ticket;
                        $scope.saveUploadImg(ticket.idTicket);
                        $scope.getPendingStatusTickets();
                        $scope.user = null;
                        $scope.title = null;
                        $scope.idTypeTicket = null;
                        $scope.idPriorityTicket = null;
                        $scope.description = null;
                        $scope.myFile = null;
                        $("#myFile").val(null);
                        $scope.assigned = null;
                    } else if (data.failure == 1) {
                        notify(data.oData.Error, $rootScope.error);
                    } else if (data.noLogin == 1) {
                        window.location = "../../../Access/Close";
                    }
                }).
                error(function (data, status, headers, config) {
                    $("#btnSave").button("reset");
                    notify("Ocurrío un error.", $rootScope.error);
                });
        }
    };

    $scope.saveUploadImg = function (id) {
        var formData = new FormData();
        var file = $("#myFile")[0];
        if (file.files.length > 0) {
            formData.append('file', file.files[0]);

            var xhr = new XMLHttpRequest();
            xhr.open('POST', "../../Tickets/UploadFile?id=" + id);

            xhr.onload = function () {
                var response = JSON.parse(xhr.responseText);
                if (response.success == 1) {
                    notify(response.oData.Message, $rootScope.success);
                } else {
                    notify(response.oData.Message, $rootScope.error);
                }

            };

            xhr.send(formData);
        }
    };

    $scope.openModalTicket = function () {
        $("#ticket").modal("show");
    };

    $scope.closeModalTicket = function () {
        $("#ticket").text("");
        $("#modalMsg").modal("hide");
    };

    $scope.openTicketDetail = function () {
        $("#ticketDetail").modal("show");
    };

    $scope.closegetTicket = function () {
        $("#ticketDetail").text("");
        $("#modalMsg").modal("hide");
    };

    $scope.loadUsers = function () {
        $http({
            method: 'POST',
            url: '../../../Users/ListAllUsers'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.users = data.oData.Users;
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

    $scope.loadAssigned = function () {
        $http({
            method: 'GET',
            url: '../../../Users/GetAttentionTicket'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.assignedUsers = data.oData.Users;
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

    $scope.getTicket = function (id) {
        $http({
            method: 'GET',
            url: '../../../Tickets/GetByid?idTicket=' + id
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.ticket = data.oData.Ticket;
                    $scope.openTicketDetail();
                } else if (data.failure == 1) {
                    notify(data.oData.Error, $rootScope.error);
                } else if (data.noLogin == 1) {
                    window.location = "../../../Access/Close";
                }

            });
    };

    $scope.getStatusTicket = function () {
        $http({
            method: 'GET',
            url: '../../../StatusTicket/Get'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.status = data.oData.Status;
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
    $scope.validateStatus = function () {
        if ($scope.ticket.idStatusTicket == 1) {
            $scope.updateStatusPending();
        } else
        if ($scope.ticket.idStatusTicket == 2) {
            $scope.updateStatusActive();
        } else
        if ($scope.ticket.idStatusTicket == 3) {
            $scope.updateStatusClosed();
        }
    };

    $scope.updateStatusPending = function () {
        $http({
            method: 'POST',
            url: '../../../Tickets/UpdateStatusPending',
            data: {
                id: $scope.ticket.idTicket,
                idStatusTicket: $scope.ticket.idStatusTicket,
                idAssignedUser: $scope.ticket.idAssignedUser,
                Estimated: $scope.ticket.Estimated,
                Filled: $scope.ticket.Filled
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.getPendingStatusTickets();
                    $scope.getActiveStatusTickets();
                    $scope.getClosedStatusTickets();
                    $("#ticketDetail").modal("hide");
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
    $scope.updateStatusActive = function () {
        $http({
            method: 'POST',
            url: '../../../Tickets/UpdateStatusActive',
            data: {
                id: $scope.ticket.idTicket,
                idStatusTicket: $scope.ticket.idStatusTicket,
                idAssignedUser: $scope.ticket.idAssignedUser,
                Estimated: $scope.ticket.Estimated,
                Filled: $scope.ticket.Filled,
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.getPendingStatusTickets();
                    $scope.getActiveStatusTickets();
                    $scope.getClosedStatusTickets();
                    $("#ticketDetail").modal("hide");
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
    $scope.updateStatusClosed = function () {
        $http({
            method: 'POST',
            url: '../../../Tickets/UpdateStatusClosed',
            data: {
                id: $scope.ticket.idTicket,
                idStatusTicket: $scope.ticket.idStatusTicket,
                idAssignedUser: $scope.ticket.idAssignedUser,
                Estimated: $scope.ticket.Estimated,
                Filled: $scope.ticket.Filled  
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.getPendingStatusTickets();
                    $scope.getActiveStatusTickets();
                    $scope.getClosedStatusTickets();
                    $("#ticketDetail").modal("hide");
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
    $scope.getPriorityTicket = function () {
        $http({
            method: 'GET',
            url: '../../../PriorityTicket/Get'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.priorities = data.oData.Priorities;
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

    $scope.getTypeTicket = function () {
        $http({
            method: 'GET',
            url: '../../../TypeTicket/Get'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.types = data.oData.Types;
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
    $scope.deactiveTicket = function (id) {
        $http({
            method: 'POST',
            url: '../../../Tickets/Deactive',
            data: {
                id: id
            }
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {
                    $scope.getClosedStatusTickets();
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
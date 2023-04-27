
angular.module("General").controller('HomeController', function ($scope, $http, $window, notify, $rootScope) {
    
    $scope.openedDateEvent = false;

    $scope.GetEvents = function () {

        var self = this;

        $.ajax({
            url: "/Events/GetEvents",
            method: 'POST',
            success: function (response) {
                if (response.success == 1) {

                    var events = new Array();

                    response.oData.Events.forEach(function (value) {

                        events.push({
                            id: value.idEvento,
                            title: value.CreadoPor,
                            allDay: value.TodoDia,
                            start: $scope.FormatDate(value.Fecha) + "T" + value.HoraInicio + ":00Z",
                            end: $scope.FormatDate(value.Fecha) + "T" + value.HoraFin + ":00Z",
                            color: value.Color,
                            backgroundColor: value.Fondo,
                            borderColor: value.Fondo,
                            textColor: value.Fuente,
                            description: value.Nombre + "\n" + value.CreadoPor
                        });

                    });

                    $('#calendar').fullCalendar({
                        defaultView: 'agendaWeek',
                        locale: 'es',
                        contentHeight: 425,
                        header: {
                            center: 'addEventButton'
                        },
                        customButtons: {
                            addEventButton: {
                                text: 'Agregar evento',
                                click: function () {
                                    window.location = "/Events/Index"
                                }
                            }
                        },
                        eventSources: [
                            {
                                events: events,
                                color: 'black',
                                textColor: 'yellow'
                            }
                        ],
                        eventClick: function (calEvent, jsEvent, view) {
                            $scope.VerifyEvent(calEvent.id);                           
                        },
                        minTime: "09:00:00",
                        maxTime: "20:00:00",
                        eventMouseover: function (data, event, view) {

                            var tooltip = '<div class="tooltiptopicevent" style="width:auto;height:auto;background:#feb811;position:absolute;z-index:10001;padding:10px 10px 10px 10px ;  line-height: 200%;">' + 'Tipo: ' + ': ' + ((data.Tipo == 1) ? "EN TIENDA" : "SALIDA A VISTA") + '<br/> Evento: ' + ': ' + data.description + '</div>';

                            $("body").append(tooltip);
                            $(this).mouseover(function (e) {
                                $(this).css('z-index', 10000);
                                $('.tooltiptopicevent').fadeIn('500');
                                $('.tooltiptopicevent').fadeTo('10', 1.9);
                            }).mousemove(function (e) {
                                $('.tooltiptopicevent').css('top', e.pageY + 10);
                                $('.tooltiptopicevent').css('left', e.pageX + 20);
                            });


                        },
                        eventMouseout: function (data, event, view) {
                            $(this).css('z-index', 8);

                            $('.tooltiptopicevent').remove();

                        }
                    });

                } else if (response.failure == 1) {
                    alert(response.oData.Error);
                } else if (response.noLogin == 1) {
                    window.location = "/Access/Close";
                }
            },
            error: function () {
                alert('Ocurrío un error');
            }
        });

    };

    $scope.GetEventsToday = function () {

        var self = this;

        $.ajax({
            url: "/Events/GetEventsToday",
            method: 'GET',
            success: function (response) {
                if (response.success == 1) {

                    $scope.events = new Array();

                    response.oData.Events.forEach(function (value) {

                        $scope.events.push({
                            id: value.idEvento,
                            title: value.Nombre + "\n" + value.CreadoPor,
                            allDay: value.TodoDia,
                            start: value.HoraInicio,
                            end: value.HoraFin,
                            color: value.Color,
                            backgroundColor: value.Fondo,
                            borderColor: value.Fondo,
                            textColor: value.Fuente,
                        });

                    });

                    if ($scope.events.length > 0) {
                        $("#modalEvents").modal("show");
                    }

                } else if (response.failure == 1) {
                    alert(response.oData.Error);
                } else if (response.noLogin == 1) {
                    window.location = "/Access/Close";
                }
            },
            error: function () {
                alert('Ocurrío un error');
            }
        });

    };

    $scope.PendingOutProducts = function (count) {

        $scope.GetEventsToday();

        if (count > 0) {

            $http({
                method: 'POST',
                url: '../../../OutProducts/GetPendingOutProducts'
            }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {

                    $scope.Pendings = data.oData.Pendings;

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

    };

    $scope.GetSalesOnLine = function (count) {

        $http({
            method: 'GET',
            url: '../../../SalesOnLine/GetBySellerId'
        }).
            success(function (data, status, headers, config) {
                if (data.success == 1) {

                    $scope.Sales = data.oData.sales;

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

    $scope.VerifyEvent = function (eventID) {

        $http({
            method: 'POST',
            url: '../../../Events/VerifyEvent',
            params: {
                eventID: eventID
            }
        }).
        success(function (data, status, headers, config) {
            if (data.success == 1) {

                window.location = "/Events/Update?idEvent=" + eventID;

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

    $scope.FormatDate = function (value) {
        if (!value) return ''
        var date = new Date(parseInt(value.substr(6)));

        return date.getFullYear() + "-" + (((parseInt(date.getMonth() + 1)) < 10) ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1)) + "-" + (((parseInt(date.getDate())) < 10) ? "0" + (date.getDate()) : (date.getDate()));
    };

});

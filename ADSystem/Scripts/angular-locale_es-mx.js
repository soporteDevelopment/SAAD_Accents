'use strict';
angular.module("ngLocale", [], ["$provide", function ($provide) {
    var PLURAL_CATEGORY = { ZERO: "zero", ONE: "one", TWO: "two", FEW: "few", MANY: "many", OTHER: "other" };
    $provide.value("$locale", {
        "DATETIME_FORMATS": {
            "AMPMS": [
              "a.m.",
              "p.m."
            ],
            "DAY": [
              "Domingo",
              "Lunes",
              "Martes",
              "Mi\u00e9rcoles",
              "Jueves",
              "Viernes",
              "S\u00e1bado"
            ],
            "ERANAMES": [
              "antes de Cristo",
              "despu\u00e9s de Cristo"
            ],
            "ERAS": [
              "a. C.",
              "d. C."
            ],
            "FIRSTDAYOFWEEK": 6,
            "MONTH": [
              "Enero",
              "Febrero",
              "Marzo",
              "Abril",
              "Mayo",
              "Junio",
              "Julio",
              "Agosto",
              "Septiembre",
              "Octubre",
              "Noviembre",
              "Diciembre"
            ],
            "SHORTDAY": [
              "Dom.",
              "Lun.",
              "Mar.",
              "Mi\u00e9.",
              "Jue.",
              "Vie.",
              "S\u00e1b."
            ],
            "SHORTMONTH": [
              "Ene",
              "Feb",
              "Mar",
              "Abr",
              "May",
              "Jun",
              "Jul",
              "Ago",
              "Sep",
              "Oct",
              "Nov",
              "Dic"
            ],
            "WEEKENDRANGE": [
              5,
              6
            ],
            "fullDate": "EEEE, d 'de' MMMM 'de' y",
            "longDate": "d 'de' MMMM 'de' y",
            "medium": "dd/MM/y h:mm:ss a",
            "mediumDate": "dd/MM/y",
            "mediumTime": "h:mm:ss a",
            "short": "dd/MM/yy h:mm a",
            "shortDate": "dd/MM/yy",
            "shortTime": "h:mm a"
        },
        "NUMBER_FORMATS": {
            "CURRENCY_SYM": "$",
            "DECIMAL_SEP": ".",
            "GROUP_SEP": ",",
            "PATTERNS": [
              {
                  "gSize": 3,
                  "lgSize": 3,
                  "maxFrac": 3,
                  "minFrac": 0,
                  "minInt": 1,
                  "negPre": "-",
                  "negSuf": "",
                  "posPre": "",
                  "posSuf": ""
              },
              {
                  "gSize": 3,
                  "lgSize": 3,
                  "maxFrac": 2,
                  "minFrac": 2,
                  "minInt": 1,
                  "negPre": "-\u00a4",
                  "negSuf": "",
                  "posPre": "\u00a4",
                  "posSuf": ""
              }
            ]
        },
        "id": "es-mx",
        "pluralCat": function (n, opt_precision) { if (n == 1) { return PLURAL_CATEGORY.ONE; } return PLURAL_CATEGORY.OTHER; }
    });
}]);
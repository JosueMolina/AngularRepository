var app = angular.module("app", ["ngRoute", "ngAnimate"]);

//Servicio
app.factory("DataService", function ($http, $q) {

    var obtenerListas = function () {
        var deferred = $q.defer();
        $http.get("/api/DatosRepositorio/").success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    }
    
    var obtenerListasWeb = function () {
        var deferred = $q.defer();
        $http.get("api/DatosRepositorio?localData=false").success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    }

    var guardarCategorias = function () {
        var deferred = $q.defer();
        $http.post("/api/CategoriasRepositorio/", categoria).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    }

    var guardarEnlaces = function (enlace) {
        var deferred = $q.defer();
        $http.post("/api/DatosRepositorio/", enlace).success(deferred.resolve).error(deferred.reject)
        return deferred.promise;
    }

    var guardarPuntuacion = function (enlacePuntuado, puntuacion) {
        var deferred = $q.defer();
        $http.post("/api/DatosRepositorio/guardarPuntuacion/", { enlacePuntuado: enlacePuntuado, puntuacion : puntuacion }).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    }

    return {
        obtenerListas: obtenerListas,
        guardarEnlaces: guardarEnlaces,
        obtenerListasWeb: obtenerListasWeb,
        guardarCategorias: guardarCategorias,
        guardarPuntuacion : guardarPuntuacion 
    }
});

//Controlador
app.controller('listasController', function ($scope, DataService) {

    $scope.cargando = true;
    var estaListo = false;

    //if (!estaListo)
    //{
    //    DataService.obtenerListasWeb().then
    //        (function (response) {
    //            $scope.listas = response.Listas;
    //            AgregarClases();
    //            Puntuando();
    //            var stringJson = JSON.stringify($scope.listas);
    //            stringJson = replaceAll(stringJson, "#text", "text");
    //            stringJson = replaceAll(stringJson, "@class", "class");
    //            stringJson = replaceAll(stringJson, "@name", "name");
    //            stringJson = replaceAll(stringJson, "@href", "href");

    //            $scope.listas = JSON.parse(stringJson);
    //            $scope.cargando = false;
                

    //            $scope.listas.Lista.forEach(function (data) {
    //                categoria = {};
    //                categoria.NombreCategoria = data.h2.text;
    //                DataService.guardarCategorias(categoria).then(function (response) { console.log(response); }, function () { });
    //                data.ul.li.forEach(function (itemEnlace) {
    //                    nuevoEnlace = {};
    //                    nuevoEnlace.Nombre = itemEnlace.a.text;
    //                    nuevoEnlace.Link = itemEnlace.a.href;
    //                    nuevoEnlace.NumeroPuntuaciones = 0;
    //                    nuevoEnlace.Puntuacion = 0;
    //                    nuevoEnlace.IdCategoria = 1;
    //                    //DataService.guardarEnlaces(nuevoEnlace);
    //                })

    //                estaListo = true;
    //            });

    //            setTimeout(function () {
    //                $("input[type='range']").on("input change", function () {
    //                    $(this).parent(".col-sm-6").siblings(".col-sm-5").find("span").html($(this).val() + "<small style='font-size:.5em;'> pts.</small>");
    //                });

    //            }, 10); 
    //        },
    //         function () { });

    //}

    DataService.obtenerListas().then(function (response) {
        $scope.enlaces = response;
        InicializarIntervalos();

    }, function () { });


    function replaceAll(text, busca, reemplaza) {
        while (text.toString().indexOf(busca) != -1)
        text = text.toString().replace(busca, reemplaza);
        return text;
    }

    $scope.enviarPuntuacion = function (e) {
        var elem = angular.element(e.target);
        var span = elem.parent(".col-sm-2").parent(".row").find(".col-sm-9").find("span");
        var puntuacionUsuario = span.text().substr(0, span.text().indexOf(' '));
        var enlace = elem.parents(".panel").find(".panel-heading").find("h3").text();

        DataService.guardarPuntuacion(enlace, puntuacionUsuario).then(function () {
            $(".Items").fadeOut(100);

            DataService.obtenerListas().then(function (response) {
                $scope.enlaces = response;
                InicializarIntervalos()

            }, function () { });

        }, function () { });
    }

    $scope.KeyDownFiltro = function () {
        AgregarClases();
    }

    $scope.setClass = function (index, items)
    {
        if (index % 2 === 0) return "par";
        return "impar";
    }

});

//Filtros
app.filter("mySecondFilter", function () {
    return function (input, row, numColumns) {
        var returnArray = [];
        for (var x = row * numColumns; x < row * numColumns + numColumns; x++) {
            if (x < input.length) {
                returnArray.push(input[x]);
            }
            else {
               // returnArray.push("");
            }
        }
        return returnArray;
    }
}).filter("myFilter", function () {
    return function (input, numColumns) {
        var filtered = [];
        for (var x = 0; x < input.length; x++) {
            if (x % numColumns === 0) {
                filtered.push(filtered.length);
            }
        }
        return filtered;
    }
});

//Funciones Extras
var InicializarIntervalos = function(){
    AgregarClases();
    Puntuando();
    Bindings();
}
var AgregarClases = function () {
    setTimeout(function () {
        $(".Item.ng-scope.par .col-sm-4:nth-child(odd) .panel")
            .addClass("panel-info")
            .removeClass("panel-danger panel-warnig panel-success")
            .find("a[class*='btn']").addClass("btn-warning");
        $(".Item.ng-scope.par .col-sm-4:nth-child(even) .panel")
            .addClass("panel-danger")
            .removeClass("panel-info panel-warnig panel-success")
            .find("a[class*='btn']").addClass("btn-success");
        $(".Item.ng-scope.impar .col-sm-4:nth-child(even) .panel")
            .addClass("panel-warning")
            .removeClass("panel-danger panel-info panel-success")
            .find("a[class*='btn']").addClass("btn-danger");
        $(".Item.ng-scope.impar .col-sm-4:nth-child(odd) .panel")
            .addClass("panel-success")
            .removeClass("panel-danger panel-warnig panel-info")
            .find("a[class*='btn']").addClass("btn-info");
    }, 500);
}

var Puntuando = function () {
    setInterval(function () {
        $(".puntuar").on("click", function () {
            $(this).parents(".panel-body").fadeOut('500', function () {
                $(this).siblings(".panel-body").fadeIn();
            });
        });

    }, 600);
}

var Bindings = function () {
    setTimeout(function () {
        $("input[type='range']").on("input change", function () {
            $(this).parent(".col-sm-6").siblings(".col-sm-5").find("span")
                .html($(this).val() + "<small style='font-size:.5em;'> pts.</small>");
        });
        $(".Items").fadeIn(1500);
    }, 10);
}
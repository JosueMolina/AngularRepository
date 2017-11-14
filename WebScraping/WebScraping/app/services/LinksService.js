app.factory("LinksService", function ($http, $q) {
  var getLinksLocal = function () {
    var deferred = $q.defer();
    $http.get("/api/LinksRepository/").success(deferred.resolve).error(deferred.reject);
    return deferred.promise;
  }

  var getLinksWeb = function () {
    var deferred = $q.defer();
    $http.get("/api/LinksRepository?localData=false").success(deferred.resolve).error(deferred.reject);
    return deferred.promise;
  }

  var addCategory = function () {
    var deferred = $q.defer();
    $http.post("/api/CategoriesRepository/", category).success(deferred.resolve).error(deferred.reject);
    return deferred.promise;
  }

  var addLink = function (link) {
    var deferred = $q.defer();
    $http.post("/api/LinksRepository/", link).success(deferred.resolve).error(deferred.reject);
    return deferred.promise;
  }

  return {
    getLinksLocal: getLinksLocal,
    addLink: addLink,
    getLinksWeb: getLinksWeb,
    addCategory: addCategory
  }
});
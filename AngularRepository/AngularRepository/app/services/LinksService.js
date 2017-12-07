app.factory("LinksService", function ($http, $q) {
  var getLinksLocal = function () {
    var deferred = $q.defer();
    $http.get("http://localhost:8080/api/LinksRepository/").success(deferred.resolve).error(deferred.reject);
    return deferred.promise;
  }

  var getLinksWeb = function () {
    var deferred = $q.defer();
    $http.get("http://localhost:8080/api/LinksRepository?localData=false").success(deferred.resolve).error(deferred.reject);
    return deferred.promise;
  }

  var addCategory = function () {
    var deferred = $q.defer();
    $http.post("http://localhost:8080/api/CategoriesRepository/", category).success(deferred.resolve).error(deferred.reject);
    return deferred.promise;
  }

  var addLink = function (link) {
    var deferred = $q.defer();
    $http.post("http://localhost:8080/api/LinksRepository/", link).success(deferred.resolve).error(deferred.reject);
    return deferred.promise;
  }

  var gettingRate = function (linkName) {
    var deferred = $q.defer();
    $http.get("http://localhost:8080/api/RatingsRepository?linkName=" + linkName).success(deferred.resolve).error(deferred.reject);
    return deferred.promise;
  }

  var rating = function (ratingModel) {
    var deferred = $q.defer();
    $http.post("http://localhost:8080/api/RatingsRepository/", ratingModel).success(deferred.resolve).error(deferred.reject);
    return deferred.promise;
  }

  return {
    getLinksLocal: getLinksLocal,
    addLink: addLink,
    getLinksWeb: getLinksWeb,
    addCategory: addCategory,
    gettingRate: gettingRate,
    rating: rating
  }
});
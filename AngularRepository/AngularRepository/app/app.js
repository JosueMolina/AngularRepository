var app = angular.module("app", ["ngRoute"]);

//Filters
app.filter("rowColumnFilter", function () {
  return function (input, row, numColumns) {
    var returnArray = [];
    for (var x = row * numColumns; x < row * numColumns + numColumns; x++) {
      if (x < input.length) {
        returnArray.push(input[x]);
      }
    }
    return returnArray;
  }
}).filter("rowFilter", function () {
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
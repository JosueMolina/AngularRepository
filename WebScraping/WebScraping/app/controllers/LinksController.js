app.controller('linksController', function ($scope, LinksService) {

  $scope.loading = true;
  var isReady = false;

  if (!isReady) {
    LinksService.getLinksWeb().then(
      function (response) {
        $scope.lists = response.Lists;

        helper.addClasses();
        helper.rating();

        var jsonString = JSON.stringify($scope.lists);
        jsonString = helper.replaceAll(jsonString, "#text", "text");
        jsonString = helper.replaceAll(jsonString, "@class", "class");
        jsonString = helper.replaceAll(jsonString, "@name", "name");
        jsonString = helper.replaceAll(jsonString, "@href", "href");

        var localLists = JSON.parse(jsonString);

        localLists.List.forEach(function(data) {

          data.ul.li.forEach(function(link) {

            if (link.a.text == undefined || link.a.href == undefined || link.a.constructor === Array)
            {
              data.ul.li.splice(data.ul.li.indexOf(link), 1);
            }

          });

        });

        //Not very effecient
        localLists.List.forEach(function (data) {
          data.ul.li.forEach(function (link) {
            if (link.a.text == undefined || link.a.href == undefined || link.a.constructor === Array) {
              data.ul.li.splice(data.ul.li.indexOf(link, 1));
            }
          });
        });

        $scope.lists = localLists;
        $scope.loading = false;

        $scope.lists.List.forEach(function (data) {

          category = {};
          category.CategoryName = data.h2.text;
          LinksService.addCategory(category).then(function(response) {

            category.Id = response.id;

            data.ul.li.forEach(function (link) {

              if (link.text != undefined) {

                link.text[0] = helper.replaceAll(link.text[0], "[", "");
                link.text[0] = helper.replaceAll(link.text[0], "]", "");
                link.text[0] = helper.replaceAll(link.text[0], "(", "");
                link.text[0] = helper.replaceAll(link.text[0], ")", "");

                link.text = link.text[0];
              }

              newLink = {};
              newLink.Name = link.a.text;
              newLink.LinkValue = link.a.href;
              newLink.NumberOfRatings = 0;
              newLink.Rating = 0;
              newLink.IdCategory = category.Id;

              LinksService.addLink(newLink).then(function () { }, function () { });
            });


          }, function () { });

          isReady = true;

        });

        setTimeout(function () {
          $("input[type='range']").on("input change", function () {
            $(this).parent(".col-sm-6").siblings(".col-sm-5").find("span")
              .html($(this).val() + "<small style='font-size:.5em;'> pts.</small>");
          });

        }, 10);
      },
      function () { }
    );
  }

  $scope.enviarPuntuacion = function (e) {
    var elem = angular.element(e.target);
  }

  $scope.KeyDownFilter = function () {
    helper.addClasses();
  }

  $scope.setClass = function (index) {
    if (index % 2 === 0)
      return "even";
    return "odd";
  }
});
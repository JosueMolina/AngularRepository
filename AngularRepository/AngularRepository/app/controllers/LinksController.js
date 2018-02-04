app.controller('linksController', function ($scope, LinksService) 
{
  $scope.loading = true;
  var isReady = false;

  if (!isReady) 
  {
    LinksService.getLinksWeb().then(function (response) 
    {
      loadLinks(response);
    }, 
    function () 
    { 
        console.log('It wasn\'t possible to load the data');
    });
  }

  function loadLinks(response) 
  {
    helper.addClasses();
    helper.rating();

    var tempLists = buildJsonList(response.Lists);

    //insert fewer links using this method (519~518)
    //tempLists = removeBrokenLinks(tempLists);

    //Not very efficient, but insert more links 
    //duplicating this block, don't know why (546)
    tempLists.List.forEach(function(data) {
      data.ul.li.forEach(function(link) {
        if ( link.a.text == undefined
          || link.a.href == undefined
          || link.a.constructor === Array )
        {
          data.ul.li.splice(data.ul.li.indexOf(link), 1);
        }
      });
    });

    tempLists.List.forEach(function (data) {
      data.ul.li.forEach(function (link) {
        if ( link.a.text == undefined
          || link.a.href == undefined
          || link.a.constructor === Array )
        {
          data.ul.li.splice(data.ul.li.indexOf(link, 1));
        }
      });
    });

    $scope.lists = tempLists;
    $scope.loading = false;

    insertNewData($scope.lists);

    addEventListeners();

  };

  function insertNewData(lists)
  {
    $scope.lists.List.forEach(function (data) {

      category = {};
      category.CategoryName = data.h2.text;

      LinksService.addCategory(category).then( function(response)
      {
        addingLinks(response, data);    
      }, 
      function () 
      { 
        console.log('An error ocurred while storing/rendering the data');
      });

      isReady = true;

    });
  }

  function addingLinks(response, data)
  {
    category.Id = response.id;
    
    data.ul.li.forEach( function (link) 
    {
      newLink = buildLink(link, category.Id);
      LinksService.addLink(newLink).then(function(response){
        if(response.numberOfRatings == 0)
        {
          link.dataRating = -1;
        } else 
        {
          link.dataRating = response.rating / response.numberOfRatings;
        }
      }, function(){});
    });
  }

  $scope.rating = function (e) 
  {
    var $elem = angular.element(e.target);

    var rate =  $elem.parent().siblings('.rate.col-sm-9')
    .children('.row').children('.col-sm-6').children('input').val();

    var title = $elem.parent().parent().parent()
    .siblings('.panel-heading').children('h3').text();

    LinksService.rating (
      { 
        'rate': rate,
       'title': title 
      }
    ).then(function (response)
    { 
      $elem.parent().parent().parent().siblings('.panel-body.row')
      .children('.col-sm-8').children('.col-md-12').children('span')
      .html(response + '<small style="font-size: .5em;">pts.</small>');

      toastr.info('Rating has been updated');
    }, 
    function()
    {
      console.log('There was an error sending the data');
      toastr.error('There was an error sending the data');
    });
  }

  $scope.KeyDownFilter = function () {
    helper.addClasses();
  }

  $scope.setClass = function (index)
  {
    if (index % 2 === 0)
      return "even";
    return "odd";
  }

}); //End of angular controller block

function buildJsonList(lists)
{
  var jsonString = JSON.stringify(lists);
  jsonString = helper.replaceAll(jsonString, "#text", "text");
  jsonString = helper.replaceAll(jsonString, "@class", "class");
  jsonString = helper.replaceAll(jsonString, "@name", "name");
  jsonString = helper.replaceAll(jsonString, "@href", "href");

  return JSON.parse(jsonString);
}

//insert fewer links using this method (519~518) 
function removeBrokenLinks(jsonLists) 
{
  jsonLists.List.forEach(function (data) {
    data.ul.li.forEach(function (link) {
      if ( link.a.text == undefined
        || link.a.href == undefined
        || link.a.constructor === Array )
      {
        data.ul.li.splice(data.ul.li.indexOf(link, 1));
      }
    });
  });

  return jsonLists;
}

function buildLink(link, categoryId)
{
  newLink = {};
  newLink.Name = link.a.text;

  if (link.text != undefined) {
    
    link.text[0] = helper.replaceAll(link.text[0], "[", "");
    link.text[0] = helper.replaceAll(link.text[0], "]", "");
    link.text[0] = helper.replaceAll(link.text[0], "(", "");
    link.text[0] = helper.replaceAll(link.text[0], ")", "");

    link.text = link.text[0];

    if (link.text.trim().length > 0)
      newLink.Name = link.text;
  }

  newLink.LinkValue = link.a.href;
  newLink.NumberOfRatings = 0;
  newLink.Rating = 0;
  newLink.IdCategory = categoryId;

  //console.log(newLink.Name);

  return newLink;
}

function addEventListeners()
{
  setTimeout( function () {
    $("input[type='range']").on("input change", function () {
      $(this).parent(".col-sm-6").siblings(".col-sm-5").find("span")
        .html($(this).val() + "<small style='font-size:.5em;'> pts.</small>");
    });
  }, 10);
}
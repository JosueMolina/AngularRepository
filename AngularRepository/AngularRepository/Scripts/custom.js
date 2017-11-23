(function($, window, undefined) {

  $(document).on('ready', function() {
    $('#input-search').focus();
  });

})(jQuery, window, undefined);

helper = {
  replaceAll : function (text, textToFind, textToReplace) {
    while (text.toString().indexOf(textToFind) != -1)
      text = text.toString().replace(textToFind, textToReplace);
    return text;
  },

  addClasses : function () {
    setTimeout( function () {

      $(".item.ng-scope.even .col-sm-4:nth-child(odd) .panel")
          .addClass("panel-info")
          .removeClass("panel-danger panel-warnig panel-success")
          .find("a[class^='btn']").addClass("btn-warning");

      $(".item.ng-scope.even .col-sm-4:nth-child(even) .panel")
          .addClass("panel-danger")
          .removeClass("panel-info panel-warnig panel-success")
          .find("a[class^='btn']").addClass("btn-success");

      $(".item.ng-scope.odd .col-sm-4:nth-child(even) .panel")
          .addClass("panel-warning")
          .removeClass("panel-danger panel-info panel-success")
          .find("a[class^='btn']").addClass("btn-danger");

      $(".item.ng-scope.odd .col-sm-4:nth-child(odd) .panel")
          .addClass("panel-success")
          .removeClass("panel-danger panel-warnig panel-info")
          .find("a[class^='btn']").addClass("btn-info");

    }, 500);
  },

  rating: function () {
    $("body").on("click", '.rating-button', function () {
      $(this).parents(".panel-body").fadeOut('500', function () {
        $(this).siblings(".panel-body").fadeIn();
      });
    });
  }

}

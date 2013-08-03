$(function () {
    var geocode = function () {
        var location = $("#LocationPlaceholder").val();

        $("#LocationPlaceholder").addClass("progress-indicator");

        $("#location-suggestions")
          .empty()
          .append("<div style='color: #828282; font-weight: bold;'>" + CreateEventResources.Searching + "</div>")
          .show();

        $.post(
            "/Locations/Geocode",
            { locationsQuery: location },
            function (results) {
                if (results.errorMessage) {
                    $("#LocationPlaceholder").removeClass("progress-indicator");

                    $("#location-suggestions")
                      .empty()
                      .append("<div class='not-found'>" + results.errorMessage + "</div>")
                      .show();

                    return;
                }

                $("#location-suggestions").empty();

                var resultsCount = results.length;

                if (resultsCount > 0) {
                    $("#location-suggestions").append("<div style='color: #828282; font-weight: bold;'>" + CreateEventResources.DidYouMean + "</div>");
                } else {
                    $("#location-suggestions").append("<div class='not-found'>" + CreateEventResources.NoResultsFound + "</div>");
                }

                $.each(results, function (index, value) {
                    $("#location-suggestions")
                        .append("<div id='location-" + value.Id + "' class='location-suggestion'>" + value.Formatted + "</div>");

                    $("#location-" + value.Id)
                        .data({
                            id: value.Id,
                            formattedAddress: value.Formatted
                        });
                });

                $("#LocationPlaceholder").removeClass("progress-indicator");
                $("#location-suggestions").show();
            });
    };
    
    var browserLanguage = window.navigator.userLanguage || window.navigator.language;
    var regional = $.datepicker.regional['en-GB'];

    if (browserLanguage.indexOf("pl") !== -1) {
        regional = $.datepicker.regional['pl'];
    } else if (browserLanguage == "en-US") {
        regional = $.datepicker.regional[''];
    }

    $.datepicker.setDefaults(regional);
    $(".datepicker").datepicker({ dateFormat: 'yy-mm-dd' });

    // handle fields with predefined prefix
    var $website = $("#Website");
    var $twitterHashTag = $("#TwitterHashTag");
    var $twitterProfile = $("#TwitterProfile");

    if ($website.val() == "") {
        $website.val("http://");
    }

    if ($twitterHashTag.val() == "") {
        $twitterHashTag.val("#");
    }

    if ($twitterProfile.val() == "") {
        $twitterProfile.val("@");
    }

    // handle tooltips
    var showTooltip = function () {
        var positionTop = $(this).position().top;
        var positionLeft = $(this).position().left + 405;
        var id = $(this).attr("id");

        if (id == "IsFreeOfCharge") {
            positionLeft -= 145;
        }

        if (id == "EndDate") {
            positionLeft -= 120;
        }

        if (id == "TwitterProfile") {
            positionLeft -= 193;
        }

        var tooltipContent = $("#" + id + "TooltipContent").text();

        $("#formTooltip .tooltipContent").text("").append(tooltipContent);
        $("#formTooltip")
            .css({ top: positionTop, left: positionLeft })
            .show();
    };

    var hideTooltip = function () {
        $("#formTooltip").hide();
        $("#formTooltip .tooltipContent").text("");
    };

    var inputsWithTooltip = ["input[type='text']", "select", "textarea", "input[type='checkbox']"];

    // TODO: ME .focus() doesn't fire in Chrome/Safari for checkboxes/radios
    $.each(inputsWithTooltip, function (index, value) {
        $(value)
            .focus(showTooltip)
            .blur(hideTooltip);
    });

    $("#Name").focus();

    $("form").submit(function () {
        var website = $("#Website").val();
        var twitterHashtag = $("#TwitterHashTag").val();
        var twitterProfile = $("#TwitterProfile").val();

        if (website == "http://") {
            $("#Website").val("");
        }

        if (twitterHashtag == "#") {
            $("#TwitterHashTag").val("");
        }

        if (twitterProfile == "@") {
            $("#TwitterProfile").val("");
        }

        return true;
    });

    $(".location-suggestion")
      .live("click", function () {
          var locationData = $(this).data();

          $("#LocationId").val(locationData.id);

          $("#LocationPlaceholder").val(locationData.formattedAddress);
          $("#location-suggestions").empty().hide();
      });

    $("#LocationPlaceholder")
    .data("timeout", null)
    .keyup(function () {
        clearTimeout($(this).data("timeout"));

        $("#LocationId").val("");

        var phraseLen = $(this).val().length;

        if (phraseLen >= 2) {
            $(this).data("timeout", setTimeout(geocode, 700));
        }
    });
});
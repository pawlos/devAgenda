$(function () {
    $("#rsvp")
      .click(function () {
          $("#rsvp").hide();

          $("#rsvp-container").append("<div id='rsvp-progress-bar'>&nbsp;</div>");

          $.post(
              "/Events/RSVP",
              { eventId: $("#Id").val() },
              function (data) {
                  $("#rsvp-progress-bar").remove();

                  if (data.errorMessage) {
                      $("#rsvp")
                    .after("<div class='ajax-error'>" + data.errorMessage + "</div>")
                    .show();

                      return;
                  }

                  $("#rsvp-info")
                    .css({ display: "inline" });

                  $("#attendees-container-inner")
                      .append(
                      "<a id='user-" + data.UserId + "' class='attendee-avatar' style='display: none;' href='" + data.UserProfileLink + "' title='" + data.UserName + "'>" +
                          "<img width='30px' height='30px' src='" + data.AvatarLink + "' alt='" + data.UserName + "' />" +
                      "</a>");

                  $("#user-" + data.UserId).fadeIn();
              });
      });

    $("#undo-rsvp")
      .click(function () {
          $("#rsvp-info").hide();

          $("#rsvp-container").append("<div id='rsvp-progress-bar'>&nbsp;</div>");

          $.post(
              "/Events/UndoRSVP",
              { eventId: $("#Id").val() },
              function (data) {
                  $("#rsvp-progress-bar").remove();

                  if (data.errorMessage) {
                      $("#rsvp-info")
                    .after("<div class='ajax-error'>" + data.errorMessage + "</div>")
                    .show();

                      return;
                  }

                  $("#rsvp").show();

                  $("#user-" + data.UserId).fadeOut().remove();
              });
      });
});
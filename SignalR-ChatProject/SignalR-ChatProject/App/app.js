var hub = $.connection.chatHub;



hub.client.message = function (msg) {
    $("#message").append("<li>" + msg + "</li>");
}

$.connection.hub.start(function() {
    $("#send").click(function() {

        hub.server.gonder($("#txt").val());
        $("#txt").val(" ");

    });
});


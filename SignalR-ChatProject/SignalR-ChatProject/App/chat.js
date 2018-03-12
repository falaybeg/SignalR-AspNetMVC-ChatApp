var chatHub = $.connection.chatHub;
$.connection.hub.start();

chatHub.client.message = function (msg) {
    $("#message").append("<li>" + msg + "</li>");
}

$("#send").click(function () {
    var message = $("#txt").val();
    chatHub.server.sendMessages(message);
    message.val("");
});
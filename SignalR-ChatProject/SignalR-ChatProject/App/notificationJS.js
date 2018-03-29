var nothub = $.connection.notificationHub;

$.connection.hub.start();

$("#btnShare").click(function () {
    var msg = $("#txtNotification").val();

    if (msg.length > 0) {
        nothub.server.sendNotification(msg);
        $("#txtNotification").val("").focus();
    }
});


nothub.client.receiveNotification = function (username, message, time) {

    $("#noti").append("<div ><span>" + username + "</span>--- " + message + "</div>");
}
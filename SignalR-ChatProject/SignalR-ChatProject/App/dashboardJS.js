var chatHub = $.connection.chatHub;


$.connection.hub.start().done();

chatHub.client.totalMessages = function (count) {
    $("#totalMessage").text(count);
}

chatHub.client.totalMymessages = function (count) {
    $("#totalMyMessage").text(count);
}

chatHub.client.todayMessages = function (count) {
    $("#todayMessages").text(count);
}

chatHub.client.todayMyMessages = function (count) {
    $("#todayMyMessages").text(count);
}

chatHub.client.totalMember = function (count) {
    $("#totalMember").text(count);
}


//chatHub.client.totalmessage = function (count) {
//    $("#todayMessage").text(count);
//}

//chatHub.client.totalmessage = function (count) {
//    $("#todayMyMessage").text(count);
//}

//chatHub.client.totalmessage = function (count) {
//    $("#totalMembers").text(count);
//}


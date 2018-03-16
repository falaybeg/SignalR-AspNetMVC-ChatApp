$(function () {

    var chatHub = $.connection.chatHub;

    registerClientMethods(chatHub);

    $.connection.hub.start().done(function () {

        registerEvents(chatHub);

    });

});


function registerEvents(chatHub) {

    $("#btnSendMsg").click(function () {

        var msg = $("#txtMessage").val();
        if (msg.length > 0) {

            var username = $("#hdUserName").val();
            chatHub.server.sendMessageToAll(username, msg);
            $("#txtMessage").val("").focus();
        }

    });

    $("#txtMessage").keypress(function (e) {
        if (e.which == 13) {
            $("#btnSendMsg").click();
        }
    });
}

function registerClientMethods(chatHub) {

    chatHub.client.onConnected = function (id, username, allUsers, messages) {

        $("#hdId").val(id);
        $("#hdUserName").val(username);
        $("#spanUser").text(username);

        toastr.success(username + " logged in", { timeOut: 1000 });
        toastr.options.showDuration = 10;


        for (i = 0; i < allUsers.length; i++) {
            AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName);
        }

        for (i = 0; i < messages.length; i++) {
            AddMessage(messages[i].UserName, messages[i].Message);
        }
    }

    chatHub.client.onNewUserConnected = function (id, name) {
        AddUser(chatHub, id, name);
        toastr.success(name + " logged in", { timeOut: 1000 });
    }

    chatHub.client.onUserDisconnected = function (id, name) {

        $("#" + id).remove();

        toastr.error(name + " logged off", { timeOut: 1000 });
    }

    chatHub.client.messageReceived = function (username, message) {
        AddMessage(username, message);
    }

    chatHub.client.updatecounter = function (count) {
        $("#onlineMember").text(count);
    }
}

function AddUser(chatHub, id, name) {

    var userId = $("#hdId").val();
    var code = "";

    if (userId === id) {

        code = "<li class='loginUser'>" + name + "</li>";
    }
    else {
        code = "<li id='" + id + "'>" + name + "</li>";
    }

    $("#divusers").append(code);
}


function AddMessage(userName, message) {

    var loginName = $("#hdUserName").val();
    var code = "";

    if (userName === loginName) {
        code = "<li class='message'><span class='loginUserName'>" + userName + "</span>: " + message + "</li>";
    }
    else {
        code = "<li class='message'><span class='userName' style='font-weight: bold'>" + userName + "</span>: " + message + "</li>";
    }

    $("#divChatWindow").append(code);

    var height = $("#divChatWindow")[0].scrollHeight;
    $("#divChatWindow").scrollTop(height);

}

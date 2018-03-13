$(function() {

    var chatHub = $.connection.chatHub;
    $.connection.hub.start();

    chatHub.client.message = function(msg) {

        debugger;
        $("#message").append("<li>" + msg + "</li>");

    }
    $("#send").click(function() {
        var message = $("#txt").val();
        chatHub.server.sendMessages(message);
        //message.val("");
    });

    chatHub.client.updatecounter = function(count) {
        $("#counter").text(count);
    }

    chatHub.client.onConnected = function(id, name, allUser) {

        $("#hdId").val(id);
        $("#hdUserName").val(name)
        $("#userOnline").html(name);

        for (i = 0; i < allUser.length; i++) {

            var userId = $("#hdId").val();
            var code = "";

            if (userId == id) {

                code = $('<div class="loginUser">' + name + "</div>");

            } else {

                code = $('<a id="' + id + '" class="user" >' + name + '<a>');
            }

            $("#divusers").append(code);
        }
    }

    chatHub.client.onNewUserConnected = function(id, name) {

        var userId = $("#hdId").val();
        var code = "";

        if (userId == id) {

            code = $('<div class="loginUser">' + name + "</div>");

        } else {

            code = $('<a id="' + id + '" class="user" >' + name + '<a>');
        }

        $("#divusers").append(code);

    }

    chatHub.client.onUserDisconnected = function (id, username) {

        $('#' + id).remove();

        var disc = $('<div class="disconnect">"' + username + '" logged off.</div>');

        $(disc).hide();
        $("#divusers").prepend(disc);
        $(disc).fadeIn(200).delay(2000).fadeOut(200);

    }
   // chatHub.client.onNewUserConnected = function(id, name) {
   //     $("#user").append("<li>" + name + "</li>");

   // }

});
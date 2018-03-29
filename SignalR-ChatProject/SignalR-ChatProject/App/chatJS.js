$(function () {

    var chatHub = $.connection.chatHub;

    registerClientMethods(chatHub);

    chatHub.client.setTime = function (time) {
        $('#clock').html(time);
    };

    $.connection.hub.start().done(function () {

        setInterval(function () {
            chatHub.server.getTime();
        }, 1000);

        registerEvents(chatHub);

    });

});


function registerEvents(chatHub) {

    $('#btnGetHistory').click(function () {
        $('#chatHistory').empty();

        var loginname = $('#hdUserName').val();
        var dateFrom = $('#dateFrom').val();
        var dateTo = $('#dateTo').val();
        var inputName = $('#inputName').val();

        var code = "";
        $.getJSON('Home/GetHistory?name=' + inputName + '&dtfrom=' + dateFrom + '&dtto=' + dateTo, function (result) {
            $.each(result, function (i, field) {
                code = "";
                if (loginname == field["UserName"]) {
                    code = $('<div class="message">[' + field["sendTime"] + ']  <strong><font color="forestgreen">' + field["UserName"] + ' :</font></strong> ' + field["Text"] + '</div>');
                } else {
                    code = $('<div class="message">[' + field["sendTime"] + ']  <strong>' + field["UserName"] + ' :</strong> ' + field["Text"] + '</div>');
                }
                $('#chatHistory').append(code);
            });
        });
    });


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

        //toastr.success(username + " logged in", { timeOut: 1000 });
        //toastr.options.showDuration = 10;


        for (i = 0; i < allUsers.length; i++) {
            AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName);
        }

        for (i = 0; i < messages.length; i++) {
            AddMessage(messages[i].UserName, messages[i].Message);
        }
    }

    chatHub.client.onNewUserConnected = function (id, name) {
        AddUser(chatHub, id, name);
    }

    chatHub.client.onUserDisconnected = function (id, name) {

        $("#" + id).remove();

        //toastr.error(name + " logged off", { timeOut: 1000 });
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

        code = "<div class='loginUser'><span class='glyphicon glyphicon-user'> " + name + "</span></div>"
    }
    else {

      code = "<div pid='" + id + "' onclick=test('" + id + "','" + name + "') ><span class='glyphicon glyphicon-user' > " + name + "</span></div>";

       
    }

    $("#divusers").append(code);
}

function AddMessage(userName, message) {

    var loginName = $("#hdUserName").val();
    var code = "";

    if (userName === loginName) {
        code = "<ul class='message'><span class='loginUserName'>" + userName + "</span>: " + message + "</ul>";
    }
    else {
        code = "<ul class='message'><span class='userName' style='font-weight: bold'>" + userName + "</span>: " + message + "</ul>";
    }

    $("#divChatWindow").append(code);

    var height = $("#divChatWindow")[0].scrollHeight;
    $("#divChatWindow").scrollTop(height);

}

/*
function test(id, name) {

    $("#userprivate").text("To: "+  name);
    $('#myModal').modal('show');

    $("#privateSent").dblclick(function () {

        var msg = $("#txtMessage").val();

        if (msg.length > 0) {
            chatHub.server.sendPrivateMessage(id, msg);
            $("#privateTxt").val("").focus();
        }

    });

}
*/

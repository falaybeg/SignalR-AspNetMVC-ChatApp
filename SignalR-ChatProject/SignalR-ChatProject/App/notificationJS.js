var nothub = $.connection.notificationHub;

$.connection.hub.start().done(function () {

    $("#btnSend").click(function () {

        var message = $("#txtMessage").val();
        nothub.server.sendNotification(message);
    });
});


//nothub.client.receiveNotification = function (username, message) {

//    var info = "<li>" + username + ":"+ message+"</li>"

//    $("#deneme").append(info);
//    // toastr.info(message,"Yeni bir mesaj var !");
//}


nothub.client.receiveNotification = function (username,message,time) {

    AddNotification(username, message, time);
   
    toastr.info("Yeni bir Bildirim var !");
}


nothub.client.listNotifications = function (notification) {

    for (i = 0; i < notification.length; i++) {
        AddNotification(notification[i].Username, notification[i].Message, notification[i].Time);
        
    }
}


function AddNotification(username, message, time) {

    var info = "<div class='message-item'> <div class='message-inner'> <div class='message-head clearfix'> <div class='avatar pull-left'><a href='./index.php?qa=user&qa_1=Oleg+Kolesnichenko'><img src='https://ssl.gstatic.com/accounts/ui/avatar_2x.png'></a></div> <div class='user-detail'> <h5 class='handle'>" + username + "</h5> <div class='post-meta'><div class='asker-meta'> <span class='qa-message - what'></span><span class='qa-message-when'>  <span class='qa-message-when-data'>" + time + "</span> </span></div></div></div></div><div class='qa-message-content'>" + message + "</div></div></div>";

    $("#notificationPanel").prepend(info);
}


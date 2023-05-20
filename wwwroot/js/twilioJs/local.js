$('#button-join').on('click', function () {
    var roomName = $('#room-name').val();

    if (roomName != '') {
        //createRoom(roomName);
        fetchRoomDetails(roomName);
    } else {
        alert('Please Enter a Room Name');
    }
});

function fetchRoomDetails(roomName) {
    $.getJSON('/token/GetRooms', { name: roomName })
        .done(function (data) {
            debugger
            var res = data.rooms;
            joinRoom(roomName);
        }).fail(function (jqxhr, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
};

function joinRoom(roomName) {
    $.getJSON('/token/GetRooms', { name: roomName })
        .done(function (data) {
            debugger
            var res = data.rooms;
        }).fail(function (jqxhr, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
};

function createRoom(roomName) {
    $.getJSON('/token/CreateRoom', { name: roomName })
        .done(function (data) {
            debugger
            var res = data.rooms;
        }).fail(function (jqxhr, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
};


//$.getJSON('/token/GetRooms', { name: "John" })
//    .done(function (data) {
//        debugger
//        var res = data.rooms;
//    }).fail(function (jqxhr, textStatus, error) {
//        var err = textStatus + ", " + error;
//        console.log("Request Failed: " + err);
//    });








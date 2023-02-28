function getSchedule(id) {
    var nameSchedule = "#schedule" + id;
    $.ajax({
        url: '/Film/getSchedule',
        type: "post",
        data: {
            id: id
        },
        success: function (data) {
            if (data.status == "OK") {
                $(nameSchedule).html(data.data);
            } else if (data.status == "ERROR") {
                $('#resultError').html(data.msg);
            }

            console.log(data);
        },
        error: function (error) {
            console.log(error);
        }
    })
}
function getShowtime(id) {

    var nameShowtime = "#showtime" + id;
    var nameSchedule = "#schedule" + id;
    var nameRoom = "#room" + id;
    var idSchedule = $(nameSchedule).val();
    var idRoom = $(nameRoom).val();
    $.ajax({
        url: '/Film/getShowtime',
        type: "post",
        data: {
            id: idSchedule,
            idRoom: idRoom
        },
        success: function (data) {
            $(nameShowtime).html(data.data);
            console.log(data);
        },
        error: function (error) {
            console.log(error);
        }
    })
}
function getRoom(id) {
    var nameRoom = "#room" + id;
    var nameSchedule = "#schedule" + id;
    var idSchedule = $(nameSchedule).val();
    $.ajax({
        url: '/Film/getRoom',
        type: "post",
        data: {
            id: idSchedule
        },
        success: function (data) {
            $(nameRoom).html(data.data);
            console.log(data);
        },
        error: function (error) {
            console.log(error);
        }
    })
}
function getSeat(id) {
    var nameShowtime = "#showtime" + id;
    var nameSchedule = "#schedule" + id;
    var nameRoom = "#room" + id;
    var nameSeat = "#seat" + id;
    $.ajax({
        url: '/Film/getSeat',
        type: "post",
        data: {
            roomId: $(nameRoom).val(),
            showtimeId: $(nameShowtime).val(),
            filmId: id,
            scheduleId: $(nameSchedule).val()
        },
        success: function (data) {
            $(nameSeat).html(data.data);
            console.log(data);
        },
        error: function (error) {
            console.log(error);
        }
    })
}
var arr = [];
function onChoose(id) {
    var nameIdInput = "in" + id;
    var nameIDinput = "#in" + id;
    var nameIdSeat = "id" + id;
    var valueCheck = document.getElementById(nameIdInput).value;
    var idSeat = document.getElementById(nameIdSeat).value;

    var nameId = "#div" + id;
    if (valueCheck == "false") {
        $(nameId).css('background-color', '#d42304')
        $(nameId).css('color', '#ffff')
        $(nameIDinput).val('true');
        arr.push(idSeat);
    } else {
        arr.splice(arr.indexOf(idSeat), 1);
        $(nameId).css('background-color', 'white')
        $(nameId).css('color', 'black')
        $(nameIDinput).val('false');
    }


}
function bookingTicket(id) {
    var nameShowtime = "#showtime" + id;
    var nameSchedule = "#schedule" + id;
    var nameRoom = "#room" + id;
    var nameSeat = "#seat" + id;
    var nameError = "#resultError" + id;
    var nameModel = "btn-close" + id;
    var idRoom = $(nameRoom).val();
    var idSchedule = $(nameSchedule).val();
    var idShowtime = $(nameShowtime).val();
    if (arr.length == 0 || idRoom == '' || idSchedule == '' || idShowtime == '' || idSchedule == 0 || idShowtime == 0 || idRoom == 0) {
        $(nameError).html("❌ Cần chọn đầy đủ thông tin");
    } else {
        $.ajax({
            url: '/Film/bookingTicket',
            type: "post",
            data: {

                film_id: id,
                schedule_id: idSchedule,
                showtime_id: idShowtime,
                room_id: idRoom,
                seat_id: arr
            },
            success: function (data) {

                var tongtien = 0;        
                var dataHTML = '';
                if (data.status == "ERROR") {
                    $(nameError).html(data.msg);
                } if (data.status == "ERROR_ONE") {
                    $(document).ready(function () {
                        $('#exampleModaltest2').modal('show');
                        $('#result2').html(data.msg);
                    });
                } else {
                    dataHTML += `<p ><span style="font-weight:bold">Tên phim : </span>` + data.data1[0].nameFilm + `</p><br>
										<p ><span style="font-weight:bold">Lịch chiếu : </span>`+ data.data1[0].schedulename+ `</p><br>
										<p ><span style="font-weight:bold">Suất chiếu : </span>`+ data.data1[0].showtimeName + `</p><br>
										<p ><span style="font-weight:bold">Phòng chiếu : </span>`+ data.data1[0].roomName + `</p><br>`
                    dataHTML += `<span style="font-weight:bold">Ghế : `
                    for (var i = 0; i < data.data2.length; i++) {
                        dataHTML += `<span>` + data.data2[i] + " " + `</span>`
                        tongtien = tongtien + 65000;
                    }
                    dataHTML += `<p style="margin-top: 25px" ><span>Tổng tiền : </span>` + tongtien + ` VNĐ</p><br>`
                    
                    $('#result').html(dataHTML);
                    $('#linkqr').html(data.data);
                    $('#sessionId').html(data.data3.Id);
                    $(nameError).html("");
                    document.getElementById(nameModel).click();
                    $(document).ready(function () {
                        $('#exampleModaltest').modal('show');
                    });
                    arr = [];
                }
            },
            error: function (error) {
                console.log(error);
            }
        })
    }

}
function closeModel() {
    $('#exampleModaltest').modal('hide');
}

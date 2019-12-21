function SearchFilterTable() {
    index = -1;
    inp = $('#filterBox').val();
    $("#data:visible tr:not(:has(>th))").each(function () {
        if (~$(this).text().toLowerCase().indexOf(inp.toLowerCase())) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
    $('#Hedding').show();
}


function SetActivationStatus(scroll_id) {
    $.ajax({
        url: '../Scrolls/SetActivationStatus',
        type: 'POST',
        dataType: 'json',
        data: { id: scroll_id},
        success: function (data) {
            if (data.success == false) {

                alert(data.Message);
            }
            else {

                var viewURL = '../Scrolls/index';
                window.location = viewURL;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

$('#department_id').on('change', function () {
    //alert(this.value);
    var departmentId = $('#department_id').val();
    $.ajax({
        url: '/DeviceScrolls/DeviceLoadbyDepartmetId?departmentId=' + departmentId,
        type: "GET",
        dataType: "JSON",
        data: { departmentId: departmentId },
        success: function (result) {
            $('#device_id').empty();
            $('#device_id').append('<option selected="selected" value="">Select One Device</option>')
            $.each(result.deviceList, function (key, device) {
                $('#device_id').append('<option value=' + device.device_id + '>' + device.name + '</option>');
            });

        }
    });
});

$(document).ready(function () {
    modalScrollTimeCreate();

    $('#select_all').on('click', function () {
        if (this.checked) {
            $('.checkbox').each(function () {
                this.checked = true;
            });
        } else {
            $('.checkbox').each(function () {
                this.checked = false;
            });
        }
    });
    $('.checkbox').on('click', function () {
        if ($('.checkbox:checked').length == $('.checkbox').length) {
            $('#select_all').prop('checked', true);
        } else {
            $('#select_all').prop('checked', false);
        }
    });
});
$('#device_id').on('change', function () {
    var deviceId = $('#device_id').val();
    $.ajax({
        url: '/DeviceScrolls/ScrollLoadbyDevice?deviceId=' + deviceId,
        type: "GET",
        dataType: "JSON",
        data: { deviceId: deviceId },
        success: function (data) {
            for (i = 0; i < data.deviceScrollList.length; i++) {
                if (data.deviceScrollList[i].status == 1) {
                    $("#status").prop('checked', true);
                }
                else {
                    $("#status").prop('checked', false);
                }
                        //.append(data.deviceScrollList[i].status);
                }
        }
    });
});
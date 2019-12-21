
$(function () {
    $.support.cors = true;
    // Declare a proxy to reference the hub.
    var notifications = $.connection.notifyDisplay;

    // Create a function that the hub can call to broadcast messages.
    notifications.client.deviceStatusChanged = function (department_id) {

        // Text to Speech if any text available for this department to speech
        if ($('#hid_department_id').val() == department_id) ReLoadDeviceStatus();
    };

    // Start the connection.
    $.connection.hub.start().done(function () {
        //ReLoadDeviceStatus();
    }).fail(function (e) {
        modalAlert(e);
    });

});

function ReLoadDeviceStatus() {
    $.ajax({
        url: "../Departments/GetDeviceCurrentStatus",
        type: 'POST',
        dataType: "json",
        success: function (data) {
            if (data.success == false) {

                modalAlert(data.Message);
            } else {
                $("#tblDeviceStatus").find("tr:gt(0)").remove();
                $.each(data.deviceStatusList, function (i, item) {
                    $("#tblDeviceStatus")
                        .append($('<tr>')
                            .append($('<td>')
                                .append(item.device_no)
                            )
                            .append($('<td>')
                                .append(item.Status)
                            )
                            .append($('<td>')
                                .append(item.user_full_name)
                            )
                            .append($('<td>')
                                .append(item.login_time_formated)
                            )
                            .append($('<td>')
                                .append(item.idle_from_formated)
                            )
                            .append($('<td>')
                                .append(item.token_no_formated)
                            )
                            .append($('<td>')
                                .append(item.call_time_formated)
                            )
                        );
                });

            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

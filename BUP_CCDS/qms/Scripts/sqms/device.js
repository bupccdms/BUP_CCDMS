$('#department_id').on('change', function () {
    //alert(this.value);
    var departmentId = $('#department_id').val();
    $.ajax({
        url: '/DevicePlayList/DeviceLoadbyDepartmetId?departmentId=' + departmentId,
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
function SetActivationStatus(device_scroll_id) {
    $.ajax({
        url: '../DeviceScrolls/SetActivationStatus',
        type: 'POST',
        dataType: 'json',
        data: { id: device_scroll_id },
        success: function (data) {
            if (data.success == false) {
                alert(data.Message);
            }
            else {
                var viewURL = '../DeviceScrolls/index';
                window.location = viewURL;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function getAllMessages() {
    var department_id = $('#department_id').val();
    $.ajax({
        url: '../Devices/GetDisplayInfo?department_id=' + department_id,
        //contentType: 'application/html ; charset:utf-8',
        type: 'GET',
        //dataType: 'html'
    }).success(function (result) {
        if (result.success == "true") {
            $("#divCurrentTokens").empty();
            var row = '<div class="col-sm-6 bg-color-head"><h2 onclick="return toggleFullScreen(document.body);" style="text-align:center">Device</h2></div><div class="col-sm-6 bg-color-head"><h2 style="text-align:center">Token</h2></div>';
            $('#divCurrentTokens').append(row);
            $.each(result.tokenInProgress, function (key, token) {
                row = '<div class="col-sm-6 bg-color"><h1>' + token.device_no + '</h1></div><div class="col-sm-6 bg-color"><h1>' + token.token_no_formated + '</h1></div>';
                $('#divCurrentTokens').append(row);
            });

            $("#nextToken").text('Next Token: ' + result.nextTokens);


        }
        else {
            modalAlert(result.message);
        }
    }).error(function (XMLHttpRequest, textStatus, errorThrown) {
        modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
    });
}



function FilterTable() {
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
};
$(document).ready(function () {
    inp = $("#department_name option:selected").text();
    if (inp == "All Department") {
        $("#department_name").attr('disabled', false);
    }
    else $("#department_name").attr('disabled', true);

    FilterTable2();
    $("#department_name").change(function () {
        // var selectedDepartment = $("#department_name option:selected").text();
        FilterTable2();

    })
});

function FilterTable2() {
    index = -1;
    inp = $("#department_name option:selected").text();
    if (inp == "All Department") {
        inp = "";
    }
    $("#data:visible tr:not(:has(>th))").each(function () {
        if (~$(this).text().toLowerCase().indexOf(inp.toLowerCase())) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
    $('#Hedding').show();
};
$(document).ready(function () {
            inp = $("#department_id option:selected").text();
            if (inp == "Select One Department") {
                $("#department_id").attr('disabled', false);
            }
            else $("#department_id").attr('disabled', true);
});

$(document).ready(function () {
    modalPlayListCreate();
});


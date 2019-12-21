$(document).ready(function () {
    var selectedDepartment = $("#department_name option:selected").text();
    var selectedVal = $("#department_name option:selected").val();
    $('#device_no').empty();
    $.ajax({
        url: "../../Home/GetDeviceByDepartmentId",
        type: "GET",
        dataType: "json",
        data: { departmentId: selectedVal },
        success: function (data) {
            // alert(response);
            // debugger;
            //$.each(data, function (key, value) {
            //    $('#device_no').append($("<option></option>").attr("value", device_id).text(device_no));
            //});
            if (data.data.length > 0) {
                $('#device_no').append($("<option value=''>Select a Device</option>"));

                for (var i = 0; i < data.data.length; i++) {
                    $('#device_no').append($("<option></option>").attr("value", data.data[i].device_id).text(data.data[i].device_no));
                }
            } else {
                $('#device_no').append($("<option value=''>No Device Found!!!</option>"));
            }
        },
        error: function (response) {
            alert(response);
        }
    });
    $("#department_name").attr('disabled', true);
});
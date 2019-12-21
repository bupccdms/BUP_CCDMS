$(document).ready(function () {
    

    InitiateReportBox();
    $('#department_id option:eq(0)').val(0);

    $('#txtDevice').append($("<option value='0'>All Devices</option>"));
    //$('#txtDevice').attr('disabled', disabled);
    $('#txtUser').append($("<option value=''>All Users</option>"));

    isOnlyServiceHolder = true;
    if ($("#is_success").length) isOnlyServiceHolder = false;

    $("#department_id").change(function () {
        // debugger;
        LoadDevicesUsers(isOnlyServiceHolder);

    });



    

    inp = $("#department_id option:selected").text();
    if ($("#department_id option:selected").val() > 0) {
        LoadDevicesUsers(isOnlyServiceHolder);
        $("#department_id").attr('disabled', true);
    }
    else $("#department_id").attr('disabled', false);
    
});


function LoadDevicesUsers(isOnlyServiceHolder) {
    var selectedDepartment = $("#department_id option:selected").text();
    var selectedVal = $("#department_id option:selected").val();
    $('#txtUser').empty();
    $('#txtDevice').empty();
    if (selectedVal > 0) {
        $.ajax({
            url: "../Home/GetUserAndDeviceByDepartmentId",
            type: "GET",
            dataType: "json",
            data: { departmentId: selectedVal, isOnlyServiceHolder: isOnlyServiceHolder },
            success: function (data) {
                if (data.userList.length > 0) {
                    $('#txtUser').append($("<option value=''>All Users</option>"));

                    for (var i = 0; i < data.userList.length; i++) {
                        $('#txtUser').append($("<option></option>").attr("value", data.userList[i].user_id).text(data.userList[i].user_name));
                    }
                } else {
                    $('#txtUser').append($("<option value=''>All Users</option>"));
                }
                if (data.deviceList.length > 0) {
                    $('#txtDevice').append($("<option value='0' selected>All Devices</option>"));

                    for (var i = 0; i < data.deviceList.length; i++) {
                        $('#txtDevice').append($("<option></option>").attr("value", data.deviceList[i].device_id).text(data.deviceList[i].device_no));
                    }
                } else {
                    $('#txtDevice').append($("<option value='0' selected>All Devices</option>"));
                }
            },
            error: function (response) {
                alert(response);
            }
        });
    }
    else {
        $('#txtUser').append($("<option value=''>All Users</option>"));
        $('#txtDevice').append($("<option value='0' selected>All Devices</option>"));
    }
}


function ShowPannel(pannel_index) {
    if (pannel_index == 1) {
        $("#collapse1").collapse('show');
        $("#collapse2").collapse('hide');
    }
    else {
        $("#collapse1").collapse('hide');
        $("#collapse2").collapse('show');
    }
}

function GenerateLocalCustomerList() {

    $('#tablebody').empty();

    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var department_id = $('#department_id').val();
    var device_id = $('#txtDevice').val();
    var user_id = $('#txtUser').val();
    var customer_type_id = $('#customer_type_id').val();
    //var token = $('#txtToken').val();
    var service_sub_type_id = $('#service_sub_type_id').val();

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

   
    $.ajax({
        url: "../Report/GetLocalCustomerInformation",
        type: 'POST',
        dataType: "json",
        data: {
            department_id: department_id,
            user_id: user_id,
            device_id: device_id,
            customer_type_id: customer_type_id,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            //debugger;

            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].department_name + '</td><td>'
                        + response.Message[i].service_datetime + '</td><td>'
                        + response.Message[i].service_sub_type_name + '</td><td>'
                        + response.Message[i].end_time + '</td><td>'
                        + response.Message[i].contact_no + '</td><td>'
                        + response.Message[i].customer_name + '</td><td>'
                        + response.Message[i].im_msisdn + '</td><td>'
                        + response.Message[i].im_name + '</td><td>'
                        + response.Message[i].remarks + '</td><td>'
                        + response.Message[i].further_followUp_needed + '</td><td>'
                        + response.Message[i].FollowUp_date + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }

        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });

}

function GenerateVisitedCustomerList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var department_id = $('#department_id').val();
    var device_id = $('#txtDevice').val();
    var user_id = $('#txtUser').val();
   
    if ($('#service_sub_type_id').val())
        var service_sub_type_id = $('#service_sub_type_id').val();
    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetVisitedCustomerInformation",
        type: 'POST',
        dataType: "json",
        data: {
            department_id: department_id,
            user_id: user_id,
            device_id: device_id,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].department_name + '</td><td>'
                        + response.Message[i].service_sub_type_name + '</td><td>'
                        + response.Message[i].total_served_token + '</td><td>'
                        + response.Message[i].single_visit_customer + '</td><td>'
                        + response.Message[i].multiple_visit_customer + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateAgentWiseReportList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var department_id = $('#department_id').val();
    var device_id = $('#txtDevice').val();
    var user_id = $('#txtUser').val();
    if ($('#service_sub_type_id').val())
        var service_sub_type_id = $('#service_sub_type_id').val();
    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetAgentWiseReportInformation",
        type: 'POST',
        dataType: "json",
        data: {
            department_id: department_id,
            user_id: user_id,
            device_id: device_id,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].department_name + '</td><td>'
                        + response.Message[i].user_name + '</td><td>'
                        + response.Message[i].handled_customer + '</td><td>'
                        + response.Message[i].average_waiting_time + '</td><td>'
                        + response.Message[i].average_service_time + '</td><td>'
                        + response.Message[i].average_TAT + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateServiceSummaryList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var department_id = $('#department_id').val();
    var device_id = $('#txtDevice').val();
    var user_id = $('#txtUser').val();
    if ($('#service_sub_type_id').val())
        var service_sub_type_id = $('#service_sub_type_id').val();
    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetServiceSummaryInformation",
        type: 'POST',
        dataType: "json",
        data: {
            department_id: department_id,
            user_id: user_id,
            device_id: device_id,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            if (response.Success) {
                var select = $('#tablebody');
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].department_name + '</td><td>'
                        + response.Message[i].service_sub_type_name + '</td><td>'
                        + response.Message[i].token_served + '</td><td>'
                        + response.Message[i].total_percentage + '</td><td>'
                        + response.Message[i].standard_time + '</td><td>'
                        + response.Message[i].actual_time + '</td><td>'
                        + response.Message[i].variance + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateGeneralSearchList() {
    $('#tablebody').empty();
     var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var department_id = $('#department_id').val();
    var device_id = $('#txtDevice').val();
    var user_id = $('#txtUser').val();
    var msisdn_no = $('#txtMSISDN').val();
    var service_sub_type_id = $('#service_sub_type_id').val();
    var token_no = $('#txtToken').val();

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetGeneralSearchInformation",
        type: 'POST',
        dataType: "json",
        data: {
            department_id: department_id,
            user_id: user_id,
            device_id: device_id,
            msisdn_no: msisdn_no,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate,
            token_no : token_no
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {

                    select.append($('<tr><td>' + response.Message[i].department_name + '</td><td>'
                        + response.Message[i].Service_date + '</td><td>'
                        + response.Message[i].user_name + '</td><td>'
                        + response.Message[i].token_no_formated + '</td><td>'
                        + response.Message[i].mobile_no + '</td><td>'
                        + response.Message[i].service_sub_type_name + '</td><td>'
                        + response.Message[i].issue_time + '</td><td>'
                        + response.Message[i].start_time + '</td><td>'
                        + response.Message[i].end_time + '</td><td>'
                        + response.Message[i].wating_time + '</td><td>'
                        + response.Message[i].std_time + '</td><td>'
                        + response.Message[i].actual_time + '</td><td>'
                        + response.Message[i].variance + '</td><td>'
                        + response.Message[i].remarks + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
            
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateBreakReportList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var department_id = $('#department_id').val();
    var device_id = $('#txtDevice').val();
    var user_id = $('#txtUser').val();
    var customer_type_id = 0;
    //if ($('#service_sub_type_id').val())
    var break_type_id = $('#break_type_id').val();
    //var token_no = $('#txtToken').val();

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetBreakReportInformation",
        type: 'POST',
        dataType: "json",
        data: {
            department_id: department_id,
            user_id: user_id,
            device_id: device_id,
            break_type_id: break_type_id,
            customer_type_id: customer_type_id,
            start_date: fromDate,
            end_date: toDate,
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].department_name + '</td><td>'
                        + response.Message[i].create_time + '</td><td style="text-align: left;">'
                        + response.Message[i].username + '</td><td>'
                        + response.Message[i].device_no + '</td><td>'
                        + response.Message[i].break_type_name + '</td><td>'
                        + response.Message[i].start_time + '</td><td>'
                        + response.Message[i].end_time + '</td><td>'
                        + response.Message[i].duration + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateNServiceList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var department_id = $('#department_id').val();
    var device_id = $('#txtDevice').val();
    var user_id = $('#txtUser').val();

    var topn = $('#txtTopN').val();

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetTopNServiceInformation",
        type: 'POST',
        dataType: "json",
        data: {
            department_id: department_id,
            user_id: user_id,
            device_id: device_id,
            start_date: fromDate,
            end_date: toDate,
            topn: topn,
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td>' + response.Message[i].department_name + '</td><td style="text-align: left;">'
                        + response.Message[i].service_name + '</td><td>'
                        + response.Message[i].total_service + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function GenerateTokenExceedingList() {
    $('#tablebody').empty();
    var flag = $('#txtFlag').val();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var department_id = $('#department_id').val();
    var device_id = $('#txtDevice').val();
    var user_id = $('#txtUser').val();
    var service_sub_type_id = $('#service_sub_type_id').val();

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetTokenExceedingInformation",
        type: 'POST',
        dataType: "json",
        data: {
            department_id: department_id,
            user_id: user_id,
            device_id: device_id,
            service_sub_type_id: service_sub_type_id,
            start_date: fromDate,
            end_date: toDate,
            flag : flag
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td style="text-align: left;">' + response.Message[i].department_name + '</td><td style="text-align: left;">'
                        + response.Message[i].user_name + '</td><td>'
                        + response.Message[i].total_served_token + '</td><td>'
                        + response.Message[i].total_exceedig_token + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}



function GenerateLogoutDetailList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var department_id = $('#department_id').val();
    var device_id = $('#txtDevice').val();
    var user_id = $('#txtUser').val();
    

    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetLogoutDetailInformation",
        type: 'POST',
        dataType: "json",
        data: {
            department_id: department_id,
            user_id: user_id,
            device_id: device_id,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td style="text-align: left;">' + response.Message[i].department_name + '</td><td style="text-align: left;">'
                        + response.Message[i].user_name + '</td><td>'
                        + response.Message[i].service_date_formated + '</td><td>'
                        + response.Message[i].device_no + '</td><td>'
                        + response.Message[i].login_time_formated + '</td><td>'
                        + response.Message[i].logout_time_formated + '</td><td>'
                        + response.Message[i].duration + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}


function GenerateLoginAttemptDetailsList() {
    $('#tablebody').empty();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var department_id = $('#department_id').val();
    var device_id = $('#txtDevice').val();
    var is_success = $('#is_success').val();
    var user_id = $('#txtUser').val();


    if ((fromDate == "" || toDate == "")) {
        alert("Pl's Fill To Date & From Date");
        return false;
    }
    else {
        ShowPannel(2);
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    $.ajax({
        url: "../Report/GetLoginAttemptDetailsInformation",
        type: 'POST',
        dataType: "json",
        data: {
            department_id: department_id,
            user_id: user_id,
            device_id: device_id,
            is_success: is_success,
            start_date: fromDate,
            end_date: toDate
        },
        success: function (response) {
            var select = $('#tablebody');
            if (response.Success) {
                for (var i = 0; i < response.Message.length; i++) {
                    select.append($('<tr><td style="text-align: left;">' + response.Message[i].department_name + '</td><td style="text-align: left;">'
                        + response.Message[i].UserName + '</td><td>'
                        + response.Message[i].FullName + '</td><td>'
                        + response.Message[i].RoleName + '</td><td>'
                        + response.Message[i].attempt_time_formatted + '</td><td>'
                        + response.Message[i].device_no + '</td><td>'
                        + response.Message[i].ip_address + '</td><td>'
                        + response.Message[i].machine_name + '</td><td>'
                        + response.Message[i].status + '</td></tr>'));
                }
            }
            else {
                modalAlert(response.Message);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}
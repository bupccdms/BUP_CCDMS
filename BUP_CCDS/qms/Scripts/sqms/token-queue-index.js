
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
}

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

    });


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
function GetList() {

    var date = $('#txtAsOnDate').val();

    if (date == "") {
        ShowModalMessage("Please select a date");
        return;
    }

    var URL = '../TokenQueues/GetList/?date=' + date;
    $.get(URL, function (data) {

        $("#data").html('');
        $("#data").html(data);
    });
};

function sms(mobileNo, tokenNo) {


    //var con = JSON.stringify(contactNo);
    $.ajax({
        url: '../TokenQueues/SMSSend',
        type: 'POST',
        dataType: 'json',
        data: { mobileNo: mobileNo, tokenNo: tokenNo },
        
            success: function (data) {
                if (data.Success == true)
                {
                    modalAlert("SMS sent successfully.");
                }
                else
                    modalAlert(data.Message);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
};

function printDiv(divName) {
    var printContents = document.getElementById(divName).innerHTML;
    var originalContents = document.body.innerHTML;

    document.body.innerHTML = printContents;

    window.print();

    document.body.innerHTML = originalContents;
}

function printToken(id) {
    //var contactNo = $("#save_btn").val();
    var tokenNo = id//$("#hidtokenNo").val();
    //var con = JSON.stringify(contactNo);
    $.ajax({
        url: '../TokenQueues/Print',
        type: 'POST',
        dataType: 'json',
        data: { tokenNo: tokenNo },
        success: function (data) {
            $("#date").text(data.Date);
            $("#printTokenId").text(data.Message);
            printDiv('printableArea');
            //  $("#service_name").empty();
        }
    });
};



function TokenReInitiate(token_id) {
    $.ajax({
        url: '../TokenQueues/ReInitiate',
        type: 'POST',
        dataType: 'json',
        data: { token_id: token_id },
        success: function (data) {
            if (data.Success == true) {
                window.location = '../TokenQueues/Index?department_name=""&device_no=""';
            }
            else {
                modalAlert(data.Message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}


var selectedTokenId = 0;
function TokenAssignToDevice(token_id) {
    selectedTokenId = token_id;
    modalPrompt("Please enter device no where you transfer this token:", TokenAssignToDeviceCall);
    
}
function TokenAssignToDeviceCall(device_no) {
    if (device_no == null || device_no == "") {
        modalAlert("Please input a device no for transfer this service");
        return;
    } 
    if (selectedTokenId == 0) {
        modalAlert("Token not selected");
        return;
    } 

    $.ajax({
        url: '../TokenQueues/AssignToDevice',
        type: 'POST',
        dataType: 'json',
        data: { token_id: selectedTokenId, device_no:device_no },
        success: function (data) {
            if (data.Success == true) {
                window.location = '../TokenQueues/Index?department_name=""&device_no=""';
            }
            else {
                modalAlert(data.Message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}
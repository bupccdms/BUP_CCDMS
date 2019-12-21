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


///--
$(document).ready(function () {
    inp = $("#role_name option:selected").text();
    if (inp == "All Role") {
        $("#role_name").attr('disabled', false);
    }
    else $("#role_name").attr('disabled', true);

    FilterTable3();
    $("#role_name").change(function () {
        
        FilterTable3();

    });
});

function FilterTable3() {
    index = -1;
    inp = $("#role_name option:selected").text();
    if (inp == "All Role") {
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


function SetActivationStatus(user_id, is_activate) {
    $.ajax({
        url: '../DepartmentUsers/SetActivationStatus',
        type: 'POST',
        dataType: 'json',
        data: { user_id: user_id, is_activate: is_activate },
        success: function (data) {
            if (data.success == false) {

                alert(data.Message);
            }
            else {
               
                var viewURL = '../DepartmentUsers/index';
                window.location = viewURL;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}
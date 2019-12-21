

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

    var URL = '../ServiceDetails/GetList/?date=' + date;
    $.get(URL, function (data) {

        $("#dvList").html('');
        $("#dvList").html(data);
    });
}
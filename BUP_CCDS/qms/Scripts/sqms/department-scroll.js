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



function SetActivationStatus(department_scroll_id) {
    $.ajax({
        url: '../DepartmentScrolls/SetActivationStatus',
        type: 'POST',
        dataType: 'json',
        data: { id: department_scroll_id },
        success: function (data) {
            if (data.success == false) {

                alert(data.Message);
            }
            else {

                var viewURL = '../DepartmentScrolls/index';
                window.location = viewURL;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}
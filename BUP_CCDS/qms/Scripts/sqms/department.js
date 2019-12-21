var dialogBox, dialogBody, inputBox, reportDialog, playListDialog, deptplayListTimeDialog;

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
function SetActivationStatus(scroll_id, is_activate) {
    $.ajax({
        url: '../Scrolls/SetActivationStatus',
        type: 'POST',
        dataType: 'json',
        data: { scroll_id: scroll_id, is_activate: is_activate },
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

$(document).ready(function () {
    modalDepartmentPlayListTime();
    modalPlayListTimeCreate();
    //playListItemValidation();
});


function playListItemValidation() {
    var file = $('#file_name').val();
    //$('#file_type').val("content");
    var extension = file.split(".").pop();
    if (extension == "mp4" || extension == "MP4") {
        $("#duration_in_second").attr('readOnly', true);
        $("#start_time").attr('readOnly', false);
        $("#end_time").attr('readOnly', false);
    }
    else {
        $("#duration_in_second").attr('readOnly', false);
        $("#start_time").attr('readOnly', true);
        $("#end_time").attr('readOnly', true);
    }

}

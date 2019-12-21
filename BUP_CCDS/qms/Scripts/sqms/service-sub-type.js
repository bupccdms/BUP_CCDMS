
function SetStatus(service_sub_type_id, is_activate) {
    $.ajax({
        url: '../ServiceSubTypes/SetStatus',
        type: 'POST',
        dataType: 'json',
        data: { service_sub_type_id: service_sub_type_id, is_activate: is_activate },
        success: function (data) {
            if (data.success == false) {

                alert(data.Message);
            }
            else {

                var viewURL = '../ServiceSubTypes/index';
                window.location = viewURL;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}
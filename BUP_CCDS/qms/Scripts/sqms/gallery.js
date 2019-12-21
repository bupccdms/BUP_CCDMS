

function LoadMedia() {
    var direcorypath = $('#txtDirectory').val();
    var viewURL = "";
    if (direcorypath.length > 0)
        var viewURL = '../Gallery/index?directory=' + direcorypath;
    else
        var viewURL = '../Gallery/index?directory=';
    window.location = viewURL;
}


function DeleteMedia(file_full_path) {
    if (file_full_path.length > 0)
        modalConfirm("Are you sure to delete this file parmanently?", function () {
            $.ajax({
                url: '../Gallery/Delete',
                type: 'POST',
                dataType: "json",
                data: { file_full_path: file_full_path },
            }).success(function (result) {
                if (result.success == "false") {
                    modalAlert(result.message);
                }
                else {
                    LoadMedia();
                }
            }).error(function (XMLHttpRequest, textStatus, errorThrown) {
                modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            });

        }, null);
}


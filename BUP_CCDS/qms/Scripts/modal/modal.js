var dialogBox, dialogBody, inputBox, serviceDialog, breakDialog, deptplayListTimeDialog, playListDialog, scrollTimeDialog, playListTimeDialog, missingListDialog, historyDialog, reportDialog, dashboardDialog, galleryDialog,contentDialog;

$(document).ready(function () {
    dialogBox = $("#dialog-message");
    dialogBody = dialogBox.find("#body");
    
    inputBox = '<br/><input type="text" id="inputBox" placeholder="XXXXXXX" />'

    
})

function modalAlert(msg, callBack){
    dialogBody.html(msg);
    dialogBox.dialog({
        resizable: false,
        modal: true,
        closeOnEscape: true,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
                if (callBack != null) {
                    callBack();
                }
            }
        }
    });
}

function modalHistoryCreate() {
    historyDialog=
        $("#div-history").dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        height: 500,
        width: 700,
        closeOnEscape: true,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        }
    });
}

function modalConfirm(msg, callback, cancelCallBack) {
    dialogBody.html(msg);
    $("#dialog-message").dialog({
        autoOpen: true,
        resizable: false,
        modal: true,
        width: "auto",
        closeOnEscape: false,
        buttons: {
            "Yes": function () {
                $(this).dialog("close");
                callback();
            },
            "No": function () {
                $(this).dialog("close");
                if (cancelCallBack != null) {
                    cancelCallBack();
                }
            }
        }
    });
}

function modalPrompt(msg, callback) {

    dialogBody.html(msg + inputBox);
    
    $("#dialog-message").dialog({
        autoOpen: true,
        resizable: false,
        modal: true,
        height: "auto",
        width: "auto",
        closeOnEscape: false,
        buttons: {
            "Ok": function () {
                var value = dialogBody.find("#inputBox").val();
                if (value == null || value == "") {
                    //modalAlert("Please input a value then press Ok or press Cancel");
                    return;
                }
                
                $(this).dialog("close");
                callback(value);
            },
            "Cancel": function () {
                $(this).dialog("close");
                //callback("close");
            }
        }
    });
}

function modalServiceTypeCreate(callback) {

     serviceDialog=

     $("#div-services").dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        height: 400,
        width: 800,
        closeOnEscape: false,
        buttons: {
            "Ok": function () {
                var value = $("input[name=radio-service]:checked").val();
                var text = $("input[name=radio-service]:checked").next('label').text();
                var max_duration = $("input[name=radio-service]:checked").next('label').attr('max_duration');
                
                if (value == null || value == "") {
                    modalAlert("Please select a service then press Ok or press Cancel");
                    return;
                }

                $(this).dialog("close");
                callback(value, text, max_duration);
                
            },
            "Cancel": function () {
                $(this).dialog("close");
                //callback("close");
            }
        }
        });

    
}

function modalBreakCreate(callback) {
    
    breakDialog =

        $("#dialog-url-break").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Take a break',
            height: 'auto',
            width: 650,
            closeOnEscape: false,
            buttons: {
                "Ok": function () {
                    var break_type_id = $("#dialog-url-break").find("#break_type_id").val();
                    var remarks = $("#dialog-url-break").find("#remarks").val();
                    callback(break_type_id, remarks);
                    $(this).dialog("close");
                },
                "Cancel": function () {
                    $(this).dialog("close");
                    //callback("close");
                }
            }
        });
}

function loadBreakDialog() {
    if ($("#is_break").val() == 1) {
        modalAlert("You already defined a break, please complete the break first.");
        return;
    }
    breakDialog.load("../DailyBreaks/Create", function () {
        breakDialog.dialog('open');
    });

}

function modalDashboardDialogCreate() {

    dashboardDialog =

        $("#dialog-url-dashboard").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Service List',
            height: 'auto',
            width: 850,
            closeOnEscape: true,
            buttons: {
                "Ok": function () {
                    $(this).dialog("close");
                }
            }
        });
}

function loadDashboardDepartmentListDialog() {
    
    dashboardDialog.load("../ServiceDetails/ServiceList", function () {
        dashboardDialog.dialog('option', 'title', 'Service List');
        dashboardDialog.dialog('open');
    });

}

function loadDashboardDeviceStatusDialog() {

    dashboardDialog.load("../ServiceDetails/DeviceServiceList", function () {
        dashboardDialog.dialog('option', 'title', 'Device Status');
        dashboardDialog.dialog('open');
    });

}

function loadDashboardUserStatusDialog() {

    dashboardDialog.load("../ServiceDetails/UserServiceList", function () {
        dashboardDialog.dialog('option', 'title', 'User Status');
        dashboardDialog.dialog('open');
    });

}

function modalMissingListCreate(callBack) {
    missingListDialog =

        $("#dialog-url-skipped").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Customer Missing List',
            height: 600,
            width: 950,
            closeOnEscape: true,
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                    if (callBack != null) {
                        callBack();
                    }
                }
            }
        });
}

function loadMissingListDialog() {
    missingListDialog.load("../TokenQueues/Skipped", function () {
        missingListDialog.dialog('open');
        SelectDepartment();
    });

}

function modalGalleryCreate(callBack) {
    galleryDialog =

        $("#dialog-url-gallery").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Media Gallery',
            height: 600,
        width: 950,
        closeOnEscape: false,
            buttons: {
                Ok: function () {
                    var value = $("input[name=radio-gallery]:checked").val();
                    $(this).dialog("close");
                    if (callBack != null) {
                        callBack(value);
                    }
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
}

function loadGalleryDialog(directory,fileName) {
    this.event.preventDefault();
    galleryDialog.load("../Gallery/List?directory=" + directory, function () {
        $("#dialog-url-gallery").dialog({
            autoOpen: true,
            open: function () {
                var cnt = $("input[name=radio-gallery]").length;
                var i;
                for (i = 0; i < cnt; i++) {
                    var xx = $("input[name=radio-gallery]:eq(" + i + ")").val();
                    if (xx == fileName) {
                        $("input[name=radio-gallery]:eq(" + i + ")").prop("checked", true);
                    }
                }
            },
        })
        
    });
}

function modalContentCreate(callBack) {
    contentDialog =
        $("#dialog-url-content").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Content Gallery',
            height: 600,
            width: 950,
            closeOnEscape: false,
            buttons: {
                Ok: function () {
                    var value = $("input[name=radio-content]:checked").val();
                    $(this).dialog("close");
                    if (callBack != null) {
                        callBack(value);
                    }
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
}

function loadContentDialog(directory,fileName) {
    this.event.preventDefault();
    contentDialog.load("../CustomContent/List", function () {
        //contentDialog.dialog('open');
        $("#dialog-url-content").dialog({
            autoOpen: true,
            open: function () {
                var cnt = $("input[name=radio-content]").length;
                var i;
                for (i = 0; i < cnt; i++) {
                    var xx = $("input[name=radio-content]:eq(" + i + ")").val();
                    if (xx == fileName) {
                        $("input[name=radio-content]:eq(" + i + ")").prop("checked", true);
                    }
                }
            },
        })
    });
}

function InitiateReportBox() {
    reportDialog =
        $("#dialog-report").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: "Report Viewer",
            height: 600,
            width: 950,
            closeOnEscape: true,
            buttons: {
                Close: function () {
                    $(this).dialog("close");
                }
            }
        });
}

function modalPlayListCreate(callBack) {
    playListDialog =

        $("#dialog-url-playList").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Play List Item',
            height: 600,
            width: 950,
            closeOnEscape: false,
            buttons: {
                Ok: function () {
                    var value = $("input[name=radio-playList]:checked").val();
                    $(this).dialog("close");
                    if (callBack != null) {
                        callBack(value);
                    }
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
}

function loadPlayListDialog(playList_id) {
    this.event.preventDefault();
    playListDialog.load("../PlayListItems/List?playlist_id=" + playList_id, function () {
        playListDialog.dialog('open');
    });
}

function loadPlaylistTimeDialog(playList_id) {
    this.event.preventDefault();
    playListTimeDialog.load("../PlayListSheduling/PlayListTime?playlist_id=" + playList_id, function () {
        playListTimeDialog.dialog('open');
    });
}

function modalPlayListTimeCreate(callBack) {
    playListTimeDialog =

        $("#dialog-playList").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Set Play List Time',
            height: 600,
            width: 950,
            closeOnEscape: false,
            buttons: {
                Ok: function () {
                    var value = $("input[name=radio-playList]:checked").val();
                    $(this).dialog("close");
                    if (callBack != null) {
                        callBack(value);
                    }
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
        },
        open: function () {
            $('#ContentCheckbox').prop('checked', true);
            $('#content').show();
            $('#url').hide();
        }

        });
}

function loadScrollTimeDialog(scroll_id) {
    this.event.preventDefault();
    scrollTimeDialog.load("../ScrollSheduling/SetScrollTime?scroll_id=" + scroll_id, function () {
        scrollTimeDialog.dialog('open');
    });
}

function modalScrollTimeCreate(callBack) {
    scrollTimeDialog =

        $("#dialog-scroll").dialog({
            autoOpen: false,
            resizable: false,
            modal: true,
            title: 'Set Scroll Time',
            height: 600,
            width: 950,
            closeOnEscape: false,
            buttons: {
                Ok: function () {
                    var value = $("input[name=radio-scroll]:checked").val();
                    $(this).dialog("close");
                    if (callBack != null) {
                        callBack(value);
                    }
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            },
            open: function () {
                $('#ScrollTimeCheckbox').prop('checked', true);
                $('#scrollTime').show();
                $('#scroll').hide();
            }

        });
}

function checkval(idvalue) {

    if (idvalue == "URL")
    {
        $('#ContentCheckbox').prop('checked', false);
        $('#is_url').prop('checked', true);
        $('#content').hide();
        $('#content').val("");
        $('#url').show();
    }
    else if (idvalue == "Content") {
        $('#ContentCheckbox').prop('checked', true);
        $('#is_url').prop('checked', false);
        $('#content').show();
        $('#url').hide();
    }
}

function checkScrollVal(idvalue) {

    if (idvalue == "Scroll") {
        $('#ScrollTimeCheckbox').prop('checked', false);
        $('#is_scroll').prop('checked', true);
        $('#scrollTime').hide();
        $('#scrollTime').val("");
        $('#scroll').show();
    }
    else if (idvalue == "ScrollTime") {
        $('#ScrollTimeCheckbox').prop('checked', true);
        $('#is_scroll').prop('checked', false);
        $('#scrollTime').show();
        $('#scroll').hide();
    }
}

function savePlaylist() {
    if (ContentCheckbox.checked) {
        var PlayListTimeValue = $('#content').val();
        if (PlayListTimeValue == "") {
            alert("Please select Playlist Time");
            return;
        }
        var PlayListTime = parseInt($('#content').val());
        var today = new Date();
        var date = (today.getMonth() + 1) + '/' + today.getDate() + '/' + today.getFullYear();
        //var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
        today.setHours(today.getHours());
        var isPM = today.getHours() >= 12;
        var isMidday = today.getHours() == 12;
        var time = [today.getHours() - (isPM && !isMidday ? 12 : 0), today.getMinutes(), today.getSeconds() || '00'].join(':') + (isPM ? ' PM' : ' AM');
        var currentDate = date + ' ' + time;
        var PlayList_id = parseInt($('#playList_id').val());
        var is_active = 1;

        var myObject = {
            PlayList_id: PlayList_id,
            when_start: currentDate,
            duration: PlayListTime,
            bool_is_active: is_active
        }

        $.ajax({
            url: '../PlayListSheduling/CreatePlayList',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ playListSheduling: myObject }),
            success: function (data) {
                if (data.success == true) {
                    var viewURL = '../PlayListSheduling/index';
                    window.location = viewURL;
                }
                else {
                    alert(data.success);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            }
        });
    }

    else {
        var PlayList_id = parseInt($('#playList_id').val());
        $.ajax({
            url: '../PlayLists/PublishToAllPlayList',
            type: 'POST',
            //dataType: "json",
            //data: { id: PlayList_id},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ id: PlayList_id}),
            success: function (data) {
                if (data.success == true) {
                    var viewURL = '../PlayLists/index';
                    window.location = viewURL;
                }
                else {
                    alert(data.success);
                    //var viewURL = '../DepartmentUsers/index';
                    //window.location = viewURL;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            }
        });
    }
}

function publishWithScrollTime() {
    if (ScrollTimeCheckbox.checked) {
        var ScrollTimeValue = $('#scrollTime').val();
        if (ScrollTimeValue == "") {
            alert("Please select Scroll Time");
            return;
        }
        var ScrollTime = parseInt($('#scrollTime').val());
        var today = new Date();
        var date = (today.getMonth() + 1) + '/' + today.getDate() + '/' + today.getFullYear();
        //var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
        today.setHours(today.getHours());
        var isPM = today.getHours() >= 12;
        var isMidday = today.getHours() == 12;
        var time = [today.getHours() - (isPM && !isMidday ? 12 : 0), today.getMinutes(), today.getSeconds() || '00'].join(':') + (isPM ? ' PM' : ' AM');
        var currentDate = date + ' ' + time;
        var Scroll_id = parseInt($('#scroll_id').val());
        var is_active = 1;

        var myObject = {
            scroll_id: Scroll_id,
            when_start: currentDate,
            duration: ScrollTime,
            bool_is_active: is_active
        }

        $.ajax({
            url: '../ScrollSheduling/CreateScroll',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ scrollSheduling: myObject }),
            success: function (data) {
                if (data.success == true) {
                    var viewURL = '../ScrollSheduling/index';
                    window.location = viewURL;
                }
                else {
                    alert(data.success);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            }
        });
    }

    else {
        var BtnName = "ScrollON";
        //var BtnName = $("ScrollON").val();
        //var BtnName = $(this).val("ScrollON");
        var Scroll_id = parseInt($('#scroll_id').val());
        $.ajax({
            url: '../Scrolls/PublishToAllScroll',
            type: 'POST',
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: { id: Scroll_id, btnName: BtnName },
            success: function (data) {
                if (data.success == true) {
                    var viewURL = '../Scrolls/index';
                    window.location = viewURL;
                }
                else {
                    alert(data.success);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            }
        });
    }
}

function modalDepartmentPlayListTime(callBack) {
    deptplayListTimeDialog =
        $("#dialog-playList-department").dialog({
            autoOpen: false,
            autoOpen: false,
            resizable: false,
            modal: true,
        title: 'Set Department Play List Time',
            height: 600,
            width: 950,
            closeOnEscape: false,
            buttons: {
                Ok: function () {
                    var value = $("input[name=radio-playList]:checked").val();
                    $(this).dialog("close");
                    if (callBack != null) {
                        callBack(value);
                    }
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            },
            open: function () {
                $('#ContentCheckbox').prop('checked', true);
                $('#content').show();
                $('#url').hide();
            }

        });
}

function loadDeptPlaylistTimeDialog(department_playlist_id, department_id, playList_id) {
    this.event.preventDefault();
    deptplayListTimeDialog.load("../DepartmentPlayListShedule/DeptPlayListTime?department_playlist_id=" + department_playlist_id + '&department_id=' + department_id + '&playList_id=' + playList_id, function () {
        deptplayListTimeDialog.dialog('open');
    });
}

function SaveDepartmentPlaylistTime() {
    if (ContentCheckbox.checked) {
        var PlayListTimeValue = $('#content').val();
        if (PlayListTimeValue == "") {
            alert("Please select Playlist Time");
            return;
        }
        var PlayListTime = parseInt($('#content').val());
        var today = new Date();
        var date = (today.getMonth() + 1) + '/' + today.getDate() + '/' + today.getFullYear();
        //var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
        today.setHours(today.getHours());
        var isPM = today.getHours() >= 12;
        var isMidday = today.getHours() == 12;
        var time = [today.getHours() - (isPM && !isMidday ? 12 : 0), today.getMinutes(), today.getSeconds() || '00'].join(':') + (isPM ? ' PM' : ' AM');
        var currentDate = date + ' ' + time;
        var Department_id = parseInt($('#department_id').val());
        var PlayList_id = parseInt($('#playList_id').val());
        var Dept_PlayList_id = parseInt($('#department_playlist_id').val());
        var is_active = 1;

        var myObject = {
            department_id: Department_id,
            playList_id: PlayList_id,
            when_start: currentDate,
            duration: PlayListTime,
            bool_is_active: is_active
        }

        $.ajax({
            url: '../DepartmentPlayListShedule/CreateDeptPlayList',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ deptPlayListShedule: myObject }),
            success: function (data) {
                if (data.success == true) {
                    var viewURL = '../DepartmentPlayListShedule/index';
                    window.location = viewURL;
                }
                else {
                    alert(data.success);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            }
        });
    }

    else {
        var Department_id = parseInt($('#department_id').val());
        var PlayList_id = parseInt($('#playList_id').val());
        $.ajax({
            url: '../DepartmentPlayList/PublishToAllDepartmentPlayList',
            type: 'POST',
            //dataType: "json",
            //data: { id: PlayList_id},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ id: Department_id }),
            success: function (data) {
                if (data.success == true) {
                    var viewURL = '../DepartmentPlayList/index';
                    window.location = viewURL;
                }
                else {
                    alert(data.success);
                    //var viewURL = '../DepartmentUsers/index';
                    //window.location = viewURL;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            }
        });
    }
}

function loadReport(url, fileName) {
    reportDialog.load(url, function () {
        var object = "<iframe src='../../../GeneratedReports/" + fileName + ".pdf' width='100%' height='97%'>";
        object += "</iframe>";
        //object = object.replace(/{FileName}/g, "Files/" + fileName);
        $("#dialog-report").html(object);
        reportDialog.dialog('open');
    });
}
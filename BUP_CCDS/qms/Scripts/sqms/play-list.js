$(document).ready(function () {
    modalGalleryCreate(GetGallerySelectedItem);
    modalContentCreate(GetContentSelectedItem);

    //$("#start_time").change(function () {
    //    var stime = parseInt($("#start_time").val());
    //    var etime = parseInt($("#end_time").val());
    //    if (etime > stime)
    //        $("#duration_in_second").val(etime - stime);
    //});
    //$("#end_time").change(function () {
    //    var stime = parseInt($("#start_time").val());
    //    var etime = parseInt($("#end_time").val());
    //    if (etime > stime)
    //        $("#duration_in_second").val(etime - stime);
    //});
    //$("#file_name").change(function () {
    //    var stime = parseInt($("#start_time").val());
    //    var etime = parseInt($("#end_time").val());
    //    if (etime > stime)
    //        $("#duration_in_second").val(etime - stime);
    //});

    $("#startTimeInMinutes").change(function () {
        LoadAllTime();
    });
    $("#startTimeInSeconds").change(function () {
        LoadAllTime();
    });
    $("#endTimeInMunites").change(function () {
        LoadAllTime();
    });
    $("#endTimeInSeconds").change(function () {
        LoadAllTime();
    });

    $("#durationInMunites").change(function () {
        LoadDuration();
    });
    $("#durationInSecond").change(function () {
        LoadDuration();
    });

});


function GetGallerySelectedItem(value) {
    $('#file_name').val(value);
    $('#file_type').val("gallery");
    var result = value.split('_').pop();

   
    $('#file_type').val("gallery");
    var extension = value.split(".").pop();
    if (extension == "mp4" || extension == "MP4" || extension == "wmv") {
        var duration = result.match(/\d+/)[0]
        var startTime = $("#start_time").val();
        $("#durationInMunites").attr('readOnly', true);
        $("#durationInSecond").attr('readOnly', true);
        $("#end_time").val(duration);
        $("#startTimeInMinutes").attr('readOnly', false);
        $("#startTimeInSeconds").attr('readOnly', false);
        $("#endTimeInMunites").attr('readOnly', false);
        $("#endTimeInSeconds").attr('readOnly', false);
        $("#volumecircle").show();
        secondsToTime(duration);
        LoadAllTime();
    }
    else {
        $("#durationInMunites").attr('readOnly', false);
        $("#durationInSecond").attr('readOnly', false);
        $("#startTimeInMinutes").attr('readOnly', true);
        $("#startTimeInSeconds").attr('readOnly', true);
        $("#endTimeInMunites").attr('readOnly', true);
        $("#endTimeInSeconds").attr('readOnly', true);
        $("#startTimeInMinutes").val("0");
        $("#startTimeInSeconds").val("0");
        $("#endTimeInMunites").val("0");
        $("#endTimeInSeconds").val("0");
        $("#durationInMunites").val("0");
        $("#durationInSecond").val("15");
        $("#volumecircle").hide();
        
        var dur = $("#durationInSecond").val();
        var hidDuration = document.getElementById('hidDuration');
        hidDuration.value = dur;
    }
}

function GetContentSelectedItem(value) {
    $('#file_name').val(value);
    $('#file_type').val("content");
    var extension = value.split(".").pop();
    if (extension == "mp4" || extension == "MP4" || extension == "wmv") {
        var duration = result.match(/\d+/)[0]
        var startTime = $("#start_time").val();
        $("#durationInMunites").attr('readOnly', true);
        $("#durationInSecond").attr('readOnly', true);
        $("#end_time").val(duration);
        $("#startTimeInMinutes").attr('readOnly', false);
        $("#startTimeInSeconds").attr('readOnly', false);
        $("#endTimeInMunites").attr('readOnly', false);
        $("#endTimeInSeconds").attr('readOnly', false);
        $("#volumecircle").show();
        secondsToTime(duration);
        LoadAllTime();
    }
    else {
        $("#durationInMunites").attr('readOnly', false);
        $("#durationInSecond").attr('readOnly', false);
        $("#startTimeInMinutes").attr('readOnly', true);
        $("#startTimeInSeconds").attr('readOnly', true);
        $("#endTimeInMunites").attr('readOnly', true);
        $("#endTimeInSeconds").attr('readOnly', true);
        $("#startTimeInMinutes").val("0");
        $("#startTimeInSeconds").val("0");
        $("#endTimeInMunites").val("0");
        $("#endTimeInSeconds").val("0");
        $("#durationInMunites").val("0");
        $("#durationInSecond").val("15");
        $("#volumecircle").hide();
        var dur = $("#durationInSecond").val();
        var hidDuration = document.getElementById('hidDuration');
        hidDuration.value = dur;

    }
}

function secondsToTime(secs) {
    var hours = Math.floor(secs / (60 * 60));
    var divisor_for_minutes = secs % (60 * 60);
    var minutes = Math.floor(divisor_for_minutes / 60);
    var divisor_for_seconds = divisor_for_minutes % 60;
    var seconds = Math.ceil(divisor_for_seconds);
    $("#endTimeInMunites").val(minutes);
    $("#endTimeInSeconds").val(seconds);

    $("#durationInMunites").val(minutes);
    $("#durationInSecond").val(seconds);
    //var fullTime = minutes + ":" + seconds

    //$("#end_time").val(fullTime);
    
}

function LoadAllTime() {
    var startMinutes = parseInt($("#startTimeInMinutes").val());
    var startSeconds = parseInt($("#startTimeInSeconds").val());
    var startTime = (startMinutes * 60) + startSeconds;
    var hidStartTime = document.getElementById('hidStartTime');
    hidStartTime.value = startTime;
    

    var endMinutes = parseInt($("#endTimeInMunites").val());
    var endSeconds = parseInt($("#endTimeInSeconds").val());
    var endTime = (endMinutes * 60) + endSeconds;
    var hidEndTime = document.getElementById('hidEndTime');
    hidEndTime.value = endTime;

    var fullDuration = endTime - startTime;
    var hidDuration = document.getElementById('hidDuration');
    hidDuration.value = fullDuration;

    var divisor_for_minutes = fullDuration % (60 * 60);
    var minutes = Math.floor(divisor_for_minutes / 60);

    var divisor_for_seconds = divisor_for_minutes % 60;
    var seconds = Math.ceil(divisor_for_seconds);

    $("#durationInMunites").val(minutes);
    $("#durationInSecond").val(seconds);
}

function LoadDuration() {
    var startMinutes = parseInt($("#startTimeInMinutes").val());
    var startSeconds = parseInt($("#startTimeInSeconds").val());
    var startTime = (startMinutes * 60) + startSeconds;
    var hidStartTime = document.getElementById('hidStartTime');
    hidStartTime.value = startTime;


    var endMinutes = parseInt($("#endTimeInMunites").val());
    var endSeconds = parseInt($("#endTimeInSeconds").val());
    var endTime = (endMinutes * 60) + endSeconds;
    var hidEndTime = document.getElementById('hidEndTime');
    hidEndTime.value = endTime;


    var dutMinutes = parseInt($("#durationInMunites").val());
    var dutSeconds = parseInt($("#durationInSecond").val());
    var duration = (dutMinutes * 60) + dutSeconds;
    var hidDuration = document.getElementById('hidDuration');
    hidDuration.value = duration;
}

function checkval() {

    if ($('#bool_is_in_mute').prop('checked')) {
        $("#volumecircle").hide();
    }
    else {
        $("#volumecircle").show();
    }
}

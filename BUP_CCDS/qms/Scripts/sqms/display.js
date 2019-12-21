/*
            window.addEventListener('load', function(){
                var newVideo = document.getElementById('myVideo');
                newVideo.addEventListener('ended', function() {
                    this.currentTime = 0;
                    this.play();
                }, false);

                newVideo.play();

            });
        */
var speakLanguage, speakGender, speakRate;
$(document).ready(function () {
    speakLanguage = $('#speakLanguage').val();
    speakGender = $('#speakGender').val();
    speakRate = $('#speakRate').val();
    modalConfirm("Do you want to display as full screen?", function () {
        toggleFullScreen(document.body);
    });
})

function toggleFullScreen(elem) {
    // ## The below if statement seems to work better ## if ((document.fullScreenElement && document.fullScreenElement !== null) || (document.msfullscreenElement && document.msfullscreenElement !== null) || (!document.mozFullScreen && !document.webkitIsFullScreen)) {
    if ((document.fullScreenElement !== undefined && document.fullScreenElement === null) || (document.msFullscreenElement !== undefined && document.msFullscreenElement === null) || (document.mozFullScreen !== undefined && !document.mozFullScreen) || (document.webkitIsFullScreen !== undefined && !document.webkitIsFullScreen)) {
        if (elem.requestFullScreen) {
            elem.requestFullScreen();
        } else if (elem.mozRequestFullScreen) {
            elem.mozRequestFullScreen();
        } else if (elem.webkitRequestFullScreen) {
            elem.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
        } else if (elem.msRequestFullscreen) {
            elem.msRequestFullscreen();
        }
    } else {
        if (document.cancelFullScreen) {
            document.cancelFullScreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
            document.webkitCancelFullScreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        }
    }
}





var vid = document.getElementById("myVideo");
vid.addEventListener('ended', videoEndHandler, false);
function videoEndHandler(e) {
    vid.load();
    vid.mute();
    vid.play();
}

$(function () {
    $.support.cors = true;
    // Declare a proxy to reference the hub.
    var notifications = $.connection.notifyDisplay;

    // Create a function that the hub can call to broadcast messages.
    notifications.client.updateMessages = function (text, department_id, device_no, token_no, tokenAdded, tokenCalled, playListChanged, footerChanged) {
        //Call API for Display Data Information [Tokens in progress at devices, next tokens and video content]
        getAllMessages();  

        // Text to Speech if any text available for this department to speech
        if (text.length > 0 && $('#department_id').val() == department_id) textToTalk(text);
    };

    // Start the connection.
    $.connection.hub.start().done(function () {
        //After load UI: Call API for Display Data Information [Tokens in progress at devices, next tokens and video content]
        getAllMessages();
        //After load UI: Text to Speech for Welcome message
        welComeSpeech();
    }).fail(function (e) {
        modalAlert(e);
    });

});

vid.onpause = function () {
    vid.load();
    vid.mute();
    vid.play();
}

function welComeSpeech() {
    var text = $('#welcome').val();

    responsiveVoice.setDefaultVoice(speakLanguage + " " + speakGender);
    responsiveVoice.speak(text, speakLanguage + " " + speakGender, { rate: speakRate });
}

function textToTalk(text) {
    responsiveVoice.setDefaultVoice(speakLanguage + " " + speakGender);
    responsiveVoice.speak(text, speakLanguage + " " + speakGender, { rate: speakRate });
}


function getAllMessages() {
    var department_id = $('#department_id').val();
    $.ajax({
        url: '../Devices/GetDisplayInfo?department_id=' + department_id,
        //contentType: 'application/html ; charset:utf-8',
        type: 'GET',
        //dataType: 'html'
    }).success(function (result) {
        if (result.success == "true") {
            $("#divCurrentTokens").empty();
            var row = '<div class="col-sm-6 bg-color-head"><h2 onclick="return toggleFullScreen(document.body);" style="text-align:center">Device</h2></div><div class="col-sm-6 bg-color-head"><h2 style="text-align:center">Token</h2></div>';
            $('#divCurrentTokens').append(row);
            $.each(result.tokenInProgress, function (key, token) {
                row = '<div class="col-sm-6 bg-color"><h1>' + token.device_no + '</h1></div><div class="col-sm-6 bg-color"><h1>' + token.token_no_formated + '</h1></div>';
                $('#divCurrentTokens').append(row);
            });

            $("#nextToken").text('Next Token: ' + result.nextTokens);

           
        }
        else {
            modalAlert(result.message);
        }
    }).error(function (XMLHttpRequest, textStatus, errorThrown) {
        modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
    });
}

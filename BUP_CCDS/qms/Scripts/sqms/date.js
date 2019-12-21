function getCurrentDate() {
    var currentdate = new Date();
    var datetime = currentdate.getDate().toString().padStart(2, '0') + "-" + getMonthMMM(currentdate.getMonth() + 1)
        + "-" + currentdate.getFullYear() + " "
        + formatAMPM(currentdate);
    return datetime;
}

function getMonthMMM(m) {
    switch (m) {
        case 1: return 'Jan';
        case 2: return 'Feb';
        case 3: return 'Mar';
        case 4: return 'Apr';
        case 5: return 'May';
        case 6: return 'Jun';
        case 7: return 'Jul';
        case 8: return 'Aug';
        case 9: return 'Sep';
        case 10: return 'Oct';
        case 11: return 'Nov';
        case 12: return 'Dec';
        default: return '';
    }
}


function getCurrentTime() {
    var currentdate = new Date();
    var datetime = formatAMPM(currentdate);
    return datetime;
}


function getDBServerCurrentTime() {
    $.ajax({
        url: "../ApiService/GetDBDate",
        type: 'GET',
        dataType: "json",
        success: function (data) {
            //debugger;
            if (data.success == true) {
                return data.dbdate;
            } else {
                modalAlert(data.message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            modalAlert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }

    });
}

var xmlHttp;
function getAppServerCurrentTime() {
    try {
        //FF, Opera, Safari, Chrome
        xmlHttp = new XMLHttpRequest();
    }
    catch (err1) {
        //IE
        try {
            xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
        }
        catch (err2) {
            try {
                xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
            }
            catch (eerr3) {
                //AJAX not supported, use CPU time.
                alert("AJAX not supported");
            }
        }
    }
    xmlHttp.open('HEAD', window.location.href.toString(), false);
    xmlHttp.setRequestHeader("Content-Type", "text/html");
    xmlHttp.send('');
    return new Date(xmlHttp.getResponseHeader("Date"));
}


function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var seconds = + date.getSeconds();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    hours = hours < 10 ? '0' + hours : hours;
    seconds = seconds < 10 ? '0' + seconds : seconds;
    var strTime = hours + ':' + minutes + ':' + seconds + ' ' + ampm;
    return strTime;
}

function getFormattedDateMMDDYYYY(date,sepeator) {
    var year = date.getFullYear();

    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;

    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return month + sepeator + day + sepeator + year;
}

function getFormattedDateDDMMYYYY(date, sepeator) {
    var year = date.getFullYear();

    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;

    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return day + sepeator + month + sepeator + year;
}

function getFormattedDateDDMMMYYYY(date, sepeator) {
    var datetime = new Date(date);
    var year = date.getFullYear();

    var month = (1 + date.getMonth());
    


    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return day + sepeator + getMonthMMM(month) + sepeator + year;
}


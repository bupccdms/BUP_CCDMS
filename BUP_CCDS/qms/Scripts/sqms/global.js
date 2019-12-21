var webRootAddtionalPath = "";


$('.alphaNumericDot').keypress(function (e) {
    var regex = new RegExp("^[a-zA-Z0-9.\s]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }
    e.preventDefault();
    return false;
});
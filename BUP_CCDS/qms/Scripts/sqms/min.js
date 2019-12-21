$(document).ready(function () {
    var url = window.location.href;
    $(".glossymenu a").each(function () {
        if (url == (this.href)) {
            $(this).closest("li").addClass("active");
        }
    });
});

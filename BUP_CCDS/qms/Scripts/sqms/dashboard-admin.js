var bgColors = [
    'rgba(255,99,132,1)',
    'rgba(54, 162, 235, 1)',
    'rgba(255, 206, 86, 1)',
    'rgba(75, 192, 192, 1)',
    'rgba(153, 102, 255, 1)',
    'rgba(255, 159, 64, 1)'
];

var devicesTokens_names = [];
var devicesTokens_tokens = [];
var servicesWaitings_names = [];
var servicesWaitings_tokens = [];
var servicesTokens_names = [];
var servicesTokens_tokens = [];


$(document).ready(function () {
    inp = $("#department_name option:selected").text();
    if (inp == "All Department") {
        $("#department_name").attr('disabled', false);
    }
    else $("#department_name").attr('disabled', true);

    FilterTable2();
    $("#department_name").change(function () {
        // var selectedDepartment = $("#department_name option:selected").text();
        FilterTable2();

    });
});

function FilterTable2() {
    index = -1;
    inp = $("#department_name option:selected").text();
    if (inp == "All Department") {
        inp = "";
    }
    $("#tblDeviceStatusList:visible tr:not(:has(>th))").each(function () {
        if (~$(this).text().toLowerCase().indexOf(inp.toLowerCase())) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
    $('#Hedding').show();
};
function FilterTable() {
    index = -1;
    inp = $('#filterBox').val();

    $("#tblDepartmentServiceDetailList:visible tr:not(:has(>th))").each(function () {
        if (~$(this).text().toLowerCase().indexOf(inp.toLowerCase())) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
    $('#Hedding').show();
};


$(document).ready(function () {
    loadData();
});


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

function reload() {
    /*devicesTokens_names = [];
    devicesTokens_tokens = [];
    servicesWaitings_names = [];
    servicesWaitings_tokens = [];
    servicesTokens_names = [];
    servicesTokens_tokens = [];
    */
    loadData();

    return false;
}

function loadData() {
    var id = $("#department_id option:selected").val();
    $.ajax({
        url: '../ApiService/GetAdminDashboard',
        type: 'POST',
        data: {id:id},
        dataType: 'json',
        success: function (result) {
            /*$.each(result.data, function (i, data) {
                devicesTokens_names.push('C' + data.device_no);
                devicesTokens_tokens.push(data.tokens);
            });

            $.each(result.servicesWaitings, function (i, data) {
                servicesWaitings_names.push(data.service_name);
                servicesWaitings_tokens.push(data.tokens);
            });

            $.each(result.servicesTokens, function (i, data) {
                servicesTokens_names.push(data.service_name);
                servicesTokens_tokens.push(data.tokens);
            });*/

            $("#tblDeviceStatusList").find("tr:gt(0)").remove();
            $.each(result.deviceStatusList, function (i, item) {
                $("#tblDeviceStatusList")
                    .append($('<tr>')
                        .append($('<td>')
                            .append(item.department_name)
                        )
                        .append($('<td>')
                            .append(item.device_name)
                        )
                        .append($('<td>')
                            .append(item.playlist_name)
                        )
                        .append($('<td>')
                            .append(item.scroll)
                        )
                    );
            });
            /*$('#tblDepartmentTokenList tr:contains("TOTAL")').attr('id', 'Hedding');

            $("#tblDepartmentServiceList").find("tr:gt(0)").remove();
            $.each(result.departmentServiceList, function (i, item) {
                $("#tblDepartmentServiceList")
                    .append($('<tr>')
                        .append($('<td>')
                            .append(item.service_name)
                        )
                        .append($('<td>')
                            .append(item.served)
                        )
                    );
            });
            $('#tblDepartmentServiceList tr:contains("TOTAL")').attr('id', 'Hedding');

            $("#tblDepartmentServiceDetailList").find("tr:gt(0)").remove();
            $.each(result.departmentServiceDetailList, function (i, item) {
                $("#tblDepartmentServiceDetailList")
                    .append($('<tr>')
                        .append($('<td>')
                            .append(item.department_name)
                        )
                        .append($('<td>')
                            .append(item.device)
                        )
                        .append($('<td>')
                            .append(item.token_no_formated)
                        )
                        .append($('<td>')
                            .append(item.customer_type)
                        )
                        .append($('<td>')
                            .append(item.service)
                        )
                        .append($('<td>')
                            .append(item.issue_time_formated)
                        )
                        .append($('<td>')
                            .append(item.start_time_formated)
                        )
                        .append($('<td>')
                            .append(item.end_time_formated)
                        )
                        .append($('<td>')
                            .append(item.service_status)
                        )
                    );
            });

            generateDevicesTokensChart();
            generateServicesWaitingsChart();
            generateServicesTokensChart();*/
            $("#currentDate").html(getCurrentDate());
        }
    });

}

function generateDevicesTokensChart() {

    var helpers = Chart.helpers;
    new Chart(document.getElementById("devicesTokenChart"), {
        "type": "pie",
        "data": {
            "labels": devicesTokens_names,
            "datasets": [{
                "label": "Device Wise Report",
                "data": devicesTokens_tokens,
                "backgroundColor": bgColors
            }]
        },
        options: {
            responsive: true,
            legend: {
                position: 'bottom',
                labels: {
                    generateLabels: function (chart) {
                        var data = chart.data;
                        if (data.labels.length && data.datasets.length) {
                            return data.labels.map(function (label, i) {
                                var meta = chart.getDatasetMeta(0);
                                var ds = data.datasets[0];
                                var arc = meta.data[i];
                                var custom = arc && arc.custom || {};
                                var valueAtIndexOrDefault = helpers.valueAtIndexOrDefault;
                                var arcOpts = chart.options.elements.arc;
                                var fill = custom.backgroundColor ? custom.backgroundColor : valueAtIndexOrDefault(ds.backgroundColor, i, arcOpts.backgroundColor);
                                var stroke = custom.borderColor ? custom.borderColor : valueAtIndexOrDefault(ds.borderColor, i, arcOpts.borderColor);
                                var bw = custom.borderWidth ? custom.borderWidth : valueAtIndexOrDefault(ds.borderWidth, i, arcOpts.borderWidth);

                                return {
                                    text: label + " : " + ds.data[i],
                                    fillStyle: fill,
                                    strokeStyle: stroke,
                                    lineWidth: bw,
                                    hidden: isNaN(ds.data[i]) || meta.data[i].hidden,

                                    // Extra data used for toggling the correct item
                                    index: i
                                };
                            });
                        }
                        return [];
                    }
                }
            },
            title: {
                display: true,
                text: 'Device Wise Report'
            },
            animation: {
                animateScale: true,
                animateRotate: true
            }
        }
    });
}

function generateServicesWaitingsChart() {

    var helpers = Chart.helpers;
    new Chart(document.getElementById("servicesWaitingChart"), {
        "type": "bar",
        "data": {
            "labels": servicesWaitings_names,
            "datasets": [{
                "label": "Service Wise Waiting Token Status",
                "data": servicesWaitings_tokens,
                "backgroundColor": bgColors
            }]
        },
        options: {
            responsive: true,
            legend: {
                position: 'bottom',
                labels: {
                    generateLabels: function (chart) {
                        var data = chart.data;
                        if (data.labels.length && data.datasets.length) {
                            return data.labels.map(function (label, i) {
                                var meta = chart.getDatasetMeta(0);
                                var ds = data.datasets[0];
                                var arc = meta.data[i];
                                var custom = arc && arc.custom || {};
                                var valueAtIndexOrDefault = helpers.valueAtIndexOrDefault;
                                var arcOpts = chart.options.elements.arc;
                                var fill = custom.backgroundColor ? custom.backgroundColor : valueAtIndexOrDefault(ds.backgroundColor, i, arcOpts.backgroundColor);
                                var stroke = custom.borderColor ? custom.borderColor : valueAtIndexOrDefault(ds.borderColor, i, arcOpts.borderColor);
                                var bw = custom.borderWidth ? custom.borderWidth : valueAtIndexOrDefault(ds.borderWidth, i, arcOpts.borderWidth);

                                return {
                                    text: label + " : " + ds.data[i],
                                    fillStyle: fill,
                                    strokeStyle: stroke,
                                    lineWidth: bw,
                                    hidden: isNaN(ds.data[i]) || meta.data[i].hidden,

                                    // Extra data used for toggling the correct item
                                    index: i
                                };
                            });
                        }
                        return [];
                    }
                }
            },
            title: {
                display: true,
                text: 'Service Wise Waiting Token Status'
            },
            animation: {
                animateScale: true,
                animateRotate: true
            },
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}


function generateServicesTokensChart() {

    var helpers = Chart.helpers;
    new Chart(document.getElementById("servicesTokenChart"), {
        "type": "pie",
        "data": {
            "labels": servicesTokens_names,
            "datasets": [{
                "label": "Service Wise Report",
                "data": servicesTokens_tokens,
                "backgroundColor": bgColors
            }]
        },
        options: {
            responsive: true,
            legend: {
                display: false
                //position: 'bottom',
                //labels: {
                //    generateLabels: function (chart) {
                //        var data = chart.data;
                //        if (data.labels.length && data.datasets.length) {
                //            return data.labels.map(function (label, i) {
                //                var meta = chart.getDatasetMeta(0);
                //                var ds = data.datasets[0];
                //                var arc = meta.data[i];
                //                var custom = arc && arc.custom || {};
                //                var valueAtIndexOrDefault = helpers.valueAtIndexOrDefault;
                //                var arcOpts = chart.options.elements.arc;
                //                var fill = custom.backgroundColor ? custom.backgroundColor : valueAtIndexOrDefault(ds.backgroundColor, i, arcOpts.backgroundColor);
                //                var stroke = custom.borderColor ? custom.borderColor : valueAtIndexOrDefault(ds.borderColor, i, arcOpts.borderColor);
                //                var bw = custom.borderWidth ? custom.borderWidth : valueAtIndexOrDefault(ds.borderWidth, i, arcOpts.borderWidth);

                //                return {
                //                    text: label + " : " + ds.data[i],
                //                    fillStyle: fill,
                //                    strokeStyle: stroke,
                //                    lineWidth: bw,
                //                    hidden: isNaN(ds.data[i]) || meta.data[i].hidden,

                //                    // Extra data used for toggling the correct item
                //                    index: i
                //                };
                //            });
                //        }
                //        return [];
                //    }
                //}
            },
            title: {
                display: true,
                text: 'Service Wise Report'
            },
            animation: {
                animateScale: true,
                animateRotate: true
            }
        }
    });
}
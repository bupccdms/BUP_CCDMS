<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingleVSMultipleVisitSummaryReport.aspx.cs" Inherits="qms.Reports.SingleVSMultipleVisitSummaryReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
   <div class="container">
    <div class="row">
        <h4 class="page-head-line">Single vs Multiple Visit Summary</h4>
        <div class="col-md-12">
            <div class="panel-group" id="accordion" style="text-align:center">
                <div class="panel panel-default">
                    <div class="panel-heading" style="height:38px">
                        <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse1" style="float:right">
                                Filter
                            </a>
                        </h4>
                    </div>

                    <div id="collapse1" class="panel-collapse collapse in" style="margin-bottom:-40px;">

                        <div class="panel-body">
                            <div class="col-md-4">
                                <div class="col-md-12 bottom-space">
                                    <div class="col-md-5">
                                        Select Department :
                                    </div>
                                    <div class="col-md-7">
                                        <%--<select name="department_id" class="form-control" id="department_id" required>
                                            <option value="0">All Department</option>
                                            @foreach (var item in ViewBag.departmentList)
                                            {
                                                <option value="@item.department_id">@item.department_name</option>
                                            }
                                        </select>--%>
                                        <asp:DropDownList ID="ddlDepartment" AppendDataBoundItems="true" runat="server" Class="from-control" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-12 bottom-space">
                                    <div class="col-md-5">
                                        Select User :
                                    </div>
                                    <div class="col-md-7">
                                        <%--<select name="txtUser" class="form-control" required id="txtUser"></select>--%>
                                    </div>

                                </div>
                                <div class="col-md-12 bottom-space">
                                    <div class="col-md-5">
                                        Select Device :
                                    </div>
                                    <div class="col-md-7">
                                        <%--<select name="txtDevice" class="form-control" required id="txtDevice"></select>--%>
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-4">
                                <div class="col-md-12 bottom-space">
                                    <div class="col-md-5">
                                        Select Service :
                                    </div>
                                    <div class="col-md-7">
                                        <%--<select name="service_sub_type_id" class="form-control" id="service_sub_type_id" required>
                                            <option value="0">All Services</option>
                                            @foreach (var item in ViewBag.serviceList)
                                            {
                                                <option value="@item.service_sub_type_id">@item.service_sub_type_name</option>
                                            }
                                        </select>--%>
                                    </div>
                                </div>
                                <div class="col-md-12 bottom-space">
                                    <div class="col-md-5">
                                        From Date :
                                    </div>
                                    <div class="col-md-7">
                                        <%--<input type="date" class="form-control" id="txtFromDate" />--%>
                                    </div>

                                </div>
                                <div class="col-md-12 bottom-space">
                                    <div class="col-md-5">
                                        To Date :
                                    </div>
                                    <div class="col-md-7">
                                        <%--<input type="date" class="form-control" id="txtToDate" />--%>
                                    </div>

                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="col-md-12 bottom-space">
                                    <div class="col-md-5">
                                        Start Time :
                                    </div>
                                    <div class="col-md-7">
                                        <div style="position: relative">
                                            <%--<input class="timepicker form-control" type="text" id="txtStartTime" />--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12 bottom-space">
                                    <div class="col-md-5">
                                        End Time :
                                    </div>
                                    <div class="col-md-7" style="position: relative">
                                        <%--<input class="timepicker form-control" type="text" id="txtEndTime" />--%>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-5">
                                    </div>
                                    <div class="col-md-7" style="position: relative">
                                        <%--<input type="button" value="Load Report" style="width:130px;" onclick="GenerateVisitedCustomerList()" class="btn btn-sm btn-color pull-right" />--%>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="collapse2" class="panel-collapse collapse">
                    <div class="row">
                        <div class="col-md-12" style="text-align:right;">
                            <%--<img src="~/img/csv_img.png" style="height:22px;" onclick="SinglevsMultipleVisitSummaryReport('CSV')" />
                            <img src="~/img/doc_img.JPG" style="height:22px;" onclick="SinglevsMultipleVisitSummaryReport('Word')" />
                            <img src="~/img/excel_img.jpg" style="height:21px;" onclick="SinglevsMultipleVisitSummaryReport('Excel')" />
                            <img src="~/img/pdf_image.jpg" style="height:25px;" onclick="SinglevsMultipleVisitSummaryReport('PDF')" />--%>
                        </div>
                    </div>
                    <%--<div class="panel-body" style="padding-top:5px">
                        <div class="row">
                            <fieldset>
                                <h4><b>Single vs Multiple Visit Summary List</b></h4>
                                <div>
                                    <div id="reportDiv" class="col-md-12 scrollbar table-scroll">
                                        <table class="table table-hover table-bordered">
                                            <thead>
                                                <tr style="text-align: center;">
                                                    <th>Department Name</th>
                                                    <th>Service Name</th>
                                                    <th>Total Served Token</th>
                                                    <th>Single Visited Customers</th>
                                                    <th>Multiple Visited Customers</th>
                                                </tr>
                                            </thead>
                                            <tbody id="tablebody"></tbody>
                                        </table>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
</div>
    </form>
</body>
</html>

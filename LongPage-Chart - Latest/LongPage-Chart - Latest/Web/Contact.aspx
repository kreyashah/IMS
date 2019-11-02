<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Web.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.3.0/Chart.min.js"></script>
    <div class="container">
        <div class="row">
            <div class="col-md-12" style="margin-top: 50px;">
                <canvas id="chartNoIndividualsByProvince" height="150"></canvas>
            </div>
            <div class="col-md-12" style="margin-top: 50px;">
                <canvas id="chartIdPsByIncidentAndProvinceData" height="100"></canvas>
            </div>
        </div>
        <div class="clear-fix"></div>
        <div class="row" style="margin-top: 50px;">
            <div class="col-md-12">
                <canvas id="piePercentageIDPsByProvince" height="100"></canvas>
            </div>
            <div class="col-md-12" style="margin-top: 50px;">
                <canvas id="chartNoIndividualsByHazard" height="100"></canvas>
            </div>
        </div>
    </div>
    <div class="cleaner h40"></div>
    <div class="cleaner"></div>
    <script type="text/javascript">
        $(document).ready(function () {
            //Chart.defaults.global.defaultFontColor = 'black';
            //Chart.defaults.global.defaultFontSize = 16;
            //var theHelp = Chart.helpers;
            GetChartData().done(function (response) {
                var allChart = JSON.parse(response.d);
                ChartNoIndividualsByHazard(allChart.Hazards);
                ChartNoIndividualsByProvince(allChart.ByProvinces);
                ChartIdPsByIncidentAndProvinceData(allChart.IncidentAndProvince);
                piePercentageIDPsByProvince(allChart.PercentageIdPsByProvinces);
            });
        });
        function GetChartData() {
            return $.ajax({
                url: '<%=ResolveUrl("Contact.aspx/GetChartData") %>',
                type: "POST",
                dataType: "json",
                traditional: true,
                contentType: "application/json; charset=utf-8"
            });
        }
        function ChartNoIndividualsByHazard(data) {
            var canvas = document.getElementById("chartNoIndividualsByHazard");
            var ctx = canvas.getContext('2d');

            var labelList = [];
            var dataList = [];
            if (data.length !== 0) {
                labelList = data.map(x => x.Incident);
                dataList = data.map(x => x.Population);
            }

            var horizontalBarChartData = {
                labels: labelList,
                datasets: [{
                    label: 'No. of Individuals',
                    backgroundColor: "#0066CC",
                    borderColor: "#0066CC",
                    borderWidth: 1,
                    data: dataList,
                    axisX: {
                        labelAutoFit: true, //false by default.
                        //labelWrap: false, // true by default.
                        labelMaxWidth: 100,
                    }
                }]

            };

            window.myHorizontalBar = new Chart(ctx, {
                type: 'horizontalBar',
                data: horizontalBarChartData,
                options: {
                    // Elements options apply to all of the options unless overridden in a dataset
                    // In this case, we are setting the border of each horizontal bar to be 2px wide
                    elements: {
                        rectangle: {
                            borderWidth: 2,
                        }
                    },
                    rotation: -0.7 * Math.PI,
                    responsive: true,
                    legend: {
                        display: false,
                        position: 'right',
                    },
                    title: {
                        display: true,
                        text: 'No. of Individuals by cause of incident',
                        position: 'top'
                    },
                    scales: {
                        //yAxes: [{
                        //    ticks: {
                        //        // Include a dollar sign in the ticks
                        //        callback: function (value, index, values) {
                        //            return formatLabel(value, 20);
                        //        }
                        //    }
                        //}],
                        xAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });

        }

        function ChartNoIndividualsByProvince(data) {
            var canvas = document.getElementById("chartNoIndividualsByProvince");
            var ctx = canvas.getContext('2d');
            var labelList = [];
            var dataList = [];
            if (data.length !== 0) {
                labelList = data.map(x => x.ProvinceName);
                dataList = data.map(x => x.Population);
            }
            var horizontalBarChartData = {
                labels: labelList,
                datasets: [{
                    label: "No. of Individuals",
                    backgroundColor: "#0066CC",
                    borderColor: "#0066CC",
                    borderWidth: 1,
                    data: dataList,
                    axisX: {
                        labelAutoFit: true, //false by default.
                        //labelWrap: false, // true by default.
                        labelMaxWidth: 100,
                    }
                }]

            };

            window.myHorizontalBar = new Chart(ctx, {
                type: 'horizontalBar',
                data: horizontalBarChartData,
                options: {
                    responsive: true,
                    rotation: -0.7 * Math.PI,
                    legend: {
                        display: false,
                        position: 'right'
                    },
                    title: {
                        display: true,
                        text: 'No. IDPs by incident and province',
                        position: 'top'
                    },
                    scales: {
                        //yAxes: [{
                        //    ticks: {
                        //        // Include a dollar sign in the ticks
                        //        callback: function (value, index, values) {
                        //            return formatLabel(value, 20);
                        //        }
                        //    }
                        //}],
                        xAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

        function ChartIdPsByIncidentAndProvinceData(data) {
            var canvas = document.getElementById("chartIdPsByIncidentAndProvinceData");
            var ctx = canvas.getContext('2d');
            if (data.length !== 0) {
                var lblGroupName = data.groupCharts.map(x => x.Name);
                var oldColor;
                data.groupChartDatas = data.groupChartDatas.map(x => {
                    oldColor = x.backgroundColor;
                    if (x.backgroundColor === "" || x.backgroundColor == null) {
                        var colour = getRandomColor();
                        if (oldColor === colour) {
                            colour = getRandomColor();
                        }
                        x.backgroundColor = colour;
                        oldColor = colour;
                    }
                    return x;
                });

                var barChartData = {
                    labels: lblGroupName,
                    datasets: data.groupChartDatas
                };

                window.myBar = new Chart(ctx, {
                    type: 'bar',
                    data: barChartData,
                    options: {
                        responsive: true,
                        legend: {
                            display: true,
                            position: 'bottom',
                        },
                        title: {
                            display: true,
                            text: 'No. of Individuals displaced by type of hazard',
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        },
                        tooltips: {
                            callbacks: {
                                label: function (tooltipItem, data) {
                                    //get the concerned dataset
                                    var dataset = data.datasets[tooltipItem.datasetIndex];

                                    //get the current items value
                                    var currentValue = dataset.data[tooltipItem.index];
                                    debugger;
                                    //calculate the precentage based on the total and current item, also this does a rough rounding to give a whole number

                                    return "No of individuals induced by '" + dataset.label + "' :" + currentValue;
                                }
                            }
                        }
                    }
                });
            }
        }

        function piePercentageIDPsByProvince(data) {
            var canvas = document.getElementById("piePercentageIDPsByProvince");
            var ctx = canvas.getContext('2d');

            var labelList = [];
            var dataList = [];
            var backgroundColorList = [];
            var oldColor;

            if (data.length !== 0) {
                labelList = data.map(x => x.ProvinceName);
                dataList = data.map(x => x.Population);
                backgroundColorList = data.map(x => {
                    oldColor = x.Color;
                    if (x.Color === "" || x.Color == null) {
                        var colour = getRandomColor();
                        if (oldColor === colour) {
                            colour = getRandomColor();
                        }
                        x.Color = colour;
                        oldColor = colour;
                    }
                    return x.Color;
                });

            }

            var pieChartData = {
                type: 'pie',
                data: {
                    labels: labelList,
                    datasets: [{
                        backgroundColor: backgroundColorList,
                        data: dataList
                    }]
                },
                options: {
                    responsive: true,
                    rotation: -0.7 * Math.PI,
                    legend: {
                        display: true,
                        position: 'bottom'
                    },
                    title: {
                        display: true,
                        text: 'Percentage of IDPs by province',
                        position: 'top'
                    },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                //get the concerned dataset
                                var dataset = data.datasets[tooltipItem.datasetIndex];

                                //get the current items value
                                var currentValue = dataset.data[tooltipItem.index];
                                //calculate the precentage based on the total and current item, also this does a rough rounding to give a whole number

                                return currentValue + "%";
                            }
                        }
                    }
                }
            };
            window.myPie = new Chart(ctx, pieChartData);
        }

        function getRandomInt(min, max) {
            return Math.floor(Math.random() * (max - min + 1)) + min;
        }

        function componentToHex(c) {
            var hex = c.toString(16);
            return hex.length === 1 ? "0" + hex : hex;
        }

        function getRandomColor() {
            //generate random red, green and blue intensity
            var r = getRandomInt(0, 255);
            var g = getRandomInt(0, 255);
            var b = getRandomInt(0, 255);

            var color = "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
            return color;
        }

        function formatLabel(str, maxwidth) {
            var sections = [];
            var words = str.split(" ");
            var temp = "";

            words.forEach(function (item, index) {
                if (temp.length > 0) {
                    var concat = temp + ' ' + item;

                    if (concat.length > maxwidth) {
                        sections.push(temp);
                        temp = "";
                    }
                    else {
                        if (index == (words.length - 1)) {
                            sections.push(concat);
                            return;
                        }
                        else {
                            temp = concat;
                            return;
                        }
                    }
                }

                if (index == (words.length - 1)) {
                    sections.push(item);
                    return;
                }

                if (item.length < maxwidth) {
                    temp = item;
                }
                else {
                    sections.push(item);
                }

            });

            return sections;
        }
    </script>
</asp:Content>

<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebUI._Default" %>



<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    	<meta charset="utf-8"/>
	<meta http-equiv="X-UA-Compatible" content="IE=edge" >
	<link rel="stylesheet" href="//netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css">
	<link rel="stylesheet" href="/Content/Default.css">
	<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
	<script src="//netdna.bootstrapcdn.com/bootstrap/3.0.3/js/bootstrap.min.js"></script>
        <div id = "wrapper">
		<div class = "canvaswrapper">
			<canvas id="myCanvas" align="center" style="" width="500" height="500"></canvas>
		</div>
		<a id="saveBtn" class="btn btn-large btn-primary" style="margin-left:30%;margin-right:5%">Сохранить результаты</a>
		<a id="clearBtn" class="btn btn-large">Обнулить результаты</a>
		</div>
    <a id="simpleScheduleDataViewer">посмотреть простые результаты </a>
    <script>
        $("#simpleScheduleDataViewer").click(function () {
            var parsedData;
            $.ajax({
                async: false,
                url: "/Default.aspx/GetSimpleStaticSchedule",
                data: "{ 'tablewidth': '" + 200 + "', 'tableheight': '" + 200 + "','quantity':'" + 50 + "'}",
                dataType: "text",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    parsedData = $.parseJSON(data);
                    parsedData = $.parseJSON(parsedData.d);
                    $("#myCanvas").attr("width", parsedData.tables.width)
			      				 .attr("height", parsedData.tables.height);
                    draw(0, 0, parsedData.tables.width, parsedData.tables.height, "white");
                    for (var i = 0; i < parsedData.coas.length; i++) {
                        var coa = parsedData.coas[i];
                        draw(coa.X, coa.Y, coa.width, coa.height, getRandomColor());
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + errorThrown);
                }
            });
        });
        
        function draw(x, y, w, h, fillColor) {
            var ctx = document.getElementById('myCanvas').getContext('2d');
            //ctx.fillStyle = "#FF0000";//getRandomColor();//"
            //ctx.fillRect (x, y, w, h);
            ctx.beginPath();
            ctx.rect(x, y, w, h);
            ctx.fillStyle = fillColor;
            ctx.fill();
            ctx.lineWidth = 1;
            ctx.strokeStyle = 'black';
            ctx.stroke();
        }
        function getRandomColor() {
            var colorArray = ["#8BD7FF", "#D5B4FF", "#99FFC6", "#FFFDBB", "#FFBA93", "#FF9D9C", "#BDFFEE"];
            var randomId = 1 + Math.floor(Math.random() * colorArray.length);
            return colorArray[randomId];
        }
</script>
</asp:Content>

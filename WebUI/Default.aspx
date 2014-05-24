<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebUI._Default" %>



<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    	<meta charset="utf-8"/>
	<meta http-equiv="X-UA-Compatible" content="IE=edge" >
	<link rel="stylesheet" href="//netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css">
	<link rel="stylesheet" href="/Content/Default.css">

        <div class="loader">
            <h1>LOADING</h1>
            <span></span>
            <span></span>
            <span></span>
        </div>
    

        <div id = "wrapper">
        <div id="inData">
            <div class="controls controls-row">
                Вибростол
                <input id="tableWidth" class="span1" type="text" placeholder="Длина"> и 
                <input id="tableHeight" class="span1" type="text" placeholder="Ширина">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Формы 
                <input id="quantity" class="span1" type="text" placeholder="Сколько?">
            </div>
            <div class="controls controls-row">
                Правила 
                <select>
                  <option>1</option>
                  <option>2</option>
                  <option>3</option>
                  <option>4</option>
                  <option>5</option>
                </select>
            </div>
            <a id="simpleScheduleDataViewer" class="btn btn-large btn-primary" style="margin-left:30%;margin-right:5%">Показать кадры</a>
		    <a id="clearBtn" class="btn btn-large">Обнулить результаты</a>
        </div>
		<div class = "canvaswrapper">
			<canvas id="myCanvas" align="center" style="margin-top: 20px;" width="500" height="500"></canvas>
            <div id="frameWrapper"></div>
		</div>
         <div id="stats"></div>

		</div>
    <script>
        var STATIC_DATA;
        $(".loader").hide();
        $("#clearBtn").click(function () {
            $(".loader").show();
        });
        $("#simpleScheduleDataViewer").click(function () {
            $(".loader").show();
            var valid = true;
            if ($("#tableWidth").val() == 0) {
                $("#tableWidth").addClass("error");
                valid = false;
            } else {
                $("#tableWidth").removeClass("error");
            }
            if ($("#tableHeight").val() == 0) {
                $("#tableHeight").addClass("error");
                valid = false;
            } else {
                $("#tableHeight").removeClass("error");
            }
            if ($("#quantity").val() == 0) {
                $("#quantity").addClass("error");
                valid = false;
            } else {
                $("#quantity").removeClass("error");
            }
            
            if (!valid) return;
            
            var parsedData;
            
            $.ajax({
                async: false,
                url: "/Default.aspx/GetSimpleStaticSchedule",
                data: "{ 'tablewidth': '" + $("#tableWidth").val() + "', 'tableheight': '" + $("#tableHeight").val() + "','quantity':'" + $("#quantity").val() + "'}",
                dataType: "text",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    parsedData = $.parseJSON(data);
                    parsedData = $.parseJSON(parsedData.d);
                    STATIC_DATA = parsedData;
                    extractFramesData(STATIC_DATA);
                    $(".loader").hide();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + errorThrown);
                }
            });
        });

        function getDataForThisFrame(i) {
            
            $("#myCanvas").attr("width", STATIC_DATA[i].VibroTable.Width)
			      				 .attr("height", STATIC_DATA[i].VibroTable.Height);
            
            draw(0, 0, STATIC_DATA[i].VibroTable.Width, STATIC_DATA[i].VibroTable.Height, "white");
            
            
            for (var j = 0; j < STATIC_DATA[i].SetList.length; j++) {
                var coa = STATIC_DATA[i].SetList[j];
                draw(coa.Left, coa.Top, coa.Width, coa.Height, getRandomColor());
            }
            
            $("#stats").html("Заполнено: " + STATIC_DATA[i].CoveredPercentage * 100 + "%");
        }

        function extractFramesData(data) {
            
            $("#frameWrapper").empty();
            $("#stats").empty();
            for (var i = 0; i < data.length; i++) {
                
                $("#frameWrapper").append("<a onClick = 'getDataForThisFrame(" + i + ")' class = 'frameViewer' id='frame" + i + "'>Кадр " + i + "</a>");
            }
            getDataForThisFrame(0);
        }

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
            var randomId = Math.floor(Math.random() * colorArray.length);
            return colorArray[randomId];
        }
    </script>
</asp:Content>

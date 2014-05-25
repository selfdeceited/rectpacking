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
                <select id="ruleOption" style="padding: 6px;margin: 10px;border-radius: 5px;" >
                    <option value="1">Пустое</option>
                    <option value="2">Сначала в угол наибольшие</option>
                    <option value="3">Квазичеловеческая</option>
                    <option value="4">Комплексное правило</option>
                    <option value="5">По похожему времени застывания (Д)</option>
                    <option value="6">Сначала долго застывающие (Д)</option>
                </select>
            </div>
            <a id="simpleScheduleDataViewer" class="btn btn-large btn-primary" style="margin-left:23%;margin-right:5%">Статическое расписание (кадры)</a>
		    <a id="clearBtn" class="btn btn-large btn-primary">Динамическое расписание</a>
        </div>
		<div class = "canvaswrapper">
			<canvas id="myCanvas" align="center" style="margin-top: 20px;" width="500" height="500"></canvas>
            <div id="stats"></div>

            <div id="frameWrapper">
                <div class="timeline">
                  <hr />
                  <div class="events">
                  </div>
                </div>
                <a id="playBtn" style="display:none" class="btn btn-large btn-primary">Слайдшоу</a>
                <a id="dynamicBtn" style="display:none" class="btn btn-large btn-primary">Слайдшоу</a>

                    <div id="remakeDiv">
                        Начиная с кадра
                      <input id="frameFromWhichToRemake" type="text" class="form-control span1" placeholder="№">
                        переделать по стратегии
                        <select id="ruleOptionCopy" style="padding: 6px;margin: 10px;border-radius: 5px;" >
                            <option value="1">Пустое</option>
                            <option value="2">Сначала в угол наибольшие</option>
                            <option value="3">Квазичеловеческая</option>
                            <option value="4">Комплексное правило</option>
                            <option value="5">По похожему времени застывания (Д)</option>
                            <option value="6">Сначала долго застывающие (Д)</option>
                        </select>
                        <a id="remakeBtn" style="" class="btn btn-large btn-primary">Начать</a>
                    </div>

            </div>
		</div>
         

		</div>

    <script>

        var STATIC_DATA;
        var DYNAMIC_DATA;
        var toStop = true;
        var el = 0;
        var dEl = 0;

        $(".loader").hide();
        $("hr").hide();
        $("#remakeDiv").hide();
        

        setInterval(function () {
            var length = $(".frameViewer").length;
            if (length === 0) return;

            if (!toStop && el < length) {
                getDataForThisFrame(el);
                $(".frameViewer").removeClass("selectedElement");
                $("#frame" + el + "").addClass("selectedElement");
                el++;
                if (el === length) el = 0;
            }
        }, 2000);

        setInterval(function () {
            var length = $(".frameViewer").length;
            if (length === 0) return;

            if (!toStop && dEl < length) {
                getDataForDynamicFrame(dEl);
                $(".frameViewer").removeClass("selectedElement");
                $("#frame" + dEl + "").addClass("selectedElement");
                dEl++;
                if (dEl === length) dEl = 0;
            }
        }, 1000);



        $("#playBtn").click(function () {
            if ($("#playBtn").text() !== "Стоп") {
                $("#playBtn").text("Стоп");
                toStop = false;
                el = 0;
            }
            else {
                toStop = true;
                el = 0;
                $(".frameViewer").removeClass("selectedElement");
                $("#playBtn").text("Слайдшоу");
            }
        });

        $("#dynamicBtn").click(function () {
            if ($("#dynamicBtn").text() !== "Стоп") {
                $("#dynamicBtn").text("Стоп");
                toStop = false;
                dEl = 0;
            }
            else {
                toStop = true;
                dEl = 0;
                $(".frameViewer").removeClass("selectedElement");
                $("#dynamicBtn").text("Слайдшоу");
            }
        });








        $("#clearBtn").click(function () {
            

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

            $("#playBtn").hide();
            $("#remakeDiv").hide();
            $("#dynamicBtn").show();
            var parsedData;

            $.ajax({
                async: false,
                url: "/Default.aspx/GetSimpleDynamicSchedule",
                data: "{ 'tablewidth': '" + $("#tableWidth").val() + "', 'tableheight': '" + $("#tableHeight").val() + "','quantity':'" + $("#quantity").val() + "', 'ruleOption':'" + $("#ruleOption").val() + "'}",
                dataType: "text",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    parsedData = $.parseJSON(data);
                    parsedData = $.parseJSON(parsedData.d);
                    DYNAMIC_DATA = parsedData;
                    extractDynamicData();
                    $(".loader").hide();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + errorThrown);
                }
            });
        });

        $("#simpleScheduleDataViewer").click(function () {
            $(".loader").show();
            $("#dynamicBtn").hide();

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
                data: "{ 'tablewidth': '" + $("#tableWidth").val() + "', 'tableheight': '" + $("#tableHeight").val() + "','quantity':'" + $("#quantity").val() + "', 'ruleOption':'" + $("#ruleOption").val() + "'}",
                dataType: "text",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    parsedData = $.parseJSON(data);
                    parsedData = $.parseJSON(parsedData.d);
                    STATIC_DATA = parsedData;
                    extractFramesData(STATIC_DATA);
                    $(".loader").hide();
                    $("hr").show();
                    $("#playBtn").show();
                    $("#remakeDiv").show();
                    
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
            
            $("#stats").html("Заполнено: " + (STATIC_DATA[i].CoveredPercentage * 100).toFixed(2) + "%");

            $("#frameFromWhichToRemake").val(i + 1);
        }

        function extractFramesData(data) {
            
            $(".events").empty();
            $("#stats").empty();
            for (var i = 0; i < data.length; i++) {
                
                $(".events").append("<div onClick = 'getDataForThisFrame(" + i + ")' class = 'frameViewer event' id='frame" + i + "'>Кадр " + (i+1).toString() + "</div>");
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

        function extractDynamicData() {
            $("hr").show();
            $(".events").empty();
            $("#stats").empty();
            var timeStamps = [];
            for (var i = 0; i < DYNAMIC_DATA.coas.length; i++) {
                var coa = DYNAMIC_DATA.coas[i];
                if ($.inArray(coa.Start, timeStamps) === -1) {
                    timeStamps.push(coa.Start);
                }
                if ($.inArray(coa.End, timeStamps) === -1) {
                    timeStamps.push(coa.End);
                }
            }
            timeStamps.sort(function (a, b) { return Date.parse(a) - Date.parse(b) });
            timeStamps.pop();

            

            for (var j = 0; j < timeStamps.length; j++) {
                $(".events").append("<div stamp = '" + timeStamps[j] + "' onClick = 'getDataForDynamicFrame(" + j + ")' class = 'frameViewer small' id='frame" + j + "'></div>");
            }
            getDataForDynamicFrame(0);
        }

        function getDataForDynamicFrame(i) {


            $("#myCanvas").attr("width", DYNAMIC_DATA.Table.Width)
                     .attr("height", DYNAMIC_DATA.Table.Height);

            draw(0, 0, DYNAMIC_DATA.Table.Width, DYNAMIC_DATA.Table.Height, "white");

            var frameTimeInMS = Date.parse($("#frame" + i).attr("stamp"));


            $("#stats").html("Время: " + $("#frame" + i).attr("stamp"));

            for (var j = 0; j < DYNAMIC_DATA.coas.length; j++) {
                var coa = DYNAMIC_DATA.coas[j];
                var start = Date.parse(coa.Start);
                var finish = Date.parse(coa.Finish);

                if (start <= frameTimeInMS && frameTimeInMS < finish) {
                    draw(coa.Left, coa.Top, coa.Width, coa.Height, getColor(finish - start));
                }
            }

        }

        function getColor(timeSpan) {
            var colorArray = ["#8BD7FF", "#D5B4FF", "#99FFC6", "#FFFDBB", "#FFBA93", "#FF9D9C", "#BDFFEE"];
            var sampleDate = new Date();
            var minMinutes = 40;
            var maxMinutes = 400;



            var minDate = addMinutes(sampleDate, minMinutes).valueOf();
            console.log(minDate);
            var maxDate = addMinutes(sampleDate, maxMinutes).valueOf();
            console.log(maxDate);
            var ourDate = new Date((new Date()).getTime() + timeSpan).valueOf();
            console.log(ourDate);

            var colorTune = Math.floor((ourDate - minDate) / (maxDate - minDate) * (maxMinutes - minMinutes));
            console.log(colorTune);
            var step = Math.floor((maxMinutes - minMinutes) / colorArray.length);
            console.log(step);

            for (var i in colorArray) {
                var estimatedValue = minMinutes + step * i;
                console.log(estimatedValue);
                if (colorTune < estimatedValue) {
                    console.log("success! " + estimatedValue);
                    return colorArray[i];
                } 
            }

            return colorArray[i];
        }

        function addMinutes(date, minutes) {
            return new Date(date.getTime() + minutes * 60000);
        }
    </script>
</asp:Content>

function newAxis(_name, _color, _data, _type, _marker)
{
	console.log(_data);
	var chartsWithName = ["pie", "column"];
	var obj = {
		type: _type,
		showInLegend: true,
		name: _name,
		markerType: _marker,
		//xValueFormatString: "DD MMM, YYYY",
		color: _color,
		dataPoints: []
	};
	for (var i = 0; i < _data.length; i++) {
		_name = chartsWithName.indexOf(_type) != -1 ? _data[i].title : null;
		var point = {
			x: _data[i].x,
			y: _data[i].y,
			name: _name,
			indexLabel: _name,
		}; 
		obj.dataPoints.push(point);
	}
	return obj;
}

function newChart(id, Name, XName, YName, markerType, dataSeries)
{
	var data = [];
	for (var i = 0; i < dataSeries.length; i++)
		data.push(dataSeries[i]);

	var options = {
		zoomEnabled: true,
		animationEnabled: true,
		markerType: markerType,
		title: { text: Name },
		axisX: { title: XName, crosshair: { enabled: true, snapToDataPoint: true } },
		axisY: { title: YName, includeZero: true, crosshair: { enabled: true } },
		data: data  
	};

	return new CanvasJS.Chart(id, options);
}

function renderChart(chart, chartConstructor) {
	if (chart != null)
		chart.destroy();

	chart = chartConstructor();
	chart.render();
}

function filtersToStrObj(filterFrom, filterTo) {
	return {
		from: filterFrom.value.toString(),
		to: filterTo.value.toString()
	}
}

function EnsureFiltersChanged(filterFrom, filterTo) {
	var filterDate = filtersToStrObj(filterFrom, filterTo),
		res = filterFrom.oldValue != filterDate.from || filterTo.oldValue != filterDate.to;

	if (res) {
		filterFrom.oldValue = filterDate.from;
		filterTo.oldValue = filterDate.to;
	}
	return res;
}

function defaultErrorHandler() {
	alert("не удалось загрузить данные");
}


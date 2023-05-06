function newAxis(_name, _color, _data)
{
	var obj = {
		type: "line",
		showInLegend: true,
		name: _name,
		markerType: "line",
		xValueFormatString: "DD MMM, YYYY",
		color: _color,
		dataPoints: []
	};
	for (var i = 0; i < _data.length; i++) {
		var point = { x: new Date(Date.parse(_data[i].x)), y: _data[i].y };
		obj.dataPoints.push(point);
	}
	return obj;
}

function newChart(id, Name, XName, YName, dataSeries)
{
	var data = [];
	for (var i = 0; i < dataSeries.length; i++)
		data.push(dataSeries[i]);

	//Better to construct options first and then pass it as a parameter
	var options = {
		zoomEnabled: true,
		animationEnabled: true,
		title: { text: Name },
		axisX: { valueFormatString: XName, crosshair: { enabled: true, snapToDataPoint: true } },
		axisY: { title: YName, includeZero: true, crosshair: { enabled: true } },
		data: data  // random data
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


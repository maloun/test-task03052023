function newChart(id, Name, XName, YName)
{
	var chart = new CanvasJS.Chart(id, {
		animationEnabled: true,
		theme: "light2", title: { text: Name },
		axisX: { valueFormatString: XName, crosshair: { enabled: true, snapToDataPoint: true } },
		axisY: { title: YName, includeZero: true, crosshair: { enabled: true } },
		/*axisZ: {includeZero: true},toolTip: {shared: true},legend: {
			cursor: "pointer",
			verticalAlign: "bottom",
			horizontalAlign: "left",
			dockInsidePlotArea: true,
			itemclick: toogleDataSeries},*/
		data: [
			{
				type: "line",
				showInLegend: true,
				name: "����������� �����",
				markerType: "square",
				xValueFormatString: "DD MMM, YYYY",
				color: "#F08080",
				dataPoints: [ /* {x: {{jsonDate(a.x)}}, y: {{jsonDouble(a.y)}}} */]
			},
			{
				type: "line",
				showInLegend: true,
				name: "�������� �����",
				lineDashType: "solid",
				dataPoints: [ /* {x: {{jsonDate(a.x)}}, y: {{jsonDouble(a.y)}}} */]
			}
		]
	});
	chart.render();
	return chart;
}
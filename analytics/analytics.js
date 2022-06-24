function plotGraph(container, title, subtitle, xCategories, yTitle, seriesData) {
	Highcharts.chart(container, {
	    chart: {
	        type: 'column'
	    },
	    title: {
	        text: title
	    },
	    subtitle: {
	        text: subtitle
	    },
	    xAxis: {
	        categories: xCategories,
	        crosshair: true
	    },
	    yAxis: {
	        min: 0,
	        title: {
	            text: yTitle
	        },
	        labels: {
	            enabled: true
        	}
	    },
	    tooltip: {
	        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
	        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
	            '<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
	        footerFormat: '</table>',
	        shared: true,
	        useHTML: true
	    },
	    plotOptions: {
	        column: {
	            pointPadding: 0.0,
	            borderWidth: 0,
	            dataLabels: {
	                enabled: true
	            }
	        }
	    },
	    series: seriesData
	});
}

function plotNumberOfAttempts() {
	var container = 'attemptsPerLevelDiv';
	var title = 'Number of attempts per level'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Attempts',
		data: [5, 7, 9, 10, 12],
		color: '#058DC7'
	}]
	var xCategories = ['Level 1', 'Level 2', 'Level 3', 'Level 4', 'Level 5']
	var yTitle = 'Attempts'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}

function levelVsCoins() {
	var container = 'coinsDiv';
	var title = 'Coins vs Level'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Total Coins',
		data: [10, 14, 19, 20, 15]
	}, {
		name: 'Coins collected',
		data: [10, 12, 15, 20, 14],
		color: '#64E572'
	}, {
		name: 'Coins spent',
		data: [1, 6, 10, 20, 14],
		color: '#FF0000'
	}]
	var xCategories = ['Level 1', 'Level 2', 'Level 3', 'Level 4', 'Level 5']
	var yTitle = 'Coins'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}

function levelVsTime() {
	var container = 'timeDiv';
	var title = 'Time vs Level'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Total time',
		data: [120, 120, 100, 90, 150],
		color: '#FF9655'
	}, {
		name: 'Time spent to complete level',
		data: [50, 70, 70, 70, 145],
		
	}]
	var xCategories = ['Level 1', 'Level 2', 'Level 3', 'Level 4', 'Level 5']
	var yTitle = 'Time(in seconds)'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}

function powerUpsPerLevel() {
	var container = 'powerUpsDiv';
	var title = 'Power-Ups per level'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Extra time',
		data: [2, 3, 2, 5, 7],
		color: '#DDDF00'
	}, {
		name: 'Shield',
		data: [1, 3, 3, 2, 3],
		color: '#FF0000'
	}, {
		name: 'Wall walk through',
		data: [1, 2, 2, 1, 1],
		color: '#64E572'
	}]
	var xCategories = ['Level 1', 'Level 2', 'Level 3', 'Level 4', 'Level 5']
	var yTitle = 'Number of power ups'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}

function obstacleCollisionGraph() {
	var container = 'obstacleDiv';
	var title = 'Obstacle collision per level'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'X 0.5',
		data: [2, 3, 2, 5, 5],
		color: '#FF9655'
	}, {
		name: 'One way door',
		data: [1, 3, 3, 2, 3],
		color: '#64E572'
	}, {
		name: 'DontKnow',
		data: [1, 2, 2, 1, 1],
		color: '#058DC7'
	}]
	var xCategories = ['Level 1', 'Level 2', 'Level 3', 'Level 4', 'Level 5']
	var yTitle = 'Number of obstacles'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}

function levelVsLoss() {
	var container = 'lossTypeDiv';
	var title = 'Level Vs Loss Scenario'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Quit level',
		data: [0, 1, 2, 4, 5],
		color: '#FF9655'
	}, {
		name: 'Lost by time',
		data: [1, 2, 3, 5, 7],
		color: '#64E572'
	}, {
		name: 'Lost by obstacle',
		data: [1, 4, 3, 8, 9],
		color: '#FF0000'
	}]
	var xCategories = ['Level 1', 'Level 2', 'Level 3', 'Level 4', 'Level 5']
	var yTitle = 'Count'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}

function startVsComplete() {
	var container = 'startVsCompleteDiv';
	var title = 'Start Vs Complete'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Start',
		data: [80, 70, 70, 50, 40],
		color: '#c42525'
	}, {
		name: 'Complete',
		data: [75, 60, 50, 30, 10],
		color: '#64E572'
		
	}]
	var xCategories = ['Level 1', 'Level 2', 'Level 3', 'Level 4', 'Level 5']
	var yTitle = 'Attempts'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}



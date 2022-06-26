var data;
var levels;
var eventsData = {};
var levelWisePlayerData;
function setData(analyticsData) {
	data = analyticsData;
}

function setLevels(analyticLevels) {
	levels = analyticLevels;
}


const groupBy = (array, key) => {
  // Return the end result
  return array.reduce((result, currentValue) => {
    // If an array already present for key, push it to the array. Else create an array and push the object
    (result[currentValue[key]] = result[currentValue[key]] || []).push(
      currentValue
    );
    // Return the current iteration `result` value, this will be taken as next iteration `result` value and accumulate
    return result;
  }, {}); // empty object is the initial value for result object
};


function processData() {
	data = data.filter(d => levels.includes(d.custom_params.level));
	var temp = groupBy(data, "name");
	for (const key in temp){
	    eventsData[key] = temp[key].map(e => e.custom_params)
	}
}

function getAvg(arr) {
	return arr.reduce((a, b) => a + b, 0) / arr.length;
}

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
	if(!levelWisePlayerData) {
		levelWisePlayerData = groupBy(eventsData.userData, "level");
	}

	var container = 'attemptsPerLevelDiv';
	var title = 'Number of attempts per level'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Attempts',
		data: Object.values(levelWisePlayerData).map(e => e.length),
		color: '#058DC7'
	}]
	var xCategories = Object.keys(levelWisePlayerData)
	var yTitle = 'Attempts'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}


function getMetricsByName(metricsByLevel, metricNames, levelNames) {
	var metricsByName = {};
	metricNames.forEach(metric => {
		var metricData = [];
		levelNames.forEach(level => {
			metricData.push(+metricsByLevel[level][metric] || 0)
		});
		metricsByName[metric] = metricData
	});
	return metricsByName;
}

function getMetricsByLevel(levelWiseData, metricNames, levelNames) {
	var metricsByLevel = {};
	for(const level in levelWiseData) {
		var levelData = {};
		metricNames.forEach(metric => {
			if(!levelData[metric]) {
				levelData[metric] = 0;
			}
			levelData[metric] = getAvg(levelWiseData[level].filter(e => e.hasOwnProperty(metric)).map(e => +e[metric]))
		});
		metricsByLevel[level] = levelData
	}
	return metricsByLevel;
}

function levelVsCoins() {

	if(!levelWisePlayerData) {
		levelWisePlayerData = groupBy(eventsData.userData, "level");
	}
	var metricNames = ["totalCoins", "coinsCollected", "coinsSpent"];
	var coinMetricsByLevel = getMetricsByLevel(levelWisePlayerData, metricNames, Object.keys(levelWisePlayerData));
	var coinMetricsByName = getMetricsByName(coinMetricsByLevel, metricNames, Object.keys(levelWisePlayerData));


	var container = 'coinsDiv';
	var title = 'Coins vs Level'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Total Coins',
		data: coinMetricsByName['totalCoins']
	}, {
		name: 'Coins collected',
		data: coinMetricsByName['coinsCollected'],
		color: '#64E572'
	}, {
		name: 'Coins spent',
		data: coinMetricsByName['coinsSpent'],
		color: '#FF0000'
	}]
	var xCategories = Object.keys(levelWisePlayerData)
	var yTitle = 'Coins'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}

function levelVsTime() {
	if(!levelWisePlayerData) {
		levelWisePlayerData = groupBy(eventsData.userData, "level");
	}

	var metricsByLevel = getMetricsByLevel(levelWisePlayerData, ['totalTime'], Object.keys(levelWisePlayerData));

	for(const level in levelWisePlayerData) {
		var levelData = metricsByLevel[level];
		levelData['timeToReachTarget'] = getAvg(levelWisePlayerData[level].filter(e => e.hasOwnProperty('exitReason') && e['exitReason'] === 'won').map(e => +e['timeToReachTarget']))
		metricsByLevel[level] = levelData
	}
	
	var metricsByName = getMetricsByName(metricsByLevel, ['totalTime', 'timeToReachTarget'], Object.keys(levelWisePlayerData));

	var container = 'timeDiv';
	var title = 'Time vs Level'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Total time',
		data: metricsByName['totalTime'],
		color: '#FF9655'
	}, {
		name: 'Time spent to complete level',
		data: metricsByName['timeToReachTarget'],
		
	}]
	var xCategories = Object.keys(levelWisePlayerData)
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
	var xCategories = Object.keys(levelWisePlayerData)
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
	if(!levelWisePlayerData) {
		levelWisePlayerData = groupBy(eventsData.userData, "level");
	}
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
	if(!levelWisePlayerData) {
		levelWisePlayerData = groupBy(eventsData.userData, "level");
	}
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



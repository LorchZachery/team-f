var data;
var levels;
var eventsData = {};
var levelWisePlayerData;

const groupBy = (array, key) => {
  return array.reduce((result, currentValue) => {
    (result[currentValue[key]] = result[currentValue[key]] || []).push(
      currentValue
    );
    return result;
  }, {});
};


function setData(analyticsData) {
	data = analyticsData;
	data = groupBy(data, "platform")
	data = data['WebGL']
}

function setLevels(analyticLevels) {
	levels = analyticLevels;
}





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
	        categories: xCategories.map(e => "Level " + e),
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
	                enabled: true,
	                format: '{y:.2f}'
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
	var powerUps = ['bonusTime', 'powerUpWalkThru', 'shield', 'shrink']
	var powerUpsByLevel = groupBy(eventsData.powerUpUsed, "level");
	powerUpsByLevel = getMetricsByLevel(powerUpsByLevel, powerUps, Object.keys(powerUpsByLevel));
	var powerUpsByName = getMetricsByName(powerUpsByLevel, powerUps, Object.keys(powerUpsByLevel));

	var container = 'powerUpsDiv';
	var title = 'Power-Ups per level'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Extra time',
		data: powerUpsByName['bonusTime'] || [],
		color: '#DDDF00'
	}, {
		name: 'Shield',
		data: powerUpsByName['shield'],
		color: '#FF0000'
	}, {
		name: 'Wall walk through',
		data: powerUpsByName['powerUpWalkThru'],
		color: '#64E572'
	},{
		name: 'Shrink',
		data: powerUpsByName['shrink'],
	}]
	var xCategories = Object.keys(levelWisePlayerData)
	var yTitle = 'Number of power ups'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}

function obstacleCollisionGraph() {
	var obstacles = ['X 0.5', 'spike']
	var obstaclesByLevel = groupBy(eventsData.collidedObstacles, "level");
	obstaclesByLevel = getMetricsByLevel(obstaclesByLevel, obstacles, Object.keys(obstaclesByLevel));
	var obstaclesByName = getMetricsByName(obstaclesByLevel, obstacles, Object.keys(obstaclesByLevel));

	var container = 'obstacleDiv';
	var title = 'Obstacle collision per level'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'X 0.5',
		data: obstaclesByName['X 0.5'],
		color: '#FF9655'
	}, {
		name: 'Spike',
		data: obstaclesByName['spike'],
		color: '#64E572'
	}, {
		name: 'DontKnow',
		data: [1, 2, 2, 1, 1],
		color: '#058DC7'
	}]
	var xCategories = Object.keys(obstaclesByLevel)
	var yTitle = 'Number of obstacles'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}

function levelVsLoss() {
	if(!levelWisePlayerData) {
		levelWisePlayerData = groupBy(eventsData.userData, "level");
	}

	var metricNames = ["quit", "restart", "obstacle", "Out of Time"];
	var metricsByLevel = {};
	for(const level in levelWisePlayerData) {
		metricsByLevel[level] = {}
		metricNames.forEach(metric => {
			metricsByLevel[level][metric] = levelWisePlayerData[level].filter(e => metric === e['exitReason']).length
		})
	}
	var metricsByName = getMetricsByName(metricsByLevel, metricNames, Object.keys(levelWisePlayerData));


	var container = 'lossTypeDiv';
	var title = 'Level Vs Loss Scenario'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Quit level',
		data: metricsByName['quit'],
		color: '#FF9655'
	},{
		name: 'Restart level',
		data: metricsByName['restart'],
		
	},{
		name: 'Lost by time',
		data: metricsByName['Out of Time'],
		color: '#64E572'
	}, {
		name: 'Lost by obstacle',
		data: metricsByName['obstacle'],
		color: '#FF0000'
	}]
	var xCategories = Object.keys(levelWisePlayerData)
	var yTitle = 'Count'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}

function startVsComplete() {
	if(!levelWisePlayerData) {
		levelWisePlayerData = groupBy(eventsData.userData, "level");
	}

	var metricNames = ["won"];
	var metricsByLevel = {};
	for(const level in levelWisePlayerData) {
		metricsByLevel[level] = {}
		metricNames.forEach(metric => {
			metricsByLevel[level][metric] = levelWisePlayerData[level].filter(e => metric === e['exitReason']).length
		})
	}
	var metricsByName = getMetricsByName(metricsByLevel, metricNames, Object.keys(levelWisePlayerData));

	var container = 'startVsCompleteDiv';
	var title = 'Start Vs Complete'
	var subtitle = 'blah blah blah'
	var seriesData = [{
		name: 'Start',
		data: Object.values(levelWisePlayerData).map(e => e.length),
		color: '#c42525'
	}, {
		name: 'Complete',
		data: Object.values(levelWisePlayerData).map(e => e.filter(res => "won" === res['exitReason']).length),
		color: '#64E572'
		
	}]
	var xCategories = Object.keys(levelWisePlayerData)
	var yTitle = 'Attempts'

	plotGraph(container, title, subtitle, xCategories, yTitle, seriesData);
}



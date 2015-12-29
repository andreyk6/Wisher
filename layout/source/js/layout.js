(function() {
	angular.module('whr-countdown-app', []).directive('countdown', [
		'Util', '$interval', function(Util, $interval) {
			return {
				restrict: 'A',
				scope: {
					date: '@'
				},
				link: function(scope, element) {
					var future;
					future = new Date(scope.date);
					$interval(function() {
						var diff;
						diff = Math.floor((future.getTime() - new Date().getTime()) / 1000);
						return element.html(Util.dhms(diff));
					}, 1000);
				}
			};
		}
	]).factory('Util', [
		function() {
			return {

				dhms: function(t) {
					var days, hours, minutes, seconds;
					days = Math.floor(t / 86400);
					t -= days * 86400;
					hours = Math.floor(t / 3600) % 24;
					t -= hours * 3600;
					minutes = Math.floor(t / 60) % 60;
					t -= minutes * 60;
					seconds = t % 60;
					return [
						"<div>", days, "<span>days</span></div>",
						"<div>", hours, "<span>hrs</span></div>",
						"<div>", minutes, "<span>min</span></div>",
						"<div>", seconds, "<span>sec</span></div>",
					].join('');
				}
			};
		}
	]);


	setTimeout(
		function(){
			whr.hide(preloader);
		},
		1000
	);




}).call(this);
//***************************************//
	var animateApp = angular.module('animateApp', ['ngRoute', 'ngAnimate']);

// ROUTING ===============================================
	animateApp.config(function($routeProvider) {

		$routeProvider

			// home page
			.when('/', {
				templateUrl: '../blocks/start.html',
				controller: 'mainController'
			})

			// sing-in page
			.when('/sing-in', {
				templateUrl: '../blocks/sing-in.html',
				controller: 'singInController'
			})

			// sing-up page
			.when('/sing-up', {
				templateUrl: '../blocks/sing-up.html',
				controller: 'singUpController'
			})

			// taste-definition page
			.when('/taste-definition', {
				templateUrl: '../blocks/taste-definition.html',
				controller: 'tasteDefinitionController'
			})

			// sing-up page
			.when('/result', {
				templateUrl: '../blocks/result-page.html',
				controller: 'resultPageController'
			});

	});


// CONTROLLERS ============================================
	animateApp.controller('mainController', function($scope) {
		$scope.pageClass = '';
	});

	animateApp.controller('singInController', function($scope) {
		$scope.pageClass = '';
	});

	animateApp.controller('singUpController', function($scope) {
		$scope.pageClass = '';
	});

	animateApp.controller('tasteDefinitionController', function($scope) {
		$scope.pageClass = '';
	});

	animateApp.controller('resultPageController', function($scope) {
		$scope.pageClass = '';
	});

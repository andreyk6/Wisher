(function () {
	var whr = {},
		hideClass = "hide";

	whr.fadeOut = function (element, fadeOutDuration) {
		var op = 1,
			duration = fadeOutDuration || 50;
		var timer = setInterval(function () {
			if (op <= 0.1){
				clearInterval(timer);
				element.style.display = 'none';
				element.classList.add(hideClass);
			}
			element.style.opacity = op;
			element.style.filter = 'alpha(opacity=' + op * 100 + ")";
			op -= op * 0.1;
		}, duration);
	};

	whr.fadeIn = function (element, fadeInDuration) {
		var op = 0.1,
			duration = fadeInDuration|| 10;
		element.classList.remove(hideClass);
		element.style.display = 'block';
		var timer = setInterval(function () {
			if (op >= 1){
				clearInterval(timer);
			}
			element.style.opacity = op;
			element.style.filter = 'alpha(opacity=' + op * 100 + ")";
			op += op * 0.1;
		}, duration);
	};

	whr.hide = function(element){
		element.classList.add(hideClass);
	};

	whr.show = function(element){
		element.classList.remove(hideClass);
	};



	var preloader = document.getElementById('preloader');

	whr.hide(preloader)
	// 	$countdown = $('#countdown');

	fullpage.init = init();

	fullpage.initialize("#pageSlider", {
		// 'anchors': ['section1', 'section2', 'section3'],
		'sectionSelector': '.page-section',
		'slideSelector': '.page-slide',
		'keyboardScrolling': false,
		'navigation': false,
		'recordHistory': false,
		'slidesNavigation': false,
		'controlArrows': false,
		'loopHorizontal': false,
		'afterLoad': function(i){
			console.log('afterLoad', i);
		},
		'onLeave': function(index, nextIndex, direction){
			console.log('onLeave', index, nextIndex, direction);
		},
		'afterRender': function(i){
			console.log('afterRender', i);
		},
		'afterResize': function(i){
			console.log('afterResize', i);
		},
		'afterReBuild': function(i){
			console.log('afterReBuild', i);
		},
		'afterSlideLoad': function(i){
			console.log('afterSlideLoad', i);
		},
		'onSlideLeave': function(anchorLink, index, slideIndex, direction, nextSlideIndex){
			console.log('onSlideLeave', anchorLink, index, slideIndex, direction, nextSlideIndex);
		}	
	});

	// fullpage.removeTouchHandler();
	fullpage.setKeyboardScrolling(false, 'all');
	// fullpage.setAllowScrolling(false, 'all');
	fullpage.setRecordHistory(false);

	// var onDomReady = function(){



	// 	$countdown.countdown({
	// 		date: "January 01, 2016 00:00:00",
	// 		render: function(data) {
	// 			var el = $(this.el);
	// 			el.empty()
	// 			.append("<div>" + this.leadingZeros(data.days, 2) + " <span>days</span></div>")
	// 			.append("<div>" + this.leadingZeros(data.hours, 2) + " <span>hrs</span></div>")
	// 			.append("<div>" + this.leadingZeros(data.min, 2) + " <span>min</span></div>")
	// 			.append("<div>" + this.leadingZeros(data.sec, 2) + " <span>sec</span></div>");
	// 		}
	// 	});

	// 	$preloader.hide();
	// };

	// onDomReady();
})();
(function () {
	var $preloader = $('#preloader'),
		$pageSlider = $('#pageSlider'),
		$countdown = $('#countdown');

	var onDomReady = function(){

		$pageSlider.fullpage({
			sectionSelector: '.page-section',
			slideSelector: '.page-slide',
			slidesNavigation: false,
			controlArrows: false,
			scrollOverflow: true,
			paddingTop: '81px'
		});

		// $.fn.fullpage.setKeyboardScrolling(false, 'all');
		// $.fn.fullpage.setAllowScrolling(false, 'all');
		// $.fn.fullpage.setRecordHistory(false);

		$countdown.countdown({
			date: "January 01, 2016 00:00:00",
			render: function(data) {
				var el = $(this.el);
				el.empty()
				.append("<div>" + this.leadingZeros(data.days, 2) + " <span>days</span></div>")
				.append("<div>" + this.leadingZeros(data.hours, 2) + " <span>hrs</span></div>")
				.append("<div>" + this.leadingZeros(data.min, 2) + " <span>min</span></div>")
				.append("<div>" + this.leadingZeros(data.sec, 2) + " <span>sec</span></div>");
			}
		});

		$preloader.hide();
	};

	onDomReady();
})();
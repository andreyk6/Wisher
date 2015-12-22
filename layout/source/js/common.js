var whr = {},
	hideClass = "hide";

var preloader = document.getElementById('preloader');

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




!function(){var n=$("#preloader"),e=$("#pageSlider"),a=$("#countdown"),d=function(){e.fullpage({sectionSelector:".slide-page"}),a.countdown({date:"January 01, 2016 00:00:00",render:function(n){var e=$(this.el);e.empty().append("<div>"+this.leadingZeros(n.days,2)+" <span>days</span></div>").append("<div>"+this.leadingZeros(n.hours,2)+" <span>hrs</span></div>").append("<div>"+this.leadingZeros(n.min,2)+" <span>min</span></div>").append("<div>"+this.leadingZeros(n.sec,2)+" <span>sec</span></div>")}}),n.hide()};d()}();
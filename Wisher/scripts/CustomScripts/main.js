$(document).ready(function () {

    checkAuthorization();

    initializeListeners();

    var wisherRouter = new WisherRouter();

    $("img "[name='category']).on("click", choosedCategory);
    $("#startButton").on("click", function(){
        $.fn.fullpage.moveSectionDown();
    });

    $("#goToSignUp").on("click", function(){
        $.fn.fullpage.moveSlideRight();
    });
});
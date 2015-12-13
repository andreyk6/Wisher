$(document).ready(function () {

    checkAuthorization();

    initializeListeners();

    var wisherRouter = new WisherRouter();

    $("img "[name='category']).on("click", choosedCategory);

});
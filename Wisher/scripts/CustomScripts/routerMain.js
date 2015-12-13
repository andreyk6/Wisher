
        var WisherRouter = Backbone.Router.extend({
        routes:{

            "tastify": "chooseCategory",
            "userProfile": "userProfile"
        },

        chooseCategory: function() {
            //initialize here all
            "use strict";

            choosedCategory();
        },

        initialize: function(){
            "use strict";
            console.log("wisher router has been initialized");
        },

        userProfile: function(){
            $("#progressBar").fadeOut();
            $("#firstImg").remove();
            $("secondImg").remove();

            //рендеринг инфы и связь с социалками.
        }
    });


    function choosedCategory() {
        $("#firstImgWrapper").fadeOut();
        $("#secondImgWrapper").fadeOut();
        $("#taste-definition").addClass("loader-bg");
        var categoryId = $(this).attr('data-id');
        if (categoryId === undefined) {
            categoryId = -1;
        }
        // var fromLocal = JSON.parse(localStorage["user"]);
        //$.ajax({
        //    type: "POST",
        //    url: serverName + '',
        //    headers: {'idUser': fromLocal.idUser},
        //    beforeSend: function (xhr) {
        //        var token = fromLocal.secret;
        //        xhr.setRequestHeader("Authorization", "Bearer " + token);
        //    },
        //    data: {"UserId": authInfo.idUser, "TrueCategoryId": "", "FalseCategoryId": categoryId},
        //    dataType: "json",
        //    statusCode: {
        //        200: function (data, statusText, xhr) {
        //            renderResponse(data);
        //        },
        //        404: function (data, statusText, error) {
        //            alert('Запрашиваемая Вами страница не существует!')
        //        },
        //        400: function (data, statusText, error) {
        //
        //        }
        //    }

        //})
        $.ajax({
            type: "GET",
            url: 'js/mock.json',
            dataType: "json",
            statusCode: {
                200: function (data, statusText, xhr) {
                    renderResponse(data);
                },
                404: function (data, statusText, error) {
                    alert('Запрашиваемая Вами страница не существует!');
                },
                400: function (data, statusText, error) {

                }
            }

        });
    }

    function renderResponse(data){
        "use strict";

        if( data.progress === 100) {

            $.fn.fullpage.moveSlideLeft();
            location.hash = "#userAccount";
            return;
        }
        $("#progressBar").show(function(){
            $(this).find(".progress-bar__inner").css("height", data.progress+"%");
        });
        var firstImg = $("#firstImg");
        var secondImg = $("secondImg");

        var firstImgCategoryName = data.cat1_name;
        var secondImgCategoryName = data.cat2_name;
        var firstImgSrc = getPictureFromGoogle(firstImgCategoryName);
        var secondImgSrc = getPictureFromGoogle(secondImgCategoryName);

        var firstImgId = data.cat1_id;
        var secondImgId = data.cat2_id;

        firstImg.attr({
            "src" : firstImgSrc,
            "data-id" : secondImgId
        });

        secondImg.attr({
            "src" : secondImgSrc,
            "data-id" : firstImgId
        });

        $("#firstImgCategoryName").text(firstImgCategoryName);
        $("#secondImgCategoryName").text(firstImgCategoryName);
        $("#taste-definition").removeClass("loader-bg");
        $("#firstImgWrapper").fadeIn();
        $("#secondImgWrapper").fadeIn();

    }
    function getPictureFromGoogle(categoryName){
        $.ajax({
            url: 'https://ajax.googleapis.com/ajax/services/search/images?v=1.0&q='+categoryName+"&as_filetype=jpg",
            dataType: "json",
            statusCode: {
                200: function (data, statusText, xhr) {
                    alert(data);
                },
                404: function (data, statusText, error) {
                    alert(data);
                },
                400: function (data, statusText, error) {
                    alert(data);
                }
            },

        });
        var img = null;
        return img;
    }

//$(".taste-definition__taste-image-wrap img").on("click", choosedCategory());



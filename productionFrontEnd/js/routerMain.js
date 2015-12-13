
// ROUTER

var WisherRouter = Backbone.Router.extend({
    routes:{

        "#tastify/:progressBarValue": "chooseCategory",
        "#userProfile": "userProfile"
    },

    chooseCategory: function(progressBarValue) {
        $.ajax({
            type: "POST",
            url: serverName + '',
            headers: {'idUser': fromLocal.idUser},
            beforeSend: function (xhr) {
                var token = fromLocal.secret;
                xhr.setRequestHeader("Authorization", "Bearer " + token);
            },
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({

            }),


    }),

        //modal.onClose();{}
        // $.fn.page.moveTo(...);
        location.hash = "#userAccount";

    }


});

checkProgressBar();
function choosedCategory() {
    var categoryId = $(this).attr('data-id');
    if (categoryId = undefined) {
        categoryId = -1;
    }
    $.ajax({
        type: "POST",
        url: serverName + '',
        headers: {'idUser': fromLocal.idUser},
        beforeSend: function (xhr) {
            var token = fromLocal.secret;
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        data: {"UserId": localStorage.getItem("User").idUser, "TrueCategoryId": "", "FalseCategoryId": categoryId},
        statusCode: {
            200: function (data, statusText, xhr) {
                //(data);
            },
            404: function (data, statusText, error) {
                alert('Запрашиваемая Вами страница не существует!:(')
            },
            400: function (data, statusText, error) {
                //json
                alert("Пароль должен быть не менее 6 символов");
            }
        }

    })

}

function getPictureFromGoogle(categoryName){
    var img = null;
    return img;
};


    $(document).ready(function () {
        window.serverName = "http://wisher.azurewebsites.net/";
        var $preloader = $('#preloader'),
            $pageSlider = $('#pageSlider'),
            $countdown = $('#countdown');


            $pageSlider.fullpage({
                sectionSelector: '.page-section',
                slideSelector: '.page-slide',
                slidesNavigation: false,
                controlArrows: false,
                scrollOverflow: true,
                paddingTop: '81px'
            });

            $.fn.fullpage.setKeyboardScrolling(false, 'all');
            $.fn.fullpage.setAllowScrolling(false, 'all');
            $.fn.fullpage.setRecordHistory(false);

            $countdown.countdown({
                date: "January 01, 2016 00:00:00",
                render: function (data) {
                    var el = $(this.el);
                    el.empty()
                        .append("<div>" + this.leadingZeros(data.days, 2) + " <span>days</span></div>")
                        .append("<div>" + this.leadingZeros(data.hours, 2) + " <span>hrs</span></div>")
                        .append("<div>" + this.leadingZeros(data.min, 2) + " <span>min</span></div>")
                        .append("<div>" + this.leadingZeros(data.sec, 2) + " <span>sec</span></div>");
                }
            });

            $preloader.hide();

        var WisherRouter = Backbone.Router.extend({
            routes:{

                "tastify": "chooseCategory",
                "userProfile": "userProfile"
            },

            chooseCategory: function() {
                //initialize here all
                "use strict";
                checkAuthorization();
                choosedCategory();
            },

            initialize: function(){
                "use strict";
                console.log("wisher router has been initialized");
            },

            userProfile: function () {
                checkAuthorization();
                $("#progressBar").fadeOut();
                $("#firstImgWrapper").remove();
                $("secondImgWrapper").remove();

                //рендеринг инфы и связь с социалками.
            }
        });


        function choosedCategory() {
            $("#firstImgWrapper").fadeOut();
            $("#secondImgWrapper").fadeOut();
            $("#taste-definition").addClass("loader-bg");
            var categoryId = $(this).find("img").attr('data-id');
            if (categoryId === undefined) {
                categoryId = -1;
            }
            var fromLocal = JSON.parse(localStorage.getItem("user"));
            $.ajax({
                type: "POST",
                url: serverName + 'api/Wish/MakeWish',
                //headers: { 'idUser': fromLocal.idUser },
                beforeSend: function(xhr) {
                    var token = fromLocal.secret;
                    xhr.setRequestHeader("Authorization", "Bearer " + token);
                },
                data: { "UserId": fromLocal.idUser, "TrueCategoryId": " ", "FalseCategoryId": categoryId },
                dataType: "json",
                statusCode: {
                    200: function(data, statusText, xhr) {
                        renderResponse(data);
                    },
                    404: function(data, statusText, error) {
                        //  alert('Запрашиваемая Вами страница не существует!')
                    },
                    400: function(data, statusText, error) {

                    }
                }
            });
        }

        $(".mock").on("click", choosedCategory);
        function renderResponse(data){
            "use strict";

            if( data.progress === 100) {

                $.fn.fullpage.moveSlideRight();
                location.hash = "#userAccount";
                return;
            }
            $("#progressBar").show(function(){
                $(this).find(".progress-bar__inner").css("height", data.progress+"%");
            });
            var firstImg = $("#firstImg");
            var secondImg = $("#secondImg");

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
            $("#secondImgCategoryName").text(secondImgCategoryName);
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



        /*
         * Check user's signIn
         */

        function checkAuthorization() {
            "use strict";
            if (localStorage.getItem('user')) {
                var fromLocal = JSON.parse(localStorage.getItem('user'));
                $.ajax({
                    type: "GET",
                    url: serverName + 'api/accounts/auth',
                    headers: {'idUser': fromLocal.idUser},
                    beforeSend: function (xhr) {
                        var token = fromLocal.secret;
                        xhr.setRequestHeader("Authorization", "Bearer " + token);
                    },
                    success: function (data, statusText, xhr) {
                        
                    },
                    error: function (xhr, textStatus, error) {
                        
                    }
                });
            }
        }


        /*
         * Регистрация пользователя
         *
         * @param dataTransferObject - temporary object to store data;
         */
        function signUp(dataTransferObject) {
            "use strict";
            $.ajax({
                url: serverName + 'api/accounts/create',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    "email": dataTransferObject.email,
                    "Name": dataTransferObject.fullName,
                    "password": dataTransferObject.password,
                    "confirmPassword": dataTransferObject.confirmPassword,
                    "gender": dataTransferObject.gender,
                    "age": dataTransferObject.age
                }),
                statusCode: {
                    200: function (data, statusText, xhr) {
                       // alert("SignUpSuccess");
                        signInAdapter(dataTransferObject);
                    },
                    404: function (data, statusText, error) {
                      //  alert("Not found!");
                    },
                    400: function (data, statusText, error) {
                        //json
                      //  alert("At least 6 symbols");
                    }
                }
            });

        }

        /*
         * User SignIn
         *
         * @param obj: response with validation
         */

        function signIn(obj) {

            $.ajax({
                url: serverName + 'oauth/token',
                type: "PUT",
                data: {"userName": obj.email, "password": obj.password, "grant_type": "password"},
                statusCode: {
                    200: function (data, statusText, xhr) {
                        var values = xhr.getResponseHeader('Keys');
                        localStorage['user'] = JSON.stringify({"idUser": values, "secret": data.access_token});

                        location.hash = "#tastify";
                        $.fn.fullpage.silentMoveTo(3, 1);
                    },
                    400: function (data, statusText, error) {
                     //   alert("Что-то пошло не так, повторите отправку.");

                    },
                    404: function (data, statusText, error) {
                      //  alert("Запрашиваемая Вами страница не найдена!");
                    }
                }
            });
            location.hash = "#tastify";
                        $.fn.fullpage.silentMoveTo(3, 1);
        }

        function signInAdapter(dataTransferObject) {
            var signInObject = {};
            signInObject.email = dataTransferObject.email;
            signInObject.password = dataTransferObject.password;
            signIn(signInObject);
        }


        function passwordConfirmation() {
            var formLine = $(this).parents(".form-line");
            if ($('#signUpPasswordSecure').val() !== $('#signUpConfirmPassword').val()) {
                formLine.addClass("error");
                $("#signUpPasswordErrorNotification").text("Пароль и подтверждение должны совпадать");
                return;

            }
            formLine.removeClass("error");
            $("#signUpPasswordNameErrorNotification").text(" ");

        };

        function passwordValidation() {
            "use strict";
            var formLine = $(this).parents(".form-line");
            if( $("#signUpPasswordSecure").val().length < 6){
                formLine.addClass("error");
                $("#signUpPasswordErrorNotification").text("Пароль должен быть не менее 6 символов");
                return;
            }
            formLine.removeClass("error");
            $("#signUpPasswordNameErrorNotification").text(" ");
        };

        function signInLoginNotNull(){
            var formLine = $(this).parents(".form-line");
            if( $("#signInEmail").val().length === 0){
                formLine.addClass("error");
                $("#signInEmailErrorNotification").text("Поле не может быть пустым");
                return;
            }

            formLine.removeClass("error");
            $("#signInEmailErrorNotification").text(" ");

        }

        function signInPasswordNotNull(){
            var formLine = $(this).parents(".form-line");
            if( $("#signInPassword").val().length == 0){
                formLine.addClass("error");
                $("#signInPasswordErrorNotification").text("Поле не может быть пустым");

            }
            formLine.removeClass("error");
            $("#signInPasswordErrorNotification").text(" ");

        }


        checkAuthorization();

        $("#signUpBtn").on("click", function(){

            var signUpInfo = {};
            signUpInfo.email = $("#signUpEmail").val();
            signUpInfo.fullName = $("#signUpFullName").val();
            signUpInfo.password = $("#signUpPasswordSecure").val();
            signUpInfo.confirmPassword = $("#signUpConfirmPassword").val();
            signUpInfo.age = $("#signUpAge").val();
            signUpInfo.gender = $("input:radio[name=gender]:checked").val();
            signUp(signUpInfo);

        });

        $("#signInBtn").on("click", function(){


            var signInInfo = {};
            signInInfo.email = $("#signInEmail").val();
            signInInfo.password = $("#signInPassword").val();
            signIn(signInInfo);

        });

        $("#signUpPasswordSecure").on("blur", function(){
            passwordValidation();
        });

        $("#signUpConfirmPassword").on("blur", function(){
            passwordConfirmation();
        });

        //var wisherRouter = new globalFunc.wisher();
        var wisherRouter = new WisherRouter();
        Backbone.history.start();
        $("img "[name = "category"]).on("click", choosedCategory);
        $("#startButton").on("click", function () {
            $.fn.fullpage.moveSectionDown();
        });

        $("#goToSignUp").on("click", function () {
            $.fn.fullpage.moveSlideRight();
        });
    });




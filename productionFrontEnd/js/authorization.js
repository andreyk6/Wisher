window.serverName = "http://wisher.azurewebsites.net/";
/**
 * Created by 23 on 12.12.2015.
 */
/*
 * Check user's signIn
 */

function checkAuthorization() {
    if (localStorage['user']) {
        var fromLocal = JSON.parse(localStorage['user']);

        $.ajax({
            type: "GET",
            url: serverName + 'auth',
            headers: {'idUser': fromLocal.idUser},
            beforeSend: function (xhr) {
                var token = fromLocal.secret;
                xhr.setRequestHeader("Authorization", "Bearer " + token);
            },
            success: function (data, statusText, xhr) {
                window.location = '#/home/';
            },
            error: function (xhr, textStatus, error) {
                window.location = '#/signIn';
            }
        });
    }
};


/*
 * Регистрация пользователя
 *
 * @param dataTransferObject - temporary object to store data;
 */
function signUp(dataTransferObject) {
    $.ajax({
        url: serverName + "api/accounts/create",
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify({
            "Name": dataTransferObject.fullName,
            "email": dataTransferObject.email,
            "password": dataTransferObject.password,
            "confirmPassword": dataTransferObject.confirmPassword,
            "gender": dataTransferObject.gender,
            "age": dataTransferObject.age
        }),
        statusCode: {
            200: function (data, statusText, xhr) {
                autoSignIn(dataTransferObject);
            },
            404: function (data, statusText, error) {
                alert('Запрашиваемая Вами страница не существует!:(')
            },
            400: function (data, statusText, error) {
                //json
                alert("Пароль должен быть не менее 6 символов");
            }
        }
    });
};

/*
 * User SignIn
 *
 * @param obj: response with validation
 */

function signIn(obj) {
    $(".infoAuth").empty();
    $.ajax({
        url: serverName + 'oauth/token',
        type: "PUT",
        data: {"userName": obj.email, "password": obj.password, "grant_type": "password"},
        statusCode: {
            200: function (data, statusText, xhr) {
                var values = xhr.getResponseHeader('Keys');
                localStorage['user'] = JSON.stringify({"idUser": values, "secret": data.access_token});

                location.hash = "#tastify";
            },
            400: function (data, statusText, error) {
                alert("Что-то пошло не так, повторите отправку.")

            },
            404: function (data, statusText, error) {
                $("#error-server").html("Запрашиваемая Вами страница не найдена!")
            }
        }
    });
};

function signInAdapter(dataTransferObject) {
    var signInObject = {};
    signInObject.email = dataTransferObject.email;
    signInObject.password = dataTransferObject.password;

    signIn(signInObject);
};


function passwordConfirmation() {
    if ($('#signUpPasswordTrue').val() !== $('#signUpConfirmation').val()) {
        $('#signUpPasswordConfirmation_error').html('password and confirmation must be equals');

    } else {
        $('#signUpPasswordConfirmation_error').html('');
    }
};

function passwordValidation() {

    if ($("#signUpPasswordTrue").val().length < 6) {
        $("#signUpPassword_error").html("Password must be at least 6 characters");
    } else {
        $("#signUpPassword_error").html("");
    }
};


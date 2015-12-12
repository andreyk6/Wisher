
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
            url: serverName+'auth',
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
 * @param obj: объект пришедший с валидной формы регистрации
 */
function signUp(obj) {
    $.ajax({
         url: serverName+"api/accounts/create",
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify({
            "Name": obj.fullName,
            "email": obj.email,
            "password": obj.password,
            "confirmPassword": obj.confirmPassword
        }),
        statusCode: {
            200: function (data, statusText, xhr) {
                //TODO добавить реакцию на успех.
                alert('bingo');
            },
            404: function (data, statusText, error) {
              alert(error);
            },
            400: function (data, statusText, error) {
               //json
                alert(error);
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
    var tokenKey = "tokenInfo";
    var userId = "userId";
    $(".infoAuth").empty();
    $.ajax({
        url: serverName+'oauth/token',
        type: "PUT",
        data: {"userName": obj.email, "password": obj.password, "grant_type": "password"},
        statusCode: {
            200: function (data, statusText, xhr) {
                var values = xhr.getResponseHeader('Keys');
                localStorage['user'] = JSON.stringify({"idUser": values, "secret": data.access_token});

                window.location = '/in/';
            },
            400: function (data, statusText, error) {
                //change later to concrete id or class.
                alert(error);
            },
            404: function (data, statusText, error) {
                alert(error);
            }
        }
    });
};

function signUpPasswordValidation() {
    if ($('#signUpPasswordTrue').val() !== $('#signUpConfirmPassword').val()) {
        $('#signUpPassword_error').html('password and confirmation must be equals');
        return false;
    } else {
        $('#signInPassword_error').html('');
        return true;
    }
};

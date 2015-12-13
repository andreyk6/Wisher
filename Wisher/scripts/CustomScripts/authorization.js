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
 * ����������� ������������
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
                signInAdapter(dataTransferObject);
            },
            404: function (data, statusText, error) {
                alert('������������� ���� �������� �� ����������!:(')
            },
            400: function (data, statusText, error) {
                //json
                alert("������ ������ ���� �� ����� 6 ��������");
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
                alert("���-�� ����� �� ���, ��������� ��������.")

            },
            404: function (data, statusText, error) {
                $("#error-server").html("������������� ���� �������� �� �������!")
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
    var formLine = $(this).parents(".form-line");
    if ($('#signUpPasswordSecure').val() !== $('#signUpConfirmPassword').val()) {
        formLine.addClass("error");
        $("#signUpPasswordErrorNotification").text("������ � ������������� ������ ���������");


    } else {
        formLine.removeClass("error");
        $("#signUpPasswordNameErrorNotification").text(" ");

    }
};

function passwordValidation() {
    var formLine = $(this).parents(".form-line");
    if( $("#signUpPasswordSecure").val().length < 6){
        formLine.addClass("error");
        $("#signUpPasswordErrorNotification").text("������ ������ ���� �� ����� 6 ��������");

    } else{
        formLine.removeClass("error");
        $("#signUpPasswordNameErrorNotification").text(" ");

    }
};

function signInLoginNotNull(){
    var formLine = $(this).parents(".form-line");
    if( $("#signInEmail").val().length == 0){
        formLine.addClass("error");
        $("#signInEmailErrorNotification").text("���� �� ����� ���� ������");

    } else{
        formLine.removeClass("error");
        $("#signInEmailErrorNotification").text(" ");

    }

}

function signInPasswordNotNull(){
    var formLine = $(this).parents(".form-line");
    if( $("#signInPassword").val().length == 0){
        formLine.addClass("error");
        $("#signInPasswordErrorNotification").text("���� �� ����� ���� ������");

    } else{
        formLine.removeClass("error");
        $("#signInPasswordErrorNotification").text(" ");

    }

}
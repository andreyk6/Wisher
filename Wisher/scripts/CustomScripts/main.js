$(document).ready(function () {

    checkAuthorization();

    $("#signInBtn").on("click", function (e) {
        var signInInfo = {};
        signInInfo.email = $("#signInEmail").val();
        signInInfo.password = $("#signInPassword").val();
        alert(JSON.stringify(signInInfo));
        signIn(signInInfo);


    });

    $("#signUpBtn").on("click", function () {

        var signUpInfo = {};
        signUpInfo.email = $("#signUpEmail").val();
        signUpInfo.fullName = $("#signUpFullName").val();
        signUpInfo.password = $("#signUpPasswordTrue").val();
        signUpInfo.confirmPassword = $("#signUpConfirmation").val();
        signUp(signUpInfo);
    });

    $("#signUpPasswordTrue").on("blur", function () {
        if ($("#signUpPasswordTrue").val().length < 6) {
            $("#signUpPassword_error").html("Password must be at least 6 characters");
        } else {
            $("#signUpPassword_error").html("");
        }

    });

    $("#signUpConfirmation").on("blur", function () {

        if ($('#signUpPasswordTrue').val() !== $('#signUpConfirmation').val()) {
            $('#signUpPasswordConfirmation_error').html('password and confirmation must be equals');

        } else {
            $('#signUpPasswordConfirmation_error').html('');
        }


    });

});
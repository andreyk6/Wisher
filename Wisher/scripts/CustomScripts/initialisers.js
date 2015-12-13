function initializeListeners(){
    $("#signUpBtn").on("click", function(){

        var signUpInfo = {};
        signUpInfo.email = $("#signUpEmail").val();
        signUpInfo.fullName = $("#signUpFullName").val();
        signUpInfo.password = $("#signUpPassword").val();
        signUpInfo.confirmPassword = $("#signUpConfirmPassword").val();
        signUpInfo.age = $("#signUpAge").val();
        signUpInfo.gender = $("input:radio[name=gender]:checked").val();
        alert("что-то не так")
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
        passwordValidation();
    });
}

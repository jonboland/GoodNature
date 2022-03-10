$(function () {

    var userLoginButton = $("#userLoginModal button[name='login']").click(onUserLoginClick);

    function onUserLoginClick() {

        var url = "UserAuth/Login";

        var antiForgeryToken = $("#userLoginModal input[name='__RequestVerificationToken']").val();

        var email = $("#userLoginModal input[name='Email']").val();
        var password = $("#userLoginModal input[name='Password']").val();
        var rememberMe = $("#userLoginModal input[name='RememberMe']").prop('checked');

        var userInput = {
            __RequestVerificationToken: antiForgeryToken,
            Email: email,
            Password: password,
            RememberMe: rememberMe
        };

        $.ajax({

            type: "POST",
            url: url,
            data: userInput,
            success: function (data) {

                var parsed = $.parseHTML(data);

                var hasErrors = $(parsed).find("input[name='LoginInvalid']").val() == "true";

                if (hasErrors == true) {

                    $("#userLoginModal").html(data);

                    userLoginButton = $("#userLoginModal button[name='login']").click(onUserLoginClick);
                }
                else {
                    location.href = "Home/Index";
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
});

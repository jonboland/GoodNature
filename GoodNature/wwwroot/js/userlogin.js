$(function () {

    let userLoginButton = $("#userLoginModal button[name='login']").click(onUserLoginClick);

    function onUserLoginClick() {

        const url = "UserAuth/Login";

        const antiForgeryToken = $("#userLoginModal input[name='__RequestVerificationToken']").val();

        const email = $("#userLoginModal input[name='Email']").val();
        const password = $("#userLoginModal input[name='Password']").val();
        const rememberMe = $("#userLoginModal input[name='RememberMe']").prop('checked');

        const userInput = {
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

                const parsed = $.parseHTML(data);

                const hasErrors = $(parsed).find("input[name='LoginInvalid']").val() == "true";

                if (hasErrors == true) {

                    $("#userLoginModal").html(data);

                    userLoginButton = $("#userLoginModal button[name='login']").click(onUserLoginClick);
                }
                else {
                    location.href = "Home/Index";
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                DisplayClosableBootstrapAlert(
                    "#alertPlaceholderLogin",
                    "danger",
                    "Error!",
                    `Status: ${xhr.status} - ${xhr.statusText}`);

                console.error(`${thrownError}\r\n${xhr.statusText}\r\n${xhr.responseText}`);
            }
        });
    }
});

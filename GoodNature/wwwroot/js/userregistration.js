$(function () {

    $("#userRegistrationModal input[name = 'AcceptUserAgreement']").click(onAcceptUserAgreementClick);

    $("#userRegistrationModal button[name = 'register']").prop("disabled", true);

    function onAcceptUserAgreementClick() {
        if ($(this).is(":checked")) {
            $("#userRegistrationModal button[name = 'register']").prop("disabled", false);
        }
        else {
            $("#userRegistrationModal button[name = 'register']").prop("disabled", true);
        }
    }

    $("#userRegistrationModal input[name = 'Email']").blur(function () {

        const email = $("#userRegistrationModal input[name = 'Email']").val();

        const url = "UserAuth/UserNameExists?userName=" + email;

        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                if (data == true) {
                    DisplayClosableBootstrapAlert(
                        "#alertPlaceholderRegister",
                        "warning",
                        "Email Error",
                        "This email address has already been registered");
                }
                else {
                    CloseAlert("#alertPlaceholderRegister");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                DisplayClosableBootstrapAlert(
                    "#alertPlaceholderRegister",
                    "danger",
                    "Error!",
                    `Status: ${xhr.status} - ${xhr.statusText}`);

                console.error(`${thrownError}\r\n${xhr.statusText}\r\n${xhr.responseText}`);
            }
        });
    });

    let registerUserButton = $("#userRegistrationModal button[name = 'register']").click(onUserRegisterClick);

    function onUserRegisterClick() {

        const url = "UserAuth/RegisterUser";

        const antiForgeryToken = $("#userRegistrationModal input[name='__RequestVerificationToken']").val();

        const email = $("#userRegistrationModal input[name='Email']").val();
        const password = $("#userRegistrationModal input[name='Password']").val();
        const confirmPassword = $("#userRegistrationModal input[name='ConfirmPassword']").val();
        const firstName = $("#userRegistrationModal input[name='FirstName']").val();
        const lastName = $("#userRegistrationModal input[name='LastName']").val();
        const address1 = $("#userRegistrationModal input[name='Address1']").val();
        const address2 = $("#userRegistrationModal input[name='Address2']").val();
        const postcode = $("#userRegistrationModal input[name='Postcode']").val();
        const phoneNumber = $("#userRegistrationModal input[name='PhoneNumber']").val();

        const user = {
            __RequestVerificationToken: antiForgeryToken,
            Email: email,
            Password: password,
            ConfirmPassword: confirmPassword,
            FirstName: firstName,
            LastName: lastName,
            Address1: address1,
            Address2: address2,
            PostCode: postcode,
            PhoneNumber: phoneNumber,
            AcceptUserAgreement: true,
        };

        $.ajax({

            type: "POST",
            url: url,
            data: user,
            success: function (data) {

                const parsed = $.parseHTML(data);

                const hasErrors = $(parsed).find("input[name='RegistrationInvalid']").val() == 'true';

                if (hasErrors) {

                    $("#userRegistrationModal").html(data);

                    $("#userRegistrationModal input[name = 'AcceptUserAgreement']").prop("checked", false);

                    $("#userRegistrationModal button[name = 'register']").prop("disabled", true);

                    $("#userRegistrationModal input[name = 'AcceptUserAgreement']").click(onAcceptUserAgreementClick);

                    registerUserButton = $("#userRegistrationModal button[name = 'register']").click(onUserRegisterClick);

                    $("#userRegistrationForm").removeData("validator");

                    $("#userRegistrationForm").removeData("unobtrusiveValidation");

                    $.validator.unobtrusive.parse("#userRegistrationForm");
                }
                else {
                    location.href = '/Home/Index';
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                DisplayClosableBootstrapAlert(
                    "#alertPlaceholderRegister",
                    "danger",
                    "Error!",
                    `Status: ${xhr.status} - ${xhr.statusText}`);

                console.error(`${thrownError}\r\n${xhr.statusText}\r\n${xhr.responseText}`);
            }
        });
    }
});

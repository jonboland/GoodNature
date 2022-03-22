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

        var email = $("#userRegistrationModal input[name = 'Email']").val();

        var url = "UserAuth/UserNameExists?userName=" + email;

        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                if (data == true) {

                    var alertHTML = '<div class="alert alert-warning alert-dismissible fade show" role="alert">'
                        + '<strong>Email Error</strong><br>This email address has already been registered'
                        + '<button type="button" class="close" data-dismiss="alert" aria-label="Close">'
                        + '<span aria-hidden="true">&times;</span>'
                        + '</button>'
                        + '</div>';

                    $("#alertPlaceholderRegister").html(alertHTML);
                }
                else {
                    $("#alertPlaceholderRegister").html("");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);
            }
        });
    });

    var registerUserButton = $("#userRegistrationModal button[name = 'register']").click(onUserRegisterClick);

    function onUserRegisterClick() {

        var url = "UserAuth/RegisterUser";

        var antiForgeryToken = $("#userRegistrationModal input[name='__RequestVerificationToken']").val();

        var email = $("#userRegistrationModal input[name='Email']").val();
        var password = $("#userRegistrationModal input[name='Password']").val();
        var confirmPassword = $("#userRegistrationModal input[name='ConfirmPassword']").val();
        var firstName = $("#userRegistrationModal input[name='FirstName']").val();
        var lastName = $("#userRegistrationModal input[name='LastName']").val();
        var address1 = $("#userRegistrationModal input[name='Address1']").val();
        var address2 = $("#userRegistrationModal input[name='Address2']").val();
        var postcode = $("#userRegistrationModal input[name='Postcode']").val();
        var phoneNumber = $("#userRegistrationModal input[name='PhoneNumber']").val();

        var user = {
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

                var parsed = $.parseHTML(data);

                var hasErrors = $(parsed).find("input[name='RegistrationInvalid']").val() == 'true';

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
                console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);
            }
        });
    }
});

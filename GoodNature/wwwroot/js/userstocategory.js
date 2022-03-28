$(function () {

    $("button[name='saveSelectedUsers']").prop('disabled', true);

    $('select').on('change', function () {

        var url = "/Admin/UsersToCategory/GetUsersForCategory?categoryId=" + this.value;

        if (this.value != 0) {
            $.ajax({

                type: "GET",
                url: url,
                success: function (data) {
                    $("#usersCheckList").html(data);
                    $("button[name='saveSelectedUsers']").prop('disabled', false);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    DisplayClosableBootstrapAlert(
                        "#alertPlaceholder",
                        "danger",
                        "Error!",
                        `Status: ${xhr.status} - ${xhr.statusText}`);

                    console.error(`${thrownError}\r\n${xhr.statusText}\r\n${xhr.responseText}`);
                }
            });
        }
        else {
            $("button[name='saveSelectedUsers']").prop('disabled', true);
            $("input[type=checkbox]").prop("checked", false);
            $("input[type=checkbox]").prop("disabled", true);
        }
    });

    $('#saveSelectedUsers').click(function () {

        var url = "/Admin/UsersToCategory/SaveSelectedUsers";

        var categoryId = $("#CategoryId").val();

        var antiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        var usersSelected = [];

        $('input[type=checkbox]:checked').each(function () {
            var userModel = {
                Id: $(this).attr("value")
            };

            usersSelected.push(userModel);
        });

        var usersSelectedForCategory = {
            __RequestVerificationToken: antiForgeryToken,
            CategoryId: categoryId,
            UsersSelected: usersSelected
        };
        $.ajax({

            type: "POST",
            url: url,
            data: usersSelectedForCategory,
            success: function (data) {
                $("#usersCheckList").html(data);

            },
            error: function (xhr, ajaxOptions, thrownError) {
                DisplayClosableBootstrapAlert(
                    "#alertPlaceholder",
                    "danger",
                    "Error!",
                    `Status: ${xhr.status} - ${xhr.statusText}`);

                console.error(`${thrownError}\r\n${xhr.statusText}\r\n${xhr.responseText}`);
            }
        });
    });
});

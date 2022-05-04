$(function () {

    $("button[name='saveSelectedUsers']").prop("disabled", true);

    $("select").on("change", function () {

        const url = "/Admin/UsersToCategory/GetUsersForCategory?categoryId=" + this.value;

        if (this.value != 0) {
            $.ajax({

                type: "GET",
                url: url,
                success: function (data) {
                    $("#usersCheckList").html(data);
                    $("button[name='saveSelectedUsers']").prop("disabled", false);
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
            $("button[name='saveSelectedUsers']").prop("disabled", true);
            $("input[type=checkbox]").prop("checked", false);
            $("input[type=checkbox]").prop("disabled", true);
        }
    });

    $("#saveSelectedUsers").click(function () {

        const url = "/Admin/UsersToCategory/SaveSelectedUsers";
        const categoryId = $("#CategoryId").val();
        const antiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        const usersSelected = [];
        const usersActive = [];

        DisableControls(true);

        const $selectedBoxes = $('input[name=usersSelected]:checked');

        $selectedBoxes.each(function () {
            const userModel = {
                Id: $(this).attr("value")
            };

            usersSelected.push(userModel);
        });

        const $activeBoxes = $('input[name=usersActive]:checked');

        $activeBoxes.each(function () {
            const userModel = {
                Id: $(this).attr("value")
            };

            usersActive.push(userModel);
        });

        const usersSelectedForCategory = {
            __RequestVerificationToken: antiForgeryToken,
            CategoryId: categoryId,
            UsersSelected: usersSelected,
            UsersActive: usersActive,
        };

        $.ajax({
            type: "POST",
            url: url,
            data: usersSelectedForCategory,
            success: function (data) {
                $("#usersCheckList").html(data);
                $(function () {
                    $(".save-success").show("fade").fadeTo(1000, 500).slideUp(500, function () {
                        DisableControls(false);
                    });
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                DisplayClosableBootstrapAlert(
                    "#alertPlaceholder",
                    "danger",
                    "Error!",
                    `Status: ${xhr.status} - ${xhr.statusText}`);

                console.error(`${thrownError}\r\n${xhr.statusText}\r\n${xhr.responseText}`);

                DisableControls(false);
            }
        });

        function DisableControls(disable) {
            $("input[type=checkbox]").prop("disabled", disable);
            $("#saveSelectedUsers").prop("disabled", disable);
            $("select").prop("disabled", disable);
        }
    });
});

$(function () {

    function configureDatepicker() {

        const monthsToAdd = 1;
        const currentDate = new Date();

        $(".datepicker").datepicker(
            {
                dateFormat: "dd/mm/yy",
                maxDate: addMonths(currentDate, monthsToAdd),
            }
        );
    }

    configureDatepicker();

});

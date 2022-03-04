$(function () {

    $(".datepicker").datepicker(
        {
            dateFormat: "dd/mm/yy",
            maxDate: addMonths(new Date(), 1),
        }
    );

    function addMonths(date, numberOfMonths) {

        var month = date.getMonth();

        var milliSeconds = new Date(date).setMonth(month + numberOfMonths);

        return new Date(milliSeconds);
    }
});

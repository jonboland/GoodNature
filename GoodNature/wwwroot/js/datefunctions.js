function addMonths(date, numberOfMonths) {

    const month = date.getMonth();

    const milliSeconds = new Date(date).setMonth(month + numberOfMonths);

    return new Date(milliSeconds);
}

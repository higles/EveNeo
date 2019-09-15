function UpdateClock() {
    var today = new Date();
    var h = today.getUTCHours();
    var m = today.getUTCMinutes();
    h = h < 10 ? "0" + h : h;
    m = m < 10 ? "0" + m : m;
    $('#gametime span').text(h + ":" + m);
}
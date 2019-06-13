$(function () {
   var valuta= $("#valuta").val($("#valuta option:first").val());
    $("#valuta").change(function () {
         valuta = $(this);
    });
    $('#pretrazi').click(function () {
        $(".error").remove();
        var data = {
            departure: $('#departure').val(),
            arrival: $('#arrival').val(),
            departureDate: $('#datepicker').val(),
            vrijednostValute: valuta.val(),
            arrivalDate: $('#datepicker2').val()
        }
        if (data.arrival < 3) {
            $('#arrival').after('<span class="error" style="color:red">Polje je obavezno i mora imati 3 slova!</span>');
            return
        }
        if (data.departure < 3) {
            $('#departure').after('<span class="error" style="color:red">Polje je obavezno i mora imati 3 slova!</span>');
            return
        }
        if (data.departureDate === "") {
            $('#datepicker').after('<span class="error" style="color:red">Polje je obavezno!</span>');
            return
        }
        if (data.arrivalDate === "") {
            $('#datepicker2').after('<span class="error" style="color:red">Polje je obavezno!</span>');
            return
        }
        $.get("/Flight/GetData", data, function (result) {
            $('#results').html(result);
        });
    })
})






 //this will add * next to required label
$('input[type=text],input[type=radio],input[type=checkbox],textarea,select,input[type=email],input[type=number]').each(function () {
    var req = $(this).attr('data-val-required');
    var exclude = $(this).attr('data-exclude');
    if (undefined != req && undefined == exclude) {
        var label = $('label[for="' + $(this).attr('id') + '"]');
        var text = label.text();
        if (text.length > 0) {
            label.append('<span style="color:red"> *</span>');
        }
    }
});
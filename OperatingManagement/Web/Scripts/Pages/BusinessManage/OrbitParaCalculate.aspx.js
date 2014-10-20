var _radios, _dialog;
$(window).ready(function () {
    _dialog = $("#dialog-form");
    _radios = $('#rblOrbitParameters').find('input');
    _radios.change(swapRadioStatus);
    swapRadioStatus();

    _dialog.dialog({
        autoOpen: false,
        height: 300,
        width: 400,
        modal: true,
        buttons: {
            '关闭': function () {
                $(this).dialog("close");
            }
        }
    });

    //if (!autoOpen) {
//        _dialog.dialog('open');
    //}
});
function swapRadioStatus() {
    var selVal = _radios.filter('[checked=checked]');
    var v = selVal.attr('value');
    var text = selVal.next().text();
    var fuParaFile = $('#fuParaFile');
    if (text.indexOf('发射坐标系') >= 0 || text.indexOf('发射惯性坐标系') >= 0) {
        fuParaFile.removeAttr('disabled');
    } else {
        fuParaFile.attr('disabled', true).val('').attr('value', '');
    }
}
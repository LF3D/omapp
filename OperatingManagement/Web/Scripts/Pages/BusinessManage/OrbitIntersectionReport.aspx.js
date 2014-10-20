var _dialog;
$(window).ready(function () {
    _dialog = $("#dialog-form");

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

//    if (!!_autoOpen) {
//        _dialog.dialog('open');
//    }
});

var _resultDialog;
$(window).ready(function () {
    _resultDialog = $("#result-form");

    _resultDialog.dialog({
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
});
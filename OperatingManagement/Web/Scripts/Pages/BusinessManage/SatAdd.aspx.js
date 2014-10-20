///<reference path="../jquery-1.4.1-vsdoc.js" />
var _dialog;
var tbWXGNs, hfWXGNs;
$(window).ready(function () {
    _dialog = $("#dialog-form");
    tbWXGNs = $('#tbWXGNs');
    hfWXGNs = $('#hfWXGNs');
    tbWXGNs.find('span.spanFNames').each(function (i, n) {
        var chk = $(n).prev();
        n.onclick = function () {
            chk.attr('checked', !chk.attr('checked'));
            wrapKeyValues(chk.get(0))
        }
        chk.click(function () {
            wrapKeyValues(this);
        });
    });
    initKeyValues();
});
function showMsg(msg) {
    var _dialog;
    _dialog = $("#dialog-form");
    _dialog.dialog({
        autoOpen: false,
        height: 150,
        width: 350,
        modal: true,
        buttons: {
            '关闭': function () {
                $(this).dialog("close");
            }
        }
    });
    _dialog.find('p.content').eq(0).html(msg);
    _dialog.dialog('open');
    return false;
}
function wrapKeyValues(o) {
    var k = o.value;
    if (!o.checked) {
        hfWXGNs.val(hfWXGNs.val().replace('[' + k + ']', ''));
    } else {
        hfWXGNs.val(hfWXGNs.val() + '[' + k + ']');
    }
}
function initKeyValues() {
    var k = hfWXGNs.val();
    if (k != '') {
        var ks = k.split('][');
        if (ks.length > 0) {
            for (var i = 0; i < ks.length; i++) {
                var _k = ks[i].ltrim('[').rtrim(']');
                if (_k != '') {
                    $('#chkGN' + _k).attr('checked', true);
                }
            }
        }
    }
}
function verifyRole() {
    if (hfWXGNs.val() == '') {
        showMsg('至少为当前卫星指定一个“功能”。');
        return false;
    }
    return true;
}
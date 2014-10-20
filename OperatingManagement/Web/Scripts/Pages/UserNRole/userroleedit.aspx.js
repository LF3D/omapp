///<reference path="../jquery-1.4.1-vsdoc.js" />
var _dialog;
var tbRoles, hfRoles;
$(window).ready(function () {
    _dialog = $("#dialog-form");
    tbRoles = $('#tbRoles');
    hfRoles = $('#hfRoles');
    tbRoles.find('span.spanTaskNotes').each(function (i, n) {
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
        hfRoles.val(hfRoles.val().replace('[' + k + ']', ''));
    } else {
        hfRoles.val(hfRoles.val() + '[' + k + ']');
    }
}
function initKeyValues() {
    var k = hfRoles.val();
    if (k != '') {
        var ks = k.split('][');
        if (ks.length > 0) {
            for (var i = 0; i < ks.length; i++) {
                var _k = ks[i].ltrim('[').rtrim(']');
                if (_k != '') {
                    $('#chkRole' + _k).attr('checked', true);
                }
            }
        }
    }
}
function verifyRole() {
    if (hfRoles.val() == '') {
        showMsg('至少为当前用户指定一个“角色”。');
        return false;
    }
    return true;
}
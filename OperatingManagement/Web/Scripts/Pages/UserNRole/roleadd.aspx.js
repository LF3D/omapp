///<reference path="../jquery-1.4.1-vsdoc.js" />
function wrapKeyValues(o) {
    var k = o.value;
    if (!o.checked) {
        hfModuleTasks.val(hfModuleTasks.val().replace('[' + k + ']', ''));
    } else {
        hfModuleTasks.val(hfModuleTasks.val() + '[' + k + ']');
    }
}
function initKeyValues() {
    var k = hfModuleTasks.val();
    if (k != '') {
        var ks = k.split('][');
        if (ks.length > 0) {
            for (var i = 0; i < ks.length; i++) {
                var _k = ks[i].ltrim('[').rtrim(']');
                if (_k != '') {
                    $('#chkPermission' + _k).attr('checked', true);
                }
            }
        }
    }
}
var tbModuleTasks, hfModuleTasks;
$(window).ready(function () {
    tbModuleTasks = $('#tbModuleTasks');
    hfModuleTasks = $('#hfModuleTasks');
    tbModuleTasks.find('span.spanTaskNotes').each(function (i, n) {
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
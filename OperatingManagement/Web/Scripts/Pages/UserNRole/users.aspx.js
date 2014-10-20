///<reference path="../jquery-1.4.1-vsdoc.js" />
var _dialog;
$(window).ready(function () {
    _dialog = $("#dialog-form");
});
function selectAll() {
    $('input:checkbox:not([disabled])').attr('checked', true);
    return false;
}
function checkAll(o) {
    $('#tbUsers').find('input:checkbox:not([disabled])').attr('checked', o.checked);
}
function editUser(id) {
    window.location.href = "/views/userandrole/useredit.aspx?id=" + id;
    return false;
}
function deleteUsers() {
    var chks = $('#tbUsers').find('input:checkbox:not([disabled])').filter('[checked=checked]');
    if (chks.length == 0) {
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
        _dialog.find('p.content').eq(0).html('请选择需要删除的用户。');
        _dialog.dialog('open');
        return false;
    }
    _dialog.dialog({
        autoOpen: false,
        height: 150,
        width: 350,
        modal: true,
        buttons: {
            '确定': function () {
                $(this).dialog("close");
                //ajax begin
                var ids = chks.map(function () { return this.value; }).get().join(',');
                var indicator = $('#submitIndicator').attr('class', 'load');
                indicator.show();

                $.ajax({
                    url: 'data.axd',
                    dataType: 'text',
                    data: {
                        ids: ids,
                        action: 'deleteUsersByIds',
                        t: Math.random()
                    },
                    error: function (resp) {
                        indicator.attr('class', 'error').html('数据提交过程中发生了异常。');
                    },
                    success: function (resp) {
                        var json = eval('(' + resp + ')');
                        if (json.suc) {
                            window.location.href = window.location.href;
                        } else {
                            indicator.attr('class', 'error').html('数据提交过程中发生了异常。');
                        }
                    }
                });

                //ajax end
            },
            '关闭': function () {
                $(this).dialog("close");
            }
        }
    });
    _dialog.find('p.content').eq(0).html('<span class="red">删除指定的用户后将不可恢复，确定要继续吗？</span>');
    _dialog.dialog('open');
    return false;
}

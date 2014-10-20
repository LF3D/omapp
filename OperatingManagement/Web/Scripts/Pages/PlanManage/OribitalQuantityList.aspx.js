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

function clearField() {
    $('input:text').val('');
    return false;
}

function showDetail(id) {
    var feature2 = 'height=200px,width=600px,toolbar=no, menubar=no,scrollbars=no,resizable=yes,location=no,status=yes,';
    //window.location.href = "/Views/PlanManage/OribitalQuantityDetail.aspx?id=" + id;
    window.open("/Views/PlanManage/OribitalQuantityDetail.aspx?id=" + id, "", feature2);
    return false;
}

function selectAll() {
    $('input:checkbox:not([disabled])').attr('checked', true);
    return false;
}

function checkAll(o) {
    if ( $('#tbGDs').length>0){
        $('#tbGDs').find('input:checkbox:not([disabled])').attr('checked', o.checked);
    }
}

function sendGD1() {
    //var chks = $('#tbGDs').find('input:checkbox:not([disabled])').filter('[checked=true]');
    var chks = $('#tbGDs').find('input:checkbox:[checked]');
    if (chks.length == 0) {
        showMsg('请选择您要发送的轨道数据。');
        return false;
    }

    var ids = chks.map(function () { return this.value; }).get().join(',');
    showSend(ids);

    return false;
}

function showSend(id) {
    var txtId = $('#txtId');
    txtId.val(id);

//    var btn = $('#btnHidden');
    //    btn.click();
    showPopSendForm();
    return false;
}

function showSelectAll() {
    var divselectAll1 = $('#selectAll1');
    divselectAll1.show();
    var divselectAll12 = $('#selectAll2');
    divselectAll12.show();

    return false;
}

function hideSelectAll() {
    var divselectAll1 = $('#selectAll1');
    divselectAll1.hide();
    var divselectAll12 = $('#selectAll2');
    divselectAll12.hide();

    return false;
}

function showPopSendForm() {
    var _dialog;
    $('#tartgetPanel').find('input:checkbox').attr('checked', false);
    _dialog = $("#tartgetPanel");
    _dialog.dialog({
        autoOpen: false,
        height: 400,
        width: 330,
        modal: true,
        buttons: {
            '确定': function () {
                var list = $('#tartgetPanel').find('input:checkbox:[checked]');
                if (list.length == 0) {
                    $('#lblTargetMessage').show();
                }
                else {
                    $('#lblTargetMessage').hide();
                    $(this).dialog("close");
                    var btn = $('#btnSubmit');
                    btn.click();
                }
            },
            '取消': function () {
                $(this).dialog("close");
            }
        }
    });
    _dialog.dialog('open');
    _dialog.parent().appendTo($("form:first"));
    return false;
}

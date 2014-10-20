var _dialog;
$(window).ready(function () {
    _dialog = $("#dialog-form");
});

function clearField() {
    $('#txtCStartDate').val("");
    $('#txtCEndDate').val("");
    $('#txtJHStartDate').val("");
    $('#txtJHEndDate').val("");
    return false;
}

function showDetail(id) {
    window.open("/Views/PlanManage/ExperimentPlanDetail.aspx?id=" + id);
    return false;
}

function selectAll() {
    $('input:checkbox:not([disabled])').attr('checked', true);
    return false;
}

function checkAll(o) {
    if ($('#tbPlans').length > 0) {
        $('#tbPlans').find('input:checkbox:not([disabled])').attr('checked', o.checked);
    }
}

function sendPlan() {
    //var chks = $('#tbPlans').find('input:checkbox:not([disabled])').filter('[checked=true]');
    _dialog.dialog({
        autoOpen: false,
        height: 150,
        width: 400,
        modal: true,
        buttons: {
            '关闭': function () {
                $(this).dialog("close");
            }
        }
    });

    var chks = $('#tbPlans').find('input:checkbox:[checked]');
    if (chks.length == 0) {
        _dialog.find('p.content').eq(0).html('请选择您要发送的计划。');
        _dialog.dialog('open');
        return false;
    }
    if (chks.length > 1) {
        _dialog.find('p.content').eq(0).html('只能选择一个计划进行发送。');
        _dialog.dialog('open');
        return false;
    }

    var ids = chks.map(function () { return this.value; }).get().join(',');

    showSend(ids);
    return false;
}

function showSend(ids) {
    var txtId = $('#txtId');
    txtId.val(ids);

    var btn = $('#btnHidden');
    btn.click();
    return false;
}

function showPopSendForm() {
    var _dialog;
    _dialog = $("#tartgetPanel");
    _dialog.dialog({
        autoOpen: false,
        height: 350,
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

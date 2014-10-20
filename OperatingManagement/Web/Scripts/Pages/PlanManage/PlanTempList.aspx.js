var _dialog;
$(window).ready(function () {
    _dialog = $("#dialog-form");
});


function clearField() {
    $('#txtCStartDate').val("");
    $('#txtCEndDate').val("");
    $('#txtJHStartDate').val("");
    $('#txtJHEndDate').val("");
    var sal = $('#ddlType');
    sal.val("YJJH");
    return false;
}


function showEdit(planid, plantype) {
    var feature1 = 'width=820px;height=600px,toolbar=no, menubar=no,scrollbars=yes,resizable=yes,location=no,status=yes,';
    var feature2 = 'width=500px;toolbar=no, menubar=no,scrollbars=no,resizable=yes,location=no,status=yes,';
    var condition = "?istemp=true&id=" + planid + "&startDate=" + $('#txtCStartDate').val() + "&endDate=" + $('#txtCEndDate').val() + "&jhStartDate=" + $('#txtJHStartDate').val() + "&jhEndDate=" + $('#txtJHEndDate').val();
    switch (plantype)
        {
            case "YJJH":
                window.location.href = "/Views/PlanManage/YJJHEdit.aspx" + condition;
                break;
            case "XXXQ":
                window.location.href = "/Views/PlanManage/XXXQEdit.aspx" + condition;
                break;
            case "DJZYJH":
                window.location.href = "/Views/PlanManage/DJZYJHDetail.aspx" + condition;
                break;
            case "DJZYSQ":
                window.location.href = "/Views/PlanManage/DJZYSQEdit.aspx" + condition;
                break;
            case "GZJH":
                window.location.href = "/Views/PlanManage/GZJHEdit.aspx" + condition;
                break;
            case "ZZGZJH":
                window.location.href = "/Views/PlanManage/ZZGZJHEdit.aspx" + condition;
                break;
            case "ZXJH":
                window.location.href = "/Views/PlanManage/ZXJHEdit.aspx" + condition;
                break;
            case "TYSJ":
                window.location.href = "/Views/PlanManage/TYSJEdit.aspx" + condition;
                break;
        }
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

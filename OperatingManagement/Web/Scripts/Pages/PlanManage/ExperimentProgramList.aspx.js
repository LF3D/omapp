
function clearField() {
    $('#txtCStartDate').val("");
    $('#txtCEndDate').val("");
    $('#txtJHStartDate').val("");
    $('#txtJHEndDate').val("");
    return false;
}

function showDetail(id) {
    //window.location.href = "/Views/PlanManage/ExperimentProgramDetail.aspx?id=" + id;
    window.open("/Views/PlanManage/ExperimentProgramDetail.aspx?id=" + id);
    return false;
}

function generatePlans(id) {
    //window.location.href = "/Views/PlanManage/GenaratedPlanList.aspx?id=" + id;
    //    window.open("/Views/PlanManage/GenaratedPlanList.aspx?id=" + id);
    var txtid = $('#txtID');
    txtid.val(id);
    var btn = $('#btnCreatePlan');
    btn.click();
    return false;
}

function showPopForm(id) {
    var txtid = $('#txtID');
    txtid.val(id);

    var _dialog;
    _dialog = $("#tartgetPanel");
    _dialog.dialog({
        autoOpen: false,
        height: 200,
        width: 500,
        modal: true,
        buttons: {
//            '生成计划': function () {
//                    //$('#lblTargetMessage').hide();
//                    var btnc = $('#btnCreatePlan');
//                    btnc.click();
//            },
//            '取消': function () {
//                $(this).dialog("close");
//            }
        }
    });
    _dialog.dialog('open');
    _dialog.parent().appendTo($("form:first"));
    return false;
}
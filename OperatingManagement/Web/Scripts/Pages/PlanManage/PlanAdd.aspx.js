$(function () {
    $("#txtStartTime").datepicker();
    $("#txtEndTime").datepicker();
});

function SelectSBJH(id, fileindex) {
    var txtId = $('#hfSBJHID');
    txtId.val(id);
    var btn = $('#btnSBJH');
    var filepath = unescape(fileindex);
    btn.text("所选设备计划： "+filepath + "    (点击取消选择)");
    return false;
}

function setdayte(o) {
    $(o).datepicker({
        changeMonth: true,
        changeYear: true
    });
}
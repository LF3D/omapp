function showMsg(msg) {
    $.fn.modal({
        title: '提示信息',
        content: function (o, e) {
            o.content.html(msg);
        },
        cancelText: '关闭'
    });
}
function clearField() {
    $('#txtFrom').val("");
    $('#txtTo').val("");
    var sel = $('#tdFilter').find("select");
    sel.val("0");
    return false;
}

function sendFile() {
    window.location.href = "/views/businessManage/sendfile.aspx";
    return false;
}

//重发文件，status=0，已提交发送，status=1，发送中
function reSendFile(id, status) {
    if (status == "0") {
        showMsg('文件正在等待发送，请不要重发。');
        return false;
    }
    if (status == "1") {
        showMsg('文件正在发送中，请不要重发。');
        return false;
    }
    var tRID = $('#txtRID');
    tRID.val(id);

    var tStatus = $('#txtStatus');
    tStatus.val(status);

    var btn = $('#btnHidRSendFile');
    btn.click();
    return false;
}
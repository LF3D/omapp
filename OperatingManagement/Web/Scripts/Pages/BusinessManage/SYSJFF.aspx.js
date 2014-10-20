function SendFile() {
    //return true;
    var _dialog;
    _dialog = $("#dialog-form");

    var datatype = $('#ddlDataType').val();
    var chks_yc;
    var chks_uf;
    var chks_fz;
    var chks_gd;
    //TJMB_Data
    if (datatype == "0") {
        chks_yc = $('#tbYCData').find('input:checkbox:not([disabled])').filter('[checked=checked]');
        chks_uf = $('#tbUFData').find('input:checkbox:not([disabled])').filter('[checked=checked]');
    }
    //KJJD_Data
    if (datatype == '1')
        chks_yc = $('#tbYCData').find('input:checkbox:not([disabled])').filter('[checked=checked]');
    //FZTY_Data
    if (datatype == '2')
        chks_fz = $('#tbFZData').find('input:checkbox:not([disabled])').filter('[checked=checked]');

    chks_gd = $('#tbGDs').find('input:checkbox:not([disabled])').filter('[checked=checked]');

    if ((chks_yc == null || chks_yc.length == 0) && (chks_fz == null || chks_fz.length == 0)
        && (chks_uf == null || chks_uf.length == 0) && (chks_gd == null || chks_gd.length == 0)) {
        _dialog.dialog({
            autoOpen: false,
            height: 150,
            width: 350,
            modal: true,
            buttons: {
                '确定': function () {
                    $(this).dialog("close");
                }
            }
        });
        _dialog.find('p.content').eq(0).html('请选择需要发送的试验数据。');
        _dialog.dialog('open');
        return false;
    }

    var ycids;
    if (chks_yc != null) {
        ycids = chks_yc.map(function () { return this.value; }).get().join(',');
        $('#hfycids').val(ycids);
    }
    else
        $('#hfycids').val("");
    var ufids;
    if (chks_uf != null) {
        ufids = chks_uf.map(function () { return this.value; }).get().join(',');
        $('#hfufids').val(ufids);
    }
    else
        $('#hfufids').val("");
    var fzids;
    if (chks_fz != null) {
        fzids = chks_fz.map(function () { return this.value; }).get().join(',');
        $('#hffzids').val(fzids);
    }
    else
        $('#hffzids').val("");
    var gdids;
    if (chks_gd != null) {
        gdids = chks_gd.map(function () { return this.value; }).get().join(',');
        $('#hfgdids').val(gdids);
    }
    else
        $('#hfgdids').val("");

    //return true;
    var _Senddialog;
    _Senddialog = $("#SendPanel");
    _Senddialog.dialog({
        autoOpen: false,
        height: 180,
        width: 350,
        modal: true,
        buttons: {
            '确定': function () {
                var _radios = $('#rblProtocol').find('input');
                var selVal = _radios.filter('[checked=checked]');
                var sendway = selVal.attr('value');
                $('#hfsendway').val(sendway);
                $(this).dialog("close");
                var btn = $('#btnHidden');
                btn.click();
            },
                '取消': function () {
                    $(this).dialog("close");
                }
        }
    });
    _Senddialog.dialog('open');
    return false;
}

function clearField() {
    $('#txtFrom').val("");
    $('#txtTo').val("");
    var sel = $('#tdTask').find("select");
    sel.val("0");
    sel = $('#tdSat').find("select");
    sel.val("0");
    sel = $('#tdData').find("select");
    sel.val("0");
    return false;
}

function createFile() {
//    return true;
    var _dialog;
    _dialog = $("#dialog-form");

    var datatype = $('#ddlDataType').val();
    var chks_yc;
    var chks_uf;
    var chks_fz;
    var chks_gd;
    //TJMB_Data
    if (datatype == "0") {
        chks_yc = $('#tbYCData').find('input:checkbox:not([disabled])').filter('[checked=checked]');
        chks_uf = $('#tbUFData').find('input:checkbox:not([disabled])').filter('[checked=checked]');
    }
    //KJJD_Data
    if (datatype == '1')
        chks_yc = $('#tbYCData').find('input:checkbox:not([disabled])').filter('[checked=checked]');
    //FZTY_Data
    if (datatype == '2')
        chks_fz = $('#tbFZData').find('input:checkbox:not([disabled])').filter('[checked=checked]');

    chks_gd = $('#tbGDs').find('input:checkbox:not([disabled])').filter('[checked=checked]');

    if ((chks_yc == null || chks_yc.length == 0) && (chks_fz == null || chks_fz.length == 0)
        && (chks_uf == null || chks_uf.length == 0) && (chks_gd == null || chks_gd.length == 0)) {
        _dialog.dialog({
            autoOpen: false,
            height: 150,
            width: 350,
            modal: true,
            buttons: {
                '确定': function () {
                    $(this).dialog("close");
                }
            }
        });
        _dialog.find('p.content').eq(0).html('请选择需要使用的试验数据。');
        _dialog.dialog('open');
        return false;
    }

    var ycids;
    if (chks_yc != null) {
        ycids = chks_yc.map(function () { return this.value; }).get().join(',');
        $('#hfycids').val(ycids);
    }
    else
        $('#hfycids').val("");
    var ufids;
    if (chks_uf != null) {
        ufids = chks_uf.map(function () { return this.value; }).get().join(',');
        $('#hfufids').val(ufids);
    }
    else
        $('#hfufids').val("");
    var fzids;
    if (chks_fz != null) {
        fzids = chks_fz.map(function () { return this.value; }).get().join(',');
        $('#hffzids').val(fzids);
    }
    else
        $('#hffzids').val("");
    var gdids;
    if (chks_gd != null) {
        gdids = chks_gd.map(function () { return this.value; }).get().join(',');
        $('#hfgdids').val(gdids);
    }
    else
        $('#hfgdids').val("");
    return true;
}

function showDetail(id) {
    var feature2 = 'height=200px,width=600px,toolbar=no, menubar=no,scrollbars=no,resizable=yes,location=no,status=yes,';
    //window.location.href = "/Views/PlanManage/OribitalQuantityDetail.aspx?id=" + id;
    window.open("/Views/PlanManage/OribitalQuantityDetail.aspx?id=" + id, "", feature2);
    return false;
}

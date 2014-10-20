function CheckClientValidate() {
    Page_ClientValidate();
    if (Page_IsValid) {
        var isvalid = true;
        var txtlist = $('#detailtable').find('input:text:not([disabled])');
        txtlist.each(function () {
            if ($(this).val() == '') {
                $(this).css({ "background-color": "#ffcccc" });
                isvalid = false;
            }
            else {
                $(this).css({ "background-color": "#f5f5f5" });
            }
        });
        if (!isvalid) {
            showMsg('您填写的计划信息不完整,请检查并填写完善后重新提交!'); 
        }

        return isvalid;
    }
}

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

function hideAllButton() {
    $(":submit").css('display','none'); 
    return false;
}

function setdayte(o){
    $(o).datepicker({
			changeMonth: true,
			changeYear: true
		});
}

//弹出进出站及航捷数据统计文件内容窗口
function showFileContentForm() {
    var _dialog;
    _dialog = $("#dialog-station");
    _dialog.dialog({
        autoOpen: false,
        height: 550,
        width: 750,
        modal: true,
        buttons: {
            '确定': function () {
                var list = $('#tbStations').find('input:checkbox:[checked]');
                if (list.length == 0) {
                    showMsg('请选择进出站及航捷数据。');
                }
                else {
                    $(this).dialog("close");
                    var ids = list.map(function () { return this.value; }).get().join(',');
                    $('#txtIds').val(ids);
                    var btn = $('#btnGetStationData');
                    btn.click();
                }
            },
            '关闭': function () {
                $(this).dialog("close");
            }
        }
    });
    _dialog.dialog('open');
    _dialog.parent().appendTo($("form:first"));
    return false;
}

function checkAll(o) {
    if ($('#tbStations').length > 0) {
        $('#tbStations').find('input:checkbox:not([disabled])').attr('checked', o.checked);
    }
}
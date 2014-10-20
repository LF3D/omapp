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
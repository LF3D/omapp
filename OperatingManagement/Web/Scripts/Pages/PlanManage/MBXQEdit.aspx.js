$(function () {
    $("#txtPlanStartTime").datepicker();
    $("#txtPlanEndTime").datepicker();

    $("#txtMBTimeSection1").datepicker({
            dateFormat: 'yymmdd'
		});
    $("#txtMBTimeSection2").datepicker({
            dateFormat: 'yymmdd'
		});
});

function showMsg(msg) {
    $.fn.modal({
        title: '提示信息',
        content: function (o, e) {
            o.content.html(msg);
        },
        cancelText: '关闭',
    });
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
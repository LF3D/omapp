
function showMsg(msg) {
    $.fn.modal({
        title: '提示信息',
        content: function (o, e) {
            o.content.html(msg);
        },
        cancelText: '关闭',
    });
}

function setdayte(o){
    $(o).datepicker({
			changeMonth: true,
			changeYear: true
		});
}

function showMsgSuccess(){
   return  showMsg('计划已成功保存！');
}

function showMsgError(){
   return  showMsg('保存计划失败，请重试！');
}
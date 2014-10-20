function CheckInput() {
    var chks = $('#cblXyxs').find('input:checkbox:[checked]');
    if (chks.length == 0) {
        $("#ltMessage").html("必须至少选择一个测站。");
        return false;
    }

    var rdoQcy = $('#tdQcy').find('input:radio:[checked]').attr("value");
    var qc = $("#txtQC").attr("value");
    if (rdoQcy == "rb2") {
        if (qc == "") {
            $("#ltMessage").html("圈次源为否时，必须填写圈次。");
            return false;
        }
    }
    return true;
}
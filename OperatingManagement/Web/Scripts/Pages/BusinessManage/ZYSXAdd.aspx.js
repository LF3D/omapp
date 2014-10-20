//$("#RadioButtonList1").find("input[@checked]").val()

$(function () {

    ValidatorEnable($('#rvInt')[0], false);
    ValidatorEnable($('#rvDouble')[0], false);
    ValidatorEnable($('#rvString')[0], false);
    ValidatorEnable($('#rvBool')[0], false);
    ValidatorEnable($('#rvEnum')[0], false);

    $("#rblType").change(function () {
        //var rbvalue = $("input[name=rblType]:radio:checked").val();
        var rbvalue = $('#rblType input:checked').val();
        var scope = $('#txtScope');

        if (rbvalue == "4") {
            scope.val('是,否');
            ValidatorEnable($('#rvInt')[0], false);
            ValidatorEnable($('#rvDouble')[0], false);
            ValidatorEnable($('#rvString')[0], false);
            ValidatorEnable($('#rvBool')[0], true);
            ValidatorEnable($('#rvEnum')[0], false);
        }
        else if (rbvalue == "1") {
            ValidatorEnable($('#rvInt')[0], true);
            ValidatorEnable($('#rvDouble')[0], false);
            ValidatorEnable($('#rvString')[0], false);
            ValidatorEnable($('#rvBool')[0], false);
            ValidatorEnable($('#rvEnum')[0], false);
        }
        else if (rbvalue == "2") {
            ValidatorEnable($('#rvInt')[0], false);
            ValidatorEnable($('#rvDouble')[0], true);
            ValidatorEnable($('#rvString')[0], false);
            ValidatorEnable($('#rvBool')[0], false);
            ValidatorEnable($('#rvEnum')[0], false);
        }
        else if (rbvalue == "3") {
            ValidatorEnable($('#rvInt')[0], false);
            ValidatorEnable($('#rvDouble')[0], false);
            ValidatorEnable($('#rvString')[0], true);
            ValidatorEnable($('#rvBool')[0], false);
            ValidatorEnable($('#rvEnum')[0], false);
        }
        else if (rbvalue == "5") {
            ValidatorEnable($('#rvInt')[0], false);
            ValidatorEnable($('#rvDouble')[0], false);
            ValidatorEnable($('#rvString')[0], false);
            ValidatorEnable($('#rvBool')[0], false);
            ValidatorEnable($('#rvEnum')[0], true);
        }
        else {

        }
    });

    $("#rblType").change();
});


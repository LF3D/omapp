function reset(o) {
    $('input:text').val('');
}

function showDetail(id) {
    window.location.href = "/Views/PlanManage/XiAnCeKongDataDetail.aspx?id=" + id;
    return false;
}
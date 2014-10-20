function reset(o) {
    $('input:text').val('');
}

function showDetail(id) {
    window.location.href = "/Views/PlanManage/MeasureDataDetail.aspx?id=" + id;
    return false;
}
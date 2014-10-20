
function showEdit(planid, plantype) {
    //window.location.href = "/Views/PlanManage/PlanEdit.aspx?id=" + id;
    window.open("/Views/PlanManage/PlanEdit.aspx?planid=" + planid + "&infotype=" + plantype);
    return false;
}
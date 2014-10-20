---------------------------------------------
-- Export file for user HTCUSER            --
-- Created by taiji on 2012/3/20, 22:20:07 --
---------------------------------------------

spool VW_01_计划管理20120320.log

prompt
prompt Creating view V_GD
prompt ==================
prompt
create or replace view htcuser.v_gd as
select "ID","CTIME","SATID","ITYPE","ICODE","D","T","TIMES","A","E","I","Q","W","M","DELTP","P","RA","RP","CDSM","KZ1","KSM","KZ2","RESERVE"
from tb_gd;

prompt
prompt Creating view V_YDSJ
prompt ====================
prompt
create or replace view htcuser.v_ydsj as
select "ID","CTIME","SPACETYPE","RESERVE","DATA_D","DATA_T","DATA_A","DATA_E","DATA_I","DATA_OHM","DATA_OMEGA","DATA_M","DATA_ID"
from tb_ydsj;


spool off

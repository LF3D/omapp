---------------------------------------------
-- Export file for user HTCUSER            --
-- Created by Cindy on 2012/9/12, 15:15:23 --
---------------------------------------------

spool up_basicdata.log

prompt
prompt Creating procedure UP_SATELLITE_INSERT
prompt ======================================
prompt
create or replace procedure up_satellite_insert
(
       p_WXMC tb_satellite.WXMC%type,
       p_WXBM tb_satellite.WXBM%type,
       p_WXBS tb_satellite.WXBS%type,
       p_State tb_satellite.State%type,
       p_MZB tb_satellite.MZB%type,
       p_BMFSXS tb_satellite.BMFSXS%type,
       p_SX tb_satellite.SX%type,
       p_GN tb_satellite.GN%type,
       p_CTime tb_satellite.CTime%type,
       v_result out number
)
is
       m_count integer;
begin
       select count(*) into m_count from tb_satellite where WXMC=p_WXMC;
       if m_count>0 then
         v_result:=3; --WXMC Duplicated.
         return;
       end if;

       select count(*) into m_count from tb_satellite where WXBM=p_WXBM;
       if m_count>0 then
         v_result:=6; --WXBM Duplicated.
         return;
       end if;

       select count(*) into m_count from tb_satellite where WXBS=p_WXBS;
       if m_count>0 then
         v_result:=7; --WXBS Duplicated.
         return;
       end if;

       savepoint p1;
       insert into tb_satellite(WXMC, WXBM, WXBS, state, MZB, BMFSXS, SX, Gn, ctime)
       values(p_WXMC,p_WXBM,p_WXBS,p_State,p_MZB,p_BMFSXS,p_SX,p_GN,p_CTime);
       commit;
       v_result:=5; -- Success

       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;
/

prompt
prompt Creating procedure UP_SATELLITE_SELECTALL
prompt =========================================
prompt
create or replace procedure UP_SATELLITE_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_SATELLITE Order By WXBM ASC;
end;
/

prompt
prompt Creating procedure UP_SATELLITE_SELECTBYID
prompt ==========================================
prompt
create or replace procedure UP_SATELLITE_SelectByID
(
       p_WXBM TB_SATELLITE.WXBM%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_SATELLITE Where WXBM=p_WXBM;
end;
/

create or replace procedure UP_Satellite_Search
(
       p_WXMC tb_satellite.WXMC%type,
       p_WXBM tb_satellite.WXBM%type,
       p_WXBS tb_satellite.WXBS%type,
       p_State tb_satellite.State%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_SATELLITE 
            Where (upper(WXMC) LIKE ('%' || upper(p_WXMC) || '%') Or p_WXMC IS NULL)
              And (upper(WXBM) LIKE ('%' || upper(p_WXBM) || '%') Or p_WXBM IS NULL)
              And (upper(WXBS) LIKE ('%' || upper(p_WXBS) || '%') Or p_WXBS IS NULL)
              And (State=p_State Or p_State IS NULL)
            Order By WXBM ASC;
end;

/

prompt
prompt Creating procedure UP_SATELLITE_UPDATE
prompt ======================================
prompt
create or replace procedure up_satellite_update
( 
       p_WXMC tb_satellite.WXMC%type,
       p_WXBM tb_satellite.WXBM%type,
       p_WXBS tb_satellite.WXBS%type,
       p_State tb_satellite.State%type,
       p_MZB tb_satellite.MZB%type,
       p_BMFSXS tb_satellite.BMFSXS%type,
       p_SX tb_satellite.SX%type,
       p_GN tb_satellite.GN%type,
       v_result out number
)
is
       m_count integer;
begin
       select count(*) into m_count from tb_satellite where WXMC=p_WXMC and WXBM!=p_WXBM;
       if m_count>0 then
         v_result:=3; --Name Duplicated.
         return;
       end if;

       select count(*) into m_count from tb_satellite where WXBS=p_WXBS and WXBM!=p_WXBM;
       if m_count>0 then
         v_result:=7; --WXBS Duplicated.
         return;
       end if;

       savepoint p1;
       update tb_satellite
       set WXMC = p_WXMC
           , WXBS = p_WXBS
           , State = p_State
           , MZB = p_MZB
           , BMFSXS = p_BMFSXS
           , SX = p_SX
           , GN = p_GN
       where WXBM=p_WXBM;
       commit;
       v_result:=5; -- Success

       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;

/

prompt
prompt Creating procedure UP_TASK_INSERT
prompt =================================
prompt
create or replace procedure up_task_insert
(
       p_taskName tb_task.TaskName%type,
       p_TaskNo tb_task.TaskNo%type,
       p_ObjectFlag tb_task.ObjectFlag%type,
       p_SatID tb_task.SatID%type,
       p_IsCurTask tb_task.IsCurTask%type,
       p_BeginTime tb_task.BeginTime%type,
       p_EndTime tb_task.EndTime%type,
       p_CTime tb_task.CTime%type,
       v_Id out tb_task.id%type,
       v_result out number
)
is
       m_count integer;
begin
       select count(*) into m_count from tb_task where TaskName=p_TaskName;
       if m_count>0 then
         v_result:=3; --Name Duplicated.
         return;
       end if;

       select count(*) into m_count from tb_task where TaskNo=p_TaskNo;
       if m_count>0 then
         v_result:=6; --TaskNo Duplicated.
         return;
       end if;

       select count(*) into m_count from tb_task where ObjectFlag=p_ObjectFlag;
       if m_count>0 then
         v_result:=7; --ObjectFlag Duplicated.
         return;
       end if;

       savepoint p1;
       select seq_tb_task.NEXTVAL INTO v_Id from dual;
       insert into tb_task(Id, TaskName, taskno, objectflag, satid, iscurtask, begintime, endtime, ctime)
       values(v_Id,p_TaskName,p_TaskNo,p_ObjectFlag,p_SatID,p_IsCurTask,p_BeginTime,p_EndTime,p_CTime);
       commit;
       v_result:=5; -- Success

       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;
/

prompt
prompt Creating procedure UP_TASK_SELECTALL
prompt ====================================
prompt
create or replace procedure UP_task_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_task;
end;
/

prompt
prompt Creating procedure UP_TASK_SELECTBYID
prompt =====================================
prompt
create or replace procedure up_task_selectbyid
(
       p_Id tb_task.id%type,
       o_cursor out sys_refcursor
)
is
begin
       open o_cursor for
            select * from tb_task where id=p_id;
end;
/

prompt
prompt Creating procedure UP_TASK_UPDATE
prompt =================================
prompt
create or replace procedure up_task_update
(
       p_id tb_task.id%type,
       p_taskName tb_task.TaskName%type,
       p_TaskNo tb_task.TaskNo%type,
       p_ObjectFlag tb_task.ObjectFlag%type,
       p_SatID tb_task.SatID%type,
       p_IsCurTask tb_task.IsCurTask%type,
       p_BeginTime tb_task.BeginTime%type,
       p_EndTime tb_task.EndTime%type,
       v_result out number
)
is
       m_count integer;
begin
       select count(*) into m_count from tb_task where TaskName=p_TaskName and id!=p_Id;
       if m_count>0 then
         v_result:=3; --Name Duplicated.
         return;
       end if;

       select count(*) into m_count from tb_task where TaskNo=p_TaskNo and id!=p_Id;
       if m_count>0 then
         v_result:=6; --TaskNo Duplicated.
         return;
       end if;

       select count(*) into m_count from tb_task where ObjectFlag=p_ObjectFlag and id!=p_Id;
       if m_count>0 then
         v_result:=7; --ObjectFlag Duplicated.
         return;
       end if;

       savepoint p1;
       update tb_task
       set TaskName = p_TaskName
           , taskno = p_TaskNo
           , objectflag = p_ObjectFlag
           , satid = p_SatId
           , ISCURTASK = p_IsCurTask
           , BEGINTIME = p_BeginTime
           , ENDTIME = p_endTime
       where id = p_id;
       commit;
       v_result:=5; -- Success

       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;
/

prompt
prompt Creating procedure UP_ZYSX_SELECTALL
prompt ====================================
prompt
create or replace procedure UP_ZYSX_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_ZYSX Order By ID Desc;
end;
/

prompt
prompt Creating procedure UP_ZYSX_SELECTBYID
prompt =====================================
prompt
create or replace procedure UP_ZYSX_SelectByID
(
       p_ID TB_ZYSX.ID%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_ZYSX Where ID=p_ID;
end;
/


prompt
prompt Creating procedure UP_ZYSX_INSERT
prompt =================================
prompt
create or replace procedure htcuser.up_zysx_insert
(
       p_PName tb_zysx.pname%type,
       p_PCode tb_zysx.pcode%type,
       p_Type tb_zysx.type%type,
       p_Scope tb_zysx.scope%type,
       p_Own tb_zysx.own%type,
       v_Id out tb_zysx.id%type,
       v_result out number
)
is
       m_count integer;
begin
       select count(*) into m_count from tb_zysx where pname=p_PName;
       if m_count>0 then
         v_result:=3; --Name Duplicated.
         return;
       end if;

       select count(*) into m_count from tb_zysx where pcode=p_PCode;
       if m_count>0 then
         v_result:=6; --zysxNo Duplicated.
         return;
       end if;

       savepoint p1;
       select seq_tb_zysx.NEXTVAL INTO v_Id from dual;
       insert into tb_zysx(Id,pname,pcode,  type, scope, own)
       values(v_Id,p_PName,p_PCode,p_Type,p_Scope,p_Own);
       commit;
       v_result:=5; -- Success

       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;
/

prompt
prompt Creating procedure UP_ZYSX_UPDATE
prompt =================================
prompt
create or replace procedure htcuser.up_zysx_update
(
       p_id tb_zysx.id%type,
       p_PName tb_zysx.pname%type,
       p_PCode tb_zysx.pcode%type,
       p_Type tb_zysx.type%type,
       p_Scope tb_zysx.scope%type,
       p_Own tb_zysx.own%type,
       v_result out number
)
is
       m_count integer;
begin
       select count(*) into m_count from tb_zysx where pname=p_PName and  id <> p_id;
       if m_count>0 then
         v_result:=3; --Name Duplicated.
         return;
       end if;

       select count(*) into m_count from tb_zysx where pcode=p_PCode  and  id <> p_id;
       if m_count>0 then
         v_result:=6; --zysxNo Duplicated.
         return;
       end if;

       savepoint p1;
       update tb_zysx
       set pname = p_PName
           , pcode = p_PCode
           , type = p_Type
           , scope = p_Scope
           , own = p_Own
       where id = p_id;
       commit;
       v_result:=5; -- Success

       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;
/



prompt

prompt Creating procedure UP_ZYGN_INSERT
prompt =================================
prompt

create or replace procedure htcuser.up_zygn_insert
(
       p_FName tb_zygn.fname%type,
       p_FCode tb_zygn.fcode%type,
       p_MatchRule tb_zygn.matchrule%type,
       v_Id out tb_zygn.id%type,
       v_result out number
)
is
       m_count integer;
begin
       select count(*) into m_count from tb_zygn where fname=p_FName;
       if m_count>0 then
         v_result:=3; --Name Duplicated.
         return;
       end if;

       select count(*) into m_count from tb_zygn where fcode=p_FCode;
       if m_count>0 then
         v_result:=6; --Code Duplicated.
         return;
       end if;

       savepoint p1;
       select seq_tb_zygn.NEXTVAL INTO v_Id from dual;
       insert into tb_zygn(Id,fname,fcode,matchrule)
       values(v_Id,p_FName,p_FCode,p_MatchRule);
       commit;
       v_result:=5; -- Success

       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;
/

prompt

prompt Creating procedure UP_ZYGN_SELECTALL
prompt ====================================
prompt

create or replace procedure htcuser.UP_ZYGN_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_ZYGN Order By ID Desc;
end;
/

prompt

prompt Creating procedure UP_ZYGN_SELECTBYID
prompt =====================================
prompt

create or replace procedure htcuser.UP_ZYGN_SelectByID
(
       p_ID TB_ZYGN.ID%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_ZYGN Where ID=p_ID;
end;
/

prompt

prompt Creating procedure UP_ZYGN_UPDATE
prompt =================================
prompt

create or replace procedure htcuser.up_zygn_update
(
       p_id tb_zygn.id%type,
  p_FName tb_zygn.fname%type,
       p_FCode tb_zygn.fcode%type,
       p_MatchRule tb_zygn.matchrule%type,
       v_result out number
)
is
       m_count integer;
begin
       select count(*) into m_count from tb_zygn where fname=p_FName and  id <> p_id;
       if m_count>0 then
         v_result:=3; --Name Duplicated.
         return;
       end if;

       select count(*) into m_count from tb_zygn where fcode=p_FCode  and  id <> p_id;
       if m_count>0 then
         v_result:=6; --Code Duplicated.
         return;
       end if;

       savepoint p1;
       update tb_zygn
       set fname = p_FName
           , fcode = p_FCode
           , matchrule = p_MatchRule
       where id = p_id;
       commit;
       v_result:=5; -- Success

       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;
/




spool off

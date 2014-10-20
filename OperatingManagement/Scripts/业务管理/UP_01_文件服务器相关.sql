--设备工作计划
create or replace procedure up_SBJH_insert
(
	p_CTime tb_SBJH.Ctime%type,
       p_Source tb_SBJH.Source%type,
       p_Destination tb_SBJH.Destination%type,
       p_TaskID tb_SBJH.Taskid%type,
       p_InfoType tb_SBJH.Infotype%type,
       p_LineCount tb_SBJH.Linecount%type,
       p_FileIndex tb_SBJH.Fileindex%type,
       p_Reserve tb_SBJH.Reserve%type,
       v_Id out tb_SBJH.Id%type,
       v_result out number
)
is
begin
       v_Id:= to_number(fn_genseqnum('0011'));
       insert into tb_SBJH(id,Ctime,Source,Destination,Taskid,Infotype,Linecount,Fileindex,Reserve)  
       values(v_Id,sysdate(),p_Source,p_Destination,p_TaskID,p_InfoType,p_LineCount,p_FileIndex,p_Reserve);
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_SBJH_insert;

--空间目标信息
create or replace procedure up_MBXX_insert
(
       p_CTime tb_MBXX.Ctime%type,
       p_Source tb_MBXX.Source%type,
       p_Destination tb_MBXX.Destination%type,
       p_TaskID tb_MBXX.Taskid%type,
       p_InfoType tb_MBXX.Infotype%type,
       p_LineCount tb_MBXX.Linecount%type,
       p_FileIndex tb_MBXX.Fileindex%type,
       p_Reserve tb_MBXX.Reserve%type,
       v_Id out tb_MBXX.Id%type,
       v_result out number
)
is
begin
       v_Id:= to_number(fn_genseqnum('0012'));
       insert into tb_MBXX(id,Ctime,Source,Destination,Taskid,Infotype,Linecount,Fileindex,Reserve)  
       values(v_Id,sysdate(),p_Source,p_Destination,p_TaskID,p_InfoType,p_LineCount,p_FileIndex,p_Reserve);
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_MBXX_insert;

--空间环境信息
create or replace procedure up_HJXX_insert
(
       p_CTime tb_HJXX.Ctime%type,
       p_Source tb_HJXX.Source%type,
       p_Destination tb_HJXX.Destination%type,
       p_TaskID tb_HJXX.Taskid%type,
       p_InfoType tb_HJXX.Infotype%type,
       p_LineCount tb_HJXX.Linecount%type,
       p_FileIndex tb_HJXX.Fileindex%type,
       p_Reserve tb_HJXX.Reserve%type,
       v_Id out tb_HJXX.Id%type,
       v_result out number
)
is
begin
       v_Id:= to_number(fn_genseqnum('0013'));
       insert into tb_HJXX(id,Ctime,Source,Destination,Taskid,Infotype,Linecount,Fileindex,Reserve)  
       values(v_Id,sysdate(),p_Source,p_Destination,p_TaskID,p_InfoType,p_LineCount,p_FileIndex,p_Reserve);
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_HJXX_insert;

--碰撞预警报告
create or replace procedure up_YJBG_insert
(
       p_CTime tb_YJBG.Ctime%type,
       p_Source tb_YJBG.Source%type,
       p_Destination tb_YJBG.Destination%type,
       p_TaskID tb_YJBG.Taskid%type,
       p_InfoType tb_YJBG.Infotype%type,
       p_LineCount tb_YJBG.Linecount%type,
       p_FileIndex tb_YJBG.Fileindex%type,
       p_Reserve tb_YJBG.Reserve%type,
       v_Id out tb_YJBG.Id%type,
       v_result out number
)
is
begin
       v_Id:= to_number(fn_genseqnum('0014'));
       insert into tb_YJBG(id,Ctime,Source,Destination,Taskid,Infotype,Linecount,Fileindex,Reserve)  
       values(v_Id,sysdate(),p_Source,p_Destination,p_TaskID,p_InfoType,p_LineCount,p_FileIndex,p_Reserve);
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_YJBG_insert;

--交会预报报告
create or replace procedure up_JHBG_insert
(
       p_CTime tb_JHBG.Ctime%type,
       p_Source tb_JHBG.Source%type,
       p_Destination tb_JHBG.Destination%type,
       p_TaskID tb_JHBG.Taskid%type,
       p_InfoType tb_JHBG.Infotype%type,
       p_LineCount tb_JHBG.Linecount%type,
       p_FileIndex tb_JHBG.Fileindex%type,
       p_Reserve tb_JHBG.Reserve%type,
       v_Id out tb_JHBG.Id%type,
       v_result out number
)
is
begin
       v_Id:= to_number(fn_genseqnum('0015'));
       insert into tb_JHBG(id,Ctime,Source,Destination,Taskid,Infotype,Linecount,Fileindex,Reserve)  
       values(v_Id,sysdate(),p_Source,p_Destination,p_TaskID,p_InfoType,p_LineCount,p_FileIndex,p_Reserve);
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_JHBG_insert;

--试验需求
create or replace procedure up_SYXQ_insert
(
       p_CTime tb_SYXQ.Ctime%type,
       p_Source tb_SYXQ.Source%type,
       p_Destination tb_SYXQ.Destination%type,
       p_TaskID tb_SYXQ.Taskid%type,
       p_InfoType tb_SYXQ.Infotype%type,
       p_LineCount tb_SYXQ.Linecount%type,
       p_FileIndex tb_SYXQ.Fileindex%type,
       p_Reserve tb_SYXQ.Reserve%type,
       v_Id out tb_SYXQ.Id%type,
       v_result out number
)
is
begin
       v_Id:= to_number(fn_genseqnum('0016'));
       insert into tb_SYXQ(id,Ctime,Source,Destination,Taskid,Infotype,Linecount,Fileindex,Reserve)  
       values(v_Id,sysdate(),p_Source,p_Destination,p_TaskID,p_InfoType,p_LineCount,p_FileIndex,p_Reserve);
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_SYXQ_insert;

--天基目标观测试验数据处理结果
create or replace procedure up_GCJG_insert
(
       p_CTime tb_GCJG.Ctime%type,
       p_Source tb_GCJG.Source%type,
       p_Destination tb_GCJG.Destination%type,
       p_TaskID tb_GCJG.Taskid%type,
       p_InfoType tb_GCJG.Infotype%type,
       p_LineCount tb_GCJG.Linecount%type,
       p_FileIndex tb_GCJG.Fileindex%type,
       p_Reserve tb_GCJG.Reserve%type,
       v_Id out tb_GCJG.Id%type,
       v_result out number
)
is
begin
       v_Id:= to_number(fn_genseqnum('0017'));
       insert into tb_GCJG(id,Ctime,Source,Destination,Taskid,Infotype,Linecount,Fileindex,Reserve)  
       values(v_Id,sysdate(),p_Source,p_Destination,p_TaskID,p_InfoType,p_LineCount,p_FileIndex,p_Reserve);
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_GCJG_insert;

--遥操作试验数据处理结果
create or replace procedure up_CZJG_insert
(
       p_CTime tb_CZJG.Ctime%type,
       p_Source tb_CZJG.Source%type,
       p_Destination tb_CZJG.Destination%type,
       p_TaskID tb_CZJG.Taskid%type,
       p_InfoType tb_CZJG.Infotype%type,
       p_LineCount tb_CZJG.Linecount%type,
       p_FileIndex tb_CZJG.Fileindex%type,
       p_Reserve tb_CZJG.Reserve%type,
       v_Id out tb_CZJG.Id%type,
       v_result out number
)
is
begin
       v_Id:= to_number(fn_genseqnum('0018'));
       insert into tb_CZJG(id,Ctime,Source,Destination,Taskid,Infotype,Linecount,Fileindex,Reserve)  
       values(v_Id,sysdate(),p_Source,p_Destination,p_TaskID,p_InfoType,p_LineCount,p_FileIndex,p_Reserve);
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_CZJG_insert;

--空间机动试验数据处理结果
create or replace procedure up_JDJG_insert
(
       p_CTime tb_JDJG.Ctime%type,
       p_Source tb_JDJG.Source%type,
       p_Destination tb_JDJG.Destination%type,
       p_TaskID tb_JDJG.Taskid%type,
       p_InfoType tb_JDJG.Infotype%type,
       p_LineCount tb_JDJG.Linecount%type,
       p_FileIndex tb_JDJG.Fileindex%type,
       p_Reserve tb_JDJG.Reserve%type,
       v_Id out tb_JDJG.Id%type,
       v_result out number
)
is
begin
       v_Id:= to_number(fn_genseqnum('0019'));
       insert into tb_JDJG(id,Ctime,Source,Destination,Taskid,Infotype,Linecount,Fileindex,Reserve)  
       values(v_Id,sysdate(),p_Source,p_Destination,p_TaskID,p_InfoType,p_LineCount,p_FileIndex,p_Reserve);
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_JDJG_insert;

--仿真推演试验数据处理结果
create or replace procedure up_TYJG_insert
(
       p_CTime tb_TYJG.Ctime%type,
       p_Source tb_TYJG.Source%type,
       p_Destination tb_TYJG.Destination%type,
       p_TaskID tb_TYJG.Taskid%type,
       p_InfoType tb_TYJG.Infotype%type,
       p_LineCount tb_TYJG.Linecount%type,
       p_FileIndex tb_TYJG.Fileindex%type,
       p_Reserve tb_TYJG.Reserve%type,
       v_Id out tb_TYJG.Id%type,
       v_result out number
)
is
begin
       v_Id:= to_number(fn_genseqnum('0020'));
       insert into tb_TYJG(id,Ctime,Source,Destination,Taskid,Infotype,Linecount,Fileindex,Reserve)  
       values(v_Id,sysdate(),p_Source,p_Destination,p_TaskID,p_InfoType,p_LineCount,p_FileIndex,p_Reserve);
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_TYJG_insert;


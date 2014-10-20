--轨道
create or replace procedure up_GD_insert
(
       p_Version  TB_UDPDATABASICINFO.Version%type,
       p_Flag  TB_UDPDATABASICINFO.Flag%type,
       p_Maintype  TB_UDPDATABASICINFO.Maintype%type,
       p_datatype  TB_UDPDATABASICINFO.Datatype%type,
       p_Source  TB_UDPDATABASICINFO.Source%type,
       p_Destination  TB_UDPDATABASICINFO.Destination%type,
       p_missioncode  TB_UDPDATABASICINFO.Missioncode%type,
       p_satellitecode  TB_UDPDATABASICINFO.Satellitecode%type,
       p_datadate  TB_UDPDATABASICINFO.Datadate%type,
       p_datatime  TB_UDPDATABASICINFO.Datatime%type,
       p_sequencenumber  TB_UDPDATABASICINFO.Sequencenumber%type,
       p_childrenpacknumber  TB_UDPDATABASICINFO.Childrenpacknumber%type,
       p_udpReserve  TB_UDPDATABASICINFO.Reserve%type,
       p_datalength  TB_UDPDATABASICINFO.Datalength%type,
       p_dataclass  TB_UDPDATABASICINFO.Dataclass%type,      
       p_Reserve tb_GD.Reserve%type,
       p_Data_D tb_GD.Data_d%type,
       p_Data_T tb_GD.Data_t%type,
       p_Data_A tb_GD.Data_a%type,
       p_Data_E tb_GD.Data_e%type,
       p_Data_I tb_GD.Data_i%type,
       p_Data_Ohm tb_GD.Data_Ohm%type,
       p_Data_Omega tb_GD.Data_Omega%type,
       p_Data_M tb_GD.Data_m%type,
       p_Data_P tb_GD.Data_p%type,
       p_Data_PI tb_GD.Data_Pi%type,
       p_Data_RA tb_GD.Data_Ra%type,
       p_Data_RP tb_GD.Data_Rp%type,
       v_Id out tb_GD.Id%type,
       v_result out number
)
is
       m_UDPId integer;
begin
       m_UDPId := to_number(fn_genseqnum('0050'));
       v_Id:= to_number(fn_genseqnum('0051'));
       insert into tb_udpdatabasicinfo (ID, CTIME, VERSION, FLAG, MAINTYPE, DATATYPE, SOURCE, DESTINATION, MISSIONCODE, SATELLITECODE, DATADATE, DATATIME, SEQUENCENUMBER, CHILDRENPACKNUMBER, RESERVE, DATALENGTH, DATACLASS)  
       values(m_UDPId,sysdate(),p_Version,p_Flag,p_Maintype,p_datatype,p_Source,p_Destination,p_missioncode,p_satellitecode,p_datadate,p_datatime,p_sequencenumber,p_childrenpacknumber,p_udpReserve,p_datalength,p_dataclass);
       insert into tb_gd (ID, CTIME, RESERVE, DATA_D, DATA_T, DATA_A, DATA_E, DATA_I, DATA_OHM, DATA_OMEGA, DATA_M, DATA_P, DATA_PI, DATA_RA, DATA_RP, DATA_ID)
       values(v_Id,sysdate(),p_Reserve,p_Data_D,p_Data_T,p_Data_A,p_Data_E,p_Data_I,p_Data_Ohm,p_Data_Omega,p_Data_M,p_Data_P,p_Data_PI,p_Data_RA,p_Data_RP,m_UDPId);
       
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_GD_insert;

--卫星事后精轨根数
create or replace procedure up_GDSH_insert
(
       p_Version  TB_UDPDATABASICINFO.Version%type,
       p_Flag  TB_UDPDATABASICINFO.Flag%type,
       p_Maintype  TB_UDPDATABASICINFO.Maintype%type,
       p_datatype  TB_UDPDATABASICINFO.Datatype%type,
       p_Source  TB_UDPDATABASICINFO.Source%type,
       p_Destination  TB_UDPDATABASICINFO.Destination%type,
       p_missioncode  TB_UDPDATABASICINFO.Missioncode%type,
       p_satellitecode  TB_UDPDATABASICINFO.Satellitecode%type,
       p_datadate  TB_UDPDATABASICINFO.Datadate%type,
       p_datatime  TB_UDPDATABASICINFO.Datatime%type,
       p_sequencenumber  TB_UDPDATABASICINFO.Sequencenumber%type,
       p_childrenpacknumber  TB_UDPDATABASICINFO.Childrenpacknumber%type,
       p_udpReserve  TB_UDPDATABASICINFO.Reserve%type,
       p_datalength  TB_UDPDATABASICINFO.Datalength%type,
       p_dataclass  TB_UDPDATABASICINFO.Dataclass%type,      
       p_Reserve tb_gdsh.Reserve%type,
       p_Data_D tb_gdsh.Data_d%type,
       p_Data_T tb_gdsh.Data_t%type,
       p_Data_A tb_gdsh.Data_a%type,
       p_Data_E tb_gdsh.Data_e%type,
       p_Data_I tb_gdsh.Data_i%type,
       p_Data_Ohm tb_gdsh.Data_Ohm%type,
       p_Data_Omega tb_gdsh.Data_Omega%type,
       p_Data_M tb_gdsh.Data_m%type,
       p_Data_CDSM tb_gdsh.data_cdsm%type,
       p_Data_KSM tb_gdsh.data_ksm%type,
       p_Data_KZ1 tb_gdsh.data_kz1%type,
       p_Data_KZ2 tb_gdsh.data_kz2%type,
       v_Id out tb_gdsh.Id%type,
       v_result out number
)
is
       m_UDPId integer;
begin
       m_UDPId := to_number(fn_genseqnum('0050'));
       v_Id:= to_number(fn_genseqnum('0052'));
       insert into tb_udpdatabasicinfo (ID, CTIME, VERSION, FLAG, MAINTYPE, DATATYPE, SOURCE, DESTINATION, MISSIONCODE, SATELLITECODE, DATADATE, DATATIME, SEQUENCENUMBER, CHILDRENPACKNUMBER, RESERVE, DATALENGTH, DATACLASS)  
       values(m_UDPId,sysdate(),p_Version,p_Flag,p_Maintype,p_datatype,p_Source,p_Destination,p_missioncode,p_satellitecode,p_datadate,p_datatime,p_sequencenumber,p_childrenpacknumber,p_udpReserve,p_datalength,p_dataclass);
       
       insert into tb_gdsh (ID, CTIME, RESERVE, DATA_D, DATA_T, DATA_A, DATA_E, DATA_I, DATA_OHM, DATA_OMEGA, DATA_M, DATA_CDSM, DATA_KSM, DATA_KZ1, DATA_KZ2, DATA_ID)
       values(v_Id,sysdate(),p_Reserve,p_Data_D,p_Data_T,p_Data_A,p_Data_E,p_Data_I,p_Data_Ohm,p_Data_Omega,p_Data_M,p_Data_CDSM,p_Data_KSM,p_Data_KZ1,p_Data_KZ2,m_UDPId);
       
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_GDSH_insert;


--星地时差
create or replace procedure up_xdsc_insert
(
       p_Version  TB_UDPDATABASICINFO.Version%type,
       p_Flag  TB_UDPDATABASICINFO.Flag%type,
       p_Maintype  TB_UDPDATABASICINFO.Maintype%type,
       p_datatype  TB_UDPDATABASICINFO.Datatype%type,
       p_Source  TB_UDPDATABASICINFO.Source%type,
       p_Destination  TB_UDPDATABASICINFO.Destination%type,
       p_missioncode  TB_UDPDATABASICINFO.Missioncode%type,
       p_satellitecode  TB_UDPDATABASICINFO.Satellitecode%type,
       p_datadate  TB_UDPDATABASICINFO.Datadate%type,
       p_datatime  TB_UDPDATABASICINFO.Datatime%type,
       p_sequencenumber  TB_UDPDATABASICINFO.Sequencenumber%type,
       p_childrenpacknumber  TB_UDPDATABASICINFO.Childrenpacknumber%type,
       p_udpReserve  TB_UDPDATABASICINFO.Reserve%type,
       p_datalength  TB_UDPDATABASICINFO.Datalength%type,
       p_dataclass  TB_UDPDATABASICINFO.Dataclass%type,      
       p_Reserve tb_xdsc.Reserve%type,
       p_Data_D tb_xdsc.Data_d%type,
       p_Data_T tb_xdsc.Data_t%type,
       p_Data_N tb_xdsc.data_n%type,
       p_Data_DeltaT tb_xdsc.data_deltat%type,
       v_Id out tb_xdsc.Id%type,
       v_result out number
)
is
       m_UDPId integer;
begin
       m_UDPId := to_number(fn_genseqnum('0050'));
       v_Id:= to_number(fn_genseqnum('0053'));
       insert into tb_udpdatabasicinfo (ID, CTIME, VERSION, FLAG, MAINTYPE, DATATYPE, SOURCE, DESTINATION, MISSIONCODE, SATELLITECODE, DATADATE, DATATIME, SEQUENCENUMBER, CHILDRENPACKNUMBER, RESERVE, DATALENGTH, DATACLASS)  
       values(m_UDPId,sysdate(),p_Version,p_Flag,p_Maintype,p_datatype,p_Source,p_Destination,p_missioncode,p_satellitecode,p_datadate,p_datatime,p_sequencenumber,p_childrenpacknumber,p_udpReserve,p_datalength,p_dataclass);
       
       insert into tb_xdsc (ID, CTIME, RESERVE, DATA_D, DATA_T, DATA_N, DATA_DELTAT, DATA_ID)
       values(v_Id,sysdate(),p_Reserve,p_Data_D,p_Data_T,p_Data_N,p_Data_DeltaT,m_UDPId);
       
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_xdsc_insert;




--起飞零点
create or replace procedure up_t0_insert
(
       p_Version  TB_UDPDATABASICINFO.Version%type,
       p_Flag  TB_UDPDATABASICINFO.Flag%type,
       p_Maintype  TB_UDPDATABASICINFO.Maintype%type,
       p_datatype  TB_UDPDATABASICINFO.Datatype%type,
       p_Source  TB_UDPDATABASICINFO.Source%type,
       p_Destination  TB_UDPDATABASICINFO.Destination%type,
       p_missioncode  TB_UDPDATABASICINFO.Missioncode%type,
       p_satellitecode  TB_UDPDATABASICINFO.Satellitecode%type,
       p_datadate  TB_UDPDATABASICINFO.Datadate%type,
       p_datatime  TB_UDPDATABASICINFO.Datatime%type,
       p_sequencenumber  TB_UDPDATABASICINFO.Sequencenumber%type,
       p_childrenpacknumber  TB_UDPDATABASICINFO.Childrenpacknumber%type,
       p_udpReserve  TB_UDPDATABASICINFO.Reserve%type,
       p_datalength  TB_UDPDATABASICINFO.Datalength%type,
       p_dataclass  TB_UDPDATABASICINFO.Dataclass%type,      
       p_Reserve tb_t0.Reserve%type,
       p_Data_TZero tb_t0.data_tzero%type,
       v_Id out tb_t0.Id%type,
       v_result out number
)
is
       m_UDPId integer;
begin
       m_UDPId := to_number(fn_genseqnum('0050'));
       v_Id:= to_number(fn_genseqnum('0054'));
       insert into tb_udpdatabasicinfo (ID, CTIME, VERSION, FLAG, MAINTYPE, DATATYPE, SOURCE, DESTINATION, MISSIONCODE, SATELLITECODE, DATADATE, DATATIME, SEQUENCENUMBER, CHILDRENPACKNUMBER, RESERVE, DATALENGTH, DATACLASS)  
       values(m_UDPId,sysdate(),p_Version,p_Flag,p_Maintype,p_datatype,p_Source,p_Destination,p_missioncode,p_satellitecode,p_datadate,p_datatime,p_sequencenumber,p_childrenpacknumber,p_udpReserve,p_datalength,p_dataclass);
       
       insert into tb_t0 (ID, CTIME, RESERVE, DATA_TZero, DATA_ID)
       values(v_Id,sysdate(),p_Reserve,p_Data_TZero,m_UDPId);
       
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_t0_insert;


--测距数据
create or replace procedure up_r_insert
(
       p_Version  TB_UDPDATABASICINFO.Version%type,
       p_Flag  TB_UDPDATABASICINFO.Flag%type,
       p_Maintype  TB_UDPDATABASICINFO.Maintype%type,
       p_datatype  TB_UDPDATABASICINFO.Datatype%type,
       p_Source  TB_UDPDATABASICINFO.Source%type,
       p_Destination  TB_UDPDATABASICINFO.Destination%type,
       p_missioncode  TB_UDPDATABASICINFO.Missioncode%type,
       p_satellitecode  TB_UDPDATABASICINFO.Satellitecode%type,
       p_datadate  TB_UDPDATABASICINFO.Datadate%type,
       p_datatime  TB_UDPDATABASICINFO.Datatime%type,
       p_sequencenumber  TB_UDPDATABASICINFO.Sequencenumber%type,
       p_childrenpacknumber  TB_UDPDATABASICINFO.Childrenpacknumber%type,
       p_udpReserve  TB_UDPDATABASICINFO.Reserve%type,
       p_datalength  TB_UDPDATABASICINFO.Datalength%type,
       p_dataclass  TB_UDPDATABASICINFO.Dataclass%type,      
       p_Reserve tb_r.Reserve%type,
       p_Data_ZT tb_r.data_zt%type,
       p_Data_T tb_r.data_t%type,
       p_Data_RE tb_r.data_re%type,
       p_Data_DeltaT tb_r.data_deltat%type,
       p_Data_SPHI tb_r.data_sphi%type,
       v_Id out tb_r.Id%type,
       v_result out number
)
is
       m_UDPId integer;
begin
       m_UDPId := to_number(fn_genseqnum('0050'));
       v_Id:= to_number(fn_genseqnum('0055'));
       insert into tb_udpdatabasicinfo (ID, CTIME, VERSION, FLAG, MAINTYPE, DATATYPE, SOURCE, DESTINATION, MISSIONCODE, SATELLITECODE, DATADATE, DATATIME, SEQUENCENUMBER, CHILDRENPACKNUMBER, RESERVE, DATALENGTH, DATACLASS)  
       values(m_UDPId,sysdate(),p_Version,p_Flag,p_Maintype,p_datatype,p_Source,p_Destination,p_missioncode,p_satellitecode,p_datadate,p_datatime,p_sequencenumber,p_childrenpacknumber,p_udpReserve,p_datalength,p_dataclass);
       
       insert into tb_r (ID, CTIME, RESERVE, DATA_ZT, DATA_T, DATA_RE, DATA_DELTAT, DATA_SPHI, DATA_ID)
       values(v_Id,sysdate(),p_Reserve,p_Data_ZT,p_Data_T,p_Data_RE,p_Data_DeltaT,p_Data_SPHI,m_UDPId);
       
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_r_insert;


--测速数据
create or replace procedure up_rr_insert
(
       p_Version  TB_UDPDATABASICINFO.Version%type,
       p_Flag  TB_UDPDATABASICINFO.Flag%type,
       p_Maintype  TB_UDPDATABASICINFO.Maintype%type,
       p_datatype  TB_UDPDATABASICINFO.Datatype%type,
       p_Source  TB_UDPDATABASICINFO.Source%type,
       p_Destination  TB_UDPDATABASICINFO.Destination%type,
       p_missioncode  TB_UDPDATABASICINFO.Missioncode%type,
       p_satellitecode  TB_UDPDATABASICINFO.Satellitecode%type,
       p_datadate  TB_UDPDATABASICINFO.Datadate%type,
       p_datatime  TB_UDPDATABASICINFO.Datatime%type,
       p_sequencenumber  TB_UDPDATABASICINFO.Sequencenumber%type,
       p_childrenpacknumber  TB_UDPDATABASICINFO.Childrenpacknumber%type,
       p_udpReserve  TB_UDPDATABASICINFO.Reserve%type,
       p_datalength  TB_UDPDATABASICINFO.Datalength%type,
       p_dataclass  TB_UDPDATABASICINFO.Dataclass%type,      
       p_Reserve tb_rr.Reserve%type,
       p_Data_ZT tb_rr.data_zt%type,
       p_Data_T tb_rr.data_t%type,
       p_Data_RR tb_rr.data_rr%type,
       p_Data_DeltaF tb_rr.data_deltaf%type,
       p_Data_SPHI tb_rr.data_sphi%type,
       v_Id out tb_rr.Id%type,
       v_result out number
)
is
       m_UDPId integer;
begin
       m_UDPId := to_number(fn_genseqnum('0050'));
       v_Id:= to_number(fn_genseqnum('0056'));
       insert into tb_udpdatabasicinfo (ID, CTIME, VERSION, FLAG, MAINTYPE, DATATYPE, SOURCE, DESTINATION, MISSIONCODE, SATELLITECODE, DATADATE, DATATIME, SEQUENCENUMBER, CHILDRENPACKNUMBER, RESERVE, DATALENGTH, DATACLASS)  
       values(m_UDPId,sysdate(),p_Version,p_Flag,p_Maintype,p_datatype,p_Source,p_Destination,p_missioncode,p_satellitecode,p_datadate,p_datatime,p_sequencenumber,p_childrenpacknumber,p_udpReserve,p_datalength,p_dataclass);
       
       insert into tb_rr (ID, CTIME, RESERVE, DATA_ZT, DATA_T, DATA_RR, DATA_DELTAF, DATA_SPHI, DATA_ID)
       values(v_Id,sysdate(),p_Reserve,p_Data_ZT,p_Data_T,p_Data_RR,p_Data_DeltaF,p_Data_SPHI,m_UDPId);
       
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_rr_insert;



--测角数据
create or replace procedure up_ae_insert
(
       p_Version  TB_UDPDATABASICINFO.Version%type,
       p_Flag  TB_UDPDATABASICINFO.Flag%type,
       p_Maintype  TB_UDPDATABASICINFO.Maintype%type,
       p_datatype  TB_UDPDATABASICINFO.Datatype%type,
       p_Source  TB_UDPDATABASICINFO.Source%type,
       p_Destination  TB_UDPDATABASICINFO.Destination%type,
       p_missioncode  TB_UDPDATABASICINFO.Missioncode%type,
       p_satellitecode  TB_UDPDATABASICINFO.Satellitecode%type,
       p_datadate  TB_UDPDATABASICINFO.Datadate%type,
       p_datatime  TB_UDPDATABASICINFO.Datatime%type,
       p_sequencenumber  TB_UDPDATABASICINFO.Sequencenumber%type,
       p_childrenpacknumber  TB_UDPDATABASICINFO.Childrenpacknumber%type,
       p_udpReserve  TB_UDPDATABASICINFO.Reserve%type,
       p_datalength  TB_UDPDATABASICINFO.Datalength%type,
       p_dataclass  TB_UDPDATABASICINFO.Dataclass%type,      
       p_Reserve tb_ae.Reserve%type,
       p_Data_ZT tb_ae.data_zt%type,
       p_Data_T tb_ae.data_t%type,
       p_Data_A tb_ae.data_a%type,
       p_Data_E tb_ae.data_e%type,
       p_Data_DeltaA1 tb_ae.data_deltaa1%type,
       p_Data_DeltaE1 tb_ae.data_deltae1%type,
       p_Data_DeltaA2 tb_ae.data_deltaa2%type,
       p_Data_DeltaE2 tb_ae.data_deltae2%type,
       p_Data_SPHI tb_ae.data_sphi%type,
       v_Id out tb_ae.Id%type,
       v_result out number
)
is
       m_UDPId integer;
begin
       m_UDPId := to_number(fn_genseqnum('0050'));
       v_Id:= to_number(fn_genseqnum('0057'));
       insert into tb_udpdatabasicinfo (ID, CTIME, VERSION, FLAG, MAINTYPE, DATATYPE, SOURCE, DESTINATION, MISSIONCODE, SATELLITECODE, DATADATE, DATATIME, SEQUENCENUMBER, CHILDRENPACKNUMBER, RESERVE, DATALENGTH, DATACLASS)  
       values(m_UDPId,sysdate(),p_Version,p_Flag,p_Maintype,p_datatype,p_Source,p_Destination,p_missioncode,p_satellitecode,p_datadate,p_datatime,p_sequencenumber,p_childrenpacknumber,p_udpReserve,p_datalength,p_dataclass);
       
       insert into tb_ae (ID, CTIME, RESERVE, DATA_ZT, DATA_T, DATA_A, DATA_E, DATA_DELTAA1, DATA_DELTAE1, DATA_DELTAA2, DATA_DELTAE2, DATA_SPHI, DATA_ID)
       values(v_Id,sysdate(),p_Reserve,p_Data_ZT,p_Data_T,p_Data_A,p_Data_E,p_Data_DeltaA1,p_Data_DeltaE1,p_Data_DeltaA2,p_Data_DeltaE2,p_Data_SPHI,m_UDPId);
       
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_ae_insert;


--空间遥操作状态数据
create or replace procedure up_czzt_insert
(
       p_Version  TB_UDPDATABASICINFO.Version%type,
       p_Flag  TB_UDPDATABASICINFO.Flag%type,
       p_Maintype  TB_UDPDATABASICINFO.Maintype%type,
       p_datatype  TB_UDPDATABASICINFO.Datatype%type,
       p_Source  TB_UDPDATABASICINFO.Source%type,
       p_Destination  TB_UDPDATABASICINFO.Destination%type,
       p_missioncode  TB_UDPDATABASICINFO.Missioncode%type,
       p_satellitecode  TB_UDPDATABASICINFO.Satellitecode%type,
       p_datadate  TB_UDPDATABASICINFO.Datadate%type,
       p_datatime  TB_UDPDATABASICINFO.Datatime%type,
       p_sequencenumber  TB_UDPDATABASICINFO.Sequencenumber%type,
       p_childrenpacknumber  TB_UDPDATABASICINFO.Childrenpacknumber%type,
       p_udpReserve  TB_UDPDATABASICINFO.Reserve%type,
       p_datalength  TB_UDPDATABASICINFO.Datalength%type,
       p_dataclass  TB_UDPDATABASICINFO.Dataclass%type,      
       p_Reserve tb_czzt.Reserve%type,
       p_Data_BJ tb_czzt.data_bj%type,
       p_Data_ZX tb_czzt.data_zx%type,
       p_Data_T0 tb_czzt.data_t0%type,
       p_Data_T1 tb_czzt.data_t1%type,
       p_Data_A tb_czzt.data_a%type,
       p_Data_E tb_czzt.data_e%type,
       p_Data_I tb_czzt.data_i%type,
       p_Data_OHM tb_czzt.data_ohm%type,
       p_Data_OMEGA tb_czzt.data_omega%type,
       v_Id out tb_czzt.Id%type,
       v_result out number
)
is
       m_UDPId integer;
begin
       m_UDPId := to_number(fn_genseqnum('0050'));
       v_Id:= to_number(fn_genseqnum('0058'));
       insert into tb_udpdatabasicinfo (ID, CTIME, VERSION, FLAG, MAINTYPE, DATATYPE, SOURCE, DESTINATION, MISSIONCODE, SATELLITECODE, DATADATE, DATATIME, SEQUENCENUMBER, CHILDRENPACKNUMBER, RESERVE, DATALENGTH, DATACLASS)  
       values(m_UDPId,sysdate(),p_Version,p_Flag,p_Maintype,p_datatype,p_Source,p_Destination,p_missioncode,p_satellitecode,p_datadate,p_datatime,p_sequencenumber,p_childrenpacknumber,p_udpReserve,p_datalength,p_dataclass);
       
       insert into tb_czzt (ID, CTIME, RESERVE, DATA_BJ, DATA_ZX, DATA_T0, DATA_T1, DATA_A, DATA_E, DATA_I, DATA_OHM, DATA_OMEGA, DATA_ID)
       values(v_Id,sysdate(),p_Reserve,p_Data_BJ,p_Data_ZX,p_Data_T0,p_Data_T1,p_Data_A,p_Data_E,p_Data_I,p_Data_OHM,p_Data_OMEGA,m_UDPId);
       
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK;
          COMMIT;
          v_result:=4; --Error
end up_czzt_insert;

create or replace procedure up_GD_Getlist
(
       p_startTime in TB_GD.Ctime%type,
       p_endTime in TB_GD.Ctime%type,
       o_cursor out sys_refcursor
) is
begin
   OPEN o_cursor FOR
  SELECT
          t.ID,   
          t.CTIME,     
          t.RESERVE,   
          t.DATA_D,    
          t.DATA_T,    
          t.DATA_A,    
          t.DATA_E,    
          t.DATA_I,
          t.DATA_OHM,
          t.DATA_OMEGA,
          t.DATA_M,
          t.DATA_P,
          t.DATA_PI,
          t.DATA_RA,
          t.DATA_RP,
          t.DATA_ID
  FROM  TB_GD t
  WHERE
  t.ctime >= p_startTime
  AND t.ctime <= p_endTime


;
end up_GD_Getlist;

create or replace procedure up_gdsh_Getlist
(
       p_startTime in TB_GDSH.Ctime%type,
       p_endTime in TB_GDSH.Ctime%type,
       o_cursor out sys_refcursor
) is
begin
   OPEN o_cursor FOR
  SELECT
          t.ID,       
          t.CTIME,    
          t.RESERVE,   
          t.DATA_D,   
          t.DATA_T,    
          t.DATA_A,    
          t.DATA_E,    
          t.DATA_I,    
          t.DATA_OHM,  
          t.DATA_OMEGA,
          t.DATA_M,    
          t.DATA_CDSM, 
          t.DATA_KSM,  
          t.DATA_KZ1,  
          t.DATA_KZ2,  
          t.DATA_ID 
  FROM  TB_GDSH t
  WHERE
  t.ctime >= p_startTime
  AND t.ctime <= p_endTime


;
end up_gdsh_Getlist;

create or replace procedure up_XDSC_Getlist
(
       p_startTime in TB_XDSC.Ctime%type,
       p_endTime in TB_XDSC.Ctime%type,
       o_cursor out sys_refcursor
) is
begin
   OPEN o_cursor FOR
  SELECT
          t.ID,        
          t.CTIME,      
          t.RESERVE,    
          t.DATA_D,     
          t.DATA_T,     
          t.DATA_N,     
          t.DATA_DELTAT,
          t.DATA_ID    
  FROM  TB_XDSC t
  WHERE
  t.ctime >= p_startTime
  AND t.ctime <= p_endTime


;
end up_XDSC_Getlist;

create or replace procedure up_T0_Getlist
(
       p_startTime in TB_T0.Ctime%type,
       p_endTime in TB_T0.Ctime%type,
       o_cursor out sys_refcursor
) is
begin
   OPEN o_cursor FOR
  SELECT
          t.ID,
          t.CTIME,
          t.RESERVE,
          t.data_tzero,
          t.data_id
  FROM  TB_T0 t
  WHERE
  t.ctime >= p_startTime
  AND t.ctime <= p_endTime


;
end up_T0_Getlist;


create or replace procedure up_r_Getlist
(
       p_startTime in TB_r.Ctime%type,
       p_endTime in TB_r.Ctime%type,
       o_cursor out sys_refcursor
) is
begin
   OPEN o_cursor FOR
  SELECT
          t.ID,     
          t.CTIME,      
          t.RESERVE,    
          t.DATA_ZT,
          t.DATA_T,     
          t.DATA_RE,    
          t.DATA_DELTAT,
          t.DATA_SPHI,  
          t.DATA_ID          
  FROM  TB_r t
  WHERE
  t.ctime >= p_startTime
  AND t.ctime <= p_endTime


;
end up_r_Getlist;

create or replace procedure up_rr_Getlist
(
       p_startTime in TB_rr.Ctime%type,
       p_endTime in TB_rr.Ctime%type,
       o_cursor out sys_refcursor
) is
begin
   OPEN o_cursor FOR
  SELECT
          t.ID,         
          t.CTIME,      
          t.RESERVE,    
          t.DATA_ZT,    
          t.DATA_T,     
          t.DATA_RR,    
          t.DATA_DELTAF,
          t.DATA_SPHI,  
          t.DATA_ID         
  FROM  TB_rr t
  WHERE
  t.ctime >= p_startTime
  AND t.ctime <= p_endTime


;
end up_rr_Getlist;


create or replace procedure up_ae_Getlist
(
       p_startTime in TB_ae.Ctime%type,
       p_endTime in TB_ae.Ctime%type,
       o_cursor out sys_refcursor
) is
begin
   OPEN o_cursor FOR
  SELECT
          t.ID,          
          t.CTIME,        
          t.RESERVE,      
          t.DATA_ZT,      
          t.DATA_T,       
          t.DATA_A,       
          t.DATA_E,       
          t.DATA_DELTAA1, 
          t.DATA_DELTAE1, 
          t.DATA_DELTAA2, 
          t.DATA_DELTAE2, 
          t.DATA_SPHI,    
          t.DATA_ID
  FROM  TB_ae t
  WHERE
  t.ctime >= p_startTime
  AND t.ctime <= p_endTime


;
end up_ae_Getlist;

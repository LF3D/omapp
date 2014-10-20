prompt PL/SQL Developer import file
prompt Created on 2012年4月25日 by Cindy
set feedback off
set define off
prompt Dropping TB_XYXSINFO...
drop table TB_XYXSINFO cascade constraints;
prompt Creating TB_XYXSINFO...
create table TB_XYXSINFO
(
  rid      NUMBER(5) not null,
  addrname VARCHAR2(100) not null,
  addrmark VARCHAR2(10),
  incode   VARCHAR2(10) not null,
  excode   VARCHAR2(20) not null,
  mainip   VARCHAR2(15),
  tcpport  NUMBER(8),
  bakip    VARCHAR2(15),
  udpport  NUMBER(8),
  ftppath  VARCHAR2(50)
)
tablespace TSHTC
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );
comment on column TB_XYXSINFO.rid
  is '记录号';
comment on column TB_XYXSINFO.addrname
  is '地址名称';
comment on column TB_XYXSINFO.addrmark
  is '地址标识';
comment on column TB_XYXSINFO.incode
  is '内部十六进制编码';
comment on column TB_XYXSINFO.excode
  is '外部十六进制编码';
comment on column TB_XYXSINFO.mainip
  is '主IP地址';
comment on column TB_XYXSINFO.tcpport
  is 'TCP端口';
comment on column TB_XYXSINFO.bakip
  is '备用IP地址';
comment on column TB_XYXSINFO.udpport
  is 'UDP端口';
comment on column TB_XYXSINFO.ftppath
  is 'FTP路径@用户名@口令';

prompt Disabling triggers for TB_XYXSINFO...
alter table TB_XYXSINFO disable all triggers;
prompt 23 records loaded
prompt Enabling triggers for TB_XYXSINFO...
alter table TB_XYXSINFO enable all triggers;
set feedback on
set define on
prompt Done.

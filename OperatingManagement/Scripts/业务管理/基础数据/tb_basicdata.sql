---------------------------------------------
-- Export file for user HTCUSER            --
-- Created by Cindy on 2012/9/12, 15:14:32 --
---------------------------------------------

spool tb_basicdata.log

prompt
prompt Creating table TB_SATELLITE
prompt ===========================
prompt
create table TB_SATELLITE
(
  wxmc   VARCHAR2(50) not null,
  wxbm   VARCHAR2(10) not null,
  wxbs   VARCHAR2(10) not null,
  state  NUMBER(1) not null,
  mzb    NUMBER(8) not null,
  bmfsxs NUMBER(8) not null,
  sx     VARCHAR2(4000),
  gn     VARCHAR2(1000),
  ctime  DATE
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
comment on column TB_SATELLITE.wxmc
  is '卫星名称';
comment on column TB_SATELLITE.wxbm
  is '卫星编码';
comment on column TB_SATELLITE.wxbs
  is '卫星标识';
comment on column TB_SATELLITE.state
  is '状态：0-可用；1-不可用';
comment on column TB_SATELLITE.mzb
  is '面质比';
comment on column TB_SATELLITE.bmfsxs
  is '表面反射系数';
comment on column TB_SATELLITE.sx
  is '属性';
comment on column TB_SATELLITE.gn
  is '功能';
comment on column TB_SATELLITE.ctime
  is '创建时间';
alter table TB_SATELLITE
  add constraint PK_TB_SATELLITE primary key (WXBM)
  using index 
  tablespace TSHTC
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table TB_TASK
prompt ======================
prompt
create table TB_TASK
(
  id         NUMBER,
  taskname   VARCHAR2(50) not null,
  taskno     VARCHAR2(50) not null,
  objectflag VARCHAR2(20) not null,
  satid      VARCHAR2(50) not null,
  iscurtask  NUMBER(1) not null,
  begintime  DATE not null,
  endtime    DATE not null,
  ctime      DATE
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
comment on column TB_TASK.taskname
  is '名称';
comment on column TB_TASK.taskno
  is '代号';
comment on column TB_TASK.objectflag
  is '对象标识';
comment on column TB_TASK.satid
  is '卫星编码，可以为多个，“;”分割';
comment on column TB_TASK.iscurtask
  is '是否为当前任务，1是，0否，本表中只有一条为1的';
comment on column TB_TASK.begintime
  is '任务开始时间';
comment on column TB_TASK.endtime
  is '任务结束时间';
comment on column TB_TASK.ctime
  is '创建时间';
alter table TB_TASK
  add constraint PK_TB_TASK primary key (TASKNO)
  using index 
  tablespace TSHTC
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table TB_ZYGN
prompt ======================
prompt
create table TB_ZYGN
(
  id        NUMBER(3) not null,
  fname     VARCHAR2(50),
  fcode     VARCHAR2(20),
  matchrule VARCHAR2(500)
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
comment on column TB_ZYGN.fname
  is '名称';
comment on column TB_ZYGN.fcode
  is '编码';
comment on column TB_ZYGN.matchrule
  is '匹配准则';
alter table TB_ZYGN
  add constraint PK_TB_ZYGN primary key (ID)
  using index 
  tablespace TSHTC
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table TB_ZYSX
prompt ======================
prompt
create table TB_ZYSX
(
  id    NUMBER(4) not null,
  pname VARCHAR2(50) not null,
  pcode VARCHAR2(20) not null,
  type  NUMBER(1),
  scope VARCHAR2(20),
  own   NUMBER(1)
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
comment on column TB_ZYSX.type
  is '属性类型：1,int;2double;3,string;4,bool;5,enum';
comment on column TB_ZYSX.scope
  is '值区间：type:1,0-xxxxxx;type:2,m.n;3,length;5,a,b,c,;';
comment on column TB_ZYSX.own
  is '属于：0：卫星，1：地面站；2卫星和地面站；3都不属于';
alter table TB_ZYSX
  add constraint PK_ZYSX primary key (ID)
  using index 
  tablespace TSHTC
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating sequence SEQ_TB_SATELLITE
prompt ==================================
prompt
create sequence SEQ_TB_SATELLITE
minvalue 1
maxvalue 999999999999999999999999999
start with 1
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_TB_TASK
prompt =============================
prompt
create sequence SEQ_TB_TASK
minvalue 1
maxvalue 999999999999999999999999999
start with 21
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_TB_ZYSX
prompt =============================
prompt
create sequence SEQ_TB_ZYSX
minvalue 1
maxvalue 999999999999999999999999999
start with 1
increment by 1
cache 20;

prompt
prompt Creating sequence SEQ_TB_ZYGN
prompt =============================
prompt
create sequence SEQ_TB_ZYGN
minvalue 1
maxvalue 999999999999999999999999999
start with 1
increment by 1
cache 20;

spool off

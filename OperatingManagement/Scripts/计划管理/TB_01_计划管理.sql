--------------------------------------------
-- Export file for user HTCUSER           --
-- Created by taiji on 2012/9/6, 14:46:20 --
--------------------------------------------

spool UP_01_计划管理20120906.log

prompt
prompt Creating table TB_GD
prompt ====================
prompt
create table HTCUSER.TB_GD
(
  ID       NUMBER(20) not null,
  CTIME    DATE not null,
  TASKID   VARCHAR2(20),
  SATID    VARCHAR2(50),
  ITYPE    NUMBER,
  ICODE    VARCHAR2(10),
  D        NUMBER(8),
  T        NUMBER(10),
  TIMES    DATE,
  A        NUMBER(12,6),
  E        NUMBER(12,6),
  I        NUMBER(12,6),
  Q        NUMBER(12,6),
  W        NUMBER(12,6),
  M        NUMBER(12,6),
  P        NUMBER(12,6),
  PP       NUMBER(12,6),
  RA       NUMBER(12,6),
  RP       NUMBER(12,6),
  CDSM     NUMBER(12,6),
  KZ1      NUMBER(12,6),
  KSM      NUMBER(12,6),
  KZ2      NUMBER(12,6),
  RESERVE  VARCHAR2(100),
  DFINFOID NUMBER(20)
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
comment on column HTCUSER.TB_GD.SATID
  is '卫星代号';
comment on column HTCUSER.TB_GD.ITYPE
  is '数据类型：0-卫星初始轨道根数；1-卫星瞬时精轨根数；2-卫星事后精轨根数';
comment on column HTCUSER.TB_GD.ICODE
  is '数据编码';
comment on column HTCUSER.TB_GD.D
  is '历元日期';
comment on column HTCUSER.TB_GD.T
  is '历元时刻';
comment on column HTCUSER.TB_GD.TIMES
  is '历元时间（历元日期和历元时刻组合的时间）';
comment on column HTCUSER.TB_GD.A
  is '轨道半长轴';
comment on column HTCUSER.TB_GD.E
  is '轨道偏心率';
comment on column HTCUSER.TB_GD.I
  is '轨道倾角';
comment on column HTCUSER.TB_GD.Q
  is '升交点赤经，单位度';
comment on column HTCUSER.TB_GD.W
  is '近地点幅角，单位度';
comment on column HTCUSER.TB_GD.M
  is '平近点角，单位为度';
comment on column HTCUSER.TB_GD.P
  is '轨道周期，单位为分钟';
comment on column HTCUSER.TB_GD.PP
  is '轨道周期变化率，单位秒/天';
comment on column HTCUSER.TB_GD.RA
  is '远地点地心距';
comment on column HTCUSER.TB_GD.RP
  is '近地点地心距';
comment on column HTCUSER.TB_GD.CDSM
  is '大气阻力摄动系数项，单位为米2/千克';
comment on column HTCUSER.TB_GD.KZ1
  is '扩展字1';
comment on column HTCUSER.TB_GD.KSM
  is '光压摄动系数项，单位为米2/千克';
comment on column HTCUSER.TB_GD.KZ2
  is '扩展字2';
alter table HTCUSER.TB_GD
  add constraint PK_TB_GD primary key (ID)
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
prompt Creating table TB_JH
prompt ====================
prompt
create table HTCUSER.TB_JH
(
  ID        NUMBER(20) not null,
  CTIME     DATE not null,
  TASKID    VARCHAR2(20),
  PLANTYPE  VARCHAR2(20),
  PLANID    NUMBER(10),
  STARTTIME DATE,
  ENDTIME   DATE,
  SRCTYPE   NUMBER(1),
  SRCID     NUMBER(20),
  FILEINDEX VARCHAR2(100),
  RESERVE   VARCHAR2(100)
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
comment on column HTCUSER.TB_JH.SRCTYPE
  is '计划源类型：空白计划0；试验程序1；设备工作计划2；';
comment on column HTCUSER.TB_JH.SRCID
  is '计划源编号：当计划源类型为1时，为试验程序编号；计划源类型为2时，为设备工作计划编号。';
alter table HTCUSER.TB_JH
  add constraint PK_TB_JH primary key (ID)
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
prompt Creating table TB_JHTEMP
prompt ========================
prompt
create table HTCUSER.TB_JHTEMP
(
  ID        NUMBER(20) not null,
  CTIME     DATE not null,
  TASKID    VARCHAR2(20),
  PLANTYPE  VARCHAR2(20),
  PLANID    NUMBER(10),
  STARTTIME DATE,
  ENDTIME   DATE,
  SRCTYPE   NUMBER(1),
  SRCID     NUMBER(20),
  FILEINDEX VARCHAR2(100),
  RESERVE   VARCHAR2(100)
)
tablespace TSHTC
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 16K
    minextents 1
    maxextents unlimited
  );
comment on column HTCUSER.TB_JHTEMP.SRCTYPE
  is '计划源类型：空白计划0；试验程序1；设备工作计划2；';
comment on column HTCUSER.TB_JHTEMP.SRCID
  is '计划源编号：当计划源类型为1时，为试验程序编号；计划源类型为2时，为设备工作计划编号。';
alter table HTCUSER.TB_JHTEMP
  add constraint PK_TB_JHTEMP primary key (ID)
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
prompt Creating table TB_SYCX
prompt ======================
prompt
create table HTCUSER.TB_SYCX
(
  ID        NUMBER(20) not null,
  CTIME     DATE not null,
  TASKID    VARCHAR2(20),
  PTYPE     VARCHAR2(20),
  PNAME     VARCHAR2(100),
  PNID      NUMBER(10),
  PLANID    NUMBER(10),
  STARTTIME DATE,
  ENDTIME   DATE,
  FILEINDEX VARCHAR2(100),
  RESERVE   VARCHAR2(100),
  INFOTYPE  VARCHAR2(50),
  LINECOUNT NUMBER(10)
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
alter table HTCUSER.TB_SYCX
  add constraint PK_TB_SYCX primary key (ID)
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
prompt Creating table TB_YDSJ
prompt ======================
prompt
create table HTCUSER.TB_YDSJ
(
  ID      NUMBER(20) not null,
  CTIME   DATE not null,
  TASKID  VARCHAR2(20),
  SATNAME VARCHAR2(50),
  D       NUMBER(8),
  T       NUMBER(10),
  TIMES   DATE,
  A       NUMBER(12,6),
  E       NUMBER(12,6),
  I       NUMBER(12,6),
  O       NUMBER(12,6),
  W       NUMBER(12,6),
  M       NUMBER(12,6),
  RESERVE VARCHAR2(100)
)
tablespace TSHTC
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 16K
    minextents 1
    maxextents unlimited
  );
comment on column HTCUSER.TB_YDSJ.CTIME
  is '记录创建时间';
comment on column HTCUSER.TB_YDSJ.TASKID
  is '任务代号';
comment on column HTCUSER.TB_YDSJ.SATNAME
  is '卫星名称';
comment on column HTCUSER.TB_YDSJ.D
  is '历元日期';
comment on column HTCUSER.TB_YDSJ.T
  is '历元时间';
comment on column HTCUSER.TB_YDSJ.A
  is '轨道半长径';
comment on column HTCUSER.TB_YDSJ.E
  is '轨道偏心率';
comment on column HTCUSER.TB_YDSJ.I
  is '轨道倾角';
comment on column HTCUSER.TB_YDSJ.O
  is '轨道升交点赤径';
comment on column HTCUSER.TB_YDSJ.W
  is '轨道近地点幅角';
comment on column HTCUSER.TB_YDSJ.M
  is '平近点角';
comment on column HTCUSER.TB_YDSJ.RESERVE
  is '备注';
alter table HTCUSER.TB_YDSJ
  add constraint PK_TB_YDSJ primary key (ID)
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


spool off

-- Create table
create table TB_GD
(
  ID         NUMBER(20) not null,
  CTIME      DATE not null,
  RESERVE    VARCHAR2(100),
  DATA_D     VARCHAR2(10),
  DATA_T     VARCHAR2(10),
  DATA_A     VARCHAR2(10),
  DATA_E     VARCHAR2(10),
  DATA_I     VARCHAR2(10),
  DATA_OHM   VARCHAR2(10),
  DATA_OMEGA VARCHAR2(10),
  DATA_M     VARCHAR2(10),
  DATA_P     VARCHAR2(10),
  DATA_PI    VARCHAR2(10),
  DATA_RA    VARCHAR2(10),
  DATA_RP    VARCHAR2(10),
  DATA_ID    NUMBER(20)
)
tablespace TSHTC
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 64
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_GD
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


-- Create table
create table TB_GDSH
(
  ID         NUMBER(20) not null,
  CTIME      DATE not null,
  RESERVE    VARCHAR2(100),
  DATA_D     VARCHAR2(10),
  DATA_T     VARCHAR2(10),
  DATA_A     VARCHAR2(10),
  DATA_E     VARCHAR2(10),
  DATA_I     VARCHAR2(10),
  DATA_OHM   VARCHAR2(10),
  DATA_OMEGA VARCHAR2(10),
  DATA_M     VARCHAR2(10),
  DATA_CDSM  VARCHAR2(10),
  DATA_KSM   VARCHAR2(10),
  DATA_KZ1   VARCHAR2(10),
  DATA_KZ2   VARCHAR2(10),
  DATA_ID    NUMBER(20)
)
tablespace TSHTC
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 16
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_GDSH
  add constraint PK_TB_GDSH primary key (ID)
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



-- Create table
create table TB_XDSC
(
  ID          NUMBER(20) not null,
  CTIME       DATE not null,
  RESERVE     VARCHAR2(100),
  DATA_D      DATE,
  DATA_T      VARCHAR2(10),
  DATA_N      VARCHAR2(10),
  DATA_DELTAT VARCHAR2(10),
  DATA_ID     NUMBER(20)
)
tablespace TSHTC
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 16
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_XDSC
  add constraint PK_TB_XDSC primary key (ID)
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


-- Create table
create table TB_T0
(
  ID         NUMBER(20) not null,
  CTIME      DATE not null,
  RESERVE    VARCHAR2(100),
  DATA_TZERO VARCHAR2(10),
  DATA_ID    NUMBER(20)
)
tablespace TSHTC
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 16
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_T0
  add constraint PK_TB_T0 primary key (ID)
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


-- Create table
create table TB_R
(
  ID          NUMBER(20) not null,
  CTIME       DATE not null,
  RESERVE     VARCHAR2(100),
  DATA_ZT     VARCHAR2(10),
  DATA_T      VARCHAR2(10),
  DATA_RE     VARCHAR2(10),
  DATA_DELTAT VARCHAR2(10),
  DATA_SPHI   VARCHAR2(10),
  DATA_ID     NUMBER(20)
)
tablespace TSHTC
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 16
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_R
  add constraint PK_TB_R primary key (ID)
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



-- Create table
create table TB_RR
(
  ID          NUMBER(20) not null,
  CTIME       DATE not null,
  RESERVE     VARCHAR2(100),
  DATA_ZT     VARCHAR2(10),
  DATA_T      VARCHAR2(10),
  DATA_RR     VARCHAR2(10),
  DATA_DELTAF VARCHAR2(10),
  DATA_SPHI   VARCHAR2(10),
  DATA_ID     NUMBER(20)
)
tablespace TSHTC
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 16
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_RR
  add constraint PK_TB_RR primary key (ID)
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


-- Create table
create table TB_AE
(
  ID           NUMBER(20) not null,
  CTIME        DATE not null,
  RESERVE      VARCHAR2(100),
  DATA_ZT      VARCHAR2(10),
  DATA_T       VARCHAR2(10),
  DATA_A       VARCHAR2(10),
  DATA_E       VARCHAR2(10),
  DATA_DELTAA1 VARCHAR2(10),
  DATA_DELTAE1 VARCHAR2(10),
  DATA_DELTAA2 VARCHAR2(10),
  DATA_DELTAE2 VARCHAR2(10),
  DATA_SPHI    VARCHAR2(10),
  DATA_ID      NUMBER(20)
)
tablespace TSHTC
  pctfree 10
  initrans 1
  maxtrans 255
  storage
  (
    initial 16
    minextents 1
    maxextents unlimited
  );
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_AE
  add constraint PK_TB_AE primary key (ID)
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

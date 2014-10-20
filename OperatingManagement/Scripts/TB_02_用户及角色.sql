/*========================================*/
/* Table: 用户表                                      */
/*========================================*/
create table TB_USER
(
  USERID         NUMBER(10) not null,
  LOGINNAME      VARCHAR2(50) not null,
  DISPLAYNAME       VARCHAR2(20) not null,
  PASSWORD1       VARCHAR2(200) not null,
  USERTYPE       NUMBER(1) not null,
  STATUS         NUMBER(1) not null,
  MOBILE         VARCHAR2(15),
  NOTE        VARCHAR2(100),
  CTIME          DATE not null,
  LASTUPDATEDTIME DATE not null,
  USERCATALOG      NUMBER(1) not null
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
alter table TB_USER
  add constraint PK_TB_USER primary key (USERID)
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
/*========================================*/
/* Table: 角色表                                      */
/*========================================*/
create table TB_ROLE
(
  ROLEID   NUMBER(10) not null,
  ROLENAME VARCHAR2(20) not null,
  NOTE  VARCHAR2(100),
  CTIME    DATE not null
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
alter table TB_ROLE
  add constraint PK_TB_ROLE primary key (ROLEID)
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

/*========================================*/
/* Table: 模块表                                      */
/*========================================*/
create table TB_MODULE
(
  MODULEID    NUMBER(10) not null,
  MODULENAME VARCHAR2(20) not null,
  NOTE  VARCHAR2(100),
  CTIME    DATE not null
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
alter table TB_MODULE
  add constraint PK_TB_MODULE primary key (MODULEID)
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

/*========================================*/
/* Table: 功能表                                      */
/*========================================*/
create table TB_ACTION
(
  ACTIONID    NUMBER(10) not null,
  ACTIONNAME VARCHAR2(20) not null,
  NOTE  VARCHAR2(100),
  CTIME  DATE not null
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
alter table TB_ACTION
  add constraint PK_TB_ACTION primary key (ACTIONID)
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

/*========================================*/
/* Table: 权限表                                      */
/*========================================*/
create table TB_PERMISSION
(
  PERMISSIONID    NUMBER(10) not null,
  MODULEID    NUMBER(10) not null,
  ACTIONID   NUMBER(10) not null
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

/*========================================*/
/* Table: 角色权限关联表                                      */
/*========================================*/
create table TB_ROLEPERMISSION
(
  ROLEID    NUMBER(10) not null,
  PERMISSIONID   NUMBER(10) not null
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

/*========================================*/
/* Table: 用户角色表                                      */
/*========================================*/
create table TB_USERROLE
(
  USERID NUMBER(10) not null,
  ROLEID NUMBER(10) not null,
  CTIME  DATE not null
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

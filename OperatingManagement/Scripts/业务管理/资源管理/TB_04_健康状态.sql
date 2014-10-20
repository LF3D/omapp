-- Create table
create table TB_HEALTHSTATUS
(
  hsid          NUMBER(10) not null,
  resourceid    NUMBER(10) not null,
  resourcetype  NUMBER(2) not null,
  functiontype  NVARCHAR2(50),
  status        NUMBER(2) not null,
  begintime     DATE,
  endtime       DATE,
  createdtime   DATE not null,
  createduserid NUMBER(10),
  updatedtime   DATE,
  updateduserid NUMBER(10)
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
-- Add comments to the columns 
comment on column TB_HEALTHSTATUS.hsid
  is '序号';
comment on column TB_HEALTHSTATUS.resourceid
  is '资源序号';
comment on column TB_HEALTHSTATUS.resourcetype
  is '资源类型';
comment on column TB_HEALTHSTATUS.functiontype
  is '功能类型';
comment on column TB_HEALTHSTATUS.status
  is '健康状态';
comment on column TB_HEALTHSTATUS.begintime
  is '起始时间';
comment on column TB_HEALTHSTATUS.endtime
  is '结束时间';
comment on column TB_HEALTHSTATUS.createdtime
  is '创建时间';
comment on column TB_HEALTHSTATUS.createduserid
  is '创建用户ID';
comment on column TB_HEALTHSTATUS.updatedtime
  is '最后修改时间';
comment on column TB_HEALTHSTATUS.updateduserid
  is '修改用户';
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_HEALTHSTATUS
  add constraint PK_TB_HEALTHSTATUS primary key (HSID)
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

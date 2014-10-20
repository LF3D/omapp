-- Create table
create table TB_USESTATUS
(
  usid          NUMBER(10) not null,
  resourceid    NUMBER(10) not null,
  resourcetype  NUMBER(2) not null,
  usedtype      NUMBER(2) not null,
  begintime     DATE not null,
  endtime       DATE not null,
  usedby        NVARCHAR2(50),
  usedcategory  NVARCHAR2(50),
  usedfor       NVARCHAR2(100),
  canbeused     NUMBER(2),
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
comment on column TB_USESTATUS.usid
  is '序号';
comment on column TB_USESTATUS.resourceid
  is '资源序号';
comment on column TB_USESTATUS.resourcetype
  is '资源类型';
comment on column TB_USESTATUS.usedtype
  is '占用类型';
comment on column TB_USESTATUS.begintime
  is '起始时间';
comment on column TB_USESTATUS.endtime
  is '结束时间';
comment on column TB_USESTATUS.usedby
  is '服务对象';
comment on column TB_USESTATUS.usedcategory
  is '服务种类';
comment on column TB_USESTATUS.usedfor
  is '占用原因';
comment on column TB_USESTATUS.canbeused
  is '是否可执行任务';
comment on column TB_USESTATUS.createdtime
  is '创建时间';
comment on column TB_USESTATUS.createduserid
  is '创建用户';
comment on column TB_USESTATUS.updatedtime
  is '最后修改时间';
comment on column TB_USESTATUS.updateduserid
  is '修改用户';
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_USESTATUS
  add constraint PK_TB_USESTATUS primary key (USID)
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

-- Create table
create table TB_CENTERRESOURCE
(
  crid          NUMBER(10) not null,
  equipmentcode NVARCHAR2(50) not null,
  equipmenttype NVARCHAR2(50) not null,
  supporttask   NVARCHAR2(50) not null,
  dataprocess   NUMBER(12,2) not null,
  status        NUMBER(2) not null,
  extproperties NVARCHAR2(2000),
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
comment on column TB_CENTERRESOURCE.crid
  is '序号';
comment on column TB_CENTERRESOURCE.equipmentcode
  is '设备编号';
comment on column TB_CENTERRESOURCE.equipmenttype
  is '设备类型';
comment on column TB_CENTERRESOURCE.supporttask
  is '支持的任务';
comment on column TB_CENTERRESOURCE.dataprocess
  is '最大数据处理量';
comment on column TB_CENTERRESOURCE.status
  is '状态';
comment on column TB_CENTERRESOURCE.extproperties
  is '扩展属性';
comment on column TB_CENTERRESOURCE.createdtime
  is '创建时间';
comment on column TB_CENTERRESOURCE.createduserid
  is '创建用户';
comment on column TB_CENTERRESOURCE.updatedtime
  is '最后修改时间';
comment on column TB_CENTERRESOURCE.updateduserid
  is '修改用户';
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_CENTERRESOURCE
  add constraint PK_TB_CENTERRESOURCE primary key (CRID)
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

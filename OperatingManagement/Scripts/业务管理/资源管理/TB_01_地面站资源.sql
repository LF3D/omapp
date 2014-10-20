-- Create table
create table TB_GROUNDRESOURCE
(
  grid             NUMBER(10) not null,
  rid              NUMBER(5) not null,
  equipmentname    NVARCHAR2(50) not null,
  equipmentcode    NVARCHAR2(50) not null,
  opticalequipment NUMBER(2) not null,
  functiontype     NVARCHAR2(50) not null,
  status           NUMBER(2) not null,
  extproperties    NVARCHAR2(2000),
  createdtime      DATE not null,
  createduserid    NUMBER(10),
  updatedtime      DATE,
  updateduserid    NUMBER(10)
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
comment on column TB_GROUNDRESOURCE.grid
  is '序号';
comment on column TB_GROUNDRESOURCE.rid
  is '地面站序号';
comment on column TB_GROUNDRESOURCE.equipmentname
  is '设备名称';
comment on column TB_GROUNDRESOURCE.equipmentcode
  is '设备编号';
comment on column TB_GROUNDRESOURCE.opticalequipment
  is '是否光学设备';
comment on column TB_GROUNDRESOURCE.functiontype
  is '功能类型';
comment on column TB_GROUNDRESOURCE.status
  is '状态';
comment on column TB_GROUNDRESOURCE.extproperties
  is '扩展属性';
comment on column TB_GROUNDRESOURCE.createdtime
  is '创建时间';
comment on column TB_GROUNDRESOURCE.createduserid
  is '创建用户';
comment on column TB_GROUNDRESOURCE.updatedtime
  is '最后修改时间';
comment on column TB_GROUNDRESOURCE.updateduserid
  is '修改用户';
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_GROUNDRESOURCE
  add constraint PK_TB_GROUNDRESOURCE primary key (GRID)
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

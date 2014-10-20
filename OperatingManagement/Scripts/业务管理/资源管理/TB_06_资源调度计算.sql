-- Create table
create table TB_RESOURCECALCULATE
(
  rcid                       NUMBER(10) not null,
  requirementfiledirectory   NVARCHAR2(100),
  requirementfilename        NVARCHAR2(50),
  requirementfiledisplayname NVARCHAR2(50),
  resultfiledirectory        NVARCHAR2(100),
  resultfilename             NVARCHAR2(50),
  resultfiledisplayname      NVARCHAR2(50),
  resultfilesource           NUMBER(2),
  calculateresult            NUMBER(2),
  status                     NUMBER(2) not null,
  createdtime                DATE not null,
  createduserid              NUMBER(10),
  updatedtime                DATE,
  updateduserid              NUMBER(10)
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
comment on column TB_RESOURCECALCULATE.rcid
  is '序号';
comment on column TB_RESOURCECALCULATE.requirementfiledirectory
  is '资源需求文件目录';
comment on column TB_RESOURCECALCULATE.requirementfilename
  is '资源需求文件名称';
comment on column TB_RESOURCECALCULATE.requirementfiledisplayname
  is '资源需求文件显示名称';
comment on column TB_RESOURCECALCULATE.resultfiledirectory
  is '资源计算结果文件目录';
comment on column TB_RESOURCECALCULATE.resultfilename
  is '资源计算结果文件名称';
comment on column TB_RESOURCECALCULATE.resultfiledisplayname
  is '资源计算结果文件显示名称';
comment on column TB_RESOURCECALCULATE.resultfilesource
  is '资源计算结果来源：系统计算（1）、用户上传（2）';
comment on column TB_RESOURCECALCULATE.calculateresult
  is '资源计算结果：计算成功（1）、计算失败（2）；';
comment on column TB_RESOURCECALCULATE.status
  is '状态：等待计算（1）、计算完成（2）；';
comment on column TB_RESOURCECALCULATE.createdtime
  is '创建时间';
comment on column TB_RESOURCECALCULATE.createduserid
  is '创建用户ID';
comment on column TB_RESOURCECALCULATE.updatedtime
  is '最后修改时间';
comment on column TB_RESOURCECALCULATE.updateduserid
  is '最后修改用户ID';
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_RESOURCECALCULATE
  add constraint PK_TB_RESOURCECALCULATE primary key (RCID)
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

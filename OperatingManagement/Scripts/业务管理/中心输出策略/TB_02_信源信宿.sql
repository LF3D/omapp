--本地测试需要执行此脚本，上线时甲方DB中应该存在该表及对应数据

-- Create table
create table TB_XYXSINFO
(
  rid           NUMBER(5) not null,
  addrname      VARCHAR2(100) not null,
  addrmark      VARCHAR2(10),
  incode        VARCHAR2(10) not null,
  excode        VARCHAR2(20) not null,
  mainip        VARCHAR2(15),
  tcpport       NUMBER(8),
  bakip         VARCHAR2(15),
  udpport       NUMBER(8),
  ftppath       VARCHAR2(50),
  type          NUMBER(1) not null,
  own           VARCHAR2(2),
  coordinate    VARCHAR2(50),
  status        NUMBER(2) not null,
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
    initial 16
    minextents 1
    maxextents unlimited
  );
-- Add comments to the columns 
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
  is '主端口';
comment on column TB_XYXSINFO.bakip
  is '备用IP地址';
comment on column TB_XYXSINFO.udpport
  is '备用端口';
comment on column TB_XYXSINFO.ftppath
  is 'FTP路径@用户名@口令';
comment on column TB_XYXSINFO.type
  is '类型；0：地面站，1：中心；2：分系统';
comment on column TB_XYXSINFO.own
  is '地面站归属（管理单位）；01：总参，02：总装，03：遥科学站';
comment on column TB_XYXSINFO.coordinate
  is '站址坐标';
comment on column TB_XYXSINFO.status
  is '状态；1：正常，2：删除';
comment on column TB_XYXSINFO.createdtime
  is '创建时间';
comment on column TB_XYXSINFO.createduserid
  is '创建用户ID';
comment on column TB_XYXSINFO.updatedtime
  is '最后修改时间';
comment on column TB_XYXSINFO.updateduserid
  is '最后修改用户ID';
-- Create/Recreate primary, unique and foreign key constraints 
alter table TB_XYXSINFO
  add constraint PK_TB_XYXSINFO primary key (RID)
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

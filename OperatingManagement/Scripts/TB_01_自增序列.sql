/*========================================*/
/* Table: –Ú¡–¥Ê¥¢±Ì                                      */
/*========================================*/
create table TB_SEQUENCENUMBER
(
  SEQUENCETYPE  VARCHAR2(4) not null,
  CURRENTVALUE  NUMBER(20) not null,
  INITVALUE     NUMBER(10) not null,
  MAXVALUE      NUMBER(20) not null,
  STEPVALUE     NUMBER(10) not null,
  PREFIXSTRING  VARCHAR2(10),
  POSTFIXSTRING VARCHAR2(10),
  COMMENTS      VARCHAR2(100),
  SEQUENCENAME  VARCHAR2(50) not null
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
alter table TB_SEQUENCENUMBER
  add constraint PK_TB_SEQUENCENUMBER primary key (SEQUENCETYPE)
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

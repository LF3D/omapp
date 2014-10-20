create or replace function fn_genseqnum(SEQTYPE   IN VARCHAR2)
 return varchar2 as
  SEQNUMBER varchar2(50);
  CVALUE     NUMBER(20) := 0;
  MVALUE     NUMBER(20) := 0;
  SVALUE     NUMBER(10) := 0;
  PRESTRING  VARCHAR(10) := '';
  POSTSTRING VARCHAR(10) := '';
begin
  --===================================================
  --
  -- 根据种子生成指定类型的序列号
  --
  --===================================================
  SEQNUMBER := '';

  SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

  -- 获取指定类型序列种子的当前值
  BEGIN
    SELECT CURRENTVALUE, MAXVALUE, STEPVALUE, PREFIXSTRING, POSTFIXSTRING
      INTO CVALUE, MVALUE, SVALUE, PRESTRING, POSTSTRING
      FROM TB_SEQUENCENUMBER
     WHERE SEQUENCETYPE = SEQTYPE;
  EXCEPTION
    WHEN NO_DATA_FOUND THEN
      ROLLBACK;
      RETURN '';
  END;

  -- 将种子值按步长增长并更新种子当前值
  BEGIN
    IF CVALUE + SVALUE > MVALUE THEN
      --是否超过最大值
      ROLLBACK;
      RETURN '';
    END IF;
    SEQNUMBER := TO_CHAR(CVALUE + SVALUE);
    IF PRESTRING IS NOT NULL THEN
      --使用前缀
      SEQNUMBER := PRESTRING || SEQNUMBER;
    END IF;
    IF POSTSTRING IS NOT NULL THEN
      --使用后缀
      SEQNUMBER := SEQNUMBER || POSTSTRING;
    END IF;

    UPDATE TB_SEQUENCENUMBER
       SET CURRENTVALUE = CVALUE + SVALUE
     WHERE SEQUENCETYPE = SEQTYPE;
  EXCEPTION
    WHEN OTHERS THEN
      ROLLBACK;
      RETURN '';
  END;

  COMMIT;
  return(SEQNUMBER);
end fn_genseqnum;

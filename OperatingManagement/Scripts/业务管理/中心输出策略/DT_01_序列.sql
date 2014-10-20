/*
* 初始化序号信息
*/
-- 序号基本信息
INSERT INTO TB_SEQUENCENUMBER VALUES('4001',0,0,99999999999999999999,1,'','','中心输出策略序号','SEQUENCE_4001');


-- 创建所有类型的Sequence,与序号基本信息一一对应
CREATE SEQUENCE SEQUENCE_4001 MINVALUE 0 MAXVALUE 999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;


--pay attention pls 
--请注意 此文本内容已经合并到Scripts\DT_01_序列.sql文本中
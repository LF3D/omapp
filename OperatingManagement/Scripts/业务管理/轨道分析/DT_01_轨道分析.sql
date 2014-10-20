begin
  insert into tb_module values(4,'OrbitParaCal','轨道参数转换',sysdate());
  commit;
end;
/
begin
  --UserManage
  insert into tb_permission values(11,4,2);
  commit;
end;

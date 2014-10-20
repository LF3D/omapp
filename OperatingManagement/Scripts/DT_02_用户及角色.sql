declare 
m_seqnum number(10);
begin
  m_seqnum:= to_number(fn_genseqnum('0002'));

  insert into TB_USER(USERID,
         LOGINNAME,
         DISPLAYNAME,
         PASSWORD1,
         USERTYPE,
         USERCATALOG,
         STATUS,
         MOBILE,
         NOTE,
         CTIME,
         LASTUPDATEDTIME) values(m_seqnum,
         'admin',
         '系统管理员',
         'XdZt2O+9K5JTQ1FOSGFeXp+B/7c=',
         1,
         0,
         0,
         null,
         null,
         sysdate(),
         sysdate());
  commit;
end;
begin
  insert into tb_action values(1,'Add','新增',sysdate());
  insert into tb_action values(2,'Edit','修改',sysdate());
  insert into tb_action values(3,'Delete','删除',sysdate());
  insert into tb_action values(4,'View','查看',sysdate());
  insert into tb_action values(5,'List','列表',sysdate());
  commit;
end;
begin
  insert into tb_module values(1,'UserManage','用户管理',sysdate());
  insert into tb_module values(2,'RoleManage','角色管理',sysdate());
  commit;
end;
begin
  --UserManage
  insert into tb_permission values(1,1,1);
  insert into tb_permission values(2,1,2);
  insert into tb_permission values(3,1,3);
  insert into tb_permission values(4,1,4);
  insert into tb_permission values(5,1,5);  
  --RoleManage
  insert into tb_permission values(6,2,1);
  insert into tb_permission values(7,2,2);
  insert into tb_permission values(8,2,3);
  insert into tb_permission values(9,2,4);
  insert into tb_permission values(10,2,5);
  commit;
end;

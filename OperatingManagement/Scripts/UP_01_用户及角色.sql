create or replace procedure up_user_selectbyloginname
(
       p_LoginName tb_user.loginname%type,
       o_cursor out sys_refcursor
)
is
begin
       open o_cursor for
            select * from tb_user where upper(LoginName)=upper(p_LoginName);
end;
/
create or replace procedure up_user_search
(
       p_keyword nvarchar2,
       o_cursor out sys_refcursor
)
is
begin
       open o_cursor for
            select * from tb_user
            where upper(loginname) like('%' || upper(p_keyword) || '%')
            or upper(displayname) like('%' || upper(p_keyword) || '%')
            or upper(note) like('%' || upper(p_keyword) || '%')
            order by userid;
end;
/
create or replace procedure up_user_selectbyid
(
       p_UserId tb_user.userid%type,
       o_cursor out sys_refcursor
)
is
begin
       open o_cursor for
            select * from tb_user where userid=p_UserId;
end;
/
create or replace procedure up_user_selectall
(
       o_cursor out sys_refcursor  
)
is 
begin
       open o_cursor for
            select * from tb_user;
end;
/ 
create or replace procedure up_role_selectall
(
       o_cursor out sys_refcursor  
)
is 
begin
       open o_cursor for
            select * from tb_role;
end;
/
create or replace procedure up_module_selectall
(
       o_cursor out sys_refcursor  
)
is 
begin
       open o_cursor for
            select * from tb_module;
end;
/
create or replace procedure up_module_search
(
       p_keyword nvarchar2,
       o_cursor out sys_refcursor
)
is
begin
       open o_cursor for
            select * from tb_module
            where upper(modulename) like('%' || upper(p_keyword) || '%')
            or upper(note) like('%' || upper(p_keyword) || '%');
end;
/
create or replace procedure up_action_selectall
(
       o_cursor out sys_refcursor  
)
is 
begin
       open o_cursor for
            select * from tb_action;
end;
/
create or replace procedure up_permission_selectall
(
       o_cursor out sys_refcursor  
)
is 
begin
       open o_cursor for
            select a.PermissionId,b.ModuleId,b.Modulename,b.note as ModuleNote,
            c.ActionId,c.actionname,c.note as ActionNote from tb_permission a
            left join tb_module b on a.moduleid=b.moduleid
            left join tb_action c on a.actionid=c.actionid
            order by a.permissionid asc;
end;
/
create or replace procedure up_permission_selectbyln
(
       p_LoginName tb_user.loginname%type,
       o_cursor out sys_refcursor  
)
is 
begin
       open o_cursor for
            select a.PermissionId,b.ModuleId,b.Modulename,b.note as ModuleNote,
            c.ActionId,c.actionname,c.note as ActionNote from tb_permission a
            left join tb_module b on a.moduleid=b.moduleid
            left join tb_action c on a.actionid=c.actionid
            where a.permissionid in(
                  select a1.permissionid from tb_rolepermission a1
                  inner join tb_rolepermission a2 on a1.roleid = a2.roleid
                  inner join tb_userrole a3 on a2.roleid = a3.roleid
                  inner join tb_user a4 on a3.userid = a4.userid
                  where a4.loginname=p_LoginName                  
            )
            order by a.permissionid asc;
end;
/
create or replace procedure up_role_insert
(
       p_RoleName tb_role.rolename%type,
       p_Note tb_role.note%type,
       p_Permissions varchar2,
       v_result out number
)
is
       m_seqnum number;
       m_count integer;
begin
       select count(*) into m_count from tb_role where rolename=p_RoleName;
       if m_count>0 then
         v_result:=3; --Name Duplicated.
         return;
       end if;
       
       savepoint p1;
       m_seqnum:= to_number(fn_genseqnum('0001'));
       insert into tb_role values(m_seqnum,p_RoleName,p_Note,sysdate());
       commit;
       
       insert into tb_rolepermission 
              select m_seqnum,to_number(COLUMN_VALUE) from table(split(p_Permissions));
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;
/
create or replace procedure up_role_update
(
       p_RoleId tb_role.roleid%type,
       p_RoleName tb_role.rolename%type,
       p_Note tb_role.note%type,
       p_Permissions varchar2,
       v_result out number
)
is
       m_count integer;
begin
       select count(*) into m_count from tb_role 
              where rolename=p_RoleName and roleid!=p_RoleId;
       if m_count>0 then
         v_result:=3; --Name Duplicated.
         return;
       end if;
       
       savepoint p1;
       update tb_role set
         RoleName = p_RoleName,
         Note = p_Note
         where ROleId = p_RoleId;
       commit;
       
       delete from tb_rolepermission where RoleId = p_RoleId;
       commit;
       
       insert into tb_rolepermission 
              select p_RoleId, to_number(COLUMN_VALUE) from table(split(p_Permissions));
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;
/
create or replace procedure up_role_selectbyid
(
       p_RoleId tb_role.roleid%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            select a.*,b.permissionid from tb_role a left join tb_rolepermission b on
            a.roleid=b.roleid
            where a.roleid = p_RoleId;
end;
/
create or replace procedure up_user_insert
(
       p_LoginName tb_user.loginname%type,
       p_DisplayName tb_user.displayname%type,
       p_Password tb_user.password1%type,
       p_UserType tb_user.usertype%type,
       p_UserCatalog tb_user.usercatalog%type,
       p_Status tb_user.status%type,
       p_Mobile tb_user.mobile%type,
       p_Note tb_user.note%type,
       v_UserId out tb_user.userid%type,
       v_result out number
)
is
       m_count integer;
begin
       select count(*) into m_count from tb_user where loginname=p_loginname;
       if m_count>0 then
         v_result:=3; --Name Duplicated.
         return;
       end if;
       
       select count(*) into m_count from tb_user where displayname=p_DisplayName;
       if m_count>0 then
         v_result:=6; --Name Duplicated.
         return;
       end if;
       
       savepoint p1;
       v_UserId:= to_number(fn_genseqnum('0002'));
       insert into tb_user values(v_UserId,p_LoginName,p_DisplayName,p_Password,p_UserType,
              p_Status,p_Mobile,p_Note,sysdate(),sysdate(),p_UserCatalog);
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;
/
create or replace procedure up_user_update
(
       p_UserId tb_user.userid%type,
       p_DisplayName tb_user.displayname%type,
       p_Password tb_user.password1%type,
       p_UserType tb_user.usertype%type,
       p_UserCatalog tb_user.usercatalog%type,
       p_Status tb_user.status%type,
       p_Mobile tb_user.mobile%type,
       p_Note tb_user.note%type,
       v_result out number
)
is
       m_count integer;
begin
       select count(*) into m_count from tb_user where 
              displayname=p_DisplayName and userid!=p_UserId;
       if m_count>0 then
         v_result:=6; --Name Duplicated.
         return;
       end if;
       
       savepoint p1;
       update tb_user set
         DisplayName = p_DisplayName,
         UserType = p_UserType,
         Status = p_Status,
         Mobile = p_Mobile,
         Note = p_Note,
         UserCatalog = p_UserCatalog,
         LastUpdatedTime = sysdate() where userid=p_UserId;
       commit;
       if p_Password is not null then
         update tb_user set password1 = p_Password  where userid=p_UserId;
         commit;
       end if;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;
/
create or replace procedure up_user_addtorole
(
       P_UserId tb_user.userid%type,
       p_Roles varchar2,
       v_result out number
)
is
       m_count integer;
begin       
       savepoint p1;
       delete from tb_userrole where userid=p_UserId;
       commit;
       
       insert into tb_userrole 
              select p_UserId,to_number(COLUMN_VALUE),sysdate() from table(split(p_Roles));
       commit;
       v_result:=1; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=0; --Error
end;
/
create or replace procedure up_user_selectrolesbyid
(
       p_UserId tb_user.userid%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            select * from tb_role
                   where roleid in (select roleid from tb_userrole where userid=p_UserId);
end;
/
create or replace procedure up_user_deletebyids
(
       p_Ids varchar2,       
       v_result out number
)
is
begin
       savepoint p1;
       delete from tb_user where userid in
       (select column_value from table(split(p_Ids,',')));
       commit;
       
       delete from tb_userrole where userid in
       (select column_value from table(split(p_Ids,',')));
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error       
end;
/
create or replace procedure up_role_deletebyids
(
       p_Ids varchar2,       
       v_result out number
)
is 
begin
       savepoint p1;
       delete from tb_userrole where roleid in 
       (select column_value from table(split(p_Ids,',')));
       commit;
       
       delete from tb_rolepermission where roleid in 
       (select column_value from table(split(p_Ids,',')));
       commit;
       
       delete from tb_role where roleid in
       (select column_value from table(split(p_Ids,',')));
       commit;
       v_result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
          v_result:=4; --Error
end;

create or replace procedure up_user_selectbyroleid
(
       p_RoleId tb_role.roleid%type,
       o_cursor out sys_refcursor
)
is
begin
       open o_cursor for
            select * from tb_user
                   where userid in (select userid from tb_userrole where roleid = p_RoleId);
end;

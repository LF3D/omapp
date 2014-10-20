Create or replace procedure UP_ZYSX_SelectByID
(
       p_ID TB_ZYSX.ID%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_ZYSX Where ID=p_ID;
end;


create or replace procedure UP_ZYSX_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_ZYSX Order By ID Desc;
end;
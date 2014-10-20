create or replace procedure UP_InfoTYPE_SelectByID
(
       p_RID TB_InfoTYPE.RID%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_InfoTYPE Where RID=p_RID;
end;


create or replace procedure UP_InfoTYPE_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_InfoTYPE Order By DataName;
end;

create or replace procedure UP_ResCalculate_SelectByID
(
       p_RCID TB_ResourceCalculate.Rcid%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_ResourceCalculate Where RCID=p_RCID;
end;


create or replace procedure UP_ResCalculate_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_ResourceCalculate Order By CreatedTime Desc;
end;


create or replace procedure UP_ResCalculate_Insert
(
       p_RequirementFileDirectory TB_ResourceCalculate.RequirementFileDirectory%type,
       p_RequirementFileName TB_ResourceCalculate.RequirementFileName%type,
       p_RequirementFileDisplayName TB_ResourceCalculate.RequirementFileDisplayName%type,
       p_ResultFileDirectory TB_ResourceCalculate.ResultFileDirectory%type,
       p_ResultFileName TB_ResourceCalculate.ResultFileName%type,
       p_ResultFileDisplayName TB_ResourceCalculate.ResultFileDisplayName%type,
       p_ResultFileSource TB_ResourceCalculate.ResultFileSource%type,
       p_CalculateResult TB_ResourceCalculate.CalculateResult%type,
       p_Status TB_ResourceCalculate.Status%type,
       p_CreatedTime TB_ResourceCalculate.Createdtime%type,
       p_CreatedUserID TB_ResourceCalculate.Createduserid%type,
       p_UpdatedTime TB_ResourceCalculate.Updatedtime%type,
       p_UpdatedUserID TB_ResourceCalculate.Updateduserid%type,
       v_RCID out TB_ResourceCalculate.RCID%type,
       v_Result out number
)
is
begin  
       savepoint p1;
       --v_RCID:=to_number(fn_genseqnum('4007'));
       Select SEQ_TB_ResourceCalculate.NEXTVAL INTO v_RCID From DUAL;
       Insert into TB_ResourceCalculate(RCID,RequirementFileDirectory,RequirementFileName,RequirementFileDisplayName,ResultFileDirectory,ResultFileName,ResultFileDisplayName,ResultFileSource,CalculateResult,Status,CreatedTime,CreatedUserID,UpdatedTime,UpdatedUserID) 
       Values(v_RCID,p_RequirementFileDirectory,p_RequirementFileName,p_RequirementFileDisplayName,p_ResultFileDirectory,p_ResultFileName,p_ResultFileDisplayName,p_ResultFileSource,p_CalculateResult,p_Status,p_CreatedTime,p_CreatedUserID,p_UpdatedTime,p_UpdatedUserID);
       commit;
       v_Result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
       v_Result:=4; --Error
end;


create or replace procedure UP_ResCalculate_Update
(
       p_RCID TB_ResourceCalculate.RCID%type,
       p_RequirementFileDirectory TB_ResourceCalculate.RequirementFileDirectory%type,
       p_RequirementFileName TB_ResourceCalculate.RequirementFileName%type,
       p_RequirementFileDisplayName TB_ResourceCalculate.RequirementFileDisplayName%type,
       p_ResultFileDirectory TB_ResourceCalculate.ResultFileDirectory%type,
       p_ResultFileName TB_ResourceCalculate.ResultFileName%type,
       p_ResultFileDisplayName TB_ResourceCalculate.ResultFileDisplayName%type,
       p_ResultFileSource TB_ResourceCalculate.ResultFileSource%type,
       p_CalculateResult TB_ResourceCalculate.CalculateResult%type,
       p_Status TB_ResourceCalculate.Status%type,
       p_CreatedTime TB_ResourceCalculate.Createdtime%type,
       p_CreatedUserID TB_ResourceCalculate.Createduserid%type,
       p_UpdatedTime TB_ResourceCalculate.Updatedtime%type,
       p_UpdatedUserID TB_ResourceCalculate.Updateduserid%type,
       v_Result out number
)
is
begin
     savepoint p1;
     
       Update TB_ResourceCalculate
       Set RequirementFileDirectory=p_RequirementFileDirectory
          ,RequirementFileName=p_RequirementFileName
          ,RequirementFileDisplayName=p_RequirementFileDisplayName
          ,ResultFileDirectory=p_ResultFileDirectory
          ,ResultFileName=p_ResultFileName
          ,ResultFileDisplayName=p_ResultFileDisplayName
          ,ResultFileSource=p_ResultFileSource
          ,CalculateResult=p_CalculateResult
          ,Status=p_Status
          ,CreatedTime=p_CreatedTime
          ,CreatedUserID=p_CreatedUserID
          ,UpdatedTime=p_UpdatedTime
          ,UpdatedUserID=p_UpdatedUserID
        Where RCID=p_RCID;
        Commit;
        v_Result:=5; -- Success
       
        EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
        v_Result:=4; --Error
end;


create or replace procedure UP_ResCalculate_Search
(
       p_ResultFileSource TB_ResourceCalculate.ResultFileSource%type,
       p_Status TB_ResourceCalculate.Status%type,
       p_BeginTime TB_ResourceCalculate.CreatedTime%type,
       p_EndTime TB_ResourceCalculate.CreatedTime%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_ResourceCalculate 
            Where (ResultFileSource=p_ResultFileSource Or p_ResultFileSource Is Null)
              And (Status=p_Status Or p_Status Is Null)
              And (CreatedTime>=p_BeginTime And CreatedTime<=p_EndTime)
            Order By CreatedTime Desc;
end;
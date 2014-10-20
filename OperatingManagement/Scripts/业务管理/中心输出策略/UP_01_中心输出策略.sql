


create or replace procedure UP_COP_SelectByID
(
       p_COPID TB_CenterOutputPolicy.COPID%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_CenterOutputPolicy Where COPID=p_COPID;
end;


create or replace procedure UP_COP_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_CenterOutputPolicy Order By CreatedTime Desc;
end;


create or replace procedure UP_COP_Insert
(
       p_TaskID  TB_CenterOutputPolicy.Taskid%type,
       p_SatName  TB_CenterOutputPolicy.Satname%type,
       p_InfoSource TB_CenterOutputPolicy.Infosource%type,
       p_InfoType TB_CenterOutputPolicy.Infotype%type,
       p_DDestination TB_CenterOutputPolicy.Ddestination%type,
       p_EffectTime TB_CenterOutputPolicy.Effecttime%type,
       p_DefectTime TB_CenterOutputPolicy.Defecttime%type,
       p_Note TB_CenterOutputPolicy.Note%type,
       p_CreatedTime TB_CenterOutputPolicy.Createdtime%type,
       p_CreatedUserID TB_CenterOutputPolicy.Createduserid%type,
       p_UpdatedTime TB_CenterOutputPolicy.Updatedtime%type,
       p_UpdatedUserID TB_CenterOutputPolicy.Updateduserid%type,
       v_COPID out TB_CenterOutputPolicy.COPID%type,
       v_Result out number
)
is
begin  
       savepoint p1;
       --v_COPID:=to_number(fn_genseqnum('4001'));
       Select SEQ_TB_CenterOutputPolicy.NEXTVAL INTO v_COPID FROM DUAL;
       Insert into TB_CenterOutputPolicy(COPID,TaskID,SatName,InfoSource,InfoType,DDestination,EffectTime,DefectTime,Note,CreatedTime,CreatedUserID,UpdatedTime,UpdatedUserID) Values(v_COPID,p_TaskID,p_SatName,p_InfoSource,p_InfoType,p_DDestination,p_EffectTime,p_DefectTime,p_Note,p_CreatedTime,p_CreatedUserID,p_UpdatedTime,p_UpdatedUserID);
       commit;
       v_Result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
       v_Result:=4; --Error
end;



create or replace procedure UP_COP_Update
(
    p_COPID TB_CenterOutputPolicy.Copid%type,
    p_TaskID  TB_CenterOutputPolicy.Taskid%type,
    p_SatName  TB_CenterOutputPolicy.Satname%type,
    p_InfoSource TB_CenterOutputPolicy.Infosource%type,
    p_InfoType TB_CenterOutputPolicy.Infotype%type,
    p_DDestination TB_CenterOutputPolicy.Ddestination%type,
    p_EffectTime TB_CenterOutputPolicy.Effecttime%type,
    p_DefectTime TB_CenterOutputPolicy.Defecttime%type,
    p_Note TB_CenterOutputPolicy.Note%type,
    p_CreatedTime TB_CenterOutputPolicy.Createdtime%type,
    p_CreatedUserID TB_CenterOutputPolicy.Createduserid%type,
    p_UpdatedTime TB_CenterOutputPolicy.Updatedtime%type,
    p_UpdatedUserID TB_CenterOutputPolicy.Updateduserid%type,
    v_Result out number
)
is
begin
     savepoint p1;
     
     update TB_CenterOutputPolicy 
        set TaskID=p_TaskID
           ,SatName=p_SatName
           ,InfoSource=p_InfoSource
           ,InfoType=p_InfoType
           ,DDestination=p_DDestination
           ,EffectTime=p_EffectTime
           ,DefectTime=p_DefectTime
           ,Note=p_Note
           ,CreatedTime=p_CreatedTime
           ,CreatedUserID=p_CreatedUserID
           ,UpdatedTime=p_UpdatedTime
           ,UpdatedUserID=p_UpdatedUserID
        where COPID=p_COPID;
        commit;
        v_Result:=5; -- Success
       
        EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
        v_Result:=4; --Error
end;



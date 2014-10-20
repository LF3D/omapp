


create or replace procedure UP_UseStatus_SelectByID
(
       p_USID TB_UseStatus.USID%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_UseStatus Where USID=p_USID;
end;


create or replace procedure UP_UseStatus_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_UseStatus Order By CreatedTime Desc;
end;


create or replace procedure UP_UseStatus_Insert
(
       p_ResourceID TB_UseStatus.ResourceID%type,
       p_ResourceType TB_UseStatus.ResourceType%type,
       p_UsedType TB_UseStatus.UsedType%type,
       p_BeginTime TB_UseStatus.BeginTime%type,
       p_EndTime TB_UseStatus.EndTime%type,
       p_UsedBy TB_UseStatus.UsedBy%type,
       p_UsedCategory TB_UseStatus.UsedCategory%type,
       p_UsedFor TB_UseStatus.UsedFor%type,
       p_CanBeUsed TB_UseStatus.CanBeUsed%type,
       p_CreatedTime TB_UseStatus.Createdtime%type,
       p_CreatedUserID TB_UseStatus.Createduserid%type,
       p_UpdatedTime TB_UseStatus.Updatedtime%type,
       p_UpdatedUserID TB_UseStatus.Updateduserid%type,
       v_USID out TB_UseStatus.USID%type,
       v_Result out number
)
is
begin  
       savepoint p1;
       --v_USID:=to_number(fn_genseqnum('4006'));
       Select SEQ_TB_UseStatus.NEXTVAL INTO v_USID From DUAL;
       Insert into TB_UseStatus(USID,ResourceID,ResourceType,UsedType,BeginTime,EndTime,UsedBy,UsedCategory,UsedFor,CanBeUsed,CreatedTime,CreatedUserID,UpdatedTime,UpdatedUserID) 
       Values(v_USID,p_ResourceID,p_ResourceType,p_UsedType,p_BeginTime,p_EndTime,p_UsedBy,p_UsedCategory,p_UsedFor,p_CanBeUsed,p_CreatedTime,p_CreatedUserID,p_UpdatedTime,p_UpdatedUserID);
       commit;
       v_Result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
       v_Result:=4; --Error
end;



create or replace procedure UP_UseStatus_Update
(
       p_USID TB_UseStatus.USID%type,
       p_ResourceID TB_UseStatus.ResourceID%type,
       p_ResourceType TB_UseStatus.ResourceType%type,
       p_UsedType TB_UseStatus.UsedType%type,
       p_BeginTime TB_UseStatus.BeginTime%type,
       p_EndTime TB_UseStatus.EndTime%type,
       p_UsedBy TB_UseStatus.UsedBy%type,
       p_UsedCategory TB_UseStatus.UsedCategory%type,
       p_UsedFor TB_UseStatus.UsedFor%type,
       p_CanBeUsed TB_UseStatus.CanBeUsed%type,
       p_CreatedTime TB_UseStatus.Createdtime%type,
       p_CreatedUserID TB_UseStatus.Createduserid%type,
       p_UpdatedTime TB_UseStatus.Updatedtime%type,
       p_UpdatedUserID TB_UseStatus.Updateduserid%type,
       v_Result out number
)
is
begin
     savepoint p1;
     
       Update TB_UseStatus
       Set ResourceID=p_ResourceID
          ,ResourceType=p_ResourceType
          ,UsedType=p_UsedType
          ,BeginTime=p_BeginTime
          ,EndTime=p_EndTime
          ,UsedBy=p_UsedBy
          ,UsedCategory=p_UsedCategory
          ,UsedFor=p_UsedFor
          ,CanBeUsed=p_CanBeUsed
          ,CreatedTime=p_CreatedTime
          ,CreatedUserID=p_CreatedUserID
          ,UpdatedTime=p_UpdatedTime
          ,UpdatedUserID=p_UpdatedUserID
        Where USID=p_USID;
        Commit;
        v_Result:=5; -- Success
       
        EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
        v_Result:=4; --Error
end;



create or replace procedure UP_UseStatus_Search
(
       p_ResourceType TB_UseStatus.Resourcetype%type,
       p_ResourceID TB_UseStatus.Resourceid%type,
       p_BeginTime TB_UseStatus.Begintime%type,
       p_EndTime TB_UseStatus.Endtime%type,
       o_Cursor out sys_refcursor
)
is
begin
    IF p_ResourceType=1 Then
       open o_Cursor for
            Select A.*,B.EQUIPMENTNAME as ResourceName,B.EQUIPMENTCODE as ResourceCode
            From TB_UseStatus A
            Inner join TB_GROUNDRESOURCE B on (A.RESOURCETYPE=1 And A.Resourceid=B.GRID)
            Where (
                  ( p_BeginTime>= A.Begintime And p_BeginTime<=A.EndTime)
               Or ( p_EndTime>= A.Begintime And p_EndTime<=A.EndTime)
               Or ( p_BeginTime<=A.Begintime And p_EndTime>=A.EndTime)
                  )
              And (A.Resourceid=p_ResourceID Or p_ResourceID Is Null)
            Order BY A.CreatedTime DESC;
     Elsif p_ResourceType=2 Then
         open o_Cursor for
            Select A.*,B.ROUTENAME as ResourceName,B.ROUTECODE as ResourceCode
            From TB_UseStatus A
            Inner join TB_COMMUNICATIONRESOURCE B on (A.RESOURCETYPE=2 And A.Resourceid=B.CRID)
            Where (
                  ( p_BeginTime>= A.Begintime And p_BeginTime<=A.EndTime)
               Or ( p_EndTime>= A.Begintime And p_EndTime<=A.EndTime)
               Or ( p_BeginTime<=A.Begintime And p_EndTime>=A.EndTime)
                  )
              And (A.Resourceid=p_ResourceID Or p_ResourceID Is Null)
            Order BY A.CreatedTime DESC;
      Elsif p_ResourceType=3 Then
         open o_Cursor for
            Select A.*,B.EQUIPMENTTYPE as ResourceName,B.EQUIPMENTCODE as ResourceCode
            From TB_UseStatus A
            Inner join TB_CENTERRESOURCE B on (A.RESOURCETYPE=3 And A.Resourceid=B.CRID)
            Where (
                  ( p_BeginTime>= A.Begintime And p_BeginTime<=A.EndTime)
               Or ( p_EndTime>= A.Begintime And p_EndTime<=A.EndTime)
               Or ( p_BeginTime<=A.Begintime And p_EndTime>=A.EndTime)
                  )
              And (A.Resourceid=p_ResourceID Or p_ResourceID Is Null)
            Order BY A.CreatedTime DESC;
      END IF;

end;





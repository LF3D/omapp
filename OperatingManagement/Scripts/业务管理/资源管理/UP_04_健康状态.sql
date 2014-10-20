


create or replace procedure UP_HealthStatus_SelectByID
(
       p_HSID TB_HealthStatus.HSID%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_HealthStatus Where HSID=p_HSID;
end;


create or replace procedure UP_HealthStatus_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_HealthStatus Order By CreatedTime Desc;
end;


create or replace procedure UP_HealthStatus_Insert
(
       p_ResourceID TB_HealthStatus.ResourceID%type,
       p_ResourceType TB_HealthStatus.ResourceType%type,
       p_FunctionType TB_HealthStatus.FunctionType%type,
       p_Status TB_HealthStatus.Status%type,
       p_BeginTime TB_HealthStatus.BeginTime%type,
       p_EndTime TB_HealthStatus.EndTime%type,
       p_CreatedTime TB_HealthStatus.Createdtime%type,
       p_CreatedUserID TB_HealthStatus.Createduserid%type,
       p_UpdatedTime TB_HealthStatus.Updatedtime%type,
       p_UpdatedUserID TB_HealthStatus.Updateduserid%type,
       v_HSID out TB_HealthStatus.HSID%type,
       v_Result out number
)
is
begin  
       savepoint p1;
       --v_HSID:=to_number(fn_genseqnum('4005'));
	     Select SEQ_TB_HealthStatus.NEXTVAL INTO v_HSID From DUAL;
       Insert into TB_HealthStatus(HSID,ResourceID,ResourceType,FunctionType,Status,BeginTime,EndTime,CreatedTime,CreatedUserID,UpdatedTime,UpdatedUserID) 
       Values(v_HSID,p_ResourceID,p_ResourceType,p_FunctionType,p_Status,p_BeginTime,p_EndTime,p_CreatedTime,p_CreatedUserID,p_UpdatedTime,p_UpdatedUserID);
       commit;
       v_Result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
       v_Result:=4; --Error
end;



create or replace procedure UP_HealthStatus_Update
(
       p_HSID TB_HealthStatus.HSID%type,
       p_ResourceID TB_HealthStatus.ResourceID%type,
       p_ResourceType TB_HealthStatus.ResourceType%type,
       p_FunctionType TB_HealthStatus.FunctionType%type,
       p_Status TB_HealthStatus.Status%type,
       p_BeginTime TB_HealthStatus.BeginTime%type,
       p_EndTime TB_HealthStatus.EndTime%type,
       p_CreatedTime TB_HealthStatus.Createdtime%type,
       p_CreatedUserID TB_HealthStatus.Createduserid%type,
       p_UpdatedTime TB_HealthStatus.Updatedtime%type,
       p_UpdatedUserID TB_HealthStatus.Updateduserid%type,
       v_Result out number
)
is
begin
     savepoint p1;
     
       Update TB_HealthStatus
       Set ResourceID=p_ResourceID
          ,ResourceType=p_ResourceType
          ,FunctionType=p_FunctionType
          ,Status=p_Status
          ,BeginTime=p_BeginTime
          ,EndTime=p_EndTime
          ,CreatedTime=p_CreatedTime
          ,CreatedUserID=p_CreatedUserID
          ,UpdatedTime=p_UpdatedTime
          ,UpdatedUserID=p_UpdatedUserID
        Where HSID=p_HSID;
        Commit;
        v_Result:=5; -- Success
       
        EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
        v_Result:=4; --Error
end;



create or replace procedure UP_HealthStatus_Search
(
       p_ResourceType TB_HEALTHSTATUS.Resourcetype%type,
       p_ResourceID TB_HEALTHSTATUS.Resourceid%type,
       p_BeginTime TB_HEALTHSTATUS.Begintime%type,
       p_EndTime TB_HEALTHSTATUS.Endtime%type,
       o_Cursor out sys_refcursor
)
is
begin
    IF p_ResourceType=1 Then
       open o_Cursor for
            Select A.*,B.Equipmentname as ResourceName,B.Equipmentcode as ResourceCode
            From TB_HEALTHSTATUS A
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
            From TB_HEALTHSTATUS A
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
            From TB_HEALTHSTATUS A
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







create or replace procedure UP_ComRes_SelectByID
(
       p_CRID TB_CommunicationResource.CRID%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_CommunicationResource Where CRID=p_CRID;
end;


create or replace procedure UP_ComRes_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_CommunicationResource Order By CreatedTime Desc;
end;


create or replace procedure UP_ComRes_Insert
(
       p_RouteName TB_CommunicationResource.RouteName%type,
       p_RouteCode TB_CommunicationResource.RouteCode%type,
       p_Direction TB_CommunicationResource.Direction%type,
       p_BandWidth TB_CommunicationResource.BandWidth%type,
       p_Status TB_CommunicationResource.Status%type,
       p_ExtProperties TB_CommunicationResource.ExtProperties%type,
       p_CreatedTime TB_CommunicationResource.Createdtime%type,
       p_CreatedUserID TB_CommunicationResource.Createduserid%type,
       p_UpdatedTime TB_CommunicationResource.Updatedtime%type,
       p_UpdatedUserID TB_CommunicationResource.Updateduserid%type,
       v_CRID out TB_CommunicationResource.CRID%type,
       v_Result out number
)
is
begin  
       savepoint p1;
       --v_CRID:=to_number(fn_genseqnum('4003'));
	   Select SEQ_TB_CommunicationResource.NEXTVAL INTO v_CRID From DUAL;
       Insert into TB_CommunicationResource(CRID,RouteName,RouteCode,Direction,BandWidth,Status,ExtProperties,CreatedTime,CreatedUserID,UpdatedTime,UpdatedUserID) 
       Values(v_CRID,p_RouteName,p_RouteCode,p_Direction,p_BandWidth,p_Status,p_ExtProperties,p_CreatedTime,p_CreatedUserID,p_UpdatedTime,p_UpdatedUserID);
       commit;
       v_Result:=5; -- Success
       
       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
       v_Result:=4; --Error
end;



create or replace procedure UP_ComRes_Update
(
       
       p_CRID TB_CommunicationResource.CRID%type,
       p_RouteName TB_CommunicationResource.RouteName%type,
       p_RouteCode TB_CommunicationResource.RouteCode%type,
       p_Direction TB_CommunicationResource.Direction%type,
       p_BandWidth TB_CommunicationResource.BandWidth%type,
       p_Status TB_CommunicationResource.Status%type,
       p_ExtProperties TB_CommunicationResource.ExtProperties%type,
       p_CreatedTime TB_CommunicationResource.Createdtime%type,
       p_CreatedUserID TB_CommunicationResource.Createduserid%type,
       p_UpdatedTime TB_CommunicationResource.Updatedtime%type,
       p_UpdatedUserID TB_CommunicationResource.Updateduserid%type,
       v_Result out number
)
is
begin
     savepoint p1;
     
       Update TB_CommunicationResource
       Set RouteName=p_RouteName
          ,RouteCode=p_RouteCode
          ,Direction=p_Direction
          ,BandWidth=p_BandWidth
          ,Status=p_Status
          ,ExtProperties=p_ExtProperties
          ,CreatedTime=p_CreatedTime
          ,CreatedUserID=p_CreatedUserID
          ,UpdatedTime=p_UpdatedTime
          ,UpdatedUserID=p_UpdatedUserID
        Where CRID=p_CRID;
        Commit;
        v_Result:=5; -- Success
       
        EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
        v_Result:=4; --Error
end;

create or replace procedure UP_ComRes_Search
(
       p_Status varchar2,
       p_TimePoint date,
       o_Cursor out sys_refcursor
)
is
begin
       IF p_Status='' Or p_Status Is Null Then--全部 
         open o_Cursor for
             Select * From TB_CommunicationResource
             Order By CreatedTime Desc;
       Elsif p_Status='4' Then---删除
          open o_Cursor for
             Select * From TB_CommunicationResource
             Where Status=2
             Order By CreatedTime Desc;
       Elsif p_Status='1' Then --正常
         open o_Cursor for
             Select * From TB_CommunicationResource
             Where Status=1
               And CRID not in (Select ResourceID From TB_HEALTHSTATUS
                                 Where ResourceType=2
                                   And Status=2
                                   And BeginTime<=p_TimePoint
                                   And EndTime>=p_TimePoint)
             Order By CreatedTime Desc;
       Elsif p_Status='2' Then --异常
          open o_Cursor for
             Select * From TB_CommunicationResource
                 Where Status=1 
                   And CRID in (Select ResourceID From TB_HEALTHSTATUS
                                 Where ResourceType=2
                                   And Status=2
                                   And BeginTime<=p_TimePoint
                                   And EndTime>=p_TimePoint)
                 Order By CreatedTime Desc;
       Elsif p_Status='3' Then --占用中
          open o_Cursor for
             Select * From TB_CommunicationResource
                 Where Status=1 
                   And CRID in (Select ResourceID From TB_USESTATUS
                                 Where ResourceType=2
                                   And BeginTime<=p_TimePoint
                                   And EndTime>=p_TimePoint)
                 Order By CreatedTime Desc;

       End IF;
end;


create or replace procedure UP_ComRes_SearchByPhase
(
       p_Status varchar2,
       p_BeginTime date,
       p_EndTime date,
       o_Cursor out sys_refcursor
)
is
begin
       IF p_Status='' Or p_Status Is Null Then--全部
         open o_Cursor for
             Select * From TB_CommunicationResource
             Order By CreatedTime Desc;
       Elsif p_Status='4' Then---删除
          open o_Cursor for
             Select * From TB_CommunicationResource
             Where Status=2
             Order By CreatedTime Desc;
       Elsif p_Status='1' Then --正常
         open o_Cursor for
             Select * From TB_CommunicationResource
             Where Status=1
               And CRID not in (Select ResourceID From TB_HEALTHSTATUS
                                 Where ResourceType=2
                                   And Status=2
                                   And (    ( p_BeginTime>= Begintime And p_BeginTime<=EndTime)
                                         Or ( p_EndTime>= Begintime And p_EndTime<=EndTime)
                                         Or ( p_BeginTime<=Begintime And p_EndTime>=EndTime)
                                        )
                                )
             Order By CreatedTime Desc;
       Elsif p_Status='2' Then --异常
          open o_Cursor for
             Select * From TB_CommunicationResource
                 Where Status=1
                   And CRID in (Select ResourceID From TB_HEALTHSTATUS
                                 Where ResourceType=2
                                   And Status=2
                                   And (    ( p_BeginTime>= Begintime And p_BeginTime<=EndTime)
                                         Or ( p_EndTime>= Begintime And p_EndTime<=EndTime)
                                         Or ( p_BeginTime<=Begintime And p_EndTime>=EndTime)
                                        )
                                )
                 Order By CreatedTime Desc;
       Elsif p_Status='3' Then --占用中
          open o_Cursor for
             Select * From TB_CommunicationResource
                 Where Status=1
                   And CRID in (Select ResourceID From TB_USESTATUS
                                 Where ResourceType=2
                                   And (    ( p_BeginTime>= Begintime And p_BeginTime<=EndTime)
                                         Or ( p_EndTime>= Begintime And p_EndTime<=EndTime)
                                         Or ( p_BeginTime<=Begintime And p_EndTime>=EndTime)
                                        )
                                )
                 Order By CreatedTime Desc;

       End IF;
end;


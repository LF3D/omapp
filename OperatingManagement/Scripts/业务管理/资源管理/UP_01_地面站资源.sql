


create or replace procedure UP_GroundRes_SelectByID
(
       p_GRID TB_GROUNDRESOURCE.GRID%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select A.*,B.* From TB_GroundResource A 
            Inner join TB_XYXSINFO B on (A.RID=B.RID And B.Type=0)
            Where A.GRID=p_GRID;
end;



create or replace procedure UP_GroundRes_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select A.*,B.* From TB_GroundResource A
            Left join TB_XYXSINFO B on (A.RID=b.RID And B.Type=0)
            Order By A.CreatedTime Desc;
end;



create or replace procedure UP_GroundRes_Insert
(
       p_RID TB_GroundResource.Rid%type,
       p_EquipmentName TB_GroundResource.EquipmentName%type,
       p_EquipmentCode TB_GroundResource.EquipmentCode%type,
       p_OpticalEquipment TB_GroundResource.OpticalEquipment%type,
       p_FunctionType TB_GroundResource.FunctionType%type,
       p_Status TB_GroundResource.Status%type,
       p_ExtProperties TB_GroundResource.ExtProperties%type,
       p_CreatedTime TB_GroundResource.Createdtime%type,
       p_CreatedUserID TB_GroundResource.Createduserid%type,
       p_UpdatedTime TB_GroundResource.Updatedtime%type,
       p_UpdatedUserID TB_GroundResource.Updateduserid%type,
       v_GRID out TB_GroundResource.GRID%type,
       v_Result out number
)
is
begin
       savepoint p1;
       --v_GRID:=to_number(fn_genseqnum('4002'));
     Select SEQ_TB_GroundResource.NEXTVAL INTO v_GRID From DUAL;
       Insert into TB_GroundResource(GRID,RID,EquipmentName,EquipmentCode,OpticalEquipment,FunctionType,Status,ExtProperties,CreatedTime,CreatedUserID,UpdatedTime,UpdatedUserID)
       Values(v_GRID,p_RID,p_EquipmentName,p_EquipmentCode,p_OpticalEquipment,p_FunctionType,p_Status,p_ExtProperties,p_CreatedTime,p_CreatedUserID,p_UpdatedTime,p_UpdatedUserID);
       commit;
       v_Result:=5; -- Success

       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
       v_Result:=4; --Error
end;




create or replace procedure UP_GroundRes_Update
(
       p_GRID TB_GroundResource.GRID%type,
       p_RID TB_GroundResource.RID%type,
       p_EquipmentName TB_GroundResource.EquipmentName%type,
       p_EquipmentCode TB_GroundResource.EquipmentCode%type,
       p_OpticalEquipment TB_GroundResource.OpticalEquipment%type,
       p_FunctionType TB_GroundResource.FunctionType%type,
       p_Status TB_GroundResource.Status%type,
       p_ExtProperties TB_GroundResource.ExtProperties%type,
       p_CreatedTime TB_GroundResource.Createdtime%type,
       p_CreatedUserID TB_GroundResource.Createduserid%type,
       p_UpdatedTime TB_GroundResource.Updatedtime%type,
       p_UpdatedUserID TB_GroundResource.Updateduserid%type,
       v_Result out number
)
is
begin
     savepoint p1;

     Update TB_GroundResource
     Set RID=p_RID
        ,EquipmentName=p_EquipmentName
        ,EquipmentCode=p_EquipmentCode
        ,OpticalEquipment=p_OpticalEquipment
        ,FunctionType=p_FunctionType
        ,Status=p_Status
        ,ExtProperties=p_ExtProperties
        ,CreatedTime=p_CreatedTime
        ,CreatedUserID=p_CreatedUserID
        ,UpdatedTime=p_UpdatedTime
        ,UpdatedUserID=p_UpdatedUserID
        where GRID=p_GRID;
        commit;
        v_Result:=5; -- Success

        EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
        v_Result:=4; --Error
end;



create or replace procedure UP_GroundRes_Search
(
       p_Status varchar2,
       p_TimePoint date,
       o_Cursor out sys_refcursor
)
is
begin
       IF p_Status='' Or p_Status Is Null Then--全部
         open o_Cursor for
             Select A.*,B.* From TB_GroundResource A
             Left join TB_XYXSINFO B ON(A.RID=B.RID And B.Type=0)
             Order By A.CreatedTime Desc;
       Elsif p_Status='4' Then---删除
         open o_Cursor for
               Select A.*,B.* From TB_GroundResource  A
               Left join TB_XYXSINFO B ON(A.RID=B.RID And B.Type=0)
               Where A.Status=2
               Order By A.CreatedTime Desc;
       Elsif p_Status='1' Then --正常
         open o_Cursor for
             Select A.*,B.* From TB_GroundResource  A
               Left join TB_XYXSINFO B ON(A.RID=B.RID And B.Type=0)
               Where A.Status=1
               And A.GRID not in (Select ResourceID From TB_HEALTHSTATUS
                                 Where ResourceType=1
                                   And Status=2
                                   And BeginTime<=p_TimePoint
                                   And EndTime>=p_TimePoint)
             Order By A.CreatedTime Desc;
       Elsif p_Status='2' Then --异常
          open o_Cursor for
            Select A.*,B.* From TB_GroundResource  A
               Left join TB_XYXSINFO B ON(A.RID=B.RID And B.Type=0)
               Where A.Status=1
               And A.GRID in (Select ResourceID From TB_HEALTHSTATUS
                                 Where ResourceType=1
                                   And Status=2
                                   And BeginTime<=p_TimePoint
                                   And EndTime>=p_TimePoint)
             Order By A.CreatedTime Desc;
       Elsif p_Status='3' Then --占用中
          open o_Cursor for
             Select A.*,B.* From TB_GroundResource  A
               Left join TB_XYXSINFO B ON(A.RID=B.RID And B.Type=0)
               Where A.Status=1
               And A.GRID in (Select ResourceID From TB_USESTATUS
                                 Where ResourceType=1
                                   And BeginTime<=p_TimePoint
                                   And EndTime>=p_TimePoint)
             Order By A.CreatedTime Desc;

       End IF;
end;


create or replace procedure UP_GroundRes_SearchByPhase
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
             Select * From TB_GroundResource A
             Left join TB_XYXSINFO B ON(A.RID=B.RID And B.Type=0)
             Order By A.CreatedTime Desc;
       Elsif p_Status='4' Then---删除
         open o_Cursor for
               Select * From TB_GroundResource A
               Left join TB_XYXSINFO B ON(A.RID=B.RID And B.Type=0)
               Where A.Status=2
               Order By A.CreatedTime Desc;
       Elsif p_Status='1' Then --正常
         open o_Cursor for
             Select * From TB_GroundResource A
             Left join TB_XYXSINFO B ON(A.RID=B.RID And B.Type=0)
             Where A.Status=1
               And A.GRID not in (Select ResourceID From TB_HEALTHSTATUS
                                 Where ResourceType=1
                                   And Status=2
                                   And (    ( p_BeginTime>= Begintime And p_BeginTime<=EndTime)
                                         Or ( p_EndTime>= Begintime And p_EndTime<=EndTime)
                                         Or ( p_BeginTime<=Begintime And p_EndTime>=EndTime)
                                        )
                                )
             Order By A.CreatedTime Desc;
       Elsif p_Status='2' Then --异常
          open o_Cursor for
             Select * From TB_GroundResource A
             Left join TB_XYXSINFO B ON(A.RID=B.RID And B.Type=0)
             Where A.Status=1
               And A.GRID in (Select ResourceID From TB_HEALTHSTATUS
                                 Where ResourceType=1
                                   And Status=2
                                   And (    ( p_BeginTime>= Begintime And p_BeginTime<=EndTime)
                                         Or ( p_EndTime>= Begintime And p_EndTime<=EndTime)
                                         Or ( p_BeginTime<=Begintime And p_EndTime>=EndTime)
                                        )
                             )
             Order By A.CreatedTime Desc;
       Elsif p_Status='3' Then --占用中
          open o_Cursor for
             Select * From TB_GroundResource A
             Left join TB_XYXSINFO B ON(A.RID=B.RID And B.Type=0)
             Where A.Status=1
               And A.GRID in (Select ResourceID From TB_USESTATUS
                                 Where ResourceType=1
                                   And  (   ( p_BeginTime>= Begintime And p_BeginTime<=EndTime)
                                         Or ( p_EndTime>= Begintime And p_EndTime<=EndTime)
                                         Or ( p_BeginTime<=Begintime And p_EndTime>=EndTime)
                                        )
                            )
             Order By A.CreatedTime Desc;

       End IF;
end;



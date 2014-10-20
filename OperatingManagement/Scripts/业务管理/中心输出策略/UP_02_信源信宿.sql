
create or replace procedure UP_XYXSINFO_SelectByID
(
       p_RID TB_XYXSINFO.RID%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_XYXSINFO Where RID=p_RID;
end;


create or replace procedure UP_XYXSINFO_SelectAll
(
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_XYXSINFO Order By ADDRName;
end;

create or replace procedure UP_XYXSINFO_Insert
(
       p_AddrName  TB_XYXSInfo.Addrname%type,
       p_AddrMark  TB_XYXSInfo.Addrmark%type,
       p_InCode TB_XYXSInfo.Incode%type,
       p_ExCode TB_XYXSInfo.Excode%type,
       p_MainIP TB_XYXSInfo.Mainip%type,
       p_TCPPort TB_XYXSInfo.Tcpport%type,
       p_BakIP TB_XYXSInfo.Bakip%type,
       p_UDPPort TB_XYXSInfo.Udpport%type,
       p_FTPPath TB_XYXSInfo.Ftppath%type,
       p_Type TB_XYXSInfo.Type%type,
       p_Own TB_XYXSInfo.Own%type,
       p_Coordinate TB_XYXSInfo.Coordinate%type,
       p_Status TB_XYXSInfo.Status%type,
       p_CreatedTime TB_XYXSInfo.Createdtime%type,
       p_CreatedUserID TB_XYXSInfo.Createduserid%type,
       p_UpdatedTime TB_XYXSInfo.Updatedtime%type,
       p_UpdatedUserID TB_XYXSInfo.Updateduserid%type,
       v_RID out TB_XYXSInfo.Rid%type,
       v_Result out number
)
is
begin
       savepoint p1;
       Select SEQ_TB_XYXSInfo.NEXTVAL INTO v_RID FROM DUAL;
       Insert into TB_XYXSInfo(RID,AddrName,Addrmark,Incode,Excode,Mainip,Tcpport,Bakip,Udpport,Ftppath,Type,Own,Coordinate,Status,CreatedTime,CreatedUserID,UpdatedTime,UpdatedUserID) 
       Values(v_RID,p_AddrName,p_AddrMark,p_InCode,p_ExCode,p_MainIP,p_TCPPort,p_BakIP,p_UDPPort,p_FTPPath,p_Type,p_Own,p_Coordinate,p_Status,p_CreatedTime,p_CreatedUserID,p_UpdatedTime,p_UpdatedUserID);
       commit;
       v_Result:=5; -- Success

       EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
       v_Result:=4; --Error
end;



create or replace procedure UP_XYXSINFO_Update
(
       p_RID TB_XYXSInfo.Rid%type,
       p_AddrName  TB_XYXSInfo.Addrname%type,
       p_AddrMark  TB_XYXSInfo.Addrmark%type,
       p_InCode TB_XYXSInfo.Incode%type,
       p_ExCode TB_XYXSInfo.Excode%type,
       p_MainIP TB_XYXSInfo.Mainip%type,
       p_TCPPort TB_XYXSInfo.Tcpport%type,
       p_BakIP TB_XYXSInfo.Bakip%type,
       p_UDPPort TB_XYXSInfo.Udpport%type,
       p_FTPPath TB_XYXSInfo.Ftppath%type,
       p_Type TB_XYXSInfo.Type%type,
       p_Own TB_XYXSInfo.Own%type,
       p_Coordinate TB_XYXSInfo.Coordinate%type,
       p_Status TB_XYXSInfo.Status%type,
       p_CreatedTime TB_XYXSInfo.Createdtime%type,
       p_CreatedUserID TB_XYXSInfo.Createduserid%type,
       p_UpdatedTime TB_XYXSInfo.Updatedtime%type,
       p_UpdatedUserID TB_XYXSInfo.Updateduserid%type,
       v_Result out number
)
is
begin
     savepoint p1;

     update TB_XYXSInfo
        set AddrName=p_AddrName
           ,Addrmark=p_AddrMark
           ,Incode=p_InCode
           ,Excode=p_ExCode
           ,Mainip=p_MainIP
           ,Tcpport=p_TCPPort
           ,Bakip=p_BakIP
           ,Udpport=p_UDPPort
           ,Ftppath=p_FTPPath
           ,Type=p_Type
           ,Own=p_Own
           ,Coordinate=p_Coordinate
           ,Status=p_Status
           ,CreatedTime=p_CreatedTime
           ,CreatedUserID=p_CreatedUserID
           ,UpdatedTime=p_UpdatedTime
           ,UpdatedUserID=p_UpdatedUserID
        where RID=p_RID;
        commit;
        v_Result:=5; -- Success

        EXCEPTION
        WHEN OTHERS THEN
          ROLLBACK TO SAVEPOINT p1;
          COMMIT;
        v_Result:=4; --Error
end;




create or replace procedure UP_XYXSINFO_Search
(
       p_AddrName TB_XYXSINFO.Addrname%type,
       p_AddrMark TB_XYXSINFO.Addrmark%type,
       p_Own TB_XYXSINFO.Own%type,
       p_Type TB_XYXSINFO.Type%type,
       p_Status TB_XYXSINFO.Status%type,
       o_Cursor out sys_refcursor
)
is
begin
       open o_Cursor for
            Select * From TB_XYXSINFO
            Where (upper(AddrName) like ('%' || upper(p_AddrName) || '%') Or p_AddrName Is Null)
              And (upper(AddrMark) like ('%' || upper(p_AddrMark) || '%') Or p_AddrMark Is Null)
              And (Own=p_Own Or p_Own Is Null)
              And (Type=p_Type Or p_Type Is Null)
              And (Status=p_Status Or p_Status Is Null)
            Order by RID DESC;
end;

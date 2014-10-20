create or replace type type_split as table of varchar2(50);

create or replace function split
(
   p_list varchar2,
   p_sep varchar2 := ','
)  return type_split pipelined
is
   l_idx  pls_integer;
   v_list  varchar2(50) := p_list;
   v_temp varchar2(50) := '';
begin
   loop
      l_idx := instr(v_list,p_sep);
      if l_idx > 0 then
          v_temp := substr(v_list,1,l_idx-1);
          if v_temp<>' ' then
             pipe row(v_temp); 
          end if;   
          v_list := substr(v_list,l_idx+length(p_sep));
      else
          if v_list<>' ' then
             pipe row(v_list);
          end if;
          exit;
      end if;
   end loop;
   return;
end split;

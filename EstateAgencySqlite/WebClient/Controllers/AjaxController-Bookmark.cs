using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.Data.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities;
using static WebClient.Program;

namespace WebClient.Controllers
{
    public partial class AjaxController : ControllerBase
    {
        /// <summary>
        /// Get a list of Locations.
        /// </summary>
        [HttpGet("/ajax/bookmark/getall")]
        public Dictionary<string, object> BookmarkGetAll(int start_index, int count)
        {
            Console.WriteLine ($"start_index={start_index}, count={count}");
            if (count<1) count=10;
            if (start_index<0) start_index=0;
            SQLiteDataReader reader = client.Query(
                $"select PersonId, ObjectId from Bookmark limit {count} offset {start_index};");
            return CommonDataview.Build(reader);
        }

        

        [HttpDelete("/ajax/bookmark/delete")]
        public bool BookmarkDelete (int personid, int objectid)
        {
            Console.WriteLine($"Delete bookamrk id={personid}");
            try
            {
                client.Execute($"delete from Bookmark where PersonId={personid} and ObjectId={objectid};");
                return true;
            } 
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        [HttpGet("/ajax/bookmark/add")]
        public Dictionary<string, object> BookmarkAdd()
        {
            int id=0;
            var reader = client.Query("select max(Id) from Location;");
            reader.Read();
            id = reader.GetInt32(0) + 1;
            client.Query($"insert into Location(Id,Region,Town,District) values ({id},'Undefined','Undefined','Undefined');");
            string Html = 
$@"<div style='width:18em; margin: 3em auto auto;'>
<h2>New bookmark</h2>
<table style='table-layout:fixed;' class='dataview'>
<tr>
<td>PersonId</td>
<td><input id='PersonId' type='text'/></td>
</tr>
<tr>
<td>ObjectId</td>
<td><input id='ObjectId' type='text'/></td>
</tr>
</table>
<table style='table-layout:fixed;width:100%;'>
<tr>
<td><button class='secondary' onclick='back()'>Cancel</button></td>
<td><button class='primary' onclick='submit()'>Save</button></td>
</tr>
</table>"+
@"<script>
function submit(){
    $.ajax({
        method: 'POST', url: '/ajax/bookmark/set',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        contentType: 'application/json', dataType: 'json',
        data: JSON.stringify({
            'PersonId':$('#PersonId').val(),
            'ObjectId':$('#ObjectId').val()
        }),
        success: function (data){
            if (data){
                changed=true; 
                back();
            }
        }
    });
}
function back(){
    if (!changed) $('#main').html(main_backup);
    else bookmark_getall(level2_offset, row_load_count);
}
</script>";
            return new Dictionary<string, object>()
            {
                ["Html"]=Html,
                ["Good"]=1
            };
        }

        [HttpPost("/ajax/bookmark/set")]
        public bool BookmarkSet(Dictionary<string, string> Data)
        {
            if(Data.ContainsKey("PersonId") && Data.ContainsKey("ObjectId"))
            {
                Console.WriteLine("Good");
                try
                {
                    int personid, objectid;
                    if(int.TryParse(Data["PersonId"], out personid) && int.TryParse(Data["ObjectId"], out objectid))
                    { 
                        client.Execute($"insert into Bookmark (PersonId, ObjectId) values ({personid}, {objectid});");
                        return true;
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            return false;
        }
    }
}

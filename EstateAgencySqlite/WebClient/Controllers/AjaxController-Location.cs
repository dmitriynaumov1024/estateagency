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
        [HttpGet("/ajax/location/getall")]
        public Dictionary<string, object> LocationGetAll(int start_index, int count)
        {
            Console.WriteLine ($"start_index={start_index}, count={count}");
            if (count<1) count=10;
            if (start_index<0) start_index=0;
            SQLiteDataReader reader = client.Query(
                $"select Id, Region, Town, District from Location limit {count} offset {start_index};");
            return CommonDataview.Build(reader);
        }

        [HttpGet("/ajax/location/get")]
        public Dictionary<string, object> LocationGet(int id)
        {
            Console.WriteLine ($"location get, id={id}");
            SQLiteDataReader reader = client.Query(
                $"select Id, Region, Town, District from Location where Id={id};");
            if(!reader.HasRows) return new Dictionary<string, object>() { ["Found"]=false };
            reader.Read();
            string Html = 
$@"<h2>Location<br/></h2>
<table style='table-layout:fixed;' class='dataview'>
<tr>
    <th><button class='secondary' onclick='back()'>&larr;</button></th>
    <th>Current</th>
    <th>New</th>
</tr>
<tr>
    <td>Id</td>
    <td>{reader[0]}</td>
    <td><div id='Id'>{reader[0]}</div></td>
</tr>
<tr>
    <td>Region</td>
    <td>{reader[1]}</td>
    <td><input id='Region' type='text'/></td>
</tr>
<tr>
    <td>Town</td>
    <td>{reader[2]}</td>
    <td><input id='Town' type='text'/></td>
</tr>
<tr>
    <td>District</td>
    <td>{reader[3]}</td>
    <td><input id='District' type='text'/></td>
</tr>
</table>
<table style='table-layout:fixed;width:100%;'>
<tr>
    <td>&ensp;</td>
    <td><button class='secondary' onclick='delete_()'>Delete?</button></td>
    <td><button class='primary' onclick='submit()'>Save</button></td>
</tr>
</table>" + 
@"<script>
function submit(){
    $.ajax({
        method: 'POST', url: '/ajax/location/set',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        contentType: 'application/json', dataType: 'json',
        data: JSON.stringify({
            'Id':$('#Id').html(), 
            'Region':$('#Region').val(),
            'Town':$('#Town').val(),
            'District':$('#District').val()
        }),
        success: function (data){
            if (!data.Good){ 
                $(`#${data.Field}`).addClass('error'); 
                return; 
            }
            changed = true;
            $('#main').html(data.Html);
        }
    });
}
function back(){
    if (!changed) $('#main').html(main_backup);
    else location_getall(level2_offset, row_load_count);
}
function delete_(){
    $.ajax({
        method: 'DELETE', url: `/ajax/location/delete?id=${$('#Id').html()}`,
        success: function (data){
            if(data) location_getall(level2_offset, row_load_count);
        }
    });
}
</script>";
            return new Dictionary<string, object>()
            {
                ["Found"] = true,
                ["Good"] = 1,
                ["Html"] = Html
            };
        }

        [HttpPost("/ajax/location/set")]
        public Dictionary<string, object> LocationSet(Dictionary<string, object> Data)
        {
            foreach(var i in Data)
            {
                Console.WriteLine($"{i.Key}: {i.Value}");
            }
            if (Data.ContainsKey("Id") && 
                Data.ContainsKey("Region") && 
                Data.ContainsKey("Town") && 
                Data.ContainsKey("District"))
            { 
                Console.WriteLine("Good");
                try
                {
                    client.Execute($"update Location set Region='{Data["Region"]}', Town='{Data["Town"]}', District='{Data["District"]}' where Id={Data["Id"]};");
                    int id;
                    if (int.TryParse(Data["Id"].ToString(), out id))
                        return LocationGet(id);
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                    return new Dictionary<string, object>() { ["Good"] = 0 };
                }
            }
            Console.WriteLine("Bad");
            return new Dictionary<string, object>() { ["Good"] = 0 };
        }

        [HttpDelete("/ajax/location/delete")]
        public bool LocationDelete (int id)
        {
            Console.WriteLine($"Delete location id={id}");
            try
            {
                client.Execute($"delete from Location where Id={id};");
                return true;
            } 
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        [HttpGet("/ajax/location/add")]
        public Dictionary<string, object> LocationAdd()
        {
            int id=0;
            var reader = client.Query("select max(Id) from Location;");
            reader.Read();
            id = reader.GetInt32(0) + 1;
            client.Query($"insert into Location(Id,Region,Town,District) values ({id},'Undefined','Undefined','Undefined');");
            return LocationGet(id);
        }
    }
}

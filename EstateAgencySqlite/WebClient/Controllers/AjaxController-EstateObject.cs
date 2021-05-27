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
        [HttpGet("/ajax/estateobject/getall")]
        public Dictionary<string, object> EstateObjectGetAll(int start_index, int count)
        {
            Console.WriteLine ($"start_index={start_index}, count={count}");
            if (count<1) count=10;
            if (start_index<0) start_index=0;
            SQLiteDataReader reader = client.Query(
                $"select Id, LocationId, Price, Description from EstateObject limit {count} offset {start_index};");
            return CommonDataview.Build(reader);
        }

        [HttpGet("/ajax/estateobject/get")]
        public Dictionary<string, object> EstateObjectGet(int id)
        {
            Console.WriteLine ($"Estate object get, id={id}");
            SQLiteDataReader reader = client.Query(
                $"select Id, SellerId, isOpen, LocationId, Price, PostDate, Variant, Description from EstateObject where Id={id};");
            if(!reader.HasRows) return new Dictionary<string, object>() { ["Found"]=false };
            reader.Read();
            string Html = 
$@"<h2>Estate object<br/></h2>
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
    <td>SellerId</td>
    <td>{reader[1]}</td>
    <td><input id='SellerId' type='text'/></td>
</tr>
<tr>
    <td>isOpen</td>
    <td>{reader[2]}</td>
    <td><input id='isOpen' type='text'/></td>
</tr>
<tr>
    <td>LocationId</td>
    <td>{reader[3]}</td>
    <td><input id='LocationId' type='text'/></td>
</tr>
<tr>
    <td>Price</td>
    <td>{reader[4]}</td>
    <td><input id='Price' type='text'/></td>
</tr>
<tr>
    <td>PostDate</td>
    <td>{reader[5]}</td>
    <td><input id='PostDate' type='text'/></td>
</tr>
<tr>
    <td>Variant</td>
    <td>{reader[6]}</td>
    <td><input id='Variant' type='text'/></td>
</tr>
<tr>
    <td>Description</td>
    <td>{reader[7]}</td>
    <td><textarea id='Description' maxlength='5000' style='height:12em;'></textarea></td>
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
        method: 'POST', url: '/ajax/estateobject/set',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        contentType: 'application/json', dataType: 'json',
        data: JSON.stringify({
            'Id':$('#Id').html(), 
            'SellerId':$('#SellerId').val(),
            'isOpen':$('#isOpen').val(),
            'LocationId':$('#LocationId').val(),
            'Price':$('#Price').val(),
            'PostDate':$('#PostDate').val(),
            'Variant':$('#Variant').val(),
            'Description':$('#Description').val()
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
    else estateobject_getall(level2_offset, row_load_count);
}
function delete_(){
    $.ajax({
        method: 'DELETE', url: `/ajax/estateobject/delete?id=${$('#Id').html()}`,
        success: function (data){
            if(data) estateobject_getall(level2_offset, row_load_count);
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

        [HttpPost("/ajax/estateobject/set")]
        public Dictionary<string, object> EstateObjectSet(Dictionary<string, object> Data)
        {
            foreach(var i in Data)
            {
                Console.WriteLine($"{i.Key}: {i.Value}");
            }
            if (Data.ContainsKey("Id") && 
                Data.ContainsKey("SellerId") && 
                Data.ContainsKey("isOpen") && 
                Data.ContainsKey("LocationId") && 
                Data.ContainsKey("PostDate") && 
                Data.ContainsKey("Price") && 
                Data.ContainsKey("Variant") && 
                Data.ContainsKey("Description"))
            { 
                Console.WriteLine("Good");
                try
                {
                    client.Execute($"update EstateObject set SellerId='{Data["SellerId"]}', isOpen='{Data["isOpen"]}', LocationId={Data["LocationId"]}, PostDate='{Data["PostDate"]}', Price={Data["Price"]}, Variant='{Data["Variant"]}', Description='{Data["Description"]}' where Id={Data["Id"]};");
                    int id;
                    if (int.TryParse(Data["Id"].ToString(), out id))
                        return EstateObjectGet(id);
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

        [HttpDelete("/ajax/estateobject/delete")]
        public bool EstateObjectDelete (int id)
        {
            Console.WriteLine($"Delete estateobject id={id}");
            try
            {
                client.Execute($"delete from EstateObject where Id={id};");
                return true;
            } 
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        [HttpGet("/ajax/estateobject/add")]
        public Dictionary<string, object> EstateObjectAdd()
        {
            var reader = client.Query("select max(Id) from EstateObject;");
            reader.Read();
            string o = reader[0].ToString();
            int id = 0;
            if(int.TryParse(o, out id)) id++;
            reader = client.Query("select min(Id) from Person;");
            reader.Read();
            int sellerid = reader.GetInt32(0);
            string now = $"{DateTime.Now: yyyy-MM-dd}";
            client.Query($"insert into EstateObject (Id,SellerId,isOpen,LocationId,PostDate,Price,Variant,Description) values ({id}, {sellerid}, 0, 0,'{now}', 1000, 'house', 'Empty description');");
            return EstateObjectGet(id);
        }
    }
}

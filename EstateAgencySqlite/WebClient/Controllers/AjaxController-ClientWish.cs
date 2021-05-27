using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data.Common;
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
        [HttpGet("/ajax/clientwish/getall")]
        public Dictionary<string, object> ClientWishGetAll(int start_index, int count)
        {
            Console.WriteLine ($"start_index={start_index}, count={count}");
            if (count<1) count=10;
            if (start_index<0) start_index=0;
            SQLiteDataReader reader = client.Query(
                $"select Id, PersonId, PostDate, Variant, Price from ClientWish limit {count} offset {start_index};");
            return CommonDataview.Build(reader);
        }

        [HttpGet("/ajax/clientwish/get")]
        public Dictionary<string, object> ClientWishGet(int id)
        {
            Console.WriteLine ($"ClientWish get, id={id}");
            SQLiteDataReader reader = client.Query(
                $"select Id, PersonId, Variant, LocationId, Price, PostDate from ClientWish where Id={id};");
            if(!reader.HasRows) return new Dictionary<string, object>() { ["Found"]=0 };
            reader.Read();
            string Html = 
$@"<h2>Client's wish<br/></h2>
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
    <td>PersonId</td>
    <td>{reader[1]}</td>
    <td><input id='SellerId' type='text'/></td>
</tr>
<tr>
    <td>Variant</td>
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
        method: 'POST', url: '/ajax/clientwish/set',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        contentType: 'application/json', dataType: 'json',
        data: JSON.stringify({
            'Id':$('#Id').html(), 
            'PersonId':$('#PersonId').val(),
            'Variant':$('#Variant').val(),
            'LocationId':$('#LocationId').val(),
            'Price':$('#Price').val(),
            'PostDate':$('#PostDate').val()
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
    else clientwish_getall(level2_offset, row_load_count);
}
function delete_(){
    $.ajax({
        method: 'DELETE', url: `/ajax/clientwish/delete?id=${$('#Id').html()}`,
        success: function (data){
            if(data) clientwish_getall(level2_offset, row_load_count);
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

        [HttpPost("/ajax/clientwish/set")]
        public Dictionary<string, object> ClientWishSet(Dictionary<string, object> Data)
        {
            foreach(var i in Data)
            {
                Console.WriteLine($"{i.Key}: {i.Value}");
            }
            if (Data.ContainsKey("Id") && 
                Data.ContainsKey("PersonId") && 
                Data.ContainsKey("Variant") && 
                Data.ContainsKey("LocationId") && 
                Data.ContainsKey("PostDate") && 
                Data.ContainsKey("Price"))
            { 
                Console.WriteLine("Good");
                try
                {
                    int id;
                    if (int.TryParse(Data["Id"].ToString(), out id))
                    { 
                        client.Execute($"update ClientWish set PersonId={Data["PersonId"]}, Variant='{Data["Variant"]}', LocationId={Data["LocationId"]}, PostDate='{Data["PostDate"]}', Price={Data["Price"]} where Id={id};");
                        return ClientWishGet(id);
                    }
                    return new Dictionary<string, object>() { ["Good"] = 0 };
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

        [HttpDelete("/ajax/clientwish/delete")]
        public bool ClientWishDelete (int id)
        {
            Console.WriteLine($"Delete clientwish id={id}");
            try
            {
                client.Execute($"delete from ClientWish where Id={id};");
                return true;
            } 
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        [HttpGet("/ajax/clientwish/add")]
        public Dictionary<string, object> ClientWishAdd()
        {
            var reader = client.Query("select max(Id) from ClientWish;");
            reader.Read();
            string o = reader[0].ToString();
            int id = 0;
            if(int.TryParse(o, out id)) id++;
            reader = client.Query("select min(Id) from Person;");
            reader.Read();
            int sellerid = reader.GetInt32(0);
            string now = $"{DateTime.Now: yyyy-MM-dd}";
            client.Query($"insert into ClientWish (Id,PersonId,Variant,LocationId,PostDate,Price) values ({id}, {sellerid}, 'house', 0, '{now}', 1000);");
            return ClientWishGet(id);
        }
    }
}

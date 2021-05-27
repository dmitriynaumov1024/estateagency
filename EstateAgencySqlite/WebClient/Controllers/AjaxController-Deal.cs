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
        /// Get a list of Persons.
        /// </summary>
        [HttpGet("/ajax/deal/getall")]
        public Dictionary<string, object> DealGetAll(int start_index, int count)
        {
            Console.WriteLine ($"start_index={start_index}, count={count}");
            if (count<1) count=10;
            if (start_index<0) start_index=0;
            SQLiteDataReader reader = client.Query(
                $"select Id, SellerId, BuyerId, AgentId, Price, DealDate from Deal limit {count} offset {start_index};");
            return CommonDataview.Build(reader);
        }

        [HttpGet("/ajax/deal/get")]
        public Dictionary<string, object> DealGet(int id)
        {
            Console.WriteLine ($"Deal get, id={id}");
            SQLiteDataReader reader = client.Query(
                $"select Id, SellerId, BuyerId, AgentId, Price, DealDate from Deal where Id={id};");
            if(!reader.HasRows) return new Dictionary<string, object>() { ["Found"]=false };
            reader.Read();
            string Html = 
$@"<h2>Deal<br/></h2>
<table style='table-layout:fixed;' class='dataview'>
<tr>
    <th><button class='secondary' onclick='back()'>&larr;</button></th>
    <th>Current</th>
    <th>New</th>
</tr>
<tr>
    <td>Id of object</td>
    <td id='Id'>{reader[0]}</td>
    <td><input id='IdNew' type='text' value='{reader[0]}'/></td>
</tr>
<tr>
    <td>Id of seller</td>
    <td>{reader[1]}</td>
    <td><input id='SellerId' type='text'/></td>
</tr>
<tr>
    <td>ID of buyer</td>
    <td>{reader[2]}</td>
    <td><input id='BuyerId' type='text'/></td>
</tr>
<tr>
    <td>Id of agent</td>
    <td>{reader[3]}</td>
    <td><input id='AgentId' type='text'/></td>
</tr>
<tr>
    <td>Price</td>
    <td>{reader[4]}</td>
    <td><input id='Price' type='text'/></td>
</tr>
<tr>
    <td>Date</td>
    <td>{reader[5]}</td>
    <td><input id='DealDate' type='text'/></td>
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
        method: 'POST', url: '/ajax/deal/set',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        contentType: 'application/json', dataType: 'json',
        data: JSON.stringify({
            'Id':$('#Id').html(), 
            'IdNew':$('#IdNew').val(),
            'SellerId':$('#SellerId').val(),
            'BuyerId':$('#BuyerId').val(),
            'AgentId':$('#AgentId').val(),
            'Price':$('#Price').val(),
            'DealDate':$('#DealDate').val()
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
    else deal_getall(level2_offset, row_load_count);
}
function delete_(){
    $.ajax({
        method: 'DELETE', url: `/ajax/deal/delete?id=${$('#Id').html()}`,
        success: function (data){
            if(data) deal_getall(level2_offset, row_load_count);
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

        [HttpPost("/ajax/deal/set")]
        public Dictionary<string, object> DealSet(Dictionary<string, object> Data)
        {
            foreach(var i in Data)
            {
                Console.WriteLine($"{i.Key}: {i.Value}");
            }
            if (Data.ContainsKey("Id") && 
                Data.ContainsKey("IdNew") && 
                Data.ContainsKey("SellerId") && 
                Data.ContainsKey("BuyerId") && 
                Data.ContainsKey("AgentId") &&
                Data.ContainsKey("Price") && 
                Data.ContainsKey("DealDate"))
            { 
                Console.WriteLine("Good");
                try
                {
                    int id;
                    if (int.TryParse(Data["IdNew"].ToString(), out id) && int.TryParse(Data["Id"].ToString(), out id))
                    { 
                        string query = $"update Deal set Id={Data["IdNew"]}, SellerId={Data["SellerId"]}, BuyerId={Data["BuyerId"]}, AgentId={Data["AgentId"]}, Price={Data["Price"]}, DealDate='{Data["DealDate"]}' where Id={Data["Id"]};";
                        Console.WriteLine(query);
                        client.Execute(query);
                        return DealGet(id);
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

        [HttpDelete("/ajax/deal/delete")]
        public bool DealDelete (int id)
        {
            Console.WriteLine($"Delete deal id={id}");
            try
            {
                client.Execute($"delete from Deal where Id={id};");
                return true;
            } 
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        [HttpGet("/ajax/deal/add")]
        public Dictionary<string, object> DealAdd()
        {
            int id=0, personid=0;
            var reader = client.Query("select Id from EstateObject where not exists (select * from Deal where Deal.Id=Id) limit 1;");
            if (!reader.Read()) return new Dictionary<string, object>() { ["Good"]=0 };
            id = reader.GetInt32(0);
            reader = client.Query("select Id from Agent limit 1;");
            if (!reader.Read()) return new Dictionary<string, object>() { ["Good"]=0 };
            personid = reader.GetInt32(0);
            string now = $"{DateTime.Now: yyyy-MM-dd}";
            client.Query($"insert into Deal (Id,SellerId,BuyerId,AgentId,Price,DealDate) values ({id}, {personid}, {personid}, {personid}, 0, '{now}');");
            return DealGet(id);
        }
    }
}

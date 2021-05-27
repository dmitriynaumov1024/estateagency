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
        [HttpGet("/ajax/agent/getall")]
        public Dictionary<string, object> AgentGetAll(int start_index, int count)
        {
            Console.WriteLine ($"start_index={start_index}, count={count}");
            if (count<1) count=10;
            if (start_index<0) start_index=0;
            SQLiteDataReader reader = client.Query(
                $"select Id, TotalDeals, MonthDeals, MonthPayment from Agent limit {count} offset {start_index};");
            return CommonDataview.Build(reader);
        }

        [HttpGet("/ajax/agent/get")]
        public Dictionary<string, object> AgentGet(int id)
        {
            Console.WriteLine ($"person get, id={id}");
            SQLiteDataReader reader = client.Query(
                $"select Id, TotalDeals, MonthDeals, MonthPayment from Agent where Id={id};");
            if(!reader.HasRows) return new Dictionary<string, object>() { ["Found"]=false };
            reader.Read();
            string Html = 
$@"<h2>Agent<br/></h2>
<table style='table-layout:fixed;' class='dataview'>
<tr>
    <th><button class='secondary' onclick='back()'>&larr;</button></th>
    <th>Current</th>
    <th>New</th>
</tr>
<tr>
    <td>Id of Person</td>
    <td id='Id'>{reader[0]}</td>
    <td><input id='IdNew' type='text' value='{reader[0]}'/></td>
</tr>
<tr>
    <td>Total deals</td>
    <td>{reader[1]}</td>
    <td><input id='TotalDeals' type='text'/></td>
</tr>
<tr>
    <td>Month deals</td>
    <td>{reader[2]}</td>
    <td><input id='MonthDeals' type='text'/></td>
</tr>
<tr>
    <td>Month payment</td>
    <td>{reader[3]}</td>
    <td><input id='MonthPayment' type='text'/></td>
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
        method: 'POST', url: '/ajax/agent/set',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        contentType: 'application/json', dataType: 'json',
        data: JSON.stringify({
            'Id':$('#Id').html(), 
            'IdNew':$('#IdNew').val(),
            'TotalDeals':$('#TotalDeals').val(),
            'MonthDeals':$('#MonthDeals').val(),
            'MonthPayment':$('#MonthPayment').val()
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
    else agent_getall(level2_offset, row_load_count);
}
function delete_(){
    $.ajax({
        method: 'DELETE', url: `/ajax/agent/delete?id=${$('#Id').html()}`,
        success: function (data){
            if(data) agent_getall(level2_offset, row_load_count);
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

        [HttpPost("/ajax/agent/set")]
        public Dictionary<string, object> AgentSet(Dictionary<string, object> Data)
        {
            foreach(var i in Data)
            {
                Console.WriteLine($"{i.Key}: {i.Value}");
            }
            if (Data.ContainsKey("Id") && 
                Data.ContainsKey("IdNew") && 
                Data.ContainsKey("TotalDeals") && 
                Data.ContainsKey("MonthDeals") && 
                Data.ContainsKey("MonthPayment"))
            { 
                Console.WriteLine("Good");
                try
                {
                    int id;
                    if (int.TryParse(Data["Id"].ToString(), out id))
                    { 
                        string query = $"update Agent set Id={Data["IdNew"]}, TotalDeals={Data["TotalDeals"]}, MonthDeals={Data["MonthDeals"]}, MonthPayment={Data["MonthPayment"]} where Id={Data["Id"]};";
                        Console.WriteLine(query);
                        client.Execute(query);
                        if(int.TryParse(Data["IdNew"].ToString(), out id)) return AgentGet(id);
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

        [HttpDelete("/ajax/agent/delete")]
        public bool AgentDelete (int id)
        {
            Console.WriteLine($"Delete agent id={id}");
            try
            {
                client.Execute($"delete from Agent where Id={id};");
                return true;
            } 
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        [HttpGet("/ajax/agent/add")]
        public Dictionary<string, object> AgentAdd()
        {
            int id=0;
            var reader = client.Query("select Id from Person where not exists (select * from Agent where Agent.Id=Id) limit 1;");
            if (!reader.HasRows) return new Dictionary<string, object>() { ["Good"]=0 };
            reader.Read();
            id = reader.GetInt32(0);
            client.Query($"insert into Agent(Id,TotalDeals,MonthDeals,MonthPayment) values ({id}, 0, 0, 0);");
            return AgentGet(id);
        }
    }
}

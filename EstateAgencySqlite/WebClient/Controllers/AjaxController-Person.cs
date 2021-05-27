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
    [ApiController]
    public partial class AjaxController : ControllerBase
    {
        /// <summary>
        /// Get a list of Persons.
        /// </summary>
        [HttpGet("/ajax/person/getall")]
        public Dictionary<string, object> PersonGetAll(int start_index, int count)
        {
            Console.WriteLine ($"start_index={start_index}, count={count}");
            if (count<1) count=10;
            if (start_index<0) start_index=0;
            SQLiteDataReader reader = client.Query(
                $"select Id, Surname, Name, Phone from Person limit {count} offset {start_index};");
            return CommonDataview.Build(reader);
        }

        [HttpGet("/ajax/person/get")]
        public Dictionary<string, object> PersonGet(int id)
        {
            Console.WriteLine ($"person get, id={id}");
            SQLiteDataReader reader = client.Query(
                $"select Id, Surname, Name, Phone, Email, LocationId, RegDate from Person where Id={id};");
            if(!reader.HasRows) return new Dictionary<string, object>() { ["Found"]=false };
            reader.Read();
            string Html = 
$@"<h2>Person<br/></h2>
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
    <td>Surname</td>
    <td>{reader[1]}</td>
    <td><input id='Surname' type='text'/></td>
</tr>
<tr>
    <td>Name</td>
    <td>{reader[2]}</td>
    <td><input id='Name' type='text'/></td>
</tr>
<tr>
    <td>Phone</td>
    <td>{reader[3]}</td>
    <td><input id='Phone' type='text'/></td>
</tr>
<tr>
    <td>Email</td>
    <td>{reader[4]}</td>
    <td><input id='Email' type='text'/></td>
</tr>
<tr>
    <td>LocationId</td>
    <td>{reader[5]}</td>
    <td><input id='LocationId' type='text'/></td>
</tr>
<tr>
    <td>Reg. date</td>
    <td>{reader[6]}</td>
    <td><input id='RegDate' type='text'/></td>
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
        method: 'POST', url: '/ajax/person/set',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        contentType: 'application/json', dataType: 'json',
        data: JSON.stringify({
            'Id':$('#Id').html(), 
            'Surname':$('#Surname').val(),
            'Name':$('#Name').val(),
            'Phone':$('#Phone').val(),
            'Email':$('#Email').val(),
            'LocationId':$('#LocationId').val(),
            'RegDate':$('#RegDate').val()
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
    else person_getall(level2_offset, row_load_count);
}
function delete_(){
    $.ajax({
        method: 'DELETE', url: `/ajax/person/delete?id=${$('#Id').html()}`,
        success: function (data){
            if(data) person_getall(level2_offset, row_load_count);
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

        [HttpPost("/ajax/person/set")]
        public Dictionary<string, object> PersonSet(Dictionary<string, object> Data)
        {
            foreach(var i in Data)
            {
                Console.WriteLine($"{i.Key}: {i.Value}");
            }
            if (Data.ContainsKey("Id") && 
                Data.ContainsKey("Surname") && 
                Data.ContainsKey("Name") && 
                Data.ContainsKey("Phone") &&
                Data.ContainsKey("Email") &&
                Data.ContainsKey("LocationId") &&
                Data.ContainsKey("RegDate"))
            { 
                Console.WriteLine("Good");
                try
                {
                    client.Execute($"update Person set Surname='{Data["Surname"]}', Name='{Data["Name"]}', Phone='{Data["Phone"]}', Email='{Data["Email"]}', LocationId='{Data["LocationId"]}', RegDate='{Data["RegDate"]}' where Id={Data["Id"]};");
                    int id;
                    if (int.TryParse(Data["Id"].ToString(), out id))
                        return PersonGet(id);
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

        [HttpDelete("/ajax/person/delete")]
        public bool PersonDelete (int id)
        {
            Console.WriteLine($"Delete person id={id}");
            try
            {
                client.Execute($"delete from Person where Id={id};");
                return true;
            } 
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        [HttpGet("/ajax/person/add")]
        public Dictionary<string, object> PersonAdd()
        {
            int id=0;
            var reader = client.Query("select max(Id) from Person;");
            reader.Read();
            id = reader.GetInt32(0) + 1;
            string now = $"{DateTime.Now: yyyy-MM-dd}";
            string tmpphone = $"{DateTime.Now.ToBinary():X}";
            client.Query($"insert into Person(Id,Surname,Name,Phone,Email,LocationId,RegDate) values ({id},'Undefined','Undefined','{tmpphone}', 'example@email.com', 0, '{now}');");
            return PersonGet(id);
        }
    }
}

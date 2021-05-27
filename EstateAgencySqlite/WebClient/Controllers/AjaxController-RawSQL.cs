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
        [HttpPost("/ajax/rawsql")]
        public Dictionary<string, object> RawSql (Dictionary<string, string> Data)
        {
            if (Data.ContainsKey("Query"))
            {
                string op = Data["Query"].Split(' ')[0].ToLower();
                if(op!="select") return new Dictionary<string, object>() 
                { 
                    ["Good"]=0, 
                    ["Message"]="Only select operation is supported, for your own safety!" 
                };
                try
                {
                    var reader = client.Query(Data["Query"]);
                    var result = CommonDataview.Build(reader);
                    result["Good"] = 1;
                    return result;
                }
                catch (Exception ee)
                {
                    return new Dictionary<string, object>() { ["Good"]=0, ["Message"]=ee.Message };
                }
            }
            return new Dictionary<string, object>() { ["Good"]=0 };
        }
    }
}

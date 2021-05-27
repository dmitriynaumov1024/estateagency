using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data.Common;

namespace WebClient
{
    public static class CommonDataview
    {
        public static Dictionary<string, object> Build(SQLiteDataReader reader)
        {
            var dataview = new Dictionary<string, object>();
            if(!reader.HasRows) 
                return new Dictionary<string, object>();
            int fieldcount = reader.FieldCount;
            var Header = new string[fieldcount];
            for(int i=0; i<fieldcount; i++)
                Header[i] = reader.GetName(i);
            dataview["Header"] = Header;
            var Data = new List<object[]>();
            foreach (DbDataRecord row in reader)
            {
                var Datarow = new object[fieldcount];
                for(int j=0; j<fieldcount; j++) 
                    Datarow[j] = row[j];
                Data.Add(Datarow);
            }
            dataview["Data"] = Data;
            return dataview;
        }
    }
}

using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdgregateData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            int currentHour=DateTime.Now.Hour;
            DataTable logs = DAL.SelectCurrentHourRecords();


            foreach (DataRow item in logs.Rows)
            {
                DataAdgregated data = new DataAdgregated()
                {
                    Country = item["Country"].ToString(),
                    GameID = new Guid(item["GameId"].ToString()),
                    Hour = currentHour,
                    View = Convert.ToInt32(item["nbCount"])
                };
                data.InsertAgregatedToSQL();
            }
            Console.WriteLine("End");
            Console.Read();
        }
    }
}

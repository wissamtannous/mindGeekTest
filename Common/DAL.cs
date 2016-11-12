using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Common
{
    public static class DAL
    {
        private static UserLog GetUserLog(DataRow r)
        {
            DateTime time;
            bool retval = DateTime.TryParse(r["Time"].ToString(), out time);
            UserLog log = new UserLog()
            {
                Country = r["Country"].ToString(),
                GameID = new Guid(r["GameID"].ToString()),
                HeaderData = r["HeaderData"].ToString(),
                Time = time,
                UserAgent = r["UserAgent"].ToString(),
                UserIP = r["UserIP"].ToString()
            };

            return log;
        }
        private static List<UserLog> GetUserLog(DataTable dt)
        {
            List<UserLog> retVal = new List<UserLog>();
            foreach (DataRow dr in dt.Rows)
            {
                UserLog coupon = GetUserLog(dr);
                retVal.Add(coupon);
            }

            return retVal;
        }

        public static void SelectSelectionByPage(UserLog log)
        {
            using (MySqlConnection connection = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Mysql"].ToString()))
            {
                connection.Open();
                string sql = "INSERT INTO `db_9af510_mgeek`.`userlog` ( `GameID`, `UserIP`, `Time`, `Country`, `UserAgent`, `HeaderData`) VALUES (@GameID, @UserIP, @Time, @Country, @UserAgent, @HeaderData);";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@GameID", log.GameID.ToString());
                    command.Parameters.AddWithValue("@UserIP", log.UserIP);
                    command.Parameters.AddWithValue("@Time", log.Time);
                    command.Parameters.AddWithValue("@Country", log.Country);
                    command.Parameters.AddWithValue("@UserAgent", log.UserAgent);
                    command.Parameters.AddWithValue("@HeaderData", log.HeaderData);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static void InsertAgregatedToSQL(this DataAdgregated data)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MSSql"].ToString()))
            {
                connection.Open();
                string sql = "INSERT INTO [DataAggregated] ( [GameID], [Hour], [Country], [View]) VALUES ( @GameID, @Hour, @Country, @View)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@GameID", data.GameID);
                    command.Parameters.AddWithValue("@Hour", data.Hour);
                    command.Parameters.AddWithValue("@Country", data.Country);
                    command.Parameters.AddWithValue("@View", data.View);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }


        public static DataTable SelectCurrentHourRecords()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Mysql"].ToString()))
            {
                connection.Open();
                string sql = "SELECT `GameID`, `UserIP`, `Time`, `Country`, count(Country) as nbCount FROM `userlog` where hour(Time)=@currentHour group by UserIP";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@currentHour", DateTime.Now.Hour);
                    dt = command.CustomExecuteReader();
                }
                connection.Close();
                return dt;
            }
        }

        public static List<UserLog> SelectAllRecords()
        {
            List<UserLog> userLogs = new List<UserLog>();
            using (MySqlConnection connection = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Mysql"].ToString()))
            {
                connection.Open();
                string sql = "select * from userlog";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (DataTable dt = command.CustomExecuteReader())
                    {
                        userLogs.AddRange(GetUserLog(dt));
                    }
                }
                connection.Close();
            }
            return userLogs;
        }

        private static DataTable CustomExecuteReader(this SqlCommand sqlCommand)
        {
            using (SqlDataAdapter dataReader = new SqlDataAdapter(sqlCommand))
            {
                DataTable retVal = new DataTable("RetrunTable");
                dataReader.Fill(retVal);
                return retVal;
            }
        }

        private static DataTable CustomExecuteReader(this MySqlCommand sqlCommand)
        {
            using (MySqlDataAdapter dataReader = new MySqlDataAdapter(sqlCommand))
            {
                DataTable retVal = new DataTable("RetrunTable");
                dataReader.Fill(retVal);
                return retVal;
            }
        }
    }
}
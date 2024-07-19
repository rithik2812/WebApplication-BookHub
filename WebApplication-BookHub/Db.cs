using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApplication_BookHub
{
    public class Db
    {

        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["BookHubDB"].ConnectionString;
        public static string GetUserProfilePicture(string username)
        {
            string profilePicturePath = "";

            using (MySqlConnection connection = new MySqlConnection(CONNECTIONSTRING))
            {
                string query = "SELECT profile_picture FROM users WHERE username = @username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        profilePicturePath = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving profile picture from database: " + ex.Message);
                }
            }

            return profilePicturePath;
        }

    }
}
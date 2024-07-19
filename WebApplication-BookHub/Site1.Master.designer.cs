using System;
using System.Configuration;
using System.Web.UI;
using MySql.Data.MySqlClient;

namespace WebApplication_BookHub
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["BookHubDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    string username = Session["Username"].ToString();
                    string profilePicturePath = GetUserProfilePicture(username);
                    if (!string.IsNullOrEmpty(profilePicturePath))
                    {
                        imgUserProfile.ImageUrl = profilePicturePath;
                    }
                    else
                    {
                        imgUserProfile.ImageUrl = "https://i.sstatic.net/l60Hf.png"; // Default profile picture URL
                    }
                }
            }
        }

        protected void imgUserProfile_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Profile.aspx");
        }

        private string GetUserProfilePicture(string username)
        {
            string profilePicturePath = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
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

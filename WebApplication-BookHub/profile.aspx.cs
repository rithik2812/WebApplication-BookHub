using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI;
using System.IO;

namespace WebApplication_BookHub
{
    public partial class Profile : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["BookHubDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    string username = Session["Username"].ToString();
                    string profilePicturePath = Db.GetUserProfilePicture(username);
                    if (!string.IsNullOrEmpty(profilePicturePath))
                    {
                        imgProfilePic.ImageUrl = profilePicturePath;
                    }
                    else
                    {
                        imgProfilePic.ImageUrl = "https://static.vecteezy.com/system/resources/thumbnails/009/292/244/small/default-avatar-icon-of-social-media-user-vector.jpg";
                    }

                    string userEmail = GetUserEmail(username);
                    txtEmail.Text = userEmail;
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void btnUploadProfilePic_Click(object sender, EventArgs e)
        {
            if (fileUploadProfilePic.HasFile)
            {
                string filename = Path.GetFileName(fileUploadProfilePic.FileName);
                string extension = Path.GetExtension(filename);
                string saveLocation = Server.MapPath("~/ProfilePictures/") + filename;

                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png")
                {
                    fileUploadProfilePic.SaveAs(saveLocation);

                    string username = Session["Username"].ToString();
                    bool isUpdated = UpdateUserProfilePicture(username, "~/ProfilePictures/" + filename);

                    if (isUpdated)
                    {
                        imgProfilePic.ImageUrl = "~/ProfilePictures/" + filename;
                    }
                    else
                    {
                        lblChangePasswordStatus.Text = "Failed to update profile picture.";
                    }
                }
                else
                {
                    lblChangePasswordStatus.Text = "Only .jpg and .png files are allowed.";
                }
            }
        }

        protected void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            string currentPassword = txtCurrentPassword.Text.Trim();
            string newPassword = txtConfirmNewPassword.Text.Trim();
            string confirmPassword = txtConfirmNewPassword.Text.Trim();

            if (currentPassword != "" && newPassword != "" && confirmPassword != "")
            {
                string username = Session["Username"].ToString();
                string storedPassword = GetPasswordFromDatabase(username);

                if (storedPassword == "")
                {
                    lblChangePasswordStatus.Text = "Error retrieving password from database.";
                    return;
                }

                if (currentPassword == storedPassword)
                {
                    if (newPassword == confirmPassword)
                    {
                        bool isPasswordUpdated = UpdateUserPasswordInDatabase(username, newPassword);

                        if (isPasswordUpdated)
                        {
                            lblChangePasswordStatus.Text = "Password changed successfully!";
                        }
                        else
                        {
                            lblChangePasswordStatus.Text = "Failed to update password.";
                        }
                    }
                    else
                    {
                        lblChangePasswordStatus.Text = "New password and confirm password do not match.";
                    }
                }
                else
                {
                    lblChangePasswordStatus.Text = "Current password is incorrect.";
                }
            }
            else
            {
                lblChangePasswordStatus.Text = "Please fill in all fields.";
            }
        }

        protected void btnUpdateEmail_Click(object sender, EventArgs e)
        {
            string newEmail = txtEmail.Text.Trim();
            string username = Session["Username"].ToString();

            bool isUpdated = UpdateUserEmail(username, newEmail);

            if (isUpdated)
            {
                lblChangePasswordStatus.Text = "Email updated successfully!";
            }
            else
            {
                lblChangePasswordStatus.Text = "Failed to update email.";
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["Username"] = null;
            Response.Redirect("Login.aspx");
        }

        protected void btnDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx");
        }

        private bool UpdateUserProfilePicture(string username, string profilePicturePath)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE users SET profile_picture = @profilePicturePath WHERE username = @username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@profilePicturePath", profilePicturePath);
                command.Parameters.AddWithValue("@username", username);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating profile picture in database: " + ex.Message);
                    return false;
                }
            }
        }

        private string GetPasswordFromDatabase(string username)
        {
            string password = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT password FROM users WHERE username = @username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        password = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving password from database: " + ex.Message);
                }
            }

            return password;
        }

        private bool UpdateUserPasswordInDatabase(string username, string newPassword)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE users SET password = @password WHERE username = @username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@password", newPassword);
                command.Parameters.AddWithValue("@username", username);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating password in database: " + ex.Message);
                    return false;
                }
            }
        }

        private string GetUserEmail(string username)
        {
            string email = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT email FROM users WHERE username = @username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        email = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving email from database: " + ex.Message);
                }
            }

            return email;
        }

        private bool UpdateUserEmail(string username, string newEmail)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE users SET email = @newEmail WHERE username = @username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@newEmail", newEmail);
                command.Parameters.AddWithValue("@username", username);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating email in database: " + ex.Message);
                    return false;
                }
            }
        }
    }
}

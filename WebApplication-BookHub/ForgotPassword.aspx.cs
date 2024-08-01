using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web;
using System.Web.UI;

namespace WebApplication_BookHub
{
    public partial class ForgotPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                Response.Write("<script>alert('Please fill in all fields.');</script>");
                return;
            }

            if (newPassword != confirmPassword)
            {
                Response.Write("<script>alert('Passwords do not match.');</script>");
                return;
            }

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BookHubDB"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE users SET password = @NewPassword WHERE username = @Username";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Response.Write("<script>alert('Password reset successfully.');</script>");
                            Response.Redirect("Login.aspx");
                        }
                        else
                        {
                            Response.Write("<script>alert('Invalid username.');</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}

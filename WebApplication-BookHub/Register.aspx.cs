using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.UI;

namespace WebApplication_BookHub
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                Response.Write("<script>alert('Registration incomplete. Please fill in all fields.');</script>");
                return;
            }

            if (password == confirmPassword)
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["BookHubDB"].ConnectionString;
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @Username";
                        using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@Username", username);
                            int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                            if (userCount > 0)
                            {
                                Response.Write("<script>alert('Username already created. Please choose a different username.');</script>");
                                return;
                            }
                        }

                        // Insert new user
                        string query = "INSERT INTO users (username, password) VALUES (@Username, @Password)";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@Password", password);

                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                Response.Write("<script>alert('Registered successfully!');</script>");
                                Response.Redirect("Login.aspx");
                            }
                            else
                            {
                                Response.Write("<script>alert('Error: Registration failed.');</script>");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Show error message
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
            else
            {
                // Show error message if passwords do not match
                Response.Write("<script>alert('Passwords do not match. Please try again.');</script>");
            }
        }
    }
}

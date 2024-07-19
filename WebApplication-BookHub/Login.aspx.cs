using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web;
using System.Web.UI;

namespace WebApplication_BookHub
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                
                Response.Write("<script>alert('Login incomplete. Please fill in all fields.');</script>");
                return;
            }

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BookHubDB"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM users WHERE username = @Username AND password = @Password";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                        if (userCount > 0)
                        {
                            Session["Username"] = username;
                            Response.Redirect("Dashboard.aspx");
                        }
                        else
                        {
                            
                            Response.Write("<script>alert('Invalid username or password. Please try again.');</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
}
